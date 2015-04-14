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
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;

namespace Meridian59.Data.Lists
{    
    /// <summary>
    /// A generic, bindable list implementing IBindingList
    /// </summary>
    /// <typeparam name="T">Must implement INotifyPropertyChanged and must have empty constructor.</typeparam>
    [Serializable]
    public class BaseList<T> : List<T>, IBindingList where T : INotifyPropertyChanged, new()
    {
        #region Constants
        protected const sbyte ASCENDING = 1;
        protected const sbyte DESCENDING = -1;
        #endregion

        #region Constructor
        public BaseList(int Capacity = 5)
            : base(Capacity)
        {
            // build propertydescriptor
            PDC = TypeDescriptor.GetProperties(typeof(T));

            // save the SyncContext of the creator thread
            SyncContext = SynchronizationContext.Current;
        }
        #endregion

        #region Fields
        protected bool allowEdit = true;
        protected bool allowNew = true;
        protected bool allowRemove = true;
        protected bool isSorted = false;
        protected ListSortDirection sortDirection;
        protected sbyte sortDirectionValue;
        protected PropertyDescriptor sortProperty;
        #endregion

        #region Properties
        public PropertyDescriptorCollection PDC { get; protected set; }
        public T LastDeletedItem { get; protected set; }
        public T LastAddedItem { get; protected set; }
        public SynchronizationContext SyncContext { get; set; }
        #endregion

        #region New List operations

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="Item"></param>
        public new virtual void Add(T Item)
        {
            if (Item != null)
            {
                // attach listener
                Item.PropertyChanged += OnItemPropertyChanged;

                // check if list is currently in sorted mode
                if (!isSorted)
                {
                    // Add to list
                    base.Add(Item);

                    LastAddedItem = Item;

                    // Raise event
                    OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, Count - 1));
                }
                else
                {
                    Insert(0, Item);
                }
            }
        }

        /// <summary>
        /// Adds a range of items to the list
        /// </summary>
        /// <param name="Collection"></param>
        public new virtual void AddRange(IEnumerable<T> Collection)
        {
            if (!isSorted)
            {
                foreach (T obj in Collection)
                    Add(obj);
            }
            else
            {
                foreach (T obj in Collection)
                    Insert(0, obj);
            }
        }

        /// <summary>
        /// Inserts item at index
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Item"></param>
        public new virtual void Insert(int Index, T Item)
        {
            // attach listener
            Item.PropertyChanged += OnItemPropertyChanged;

            // add to list
            base.Insert(Index, Item);

            // store item as last added one
            LastAddedItem = Item;

            // trigger IBindingList item inserted notification
            OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, Index));
        }

        /// <summary>
        /// Removes specific item
        /// </summary>
        /// <param name="Item"></param>
        public new virtual void Remove(T Item)
        {
            // get index of item
            int index = IndexOf(Item);

            // if contained remove
            if (index > -1)           
                RemoveAt(index);                          
        }

        /// <summary>
        /// Removes item at index
        /// </summary>
        /// <param name="Index"></param>
        public new virtual void RemoveAt(int Index)
        {
            // process existing index only
            if (Index > -1 && Index < Count)
            {
                // get item
                T Item = base[Index];

                // detach listener
                Item.PropertyChanged -= OnItemPropertyChanged;

                // store item as last deleted one
                LastDeletedItem = Item;

                // remove it
                base.RemoveAt(Index);

                // trigger IBindingList item deleted notification
                OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, Index));
            }
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public new virtual void Clear()
        {
            // remove one-by-one, backward iteration
            // also allows detaching event listeners properply
            for (int i = Count - 1; i >= 0; i--)            
                RemoveAt(i);

            OnListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Index accessor
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public new virtual T this[int Index]
        {
            get
            {
                return base[Index];
            }
            set
            {
                base[Index] = value;

                // trigger IBindingList item changed notification
                OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, Index));
            }
        }

        /// <summary>
        /// Swaps two entries.
        /// </summary>
        /// <param name="Index1"></param>
        /// <param name="Index2"></param>
        public virtual void Swap(int Index1, int Index2)
        {
            T temp = this[Index1];

            this[Index1] = this[Index2];
            this[Index2] = temp;

            // trigger IBindingList item changed notifications
            OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, Index1));
            OnListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, Index2));
        }
        #endregion

        #region IBindingList

        /// <summary>
        /// Triggered when list changed somehow
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        public bool AllowEdit
        {
            get { return allowEdit; }
            set { allowEdit = value; }
        }

        public bool AllowNew
        {
            get { return allowNew; }
            set { allowNew = value; }
        }

        public bool AllowRemove
        {
            get { return allowRemove; }
            set { allowRemove = value; }
        }

        public bool SupportsSorting
        {
            get { return true; }
        }

        public bool SupportsChangeNotification
        {
            get { return true; }
        }

        public bool SupportsSearching
        {
            get { return false; }
        }

        public bool IsSorted
        {
            get { return isSorted; }
        }

        public ListSortDirection SortDirection
        {
            get { return sortDirection; }
        }

        public PropertyDescriptor SortProperty
        {
            get { return sortProperty; }
        }

        public T AddNew()
        {
            // create new instance of T using empty constructor
            T obj = new T();

            // attach listener
            obj.PropertyChanged += OnItemPropertyChanged;

            // add to list
            this.Add(obj);

            // return it
            return obj;
        }

        object IBindingList.AddNew()
        {
            return AddNew();
        }

        public virtual void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            // save values
            sortProperty = property;
            sortDirection = direction;

            // set property for faster comparators
            sortDirectionValue = (direction == ListSortDirection.Ascending) ? ASCENDING : DESCENDING;

            // set sorted state to true
            isSorted = true;
        }

        public void RemoveSort()
        {
            isSorted = false;
        }

        public int Find(PropertyDescriptor property, object key)
        {
            throw new NotImplementedException();
        }

        public void AddIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }

        public void RemoveIndex(PropertyDescriptor property)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executed when list changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnListChanged(object sender, ListChangedEventArgs e)
        {
            if (ListChanged != null)
            {
                // If there is a SyncContext object and it's not the one from current (UI) thread
                // send delegate to execute listeners, otherwise just execute listeners
                if (SyncContext != null && SyncContext != SynchronizationContext.Current)
                {
                    SyncContext.Send(delegate { ListChanged(this, e); }, null);
                }
                else
                    ListChanged(sender, e);
            }
        }

        /// <summary>
        /// Executed when an item triggered PropertyChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ListChanged != null)            
                ListChanged(this, new ListChangedEventArgs(
                    ListChangedType.ItemChanged, base.IndexOf((T)sender), PDC[e.PropertyName]));           
        }
        #endregion
    }
}
