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
    /// Handles colors based on object flags.
    /// </summary>
    public static class NameColors
    {
        public const uint NORMAL    = 0xFFFFFFFF; // white (255, 255, 255)
        public const uint OUTLAW    = 0xFFFC8000; // orange (252, 128, 0)        
        public const uint KILLER    = 0xFFFF0000; // red (255, 0, 0)
        public const uint EVENTCHAR = 0xFFFF00FF; // purple (255, 0, 255)
        public const uint CREATOR   = 0xFFFFFF00; // yellow (255, 255, 0)
        public const uint SUPERDM   = 0xFF00FF00; // green (0, 255, 0)
        public const uint DM        = 0xFF00FFFF; // magenta (0, 255, 255)
        public const uint BLACK     = 0xFF000000; // black (0, 0, 0)

#if !VANILLA
        public const uint DAENKS    = 0xFFB300B3; // purple2 (179, 0, 179)
        public const uint MAGIC     = 0xFFFF5000; // orange2 (255, 80, 0)
        public const uint MODERATOR = 0xFF0078FF; // kinda blue (0, 120, 255)
#endif

        /// <summary>
        /// Returns a 32bit color based on ObjectFlags.
        /// </summary>
        /// <param name="Flags"></param>
        /// <returns></returns>
        public static uint GetColorFor(ObjectFlags Flags)
        {
#if !VANILLA
            uint color;
            // openmeridian has a name-color transferred from server in flags
            // however it has opacity set to 0, so we make it full opaque here.
            if (Flags.IsMagicItem)
                color = MAGIC | 0xFF000000;
            else
                color = Flags.NameColor | 0xFF000000;

            // lots of kod objects have black as color
            // which is turned into white as a workaround here
            return color != 0xFF000000 ? color : 0xFFFFFFFF;
#else
            if (Flags.Player == ObjectFlags.PlayerType.SuperDM)
                return SUPERDM;

            else if (Flags.Player == ObjectFlags.PlayerType.EventChar)
                return EVENTCHAR;

            else if (Flags.Player == ObjectFlags.PlayerType.Creator)
                return CREATOR;

            else if (Flags.Player == ObjectFlags.PlayerType.DM)
                return DM;

            else if (Flags.Drawing == ObjectFlags.DrawingType.Invisible)
                return NORMAL;

            else if (Flags.Drawing == ObjectFlags.DrawingType.Black)
                return BLACK;

            else if (Flags.Player == ObjectFlags.PlayerType.Killer)
                return KILLER;

            else if (Flags.Player == ObjectFlags.PlayerType.Outlaw)
                return OUTLAW;

            else
                return NORMAL;
#endif
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
