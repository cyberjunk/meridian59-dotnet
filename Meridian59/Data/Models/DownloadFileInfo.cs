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
using System.ComponentModel;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// DownloadFileInfo for updates.
    /// </summary>
    [Serializable]
    public class DownloadFileInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public int ByteLength { 
            get {
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.SHORT + fileName.Length; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            time = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            flags = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
            
            size = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            fileName = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(time), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(flags), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(size), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(fileName.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(fileName), 0, Buffer, cursor, fileName.Length);
            cursor += fileName.Length;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            time = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            flags = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            size = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            fileName = new string((sbyte*)Buffer, 0, len);
            Buffer += len;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = time;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = flags;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = size;
            Buffer += TypeSizes.INT;

            int a, b; bool c;
            fixed (char* pString = fileName)
            {
                ushort len = (ushort)fileName.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;
               
                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }
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

        #region Fields
        protected uint time;
        protected uint flags;
        protected uint size;
        protected string fileName;
        #endregion

        #region Properties
        public uint Time
        {
            get
            {
                return time;
            }
            set
            {
                if (time != value)
                {
                    time = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Time"));
                }
            }
        }

        public uint Flags
        {
            get
            {
                return flags;
            }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Flags"));
                }
            }
        }

        public uint Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size != value)
                {
                    size = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Size"));
                }
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("FileName"));
                }
            }
        }
        #endregion

        #region Constructors
        public DownloadFileInfo()
        {
            Clear(false);
        }

        public DownloadFileInfo(uint Time, uint Flags, uint Size, string FileName)
        {
            this.time = Time;
            this.flags = Flags;
            this.size = Size;
            this.fileName = FileName;
        }

        public DownloadFileInfo(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe DownloadFileInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Time = 0;
                Flags = 0;
                Size = 0;
                FileName = String.Empty;
            }
            else
            {
                time = 0;
                flags = 0;
                size = 0;
                fileName = String.Empty;
            }
        }
        #endregion
    }
}
