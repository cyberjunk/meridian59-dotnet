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
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Common.Enums;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class StatMessage : GameModeMessage
    {       
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.BYTE + Stat.ByteLength;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Buffer[cursor] = (byte)Group;
            cursor++;

            cursor += Stat.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            Group = (StatGroup)Buffer[cursor];
            cursor++;

            Stat = Stat.ExtractStat(Buffer, cursor);
            cursor += Stat.ByteLength;
           
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);
            
            Buffer[0] = (byte)Group;
            Buffer++;

            Stat.WriteTo(ref Buffer);          
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            Group = (StatGroup)Buffer[0];
            Buffer++;

            Stat = Stat.ExtractStat(ref Buffer);
        }

        #endregion

        public StatGroup Group { get; set; }
        public Stat Stat { get; set; }

        public StatMessage(StatGroup Group, Stat Stat) 
            : base(MessageTypeGameMode.Stat)
        {
            this.Group = Group;
            this.Stat = Stat;
        }

        public StatMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe StatMessage(ref byte* Buffer)
            : base (ref Buffer) { }
    }
}
