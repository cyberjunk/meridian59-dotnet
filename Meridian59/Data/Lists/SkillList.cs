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
    /// List for skills
    /// </summary>
    [Serializable]
    public class SkillList : BaseList<StatList>
    {
        public SkillList(int Capacity = 5) : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }
       
        public byte GetMaximumNum()
        {
            byte max = 0;
            foreach (StatList entry in this)
                if (entry.Num > max)
                    max = entry.Num;

            return max;     
        }

        public byte GetMinimumNum()
        {
            byte min = 0;
            foreach (StatList entry in this)
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

        public StatList GetItemByNum(uint Num)
        {
            foreach (StatList entry in this)
                if (entry.Num == Num)
                    return entry;

            return null;
        }

        public IEnumerable<StatList> GetItemsByNums(uint[] Nums)
        {
            List<StatList> list = new List<StatList>(Capacity);

            foreach (StatList entry in this)
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

        public StatList GetItemByID(uint ID)
        {
            foreach (StatList entry in this)
                if (entry.ObjectID == ID)
                    return entry;

            return null;
        }

        public SkillList GetItemsByPrefix(string Prefix, bool CaseSensitive = true)
        {
            SkillList list = new SkillList();

            if (Prefix == null)
                return list;

            if (CaseSensitive)
            { 
                // add matches
                foreach (StatList entry in this)
                    if (entry.ResourceName != null && entry.ResourceName.IndexOf(Prefix) == 0)
                        list.Add(entry);
            }
            else
            {
                // add matches
                foreach (StatList entry in this)
                    if (entry.ResourceName != null && entry.ResourceName.ToLower().IndexOf(Prefix.ToLower()) == 0)
                        list.Add(entry);
            }

            return list;
        }
        public StatList GetItemByName(string Name, bool CaseSensitive = true)
        {
            if (CaseSensitive)
            {
                foreach (StatList entry in this)
                    if (String.Equals(entry.ResourceName, Name))
                        return entry;
            }
            else
            {
                foreach (StatList entry in this)
                    if (String.Equals(entry.ResourceName.ToLower(), Name.ToLower()))
                        return entry;
            }

            return null;
        }

        public SkillList GetItemsByName(string Name, bool CaseSensitive = true)
        {
            SkillList list = new SkillList();

            if (CaseSensitive)
            {
                foreach (StatList entry in this)
                    if (String.Equals(entry.ResourceName, Name))
                        list.Add(entry);
            }
            else
            {
                foreach (StatList entry in this)
                    if (String.Equals(entry.ResourceName.ToLower(), Name.ToLower()))
                        list.Add(entry);
            }

            return list;
        }

        public bool RemoveByNum(uint Num)
        {
            bool returnValue = false;
            for (int i = 0; i < this.Count; i++)
                if (this[i].Num == Num)
                {
                    this.RemoveAt(i);
                    returnValue = true;
                    break;
                }

            return returnValue;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (sortProperty.Name)
            {
                case StatList.PROPNAME_NUM:
                    this.Sort(CompareByNum);
                    break;

                case StatList.PROPNAME_RESOURCENAME:
                    this.Sort(CompareByResourceName);
                    break;
            }        
        }

        public override void Insert(int Index, StatList Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case StatList.PROPNAME_NUM:
                        Index = FindSortedIndexByNum(Item);
                        break;

                    case StatList.PROPNAME_RESOURCENAME:
                        Index = FindSortedIndexByResourceName(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByNum()
        {
            sortProperty = PDC[StatList.PROPNAME_NUM];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByResourceName()
        {
            sortProperty = PDC[StatList.PROPNAME_RESOURCENAME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByNum(StatList Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByNum(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByResourceName(StatList Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByResourceName(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByNum(StatList A, StatList B)
        {
            return sortDirectionValue * A.Num.CompareTo(B.Num);
        }

        protected int CompareByResourceName(StatList A, StatList B)
        {
            return sortDirectionValue * A.ResourceName.CompareTo(B.ResourceName);
        }
    }
}
