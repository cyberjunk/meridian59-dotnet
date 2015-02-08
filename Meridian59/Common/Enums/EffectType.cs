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
    /// Possible effects 
    /// </summary>
    public enum EffectType : ushort
    {
        None        = 0,
        Invert      = 1,   // Invert screen
        Shake       = 2,   // Shake player's view
        Paralyze    = 3,   // Paralyze player (no motion)
        Release     = 4,   // Stop paralysis of player
        Blind       = 5,   // Make player blind
        See         = 6,   // Remove blindness
        Pain        = 7,   // Make player see they're hurting
        Blur        = 8,   // Distort player's vision
        Raining     = 9,   // Start rain
        Snowing     = 10,  // Start snow
        ClearWeather = 11,  // Stop all weather effects
        Sand        = 12,  // Sandstorm
        ClearSand   = 13,  // Stop sandstorm
        Waver       = 14,  // Wavering sideways
        FlashXLat   = 15,  // Flashes screen with a given XLAT number
        WhiteOut    = 16,  // Got from full white and fade back to normal
        XLatOverride = 17,  // Use this xlat at end over the whole screen
    }
}
