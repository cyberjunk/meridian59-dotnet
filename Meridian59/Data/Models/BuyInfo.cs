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
using Meridian59.Common.Interfaces;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A set of information for the buy window (buy from NPC)
    /// </summary>
    [Serializable]
    public class BuyInfo : INotifyPropertyChanged, IClearable
    {        
        #region Constants
        public const string PROPNAME_TRADEPARTNER = "TradePartner";
        public const string PROPNAME_ITEMS = "Items";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region Fields
        protected ObjectBase tradePartner;
        protected readonly ObjectBaseList<TradeOfferObject> items = new ObjectBaseList<TradeOfferObject>();
        protected bool isVisible;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ObjectBase TradePartner
        {
            get
            {
                return tradePartner;
            }
            set
            {
                if (tradePartner != value)
                {
                    tradePartner = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TRADEPARTNER));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObjectBaseList<TradeOfferObject> Items { get { return items; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISVISIBLE));
                }
            }
        }
        #endregion

        #region Constructors
        public BuyInfo()
        {
            Clear(false);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                TradePartner = null;
                Items.Clear();
                IsVisible = false;
            }
            else
            {
                tradePartner = null;
                items.Clear();
                isVisible = false;
            }
        }
        #endregion
    }
}
