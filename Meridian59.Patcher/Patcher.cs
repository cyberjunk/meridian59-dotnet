﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Windows.Forms;

namespace Meridian59.Patcher
{
    public static class Patcher
    {
        private const int NUMWORKERS       = 8;
        private const int MAXRETRIES       = 3;
        private const string URLDATAFILE   = "patchurl.txt";
        private const string CLIENTEXE     = "Meridian59.Ogre.Client.exe";
        private const string PATCHEREXE    = "Meridian59.Patcher.exe";
        private const string FOLDERX64     = "x64";
        private const string FOLDERX86     = "x86";
        private const string CLIENTX64     = FOLDERX64 + "/" + CLIENTEXE;
        private const string CLIENTX86     = FOLDERX86 + "/" + CLIENTEXE;

        private static readonly string[] EXCLUSIONS  = new string[] { "club.exe", PATCHEREXE };
        private static readonly double MSTICKDIVISOR = (double)Stopwatch.Frequency / 1000.0;      
        
        private static readonly ConcurrentQueue<PatchFile> queue = new ConcurrentQueue<PatchFile>();
        private static readonly ConcurrentQueue<PatchFile> queueFinished = new ConcurrentQueue<PatchFile>();
        private static readonly ConcurrentQueue<PatchFile> queueErrors = new ConcurrentQueue<PatchFile>();

        private static readonly List<PatchFile> files   = new List<PatchFile>();
        private static readonly Worker[] workers        = new Worker[NUMWORKERS];       
        private static readonly Stopwatch watch         = new Stopwatch();
        private static readonly WebClient webClient     = new WebClient();

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

            ///////////////////////////////////////////////////////////////////////

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
     
            // start client in case patching went well
            if (!abort)
            {
                string clientExec = Assembly.GetEntryAssembly().Location;
                string clientPath = Path.GetDirectoryName(clientExec);
                string fileName   = Environment.Is64BitOperatingSystem ? CLIENTX64 : CLIENTX86;
                string workDir    = Environment.Is64BitOperatingSystem ? FOLDERX64 : FOLDERX86;

                ProcessStartInfo pi = new ProcessStartInfo();
                pi.FileName         = Path.Combine(clientPath, fileName);
                pi.Arguments        = "";
                pi.UseShellExecute  = true;
                pi.WorkingDirectory = Path.Combine(clientPath, workDir);

                Process process = new Process();
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
            while (queueFinished.TryDequeue(out file))
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
                    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                    string assemblyPath = Path.GetDirectoryName(assemblyLocation);

                    workers[i] = new Worker(
                        assemblyPath,
                        baseUrl,
                        queue,
                        queueFinished,
                        queueErrors);

                    workers[i].Start();
                }
            }
        }
    }
}