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
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Files;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Reads messages from a ServerConnection and enriches their included models with their resources.
    /// </summary>
    /// <remarks>
    /// This class has its own thread and is supposed to work in a pipe (asyn producer/consumer pattern)
    /// together with ServerConnection and your application's mainthread.
    /// 
    /// It basically resolves resources (loads them from harddisk) and decompresses them.
    /// All before the messages are passed to the application.
    /// 
    /// Do not read pending GameMessages in the ReceiveQueue of the ServerConnection yourself if you use this class.
    /// Instead read them from the OutputQueue property of this class.
    /// </remarks>    
    public class MessageEnrichment
    {
        #region Constants
        protected const int SLEEPTIME = 5;
        #endregion

        #region Fields
        protected Thread workThread;
        protected ResourceManager resourceManager;
        protected ServerConnection serverConnection;
        public volatile bool IsRunning;
        #endregion

        #region Properties
        /// <summary>
        /// Enriched GameMessages can be read from here.
        /// </summary>
        public LockingQueue<GameMessage> OutputQueue { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="M59ResourceManager">Will be used to resolve/load resources</param>
        /// <param name="ServerConnection">Reads incoming messages from this ServerConnection</param>
        public MessageEnrichment(ResourceManager M59ResourceManager, ServerConnection ServerConnection)
        {
            // init output queue
            OutputQueue = new LockingQueue<GameMessage>();

            // save references
            resourceManager = M59ResourceManager;
            serverConnection = ServerConnection;
            
            // mark running
            IsRunning = true;

            // start own workthread
            workThread = new Thread(ThreadProc);
            workThread.IsBackground = true;
            workThread.Start();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Internal thread procedure
        /// </summary>
        protected void ThreadProc()
        {
            // threadloop
            while(IsRunning)
            {
                GameMessage Message;

                // Process queues:
                // Enriche incoming messages from serverconnection and puts them into output queue
                // Handle all pending incoming messages
                while (serverConnection.ReceiveQueue.TryDequeue(out Message))
                {
                    // start enrichtment
                    HandleGameMessage(Message);

                    // finally enqueue it to output
                    OutputQueue.Enqueue(Message);
                }

                // sleep
                Thread.Sleep(SLEEPTIME);
            }
        }

        #endregion

        #region Meta message handling
        /// <summary>
        /// Top level handler for a new GameMessage from server
        /// </summary>
        /// <param name="Message"></param>
        private void HandleGameMessage(GameMessage Message)
        {
            /* First determine the type, then call the sub-handlers
             * Some few messages don't have typed classes yet,
             * so they appear as GenericGameMessage
             */

            if (Message is LoginModeMessage)
            {                              
                HandleLoginModeMessage((LoginModeMessage)Message);
            }
            else if (Message is GameModeMessage)
            {              
                HandleGameModeMessage((GameModeMessage)Message);
            }

            // GenericGameMessage
            else
            {
                //             
            }
        }

        /// <summary>
        /// Will be executed for any new LoginMode message from the server
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleLoginModeMessage(LoginModeMessage Message)
        {
            /*switch ((MessageTypeLoginMode)Message.PI)
            {
                
            }*/
        }

        /// <summary>
        /// Will be executed for any new GameMode message from the server
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleGameModeMessage(GameModeMessage Message)
        {
            switch ((MessageTypeGameMode)Message.PI)
            {
                case MessageTypeGameMode.Player:                                // 130
                    HandlePlayerMessage((PlayerMessage)Message);
                    break;

                case MessageTypeGameMode.Stat:                                  // 131
                    HandleStatMessage((StatMessage)Message);
                    break;

                case MessageTypeGameMode.StatGroup:                             // 132
                    HandleStatGroupMessage((StatGroupMessage)Message);
                    break;

                case MessageTypeGameMode.RoomContents:                          // 134
                    HandleRoomContentsMessage((RoomContentsMessage)Message);
                    break;

                case MessageTypeGameMode.ObjectContents:                        // 135
                    HandleObjectContentsMessage((ObjectContentsMessage)Message);
                    break;

                case MessageTypeGameMode.CharInfo:                              // 140
                    HandleCharInfoMessage((CharInfoMessage)Message);
                    break;

                case MessageTypeGameMode.Spells:                                // 141
                    HandleSpellsMessage((SpellsMessage)Message);
                    break;

                case MessageTypeGameMode.SpellAdd:                              // 142
                    HandleSpellAddMessage((SpellAddMessage)Message);
                    break;

                case MessageTypeGameMode.AddEnchantment:                        // 147
                    HandleAddEnchantmentMessage((AddEnchantmentMessage)Message);
                    break;

                case MessageTypeGameMode.Background:                            // 150
                    HandleBackgroundMessage((BackgroundMessage)Message);
                    break;

                case MessageTypeGameMode.PlayerOverlay:                         // 151
                    HandlePlayerOverlay((PlayerOverlayMessage)Message);
                    break;

                case MessageTypeGameMode.UserCommand:                           // 155
                    HandleUserCommandMessage((UserCommandMessage)Message);
                    break;

                case MessageTypeGameMode.PlayWave:                              // 170
                    HandlePlayWaveMessage((PlayWaveMessage)Message);
                    break;

                case MessageTypeGameMode.PlayMusic:                             // 171
                    HandlePlayMusicMessage((PlayMusicMessage)Message);
                    break;

                case MessageTypeGameMode.PlayMidi:                              // 172
                    HandlePlayMidiMessage((PlayMidiMessage)Message);
                    break;

                case MessageTypeGameMode.LookNewsGroup:                         // 180
                    HandleLookNewsGroupMessage((LookNewsGroupMessage)Message);
                    break;
                
                case MessageTypeGameMode.Shoot:                                 // 202
                    HandleShootMessage((ShootMessage)Message);
                    break;

                case MessageTypeGameMode.Look:                                  // 207
                    HandleLookMessage((LookMessage)Message);
                    break;

                case MessageTypeGameMode.Inventory:                             // 208
                    HandleInventoryMessage((InventoryMessage)Message);
                    break;

                case MessageTypeGameMode.InventoryAdd:                          // 209
                    HandleInventoryAddMessage((InventoryAddMessage)Message);
                    break;

                case MessageTypeGameMode.Offer:                                 // 211
                    HandleOfferMessage((OfferMessage)Message);
                    break;

                case MessageTypeGameMode.Offered:                               // 213
                    HandleOfferedMessage((OfferedMessage)Message);
                    break;

                case MessageTypeGameMode.CounterOffer:                          // 214
                    HandleCounterOfferMessage((CounterOfferMessage)Message);
                    break;

                case MessageTypeGameMode.CounterOffered:                        // 215
                    HandleCounterOfferedMessage((CounterOfferedMessage)Message);
                    break;

                case MessageTypeGameMode.BuyList:                               // 216
                    HandleBuyListMessage((BuyListMessage)Message);
                    break;
                
                case MessageTypeGameMode.Create:                                // 217
                    HandleCreateMessage((CreateMessage)Message);
                    break;

                case MessageTypeGameMode.Change:                                // 219
                    HandleChangeMessage((ChangeMessage)Message);
                    break;

                case MessageTypeGameMode.ChangeTexture:                         // 227
                    HandleChangeTextureMessage((ChangeTextureMessage)Message);
                    break;
            }
        }
        #endregion

        #region Message handlers
        protected virtual void HandlePlayerMessage(PlayerMessage Message)
        {
            Message.RoomInfo.ResolveResources(resourceManager, false);
 
            if (Message.RoomInfo.ResourceRoom != null)                             
                Message.RoomInfo.ResourceRoom.UncompressAll();                            
        }

        protected virtual void HandleStatMessage(StatMessage Message)
        {           
            Message.Stat.ResolveResources(resourceManager, false);

            if (Message.Stat.Resource != null)
                Message.Stat.Resource.DecompressAll();           
        }

        protected virtual void HandleStatGroupMessage(StatGroupMessage Message)
        {
            foreach (Stat stat in Message.Stats)
            {
                stat.ResolveResources(resourceManager, false);

                if (stat.Resource != null)                    
                    stat.Resource.DecompressAll();
            }
        }

        protected virtual void HandleRoomContentsMessage(RoomContentsMessage Message)
        {
            foreach (RoomObject obj in Message.RoomObjects)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleObjectContentsMessage(ObjectContentsMessage Message)
        {
            foreach (ObjectBase obj in Message.ContentObjects)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleCharInfoMessage(CharInfoMessage Message)
        {
            Message.CharCreationInfo.ResolveResources(resourceManager, false);
            Message.CharCreationInfo.DecompressResources();
        }

        protected virtual void HandleSpellsMessage(SpellsMessage Message)
        {
            foreach (SpellObject obj in Message.SpellObjects)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleSpellAddMessage(SpellAddMessage Message)
        {
            if (Message.NewSpellObject != null)
            {
                Message.NewSpellObject.ResolveResources(resourceManager, false);
                Message.NewSpellObject.DecompressResources();
            }            
        }

        protected virtual void HandleAddEnchantmentMessage(AddEnchantmentMessage Message)
        {
            ObjectBase buffObj = Message.NewBuffObject;
            buffObj.ResolveResources(resourceManager, false);
            buffObj.DecompressResources();   
        }

        protected virtual void HandleBackgroundMessage(BackgroundMessage Message)
        {
            ResourceIDBGF resource = Message.ResourceID;
            resource.ResolveResources(resourceManager, false);

            if (resource.Resource != null)
                resource.Resource.DecompressAll();
        }

        protected virtual void HandleUserCommandMessage(UserCommandMessage Message)
        {
            switch (Message.Command.CommandType)
            {
                case UserCommandType.LookPlayer:
                    ObjectBase charData = ((UserCommandLookPlayer)Message.Command).PlayerInfo.ObjectBase;
                    
                    charData.ResolveResources(resourceManager, false);
                    charData.DecompressResources();
                    break;

                case UserCommandType.GuildShields:
                    // can either be GuildShieldList or GuildShieldListReq
                    if (Message.Command is UserCommandGuildShieldList)
                    {
                        UserCommandGuildShieldList comGuildShields = (UserCommandGuildShieldList)Message.Command;

                        foreach (ResourceIDBGF obj in comGuildShields.ShieldResources)
                        {
                            obj.ResolveResources(resourceManager, false);

                            if (obj.Resource != null)
                                obj.Resource.DecompressAll();
                        }
                    }
                    break;
            }
        }

        protected virtual void HandlePlayWaveMessage(PlayWaveMessage Message)
        {
            Message.PlayInfo.ResolveResources(resourceManager, false);
        }

        protected virtual void HandlePlayMusicMessage(PlayMusicMessage Message)
        {           
            Message.PlayInfo.ResolveResources(resourceManager, false);
        }

        protected virtual void HandlePlayMidiMessage(PlayMidiMessage Message)
        {
            Message.PlayInfo.ResolveResources(resourceManager, false);
        }

        protected virtual void HandleLookNewsGroupMessage(LookNewsGroupMessage Message)
        {
            Message.NewsGroup.NewsGlobeObject.ResolveResources(resourceManager, false);
            Message.NewsGroup.NewsGlobeObject.DecompressResources();
        }

        protected virtual void HandleShootMessage(ShootMessage Message)
        {
            Message.Projectile.ResolveResources(resourceManager, false);
            Message.Projectile.DecompressResources();
        }

        protected virtual void HandleLookMessage(LookMessage Message)
        {
            Message.ObjectInfo.ObjectBase.ResolveResources(resourceManager, false);
            Message.ObjectInfo.ObjectBase.DecompressResources();
        }

        protected virtual void HandleInventoryMessage(InventoryMessage Message)
        {
            foreach (ObjectBase obj in Message.InventoryObjects)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleInventoryAddMessage(InventoryAddMessage Message)
        {
            Message.NewInventoryObject.ResolveResources(resourceManager, false);
            Message.NewInventoryObject.DecompressResources();
        }

        protected virtual void HandleOfferMessage(OfferMessage Message)
        {
            Message.TradePartner.ResolveResources(resourceManager, false);
            Message.TradePartner.DecompressResources();

            foreach (ObjectBase obj in Message.OfferedItems)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleOfferedMessage(OfferedMessage Message)
        {           
            foreach (ObjectBase obj in Message.OfferedItems)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();                
            }
        }

        protected virtual void HandleCounterOfferMessage(CounterOfferMessage Message)
        {           
            foreach (ObjectBase obj in Message.OfferedItems)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleCounterOfferedMessage(CounterOfferedMessage Message)
        {
            foreach (ObjectBase obj in Message.OfferedItems)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleBuyListMessage(BuyListMessage Message)
        {
            Message.TradePartner.ResolveResources(resourceManager, false);
            Message.TradePartner.DecompressResources();

            foreach (TradeOfferObject obj in Message.OfferedItems)
            {
                obj.ResolveResources(resourceManager, false);
                obj.DecompressResources();
            }
        }

        protected virtual void HandleCreateMessage(CreateMessage Message)
        {
            Message.NewRoomObject.ResolveResources(resourceManager, false);
            Message.NewRoomObject.DecompressResources();
        }

        protected virtual void HandleChangeMessage(ChangeMessage Message)
        {
            Message.UpdatedObject.ResolveResources(resourceManager, false);
            Message.UpdatedObject.DecompressResources();
        }

        protected virtual void HandleChangeTextureMessage(ChangeTextureMessage Message)
        {
            Message.TextureChangeInfo.ResolveResources(resourceManager, false);

            if (Message.TextureChangeInfo.Resource != null)
                Message.TextureChangeInfo.Resource.DecompressAll();
        }

        protected virtual void HandlePlayerOverlay(PlayerOverlayMessage Message)
        {
            Message.HandItemObject.ResolveResources(resourceManager, false);
            Message.HandItemObject.DecompressResources();
        }
        #endregion      
    }
}
