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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class UserCommandGuildShieldError : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.GuildShieldError; } }

        #region IByteSerializable implementation
        public override int ByteLength {
            get {
                return TypeSizes.BYTE + ShieldError.ByteLength;
            }
        }
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;
            cursor++;

            cursor += ShieldError.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if ((UserCommandType)Buffer[cursor] != CommandType)
                throw new Exception(ERRORWRONGTYPEBYTE);
            else
            {
                cursor++;

                ShieldError = new ServerString(ChatMessageType.ObjectChatMessage, LookupList, Buffer, cursor);
                cursor += ShieldError.ByteLength;
            }

            return cursor - StartIndex;
        }
        #endregion

        public ServerString ShieldError { get; set; }
        public StringDictionary LookupList { get; private set; }

        public UserCommandGuildShieldError(ServerString ShieldError, StringDictionary LookupList)
        {
            this.LookupList = LookupList;
            this.ShieldError = ShieldError;
        }

        public UserCommandGuildShieldError(StringDictionary LookupList, byte[] Buffer, int StartIndex = 0)
        {
            this.LookupList = LookupList;
            ReadFrom(Buffer, StartIndex);
        }
    }
}
