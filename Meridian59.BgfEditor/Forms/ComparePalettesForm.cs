using Meridian59.Drawing2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Meridian59.BgfEditor
{
    public partial class ComparePalettesForm : Form
    {       
        public ComparePalettesForm()
        {           
            InitializeComponent();

            picPaletteLeft.Image = PalettesGDI.GetPaletteBitmap(PalettesGDI.Palettes[0]);
            picPaletteRight.Image = PalettesGDI.GetPaletteBitmap(PalettesGDI.Palettes[0]);
        }

        private void cbPaletteLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image oldImg;

            if (picPaletteLeft.Image != null)
            {
                oldImg = picPaletteLeft.Image;
                picPaletteLeft.Image = null;
                oldImg.Dispose();
            }

            // pick palette or special vale palette
            ColorPalette pal = (cbPaletteLeft.SelectedIndex > 255) ? PalettesGDI.PaletteVale 
                : PalettesGDI.Palettes[(byte)cbPaletteLeft.SelectedIndex];

            picPaletteLeft.Image = PalettesGDI.GetPaletteBitmap(pal);
        }

        private void cbPaletteRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image oldImg;

            if (picPaletteRight.Image != null)
            {
                oldImg = picPaletteRight.Image;
                picPaletteRight.Image = null;
                oldImg.Dispose();
            }

            // pick palette or special vale palette
            ColorPalette pal = (cbPaletteRight.SelectedIndex > 255) ? PalettesGDI.PaletteVale
                : PalettesGDI.Palettes[(byte)cbPaletteRight.SelectedIndex];

            picPaletteRight.Image = PalettesGDI.GetPaletteBitmap(pal);
        }
    }
}
