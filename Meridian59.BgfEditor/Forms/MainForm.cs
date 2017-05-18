using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Meridian59.Files.BGF;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;
using Meridian59.Common.Enums;
using Meridian59.BgfEditor.Forms;

namespace Meridian59.BgfEditor
{
    public partial class MainForm : Form
    {
        public const string STR_AREYOUSURE = "Are you sure?";
        public const string STR_REMOVEGROUP = "Remove group";
        public const string STR_REMOVEFRAME = "Remove frame";
        public const string STR_ERRORSTILLINKED = "Can't remove. Still linked in a group!";

        public BgfBitmap SelectedFrame
        {
            get
            {
                if (dgFrames.SelectedRows.Count > 0 && dgFrames.SelectedRows[0].DataBoundItem != null)
                    return (BgfBitmap)dgFrames.SelectedRows[0].DataBoundItem;

                return null;
            }
        }

        public BgfBitmapHotspot SelectedHotspot
        {
            get
            {
                if (dgHotspots.SelectedRows.Count > 0 && dgHotspots.SelectedRows[0].DataBoundItem != null)
                    return (BgfBitmapHotspot)dgHotspots.SelectedRows[0].DataBoundItem;

                return null;
            }
        }

        public BgfFrameSet SelectedFrameSet
        {
            get
            {
                if (listFrameSets.SelectedIndex > -1)
                    return Program.CurrentFile.FrameSets[listFrameSets.SelectedIndex];

                return null;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        protected void OnLoad(object sender, EventArgs e)
        {
            dgFrames.AutoGenerateColumns = false;
            dgHotspots.AutoGenerateColumns = false;

            // set frames datasource, selection changes hotspots
            dgFrames.DataSource = Program.CurrentFile.Frames;

            // set framesets/groups datasource, selection changes framenums
            listFrameSets.DataSource = Program.CurrentFile.FrameSets;
        }

        protected void OnResizeEnd(object sender, EventArgs e)
        {
            BgfBitmap bgfBitmap = SelectedFrame;

            if (bgfBitmap != null)
                Program.ShowFrame(true, bgfBitmap.GetBitmap(), picFrameImage);
        }

        protected void OnFrameAddClick(object sender, EventArgs e)
        {
            fdAddFrame.ShowDialog();
        }

        protected void OnFrameRemoveClick(object sender, EventArgs e)
        {
            BgfBitmap bgfBitmap = SelectedFrame;

            if (bgfBitmap != null)
            {
                uint oldnum = bgfBitmap.Num;
                int index = dgFrames.Rows.IndexOf(dgFrames.SelectedRows[0]);

                if (index > -1)
                {
                    DialogResult = MessageBox.Show(STR_AREYOUSURE, STR_REMOVEFRAME, MessageBoxButtons.YesNo);
                    if (DialogResult == DialogResult.Yes)
                    {
                        if (Program.CurrentFile.IsFrameIndexLinkedInFrameSet(index))
                        {
                            MessageBox.Show(STR_ERRORSTILLINKED, "Error", MessageBoxButtons.OK);
                        }
                        else
                        {
                            Program.CurrentFile.Frames.Remove(bgfBitmap);

                            // adjust nums for rest
                            for (int i = (int)oldnum - 1; i < Program.CurrentFile.Frames.Count; i++)
                                Program.CurrentFile.Frames[i].Num--;
                        }
                    }
                }
            }
        }

        protected void OnFrameUpClick(object sender, EventArgs e)
        {
            BgfBitmap bgfBitmap = SelectedFrame;

            if (bgfBitmap != null)
            {
                int index = dgFrames.Rows.IndexOf(dgFrames.SelectedRows[0]);

                if (index > 0)
                {
                    // swap listitems
                    BgfBitmap temp = Program.CurrentFile.Frames[index - 1];
                    Program.CurrentFile.Frames[index - 1] = bgfBitmap;
                    Program.CurrentFile.Frames[index] = temp;

                    // update selection
                    dgFrames.Rows[index - 1].Selected = true;

                    Program.CurrentFile.UpdateFrameIndexInFrameSets(index - 1, -1);
                    Program.CurrentFile.UpdateFrameIndexInFrameSets(index, index - 1);
                    Program.CurrentFile.UpdateFrameIndexInFrameSets(-1, index);

                    // adjust nums
                    Program.CurrentFile.Frames[index - 1].Num--;
                    Program.CurrentFile.Frames[index].Num++;

                    UpdateFrameNums();
                }
            }
        }

        protected void OnFrameDownClick(object sender, EventArgs e)
        {
            BgfBitmap bgfBitmap = SelectedFrame;

            if (bgfBitmap != null)
            {
                int index = dgFrames.Rows.IndexOf(dgFrames.SelectedRows[0]);

                if (index < dgFrames.Rows.Count - 1)
                {
                    // swap listitems
                    BgfBitmap temp = Program.CurrentFile.Frames[index + 1];
                    Program.CurrentFile.Frames[index + 1] = bgfBitmap;
                    Program.CurrentFile.Frames[index] = temp;

                    // update selection
                    dgFrames.Rows[index + 1].Selected = true;

                    Program.CurrentFile.UpdateFrameIndexInFrameSets(index + 1, -1);
                    Program.CurrentFile.UpdateFrameIndexInFrameSets(index, index + 1);
                    Program.CurrentFile.UpdateFrameIndexInFrameSets(-1, index);

                    // adjust nums
                    Program.CurrentFile.Frames[index + 1].Num++;
                    Program.CurrentFile.Frames[index].Num--;

                    UpdateFrameNums();
                }
            }
        }

        protected void OnHotspotAddClick(object sender, EventArgs e)
        {
            BgfBitmap bgfBitmap = SelectedFrame;

            if (bgfBitmap != null)
            {
                bgfBitmap.HotSpots.Add(new BgfBitmapHotspot());
            }
        }

        protected void OnHotspotRemoveClick(object sender, EventArgs e)
        {
            BgfBitmapHotspot bgfHotspot = SelectedHotspot;

            if (bgfHotspot != null)
            {
                int index = dgHotspots.Rows.IndexOf(dgHotspots.SelectedRows[0]);

                if (index > -1)
                {
                    DialogResult = MessageBox.Show("Are you sure?", "Remove hotspot", MessageBoxButtons.YesNo);
                    if (DialogResult == DialogResult.Yes)
                    {
                        SelectedFrame.HotSpots.Remove(bgfHotspot);
                    }
                }
            }
        }

        protected void OnHotspotUpClick(object sender, EventArgs e)
        {
            BgfBitmapHotspot bgfHotspot = SelectedHotspot;

            if (bgfHotspot != null)
            {
                int index = dgHotspots.Rows.IndexOf(dgHotspots.SelectedRows[0]);

                if (index > 0)
                {
                    BgfBitmap bgfBitmap = SelectedFrame;

                    // swap listitems
                    BgfBitmapHotspot temp = bgfBitmap.HotSpots[index - 1];
                    bgfBitmap.HotSpots[index - 1] = bgfHotspot;
                    bgfBitmap.HotSpots[index] = temp;

                    // update selection
                    dgHotspots.Rows[index - 1].Selected = true;
                }
            }
        }

        protected void OnHotspotDownClick(object sender, EventArgs e)
        {
            BgfBitmapHotspot bgfHotspot = SelectedHotspot;

            if (bgfHotspot != null)
            {
                int index = dgHotspots.Rows.IndexOf(dgHotspots.SelectedRows[0]);

                if (index < dgHotspots.Rows.Count - 1)
                {
                    BgfBitmap bgfBitmap = SelectedFrame;

                    // swap listitems
                    if (bgfBitmap != null)
                    {
                        BgfBitmapHotspot temp = bgfBitmap.HotSpots[index + 1];
                        bgfBitmap.HotSpots[index + 1] = bgfHotspot;
                        bgfBitmap.HotSpots[index] = temp;

                        // update selection
                        dgHotspots.Rows[index + 1].Selected = true;
                    }
                }
            }
        }

        protected void OnFrameSetAddClick(object sender, EventArgs e)
        {
            Program.CurrentFile.AddFrameSet();

            if (listFrameSets.Items.Count > 0)
                listFrameSets.SelectedIndex = listFrameSets.Items.Count - 1;
        }

        protected void OnFrameSetRemoveClick(object sender, EventArgs e)
        {
            BgfFrameSet bgfFrameSet = SelectedFrameSet;

            if (bgfFrameSet != null)
            {
                uint oldnum = bgfFrameSet.Num;

                DialogResult = MessageBox.Show(STR_AREYOUSURE, STR_REMOVEGROUP, MessageBoxButtons.YesNo);
                if (DialogResult == DialogResult.Yes)
                {
                    Program.CurrentFile.FrameSets.Remove(bgfFrameSet);

                    // adjust nums for rest
                    for (int i = (int)oldnum - 1; i < Program.CurrentFile.FrameSets.Count; i++)
                        Program.CurrentFile.FrameSets[i].Num--;

                    UpdateFrameSetFlow();
                }
            }
        }

        protected void OnFrameSetUpClick(object sender, EventArgs e)
        {
            BgfFrameSet bgfFrameSet = SelectedFrameSet;

            if (bgfFrameSet != null)
            {
                int index = listFrameSets.SelectedIndex;

                if (index > 0)
                {
                    // swap listitems
                    BgfFrameSet temp = Program.CurrentFile.FrameSets[index - 1];
                    Program.CurrentFile.FrameSets[index - 1] = bgfFrameSet;
                    Program.CurrentFile.FrameSets[index] = temp;

                    // adjust nums
                    Program.CurrentFile.FrameSets[index - 1].Num--;
                    Program.CurrentFile.FrameSets[index].Num++;

                    // update selection
                    listFrameSets.SelectedIndex = index - 1;
                }
            }
        }

        protected void OnFrameSetDownClick(object sender, EventArgs e)
        {
            BgfFrameSet bgfFrameSet = SelectedFrameSet;
            int index = listFrameSets.SelectedIndex;

            if (bgfFrameSet != null &&
                index > -1 &&
                index < listFrameSets.Items.Count - 1)
            {
                // swap listitems
                BgfFrameSet temp = Program.CurrentFile.FrameSets[index + 1];
                Program.CurrentFile.FrameSets[index + 1] = bgfFrameSet;
                Program.CurrentFile.FrameSets[index] = temp;

                // adjust nums
                Program.CurrentFile.FrameSets[index + 1].Num++;
                Program.CurrentFile.FrameSets[index].Num--;

                // update selection
                listFrameSets.SelectedIndex = index + 1;
            }
        }

        protected void OnFrameIndexAddClick(object sender, EventArgs e)
        {
            Program.AddFrameSetIndexForm.CurrentFrameSetIndex = listFrameSets.SelectedIndex;
            Program.AddFrameSetIndexForm.Show();
        }

        protected void OnFrameIndexRemoveClick(object sender, EventArgs e)
        {
            if (listFrameNums.SelectedIndex > -1)
            {
                BgfFrameSet bgfFrameSet = SelectedFrameSet;

                if (bgfFrameSet != null)
                {
                    bgfFrameSet.FrameIndices.RemoveAt(listFrameNums.SelectedIndex);

                    UpdateFrameNums();
                    UpdateFrameSetFlow();
                }
            }
        }

        protected void OnFrameIndexUpClick(object sender, EventArgs e)
        {
            if (listFrameNums.SelectedIndex > 0)
            {
                BgfFrameSet bgfFrameSet = SelectedFrameSet;

                if (bgfFrameSet != null)
                {
                    int index = listFrameNums.SelectedIndex;

                    // swap listitems
                    int temp = bgfFrameSet.FrameIndices[index - 1];
                    bgfFrameSet.FrameIndices[index - 1] = bgfFrameSet.FrameIndices[index];
                    bgfFrameSet.FrameIndices[index] = temp;

                    UpdateFrameNums();
                    UpdateFrameSetFlow();

                    // update selection
                    listFrameNums.SelectedIndex = index - 1;
                }
            }
        }

        protected void OnFrameIndexDownClick(object sender, EventArgs e)
        {
            if (listFrameNums.SelectedIndex > -1 && listFrameNums.SelectedIndex < listFrameNums.Items.Count - 1)
            {
                BgfFrameSet bgfFrameSet = SelectedFrameSet;

                if (bgfFrameSet != null)
                {
                    int index = listFrameNums.SelectedIndex;

                    // swap listitems
                    int temp = bgfFrameSet.FrameIndices[index + 1];
                    bgfFrameSet.FrameIndices[index + 1] = bgfFrameSet.FrameIndices[index];
                    bgfFrameSet.FrameIndices[index] = temp;

                    UpdateFrameNums();
                    UpdateFrameSetFlow();

                    // update selection
                    listFrameNums.SelectedIndex = index + 1;
                }
            }
        }

        protected void OnMenuNewClick(object sender, EventArgs e)
        {
            Program.New();
        }

        protected void OnMenuOpenClick(object sender, EventArgs e)
        {
            fdOpenFile.ShowDialog();
        }

        protected void OnMenuSaveAsClick(object sender, EventArgs e)
        {
            fdSaveFile.FileName = Program.CurrentFile.Filename;
            fdSaveFile.ShowDialog();
        }

        protected void OnMenuAboutClick(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        protected void OnMenuExportAllBGFToXMLClick(object sender, EventArgs e)
        {
            SourceTargetForm frm = new SourceTargetForm();
            frm.Mode = SourceTargetMode.Extract;
            frm.Show();
        }

        protected void OnMenuDecompressAllBGFClick(object sender, EventArgs e)
        {
            SourceTargetForm frm = new SourceTargetForm();
            frm.Mode = SourceTargetMode.Decompress;
            frm.Show();
        }

        protected void OnMenuSetShrinkClick(object sender, EventArgs e)
        {
            Program.SettingsForm.Show();
        }

        protected void OnMenuConvertAllToV10Click(object sender, EventArgs e)
        {
            SourceTargetForm frm = new SourceTargetForm();
            frm.Mode = SourceTargetMode.SetVersion10;
            frm.Show();
        }

        protected void OnMenuConvertAllToV10FromValeClick(object sender, EventArgs e)
        {
            SourceTargetForm frm = new SourceTargetForm();
            frm.Mode = SourceTargetMode.SetVersion10FromVale;
            frm.Show();
        }

        protected void OnMenuConvertAllToV9Click(object sender, EventArgs e)
        {
            SourceTargetForm frm = new SourceTargetForm();
            frm.Mode = SourceTargetMode.SetVersion9;
            frm.Show();
        }

        protected void OnMenuConverFromValeColors(object sender, EventArgs e)
        {
            if (Program.CurrentFile == null)
                return;

            foreach (BgfBitmap bmp in Program.CurrentFile.Frames)
                bmp.ConvertFromVale();

            UpdateFrameNums();
            UpdateFrameSetFlow();

            OnFramesSelectionChanged(this, null);
        }

        protected void OnMenuComparePalettesClick(object sender, EventArgs e)
        {
            ComparePalettesForm frm = new ComparePalettesForm();
            frm.Show();
        }

        protected void OnFileDialogOpenFileOk(object sender, CancelEventArgs e)
        {
            // load from file
            Program.Load(fdOpenFile.FileName);
        }

        protected void OnFileDialogAddFrameFileOk(object sender, CancelEventArgs e)
        {
            // load bitmap from file
            Bitmap bitmap = new Bitmap(fdAddFrame.FileName);

            // get pixels
            byte[] pixelData = BgfBitmap.BitmapToPixelData(bitmap);

            // create BgfBitmap
            BgfBitmap bgfBitmap = new BgfBitmap(
                (uint)Program.CurrentFile.Frames.Count + 1,
                Program.CurrentFile.Version,
                (uint)bitmap.Width,
                (uint)bitmap.Height,
                0,
                0,
                new BgfBitmapHotspot[0],
                false,
                0,
                pixelData);

            // cleanp temporary bitmap
            bitmap.Dispose();

            // add to frames
            Program.CurrentFile.Frames.Add(bgfBitmap);
        }

        protected void OnFileDialogSaveFileOk(object sender, CancelEventArgs e)
        {
            // save to file
            Program.Save(fdSaveFile.FileName);
        }

        protected void OnFramesSelectionChanged(object sender, EventArgs e)
        {
            BgfBitmap bgfBitmap = SelectedFrame;

            if (bgfBitmap != null)
            {
                // set hotspotlist
                dgHotspots.DataSource = bgfBitmap.HotSpots;

                // show frame
                Program.ShowFrame(true, bgfBitmap.GetBitmap(), picFrameImage);
            }
        }

        protected void OnFrameSetsSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFrameNums();
            UpdateFrameSetFlow();
        }
        
        public void UpdateFrameNums()
        {
            BgfFrameSet bgfFrameSet = SelectedFrameSet;

            if (bgfFrameSet != null)
            {
                listFrameNums.Items.Clear();

                foreach (int i in bgfFrameSet.FrameIndices)
                    listFrameNums.Items.Add(i + 1);
            }
        }

        public void UpdateFrameSetFlow()
        {
            flowLayoutFrameSet.Controls.Clear();

            BgfFrameSet bgfFrameSet = SelectedFrameSet;
            if (bgfFrameSet != null)
            {
                foreach (int index in bgfFrameSet.FrameIndices)
                {
                    PictureBox picBox = new PictureBox();

                    if (index > -1)
                    {
                        picBox.Width = (int)Program.CurrentFile.Frames[index].Width;
                        picBox.Height = (int)Program.CurrentFile.Frames[index].Height;
                        picBox.Image = Program.CurrentFile.Frames[index].GetBitmap();
                    }
                    else
                    {
                        picBox.Width = 50;
                        picBox.Height = 50;
                        picBox.Image = new Bitmap(50, 50, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    }

                    flowLayoutFrameSet.Controls.Add(picBox);
                }
            }
        }

        private void OnMenuCutTransparency(object sender, EventArgs e)
        {
            Program.CurrentFile.CutParallel();
        }

        private void OnMenuOpenAnimationViewer(object sender, EventArgs e)
        {
            Viewer v = new Viewer();
            v.DataSource = Program.RoomObject;
            v.Show();
        }
    }
}
