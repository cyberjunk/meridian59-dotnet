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
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An entry of the own guild memberlist.
    /// </summary>
    [Serializable]
    public class GuildMemberEntry : ObjectID, IClearable
    {
        #region Constants
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_RANK = "Rank";
        public const string PROPNAME_GENDER = "Gender";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get {
                return base.ByteLength + TypeSizes.SHORT + name.Length + TypeSizes.BYTE + TypeSizes.BYTE;
            }
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);            

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            name = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            rank = Buffer[cursor];
            cursor++;

            gender = (Gender)Buffer[cursor];
            cursor++;

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

            Buffer[cursor] = rank;                                                                                  // Rank (1 byte)
            cursor++;

            Buffer[cursor] = (byte)gender;                                                                          // Gender (1 byte)
            cursor++;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            name = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            rank = Buffer[0];
            Buffer++;

            gender = (Gender)Buffer[0];
            Buffer++;
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

            Buffer[0] = rank;
            Buffer++;

            Buffer[0] = (byte)gender;
            Buffer++;
        }
        #endregion

        #region Fields
        public string name;
        public byte rank;
        public Gender gender;
        #endregion

        #region Properties
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

        public byte Rank
        {
            get { return rank; }
            set
            {
                if (rank != value)
                {
                    rank = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK));
                }
            }
        }

        public Gender Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GENDER));
                }
            }
        }
        #endregion

        #region Constructors
        public GuildMemberEntry() : base()
        {
        }

        public GuildMemberEntry(uint ID, uint Count = 0, string Name = "", byte Rank=1, Gender Gender = Gender.Male)        
            : base(ID, Count)
        {
            this.name = Name;
            this.rank = Rank;
            this.gender = Gender;        
        }

        public GuildMemberEntry(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe GuildMemberEntry(ref byte* Buffer)
            : base(ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Name = String.Empty;
                Rank = 0;
                Gender = Gender.Male;
            }
            else
            {
                name = String.Empty;
                rank = 0;
                gender = Gender.Male;
            }
        }
        #endregion
    }
}
