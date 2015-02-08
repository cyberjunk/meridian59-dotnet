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
using System.Text;
using Meridian59.Protocol.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class ChangeResourceMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.INT + TypeSizes.SHORT + NewValue.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(ResourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(NewValue.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(NewValue), 0, Buffer, cursor, NewValue.Length);
            cursor += NewValue.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ResourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            NewValue = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            int a, b; bool c;

            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = ResourceID;
            Buffer += TypeSizes.INT;

            fixed (char* pString = NewValue)
            {
                ushort len = (ushort)NewValue.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            ResourceID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            NewValue = new string((sbyte*)Buffer, 0, len);
            Buffer += len;
        }
        #endregion
        
        public uint ResourceID { get; set; }
        public string NewValue { get; set; }
        
        public ChangeResourceMessage(uint ResourceID, string NewValue) 
            : base(MessageTypeGameMode.ChangeResource)
        {         
            this.ResourceID = ResourceID;
            this.NewValue = NewValue;
        }

        public ChangeResourceMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe ChangeResourceMessage(ref byte* Buffer)
            : base(ref Buffer) { }  
    }
}
