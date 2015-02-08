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

namespace Meridian59.Files.BGF
{
    /// <summary>
    /// A FrameSet(=Group) links frames together which represent the same object,
    /// at the same moment of time and motion, but from different angles.
    /// </summary>
    public class BgfFrameSet : IByteSerializable, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_NUM = "Num";
        public const string PROPNAME_FRAMEINDICES = "FrameIndices";
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
                return TypeSizes.INT + (TypeSizes.INT * FrameIndices.Count);
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(FrameIndices.Count), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            foreach (int Index in FrameIndices)
            {
                Array.Copy(BitConverter.GetBytes(Index), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;
            }

            return ByteLength;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            int len = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            FrameIndices = new List<int>(len);
            for (int i = 0; i < len; i++)
            {
                FrameIndices.Add(BitConverter.ToInt32(Buffer, cursor));
                cursor += TypeSizes.INT;
            }

            return ByteLength;
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
        protected uint num;
        protected List<int> frameIndices = new List<int>();
        #endregion

        #region Properties
        /// <summary>
        /// The num of this group/frameset (1 based)
        /// </summary>
        public uint Num
        {
            get { return num; }
            set
            {
                if (num != value)
                {
                    num = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUM));
                }
            }
        }

        /// <summary>
        /// List with frames that form this Group
        /// </summary>
        public List<int> FrameIndices 
        {
            get { return frameIndices; }
            protected set
            {
                if (frameIndices != value)
                {
                    frameIndices = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FRAMEINDICES));
                }
            }
        }
      
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public BgfFrameSet()
        {
            Clear(false);      
        }

        /// <summary>
        /// Constructor by value
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="FrameIndices"></param>
        public BgfFrameSet(uint Num, List<int> FrameIndices)
        {
            num = Num;
            frameIndices = FrameIndices;
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public BgfFrameSet(uint Num, byte[] Buffer, int StartIndex = 0)
        {
            num = Num;
            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return num.ToString();
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                num = 1;
                frameIndices.Clear();
            }
            else
            {
                Num = 1;
                frameIndices.Clear();
            }
        }
        #endregion
    }
}
