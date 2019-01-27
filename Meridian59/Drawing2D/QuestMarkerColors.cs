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

using Meridian59.Common;
using Meridian59.Data.Models;

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Handles quest marker colors based on object flags.
    /// </summary>
    public static class QuestMarkerColors
    {
        public const uint NPCACTIVEQUEST = 0xFF00FF78; // green (0, 255, 120)
        public const uint NPCHASQUEST    = 0xFFFFFF00; // yellow (255, 255, 0)
        public const uint MOBKILLQUEST   = 0xFFB300B3; // purple (179, 0, 179)

        /// <summary>
        /// Returns a 32bit color based on ObjectFlags.
        /// </summary>
        /// <param name="Flags"></param>
        /// <returns></returns>
        public static uint GetColorFor(ObjectFlags Flags)
        {
            if (Flags.IsNPCActiveQuest)
                return NPCACTIVEQUEST;
            if (Flags.IsMobKillQuest)
                return MOBKILLQUEST;
            if (Flags.IsNPCHasQuests)
                return NPCHASQUEST;

            return 0xFFFFFFFF;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Flags"></param>
        /// <param name="A"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public static void GetColorComponentsFor(ObjectFlags Flags, out byte A, out byte R, out byte G, out byte B)
        {
            Util.GetIntegerBytes(GetColorFor(Flags), out A, out R, out G, out B);
        }
    }
}
