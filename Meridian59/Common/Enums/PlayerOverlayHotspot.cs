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

namespace Meridian59.Common.Enums
{
    /// <summary>
    /// Hotspots for 1. person PlayerOverlays.
    /// Each one specifies a certain point on the screen.
    /// </summary>
    /// <remarks>
    /// See original: proto.h
    /// </remarks>
    public enum PlayerOverlayHotspot : byte
    {
        HOTSPOT_HIDE    = 0,
        HOTSPOT_NW      = 1,
        HOTSPOT_N       = 2,
        HOTSPOT_NE      = 3,
        HOTSPOT_E       = 4,
        HOTSPOT_SE      = 5,
        HOTSPOT_S       = 6,
        HOTSPOT_SW      = 7,
        HOTSPOT_W       = 8,
        HOTSPOT_CENTER  = 9
    }
}
