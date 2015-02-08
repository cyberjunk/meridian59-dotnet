using Meridian59.Files.BGF;
using System.Windows.Forms;

namespace Meridian59.BgfEditor
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            lblVersion.Text = BgfFile.VERSION9.ToString() + " / " + BgfFile.VERSION10.ToString();

            if (System.IO.File.Exists("crush32.dll"))
            {
                txtCrush.Text = "AVAILABLE";
            }
            else
                txtCrush.Text = "NOT AVAILABLE";
        }
    }
}
