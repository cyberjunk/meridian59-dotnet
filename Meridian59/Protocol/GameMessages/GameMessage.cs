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

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Abstract base implementation of a GameMessage exchanged
    /// between server and client.
    /// </summary>
    [Serializable]
    public class GameMessage : MessageHeader
    {
        #region Constructors
        public GameMessage(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex = 0) { }

        public unsafe GameMessage(ref byte* Buffer)
            : base(ref Buffer) { }

        public GameMessage(byte PI)
            : base() 
        {
            this.PI = PI;
        }

        public GameMessage()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The unique identifier for the type of the GameMessage.
        /// Interpretation depends on type of message (Login/Game)
        /// </summary>
        public virtual byte PI { get; set; }

        /// <summary>
        /// Stores the plain, received encrypted PI for later use
        /// </summary>
        public byte EncryptedPI { get; set; }

        /// <summary>
        /// A string description of the message.
        /// </summary>
        public virtual string Description { get { return String.Empty; } }
        
        /// <summary>
        /// Length of the data in bytes.
        /// </summary>
        public int DataLength { get { return BodyLength - TypeSizes.BYTE; } }

        /// <summary>
        /// Creates a byte[] of length 'DataLength' with all data values serialized.
        /// Data does not include the message type (PI).
        /// Note: This just copies from 'Bytes' property.
        /// </summary>
        public byte[] DataBytes
        {
            get
            {
                byte[] data = new byte[DataLength];

                // copy data block from fully serializes 'Bytes'
                Array.Copy(Bytes, GameMessage.HEADERLENGTH + TypeSizes.BYTE, data, 0, DataLength);
                
                return data;
            }
        }

        /// <summary>
        /// Creates a byte[] of length 'BodyLength' with all data values serialized.
        /// Body does include the message type (PI).
        /// Note: This just copies from 'Bytes' property. 
        /// </summary>
        public byte[] BodyBytes
        {
            get
            {
                byte[] body = new byte[BodyLength];

                // copy body block from fully serializes 'Bytes'
                Array.Copy(Bytes, GameMessage.HEADERLENGTH, body, 0, BodyLength);

                return body;
            }
        }
        #endregion

        /// <summary>
        /// Returns a clone of type GenericGameMessage
        /// </summary>
        /// <returns></returns>
        public GenericGameMessage CloneToGeneric()
        {
            GenericGameMessage genMsg = new GenericGameMessage(this.Bytes);

            // clone values not read from bytes
            genMsg.TransferDirection = TransferDirection;
            genMsg.MemoryStartAddress = MemoryStartAddress;
            genMsg.EncryptedPI = EncryptedPI;

            return genMsg;
        }

        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.BYTE;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Buffer[cursor] = PI;
            cursor++;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);
            
            PI = Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            PI = Buffer[0];
            Buffer++;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            Buffer[0] = PI;
            Buffer++;
        }
        #endregion      
    }
}
