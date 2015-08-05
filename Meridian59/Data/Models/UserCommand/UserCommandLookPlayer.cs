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
using System.Collections.Concurrent;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class UserCommandLookPlayer : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.LookPlayer; } }

        #region IByteSerializable implementation
        public override int ByteLength { 
            get { 
                // CommandType + PlayerInfo length
                return TypeSizes.BYTE + PlayerInfo.ByteLength;
            }
        } 
   
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;                     // Type     (1 byte)
            cursor++;

            cursor += PlayerInfo.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if ((UserCommandType)Buffer[cursor] != CommandType)
                throw new Exception(ERRORWRONGTYPEBYTE);
            else
            {
                cursor++;                                           // Type     (1 byte)

                PlayerInfo = new PlayerInfo(stringResources, Buffer, cursor);
                cursor += PlayerInfo.ByteLength;               
            }

            return cursor - StartIndex;
        }
        #endregion

        public PlayerInfo PlayerInfo { get; set; }

		protected StringDictionary stringResources;

        public UserCommandLookPlayer(PlayerInfo PlayerInfo)
        {
            this.PlayerInfo = PlayerInfo;       
        }

		public UserCommandLookPlayer(StringDictionary LookupList, byte[] Buffer, int StartIndex = 0)
        {
            stringResources = LookupList;
            ReadFrom(Buffer, StartIndex);
        }
    }
}
