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
    /// An advertise item in the character selectionscreen.
    /// </summary>
    [Serializable]
    public class CharSelectAd : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_FILENAME = "FileName";
        public const string PROPNAME_URL = "URL";
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
                return TypeSizes.SHORT + fileName.Length + TypeSizes.SHORT + url.Length; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);          // FileNameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            fileName = Encoding.Default.GetString(Buffer, cursor, strlen);  // FileName (n bytes)
            cursor += strlen;

            strlen = BitConverter.ToUInt16(Buffer, cursor);                 // URLLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            url = Encoding.Default.GetString(Buffer, cursor, strlen);       // URL (n bytes)
            cursor += strlen;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(fileName.Length)), 0, Buffer, cursor, TypeSizes.SHORT);       // FileNameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(fileName), 0, Buffer, cursor, fileName.Length);                            // FileName (n bytes)
            cursor += fileName.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(url.Length)), 0, Buffer, cursor, TypeSizes.SHORT);            // URLLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(url), 0, Buffer, cursor, url.Length);                                      // URL (n bytes)
            cursor += url.Length;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            fileName = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            url = new string((sbyte*)Buffer, 0, len);
            Buffer += len;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            int a, b; bool c;

            fixed (char* pString = fileName)
            {
                ushort len = (ushort)fileName.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;
               
                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = url)
            {
                ushort len = (ushort)url.Length;

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
        protected string fileName;
        protected string url;
        #endregion

        #region Properties
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
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FILENAME));
                }
            }
        }

        public string URL
        {
            get
            {
                return url;
            }
            set
            {
                if (url != value)
                {
                    url = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_URL));
                }
            }
        }
        #endregion

        #region Constructors
        public CharSelectAd()
        {
            Clear(false);
        }

        public CharSelectAd(string FileName, string URL)
        {
            this.fileName = FileName;
            this.url = URL;
        }

        public CharSelectAd(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe CharSelectAd(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                FileName = String.Empty;
                URL = String.Empty;
            }
            else
            {
                fileName = String.Empty;
                url = String.Empty;
            }
        }
        #endregion
    }
}
