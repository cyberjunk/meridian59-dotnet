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
    /// Tells the client an object on the map has turned.
    /// </summary>
    public class TurnMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.INT + TypeSizes.SHORT;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(ObjectID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Angle), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ObjectID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Angle = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = ObjectID;
            Buffer += TypeSizes.INT;

            *((ushort*)Buffer) = Angle;
            Buffer += TypeSizes.SHORT;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            ObjectID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            Angle = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;
        }

        #endregion
        
        public uint ObjectID { get; set; }
        public ushort Angle { get; set; }      

        public TurnMessage(uint ObjectID, ushort ViewDirection) 
            : base(MessageTypeGameMode.Turn)
        {
            this.ObjectID = ObjectID;
            this.Angle = ViewDirection;           
        }

        public TurnMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe TurnMessage(ref byte* Buffer)
            : base (ref Buffer) { }
    }
}
