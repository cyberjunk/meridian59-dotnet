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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Sphere position in M59 style
    /// </summary>
    [Serializable]
    public class SpherePosition : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<SpherePosition>
    {
        #region Constants
        public const string PROPNAME_ANGLE = "Angle";
        public const string PROPNAME_HEIGHT = "Height";
        #endregion

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
                return TypeSizes.SHORT + TypeSizes.SHORT; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            angle = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            height = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            Array.Copy(BitConverter.GetBytes(angle), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(height), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            angle = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            height = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {           
            *((ushort*)Buffer) = angle;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = height;
            Buffer += TypeSizes.SHORT;
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
        protected ushort angle;
        protected short height;
        #endregion

        #region Properties
        public ushort Angle
        {
            get
            {
                return angle;
            }
            set
            {
                if (angle != value)
                {
                    angle = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANGLE));
                }
            }
        }

        public short Height
        {
            get
            {
                return height;
            }
            set
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEIGHT));
                }
            }
        }
        #endregion

        #region Constructors
        public SpherePosition()
        {
            Clear(false);
        }

        public SpherePosition(ushort Angle, short Height)
        {
            this.angle = Angle;
            this.height = Height;
        }

        public SpherePosition(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe SpherePosition(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Angle = 0;
                Height = 0;
            }
            else
            {
                angle = 0;
                height = 0;
            }
        }
        #endregion

        #region IUpdatable
        public virtual void UpdateFromModel(SpherePosition Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Angle = Model.Angle;
                Height = Model.Height;
            }
            else
            {
                angle = Model.Angle;
                height = Model.Height;
            }
        }
        #endregion
    }
}
