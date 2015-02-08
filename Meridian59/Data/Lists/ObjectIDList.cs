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

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// List of ObjectID (or higher)
    /// </summary>
    [Serializable]
    public class ObjectIDList<T> : BaseList<T> where T : ObjectID, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Capacity"></param>
        public ObjectIDList(int Capacity = 5) : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }
     
        /// <summary>
        /// Gets the maximum ID in the list
        /// </summary>
        /// <returns></returns>
        public uint GetMaximumID()
        {
            uint max = 0;
            foreach (ObjectID entry in this)
                if (entry.ID > max)
                    max = entry.ID;

            return max;     
        }

        /// <summary>
        /// Gets the minimum ID in the list
        /// </summary>
        /// <returns></returns>
        public uint GetMinimumID()
        {
            uint min = 0;
            foreach (ObjectID entry in this)
                if (entry.ID < min)
                    min = entry.ID;

            return min;
        }

        /// <summary>
        /// Gets the index of an ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int GetIndexByID(uint ID)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].ID == ID)
                    return i;
            return -1;
        }

        /// <summary>
        /// Returns the item with given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetItemByID(uint ID)
        {
            foreach (T entry in this)
                if (entry.ID == ID)
                    return entry;

            return null;
        }

        /// <summary>
        /// Returns all items for given IDs
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public IEnumerable<T> GetItemsByIDs(IEnumerable<uint> IDs)
        {
            // simple collection for returns
            List<T> list = new List<T>(Capacity);

            // loop list
            foreach (T entry in this)
            {
                // compare with each selected ID
                foreach (uint ID in IDs)
                {
                    if (entry.ID == ID)
                    {
                        // add to entries
                        list.Add(entry);

                        // skip rest of selected IDs
                        break;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Removes an item by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RemoveByID(uint ID)
        {
            bool returnValue = false;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].ID == ID)
                {
                    // remove entry
                    RemoveAt(i);
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Overridden
        /// </summary>
        /// <param name="Property"></param>
        /// <param name="Direction"></param>
        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case ObjectID.PROPNAME_ID:
                    this.Sort(CompareByID);
                    break;               
            }         
        }
        
        /// <summary>
        /// Overridden
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Item"></param>
        public override void Insert(int Index, T Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case ObjectID.PROPNAME_ID:
                        Index = FindSortedIndexByID(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        /// <summary>
        /// Sorts the list by ascending ID
        /// </summary>
        public void SortByID()
        {
            sortProperty = PDC[ObjectID.PROPNAME_ID];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        /// <summary>
        /// Finds the index for a new entry in a sorted list
        /// based on ID value.
        /// </summary>
        /// <param name="Candidate"></param>
        /// <returns></returns>
        protected int FindSortedIndexByID(ObjectID Candidate)
        {
            for (int i = 0; i < this.Count; i++)            
                if (CompareByID(this[i], Candidate) > 0)                
                    return i;

            return Count;
        }
        
        /// <summary>
        /// Compares two ObjectID by ID
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected int CompareByID(ObjectID A, ObjectID B)
        {
            return sortDirectionValue * A.ID.CompareTo(B.ID);
        }
    }
}
