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
using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EffectGeneric : Effect
    {       
        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return base.ByteLength + Data.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(Data, 0, Buffer, cursor, Data.Length);
            cursor += Data.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            Array.Copy(Buffer, cursor, Data, 0, Data.Length);
            cursor += Data.Length;

            return cursor - StartIndex;
        }
        #endregion

        public byte[] Data { get; set; }

        public EffectGeneric(EffectType EffectType, byte[] Data)
            : base(EffectType)
        {
            this.Data = Data;           
        }

        public EffectGeneric(byte[] Buffer, int StartIndex = 0, int Length = 0)
            : base()
        {
            this.Data = new byte[Length - TypeSizes.SHORT];
            ReadFrom(Buffer, StartIndex);
        }
    }
}
