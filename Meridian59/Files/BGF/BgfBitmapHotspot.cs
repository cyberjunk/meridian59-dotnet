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

namespace Meridian59.Files.BGF
{
    /// <summary>
    /// A hotspot (point) used as anchor to attach images on others
    /// </summary>
    [Serializable]
    public class BgfBitmapHotspot : IByteSerializableFast, IClearable, INotifyPropertyChanged
    {
        #region Constants
        public const string PROPNAME_INDEX = "Index";
        public const string PROPNAME_X = "X";
        public const string PROPNAME_Y = "Y";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion

        #region IByteSerializable
        public int ByteLength 
        {
            get 
            { 
                return TypeSizes.BYTE + TypeSizes.INT + TypeSizes.INT; 
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (byte)Index;
            cursor++;

            Array.Copy(BitConverter.GetBytes(X), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
            
            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Index = (sbyte)Buffer[cursor];
            cursor++;

            X = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)Index;
            Buffer++;

            *((int*)Buffer) = X;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Y;
            Buffer += TypeSizes.INT;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            Index = (sbyte)Buffer[0];
            Buffer++;

            X = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y = *((int*)Buffer);
            Buffer += TypeSizes.INT;
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
        #endregion

        #region Fields
        protected sbyte index;
        protected int x;
        protected int y;
        #endregion

        #region Properties
        /// <summary>
        /// Hotspot index (can be negative for underlay)
        /// </summary>
        public sbyte Index 
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_INDEX));
                }
            } 
        } 
        
        /// <summary>
        /// X coordinate of the hotspot
        /// </summary>
        public int X 
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_X));
                }
            } 
        }
        
        /// <summary>
        /// Y coordinate of the hotspot
        /// </summary>
        public int Y 
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_Y));
                }
            } 
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public BgfBitmapHotspot()
        {
            Clear(false);         
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public BgfBitmapHotspot(sbyte Index, int X, int Y)
        {
            index = Index;
            x = X;
            y = Y;
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public BgfBitmapHotspot(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe BgfBitmapHotspot(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Index = 0;
                X = 0;
                Y = 0;
            }
            else
            {
                index = 0;
                x = 0;
                y = 0;
            }
        }
        #endregion

        /// <summary>
        /// Overridden. Returns Index as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return index.ToString();
        }
    }
}
