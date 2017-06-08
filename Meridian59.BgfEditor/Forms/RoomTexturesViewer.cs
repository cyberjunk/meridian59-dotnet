using Meridian59.Common.Constants;
using Meridian59.Files.BGF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meridian59.BgfEditor.Forms
{
    public partial class RoomTexturesViewer : Form
    {
        public class Item
        {
            public string Frame { get; set; }
            public uint Shrink { get; set; }
            public BgfBitmap Bgf { get; set; }
            public uint BgfWidth { get; set; }
            public uint BgfHeight { get; set; }
            public float BgfRatio { get; set; }
            public int PngWidth { get; set; }
            public int PngHeight { get; set; }
            public float PngRatio { get; set; }
            public float Scale { get; set; }
            public string State { get; set; }
            public Image Png { get; set; }
        }

        protected readonly BindingList<Item> items = new BindingList<Item>();

        public RoomTexturesViewer()
        {
            InitializeComponent();
            gridTextures.AutoGenerateColumns = false;
        }

        public void Collect(string PathBgf, string PathPng)
        {
            items.Clear();

            string[] bgfs = Directory.GetFiles(PathBgf, "*.bgf");

            foreach (string sbgf in bgfs)
            {
                BgfFile bgfFile = new BgfFile(sbgf);

                for (int i = 0; i < bgfFile.Frames.Count; i++)
                {
                    BgfBitmap frame = bgfFile.Frames[i];

                    // create new item
                    Item item = new Item();

                    // build file-frame name
                    item.Frame = bgfFile.Filename + "-" + i.ToString();
                    item.Shrink = bgfFile.ShrinkFactor;

                    // set values from bgf frame
                    item.Bgf = frame;
                    item.BgfWidth = frame.Width;
                    item.BgfHeight = frame.Height;
                    item.BgfRatio = (float)frame.Width / (float)frame.Height;

                    // build png path
                    string pngname = PathPng + "/" + bgfFile.Filename + "-" + i.ToString() + FileExtensions.PNG;

                    if (File.Exists(pngname))
                    {
                        item.Png = new Bitmap(pngname);
                        item.PngWidth = item.Png.Width;
                        item.PngHeight = item.Png.Height;
                        item.PngRatio = (float)item.Png.Width / (float)item.Png.Height;

                        // scale between bgf and png
                        item.Scale = (float)item.Png.Width / (float)frame.Width;

                        // compare ratio
                        if (item.PngRatio != item.BgfRatio)
                            item.State = "ERROR";
                        else
                            item.State = "OK";

                    }
                    else
                        item.State = "MISS";

                    // add new item
                    items.Add(item);
                }
            }

            gridTextures.DataSource = items;
        }

        private void OnBtnBgfClick(object sender, EventArgs e)
        {
            DialogResult diagResult = fbBgf.ShowDialog();

            if (diagResult == DialogResult.OK)
            {
                txtBgf.Text = fbBgf.SelectedPath;
            }
        }

        private void OnBtnPngClick(object sender, EventArgs e)
        {
            DialogResult diagResult = fbPng.ShowDialog();

            if (diagResult == DialogResult.OK)
            {
                txtPng.Text = fbPng.SelectedPath;
            }
        }

        private void OnBtnLoadClick(object sender, EventArgs e)
        {
            Collect(txtBgf.Text, txtPng.Text);
        }

        private void OnGridTexturesSelectionChanged(object sender, EventArgs e)
        {
            if (gridTextures.SelectedRows.Count <= 0)
                return;

            Item item = (Item)gridTextures.SelectedRows[0].DataBoundItem;

            if (picBGF.Image != null)
                picBGF.Image.Dispose();

            picBGF.Image = item.Bgf.GetBitmapA8R8G8B8();
            picPNG.Image = item.Png;
            
        }
    }
}
