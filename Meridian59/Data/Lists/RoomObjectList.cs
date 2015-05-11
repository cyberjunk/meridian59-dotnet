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
    /// List of RoomObject
    /// </summary>
    [Serializable]
    public class RoomObjectList : ObjectBaseList<RoomObject>
    {
        /// <summary>
        /// Last ID Hightlight() was called with
        /// </summary>
        public uint HighlightedID { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Capacity"></param>
        public RoomObjectList(int Capacity = 5) : base(Capacity)
        {            
        }

        /// <summary>
        /// Returns the first object marked highlighted
        /// </summary>
        /// <returns></returns>
        public RoomObject GetHighlightedItem()
        {
            foreach (RoomObject obj in this)
                if (obj.IsHighlighted)
                    return obj;

            return null;
        }

        /// <summary>
        /// Returns the closest RoomObject from another RoomObject
        /// </summary>
        /// <param name="RoomObject"></param>
        /// <returns></returns>
        public RoomObject GetClosestCreature(RoomObject RoomObject)
        {
            // TODO: This should not be here in the list if in use at all

            RoomObject closest = null;
            Real smallestdist = Single.MaxValue;

            foreach (RoomObject obj in this)
            {
                // object itself doesn't count
                if (obj != RoomObject && obj.Flags.IsCreature)
                {
                    Real dist = obj.GetDistanceSquared(RoomObject);
                    if (dist <= smallestdist)
                    {
                        smallestdist = dist;
                        closest = obj;
                    }                    
                }
            }

            return closest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseRenderTypes"></param>
        /// <returns></returns>
        public RoomObjectList GetItemsByBaseRenderTypes(byte[] BaseRenderTypes)
        {
            RoomObjectList list = new RoomObjectList(this.Capacity);
            foreach (RoomObject entry in this)
                foreach (BaseRenderType brt in BaseRenderTypes)
                {
                    switch (brt)
                    {
                        case BaseRenderType.CREATURE:
                            if (entry.Flags.IsCreature)
                                list.Add(entry);
                            break;

                        case BaseRenderType.PLAYER:
                            if (entry.Flags.IsPlayer)
                                list.Add(entry);
                            break;

                        case BaseRenderType.ITEM:
                            if (entry.Flags.IsGettable)
                                list.Add(entry);
                            break;

                        case BaseRenderType.NPC:
                            if (entry.Flags.IsBuyable)
                                list.Add(entry);
                            break;
                    }
                }
            return list;
        }

        /// <summary>
        /// Returns list with contained items flagged as Enemy, GuildMate or Friend.
        /// </summary>
        /// <returns></returns>
        public RoomObjectList GetItemsByPVPFlags()
        {
            RoomObjectList list = new RoomObjectList();

            foreach (RoomObject entry in this)
            {
                if (entry.Flags.IsMinimapEnemy ||
                    entry.Flags.IsMinimapGuildMate ||
                    entry.Flags.IsMinimapFriend)
                {
                    list.Add(entry);
                }    
            }
            
            return list;
        }

        /// <summary>
        /// Checks whether there's an object
        /// with invisible effect flags in the list.
        /// </summary>
        /// <returns></returns>
        public bool HasInvisibleRoomObject()
        {
            foreach (RoomObject obj in this)
                if (obj.Flags.Drawing == ObjectFlags.DrawingType.Invisible)
                    return true;

            return false;
        }

        /// <summary>
        /// Marks the object with given ID as highlighted,
        /// if contained in the list. Any other as not highlighted.
        /// </summary>
        /// <param name="ID"></param>
        public void HighlightObject(uint ID)
        {
            bool found = false;

            foreach (RoomObject obj in this)
            {
                if (obj.ID == ID)
                {
                    found = true;
                    obj.IsHighlighted = true;
                }
                else
                    obj.IsHighlighted = false;
            }

            if (found)
                HighlightedID = ID;

            else
                HighlightedID = UInt32.MaxValue;
        }

        /// <summary>
        /// Marks any contained RoomObject as not highlighted.
        /// </summary>
        public void ResetHighlighted()
        {
            foreach (RoomObject obj in this)
                obj.IsHighlighted = false;

            HighlightedID = UInt32.MaxValue;
        }
    }
}
