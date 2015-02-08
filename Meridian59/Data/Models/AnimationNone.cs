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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An animation which has just a static frame
    /// </summary>
    public class AnimationNone : Animation, IClearable
    {
        #region Constants
        public const string PROPNAME_GROUP = "Group";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return TypeSizes.BYTE + TypeSizes.SHORT; } }
        
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (byte)AnimationType;
            cursor++;

            Array.Copy(BitConverter.GetBytes(group), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return ByteLength;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            if ((AnimationType)Buffer[StartIndex] != AnimationType)
                throw new Exception(ERROR1);
            else
            {
                int cursor = StartIndex + TypeSizes.BYTE;

                group = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;
            }

            return ByteLength;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)AnimationType;
            Buffer++;

            *((ushort*)Buffer) = group;
            Buffer += TypeSizes.SHORT;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            if ((AnimationType)Buffer[0] != AnimationType)
                throw new Exception(ERROR1);
            else
            {
                Buffer++;

                group = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;
            }
        }

        public override byte[] Bytes
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
        protected ushort group;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public override AnimationType AnimationType { get { return AnimationType.NONE; } }

        /// <summary>
        /// 
        /// </summary>
        public override ushort CurrentGroup
        {
            get
            {
                return group;
            }
            set
            {
                //
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort Group
        {
            get { return group; }
            set
            {
                if (group != value)
                {
                    group = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GROUP));
                }
            }
        }
        #endregion

        #region Constructors
        public AnimationNone()
        {
            Clear(false);
        }

        public AnimationNone(ushort Group)
        {
            group = Group;
        }

        public AnimationNone(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe AnimationNone(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Group = 1;                
            }
            else
            {
                group = 1;              
            }
        }
        
        #endregion
    }
}
