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
    public class StatGroupsMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + TypeSizes.BYTE;

                foreach (ResourceID obj in ResourceIDs)
                    len += obj.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Buffer[cursor] = Convert.ToByte(ResourceIDs.Length);
            cursor++;

            foreach (ResourceID obj in ResourceIDs)
                cursor += obj.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            byte len = Buffer[cursor];
            cursor++;

            ResourceIDs = new ResourceID[len];
            for (int i = 0; i < len; i++)
            {
                ResourceIDs[i] = new ResourceID(Buffer, cursor);
                cursor += ResourceIDs[i].ByteLength;
            }

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            Buffer[0] = (byte)ResourceIDs.Length;
            Buffer++;

            foreach (ResourceID obj in ResourceIDs)
                obj.WriteTo(ref Buffer);
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            byte len = Buffer[0];
            Buffer++;

            ResourceIDs = new ResourceID[len];
            for (int i = 0; i < len; i++)
                ResourceIDs[i] = new ResourceID(ref Buffer);
            
        }
        #endregion
       
        public ResourceID[] ResourceIDs { get; set; }

        public StatGroupsMessage(ResourceID[] ResourceIDs) 
            : base(MessageTypeGameMode.StatGroups)
        {
            this.ResourceIDs = ResourceIDs;
        }

        public StatGroupsMessage(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex = 0) { }

        public unsafe StatGroupsMessage(ref byte* Buffer)
            : base(ref Buffer) { }
    }
}
