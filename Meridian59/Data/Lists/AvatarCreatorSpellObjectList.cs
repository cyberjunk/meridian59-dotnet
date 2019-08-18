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
    /// List for AvatarCreatorSpellObject
    /// </summary>
    [Serializable]
    public class AvatarCreatorSpellObjectList : BaseList<AvatarCreatorSpellObject>
    {
        public AvatarCreatorSpellObjectList(int Capacity = 5) : base(Capacity)
        {
            
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            this.Sort(Compare);

             for (int i = 0; i < Count; i++)
                 OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, i));
        }

        public override void Insert(int Index, AvatarCreatorSpellObject Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                Index = FindSortedIndex(Item);
                base.Insert(Index, Item);
            }
        }

        public void SortBySchoolAndName()
        {
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndex(AvatarCreatorSpellObject Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (Compare(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int Compare(AvatarCreatorSpellObject A, AvatarCreatorSpellObject B)
        {
            return sortDirectionValue * A.SpellListDescription.CompareTo(B.SpellListDescription);
        }
    }
}
