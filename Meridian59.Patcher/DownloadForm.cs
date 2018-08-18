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
        private string appendLog;
        private double lastTick;
        private long lastLengthDone;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="files"></param>
        public DownloadForm(List<PatchFile> files, LanguageHandler languageHandler)
        {
            this.languageHandler = languageHandler;
            this.files = files;
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
            long todo = 0;
            long done = 0;
            int numfiles = 0;
            int numdone  = 0;

            // iterate files
            foreach(PatchFile file in files)
            {
                // get this once, cause it has a locking
                long filedone = file.LengthDone;

                // byte-counters
                todo += file.Length;
                done += filedone;

                // filescounter
                numfiles++;

                // done files counter
                if (filedone >= file.Length)
                    numdone++;
            }

            // update progress bar
            double progress       = (todo == 0) ? 1.0 : (double)done / (double)todo;
            int progressint       = Convert.ToInt32(progress * (double)progressOverall.Maximum);
            progressOverall.Value = Math.Min(progressOverall.Maximum, progressint);

            // update files-done counter
            lblFilesProcessed.Text = numdone + " / " + numfiles;

            // update textLog with pending lines and reset
            infoTextBox.AppendText(appendLog);
            appendLog = "";

            // update update download speed and processed bytes not more than once per second
            double msinterval = Tick - lastTick;
            if (msinterval > 500 || force_update)
            {
                // update speed for last interval
                double bytes_in_interval = done - lastLengthDone;
                double interval_in_s = 0.001 * msinterval;
                
                double kbps = (interval_in_s < 0.0001 && interval_in_s > -0.0001) ? 0.0 : 
                    0.001 * (bytes_in_interval / interval_in_s);

                lblSpeed.Text = String.Format("{0:0.00} KB/s", kbps);

                // update processed MB counter
                double done_mb = (double)done / (1024.0 * 1024.0);
                double todo_mb = (double)todo / (1024.0 * 1024.0);
                lblDataProcessed.Text = 
                    String.Format("{0:0.00} MB", done_mb) + " / " +
                    String.Format("{0:0.00} MB", todo_mb);

                // remember values for next execution
                lastLengthDone = done;
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
