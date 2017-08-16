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
using System.ComponentModel;
using Meridian59.Common;
using Meridian59.Data.Models;

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Implements ImageComposer with .NET GDI (Bitmap class)
    /// and draws images using own methods.
    /// </summary>
    /// <typeparam name="T">Classtype, ObjectBase or higher</typeparam>
    public class ImageComposerNative<T> : ImageComposer<T, Bitmap> where T : ObjectBase
    {
        BitmapData pixelData;

        protected override void PrepareDraw()
        {
            // create bitmap to hold composed object
            Image = new Bitmap(
                Convert.ToInt32(RenderInfo.Dimension.X), 
                Convert.ToInt32(RenderInfo.Dimension.Y),
                PixelFormat.Format32bppArgb);

            // defines whole image to lock
            Rectangle rect = new Rectangle(0, 0, Image.Width, Image.Height);

            // lock image to access buffer (must use ReadWrite or background is not zeroed in mono)
            pixelData = Image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }

        protected override void DrawBackground()
        {
        }

        protected override unsafe void DrawMainOverlay()
        {
            // todo: negative positions not supported by method below
            if (RenderInfo.Origin.X < 0.0f ||
                RenderInfo.Origin.Y < 0.0f)
                return;

            RenderInfo.Bgf.FillPixelDataAsA8R8G8B8TransparencyBlackScaled(
                (uint*)pixelData.Scan0.ToPointer(),
                (uint)Image.Width, (uint)Image.Height,
                Convert.ToUInt32(RenderInfo.Origin.X),
                Convert.ToUInt32(RenderInfo.Origin.Y),
                Convert.ToUInt32(RenderInfo.Size.X),
                Convert.ToUInt32(RenderInfo.Size.Y),
                RenderInfo.BgfColor);
        }

        protected override unsafe void DrawSubOverlay(SubOverlay.RenderInfo RenderInfo)
        {
            // todo: negative positions not supported by method below
            if (RenderInfo.Origin.X < 0.0f ||
                RenderInfo.Origin.Y < 0.0f)
                return;

            RenderInfo.Bgf.FillPixelDataAsA8R8G8B8TransparencyBlackScaled(
                (uint*)pixelData.Scan0.ToPointer(),
                (uint)Image.Width, (uint)Image.Height,
                Convert.ToUInt32(RenderInfo.Origin.X),
                Convert.ToUInt32(RenderInfo.Origin.Y),
                Convert.ToUInt32(RenderInfo.Size.X),
                Convert.ToUInt32(RenderInfo.Size.Y),
                RenderInfo.SubOverlay.ColorTranslation);
        }

        protected override void DrawPostEffects()
        {
        }

        protected override void FinishDraw()
        {
            Image.UnlockBits(pixelData);
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
    }
}
#endif