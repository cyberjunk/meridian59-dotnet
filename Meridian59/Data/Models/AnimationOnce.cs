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
    /// An animation which is played once without repeat.
    /// </summary>
    public class AnimationOnce : Animation, IClearable
    {
        #region Constants
        public const string PROPNAME_PERIOD = "Period";
        public const string PROPNAME_GROUPLOW = "GroupLow";
        public const string PROPNAME_GROUPHIGH = "GroupHigh";
        public const string PROPNAME_GROUPFINAL = "GroupFinal";
        public const string PROPNAME_FINISHED = "Finished";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                // Sum up value lengths
                return TypeSizes.BYTE + TypeSizes.INT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT; } }
        
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (byte)AnimationType;                                               // Type       (1 byte)
            cursor++;

            Array.Copy(BitConverter.GetBytes(period), 0, Buffer, cursor, TypeSizes.INT);        // Period     (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(groupLow), 0, Buffer, cursor, TypeSizes.SHORT);    // GroupLow   (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(groupHigh), 0, Buffer, cursor, TypeSizes.SHORT);   // GroupHigh  (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(groupFinal), 0, Buffer, cursor, TypeSizes.SHORT);  // GroupFinal (2 bytes)
            cursor += TypeSizes.SHORT;

            return ByteLength;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            if ((AnimationType)Buffer[StartIndex] != AnimationType)
                throw new Exception(ERROR1);
            else
            {
                int cursor = StartIndex + TypeSizes.BYTE;
                
                period = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                groupLow = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                groupHigh = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                groupFinal = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                // set initial group
                currentGroup = groupLow;
            }

            return ByteLength;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)AnimationType;
            Buffer++;

            *((uint*)Buffer) = period;
            Buffer += TypeSizes.INT;

            *((ushort*)Buffer) = groupLow;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = groupHigh;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = groupFinal;
            Buffer += TypeSizes.SHORT;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            if ((AnimationType)Buffer[0] != AnimationType)
                throw new Exception(ERROR1);
            else
            {
                Buffer++;

                period = *((uint*)Buffer);
                Buffer += TypeSizes.INT;

                groupLow = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                groupHigh = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                groupFinal = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                // set initial group
                currentGroup = groupLow;
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
        protected uint period;
        protected ushort groupLow;
        protected ushort groupHigh;
        protected ushort groupFinal;
        protected bool finished;
        #endregion

        #region Properties
        /// <summary>
        /// Type of this animation
        /// </summary>
        public override AnimationType AnimationType { get { return AnimationType.ONCE; } }

        /// <summary>
        /// Interval for changing to next frame
        /// </summary>
        public uint Period
        {
            get { return period; }
            set
            {
                if (period != value)
                {
                    period = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PERIOD));
                }
            }
        }

        /// <summary>
        /// Group to start with
        /// </summary>
        public ushort GroupLow
        {
            get { return groupLow; }
            set
            {
                if (groupLow != value)
                {
                    groupLow = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GROUPLOW));
                }
            }
        }

        /// <summary>
        /// Group to go up to
        /// </summary>
        public ushort GroupHigh
        {
            get { return groupHigh; }
            set
            {
                if (groupHigh != value)
                {
                    groupHigh = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GROUPHIGH));
                }
            }
        }

        /// <summary>
        /// Group to show when finished
        /// </summary>
        public ushort GroupFinal
        {
            get { return groupFinal; }
            set
            {
                if (groupFinal != value)
                {
                    groupFinal = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GROUPFINAL));
                }
            }
        }

        /// <summary>
        /// Whether the animation is finished or not
        /// </summary>
        public bool Finished 
        {
            get { return finished; }
            protected set
            {
                if (finished != value)
                {
                    finished = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FINISHED));
                }
            }
        }

        #endregion

        #region Constructors
        public AnimationOnce()
        {
            Clear(false);
        }

        public AnimationOnce(uint Period, ushort GroupLow, ushort GroupHigh, ushort GroupFinal)
        {
            period = Period;
            groupLow = GroupLow;
            groupHigh = GroupHigh;
            groupFinal = GroupFinal;
            currentGroup = GroupLow;           
        }

        public AnimationOnce(byte[] Buffer, int StartIndex)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe AnimationOnce(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Period = 0;
                GroupLow = 1;
                GroupHigh = 1;
                GroupFinal = 1;
                CurrentGroup = 1;
                Finished = false;
            }
            else
            {
                period = 0;
                groupLow = 1;
                groupHigh = 1;
                groupFinal = 1;
                currentGroup = 1;
                finished = false;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public override void Tick(double Tick, double Span)
        {
            if (!Finished)
            {               
                // not first call ?
                if (lastAnimationTick > 0)
                {
                    // convert to ms
                    double span = Tick - lastAnimationTick;

                    // elapsed?
                    if (span >= Period)
                    {
                        // end reached? reset to low group
                        if (CurrentGroup == GroupHigh)
                        {
                            CurrentGroup = GroupFinal;
                            Finished = true;
                        }
                        else
                            CurrentGroup++;

                        lastAnimationTick = Tick;
                    }
                }
                else
                {
                    CurrentGroup = groupLow;
                    lastAnimationTick = Tick;
                }
            }
        }
        #endregion
    }
}
