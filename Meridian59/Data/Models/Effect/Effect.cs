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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    [Serializable]
    public abstract class Effect : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        public const string PROPNAME_ISACTIVE = "IsActive";

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion
        
        #region IByteSerializable
        public virtual int ByteLength
        {
            get { return TypeSizes.SHORT; }
        }

        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }

            set
            {               
                ReadFrom(value);              
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes((ushort)EffectType), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            EffectType = (EffectType)BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;
            
            return cursor - StartIndex;
        }

        public unsafe virtual void WriteTo(ref byte* Buffer)
        {
            *((ushort*)Buffer) = (ushort)EffectType;
            Buffer += TypeSizes.SHORT;
        }

        public unsafe virtual void ReadFrom(ref byte* Buffer)
        {
            EffectType = (EffectType)(*((ushort*)Buffer));
            Buffer += TypeSizes.SHORT;
        }
        #endregion

        protected bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISACTIVE));
                }
            }
        }

        public EffectType EffectType { get; private set; }

        #region Constructors
        public Effect() { }

        public Effect(EffectType EffectType)
        {
            this.EffectType = EffectType;
        }

        public Effect(byte[] Buffer, int StartIndex)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe Effect(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Extracts an effect from Buffer, 
        /// </summary>
        /// <param name="Buffer">Buffer to extract from</param>
        /// <param name="StartIndex">Index to start reading</param>
        /// <param name="Length">Bytes to read</param>
        /// <returns>Typed effect class instance or NULL</returns>
        public static Effect ExtractEffect(byte[] Buffer, int StartIndex, int Length)
        {
            Effect returnValue = null;

            EffectType effecttype = (EffectType)BitConverter.ToUInt16(Buffer, StartIndex);
            switch (effecttype)
            {
                case EffectType.Invert:                                             // 1
                    returnValue = new EffectInvert(Buffer, StartIndex);
                    break;

                case EffectType.Shake:                                              // 2
                    returnValue = new EffectShake(Buffer, StartIndex);
                    break;

                case EffectType.Paralyze:                                           // 3
                    returnValue = new EffectParalyze(Buffer, StartIndex);
                    break;

                case EffectType.Release:                                            // 4
                    returnValue = new EffectRelease(Buffer, StartIndex);
                    break;

                case EffectType.Blind:                                              // 5
                    returnValue = new EffectBlind(Buffer, StartIndex);
                    break;

                case EffectType.See:                                                // 6
                    returnValue = new EffectSee(Buffer, StartIndex);
                    break;

                case EffectType.Pain:                                               // 7
                    returnValue = new EffectPain(Buffer, StartIndex);
                    break;

                case EffectType.Blur:                                               // 8
                    returnValue = new EffectBlur(Buffer, StartIndex);
                    break;

                case EffectType.Raining:                                            // 9
                    returnValue = new EffectRaining(Buffer, StartIndex);
                    break;

                case EffectType.Snowing:                                            // 10
                    returnValue = new EffectSnowing(Buffer, StartIndex);
                    break;

                case EffectType.ClearWeather:                                       // 11
                    returnValue = new EffectClearWeather(Buffer, StartIndex);
                    break;

                case EffectType.Sand:                                               // 12
                    returnValue = new EffectSand(Buffer, StartIndex);
                    break;

                case EffectType.ClearSand:                                          // 13
                    returnValue = new EffectClearSand(Buffer, StartIndex);
                    break;

                case EffectType.Waver:                                              // 14
                    returnValue = new EffectWaver(Buffer, StartIndex);
                    break;

                case EffectType.FlashXLat:                                          // 15
                    returnValue = new EffectFlashXLat(Buffer, StartIndex);
                    break;

                case EffectType.WhiteOut:                                           // 16
                    returnValue = new EffectWhiteOut(Buffer, StartIndex);
                    break;

                case EffectType.XLatOverride:                                       // 17
                    returnValue = new EffectXLatOverride(Buffer, StartIndex);

                    break;
                default:
                    returnValue = new EffectGeneric(Buffer, StartIndex, Length);
                    break;
              
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                IsActive = false;
            }
            else
            {
                isActive = false;
            }
        }
        #endregion
    }
}
