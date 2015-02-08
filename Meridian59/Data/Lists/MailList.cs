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
    /// List for Mail objects
    /// </summary>
    [Serializable]
    public class MailList : BaseList<Mail>
    {
        public MailList(int Capacity = 5)
            : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }

        public uint GetMaximumNum()
        {
            uint max = 0;
            foreach (Mail entry in this)
                if (entry.Num > max)
                    max = entry.Num;

            return max;     
        }

        public uint GetMinimumNum()
        {
            uint min = 0;
            foreach (Mail entry in this)
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

        public Mail GetItemByNum(uint Num)
        {
            foreach (Mail entry in this)
                if (entry.Num == Num)
                    return entry;

            return null;
        }

        public IEnumerable<Mail> GetItemsByNums(uint[] Nums)
        {
            List<Mail> list = new List<Mail>(Capacity);

            foreach (Mail entry in this)
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
            {
                if (this[i].Num == Num)
                {
                    RemoveAt(i);
                    returnValue = true;
                    break;
                }
            }

            return returnValue;
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);

            switch (Property.Name)
            {
                case Mail.PROPNAME_NUM:
                    this.Sort(CompareByNum);
                    break;

                case Mail.PROPNAME_SENDER:
                    this.Sort(CompareBySender);
                    break;

                case Mail.PROPNAME_TIMESTAMP:
                    this.Sort(CompareByTimeStamp);
                    break;
            }        
        }

        public override void Insert(int Index, Mail Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case Mail.PROPNAME_NUM:
                        Index = FindSortedIndexByNum(Item);
                        break;

                    case Mail.PROPNAME_SENDER:
                        Index = FindSortedIndexBySender(Item);
                        break;

                    case Mail.PROPNAME_TIMESTAMP:
                        Index = FindSortedIndexByTimeStamp(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByNum()
        {
            sortProperty = PDC[Mail.PROPNAME_NUM];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortBySender()
        {
            sortProperty = PDC[Mail.PROPNAME_SENDER];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByTimeStamp()
        {
            sortProperty = PDC[Mail.PROPNAME_TIMESTAMP];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByNum(Mail Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByNum(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByTimeStamp(Mail Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByTimeStamp(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexBySender(Mail Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareBySender(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByNum(Mail A, Mail B)
        {
            return sortDirectionValue * A.Num.CompareTo(B.Num);
        }

        protected int CompareByTimeStamp(Mail A, Mail B)
        {
            return sortDirectionValue * A.Timestamp.CompareTo(B.Timestamp);
        }

        protected int CompareBySender(Mail A, Mail B)
        {
            return sortDirectionValue * A.Sender.CompareTo(B.Sender);
        }
    }
}
