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
    /// Wall animation change info.
    /// </summary>
    [Serializable]
    public class WallAnimationChange : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_SIDEDEFSERVERID = "SideDefServerID";
        public const string PROPNAME_ANIMATION       = "Animation";
        public const string PROPNAME_ACTION          = "Action";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected ushort sideDefServerID;
        protected Animation animation;
        protected RoomAnimationAction action;

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ushort SideDefServerID
        {
            get { return sideDefServerID; }
            set
            {
                if (sideDefServerID != value)
                {
                    sideDefServerID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SIDEDEFSERVERID));
                }
            }
        }

        public Animation Animation
        {
            get { return animation; }
            set
            {
                if (animation != value)
                {
                    animation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));
                }
            }
        }

        public RoomAnimationAction Action
        {
            get { return action; }
            set
            {
                if (this.action != value)
                {
                    this.action = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ACTION));
                }
            }
        }
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get
            {
                return TypeSizes.SHORT + Animation.ByteLength + TypeSizes.BYTE;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
                   
            Array.Copy(BitConverter.GetBytes(sideDefServerID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            cursor += Animation.WriteTo(Buffer, cursor);

            Buffer[cursor] = (byte)Action;
            cursor++;

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            SideDefServerID = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Animation = Animation.ExtractAnimation(Buffer, cursor);
            cursor += Animation.ByteLength;

            Action = (RoomAnimationAction)Buffer[cursor];
            cursor++;

            return cursor - StartIndex;;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            *((ushort*)Buffer) = SideDefServerID;
            Buffer += TypeSizes.SHORT;

            Animation.WriteTo(ref Buffer);

            Buffer[0] = (byte)Action;
            Buffer++;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            SideDefServerID = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Animation = Animation.ExtractAnimation(ref Buffer);

            Action = (RoomAnimationAction)Buffer[0];
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
        public WallAnimationChange()
        {
            Clear(false);
        }
        
        public WallAnimationChange(ushort SideDefServerID, Animation Animation, RoomAnimationAction Action)
        {
            this.sideDefServerID = SideDefServerID;
            this.animation = Animation;
            this.action = Action;
        }

        public WallAnimationChange(byte[] Buffer, int StartIndex = 0)
        {
            this.ReadFrom(Buffer, StartIndex);
        }

        public unsafe WallAnimationChange(ref byte* Buffer)
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
                SideDefServerID = 0;
                Animation = new AnimationNone();
                Action = 0;
            }
            else
            {
                sideDefServerID = 0;
                animation = new AnimationNone();
                action = 0;
            }
        }
        #endregion
    }
}
