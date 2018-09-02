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
    /// Wrapper for sector flags
    /// </summary>
    /// <remarks>
    /// See original manual page 118
    /// </remarks>
    [Serializable]
    public class RooSectorFlags : Flags
    {
        #region Bitmasks
        // BITS: 0-1       ENUM (SectorDepth Type)
        private const uint SF_DEPTH0            = 0x00000000;      // Sector has default (0) depth
        private const uint SF_DEPTH1            = 0x00000001;      // Sector has shallow depth
        private const uint SF_DEPTH2            = 0x00000002;      // Sector has deep depth
        private const uint SF_DEPTH3            = 0x00000003;      // Sector has very deep depth
        private const uint SF_MASK_DEPTH        = 0x00000003;
        private const uint SF_CHANGE_OVERRIDE   = 0x00000004;      // Server sends this in SectorChange when depth does not change

        // BITS: 2-3       ENUM (ScrollSpeed Type) (see Common/Enum/TextureScrollSpeed.cs)
        private const uint SF_MASK_SCROLLSPEED  = 0x0000000C;

        // BITS: 4-5       ENUM (ScrollDirection Type) (see Common/Enum/TextureScrollDirection.cs)
        private const uint SF_MASK_SCROLLDIR    = 0x00000070;
        
        // BITS: 7-12      BOOLS
        private const uint SF_SCROLL_FLOOR      = 0x00000080;      // Scroll floor texture
        private const uint SF_SCROLL_CEILING    = 0x00000100;      // Scroll ceiling texture
        private const uint SF_FLICKER           = 0x00000200;      // Flicker light in sector
        private const uint SF_SLOPED_FLOOR      = 0x00000400;      // Sector has sloped floor
        private const uint SF_SLOPED_CEILING    = 0x00000800;      // Sector has sloped ceiling
        private const uint SF_HAS_ANIMATED      = 0x00001000;      // has animated once and hence is dynamic geometry, required for new client             
#if !VANILLA && !OPENMERIDIAN
        private const uint SF_NOMOVE            = 0x00002000;      // Sector can't be moved on by mobs or players
#endif
        #endregion

        /// <summary>
        /// Possible depth types a sector can have.
        /// </summary>
        public enum DepthType : uint
        {
            Depth0  = SF_DEPTH0,
            Depth1  = SF_DEPTH1,
            Depth2  = SF_DEPTH2,
            Depth3  = SF_DEPTH3,
            ChangeOverride = SF_CHANGE_OVERRIDE
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value"></param>
        public RooSectorFlags(uint Value = 0)
            : base(Value) { }

        #region SECTION 1 - BITS [0-1] - SectorDepthType
        /// <summary>
        /// Depth of this sector
        /// </summary>
        public DepthType SectorDepth
        {
            get { return (DepthType)(flags & SF_MASK_DEPTH); }
            set
            {
                flags &= ~SF_MASK_DEPTH;    // unset all bits of enum
                flags |= (uint)value;       // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 2 - BITS [2-3] - ScrollSpeedType
        /// <summary>
        /// Scrollspeed assigned on this sector.
        /// </summary>
        public TextureScrollSpeed ScrollSpeed
        {
            get { return (TextureScrollSpeed)(((flags) & SF_MASK_SCROLLSPEED) >> 2); }
            set
            {
                flags &= ~SF_MASK_SCROLLSPEED;      // unset all bits of enum
                flags |= ((uint)value << 2);        // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 3 - BITS [4-6] - ScrollDirectionType
        /// <summary>
        /// Direction to scroll
        /// </summary>
        public TextureScrollDirection ScrollDirection
        {
            get { return (TextureScrollDirection)(((flags) & SF_MASK_SCROLLDIR) >> 4); }
            set
            {
                flags &= ~SF_MASK_SCROLLDIR;        // unset all bits of enum
                flags |= ((uint)value << 4);        // set bits of value

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
        #endregion

        #region SECTION 4 - BITS [7-12] - BOOLS
        /// <summary>
        /// Scroll floor texture
        /// </summary>
        public bool IsScrollFloor
        {
            get { return (flags & SF_SCROLL_FLOOR) == SF_SCROLL_FLOOR; }
            set
            {
                if (value) flags |= SF_SCROLL_FLOOR;
                else flags &= ~SF_SCROLL_FLOOR;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Scroll ceiling texture
        /// </summary>
        public bool IsScrollCeiling
        {
            get { return (flags & SF_SCROLL_CEILING) == SF_SCROLL_CEILING; }
            set
            {
                if (value) flags |= SF_SCROLL_CEILING;
                else flags &= ~SF_SCROLL_CEILING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Flicker light in sector
        /// </summary>
        public bool IsFlicker
        {
            get { return (flags & SF_FLICKER) == SF_FLICKER; }
            set
            {
                if (value) flags |= SF_FLICKER;
                else flags &= ~SF_FLICKER;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Sector has sloped floor
        /// </summary>
        public bool IsSlopedFloor
        {
            get { return (flags & SF_SLOPED_FLOOR) == SF_SLOPED_FLOOR; }
            set
            {
                if (value) flags |= SF_SLOPED_FLOOR;
                else flags &= ~SF_SLOPED_FLOOR;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Sector has sloped ceiling
        /// </summary>
        public bool IsSlopedCeiling
        {
            get { return (flags & SF_SLOPED_CEILING) == SF_SLOPED_CEILING; }
            set
            {
                if (value) flags |= SF_SLOPED_CEILING;
                else flags &= ~SF_SLOPED_CEILING;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Has animated once and hence is dynamic geometry, required for new client
        /// </summary>
        public bool IsHasAnimated
        {
            get { return (flags & SF_HAS_ANIMATED) == SF_HAS_ANIMATED; }
            set
            {
                if (value) flags |= SF_HAS_ANIMATED;
                else flags &= ~SF_HAS_ANIMATED;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

#if !VANILLA && !OPENMERIDIAN
        /// <summary>
        /// Sector can't be moved on by mobs or players
        /// </summary>
        public bool IsNoMove
        {
            get { return (flags & SF_NOMOVE) == SF_NOMOVE; }
            set
            {
                if (value) flags |= SF_NOMOVE;
                else flags &= ~SF_NOMOVE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }
#endif
        #endregion
   }
}
