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
    /// The movement speeds.
    /// These are known as viSpeed of objects on the server-side.
    /// Defined in blakston.khd and some .kod files
    /// </summary>
    public enum MovementSpeed : byte
    {
        SPEED_NONE          = 0,
        SPEED_VERY_SLOW     = 4,
        SPEED_SLOW          = 8,
        SPEED_AVERAGE       = 12,
        SPEED_FAST          = 16,
        SPEED_FASTER        = 18,
        SPEED_VERY_FAST     = 20,

        USER_WALKING_SPEED  = 25,
        USER_RUNNING_SPEED  = 50,

        Teleport    = SPEED_NONE,
        Walk        = USER_WALKING_SPEED,
        Run         = USER_RUNNING_SPEED
    }
}
