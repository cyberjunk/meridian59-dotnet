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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.Exceptions;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// The header implementation of a GameMessage.
    /// </summary>
    [Serializable]
    public abstract class MessageHeader : IByteSerializableFast
    {
        public class Tcp : MessageHeader
        {
            /// <summary>
            /// The TCP headerlength (7)
            /// </summary>
            public const ushort HEADERLENGTH = TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE;

            public override bool IsTCP { get { return true; } }

            public Tcp(ushort Length = 0, ushort CRC = 0, byte ServerSave = 0)
                : base(Length, CRC, ServerSave) { }

            public Tcp(byte[] Buffer, int StartIndex = 0, MessageDirection Direction = MessageDirection.ServerToClient)
                : base(Buffer, StartIndex, Direction) { }

            public unsafe Tcp(ref byte* Buffer, MessageDirection Direction = MessageDirection.ServerToClient)
                : base(ref Buffer, Direction) { }

            public override int ByteLength { get { return HEADERLENGTH; } }

            public override int WriteTo(byte[] Buffer, int StartIndex = 0)
            {
                int cursor = StartIndex;

                Array.Copy(BitConverter.GetBytes(BodyLength), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                Array.Copy(BitConverter.GetBytes(HeaderCRC), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                Array.Copy(BitConverter.GetBytes(BodyLength), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                Buffer[cursor] = HeaderSS;
                cursor++;

                return cursor - StartIndex;
            }

            public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
            {
                int cursor = StartIndex;

                ushort len1 = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                HeaderCRC = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                ushort len2 = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                if (len1 == len2)
                {
                    BodyLength = len1;

                    HeaderSS = Buffer[cursor];
                    cursor++;
                }
                else
                    throw new MismatchLENException(len2, len1);

                return cursor - StartIndex;
            }

            public override unsafe void WriteTo(ref byte* Buffer)
            {
                *((ushort*)Buffer) = BodyLength;
                Buffer += TypeSizes.SHORT;

                *((ushort*)Buffer) = HeaderCRC;
                Buffer += TypeSizes.SHORT;

                *((ushort*)Buffer) = BodyLength;
                Buffer += TypeSizes.SHORT;

                Buffer[0] = HeaderSS;
                Buffer++;
            }

            public override unsafe void ReadFrom(ref byte* Buffer)
            {
                ushort len1 = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                HeaderCRC = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                ushort len2 = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                if (len1 == len2)
                {
                    BodyLength = len1;

                    HeaderSS = Buffer[0];
                    Buffer++;
                }
                else
                    throw new MismatchLENException(len2, len1);
            }

            public override byte[] HeaderBytes
            {
                get
                {
                    byte[] header = new byte[HEADERLENGTH];

                    Array.Copy(BitConverter.GetBytes(BodyLength), 0, header, 0, TypeSizes.SHORT);     // LEN1 (2 bytes)
                    Array.Copy(BitConverter.GetBytes(HeaderCRC), 0, header, 2, TypeSizes.SHORT);      // CRC  (2 bytes)
                    Array.Copy(BitConverter.GetBytes(BodyLength), 0, header, 4, TypeSizes.SHORT);     // LEN2 (2 bytes)
                    header[6] = HeaderSS;                                                             // SS   (1 byte)

                    return header;
                }
            }
        }

        public class Udp : MessageHeader
        {
            /// <summary>
            /// The UDP headerlength (11)
            /// </summary>
            public const ushort HEADERLENGTH = TypeSizes.INT + TypeSizes.INT + TypeSizes.SHORT + TypeSizes.BYTE;

            public override bool IsTCP { get { return false; } }

            public int SessionID { get; set; }
            public uint SequenceNumber { get; set; }

            public Udp(int SessionID = 0, uint SeqNo = 0, ushort CRC = 0, byte ServerSave = 0)
                : base(0, CRC, ServerSave) { }

            public Udp(byte[] Buffer, int StartIndex = 0, MessageDirection Direction = MessageDirection.ServerToClient)
                : base(Buffer, StartIndex, Direction) { }

            public unsafe Udp(ref byte* Buffer, MessageDirection Direction = MessageDirection.ServerToClient)
                : base(ref Buffer, Direction) { }

            public override int ByteLength { get { return HEADERLENGTH; } }

            public override int WriteTo(byte[] Buffer, int StartIndex = 0)
            {
                int cursor = StartIndex;

                Array.Copy(BitConverter.GetBytes(SessionID), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(SequenceNumber), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(HeaderCRC), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                Buffer[cursor] = HeaderSS;
                cursor++;

                return cursor - StartIndex;
            }

            public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
            {
                int cursor = StartIndex;

                SessionID = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                SequenceNumber = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                //BodyLength = len1;

                HeaderCRC = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                HeaderSS = Buffer[cursor];
                cursor++;

                return cursor - StartIndex;
            }

            public override unsafe void WriteTo(ref byte* Buffer)
            {
                *((int*)Buffer) = SessionID;
                Buffer += TypeSizes.INT;

                *((uint*)Buffer) = SequenceNumber;
                Buffer += TypeSizes.INT;

                *((ushort*)Buffer) = HeaderCRC;
                Buffer += TypeSizes.SHORT;

                Buffer[0] = HeaderSS;
                Buffer++;
            }

            public override unsafe void ReadFrom(ref byte* Buffer)
            {
                SessionID = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                SequenceNumber = *((uint*)Buffer);
                Buffer += TypeSizes.INT;

                //BodyLength = len1;

                HeaderCRC = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                HeaderSS = Buffer[0];
                Buffer++;
            }

            public override byte[] HeaderBytes
            {
                get
                {
                    byte[] header = new byte[HEADERLENGTH];

                    Array.Copy(BitConverter.GetBytes(SessionID), 0, header, 0, TypeSizes.INT);        // SessionID (4 bytes)
                    Array.Copy(BitConverter.GetBytes(SequenceNumber), 0, header, 4, TypeSizes.INT);   // SequenceNum (4 bytes)
                    Array.Copy(BitConverter.GetBytes(HeaderCRC), 0, header, 8, TypeSizes.SHORT);      // CRC  (2 bytes)
                    header[10] = HeaderSS;                                                            // SS   (1 byte)

                    return header;
                }
            }
        }

        #region INotifyPropertyChanged
        /// <summary>
        /// Not really used in Message classes.
        /// Most Properties will NOT raise changed events.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        /// <summary>
        /// Length of body
        /// </summary>
        public ushort BodyLength { get; set; }
        
        /// <summary>
        /// The CRC value from header
        /// </summary>
        public ushort HeaderCRC { get; set; }
        
        /// <summary>
        /// The serversave cycle value from header
        /// </summary>
        public byte HeaderSS { get; set; }

        /// <summary>
        /// Can store a memory pointer to the message
        /// </summary>
        public IntPtr MemoryStartAddress { get; set; }
            
        #region IByteSerializable
        public abstract int ByteLength { get; }
        public abstract int WriteTo(byte[] Buffer, int StartIndex = 0);
        public abstract int ReadFrom(byte[] Buffer, int StartIndex = 0);
        public abstract unsafe void WriteTo(ref byte* Buffer);
        public abstract unsafe void ReadFrom(ref byte* Buffer);
        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Length"></param>
        /// <param name="CRC"></param>
        /// <param name="ServerSave"></param>
        public MessageHeader(ushort Length = 0, ushort CRC = 0, byte ServerSave = 0)
        {
            this.BodyLength = Length;
            this.HeaderCRC = CRC;
            this.HeaderSS = ServerSave;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        /// <param name="Direction"></param>
        public MessageHeader(byte[] Buffer, int StartIndex = 0, MessageDirection Direction = MessageDirection.ServerToClient)
        {
            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="Direction"></param>
        public unsafe MessageHeader(ref byte* Buffer, MessageDirection Direction = MessageDirection.ServerToClient)
        {
            ReadFrom(ref Buffer);
        }

        /// <summary>
        /// True if body has length of zero.
        /// </summary>
        public bool HasEmptyBody { get { return (BodyLength == 0); } }

        /// <summary>
        /// True for TCP instances
        /// </summary>
        public abstract bool IsTCP { get; }

        /// <summary>
        /// Creates a byte[] with all header values serialized.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] HeaderBytes { get; }
    }
}
