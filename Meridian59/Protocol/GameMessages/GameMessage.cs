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
using Meridian59.Common.Interfaces;
using System.ComponentModel;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Abstract base implementation of a GameMessage exchanged
    /// between server and client.
    /// </summary>
    [Serializable]
    public class GameMessage : IByteSerializableFast, INotifyPropertyChanged
    {
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

        #region Constructors
        public GameMessage(byte[] Buffer, int StartIndex = 0, bool IsTCP = true)
        {
            if (IsTCP) this.Header = new MessageHeader.Tcp();
            else       this.Header = new MessageHeader.Udp();

            ReadFrom(Buffer, StartIndex);
        }

        public unsafe GameMessage(ref byte* Buffer, bool IsTCP = true)
        {
            if (IsTCP) this.Header = new MessageHeader.Tcp();
            else       this.Header = new MessageHeader.Udp();

            ReadFrom(ref Buffer);
        }

        public GameMessage(byte PI, bool IsTCP = true)
        {
            if (IsTCP) this.Header = new MessageHeader.Tcp();
            else       this.Header = new MessageHeader.Udp();

            this.PI = PI;
        }

        public GameMessage()
        {
            this.Header = new MessageHeader.Tcp();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Transferdirection of the message
        /// </summary>
        public MessageDirection TransferDirection { get; set; }

        /// <summary>
        /// TCP (or UDP) header used for this GameMessage
        /// </summary>
        public MessageHeader Header { get; set; }

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
        public int DataLength { get { return Header.BodyLength - TypeSizes.BYTE; } }

        /// <summary>
        /// Returns Header.Bytes
        /// </summary>
        public byte[] HeaderBytes { get { return Header.Bytes; } }

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
                Array.Copy(Bytes, Header.ByteLength + TypeSizes.BYTE, data, 0, DataLength);
                
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
                byte[] body = new byte[Header.BodyLength];

                // copy body block from fully serializes 'Bytes'
                Array.Copy(Bytes, Header.ByteLength, body, 0, Header.BodyLength);

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
            genMsg.Header.MemoryStartAddress = Header.MemoryStartAddress;
            genMsg.EncryptedPI = EncryptedPI;

            return genMsg;
        }

        #region IByteSerializable implementation
        public virtual int ByteLength
        {
            get
            {
                return Header.ByteLength + TypeSizes.BYTE;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += Header.WriteTo(Buffer, StartIndex);

            Buffer[cursor] = PI;
            cursor++;

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += Header.ReadFrom(Buffer, StartIndex);
            
            PI = Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            Header.ReadFrom(ref Buffer);

            PI = Buffer[0];
            Buffer++;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            Header.WriteTo(ref Buffer);

            Buffer[0] = PI;
            Buffer++;
        }

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
    }
}
