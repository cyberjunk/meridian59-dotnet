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
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class UserCommandAppeal : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.Appeal; } }

        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                // CommandType + message length + message
                return TypeSizes.BYTE + TypeSizes.SHORT + Message.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (byte)CommandType;                                                                         // Type     (1 byte)
            cursor++;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Message.Length)), 0, Buffer, cursor, TypeSizes.SHORT);   // MessageLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Message), 0, Buffer, cursor, Message.Length);                            // Message (n bytes)
            cursor += Message.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            if ((UserCommandType)Buffer[cursor] != CommandType)
                throw new Exception(ERRORWRONGTYPEBYTE);
            else
            {
                cursor++;                                                           // Type     (1 byte)

                ushort len = BitConverter.ToUInt16(Buffer, cursor);                 // MessageLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Message = Util.Encoding.GetString(Buffer, cursor, len);             // Message    (n bytes)
                cursor += len;
            }

            return cursor - StartIndex;
        }
        #endregion

        public string Message { get; set; }

        public UserCommandAppeal(string Message)
        {
            this.Message = Message;
        }

        public UserCommandAppeal(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }
    }
}
