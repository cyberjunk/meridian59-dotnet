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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Files;
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Bar data used for character attributes and more
    /// </summary>
    [Serializable]
    public class StatNumeric : Stat, IUpdatable<StatNumeric>
    {
        #region Constants
        public const string PROPNAME_TAG = "Tag";
        public const string PROPNAME_VALUECURRENT = "ValueCurrent";
        public const string PROPNAME_VALUERENDERMIN = "ValueRenderMin";
        public const string PROPNAME_VALUERENDERMAX = "ValueRenderMax";
        public const string PROPNAME_VALUEMAXIMUM = "ValueMaximum";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.BYTE + 
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;
            }
        }      
 
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
          
            Buffer[cursor] = (byte)Type;
            cursor++;

            Buffer[cursor] = tag;
            cursor++;

            Array.Copy(BitConverter.GetBytes(valueCurrent), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(valueRenderMin), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(valueRenderMax), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(valueMaximum), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            // skip type
            cursor++;
           
            tag = Buffer[cursor];
            cursor++;

            valueCurrent = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            valueRenderMin = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            valueRenderMax = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            valueMaximum = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }       
        
        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);
           
            Buffer[0] = (byte)Type;
            Buffer++;

            Buffer[0] = tag;
            Buffer++;

            *((int*)Buffer) = valueCurrent;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = valueRenderMin;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = valueRenderMax;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = valueMaximum;
            Buffer += TypeSizes.INT;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            // skip type
            Buffer++;

            tag = Buffer[0];
            Buffer++;

            valueCurrent = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            valueRenderMin = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            valueRenderMax = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            valueMaximum = *((int*)Buffer);
            Buffer += TypeSizes.INT;
        }
       
        #endregion

        #region Fields
        protected byte tag;
        protected int valueCurrent;
        protected int valueRenderMin;
        protected int valueRenderMax;
        protected int valueMaximum;
        #endregion

        #region Properties

        /// <summary>
        /// The Stat type
        /// </summary>
        public override StatType Type
        {
            get { return StatType.Numeric; }
        }

        /// <summary>
        /// The tag of this stat
        /// </summary>
        public byte Tag
        {
            get { return tag; }
            set
            {
                if (tag != value)
                {
                    tag = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TAG));
                }
            }
        }

        /// <summary>
        /// Current value
        /// </summary>
        public int ValueCurrent
        {
            get { return valueCurrent; }
            set
            {
                if (valueCurrent != value)
                {
                    valueCurrent = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VALUECURRENT));
                }
            }
        }

        /// <summary>
        /// Minimum value for rendering
        /// </summary>
        public int ValueRenderMin
        {
            get { return valueRenderMin; }
            set
            {
                if (valueRenderMin != value)
                {
                    valueRenderMin = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VALUERENDERMIN));
                }
            }
        }

        /// <summary>
        /// Maximum value for rendering
        /// </summary>
        public int ValueRenderMax
        {
            get { return valueRenderMax; }
            set
            {
                if (valueRenderMax != value)
                {
                    valueRenderMax = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VALUERENDERMAX));
                }
            }
        }

        /// <summary>
        /// Maximum value
        /// </summary>
        public int ValueMaximum
        {
            get { return valueMaximum; }
            set
            {
                if (valueMaximum != value)
                {
                    valueMaximum = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VALUEMAXIMUM));
                }
            }
        }
        #endregion

        #region Constructors
        public StatNumeric()
        {
            Clear(false);
        }
        
        public StatNumeric(byte Num)
            : base(Num) { }

        public StatNumeric(byte Num, uint ResourceID, byte Tag, int ValueCurrent, int ValueRenderMin, int ValueRenderMax, int ValueMaximum)
            : base(Num, ResourceID)
        {
            this.tag = Tag;
            this.valueCurrent = ValueCurrent;
            this.valueRenderMin = ValueRenderMin;
            this.valueRenderMax = ValueRenderMax;
            this.valueMaximum = ValueMaximum;
        }

        public StatNumeric(byte[] Buffer, int startIndex=0)
        {
            ReadFrom(Buffer, startIndex);
        }

        public unsafe StatNumeric(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Tag = 0;
                ValueCurrent = 0;
                ValueRenderMin = 0;
                ValueRenderMax = 0;
                ValueMaximum = 0;
            }
            else
            {               
                tag = 0;
                valueCurrent = 0;
                valueRenderMin = 0;
                valueRenderMax = 0;
                valueMaximum = 0;
            }
        }
        #endregion

        #region IResourceResolvable
        public override void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (resourceName != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    Resource = M59ResourceManager.GetObject(resourceName);
                }
                else
                {
                    resource = M59ResourceManager.GetObject(resourceName);
                }
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(StatNumeric Model, bool RaiseChangedEvent)
        {
            base.UpdateFromModel(Model, RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Tag = Model.Tag;
                ValueCurrent = Model.ValueCurrent;
                ValueRenderMin = Model.ValueRenderMin;
                ValueRenderMax = Model.ValueRenderMax;
                ValueMaximum = Model.ValueMaximum;            
            }
            else
            {
                tag = Model.Tag;
                valueCurrent = Model.ValueCurrent;
                valueRenderMin = Model.ValueRenderMin;
                valueRenderMax = Model.ValueRenderMax;
                valueMaximum = Model.ValueMaximum;
            }
        }
        #endregion
    }
}
