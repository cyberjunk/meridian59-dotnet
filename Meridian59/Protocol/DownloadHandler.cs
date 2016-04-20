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

using Meridian59.Common.Events;
using Meridian59.Data.Models;

using System.Net;
using System.Diagnostics;
using System.Reflection;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Handles the downloading of client patch files and updater launch.
    /// </summary>
    public class DownloadHandler : IDisposable
    {
        #region Constants
        protected const string DEFAULTUPDATEFILEPATH = "..\\";
        protected const string URLDATAFILE = "patchurl.txt";
        protected const string SUCCESSMESSAGE = "Success!";
        protected const string DOWNLOADFAIL = "Failed to download updater.";
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

        /// <summary>
        /// Downloads the current version of the game patcher.
        /// Once done the client quits and the patcher launches.
        /// </summary>
        /// <param name="ClientPatchInfo"></param>
        public void DownloadClientPatcher(ClientPatchInfo ClientPatchInfo)
        {
            if (DownloadStarted != null)
                DownloadStarted(this, new EventArgs());

            // Set download progress to 0%.
            if (DownloadProgress != null)
                DownloadProgress(this, new IntegerEventArgs(0));

            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnUpdaterDownloadCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnUpdaterDownloadProgressChanged);

            // Update UI with download info.
            if (DownloadText != null)
                DownloadText(this, new StringEventArgs("Downloading Patcher"));

            try
            {
                // Start downloading the file
                webClient.DownloadFileAsync(new Uri(ClientPatchInfo.GetUpdaterURL()),
                    DEFAULTUPDATEFILEPATH + ClientPatchInfo.UpdaterFile, ClientPatchInfo);
            }
            catch (Exception ex)
            {
                // Update UI with failed web download message.
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs("Error: " + ex.Message));
            }
        }

        private void OnUpdaterDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (DownloadProgress != null)
                DownloadProgress(this, new IntegerEventArgs(e.ProgressPercentage));
        }

        private void OnUpdaterDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled == true)
            {
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs(DOWNLOADFAIL));

                return;
            }

            // get attached userdata
            ClientPatchInfo info = (ClientPatchInfo)e.UserState;
            
            // write patcher data
            File.WriteAllLines(DEFAULTUPDATEFILEPATH + URLDATAFILE,
                new string[] { info.GetUpdateBasePath(), info.GetJsonDataURL() });

            // startup patcher
            string clientExec = Assembly.GetEntryAssembly().Location;
            string clientPath = Path.GetDirectoryName(clientExec);

            ProcessStartInfo pi = new ProcessStartInfo();
            pi.FileName         = Path.Combine(clientPath, DEFAULTUPDATEFILEPATH, info.UpdaterFile);
            pi.WorkingDirectory = Path.Combine(clientPath, DEFAULTUPDATEFILEPATH);
            pi.Arguments        = "";
            pi.UseShellExecute  = true;

            Process process = new Process();
            process.StartInfo = pi;
            process.Start();

            if (DownloadFinished != null)
                DownloadFinished(this, new StringEventArgs(SUCCESSMESSAGE));

            if (ExitRequestEvent != null)
                ExitRequestEvent(this, new EventArgs());
        }

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
}
