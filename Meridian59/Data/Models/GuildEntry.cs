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
    /// A Meridian59 Guild notation in Guild window.
    /// </summary>
    [Serializable]
    public class GuildEntry : ObjectID, IClearable
    {
        public const string PROPNAME_NAME = "Name";

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return base.ByteLength + TypeSizes.SHORT + name.Length; 
            } 
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);                    // ID (4/8 bytes)           

            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);      // NameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            name = Encoding.Default.GetString(Buffer, cursor, strlen);  // Name (n bytes)
            cursor += strlen;

            return cursor - StartIndex; 
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            cursor += base.WriteTo(Buffer, cursor);                                                                 // ID (4/8 bytes)

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(name.Length)), 0, Buffer, cursor, TypeSizes.SHORT);   // NameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(name), 0, Buffer, cursor, name.Length);                            // Name (n bytes)
            cursor += name.Length;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            name = new string((sbyte*)Buffer, 0, len);
            Buffer += len;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            fixed (char* pName = name)
            {
                ushort len = (ushort)name.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                int a, b; bool c;
                Encoding.Default.GetEncoder().Convert(pName, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }
        }
        #endregion

        protected string name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NAME));
                }
            }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public GuildEntry() : base() { }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ID"></param>
        /// <param name="Count"></param>
        public GuildEntry(string Name, uint ID, uint Count = 0)
            : base(ID, Count)
        {
            name = Name;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public GuildEntry(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        /// <summary>
        /// Constructor by pointer parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe GuildEntry(ref byte* Buffer)
            : base(ref Buffer) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Name = String.Empty;                
            }
            else
            {
                name = String.Empty;
            }
        }
    }
}
