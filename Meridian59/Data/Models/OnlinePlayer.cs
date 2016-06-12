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
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Online player information (id, name, flags, ...)
    /// </summary>
    [Serializable]
    public class OnlinePlayer : ObjectID
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_NAMERID = "NameRID";
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_FLAGS = "Flags";
        #endregion

        #region IByteSerializable
        public override int ByteLength
        {
            get 
            {
                return base.ByteLength + TypeSizes.INT + TypeSizes.SHORT + name.Length + flags.ByteLength;
            }
        }
        
        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            cursor += base.WriteTo(Buffer, StartIndex);                                                             // ID (4/8 bytes)

            Array.Copy(BitConverter.GetBytes(nameRID), 0, Buffer, cursor, TypeSizes.INT);                          // StringID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(name.Length)), 0, Buffer, cursor, TypeSizes.SHORT);   // NameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(name), 0, Buffer, cursor, name.Length);                            // Name (n bytes)
            cursor += name.Length;

            cursor += flags.WriteTo(Buffer, cursor);                                                                // Flags (n bytes)

            return cursor - StartIndex;
        }
        
        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);                    // ID (4/8 bytes)
            
            nameRID = BitConverter.ToUInt32(Buffer, cursor);               // StringID (4 bytes)
            cursor += TypeSizes.INT;

            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);          // NameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            name = Util.Encoding.GetString(Buffer, cursor, strlen);      // Name (n bytes)
            cursor += strlen;

            flags.ReadFrom(Buffer, cursor);                                 // Flags (n bytes)
            cursor += flags.ByteLength;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = nameRID;
            Buffer += TypeSizes.INT;
         
            fixed (char* pName = name)
            {
                ushort len = (ushort)name.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                int a, b; bool c;
                Util.Encoding.GetEncoder().Convert(pName, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            flags.WriteTo(ref Buffer);            
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            nameRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            name = new string((sbyte*)Buffer, 0, len, Util.Encoding);
            Buffer += len;

            flags.ReadFrom(ref Buffer);
        }

        #endregion

        #region Fields
        protected uint nameRID;
        protected string name;
        protected readonly ObjectFlags flags = new ObjectFlags();

        #endregion

        #region Properties
        public uint NameRID
        {
            get { return nameRID; }
            set
            {
                if (nameRID != value)
                {
                    nameRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NAMERID));
                }
            }
        }
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
        public ObjectFlags Flags
        {
            get { return flags; }          
        }
        #endregion

        #region Constructors
        public OnlinePlayer() : base()
        {
        }

        public OnlinePlayer(uint ID, uint NameRID, string Name, uint Flags)
            : base (ID)
        {
            this.nameRID = NameRID;          
            this.name = Name;
            this.flags.Value = Flags;                     
        }

        public OnlinePlayer(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe OnlinePlayer(ref byte* Buffer)
            : base(ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                NameRID = 0;
                Name = String.Empty;              
            }
            else
            {
                nameRID = 0;
                name = String.Empty;
            }

            flags.Clear(RaiseChangedEvent);
        }

        #endregion
    }
}
