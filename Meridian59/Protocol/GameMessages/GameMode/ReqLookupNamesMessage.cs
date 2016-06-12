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
using System.Text;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Data.Models;
using Meridian59.Common;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class ReqLookupNamesMessage : GameModeMessage
    {        
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                int length = base.ByteLength + TypeSizes.SHORT;

                for (int i = 0; i < Names.Length; i++)
                    length += TypeSizes.SHORT + Names[i].Length;
                
                return length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Names.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (string s in Names)
            {
                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(s.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                Array.Copy(Util.Encoding.GetBytes(s), 0, Buffer, cursor, s.Length);
                cursor += s.Length;
            }

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Names = new string[len];
            for (int i = 0; i < len; i++)
            {
                ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                Names[i] = Util.Encoding.GetString(Buffer, cursor, strlen);
                cursor += strlen;
            } 

            return cursor - StartIndex;
        }
        #endregion

        public string[] Names { get; set; }

        public ReqLookupNamesMessage(string[] Names) 
            : base(MessageTypeGameMode.ReqLookupNames)
        {           
            this.Names = Names;
        }

        public ReqLookupNamesMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }        
    }
}
