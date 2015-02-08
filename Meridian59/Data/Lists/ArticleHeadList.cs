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
    /// List for newsglobe articles
    /// </summary>
    [Serializable]
    public class ArticleHeadList : BaseList<ArticleHead>
    {
        public ArticleHeadList(int Capacity = 5)
            : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }

        public uint GetMaximumNum()
        {
            uint max = 0;
            foreach (ArticleHead entry in this)
                if (entry.Number > max)
                    max = entry.Number;

            return max;     
        }

        public uint GetMinimumNum()
        {
            uint min = 0;
            foreach (ArticleHead entry in this)
                if (entry.Number < min)
                    min = entry.Number;

            return min;
        }

        public int GetIndexByNum(uint Num)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Number == Num)
                    return i;
            return -1;
        }

        public ArticleHead GetItemByNum(uint Num)
        {
            foreach (ArticleHead entry in this)
                if (entry.Number == Num)
                    return entry;

            return null;
        }

        public IEnumerable<ArticleHead> GetItemsByNums(uint[] Nums)
        {
            List<ArticleHead> list = new List<ArticleHead>(Capacity);

            foreach (ArticleHead entry in this)
            {
                foreach (uint ID in Nums)
                {
                    if (entry.Number == ID)
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
                if (this[i].Number == Num)
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
                case ArticleHead.PROPNAME_NUMBER:
                    this.Sort(CompareByNumber);
                    break;

                case ArticleHead.PROPNAME_POSTER:
                    this.Sort(CompareByPoster);
                    break;

                case ArticleHead.PROPNAME_TIME:
                    this.Sort(CompareByTime);
                    break;
            }        
        }

        public override void Insert(int Index, ArticleHead Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                switch (sortProperty.Name)
                {
                    case ArticleHead.PROPNAME_NUMBER:
                        Index = FindSortedIndexByNumber(Item);
                        break;

                    case ArticleHead.PROPNAME_POSTER:
                        Index = FindSortedIndexByTime(Item);
                        break;

                    case ArticleHead.PROPNAME_TIME:
                        Index = FindSortedIndexByPoster(Item);
                        break;
                }

                base.Insert(Index, Item);
            }
        }

        public void SortByNumber()
        {
            sortProperty = PDC[ArticleHead.PROPNAME_NUMBER];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByPoster()
        {
            sortProperty = PDC[ArticleHead.PROPNAME_POSTER];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        public void SortByTime()
        {
            sortProperty = PDC[ArticleHead.PROPNAME_TIME];
            sortDirection = ListSortDirection.Ascending;

            ApplySort(sortProperty, sortDirection);
        }

        protected int FindSortedIndexByNumber(ArticleHead Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByNumber(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByTime(ArticleHead Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByTime(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int FindSortedIndexByPoster(ArticleHead Candidate)
        {
            for (int i = 0; i < this.Count; i++)
                if (CompareByPoster(this[i], Candidate) > 0)
                    return i;

            return Count;
        }

        protected int CompareByNumber(ArticleHead A, ArticleHead B)
        {
            return sortDirectionValue * A.Number.CompareTo(B.Number);
        }

        protected int CompareByTime(ArticleHead A, ArticleHead B)
        {
            return sortDirectionValue * A.Time.CompareTo(B.Time);
        }

        protected int CompareByPoster(ArticleHead A, ArticleHead B)
        {
            return sortDirectionValue * A.Poster.CompareTo(B.Poster);
        }
    }
}
