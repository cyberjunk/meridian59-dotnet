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
using Meridian59.Common.Enums;

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// Wrapper for wallside flags
    /// </summary>
    /// <remarks>
    /// See original manual page 118
    /// </remarks>
    [Serializable]
    public class RooSideDefFlags : Flags
    {
        #region Bitmasks
        // BITS: 0-9       BOOLS
        private const uint WF_BACKWARDS         = 0x00000001;   // Draw bitmap right/left reversed
        private const uint WF_TRANSPARENT       = 0x00000002;   // normal wall has some transparency
        private const uint WF_PASSABLE          = 0x00000004;   // wall can be walked through
        private const uint WF_MAP_NEVER         = 0x00000008;   // Don't show wall on map
        private const uint WF_MAP_ALWAYS        = 0x00000010;   // Always show wall on map
        private const uint WF_NOLOOKTHROUGH     = 0x00000020;   // bitmap can't be seen through even though it's transparent
        private const uint WF_ABOVE_BOTTOMUP    = 0x00000040;   // Draw upper texture bottom-up
        private const uint WF_BELOW_TOPDOWN     = 0x00000080;   // Draw lower texture top-down
        private const uint WF_NORMAL_TOPDOWN    = 0x00000100;   // Draw normal texture top-down
        private const uint WF_NO_VTILE          = 0x00000200;   // Don't tile texture vertically (must be transparent)
        private const uint WF_HAS_ANIMATED      = 0x00000400;   // has animated once and hence is dynamic geometry, required for new client

        // BITS: 10-11     ENUM (ScrollSpeed Type) (see Common/Enum/TextureScrollSpeed.cs)
        private const uint WF_MASK_SCROLLSPEED  = 0x00000C00;

        // BITS: 12-14     ENUM (ScrollDirection Type) (see Common/Enum/TextureScrollDirection.cs)
        private const uint WF_MASK_SCROLLDIR    = 0x00007000;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value"></param>
        public RooSideDefFlags(uint Value = 0)
            : base(Value) { }

        #region SECTION 1 - BOOLS
        /// <summary>
        /// Draw bitmap right/left reversed
        /// </summary>
        public bool IsBackwards
        {
            get { return (flags & WF_BACKWARDS) == WF_BACKWARDS; }
            set 
            {
                if (value) flags |= WF_BACKWARDS;
                else flags &= ~WF_BACKWARDS;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Normal wall has some transparency
        /// </summary>
        public bool IsTransparent
        {
            get { return (flags & WF_TRANSPARENT) == WF_TRANSPARENT; }
            set
            {
                if (value) flags |= WF_TRANSPARENT;
                else flags &= ~WF_TRANSPARENT;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Wall can be walked through
        /// </summary>
        public bool IsPassable
        {
            get { return (flags & WF_PASSABLE) == WF_PASSABLE; }
            set
            {
                if (value) flags |= WF_PASSABLE;
                else flags &= ~WF_PASSABLE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Don't show wall on map
        /// </summary>
        public bool IsMapNever
        {
            get { return (flags & WF_MAP_NEVER) == WF_MAP_NEVER; }
            set
            {
                if (value) flags |= WF_MAP_NEVER;
                else flags &= ~WF_MAP_NEVER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Always show wall on map
        /// </summary>
        public bool IsMapAlways
        {
            get { return (flags & WF_MAP_ALWAYS) == WF_MAP_ALWAYS; }
            set
            {
                if (value) flags |= WF_MAP_ALWAYS;
                else flags &= ~WF_MAP_ALWAYS;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Bitmap can't be seen through even though it's transparent
        /// </summary>
        public bool IsNoLookThrough
        {
            get { return (flags & WF_NOLOOKTHROUGH) == WF_NOLOOKTHROUGH; }
            set
            {
                if (value) flags |= WF_NOLOOKTHROUGH;
                else flags &= ~WF_NOLOOKTHROUGH;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Draw upper texture bottom-up
        /// </summary>
        public bool IsAboveBottomUp
        {
            get { return (flags & WF_ABOVE_BOTTOMUP) == WF_ABOVE_BOTTOMUP; }
            set
            {
                if (value) flags |= WF_ABOVE_BOTTOMUP;
                else flags &= ~WF_ABOVE_BOTTOMUP;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Draw lower texture top-down
        /// </summary>
        public bool IsBelowTopDown
        {
            get { return (flags & WF_BELOW_TOPDOWN) == WF_BELOW_TOPDOWN; }
            set
            {
                if (value) flags |= WF_BELOW_TOPDOWN;
                else flags &= ~WF_BELOW_TOPDOWN;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Draw normal texture top-down
        /// </summary>
        public bool IsNormalTopDown
        {
            get { return (flags & WF_NORMAL_TOPDOWN) == WF_NORMAL_TOPDOWN; }
            set
            {
                if (value) flags |= WF_NORMAL_TOPDOWN;
                else flags &= ~WF_NORMAL_TOPDOWN;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Don't tile texture vertically (must be transparent)
        /// </summary>
        public bool IsNoVTile
        {
            get { return (flags & WF_NO_VTILE) == WF_NO_VTILE; }
            set
            {
                if (value) flags |= WF_NO_VTILE;
                else flags &= ~WF_NO_VTILE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Has animated once and hence is dynamic geometry, required for new client
        /// </summary>
        public bool IsHasAnimated
        {
            get { return (flags & WF_HAS_ANIMATED) == WF_HAS_ANIMATED; }
            set
            {
                if (value) flags |= WF_HAS_ANIMATED;
                else flags &= ~WF_HAS_ANIMATED;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }        
        #endregion     

        #region SECTION 2 - ScrollSpeedType
        /// <summary>
        /// Scrollspeed assigned on this wallside texture.
        /// </summary>
        public TextureScrollSpeed ScrollSpeed
        {
            get { return (TextureScrollSpeed)(((flags) & WF_MASK_SCROLLSPEED) >> 10); }
            set
            {
                flags &= ~WF_MASK_SCROLLSPEED;      // unset all bits of enum
                flags |= ((uint)value << 10);       // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 3 - ScrollSpeedDirectionType
        /// <summary>
        /// Direction to scroll
        /// </summary>
        public TextureScrollDirection ScrollDirection
        {
            get { return (TextureScrollDirection)(((flags) & WF_MASK_SCROLLDIR) >> 12); }
            set
            {
                flags &= ~WF_MASK_SCROLLDIR;        // unset all bits of enum
                flags |= ((uint)value << 12);       // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion
    }
}
