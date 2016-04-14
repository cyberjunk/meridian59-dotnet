/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Common.Events;
using Meridian59.Data.Models;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.Events;

using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Handles the downloading of client patch files and updater launch.
    /// </summary>
    public class DownloadHandler : IDisposable
    {
        #region Constants
        protected const string DEFAULTUPDATEFILE = "club.exe";
        protected const string DEFAULTUPDATEFILEPATH = "..\\";
        protected const string SUCCESSMESSAGE = "Success!";
        protected const string DOWNLOADFAIL = "Failed to download file.";
        protected const string CACHELOADFAIL = "Unable to load patch file. Please contact an administrator.";
        #endregion

        #region Event Handlers
        public event EventHandler DownloadStarted;
        public event EventHandler<StringEventArgs> DownloadText;
        public event EventHandler<IntegerEventArgs> DownloadProgress;
        public event EventHandler<StringEventArgs> DownloadFinished;
        public event EventHandler ExitRequestEvent;
        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~DownloadHandler()
        {
            Dispose(false);
        }
        #endregion

        #region WebClient Events

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Update UI with progress so far.
            if (DownloadProgress != null)
                DownloadProgress(this, new IntegerEventArgs(e.ProgressPercentage));
        }

        // The event that will trigger when the WebClient is completed
        private void CacheFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // Let client know download failed/was cancelled.
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs(DOWNLOADFAIL));
                return;
            }
            CacheFileProcess((ClientPatchInfo)e.UserState);
        }

        // The event that will trigger when the WebClient is completed
        private void UpdaterFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs(DOWNLOADFAIL));
                return;
            }
            UpdaterLaunch((ClientPatchInfo)e.UserState);
        }

        #endregion

        #region Download Client Patch

        public void DownloadClientPatch(ClientPatchInfo ClientPatchInfo)
        {
            if (DownloadStarted != null)
                DownloadStarted(this, new EventArgs());
            // Set download progress to 0%.
            if (DownloadProgress != null)
                DownloadProgress(this, new IntegerEventArgs(0));

            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CacheFileCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            // Update UI with download info.
            if (DownloadText != null)
                DownloadText(this, new StringEventArgs("Downloading " + ClientPatchInfo.PatchFile));
            // Download cache file first.
            try
            {
                // Start downloading the file
                webClient.DownloadFileAsync(ClientPatchInfo.GetCacheURL(), ClientPatchInfo.PatchFile, ClientPatchInfo);
            }
            catch (Exception ex)
            {
                // Update UI with failed web download message.
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs("Error: " + ex.Message));
            }
        }

        private void CacheFileProcess(ClientPatchInfo ClientPatchInfo)
        {
            // Get the executable path.
            string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // Combine with cache filename from ClientPatchInfo.
            string patchFilePath = System.IO.Path.Combine(currentPath, ClientPatchInfo.PatchFile);

            // Read the JSON array.
            List<PatchFile> pL = JsonConvert.DeserializeObject<List<PatchFile>>(File.ReadAllText(patchFilePath));
            // Try to get club.exe.
            PatchFile p = pL.FirstOrDefault(x => x.Filename == DEFAULTUPDATEFILE);

            if (p == null)
            {
                // Tell UI that we failed to load JSON/find club... split into two errors?
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs(CACHELOADFAIL));
                return;
            }

            if (!CompareCacheToLocalFile(System.IO.Path.Combine(currentPath, DEFAULTUPDATEFILEPATH), p))
            {
                // Download Updater.
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(UpdaterFileCompleted);

                // Set download progress back to 0%.
                if (DownloadProgress != null)
                    DownloadProgress(this, new IntegerEventArgs(0));

                // Update UI with new file to download.
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs("Downloading " + p.Filename));

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(ClientPatchInfo.GetUpdaterURL(p.Filename),
                        "..\\" + p.Filename, ClientPatchInfo);
                }
                catch (Exception ex)
                {
                    // Update UI with failed download message.
                    if (DownloadText != null)
                        DownloadText(this, new StringEventArgs("Error: " + ex.Message));
                }
            }
            else
            {
                // Have a valid version of the updater, launch it.
                UpdaterLaunch(ClientPatchInfo);
            }
        }

        private void UpdaterLaunch(ClientPatchInfo ClientPatchInfo)
        {
            // Location of client executable.
            string clientExec = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string clientPath = System.IO.Path.GetDirectoryName(clientExec);

            Process process = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.FileName = System.IO.Path.Combine(clientPath, DEFAULTUPDATEFILEPATH, DEFAULTUPDATEFILE);
            pi.Arguments = "\"" + clientExec + "\"" + " UPDATE \""
                + ClientPatchInfo.Machine + "\" \"" + ClientPatchInfo.PatchPath
                + "\" \"" + System.IO.Path.Combine(clientPath, ClientPatchInfo.PatchFile)
                + "\" \"" + "..\\\"";
            pi.UseShellExecute = true;
            pi.WorkingDirectory = System.IO.Path.Combine(clientPath, DEFAULTUPDATEFILEPATH);
            process.StartInfo = pi;

            process.Start();

            if (DownloadFinished != null)
                DownloadFinished(this, new StringEventArgs(SUCCESSMESSAGE));

            if (ExitRequestEvent != null)
                ExitRequestEvent(this, new EventArgs());
        }

        private bool CompareCacheToLocalFile(string CurrentPath, PatchFile PatchFile)
        {
            string combinePath = System.IO.Path.Combine(CurrentPath, PatchFile.Filename);
            // Check if we already have club.exe locally, if it's the same, don't download it again.
            if (!System.IO.File.Exists(combinePath))
                return false;

            // Open file.
            FileStream stream = File.OpenRead(combinePath);

            // Check file length.
            if (stream.Length != PatchFile.Length)
            {
                stream.Close();
                return false;
            }

            // Number of bytes to read.
            long numBytesToRead = 1024;
            // Make sure we dont read past the end of a file smaller than 1024 bytes.
            if (stream.Length < 1024)
                numBytesToRead = stream.Length;

            byte[] fileBytes = new byte[numBytesToRead];
            byte[] fileNameBytes = Encoding.ASCII.GetBytes(PatchFile.Filename);
            byte[] hashableBytes = new byte[numBytesToRead + PatchFile.Filename.Length];

            // Read bytes.
            stream.Read(fileBytes, 0, (int)numBytesToRead);
            stream.Close();

            // Copy to buffer.
            Buffer.BlockCopy(fileBytes, 0, hashableBytes, 0, fileBytes.Length);
            Buffer.BlockCopy(fileNameBytes, 0, hashableBytes, fileBytes.Length, fileNameBytes.Length);

            // Hash bytes
            string localHash = BitConverter.ToString(MeridianMD5.ComputeGenericMD5(hashableBytes)).Replace("-", "");

            // Compare hashes
            return (localHash == PatchFile.MyHash);
        }

        #endregion

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public class PatchFileList
    {
        public List<PatchFile> PatchFile;
    }

    public class PatchFile
    {
        public string Basepath;
        public string Filename;
        public string MyHash;
        public long Length;
        public bool Download;
        public int Version;
    }
}
