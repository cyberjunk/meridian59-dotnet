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
    /// List for ResourceID
    /// </summary>
    [Serializable]
    public class StringList : BaseList<ResourceID>
    {      
        public StringList(int Capacity = 5) : base (Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;           
        }

        public uint GetMaximumValue()
        {
            uint max = 0;
            foreach (ResourceID entry in this)
                if (entry.Value > max)
                    max = entry.Value;

            return max;     
        }
        
        public uint GetMinimumValue()
        {
            uint min = 0;
            foreach (ResourceID entry in this)
                if (entry.Value < min)
                    min = entry.Value;

            return min;
        }
        
        public int GetIndexByValue(uint Value)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Value == Value)
                    return i;
            return -1;
        }
        
        public int GetIndexByName(string Name)
        {
            for (int i = 0; i < Count; i++)
                if (String.Equals(this[i].Name, Name))
                    return i;
            return -1;
        }

        public ResourceID GetItemByValue(uint Value)
        {
            foreach (ResourceID entry in this)
                if (entry.Value == Value)
                    return entry;

            return null;
        }

        public ResourceID GetItemByName(string Name)
        {
            foreach (ResourceID entry in this)
                if (String.Equals(entry.Name, Name))
                    return entry;

            return null;
        }

        public IEnumerable<ResourceID> GetItemsByValues(uint[] Values)
        {
            List<ResourceID> list = new List<ResourceID>(Capacity);

            foreach (ResourceID entry in this)
            {
                foreach (uint ID in Values)
                {
                    if (entry.Value == ID)
                    {
                        list.Add(entry);
                        break;
                    }
                }
            }

            return list;
        }
        
        public IEnumerable<ResourceID> GetItemsBySubstring(string Substring)
        {
            List<ResourceID> list = new List<ResourceID>(Capacity);

            foreach (ResourceID entry in this)
                if ((Substring == String.Empty) || (entry.Name.IndexOf(Substring) > -1))
                    list.Add(entry);

            return list;
        }

        public string GetNameByValue(uint Value)
        {
            foreach (ResourceID entry in this)
                if (entry.Value == Value)
                    return entry.Name;

            return String.Empty;
        }
        
        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case ResourceID.PROPNAME_VALUE:
                    this.Sort(CompareByValue);
                    break;

                case ResourceID.PROPNAME_NAME:
                    this.Sort(CompareByName);
                    break;
            }        
        }

        public override void Insert(int Index, ResourceID Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case ResourceID.PROPNAME_VALUE:
                        Index = FindSortedIndexByValue(Item);
                        break;

                    case ResourceID.PROPNAME_NAME:
                        Index = FindSortedIndexByName(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByValue()
        {
            sortProperty = PDC[ResourceID.PROPNAME_VALUE];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByName()
        {
            sortProperty = PDC[ResourceID.PROPNAME_NAME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByValue(ResourceID Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByValue(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByName(ResourceID Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByValue(ResourceID A, ResourceID B)
        {
            return sortDirectionValue * A.Value.CompareTo(B.Value);
        }

        protected int CompareByName(ResourceID A, ResourceID B)
        {
            return sortDirectionValue * A.Name.CompareTo(B.Name);
        }
    }
}
