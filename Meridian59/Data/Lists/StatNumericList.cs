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
    /// List for StatNumeric
    /// </summary>
    [Serializable]
    public class StatNumericList : BaseList<StatNumeric>
    {
        public StatNumericList(int Capacity = 5) : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }

        public byte GetMaximumNum()
        {
            byte max = 0;
            foreach (StatNumeric entry in this)
                if (entry.Num > max)
                    max = entry.Num;

            return max;     
        }

        public byte GetMinimumNum()
        {
            byte min = 0;
            foreach (StatNumeric entry in this)
                if (entry.Num < min)
                    min = entry.Num;

            return min;
        }

        public int GetIndexByNum(uint Num)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Num == Num)
                    return i;
            return -1;
        }

        public StatNumeric GetItemByNum(uint Num)
        {
            foreach (StatNumeric entry in this)
                if (entry.Num == Num)
                    return entry;

            return null;
        }

        public StatNumeric GetItemByName(string Name)
        {
            foreach (StatNumeric entry in this)
                if (entry.ResourceName == Name)
                    return entry;

            return null;
        }

        public IEnumerable<StatNumeric> GetItemsByNums(uint[] Nums)
        {
            List<StatNumeric> list = new List<StatNumeric>(Capacity);

            foreach (StatNumeric entry in this)
            {
                foreach (uint ID in Nums)
                {
                    if (entry.Num == ID)
                    {
                        list.Add(entry);
                        break;
                    }
                }
            }

            return list;
        }

        public bool RemoveByNum(uint Num)
        {
            bool returnValue = false;
            for (int i = 0; i < this.Count; i++)
                if (this[i].Num == Num)
                {
                    RemoveAt(i);
                    returnValue = true;
                    break;
                }

            return returnValue;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case StatNumeric.PROPNAME_NUM:
                    this.Sort(CompareByNum);
                    break;

                case StatNumeric.PROPNAME_RESOURCENAME:
                    this.Sort(CompareByResourceName);
                    break;
            }       
        }

        public override void Insert(int Index, StatNumeric Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case StatNumeric.PROPNAME_NUM:
                        Index = FindSortedIndexByNum(Item);
                        break;

                    case StatNumeric.PROPNAME_RESOURCENAME:
                        Index = FindSortedIndexByResourceName(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        protected int FindSortedIndexByNum(StatNumeric Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByNum(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByResourceName(StatNumeric Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByResourceName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByNum(StatNumeric A, StatNumeric B)
        {
            return sortDirectionValue * A.Num.CompareTo(B.Num);
        }

        protected int CompareByResourceName(StatNumeric A, StatNumeric B)
        {
            return sortDirectionValue * A.ResourceName.CompareTo(B.ResourceName);
        }
    }
}
