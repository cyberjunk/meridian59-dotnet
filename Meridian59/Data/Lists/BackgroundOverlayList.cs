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
    /// List for BackgroundOverlay
    /// </summary>
    [Serializable]
    public class BackgroundOverlayList : ObjectIDList<BackgroundOverlay>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Capacity"></param>
        public BackgroundOverlayList(int Capacity = 4)
            : base(Capacity)
        {
             
        }

        public int GetIndexByName(string Name)
        {
            for (int i = 0; i < Count; i++)
                if (String.Equals(this[i].Name, Name))
                    return i;
            return -1;
        }

        public BackgroundOverlay GetItemByName(string Name)
        {
            foreach (BackgroundOverlay entry in this)
                if (String.Equals(entry.Name, Name))
                    return entry;

            return null;
        }
        
        public string GetNameByID(uint ID)
        {
            string returnValue = String.Empty;
            foreach (BackgroundOverlay entry in this)
                if (entry.ID == ID)
                    returnValue = entry.Name;

            return returnValue;
        }

        public string GetBackgroundOverlayNameByResourceID(uint ResourceID)
        {
            string returnValue = String.Empty;
            foreach (BackgroundOverlay entry in this)
                if (entry.NameRID == ResourceID)
                    returnValue = entry.Name;

            return returnValue;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case BackgroundOverlay.PROPNAME_NAME:
                    this.Sort(CompareByName);
                    break;
            }        
        }

        public override void Insert(int Index, BackgroundOverlay Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case BackgroundOverlay.PROPNAME_NAME:
                        Index = FindSortedIndexByName(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByName()
        {
            sortProperty = PDC[BackgroundOverlay.PROPNAME_NAME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByName(BackgroundOverlay Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByName(BackgroundOverlay A, BackgroundOverlay B)
        {
            return sortDirectionValue * A.Name.CompareTo(B.Name);
        }
    }
}
