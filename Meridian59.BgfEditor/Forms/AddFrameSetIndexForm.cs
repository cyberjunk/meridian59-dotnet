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

            cbFramesMax.BindingContext = new BindingContext();
            cbFramesMax.DataSource = Program.CurrentFile.Frames;
            cbFramesMax.DisplayMember = BgfBitmap.PROPNAME_NUM;
        }

        protected void OnFramesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFrames.SelectedItem != null && cbFrames.SelectedItem is BgfBitmap)
            {
                BgfBitmap bgfBitmap = (BgfBitmap)cbFrames.SelectedItem;

                Program.ShowFrame(true, bgfBitmap.GetBitmap(), picBox);
            }
        }

        protected void OnFramesMaxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFramesMax.SelectedItem != null && cbFramesMax.SelectedItem is BgfBitmap)
            {
                BgfBitmap bgfBitmap = (BgfBitmap)cbFramesMax.SelectedItem;

                Program.ShowFrame(true, bgfBitmap.GetBitmap(), picBox2);
            }
        }

        protected void OnOK_Click(object sender, EventArgs e)
        {
            Hide();

            // add selected frame
            if (
                    cbFrames.SelectedItem != null &&
                    cbFrames.SelectedItem is BgfBitmap &&
                    Program.CurrentFile.FrameSets.Count > CurrentFrameSetIndex &&
                    cbFramesMax.SelectedItem != null &&
                    cbFramesMax.SelectedItem is BgfBitmap &&
                    Program.CurrentFile.FrameSets.Count > CurrentFrameSetIndex &&
                    cbFrames.SelectedIndex <= cbFramesMax.SelectedIndex
                )
            {
                Program.HasFileChanged = true;

                for (int x = cbFrames.SelectedIndex; x <= cbFramesMax.SelectedIndex; x++)
                {
                    Program.CurrentFile.FrameSets[CurrentFrameSetIndex].FrameIndices.Add(x);

                    Program.MainForm.UpdateFrameNums();
                    Program.MainForm.UpdateFrameSetFlow();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void AddFrameSetIndexForm_Load(object sender, EventArgs e)
        {

        }
    }
}
