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
using System.Collections.Generic;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using Meridian59.Data.Lists;
using Meridian59.Protocol.GameMessages;
using Meridian59.Data;
using Meridian59.Files;

namespace Meridian59.Bot.Shop
{
    /// <summary>
    /// A client which acts as a Shop bot
    /// </summary>
    public class ShopBotClient : BotClient<GameTick, ResourceManager, DataController, ShopBotConfig>
    {
        #region Constants
        protected const string NAME_SHILLING        = "shilling";
        protected const string COMMAND_BUY          = "buy";
        protected const string COMMAND_SHOWOFFERS   = "showoffers";
        protected const string COMMAND_GIVECASH     = "givecash";
        protected const string TELL_WRONGSYNTAXBUY  = "Wrong syntax. To buy whisper me: buy [amount] [itemname]";
        protected const string TELL_THANKS          = "Thank you for business.";
        protected const string TELL_NOMONEY         = "Sorry, I don't have enough shillings left.";
        protected const string LOG_ADVBROADCASTED   = "Advertisement broadcasted";
        protected const string LOG_ADVSAID          = "Advertisement said";
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopBotClient()
            : base()
        {                       

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();

            // set intervals
            GameTick.INTERVALBROADCAST = Config.IntervalBroadcast;
            GameTick.INTERVALSAY = Config.IntervalSay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleGameModeMessage(GameModeMessage Message)
        {
            base.HandleGameModeMessage(Message);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandlePlayerMessage(PlayerMessage Message)
        {
            base.HandlePlayerMessage(Message);

            // make sure we're resting to have mana for broadcasts
            SendUserCommandRest();  
        }
        
        /// <summary>
        /// Handles someone offers you first
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleOfferMessage(OfferMessage Message)
        {
            base.HandleOfferMessage(Message);
            
            // no tradepartner set (bug!?) or
            // someone tried to offer nothing (pointless)
            if (Data.Trade.TradePartner == null || Data.Trade.ItemsPartner.Count == 0)
            {
                SendCancelOffer();
                return;
            }

            ///
            /// ADMIN
            ///
            
            // accept any items from configured admins in config, offer nothing in return
            if (Config.IsAdmin(Data.Trade.TradePartner.Name))
            {
                // nothing
                SendReqCounterOffer(new ObjectID[0]);

                // tell admin
                SendSayGroupMessage(
                    Data.Trade.TradePartner.ID,
                    Config.ChatPrefixString + "I will take that, master " + Data.Trade.TradePartner.Name);

                // exit
                return;
            }

            ///
            /// NORMAL
            ///

            uint offersum = 0;
            Dictionary<string, uint> dict = new Dictionary<string, uint>();

            // see what they offered
            for(int i = 0; i < Data.Trade.ItemsPartner.Count; i++)
            {
                ObjectBase obj = Data.Trade.ItemsPartner[i];
                ShopItem entry = Config.BuyListGetItemByName(obj.Name);

                // if there's anything we don't buy, reject offer
                if (entry == null)
                {
                    // tell customer
                    SendSayGroupMessage(
                        new uint[] { Data.Trade.TradePartner.ID },
                        Config.ChatPrefixString + "Sorry, I'm not buying " + obj.Name);

                    SendCancelOffer();

                    return;
                }

                // process stackable offered item
                if (obj.IsStackable)
                {
                    // get the ones we already have
                    ObjectBaseList<InventoryObject> inventoryObjects =
                        Data.InventoryObjects.GetItemsByName(entry.Name, false);

                    int toBuy;

                    // have none yet? accept up to amount
                    if (inventoryObjects.Count == 0)
                        toBuy = (int)entry.Amount;

                    // stackable
                    else if (inventoryObjects[0].IsStackable)                    
                        toBuy = (int)entry.Amount - (int)inventoryObjects[0].Count;

                    // non stackable
                    else                    
                        toBuy = (int)entry.Amount - (int)inventoryObjects.Count;

                    if (toBuy <= 0)
                    {
                        // tell customer
                        SendSayGroupMessage(
                            new uint[] { Data.Trade.TradePartner.ID },
                            Config.ChatPrefixString + "Sorry, I'm not buying " + entry.Name);

                        SendCancelOffer();

                        return;
                    }
                    else if (obj.Count > toBuy)
                    {
                        // tell customer
                        SendSayGroupMessage(
                            new uint[] { Data.Trade.TradePartner.ID },
                            Config.ChatPrefixString + "Sorry, I'm only buying " + toBuy + " " + entry.Name);

                        SendCancelOffer();

                        return;
                    }

                    // add to sum
                    offersum += obj.Count * entry.UnitPrice;
                }

                // process nonstackable offered item
                else
                {
                    // in this iteration we just count them up

                    uint count;
                    if (dict.TryGetValue(obj.Name, out count))
                    {
                        dict[obj.Name] = dict[obj.Name] + 1;
                    }
                    else
                    {
                        dict.Add(obj.Name, 1);
                    }
                }
            }

            // process nonstackable offered items now
            foreach (KeyValuePair<string, uint> keyvalue in dict)
            {
                ShopItem entry = Config.BuyListGetItemByName(keyvalue.Key);

                if (entry != null)
                {
                    // get the ones we already have
                    ObjectBaseList<InventoryObject> inventoryObjects =
                        Data.InventoryObjects.GetItemsByName(entry.Name, false);

                    int toBuy;

                    // have none yet? accept up to amount
                    if (inventoryObjects.Count == 0)
                        toBuy = (int)entry.Amount;

                    // non stackable
                    else
                        toBuy = (int)entry.Amount - (int)inventoryObjects.Count;

                    if (toBuy <= 0)
                    {
                        // tell customer
                        SendSayGroupMessage(
                            new uint[] { Data.Trade.TradePartner.ID },
                            Config.ChatPrefixString + "Sorry, I'm not buying " + entry.Name);

                        SendCancelOffer();

                        return;
                    }
                    else if (keyvalue.Value > toBuy)
                    {
                        // tell customer
                        SendSayGroupMessage(
                            new uint[] { Data.Trade.TradePartner.ID },
                            Config.ChatPrefixString + "Sorry, I'm only buying " + toBuy + " " + entry.Name);

                        SendCancelOffer();

                        return;
                    }

                    // add to sum
                    offersum += keyvalue.Value * entry.UnitPrice;
                }
            }

            // get our shillings from inventory
            InventoryObject shillings = Data.InventoryObjects.GetItemByName(NAME_SHILLING, false);

            // do we have enough money?
            bool haveMoney = (shillings != null && shillings.Count >= offersum);

            // offer shillings
            if (offersum > 0 && haveMoney)            
                SendReqCounterOffer(new ObjectID[] { new ObjectID(shillings.ID, offersum) });          

            // offer nothing
            else if (offersum == 0)
                SendReqCounterOffer(new ObjectID[0]);

            // cancel trade
            else
            {
                // tell customer
                SendSayGroupMessage(
                    new uint[] { Data.Trade.TradePartner.ID },
                    Config.ChatPrefixString + TELL_NOMONEY);

                SendCancelOffer();

                // log warning
                Log("WARN", "Not enough shillings to perform trade with " + Data.Trade.TradePartner.Name);
            }
        }

        /// <summary>
        /// Handles a counteroffer (you offered first)
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleCounterOfferMessage(CounterOfferMessage Message)
        {
            base.HandleCounterOfferMessage(Message);

            if (Data.Trade.TradePartner == null)
                return;

            // accept anything from configured admins in config
            if (Config.IsAdmin(Data.Trade.TradePartner.Name))
            {
                // tell admin
                SendSayGroupMessage(
                    new uint[] { Data.Trade.TradePartner.ID },
                    Config.ChatPrefixString + "Thank you, master " + Data.Trade.TradePartner.Name);

                // accept
                SendAcceptOffer();

                // exit
                return;
            }

            uint offeredsum = 0;
            uint expectedsum = 0;

            // get how much he offers us
            foreach (ObjectBase obj in Data.Trade.ItemsPartner)           
                if (obj.Name == NAME_SHILLING)
                    offeredsum += obj.Count;
            
            // get how much we want for that
            foreach (ObjectBase obj in Data.Trade.ItemsYou)
            {
                ShopItem entry = Config.OfferListGetItemByName(obj.Name);

                if (entry != null)
                {
                    uint count = (obj.IsStackable) ? obj.Count : 1;
                    expectedsum += (entry.UnitPrice * count);
                }
            }

            // if he offered enough, accept
            if (offeredsum >= expectedsum)
            {               
                // tell customer
                SendSayGroupMessage(
                    Data.Trade.TradePartner.ID, 
                    Config.ChatPrefixString + TELL_THANKS);

                // preserve partnername (gets cleaned next call)
                string partnername = String.Copy(Data.Trade.TradePartner.Name);

                // do trade
                SendAcceptOffer();

                Log("GOOD", "I sold something to " + partnername + " for " + offeredsum + " shillings.");

            }
            else
            {
                // tell customer
                SendSayGroupMessage(
                    Data.Trade.TradePartner.ID, 
                    Config.ChatPrefixString + "Sorry, I was expecting " + expectedsum + " shillings from you.");

                SendCancelOffer();
            }
        }

        /// <summary>
        /// Handles a new player enters room
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleCreateMessage(CreateMessage Message)
        {
            base.HandleCreateMessage(Message);

            RoomObject roomObject = Message.NewRoomObject;

            // tell offers to new roomobject
            if (roomObject.Flags.Drawing != ObjectFlags.DrawingType.Invisible &&
                roomObject.Flags.IsPlayer &&
                !roomObject.IsAvatar &&
                Config.TellOnEnter)
            {
                TellAdvertisement(roomObject);
            }

            // check if is bot admin who entered our room
            if (Config.IsAdmin(roomObject.Name))
            {
                // get our shillings from inventory
                InventoryObject shillings = Data.InventoryObjects.GetItemByName(NAME_SHILLING, false);

                uint count = 0;

                // tell master shillingscount
                if (shillings != null)
                    count = shillings.Count;

                SendSayGroupMessage(
                    roomObject.ID,
                    Config.ChatPrefixString + "Master " + roomObject.Name + ", I have " + count + " shillings");
            }
        }

        /// <summary>
        /// Main handler for commands
        /// </summary>
        /// <param name="PartnerID"></param>
        /// <param name="Words">First element is command name</param>
        protected override void ProcessCommand(uint PartnerID, string[] Words)
        {
            switch (Words[0])
            {
                case COMMAND_BUY:
                    ProcessCommandBuy(PartnerID, Words);
                    break;

                case COMMAND_SHOWOFFERS:
                    ProcessCommandShowOffers(PartnerID, Words);
                    break;

                case COMMAND_GIVECASH:
                    ProcessCommandGiveCash(PartnerID, Words);
                    break;

                default:
                    // show help
                    SendSayGroupMessage(PartnerID, GetHelp());
                    break;
            }           
        }

        /// <summary>
        /// Processed a received buy command
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="Words"></param>
        protected void ProcessCommandBuy(uint CustomerID, string[] Words)
        {
            uint amount = 0;
            string name;          

            // buy needs at least 3 parameters (e.g. buy 10 purple mushroom)
            // get amount user wants
            if (Words.Length < 3 || !UInt32.TryParse(Words[1], out amount) || amount == 0)
            {        
                // tell customer
                SendSayGroupMessage(CustomerID, Config.ChatPrefixString + TELL_WRONGSYNTAXBUY);     
       
                // return
                return;
            }

            // concatenate other words
            name = String.Empty;
            for (int i = 2; i < Words.Length; i++)
                name += Words[i] + " ";

            name = name.TrimEnd();

            // try to get this from offered items
            ShopItem offerItem = Config.OfferListGetItemByName(name);

            // not selling this? return
            if (offerItem == null)
            {
                // tell customer
                SendSayGroupMessage(CustomerID, Config.ChatPrefixString + "Sorry, I'm not selling " + name + ".");

                return;
            }

            // get the items from our inventory that match the name
            ObjectBaseList<InventoryObject> inventoryObjects = 
                Data.InventoryObjects.GetItemsByName(name, false);

            // don't have that item anymore?
            if (inventoryObjects.Count == 0)
            {
                // tell customer
                SendSayGroupMessage(CustomerID, Config.ChatPrefixString + "Sorry, I don't have any more " + name + ".");

                return;
            }

            int tempamount;
            bool isstackable = false;
            
            // non stackable item
            if (!inventoryObjects[0].IsStackable)
            {
                isstackable = false;
                tempamount = (int)inventoryObjects.Count - (int)offerItem.Amount;

                if (tempamount <= 0)
                {
                    // tell customer
                    SendSayGroupMessage(CustomerID, Config.ChatPrefixString + "Sorry, I don't have any more " + name + ".");

                    return;
                }
                else
                    amount = Math.Min(amount, (uint)tempamount);
            }

            // stackable
            else
            {
                isstackable = true;
                tempamount = (int)inventoryObjects[0].Count - (int)offerItem.Amount;

                if (tempamount <= 0)
                {
                    // tell customer
                    SendSayGroupMessage(CustomerID, Config.ChatPrefixString + "Sorry, I don't have any more " + name + ".");

                    return;
                }
                else
                    amount = Math.Min(amount, (uint)tempamount);
            }

            // get the object we trade with
            RoomObject tradePartner = Data.RoomObjects.GetItemByID(CustomerID);

            if (tradePartner == null)
                return;

            // save this as our tradepartner (required for we offer first)
            Data.Trade.TradePartner = tradePartner;
            
            // how much we expect for our offer
            uint expectedsum = offerItem.UnitPrice * amount;

            // make offer depending on stackflag
            if (isstackable)
            {
                ObjectID[] offer = new ObjectID[] { new ObjectID(inventoryObjects[0].ID, amount) };

                // finally offer him stackable item
                SendReqOffer(new ObjectID(CustomerID), offer);

                // tell customer
                SendSayGroupMessage(CustomerID, Config.ChatPrefixString + "This costs you " + expectedsum + " shillings.");
            }
            else
            {
                ObjectID[] offer = new ObjectID[amount];
                
                for (int i = 0; i < amount; i++)
                    offer[i] = new ObjectID(inventoryObjects[i].ID);

                // finally offer him non stackable item(s)                
                SendReqOffer(new ObjectID(CustomerID), offer);

                // tell customer
                SendSayGroupMessage(CustomerID, Config.ChatPrefixString + "This costs you " + expectedsum + " shillings.");
            }
        }
        
        /// <summary>
        /// Processed an received command to show offers
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="Words"></param>
        protected void ProcessCommandShowOffers(uint CustomerID, string[] Words)
        {
            string offers = GetAdvertisement();

            if (offers != null)
            {
                // tell customer
                SendSayGroupMessage(CustomerID, Config.ChatPrefixString + offers);
            }
        }

        /// <summary>
        /// Processed an received botadmin command to give shillings
        /// </summary>
        /// <param name="AdminID"></param>
        protected void ProcessCommandGiveCash(uint AdminID, string[] Words)
        {
            // need to read amount
            if (Words.Length < 2)
                return;

            // try to get shills amount he want
            uint wantCash = 0;
            if (!UInt32.TryParse(Words[1], out wantCash))
                return;

            // try to get player from who list since it's a whisper
            RoomObject roomObject = Data.RoomObjects.GetItemByID(AdminID);

            if (roomObject == null)
                return;
            
            // save this as our tradepartner (required for we offer first)
            Data.Trade.TradePartner = roomObject;
            
            // if it's a bot admin
            if (Config.IsAdmin(roomObject.Name))
            {
                // get our shillings from inventory
                InventoryObject shillings = Data.InventoryObjects.GetItemByName(NAME_SHILLING, false);

                // get how much cash we have
                uint haveCash = 0;
                if (shillings != null)
                    haveCash = shillings.Count;

                // tell we don't have that many
                uint offerCash = wantCash;                
                if (haveCash < wantCash)
                {
                    offerCash = haveCash;

                    // tell admin
                    SendSayGroupMessage(
                        AdminID, Config.ChatPrefixString + "Master " + roomObject.Name + ", I don't have that many shillings. I give you all I have.");
                }

                // the shillings we offer him
                ObjectID[] offer = new ObjectID[] { new ObjectID(shillings.ID, offerCash) };

                // send offer            
                SendReqOffer(new ObjectID(AdminID), offer);
            }
        }

        /// <summary>
        /// Returns an advertisement string for broadcast and whispers.
        /// </summary>
        /// <returns>Advertise message or NULL if no items</returns>
        protected string GetAdvertisement()
        {
            string head = Config.Shopname
                + " ~n~B~k - (~n " + Data.RoomInformation.RoomName + " ~B~k)~n"
                + " ~n~B~k - (~n Prices are per unit ~B~k)~n";  
            
            string offerstr = "~B~kSelling:~n";
            string buystr   = "~B~kBuying:~n";

            List<string[]> offerStrings = new List<string[]>();
            List<string[]> buyStrings = new List<string[]>();

            /// 
            /// Prepare offer-item strings
            /// 
            foreach (ShopItem shopItem in Config.OfferList)
            {
                // get the ones we still have
                ObjectBaseList<InventoryObject> inventoryObjects =
                    Data.InventoryObjects.GetItemsByName(shopItem.Name, false);

                // no more? skip rest of iteration.
                if (inventoryObjects.Count == 0)
                    continue;

                // it's stackable
                if (inventoryObjects[0].IsStackable)
                {
                    int toSell = (int)inventoryObjects[0].Count - (int)shopItem.Amount;

                    if (toSell > 0)                    
                        offerStrings.Add(new string[2] { 
                            toSell.ToString(), 
                            shopItem.Name + " ~r@~n " + shopItem.UnitPrice + " shillings" });
                }

                // non stackable, count entries
                else
                {
                    int toSell = (int)inventoryObjects.Count - (int)shopItem.Amount;

                    if (toSell > 0)
                        offerStrings.Add(new string[2] { 
                            toSell.ToString(),
                            shopItem.Name + " ~r@~n " + shopItem.UnitPrice + " shillings" });                   
                }                              
            }

            /// 
            /// Prepare buy-item strings
            ///
            foreach (ShopItem shopItem in Config.BuyList)
            {
                // skip elements with 0
                if (shopItem.Amount == 0)
                    continue;

                // get the ones we have already
                ObjectBaseList<InventoryObject> inventoryObjects =
                    Data.InventoryObjects.GetItemsByName(shopItem.Name, false);

                // none yet?
                if (inventoryObjects.Count == 0)
                {
                    buyStrings.Add(new string[2] {
                            shopItem.Amount.ToString(),
                            shopItem.Name + " ~r@~n " + shopItem.UnitPrice + " shillings" });
                }

                // it's stackable
                else if (inventoryObjects[0].IsStackable)
                {
                    int toBuy = (int)shopItem.Amount - (int)inventoryObjects[0].Count;

                    if (toBuy > 0)
                        buyStrings.Add(new string[2] {
                                toBuy.ToString(),
                                shopItem.Name + " ~r@~n " + shopItem.UnitPrice + " shillings" });                  
                }

                // non stackable, count entries
                else
                {
                    int toBuy = (int)shopItem.Amount - (int)inventoryObjects.Count;

                    if (toBuy > 0)
                        buyStrings.Add(new string[2] {
                            toBuy.ToString(),
                            shopItem.Name + " ~r@~n " + shopItem.UnitPrice + " shillings" });                  
                }
            }

            /// 
            /// Start building message
            /// 

            string message = head + Environment.NewLine;

            if (offerStrings.Count > 0)
            {
                message += offerstr;
                foreach (string[] strings in offerStrings)
                {
                    message += " ~B~r[~n" + strings[0] + "x " + strings[1] + "~B~r]~n";
                }

                message += Environment.NewLine;
            }

            if (buyStrings.Count > 0)
            {
                message += buystr;
                foreach (string[] strings in buyStrings)
                {
                    message += " ~B~r[~n" + strings[0] + "x " + strings[1] + "~B~r]~n";
                }
            }

            // return message or null
            return (offerStrings.Count > 0 || buyStrings.Count > 0) ? message : null;
        }

        /// <summary>
        /// Returns an help string for chat to tell customer how to use bot.
        /// </summary>
        /// <returns></returns>
        protected string GetHelp()
        {
            string message = Config.Shopname + " ~n~B~k(~n" + Data.RoomInformation.RoomName + "~B~k)~n";

            message += Environment.NewLine;
            message += Environment.NewLine;

            message += "  ~B~rUnknown command~n" + Environment.NewLine + Environment.NewLine;

            message += "  ~B~kWhisper these commands:~n" + Environment.NewLine;
            message += "    " + COMMAND_SHOWOFFERS + "\t\t to see my offers" + Environment.NewLine;
            message += "    " + COMMAND_BUY + " [amount] [name]\t to buy something" + Environment.NewLine;
            message += Environment.NewLine;
            message += "  ~B~kTo sell just offer me the items I'm buying";

            return message;
        }

        /// <summary>
        /// Whispers advertisement to a player
        /// </summary>
        /// <param name="RoomObject"></param>
        protected void TellAdvertisement(RoomObject RoomObject)
        {
            string message = GetAdvertisement();

            if (message != null)
            {
                // tell player
                SendSayGroupMessage(RoomObject.ID, message);

                // log
                Log("BOT", "I whispered advertise to player " + RoomObject.Name);
            }
        }

        /// <summary>
        /// Overrides Update from BaseClient to trigger advertising in intervals.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // make sure to not execute it too early before fully logged in
            if (ObjectID.IsValid(Data.AvatarID) &&
                Data.InventoryObjects.Count > 0)
            {
                string message;
                
                // time to broadcast
                if (GameTick.CanBroadcast())
                {
                    // get adv. message
                    message = GetAdvertisement();

                    // content?
                    if (message != null)
                    {
                        // try to broadcast shop offers
                        SendSayToMessage(ChatTransmissionType.Everyone, message);

                        // log
                        Log("BOT", LOG_ADVBROADCASTED);

                        // additionally mark said also
                        GameTick.DidSay(); 
                    }                  
                }
              
                // time to say
                if (GameTick.CanSay())
                {
                    // get adv. message
                    message = GetAdvertisement();

                    // content?
                    if (message != null)
                    {
                        // try to broadcast shop offers
                        SendSayToMessage(ChatTransmissionType.Normal, message);

                        // log
                        Log("BOT", LOG_ADVSAID);                
                    }  
                }
            }
        }
    }
}
