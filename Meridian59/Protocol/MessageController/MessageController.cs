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
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.Structs;
using Meridian59.Data.Models;
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Implements the basic tasks: Packet-Splitting, PI-Decoding, CRC-Creation
    /// </summary>
    public abstract class MessageController
    {     
        #region Events
        
        /// <summary>
        /// Raised when there is a new message available for processing
        /// </summary>
        public event GameMessageEventHandler MessageAvailable;

        /// <summary>
        /// Raised when the parser has found an incomplete message in tcp packet
        /// </summary>
        public event SplittedMessageFoundEventHandler SplittedMessageFound;

        /// <summary>
        /// Raised when parser is completing a splitted message
        /// </summary>
        public event CompletingSplittedMessageEventHandler CompletingSplittedMessage;
        
        /// <summary>
        /// Raised when an header contained two mismatching body lengths
        /// </summary>
        public event MismatchMessageLengthFoundEventHandler MismatchMessageLengthFound;
        
        /// <summary>
        /// Raised when an header only message was received
        /// </summary>
        public event EmptyMessageFoundEventHandler EmptyMessageFound;
        
        /// <summary>
        /// Raised whenever the ServerSave value in the header has changed
        /// </summary>
        public event ServerSaveChangedEventHandler ServerSaveChanged;
        
        /// <summary>
        /// Raised when an error has occured
        /// </summary>
        public event HandlerErrorEventHandler HandlerError;
        
        /// <summary>
        /// Raised when the protocol mode changes
        /// </summary>
        public event EventHandler ProtocolModeChanged;
        #endregion

        #region Fields
        /// <summary>
        /// Reference to a populated dictionary with game strings.
        /// </summary>
        protected StringDictionary stringResources;
        #endregion

        #region Properties
        /// <summary>
        /// Current ServerSave (header value)
        /// </summary>
        public byte CurrentServerSave { get; protected set; }

        /// <summary>
        /// Last ServerSave
        /// </summary>
        public byte LastServerSave { get; protected set; }

        /// <summary>
        /// The current protocol mode
        /// </summary>
        public ProtocolMode Mode { get; protected set; }

        /// <summary>
        /// Instance of a PI decoder used to decode PacketIdentifiers
        /// </summary>
        public PIDecoder PIDecoder { get; protected set; }

        /// <summary>
        /// Instance of a CRCreator used to encrypt outgoing Packet-CRCs
        /// </summary>
        public CRCCreator CRCCreator { get; protected set; }
   
        /// <summary>
        /// Whether the encrypted CRC generator is enabled
        /// </summary>
        public bool CRCCreatorEnabled { get; protected set; }

        /// <summary>
        /// The current HashTable used by the CRC Creator
        /// </summary>
        public HashTable CRCCreatorHashTable 
        { 
            get { return CRCCreator.CurrentHashTable; } 
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StringResources">A dictionary to resolve Meridian59 strings from.</param>
		public MessageController(StringDictionary StringResources)
        {
            // save string dictionary
            this.stringResources = StringResources;

            // reset
            Reset();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Resets the PacketController to initial state
        /// </summary>
        public virtual void Reset()
        {
            CurrentServerSave = 0x00;
            LastServerSave = 0x00;
            PIDecoder = new PIDecoder(stringResources);
            CRCCreator = new CRCCreator();
            CRCCreatorEnabled = false;            
            Mode = ProtocolMode.Login;
        }
        
        /// <summary>
        /// Sets the serversave cycle value in the message header.
        /// Also adds an scrambled CRC checksum to the message, depending on the protocol state.
        /// </summary>
        /// <param name="Message">Message to add encrypted CRC to</param>
        public void SignMessage(GameMessage Message)
        {
            ushort dummy;
                
            // set SS
            Message.HeaderSS = CurrentServerSave;
            
            // update headerlength
            Message.BodyLength = Convert.ToUInt16(Message.ByteLength - GameMessage.HEADERLENGTH);

            // add a valid CRC if enabled
            if (CRCCreatorEnabled)           
                CRCCreator.CreatePacketCRC(Message, out dummy);            
        }
       
        /// <summary>
        /// Compares the ServerSave value with the current one and fires event if changed
        /// </summary>
        /// <param name="Packet"></param>
        protected void CheckServerSave(GameMessage Packet)
        {
            // check if the serversave has changed
            if (Packet.HeaderSS != CurrentServerSave)
            {
                LastServerSave = CurrentServerSave;
                CurrentServerSave = Packet.HeaderSS;

                OnNewServerSave(new ServerSaveChangedEventArgs(LastServerSave, CurrentServerSave));
            }
        }
        #endregion

        #region Message Parsers
        /// <summary>
        /// Extract Message from a MessageBuffer (eventargs)
        /// </summary>
        /// <param name="e">MessageBufferEvent</param>
        /// <returns>Typed message or generic</returns>
        protected GameMessage ExtractMessage(MessageBufferEventArgs e)
        {
            GameMessage TypedMessage = null;
            byte PI = e.MessageBuffer[GameMessage.HEADERLENGTH];

            // parse packet based on current protocol mode
            switch (Mode)
            {
                // protocol mode Login
                case ProtocolMode.Login:
                    TypedMessage = ExtractLoginModeMessage(e, (MessageTypeLoginMode)PI);
                    break;

                // protocol mode Game
                case ProtocolMode.Game:
                    TypedMessage = ExtractGameModeMessage(e, (MessageTypeGameMode)PI);
                    break;
            }

            if (TypedMessage != null)
                TypedMessage.TransferDirection = e.Direction;

            return TypedMessage;
        }

        /// <summary>
        /// Helper function for "ExtractMessage"
        /// </summary>
        /// <param name="e">LoginMode Buffer</param>
        /// <param name="PI">Decoded PI</param>
        /// <returns>Typed message or generic</returns>
        protected unsafe GameMessage ExtractLoginModeMessage(MessageBufferEventArgs e, MessageTypeLoginMode PI)
        {
            GameMessage TypedMessage = null;
           
            // pin the byte[] for pointer parsers
            fixed (byte* pBuffer = e.MessageBuffer)
            {
                byte* pMessage = pBuffer;

                switch (PI)
                {
                    case MessageTypeLoginMode.Login:                  // PI: 2
                        TypedMessage = new LoginMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.ReqGame:                // PI: 4
                        TypedMessage = new ReqGameStateMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.ReqAdmin:               // PI: 5
                        TypedMessage = new ReqAdminMessageLoginMode(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.GetClient:              // PI: 7
                        TypedMessage = new GetClientMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.GetLogin:               // PI: 21
                        TypedMessage = new GetLoginMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.GetChoice:              // PI: 22
                        TypedMessage = new GetChoiceMessage(e.MessageBuffer);
                        HandleGetChoice((GetChoiceMessage)TypedMessage);
                        break;

                    case MessageTypeLoginMode.LoginOK:                // PI: 23
                        TypedMessage = new LoginOKMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.LoginFailed:            // PI: 24
                        TypedMessage = new LoginFailedMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.Game:                   // PI: 25
                        TypedMessage = new GameStateMessage(e.MessageBuffer);
                        HandleGameState((GameStateMessage)TypedMessage);
                        break;

                    case MessageTypeLoginMode.Admin:                  // PI: 26
                        TypedMessage = new AdminMessageLoginMode(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.Credits:                // PI: 30
                        TypedMessage = new CreditsMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.Download:               // PI: 31
                        TypedMessage = new DownloadMessage(ref pMessage);
                        break;
                        
                    case MessageTypeLoginMode.Message:                // PI: 34
                        TypedMessage = new LoginModeMessageMessage(e.MessageBuffer);
                        break;

                    case MessageTypeLoginMode.NoCharacters:           // PI: 37
                        TypedMessage = new NoCharactersMessage(e.MessageBuffer);
                        break;

                    default:
                        TypedMessage = new GenericGameMessage(e.MessageBuffer);     // All unknown ones
                        break;
                }
            }

            return TypedMessage;
        }

        /// <summary>
        /// Helper function for "ExtractMessage"
        /// </summary>
        /// <param name="e">MessageBuffer to extract from</param>
        /// <param name="PI">Message type</param>
        /// <returns>Typed message or generic instance of GameMessage</returns>
        protected unsafe GameMessage ExtractGameModeMessage(MessageBufferEventArgs e, MessageTypeGameMode PI)
        {
            GameMessage TypedMessage = null;

            // pin the byte[] for pointer parsers
            fixed (byte* pBuffer = e.MessageBuffer)
            {
                byte* pMessage = pBuffer;
                
                switch (PI)
                {
                    case MessageTypeGameMode.EchoPing:                                        // PI: 1
                        TypedMessage = new EchoPingMessage(e.MessageBuffer);
                        HandleEchoPing((EchoPingMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Ping:                                            // PI: 3  
                        TypedMessage = new PingMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.System:                                          // PI: 6
                        TypedMessage = new SystemMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Wait:                                            // PI: 21
                        TypedMessage = new WaitMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.Unwait:                                          // PI: 22
                        TypedMessage = new UnwaitMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.ChangePassword:                                  // PI: 23
                        TypedMessage = new ChangePasswordMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ChangeResource:                                  // PI: 30
                        TypedMessage = new ChangeResourceMessage(ref pMessage);
                        HandleChangeResource((ChangeResourceMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.SysMessage:                                      // PI: 31
                        TypedMessage = new SysMessageMessage(stringResources, e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Message:                                         // PI: 32
                        TypedMessage = new MessageMessage(stringResources, ref pMessage);
                        break;

                    case MessageTypeGameMode.SendPlayer:                                      // PI: 40
                        TypedMessage = new SendPlayerMessage(ref pMessage);

                        break;
                    case MessageTypeGameMode.SendStats:                                       // PI: 41
                        TypedMessage = new SendStatsMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SendRoomContents:                                // PI: 42
                        TypedMessage = new SendRoomContentsMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.SendObjectContents:                              // PI: 43
                        TypedMessage = new SendObjectContentsMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.SendPlayers:                                     // PI: 44
                        TypedMessage = new SendPlayersMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.SendCharacters:                                  // PI: 45           
                        TypedMessage = new SendCharactersMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.UseCharacter:                                    // PI: 46
                        TypedMessage = new UseCharacterMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SendSpells:                                      // PI: 50
                        TypedMessage = new SendSpellsMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SendSkills:                                      // PI: 51
                        TypedMessage = new SendSkillsMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SendStatGroups:                                  // PI: 52
                        TypedMessage = new SendStatGroups(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SendEnchantments:                                // PI: 53
                        TypedMessage = new SendEnchantmentsMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqQuit:                                         // PI: 54
                        TypedMessage = new ReqQuitMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.CharInfoOk:                                      // PI: 56
                        TypedMessage = new CharInfoOkMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.CharInfoNotOk:                                   // PI: 57
                        TypedMessage = new CharInfoNotOkMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.LoadModule:                                      // PI: 58
                        TypedMessage = new LoadModuleMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqAdmin:                                        // PI: 60
                        TypedMessage = new ReqAdminMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqDM:                                           // PI: 61
                        TypedMessage = new ReqDMMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Effect:                                          // PI: 70
                        TypedMessage = new EffectMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Mail:                                            // PI: 80
                        TypedMessage = new MailMessage(stringResources, e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqGetMail:                                      // PI: 81
                        TypedMessage = new ReqGetMailMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SendMail:                                        // PI: 82
                        TypedMessage = new SendMailMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.DeleteMail:                                      // PI: 83
                        TypedMessage = new DeleteMailMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqArticles:                                     // PI: 85
                        TypedMessage = new ReqArticlesMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqArticle:                                      // PI: 86
                        TypedMessage = new ReqArticleMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.PostArticle:                                     // PI: 87
                        TypedMessage = new PostArticleMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqLookupNames:                                  // PI: 88
                        TypedMessage = new ReqLookupNamesMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Action:                                          // PI: 90
                        TypedMessage = new ActionMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqMove:                                         // PI: 100
                        TypedMessage = new ReqMoveMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqTurn:                                         // PI: 101
                        TypedMessage = new ReqTurnMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqGo:                                           // PI: 102
                        TypedMessage = new ReqGoMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqAttack:                                       // PI: 103
                        TypedMessage = new ReqAttackMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqCast:                                         // PI: 105
                        TypedMessage = new ReqCastMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqUse:                                          // PI: 106
                        TypedMessage = new ReqUseMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqUnuse:                                        // PI: 107
                        TypedMessage = new ReqUnuseMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqApply:                                        // PI: 108
                        TypedMessage = new ReqApplyMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqActivate:                                     // PI: 109
                        TypedMessage = new ReqActivateMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SayTo:                                           // PI: 110
                        TypedMessage = new SayToMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SayGroup:                                        // PI: 111
                        TypedMessage = new SayGroupMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqPut:                                          // PI: 112
                        TypedMessage = new ReqPutMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqGet:                                          // PI: 113
                        TypedMessage = new ReqGetMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqLook:                                         // PI: 116
                        TypedMessage = new ReqLookMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqInventory:                                    // PI: 117
                        TypedMessage = new ReqInventoryMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqDrop:                                         // PI: 118
                        TypedMessage = new ReqDropMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqOffer:                                        // PI: 120
                        TypedMessage = new ReqOfferMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.AcceptOffer:                                     // PI: 121
                        TypedMessage = new AcceptOfferMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.CancelOffer:                                     // PI: 122
                        TypedMessage = new CancelOfferMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqCounterOffer:                                 // PI: 123
                        TypedMessage = new CounterOfferMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqBuy:                                          // PI: 124
                        TypedMessage = new ReqBuyMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqBuyItems:                                     // PI: 125
                        TypedMessage = new ReqBuyItemsMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ChangeDescription:                               // PI: 126
                        TypedMessage = new ChangeDescriptionMessage(e.MessageBuffer);
                        break;
#if !VANILLA
                    case MessageTypeGameMode.ReqInventoryMove:                                // PI: 127
                        TypedMessage = new ReqInventoryMoveMessage(e.MessageBuffer);
                        break;
#endif
                    case MessageTypeGameMode.Player:                                          // PI: 130
                        TypedMessage = new PlayerMessage(ref pMessage);
                        HandlePlayer((PlayerMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Stat:                                            // PI: 131
                        TypedMessage = new StatMessage(ref pMessage);
                        HandleStat((StatMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.StatGroup:                                       // PI: 132
                        TypedMessage = new StatGroupMessage(ref pMessage);
                        HandleStatGroup((StatGroupMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.StatGroups:                                      // PI: 133
                        TypedMessage = new StatGroupsMessage(ref pMessage);
                        HandleStatGroups((StatGroupsMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.RoomContents:                                    // PI: 134
                        TypedMessage = new RoomContentsMessage(ref pMessage);
                        HandleRoomContents((RoomContentsMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.ObjectContents:                                  // PI: 135
                        TypedMessage = new ObjectContentsMessage(ref pMessage);
                        HandleObjectContents((ObjectContentsMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Players:                                         // PI: 136
                        TypedMessage = new PlayersMessage(ref pMessage);
                        HandlePlayers((PlayersMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.PlayerAdd:                                       // PI: 137
                        TypedMessage = new PlayerAddMessage(ref pMessage);
                        HandlePlayerAdd((PlayerAddMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.PlayerRemove:                                    // PI: 138
                        TypedMessage = new PlayerRemoveMessage(e.MessageBuffer);
                        HandlePlayerRemove((PlayerRemoveMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Characters:                                      // PI: 139
                        TypedMessage = new CharactersMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.CharInfo:                                        // PI: 140
                        TypedMessage = new CharInfoMessage(e.MessageBuffer);
                        HandleCharInfo((CharInfoMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Spells:                                          // PI: 141
                        TypedMessage = new SpellsMessage(ref pMessage);
                        HandleSpells((SpellsMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.SpellAdd:                                        // PI: 142
                        TypedMessage = new SpellAddMessage(ref pMessage);
                        HandleSpellAdd((SpellAddMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.SpellRemove:                                     // PI: 143
                        TypedMessage = new SpellRemoveMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Skills:                                          // PI: 144
                        TypedMessage = new SkillsMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.SkillAdd:                                        // PI: 145
                        TypedMessage = new SkillAddMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SkillRemove:                                     // PI: 146
                        TypedMessage = new SkillRemoveMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.AddEnchantment:                                  // PI: 147
                        TypedMessage = new AddEnchantmentMessage(e.MessageBuffer);
                        HandleAddEnchantment((AddEnchantmentMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.RemoveEnchantment:                               // PI: 148
                        TypedMessage = new RemoveEnchantmentMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Quit:                                            // PI: 149
                        TypedMessage = new QuitMessage(e.MessageBuffer);
                        HandleQuit((QuitMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Background:                                      // PI: 150
                        TypedMessage = new BackgroundMessage(e.MessageBuffer);
                        HandleBackground((BackgroundMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.PlayerOverlay:                                   // PI: 151
                        TypedMessage = new PlayerOverlayMessage(e.MessageBuffer);
                        HandlePlayerOverlay((PlayerOverlayMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.AddBgOverlay:                                    // PI: 152
                        TypedMessage = new AddBgOverlayMessage(ref pMessage);
                        HandleAddBgOverlay((AddBgOverlayMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.ChangeBgOverlay:                                 // PI: 154
                        TypedMessage = new ChangeBgOverlayMessage(ref pMessage);
                        HandleChangeBgOverlay((ChangeBgOverlayMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.UserCommand:                                     // PI: 155
                        TypedMessage = new UserCommandMessage(stringResources, e.Direction, e.MessageBuffer);
                        HandleUserCommand((UserCommandMessage)TypedMessage);
                        break;
#if !VANILLA
                    case MessageTypeGameMode.ReqStatChange:                                   // PI: 156
                        TypedMessage = new ReqStatChangeMessage(e.MessageBuffer);                
                        break;

                    case MessageTypeGameMode.ChangedStats:                                    // PI: 157
                        TypedMessage = new ChangedStatsMessage(e.MessageBuffer);
                        break;
#endif
                    case MessageTypeGameMode.PasswordOK:
                        TypedMessage = new PasswordOKMessage(e.MessageBuffer);                // PI: 160
                        break;

                    case MessageTypeGameMode.PasswordNotOK:                                   // PI: 161
                        TypedMessage = new PasswordNotOKMessage(e.MessageBuffer);                
                        break;

                    case MessageTypeGameMode.Admin:                                           // PI: 162
                        TypedMessage = new AdminMessage(e.MessageBuffer);                     
                        break;

                    case MessageTypeGameMode.PlayWave:                                        // PI: 170
                        TypedMessage = new PlayWaveMessage(ref pMessage);
                        HandlePlayWave((PlayWaveMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.PlayMusic:                                       // PI: 171
                        TypedMessage = new PlayMusicMessage(e.MessageBuffer);
                        HandlePlayMusic((PlayMusicMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.PlayMidi:                                        // PI: 172
                        TypedMessage = new PlayMidiMessage(e.MessageBuffer);
                        HandlePlayMidi((PlayMidiMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.LookNewsGroup:                                   // PI: 180
                        TypedMessage = new LookNewsGroupMessage(e.MessageBuffer);
                        HandleLookNewsGroup((LookNewsGroupMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Articles:                                        // PI: 181
                        TypedMessage = new ArticlesMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.Article:                                         // PI: 182
                        TypedMessage = new ArticleMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.LookupNames:                                     // PI: 190
                        TypedMessage = new LookupNamesMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Move:                                            // PI: 200
                        TypedMessage = new MoveMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.Turn:                                            // PI: 201
                        TypedMessage = new TurnMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.Shoot:                                           // PI: 202
                        TypedMessage = new ShootMessage(e.MessageBuffer);
                        HandleShoot((ShootMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Use:                                             // PI: 203
                        TypedMessage = new UseMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Unuse:                                           // PI: 204
                        TypedMessage = new UnuseMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.UseList:                                         // PI: 205
                        TypedMessage = new UseListMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Said:                                            // PI: 206
                        TypedMessage = new SaidMessage(stringResources, ref pMessage);
                        break;

                    case MessageTypeGameMode.Look:                                            // PI: 207
                        TypedMessage = new LookMessage(stringResources, e.MessageBuffer);
                        HandleLook((LookMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Inventory:                                       // PI: 208
                        TypedMessage = new InventoryMessage(ref pMessage);
                        HandleInventory((InventoryMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.InventoryAdd:                                    // PI: 209
                        TypedMessage = new InventoryAddMessage(e.MessageBuffer);
                        HandleInventoryAdd((InventoryAddMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.InventoryRemove:                                 // PI: 210
                        TypedMessage = new InventoryRemoveMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Offer:                                           // PI: 211
                        TypedMessage = new OfferMessage(e.MessageBuffer);
                        HandleOffer((OfferMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.OfferCanceled:                                   // PI: 212
                        TypedMessage = new OfferCanceledMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Offered:                                         // PI: 213
                        TypedMessage = new OfferedMessage(e.MessageBuffer);
                        HandleOffered((OfferedMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.CounterOffer:                                    // PI: 214
                        TypedMessage = new CounterOfferMessage(e.MessageBuffer);
                        HandleCounterOffer((CounterOfferMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.CounterOffered:                                  // PI: 215
                        TypedMessage = new CounterOfferedMessage(e.MessageBuffer);
                        HandleCounterOffered((CounterOfferedMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.BuyList:                                         // PI: 216
                        TypedMessage = new BuyListMessage(e.MessageBuffer);
                        HandleBuyList((BuyListMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Create:                                          // PI: 217
                        TypedMessage = new CreateMessage(ref pMessage);
                        HandleCreate((CreateMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.Remove:                                          // PI: 218
                        TypedMessage = new RemoveMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.Change:                                          // PI: 219
                        TypedMessage = new ChangeMessage(ref pMessage);
                        HandleChange((ChangeMessage)TypedMessage);
                        break;

                    case MessageTypeGameMode.LightAmbient:                                    // PI: 220
                        TypedMessage = new LightAmbientMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.LightPlayer:                                     // PI: 221
                        TypedMessage = new LightPlayerMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.LightShading:                                    // PI: 222
                        TypedMessage = new LightShadingMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SectorMove:                                      // PI: 223
                        TypedMessage = new SectorMoveMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.SectorLight:                                     // PI: 224
                        TypedMessage = new SectorLightMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.WallAnimate:                                     // PI: 225
                        TypedMessage = new WallAnimateMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.ChangeTexture:                                   // PI: 227
                        TypedMessage = new ChangeTextureMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.InvalidateData:                                  // PI: 228
                        TypedMessage = new InvalidateDataMessage(ref pMessage);
                        break;

                    case MessageTypeGameMode.ReqDeposit:                                      // PI: 230
                        TypedMessage = new ReqDepositMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.WithDrawAlList:                                  // PI: 231
                        TypedMessage = new WithdrawAlListMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqWithdrawAl:                                   // PI: 232
                        TypedMessage = new ReqWithdrawAlMessage(e.MessageBuffer);
                        break;

                    case MessageTypeGameMode.ReqWithdrawAlItems:                              // PI: 233
                        TypedMessage = new ReqWithDrawAlItemsMessage(e.MessageBuffer);
                        break;

                    default:
                        TypedMessage = new GenericGameMessage(e.MessageBuffer);              // All unknown ones
                        break;
                }
            }

            return TypedMessage;
        }        
        
        #endregion

        #region Event wrappers
        protected void OnNewMessageAvailable(GameMessageEventArgs e)
        {
            if (MessageAvailable != null)
                MessageAvailable(this, e);
        }

        protected void OnSplittedPacketFound(SplittedMessageFoundEventArgs e)
        {
            if (SplittedMessageFound != null)
                SplittedMessageFound(this, e);
        }

        protected void OnCompletingSplittedPacket(CompletingSplittedMessageEventArgs e)
        {
            if (CompletingSplittedMessage != null)
                CompletingSplittedMessage(this, e);
        }

        protected void OnMismatchPacketLENFound(MismatchMessageLengthFoundEventArgs e)
        {
            if (MismatchMessageLengthFound != null)
                MismatchMessageLengthFound(this, e);
        }

        protected void OnEmptyPacketFound(EmptyMessageFoundEventArgs e)
        {
            if (EmptyMessageFound != null)
                EmptyMessageFound(this, e);
        }

        protected void OnNewServerSave(ServerSaveChangedEventArgs e)
        {
            if (ServerSaveChanged != null)
                ServerSaveChanged(this, e);
        }

        protected void OnHandlerError(HandlerErrorEventArgs e)
        {
            if (HandlerError != null)
                HandlerError(this, e);
        }

        protected void OnProtocolModeChanged(EventArgs e)
        {
            if (ProtocolModeChanged != null)
                ProtocolModeChanged(this, e);
        }
        #endregion

        #region Message Handlers

        protected void HandleGetChoice(GetChoiceMessage Message)
        {
            // set the new hash on the CRCCreator
            CRCCreator.CurrentHashTable = Message.HashTable;
        }

        protected void HandleGameState(GameStateMessage Message)
        {
            CRCCreatorEnabled = true;

            // go to game protocol mode
            Mode = ProtocolMode.Game;
            OnProtocolModeChanged(new EventArgs());
        }

        protected void HandleQuit(QuitMessage Message)
        {
            CRCCreatorEnabled = false;

            // go back to login protocol mode
            Mode = ProtocolMode.Login;
            OnProtocolModeChanged(new EventArgs());
        }

        protected void HandleEchoPing(EchoPingMessage Message)
        {
            // update the message type decoder with values from EchoPing
            PIDecoder.Update(Message.NewPIDecByte, Message.ResourceID);
        }

        protected void HandleBuyList(BuyListMessage Message)
        {
            Message.TradePartner.ResolveStrings(stringResources, false);

            foreach (TradeOfferObject obj in Message.OfferedItems)
                obj.ResolveStrings(stringResources, false);    
        }

        protected void HandleOffered(OfferedMessage Message)
        {
            foreach (ObjectBase obj in Message.OfferedItems)
                obj.ResolveStrings(stringResources, false);   
        }

        protected void HandleOffer(OfferMessage Message)
        {
            Message.TradePartner.ResolveStrings(stringResources, false);

            foreach (ObjectBase obj in Message.OfferedItems)
                obj.ResolveStrings(stringResources, false);  
        }

        protected void HandleCounterOffer(CounterOfferMessage Message)
        {
            foreach (ObjectBase obj in Message.OfferedItems)
                obj.ResolveStrings(stringResources, false);
        }

        protected void HandleCounterOffered(CounterOfferedMessage Message)
        {
            foreach (ObjectBase obj in Message.OfferedItems)
                obj.ResolveStrings(stringResources, false);
        }

        protected void HandlePlayerOverlay(PlayerOverlayMessage Message)
        {
            Message.HandItemObject.ResolveStrings(stringResources, false);
        }

        protected void HandleBackground(BackgroundMessage Message)
        {
            Message.ResourceID.ResolveStrings(stringResources, false);
        }

        protected void HandleAddBgOverlay(AddBgOverlayMessage Message)
        {
            Message.BackgroundOverlay.ResolveStrings(stringResources, false);
        }

        protected void HandleChangeBgOverlay(ChangeBgOverlayMessage Message)
        {
            Message.BackgroundOverlay.ResolveStrings(stringResources, false);
        }

        protected void HandlePlayers(PlayersMessage Message)
        {
            foreach (OnlinePlayer obj in Message.OnlinePlayers)
                stringResources.TryAdd(obj.NameRID, obj.Name, LanguageCode.English);
        }

        protected void HandlePlayerAdd(PlayerAddMessage Message)
        {
            stringResources.TryAdd(Message.NewOnlinePlayer.NameRID, Message.NewOnlinePlayer.Name, LanguageCode.English);
        }

        protected void HandlePlayerRemove(PlayerRemoveMessage Message)
        {
            string temp;
            stringResources.TryRemove(Message.ObjectID, out temp, LanguageCode.English);
        }

        protected void HandleStatGroup(StatGroupMessage Message)
        {
            foreach (Stat stat in Message.Stats)
            {
                switch (stat.Type)
                {
                    case StatType.Numeric:
                        StatNumeric numStat = (StatNumeric)stat;                      
                        numStat.ResolveStrings(stringResources, false);                       
                        break;

                    case StatType.List:
                        StatList listStat = (StatList)stat;
                        listStat.ResolveStrings(stringResources, false);                       
                        break;
                }
            }           
        }

        protected void HandleStatGroups(StatGroupsMessage Message)
        {
            foreach (ResourceID obj in Message.ResourceIDs)
                obj.ResolveStrings(stringResources, false);          
        }

        protected void HandleStat(StatMessage Message)
        {
            switch (Message.Stat.Type)
            {
                case StatType.Numeric:
                    StatNumeric value1 = (StatNumeric)Message.Stat;
                    value1.ResolveStrings(stringResources, false);
                    break;

                case StatType.List:
                    StatList value2 = (StatList)Message.Stat;
                    value2.ResolveStrings(stringResources, false);
                    break;
            }
        }

        protected void HandleCharInfo(CharInfoMessage Message)
        {
            Message.CharCreationInfo.ResolveStrings(stringResources, false);
        }

        protected void HandleSpells(SpellsMessage Message)
        {
            foreach (SpellObject obj in Message.SpellObjects)            
                obj.ResolveStrings(stringResources, false);         
        }

        protected void HandleSpellAdd(SpellAddMessage Message)
        {
            Message.NewSpellObject.ResolveStrings(stringResources, false);
        }

        protected void HandleUserCommand(UserCommandMessage Message)
        {
            switch (Message.Command.CommandType)
            {
                case UserCommandType.LookPlayer:
                    UserCommandLookPlayer comLookPlayer = (UserCommandLookPlayer)Message.Command;
                    ObjectBase charData = comLookPlayer.PlayerInfo.ObjectBase;

                    charData.ResolveStrings(stringResources, false);
                    break;

                case UserCommandType.GuildShields:
                    // can either be GuildShieldList or GuildShieldListReq
                    if (Message.Command is UserCommandGuildShieldList)
                    {
                        UserCommandGuildShieldList comGuildShields = (UserCommandGuildShieldList)Message.Command;

                        foreach (ResourceIDBGF obj in comGuildShields.ShieldResources)
                            obj.ResolveStrings(stringResources, false);
                    }
                    break;
            }
        }

        protected void HandleLook(LookMessage Message)
        {
            ObjectBase objectBase = Message.ObjectInfo.ObjectBase;
            objectBase.ResolveStrings(stringResources, false);          
        }

        protected void HandlePlayer(PlayerMessage Message)
        {
            Message.RoomInfo.ResolveStrings(stringResources, false);
        }

        protected void HandleInventory(InventoryMessage Message)
        {
            foreach (ObjectBase obj in Message.InventoryObjects)           
                obj.ResolveStrings(stringResources, false);           
        }

        protected void HandleInventoryAdd(InventoryAddMessage Message)
        {
            Message.NewInventoryObject.ResolveStrings(stringResources, false);
        }

        protected void HandleAddEnchantment(AddEnchantmentMessage Message)
        {
            Message.NewBuffObject.ResolveStrings(stringResources, false);
        }

        protected void HandleChangeResource(ChangeResourceMessage Message)
        {
            string temp;

            // remove old (always english)
            stringResources.TryRemove(Message.ResourceID, out temp, LanguageCode.English);

            // add new (always english)
            stringResources.TryAdd(Message.ResourceID, Message.NewValue, LanguageCode.English);
        }

        protected void HandleRoomContents(RoomContentsMessage Message)
        {
            foreach (RoomObject obj in Message.RoomObjects)           
                obj.ResolveStrings(stringResources, false);           
        }

        protected void HandleObjectContents(ObjectContentsMessage Message)
        {
            foreach (ObjectBase obj in Message.ContentObjects)            
                obj.ResolveStrings(stringResources, false);            
        }

        protected void HandleCreate(CreateMessage Message)
        {
            Message.NewRoomObject.ResolveStrings(stringResources, false);
        }

        protected void HandleChange(ChangeMessage Message)
        {
            Message.UpdatedObject.ResolveStrings(stringResources, false);
        }

        protected void HandlePlayWave(PlayWaveMessage Message)
        {
            Message.PlayInfo.ResolveStrings(stringResources, false);
        }

        protected void HandlePlayMusic(PlayMusicMessage Message)
        {
            Message.PlayInfo.ResolveStrings(stringResources, false);
        }

        protected void HandlePlayMidi(PlayMidiMessage Message)
        {
            Message.PlayInfo.ResolveStrings(stringResources, false);
        }

        protected void HandleLookNewsGroup(LookNewsGroupMessage Message)
        {
            Message.NewsGroup.ResolveStrings(stringResources, false);
            Message.NewsGroup.NewsGlobeObject.ResolveStrings(stringResources, false);
        }

        protected void HandleShoot(ShootMessage Message)
        {
            Message.Projectile.ResolveStrings(stringResources, false);
        }
        #endregion
    }
}
