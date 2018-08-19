using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Meridian59.Patcher
{
    public partial class DownloadForm : Form
    {
        private readonly LanguageHandler languageHandler;
        private readonly PatchDownloadStats patchDownloadStats;
        private string appendLog;
        private double lastTick;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="files"></param>
        public DownloadForm(LanguageHandler languageHandler, PatchDownloadStats patchDownloadStats)
        {
            this.languageHandler = languageHandler;
            this.patchDownloadStats = patchDownloadStats;
            InitializeComponent();
            CenterToScreen();
        }

        /// <summary>
        /// Displays a string in the infobox.
        /// </summary>
        /// <param name="Info"></param>
        public void DisplayInfo(string Info)
        {
            appendLog += (Info) + Environment.NewLine;
        }

        /// <summary>
        /// Updates the UI based on gathered stats from the files.
        /// </summary>
        /// <param name="Tick"></param>
        public void Tick(double Tick, bool force_update)
        {
            long lengthToDownload = patchDownloadStats.LengthToDownload;
            long lengthDownloaded = patchDownloadStats.LengthDownloaded;

            // update progress bar
            double progress       = (lengthToDownload == 0) ? 1.0 : (double)lengthDownloaded / (double)lengthToDownload;
            int progressint       = Convert.ToInt32(progress * (double)progressOverall.Maximum);
            progressOverall.Value = Math.Min(progressOverall.Maximum, progressint);

            // update files-done counter
            lblFilesProcessed.Text = patchDownloadStats.NumFilesDone + " / " + patchDownloadStats.NumFilesToDownload;

            // update textLog with pending lines and reset
            infoTextBox.AppendText(appendLog);
            appendLog = "";

            // update update download speed and processed bytes not more than once per second
            double msinterval = Tick - lastTick;
            if (msinterval > 500 || force_update)
            {
                // update speed for last interval
                double bytes_in_interval = lengthDownloaded - patchDownloadStats.lastLengthDone;
                double interval_in_s = 0.001 * msinterval;
                
                double kbps = (interval_in_s < 0.0001 && interval_in_s > -0.0001) ? 0.0 : 
                    0.001 * (bytes_in_interval / interval_in_s);

                lblSpeed.Text = String.Format("{0:0.00} KB/s", kbps);

                // update processed MB counter
                double done_mb = (double)lengthDownloaded / (1024.0 * 1024.0);
                double todo_mb = (double)lengthToDownload / (1024.0 * 1024.0);
                lblDataProcessed.Text = 
                    String.Format("{0:0.00} MB", done_mb) + " / " +
                    String.Format("{0:0.00} MB", todo_mb);

                // remember values for next execution
                patchDownloadStats.lastLengthDone = lengthDownloaded;
                lastTick = Tick;
            }
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
