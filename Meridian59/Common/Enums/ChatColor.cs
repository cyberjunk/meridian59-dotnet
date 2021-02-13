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
    /// Possible M59 chat colors
    /// </summary>
    public enum ChatColor
    {
        Black  = 'k',
        White  = 'w',
        Red    = 'r',
        Green  = 'g',
        Blue   = 'b',
        Purple = 'q'

#if !VANILLA
        ,
        Aquamarine = 'a',
        Cyan = 'c',
        Drab = 'd',
        Emerald = 'e',
        Fire = 'f',
        Champagne = 'h',
        ImperialBlue = 'i',
        Jonquil = 'j',
        Lime = 'l',
        Magenta = 'm',
        Orange = 'o',
        Pink = 'p',
        Steel = 's',
        ToxicGreen = 't',
        OffWhite = 'u',
        Violet = 'v',
        Golden = 'x',
        Yellow = 'y',
        Bronze = 'z',
        Gray1 = '0',
        Gray2 = '1',
        Gray3 = '2',
        Gray4 = '3',
        Gray5 = '4',
        Gray6 = '5',
        Gray7 = '6',
        Gray8 = '7',
        Gray9 = '8',
        Gray10 = '9',
        QuestGreen = 'G',
        QuestRed = 'R',
        MercenaryColor = 'M'
#endif
    }
}
