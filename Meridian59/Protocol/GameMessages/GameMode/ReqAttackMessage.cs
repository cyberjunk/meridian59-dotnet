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

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class ReqAttackMessage : GameModeMessage
    {
        /* Constants for <attack_info> for BP_REQ_ATTACK message */
        public const byte ATTACK_NORMAL = 1;

        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.INT + TypeSizes.BYTE;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Buffer[cursor] = Info;
            cursor++;

            Array.Copy(BitConverter.GetBytes(ObjectID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
           
            Buffer[cursor] = Unknown1;
            cursor++;
            
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            Info = Buffer[cursor];
            cursor++;

            ObjectID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;          

            Unknown1 = Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }
        #endregion

        #region Properties
        public byte Info { get; set; }
        public uint ObjectID { get; set; }
        public byte Unknown1 { get; set; }
        #endregion

        #region Constructors
        public ReqAttackMessage(byte Info, uint ObjectID) 
            : base(MessageTypeGameMode.ReqAttack)
        {
            this.Info = Info;
            this.ObjectID = ObjectID;
            this.Unknown1 = 0;         
        }

        public ReqAttackMessage(byte[] Buffer, int StartIndex = 0, bool IsTCP = true) 
            : base (Buffer, StartIndex, IsTCP) { }
        #endregion
    }
}
