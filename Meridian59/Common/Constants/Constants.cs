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

namespace Meridian59.Common.Constants
{
    /// <summary>
    /// CRC16 values of recent meridian.exe files
    /// </summary>
    public static class MeridianExeCRCs
    {
        public const ushort BUILD708 = 0xF7B6;
        public const ushort BUILD710 = 0xECBA;
        public const ushort BUILD711 = 0xA842;
        public const ushort BUILD712 = 0x36DF;
        public const ushort BUILD713 = 0xFA8B;
        public const ushort BUILD714 = 0x5B89;
        public const ushort BUILD715 = 0xA0E4;
        public const ushort BUILD716 = 0x1B13;
        public const ushort NEWCLIENTDETECT = 0xFFFF;
    }

    /// <summary>
    /// Major and minor versions of recent clients
    /// </summary>
    public static class ClientVersions
    {
        public const byte BUILD708major = 7;
        public const byte BUILD710major = 7;
        public const byte BUILD711major = 7;
        public const byte BUILD712major = 7;
        public const byte BUILD713major = 7;
        public const byte BUILD714major = 7;
        public const byte BUILD715major = 7;
        public const byte BUILD716major = 7;
       
        public const byte BUILD708minor = 8;
        public const byte BUILD710minor = 10;
        public const byte BUILD711minor = 11;
        public const byte BUILD712minor = 12;
        public const byte BUILD713minor = 13;
        public const byte BUILD714minor = 14;
        public const byte BUILD715minor = 15;
        public const byte BUILD716minor = 16;       
    }

    /// <summary>
    /// The num for the common stats as they appear in Stat class.
    /// </summary>
    public static class StatNums
    {
        public const byte HITPOINTS = 1;
        public const byte MANA = 2;
        public const byte VIGOR = 3;
        public const byte TOUGHERCHANCE = 4;
    }

    /// <summary>
    /// Some special values appearing in stats.
    /// </summary>
    public static class StatNumsValues
    {
        public const int LOWVIGOR = 10;
        public const int DEFAULTVIGOR = 80;
        public const int MAXVIGOR = 200;
        public const int SKILLMAX = 99;
        public const int SPELLMAX = SKILLMAX;
    }

    /// <summary>
    /// Some special colorpalette indices of robes
    /// </summary>
    public static class RobesColors
    {
        public const byte PURPLE1   = 0xD5;
        public const byte PURPLE2   = 0xD6;
        public const byte PURPLE3   = 0xD7;
        public const byte WHITE1    = 0xEB;
        public const byte WHITE2    = 0xEC;
        public const byte WHITE3    = 0xED;
        public const byte GREEN1    = 0xBF;
        public const byte GREEN2    = 0xC0;
        public const byte GREEN3    = 0xC1;
        public const byte BLUE1     = 0xCA;
        public const byte BLUE2     = 0xCB;
        public const byte BLUE3     = 0xCC;
        public const byte GREY1     = 0x7D;
        public const byte GREY2     = 0x7E;
        public const byte GREY3     = 0x7F;
        public const byte RED1      = 0x88;
        public const byte RED2      = 0x89;
        public const byte RED3      = 0x8A;
    }

    /// <summary>
    /// Thresholds for categorizing RTT values.
    /// Defined as "smaller or equal".
    /// </summary>
    public static class RTTValues
    {
        public const int GOOD = 150;
        public const int OK = 300;
        public const int BAD = 500;
    }

    /// <summary>
    /// Thresholds for categorizing FPS values.
    /// Defined as "greater or equal".
    /// </summary>
    public static class FPSValues
    {
        public const int GOOD = 35;
        public const int OK = 25;
        public const int BAD = 15;
    }
}
