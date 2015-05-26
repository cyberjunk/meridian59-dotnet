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
using Meridian59.Common;
using Meridian59.Common.Constants;
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
        public const Real DEFAULTZOOM = 4.0f;
        public const Real MINZOOM = 0.05f;
        public const Real MAXZOOM = 20.0f;
        public const int DEFAULTWIDTH  = 256;
        public const int DEFAULTHEIGHT = 256;

        //                                              AARRGGBB
        public const uint COLOR_MAP_WALL            = 0xFF000000; //PALETTERGB(0, 0, 0)
        public const uint COLOR_MAP_PLAYER          = 0xFF0000FF; //PALETTERGB(0, 0, 255)
        public const uint COLOR_MAP_PLAYER_FRONT    = 0xFF000000; //PALETTERGB(0, 0, 0)      // Pixel at front of player
        public const uint COLOR_MAP_OBJECT          = 0xFFFF0000; //PALETTERGB(255, 0, 0)    // Red
        public const uint COLOR_MAP_FRIEND          = 0xFF00FF78; //PALETTERGB(0, 255, 120)  // Green with blue tint
        public const uint COLOR_MAP_ENEMY           = 0xFFFF0000; //PALETTERGB(255, 0, 0)    // Red
        public const uint COLOR_MAP_GUILDMATE       = 0xFFFFFF00; //PALETTERGB(255, 255, 0)  // Yellow
#if !VANILLA        
        public const uint COLOR_MAP_MINION          = 0xFF00C800; //PALETTERGB(0, 200, 0)    // Green
        public const uint COLOR_MAP_MINION_OTH      = 0xFF460582; //PALETTERGB(70,5,130)     // Purple
        public const uint COLOR_MAP_BUILDGRP        = 0xFF00FF00; //PALETTERGB(0, 255, 0)    // Bright Green
        public const uint COLOR_MAP_NPC             = 0xFF000000; //PALETTERGB(0, 0, 0)      // Black
        public const uint COLOR_MAP_TEMPSAFE        = 0xFF00AAFF; //PALETTERGB(0,170,255)    // Cyan
        public const uint COLOR_MAP_MINIBOSS        = 0xFFA042C2; //PALETTERGB(160, 66, 194) // Purple
        public const uint COLOR_MAP_BOSS            = 0xFF7F0000; //PALETTERGB(127, 0, 0)    // Dark Red
#endif
        #endregion

        protected Real zoom;
        protected BoundingBox2D scope;

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
        /// 
        /// </summary>
        public Real ZoomInv
        {
            get { return 1.0f / zoom; }
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
        public void Tick(double Tick, double Span)
        {
            Real deltax, deltay;
            Real transx1, transy1, transx2, transy2;
            RooFile room;
            RoomObject avatar;

            // basic checks
            if (DataController == null ||
                DataController.AvatarObject == null ||
                DataController.RoomInformation == null ||
                DataController.RoomInformation.ResourceRoom == null)
                    return;

            /***************************************************************************/

            room   = DataController.RoomInformation.ResourceRoom;
            avatar = DataController.AvatarObject;

            // get the deltas based on zoom, zoombase and mapsize
            // the center of the bounding box is the player position
            deltax = 0.5f * zoom * (Real)Width;
            deltay = 0.5f * zoom * (Real)Height;

            // update box boundaries (box = what to draw from map)
            scope.Min.X = avatar.Position3D.X - deltax;
            scope.Min.Y = avatar.Position3D.Z - deltay;
            scope.Max.X = avatar.Position3D.X + deltax;
            scope.Max.Y = avatar.Position3D.Z + deltay;

            // prepare drawing
            PrepareDraw();
            
            /***************************************************************************/
                
            // start drawing walls from roo
            foreach (RooWall rld in room.Walls)
            {
                // Don't show line if:
                // 1) both sides not set
                // 2) left side set to not show up on map, right side unset
                // 3) right side set to not show up on map, left side unset
                // 4) both sides set and set to not show up on map
                if ((rld.LeftSide == null && rld.RightSide == null) ||
                    (rld.LeftSide != null && rld.RightSide == null && rld.LeftSide.Flags.IsMapNever) ||
                    (rld.LeftSide == null && rld.RightSide != null && rld.RightSide.Flags.IsMapNever) ||
                    (rld.LeftSide != null && rld.LeftSide != null && rld.LeftSide.Flags.IsMapNever && rld.RightSide.Flags.IsMapNever))
                    continue;
                   
                // transform wall points
                transx1 = (rld.P1.X * 0.0625f + 64f - scope.Min.X) * ZoomInv;
                transy1 = (rld.P1.Y * 0.0625f + 64f - scope.Min.Y) * ZoomInv;
                transx2 = (rld.P2.X * 0.0625f + 64f - scope.Min.X) * ZoomInv;
                transy2 = (rld.P2.Y * 0.0625f + 64f - scope.Min.Y) * ZoomInv;

                // draw wall
                DrawWall(rld, transx1, transy1, transx2, transy2);
            }

            /***************************************************************************/
                
            // draw roomobjects
            foreach (RoomObject obj in DataController.RoomObjects)
            {
                transx1 = (obj.Position3D.X - scope.Min.X) * ZoomInv;
                transy1 = (obj.Position3D.Z - scope.Min.Y) * ZoomInv;

                if (!obj.IsAvatar)
                {
                    Real width = 50.0f * ZoomInv;
                    Real widthhalf = width / 2.0f;
                    Real rectx = transx1 - widthhalf;
                    Real recty = transy1 - widthhalf;

                    DrawObject(obj, rectx, recty, width, width);
                }
                else
                {
                    V2 pos   = new V2(transx1, transy1);
                    V2 line1 = MathUtil.GetDirectionForRadian(obj.Angle) * 50.0f * ZoomInv;
                    V2 line2 = line1.Clone();
                    V2 line3 = line1.Clone();

                    line2.Rotate(GeometryConstants.HALFPERIOD - 0.5f);
                    line3.Rotate(-GeometryConstants.HALFPERIOD + 0.5f);

                    DrawAvatar(obj, pos + line1, pos + line2, pos + line3);
                }                
            }

            /***************************************************************************/
                
            FinishDraw();
               
            // trigger event
            RaiseImageChanged();         
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
        /// 
        /// </summary>
        /// <param name="RoomObject"></param>
        /// <param name="P1">Triangle point 1</param>
        /// <param name="P2">Triangle point 2</param>
        /// <param name="P3">Triangle point 3</param>
        public abstract void DrawAvatar(RoomObject RoomObject, V2 P1, V2 P2, V2 P3);

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
