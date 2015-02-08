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
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Response from the server to "Ping" message. 
    /// Contains a decode byte for message type decoding.
    /// </summary>
    [Serializable]
    public class EchoPingMessage : GameModeMessage
    {       
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.INT;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Buffer[cursor] = NewPIDecByte;
            cursor++;

            Array.Copy(BitConverter.GetBytes(ResourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);
            
            NewPIDecByte = Buffer[cursor];
            cursor++;

            ResourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        #endregion
        
        public byte NewPIDecByte { get; set; }
        public uint ResourceID { get; set; }

        public EchoPingMessage(byte NewPIDecByte, uint ResourceID) 
            : base(MessageTypeGameMode.EchoPing)
        {
            this.NewPIDecByte = NewPIDecByte;
            this.ResourceID = ResourceID;        
        }

        public EchoPingMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }        
    }
}
