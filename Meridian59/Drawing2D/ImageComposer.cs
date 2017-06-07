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
        /// Raised when new image was composed or retrieved from cache
        /// </summary>
        public event EventHandler NewImageAvailable;

        protected T dataSource;
        protected U image;
        protected Cache.Item item;
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
        /// Destructor
        /// </summary>
        ~ImageComposer()
        {
            if (item != null)
            {
                item.Refs--;
                item = null;
            }
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
            // must have datasource to draw
            if (dataSource == null)
                return;

            // refresh the renderinfo
            // and hashvalue for cache retrieval
            UpdateRenderInfo(dataSource);

            if (RenderInfo.Dimension.X > 0.0f &&
                RenderInfo.Dimension.Y > 0.0f)
            {
                // decrease ref counts on last used cache-hit
                if (item != null)
                    item.Refs--;

                // try get image from cache
                if (!Cache.TryGetValue(AppearanceHash, out item))
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
                    if (Cache.IsEnabled)
                    {
                        item = new Cache.Item();
                        item.Key = AppearanceHash;
                        item.Image = image;
                        item.Refs = 1;
                        item.Tick = DateTime.Now.Ticks;
                        item.Size = 4U * (uint)(RenderInfo.Dimension.X * RenderInfo.Dimension.Y);
                                             
                        Cache.Add(AppearanceHash, item);
                    }
                }
                else
                {
                    image = item.Image;
                    item.Refs++;
                    item.Tick = DateTime.Now.Ticks;
                }

                // fire event
                RaiseNewImageAvailable();
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

        /// <summary>
        /// Cache providing entries
        /// </summary>
        public static class Cache
        {
            /// <summary>
            /// The type of the cache entries
            /// </summary>
            public class Item
            {
                public uint Key;
                public U Image;
                public uint Size;
                public uint Refs;
                public long Tick;
            }

            /// <summary>
            /// EventArgs carrying an array of cache items
            /// </summary>
            public class ItemEventArgs : EventArgs
            {
                public readonly Item[] Items;
                
                public ItemEventArgs(List<Item> Items)
                {
                    this.Items = Items.ToArray();
                }
            }

            /// <summary>
            /// Raised by 'Prune()' for entries without active references.
            /// </summary>
            public static event EventHandler<ItemEventArgs> RemoveSuggested;

            /// <summary>
            /// Cache used in lookup
            /// </summary>
            private static readonly Dictionary<uint, Item> cache = new Dictionary<uint, Item>();

            /// <summary>
            /// List of candidates to removed in Prune()
            /// </summary>
            private static readonly List<Item> candidates = new List<Item>(8);

            /// <summary>
            /// Last tick Prune() tried to remove items
            /// </summary>
            private static long tickPrune = 0;

            /// <summary>
            /// Whether to cache created images
            /// </summary>
            public static bool IsEnabled = true;

            /// <summary>
            /// Size of the cache in bytes (for 32-bit pixeldata)
            /// </summary>
            public static uint CacheSize = 0;

            /// <summary>
            /// Maximum size of the cache in bytes (default: 32 MB)
            /// </summary>
            public static uint CacheSizeMax = 32 * 1024 * 1024;

            /// <summary>
            /// Count of currently stored items
            /// </summary>
            public static int Count { get { return cache.Count; } }

            /// <summary>
            /// Tries to lookup an item from the cache
            /// </summary>
            /// <param name="Key"></param>
            /// <param name="Value"></param>
            /// <returns></returns>
            public static bool TryGetValue(uint Key, out Item Value)
            {
                return cache.TryGetValue(Key, out Value);
            }

            /// <summary>
            /// Adds an item to the cache
            /// </summary>
            /// <param name="Key"></param>
            /// <param name="Value"></param>
            public static void Add(uint Key, Item Value)
            {
                cache.Add(Key, Value);
                CacheSize += Value.Size;

                // possibly remove some unused ones
                Prune();
            }

            /// <summary>
            /// Tries to remove a cache item
            /// </summary>
            /// <param name="Item"></param>
            /// <returns></returns>
            public static bool Remove(Item Item)
            {
                if (Item.Refs > 0)
                    return false;

                bool ok = cache.Remove(Item.Key);

                if (ok)
                    CacheSize -= Item.Size;

                return ok;
            }

            /// <summary>
            /// Clears all entries from the cache
            /// </summary>
            public static void Clear()
            {
                cache.Clear();
                CacheSize = 0;
            }

            /// <summary>
            /// Raises a 'RemoveSuggested' event for all cache entries without active references.
            /// Automatically called if the maximum cache size is exceeded.
            /// </summary>
            public static void Prune()
            {
                if (RemoveSuggested == null)
                    return;

                // true if above limits
                bool exceeded = CacheSize > CacheSizeMax;

                // get current tick and span since last prune
                long tick = DateTime.Now.Ticks;
                long span = tick - tickPrune;

                // don't prune more often than once per second
                // as long as size is not exceeded
                const long ONESECOND = 1 * 1000 * 10000;
                if (!exceeded && span <= ONESECOND)
                    return;

                // save prune tick
                tickPrune = tick;

                // timespans for thirty and sixty seconds
                const long THIRTYSECONDS = 30 * 1000 * 10000;
                const long SIXTYSECONDS  = 60 * 1000 * 10000;

                // pick unused span based on exceeded state
                long spanUnused = (exceeded) ? THIRTYSECONDS : SIXTYSECONDS;

                // collect candidates first so we don't have to remove during iteration
                foreach (KeyValuePair<uint, Item> item in cache)
                {
                    // skip ones in use
                    if (item.Value.Refs > 0)
                        continue;

                    // get time delta
                    span = tick - item.Value.Tick;

                    // skip ones accessed within last N seconds
                    if (span <= spanUnused)
                        continue;

                    // add to ones to be removed
                    candidates.Add(item.Value);

                    // never remove more than several at once (lagspike..)
                    if (candidates.Count >= 8)
                        break;
                }

                // raise event
                RemoveSuggested(typeof(ImageComposer<T, U>.Cache), new ItemEventArgs(candidates));

                // clear temporary candidates list
                candidates.Clear();
            }

            /// <summary>
            /// Prints cache stats to console
            /// </summary>
            public static void PrintCacheStats()
            {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("T:    " + typeof(T).ToString());
                Console.WriteLine("U:    " + typeof(U).ToString());
                Console.WriteLine("SIZE: " + (CacheSize / 1024).ToString() + " KB");
                Console.WriteLine("MAX:  " + (CacheSizeMax / 1024).ToString() + " KB");
                Console.WriteLine("HASH         REFS   KB     TICK");
                Console.WriteLine("---------------------------------------------");

                foreach (KeyValuePair<uint, Item> entry in cache)
                {
                    string hash = entry.Key.ToString().PadRight(13);
                    string refs = entry.Value.Refs.ToString().PadRight(7);
                    string size = (entry.Value.Size / 1024).ToString().PadRight(7);
                    string tick = entry.Value.Tick.ToString();
                    
                    Console.WriteLine(hash + refs + size + tick);
                }
                Console.WriteLine("---------------------------------------------");
            }
        }
    }
}
