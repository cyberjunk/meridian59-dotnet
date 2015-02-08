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
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A model for a trade-object used in NPC offers with price.
    /// </summary>
    [Serializable]
    public class TradeOfferObject : ObjectBase
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_PRICE = "Price";       
        #endregion

        #region IByteSerializable
        public override int ByteLength
        { 
            get { 
                return base.ByteLength + TypeSizes.INT; 
            }
        }
    
        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);
            
            price = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex; 
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(price), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
            
            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            price = *((uint*)Buffer);
            Buffer += TypeSizes.INT;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = price;
            Buffer += TypeSizes.INT;
        }
        #endregion

        #region Fields
        protected uint price;
        #endregion

        #region Properties
        /// <summary>
        /// Per unit price of the object.
        /// </summary>
        public uint Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PRICE));
                }
            }
        }  
        #endregion

        #region Constructors
        public TradeOfferObject() 
            : base() { }

        public TradeOfferObject(
            uint ID,
            uint Count, 
            uint OverlayFileRID, 
            uint NameRID, 
            uint Flags,
            ushort LightFlags, 
            byte LightIntensity, 
            ushort LightColor, 
            AnimationType FirstAnimationType, 
            byte ColorTranslation, 
            byte Effect,
            Animation Animation, 
            BaseList<SubOverlay> SubOverlays,            
            uint Price)        
            : base(
                ID, Count, OverlayFileRID, NameRID, Flags, 
                LightFlags, LightIntensity, LightColor, 
                FirstAnimationType, ColorTranslation, Effect, 
                Animation, SubOverlays)
        {
            this.price = Price;
        }

        public TradeOfferObject(byte[] Buffer, int StartIndex = 0)
            : base(true, Buffer, StartIndex) { }

        public unsafe TradeOfferObject(ref byte* Buffer)
            : base(true, ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Price = 0;
            }
            else
            {
                price = 0;
            }
        }
        #endregion
    }
}
