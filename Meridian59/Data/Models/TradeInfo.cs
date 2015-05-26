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
    /// Cumulated info for trade.
    /// </summary>
    public class TradeInfo : INotifyPropertyChanged, IClearable, ITickable
    {
        public const string PROPNAME_ISVISIBLE          = "IsVisible";      
        public const string PROPNAME_ISPENDING          = "IsPending";
        public const string PROPNAME_ISBACKGROUNDOFFER  = "IsBackgroundOffer";
        public const string PROPNAME_ISITEMSYOUSET      = "IsItemsYouSet";
        public const string PROPNAME_ISITEMSPARTNERSET  = "IsItemsPartnerSet";
        public const string PROPNAME_ITEMSYOU           = "ItemsYou";
        public const string PROPNAME_ITEMSPARTNER       = "ItemsPartner";
        public const string PROPNAME_TRADEPARTNER       = "TradePartner";

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool isVisible;
        protected bool isPending;
        protected bool isBackgroundOffer;
        protected bool isItemsYouSet;
        protected bool isItemsPartnerSet;
        protected ObjectBase tradePartner;
        protected ObjectBaseList<ObjectBase> itemsYou;
        protected ObjectBaseList<ObjectBase> itemsPartner;

        /// <summary>
        /// This is true once the trade is switched visible,
        /// even before it might be server recognized.
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISVISIBLE));
                }
            }
        }

        /// <summary>
        /// This is true once the trade is server-recognized,
        /// so once at least one party has offered their items.
        /// </summary>
        public bool IsPending
        {
            get { return isPending; }
            set
            {
                if (isPending != value)
                {
                    isPending = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISPENDING));
                }
            }
        }

        /// <summary>
        /// Whether this trade was created by BP_OFFER in background
        /// </summary>
        public bool IsBackgroundOffer
        {
            get { return isBackgroundOffer; }
            set
            {
                if (isBackgroundOffer != value)
                {
                    isBackgroundOffer = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ITEMSPARTNER));
                }
            }
        }

        /// <summary>
        /// Whether you have defined your items yet or not
        /// </summary>
        public bool IsItemsYouSet
        {
            get { return isItemsYouSet; }
            set
            {
                if (isItemsYouSet != value)
                {
                    isItemsYouSet = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISITEMSYOUSET));
                }
            }
        }

        /// <summary>
        /// Whether your partner has set his items yet or not
        /// </summary>
        public bool IsItemsPartnerSet
        {
            get { return isItemsPartnerSet; }
            set
            {
                if (isItemsPartnerSet != value)
                {
                    isItemsPartnerSet = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISITEMSPARTNERSET));
                }
            }
        }

        /// <summary>
        /// Your current tradepartner or NULL.
        /// </summary>
        public ObjectBase TradePartner
        {
            get { return tradePartner; }
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
        /// The last set of items you have been offered.
        /// </summary>
        public ObjectBaseList<ObjectBase> ItemsPartner 
        {
            get { return itemsPartner; }
            protected set
            {
                if (itemsPartner != value)
                {
                    itemsPartner = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ITEMSPARTNER));
                }
            }
        }

        /// <summary>
        /// The last set of items you offered.
        /// </summary>
        public ObjectBaseList<ObjectBase> ItemsYou 
        {
            get { return itemsYou; }
            protected set
            {
                if (itemsYou != value)
                {
                    itemsYou = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ITEMSYOU));
                }
            }
        }
       
        /// <summary>
        /// Constructor
        /// </summary>
        public TradeInfo()
        {
            itemsYou = new ObjectBaseList<ObjectBase>(20);
            itemsPartner = new ObjectBaseList<ObjectBase>(20);

            Clear(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                IsVisible = false;
                IsPending = false;
                IsBackgroundOffer = false;
                IsItemsYouSet = false;
                IsItemsPartnerSet = false;
                TradePartner = null;
                itemsYou.Clear();
                itemsPartner.Clear();
            }
            else
            {
                isVisible = false;
                isPending = false;
                isBackgroundOffer = false;
                isItemsYouSet = false;
                isItemsPartnerSet = false;
                tradePartner = null;
                itemsYou.Clear();
                itemsPartner.Clear();
            }
        }

        /// <summary>
        /// Call regularly to update objects
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(double Tick, double Span)
        {
            // update items partner            
            foreach (ObjectBase obj in ItemsPartner)
                obj.Tick(Tick, Span);

            // update items you          
            foreach (ObjectBase obj in ItemsYou)
                obj.Tick(Tick, Span);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
    }
}
