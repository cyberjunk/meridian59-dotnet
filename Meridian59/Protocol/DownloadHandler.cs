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
using System.Diagnostics;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Handles the downloading of client patch files and updater launch.
    /// </summary>
    public class DownloadHandler : IDisposable
    {
        #region Constants
        protected const string DEFAULTUPDATEFILEPATH = "..\\";
        protected const string SUCCESSMESSAGE = "Success!";
        protected const string DOWNLOADFAIL = "Failed to download file.";
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
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(UpdaterFileCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            // Update UI with download info.
            if (DownloadText != null)
                DownloadText(this, new StringEventArgs("Downloading " + ClientPatchInfo.UpdaterFile));

            // Download Updater.
            try
            {
                webClient.DownloadFileAsync(ClientPatchInfo.GetUpdaterURL(),
                    "..\\" + ClientPatchInfo.UpdaterFile, ClientPatchInfo);
            }
            catch (Exception ex)
            {
                // Update UI with failed web download message.
                if (DownloadText != null)
                    DownloadText(this, new StringEventArgs("Error: " + ex.Message));
            }
        }

        private void UpdaterLaunch(ClientPatchInfo ClientPatchInfo)
        {
            // Location of client executable.
            string clientExec = System.Reflection.Assembly.GetEntryAssembly().Location;
            string clientPath = System.IO.Path.GetDirectoryName(clientExec);

            // Create the arguments to run the updater with.
            string args = String.Format(" \"{0}\" UPDATE \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"",
                clientExec, ClientPatchInfo.Machine, ClientPatchInfo.PatchPath,
                ClientPatchInfo.PatchCachePath, ClientPatchInfo.PatchFile,
                DEFAULTUPDATEFILEPATH);
            // Save the arguments to a file also, for launching updater separately.
            System.IO.File.WriteAllText(DEFAULTUPDATEFILEPATH + "dlinfo.txt", args);

            Process process = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.FileName = System.IO.Path.Combine(clientPath, DEFAULTUPDATEFILEPATH, ClientPatchInfo.UpdaterFile);
            pi.Arguments = args;
            pi.UseShellExecute = true;
            pi.WorkingDirectory = System.IO.Path.Combine(clientPath, DEFAULTUPDATEFILEPATH);
            process.StartInfo = pi;

            process.Start();

            if (DownloadFinished != null)
                DownloadFinished(this, new StringEventArgs(SUCCESSMESSAGE));

            if (ExitRequestEvent != null)
                ExitRequestEvent(this, new EventArgs());
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
}
