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
using System.ComponentModel;
using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EffectXLatOverride : Effect
    {
        public const string PROPNAME_XLAT = "XLat";

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return base.ByteLength + TypeSizes.INT; } }
        
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
          
            Array.Copy(BitConverter.GetBytes(XLat), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            XLat = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
            
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = XLat;  
            Buffer += TypeSizes.INT;            
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            XLat = *((uint*)Buffer);
            Buffer += TypeSizes.INT;            
        }
        #endregion

        protected uint xlat;

        public uint XLat 
        {
            get { return xlat; }
            set
            {
                if (xlat != value)
                {
                    xlat = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_XLAT));
                }
            }
        }       

        public EffectXLatOverride(uint XLat = 0)
            : base(EffectType.XLatOverride)
        {
            this.XLat = XLat;           
        }

        public EffectXLatOverride(byte[] Buffer, int StartIndex)
            : base(Buffer, StartIndex) { }

        public unsafe EffectXLatOverride(ref byte* Buffer)
            : base(ref Buffer) { }

        public override string ToString()
        {
            return "EffectXLatOverride";
        }
    }
}
