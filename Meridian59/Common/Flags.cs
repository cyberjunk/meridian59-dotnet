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

namespace Meridian59.Common
{
    /// <summary>
    /// Base class for uint flags.
    /// </summary>
    [Serializable]
    public abstract class Flags : INotifyPropertyChanged, IByteSerializableFast, IClearable
    {
        /// <summary>
        /// 
        /// </summary>
        public const string PROPNAME_FLAGS = "Flags";

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected uint flags;

        /// <summary>
        /// The flags
        /// </summary>
        public uint Value
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value"></param>
        public Flags(uint Value = 0)
        {
            flags = Value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public Flags(byte[] Buffer, int StartIndex = 0)
        {
            this.ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe Flags(ref byte* Buffer)
        {
            this.ReadFrom(ref Buffer);
        }

        /// <summary>
        /// Overriden. Will return value of flags as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return flags.ToString();
        }
        
        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Value = 0;
            }
            else
            {
                flags = 0;
            }
        }
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get
            {
                return TypeSizes.INT;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Value), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Value = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex; ;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = Value;
            Buffer += TypeSizes.INT;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            Value = *((uint*)Buffer);
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
        }
        #endregion
    }
}
