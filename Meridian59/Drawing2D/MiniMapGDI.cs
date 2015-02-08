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

using System.Drawing;
using System.Drawing.Drawing2D;
using Meridian59.Data;
using Meridian59.Data.Models;
using Meridian59.Files.ROO;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Implements MiniMap from core library using GDI
    /// </summary>
    public class MiniMapGDI : MiniMap<Bitmap>
    {       
        /// <summary>
        /// Color to use for map background
        /// </summary>
        public Color ColorBackground { get; set; }

        protected Bitmap oldImage;
        protected Graphics g;
        protected Pen wallPen = new Pen(Color.Black, 1f);
        protected SolidBrush purpleBrush = new SolidBrush(Color.Purple);
        protected SolidBrush redBrush = new SolidBrush(Color.Red);
        protected SolidBrush blueBrush = new SolidBrush(Color.Blue);
        protected SolidBrush greenBrush = new SolidBrush(Color.Green);
        protected SolidBrush orangeBrush = new SolidBrush(Color.Orange);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DataController"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Zoom"></param>
        public MiniMapGDI(DataController DataController, int Width = DEFAULTWIDTH, int Height = DEFAULTHEIGHT, Real Zoom = DEFAULTZOOM)
            : base(DataController, Width, Height, Zoom)
        {
            // default background
            ColorBackground = Color.Transparent;

            // create bitmap to draw on
            Image = new Bitmap(Width, Height);
            g = Graphics.FromImage(Image);

            GraphicsPath gpath = new GraphicsPath();
            gpath.AddPie(0,0, Width, Height, 0, 360);
            gpath.CloseFigure();

            g.Clip = new Region(gpath);
        }

        public override void SetDimension(int Width, int Height)
        {
            base.SetDimension(Width, Height);

            // save refeence to old ones
            Bitmap oldBitmap = Image;
            Graphics oldGraphics = g;

            // create new ones
            Image = new Bitmap(Width, Height);
            g = Graphics.FromImage(Image);

            GraphicsPath gpath = new GraphicsPath();
            gpath.AddPie(0, 0, Width, Height, 0, 360);
            gpath.CloseFigure();

            g.Clip = new Region(gpath);

            // let outer world release the old one
            RaiseImageChanged();

            // cleanup old ones
            oldBitmap.Dispose();
            oldGraphics.Dispose();
        }

        public override void PrepareDraw()
        {
            // clear 
            g.Clear(ColorBackground);
        }

        public override void DrawWall(RooWall Wall, Real x1, Real y1, Real x2, Real y2)
        {
            // draw
            g.DrawLine(wallPen, (float)x1, (float)y1, (float)x2, (float)y2);
        }

        public override void DrawObject(RoomObject RoomObject, Real x, Real y, Real width, Real height)
        {
            // any object not ourself
            if (RoomObject.ID != DataController.AvatarID)
            {
                // skip invisible ones
                if (RoomObject.Flags.Drawing == ObjectFlags.DrawingType.Invisible)
                    return;

                if (RoomObject.Flags.IsEnemy)
                {
                    // guildenemy
                    g.FillEllipse(orangeBrush, (float)x, (float)y, (float)width, (float)width);
                }

                else if (RoomObject.Flags.IsGuildMate)
                {
                    // guildmate
                    g.FillEllipse(greenBrush, (float)x, (float)y, (float)width, (float)width);
                }

                else if (RoomObject.Flags.IsPlayer)
                {
                    // player
                    g.FillEllipse(blueBrush, (float)x, (float)y, (float)width, (float)width);
                }

                else if (RoomObject.Flags.IsAttackable)
                {
                    // attackable: red
                    g.FillEllipse(redBrush, (float)x, (float)y, (float)width, (float)width);
                }
            }
            else
            {
                // our player = purple
                g.FillEllipse(purpleBrush, (float)x, (float)y, (float)width, (float)width);
            } 
        }

        public override void FinishDraw()
        {
        }        
    }
}
#endif