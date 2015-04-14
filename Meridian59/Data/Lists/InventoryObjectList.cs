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

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// List for InventoryObject
    /// </summary>
    [Serializable]
    public class InventoryObjectList : ObjectBaseList<InventoryObject>
    {
        public InventoryObjectList(int Capacity = 5)
            : base(Capacity)
        {
             
        }

        /// <summary>
        /// Update the NumOfSameName property of all
        /// items with the same name as the argument.
        /// </summary>
        /// <param name="Item"></param>
        protected void RefreshNumOfSameName(InventoryObject Item)
        {
            if (Item == null)
                return;

            uint num = 0;

            foreach (InventoryObject obj in this)
            {
                if (obj.Name == Item.Name)
                {
                    obj.NumOfSameName = num;
                    num++;
                }
            }
        }

        public override void Add(InventoryObject Item)
        {
            base.Add(Item);
            RefreshNumOfSameName(Item);
        }

        public override void Insert(int Index, InventoryObject Item)
        {
            base.Insert(Index, Item);
            RefreshNumOfSameName(Item);
        }

        public override void RemoveAt(int Index)
        {
            base.RemoveAt(Index);
            RefreshNumOfSameName(LastDeletedItem);
        }

        public override void Swap(int Index1, int Index2)
        {
            base.Swap(Index1, Index2);
            RefreshNumOfSameName((Index1 > -1 && Index1 < this.Count) ? this[Index1] : null);
            RefreshNumOfSameName((Index2 > -1 && Index2 < this.Count) ? this[Index2] : null);
        }
    }
}
