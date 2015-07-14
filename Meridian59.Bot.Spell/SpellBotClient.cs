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
using Meridian59.Data;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Enums;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common;
using Meridian59.Files;
using Meridian59.Data.Lists;

namespace Meridian59.Bot.Spell
{
    /// <summary>
    /// A client which acts as a spell training bot
    /// </summary>
    public class SpellBotClient : BotClient<GameTick, ResourceManager, DataController, SpellBotConfig>
    {
        #region Constants
        protected const double STARTUPSLEEP = 5000.0;
        #endregion

        protected double tickSleepUntil;
        protected BotTask currentTask;
        protected uint imps = 0;

        protected Dictionary<string,byte> foodList;
        protected bool repeatEat = false;
        protected int lastEatVigor = 0;

        protected bool recoveringHp = false;
        protected bool recoveringMp = false;
        protected bool recoveringVig = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public SpellBotClient()
            : base()
        {           
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();

            // set initial sleep for startup
            tickSleepUntil = GameTick.Current + STARTUPSLEEP;

            // setup list of known food and nutrition
            foodList = new Dictionary<string, byte>() { 
            {"bunch of grapes",7}, {"loaf of bread",20}, {"apple",10}, {"wheel of cheese",30},
            {"mug of stout",6}, {"water skin",3}, {"goblet of wine",6}, {"goblet of Ale",3},
            {"mugs of bew",3}, {"inky-cap mushroom",50},{"edible mushroom",5}, {"meat pie",30},
            {"bowl of soup",9}, {"bowl of stew",15}, {"fortune cookie",1}, {"slice of pork",9},
            {"turkey leg",15}, {"drum stick",9}, {"drumstick",9}, {"spider eyes",9},
            {"spider eye",9},{"copper pekonch mugs",3} };
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
        }
        
        /// <summary>
        /// Handles someone offers you first
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleOfferMessage(OfferMessage Message)
        {
            base.HandleOfferMessage(Message);

            if (Data.Trade.TradePartner == null)
                return;

            // accept any items from configured admins in config, offer nothing in return
            if (Config.IsAdmin(Data.Trade.TradePartner.Name))
            {
                // nothing
                SendReqCounterOffer(new ObjectID[0]);

                // tell admin
                SendSayGroupMessage(
                    Data.Trade.TradePartner.ID,
                    "I will take that, master " + Data.Trade.TradePartner.Name);
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
                    Data.Trade.TradePartner.ID,
                    "Thank you, master " + Data.Trade.TradePartner.Name);

                // accept
                SendAcceptOffer();
            }
        }

        /// <summary>
        /// Handles a new player enters room
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleCreateMessage(CreateMessage Message)
        {
            base.HandleCreateMessage(Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleMessageMessage(MessageMessage Message)
        {
            base.HandleMessageMessage(Message);

            if (Message.Message.FullString.Contains(ChatSubStrings.IMPROVED))
                imps++;

            Log("CHAT", Message.Message.FullString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PartnerID"></param>
        /// <param name="Words"></param>
        protected override void ProcessCommand(uint PartnerID, string[] Words)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool ProcessIsStatRecovered(string stat)
        {
            StatNumeric statData = null;
            bool recoverActive = false;

            switch(stat)
            {
                case "HP":
                    {
                        statData = Data.AvatarCondition.GetItemByNum(StatNums.HITPOINTS);
                        if (statData == null) return false; // false for now to prevent resting being stopped by server save, if creates deadlock then change to true. 
                        recoverActive = recoveringHp = (statData.ValueCurrent < statData.ValueMaximum);
                        Log("Log", "Recovery status HP: " + (int)((float)statData.ValueCurrent / (float)statData.ValueMaximum * 100.0) + "%");
                        break;
                    }
                case "MP":
                    {
                        statData = Data.AvatarCondition.GetItemByNum(StatNums.MANA);
                        if (statData == null) return false;
                        recoverActive = recoveringMp = (statData.ValueCurrent < statData.ValueMaximum);
                        Log("Log", "Recovery status MP: " + (int)((float)statData.ValueCurrent / (float)statData.ValueMaximum * 100.0) + "%");
                        break;
                    }
                case "VIG":
                    {
                        statData = Data.AvatarCondition.GetItemByNum(StatNums.VIGOR);
                        if (statData == null) return false;

                        int current = statData.ValueCurrent;

                        //if (Data.AvatarSkills. != null && Data.AvatarSkills.GetItemsByPrefix("second"))

                        SkillList tl = Data.AvatarSkills;
                        StatList[] sl = tl.ToArray();

                        int max = StatNumsValues.MAXRESTEDVIGOR; // default
                        uint secondWindLevel = 0;
                        for (uint i = 0; i < sl.Length; ++i)
                            if (sl[i].ResourceName.Equals("second wind"))
                            {
                                secondWindLevel = sl[i].SkillPoints;
                                //Log("Log", "Detected Secondwind at Level: " + secondWindLevel);
                                max = StatNumsValues.MAXRESTEDVIGOR + (int)((secondWindLevel + 1) / 5);
                            }

                        recoverActive = recoveringVig = (current < max);
                        Log("Log", "Recovery status VIG: " + (int)((float)current/(float)max * 100.0) + "%");
                        break;
                    }
                default: Log("ERROR", "Unknown stat: " + stat); DoStand(new BotTaskStand()); return true;
            }

            if (!recoverActive)
            {
                Log("Log", "... Recovery of "+ stat +" finished!");
                DoStand(new BotTaskStand());
                tickSleepUntil = GameTick.Current + (Common.GameTick.MSINSECOND * 10);
            }

            return (!recoverActive);
        }

        /// <summary>
        /// Overrides Update from BaseClient to tasks in intervals.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // ...
            if (!Data.IsWaiting &&
                ObjectID.IsValid(Data.AvatarID) &&
                Data.SpellObjects.Count > 0 &&
                Data.AvatarSpells.Count > 0 &&
                GameTick.Current > tickSleepUntil)
            {
                //#if !VANILLA
                    //// Emergency log
                    //if (Data.HitPoints < (Data.AvatarCondition.GetItemByNum(1).ValueMaximum / 3))
                    //{
                    //    DoCast(new BotTaskCast("phase", "", "", "", 100));
                    //}
                //#endif

                bool waitForRecover = false;
                if (recoveringHp)
                    waitForRecover = !ProcessIsStatRecovered("HP");
                if (recoveringMp)
                    waitForRecover |= !ProcessIsStatRecovered("MP");
                if (recoveringVig)
                    waitForRecover |= !ProcessIsStatRecovered("VIG");

                if (waitForRecover)
                    tickSleepUntil = GameTick.Current + (Common.GameTick.MSINSECOND * 10); // Wait 10 Sec
                else if (repeatEat)
                    DoEat(new BotTaskEat("feeding")); // eat more stuff automaticly
                else
                {
                    // get next task
                    currentTask = Config.GetNextTask();

                    // no task?
                    if (currentTask == null)
                    {
                        // log
                        Log("ERROR", "No task found.");

                        return;
                    }

                    // recover
                    if (currentTask is BotTaskRecover)
                    {
                        DoRecover((BotTaskRecover)currentTask);
                        Log("Log", "Waiting for " + ((BotTaskRecover)currentTask).Stat + " to recover ...");
                    }

                    // rest
                    if (currentTask is BotTaskRest)
                        DoRest((BotTaskRest)currentTask);

                    // stand
                    else if (currentTask is BotTaskStand)
                        DoStand((BotTaskStand)currentTask);

                    // sleep: blocks executions until sleep is over
                    else if (currentTask is BotTaskSleep)
                        DoSleep((BotTaskSleep)currentTask);

                    // cast
                    else if (currentTask is BotTaskCast)
                        DoCast((BotTaskCast)currentTask);

                    // eat
                    else if (currentTask is BotTaskEat)
                        DoEat((BotTaskEat)currentTask);

                    // use
                    else if (currentTask is BotTaskUse)
                        DoUse((BotTaskUse)currentTask);

                    // equip
                    else if (currentTask is BotTaskEquip)
                        DoEquip((BotTaskEquip)currentTask);

                    // discard
                    else if (currentTask is BotTaskDiscard)
                        DoDiscard((BotTaskDiscard)currentTask);

                    //selectNPc
                    else if (currentTask is BotTaskSelectNPC)
                        DoSelectNPC((BotTaskSelectNPC)currentTask);

                    //sendBuy
                    else if (currentTask is BotTaskSendBuy)
                        DoSendBuy((BotTaskSendBuy)currentTask);

                    //buyItem
                    else if (currentTask is BotTaskBuyItem)
                        DoBuyItem((BotTaskBuyItem)currentTask);

                    //move
                    else if (currentTask is BotTaskMove)
                        Move((BotTaskMove)currentTask);
                }
            }

            double slp = (tickSleepUntil - GameTick.Current) / (double)Common.GameTick.MSINSECOND;
            
            // draw sleep & imps
            DrawDynamic("SLP: " + string.Format("{0:N0}", slp).PadRight(3) + " IMPS: " + imps.ToString().PadRight(3));
        }

        /// <summary>
        /// Executes a Task 'rest'
        /// </summary>
        protected void DoRest(BotTaskRest Task)
        {
            // request to rest
            SendUserCommandRest();

            // log
            Log("BOT", "Executed task 'rest'.");
        }

        /// <summary>
        /// Executes a Task 'rest'
        /// </summary>
        protected void DoRecover(BotTaskRecover Task)
        {
            switch (Task.Stat.ToLower())
            {
                case "hp":
                case "hps":
                case "hitpoint":
                case "lp":
                case "lebenspunkte":
                case "hitpoints": if (Data.HitPoints >= Data.AvatarCondition.GetItemByNum(StatNums.HITPOINTS).ValueMaximum) return; recoveringHp = true; break;
                case "mp":
                case "mps":
                case "manapoints":
                case "mana": if (Data.ManaPoints >= Data.AvatarCondition.GetItemByNum(StatNums.MANA).ValueMaximum) return; recoveringMp = true; break;
                case "vig":
                case "vigor":
                case "ausdauer":
                case "stamina": if (Data.VigorPoints > StatNumsValues.MAXRESTEDVIGORSECWIND) return; recoveringVig = true; break;
            }
            
            // request to rest
            SendUserCommandRest();

            // log
            Log("BOT", "Executed task 'recovering' "+ Task.Stat +".");
        }

        /// <summary>
        /// Executes a Task 'stand'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoStand(BotTaskStand Task)
        {
            SendUserCommandStand();

            // log
            Log("BOT", "Executed task 'stand'.");
        }

        /// <summary>
        /// Executes a Task 'sleep'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoSleep(BotTaskSleep Task)
        {
            // set sleep tick
            tickSleepUntil = GameTick.Current + Task.Duration;

            // log
            Log("BOT", "Executed task 'sleep' " + (Task.Duration / 1000).ToString() + ".");
        }

        /// <summary>
        /// Executes a Task 'cast'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoCast(BotTaskCast Task)
        {
            SpellObject spellObject = null;
            StatList spellStat = null;

            if (Data.AvatarObject == null || Data.SpellObjects == null )
            {
                Log("WARN", "Can't execute task 'cast'. " + Task.Where + ". Saver is maybe saving or you have no spells.");
                return;
            }

            // test for selfcast
            if (Task.Target.ToLower().Equals("self"))
                Task.Target = Data.AvatarObject.Name;

            // try to get the spell from the spells
            spellObject = Data.SpellObjects.GetItemByName(Task.Name, false);

            // try to get stat for % value
            if (spellObject != null)
                spellStat = Data.AvatarSpells.GetItemByID(spellObject.ID);

            // one not found
            if (spellObject == null || spellStat == null)
            {
                // log
                Log("WARN", "Cant execute task 'cast'. Spell " + Task.Name + " found.");

                return;
            }

            // handle maxed out spell
            if (Task.Cap > 0 && spellStat.SkillPoints >= Task.Cap)
            {
                if (Task.OnMax == SpellBotConfig.XMLVALUE_QUIT)
                {
                    // log
                    Log("BOT", "Quitting.. spell " + spellObject.Name + " reached "+ Task.Cap +"%.");

                    // prepare quit
                    IsRunning = false;
                    return;
                }
                else if (Task.OnMax == SpellBotConfig.XMLVALUE_SKIP)
                {
                    // log
                    Log("BOT", "Skipped task 'cast' " + spellObject.Name + " ("+ Task.Cap +")");

                    return;
                }
            }

            // spell doesn't need a target
            if (spellObject.TargetsCount == 0)
            {
                // send castreq
                SendReqCastMessage(spellObject);

                // log
                Log("BOT", "Executed task 'cast' " + spellObject.Name);
            }

            // speed needs a target
            else if (spellObject.TargetsCount > 0)
            {
                // marked to cast on roomobject
                if (Task.Where == SpellBotConfig.XMLVALUE_ROOM)
                {
                    // try to get the target
                    RoomObject roomObject =
                        Data.RoomObjects.GetItemByName(Task.Target, false);

                    // target not found
                    if (roomObject == null)
                    {
                        // log
                        Log("WARN", "Can't execute task 'cast'. RoomObject " + Task.Target + " not found.");

                        return;
                    }
                 
                    // send castreq
                    ReqCastMessage reqCastMsg = new ReqCastMessage(
                        spellObject.ID, new ObjectID[] { new ObjectID(roomObject.ID) });

                    SendGameMessage(reqCastMsg);

                    // log
                    Log("BOT", "Executed task 'cast' " + spellObject.Name);
                }

                // cast on inventory item
                else if (Task.Where == SpellBotConfig.XMLVALUE_INVENTORY)
                {
                    // try to get the target
                    InventoryObject inventoryObject =
                        Data.InventoryObjects.GetItemByName(Task.Target, false);

                    // target not found
                    if (inventoryObject == null)
                    {
                        // log
                        Log("WARN", "Can't execute task 'cast'. Item " + Task.Target + " not found.");

                        return;
                    }

                    // send castreq
                    ReqCastMessage reqCastMsg = new ReqCastMessage(
                        spellObject.ID, new ObjectID[] { new ObjectID(inventoryObject.ID) });

                    SendGameMessage(reqCastMsg);

                    // log
                    Log("BOT", "Executed task 'cast' " + spellObject.Name);
                }
                else
                {
                    // log
                    Log("WARN", "Can't execute task 'cast'. " + Task.Where + " is unknown 'where'.");
                }
            }
        }

        /// <summary>
        /// Executes a Task 'eat'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoEat(BotTaskEat Task)
        {

            if (Task.Name.ToLower().Equals("feeding")) // eat as much as possible
            {
                int nutritToEat = 200 - Data.VigorPoints;

                if(nutritToEat < 2) // end feeding when fully feeded
                {
                    repeatEat = false;
                    Log("Log", "Ended feeding because vigor is full!");
                    return;
                }

                // If message was to full to eat try to eat a small food item
                if (Data.ChatMessages.LastAddedItem.FullString.ToLower().Equals(ChatSubStrings.FULL))
                {
                    InventoryObject tinyEat = null;
                    foreach (string key in foodList.Keys)
                    {
                        InventoryObject t = Data.InventoryObjects.GetItemByName(key, false);

                        if (t != null)
                        {
                            // Add initial found food that suits needs
                            if (tinyEat == null && foodList[t.Name.ToLower()] <= nutritToEat)
                                tinyEat = t;
                            // Add found food that suits better than the existing one
                            if (tinyEat != null && foodList[t.Name.ToLower()] <= nutritToEat && foodList[t.Name.ToLower()] < foodList[tinyEat.Name.ToLower()])
                                tinyEat = t;
                        }
                    }

                    repeatEat = false;
                    Log("Log", "Ended feeding because stomach is full!");
                    return;
                }

                // Seek food in invent
                InventoryObject bestEat = null;
                foreach (string key in foodList.Keys)
                {
                    InventoryObject t = Data.InventoryObjects.GetItemByName(key, false);

                    if(t != null)
                    {
                        // Add initial found food that suits needs
                        if (bestEat == null && foodList[t.Name.ToLower()] < nutritToEat)
                            bestEat = t;
                        // Add found food that suits better than the existing one
                        if (bestEat != null && foodList[t.Name.ToLower()] < nutritToEat && foodList[t.Name.ToLower()] > foodList[bestEat.Name.ToLower()])
                            bestEat = t;
                    }
                }

                if (bestEat.Count == 0)
                {
                    Log("Log", "Ended feeding because not suitable food was found!");
                    repeatEat = false;
                    return;
                }
                else
                {
                    repeatEat = true;
                    SendReqUseMessage(bestEat.ID);
                    Log("BOT", "Executed task 'feeding' " + bestEat.Name);
                    DoSleep(new BotTaskSleep(1000));
                    return;
                }
                
            }
            else // eat concrete food item
            {
                if (Data.ChatMessages.LastAddedItem.FullString.ToLower().Equals("You are too full to eat."))
                {
                    Log("Log", "Skipped eat " + Task.Name + " because stomach is full!");
                    return;
                }

                // try to get the item from the inventory
                InventoryObject foodObject =
                Data.InventoryObjects.GetItemByName(Task.Name, false);

                // item not found
                if (foodObject == null)
                {
                    // log
                    Log("WARN", "Cant execute task 'eat'. Item " + Task.Name + " not found.");
                    return;
                }

                // if food is known internally test if useage would be waste
                if(foodList.ContainsKey(foodObject.Name))
                    if((StatNumsValues.MAXVIGOR - Data.VigorPoints) < foodList[foodObject.Name] )
                    {
                        Log("Log", "Skipped eat "+Task.Name+" because it would be waste!");
                        return;
                    }

                // eat the food
                SendReqUseMessage(foodObject.ID);
                Log("BOT", "Executed task 'eat' " + foodObject.Name);
            }
        }

        /// <summary>
        /// Executes a Task 'use'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoUse(BotTaskUse Task)
        {          
            // try to get the item from the inventory
            InventoryObject inventoryObject =
                Data.InventoryObjects.GetItemByName(Task.Name, false);

            // item not found
            if (inventoryObject == null)
            {
                // log
                Log("WARN", "Cant execute task 'use'. Item " + Task.Name + " not found.");

                return;
            }

            // send requse
            if (!inventoryObject.IsInUse)
                SendReqUseMessage(inventoryObject.ID);
            // send requnuse
            else
                SendReqUnuseMessage(inventoryObject.ID);

            // log
            Log("BOT", "Executed task 'use' " + inventoryObject.Name);
        }

        /// <summary>
        /// Executes a Task 'discard'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoDiscard(BotTaskDiscard Task)
        {
            // try to get the item from the inventory
            ObjectBaseList<InventoryObject> foundItems =
                Data.InventoryObjects.GetItemsByName(Task.Name, false);

            // item not found
            if (foundItems.Count == 0)
            {
                // log
                Log("WARN", "Cant execute task 'discard'. Item " + Task.Name + " not found in inventory.");

                return;
            }

            // send requse
            foreach (InventoryObject inventoryObject in foundItems)
                if (inventoryObject.IsInUse)
                {
                    SendReqUnuseMessage(inventoryObject.ID);
                    Log("BOT", "Executed task 'discard' " + inventoryObject.Name);
                    return;
                }

            Log("BOT", "Item " + foundItems[0].Name + " wasn't equiped!");
            

            // log
            
        }

        /// <summary>
        /// Executes a Task 'equip'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoEquip(BotTaskEquip Task)
        {
            // try to get the item from the inventory
            InventoryObject foundItem = Data.InventoryObjects.GetItemByName(Task.Name, false);

            // item not found
            if (foundItem == null)
            {
                Log("WARN", "Cant execute task 'equip'. Item " + Task.Name + " not found in inventory.");
                return;
            }

            SendReqUseMessage(foundItem.ID);

            // log
            Log("BOT", "Executed task 'equip' " + foundItem.Name);
        }

        /// <summary>
        /// Executes a Task 'selectNpc'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoSelectNPC(BotTaskSelectNPC Task)
        {
            // try to get the npc target
            RoomObject roomObject =
                Data.RoomObjects.GetItemByName(Task.Name, false);

            // target not found
            if (roomObject == null)
            {
                // log
                Log("WARN", "Can't execute task 'buy'. NPC " + Task.Name + " not found.");
                return;
            }
            else
            {
                Log("BOT", "Found NPC " + roomObject.Name);
            }

            // target NPC
            Data.TargetID = roomObject.ID;
        }

        /// <summary>
        /// Executes a Task 'sendBuy'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoSendBuy(BotTaskSendBuy Task)
        {
            if (Data.TargetObject != null)
            {
                Log("BOT", "Buying items");
                // request buy menu
                SendReqBuyMessage();
            }
            else
                Log("ERROR", "No buy target!");
        }

        protected void DoBuyItem(BotTaskBuyItem Task)
        {
            TradeOfferObject targetItem = Data.Buy.Items.GetItemByName(Task.Name);

            // Find the item in offer
            if (targetItem == null)
            {
                Log("WARN","Item is not for sale: " + Task.Name);
                return;
            }

            uint finalAmount = Task.Amount;

            // Cap to maxamount
            InventoryObject existingStock = Data.InventoryObjects.GetItemByName(targetItem.Name, false);
            if (existingStock.Name.Equals(targetItem.Name))
            {
                if (existingStock.Count > Task.MaxAmount)
                {
                    Log("BOT", "Enough " + Task.Name + " in inventory. Skipped buy!");
                    return;
                }
                if (finalAmount + existingStock.Count > Task.MaxAmount)
                    finalAmount = Task.MaxAmount - existingStock.Count;
            }

            // Test if inventory space is enough
            // todo

            // Test if money is enough

            // get our shillings from inventory
            uint offerSum = finalAmount * targetItem.Price;
            InventoryObject shillings = Data.InventoryObjects.GetItemByName("shilling", false);

            // do we have enough money?
            if (shillings == null || shillings.Count < targetItem.Price)
            {
                Log("WARN", "Not enough money to buy any " + Task.Name);
                return;
            }
            finalAmount = (shillings.Count >= offerSum) ? finalAmount : (uint)(shillings.Count / targetItem.Price);

            if (finalAmount > 0)
            {
                if (Data.Buy.TradePartner != null)
                {
                    Log("WARN", "Lost trade partner, skipping buy! Server is maybe saving!");
                    return;
                }
                // Buy the maximum or desired amount
                targetItem.Count = finalAmount;

                //ReqBuyItemsMessage buyMsg = new ReqBuyItemsMessage(Data.Buy.TradePartner.ID, new ObjectID[] { new ObjectID(targetItem.ID, finalAmount) });

                ObjectID[] orderArray = new ObjectID[1];
                orderArray[0] = new ObjectID(targetItem.ID, finalAmount);
                SendReqBuyItemsMessage(Data.Buy.TradePartner.ID, orderArray);
            }

            // log
            Log("BOT", "Executed task 'buy' " + Task.Amount + "(" + finalAmount + ")" + "x " + Task.Name);
        }

        /// <summary>
        /// Executes a Task 'say'
        /// </summary>
        /// <param name="Task"></param>
        protected void DoSay(BotTaskSay Task)
        {
            // send say
            SendSayToMessage(ChatTransmissionType.Normal, Task.Text);

            // log
            Log("BOT", "Executed task 'say': " + Task.Text);
        }

        protected void Move(BotTaskMove Task)
        {
            V2 BkDoor1 = new V2(80,220);
            V2 BkDoor2 = new V2(540,480);
            Log("BOT", "Send Move");
            //SendReqMoveMessage(ushort X, ushort Y, byte Speed, bool ForceSend = false)
            SendReqMoveMessage((ushort)BkDoor1.X, (ushort)BkDoor1.Y, 50, true); // 50 is run
        }

    }
}
