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
    public class UserCommandGuildRent : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.GuildRent; } }

        #region IByteSerializable implementation
        public override int ByteLength 
        { 
            get 
            { 
                return TypeSizes.BYTE + TypeSizes.INT + TypeSizes.SHORT + Password.Length;
            }
        }       
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;
            cursor++;

            Array.Copy(BitConverter.GetBytes(HallID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Password.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Password), 0, Buffer, cursor, Password.Length);
            cursor += Password.Length;

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

                HallID = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                Password = Util.Encoding.GetString(Buffer, cursor, strlen);
                cursor += strlen;
            }

            return cursor - StartIndex;
        }
        #endregion

        public uint HallID;
        public string Password;

        public UserCommandGuildRent(uint HallID, string Password)
        {
            this.HallID = HallID;
            this.Password = Password;
        }

        public UserCommandGuildRent(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
