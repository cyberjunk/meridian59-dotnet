using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Drawing2D;
using Meridian59.Files.BGF;

namespace Meridian59.BgfEditor
{
    public partial class AddFrameSetIndexForm : Form
    {
        public int CurrentFrameSetIndex = 0;

        public AddFrameSetIndexForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            cbFrames.DataSource = Program.CurrentFile.Frames;
            cbFrames.DisplayMember = BgfBitmap.PROPNAME_NUM;
        }

        protected void OnFramesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFrames.SelectedItem != null && cbFrames.SelectedItem is BgfBitmap)
            {
                BgfBitmap bgfBitmap = (BgfBitmap)cbFrames.SelectedItem;

                Program.ShowFrame(true, bgfBitmap.GetBitmap(), picBox);
            }
        }

        protected void OnOK_Click(object sender, EventArgs e)
        {
            Hide();

            // add selected frame
            if (cbFrames.SelectedItem != null &&
                cbFrames.SelectedItem is BgfBitmap &&
                Program.CurrentFile.FrameSets.Count > CurrentFrameSetIndex)
            {
                Program.HasFileChanged = true;
                Program.CurrentFile.FrameSets[CurrentFrameSetIndex].FrameIndices.Add(cbFrames.SelectedIndex);

                Program.MainForm.UpdateFrameNums();
                Program.MainForm.UpdateFrameSetFlow();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
