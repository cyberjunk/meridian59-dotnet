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
    /// List for skillobject
    /// </summary>
    [Serializable]
    public class SkillObjectList : ObjectBaseList<SkillObject>
    {
        public SkillObjectList(int Capacity = 5) : base(Capacity)
        {
            
        }

        public override void ApplySort(PropertyDescriptor Property, ListSortDirection Direction)
        {
            base.ApplySort(Property, Direction);       
        }

        public override void Insert(int Index, SkillObject Item)
        {
            if (!isSorted)
                base.Insert(Index, Item);
            else
            {
                /*switch (sortProperty.Name)
                {
                    case SkillObject.PROPNAME_SCHOOLTYPE:
                        break;
                }*/

                base.Insert(Index, Item);
            }
        }

        public List<SkillObject> GetItemsByNamePrefix(string Prefix)
        {
            // list for results
            List<SkillObject> list = new List<SkillObject>();

            // prefix to lowercase
            string lowerPrefix = Prefix.ToLower();

            foreach (SkillObject obj in this)
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
    }
}
