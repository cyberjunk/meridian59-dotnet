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
using Meridian59.Data.Models;
using Meridian59.Common.Enums;
using Meridian59.Common;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// This list is a filter on the RoomObjectList.
    /// It's initialized on an existing list and filters
    /// items from it accordingly. Will also live-update from source.
    /// </summary>
    public class RoomObjectListFiltered : RoomObjectList
    {
        protected RoomObjectList source;
        
        /// <summary>
        /// These flags are OR to each other - It's enough if one of the flags filter in here matches.
        /// Otherwise just specify only 1 ObjectFlags with all required bits set.
        /// </summary>
        public List<ObjectFlags> FlagsFilter { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ObjectFlags.PlayerType> PlayerTypesFilter { get; protected set; }

        /// <summary>
        /// Maximum value for SquaredDistanceToAvatar property (0=disabled)
        /// </summary>
        public Real SquaredDistanceToAvatarFilter { get; set; }

        /// <summary>
        /// True if SquaredDistanceToAvatarFilter is greater than 0.0
        /// </summary>
        public bool IsSquaredDistancetoAvatarFilterEnabled { get { return SquaredDistanceToAvatarFilter > 0.0f; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source"></param>
        public RoomObjectListFiltered(RoomObjectList Source)
        {
            // keep reference to source list
            source = Source;

            // set capacity to source
            Capacity = source.Capacity;

            // setup default filter (flags 0 = all items)
            FlagsFilter = new List<ObjectFlags>();
            PlayerTypesFilter = new List<ObjectFlags.PlayerType>();

            // this filtered list is always name sorted
            SortByName();
                     
            // add existing items
            foreach (RoomObject obj in source)
                TryAdd(obj);

            // hookup observing
            source.ListChanged += OnSourceListChanged;
        }
     
        /// <summary>
        /// Call this after you've changed something in the Filter settings.
        /// </summary>
        public void Refresh()
        {
            // remove all items
            Clear();

            // readd existing items matching current filter
            foreach (RoomObject obj in source)
                TryAdd(obj);
        }

        protected void TryAdd(RoomObject Item)
        {
            // never add invis flagged ones (DI removes flag)
            if (Item.Flags.Drawing == ObjectFlags.DrawingType.Invisible)
                return;

            // first check if distance filter is active and possibly skip
            if (IsSquaredDistancetoAvatarFilterEnabled &&
                Item.DistanceToAvatarSquared > SquaredDistanceToAvatarFilter)
            {
                return;
            }

            // add it if there is no filter active
            if (FlagsFilter.Count == 0 && PlayerTypesFilter.Count == 0)                
            {               
                // sorted add
                Insert(0, Item);
 
                return;
            }

            // flags filter match
            if (FlagsFilter.Count > 0)
            {
                foreach (ObjectFlags flags in FlagsFilter)
                {
                    if (Item.Flags.IsSubset(flags))
                    {
                        // sorted add
                        Insert(0, Item);

                        return;
                    }
                }
            }

            // player type filter match
            if (PlayerTypesFilter.Count > 0)
            {
                foreach (ObjectFlags.PlayerType ptype in PlayerTypesFilter)
                {
                    if (Item.Flags.Player == ptype)
                    {
                        // sorted add
                        Insert(0, Item);

                        return;
                    }
                }
            }
        }

        protected void Evaluate(RoomObject Item)
        {
            // if we don't have it yet, try add it
            if (!Contains(Item))
                TryAdd(Item);

            // if we have it, see if it still matches a filter
            else
            {
                // never show invis objects
                if (Item.Flags.Drawing == ObjectFlags.DrawingType.Invisible)
                    Remove(Item);

                // otherwise verify against filters
                else
                {
                    // verify still in range limit
                    if (IsSquaredDistancetoAvatarFilterEnabled &&
                        Item.DistanceToAvatarSquared > SquaredDistanceToAvatarFilter)
                    {
                        Remove(Item);
                        return;
                    }

                    // no need to remove it if we don't have a filter
                    if (FlagsFilter.Count == 0 && PlayerTypesFilter.Count == 0)
                        return;

                    // still matches a flag filter
                    if (FlagsFilter.Count > 0)
                        foreach (ObjectFlags flags in FlagsFilter)
                            if (Item.Flags.IsSubset(flags))
                                return;

                    // still matches a player type filter
                    if (PlayerTypesFilter.Count > 0)
                        foreach (ObjectFlags.PlayerType ptype in PlayerTypesFilter)
                            if (Item.Flags.Player == ptype)
                                return;

                    // no current filter matches anymore, remove it
                    Remove(Item);
                }
            }
        }

        protected void OnSourceListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    // possibly add it to our filtered list
                    Evaluate(source.LastAddedItem);
                    break;

                case ListChangedType.ItemDeleted:
                    // try remove it, won't do anything if not in the list
                    Remove(source.LastDeletedItem);
                    break;

                case ListChangedType.Reset:
                    Clear();
                    break;

                case ListChangedType.ItemChanged:
                    // handle changes which could require us to take
                    // the item into the filtered list
                    // if we don't have it yet (or off the list)
                    if (e.NewIndex > -1 && e.NewIndex < source.Count &&
                        (String.Equals(e.PropertyDescriptor.Name, RoomObject.PROPNAME_FLAGS) ||
                        (IsSquaredDistancetoAvatarFilterEnabled && String.Equals(e.PropertyDescriptor.Name, RoomObject.PROPNAME_DISTANCETOAVATARSQUARED))))  
                    {
                        // possibly add it if it matches a filter now
                        // or possibly remove it, if it does not match anymore
                        Evaluate(source[e.NewIndex]);
                    }
                    break;
            }
        }
    }
}
