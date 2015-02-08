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

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// Wrapper for RoomInfo flags
    /// </summary>
    [Serializable]
    public class RoomInfoFlags : Flags
    {
        #region Bitmasks
        private const uint ROOM_OVERRIDE_DEPTH1  = 0x00000001;
        private const uint ROOM_OVERRIDE_DEPTH2  = 0x00000002;
        private const uint ROOM_OVERRIDE_DEPTH3  = 0x00000004;
        private const uint ROOM_OVERRIDE_MASK    = 0x00000007;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value"></param>
        public RoomInfoFlags(uint Value = 0)
            : base(Value) { }

        /// <summary>
        /// True if SectorDepth1 "Shallow" in ROO should be overwritten with server-sent height.
        /// </summary>
        public bool IsOverrideDepth1
        {
            get { return (flags & ROOM_OVERRIDE_DEPTH1) == ROOM_OVERRIDE_DEPTH1; }
            set 
            {
                if (value) flags |= ROOM_OVERRIDE_DEPTH1;
                else flags &= ~ROOM_OVERRIDE_DEPTH1;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if SectorDepth2 "Deep" in ROO should be overwritten with server-sent height.
        /// </summary>
        public bool IsOverrideDepth2
        {
            get { return (flags & ROOM_OVERRIDE_DEPTH2) == ROOM_OVERRIDE_DEPTH2; }
            set
            {
                if (value) flags |= ROOM_OVERRIDE_DEPTH2;
                else flags &= ~ROOM_OVERRIDE_DEPTH2;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// True if SectorDepth3 "Very deep" in ROO should be overwritten with server-sent height.
        /// </summary>
        public bool IsOverrideDepth3
        {
            get { return (flags & ROOM_OVERRIDE_DEPTH3) == ROOM_OVERRIDE_DEPTH3; }
            set
            {
                if (value) flags |= ROOM_OVERRIDE_DEPTH3;
                else flags &= ~ROOM_OVERRIDE_DEPTH3;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
