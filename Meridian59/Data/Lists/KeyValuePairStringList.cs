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
    /// List of KeyValuePairString
    /// </summary>
    [Serializable]
    public class KeyValuePairStringList : BaseList<KeyValuePairString>
    {
        public KeyValuePairStringList(int Capacity = 5)
            : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }

        public int GetIndexByKey(string Key)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Key == Key)
                    return i;
            return -1;
        }

        public KeyValuePairString GetItemByKey(string Key)
        {
            foreach (KeyValuePairString entry in this)
                if (entry.Key == Key)
                    return entry;

            return null;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case KeyValuePairString.PROPNAME_KEY:
                    this.Sort(CompareByKey);
                    break;

                case KeyValuePairString.PROPNAME_VALUE:
                    this.Sort(CompareByValue);
                    break;
            }        
        }

        public override void Insert(int Index, KeyValuePairString Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case KeyValuePairString.PROPNAME_KEY:
                        Index = FindSortedIndexByKey(Item);
                        break;

                    case KeyValuePairString.PROPNAME_VALUE:
                        Index = FindSortedIndexByValue(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByKey()
        {
            sortProperty = PDC[KeyValuePairString.PROPNAME_KEY];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByValue()
        {
            sortProperty = PDC[KeyValuePairString.PROPNAME_VALUE];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByKey(KeyValuePairString Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByKey(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByValue(KeyValuePairString Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByValue(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByKey(KeyValuePairString A, KeyValuePairString B)
        {
            return sortDirectionValue * A.Key.CompareTo(B.Key);
        }

        protected int CompareByValue(KeyValuePairString A, KeyValuePairString B)
        {
            return sortDirectionValue * A.Value.CompareTo(B.Value);
        }
    }
}
