using Meridian59.Files.BGF;
using Meridian59.Drawing2D;
using System;
using System.IO;
using System.Windows.Forms;

namespace Meridian59.BgfEditor
{
    public partial class SourceTargetForm : Form
    {       
        public SourceTargetMode Mode { get; set; }

        public SourceTargetForm()
        {
            InitializeComponent();
            txtSource.DataBindings.Clear();
            txtSource.DataBindings.Add("Text", fbdSource, "SelectedPath");
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            DialogResult diagResult = fbdSource.ShowDialog();

            if (diagResult == DialogResult.OK)
            {
                txtSource.Text = fbdSource.SelectedPath;
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            DialogResult diagResult = fbdTarget.ShowDialog();

            if (diagResult == DialogResult.OK)
            {
                txtTarget.Text = fbdTarget.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtSource.Text) && Directory.Exists(txtTarget.Text))
            {
                string[] files = Directory.GetFiles(txtSource.Text, "*.bgf");

                switch (Mode)
                {
                    case SourceTargetMode.Extract:
                        foreach (string file in files)
                        {
                            try
                            {
                                BgfFile bgf = new BgfFile(file);
                                bgf.WriteXml(txtTarget.Text + "\\" + bgf.Filename);

                                txtLog.Text += "Extracted " + bgf.Filename + ".bgf" + Environment.NewLine;
                            }
                            catch (Exception)
                            {
                                txtLog.Text += "Error with file " + file;
                            }
                        }
                        break;

                    case SourceTargetMode.Decompress:
                        foreach (string file in files)
                        {
                            try
                            {
                                BgfFile bgf = new BgfFile(file);
                                foreach (BgfBitmap bitmap in bgf.Frames)
                                    bitmap.IsCompressed = false;

                                bgf.Save(txtTarget.Text + "\\" + bgf.Filename + ".bgf");

                                txtLog.Text += "Decompressed " + bgf.Filename + ".bgf" + Environment.NewLine;
                            }
                            catch (Exception)
                            {
                                txtLog.Text += "Error with file " + file;
                            }
                        }
                        break;

                    case SourceTargetMode.SetVersion9:
                        foreach (string file in files)
                        {
                            try
                            {
                                BgfFile bgf = new BgfFile(file);
                                bgf.Version = BgfFile.VERSION9;
                                bgf.Save(txtTarget.Text + "\\" + bgf.Filename + ".bgf");

                                txtLog.Text += "Converted " + bgf.Filename + ".bgf" + " to V9 (crush32)" + Environment.NewLine;
                            }
                            catch (Exception)
                            {
                                txtLog.Text += "Error with file " + file;
                            }
                        }
                        break;

                    case SourceTargetMode.SetVersion10:
                        foreach (string file in files)
                        {
                            try
                            {
                                BgfFile bgf = new BgfFile(file);
                                bgf.Version = BgfFile.VERSION10;
                                bgf.Save(txtTarget.Text + "\\" + bgf.Filename + ".bgf");

                                txtLog.Text += "Converted " + bgf.Filename + ".bgf" + " to V10 (zlib)" + Environment.NewLine;
                            }
                            catch (Exception)
                            {
                                txtLog.Text += "Error with file " + file;
                            }
                        }
                        break;

                    case SourceTargetMode.SetVersion10FromVale:
                        foreach (string file in files)
                        {
                            try
                            {
                                BgfFile bgf = new BgfFile(file);
                                bgf.ConvertFromVale();
                                bgf.Version = BgfFile.VERSION10;
                                bgf.Save(txtTarget.Text + "\\" + bgf.Filename + ".bgf");

                                txtLog.Text += "Converted " + bgf.Filename + ".bgf" + " to V10 (zlib)" + Environment.NewLine;
                            }
                            catch (Exception)
                            {
                                txtLog.Text += "Error with file " + file;
                            }
                        }
                        break;
                }
            }
        }
    }
}
