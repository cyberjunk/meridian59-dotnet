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
using System.ComponentModel;
using System.Collections.Generic;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Derived from ServerString.
	/// Additionally has blakserv ID and resource ID of sender of the message.
    /// </summary>
    public class ObjectChatMessage : ServerString
    {
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.BYTE + base.ByteLength;
            }
        }
        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(sourceObjectID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(sourceResourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = (byte)transmissionType;
            cursor++;

            cursor += base.WriteTo(Buffer, cursor);   
      
            return cursor - StartIndex;
        }
        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            sourceObjectID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            sourceResourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            transmissionType = (ChatTransmissionType)Buffer[cursor];
            cursor++;

            cursor += base.ReadFrom(Buffer, cursor);           

            return cursor - StartIndex;
        }
        public override unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = sourceObjectID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = sourceResourceID;
            Buffer += TypeSizes.INT;

            Buffer[0] = (byte)transmissionType;
            Buffer++;

            base.WriteTo(ref Buffer);
        }
        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            sourceObjectID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            sourceResourceID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            transmissionType = (ChatTransmissionType)Buffer[0];
            Buffer++;

            base.ReadFrom(ref Buffer);
        }
        #endregion

        #region Fields
        protected uint sourceObjectID;
        protected uint sourceResourceID;
        protected ChatTransmissionType transmissionType;
        #endregion

        #region Properties
        public uint SourceObjectID
        {
            get { return sourceObjectID; }
            set
            {
                if (sourceObjectID != value)
                {
                    sourceObjectID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("SourceObjectID"));
                }
            }
        }

        public uint SourceResourceID
        {
            get { return sourceResourceID; }
            set
            {
                if (sourceResourceID != value)
                {
                    sourceResourceID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("SourceResourceID"));
                }
            }
        }

        public ChatTransmissionType TransmissionType
        {
            get { return transmissionType; }
            set
            {
                if (transmissionType != value)
                {
                    transmissionType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("TransmissionType"));
                }
            }
        }
        #endregion

        #region Constructors

        public ObjectChatMessage() : base(ChatMessageType.ObjectChatMessage) { }

        public ObjectChatMessage(
            uint SourceObjectID,
            uint SourceResourceID,
            ChatTransmissionType TransmissionType,
			StringDictionary LookupList,
            uint ResourceID, 
            List<InlineVariable> Variables,
            List<ChatStyle> Styles) 
            : base(ChatMessageType.ObjectChatMessage, LookupList, ResourceID, Variables, Styles)
        {
            this.sourceObjectID = SourceObjectID;
            this.sourceResourceID = SourceResourceID;
            this.transmissionType = TransmissionType;
        }

		public ObjectChatMessage(StringDictionary LookupList, byte[] Buffer, int StartIndex = 0)
            : base(ChatMessageType.ObjectChatMessage, LookupList, Buffer, StartIndex) { }

		public unsafe ObjectChatMessage(StringDictionary LookupList, ref byte* Buffer)
            : base(ChatMessageType.ObjectChatMessage, LookupList, ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);
        
            if (RaiseChangedEvent)
            {
                SourceObjectID = 0;
                SourceResourceID = 0;
                TransmissionType = ChatTransmissionType.Normal;
            }
            else
            {
                sourceObjectID = 0;
                sourceResourceID = 0;
                transmissionType = ChatTransmissionType.Normal;
            }
        }
        #endregion
    }
}
