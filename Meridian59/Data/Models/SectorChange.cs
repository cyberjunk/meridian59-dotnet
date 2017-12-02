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
    public class SectorChange : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_SECTORNR    = "SectorNr";
        public const string PROPNAME_DEPTH       = "Height";
        public const string PROPNAME_SCROLLSPEED = "Speed";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected ushort sectorNr;
        protected RooSectorFlags.DepthType depth;
        protected TextureScrollSpeed scrollSpeed;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
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

        public RooSectorFlags.DepthType Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DEPTH));
                }
            }
        }

        public TextureScrollSpeed ScrollSpeed
        {
            get { return scrollSpeed; }
            set
            {
                if (this.scrollSpeed != value)
                {
                    this.scrollSpeed = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SCROLLSPEED));
                }
            }
        }
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get
            {
                return TypeSizes.SHORT + TypeSizes.BYTE + TypeSizes.BYTE;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
                   
            Array.Copy(BitConverter.GetBytes(sectorNr), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = (byte)depth;
            cursor++;

            Buffer[cursor] = (byte)scrollSpeed;
            cursor++;

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            sectorNr = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            depth = (RooSectorFlags.DepthType)Buffer[cursor];
            cursor++;

            scrollSpeed = (TextureScrollSpeed)Buffer[cursor];
            cursor++;

            return cursor - StartIndex;;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            *((ushort*)Buffer) = sectorNr;
            Buffer += TypeSizes.SHORT;

            Buffer[0] = (byte)depth;
            Buffer++;

            Buffer[0] = (byte)scrollSpeed;
            Buffer++;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            sectorNr = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;  

            depth = (RooSectorFlags.DepthType)Buffer[0];
            Buffer++;

            scrollSpeed = (TextureScrollSpeed)Buffer[0];
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
        public SectorChange()
        {
            Clear(false);
        }
        
        public SectorChange(ushort SectorNr, RooSectorFlags.DepthType Depth, TextureScrollSpeed ScrollSpeed)
        {
            this.sectorNr = SectorNr;
            this.depth = Depth;
            this.scrollSpeed = ScrollSpeed;
        }

        public SectorChange(byte[] Buffer, int StartIndex = 0)
        {
            this.ReadFrom(Buffer, StartIndex);
        }

        public unsafe SectorChange(ref byte* Buffer)
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
                SectorNr = 0;
                Depth = 0;
                ScrollSpeed = 0;
            }
            else
            {
                sectorNr = 0;
                depth = 0;
                scrollSpeed = 0;
            }
        }
        #endregion
    }
}
