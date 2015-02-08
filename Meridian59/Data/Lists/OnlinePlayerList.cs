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
    /// List of current online players
    /// </summary>
    [Serializable]
    public class OnlinePlayerList : ObjectIDList<OnlinePlayer>
    {
        public OnlinePlayerList(int Capacity = 5) : base(Capacity)
        {
             
        }

        public int GetIndexByResourceID(uint ResourceID)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].NameRID == ResourceID)
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

        public OnlinePlayer GetItemByResourceID(uint ResourceID)
        {
            foreach (OnlinePlayer entry in this)
                if (entry.NameRID == ResourceID)
                    return entry;

            return null;
        }

        public OnlinePlayer GetItemByName(string Name)
        {
            foreach (OnlinePlayer entry in this)
                if (String.Equals(entry.Name, Name, StringComparison.OrdinalIgnoreCase))
                    return entry;

            return null;
        }

        public List<OnlinePlayer> GetItemsByNamePrefix(string Prefix)
        {
            // list for results
            List<OnlinePlayer> list = new List<OnlinePlayer>();
            
            // prefix to lowercase
            string lowerPrefix = Prefix.ToLower();

            foreach (OnlinePlayer obj in this)
            {
                string lowerName = obj.Name.ToLower();

                // insert full match at pos 0
                bool equals = String.Equals(lowerPrefix, lowerName);
                if (equals)
                {
                    list.Insert(0, obj);
                }
                else
                {
                    bool startwith = lowerName.StartsWith(lowerPrefix);

                    if (startwith)
                        list.Add(obj);
                }
            }

            return list;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case OnlinePlayer.PROPNAME_NAME:
                    this.Sort(CompareByName);
                    break;
            }         
        }

        public override void Insert(int Index, OnlinePlayer Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case OnlinePlayer.PROPNAME_NAME:
                        Index = FindSortedIndexByName(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByName()
        {
            sortProperty = PDC[OnlinePlayer.PROPNAME_NAME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByName(OnlinePlayer Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByName(OnlinePlayer A, OnlinePlayer B)
        {
            return sortDirectionValue * A.Name.CompareTo(B.Name);
        }
    }
}
