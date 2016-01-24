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
using Meridian59.Data.Lists;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Enums;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;
using Meridian59.Files.ROO;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Data
{
    /// <summary>
    /// Offers a bindable, observable and self updating data layer.
    /// This contains basically any information you need at runtime.
    /// Make sure to call the Message handlers.
    /// </summary>
    public class DataController : INotifyPropertyChanged, ITickable
    {
        #region Constants
        public const string PROPNAME_AVATAROBJECT = "AvatarObject";
        public const string PROPNAME_TARGETOBJECT = "TargetObject";
        public const string PROPNAME_SELFTARGET = "SelfTarget";
        public const string PROPNAME_TARGETID = "TargetID";
        public const string PROPNAME_ISRESTING = "IsResting";
        public const string PROPNAME_ISWAITING = "IsWaiting";
        public const string PROPNAME_MERIDIANTIME = "MeridianTime";
        public const string PROPNAME_TPS = "TPS";
        public const string PROPNAME_RTT = "RTT";
        public const string PROPNAME_VIEWERPOSITION = "ViewerPosition";
        public const string PROPNAME_ACCOUNTTYPE = "AccountType";
        public const string PROPNAME_UIMODE = "UIMode";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected RoomObject avatarObject;
        protected ObjectBase targetObject;
        protected uint targetID = UInt32.MaxValue;
        protected bool selfTarget;
        protected bool isResting;

        protected bool isWaiting;
        protected uint tps;
        protected uint rtt;
        protected DateTime meridianTime;
        protected V3 viewerPosition;
        protected AccountType accountType;
        protected UIMode uiMode;

        protected readonly RoomObjectList roomObjects;
        protected readonly RoomObjectListFiltered roomObjectsFiltered;
        protected readonly ProjectileList projectiles;
        protected readonly OnlinePlayerList onlinePlayers;
        protected readonly InventoryObjectList inventoryObjects;
        protected readonly StatNumericList avatarCondition;
        protected readonly StatNumericList avatarAttributes;
        protected readonly SkillList avatarSkills;
        protected readonly SkillList avatarSpells;
        protected readonly SkillList avatarQuests;
        protected readonly ObjectBaseList<ObjectBase> roomBuffs;
        protected readonly ObjectBaseList<ObjectBase> avatarBuffs;
        protected readonly SpellObjectList spellObjects;
        protected readonly BackgroundOverlayList backgroundOverlays;
        protected readonly ObjectBaseList<PlayerOverlay> playerOverlays;
        protected readonly BaseList<ServerString> chatMessages;
        protected readonly BaseList<GameMessage> gameMessageLog;
        protected readonly List<RoomObject> visitedTargets;
        protected readonly List<uint> clickedTargets;
        protected readonly ActionButtonList actionButtons;
        protected readonly List<string> ignoreList;
        protected readonly List<string> chatCommandHistory;

        protected readonly RoomInfo roomInformation;
        protected readonly LightShading lightShading;
        protected readonly PlayMusic backgroundMusic;
        protected readonly GuildInfo guildInfo;
        protected readonly GuildShieldInfo guildShieldInfo;
        protected readonly GuildAskData guildAskData;
        protected readonly DiplomacyInfo diplomacyInfo;
        protected readonly AdminInfo adminInfo;
        protected readonly TradeInfo tradeInfo;
        protected readonly BuyInfo buyInfo;
        protected readonly WelcomeInfo welcomeInfo;
        protected readonly CharCreationInfo charCreationInfo;
        protected readonly StatChangeInfo statChangeInfo;
        protected readonly NewsGroup newsGroup;
        protected readonly ObjectContents objectContents;
        protected readonly Effects effects;
        protected readonly PlayerInfo lookPlayer;
        protected readonly ObjectInfo lookObject;
        protected readonly PreferencesFlags clientPreferences;
        #endregion

        #region Properties
        #region Collections
        /// <summary>
        /// List of objects in your room
        /// </summary>
        public RoomObjectList RoomObjects { get { return roomObjects; } }

        /// <summary>
        /// Contains all objects from RoomObjects matching
        /// the currently set filter.
        /// </summary>
        public RoomObjectListFiltered RoomObjectsFiltered { get { return roomObjectsFiltered; } }

        /// <summary>
        /// List of current projectiles in room
        /// </summary>
        public ProjectileList Projectiles { get { return projectiles; } }

        /// <summary>
        /// List of online players
        /// </summary>
        public OnlinePlayerList OnlinePlayers { get { return onlinePlayers; } }

        /// <summary>
        /// Your inventory objects
        /// </summary>
        public InventoryObjectList InventoryObjects { get { return inventoryObjects; } }

        /// <summary>
        /// Your condition values (HP, MP, Vigor)
        /// </summary>
        public StatNumericList AvatarCondition { get { return avatarCondition; } }

        /// <summary>
        /// Your attributes (STR, STAM, ...)
        /// </summary>
        public StatNumericList AvatarAttributes { get { return avatarAttributes; } }

        /// <summary>
        /// Your skills (slash, block, ...)
        /// </summary>
        public SkillList AvatarSkills { get { return avatarSkills; } }

        /// <summary>
        /// Your spells
        /// </summary>
        public SkillList AvatarSpells { get { return avatarSpells; } }

        /// <summary>
        /// The quests list
        /// </summary>
        public SkillList AvatarQuests { get { return avatarQuests; } }

        /// <summary>
        /// Currently active room enchantments
        /// </summary>
        public ObjectBaseList<ObjectBase> RoomBuffs { get { return roomBuffs; } }

        /// <summary>
        /// Currently active enchantments on your avatar
        /// </summary>
        public ObjectBaseList<ObjectBase> AvatarBuffs { get { return avatarBuffs; } }

        /// <summary>
        /// Your known spells
        /// </summary>
        public SpellObjectList SpellObjects { get { return spellObjects; } }

        /// <summary>
        /// List of background overlays (like sun)
        /// </summary>
        public BackgroundOverlayList BackgroundOverlays { get { return backgroundOverlays; } }

        /// <summary>
        /// First person weapon/shield/... overlay data
        /// </summary>
        public ObjectBaseList<PlayerOverlay> PlayerOverlays { get { return playerOverlays; } }

        /// <summary>
        /// A list of visited targets for NextTarget function
        /// </summary>
        public List<RoomObject> VisitedTargets { get { return visitedTargets; } }

        /// <summary>
        /// A list of recently clicked targets for click iteration.
        /// </summary>
        public List<uint> ClickedTargets { get { return clickedTargets; } }

        /// <summary>
        /// Messages from these players won't be added to the chatmessages list.
        /// </summary>
        public List<string> IgnoreList { get { return ignoreList; } }

        /// <summary>
        /// List of received chat messages
        /// </summary>
        public BaseList<ServerString> ChatMessages { get { return chatMessages; } }

        /// <summary>
        /// Saves last executed chatcommands
        /// </summary>
        public List<string> ChatCommandHistory { get { return chatCommandHistory; } }

        /// <summary>
        /// Log of message flow between client and server
        /// </summary>
        public BaseList<GameMessage> GameMessageLog { get { return gameMessageLog; } }

        /// <summary>
        /// The active actionbuttons / shortcuts.
        /// </summary>
        public ActionButtonList ActionButtons { get { return actionButtons; } }
        #endregion

        #region Single Sub Data Objects
        /// <summary>
        /// Information about the current room
        /// </summary>
        public RoomInfo RoomInformation { get { return roomInformation; } }

        /// <summary>
        /// Information about current directional light
        /// </summary>
        public LightShading LightShading { get { return lightShading; } }

        /// <summary>
        /// The current background music
        /// </summary>
        public PlayMusic BackgroundMusic { get { return backgroundMusic; } }

        /// <summary>
        /// Information about the guild you belong to
        /// </summary>
        public GuildInfo GuildInfo { get { return guildInfo; } }

        /// <summary>
        /// Data for your or the currently selected guildshield
        /// </summary>
        public GuildShieldInfo GuildShieldInfo { get { return guildShieldInfo; } }

        /// <summary>
        /// Info transmitted for the GuildCreate window (e.g. cost)
        /// </summary>
        public GuildAskData GuildAskData { get { return guildAskData; } }

        /// <summary>
        /// Diplomacy info between your guild and others
        /// </summary>
        public DiplomacyInfo DiplomacyInfo { get { return diplomacyInfo; } }

        /// <summary>
        /// Admin info (for admin console) and more.
        /// </summary>
        public AdminInfo AdminInfo { get { return adminInfo; } }

        /// <summary>
        /// Info about a possible active trade
        /// </summary>
        public TradeInfo Trade { get { return tradeInfo; } }

        /// <summary>
        /// Info about a possibly active Buy (from NPC) window
        /// </summary>
        public BuyInfo Buy { get { return buyInfo; } }

        /// <summary>
        /// Stores the welcome info transferred at login, containing selectable
        /// avatars and message of the day.
        /// </summary>
        public WelcomeInfo WelcomeInfo { get { return welcomeInfo; } }

        /// <summary>
        /// Info for the avatar creation wizard.
        /// </summary>
        public CharCreationInfo CharCreationInfo { get { return charCreationInfo; } }

        /// <summary>
        /// Info for the stat change wizard
        /// </summary>
        public StatChangeInfo StatChangeInfo { get { return statChangeInfo; } }

        /// <summary>
        /// The last inspected newsgroup and its articles
        /// </summary>
        public NewsGroup NewsGroup { get { return newsGroup; } }

        /// <summary>
        /// The data of the last ObjectsContentMessage
        /// </summary>
        public ObjectContents ObjectContents { get { return objectContents; } }

        /// <summary>
        /// Currently active effects
        /// </summary>
        public Effects Effects { get { return effects; } }

        /// <summary>
        /// The last inspected player
        /// </summary>
        public PlayerInfo LookPlayer { get { return lookPlayer; } }

        /// <summary>
        /// The last inspected nonplayer-object.
        /// This instance stays the same. Its properties change!
        /// </summary>
        public ObjectInfo LookObject { get { return lookObject; } }

        /// <summary>
        /// Current client gameplay preferences (e.g. safety, tempsafe).
        /// </summary>
        public PreferencesFlags ClientPreferences { get { return clientPreferences; } }

        #endregion

        #region Own Values
        /// <summary>
        /// Maximum entries in chat before remove
        /// </summary>
        public int ChatMessagesMaximum { get; set; }

        /// <summary>
        /// Used in GetNext and GetPrevious.
        /// </summary>
        public int ChatCommandHistoryIndex { get; set; }

        /// <summary>
        /// Maximum entries in executed chatcommand history
        /// </summary>
        public int ChatCommandHistoryMaximum { get; set; }

        /// <summary>
        /// Whether to log outgoing messages
        /// </summary>
        public bool LogOutgoingMessages { get; set; }

        /// <summary>
        /// Whether to log incoming messages
        /// </summary>
        public bool LogIncomingMessages { get; set; }

        /// <summary>
        /// Whether to log Ping and PingEcho messages
        /// </summary>
        public bool LogPingMessages { get; set; }

        /// <summary>
        /// The ID of your avatar
        /// </summary>
        public uint AvatarID { get; set; }

        /// <summary>
        /// Whether cast/uses are go on yourself.
        /// </summary>
        public bool SelfTarget
        {
            get { return selfTarget; }
            set
            {
                if (selfTarget != value)
                {
                    selfTarget = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SELFTARGET));
                }
            }
        }

        /// <summary>
        /// The ID of your currently selected object
        /// </summary>
        public uint TargetID
        {
            get { return targetID; }
            set
            {
                if (targetID != value)
                {
                    targetID = value;

                    // find/mark target in roomobjects
                    bool found = false;
                    foreach (RoomObject obj in RoomObjects)
                    {
                        if (obj.ID == value)
                        {
                            obj.IsTarget = true;
                            TargetObject = obj;
                            found = true;
                        }
                        else
                            obj.IsTarget = false;
                    }

                    if (!found)
                    {
                        // find target in inventory
                        foreach (InventoryObject obj in InventoryObjects)
                        {
                            if (obj.ID == value)
                            {
                                TargetObject = obj;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            TargetObject = null;
                    }

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TARGETID));
                }
            }
        }

        /// <summary>
        /// Your currently selected targetobject or NULL.
        /// Use TargetID to assign.
        /// </summary>
        public ObjectBase TargetObject
        {
            get { return targetObject; }
            protected set
            {
                if (targetObject != value)
                {
                    targetObject = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TARGETOBJECT));

                    // update tradepartner if visible but not yet pending
                    if (Trade.IsVisible && !Trade.IsPending)
                        Trade.TradePartner = targetObject;
                }
            }
        }

        /// <summary>
        /// The RoomObject of your avatar.
        /// Be careful: This is NULL at first and
        /// for a short moment whenever you change a room.
        /// </summary>
        public RoomObject AvatarObject
        {
            get { return avatarObject; }
            set
            {
                if (avatarObject != value)
                {
                    avatarObject = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATAROBJECT));
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsNextAttackApplyCastOnHighlightedObject { get; set; }

        /// <summary>
        /// Whether you're currently resting or not
        /// </summary>
        public bool IsResting
        {
            get { return isResting; }
            set
            {
                if (isResting != value)
                {
                    isResting = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISRESTING));
                }
            }
        }

        /// <summary>
        /// Whether server is saving right now or not
        /// </summary>
        public bool IsWaiting
        {
            get { return isWaiting; }
            set
            {
                if (isWaiting != value)
                {
                    isWaiting = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISWAITING));
                }
            }
        }

        /// <summary>
        /// The current time in Meridian 59.
        /// With date set to 1/1/1.
        /// Updated when Tick() is called.
        /// </summary>
        public DateTime MeridianTime
        {
            get { return meridianTime; }
            set
            {
                if (meridianTime != value)
                {
                    meridianTime = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MERIDIANTIME));
                }
            }
        }

        /// <summary>
        /// Ticks per second
        /// </summary>
        public uint TPS
        {
            get { return tps; }
            set
            {
                if (tps != value)
                {
                    tps = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TPS));
                }
            }
        }

        /// <summary>
        /// Roundtrip time
        /// </summary>
        public uint RTT
        {
            get { return rtt; }
            set
            {
                if (rtt != value)
                {
                    rtt = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RTT));
                }
            }
        }

        /// <summary>
        /// Position of a viewer (camera)
        /// </summary>
        public V3 ViewerPosition
        {
            get { return viewerPosition; }
            set
            {
                if (!viewerPosition.Equals(value))
                {
                    viewerPosition = value;

                    // update viewer angles
                    ProcessViewerAngle();

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VIEWERPOSITION));
                }
            }
        }

        /// <summary>
        /// The current UI state
        /// </summary>
        public UIMode UIMode
        {
            get { return uiMode; }
            set
            {
                if (uiMode != value)
                {
                    uiMode = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_UIMODE));
                }
            }
        }

        /// <summary>
        /// The accounttype received with LoginOK
        /// </summary>
        public AccountType AccountType
        {
            get { return accountType; }
            protected set
            {
                if (accountType != value)
                {
                    accountType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ACCOUNTTYPE));
                }
            }
        }
        #endregion

        #region Easy access to HP/MP/...
        /// <summary>
        /// Returns your current hitpoints or 0 if not known.
        /// </summary>
        public int HitPoints
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.HITPOINTS);

                if (stat != null)
                    return stat.ValueCurrent;

                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns your current manapoints or 0 if not known.
        /// </summary>
        public int ManaPoints
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.MANA);

                if (stat != null)
                    return stat.ValueCurrent;

                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns your current vigorpoints or 0 if not known.
        /// </summary>
        public int VigorPoints
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.VIGOR);

                if (stat != null)
                    return stat.ValueCurrent;

                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the delta between maximum vigor (usually 200) and your current vigor or 0 if not known.
        /// </summary>
        public int Hunger
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.VIGOR);

                if (stat != null)

                    // note: the absolute maximum value for vigor is saved in ValueRenderMax
                    // usually 200
                    return Math.Max(0, stat.ValueRenderMax - stat.ValueCurrent);

                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the delta between maximum vigor you can get by resting and your current vigor or 0 if not known.
        /// </summary>
        public int Fatigue
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.VIGOR);

                if (stat != null)

                    // note: the maximum value for vigor you get by only resting is saved in ValueMaximum
                    // usually 80-100 based on second wind
                    return Math.Max(0, stat.ValueMaximum - stat.ValueCurrent);

                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the delta between your maximum hitpoints and your current hitpoints or 0 if not known.
        /// So this tells you how much you can heal.
        /// </summary>
        public int Injury
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.HITPOINTS);

                if (stat != null)

                    // note: the real maximum value of hitpoints you can heal/got is saved in ValueMaximum
                    // the RenderMaximum refers to 100 by default
                    return Math.Max(0, stat.ValueMaximum - stat.ValueCurrent);

                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the delta between your maximum manapoints and your current manapoints or 0 if not known.
        /// </summary>
        public int ManaMissing
        {
            get
            {
                StatNumeric stat = AvatarCondition.GetItemByNum(StatNums.MANA);

                if (stat != null)

                    // note: the real maximum value of manapoints you can get is saved in ValueMaximum
                    // the RenderMaximum refers to 100 by default
                    return Math.Max(0, stat.ValueMaximum - stat.ValueCurrent);

                else
                    return 0;
            }
        }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public DataController()
        {
            // create lists
            roomObjects = new RoomObjectList(300);
            roomObjectsFiltered = new RoomObjectListFiltered(roomObjects);
            projectiles = new ProjectileList(50);
            onlinePlayers = new OnlinePlayerList(200);
            inventoryObjects = new InventoryObjectList(100);
            avatarCondition = new StatNumericList(5);
            avatarAttributes = new StatNumericList(10);
            avatarSkills = new SkillList(100);
            avatarSpells = new SkillList(100);
            avatarQuests = new SkillList(100);
            roomBuffs = new ObjectBaseList<ObjectBase>(30);
            avatarBuffs = new ObjectBaseList<ObjectBase>(30);
            spellObjects = new SpellObjectList(100);
            backgroundOverlays = new BackgroundOverlayList(5);
            playerOverlays = new ObjectBaseList<PlayerOverlay>(10);
            chatMessages = new BaseList<ServerString>(101);
            gameMessageLog = new BaseList<GameMessage>(100);
            visitedTargets = new List<RoomObject>(50);
            clickedTargets = new List<uint>(50);
            actionButtons = new ActionButtonList();
            ignoreList = new List<string>(20);
            chatCommandHistory = new List<string>(20);

            // attach some listeners
            RoomObjects.ListChanged += OnRoomObjectsListChanged;
            Projectiles.ListChanged += OnProjectilesListChanged;
            ChatMessages.ListChanged += OnChatMessagesListChanged;

            // make some lists sorted
            OnlinePlayers.SortByName();
            AvatarSkills.SortByResourceName();
            AvatarSpells.SortByResourceName();
            SpellObjects.SortByName();

            // create single data objects
            roomInformation = new RoomInfo();
            lightShading = new LightShading(0, new SpherePosition(0, 0));
            backgroundMusic = new PlayMusic();
            guildInfo = new GuildInfo();
            guildShieldInfo = new GuildShieldInfo();
            guildAskData = new GuildAskData();
            diplomacyInfo = new DiplomacyInfo();
            adminInfo = new AdminInfo();
            tradeInfo = new TradeInfo();
            buyInfo = new BuyInfo();
            welcomeInfo = new WelcomeInfo();
            charCreationInfo = new CharCreationInfo();
            statChangeInfo = new StatChangeInfo();
            newsGroup = new NewsGroup();
            objectContents = new ObjectContents();
            effects = new Effects();
            lookPlayer = new PlayerInfo();
            lookObject = new ObjectInfo();
            clientPreferences = new PreferencesFlags();

            // some values
            ChatMessagesMaximum = 100;
            ChatCommandHistoryMaximum = 20;
            ChatCommandHistoryIndex = -1;
            AvatarObject = null;
            IsResting = false;
            SelfTarget = false;
            IsNextAttackApplyCastOnHighlightedObject = false;
            AvatarID = UInt32.MaxValue;
            TargetID = UInt32.MaxValue;
            ViewerPosition = V3.ZERO;
            UIMode = UIMode.None;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reset the data to clear state
        /// </summary>
        public void Reset()
        {
            // clear non server-related lists
            ChatMessages.Clear();
            GameMessageLog.Clear();
            ChatCommandHistory.Clear();

            // clear values
            ChatCommandHistoryIndex = -1;
            ViewerPosition = V3.ZERO;

            // reset others
            Invalidate();
        }

        /// <summary>
        /// Invalidates all data not valid anymore
        /// after server-save cycle
        /// </summary>
        public void Invalidate()
        {
            // clear lists
            RoomObjects.Clear();
            Projectiles.Clear();
            OnlinePlayers.Clear();
            InventoryObjects.Clear();
            AvatarCondition.Clear();
            AvatarAttributes.Clear();
            AvatarSkills.Clear();
            AvatarSpells.Clear();
            AvatarBuffs.Clear();
            AvatarQuests.Clear();
            RoomBuffs.Clear();
            SpellObjects.Clear();
            BackgroundOverlays.Clear();
            PlayerOverlays.Clear();
            VisitedTargets.Clear();
            ClickedTargets.Clear();

            // clear single data models
            Effects.Clear(true);
            GuildInfo.Clear(true);
            GuildAskData.Clear(true);
            DiplomacyInfo.Clear(true);
            AdminInfo.Clear(true);
            ClientPreferences.Clear(false); // Do not propagate clear to server.

            LookObject.Clear(true);
            LookPlayer.Clear(true);
            RoomInformation.Clear(true);
            LightShading.Clear(true);
            BackgroundMusic.Clear(true);
            NewsGroup.Clear(true);
            Trade.Clear(true);
            Buy.Clear(true);
            WelcomeInfo.Clear(true);
            CharCreationInfo.Clear(true);
            ObjectContents.Clear(true);
            GuildShieldInfo.Clear(true);
            StatChangeInfo.Clear(true);

            // reset values/references
            AvatarObject = null;
            IsResting = false;
            SelfTarget = false;
            IsNextAttackApplyCastOnHighlightedObject = false;
            AvatarID = UInt32.MaxValue;
            TargetID = UInt32.MaxValue;
        }

        /// <summary>
        /// Updates all ITickable instances in the DataController.
        /// Call this regularly (e.g. once each thread-loop).
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(double Tick, double Span)
        {
            // these iterations are backwards to allow
            // removing entries while iterating

            // update inventoryobjects
            for (int i = InventoryObjects.Count - 1; i >= 0; i--)
                InventoryObjects[i].Tick(Tick, Span);

            // update projectiles
            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                Projectiles[i].Tick(Tick, Span);

                if (RoomInformation.ResourceRoom != null)
                    Projectiles[i].UpdatePosition(Span, RoomInformation.ResourceRoom);
            }

            // update roomobjects
            for (int i = RoomObjects.Count - 1; i >= 0; i--)
            {
                RoomObjects[i].Tick(Tick, Span);
                RoomObjects[i].UpdatePosition(Span, RoomInformation);
            }

            // update 1. person playeroverlays
            for (int i = PlayerOverlays.Count - 1; i >= 0; i--)
            {
                PlayerOverlay ov = PlayerOverlays[i];

                // update this tick
                ov.Tick(Tick, Span);

                // remove overlays which have a finished ONCE animation and finalgroup = 0
                // this finalgroup value is basically invalid
                // at least for playeroverlays it indicates the overlay should be removed/not drawn anymore
                if (ov.Animation.AnimationType == AnimationType.ONCE)
                {
                    // cast
                    AnimationOnce typedOv = (AnimationOnce)ov.Animation;

                    // finished & finalgroup = 0
                    if (typedOv.Finished && typedOv.GroupFinal == 0)
                    {
                        // remove it
                        PlayerOverlays.Remove(ov);
                    }
                }
            }

            // update effects
            Effects.Tick(Tick, Span);

            // update lookobject
            if (LookObject != null && LookObject.ObjectBase != null)
                LookObject.ObjectBase.Tick(Tick, Span);

            // update lookplayer
            if (LookPlayer != null && LookPlayer.ObjectBase != null)
                LookPlayer.ObjectBase.Tick(Tick, Span);

            // update charcreation model
            if (CharCreationInfo != null && CharCreationInfo.ExampleModel != null)
                CharCreationInfo.ExampleModel.Tick(Tick, Span);

            // update guildshield model
            if (GuildShieldInfo != null && GuildShieldInfo.ExampleModel != null)
                GuildShieldInfo.ExampleModel.Tick(Tick, Span);

            // update tradedata
            Trade.Tick(Tick, Span);

            // update current m59 time
            MeridianTime = MeridianDate.GetMeridianTime();
        }

        /// <summary>
        /// Switches your target to "next" one
        /// </summary>
        public void NextTarget()
        {
            if (RoomInformation.ResourceRoom != null && AvatarObject != null)
            {
                // get visible objects within distances
                List<RoomObject> candidates = avatarObject.GetObjectsWithinDistance(RoomObjects, RoomInformation.ResourceRoom,
                    GeometryConstants.TARGETBEHINDRADIUS, GeometryConstants.TARGETFRONTRADIUS, false);

                // get the ones not visited yet and attackable or guildenemy
                List<RoomObject> bettercandidates = new List<RoomObject>();

                foreach (RoomObject obj in candidates)
                {
                    if ((obj.Flags.IsAttackable || obj.Flags.IsMinimapEnemy) && !VisitedTargets.Contains(obj))
                        bettercandidates.Add(obj);
                }

                // if all objects have been visited,
                // clear visited list and use all candidates
                if (bettercandidates.Count == 0)
                {
                    VisitedTargets.Clear();
                    foreach (RoomObject obj in candidates)
                        if ((obj.Flags.IsAttackable || obj.Flags.IsMinimapEnemy))
                            bettercandidates.Add(obj);
                }

                // 1. iteration, look for guild enemies
                bool found = false;
                Real dist2;
                Real mindist2 = Real.MaxValue;
                RoomObject minObj = null;
                foreach (RoomObject obj in bettercandidates)
                {
                    if (obj.Flags.IsMinimapEnemy)
                    {
                        // mark found
                        found = true;

                        // get distsquared
                        dist2 = AvatarObject.GetDistanceSquared(obj);

                        // closer than last candidate?
                        if (dist2 < mindist2)
                        {
                            // save obj and min dist
                            mindist2 = dist2;
                            minObj = obj;
                        }
                    }
                }

                if (found && minObj != null)
                {
                    VisitedTargets.Add(minObj);
                    TargetID = minObj.ID;
                }

                // 2. iteration, use anything attackable
                else
                {
                    foreach (RoomObject obj in bettercandidates)
                    {
                        if (obj.Flags.IsAttackable)
                        {
                            // mark found
                            found = true;

                            // get distsquared
                            dist2 = AvatarObject.GetDistanceSquared(obj);

                            // closer than last candidate?
                            if (dist2 < mindist2)
                            {
                                // save obj and min dist
                                mindist2 = dist2;
                                minObj = obj;
                            }
                        }
                    }

                    if (found && minObj != null)
                    {
                        VisitedTargets.Add(minObj);
                        TargetID = minObj.ID;
                    }
                }
            }
        }

        /// <summary>
        /// Switches your target from a click
        /// </summary>
        /// <param name="RoomObjectIDs">Distance sorted list of overlapping IDs clicked at once.</param>
        /// <param name="UseFirst"></param>
        public void ClickTarget(List<uint> RoomObjectIDs, bool UseFirst = false)
        {
            // if usable info
            if (RoomObjectIDs != null && RoomObjectIDs.Count > 0)
            {
                // if forced to use first value
                if (UseFirst)
                {
                    uint id = RoomObjectIDs[0];
                    TargetID = id;

                    // clear list and add the first
                    ClickedTargets.Clear();
                    ClickedTargets.Add(id);
                }
                else
                {
                    bool found = false;
                    foreach (uint id in RoomObjectIDs)
                    {
                        if (!ClickedTargets.Contains(id))
                        {
                            TargetID = id;
                            ClickedTargets.Add(id);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        uint id = RoomObjectIDs[0];
                        TargetID = id;

                        ClickedTargets.Clear();
                        ClickedTargets.Add(id);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the closest attackble roomobject in front of the avatar.
        /// </summary>
        /// <returns></returns>
        public RoomObject ClosestAttackableInFront()
        {
            if (RoomInformation.ResourceRoom == null || AvatarObject == null)
                return null;

            // get visible objects within distances
            List<RoomObject> candidates = avatarObject.GetObjectsWithinDistance(RoomObjects, RoomInformation.ResourceRoom,
                32.0f, 512.0f, false);

            // get the closest
            Real dist2;
            Real mindist2 = Real.MaxValue;
            RoomObject minObj = null;

            foreach (RoomObject obj in candidates)
            {
                if (obj.Flags.IsAttackable)
                {
                    // get distsquared
                    dist2 = AvatarObject.GetDistanceSquared(obj);

                    // closer than last candidate?
                    if (dist2 < mindist2)
                    {
                        // save obj and min dist
                        mindist2 = dist2;
                        minObj = obj;
                    }
                }
            }

            return minObj;
        }

        /// <summary>
        /// Adds a string at the beginning of the chatcommandhistory.
        /// May remove the last element.
        /// </summary>
        /// <param name="ChatCommand"></param>
        public void ChatCommandHistoryAdd(string ChatCommand)
        {
            // remove last element
            if (ChatCommandHistory.Count >= ChatCommandHistoryMaximum)
                ChatCommandHistory.RemoveAt(ChatCommandHistory.Count - 1);

            // add at beginnig
            ChatCommandHistory.Insert(0, ChatCommand);
        }

        /// <summary>
        /// Tries to get the next element from the chatcommand history,
        /// if there is one more.
        /// </summary>
        /// <returns>ChatCommand string or NULL</returns>
        public string ChatCommandHistoryGetNext()
        {
            // see if we can get one more element from the end of the list
            if (ChatCommandHistory.Count > ChatCommandHistoryIndex + 1)
            {
                // raise
                ChatCommandHistoryIndex++;

                return ChatCommandHistory[ChatCommandHistoryIndex];
            }
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string ChatCommandHistoryGetPrevious()
        {
            // see if we can get one more element from the beginning of the list
            if (ChatCommandHistoryIndex > 0 &&
                ChatCommandHistory.Count > ChatCommandHistoryIndex - 1)
            {
                // decrease
                ChatCommandHistoryIndex--;

                return ChatCommandHistory[ChatCommandHistoryIndex];
            }
            else
            {
                ChatCommandHistoryIndex = -1;

                return null;
            }
        }

        /// <summary>
        /// Executed when RoomObjects list changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRoomObjectsListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    RoomObjects.LastAddedItem.PropertyChanged += OnRoomObjectPropertyChanged;
                    break;

                case ListChangedType.ItemDeleted:
                    RoomObjects.LastDeletedItem.PropertyChanged -= OnRoomObjectPropertyChanged;
                    break;
            }
        }

        /// <summary>
        /// Executed when a property on a roomobject changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRoomObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RoomObject obj = (RoomObject)sender;

            switch (e.PropertyName)
            {
                case RoomObject.PROPNAME_POSITION3D:
                case RoomObject.PROPNAME_ANGLE:
                    // update viewerangle in case the object moved or rotated
                    obj.UpdateViewerAngle(new V2(viewerPosition.X, viewerPosition.Z));
                    break;
            }
        }

        /// <summary>
        /// Executed when Projectiles list changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnProjectilesListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Projectiles.LastAddedItem.PropertyChanged += OnProjectilePropertyChanged;
                    break;

                case ListChangedType.ItemDeleted:
                    Projectiles.LastDeletedItem.PropertyChanged -= OnProjectilePropertyChanged;
                    break;
            }
        }

        /// <summary>
        /// Executed when ChateMessages list changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnChatMessagesListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Logger.LogChat(ChatMessages[e.NewIndex].FullString);
                    break;
            }
        }

        /// <summary>
        /// Executed when a property on a projectile changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnProjectilePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Projectile obj = (Projectile)sender;

            switch (e.PropertyName)
            {
                case Projectile.PROPNAME_POSITION3D:
                    // update viewerangle in case the projectile moved
                    obj.UpdateViewerAngle(new V2(viewerPosition.X, viewerPosition.Z));
                    break;
            }
        }

        /// <summary>
        /// Executed when a sector in the room moves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRoomSectorMoved(object sender, SectorMovedEventArgs e)
        {
            if (RoomInformation.ResourceRoom != null)
            {
                // look for objects in this sector and update their height
                foreach (RoomObject obj in RoomObjects)
                {
                    if (obj.SubSector != null && obj.SubSector.Sector == e.Sector)
                        obj.UpdateHeightPosition(RoomInformation);
                }
            }
        }

        /// <summary>
        /// Update Viewer dependent properties on RoomObjects
        /// based on ViewerPosition in DataController
        /// </summary>
        protected void ProcessViewerAngle()
        {
            // get 2D position of viewer
            V2 position = new V2(ViewerPosition.X, ViewerPosition.Z);

            // update roomobjects
            foreach (RoomObject obj in RoomObjects)
                obj.UpdateViewerAngle(position);

            // update projectiles
            foreach (Projectile obj in Projectiles)
                obj.UpdateViewerAngle(position);
        }
        #endregion

        #region Logging
        public void LogOutgoingPacket(GameMessage Message)
        {
            if (LogOutgoingMessages && (LogPingMessages || !((MessageTypeGameMode)Message.PI == MessageTypeGameMode.Ping)))
                GameMessageLog.Add(Message);
        }

        public void LogIncomingGameModeMessage(GameMessage Message)
        {
            if (LogIncomingMessages && (LogPingMessages || !((MessageTypeGameMode)Message.PI == MessageTypeGameMode.EchoPing)))
                GameMessageLog.Add(Message);
        }

        public void LogIncomingLoginModeMessage(GameMessage Message)
        {
            if (LogIncomingMessages)
                GameMessageLog.Add(Message);
        }
        #endregion

        #region Message handling
        /// <summary>
        /// Call with any incoming LoginModeMessage
        /// </summary>
        /// <param name="Message"></param>
        public void HandleIncomingLoginModeMessage(GameMessage Message)
        {
            // possibly log it
            LogIncomingLoginModeMessage(Message);

            // select the handler
            switch ((MessageTypeLoginMode)Message.PI)
            {
                case MessageTypeLoginMode.LoginOK:                  // 23
                    HandleLoginOK((LoginOKMessage)Message);
                    break;
            }
        }

        /// <summary>
        /// Call with any incoming GameModeMessage
        /// </summary>
        /// <param name="Message"></param>
        public void HandleIncomingGameModeMessage(GameMessage Message)
        {
            // possibly log it
            LogIncomingGameModeMessage(Message);

            // select the handler
            switch ((MessageTypeGameMode)Message.PI)
            {
                case MessageTypeGameMode.Wait:                      // 21
                    HandleWait((WaitMessage)Message);
                    break;

                case MessageTypeGameMode.Unwait:                    // 22
                    HandleUnwait((UnwaitMessage)Message);
                    break;

                case MessageTypeGameMode.SysMessage:                // 31
                    HandleSysMessage((SysMessageMessage)Message);
                    break;

                case MessageTypeGameMode.Message:                   // 32
                    HandleMessage((MessageMessage)Message);
                    break;

                case MessageTypeGameMode.CharInfoNotOk:             // 57
                    HandleCharInfoNotOKMessage((CharInfoNotOkMessage)Message);
                    break;

                case MessageTypeGameMode.Effect:                    // 70
                    HandleEffect((EffectMessage)Message);
                    break;

                case MessageTypeGameMode.Player:                    // 130
                    HandlePlayer((PlayerMessage)Message);
                    break;

                case MessageTypeGameMode.Stat:                      // 131
                    HandleStat((StatMessage)Message);
                    break;

                case MessageTypeGameMode.StatGroup:                 // 132
                    HandleStatGroup((StatGroupMessage)Message);
                    break;

                case MessageTypeGameMode.RoomContents:              // 134
                    HandleRoomContents((RoomContentsMessage)Message);
                    break;

                case MessageTypeGameMode.ObjectContents:            // 135
                    HandleObjectContents((ObjectContentsMessage)Message);
                    break;

                case MessageTypeGameMode.Players:                   // 136
                    HandlePlayers((PlayersMessage)Message);
                    break;

                case MessageTypeGameMode.PlayerAdd:                 // 137
                    HandlePlayerAdd((PlayerAddMessage)Message);
                    break;

                case MessageTypeGameMode.PlayerRemove:              // 138
                    HandlePlayerRemove((PlayerRemoveMessage)Message);
                    break;

                case MessageTypeGameMode.Characters:                // 139
                    HandleCharacters((CharactersMessage)Message);
                    break;

                case MessageTypeGameMode.CharInfo:                  // 140
                    HandleCharInfo((CharInfoMessage)Message);
                    break;

                case MessageTypeGameMode.Spells:                    // 141
                    HandleSpells((SpellsMessage)Message);
                    break;

                case MessageTypeGameMode.SpellAdd:                  // 142
                    HandleSpellAdd((SpellAddMessage)Message);
                    break;

                case MessageTypeGameMode.SpellRemove:               // 143
                    HandleSpellRemove((SpellRemoveMessage)Message);
                    break;

                case MessageTypeGameMode.AddEnchantment:            // 147
                    HandleAddEnchantment((AddEnchantmentMessage)Message);
                    break;

                case MessageTypeGameMode.RemoveEnchantment:         // 148
                    HandleRemoveEnchantment((RemoveEnchantmentMessage)Message);
                    break;

                case MessageTypeGameMode.Background:                // 150
                    HandleBackground((BackgroundMessage)Message);
                    break;

                case MessageTypeGameMode.PlayerOverlay:             // 151
                    HandlePlayerOverlay((PlayerOverlayMessage)Message);
                    break;

                case MessageTypeGameMode.AddBgOverlay:              // 152
                    HandleAddBgOverlay((AddBgOverlayMessage)Message);
                    break;

                case MessageTypeGameMode.ChangeBgOverlay:           // 154
                    HandleChangeBgOverlay((ChangeBgOverlayMessage)Message);
                    break;

                case MessageTypeGameMode.UserCommand:               // 155
                    HandleUserCommand((UserCommandMessage)Message);
                    break;
#if !VANILLA
                case MessageTypeGameMode.ReqStatChange:             // 156
                    HandleReqStatChange((ReqStatChangeMessage)Message);
                    break;
#endif
                case MessageTypeGameMode.Admin:                     // 162
                    HandleAdmin((AdminMessage)Message);
                    break;

                case MessageTypeGameMode.PlayWave:                  // 170
                    HandlePlayWave((PlayWaveMessage)Message);
                    break;

                case MessageTypeGameMode.PlayMusic:                 // 171
                    HandlePlayMusic((PlayMusicMessage)Message);
                    break;

                case MessageTypeGameMode.LookNewsGroup:             // 180
                    HandleLookNewsGroup((LookNewsGroupMessage)Message);
                    break;

                case MessageTypeGameMode.Articles:                  // 181
                    HandleArticles((ArticlesMessage)Message);
                    break;

                case MessageTypeGameMode.Article:                   // 182
                    HandleArticle((ArticleMessage)Message);
                    break;

                case MessageTypeGameMode.Move:                      // 200
                    HandleMove((MoveMessage)Message);
                    break;

                case MessageTypeGameMode.Turn:                      // 201
                    HandleTurn((TurnMessage)Message);
                    break;

                case MessageTypeGameMode.Shoot:                     // 202
                    HandleShoot((ShootMessage)Message);
                    break;

                case MessageTypeGameMode.Use:                       // 203
                    HandleUse((UseMessage)Message);
                    break;

                case MessageTypeGameMode.Unuse:                     // 204
                    HandleUnuse((UnuseMessage)Message);
                    break;

                case MessageTypeGameMode.UseList:                   // 205
                    HandleUseList((UseListMessage)Message);
                    break;

                case MessageTypeGameMode.Said:                      // 206
                    HandleSaid((SaidMessage)Message);
                    break;

                case MessageTypeGameMode.Look:                      // 207
                    HandleLook((LookMessage)Message);
                    break;

                case MessageTypeGameMode.Inventory:                 // 208
                    HandleInventory((InventoryMessage)Message);
                    break;

                case MessageTypeGameMode.InventoryAdd:              // 209
                    HandleInventoryAdd((InventoryAddMessage)Message);
                    break;

                case MessageTypeGameMode.InventoryRemove:           // 210
                    HandleInventoryRemove((InventoryRemoveMessage)Message);
                    break;

                case MessageTypeGameMode.Offer:                     // 211
                    HandleOffer((OfferMessage)Message);
                    break;

                case MessageTypeGameMode.OfferCanceled:             // 212
                    HandleOfferCanceled((OfferCanceledMessage)Message);
                    break;

                case MessageTypeGameMode.Offered:                   // 213
                    HandleOffered((OfferedMessage)Message);
                    break;

                case MessageTypeGameMode.CounterOffer:              // 214
                    HandleCounterOffer((CounterOfferMessage)Message);
                    break;

                case MessageTypeGameMode.CounterOffered:            // 215
                    HandleCounterOffered((CounterOfferedMessage)Message);
                    break;

                case MessageTypeGameMode.BuyList:                   // 216
                    HandleBuyList((BuyListMessage)Message);
                    break;

                case MessageTypeGameMode.Create:                    // 217
                    HandleCreate((CreateMessage)Message);
                    break;

                case MessageTypeGameMode.Remove:                    // 218
                    HandleRemove((RemoveMessage)Message);
                    break;

                case MessageTypeGameMode.Change:                    // 219
                    HandleChange((ChangeMessage)Message);
                    break;

                case MessageTypeGameMode.LightAmbient:              // 220
                    HandleLightAmbient((LightAmbientMessage)Message);
                    break;

                case MessageTypeGameMode.LightPlayer:               // 221
                    HandleLightPlayer((LightPlayerMessage)Message);
                    break;

                case MessageTypeGameMode.LightShading:              // 222
                    HandleLightShading((LightShadingMessage)Message);
                    break;

                case MessageTypeGameMode.InvalidateData:            // 228
                    HandleInvalidateData((InvalidateDataMessage)Message);
                    break;
            }
        }

        protected void HandleLoginOK(LoginOKMessage Message)
        {
            AccountType = Message.AccountType;
        }

        protected void HandlePlayers(PlayersMessage Message)
        {
            OnlinePlayers.Clear();
            OnlinePlayers.AddRange(Message.OnlinePlayers);
        }

        protected void HandlePlayerAdd(PlayerAddMessage Message)
        {
            OnlinePlayers.Add(Message.NewOnlinePlayer);
        }

        protected void HandlePlayerRemove(PlayerRemoveMessage Message)
        {
            OnlinePlayers.RemoveByID(Message.ObjectID);
        }

        protected void HandleCharacters(CharactersMessage Message)
        {
            WelcomeInfo.UpdateFromModel(Message.WelcomeInfo, true);
        }

        protected void HandleCharInfo(CharInfoMessage Message)
        {
            CharCreationInfo.UpdateFromModel(Message.CharCreationInfo, true);

            // set example datamodel to default
            CharCreationInfo.SetExampleModel();
        }

        protected void HandleRoomContents(RoomContentsMessage Message)
        {
            RoomObjects.Clear();

            foreach (RoomObject Model in Message.RoomObjects)
            {
                // set initial height from mapdata
                if (RoomInformation.ResourceRoom != null)
                    Model.UpdateHeightPosition(RoomInformation);

                // check if this is our avatar
                if (Model.ID == AvatarID)
                {
                    AvatarObject = Model;
                    Model.IsAvatar = true;
                }

                // reassign target if same id
                if (Model.ID == TargetID)
                    TargetObject = Model;

                // add to list
                RoomObjects.Add(Model);
            }
        }

        protected void HandleObjectContents(ObjectContentsMessage Message)
        {
            // update objectID value
            ObjectContents.ObjectID = Message.ObjectID;

            // clear and insert new items
            ObjectContents.Items.Clear();
            ObjectContents.Items.AddRange(Message.ContentObjects);

            // mark visible
            ObjectContents.IsVisible = true;
        }

        protected void HandleCreate(CreateMessage Message)
        {
            // set initial height from current mapdata
            if (RoomInformation.ResourceRoom != null)
                Message.NewRoomObject.UpdateHeightPosition(RoomInformation);

            // add to list
            RoomObjects.Add(Message.NewRoomObject);
        }

        protected void HandleRemove(RemoveMessage Message)
        {
            RoomObjects.RemoveByID(Message.ObjectID);

            // reset target if our target left
            if (TargetID == Message.ObjectID)
                TargetID = UInt32.MaxValue;

            // if tradepartner (or upcoming tradepartner) left room
            if (Trade.TradePartner != null && Trade.TradePartner.ID == Message.ObjectID)
                Trade.Clear(true);
        }

        protected void HandleChange(ChangeMessage Message)
        {
            RoomObject roomObject = RoomObjects.GetItemByID(Message.UpdatedObject.ID);
            if (roomObject != null)
            {
                roomObject.UpdateFromModel(Message.UpdatedObject, true);
            }
            else
            {
                ObjectBase inventoryObject = InventoryObjects.GetItemByID(Message.UpdatedObject.ID);
                if (inventoryObject != null)
                {
                    inventoryObject.UpdateFromModel(Message.UpdatedObject, true);
                }
            }
        }

        protected void HandleMove(MoveMessage Message)
        {
            RoomObject roomObject = RoomObjects.GetItemByID(Message.ObjectID);
            if (roomObject != null)
            {
                // create destination from values
                V2 destination = new V2(Message.NewCoordinateX, Message.NewCoordinateY);

                // initiate movement
                roomObject.StartMoveTo(destination, (byte)Message.MovementSpeed);
            }
        }

        protected void HandleTurn(TurnMessage Message)
        {
            RoomObject roomObject = RoomObjects.GetItemByID(Message.ObjectID);
            if (roomObject != null)
                roomObject.AngleUnits = Message.Angle;
        }

        protected void HandlePlayer(PlayerMessage Message)
        {
            // detach old sectormove listener
            if (RoomInformation.ResourceRoom != null)
                RoomInformation.ResourceRoom.SectorMoved -= OnRoomSectorMoved;

            // update
            RoomInformation.UpdateFromModel(Message.RoomInfo, true);

            // attach new sectormove listener
            if (RoomInformation.ResourceRoom != null)
                RoomInformation.ResourceRoom.SectorMoved += OnRoomSectorMoved;

            // update avatar id
            AvatarID = Message.RoomInfo.AvatarID;

            // clear roombuffs
            RoomBuffs.Clear();

            // clear projectiles
            Projectiles.Clear();

            // reset target
            TargetID = UInt32.MaxValue;

            // clear roomobjects
            RoomObjects.Clear();

            // clear trade & buyinfo (also hides)
            Trade.Clear(true);
            Buy.Clear(true);
            NewsGroup.Clear(true);
            LookObject.Clear(true);
            LookPlayer.Clear(true);

            // reset avatar object
            AvatarObject = null;
        }

        protected void HandleBackground(BackgroundMessage Message)
        {
            RoomInformation.BackgroundFileRID = Message.ResourceID.Value;
            RoomInformation.BackgroundFile = Message.ResourceID.Name;
        }

        protected void HandlePlayerOverlay(PlayerOverlayMessage Message)
        {
            // remove
            if (Message.HandItemObject.RenderPosition == PlayerOverlayHotspot.HOTSPOT_HIDE)
                PlayerOverlays.RemoveByID(Message.HandItemObject.ID);

            // add or update
            else
            {
                // try get existing
                PlayerOverlay ov = PlayerOverlays.GetItemByID(Message.HandItemObject.ID);

                // exists already
                if (ov != null)
                {
                    // same id, same hotspot -> update
                    if (ov.RenderPosition == Message.HandItemObject.RenderPosition)
                    {
                        ov.UpdateFromModel(Message.HandItemObject, true);
                    }

                    // same id, different hotspot
                    else
                    {
                        // WTF? can this happen?
                    }
                }

                // new id
                else
                {
                    PlayerOverlays.Add(Message.HandItemObject);
                }
            }
        }

        protected void HandleLightAmbient(LightAmbientMessage Message)
        {
            RoomInformation.AmbientLight = Message.RoomBrightness;
        }

        protected void HandleLightPlayer(LightPlayerMessage Message)
        {
            RoomInformation.AvatarLight = Message.CharacterBrightness;
        }

        protected void HandleLightShading(LightShadingMessage Message)
        {
            LightShading.UpdateFromModel(Message.LightShading, true);
        }

        protected void HandleInventory(InventoryMessage Message)
        {
            InventoryObjects.Clear();

            foreach (InventoryObject obj in Message.InventoryObjects)
            {
                InventoryObjects.Add(obj);

                // look up buttons which are assigned to this item
                // lookup for items by name is with issues (non unique)!
                foreach (ActionButtonConfig button in ActionButtons)
                {
                    if (button.ButtonType == ActionButtonType.Item &&
                        button.Name.ToLower() == obj.Name.ToLower() &&
                        button.NumOfSameName == obj.NumOfSameName)
                    {
                        button.SetToItem(obj);
                    }
                }
            }
        }

        protected void HandleInventoryAdd(InventoryAddMessage Message)
        {
            InventoryObjects.Add(Message.NewInventoryObject);

            // look up buttons which are assigned to this item
            // lookup for items by name is with issues (non unique)!
            foreach (ActionButtonConfig button in ActionButtons)
            {
                if (button.ButtonType == ActionButtonType.Item &&
                    button.Name.ToLower() == Message.NewInventoryObject.Name.ToLower() &&
                    button.NumOfSameName == Message.NewInventoryObject.NumOfSameName)
                {
                    button.SetToItem(Message.NewInventoryObject);
                }
            }
        }

        protected void HandleInventoryRemove(InventoryRemoveMessage Message)
        {
            InventoryObjects.RemoveByID(Message.ID.ID);

            // reset target if our target left
            if (TargetID == Message.ID.ID)
                TargetID = UInt32.MaxValue;
        }

        protected void HandleUseList(UseListMessage Message)
        {
            // mark inventory objects as in use
            foreach (ObjectID objectID in Message.EquippedObjects)
            {
                foreach (InventoryObject inventoryObject in InventoryObjects)
                {
                    if (objectID.ID == inventoryObject.ID)
                    {
                        inventoryObject.IsInUse = true;
                        break;
                    }
                }
            }
        }

        protected void HandleUnuse(UnuseMessage Message)
        {
            InventoryObject inventoryObject = InventoryObjects.GetItemByID(Message.ID.ID);

            if (inventoryObject != null)
                inventoryObject.IsInUse = false;
        }

        protected void HandleUse(UseMessage Message)
        {
            InventoryObject inventoryObject = InventoryObjects.GetItemByID(Message.NewEquippedItem.ID);

            if (inventoryObject != null)
                inventoryObject.IsInUse = true;
        }

        protected void HandleSpells(SpellsMessage Message)
        {
            SpellObjects.Clear();

            foreach (SpellObject spell in Message.SpellObjects)
            {
                SpellObjects.Add(spell);

                // look up buttons which are assigned to this spell
                foreach (ActionButtonConfig button in ActionButtons)
                {
                    if (button.ButtonType == ActionButtonType.Spell &&
                        button.Name.ToLower() == spell.Name.ToLower())
                    {
                        button.SetToSpell(spell);
                    }
                }
            }
        }

        protected void HandleSpellAdd(SpellAddMessage Message)
        {
            SpellObjects.Add(Message.NewSpellObject);

            // look up buttons which are assigned to this spell
            foreach (ActionButtonConfig button in ActionButtons)
            {
                if (button.ButtonType == ActionButtonType.Spell &&
                    button.Name.ToLower() == Message.NewSpellObject.Name.ToLower())
                {
                    button.SetToSpell(Message.NewSpellObject);
                }
            }
        }

        protected void HandleSpellRemove(SpellRemoveMessage Message)
        {
            SpellObjects.RemoveByID(Message.ID.ID);
        }

        protected void HandleStatGroup(StatGroupMessage Message)
        {
            switch (Message.Group)
            {
                case StatGroup.Condition:
                    AvatarCondition.Clear();
                    foreach(Stat stat in Message.Stats)
                        AvatarCondition.Add((StatNumeric)stat);
                    break;

                case StatGroup.Attributes:
                    AvatarAttributes.Clear();
                    foreach (Stat stat in Message.Stats)
                        AvatarAttributes.Add((StatNumeric)stat);
                    break;

                case StatGroup.Skills:
                    AvatarSkills.Clear();
                    foreach (Stat stat in Message.Stats)
                        AvatarSkills.Add((StatList)stat);
                    break;

                case StatGroup.Spells:
                    AvatarSpells.Clear();
                    foreach (Stat stat in Message.Stats)
                        AvatarSpells.Add((StatList)stat);
                    break;
#if !VANILLA
                case StatGroup.Quests:
                    AvatarQuests.Clear();
                    foreach (Stat stat in Message.Stats)
                        AvatarQuests.Add((StatList)stat);
                    break;
#endif
            }
        }

        protected void HandleStat(StatMessage Message)
        {
            StatNumeric newValueBarData;
            StatNumeric oldValueBarData;

            StatList newSkillEntry;
            StatList oldSkillEntry;

            switch (Message.Group)
            {
                case StatGroup.Condition:
                    newValueBarData = (StatNumeric)Message.Stat;
                    oldValueBarData = AvatarCondition.GetItemByNum(newValueBarData.Num);

                    if (oldValueBarData != null)
                    {
                        oldValueBarData.UpdateFromModel(newValueBarData, true);
                    }
                    break;

                case StatGroup.Attributes:
                    newValueBarData = (StatNumeric)Message.Stat;
                    oldValueBarData = AvatarAttributes.GetItemByNum(newValueBarData.Num);

                    if (oldValueBarData != null)
                    {
                        oldValueBarData.UpdateFromModel(newValueBarData, true);
                    }
                    break;

                case StatGroup.Skills:
                    newSkillEntry = (StatList)Message.Stat;
                    oldSkillEntry = AvatarSkills.GetItemByNum(newSkillEntry.Num);
                    if (oldSkillEntry != null)
                    {
                        oldSkillEntry.UpdateFromModel(newSkillEntry, true);
                    }
                    break;

                case StatGroup.Spells:
                    newSkillEntry = (StatList)Message.Stat;
                    oldSkillEntry = AvatarSpells.GetItemByNum(newSkillEntry.Num);
                    if (oldSkillEntry != null)
                    {
                        oldSkillEntry.UpdateFromModel(newSkillEntry, true);
                    }
                    break;
#if !VANILLA
                case StatGroup.Quests:
                    newSkillEntry = (StatList)Message.Stat;
                    oldSkillEntry = AvatarQuests.GetItemByNum(newSkillEntry.Num);
                    if (oldSkillEntry != null)
                    {
                        oldSkillEntry.UpdateFromModel(newSkillEntry, true);
                    }
                    break;
#endif
            }
        }

        protected void HandleAddEnchantment(AddEnchantmentMessage Message)
        {
            switch (Message.BuffType)
            {
                case BuffType.AvatarBuff:
                    AvatarBuffs.Add(Message.NewBuffObject);
                    break;

                case BuffType.RoomBuff:
                    RoomBuffs.Add(Message.NewBuffObject);
                    break;
            }
        }

        protected void HandleRemoveEnchantment(RemoveEnchantmentMessage Message)
        {
            switch (Message.BuffType)
            {
                case BuffType.AvatarBuff:
                    AvatarBuffs.RemoveByID(Message.ID);
                    break;

                case BuffType.RoomBuff:
                    RoomBuffs.RemoveByID(Message.ID);
                    break;
            }
        }

        protected void HandleMessage(MessageMessage Message)
        {
            if (ChatMessages.Count > ChatMessagesMaximum)
                ChatMessages.Remove(ChatMessages[0]);

            ChatMessages.Add(Message.Message);
        }

        protected void HandleCharInfoNotOKMessage(CharInfoNotOkMessage Message)
        {
            // mark as not OK
            CharCreationInfo.IsDataOK = false;
        }

        protected void HandleSaid(SaidMessage Message)
        {
            // try get player for this message
            OnlinePlayer player = OnlinePlayers.GetItemByID(Message.Message.SourceObjectID);

            // can't find player or player not on ignorelist
            if (player == null || !IgnoreList.Contains(player.Name))
            {
                if (ChatMessages.Count > ChatMessagesMaximum)
                    ChatMessages.Remove(ChatMessages[0]);

                ChatMessages.Add(Message.Message);
            }
        }

        protected void HandleSysMessage(SysMessageMessage Message)
        {
            if (ChatMessages.Count > ChatMessagesMaximum)
                ChatMessages.Remove(ChatMessages[0]);

            ChatMessages.Add(Message.Message);
        }

        protected void HandleUserCommand(UserCommandMessage Message)
        {
            switch (Message.Command.CommandType)
            {
                case UserCommandType.LookPlayer:
                    LookPlayer.UpdateFromModel(((UserCommandLookPlayer)Message.Command).PlayerInfo, true);
                    LookPlayer.IsVisible = true;
                    break;

                case UserCommandType.GuildInfo:
                    GuildInfo.UpdateFromModel(((UserCommandGuildInfo)Message.Command).GuildInfo, true);
                    GuildInfo.IsVisible = true;
                    break;
#if !VANILLA
                case UserCommandType.ReceivePreferences:
                    ClientPreferences.UpdateFromModel(((UserCommandReceivePreferences)Message.Command).ClientPreferences, true);
                    ClientPreferences.Enabled = true;
                    break;
#endif
                case UserCommandType.GuildShield:
                    // this can either be GuildShieldInfo or GuildshieldInfoReq
                    if (Message.Command is UserCommandGuildShieldInfo)
                    {
                        GuildShieldInfo.UpdateFromModel(((UserCommandGuildShieldInfo)Message.Command).ShieldInfo, true);
                    }
                    break;

                case UserCommandType.GuildAsk:
                    GuildAskData.UpdateFromModel(((UserCommandGuildAsk)Message.Command).Data, true);
                    GuildAskData.IsVisible = true;
                    break;

                case UserCommandType.GuildShields:
                    // this can either bei GuildShieldList or GuildShieldListReq
                    if (Message.Command is UserCommandGuildShieldList)
                    {
                        UserCommandGuildShieldList commandShieldList = (UserCommandGuildShieldList)Message.Command;
                        GuildShieldInfo.Shields = commandShieldList.ShieldResources;
                    }
                    break;

                case UserCommandType.GuildList:
                    DiplomacyInfo.UpdateFromModel(((UserCommandGuildGuildList)Message.Command).DiplomacyInfo, true);
                    break;
            }
        }

#if !VANILLA
        protected void HandleReqStatChange(ReqStatChangeMessage Message)
        {
            // update own instance with new values
            StatChangeInfo.UpdateFromModel(Message.StatChangeInfo, true);
            StatChangeInfo.IsVisible = true;
        }
#endif

        protected void HandleAddBgOverlay(AddBgOverlayMessage Message)
        {
            BackgroundOverlays.Add(Message.BackgroundOverlay);
        }

        protected void HandleChangeBgOverlay(ChangeBgOverlayMessage Message)
        {
            BackgroundOverlay bgOverlay = BackgroundOverlays.GetItemByID(Message.BackgroundOverlay.ID);

            if (bgOverlay != null)
                bgOverlay.UpdateFromModel(Message.BackgroundOverlay, true);
        }

        protected void HandleAdmin(AdminMessage Message)
        {
            AdminInfo.ProcessServerResponse(Message.Message);
        }

        protected void HandlePlayWave(PlayWaveMessage Message)
        {
            // care only for sounds played with source object id
            if (Message.PlayInfo.ID > 0)
            {
                // try get source object
                RoomObject roomObj = RoomObjects.GetItemByID(Message.PlayInfo.ID);

                if (roomObj == null)
                    return;

                // see if this sound was a known ouch sound
                HealthStatus ouch = ResourceStrings.Sounds.IsOuch(Message.PlayInfo.ResourceName);

                // not care about anything else than ouch
                if (ouch == HealthStatus.Unknown)
                    return;

                // otherwise set healthstatus
                roomObj.HealthStatus = ouch;
            }
        }

        protected void HandlePlayMusic(PlayMusicMessage Message)
        {
            BackgroundMusic.UpdateFromModel(Message.PlayInfo, true);
        }

        protected void HandleShoot(ShootMessage Message)
        {
            Projectile projectile = Message.Projectile;

            // resolve the source and targetobject references of the projectile
            projectile.ResolveSourceTarget(RoomObjects, false);

            // must have valid source and target
            if (projectile.TargetObject != null && projectile.SourceObject != null)
            {
                // add to list
                Projectiles.Add(Message.Projectile);

                // trigger movement start
                projectile.StartMove();
            }
        }

        protected void HandleLook(LookMessage Message)
        {
            LookObject.UpdateFromModel(Message.ObjectInfo, true);

            // set visible if not
            lookObject.IsVisible = true;
        }

        protected void HandleLookNewsGroup(LookNewsGroupMessage Message)
        {
            NewsGroup.UpdateFromModel(Message.NewsGroup, true);

            NewsGroup.IsVisible = true;
        }

        protected void HandleArticles(ArticlesMessage Message)
        {
            NewsGroup.Articles.AddRange(Message.Articles);
        }

        protected void HandleArticle(ArticleMessage Message)
        {
            NewsGroup.Text = Message.Message;
        }

        protected void HandleInvalidateData(InvalidateDataMessage Message)
        {
            // clear all invalid data
            Invalidate();
        }

        protected void HandleEffect(EffectMessage Message)
        {
            Effects.HandleEffect(Message.Effect);
        }

        protected void HandleWait(WaitMessage Message)
        {
            IsWaiting = true;
        }

        protected void HandleUnwait(UnwaitMessage Message)
        {
            IsWaiting = false;
        }

        protected void HandleCounterOffer(CounterOfferMessage Message)
        {
            Trade.ItemsPartner.Clear();
            Trade.ItemsPartner.AddRange(Message.OfferedItems);

            Trade.IsVisible = true;
            Trade.IsItemsPartnerSet = true;
            Trade.IsPending = true;
        }

        protected void HandleCounterOffered(CounterOfferedMessage Message)
        {
            Trade.ItemsYou.Clear();
            Trade.ItemsYou.AddRange(Message.OfferedItems);

            Trade.IsVisible = true;
            Trade.IsItemsYouSet = true;
            Trade.IsPending = true;
        }

        protected void HandleOffered(OfferedMessage Message)
        {
            Trade.ItemsYou.Clear();
            Trade.ItemsYou.AddRange(Message.OfferedItems);

            Trade.IsVisible = true;
            Trade.IsItemsYouSet = true;
            Trade.IsPending = true;
        }

        protected void HandleOffer(OfferMessage Message)
        {
            // this is a background trade start
            Trade.Clear(true);

            Trade.IsBackgroundOffer = true;
            Trade.TradePartner = Message.TradePartner;

            Trade.ItemsPartner.AddRange(Message.OfferedItems);

            Trade.IsVisible = false;
            Trade.IsItemsYouSet = false;
            Trade.IsItemsPartnerSet = true;
            Trade.IsPending = true;
        }

        protected void HandleOfferCanceled(OfferCanceledMessage Message)
        {
            Trade.Clear(true);
        }

        protected void HandleBuyList(BuyListMessage Message)
        {
            // set tradepartner
            Buy.TradePartner = Message.TradePartner;

            // clear old offered items
            Buy.Items.Clear();

            // add his items
            Buy.Items.AddRange(Message.OfferedItems);

            // set visible
            Buy.IsVisible = true;
        }
        #endregion
    }
}
