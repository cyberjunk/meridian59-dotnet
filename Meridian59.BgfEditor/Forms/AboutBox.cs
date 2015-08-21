using Meridian59.Files.BGF;
using System;
using System.IO;
using System.Windows.Forms;

namespace Meridian59.BgfEditor
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            lblVersion.Text = BgfFile.VERSION9.ToString() + " / " + BgfFile.VERSION10.ToString();

            // check for crush32.dll either in program directory or system32 (syswow64)
            if (System.IO.File.Exists("crush32.dll") ||
                System.IO.File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "crush32.dll")))
            {
                txtCrush.Text = "AVAILABLE";
            }
            else
                txtCrush.Text = "NOT AVAILABLE";
        }
    }
}
