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
    /// A generic implementation able to hold any gamemessage (just a byte[] wrapper).
    /// </summary>
    [Serializable]
    public class GenericGameMessage : GameMessage
    {
        /// <summary>
        /// Data attached to this message.
        /// </summary>
        public byte[] Data { get; set; }

        #region IByteSerializable implementation
        public override int ByteLength {
            get {
                return base.ByteLength + Data.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(Data, 0, Buffer, cursor, Data.Length);
            cursor += Data.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            int toread = (StartIndex + MessageHeader.HEADERLENGTH + Header.BodyLength) - (StartIndex + cursor);

            Data = new byte[toread];   
            Array.Copy(Buffer, cursor, Data, 0, toread);
            cursor += toread;
            
            return cursor - StartIndex;
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PI"></param>
        /// <param name="Data"></param>
        public GenericGameMessage(byte PI, byte[] Data)
            : base(PI)
        {
            this.Data = Data;
            this.Header.BodyLength = Convert.ToUInt16(TypeSizes.BYTE + Data.Length);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public GenericGameMessage(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }
    }
}
