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
using Meridian59.Common;

namespace Meridian59.BgfEditor.Controls
{
    /// <summary>
    /// Extends PictureBox to show image provided by ImageComposer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PictureBoxGame<T> : Control where T: ObjectBase, INotifyPropertyChanged
    {
        protected readonly ImageComposerGDI<T> imageComposer = new ImageComposerGDI<T>();
        protected float zoom = 1.0f;

        protected int mouseX;
        protected int mouseY;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Zoom Level in Integer increments
        /// </summary>
        public int Zoom
        {
            get { return (int)(zoom * 1000.0f); }
            set { zoom = 0.001f * (float)value; }
        }

        public bool UseOffset
        {
            get { return imageComposer.ApplyYOffset; }
            set { imageComposer.ApplyYOffset = value; }
        }

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

            // don't use yoffset by default
            imageComposer.ApplyYOffset = false;

            // hookup event when new image is available
            imageComposer.NewImageAvailable += OnImageComposerNewImageAvailable;

            mouseX = Cursor.Position.X;
            mouseY = Cursor.Position.Y;
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
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

            // get object size in world size          
            float scaledwidth = (float)imageComposer.RenderInfo.WorldSize.X * zoom;
            float scaledheight = (float)imageComposer.RenderInfo.WorldSize.Y * zoom;

            // important:
            // center x (extends x, -x -> right, left)
            // fix y to bottom (extends y -> up)
            float posx = ((float)Width * 0.5f) - (scaledwidth * 0.5f);
            float posy = (float)Height - scaledheight;

            e.Graphics.Clear(BackColor);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // draw mainbmp into target bitmap
            e.Graphics.DrawImage(imageComposer.Image,
                new System.Drawing.Rectangle((int)posx, (int)posy, (int)scaledwidth, (int)scaledheight),
                new System.Drawing.Rectangle(0, 0, imageComposer.Image.Width, imageComposer.Image.Height),
                GraphicsUnit.Pixel);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            zoom += e.Delta * 0.001f;

            // bound
            zoom = System.Math.Max(1.0f, zoom);
            zoom = System.Math.Min(30.0f, zoom);

            Refresh();
            RaisePropertyChanged(new PropertyChangedEventArgs("Zoom"));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (imageComposer.DataSource == null)
                return;

            // use cursor position here instead of arguments
            // because these have been initialized
            int dx = Cursor.Position.X - mouseX;
            int dy = Cursor.Position.Y - mouseY;

            if (dx != 0 && e.Button == MouseButtons.Left)
                imageComposer.DataSource.ViewerAngle -= (ushort)(dx * 16);

            mouseX = Cursor.Position.X;
            mouseY = Cursor.Position.Y;
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
