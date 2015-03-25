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
using System.Collections.Generic;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Files.ROO;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Sector movement info.
    /// </summary>
    [Serializable]
    public class SectorMove : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_TYPE       = "Type";
        public const string PROPNAME_SECTORNR   = "SectorNr";
        public const string PROPNAME_HEIGHT     = "Height";
        public const string PROPNAME_SPEED      = "Speed";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected AnimationType type;
        protected ushort sectorNr;
        protected ushort height;
        protected byte speed;       
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public AnimationType Type
        {
            get { return type; }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TYPE));
                }
            }
        }

        public ushort SectorNr
        {
            get { return sectorNr; }
            set
            {
                if (sectorNr != value)
                {
                    sectorNr = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SECTORNR));
                }
            }
        }

        public ushort Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEIGHT));
                }
            }
        }

        public byte Speed
        {
            get { return speed; }
            set
            {
                if (this.speed != value)
                {
                    this.speed = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPEED));
                }
            }
        }
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get
            {
                return TypeSizes.BYTE + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (Byte)Type;
            cursor++;
                    
            Array.Copy(BitConverter.GetBytes(sectorNr), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(height), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = speed;
            cursor++;
            
            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            type = (AnimationType)Buffer[cursor];
            cursor++;

            sectorNr = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            height = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            speed = Buffer[cursor];
            cursor++;

            return cursor - StartIndex;;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)type;
            Buffer++;

            *((ushort*)Buffer) = sectorNr;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = height;
            Buffer += TypeSizes.SHORT;

            Buffer[0] = speed;
            Buffer++;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            type = (AnimationType)Buffer[0];
            Buffer++;

            sectorNr = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;  

            height = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT; 

            speed = Buffer[0];
            Buffer++;
        }

        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
        }
        #endregion

        #region Constructors
        public SectorMove()
        {
            Clear(false);
        }
        
        public SectorMove(AnimationType Type, ushort SectorNr, ushort Height, byte Speed)
        {
            this.type = Type;
            this.sectorNr = SectorNr;
            this.height = Height;
            this.speed = Speed;
        }

        public SectorMove(byte[] Buffer, int StartIndex = 0)
        {
            this.ReadFrom(Buffer, StartIndex);
        }

        public unsafe SectorMove(ref byte* Buffer)
        {
            this.ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Type = 0;
                SectorNr = 0;
                Height = 0;
                Speed = 0;
            }
            else
            {
                type = 0;
                sectorNr = 0;
                height = 0;
                speed = 0;
            }
        }
        #endregion
    }
}
