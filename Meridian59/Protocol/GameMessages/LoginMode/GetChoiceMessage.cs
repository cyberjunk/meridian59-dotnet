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
using Meridian59.Common.Constants;
using Meridian59.Protocol.Structs;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class GetChoiceMessage : LoginModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(HashTable.HASH1), 0, Buffer, cursor, TypeSizes.INT);   // HASH1        (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(HashTable.HASH2), 0, Buffer, cursor, TypeSizes.INT);   // HASH2        (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(HashTable.HASH3), 0, Buffer, cursor, TypeSizes.INT);   // HASH3        (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(HashTable.HASH4), 0, Buffer, cursor, TypeSizes.INT);   // HASH4        (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(HashTable.HASH5), 0, Buffer, cursor, TypeSizes.INT);   // HASH5        (4 bytes)
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            HashTable.HASH1 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            HashTable.HASH2 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            HashTable.HASH3 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            HashTable.HASH4 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            HashTable.HASH5 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        #endregion   
       
        public HashTable HashTable;
        
        public GetChoiceMessage(HashTable HashTable) 
            : base(MessageTypeLoginMode.GetChoice)
        {            
            this.HashTable = HashTable;                 
        }

        public GetChoiceMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }
    }
}
