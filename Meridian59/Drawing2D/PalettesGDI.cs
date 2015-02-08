/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

#if DRAWING

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Provides GDI+ palettes for Meridian
    /// </summary>
    public static class PalettesGDI
    {
        /// <summary>
        /// The GDI+ colortable variants of core library palettes
        /// </summary>
        public static ColorPalette[] Palettes;

        /// <summary>
        /// Create GDI+ palettes from core library palettes.
        /// Requires initialized ColorTransformation.Provider.
        /// </summary>
        public static void Initialize()
        {
            Palettes = new ColorPalette[ColorTransformation.PALETTECOUNT];

            for (int i = 0; i < ColorTransformation.PALETTECOUNT; i++)           
                Palettes[i] = GetColorPalette(ColorTransformation.Palettes[i]);           
        }

        /// <summary>
        /// Creates a ColorTable object from raw uint[]
        /// </summary>
        /// <param name="Palette"></param>
        /// <returns></returns>
        public static ColorPalette GetColorPalette(uint[] Palette)
        {
            Bitmap dummy = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            ColorPalette pal = dummy.Palette;

            dummy.Dispose();
            dummy = null;

            for (int i = 0; i < ColorTransformation.COLORCOUNT; i++)
                pal.Entries[i] = Color.FromArgb((int)Palette[i]);

            return pal;
        }

        /// <summary>
        /// Creates a bitmap of a palette with size 16x16. Each pixel has the
        /// color of the corresponding palette index.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public unsafe static System.Drawing.Bitmap GetPaletteBitmap(byte Index)
        {
            // create bitmap
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
                16, 16, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            // lock pixels
            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, 16, 16), 
                ImageLockMode.WriteOnly, 
                PixelFormat.Format8bppIndexed);

            byte* ptrScan0 = (byte*)data.Scan0;

            // create pixels
            // this will set each pixel to an index from the colorpal
            for (int i = 0; i < 256; i++)
            {
                // set pixel to color i
                *ptrScan0 = (byte)i;

                // increase ptr to next pixel
                ptrScan0++;
            }

            // unlock
            bmp.UnlockBits(data);

            // set palette
            bmp.Palette = Palettes[Index];

            return bmp;
        }
    }
}
#endif