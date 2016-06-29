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
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Implements ImageComposer with .NET GDI (Bitmap class)
    /// </summary>
    /// <typeparam name="T">Classtype, ObjectBase or higher</typeparam>
    public class ImageComposerGDI<T> : ImageComposer<T, Bitmap> where T : ObjectBase
    {
        /// <summary>
        /// The interpolation quality used in upscaling
        /// </summary>
        public static InterpolationMode InterpolationMode = InterpolationMode.HighQualityBicubic;

        protected Graphics drawTo;

        protected override void PrepareDraw()
        {
            // create bitmap to hold composed object
            Image = new Bitmap(
                Convert.ToInt32(RenderInfo.Dimension.X), 
                Convert.ToInt32(RenderInfo.Dimension.Y));
            
            // initialize the drawing canvas
            drawTo = Graphics.FromImage(Image);
            drawTo.InterpolationMode = InterpolationMode; 
          
#if DEBUG
            drawTo.Clear(Color.Black);
#endif
        }

        protected override void DrawBackground()
        {           
            // the glowing background
            Bitmap glowBitmap = Properties.Resources.Glow;

            // copy from this rectangle in source
            Rectangle fromRectangle = new Rectangle(0, 0, glowBitmap.Width, glowBitmap.Height);

            // copy to this rectangle in destination
            Rectangle toRectangle = new Rectangle(
                0,
                0,
                Convert.ToInt32(RenderInfo.Dimension.X),
                Convert.ToInt32(RenderInfo.Dimension.Y));

            // draw from mainbitmap into DrawTo object using rectangles
            drawTo.DrawImage(glowBitmap, toRectangle, fromRectangle, GraphicsUnit.Pixel);
        }

        protected override void DrawMainOverlay()
        {
            // copy from this rectangle in source
            Rectangle fromRectangle = new Rectangle(0, 0, (int)RenderInfo.Bgf.Width, (int)RenderInfo.Bgf.Height);

            // copy to this rectangle in destination
            Rectangle toRectangle = new Rectangle(
                Convert.ToInt32(RenderInfo.Origin.X),
                Convert.ToInt32(RenderInfo.Origin.Y),
                Convert.ToInt32(RenderInfo.Size.X),
                Convert.ToInt32(RenderInfo.Size.Y));

            // get subimage to draw
            Bitmap bmp = RenderInfo.Bgf.GetBitmap(RenderInfo.BgfColor);

            // draw from mainbitmap into DrawTo object using rectangles
            drawTo.DrawImage(bmp, toRectangle, fromRectangle, GraphicsUnit.Pixel);

            // dispose subimage
            bmp.Dispose();
        }

        protected override void DrawSubOverlay(SubOverlay.RenderInfo RenderInfo)
        {
            // copy from this rectangle in source           
            Rectangle fromRectangle = new Rectangle(0, 0, (int)RenderInfo.Bgf.Width, (int)RenderInfo.Bgf.Height);

            // copy to this rectangle in destination
            Rectangle toRectangle = new Rectangle(
                Convert.ToInt32(RenderInfo.Origin.X),
                Convert.ToInt32(RenderInfo.Origin.Y),
                Convert.ToInt32(RenderInfo.Size.X),
                Convert.ToInt32(RenderInfo.Size.Y));

            // get subimage to draw
            Bitmap bmp = RenderInfo.Bgf.GetBitmap(RenderInfo.SubOverlay.ColorTranslation);

            // draw
            drawTo.DrawImage(bmp, toRectangle, fromRectangle, GraphicsUnit.Pixel);

            // dispose subimage
            bmp.Dispose();
        }

        protected override void DrawPostEffects()
        {
            if (dataSource.Flags.Drawing == ObjectFlags.DrawingType.DitherInvis)
                DrawPostEffectDitherInvis(Image);

            else if (dataSource.Flags.Drawing == ObjectFlags.DrawingType.Black)
                DrawPostEffectBlack(Image);
        }

        protected override void FinishDraw()
        {
            // cleanup graphics
            drawTo.Dispose();
        }

        protected override uint GetAppearanceHash(uint Seed)
        {
            // calculate hash
            Murmur3 hash = new Murmur3(Seed);

            hash.Step((uint)(RenderInfo.Dimension.X));
            hash.Step((uint)(RenderInfo.Dimension.Y));
            hash.Step((uint)(HotspotIndex));

            // additional to generic values: add flags
            hash.Step(dataSource.Flags.Value);

            return hash.Finish();
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case ObjectBase.PROPNAME_FLAGS:
                    Refresh();
                    break;
            }
        }

        /// <summary>
        /// Returns a A8R8G8B8 (pow2 sized) bitmap with the name of the object drawn in it.
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapForName(ObjectBase Object)
        {
            const int ESTIMATEDCHARWIDTH = 16;
            const int HEIGHT = 32;

            int width = (int)MathUtil.NextPowerOf2((uint)(Object.Name.Length * ESTIMATEDCHARWIDTH));
            
            // get color to use for name based on objectflags
            Color color = Color.FromArgb((int)NameColors.GetColorFor(Object.Flags));

            // font to use
            Font font = new Font(FontFamily.GenericSansSerif, 20,
                FontStyle.Regular, GraphicsUnit.Pixel);
           
            // create bitmap to draw on
            Bitmap bitmap = new Bitmap(width, HEIGHT, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // draw name into bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                string text = Object.Name;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                // draw text
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.Clear(Color.Transparent);
                    g.DrawString(text, font, brush, new RectangleF(0, 0, width, HEIGHT), format);
                }               
            }

            return bitmap;
        }

        /// <summary>
        /// Adds a red border next to non transparent pixels
        /// </summary>
        /// <param name="Bitmap"></param>
        public static unsafe void DrawPostEffectTarget(Bitmap Bitmap)
        {
            // overall sum of pixels
            int pixelcount = Bitmap.Width * Bitmap.Height;

            // parts to lock
            Rectangle lockRect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);

            // lock the bitmap
            BitmapData bmpData = Bitmap.LockBits(
                lockRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // get pointer
            uint* ptr = (uint*)bmpData.Scan0;

            // walk pixels
            for (int i = 1; i < pixelcount - 1; i++)
                if (ptr[i] != 0x00000000 && (ptr[i - 1] == 0x00000000 || ptr[i + 1] == 0x00000000))
                    ptr[i] = 0xFFFF0000;

            // unlock
            Bitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// Makes any second non transparent pixel transparent
        /// in a checkd pattern look.
        /// </summary>
        /// <param name="Bitmap"></param>
        public static unsafe void DrawPostEffectDitherInvis(Bitmap Bitmap)
        {
            // overall sum of pixels
            int pixelcount = Bitmap.Width * Bitmap.Height;

            // parts to lock
            Rectangle lockRect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);

            // lock the bitmap
            BitmapData bmpData = Bitmap.LockBits(
                lockRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // get pointer
            uint* ptr = (uint*)bmpData.Scan0;

            // flag for pixel skipping
            bool skip = false;

            // walk pixels
            for (int i = 0; i < pixelcount; i++)
            {
                // set alpha to zero for any nontransparent second pixel             
                if ((ptr[i] & 0xFF000000) == 0xFF000000 && skip)
                    ptr[i] &= ~0xFF000000;

                // if not first rowpixel, skip next
                // creates the checked pattern look
                if (i % Bitmap.Width > 0)
                    skip = !skip;
            }

            // unlock
            Bitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// Makes any non transparent pixel black
        /// </summary>
        /// <param name="Bitmap"></param>
        public static unsafe void DrawPostEffectBlack(Bitmap Bitmap)
        {
            // overall sum of pixels
            int pixelcount = Bitmap.Width * Bitmap.Height;

            // parts to lock
            Rectangle lockRect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);

            // lock the bitmap
            BitmapData bmpData = Bitmap.LockBits(
                lockRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // get pointer
            uint* ptr = (uint*)bmpData.Scan0;

            // walk pixels
            for (int i = 0; i < pixelcount; i++)
                if (((ptr[i] & 0xFF000000) == 0xFF000000))  // nontransparent pixels only            
                    ptr[i] = 0xFF000000;                    // to black

            // unlock
            Bitmap.UnlockBits(bmpData);
        }
    }
}
#endif