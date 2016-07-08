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
        private const int NUMWORKERS        = 8;
        private const int MAXRETRIES        = 3;
        private const string URLDATAFILE    = "patchurl.txt";
        private const string CLIENTEXENAME  = "Meridian59.Ogre.Client.exe";
        private const string CLIENTPROCESS  = "Meridian59.Ogre.Client";
        private const string PATCHEREXENAME = "Meridian59.Patcher.exe";
        private const string FOLDERX64      = "x64";
        private const string FOLDERX86      = "x86";
        private const string NGENX64        = "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\ngen.exe";
        private const string NGENX86        = "C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\ngen.exe";

        private static readonly string[] EXCLUSIONS   = new string[] { "club.exe", PATCHEREXENAME };
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
        
        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // start ticker
            watch.Start();

            // parse commandline arguments
            ReadCommandLineArguments(args);

            // create UI if not-headless
            if (!isHeadless)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // create ui
                form = new DownloadForm(files);
                form.FormClosed += OnFormClosed;
                form.Show();
            }

            // read urls
            if (!ReadUrlDataFile())
                return;

            // start download of patchinfo.txt
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
                        form.Tick(mstick);

                    // process messages
                    Application.DoEvents();
                }

                // sleep a bit (-> ~60 FPS)
                Thread.Sleep(16);
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
                if (principal.IsInRole(WindowsBuiltInRole.Administrator))                  
                {
                    if (File.Exists(NGENX64))
                    {
                        // start ngen
                        pi                  = new ProcessStartInfo();
                        pi.FileName         = NGENX64;
                        pi.Arguments        = "install Meridian59.Ogre.Client.exe";
                        pi.UseShellExecute  = true;
                        pi.WorkingDirectory = CLIENTPATHX64;
                        
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

                        process = new Process();
                        process.StartInfo = pi;
                        process.Start();
                        process.WaitForExit();
                    }
                }

                // start client
                pi                  = new ProcessStartInfo();
                pi.FileName         = CLIENTEXEAUTO;
                pi.Arguments        = "";
                pi.UseShellExecute  = true;
                pi.WorkingDirectory = CLIENTPATHAUTO;

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
                }
                else
                {
                    if (!isHeadless)
                    {
                        MessageBox.Show("Download of file " + file.Filename + " failed.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Reads the file providing URLs for root path of files and the
        /// JSON data file.
        /// </summary>
        /// <returns></returns>
        private static bool ReadUrlDataFile()
        {
            // show error in case the URLDATAFILE is missing
            if (!File.Exists(URLDATAFILE))
            {
                if (!isHeadless)
                {
                    MessageBox.Show("Required file " + URLDATAFILE + " is missing. Please reinstall the client.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }

            // parse url lines
            string[] urls = File.ReadAllLines(URLDATAFILE);

            // show error in case content is invalid
            if (urls == null || urls.Length < 2)
            {
                if (!isHeadless)
                {
                    MessageBox.Show("File " + URLDATAFILE + " is corrupted. Please reinstall the client.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }

            // use URLs
            baseUrl = urls[0];
            jsonUrl = urls[1];

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
                    MessageBox.Show("Download of JSON patch data failed. Please try again or reinstall client.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
