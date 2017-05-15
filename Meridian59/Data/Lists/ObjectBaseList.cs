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
    /// List for ObjectBase
    /// </summary>
    [Serializable]
    public class ObjectBaseList<T> : ObjectIDList<T> where T : ObjectBase, new()
    {
        public ObjectBaseList(int Capacity = 5) : base(Capacity)
        {
             
        }

        public T GetItemByName(string Name, bool CaseSensitive = true)
        {
            if (CaseSensitive)
            {
                foreach (T entry in this)
                    if (String.Equals(entry.Name, Name))
                        return entry;
            }
            else
            {
                foreach (T entry in this)
                    if (String.Equals(entry.Name.ToLower(), Name.ToLower()))
                        return entry;
            }

            return null;
        }

        public ObjectBaseList<T> GetItemsByName(string Name, bool CaseSensitive = true)
        {
            ObjectBaseList<T> list = new ObjectBaseList<T>();

            if (CaseSensitive)
            {
                foreach (T entry in this)
                    if (String.Equals(entry.Name, Name))
                        list.Add(entry);
            }
            else
            {
                foreach (T entry in this)
                    if (String.Equals(entry.Name.ToLower(), Name.ToLower()))
                        list.Add(entry);
            }

            return list;
        }

        public int GetIndexByName(string Name)
        {
            for(int i = 0; i < Count; i++)
                if (String.Equals(this[i].Name, Name))
                    return i;

            return -1;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case ObjectBase.PROPNAME_NAME:
                    this.Sort(CompareByName);
                    break;

                case ObjectBase.PROPNAME_OVERLAYFILE:
                    this.Sort(CompareByOverlayFile);
                    break;
            }

            for (int i = 0; i < Count; i++)
                OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, i));
        }

        public override void Insert(int Index, T Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {                
                switch (sortProperty.Name)
                {
                    case ObjectBase.PROPNAME_NAME:
                        Index = FindSortedIndexByName(Item);
                        break;

                    case ObjectBase.PROPNAME_OVERLAYFILE:
                        Index = FindSortedIndexByOverlayFile(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByName()
        {
            sortProperty = PDC[ObjectBase.PROPNAME_NAME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByOverlayFile()
        {
            sortProperty = PDC[ObjectBase.PROPNAME_OVERLAYFILE];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        /// <summary>
        /// Finds the index for an entry in a sorted list
        /// </summary>
        /// <param name="Candidate"></param>
        /// <returns></returns>
        protected int FindSortedIndexByName(ObjectBase Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        /// <summary>
        /// Finds the index for an entry in a sorted list
        /// </summary>
        /// <param name="Candidate"></param>
        /// <returns></returns>
        protected int FindSortedIndexByOverlayFile(ObjectBase Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByOverlayFile(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        /// <summary>
        /// Compares two ObjectBase by Name property
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected int CompareByName(ObjectBase A, ObjectBase B)
        {
            return sortDirectionValue * A.Name.CompareTo(B.Name);
        }

        /// <summary>
        /// Compares two ObjectBase by OverlayFile property
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected int CompareByOverlayFile(ObjectBase A, ObjectBase B)
        {
            return sortDirectionValue * A.OverlayFile.CompareTo(B.OverlayFile);
        }
    }
}
