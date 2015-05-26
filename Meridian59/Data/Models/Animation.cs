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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Abstract animation base class.
    /// </summary>
    [Serializable]
    public abstract class Animation : IByteSerializableFast, INotifyPropertyChanged, ITickable
    {
        #region Constants
        public const string PROPNAME_ANIMATIONTYPE = "AnimationType";
        public const string PROPNAME_CURRENTGROUP = "CurrentGroup";
        public const string PROPNAME_GROUPMAX = "GroupMax";
        protected const string ERROR1 = "Wrong 1.Byte (AnimationType) for this AnimationClass";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion
        
        #region IByteSerializable
        
        public abstract int ByteLength { get; }
        public abstract byte[] Bytes { get; }
        public abstract int WriteTo(byte[] Buffer, int StartIndex=0);
        public abstract int ReadFrom(byte[] Buffer, int StartIndex=0);
        public abstract unsafe void WriteTo(ref byte* Buffer);
        public abstract unsafe void ReadFrom(ref byte* Buffer);
        
        #endregion

        #region Fields
        protected double lastAnimationTick = 0;
        protected ushort currentGroup = 0;
        protected int groupMax = -1;
        #endregion

        #region Properties
        public abstract AnimationType AnimationType { get; }
        
        /// <summary>
        /// Current group from the BGF to show.
        /// Updated by calling Update()
        /// </summary>
        public virtual ushort CurrentGroup
        {
            get { return currentGroup; }
            set
            {
                if (currentGroup != value)
                {
                    currentGroup = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CURRENTGROUP));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GroupMax 
        { 
            get { return groupMax; } 
            set 
            {
                if (groupMax != value)
                {
                    groupMax = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GROUPMAX));
                }
            } 
        }

        #endregion

        #region Methods
        /// <summary>
        /// Extracts an animation from Buffer, 
        /// if the 1. read byte signals a known animation type.
        /// </summary>
        /// <param name="Buffer">Buffer to extract from</param>
        /// <param name="StartIndex">Index to start reading</param>
        /// <returns>Typed animation class instance or NULL</returns>
        public static Animation ExtractAnimation(byte[] Buffer, int StartIndex)
        {
            Animation returnValue = null;

            // try to parse the animation
            switch ((AnimationType)Buffer[StartIndex])
            {
                case AnimationType.NONE:
                    returnValue = new AnimationNone(Buffer, StartIndex);
                    break;

                case AnimationType.CYCLE:
                    returnValue = new AnimationCycle(Buffer, StartIndex);
                    break;

                case AnimationType.ONCE:
                    returnValue = new AnimationOnce(Buffer, StartIndex);
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// See managed variant.
        /// </summary>
        /// <param name="Buffer">Referenced pointer to Animation start</param>
        /// <returns>Typed animation class instance or NULL</returns>
        public static unsafe Animation ExtractAnimation(ref byte* Buffer)
        {
            Animation returnValue = null;

            // try to parse the animation
            switch ((AnimationType)Buffer[0])
            {
                case AnimationType.NONE:
                    returnValue = new AnimationNone(ref Buffer);
                    break;

                case AnimationType.CYCLE:
                    returnValue = new AnimationCycle(ref Buffer);
                    break;

                case AnimationType.ONCE:
                    returnValue = new AnimationOnce(ref Buffer);
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets group values and fires a single propertychanged for currentgroup
        /// </summary>
        /// <param name="CurrentGroup"></param>
        /// <param name="GroupMax"></param>
        /// <param name="RaiseChangedEvent"></param>
        public void SetValues(ushort CurrentGroup, int GroupMax, bool RaiseChangedEvent = true)
        {
            currentGroup = CurrentGroup;
            groupMax = GroupMax;

            if (RaiseChangedEvent)
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CURRENTGROUP));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        { 
            return AnimationType.ToString(); 
        }

        /// <summary>
        /// Call regularly to update CurrentGroup
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public virtual void Tick(double Tick, double Span) { }

        #endregion
    }
}
