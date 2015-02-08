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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class StatGroupMessage : GameModeMessage
    {       
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                // group, listlen
                int length = base.ByteLength + TypeSizes.BYTE + TypeSizes.BYTE;

                foreach (Stat stat in Stats)
                    length += stat.ByteLength;

                return length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Buffer[cursor] = (byte)Group;
            cursor++;

            Buffer[cursor] = (byte)Stats.Length;
            cursor++;

            foreach (Stat stat in Stats)
                cursor += stat.WriteTo(Buffer, cursor);
           
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            Group = (StatGroup)Buffer[cursor];
            cursor++;

            byte listlength = Buffer[cursor];
            cursor++;

            Stats = new Stat[listlength];
            for (int i = 0; i < listlength; i++)
            {
                Stats[i] = Stat.ExtractStat(Buffer, cursor);
                cursor += Stats[i].ByteLength;
            }
                      
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            Buffer[0] = (byte)Group;
            Buffer++;

            Buffer[0] = (byte)Stats.Length;
            Buffer++;

            foreach (Stat stat in Stats)
                stat.WriteTo(ref Buffer);
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            Group = (StatGroup)Buffer[0];
            Buffer++;

            byte listlength = Buffer[0];
            Buffer++;

            Stats = new Stat[listlength];
            for (int i = 0; i < listlength; i++)           
                Stats[i] = Stat.ExtractStat(ref Buffer);                     
        }
        #endregion

        #region Properties
        public StatGroup Group { get; set; }
        public Stat[] Stats { get; set; }
        #endregion

        #region Constructors
        public StatGroupMessage(StatGroup Group, Stat[] Stats) 
            : base(MessageTypeGameMode.StatGroup)
        {
            this.Group = Group;
            this.Stats = Stats; 
        }

        public StatGroupMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe StatGroupMessage(ref byte* Buffer)
            : base(ref Buffer) { }
        #endregion
    }
}
