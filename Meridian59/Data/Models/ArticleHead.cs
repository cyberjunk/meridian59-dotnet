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
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An article header used in messages overview of newsglobes.
    /// </summary>
    [Serializable]
    public class ArticleHead : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_NUMBER = "Number";
        public const string PROPNAME_TIME = "Time";
        public const string PROPNAME_POSTER = "Poster";
        public const string PROPNAME_TITLE = "Title";
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
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.SHORT + poster.Length + TypeSizes.SHORT + title.Length; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            number = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            time = MeridianDate.ToDateTime(BitConverter.ToUInt32(Buffer, cursor));
            cursor += TypeSizes.INT;

            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            poster = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen;

            strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            title = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(number), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(MeridianDate.ToMeridianDate(time)), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(poster.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(poster), 0, Buffer, cursor, poster.Length);
            cursor += poster.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(title.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(title), 0, Buffer, cursor, title.Length);
            cursor += title.Length;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            number = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            time = MeridianDate.ToDateTime(*((uint*)Buffer));
            Buffer += TypeSizes.INT;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            poster = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            title = new string((sbyte*)Buffer, 0, len);
            Buffer += len;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            int a, b; bool c;

            *((uint*)Buffer) = number;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = MeridianDate.ToMeridianDate(time);
            Buffer += TypeSizes.INT;

            fixed (char* pString = poster)
            {
                ushort len = (ushort)poster.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;
               
                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = title)
            {
                ushort len = (ushort)title.Length;

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
        protected uint number;
        protected DateTime time;
        protected string poster;
        protected string title;
        #endregion

        #region Properties      
        /// <summary>
        /// 
        /// </summary>
        public uint Number
        {
            get
            {
                return number;
            }
            set
            {
                if (number != value)
                {
                    number = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUMBER));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time
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
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TIME));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Poster
        {
            get
            {
                return poster;
            }
            set
            {
                if (poster != value)
                {
                    poster = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POSTER));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TITLE));
                }
            }
        }
        #endregion

        #region Constructors
        public ArticleHead()
        {
            Clear(false);
        }

        public ArticleHead(uint Number, DateTime Time, string Poster, string Title)
        {
            this.number = Number;
            this.time = Time;
            this.poster = Poster;
            this.title = Title;
        }

        public ArticleHead(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe ArticleHead(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Number = 0;
                Time = MeridianDate.MERIDIANZERO;
                Poster = String.Empty;
                Title = String.Empty;
            }
            else
            {
                number = 0;
                time = MeridianDate.MERIDIANZERO;
                poster = String.Empty;
                title = String.Empty;
            }
        }
        #endregion
    }
}
