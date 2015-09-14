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
using Meridian59.Files.RSB;
using Meridian59.Common.Enums;

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// List for RsbResourceID
    /// </summary>
    [Serializable]
    public class StringList : BaseList<RsbResourceID>
    {      
        public StringList(int Capacity = 5) : base (Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;           
        }

        public uint GetMaximumID()
        {
            uint max = 0;
            foreach (RsbResourceID entry in this)
                if (entry.ID > max)
                    max = entry.ID;

            return max;     
        }
        
        public uint GetMinimumID()
        {
            uint min = 0;
            foreach (RsbResourceID entry in this)
                if (entry.ID < min)
                    min = entry.ID;

            return min;
        }
        
        public int GetIndexByKey(uint ID, LanguageCode Language)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].ID == ID && this[i].Language == Language)
                    return i;

            return -1;
        }
        
        public int GetIndexByText(string Text)
        {
            for (int i = 0; i < Count; i++)
                if (String.Equals(this[i].Text, Text))
                    return i;
            return -1;
        }

        public RsbResourceID GetItemByKey(uint ID, LanguageCode Language)
        {
            foreach (RsbResourceID entry in this)
                if (entry.ID == ID && entry.Language == Language)
                    return entry;

            return null;
        }

        public RsbResourceID GetItemByText(string Text)
        {
            foreach (RsbResourceID entry in this)
                if (entry.Text == Text)
                    return entry;

            return null;
        }

        public IEnumerable<RsbResourceID> GetItemsByIDs(uint[] IDs)
        {
            List<RsbResourceID> list = new List<RsbResourceID>(Capacity);

            foreach (RsbResourceID entry in this)
            {
                foreach (uint ID in IDs)
                {
                    if (entry.ID == ID)
                    {
                        list.Add(entry);
                        break;
                    }
                }
            }

            return list;
        }

        public IEnumerable<RsbResourceID> GetItemsBySubstring(string Substring)
        {
            List<RsbResourceID> list = new List<RsbResourceID>(Capacity);

            foreach (RsbResourceID entry in this)
                if (Substring == String.Empty || entry.Text.IndexOf(Substring) > -1)
                    list.Add(entry);

            return list;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case RsbResourceID.PROPNAME_ID:
                    this.Sort(CompareByID);
                    break;

                case RsbResourceID.PROPNAME_LANGUAGE:
                    this.Sort(CompareByLanguage);
                    break;

                case RsbResourceID.PROPNAME_TEXT:
                    this.Sort(CompareByText);
                    break;
            }        
        }

        public override void Insert(int Index, RsbResourceID Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case RsbResourceID.PROPNAME_ID:
                        Index = FindSortedIndexByID(Item);
                        break;

                    case RsbResourceID.PROPNAME_LANGUAGE:
                        Index = FindSortedIndexByLanguage(Item);
                        break;

                    case RsbResourceID.PROPNAME_TEXT:
                        Index = FindSortedIndexByText(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByID()
        {
            sortProperty = PDC[RsbResourceID.PROPNAME_ID];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByLanguage()
        {
            sortProperty = PDC[RsbResourceID.PROPNAME_LANGUAGE];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByText()
        {
            sortProperty = PDC[RsbResourceID.PROPNAME_TEXT];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByID(RsbResourceID Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByID(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByLanguage(RsbResourceID Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByLanguage(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByText(RsbResourceID Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByText(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByID(RsbResourceID A, RsbResourceID B)
        {
            return sortDirectionValue * A.ID.CompareTo(B.ID);
        }

        protected int CompareByLanguage(RsbResourceID A, RsbResourceID B)
        {
            return sortDirectionValue * A.Language.CompareTo(B.Language);
        }

        protected int CompareByText(RsbResourceID A, RsbResourceID B)
        {
            return sortDirectionValue * A.Text.CompareTo(B.Text);
        }
    }
}
