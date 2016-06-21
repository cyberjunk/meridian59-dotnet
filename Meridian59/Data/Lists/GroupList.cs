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
using System.Collections.Generic;

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// List for Group
    /// </summary>
    [Serializable]
    public class GroupList : BaseList<Group>
    {
        public GroupList(int Capacity = 5)
            : base(Capacity)
        {
            
        }

        public Group GetItemByName(string Name, bool CaseSensitive = true)
        {
            if (CaseSensitive)
            {
                foreach (Group entry in this)
                    if (String.Equals(entry.Name, Name))
                        return entry;
            }
            else
            {
                foreach (Group entry in this)
                    if (String.Equals(entry.Name.ToLower(), Name.ToLower()))
                        return entry;
            }
            
            return null;
        }

        public List<Group> GetItemsByNamePrefix(string Prefix)
        {
            // list for results
            List<Group> list = new List<Group>();

            // prefix to lowercase
            string lowerPrefix = Prefix.ToLower();

            foreach (Group obj in this)
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
                case Group.PROPNAME_NAME:
                    this.Sort(CompareByName);
                    break;
            }
        }

        public override void Insert(int Index, Group Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case Group.PROPNAME_NAME:
                        Index = FindSortedIndexByName(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByName()
        {
            sortProperty = PDC[Group.PROPNAME_NAME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        /// <summary>
        /// Finds the index for an entry in a sorted list
        /// </summary>
        /// <param name="Candidate"></param>
        /// <returns></returns>
        protected int FindSortedIndexByName(Group Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        /// <summary>
        /// Compares two Group by Name property
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected int CompareByName(Group A, Group B)
        {
            return sortDirectionValue * A.Name.CompareTo(B.Name);
        }
    }
}
