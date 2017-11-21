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
    /// Error codes received from the server during character creation.
    /// </summary>
    public enum CharInfoNotOkError : byte
    {
        NoError = 0x00, // Everything went OK.
        GenericError = 0x01, // Use for e.g. broken protocol message.
        NotFirstTime = 0x02, // Character already exists on slot, or bug with restart time.
        NameTooLong = 0x03,
        NameBadCharacters = 0x04, // Invalid letters in name.
        NameInUse = 0x05,
        NoMobName = 0x06,
        NoNPCName = 0x07,
        NoGuildName = 0x08,
        NoBadWords = 0x09,
        NoConfusingName = 0x0A, // Names of M59 Gods, 'You', 'Administrator'
        NoRetiredName = 0x0B, // Names of old designers/admins.
        DescriptionTooLong = 0x0C,
        InvalidGender = 0x0D // e.g. client sending anything except 1 or 2 to server.
    }
}
