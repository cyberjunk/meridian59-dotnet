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
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class RoomContentsFlagsMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + RoomObjectID.ByteLength + TypeSizes.SHORT;

                foreach (ObjectFlagsUpdate obj in ObjectsToUpdate)
                    len += obj.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
            
            cursor += RoomObjectID.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(ObjectsToUpdate.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (ObjectFlagsUpdate obj in ObjectsToUpdate)
                cursor += obj.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            RoomObjectID = new ObjectID(Buffer, cursor);
            cursor += RoomObjectID.ByteLength;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ObjectsToUpdate = new ObjectFlagsUpdate[len];
            for (int i = 0; i < len; i++)
            {
                ObjectsToUpdate[i] = new ObjectFlagsUpdate(Buffer, cursor);
                cursor += ObjectsToUpdate[i].ByteLength;
            }

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            RoomObjectID.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)ObjectsToUpdate.Length;
            Buffer += TypeSizes.SHORT;

            foreach (ObjectFlagsUpdate obj in ObjectsToUpdate)
                obj.WriteTo(ref Buffer);
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            RoomObjectID = new ObjectID(ref Buffer);
            
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            ObjectsToUpdate = new ObjectFlagsUpdate[len];
            for (int i = 0; i < len; i++)
                ObjectsToUpdate[i] = new ObjectFlagsUpdate(ref Buffer);
        }
        #endregion

        public ObjectID RoomObjectID { get; set; }
        public ObjectFlagsUpdate[] ObjectsToUpdate { get; set; }

        public RoomContentsFlagsMessage(ObjectID RoomObjectID, ObjectFlagsUpdate[] ObjectsToUpdate) 
            : base(MessageTypeGameMode.RoomContentsFlags)
        {           
            this.RoomObjectID = RoomObjectID;
            this.ObjectsToUpdate = ObjectsToUpdate;
        }

        public RoomContentsFlagsMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe RoomContentsFlagsMessage(ref byte* Buffer)
            : base(ref Buffer) { } 
    }
}
