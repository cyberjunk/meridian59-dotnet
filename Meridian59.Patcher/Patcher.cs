using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Meridian59.Patcher
{
    /// <summary>
    /// Main class/controller of the Meridian59 Patcher.
    /// Contains the Main() method.
    /// </summary>
    public static class Patcher
    {
        private enum UpdateFormat
        {
            None,
            DotNet,
            Classic6Arg,
            Classic7Arg,
            ClassicManual
        };

        private const int NUMWORKERS        = 8;
        private const int MAXRETRIES        = 3;

        private const string CLASSICURLDATAFILE = "dlinfo.txt";
        private const string CLASSICEXENAME     = "meridian.exe";
        private const string CLASSICPROCESS     = "meridian";
        private const string CLASSICPATCHEREXENAME = "club.exe";

        // Fallback default for old 6-arg classic client, expected to be very low prevalence.
        private const string CLASSICDEFAULTJSON = "clientpatch.txt";

        private const string DOTNETURLDATAFILE = "patchurl.txt";
        private const string CLIENTEXENAME  = "Meridian59.Ogre.Client.exe";
        private const string CLIENTPROCESS  = "Meridian59.Ogre.Client";
        private const string PATCHEREXENAME = "Meridian59.Patcher.exe";
        private const string FOLDERX64      = "x64";
        private const string FOLDERX86      = "x86";
        private const string NGENX64        = "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\ngen.exe";
        private const string NGENX86        = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\ngen.exe";

        private static readonly string[] EXCLUSIONS   = new string[] { CLASSICPATCHEREXENAME, PATCHEREXENAME };
        private static readonly double MSTICKDIVISOR  = (double)Stopwatch.Frequency / 1000.0;
        private static readonly string PATCHEREXE     = Assembly.GetEntryAssembly().Location;

        private static readonly string PATCHERPATH    = Path.GetDirectoryName(PATCHEREXE);
        private static readonly string CLIENTPATHX86  = Path.Combine(PATCHERPATH, FOLDERX86);
        private static readonly string CLIENTPATHX64  = Path.Combine(PATCHERPATH, FOLDERX64);
        private static readonly string CLIENTEXE86    = Path.Combine(CLIENTPATHX86, CLIENTEXENAME);
        private static readonly string CLIENTEXE64    = Path.Combine(CLIENTPATHX64, CLIENTEXENAME);
        private static readonly string CLIENTEXEAUTO  = Environment.Is64BitOperatingSystem ? CLIENTEXE64 : CLIENTEXE86;
        private static readonly string CLIENTPATHAUTO = Environment.Is64BitOperatingSystem ? CLIENTPATHX64 : CLIENTPATHX86;
        
        private static readonly WindowsIdentity identity   = WindowsIdentity.GetCurrent();
        private static readonly WindowsPrincipal principal = new WindowsPrincipal(identity);
        private static readonly LanguageHandler languageHandler = new LanguageHandler();
        private static readonly PatchFileQueue queue       = new PatchFileQueue();
        private static readonly PatchFileQueue queueDone   = new PatchFileQueue();
        private static readonly PatchFileQueue queueErrors = new PatchFileQueue();
        private static readonly List<PatchFile> files      = new List<PatchFile>();
        private static readonly Worker[] workers           = new Worker[NUMWORKERS];       
        private static readonly Stopwatch watch            = new Stopwatch();
        private static readonly WebClient webClient        = new WebClient();

        private static DownloadForm form = null;
        private static int filesDone     = 0;
        private static bool abort        = false;
        private static bool isRunning    = true;
        private static bool isHeadless   = false;
        private static string baseUrl    = "";
        private static string jsonUrl    = "";
        
        private static UpdateFormat updateFormat = UpdateFormat.None;
        private static string clientExecutable = "";
        private static string clientPath = "";

        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // start ticker
            watch.Start();

            ///////////////////////////////////////////////////////////////////////
            // parse commandline arguments and set UpdateFormat.

            // CLASSIC CLIENT USING CMD ARGS
            if ((args.Length == 6 || args.Length == 7)
                && args[1].Contains("UPDATE"))
            {
                updateFormat = (args.Length == 6) ? 
                    UpdateFormat.Classic6Arg : 
                    UpdateFormat.Classic7Arg;

                ReadCommandLineArgumentsClassic(args);
            }

            // OGRE CLIENT
            else if (args.Length > 0 || File.Exists(DOTNETURLDATAFILE))
            {
                updateFormat = UpdateFormat.DotNet;
                ReadCommandLineArguments(args);
            }

            // CLASSIC CLIENT USING FILE
            else if (File.Exists(CLASSICURLDATAFILE))
            {
                updateFormat = UpdateFormat.ClassicManual;
            }

            ///////////////////////////////////////////////////////////////////////
            // create UI if not-headless
            if (!isHeadless)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // create ui
                form = new DownloadForm(files, languageHandler);
                form.FormClosed += OnFormClosed;
                form.Show();
            }

            ///////////////////////////////////////////////////////////////////////
            // read url data file if necessary

            switch (updateFormat)
            {
                case UpdateFormat.DotNet:
                    if (!ReadUrlDataFile())
                        return;
                    break;
                case UpdateFormat.Classic6Arg:
                case UpdateFormat.Classic7Arg:
                    // No need to parse file.
                    break;
                case UpdateFormat.ClassicManual:
                    if (!ReadUrlDataFileClassic())
                        return;
                    break;
                case UpdateFormat.None:
                    if (!isHeadless)
                    {
                        MessageBox.Show(String.Format(languageHandler.UrlInfoMissing, DOTNETURLDATAFILE),
                            languageHandler.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
            }

            ///////////////////////////////////////////////////////////////////////
            // start download of patchinfo.txt

            if (!isHeadless)
                form.DisplayInfo(languageHandler.DownloadingPatch);

            webClient.DownloadDataCompleted += OnWebClientDownloadDataCompleted;
            webClient.DownloadDataAsync(new Uri(jsonUrl));
           
            ///////////////////////////////////////////////////////////////////////
            // mainthread loop

            while(isRunning)
            {
                long   tick   = watch.ElapsedTicks;
                double mstick = (double)tick / MSTICKDIVISOR;

                // handle PatchFile instances returned by workers
                ProcessQueues();

                // update UI
                if (!isHeadless)
                {
                    // tick ui
                    if (form != null)
                        form.Tick(mstick, false);

                    // process messages
                    Application.DoEvents();
                }

                // sleep a bit (-> ~60 FPS)
                Thread.Sleep(16);
            }

            ///////////////////////////////////////////////////////////////////////

            // Tick form once more to finalize download numbers.
            if (!isHeadless && !abort)
            {
                long tick = watch.ElapsedTicks;
                double mstick = (double)tick / MSTICKDIVISOR;

                if (form != null)
                    form.Tick(mstick, true);

                // Either client up to date (no files downloaded) or some files were downloaded.
                if (filesDone == 0)
                {
                    MessageBox.Show(languageHandler.ClientUpToDate, languageHandler.InfoText,
                        MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show(languageHandler.ClientWasUpdated, languageHandler.InfoText,
                        MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }

            ///////////////////////////////////////////////////////////////////////

            // also stop worker-instances
            foreach(Worker w in workers)
                if (w != null)
                    w.Stop();
     
            // in case patching went well
            if (!abort)
            {
                ProcessStartInfo pi;
                Process process;

                // if admin, try to 'ngen' the exe files
                if (updateFormat == UpdateFormat.DotNet
                    && principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    if (File.Exists(NGENX64))
                    {
                        // start ngen
                        pi                  = new ProcessStartInfo();
                        pi.FileName         = NGENX64;
                        pi.Arguments        = "install Meridian59.Ogre.Client.exe";
                        pi.UseShellExecute  = true;
                        pi.WorkingDirectory = CLIENTPATHX64;
                        pi.WindowStyle      = ProcessWindowStyle.Hidden;

                        process = new Process();
                        process.StartInfo = pi;
                        process.Start();
                        process.WaitForExit();
                    }                  
                    
                    if (File.Exists(NGENX86))
                    {
                        // start ngen
                        pi                  = new ProcessStartInfo();
                        pi.FileName         = NGENX86;
                        pi.Arguments        = "install Meridian59.Ogre.Client.exe";
                        pi.UseShellExecute  = true;
                        pi.WorkingDirectory = CLIENTPATHX86;
                        pi.WindowStyle      = ProcessWindowStyle.Hidden;

                        process = new Process();
                        process.StartInfo = pi;
                        process.Start();
                        process.WaitForExit();
                    }
                }

                // start client
                pi                  = new ProcessStartInfo();
                pi.FileName         = clientExecutable;
                pi.Arguments        = "";
                pi.UseShellExecute  = true;
                pi.WorkingDirectory = clientPath;

                process = new Process();
                process.StartInfo = pi;
                process.Start();
            }
        }

        /// <summary>
        /// Handles all returned PatchFile instances in the queues.
        /// </summary>
        private static void ProcessQueues()
        {
            PatchFile file;

            // handle error files
            while (!abort && queueErrors.TryDequeue(out file))
            {
                // raise errorcounter for this file
                file.ErrorCount++;

                // try again by enqueueing again
                if (file.ErrorCount < MAXRETRIES)
                {
                    file.LengthDone = 0;
                    queue.Enqueue(file);
                    if (!isHeadless)
                        form.DisplayInfo(String.Format(languageHandler.RetryingFile, file.Filename));
                }
                else
                {
                    if (!isHeadless)
                    {
                        MessageBox.Show(String.Format(languageHandler.FileFailed + file.Filename),
                            languageHandler.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // make sure to quit loop
                    abort = true;
                    isRunning = false;
                }
            }

            // handle finished files
            while (queueDone.TryDequeue(out file))
            {
                // raise counter for finished files
                filesDone++;
                if (!isHeadless)
                    form.DisplayInfo(String.Format(languageHandler.FileDownloaded, file.Filename));

                // check for finish
                if (filesDone >= files.Count)
                    isRunning = false;
            }
        }
        
        /// <summary>
        /// Reads the command line arguments
        /// </summary>
        /// <param name="args"></param>
        private static void ReadCommandLineArguments(string[] args)
        {
            foreach (string s in args)
            {
                // null or empty
                if (s == null || s.Length == 0)
                    return;

                // no valid argument tag char
                if (s[0] != '-' && s[0] != '/')
                    return;

                // get argument without tag char
                string arg = s.Substring(1);

                // evaluate argument
                switch(arg)
                {
                    case "headless": isHeadless = true; break;
                }
            }
        }

        private static void ReadCommandLineArgumentsClassic(string[] args)
        {
            // Classic client has two possible argument formats - both are a single string
            // with data in quotes except for an unquoted UPDATE in the second position.
            // The early classic updater contained 5 quoted fields, the modern one contains 6.
            // The difference is the last two arguments, 6 arg is missing patch file name
            //(e.g. clientpatch.txt) which is the second-last argument in 7 arg.

            // Get rid of quotes.
            for (int i = 0; i < args.Length; ++i)
                args[i] = args[i].Replace("\"", "");

            // Common to both.
            clientExecutable = args[0].Substring(args[0].LastIndexOf('\\') + 1);
            baseUrl = "http://" + args[2] + args[3];

            if (args.Length == 6)
                jsonUrl = "http://" + args[2] + args[4] + CLASSICDEFAULTJSON;
            else if (args.Length == 7)
                jsonUrl = "http://" + args[2] + args[4] + args[5];
        }

        /// <summary>
        /// Reads the file providing URLs for root path of files and the
        /// JSON data file for DotNet clients.
        /// </summary>
        /// <returns></returns>
        private static bool ReadUrlDataFile()
        {
            // parse url lines
            string[] urls = File.ReadAllLines(DOTNETURLDATAFILE);

            // show error in case content is invalid
            if (urls == null || urls.Length < 2)
            {
                if (!isHeadless)
                {
                    MessageBox.Show(String.Format(languageHandler.UrlInfoMissing + DOTNETURLDATAFILE),
                        languageHandler.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }

            // use URLs
            baseUrl = urls[0];
            jsonUrl = urls[1];

            // Assign client launch data from defaults.
            clientExecutable = CLIENTEXEAUTO;
            clientPath = CLIENTPATHAUTO;

            return true;
        }

        /// <summary>
        /// Reads in data from the classic client's update info file.
        /// Data is all on one line, 6 (old) or 7 (new) quoted fields
        /// except for field 2 which is UPDATE unquoted.
        /// </summary>
        /// <returns></returns>
        private static bool ReadUrlDataFileClassic()
        {
            string data = File.ReadAllText(CLASSICURLDATAFILE);

            // Match quoted fields which contain any valid letters, slashes,
            // colons, dots or numbers.
            Regex rxg = new Regex("\\\"[\\p{L}\\d\\\\\\:\\.\\/\\ ]*\\\"");
            MatchCollection mc = rxg.Matches(data);

            // show error in case content is invalid
            if (mc == null || mc.Count < 5 || mc.Count > 6)
            {
                if (!isHeadless)
                {
                    MessageBox.Show(String.Format(languageHandler.UrlInfoMissing + CLASSICURLDATAFILE),
                        languageHandler.ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }

            clientExecutable = mc[0].Value.Substring(mc[0].Value.LastIndexOf('\\') + 1).Replace("\"", "");
            string patchHost = mc[1].Value.Replace("\"", "");
            string patchPath = mc[2].Value.Replace("\"", "");
            string patchCachePath = mc[3].Value.Replace("\"", "");

            baseUrl = "http://" + patchHost + patchPath;
            if (mc.Count == 5)
            {
                jsonUrl = "http://" + patchHost + patchCachePath + CLASSICDEFAULTJSON;
                clientPath = mc[4].Value.Replace("\"", "");

            }
            else if (mc.Count == 6)
            {
                jsonUrl = "http://" + patchHost + patchCachePath + mc[4].Value.Replace("\"", "");
                clientPath = mc[5].Value.Replace("\"", "");
            }

            return true;
        }

        /// <summary>
        /// Parses the entries from patchinfo.txt
        /// </summary>
        /// <returns></returns>
        private static void ReadJsonData(byte[] JsonData)
        {
            // clear current instances if any
            files.Clear();

            // filestream on file
            MemoryStream fs = new MemoryStream(JsonData);

            // json reader
            DataContractJsonSerializer reader = 
                new DataContractJsonSerializer(typeof(List<PatchFile>));
            
            // deserialize list of PatchFile
            List<PatchFile> list = (List<PatchFile>)reader.ReadObject(fs);

            // cleanup filestream
            fs.Close();
            fs.Dispose();

            // remove unwanted entries
            for (int i = list.Count - 1; i >= 0; i--)
            {
                PatchFile f = list[i];

                // remove files marked to be not downloaded
                if (!f.Download)
                    list.RemoveAt(i);

                // look for hardcoded exclusions
                else 
                {
                    foreach (string s in EXCLUSIONS)
                    {
                        if (f.Filename.Equals(s))
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            // add them to the real list instance
            files.AddRange(list);
        }

        /// <summary>
        /// Raised when the UI is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // make sure to quit loop
            abort = true;
            isRunning = false;
        }

        /// <summary>
        /// Raised when the JSON patch data is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnWebClientDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            // error downloading patchinfo.txt
            if (e.Error != null)
            {
                if (!isHeadless)
                {
                    form.DisplayInfo(languageHandler.PatchDownloadFailed);
                    DialogResult result = MessageBox.Show(languageHandler.JsonDownloadFailed,
                        languageHandler.ErrorText, MessageBoxButtons.RetryCancel);
                    switch (result)
                    {
                        case DialogResult.Retry:
                            form.DisplayInfo(languageHandler.DownloadingPatch);
                            // OnWebClientDownloadDataCompleted event still fires at end.
                            webClient.DownloadDataAsync(new Uri(jsonUrl));
                            return;
                        case DialogResult.Cancel:
                            break;
                    }
                }

                abort = true;
                isRunning = false;
            }
            else
            {
                // parse json patch data
                ReadJsonData(e.Result);

                // enqueue entries
                foreach (PatchFile entry in files)
                    queue.Enqueue(entry);

                if (!isHeadless)
                {
                    form.DisplayInfo(languageHandler.ScanningFiles);
                }

                // create worker-instances and start them
                for (int i = 0; i < workers.Length; i++)
                {                   
                    workers[i] = new Worker(
                        PATCHERPATH,
                        baseUrl,
                        queue,
                        queueDone,
                        queueErrors);

                    workers[i].Start();
                }
            }
        }
    }
}
