using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Meridian59.Patcher
{
    public partial class DownloadForm : Form
    {
        private readonly List<PatchFile> files;
        private readonly LanguageHandler languageHandler;
        private readonly JsonFileProgress jsonFileProgress;
        private double lastTick;
        private long lastLengthDone;
        private UpdateStage updateStage;

        public UpdateStage UpdateStage
        {
            get
            {
                return updateStage;
            }
            set
            {
                // Reset lastLengthDone if switching out of DownloadingJson.
                if (updateStage == UpdateStage.DownloadingJson && value != updateStage)
                    lastLengthDone = 0;
                if (value == UpdateStage.FinishedTransition)
                {
                    progressOverall.Text = languageHandler.ProgressFinished;
                    progressOverall.Value = progressOverall.Maximum;
                }
                else if (value == UpdateStage.Abort)
                {
                    statusText.Text = languageHandler.UpdateAborted;
                    progressOverall.Text = languageHandler.ProgressAborted;
                    progressOverall.Value = progressOverall.Maximum;
                }
                updateStage = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="files"></param>
        public DownloadForm(List<PatchFile> files, LanguageHandler languageHandler, JsonFileProgress jsonFileProgress)
        {
            this.languageHandler = languageHandler;
            this.files = files;
            this.jsonFileProgress = jsonFileProgress;
            updateStage = UpdateStage.None;
            InitializeComponent();
            CenterToScreen();
        }

        /// <summary>
        /// Displays a string in the infobox.
        /// </summary>
        /// <param name="Info"></param>
        public void DisplayStatus(string Status)
        {
            statusText.Text = Status;
        }

        /// <summary>
        /// Updates the UI based on gathered stats from the files.
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="MaxStages"></param>
        /// <param name="X64NgenDone"></param>
        /// <param name="X86NgenDone"></param>
        public void Tick(double Tick, int MaxStages, bool X64NgenDone, bool X86NgenDone)
        {
            // Determine whether we are updating UI text this tick.
            double msinterval = Tick - lastTick;
            bool updateUIText = msinterval > 500;
            if (updateUIText)
                lastTick = Tick;

            switch (updateStage)
            {
                case UpdateStage.None:
                    // Make sure progressbar is empty, nothing else to do.
                    progressOverall.Value = 0;
                    break;
                case UpdateStage.Ngen:
                    UpdateDownloadFormNgen(msinterval, updateUIText, X64NgenDone, X86NgenDone);
                    break;
                case UpdateStage.DownloadingFiles:
                    UpdateDownloadFormDownloadingFiles(msinterval, updateUIText, MaxStages);
                    break;
                case UpdateStage.DownloadingJson:
                    UpdateDownloadFormDownloadingJson(msinterval, updateUIText);
                    break;
                case UpdateStage.HashingFiles:
                    UpdateDownloadFormHashing(updateUIText);
                    break;
                default:
                    break;
            }
        }

        private void UpdateDownloadFormDownloadingJson(double MsInterval, bool UpdateUIText)
        {
            // These have lockings, get once.
            long todo = jsonFileProgress.BytesTotal;
            long done = jsonFileProgress.BytesReceived;

            // update download speed and processed bytes not more than once per second
            if (UpdateUIText)
            {
                // update speed for last interval
                double bytes_in_interval = done - lastLengthDone;
                double interval_in_s = 0.001 * MsInterval;

                double kbps = (interval_in_s < 0.0001 && interval_in_s > -0.0001) ? 0.0 :
                    0.001 * (bytes_in_interval / interval_in_s);

                // update processed MB counter
                double done_mb = (double)done / (1024.0 * 1024.0);
                double todo_mb = (double)todo / (1024.0 * 1024.0);
                progressOverall.Text =
                    String.Format("{0:0.00}", done_mb) + " / " +
                    String.Format("{0:0.00} MB", todo_mb) + " @ " +
                    String.Format("{0:0.00} KB/s", kbps);

                // remember values for next execution
                lastLengthDone = done;
            }

            // update progress bar
            double progress = (todo == 0) ? 0.0 : (double)done / (double)todo;
            int progressint = Convert.ToInt32(progress * (double)progressOverall.Maximum / 10.0); // 10% of total
            progressOverall.Value = Math.Min(progressOverall.Maximum, progressint);
        }

        private void UpdateDownloadFormHashing(bool UpdateUIText)
        {
            int numfiles = 0;
            int numnothashed = 0;

            // iterate files
            foreach (PatchFile file in files)
            {
                // get this once, cause they have a locking
                PatchFileHashedStatus filestatus = file.HashedStatus;

                if (filestatus == PatchFileHashedStatus.NotHashed)
                    numnothashed++;

                // filescounter
                numfiles++;
            }

            int numdone = numfiles - numnothashed;
            // update progress bar
            double progress = (numfiles == 0) ? 0.0 : (double)numdone / (double)numfiles;
            int progressint = Convert.ToInt32(((progress * (double)progressOverall.Maximum) * 0.4) + (double)progressOverall.Maximum * 0.1);
            progressOverall.Value = Math.Min(progressOverall.Maximum, progressint);

            // update processed files not more than once per second
            if (UpdateUIText)
                progressOverall.Text = String.Format(languageHandler.NumFiles, numdone, numfiles);
        }

        private void UpdateDownloadFormDownloadingFiles(double MsInterval, bool UpdateUIText, int MaxStages)
        {
            int numfiles = 0;
            int numdone = 0;
            long todo = 0;
            long done = 0;

            // iterate files
            foreach (PatchFile file in files)
            {
                // get these once, cause they have a locking
                long filedone = file.LengthDone;
                PatchFileHashedStatus filestatus = file.HashedStatus;

                if (filestatus == PatchFileHashedStatus.Download)
                {
                    // byte-counters
                    todo += file.Length;
                    done += filedone;

                    // done files counter
                    if (filedone >= file.Length)
                        numdone++;
                    numfiles++;
                }
            }

            // update download speed and processed bytes not more than once per second
            if (UpdateUIText)
            {
                // update speed for last interval
                double bytes_in_interval = done - lastLengthDone;
                double interval_in_s = 0.001 * MsInterval;

                double kbps = (interval_in_s < 0.0001 && interval_in_s > -0.0001) ? 0.0 :
                    0.001 * (bytes_in_interval / interval_in_s);

                // update processed MB counter
                double done_mb = (double)done / (1024.0 * 1024.0);
                double todo_mb = (double)todo / (1024.0 * 1024.0);
                progressOverall.Text =
                    String.Format("{0:0.00}", done_mb) + " / " +
                    String.Format("{0:0.00} MB", todo_mb) + " @ " +
                    String.Format("{0:0.00} KB/s", kbps);

                // remember values for next execution
                lastLengthDone = done;
            }

            // update progress bar
            double progress = (todo == 0) ? 0.0 : (double)done / (double)todo;
            double maxStageProgress = (MaxStages == 4 ? 0.4 : 0.5);
            int progressint = Convert.ToInt32(((progress * (double)progressOverall.Maximum) * maxStageProgress) + (double)progressOverall.Maximum * 0.5);
            progressOverall.Value = Math.Min(progressOverall.Maximum, progressint);
        }

        private void UpdateDownloadFormNgen(double MsInterval, bool UpdateUIText, bool X64NgenDone, bool X86NgenDone)
        {
            int done = 0;
            int todo = 2;
            // update progress bar
            if (X64NgenDone)
                done++;
            if (X86NgenDone)
                done++;

            // update processed files not more than once per second
            if (UpdateUIText)
                progressOverall.Text = String.Format(languageHandler.NumFiles, done, todo);
            double progress = (done == 0) ? 0.0 : (double)done / (double)todo;
            int progressint = Convert.ToInt32(((progress * (double)progressOverall.Maximum) * 0.1) + (double)progressOverall.Maximum * 0.9);
            progressOverall.Value = Math.Min(progressOverall.Maximum, progressint);
        }

        private void OnDownloadFormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(languageHandler.ConfirmCancel,
                languageHandler.AbortText, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
