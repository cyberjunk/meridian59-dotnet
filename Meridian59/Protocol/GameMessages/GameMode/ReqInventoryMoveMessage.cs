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

#if !VANILLA

using System;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Requests to swap to inventory items given by their IDs.
    /// </summary>
    [Serializable]
    public class ReqInventoryMoveMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.INT + TypeSizes.INT;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(FromID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(ToID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            FromID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ToID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        #endregion
              
        public uint FromID { get; set; }
        public uint ToID { get; set; }

        public ReqInventoryMoveMessage(uint FromID, uint ToID) 
            : base(MessageTypeGameMode.ReqInventoryMove)
        {
            this.FromID = FromID;
            this.ToID = ToID;
        }

        public ReqInventoryMoveMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }      
    }
}
#endif