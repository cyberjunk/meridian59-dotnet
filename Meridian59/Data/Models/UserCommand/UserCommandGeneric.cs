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

namespace Meridian59.Data.Models
{
    [Serializable]
    public class UserCommandGeneric : UserCommand
    {
        public override UserCommandType CommandType { get { return commandType; } }
        private UserCommandType commandType = 0;

        #region IByteSerializable implementation
        public override int ByteLength { 
            get { 
                // CommandType + Generic Byte Arr length
                return TypeSizes.BYTE + Data.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;                     // Type     (1 byte)
            cursor++;
            
            Array.Copy(Data, 0, Buffer, cursor, Data.Length);       // Data     (n bytes)
            cursor += Data.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            commandType = (UserCommandType)Buffer[cursor];
            cursor++;                                               // Type     (1 byte)

            Array.Copy(Buffer, cursor, Data, 0, Data.Length);
            cursor += Data.Length;

            return cursor - StartIndex;
        }
        #endregion

        public byte[] Data { get; set; }

        public UserCommandGeneric(UserCommandType CommandType, byte[] Data)
        {
            this.commandType = CommandType;
            this.Data = Data;           
        }

        public UserCommandGeneric(byte[] Buffer, int StartIndex = 0, int Length = 0)
        {
            this.Data = new byte[Length - TypeSizes.BYTE];
            ReadFrom(Buffer, StartIndex);
        }
    }
}
