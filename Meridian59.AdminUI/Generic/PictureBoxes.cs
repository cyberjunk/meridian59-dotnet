/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.AdminUI".

 "Meridian59.AdminUI" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59.AdminUI" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59.AdminUI".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Meridian59.AdminUI.Generic
{
    /// <summary>
    /// Extends PictureBox to show image provided by ImageComposer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PictureBoxGame<T> : Control where T: ObjectBase
    {
        protected readonly ImageComposerGDI<T> imageComposer = new ImageComposerGDI<T>();

        /// <summary>
        /// The object to be shown
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public T DataSource
        {
            get { return imageComposer.DataSource; }
            set
            {
                imageComposer.DataSource = value;    
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PictureBoxGame()
        {
            // required for proper drawing
            DoubleBuffered = true;

            // hookup event when new image is available
            imageComposer.NewImageAvailable += OnImageComposerNewImageAvailable;
        }

        protected void OnImageComposerNewImageAvailable(object sender, EventArgs e)
        {
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (imageComposer.Image == null)
                return;



            // scaling calculcations
            float MainOverlayX = 0;
            float MainOverlayY = 0;
            float nPercentW = ((float)Width / (float)imageComposer.Image.Width);
            float nPercentH = ((float)Height / (float)imageComposer.Image.Height);

            float MainOverlayScale;
            if (nPercentH < nPercentW)
            {
                MainOverlayScale = nPercentH;
                MainOverlayX = (Width - (imageComposer.Image.Width * MainOverlayScale)) * 0.5f;
            }
            else
            {
                MainOverlayScale = nPercentW;
                MainOverlayY = (Height - (imageComposer.Image.Height * MainOverlayScale)) * 0.5f;
            }

            int destWidth = (int)(imageComposer.Image.Width * MainOverlayScale);
            int destHeight = (int)(imageComposer.Image.Height * MainOverlayScale);

            
            
            e.Graphics.Clear(Color.Transparent);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // draw mainbmp into target bitmap
            e.Graphics.DrawImage(imageComposer.Image,
                new System.Drawing.Rectangle((int)MainOverlayX, (int)MainOverlayY, destWidth, destHeight),
                new System.Drawing.Rectangle(0, 0, imageComposer.Image.Width, imageComposer.Image.Height),
                GraphicsUnit.Pixel);


            //e.Graphics.DrawImage(imageComposer.Image)
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            //imageComposer.Width = (uint)Width;
            //imageComposer.Height = (uint)Height;

            base.OnSizeChanged(e);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PictureBoxObjectBase : PictureBoxGame<ObjectBase>
    {
        public PictureBoxObjectBase()
            : base()
        {
            imageComposer.UseViewerFrame = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PictureBoxRoomObject : PictureBoxGame<RoomObject>
    {
        public PictureBoxRoomObject() 
            : base()
        {
            imageComposer.UseViewerFrame = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PictureBoxInventoryObject : PictureBoxGame<InventoryObject>
    {
        public PictureBoxInventoryObject()
            : base()
        {
            imageComposer.UseViewerFrame = false;
        }
    }
}
