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
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Data;
using Meridian59.Protocol;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.SubMessage;
using Meridian59.Files;
using Meridian59.Files.ROO;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Client
{
    /// <summary>
    /// Still abstract and generic client extending RootClient class.
    /// This adds internal server-connection and message enrichment modules
    /// to the RootClient implementation, so it's able to maintain it's own
    /// connection to a server and send game messages.
    /// </summary>
    /// <remarks>
    /// The generics are supposed to allow you to use the abstract
    /// client class-hierarchy, but plug in your own/modified, deriving module-implementations.
    /// </remarks>
    /// <typeparam name="T">Type of GameTick or deriving class</typeparam>
    /// <typeparam name="R">Type of ResourceManager or deriving class</typeparam>
    /// <typeparam name="D">Type of DataController or deriving class</typeparam>
    /// <typeparam name="C">Type of Config or deriving class</typeparam>
    public abstract class BaseClient<T, R, D, C> : RootClient<T, R, D, C>
        where T:GameTick, new()
        where R:ResourceManager, new()
        where D:DataController, new()
        where C:Config, new()
    {
        #region Constants
        protected const string REQSPELLTCERROR      = "Warning: Can't send ReqCast. This spell seems to require more than 1 target.";
        protected const string WRONGCREDENTIALS     = "Your account credentials are not correct.";
        protected const string APPVERSIONMISMATCH   = "Your major/minor versions don't match the server.";
        protected const string RESVERSIONMISMATCH   = "Your Download (resources) version don't match the server.";
        protected const string NOCHARACTERS         = "Your account doesn't have any character slots.";
        #endregion

        #region Abstract properties to implement
        public abstract byte AppVersionMajor { get; }
        public abstract byte AppVersionMinor { get; }
        #endregion

        #region Fields
        protected uint tpsCounter;
        #endregion
      
        #region Major components
        public ServerConnection ServerConnection { get; protected set; }
        public MessageEnrichment MessageEnrichment { get; protected set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseClient()
            : base()
        {            
            // Initialize NetworkClient
            ServerConnection = new ServerConnection(ResourceManager.StringResources);

            // Initialize resource loader (message enrichment thread)
            MessageEnrichment = new MessageEnrichment(ResourceManager, ServerConnection);

            // hook up lists/model observers
            Data.ActionButtons.ListChanged += OnActionButtonListChanged;
        }
        
        #endregion

        #region Methods
        /// <summary>
        /// Connects to the currently selected ConnectionInfo in Config
        /// </summary>
        public virtual void Connect()
        {
            // must have active connectionentry to connect
            if (Config.SelectedConnectionInfo == null)
                return;

            // load the strings for this connectionentry
		    ResourceManager.SelectStringDictionary(
                Config.SelectedConnectionInfo.StringDictionary,
				LanguageCode.English); // todo: from config

		    // fill ignore list in datacontroller with ignored
            // playernames for this connectionentry.
		    Data.IgnoreList.Clear();
            Data.IgnoreList.AddRange(Config.SelectedConnectionInfo.IgnoreList);

            // connect to server
            ServerConnection.Connect(
                Config.SelectedConnectionInfo.Host, 
                Config.SelectedConnectionInfo.Port,
                Config.SelectedConnectionInfo.UseIPv6);
        }

        /// <summary>
        /// Disconnects from the server and resets datalayer.
        /// </summary>
        public virtual void Disconnect()
        {
            ServerConnection.Disconnect();

            Data.Reset();
            Data.UIMode = UIMode.Login;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            // init the legacy resources
		    ResourceManager.Init(
			    Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHSTRINGS,
			    Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHROOMS,
			    Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHOBJECTS,
			    Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHROOMTEXTURES,
			    Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHSOUNDS,
			    Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHMUSIC,
                Config.ResourcesPath + "/" + Meridian59.Files.ResourceManager.SUBPATHMAILS);
        }

        /// <summary>
        /// Implement this with your code for each tick/update.
        /// </summary>
        public override void Update()
        {
            // process queues (read new messages and react)
            ProcessQueues();

            // update room (wallside animations, movements, ...)
            if (CurrentRoom != null)
                CurrentRoom.Tick(GameTick.Current, GameTick.Span);

            // update data in datacontroller
            Data.Tick(GameTick.Current, GameTick.Span);

            // update the TPS value
            UpdateTPS();

            // update RTT value
            Data.RTT = (uint)ServerConnection.RTT;

            if (Data.AvatarObject != null &&
                Data.AvatarObject.IsMoving)
            {
                // possibly send a position update            
                SendReqMoveMessage();
            }  
        }

        /// <summary>
        /// Updates the TPS value in DataController with
        /// ticks processed in last second.
        /// </summary>
        protected void UpdateTPS()
        {
            // raise the tpsCounter for measuring
            tpsCounter++;

            if (GameTick.CanTPSMeasure())
            {
                // calc tps based on processed ticks and span
                Data.TPS = (uint)((Real)tpsCounter / ((Real)Math.Max(0.0001f, (Real)GameTick.SpanTPSMeasure) / (Real)Common.GameTick.MSINSECOND));
                
                // reset tps measure
                tpsCounter = 0;

                // save last tps measure tick
                GameTick.DidTPSMeasure();
            }
        }

        /// <summary>
        /// Executed when an exception was asynchronously received from the ServerConnection
        /// through it ExceptionQueue
        /// </summary>
        protected abstract void OnServerConnectionException(Exception Error);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnActionButtonListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Data.ActionButtons[e.NewIndex].Activated += OnActionButtonActivated;
                    break;

                case ListChangedType.ItemDeleted:
                    Data.ActionButtons.LastDeletedItem.Activated -= OnActionButtonActivated;
                    break;
            }
        }

        /// <summary>
        /// Executed when an actionbutton in the datacontroller is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnActionButtonActivated(object sender, EventArgs e)
        {
            ActionButtonConfig button = (ActionButtonConfig)sender;

            if (button.Data != null)
            {
                switch (button.ButtonType)
                {
                    case ActionButtonType.Action:
                        ExecAction((AvatarAction)button.Data);
                        break;

                    case ActionButtonType.Item:
                        UseUnuseApply((InventoryObject)button.Data);
                        break;

                    case ActionButtonType.Spell:
                        SendReqCastMessage((SpellObject)button.Data);
                        break;
                }
            }
        }

        /// <summary>
        /// Cleanup, runs after loop exits
        /// </summary>
        protected override void Cleanup()
        {
            if (MessageEnrichment != null)
            {
                MessageEnrichment.IsRunning = false;
                MessageEnrichment = null;
            }

            if (ServerConnection != null)
            {
                ServerConnection.Disconnect();
                ServerConnection = null;
            }

            // last because of logging
            base.Cleanup();
        }

        /// <summary>
        /// Handles the queues of other threads (network/enrichment)
        /// </summary>
        protected virtual void ProcessQueues()
        {
            Exception error;
            GameMessage message;

            // Handle all exceptions from networkclient
            while (ServerConnection.ExceptionQueue.TryDequeue(out error))          
                OnServerConnectionException(error);
                             
            // Handle all pending incoming messages done from enrichment
            while (MessageEnrichment.OutputQueue.TryDequeue(out message))          
                HandleGameMessage(message);
            
            // Handle the outgoing MessageLog (debug for sent packets)
            while (ServerConnection.OutgoingPacketLog.TryDequeue(out message))
                Data.LogOutgoingPacket(message);         
        }

        #endregion

        #region Meta message handling
        /// <summary>
        /// Will be executed for any new LoginMode message from the server
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleLoginModeMessage(LoginModeMessage Message)
        {
            switch ((MessageTypeLoginMode)Message.PI)
            {
                case MessageTypeLoginMode.GetClient:                        // 7
                    HandleGetClientMessage((GetClientMessage)Message);
                    break;

                case MessageTypeLoginMode.GetLogin:                         // 21
                    HandleGetLoginMessage((GetLoginMessage)Message);
                    break;

                case MessageTypeLoginMode.GetChoice:                        // 22
                    HandleGetChoiceMessage((GetChoiceMessage)Message);
                    break;

                case MessageTypeLoginMode.LoginOK:                          // 23
                    HandleLoginOKMessage((LoginOKMessage)Message);
                    break;

                case MessageTypeLoginMode.LoginFailed:                      // 24
                    HandleLoginFailedMessage((LoginFailedMessage)Message);
                    break;

                case MessageTypeLoginMode.Download:                         // 31
                    HandleDownloadMessage((DownloadMessage)Message);
                    break;

                case MessageTypeLoginMode.Message:                          // 34
                    HandleLoginModeMessageMessage((LoginModeMessageMessage)Message);
                    break;

                case MessageTypeLoginMode.NoCharacters:                     // 37
                    HandleNoCharactersMessage((NoCharactersMessage)Message);
                    break;
            }
        }

        /// <summary>
        /// Will be executed for any new GameMode message from the server
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleGameModeMessage(GameModeMessage Message)
        {
            switch ((MessageTypeGameMode)Message.PI)
            {
                case MessageTypeGameMode.Message:                           // 32
                    HandleMessageMessage((MessageMessage)Message);
                    break;

                case MessageTypeGameMode.CharInfoOk:                        // 56
                    HandleCharInfoOKMessage((CharInfoOkMessage)Message);
                    break;
                
                case MessageTypeGameMode.LoadModule:                        // 58
                    HandleLoadModuleMessage((LoadModuleMessage)Message);
                    break;

                case MessageTypeGameMode.Mail:                              // 80
                    HandleMailMessage((MailMessage)Message);
                    break;

                case MessageTypeGameMode.Player:                            // 130
                    HandlePlayerMessage((PlayerMessage)Message);
                    break;

                case MessageTypeGameMode.Characters:                        // 139
                    HandleCharactersMessage((CharactersMessage)Message);
                    break;

                case MessageTypeGameMode.Quit:                              // 149
                    HandleQuitMessage((QuitMessage)Message);
                    break;

                case MessageTypeGameMode.UserCommand:                       // 155
                    HandleUserCommandMessage((UserCommandMessage)Message);
                    break;

                case MessageTypeGameMode.Admin:                             // 162
                    HandleAdminMessage((AdminMessage)Message);
                    break;

                case MessageTypeGameMode.LookNewsGroup:                     // 180
                    HandleLookNewsGroupMessage((LookNewsGroupMessage)Message);
                    break;

                case MessageTypeGameMode.LookupNames:                       // 190
                    HandleLookupNamesMessage((LookupNamesMessage)Message);
                    break;

                case MessageTypeGameMode.Said:                              // 206
                    HandleSaidMessage((SaidMessage)Message);
                    break;

                case MessageTypeGameMode.WallAnimate:                       // 225
                    HandleWallAnimateMessage((WallAnimateMessage)Message);
                    break;

                case MessageTypeGameMode.InvalidateData:                    // 228
                    HandleInvalidateDataMessage((InvalidateDataMessage)Message);
                    break;
            }
        }
        #endregion

        #region LoginMode message handling

        /// <summary>
        /// Your client major/minor versions don't match server. 
        /// Implement this with a proper response.
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleGetClientMessage(GetClientMessage Message);

        /// <summary>
        /// Implement this with a proper Login response
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleGetLoginMessage(GetLoginMessage Message);

        /// <summary>
        /// Internally handled, no need for extra stuff
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleGetChoiceMessage(GetChoiceMessage Message)
        {
            // request game state
            SendReqGameStateMessage();
        }

        /// <summary>
        /// Overwrite with your code for successful login credentials
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleLoginOKMessage(LoginOKMessage Message);

        /// <summary>
        /// Overwrite with your code for unsuccessful login credentials
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleLoginFailedMessage(LoginFailedMessage Message)
        {
            if (ServerConnection != null)
                ServerConnection.Disconnect();
        }

        /// <summary>
        /// Overwrite with your code for no characters error
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleNoCharactersMessage(NoCharactersMessage Message)
        {
            if (ServerConnection != null)
                ServerConnection.Disconnect();
        }

        /// <summary>
        /// Overwrite with your code for a message instead of succssful login
        /// (i.e. account blocked, maintenance...)
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleLoginModeMessageMessage(LoginModeMessageMessage Message);     

        /// <summary>
        /// Overwrite with your code for a mismatch resources situation (update/download)
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleDownloadMessage(DownloadMessage Message)
        {          
        }
        #endregion

        #region GameMode message handling
        /// <summary>
        /// Handles the "CharInfoOK" message.
        /// Sends a UseCharacter as reply to login the accepted avatar.
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleCharInfoOKMessage(CharInfoOkMessage Message)
        {
            SendUseCharacterMessage(new ObjectID(Message.CharacterID), true);
		    SendUserCommandSafetyMessage(true);
        }

        /// <summary>
        /// Handles the "LoadModule" message, basically obsolete for this framework
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleLoadModuleMessage(LoadModuleMessage Message)
        {
            /* workaround:
             * if server tells client to load the "char.dll",
             * we can request the available characters.
             * 
             * if something goes wrong here, connection establishing may hang
             */
            
            string modulefile;
            if (ResourceManager.StringResources.TryGetValue(Message.ResourceID, out modulefile, LanguageCode.English))
            {
                if (String.Equals(modulefile, CHARDLL))
                {
                    SendSendCharactersMessage();
                }
            }
        }

        /// <summary>
        /// Handle the "Mail" message.
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleMailMessage(MailMessage Message)
        {
            // request to delete this mail now that we've downloaded it
            SendDeleteMail(Message.Mail.Num);

            // update the num to our local next one
            Message.Mail.Num = ResourceManager.Mails.GetMaximumNum() + 1;

            // add it to our list (will trigger a save to disk)
            ResourceManager.Mails.Add(Message.Mail);
        }

        /// <summary>
        /// Handles a player info message (initializes a room)
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandlePlayerMessage(PlayerMessage Message)
        {                      
        }

        /// <summary>
        /// Implement this with your handler when server sends your available characters,
        /// this is not stored in datalayer.
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleCharactersMessage(CharactersMessage Message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleQuitMessage(QuitMessage Message)
        {
            Data.Reset();
        }

        /// <summary>
        /// Handles a LookNewsGroupMessage by requesting its articles.
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleLookNewsGroupMessage(LookNewsGroupMessage Message)
        {
            SendReqArticles();
        }

        /// <summary>
        /// Handles a LookupNames message.
        /// Does nothing in this base class. Overwrite it.
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleLookupNamesMessage(LookupNamesMessage Message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleSaidMessage(SaidMessage Message)
        {
            // try get player for this message
            OnlinePlayer player = Data.OnlinePlayers.GetItemByID(Message.Message.SourceObjectID);

            // skip ignored players
            if (player != null && Data.IgnoreList.Contains(player.Name))
                return;
        }

        /// <summary>
        /// Handles a wall animate message, updates the according animation object
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleWallAnimateMessage(WallAnimateMessage Message)
        {
            if (CurrentRoom != null)
            {
                foreach (RooSideDef wallSide in CurrentRoom.SideDefs)
                {                    
                    if (wallSide.ServerID == Message.SideDefServerID)
                    {
                        // set animation
                        wallSide.Animation = Message.Animation;

                        switch (Message.Action)
                        {
                            case RoomAnimationAction.RA_NONE:
                                break;

                            case RoomAnimationAction.RA_PASSABLE_END:
                                wallSide.Flags.IsPassable = true;
                                break;

                            case RoomAnimationAction.RA_IMPASSABLE_END:
                                wallSide.Flags.IsPassable = false;
                                break;

                            case RoomAnimationAction.RA_INVISIBLE_END:
                                wallSide.Flags.IsPassable = true;
                                //todo: hide normal walltexture
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles UserCommand messages
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleUserCommandMessage(UserCommandMessage Message)
        {
            switch (Message.Command.CommandType)
            {
                case UserCommandType.SendQuit:
                    SendReqQuit();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleAdminMessage(AdminMessage Message)
        {
        }

        /// <summary>
        /// Handles the invalidate request which resetted the datalayer
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleInvalidateDataMessage(InvalidateDataMessage Message)
        {
            // request all required info since everything is invalidated
            RequestInfoAfterServerSave();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleMessageMessage(MessageMessage Message)
        {
        }
        #endregion

        #region Message senders
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public override void SendGameMessage(GameMessage Message)
        {
            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(Message);
        }

        /// <summary>
        /// Sends the account credentials to the server (use in respond to ReqLogin)
        /// </summary>
        /// <param name="Username">accountname</param>
        /// <param name="Password">password for account</param>
        public virtual void SendLoginMessage(string Username, string Password)
        {
            // create message instance
            LoginMessage message = new LoginMessage(
                Username, Password,
                AppVersionMajor, AppVersionMinor,
                LoginMessage.WINTYPE_NT, 6, 1, 512000000, LoginMessage.CPUTYPE_PENTIUM,
                MeridianExeCRCs.NEWCLIENTDETECT,
                1024, 768, 0, 0, 32, 30, 0);
            
            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests a switch to gamemode protocol with final verification data (like the downloadversion)
        /// </summary>
        public virtual void SendReqGameStateMessage()
        {
            // create message instance
            ReqGameStateMessage message = new ReqGameStateMessage(
                Config.ResourcesVersion, 
                AppVersionMajor, 
                AppVersionMinor, 
                String.Empty);
            
            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the characters belonging to the current account.
        /// </summary>
        public virtual void SendSendCharactersMessage()
        {
            // create message instance
            SendCharactersMessage message = new SendCharactersMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to login with the selected avatar ID (used in selection screen).
        /// </summary>
        /// <param name="ID">Avatar ID</param>
        /// <param name="RequestBasicInfo">Will automatically also request required info for play</param>
        /// <param name="Name">A name of the character as shown with the ID</param>
        public virtual void SendUseCharacterMessage(ObjectID ID, bool RequestBasicInfo, string Name = null)
        {
            // create message instance
            UseCharacterMessage message = new UseCharacterMessage(ID);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);

            // request all basic gameinfo
            // like known spells, inventory, current stats, condition, ...
            if (RequestBasicInfo)           
                RequestInfoAfterLogin();

            if (Name == null)
                Name = String.Empty;

            // prepare actionbuttons
            Data.ActionButtons.Clear();
            Data.ActionButtons.PlayerName = Name;

            // set uimode to playing
            Data.UIMode = UIMode.Playing;
        }

        /// <summary>
        /// Requests to login with the avatar at Index in
        /// DataController WelcomeInfo
        /// </summary>
        /// <param name="Index">Index to login from WelcomeInfo</param>
        /// <param name="RequestBasicInfo">Will automatically also request required info for play</param>
        public virtual void SendUseCharacterMessage(int Index, bool RequestBasicInfo = true)
        {
            // valid?
            if (Index > -1 && Data.WelcomeInfo.Characters.Count > Index)
            {
                // get character at this index
                CharSelectItem item = Data.WelcomeInfo.Characters[Index];

                // use other sender
                SendUseCharacterMessage(new ObjectID(item.ID), RequestBasicInfo, item.Name);
            }
        }

        /// <summary>
        /// Requests your avatar's stats
        /// </summary>
        /// <param name="Type">Condition, attributes, skills, ...</param>
        public virtual void SendSendStatsMessage(StatGroup Type)
        {
            // create message instance
            SendStatsMessage message = new SendStatsMessage(Type);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests current inventory from server
        /// </summary>
        public virtual void SendReqInventoryMessage()
        {
            // create message instance
            ReqInventoryMessage message = new ReqInventoryMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests current enchantments from the server.
        /// </summary>
        /// <param name="Type">Either for player or for room</param>
        public virtual void SendSendEnchantmentsMessage(BuffType Type)
        {
            // create message instance
            SendEnchantmentsMessage message = new SendEnchantmentsMessage(Type);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests avatar's known spells.
        /// </summary>
        public virtual void SendSendSpellsMessage()
        {
            // create message instance
            SendSpellsMessage message = new SendSpellsMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests avatar's known skills
        /// </summary>
        public virtual void SendSendSkillsMessage()
        {
            // create message instance
            SendSkillsMessage message = new SendSkillsMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Sends a self created UserCommand
        /// </summary>
        /// <param name="UserCommand"></param>
        public virtual void SendUserCommand(UserCommand UserCommand)
        {
            // create message instance
            UserCommandMessage message = new UserCommandMessage(UserCommand, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Tells server to set safety on or off
        /// </summary>
        /// <param name="On"></param>
        public virtual void SendUserCommandSafetyMessage(bool On)
        {
            byte val = Convert.ToByte(On);
            
            // create message instance
            UserCommandSafety userCommand = new UserCommandSafety(val);
            UserCommandMessage message = new UserCommandMessage(userCommand, null);
            
            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);

            // save updated state in datacontroller
            Data.IsSafety = On;
        }

        /// <summary>
        /// Requests to deposit something to the closest NPC? (no ID!)
        /// </summary>
        /// <param name="Amount">Amount to try to deposit</param>
        public virtual void SendUserCommandDeposit(uint Amount)
        {
            // create message instance
            UserCommandDeposit userCommand = new UserCommandDeposit(Amount);
            UserCommandMessage message = new UserCommandMessage(userCommand, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to withdraw something from the closest NPC (no ID!)
        /// </summary>
        /// <param name="Amount">Amount to try to withdraw</param>
        public virtual void SendUserCommandWithDraw(uint Amount)
        {
            // create message instance
            UserCommandWithDraw userCommand = new UserCommandWithDraw(Amount);
            UserCommandMessage message = new UserCommandMessage(userCommand, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the basic info about your guild
        /// </summary>
        public virtual void SendUserCommandGuildInfoReq()
        {
            // create message instance
            UserCommand command = new UserCommandGuildInfoReq();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the list of existing guilds and your relation to them
        /// </summary>
        public virtual void SendUserCommandGuildGuildListReq()
        {
            // create message instance
            UserCommand command = new UserCommandGuildGuildListReq();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the list of existing guildshields
        /// </summary>
        public virtual void SendUserCommandGuildShieldListReq()
        {
            // create message instance
            UserCommand command = new UserCommandGuildShieldListReq();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests data for your guildshield design.
        /// </summary>
        public virtual void SendUserCommandGuildShieldInfoReq()
        {
            // create message instance
            UserCommand command = new UserCommandGuildShieldInfoReq();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to give leadership of your guild to someone else
        /// </summary>
        /// <param name="ID">ID of new leader</param>
        public virtual void SendUserCommandGuildAbdicate(uint ID)
        {
            // create message instance
            UserCommand command = new UserCommandGuildAbdicate(new ObjectID(ID));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to exile a member of your guild.
        /// </summary>
        /// <param name="ID">ID of member to exile</param>
        public virtual void SendUserCommandGuildExile(uint ID)
        {
            // create message instance
            UserCommand command = new UserCommandGuildExile(new ObjectID(ID));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to set rank of a guildmember
        /// </summary>
        /// <param name="Member"></param>
        /// <param name="Rank"></param>
        public virtual void SendUserCommandGuildSetRank(uint Member, byte Rank)
        {
            // create message instance
            UserCommand command = new UserCommandGuildSetRank(new ObjectID(Member), Rank);
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to vote for a guildmember for leadership
        /// </summary>
        /// <param name="Member"></param>
        public virtual void SendUserCommandGuildVote(uint Member)
        {
            // create message instance
            UserCommand command = new UserCommandGuildVote(new ObjectID(Member));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Tries to invite your current target to your guild
        /// </summary>
        public virtual void SendUserCommandGuildInvite()
        {
            if (Data.TargetObject != null)
            {
                // create message instance
                UserCommand command = new UserCommandGuildVote(new ObjectID(Data.TargetObject.ID));
                UserCommandMessage message = new UserCommandMessage(command, null);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);
            }
        }

        /// <summary>
        /// Requests to set the guild password
        /// </summary>
        /// <param name="Password"></param>
        public virtual void SendUserCommandGuildSetPassword(string Password)
        {          
            // create message instance
            UserCommand command = new UserCommandGuildSetPassword(Password);
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);           
        }

        /// <summary>
        /// Requests to renounce your ties to your guild
        /// </summary>
        public virtual void SendUserCommandGuildRenounce()
        {
            // create message instance
            UserCommand command = new UserCommandGuildRenounce();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to disband your guild
        /// </summary>
        public virtual void SendUserCommandGuildDisband()
        {
            // create message instance
            UserCommand command = new UserCommandGuildDisband();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to make a guild your ally
        /// </summary>
        /// <param name="GuildID"></param>
        public virtual void SendUserCommandGuildMakeAlliance(uint GuildID)
        {
            // create message instance
            UserCommand command = new UserCommandGuildMakeAlliance(new ObjectID(GuildID));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to make a guild your enemy
        /// </summary>
        /// <param name="GuildID"></param>
        public virtual void SendUserCommandGuildMakeEnemy(uint GuildID)
        {
            // create message instance
            UserCommand command = new UserCommandGuildMakeEnemy(new ObjectID(GuildID));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to turn a guild from enemy to neutral
        /// </summary>
        /// <param name="GuildID"></param>
        public virtual void SendUserCommandGuildEndEnemy(uint GuildID)
        {
            // create message instance
            UserCommand command = new UserCommandGuildEndEnemy(new ObjectID(GuildID));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to turn a guild from ally to neutral
        /// </summary>
        /// <param name="GuildID"></param>
        public virtual void SendUserCommandGuildEndAlliance(uint GuildID)
        {
            // create message instance
            UserCommand command = new UserCommandGuildEndAlliance(new ObjectID(GuildID));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Tries to claim the guildshield as currently stored
        /// in DataController GuildShieldInfo or to retrieve just data about it.
        /// </summary>
        /// <param name="ReallyClaim">Set to false to retrieve data, true to claim it</param>
        public virtual void SendUserCommandClaimShield(bool ReallyClaim)
        {
            // create message instance
            UserCommand command = new UserCommandClaimShield(
                Data.GuildShieldInfo.Color1,
                Data.GuildShieldInfo.Color2,
                Data.GuildShieldInfo.Design,
                Convert.ToByte(ReallyClaim));

            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to create a guild with given info.
        /// </summary>
        /// <param name="GuildName"></param>
        /// <param name="Rank1Male"></param>
        /// <param name="Rank2Male"></param>
        /// <param name="Rank3Male"></param>
        /// <param name="Rank4Male"></param>
        /// <param name="Rank5Male"></param>
        /// <param name="Rank1Female"></param>
        /// <param name="Rank2Female"></param>
        /// <param name="Rank3Female"></param>
        /// <param name="Rank4Female"></param>
        /// <param name="Rank5Female"></param>
        /// <param name="SecredGuild"></param>
        public virtual void SendUserCommandGuildCreate(
            string GuildName,
            string Rank1Male, string Rank2Male, string Rank3Male, string Rank4Male, string Rank5Male, 
            string Rank1Female, string Rank2Female, string Rank3Female, string Rank4Female, string Rank5Female, 
            bool SecredGuild)
        {
            // create user command
            UserCommand command = new UserCommandGuildCreate(
                GuildName,
                Rank1Male, Rank2Male, Rank3Male, Rank4Male, Rank5Male, 
                Rank1Female, Rank2Female, Rank3Female, Rank4Female, Rank5Female, 
                SecredGuild);

            // create message
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to start resting
        /// </summary>
        public virtual void SendUserCommandRest()
        {
            if (GameTick.CanReqUserCommand())
            {
                // create message instance
                UserCommand command = new UserCommandRest();
                UserCommandMessage message = new UserCommandMessage(command, null);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // mark resting
                Data.IsResting = true;

                // save tick
                GameTick.DidReqUserCommand();
            }
        }

        /// <summary>
        /// Requests to stop resting
        /// </summary>
        public virtual void SendUserCommandStand()
        {
            if (GameTick.CanReqUserCommand())
            {
                // create message instance
                UserCommand command = new UserCommandStand();
                UserCommandMessage message = new UserCommandMessage(command, null);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // mark not resting anymore
                Data.IsResting = false;

                // save tick
                GameTick.DidReqUserCommand();
            }
        }

        /// <summary>
        /// Changes the URL in your avatar's details/description.
        /// </summary>
        /// <param name="URL"></param>
        public virtual void SendUserCommandChangeURL(string URL)
        {
            if (URL != null && ObjectID.IsValid(Data.AvatarID))
            {
                // create message instance
                UserCommand command = new UserCommandChangeURL(Data.AvatarID, URL);
                UserCommandMessage message = new UserCommandMessage(command, null);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);
            }
        }

        /// <summary>
        /// WARNING: This will delete your current avatar.
        /// </summary>
        public virtual void SendUserCommandSuicide()
        {         
            // create message instance
            UserCommand command = new UserCommandSuicide();
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);            
        }
#if !VANILLA
        /// <summary>
        /// Disables or enables the temporary angel after being murdered.
        /// </summary>
        public virtual void SendUserCommandTempSafe(bool On)
        {
            // create message instance
            UserCommand command = new UserCommandTempSafe(Convert.ToByte(On));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Disables or enables the grouping feature
        /// </summary>
        public virtual void SendUserCommandGrouping(bool On)
        {
            // create message instance
            UserCommand command = new UserCommandGrouping(Convert.ToByte(On));
            UserCommandMessage message = new UserCommandMessage(command, null);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to move inventoryitem 'FromID' right in front of 'ToID'.
        /// </summary>
        /// <param name="FromID">Item ID to move</param>
        /// <param name="ToID">Item ID where to move</param>
        public virtual void SendReqInventoryMoveMessage(uint FromID, uint ToID)
        {
            // create message instance
            ReqInventoryMoveMessage message = new ReqInventoryMoveMessage(
                FromID, ToID);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

#endif
        /// <summary>
        /// Sends a custom Action request
        /// </summary>
        /// <param name="Action"></param>
        public virtual void SendActionMessage(ActionType Action)
        {
            if (GameTick.CanReqAction())
            {
                // create message instance
                ActionMessage message = new ActionMessage(Action);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick
                GameTick.DidReqAction();
            }
        }
      
        /// <summary>
        /// Requests to hit your current target or highlighted target
        /// </summary>
        public virtual void SendReqAttackMessage()
        {
            // attack highlighted
            if (Data.IsNextAttackApplyCastOnHighlightedObject)
            {
                if (ObjectID.IsValid(Data.RoomObjects.HighlightedID))               
                    SendReqAttackMessage(Data.RoomObjects.HighlightedID);
                
                // unset nexttarget on highlight
                Data.IsNextAttackApplyCastOnHighlightedObject = false;
            }
           
            // attack current target
            else if (Data.TargetObject != null)
            {
                SendReqAttackMessage(Data.TargetObject.ID);
            }     
      
            // else attack attackable in front
            else
            {
                RoomObject obj = Data.ClosestAttackableInFront();

                if (obj != null)
                    SendReqAttackMessage(obj.ID);
            }
        }

        /// <summary>
        /// Requests to attack an object ID.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SendReqAttackMessage(uint ID)
        {
            // try get it from list
            RoomObject roomObject = Data.RoomObjects.GetItemByID(ID);
            
            // call other handler (handles null)
            SendReqAttackMessage(roomObject);
        }

        /// <summary>
        /// Requests to attack a RoomObject.
        /// </summary>
        /// <param name="Target">The object to attack.</param>
        public virtual void SendReqAttackMessage(RoomObject Target)
        {
            // src is our avatar
            RoomObject avatar = Data.AvatarObject;

            if (Target != null && avatar != null && GameTick.CanReqAttack())
            {                
                // verify the object is visible
                if (Target.IsVisibleFrom(avatar.Position3D, CurrentRoom))
                {
                    // create message instance
                    ReqAttackMessage message = new ReqAttackMessage(
                        ReqAttackMessage.ATTACK_NORMAL, Target.ID);

                    // send/enqueue it (async)
                    ServerConnection.SendQueue.Enqueue(message);

                    // save tick we last sent an update
                    GameTick.DidReqAttack();
                }
            }
        }

        /// <summary>
        /// Requests to Go somewhere (i.e. change room by door)
        /// </summary>
        /// <param name="SendPositionBefore"></param>
        public virtual void SendReqGo(bool SendPositionBefore = true)
        {
            if (SendPositionBefore)           
                SendReqMoveMessage(true);
                        
            // create message instance
            ReqGoMessage message = new ReqGoMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests a turn of an ID by given Angle.
        /// </summary>
        /// <param name="Angle">New angle in binary-measurement value (0 = UNIT_X, 0 = 0°, 4096 = 0°, counterclockwise)</param>
        /// <param name="ID">An object ID to turn, usually your avatar ID</param>
        public virtual void SendReqTurnMessage(ushort Angle, uint ID)
        {
            if (GameTick.CanReqTurn())
            {
                // create message instance
                ReqTurnMessage message = new ReqTurnMessage(Angle, ID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick we last sent an update
                GameTick.DidReqTurn();
            }
        }

        /// <summary>
        /// Requests a turn of your avatar to current datamodels
        /// orientation (angle).
        /// </summary>
        public virtual void SendReqTurnMessage()
        {
            if (Data.AvatarObject != null)
                SendReqTurnMessage(Data.AvatarObject.AngleUnits, Data.AvatarID);
        }

        /// <summary>
        /// Requests a move of your avatar
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Speed"></param>
        /// <param name="ForceSend"></param>
        public virtual void SendReqMoveMessage(ushort X, ushort Y, byte Speed, bool ForceSend = false)
        {
            if (Data.Effects.Paralyze.IsActive ||
                Data.IsResting ||
                Data.IsWaiting)
                return;

            // check for updaterate limit or override flag
            if ((ForceSend && GameTick.SpanReqMove > 0) || GameTick.CanReqMove())
            {
                // create message instance
                ReqMoveMessage message = new ReqMoveMessage(
                    X, Y, (MovementSpeed)Speed, Data.RoomInformation.RoomID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick we last sent an update
                GameTick.DidReqMove();
            }
        }

        /// <summary>
        /// Requests a move of your avatar
        /// to current datamodels coordinates and speed.
        /// </summary>
        /// <param name="ForceSend">Ignores the update-span, but no other conditions.</param>
        public virtual void SendReqMoveMessage(bool ForceSend = false)
        {
            // must have an avatar object
            if (Data.AvatarObject == null)
                return;

            // use horizontalspeed or default to RUN for TELEPORT (0)
            byte speed = ((byte)Data.AvatarObject.HorizontalSpeed == (byte)MovementSpeed.Teleport) ?
                (byte)MovementSpeed.Run : (byte)Data.AvatarObject.HorizontalSpeed;

            // use the generic variant with our updated values in datalayer
            SendReqMoveMessage(
                Data.AvatarObject.CoordinateX,
                Data.AvatarObject.CoordinateY,
                speed,
                ForceSend);           
        }

        /// <summary>
        /// Requests additional information about the current target object 
        /// (i.e. item or character popup)
        /// </summary>
        public virtual void SendReqLookMessage()
        {
            SendReqLookMessage(Data.TargetID);            
        }

        /// <summary>
        /// Requests additional information about the given ID
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SendReqLookMessage(uint ID)
        {
            if (ObjectID.IsValid(ID) && GameTick.CanInteract(ID))
            {
                // create message instance
                ReqLookMessage message = new ReqLookMessage(new ObjectID(ID));

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                GameTick.DidInteract(ID);
            }
        }

        /// <summary>
        /// Requests to cast a spell on your current targets,
        /// or on yourself depending on datacontroller values.
        /// </summary>
        /// <param name="SpellID">The SpellID to cast</param>
        public virtual void SendReqCastMessage(uint SpellID)
        {
            // try get spellobject for ID
			SpellObject spellObject =
                Data.SpellObjects.GetItemByID(SpellID);

            if (spellObject != null)
                SendReqCastMessage(spellObject);
        }

        /// <summary>
        /// Requests to cast a spell on your current targets,
        /// or on yourself depending on datacontroller values.
        /// </summary>
        /// <param name="Spell">The SpellObject to cast</param>
        public virtual void SendReqCastMessage(SpellObject Spell)
        {
            ObjectBase[] targetIDs = null;

            // if the spell requires (at least) one target
            if (Spell.TargetsCount > 0)
            {               
                // target: highlighted
                if (Data.IsNextAttackApplyCastOnHighlightedObject)
                {
                    targetIDs = new ObjectBase[1];
                    targetIDs[0] = Data.RoomObjects.GetHighlightedItem();
                }

                // target: self
                else if (Data.SelfTarget)
                {
                    targetIDs = new ObjectBase[1];
                    targetIDs[0] = Data.AvatarObject;
                }

                // target: target (default)
                else if (Data.TargetObject != null)
                {
                    targetIDs = new ObjectBase[1];
                    targetIDs[0] = Data.TargetObject;
                }
            }

            // room enchantments for example don't require a target
            else          
                targetIDs = new ObjectBase[0];

            // if we have proper target(s) for this spell
            if (targetIDs != null)
            {
                if (GameTick.CanReqCast())
                {
                    // send by default
                    bool send = true;

                    // if no non-target spell, verify view for roomobjects
                    if (targetIDs.Length > 0)
                    {
                        // src is our avatar
                        RoomObject src = Data.AvatarObject;

                        if (targetIDs[0] != null)
                        {
                            // verify the object is visible
                            if (targetIDs[0] is RoomObject)
                                send = ((RoomObject)targetIDs[0]).IsVisibleFrom(src.Position3D, CurrentRoom);
                            
                            // other objects are "always visible", e.g. inventory
                            else
                                send = true;
                        }
                        else 
                            send = false;          
                    }

                    // send or not to send
                    if (send)
                    {
                        // create message instance
                        ReqCastMessage message = new ReqCastMessage(Spell.ID, targetIDs);

                        // send/enqueue it (async)
                        ServerConnection.SendQueue.Enqueue(message);

                        // save tick we last sent an update
                        GameTick.DidReqCast();
                    }
                }
            }
            else
            {
                // don't spam feedback so
                // create a delay like a cast
                if (GameTick.CanReqCast())
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "This spell requires a target."));

                    GameTick.DidReqCast();
                }
            }

            // reset highlighted attack marker
            if (Data.IsNextAttackApplyCastOnHighlightedObject)
                Data.IsNextAttackApplyCastOnHighlightedObject = false;
        }
        
        /// <summary>
        /// Sends chatmessage as say, yell, broadcast, ...
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Text"></param>
        public virtual void SendSayToMessage(ChatTransmissionType Type, string Text)
        {
            // don't try to send admincommands if not admin
            // this gets logged, be warned :)         
            if (Type == ChatTransmissionType.DM && Data.AccountType != AccountType.ADMIN)
                return;

            // get tick yesno
            bool canDo = true;
            switch (Type)
            {
                case ChatTransmissionType.Normal:
                    canDo = GameTick.CanSay();
                    break;

                case ChatTransmissionType.Everyone:
                    canDo = GameTick.CanBroadcast();
                    break;
            }

            if (canDo)
            {
                // create message instance
                SayToMessage message = new SayToMessage(Type, Text);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                switch (Type)
                {
                    case ChatTransmissionType.Normal:
                        GameTick.DidSay();
                        break;

                    case ChatTransmissionType.Everyone:
                        GameTick.DidBroadcast();
                        break;
                }
            }
        }

        /// <summary>
        /// Sends a chatmessage to the given object IDs (group).
        /// </summary>
        /// <param name="TargetIDs"></param>
        /// <param name="Text"></param>
        public virtual void SendSayGroupMessage(uint[] TargetIDs, string Text)
        {
            // create message instance
            SayGroupMessage message = new SayGroupMessage(TargetIDs, Text);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Sends a chatmessage to the given object ID (tell).
        /// </summary>
        /// <param name="TargetID"></param>
        /// <param name="Text"></param>
        public virtual void SendSayGroupMessage(uint TargetID, string Text)
        {
            SendSayGroupMessage(new uint[] { TargetID }, Text);
        }

        /// <summary>
        /// Requests to use (equip) an inventory item.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SendReqUseMessage(uint ID)
        {
            if (ObjectID.IsValid(ID) && GameTick.CanInteract(ID))
            {
                // create message instance
                ReqUseMessage message = new ReqUseMessage(ID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick
                GameTick.DidInteract(ID);
            }
        }

        /// <summary>
        /// Requests to apply an inventory item with given ID on one of these 
        /// automatically selected targets.
        /// 1) Highlighted
        /// 2) Yourself
        /// 3) Current Target
        /// </summary>
        /// <param name="ID">ID of inventory object to</param>
        public virtual void SendReqApply(uint ID)
        {           
            // default invalid targetid
            uint targetID = UInt32.MaxValue;

            // target: highlighted
            if (Data.IsNextAttackApplyCastOnHighlightedObject)
            {
                targetID = Data.RoomObjects.HighlightedID;
                Data.IsNextAttackApplyCastOnHighlightedObject = false;
            }

            // target: self
            else if (Data.SelfTarget)
            {
                targetID = Data.AvatarID;
            }

            // target: target (default)
            else
            {
                targetID = Data.TargetID;
            }

            // verify IDs and delay
            if (ObjectID.IsValid(ID) && ObjectID.IsValid(targetID) && GameTick.CanInteract(ID))
            {                
                // create message instance
                ReqApplyMessage message = new ReqApplyMessage(ID, targetID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                GameTick.DidInteract(ID);
            }
        }

        /// <summary>
        /// Requests to activate your current target.
        /// </summary>
        public virtual void SendReqActivate()
        {
            SendReqActivate(Data.TargetID);
        }

        /// <summary>
        /// Requests to activate an object with given ID.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SendReqActivate(uint ID)
        {
            if (ObjectID.IsValid(ID) && GameTick.CanInteract(ID))
            {
                // create message instance
                ReqActivateMessage message = new ReqActivateMessage(ID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick
                GameTick.DidInteract(ID);
            }
        }

        /// <summary>
        /// Requests to unuse (unequip) an inventory item.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SendReqUnuseMessage(uint ID)
        {
            if (ObjectID.IsValid(ID) && GameTick.CanInteract(ID))
            {
                // create message instance
                ReqUnuseMessage message = new ReqUnuseMessage(ID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick
                GameTick.DidInteract(ID);
            }
        }

        /// <summary>
        /// Requests to drop an inventory item.
        /// </summary>
        /// <param name="ID"></param>
        public virtual void SendReqDropMessage(ObjectID ID)
        {
            if (GameTick.CanInteract(ID.ID))
            {
                // create message instance
                ReqDropMessage message = new ReqDropMessage(ID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // save tick
                GameTick.DidInteract(ID.ID);
            }
        }

        /// <summary>
        /// Requests the buy menu from a NPC (current target)
        /// </summary>
        public virtual void SendReqBuyMessage()
        {
            if (ObjectID.IsValid(Data.TargetID) &&
                GameTick.CanInteract(Data.TargetID))
            {            
                // create message instance
                ReqBuyMessage message = new ReqBuyMessage(Data.TargetID);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                GameTick.DidInteract(Data.TargetID);
            }
        }

        /// <summary>
        /// Requests to buy Items from Target ID
        /// </summary>
        /// <param name="TargetID"></param>
        /// <param name="Items"></param>
        public virtual void SendReqBuyItemsMessage(uint TargetID, ObjectID[] Items)
        {
            if (ObjectID.IsValid(TargetID))
            {
                // create message instance
                ReqBuyItemsMessage message = new ReqBuyItemsMessage(TargetID, Items);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);
            }
        }

        /// <summary>
        /// Requests to loot/get an item
        /// </summary>
        /// <param name="Target"></param>
        public virtual void SendReqGetMessage(ObjectID Target)
        {
            if (GameTick.CanInteract(Target.ID))
            {
                // create message instance
                ReqGetMessage message = new ReqGetMessage(Target);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                GameTick.DidInteract(Target.ID);
            }
        }

        /// <summary>
        /// Requests to get/loot your current target object
        /// </summary>
        public virtual void SendReqGetMessage()
        {
            if (ObjectID.IsValid(Data.TargetID))
                SendReqGetMessage(new ObjectID(Data.TargetID));
        }

        /// <summary>
        /// Requests admin access in login protocol mode (AP_REQ_ADMIN)
        /// </summary>
        public virtual void SendReqAdminMessageLoginMode()
        {
            // create message instance
            ReqAdminMessageLoginMode message = new ReqAdminMessageLoginMode();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to execute an admin command. (BP_REQ_ADMIN).
        /// This is strictly for admin console command.
        /// See SendReqDM() and SendSayTo() for others.
        /// </summary>
        public virtual void SendReqAdminMessage(string Text)
        {
            // we don't want to trigger ALERTS on the server
            if (Data.AccountType != AccountType.ADMIN)
                return;

            // create message instance
            ReqAdminMessage message = new ReqAdminMessage(Text);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the articles of Newsglobe given by ID
        /// </summary>
        /// <param name="NewsGroupID"></param>
        public virtual void SendReqArticles(ushort NewsGroupID)
        {
            // create message instance
            ReqArticlesMessage message = new ReqArticlesMessage(NewsGroupID);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the articles of last look newsglobe
        /// </summary>
        public virtual void SendReqArticles()
        {
            // clear old ones
            Data.NewsGroup.Articles.Clear();

            // create message instance
            ReqArticlesMessage message = new ReqArticlesMessage(Data.NewsGroup.NewsGlobeID);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests an article with num from globe with ID
        /// </summary>
        public virtual void SendReqArticle(uint GlobeID, uint ArticleNum)
        {
            // create message instance
            ReqArticleMessage message = new ReqArticleMessage(
                Data.NewsGroup.NewsGlobeID, ArticleNum);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests an article with num from last look newsglobe
        /// </summary>
        public virtual void SendReqArticle(uint ArticleNum)
        {
            SendReqArticle(Data.NewsGroup.NewsGlobeID, ArticleNum);
        }

        /// <summary>
        /// Requests to post an article in a newsgroup (globe).
        /// </summary>
        /// <param name="GlobeID"></param>
        /// <param name="Title"></param>
        /// <param name="Text"></param>
        public virtual void SendPostArticle(ushort GlobeID, string Title, string Text)
        {
            // create message instance           
            PostArticleMessage message = new PostArticleMessage(
                GlobeID, Title, Text);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Offers a set of items to an object as trade initiation
        /// </summary>
        public virtual void SendReqOffer(ObjectID TradePartner, ObjectID[] OfferItems)
        {
            if (TradePartner != null && OfferItems != null)
            {
                // clone IDs for the case they are deriving classes
                // and trigger wrong WriteTo serializer

                ObjectID id = new ObjectID(TradePartner.ID);
                ObjectID[] items = new ObjectID[OfferItems.Length];

                for (int i = 0; i < OfferItems.Length; i++)
                    items[i] = new ObjectID(OfferItems[i].ID, OfferItems[i].Count);

                // create message instance
                ReqOfferMessage message = new ReqOfferMessage(id, items);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);

                // clear own items (will be echoed back)
                Data.Trade.ItemsYou.Clear();

                // hide it at that point, server possibly echos back our offer
                // do NOT clear here because of TradePartner set earlier
                Data.Trade.IsVisible = false;
            }
        }

        /// <summary>
        /// Requests to deposit items into an holder.
        /// </summary>
        /// <param name="Holder"></param>
        /// <param name="Items"></param>
        public virtual void SendReqDeposit(ObjectID Holder, ObjectID[] Items)
        {
            // create message instance
            ReqDepositMessage message = new ReqDepositMessage(Holder, Items);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Offers a set of items as respone to a trade initiated by other party.
        /// </summary>
        public virtual void SendReqCounterOffer(ObjectID[] OfferItems)
        {
            if (OfferItems != null)
            {
                // clone IDs for the case they are deriving classes
                // and trigger wrong WriteTo serializer

                ObjectID[] items = new ObjectID[OfferItems.Length];

                for (int i = 0; i < OfferItems.Length; i++)
                    items[i] = new ObjectID(OfferItems[i].ID, OfferItems[i].Count);

                // create message instance
                ReqCounterOfferMessage message = new ReqCounterOfferMessage(items);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);
            }
        }

#if !VANILLA
        /// <summary>
        /// Sends modified stat values from datalayer to the server, requesting a stat change.
        /// </summary>
        public virtual void SendChangedStatsMessage()
        {
            // get (copy) of currently selected data from datalayer
            StatChangeInfo info = new StatChangeInfo();
            info.UpdateFromModel(Data.StatChangeInfo, false);

            // create message instance
            ChangedStatsMessage message = new ChangedStatsMessage(info);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }
#endif

        /// <summary>
        /// Accepts a pending trade/offer.
        /// This also clears the Trade in the DataController afterwards.
        /// </summary>
        public virtual void SendAcceptOffer()
        {
            // create message instance
            AcceptOfferMessage message = new AcceptOfferMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);

            // clear trade
            Data.Trade.Clear(true);
        }

        /// <summary>
        /// Rejects a pending trade/offer.
        /// This also clears the Trade in DataController afterwards.
        /// </summary>
        public virtual void SendCancelOffer()
        {
            // create message instance
            CancelOfferMessage message = new CancelOfferMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);

            // clear offered items
            Data.Trade.Clear(true);
        }

        /// <summary>
        /// Request the basic room/player info
        /// </summary>
        public virtual void SendSendPlayer()
        {
            // create message instance
            SendPlayerMessage message = new SendPlayerMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Request the room contents
        /// </summary>
        public virtual void SendSendRoomContents()
        {
            // create message instance
            SendRoomContentsMessage message = new SendRoomContentsMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the contents of an object
        /// </summary>
        public virtual void SendSendObjectContents(uint ID)
        {
            // create message instance
            SendObjectContentsMessage message = new SendObjectContentsMessage(ID);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the contents of your current target ID
        /// </summary>
        public virtual void SendSendObjectContents()
        {
            if (!ObjectID.IsValid(Data.TargetID))
                return;
            
            SendSendObjectContents(Data.TargetID);
        }

        /// <summary>
        /// Requests to put 'Item' into 'Target'
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Target"></param>
        public virtual void SendReqPut(ObjectID Item, ObjectID Target)
        {
            // create message instance
            ReqPutMessage message = new ReqPutMessage(Item, Target);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Request the list of online players
        /// </summary>
        public virtual void SendSendPlayers()
        {
            // create message instance
            SendPlayersMessage message = new SendPlayersMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Sends a ReqDM message with commands: go, goplayer, getplayer.
        /// For admin-console commands see SendReqAdmin()
        /// For "dm" commands see SendSayTo()
        /// </summary>
        public virtual void SendReqDM(DMCommandType Type, string Text)
        {
            // don't try to send admincommands if not admin
            // this gets logged, be warned :)

            if (Data.AccountType == AccountType.ADMIN)
            {
                // create message instance
                ReqDMMessage message = new ReqDMMessage(Type, Text);
                
                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);
            }
        }

        /// <summary>
        /// Changes your avatar's description in your details window.
        /// </summary>
        /// <param name="Description"></param>
        public virtual void SendChangeDescription(string Description)
        {
            if (ObjectID.IsValid(Data.AvatarID))
            {
                // create message instance
                ChangeDescriptionMessage message = new ChangeDescriptionMessage(
                    new ObjectID(Data.AvatarID), Description);

                // send/enqueue it (async)
                ServerConnection.SendQueue.Enqueue(message);
            }
        }

        /// <summary>
        /// Requests the avatar creation wizard info
        /// </summary>
        /// <param name="SlotID">
        /// Optional and not transfered to the server here.
        /// If set it will update DataController.CharCreationInfo.SlotID
        /// </param>
        public virtual void SendSystemMessageSendCharInfo(uint SlotID = 0)
        {
            // create message instance
            SystemMessage message = new SystemMessage(
                new SubMessageSendCharInfo());

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);

            // update the slotID
            Data.CharCreationInfo.SlotID = SlotID;
        }

        /// <summary>
        /// Requests to create an avatar with the data from
        /// DataController.CharCreationInfo.
        /// </summary>
        public virtual void SendSystemMessageNewCharInfo()
        {
            // create message instance
            SystemMessage message = new SystemMessage(
                new SubMessageNewCharInfo(
                    new ObjectID(Data.CharCreationInfo.SlotID), 
                    Data.CharCreationInfo.AvatarName, 
                    Data.CharCreationInfo.AvatarDescription, 
                    (byte)Data.CharCreationInfo.Gender, 
                    Data.CharCreationInfo.GetSelectedResourceIDs(), 
                    Data.CharCreationInfo.HairColor, 
                    Data.CharCreationInfo.SkinColor, 
                    Data.CharCreationInfo.GetAttributesArray(),
                    Data.CharCreationInfo.GetSelectedSpellIDs(),
                    Data.CharCreationInfo.GetSelectedSkillIDs()
                    ));

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SendReqQuit()
        {
            // create message instance
            ReqQuitMessage message = new ReqQuitMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to delete the mail with Num on server.
        /// </summary>
        /// <param name="Num"></param>
        public virtual void SendDeleteMail(uint Num)
        {
            // create message instance
            DeleteMailMessage message = new DeleteMailMessage(Num);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to download all mails from the server.
        /// </summary>
        public virtual void SendReqGetMail()
        {
            // create message instance
            ReqGetMailMessage message = new ReqGetMailMessage();

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests the current object IDs for players with given names.
        /// Server will respond a "LookupNames" message result.
        /// </summary>
        /// <param name="Names"></param>
        public virtual void SendReqLookupNames(string[] Names)
        {
            // create message instance
            ReqLookupNamesMessage message = new ReqLookupNamesMessage(Names);
            
            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        /// <summary>
        /// Requests to send a mail with given Subject and Text to Recipients.
        /// </summary>
        /// <param name="RecipientIDs"></param>
        /// <param name="Subject"></param>
        /// <param name="Text"></param>
        public virtual void SendSendMail(ObjectID[] RecipientIDs, string Subject, string Text)
        {
            // combine subject and text..
            string str = "Subject: " + Subject + "\n" + Text;

            // create message instance
            SendMailMessage message = new SendMailMessage(RecipientIDs, str);

            // send/enqueue it (async)
            ServerConnection.SendQueue.Enqueue(message);
        }

        #endregion

        #region Others

        /// <summary>
        /// Requests basic info not automatically transmitted after login
        /// </summary>
        protected void RequestInfoAfterLogin()
        {
            // request stats
            SendSendStatsMessage(StatGroup.Condition);
            SendSendStatsMessage(StatGroup.Attributes);
            SendSendStatsMessage(StatGroup.Skills);
            SendSendStatsMessage(StatGroup.Spells);
#if !VANILLA
            SendSendStatsMessage(StatGroup.Quests);
#endif

            // request inventory
            SendReqInventoryMessage();

            // request spell & skillobjects
            SendSendSpellsMessage();
            SendSendSkillsMessage();

            // request buff states
            SendSendEnchantmentsMessage(BuffType.AvatarBuff);            
        }

        /// <summary>
        /// Requests all invalidated info after server save
        /// </summary>
        protected void RequestInfoAfterServerSave()
        {
            // request online players
            SendSendPlayers();

            // request room/player info
            SendSendPlayer();

            // request room contents
            SendSendRoomContents();

            // request to stand
            SendUserCommandStand();

            // request the default set after login
            RequestInfoAfterLogin();
        }

        /// <summary>
        /// Tries to move your avatar in the given 2D direction on the ground.
        /// Call this method each gametick (=threadloop) you want to process a movement.
        /// This honors map collisions and object collisions.
        /// </summary>
        /// <param name="Direction">Direction vector, gets normalized</param>
        /// <param name="Speed">The speed of the movement.</param>
        /// <param name="PlayerHeight">Height of the player for ceiling collisions (in ROO scale!)</param>
        public void TryMove(V2 Direction, byte Speed, Real PlayerHeight)
        {
            // no movements when resting or paralyzed
            if (!Data.IsResting &&
                !Data.Effects.Paralyze.IsActive &&
                CurrentRoom != null)
            {
                // slow down movements to walkspeed if not enough vigor
                if (Data.VigorPoints < StatNumsValues.LOWVIGOR && Speed > (byte)MovementSpeed.Walk)
                    Speed = (byte)MovementSpeed.Walk;
                
                // avatar we're controlling
                RoomObject avatar = Data.AvatarObject;

                // normalize direction
                Direction.Normalize();

                // start is the current avatar position
                V3 start = avatar.Position3D;
                V2 start2D = avatar.Position2D;

                // step based on direction and tick delta
                V2 step = Direction * (Real)Speed * (Real)GameTick.Span * GeometryConstants.MOVEBASECOEFF;

                // slow down movements sectors with depth modifiers
                if (avatar.SubSector != null && avatar.SubSector.Sector != null)
                {
                    switch(avatar.SubSector.Sector.Flags.SectorDepth)
                    {
                        case RooSectorFlags.DepthType.Depth0: break;
                        case RooSectorFlags.DepthType.Depth1: step.Scale(0.75f); break;
                        case RooSectorFlags.DepthType.Depth2: step.Scale(0.5f);  break;
                        case RooSectorFlags.DepthType.Depth3: step.Scale(0.25f); break;
                    }
                }

                // apply step on start ("end candidate")
                V2 end = start2D + step;
                
                //// 1. VERIFY OBJECT COLLISION

                // check against roomnodes which have nomoveon set
                foreach (RoomObject obj in Data.RoomObjects)
                {
                    // check if object should create collision
                    if (!obj.IsAvatar &&
                        obj.Flags.MoveOn == ObjectFlags.MoveOnType.No)
                    {
                        V2 diff = obj.Position2D - start2D;

                        // care only about movements from outside to inside of blocking circle
                        if (diff.LengthSquared > GeometryConstants.MIN_NOMOVEON2)
                        {
                            V2 intersect;

                            // if intersection exists
                            if (MathUtil.IntersectLineCircle(
                                    start2D,
                                    end,
                                    obj.Position2D,
                                    GeometryConstants.MIN_NOMOVEON, 
                                    out intersect))
                            {
                                // get a perpendicular vector
                                V2 perp = (obj.Position2D - intersect).GetPerpendicular1();

                                // project step on perpendicular vector ("slide along")
                                step = step.GetProjection(perp);

                                // update end
                                end = start2D + step;

                                // break here, only one collision!
                                // technically this allows sliding into another object
                                // but proceeding here might allow sliding back into first object as well
                                break;
                            }
                        }
                    }
                }

                //// 2. VERIFY ROOM COLLISION
                
                // roo format variants
                V3 rooStart = start.Clone();
                V2 rooEnd = end.Clone();

                // convert to ROO format
                rooStart.ConvertToROO();
                rooEnd.ConvertToROO();

                // check against roowalls (this can modify step)
                step = CurrentRoom.VerifyMove(rooStart, rooEnd, PlayerHeight);

                // convert back to worldsize
                step.Scale(0.0625f);

                //// 3. APPLY MOVE
               
                // destination (might be the old)
                end = start2D + step;

                // start movement
                avatar.StartMoveTo(end, Speed);
            }
        }

        /// <summary>
        /// Tries to yaw your avatar by given radian angle.
        /// Negative and Positive gives direction.
        /// </summary>
        /// <param name="Radian"></param>
        /// <returns>Whether the yaw was successful or not</returns>
        public bool TryYaw(Real Radian)
        {
            RoomObject avatar = Data.AvatarObject;

            if (avatar != null && !Data.Effects.Paralyze.IsActive)
            {
                if (Radian > 0)
                    avatar.Angle += Math.Abs(Radian);
                
                else
                    avatar.Angle -= Math.Abs(Radian);

                // possibly send update
                SendReqTurnMessage();

                return true;
            }

            return false;
        }

        /// <summary>
        /// This loots all objects in range.
        /// </summary>
        public void LootAll()
        {
            RooFile room = Data.RoomInformation.ResourceRoom;
            RoomObject avatar = Data.AvatarObject;

            if (room != null && avatar != null)
            {
                // get visible objects within distances
                List<RoomObject> candidates = avatar.GetObjectsWithinDistance(
                    Data.RoomObjects, 
                    room, 
                    GeometryConstants.CLOSEDISTANCE, 
                    GeometryConstants.CLOSEDISTANCE, 
                    false);

                foreach (RoomObject obj in candidates)
                {
                    if (obj.Flags.IsGettable)
                    {
                        SendReqGetMessage(new ObjectID(obj.ID));
                    }
                }
            }
        }

        /// <summary>
        /// Uses, unuses or applies an item based on its flags/inuse state.
        /// </summary>
        /// <param name="Item"></param>
        public void UseUnuseApply(InventoryObject Item)
        {
            // check if the item needs a target to apply on
            if (Item.Flags.IsApplyable)
            {
                SendReqApply(Item.ID);
            }
            else
            {
                // usable targets
                if (!Item.IsInUse)
                {
                    SendReqUseMessage(Item.ID);
                }
                else
                {
                    SendReqUnuseMessage(Item.ID);
                }
            }
        }

        /// <summary>
        /// Try to execute a known command from chat/textinput
        /// </summary>
        /// <param name="Text"></param>
        public void ExecChatCommand(string Text)
        {
            if (Text == null || String.Equals(Text, String.Empty))
                return;

            // log all kind of chatcommands, even the malformed
            Data.ChatCommandHistoryAdd(Text);

            // parse chatcommand
            ChatCommand chatCommand = ChatCommand.Parse(Text, Data, Config);

            // handle chatcommand
            if (chatCommand != null)
            {
                switch (chatCommand.CommandType)
                {
                    case ChatCommandType.Say:
                        ChatCommandSay chatCommandSay = (ChatCommandSay)chatCommand;
                        SendSayToMessage(ChatTransmissionType.Normal, chatCommandSay.Text);
                        break;

                    case ChatCommandType.Emote:
                        ChatCommandEmote chatCommandEmote = (ChatCommandEmote)chatCommand;
                        SendSayToMessage(ChatTransmissionType.Emote, chatCommandEmote.Text);
                        break;

                    case ChatCommandType.Yell:
                        ChatCommandYell chatCommandYell = (ChatCommandYell)chatCommand;
                        SendSayToMessage(ChatTransmissionType.Yell, chatCommandYell.Text);
                        break;

                    case ChatCommandType.Broadcast:
                        ChatCommandBroadcast chatCommandBroadcast = (ChatCommandBroadcast)chatCommand;
                        SendSayToMessage(ChatTransmissionType.Everyone, chatCommandBroadcast.Text);
                        break;

                    case ChatCommandType.Guild:
                        ChatCommandGuild chatCommandGuild = (ChatCommandGuild)chatCommand;
                        SendSayToMessage(ChatTransmissionType.Guild, chatCommandGuild.Text);
                        break;

                    case ChatCommandType.Tell:
                        ChatCommandTell chatCommandTell = (ChatCommandTell)chatCommand;
                        SendSayGroupMessage(new uint[] { chatCommandTell.TargetID }, chatCommandTell.Text);
                        break;

                    case ChatCommandType.Cast:
                        ChatCommandCast chatCommandCast = (ChatCommandCast)chatCommand;
                        SendReqCastMessage(chatCommandCast.Spell);
                        break;

                    case ChatCommandType.Deposit:
                        ChatCommandDeposit chatCommandDeposit = (ChatCommandDeposit)chatCommand;
                        SendUserCommandDeposit(chatCommandDeposit.Amount);
                        break;

                    case ChatCommandType.WithDraw:
                        ChatCommandWithDraw chatCommandWithDraw = (ChatCommandWithDraw)chatCommand;
                        SendUserCommandWithDraw(chatCommandWithDraw.Amount);
                        break;

                    case ChatCommandType.Suicide:
                        ChatCommandSuicide chatCommandSuicide = (ChatCommandSuicide)chatCommand;
                        
                        // execute virtual function, does nothing in this class
                        Suicide();
                        break;

                    case ChatCommandType.DM:
                        ChatCommandDM chatCommandDM = (ChatCommandDM)chatCommand;
                        SendSayToMessage(ChatTransmissionType.DM, chatCommandDM.Text);
                        break;

                    case ChatCommandType.Go:
                        ChatCommandGo chatCommandGo = (ChatCommandGo)chatCommand;
                        SendReqDM(DMCommandType.GoRoom, chatCommandGo.Text);
                        break;

                    case ChatCommandType.GoPlayer:
                        ChatCommandGoPlayer chatCommandGoPlayer = (ChatCommandGoPlayer)chatCommand;
                        SendReqDM(DMCommandType.GoPlayer, chatCommandGoPlayer.ID.ToString());
                        break;

                    case ChatCommandType.GetPlayer:
                        ChatCommandGetPlayer chatCommandGetPlayer = (ChatCommandGetPlayer)chatCommand;
                        SendReqDM(DMCommandType.GetPlayer, chatCommandGetPlayer.ID.ToString());
                        break;

                    case ChatCommandType.Rest:                        
                        SendUserCommandRest();
                        break;

                    case ChatCommandType.Stand:
                        SendUserCommandStand();
                        break;

                    case ChatCommandType.Quit:
                        SendReqQuit();
                        break;
#if !VANILLA
                    case ChatCommandType.TempSafe:
                        SendUserCommandTempSafe(((ChatCommandTempSafe)chatCommand).On);
                        break;

                    case ChatCommandType.Grouping:
                        SendUserCommandGrouping(((ChatCommandGrouping)chatCommand).On);
                        break;
#endif
                }
            }    
        }

        /// <summary>
        /// Try to execute a defined action.
        /// </summary>
        /// <param name="Action"></param>
        public void ExecAction(AvatarAction Action)
        {
            switch (Action)
            {
                case AvatarAction.Attack:
                    SendReqAttackMessage();
                    break;

                case AvatarAction.Rest:
                    // if not resting, start resting
                    if (!Data.IsResting)
                    {
                        // send command
                        SendUserCommandRest();
                    }
                    // else stop resting
                    else
                    {
                        // send command
                        SendUserCommandStand();
                    }
                    break;

                case AvatarAction.Dance:
                    SendActionMessage(ActionType.Dance);
                    break;

                case AvatarAction.Wave:
                    SendActionMessage(ActionType.Wave);
                    break;

                case AvatarAction.Point:
                    SendActionMessage(ActionType.Point);
                    break;

                case AvatarAction.Loot:
                    // loot all on no target
                    if (Data.TargetID == 0 ||
                        Data.TargetID > ObjectID.UINT28MAX)
                    {
                        LootAll();
                    }
                    else if (Data.SelfTarget)
                    {
                        // not impl: show loot list
                    }
                    else if (Data.TargetID > 0)
                        SendReqGetMessage();
                    break;

                case AvatarAction.Buy:
                    SendReqBuyMessage();
                    break;

                case AvatarAction.Inspect:
                    SendReqLookMessage();
                    break;

                case AvatarAction.Activate:
                    // for objects set with cointaier flag
                    if (Data.TargetObject != null)
                    {
                        if (Data.TargetObject.Flags.IsContainer)                           
                            SendSendObjectContents();
                        
                        else 
                            SendReqActivate();
                    }
                   
                    break;

                case AvatarAction.Trade:
                    if (Data.TargetObject != null || Data.Trade.IsBackgroundOffer)
                    {
                        if (!Data.Trade.IsBackgroundOffer)
                            Data.Trade.TradePartner = Data.TargetObject;

                        Data.Trade.IsVisible = true;
                    }
                    break;

                case AvatarAction.GuildInvite:
                    SendUserCommandGuildInvite();
                    break;
            }
        }

        /// <summary>
        /// By default this function does nothing.
        /// It's executed by 'Suicide' ChatCommand and must be overwritten with code calling
        /// SendUserCommandSuicide() to actually perform the suicide.
        /// </summary>
        protected virtual void Suicide()
        {
        }           
        #endregion
    }
}
