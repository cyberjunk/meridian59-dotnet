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
using Meridian59.Files;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using System.Text;

namespace Meridian59.Files.RSB
{
    /// <summary>
    /// Extends ResourceID to be used in RsbFile
    /// </summary>
    [Serializable]
    public class RsbResourceID : INotifyPropertyChanged, IClearable, IByteSerializable
    {
        public const string PROPNAME_RSBVERSION = "RsbVersion";
        public const string PROPNAME_ID = "ID";
        public const string PROPNAME_TEXT = "Text";
        public const string PROPNAME_LANGUAGE = "Language";
                
        public event PropertyChangedEventHandler PropertyChanged;

        protected uint rsbVersion;
        protected uint id;
        protected string text;
        protected LanguageCode language;

        public uint RsbVersion
        {
            get { return rsbVersion; }
            set
            {
                if (rsbVersion != value)
                {
                    rsbVersion = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RSBVERSION));
                }
            }
        }

        public uint ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ID));
                }
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TEXT));
                }
            }
        }

        public LanguageCode Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LANGUAGE));
                }
            }
        }
       
        public virtual int ByteLength
        {
            get
            {
                int len = TypeSizes.INT + text.Length + TypeSizes.BYTE;

                // language code
                if (rsbVersion >= RsbFile.VERSION5)
                    len += TypeSizes.INT;

                return len;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(id), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // write string
            Array.Copy(Encoding.Default.GetBytes(text), 0, Buffer, cursor, text.Length);
            cursor += text.Length;

            // c-str termination
            Buffer[cursor] = 0x00;
            cursor++;

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            id = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            // version 5 and above has additional language code
            if (rsbVersion >= RsbFile.VERSION5)
            {
                language = (LanguageCode)BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            // look for terminating 0x00 (NULL)
            ushort strlen = 0;
            while ((Buffer.Length > cursor + strlen) && Buffer[cursor + strlen] != 0x00)
                strlen++;

            // get string
            text = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen + TypeSizes.BYTE;

            return cursor - StartIndex;
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

        public RsbResourceID()
            : base()
        {
            Clear(false);
        }

        public RsbResourceID(uint ID, string Text, uint RsbVersion, LanguageCode Language = LanguageCode.English)
        {
            id = ID;
            text = Text;
            rsbVersion = RsbVersion;
            language = Language;
        }

        public RsbResourceID(uint RsbVersion, byte[] Buffer, int StartIndex = 0)
        {
            rsbVersion = RsbVersion;
            ReadFrom(Buffer, StartIndex);
        }

        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ID = 0;
                Text = String.Empty;
                RsbVersion = RsbFile.VERSION5;
                Language = LanguageCode.English;
            }
            else
            {
                id = 0;
                text = String.Empty;
                rsbVersion = RsbFile.VERSION5;
                language = LanguageCode.English;
            }
        }
    
        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
    }
}

