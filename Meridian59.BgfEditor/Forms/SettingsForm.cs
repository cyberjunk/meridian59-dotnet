using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Meridian59.BgfEditor
{
    public partial class SettingsForm : Form
    {
        public uint ShrinkFactor
        {
            get { return Convert.ToUInt32(numericUpDown1.Value); }
            set { numericUpDown1.Value = value; }
        }

        public uint Version
        {
            get { return Convert.ToUInt32(numericUpDown2.Value); }
            set {
                if (value > numericUpDown2.Maximum ||
                    value < numericUpDown2.Minimum)
                {
                    numericUpDown2.Value = numericUpDown2.Maximum;
                }
                else
                    numericUpDown2.Value = value;
            }
        }

        public bool IsSaveCompresed
        {
            get { return chkSaveCompressed.Checked; }
            set
            {
                chkSaveCompressed.Checked = value;
            }
        }

        public string BgfName
        {
            get { return txtName.Text; }
            set
            {
                txtName.Text = value;
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // cancel close and hide instead
            e.Cancel = true;
            Hide();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {           
            Hide();
        }
    }
}
