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
using Meridian59.Protocol.Enums;
using Meridian59.Data.Models;
using Meridian59.Common.Constants;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// This message transfers contents of guild hall chests and others.   
    /// </summary>
    [Serializable]
    public class ObjectContentsMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + ObjectID.ByteLength;
                
                len += TypeSizes.SHORT;
                foreach (ObjectBase obj in ContentObjects)
                    len += obj.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            cursor += ObjectID.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(ContentObjects.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (ObjectBase inventoryObject in ContentObjects)
                cursor += inventoryObject.WriteTo(Buffer, cursor);
            
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ObjectID = new ObjectID(Buffer, cursor);
            cursor += ObjectID.ByteLength;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ContentObjects = new ObjectBase[len];
            for (int i = 0; i < len; i++)
            {
                ContentObjects[i] = new ObjectBase(true, Buffer, cursor);
                cursor += ContentObjects[i].ByteLength;
            }

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            ObjectID.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)ContentObjects.Length;
            Buffer += TypeSizes.SHORT;

            foreach (ObjectBase obj in ContentObjects)
                obj.WriteTo(ref Buffer);
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            ObjectID = new ObjectID(ref Buffer);

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            ContentObjects = new ObjectBase[len];
            for (int i = 0; i < len; i++)
                ContentObjects[i] = new ObjectBase(true, ref Buffer);
        }
        #endregion

        public ObjectID ObjectID { get; set; }
        public ObjectBase[] ContentObjects { get; set; }

        public ObjectContentsMessage(ObjectID ObjectID, ObjectBase[] InventoryObjects) 
            : base(MessageTypeGameMode.ObjectContents)
        {
            this.ObjectID = ObjectID;
            this.ContentObjects = InventoryObjects;
        }

        public ObjectContentsMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe ObjectContentsMessage(ref byte* Buffer)
            : base(ref Buffer) { } 
    }
}
