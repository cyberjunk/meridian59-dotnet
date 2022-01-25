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
            // sort by kind of item first
            int kA = GetKindValue(A);
            int kB = GetKindValue(B);

            // sort by name if both names share the same kind
            if (kA == kB)
            {
                return sortDirectionValue * A.Name.CompareTo(B.Name);
            }

            // A has higher priority than B
            if (kA < kB)
            {
                return sortDirectionValue * -1;
            }
            // A has lower priority than B
            return sortDirectionValue * 1;
        }

        /// <summary>
        /// Gets the kind of an item as a value which represents an order.
        /// </summary>
        /// <param name="O"></param>
        /// <returns></returns>
        public static int GetKindValue(ObjectBase O)
        {
            switch (O.Name)
            {
                // Reagent
                case "blue dragon scale":
                case "blue mushroom":
                case "cyan mushroom":
                case "dark angel feather":
                case "dragonfly eye":
                case "diamond":
                case "elderberry":
                case "emerald":
                case "entroot berry":
                case "fairy wing":
                case "firesand":
                case "gray mushroom":
                case "green mushroom":
                case "herb":
                case "kriipa claw":
                case "mushroom":
                case "orc tooth":
                case "polished seraphym":
                case "purple mushroom":
                case "rainbow fern":
                case "red mushroom":
                case "ruby":
                case "sapphire":
                case "shaman blood":
                case "uncut seraphym":
                case "vial of solagh":
                case "web moss":
                case "yellow mushroom":
                case "yrxl sap":
                    return 0;
                // Armors
                case "chain armor":
                case "leather armor":
                case "light robes":
                case "long skirt":
                case "gauntlets":
                case "nerudite armor":
                case "plate armor":
                case "robes":
                case "robes of the disciple":
                case "scale armor":
                case "short skirt":
                    return 1;
                // Shields
                case "gold round shield":
                case "herald shield":
                case "knight's shield":
                case "small round shield":
                case "orc shield":
                case "soldier's shield":
                case "torch":
                case "wooden shield":
                    return 2;
                // Helms
                case "ant mask":
                case "circlet":
                case "cow mask":
                case "daemon helm":
                case "daemon skeleton mask":
                case "dusk rat mask":
                case "fey mask":
                case "helm":
                case "ivy circlet":
                case "kriipa mask":
                case "magic spirit helmet":
                case "mummy mask":
                case "qormas helm":
                case "rat mask":
                case "shrunken head mask":
                case "skull mask":
                case "stone troll mask":
                case "troll mask":
                case "xeochicatl mask":
                    return 3;
                // Weapons
                case "axe":
                case "battle bow":
                case "black dagger":
                case "gold sword":
                case "hammer":
                case "Jewel of Froz":
                case "long bow":
                case "long sword":
                case "mace":
                case "magic bow":
                case "mystic sword":
                case "nerudite bow":
                case "nerudite sword":
                case "practice bow":
                case "sword of Riija":
                case "scimitar":
                case "short sword":
                case "spiritual hammer":
                case "snowball":
                    return 4;
                // Others
                default:
                    return 5;
            }
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
