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
    [Serializable]
    public class ReqOfferMessage : GameModeMessage
    {
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + TradePartner.ByteLength + TypeSizes.SHORT;

                foreach (ObjectID obj in OfferedItems)
                    len += obj.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            cursor += TradePartner.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(OfferedItems.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (ObjectID obj in OfferedItems)
                cursor += obj.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            TradePartner = new ObjectID(Buffer, cursor);
            cursor += TradePartner.ByteLength;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            OfferedItems = new ObjectID[len];
            for (int i = 0; i < len; i++)
            {
                OfferedItems[i] = new ObjectID(Buffer, cursor);
                cursor += OfferedItems[i].ByteLength;
            }

            return cursor - StartIndex;
        }
        #endregion

        public ObjectID TradePartner { get; set; }
        public ObjectID[] OfferedItems { get; set; }

        public ReqOfferMessage(ObjectID TradePartner, ObjectID[] OfferedItems)
            : base(MessageTypeGameMode.ReqOffer)
        {           
            this.TradePartner = TradePartner; 
            this.OfferedItems = OfferedItems;          
        }

        public ReqOfferMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }      
    }
}
