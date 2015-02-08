using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Meridian59.Files.ROO;
using Meridian59.Files;
using Meridian59.Drawing2D;

namespace Meridian59.Tools.RoomTexExtract
{
    public partial class Main : Form
    {
        public Main()
        {
            // init meridian colorpalettes
            ColorTransformation.Provider.Initialize();
            PalettesGDI.Initialize();
            
            InitializeComponent();
        }

        private void btnSelectRoom_Click(object sender, EventArgs e)
        {
            DialogResult result = diagRoom.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtRoomFile.Text = diagRoom.FileName;
                groupBGFFolder.Enabled = true;

                if (txtBGFFolder.Text != String.Empty)
                {
                    groupOutputFolder.Enabled = true;

                    if (groupOutputFolder.Text != String.Empty)
                        btnGO.Enabled = true;
                }
            }
        }

        private void btnSelectBGFFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = diagBGFFolder.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtBGFFolder.Text = diagBGFFolder.SelectedPath;
                groupOutputFolder.Enabled = true;

                // save for next start
                Properties.Settings.Default.BGFFolder = txtBGFFolder.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void btnSelectOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = diagOutputFolder.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtOutputFolder.Text = diagOutputFolder.SelectedPath;
                btnGO.Enabled = true;

                // save for next start
                Properties.Settings.Default.OutputFolder = txtOutputFolder.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            // check
            if (!File.Exists(txtRoomFile.Text) ||
                !Directory.Exists(txtBGFFolder.Text) ||
                !Directory.Exists(txtOutputFolder.Text))
                return;

            // init a resourcemanager with room bgfs only
            ResourceManager resMan = new ResourceManager();
            resMan.InitConfig(new ResourceManagerConfig(
                0, false, false, false, false,
                null, null, null, txtBGFFolder.Text, null, null));

            // load room and resolve resources
            RooFile rooFile = new RooFile(txtRoomFile.Text);
            rooFile.ResolveResources(resMan);

            // make output subfolder
            string subfolder = Path.Combine(txtOutputFolder.Text, rooFile.Filename);
            if (!Directory.Exists(subfolder))
                Directory.CreateDirectory(subfolder);
                
            // extract textures
            Bitmap bmp;
            string filename;
            foreach (RooFile.TextureInfo texInfo in rooFile.Textures)
            {
                filename = Path.Combine(
                    subfolder, 
                    texInfo.Container.Filename + "-" + texInfo.Container.Frames.IndexOf(texInfo.Texture) + ".png");
                
                bmp = texInfo.Texture.GetBitmap();
                bmp.MakeTransparent(System.Drawing.Color.Cyan);
                bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                
                bmp.Dispose();
                bmp = null;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.BGFFolder != null &&
                Properties.Settings.Default.BGFFolder != String.Empty)
            {
                txtBGFFolder.Text = Properties.Settings.Default.BGFFolder;
                groupBGFFolder.Enabled = true;
            }

            if (Properties.Settings.Default.OutputFolder != null &&
                Properties.Settings.Default.OutputFolder != String.Empty)
            {
                txtOutputFolder.Text = Properties.Settings.Default.OutputFolder;
                groupOutputFolder.Enabled = true;               
            }
        }
    }
}
