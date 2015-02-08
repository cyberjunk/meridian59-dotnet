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

using System;
using Meridian59.Common.Interfaces;
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
    /// Implements an abstract M59 minimap.
    /// </summary>
    /// <typeparam name="T">The type of the composed image.</typeparam>
    public abstract class MiniMap<T> : ITickable
    {
        #region Constants
        public const long UPDATEINVERVALMS = 150;
        public const Real ZOOMBASE = 4.0f;
        public const Real DEFAULTZOOM = 1.0f;
        public const Real MINZOOM = 0.05f;
        public const Real MAXZOOM = 20.0f;
        public const int DEFAULTWIDTH = 256;
        public const int DEFAULTHEIGHT = 256;
        #endregion

        /// <summary>
        /// Last tick we updated the map picture
        /// </summary>
        protected long tickLastUpdate;

        /// <summary>
        /// Zoom level
        /// </summary>
        protected Real zoom;

        /// <summary>
        /// Fired when there is an update image to show
        /// </summary>
        public event EventHandler ImageChanged;

        /// <summary>
        /// Reference to the datalayer to get information from
        /// </summary>
        public DataController DataController { get; protected set; }
      
        /// <summary>
        /// The current MiniMap image to display
        /// </summary>
        public T Image { get; protected set; }

        /// <summary>
        /// The width of the mapimage
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// The width of the mapimage
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Zoomfactor, adjust for zoom in/out
        /// </summary>
        public Real Zoom 
        {
            get { return zoom; }
            set
            {
                zoom = System.Math.Min(System.Math.Max(value, MINZOOM), MAXZOOM);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DataController"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Zoom"></param>
        public MiniMap(DataController DataController, int Width = DEFAULTWIDTH, int Height = DEFAULTHEIGHT, Real Zoom = DEFAULTZOOM)
        {
            this.DataController = DataController;
            this.Width = Width;
            this.Height = Height;
            this.Zoom = Zoom;       
        }

        /// <summary>
        /// Call regularly from mainthread, will possibly update mapimage and trigger event
        /// </summary>
        public void Tick(long Tick, long Span)
        {
            // get elapsed ms since last image draw
            long msspan = Tick - tickLastUpdate;

            if (DataController != null && 
                DataController.AvatarObject != null && 
                DataController.RoomInformation != null &&
                DataController.RoomInformation.ResourceRoom != null &&
                msspan >= UPDATEINVERVALMS)
            {
                RooFile RoomFile = DataController.RoomInformation.ResourceRoom;

                // get the deltas based on zoom, zoombase and mapsize
                // the center of the bounding box is the player position
                Real deltax = Zoom * ZOOMBASE * (Real)Width;
                Real deltay = Zoom * ZOOMBASE * (Real)Height;

                // the top left corner of bounding box
                int topx = Convert.ToInt32((Real)DataController.AvatarObject.CoordinateX - deltax);
                int topy = Convert.ToInt32((Real)DataController.AvatarObject.CoordinateY - deltay);

                // bottom right corner of bounding box
                int bottomx = Convert.ToInt32((Real)DataController.AvatarObject.CoordinateX + deltax);
                int bottomy = Convert.ToInt32((Real)DataController.AvatarObject.CoordinateY + deltay);
               
                // get scale between actual pixelsize of image to show and the bounding box
                Real boxscale = (Real)Width / (Real)(bottomx - topx);

                // prepare drawing
                PrepareDraw();
            
                // start drawing lines from roo
                foreach (RooWall rld in RoomFile.Walls)
                {
                    // Don't proceed if:
                    // 1) line has no sides
                    // 2) both sides are flagged to not be shown on map
                    if ((rld.LeftSideReference == 0 && rld.RightSideReference == 0) ||
                        ((rld.LeftSideReference > 0 && RoomFile.SideDefs[rld.LeftSideReference - 1].Flags.IsMapNever) &&
                        (rld.RightSideReference > 0 && RoomFile.SideDefs[rld.RightSideReference - 1].Flags.IsMapNever)))
                        continue;

                    // convert to traffic-coords (/16 is RSHIFT 4, +64 offset):
                    int x1 = (rld.X1 >> 4) + 64;
                    int y1 = (rld.Y1 >> 4) + 64;
                    int x2 = (rld.X2 >> 4) + 64;
                    int y2 = (rld.Y2 >> 4) + 64;

                    // expressions whether point is in rectangle
                    bool isX1inScope = (x1 >= topx && x1 <= bottomx);
                    bool isY1inScope = (y1 >= topy && y1 <= bottomy);
                    bool isX2inScope = (x2 >= topx && x2 <= bottomx);
                    bool isY2inScope = (y2 >= topy && y2 <= bottomy);

                    // if at least one of the line points is in the mapscope, draw the line
                    if ((isX1inScope && isY1inScope) || (isX2inScope && isY2inScope))
                    {
                        // transform points to match world of pixeldrawing
                        Real transx1 = (x1 - topx) * boxscale;
                        Real transy1 = (y1 - topy) * boxscale;
                        Real transx2 = (x2 - topx) * boxscale;
                        Real transy2 = (y2 - topy) * boxscale;

                        // draw wall
                        DrawWall(rld, transx1, transy1, transx2, transy2);
                    }
                }

                // draw roomobjects
                foreach (RoomObject obj in DataController.RoomObjects)
                {
                    Real objx = (obj.CoordinateX - topx) * boxscale;
                    Real objy = (obj.CoordinateY - topy) * boxscale;
                    int width = Convert.ToInt32(50.0f * boxscale);
                    Real widthhalf = (Real)width / 2.0f;
                    int rectx = Convert.ToInt32((Real)objx - widthhalf);
                    int recty = Convert.ToInt32((Real)objy - widthhalf);

                    DrawObject(obj, rectx, recty, width, width);
                }
                

                FinishDraw();
               
                // trigger event
                RaiseImageChanged();

                // save this update tick
                tickLastUpdate = Tick;
            }
        }

        /// <summary>
        /// Adjust the map dimension
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public virtual void SetDimension(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        /// <summary>
        /// Draw preparation
        /// </summary>
        public abstract void PrepareDraw();

        /// <summary>
        /// Finish drawing
        /// </summary>
        public abstract void FinishDraw();

        /// <summary>
        /// Draw a wall
        /// </summary>
        /// <param name="Wall"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public abstract void DrawWall(RooWall Wall, Real x1, Real y1, Real x2, Real y2);
        
        /// <summary>
        /// Draw an object
        /// </summary>
        /// <param name="RoomObject"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void DrawObject(RoomObject RoomObject, Real x, Real y, Real width, Real height);

        /// <summary>
        /// Triggers ImageChanged event
        /// </summary>
        protected void RaiseImageChanged()
        {
            if (ImageChanged != null)
                ImageChanged(this, new EventArgs());
        }
    }
}
