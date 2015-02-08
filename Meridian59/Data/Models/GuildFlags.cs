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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Wrapper around 32bit Guild flags
    /// </summary>
    [Serializable]
    public class GuildFlags : INotifyPropertyChanged
    {
        #region Constants
        public string PROPNAME_FLAGS = "Flags";
        #endregion

        #region Bitmasks
        private const uint GC_INVITE        = 0x00000001;      // Invite
        private const uint GC_EXILE         = 0x00000002;      // Exile guild member
        private const uint GC_RENOUNCE      = 0x00000004;      // Renounce guild ties
        private const uint GC_VOTE          = 0x00000020;      // Vote for guild member
        private const uint GC_ABDICATE      = 0x00000040;      // Abdicate guildmaster position
        private const uint GC_MAKE_ALLIANCE = 0x00000100;      // Make alliance with another guild
        private const uint GC_END_ALLIANCE  = 0x00000200;      // End guild alliance
        private const uint GC_DECLARE_ENEMY = 0x00000400;      // Declare another guild as enemy
        private const uint GC_END_ENEMY     = 0x00000800;      // Make peace with enemy guild
        private const uint GC_SET_RANK      = 0x00001000;      // Set guild member's rank
        private const uint GC_DISBAND       = 0x00002000;      // Disband (destroy) guild
        private const uint GC_ABANDON       = 0x00004000;      // Abandon guild hall
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        protected uint flags;

        public uint Flags
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }
        
        public GuildFlags(uint Flags = 0)
        {
            this.Flags = Flags;
        }

        public override string ToString()
        {
            return Flags.ToString();
        }

        #region Property Accessors
        /* 
         * Easy to use property accessors,
         * Check set: AND
         * Set: OR
         * Unset: AND NEG
         */
        
        public bool IsInvite
        {
            get { return (Flags & GC_INVITE) == GC_INVITE; }
            set 
            {
                if (value) Flags |= GC_INVITE;
                else Flags &= ~GC_INVITE;
            }
        }
        public bool IsExile
        {
            get { return (Flags & GC_EXILE) == GC_EXILE; }
            set
            {
                if (value) Flags |= GC_EXILE;
                else Flags &= ~GC_EXILE;
            }
        }
        public bool IsRenounce
        {
            get { return (Flags & GC_RENOUNCE) == GC_RENOUNCE; }
            set
            {
                if (value) Flags |= GC_RENOUNCE;
                else Flags &= ~GC_RENOUNCE;
            }
        }
        public bool IsVote
        {
            get { return (Flags & GC_VOTE) == GC_VOTE; }
            set
            {
                if (value) Flags |= GC_VOTE;
                else Flags &= ~GC_VOTE;
            }
        }
        public bool IsAbdicate
        {
            get { return (Flags & GC_ABDICATE) == GC_ABDICATE; }
            set
            {
                if (value) Flags |= GC_ABDICATE;
                else Flags &= ~GC_ABDICATE;
            }
        }
        public bool IsMakeAlliance
        {
            get { return (Flags & GC_MAKE_ALLIANCE) == GC_MAKE_ALLIANCE; }
            set
            {
                if (value) Flags |= GC_MAKE_ALLIANCE;
                else Flags &= ~GC_MAKE_ALLIANCE;
            }
        }
        public bool IsEndAlliance
        {
            get { return (Flags & GC_END_ALLIANCE) == GC_END_ALLIANCE; }
            set
            {
                if (value) Flags |= GC_END_ALLIANCE;
                else Flags &= ~GC_END_ALLIANCE;
            }
        }
        public bool IsDeclareEnemy
        {
            get { return (Flags & GC_DECLARE_ENEMY) == GC_DECLARE_ENEMY; }
            set
            {
                if (value) Flags |= GC_DECLARE_ENEMY;
                else Flags &= ~GC_DECLARE_ENEMY;
            }
        }
        public bool IsEndEnemy
        {
            get { return (Flags & GC_END_ENEMY) == GC_END_ENEMY; }
            set
            {
                if (value) Flags |= GC_END_ENEMY;
                else Flags &= ~GC_END_ENEMY;
            }
        }
        public bool IsSetRank
        {
            get { return (Flags & GC_SET_RANK) == GC_SET_RANK; }
            set
            {
                if (value) Flags |= GC_SET_RANK;
                else Flags &= ~GC_SET_RANK;
            }
        }
        public bool IsDisband
        {
            get { return (Flags & GC_DISBAND) == GC_DISBAND; }
            set
            {
                if (value) Flags |= GC_DISBAND;
                else Flags &= ~GC_DISBAND;
            }
        }
        public bool IsAbandon
        {
            get { return (Flags & GC_ABANDON) == GC_ABANDON; }
            set
            {
                if (value) Flags |= GC_ABANDON;
                else Flags &= ~GC_ABANDON;
            }
        }
        #endregion
    }
}
