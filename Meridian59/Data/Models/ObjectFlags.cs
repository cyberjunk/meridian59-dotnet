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
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Wrapper around 28-Bit ObjectFlags (for VANILLA), or with several extended datavalues (OpenMeridian).
    /// </summary>
    /// <remarks>
    /// * Blakserv is using 28-Bit (but 32-Bit are shown here, so upper 4 bits are always 0)
    /// * Bitmasks sections marked with ENUM: Must interpret all bits together as type value
    /// * Bitmasks sections marked with BOOLS: Must interpret each bit as boolean
    /// * See official manual pages 92-93 (is a bit outdated)
    /// * Definitions for C in 'include\proto.h'
    /// * Definitions for KOD in 'kod\include\blakston.khd'
    /// </remarks>
    [Serializable]
    public class ObjectFlags : Flags, IUpdatable<ObjectFlags>
    {
        //////////////////////////////////////////////////////////////////////////////////
        //////////////////////// IMPLEMENTATION FOR OPENMERIDIAN /////////////////////////
#if !VANILLA      
        #region CONSTANTS
        // DEDICATED BYTE - ENUM (MoveOn Type)
        private const uint OF_MOVEON_YES        = 0x00000000;    // Can always move on object
        private const uint OF_MOVEON_NO         = 0x00000001;    // Can never move on object
        private const uint OF_MOVEON_TELEPORTER = 0x00000002;    // Can move on object, but then kod will move you elsewhere
        private const uint OF_MOVEON_NOTIFY     = 0x00000003;
        
        // BOOLS - OLD FLAGS
        private const uint OF_PLAYER            = 0x00000004;    // Set if object is a player
        private const uint OF_ATTACKABLE        = 0x00000008;    // Set if object is legal target for an attack
        private const uint OF_GETTABLE          = 0x00000010;    // Set if player can try to pick up object
        private const uint OF_CONTAINER         = 0x00000020;    // Set if player can put objects inside this one
        private const uint OF_NOEXAMINE         = 0x00000040;    // Set if player CAN'T examine object
        private const uint OF_ITEM_MAGIC        = 0x00000080;    // Set for magic item to color in lists
        private const uint OF_HANGING           = 0x00000100;    // Ceiling pinned objects like kriipa
        private const uint OF_OFFERABLE         = 0x00000200;    // Set if object can be offered to
        private const uint OF_BUYABLE           = 0x00000400;    // Set if object can be bought from
        private const uint OF_ACTIVATABLE       = 0x00000800;    // Set if object can be activated
        private const uint OF_APPLYABLE         = 0x00001000;    // Set if object can be applied to another object
        private const uint OF_SAFETY            = 0x00002000;    // Set if player has safety on (self only)
        private const uint OF_BOUNCING          = 0x00010000;    // If both flags on then object is bouncing
        private const uint OF_FLICKERING        = 0x00020000;    // For players or objects if holding a flickering light.
        private const uint OF_FLASHING          = 0x00040000;    // For players or objects if flashing with light.
        private const uint OF_PHASING           = 0x00080000;    // For players or objects if phasing translucent/solid.

        // DEDICATED BYTE - ENUM (Player Type)
        private const uint OT_NONE              = 0x00000000;    // Default for most objects.
        private const uint OT_KILLER            = 0x00000001;    // Set if object is a killer (must also have OF_PLAYER)
        private const uint OT_OUTLAW            = 0x00000002;    // Set if object is an outlaw (must also have OF_PLAYER)
        private const uint OT_DM                = 0x00000003;    // Set if object is a DM player
        private const uint OT_CREATOR           = 0x00000004;    // Set if object is a creator player
        private const uint OT_SUPER             = 0x00000005;    // Set if object is a "super DM"
        private const uint OT_MODERATOR         = 0x00000006;    // Set if object is a "moderator"
        private const uint OT_EVENTCHAR         = 0x00000007;    // Set if object is an event character

        // DEDICATED BYTE - ENUM (Drawing Type)
        private const uint DRAWFX_DRAW_PLAIN    = 0x00000000;    // No special drawing effects
        private const uint DRAWFX_TRANSLUCENT25 = 0x00000001;    // Set if object should be drawn at 25% opacity
        private const uint DRAWFX_TRANSLUCENT50 = 0x00000002;    // Set if object should be drawn at 50% opacity
        private const uint DRAWFX_TRANSLUCENT75 = 0x00000003;    // Set if object should be drawn at 75% opacity
        private const uint DRAWFX_BLACK         = 0x00000004;    // Set if object should be drawn all black
        private const uint DRAWFX_INVISIBLE     = 0x00000005;    // Set if object should be drawn with invisibility effect
        private const uint DRAWFX_TRANSLATE     = 0x00000006;    // Reserved (used internally by client)
        private const uint DRAWFX_DITHERINVIS   = 0x00000007;    // Haze (dither with transparency) 50% of pixels
        private const uint DRAWFX_DITHERTRANS   = 0x00000008;    // Dither (with two translates) 50% of pixels
        private const uint DRAWFX_DOUBLETRANS   = 0x00000009;    // Translate twice each pixel, plus lighting
        private const uint DRAWFX_SECONDTRANS   = 0x0000000A;    // Ignore per-overlay xlat and use only secondary xlat

        // DEDICATED UINT - MINIMAP BOOLS
        private const uint MM_NONE          = 0x00000000; // No dot (default for all objects)
	    private const uint MM_PLAYER        = 0x00000001; // Standard blue player dot
	    private const uint MM_ENEMY         = 0x00000002; // Enemy (halo or attackable) player
	    private const uint MM_FRIEND        = 0x00000004; // Friendly (guild ally) player
	    private const uint MM_GUILDMATE     = 0x00000008; // Guildmate player
	    private const uint MM_BUILDER_GROUP = 0x00000010; // Player is in same building group
	    private const uint MM_MONSTER       = 0x00000020; // Default monster dot
	    private const uint MM_NPC           = 0x00000040; // NPC
	    private const uint MM_MINION_OTHER  = 0x00000080; // Set if monster is other's minion
        private const uint MM_MINION_SELF   = 0x00000100; // Set if a monster is our minion
        private const uint MM_TEMPSAFE      = 0x00000200; // Set if player has a temporary angel.
        private const uint MM_MINIBOSS      = 0x00000400; // Set if mob is a miniboss (survival arena).
        private const uint MM_BOSS          = 0x00000800; // Set if mob is a boss (survival arena).
        #endregion

        #region Enums
        /// <summary>
        /// Possible MoveOnTypes
        /// </summary>
        public enum MoveOnType : uint
        {
            Yes             = OF_MOVEON_YES,
            No              = OF_MOVEON_NO,
            Teleport        = OF_MOVEON_TELEPORTER,
            Notify          = OF_MOVEON_NOTIFY
        }

        /// <summary>
        /// Possible PlayerTypes
        /// </summary>
        public enum PlayerType : uint
        {
            None            = OT_NONE,
            Killer          = OT_KILLER,
            Outlaw          = OT_OUTLAW,
            DM              = OT_DM,
            Creator         = OT_CREATOR,
            SuperDM         = OT_SUPER,
            Moderator       = OT_MODERATOR,
            EventChar       = OT_EVENTCHAR
        }

        /// <summary>
        /// Possible DrawingTypes
        /// </summary>
        public enum DrawingType : uint
        {
            Plain           = DRAWFX_DRAW_PLAIN,
            Translucent25   = DRAWFX_TRANSLUCENT25,
            Translucent50   = DRAWFX_TRANSLUCENT50,
            Translucent75   = DRAWFX_TRANSLUCENT75,
            Black           = DRAWFX_BLACK,
            Invisible       = DRAWFX_INVISIBLE,
            Translate       = DRAWFX_TRANSLATE,
            DitherInvis     = DRAWFX_DITHERINVIS,
            DitherTrans     = DRAWFX_DITHERTRANS,
            DoubleTrans     = DRAWFX_DOUBLETRANS,
            SecondTrans     = DRAWFX_SECONDTRANS
        }
        #endregion

        protected DrawingType drawing;
        protected uint minimap;
        protected uint namecolor;
        protected PlayerType player;
        protected MoveOnType moveon;

        /// <summary>
        /// 32-Bit name color
        /// </summary>
        public uint NameColor
        {
            get { return namecolor; }

            set
            {
                if (namecolor != value)
                {
                    namecolor = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// Minimap flags
        /// </summary>
        public uint Minimap
        {
            get { return minimap; }

            set
            {
                if (minimap != value)
                {
                    minimap = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }
        
        /// <summary>
        /// MoveOnType as embedded in this flags
        /// </summary>
        public MoveOnType MoveOn
        {
            get { return moveon; }

            set
            {
                if (moveon != value)
                {
                    moveon = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// PlayerType as embedded in this flags
        /// </summary>
        public PlayerType Player
        {
            get { return player; }

            set
            {
                if (player != value)
                {
                    player = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// DrawingType as embedded in this flags
        /// </summary>
        public DrawingType Drawing
        {
            get { return drawing; }

            set
            {
                if (drawing != value)
                {
                    drawing = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Old flags</param>
        /// <param name="Drawing">Dedicated drawing type</param>
        /// <param name="Minimap">Dedicated minimap flags</param>
        /// <param name="NameColor">Dedicated name color</param>
        /// <param name="Player">Dedicated player type</param>
        /// <param name="MoveOn">Dedicated moveon type</param>
        public ObjectFlags(
            uint Value = 0,
            DrawingType Drawing = DrawingType.Plain,
            uint Minimap = 0,
            uint NameColor = 0,
            PlayerType Player = PlayerType.None,
            MoveOnType MoveOn = MoveOnType.Yes)
            : base(Value)
        {
            drawing = Drawing;
            minimap = Minimap;
            namecolor = NameColor;
            player = Player;
            moveon = MoveOn;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public ObjectFlags(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe ObjectFlags(ref byte* Buffer)
            : base(ref Buffer) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                drawing = DrawingType.Plain;
                minimap = 0;
                namecolor = 0;
                player = PlayerType.None;
                moveon = MoveOnType.Yes;
            }
            else
            {
                Drawing = DrawingType.Plain;
                Minimap = 0;
                NameColor = 0;
                Player = PlayerType.None;
                MoveOn = MoveOnType.Yes;
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Flags"></param>
        /// <param name="RaiseChangedEvent"></param>
        public void UpdateFromModel(ObjectFlags Flags, bool RaiseChangedEvent)
        {
            if (Flags == null)
                return;

            if (RaiseChangedEvent)
            {
                Value = Flags.Value;
                Drawing = Flags.Drawing;
                Minimap = Flags.Minimap;
                NameColor = Flags.NameColor;
                Player = Flags.Player;
                MoveOn = Flags.MoveOn;
            }
            else
            {
                flags = Flags.Value;
                drawing = Flags.Drawing;
                minimap = Flags.Minimap;
                namecolor = Flags.NameColor;
                player = Flags.Player;
                moveon = Flags.MoveOn;
            }
        }

        #region IByteSerializable
        public override int ByteLength
        {
            get { return base.ByteLength + TypeSizes.BYTE + TypeSizes.INT + TypeSizes.INT + TypeSizes.BYTE + TypeSizes.BYTE; }
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            drawing = (DrawingType)Buffer[cursor];
            cursor++;

            minimap = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            namecolor = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            player = (PlayerType)Buffer[cursor];
            cursor++;

            moveon = (MoveOnType)Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            drawing = (DrawingType)Buffer[0];
            Buffer++;

            minimap = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            namecolor = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            player = (PlayerType)Buffer[0];
            Buffer++;

            moveon = (MoveOnType)Buffer[0];
            Buffer++;
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Buffer[cursor] = (byte)drawing;
            cursor++;

            Array.Copy(BitConverter.GetBytes(minimap), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(namecolor), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = (byte)player;
            cursor++;

            Buffer[cursor] = (byte)moveon;
            cursor++;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            Buffer[0] = (byte)drawing;
            Buffer++;

            *((uint*)Buffer) = minimap;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = namecolor;
            Buffer += TypeSizes.INT;

            Buffer[0] = (byte)player;
            Buffer++;

            Buffer[0] = (byte)moveon;
            Buffer++;
        }
        #endregion

        #region Bools
        /// <summary>
        /// True if the object is a player.
        /// </summary>
        public bool IsPlayer
        {
            get { return (flags & OF_PLAYER) == OF_PLAYER; }
            set
            {
                if (value) flags |= OF_PLAYER;
                else flags &= ~OF_PLAYER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is attackable.
        /// </summary>
        public bool IsAttackable
        {
            get { return (flags & OF_ATTACKABLE) == OF_ATTACKABLE; }
            set
            {
                if (value) flags |= OF_ATTACKABLE;
                else flags &= ~OF_ATTACKABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is gettable.
        /// </summary>
        public bool IsGettable
        {
            get { return (flags & OF_GETTABLE) == OF_GETTABLE; }
            set
            {
                if (value) flags |= OF_GETTABLE;
                else flags &= ~OF_GETTABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a container.
        /// </summary>
        public bool IsContainer
        {
            get { return (flags & OF_CONTAINER) == OF_CONTAINER; }
            set
            {
                if (value) flags |= OF_CONTAINER;
                else flags &= ~OF_CONTAINER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can't be examined.
        /// </summary>
        public bool IsNoExamine
        {
            get { return (flags & OF_NOEXAMINE) == OF_NOEXAMINE; }
            set
            {
                if (value) flags |= OF_NOEXAMINE;
                else flags &= ~OF_NOEXAMINE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a magic item.
        /// </summary>
        public bool IsMagicItem
        {
            get { return (flags & OF_ITEM_MAGIC) == OF_ITEM_MAGIC; }
            set
            {
                if (value) flags |= OF_ITEM_MAGIC;
                else flags &= ~OF_ITEM_MAGIC;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is hanging from the ceiling.
        /// Note: For VANILLA this overlaps with PF_CREATOR
        /// </summary>
        public bool IsHanging
        {
            get { return (flags & OF_HANGING) == OF_HANGING; }
            set
            {
                if (value) flags |= OF_HANGING;
                else flags &= ~OF_HANGING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can be offered to.
        /// </summary>
        public bool IsOfferable
        {
            get { return (flags & OF_OFFERABLE) == OF_OFFERABLE; }
            set
            {
                if (value) flags |= OF_OFFERABLE;
                else flags &= ~OF_OFFERABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if you can buy from the object.
        /// </summary>
        public bool IsBuyable
        {
            get { return (flags & OF_BUYABLE) == OF_BUYABLE; }
            set
            {
                if (value) flags |= OF_BUYABLE;
                else flags &= ~OF_BUYABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can be activated.
        /// </summary>
        public bool IsActivatable
        {
            get { return (flags & OF_ACTIVATABLE) == OF_ACTIVATABLE; }
            set
            {
                if (value) flags |= OF_ACTIVATABLE;
                else flags &= ~OF_ACTIVATABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can be applied.
        /// </summary>
        public bool IsApplyable
        {
            get { return (flags & OF_APPLYABLE) == OF_APPLYABLE; }
            set
            {
                if (value) flags |= OF_APPLYABLE;
                else flags &= ~OF_APPLYABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object has safety on.
        /// </summary>
        public bool IsSafety
        {
            get { return (flags & OF_SAFETY) == OF_SAFETY; }
            set
            {
                if (value) flags |= OF_SAFETY;
                else flags &= ~OF_SAFETY;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is flickering.
        /// </summary>
        public bool IsFlickering
        {
            get { return (flags & OF_FLICKERING) == OF_FLICKERING; }
            set
            {
                if (value) flags |= OF_FLICKERING;
                else flags &= ~OF_FLICKERING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is flashing.
        /// </summary>
        public bool IsFlashing
        {
            get { return (flags & OF_FLASHING) == OF_FLASHING; }
            set
            {
                if (value) flags |= OF_FLASHING;
                else flags &= ~OF_FLASHING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is bouncing.
        /// Note: Overlaps. Bouncing if Flickering AND Flashing true
        /// </summary>
        public bool IsBouncing
        {
            get { return (flags & OF_BOUNCING) == OF_BOUNCING; }
            set
            {
                if (value) flags |= OF_BOUNCING;
                else flags &= ~OF_BOUNCING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is phasing.
        /// </summary>
        public bool IsPhasing
        {
            get { return (flags & OF_PHASING) == OF_PHASING; }
            set
            {
                if (value) flags |= OF_PHASING;
                else flags &= ~OF_PHASING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region Bools - Minimap
        /// <summary>
        /// True for a standard blue player dot in minimap.
        /// </summary>
        public bool IsMinimapPlayer
        {
            get { return (minimap & MM_PLAYER) == MM_PLAYER; }
            set
            {
                if (value) minimap |= MM_PLAYER;
                else minimap &= ~MM_PLAYER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is an enemy player.
        /// </summary>
        public bool IsMinimapEnemy
        {
            get { return (minimap & MM_ENEMY) == MM_ENEMY; }
            set
            {
                if (value) minimap |= MM_ENEMY;
                else minimap &= ~MM_ENEMY;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a friendly player.
        /// </summary>
        public bool IsMinimapFriend
        {
            get { return (minimap & MM_FRIEND) == MM_FRIEND; }
            set
            {
                if (value) minimap |= MM_FRIEND;
                else minimap &= ~MM_FRIEND;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a guildmate.
        /// </summary>
        public bool IsMinimapGuildMate
        {
            get { return (minimap & MM_GUILDMATE) == MM_GUILDMATE; }
            set
            {
                if (value) minimap |= MM_GUILDMATE;
                else minimap &= ~MM_GUILDMATE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the minimap should show buildergroup dot
        /// </summary>
        public bool IsMinimapBuilderGroup
        {
            get { return (minimap & MM_BUILDER_GROUP) == MM_BUILDER_GROUP; }
            set
            {
                if (value) minimap |= MM_BUILDER_GROUP;
                else minimap &= ~MM_BUILDER_GROUP;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object should get a monster minimap dot.
        /// </summary>
        public bool IsMinimapMonster
        {
            get { return (minimap & MM_MONSTER) == MM_MONSTER; }
            set
            {
                if (value) minimap |= MM_MONSTER;
                else minimap &= ~MM_MONSTER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object should get a NPC minimap dot.
        /// </summary>
        public bool IsMinimapNPC
        {
            get { return (minimap & MM_NPC) == MM_NPC; }
            set
            {
                if (value) minimap |= MM_NPC;
                else minimap &= ~MM_NPC;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Set if a monster is our minion
        /// </summary>
        public bool IsMinimapMinionSelf
        {
            get { return (minimap & MM_MINION_SELF) == MM_MINION_SELF; }
            set
            {
                if (value) minimap |= MM_MINION_SELF;
                else minimap &= ~MM_MINION_SELF;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Set if monster is other's minion
        /// </summary>
        public bool IsMinimapMinionOther
        {
            get { return (minimap & MM_MINION_OTHER) == MM_MINION_OTHER; }
            set
            {
                if (value) minimap |= MM_MINION_OTHER;
                else minimap &= ~MM_MINION_OTHER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Set if player has a temporary angel.
        /// </summary>
        public bool IsMinimapTempSafe
        {
            get { return (minimap & MM_TEMPSAFE) == MM_TEMPSAFE; }
            set
            {
                if (value) minimap |= MM_TEMPSAFE;
                else minimap &= ~MM_TEMPSAFE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Set if mob is a miniboss (survival arena).
        /// </summary>
        public bool IsMinimapMiniBoss
        {
            get { return (minimap & MM_MINIBOSS) == MM_MINIBOSS; }
            set
            {
                if (value) minimap |= MM_MINIBOSS;
                else minimap &= ~MM_MINIBOSS;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Set if mob is a boss (survival arena).
        /// </summary>
        public bool IsMinimapBoss
        {
            get { return (minimap & MM_BOSS) == MM_BOSS; }
            set
            {
                if (value) minimap |= MM_BOSS;
                else minimap &= ~MM_BOSS;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        /// <summary>
        /// Returns true if all bits from Flags parameter
        /// are also set in the current value or if Flags
        /// paramter is null.
        /// </summary>
        /// <param name="Flags"></param>
        /// <returns></returns>
        public bool IsSubset(ObjectFlags Flags)
        {
            return (Flags == null || (
                (flags & Flags.Value) == Flags.Value &&
                (drawing & Flags.Drawing) == Flags.Drawing &&
                (minimap & Flags.Minimap) == Flags.Minimap &&
                (namecolor & Flags.NameColor) == Flags.NameColor &&
                (player & Flags.Player) == Flags.Player &&
                (moveon & Flags.MoveOn) == Flags.MoveOn));
        }

        /// <summary>
        /// True if attackable, not a player and moveon=no
        /// </summary>     
        public bool IsCreature
        {
            get { return MoveOn == MoveOnType.No && IsAttackable && !IsPlayer; }
        }


        //////////////////////////////////////////////////////////////////////////////////
        /////////////////////////// IMPLEMENTATION FOR VANILLA ///////////////////////////
#else
        #region CONSTANTS
        // BITS: 0-1       ENUM (MoveOn Type)
        private const uint OF_MOVEON_YES        = 0x00000000;    // Can always move on object
        private const uint OF_MOVEON_NO         = 0x00000001;    // Can never move on object
        private const uint OF_MOVEON_TELEPORTER = 0x00000002;    // Can move on object, but then kod will move you elsewhere
        private const uint OF_MOVEON_NOTIFY     = 0x00000003;
        private const uint OF_NOMOVEON_MASK     = 0x00000003;

        // BITS: 2-13      BOOLS 
        private const uint OF_PLAYER            = 0x00000004;    // Set if object is a player
        private const uint OF_ATTACKABLE        = 0x00000008;    // Set if object is legal target for an attack
        private const uint OF_GETTABLE          = 0x00000010;    // Set if player can try to pick up object
        private const uint OF_CONTAINER         = 0x00000020;    // Set if player can put objects inside this one
        private const uint OF_NOEXAMINE         = 0x00000040;    // Set if player CAN'T examine object
        private const uint OF_OFFERABLE         = 0x00000200;    // Set if object can be offered to
        private const uint OF_BUYABLE           = 0x00000400;    // Set if object can be bought from
        private const uint OF_ACTIVATABLE       = 0x00000800;    // Set if object can be activated
        private const uint OF_APPLYABLE         = 0x00001000;    // Set if object can be applied to another object
        private const uint OF_SAFETY            = 0x00002000;    // Set if player has safety on (self only)

        // BITS: 14-16     ENUM (Player Type)
        private const uint PF_KILLER            = 0x00004000;    // Set if object is a killer (must also have OF_PLAYER)
        private const uint PF_OUTLAW            = 0x00008000;    // Set if object is an outlaw (must also have OF_PLAYER)
        private const uint PF_DM                = 0x0000C000;    // Set if object is a DM player
        private const uint PF_CREATOR           = 0x00010000;    // Set if object is a creator player
        private const uint PF_SUPER             = 0x00014000;    // Set if object is a "super DM"
        private const uint PF_EVENTCHAR         = 0x0001C000;    // Set if object is an event character
        private const uint OF_PLAYER_MASK       = 0x0001C000;    // Mask to get player flag bits

        // BITS: 17-19     BOOLS 
        private const uint OF_FLICKERING        = 0x00020000;    // For players or objects if holding a flickering light.
        private const uint OF_FLASHING          = 0x00040000;    // For players or objects if flashing with light.
        private const uint OF_BOUNCING          = 0x00060000;    // If both flags on then object is bouncing
        private const uint OF_PHASING           = 0x00080000;    // For players or objects if phasing translucent/solid.

        // BITS: 20-24     ENUM (Drawing Type)
        private const uint OF_DRAW_PLAIN         = 0x00000000;    // No special drawing effects
        private const uint OF_TRANSLUCENT25      = 0x00100000;    // Set if object should be drawn at 25% opacity
        private const uint OF_TRANSLUCENT50      = 0x00200000;    // Set if object should be drawn at 50% opacity
        private const uint OF_TRANSLUCENT75      = 0x00300000;    // Set if object should be drawn at 75% opacity
        private const uint OF_BLACK              = 0x00400000;    // Set if object should be drawn all black
        private const uint OF_INVISIBLE          = 0x00500000;    // Set if object should be drawn with invisibility effect
        private const uint OF_TRANSLATE          = 0x00600000;    // Reserved (used internally by client)
        private const uint OF_DITHERINVIS        = 0x00700000;    // Haze (dither with transparency) 50% of pixels
        private const uint OF_DITHERTRANS        = 0x00800000;    // Dither (with two translates) 50% of pixels
        private const uint OF_DOUBLETRANS        = 0x00900000;    // Translate twice each pixel, plus lighting
        private const uint OF_SECONDTRANS        = 0x00A00000;    // Ignore per-overlay xlat and use only secondary xlat
        private const uint OF_EFFECT_MASK        = 0x01F00000;    // Mask to get object drawing effect bits
        
        // BITS: 25-27     BOOLS
        private const uint OF_ENEMY              = 0x02000000;    // Enemy player
        private const uint OF_FRIEND             = 0x04000000;    // Friendly player
        private const uint OF_GUILDMATE          = 0x08000000;    // Guildmate player

        // MIXED / OVERLAPPING      
        private const uint OF_HANGING            = 0x00010000;    // Overlaps with PF_CREATOR.
                                                                  //     FALSE for most objects.
                                                                  //     TRUE for creators or ceiling-pinned objects.
        #endregion

        #region Enums
        /// <summary>
        /// Possible MoveOnTypes
        /// </summary>
        public enum MoveOnType : uint
        {
            Yes             = OF_MOVEON_YES,
            No              = OF_MOVEON_NO,
            Teleport        = OF_MOVEON_TELEPORTER,
            Notify          = OF_MOVEON_NOTIFY
        }

        /// <summary>
        /// Possible PlayerTypes
        /// </summary>
        public enum PlayerType : uint
        {
            Killer          = PF_KILLER,
            Outlaw          = PF_OUTLAW,
            DM              = PF_DM,
            Creator         = PF_CREATOR,
            SuperDM         = PF_SUPER,
            EventChar       = PF_EVENTCHAR
        }

        /// <summary>
        /// Possible DrawingTypes
        /// </summary>
        public enum DrawingType : uint
        {
            Plain           = OF_DRAW_PLAIN,
            Translucent25   = OF_TRANSLUCENT25,
            Translucent50   = OF_TRANSLUCENT50,
            Translucent75   = OF_TRANSLUCENT75,
            Black           = OF_BLACK,
            Invisible       = OF_INVISIBLE,
            Translate       = OF_TRANSLATE,
            DitherInvis     = OF_DITHERINVIS,
            DitherTrans     = OF_DITHERTRANS,
            DoubleTrans     = OF_DOUBLETRANS,
            SecondTrans     = OF_SECONDTRANS
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value"></param>
        public ObjectFlags(uint Value = 0)
            : base(Value) { }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public ObjectFlags(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe ObjectFlags(ref byte* Buffer)
            : base(ref Buffer) { }

        #region SECTION 1 - MoveOnType
        /// <summary>
        /// MoveOnType as embedded in this flags
        /// </summary>
        public MoveOnType MoveOn
        {
            get
            {
                return (MoveOnType)(flags & OF_NOMOVEON_MASK);
            }

            set
            {
                flags &= ~OF_NOMOVEON_MASK; // unset all bits of enum
                flags |= (uint)value;       // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 2 - BOOLS
        /// <summary>
        /// True if the object is a player.
        /// </summary>
        public bool IsPlayer
        {
            get { return (flags & OF_PLAYER) == OF_PLAYER; }
            set
            {
                if (value) flags |= OF_PLAYER;
                else flags &= ~OF_PLAYER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is attackable.
        /// </summary>
        public bool IsAttackable
        {
            get { return (flags & OF_ATTACKABLE) == OF_ATTACKABLE; }
            set
            {
                if (value) flags |= OF_ATTACKABLE;
                else flags &= ~OF_ATTACKABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is gettable.
        /// </summary>
        public bool IsGettable
        {
            get { return (flags & OF_GETTABLE) == OF_GETTABLE; }
            set
            {
                if (value) flags |= OF_GETTABLE;
                else flags &= ~OF_GETTABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a container.
        /// </summary>
        public bool IsContainer
        {
            get { return (flags & OF_CONTAINER) == OF_CONTAINER; }
            set
            {
                if (value) flags |= OF_CONTAINER;
                else flags &= ~OF_CONTAINER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can't be examined.
        /// </summary>
        public bool IsNoExamine
        {
            get { return (flags & OF_NOEXAMINE) == OF_NOEXAMINE; }
            set
            {
                if (value) flags |= OF_NOEXAMINE;
                else flags &= ~OF_NOEXAMINE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is hanging from the ceiling.
        /// Note: For VANILLA this overlaps with PF_CREATOR
        /// </summary>
        public bool IsHanging
        {
            get { return (flags & OF_HANGING) == OF_HANGING; }
            set
            {
                if (value) flags |= OF_HANGING;
                else flags &= ~OF_HANGING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can be offered to.
        /// </summary>
        public bool IsOfferable
        {
            get { return (flags & OF_OFFERABLE) == OF_OFFERABLE; }
            set
            {
                if (value) flags |= OF_OFFERABLE;
                else flags &= ~OF_OFFERABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if you can buy from the object.
        /// </summary>
        public bool IsBuyable
        {
            get { return (flags & OF_BUYABLE) == OF_BUYABLE; }
            set
            {
                if (value) flags |= OF_BUYABLE;
                else flags &= ~OF_BUYABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can be activated.
        /// </summary>
        public bool IsActivatable
        {
            get { return (flags & OF_ACTIVATABLE) == OF_ACTIVATABLE; }
            set
            {
                if (value) flags |= OF_ACTIVATABLE;
                else flags &= ~OF_ACTIVATABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object can be applied.
        /// </summary>
        public bool IsApplyable
        {
            get { return (flags & OF_APPLYABLE) == OF_APPLYABLE; }
            set
            {
                if (value) flags |= OF_APPLYABLE;
                else flags &= ~OF_APPLYABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object has safety on.
        /// </summary>
        public bool IsSafety
        {
            get { return (flags & OF_SAFETY) == OF_SAFETY; }
            set
            {
                if (value) flags |= OF_SAFETY;
                else flags &= ~OF_SAFETY;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 3 - PlayerType
        /// <summary>
        /// PlayerType as embedded in this flags
        /// </summary>
        public PlayerType Player
        {
            get
            {
                return (PlayerType)(flags & OF_PLAYER_MASK);
            }

            set
            {
                flags &= ~OF_PLAYER_MASK;   // unset all bits of enum
                flags |= (uint)value;       // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 4 - BOOLS
        /// <summary>
        /// True if the object is flickering.
        /// </summary>
        public bool IsFlickering
        {
            get { return (flags & OF_FLICKERING) == OF_FLICKERING; }
            set
            {
                if (value) flags |= OF_FLICKERING;
                else flags &= ~OF_FLICKERING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is flashing.
        /// </summary>
        public bool IsFlashing
        {
            get { return (flags & OF_FLASHING) == OF_FLASHING; }
            set
            {
                if (value) flags |= OF_FLASHING;
                else flags &= ~OF_FLASHING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is bouncing.
        /// Note: Overlaps. Bouncing if Flickering AND Flashing true
        /// </summary>
        public bool IsBouncing
        {
            get { return (flags & OF_BOUNCING) == OF_BOUNCING; }
            set
            {
                if (value) flags |= OF_BOUNCING;
                else flags &= ~OF_BOUNCING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is phasing.
        /// </summary>
        public bool IsPhasing
        {
            get { return (flags & OF_PHASING) == OF_PHASING; }
            set
            {
                if (value) flags |= OF_PHASING;
                else flags &= ~OF_PHASING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 5 - DrawingType
        /// <summary>
        /// DrawingType as embedded in this flags
        /// </summary>
        public DrawingType Drawing
        {
            get
            {
                return (DrawingType)(flags & OF_EFFECT_MASK);
            }

            set
            {
                flags &= ~OF_EFFECT_MASK;   // unset all bits of enum
                flags |= (uint)value;       // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 6 - BOOLS
        /// <summary>
        /// True if the object is an enemy player.
        /// </summary>
        public bool IsMinimapEnemy
        {
            get { return (flags & OF_ENEMY) == OF_ENEMY; }
            set
            {
                if (value) flags |= OF_ENEMY;
                else flags &= ~OF_ENEMY;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a friendly player.
        /// </summary>
        public bool IsMinimapFriend
        {
            get { return (flags & OF_FRIEND) == OF_FRIEND; }
            set
            {
                if (value) flags |= OF_FRIEND;
                else flags &= ~OF_FRIEND;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if the object is a guildmate.
        /// </summary>
        public bool IsMinimapGuildMate
        {
            get { return (flags & OF_GUILDMATE) == OF_GUILDMATE; }
            set
            {
                if (value) flags |= OF_GUILDMATE;
                else flags &= ~OF_GUILDMATE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        /// <summary>
        /// Returns true if all bits from Flags parameter
        /// are also set in the current value or if Flags
        /// paramter is null.
        /// </summary>
        /// <param name="Flags"></param>
        /// <returns></returns>
        public bool IsSubset(ObjectFlags Flags)
        {
            return (Flags == null || ((flags & Flags.Value) == Flags.Value));
        }

        /// <summary>
        /// True if attackable, not a player and moveon=no
        /// </summary>     
        public bool IsCreature
        {
            get { return MoveOn == MoveOnType.No && IsAttackable && !IsPlayer; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Flags"></param>
        /// <param name="RaiseChangedEvent"></param>
        public void UpdateFromModel(ObjectFlags Flags, bool RaiseChangedEvent)
        {
            if (Flags == null)
                return;

            if (RaiseChangedEvent)
            {
                Value = Flags.Value;               
            }
            else
            {
                flags = Flags.Value;
            }
        }
#endif
    }
}
