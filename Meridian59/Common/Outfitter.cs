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

using System.Collections.Generic;
using System.ComponentModel;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;
using Meridian59.Common.Constants;
using Meridian59.Protocol.GameMessages;

namespace Meridian59.Common
{
    /// <summary>
    /// An outfit helper. Tracks items and switches outfits.
    /// </summary>
    public class Outfitter
    {
        public enum OutfitType { WC, Sha, Qor, Kra, Far, Rij, Jal }
        
        protected InventoryObjectList inventoryItems;
        
        public InventoryObject ShaRobe { get; protected set; }
        public InventoryObject QorRobe { get; protected set; }
        public InventoryObject KraRobe { get; protected set; }
        public InventoryObject FarRobe { get; protected set; }
        public InventoryObject RijRobe { get; protected set; }
        public InventoryObject JalRobe { get; protected set; }
        public InventoryObject Ivy { get; protected set; }
        public InventoryObject MSH { get; protected set; }
        public InventoryObject Helm { get; protected set; }
        public InventoryObject Scale { get; protected set; }
        public InventoryObject Plate { get; protected set; }
        public InventoryObject Pants { get; protected set; }
        public InventoryObject Jerkin { get; protected set; }
        public InventoryObject Gauntlets { get; protected set; }
        public InventoryObject JalaNecklace { get; protected set; }
        public InventoryObject TrueLute { get; protected set; }
        public InventoryObject FrozJewel { get; protected set; }
        public InventoryObject RiijaSword { get; protected set; }

        public OutfitType CurrentOutfit { get; protected set; }

        public InventoryObjectList InventoryItems
        {
            get { return inventoryItems; }
            set
            {
                inventoryItems = value;
                inventoryItems.ListChanged += new ListChangedEventHandler(OnInventoryItemsListChanged);
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public Outfitter() 
        {
            Reset();
        }

        public Outfitter(InventoryObjectList InventoryItems) 
        {           
            this.InventoryItems = InventoryItems;
            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            ShaRobe = null;
            QorRobe = null;
            KraRobe = null;
            FarRobe = null;
            RijRobe = null;
            JalRobe = null;

            MSH = null;
            Helm = null;
            Ivy = null;

            Plate = null;
            Scale = null;

            Pants = null;
            Jerkin = null;
            Gauntlets = null;
            JalaNecklace = null;
            TrueLute = null;
            FrozJewel = null;
            RiijaSword = null;

            if (inventoryItems != null)
                foreach (InventoryObject obj in InventoryItems)
                    ParseItem(obj);
        }

        /// <summary>
        /// Call this to get messages which will switch outfit accordingly
        /// </summary>
        /// <param name="TargetOutfit"></param>
        /// <param name="DoEquippedCheck"></param>
        /// <returns>A list of GameMessages to send to switch the outfit</returns>
        public List<GameMessage> SwitchOutfit(OutfitType TargetOutfit, bool DoEquippedCheck)
        {
            List<GameMessage> PacketList = new List<GameMessage>();

            switch (TargetOutfit)
            {
                case OutfitType.WC:
                    // armor (plate > scale)
                    if ((Plate != null) && (!Plate.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Plate.ID));
                    else if ((Scale != null) && (!Scale.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Scale.ID));

                    // helm (msh > helm)
                    if ((MSH != null) && (!MSH.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(MSH.ID));
                    else if ((Helm != null) && (!Helm.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Helm.ID));

                    // pants + jerkin + gauntlets
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    if ((Pants != null) && (!Pants.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Pants.ID));

                    if ((Gauntlets != null) && (!Gauntlets.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Gauntlets.ID));
                    break;

                case OutfitType.Sha:
                    // robe
                    if ((ShaRobe != null) && (!ShaRobe.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(ShaRobe.ID));

                    if ((Ivy != null) && (!Ivy.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Ivy.ID));

                    // jerkin
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    break;

                case OutfitType.Qor:
                    // robe
                    if ((QorRobe != null) && (!QorRobe.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(QorRobe.ID));

                    // jerkin
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    break;

                case OutfitType.Kra:
                    // robe
                    if ((KraRobe != null) && (!KraRobe.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(KraRobe.ID));

                    // jerkin
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    break;

                case OutfitType.Far:
                    // robe
                    if ((FarRobe != null) && (!FarRobe.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(FarRobe.ID));

                    // froz
                    if ((FrozJewel != null) && (!FrozJewel.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(FrozJewel.ID));

                    // jerkin
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    break;

                case OutfitType.Rij:
                    // robe
                    if ((RijRobe != null) && (!RijRobe.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(RijRobe.ID));

                    // jerkin
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    // sword
                    if ((RiijaSword != null) && (!RiijaSword.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(RiijaSword.ID));

                    break;

                case OutfitType.Jal:
                    // robe
                    if ((JalRobe != null) && (!JalRobe.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(JalRobe.ID));

                    // jerkin
                    if ((Jerkin != null) && (!Jerkin.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(Jerkin.ID));

                    // jalanecklace
                    if ((JalaNecklace != null) && (!JalaNecklace.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(JalaNecklace.ID));

                    // truelute
                    if ((TrueLute != null) && (!TrueLute.IsInUse || !DoEquippedCheck))
                        PacketList.Add(new ReqUseMessage(TrueLute.ID));

                    break;
            }

            CurrentOutfit = TargetOutfit;
            return PacketList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InventoryItem"></param>
        /// <param name="UnsetMode"></param>
        protected void ParseItem(InventoryObject InventoryItem, bool UnsetMode = false)
        {
            uint id = 0;
            if (!UnsetMode)
                id = InventoryItem.ID;

            switch (InventoryItem.Name)
            {
                // robes
                case ResourceStrings.Items.Equipment.DISCIPLEROBES:                  
                    
                    switch(InventoryItem.ColorTranslation)  
                    {
                        case RobesColors.WHITE1:
                        case RobesColors.WHITE2:
                        case RobesColors.WHITE3:
                            ShaRobe = InventoryItem;
                            break;

                        case RobesColors.GREY1:
                        case RobesColors.GREY2:
                        case RobesColors.GREY3:
                            QorRobe = InventoryItem;
                            break;

                        case RobesColors.BLUE1:
                        case RobesColors.BLUE2:
                        case RobesColors.BLUE3:
                            KraRobe = InventoryItem;
                            break;

                        case RobesColors.RED1:
                        case RobesColors.RED2:
                        case RobesColors.RED3:
                            FarRobe = InventoryItem;
                            break;

                        case RobesColors.PURPLE1:
                        case RobesColors.PURPLE2:
                        case RobesColors.PURPLE3:
                            RijRobe = InventoryItem;
                            break;

                        case RobesColors.GREEN1:
                        case RobesColors.GREEN2:
                        case RobesColors.GREEN3:
                            JalRobe = InventoryItem;
                            break;
                    }
                    break;

                // armor
                case ResourceStrings.Items.Armor.PLATE:
                    Plate = InventoryItem;
                    break;

                case ResourceStrings.Items.Armor.SCALE:
                    Scale = InventoryItem;
                    break;

                // head
                case ResourceStrings.Items.Helms.IVYCIRCLET:
                    Ivy = InventoryItem;
                    break;

                case ResourceStrings.Items.Helms.MSH:
                    MSH = InventoryItem;
                    break;

                case ResourceStrings.Items.Helms.HELM:
                    Helm = InventoryItem;
                    break;

                // other
                case ResourceStrings.Items.Equipment.PANTS:
                    Pants = InventoryItem;
                    break;

                case ResourceStrings.Items.Equipment.LIGHTJERKIN:
                    Jerkin = InventoryItem;
                    break;

                case ResourceStrings.Items.Equipment.GAUNTLETS:
                    Gauntlets = InventoryItem;
                    break;

                case ResourceStrings.Items.Equipment.JALANECKLACE:
                    JalaNecklace = InventoryItem;
                    break;

                case ResourceStrings.Items.Equipment.TRUELUTE:
                    TrueLute = InventoryItem;  
                    break;

                case ResourceStrings.Items.Equipment.JEWELOFFROZ:
                    FrozJewel = InventoryItem;
                    break;

                case ResourceStrings.Items.Weapons.RIIJASWORD:
                    RiijaSword = InventoryItem;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnInventoryItemsListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    ParseItem(InventoryItems[e.NewIndex]);
                    break;

                case ListChangedType.ItemDeleted:
                    ParseItem(InventoryItems.LastDeletedItem, true);
                    break;

                case ListChangedType.Reset:
                    Reset();
                    break;
            }
        }
    }
}
