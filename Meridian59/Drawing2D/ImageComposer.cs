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
using System.ComponentModel;
using System.Collections.Generic;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Data.Models;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Abstract class for composing images of objects
    /// </summary>
    /// <typeparam name="T">Class of Data (ObjectBase or above)</typeparam>
    /// <typeparam name="U">Type of composed image</typeparam>
    public abstract class ImageComposer<T, U> where T:ObjectBase
    {
        /// <summary>
        /// The texture quality
        /// </summary>
        public static Real DefaultQuality = RenderInfo.DEFAULTQUALITY;

        /// <summary>
        /// Whether to cache created images
        /// </summary>
        public static bool IsCacheEnabled = true;

        /// <summary>
        /// Cache used in lookup
        /// </summary>
        public static Dictionary<uint, U> Cache = new Dictionary<uint, U>();

        /// <summary>
        /// Raised when new image was composed or retrieved from cache
        /// </summary>
        public event EventHandler NewImageAvailable;

        protected T dataSource;
        protected U image;
        protected bool drawBackground;
        protected readonly Murmur3 hash = new Murmur3();

        public uint AppearanceHash { get; protected set; }
        public RenderInfo RenderInfo { get; protected set; }
        public Real Quality { get; set; }
        public bool ApplyYOffset { get; set; }
        public byte HotspotIndex { get; set; }
        public bool IsScalePow2 { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public bool CenterVertical { get; set; }
        public bool CenterHorizontal { get; set; }
        public bool UseViewerFrame { get; set; }

        /// <summary>
        /// The Data used to create.
        /// Can trigger Refresh() when set.
        /// </summary>
        public T DataSource 
        {
            get { return dataSource; }
            set
            {
                SetDataSource(value);   
            }
        }
       
        /// <summary>
        /// The composed image/texture/...
        /// </summary>
        public U Image
        {
            get { return image; }
            protected set { image = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageComposer()
        {
            Quality = DefaultQuality;
            HotspotIndex = 0;
            IsScalePow2 = false;
            Width = 0;
            Height = 0;
            CenterVertical = false;
            CenterHorizontal = false;
            UseViewerFrame = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        protected void SetDataSource(ObjectBase Data)
        {
            if (dataSource != Data)
            {
                if (dataSource != null)
                {                    
                    dataSource.AppearanceChanged -= OnAppearanceChanged;
                    dataSource.PropertyChanged -= OnPropertyChanged;
                }

                dataSource = (T)Data;

                if (dataSource != null)
                {
                    dataSource.PropertyChanged += OnPropertyChanged;

                    // attach right handler
                    if (Data is RoomObject && UseViewerFrame)
                    {
                        ((RoomObject)Data).ViewerAppearanceChanged += OnAppearanceChanged;
                    }
                    else
                    {
                        dataSource.AppearanceChanged += OnAppearanceChanged;                      
                    }

                    Refresh();
                }
            }
        }

        /// <summary>
        /// Create or retrieve an image of the DataSource,
        /// saves in Image property.
        /// May raise NewImageAvailable event.
        /// </summary>
        public void Refresh()
        {
            if (dataSource != null)
            {
                // refresh the renderinfo
                // and hashvalue for cache retrieval
                UpdateRenderInfo(dataSource);

                if (RenderInfo.Dimension.X > 0.0f &&
                    RenderInfo.Dimension.Y > 0.0f)
                {
                    // try get image from cache
                    if (!IsCacheEnabled || !Cache.TryGetValue(AppearanceHash, out image))
                    {
                        // prepare drawing
                        PrepareDraw();

                        // glowing background if marked
                        if (drawBackground)
                            DrawBackground();

                        // main rendering
                        DrawSubOverlays(true);
                        DrawMainOverlay();
                        DrawSubOverlays(false);

                        // post effects
                        DrawPostEffects();

                        // finish drawing
                        FinishDraw();

                        // possibly add image to cache
                        if (IsCacheEnabled)
                            Cache.Add(AppearanceHash, image);
                    }

                    // fire event
                    RaiseNewImageAvailable();
                }
            }
        }

        /// <summary>
        /// Updates the renderinfo property from object data
        /// </summary>
        /// <param name="Data"></param>
        protected void UpdateRenderInfo(ObjectBase Data)
        {
            uint apphash;

            if (Data is RoomObject)
            {
                // get new renderinfo for RoomObject
                RenderInfo = ((RoomObject)Data).GetRenderInfo(UseViewerFrame,
                    Quality, ApplyYOffset, HotspotIndex, IsScalePow2, Width, Height, CenterVertical, CenterHorizontal);

                // use hashbase
                apphash = UseViewerFrame ? ((RoomObject)Data).ViewerAppearanceHash : Data.AppearanceHash;

                // no glowing on RoomObjects
                drawBackground = false;
            }
            else
            {
                if (Data is InventoryObject)
                    drawBackground = ((InventoryObject)Data).IsInUse;
                else
                    drawBackground = false;

                // get new renderinfo for ObjectBase
                RenderInfo = Data.GetRenderInfo(
                    Quality, ApplyYOffset, HotspotIndex, IsScalePow2, Width, Height, CenterVertical, CenterHorizontal);

                // use hashbase
                apphash = Data.AppearanceHash;
            }

            // calculate hash
            AppearanceHash = GetAppearanceHash(apphash);
        }

        /// <summary>
        /// Calls DrawSubOverlay for each suboverlay to process
        /// </summary>
        /// <param name="IsUnderlays"></param>
        protected void DrawSubOverlays(bool IsUnderlays)
        {
            // There are 3 passes to draw all overlays (and nother 3 to draw all underlays).
            // Each pass will process a special variant of hotspots
            for (int pass = 0; pass < 3; pass++)
            {
                // the kind of hotspot to process this time
                HotSpotType hstToProcess = HotSpotType.HOTSPOT_NONE;
                switch (pass)
                {
                    case 0:
                        if (IsUnderlays) hstToProcess = HotSpotType.HOTSPOT_UNDERUNDER;
                        else hstToProcess = HotSpotType.HOTSPOT_OVERUNDER;
                        break;

                    case 1:
                        if (IsUnderlays) hstToProcess = HotSpotType.HOTSPOT_UNDER;
                        else hstToProcess = HotSpotType.HOTSPOT_OVER;
                        break;

                    case 2:
                        if (IsUnderlays) hstToProcess = HotSpotType.HOTSPOT_UNDEROVER;
                        else hstToProcess = HotSpotType.HOTSPOT_OVEROVER;
                        break;
                }

                foreach (SubOverlay.RenderInfo subInfo in RenderInfo.SubBgf)               
                    if (subInfo.HotspotType == hstToProcess)                   
                        DrawSubOverlay(subInfo);    
            }
        }
       
        /// <summary>
        /// Override with draw preparations
        /// </summary>
        protected abstract void PrepareDraw();

        /// <summary>
        /// Override with background drawing
        /// </summary>
        protected abstract void DrawBackground();

        /// <summary>
        /// Override with code drawing mainoverlay
        /// </summary>
        protected abstract void DrawMainOverlay();

        /// <summary>
        /// Override with code drawing suboverlays
        /// </summary>
        /// <param name="RenderInfo"></param>
        protected abstract void DrawSubOverlay(SubOverlay.RenderInfo RenderInfo);

        /// <summary>
        /// Override with code drawing post effects
        /// </summary>
        protected abstract void DrawPostEffects();

        /// <summary>
        /// Override with code to finish draw
        /// </summary>
        protected abstract void FinishDraw();

        /// <summary>
        /// Creates a appearance hash
        /// </summary>
        /// <param name="Seed"></param>
        /// <returns></returns>
        protected virtual uint GetAppearanceHash(uint Seed)
        {
            // calculate hash
            hash.Reset(Seed);

            hash.Step((uint)(RenderInfo.Dimension.X));
            hash.Step((uint)(RenderInfo.Dimension.Y));
            hash.Step((uint)(HotspotIndex));

            return hash.Finish();
        }

        /// <summary>
        /// Executed when appearance changed on T
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAppearanceChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Executed when property changed on T
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Triggers NewImageAvailable event
        /// </summary>
        protected void RaiseNewImageAvailable()
        {
            if (NewImageAvailable != null)
                NewImageAvailable(this, new EventArgs());
        }
    }
}
