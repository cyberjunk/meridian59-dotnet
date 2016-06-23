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
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A Meridian59 GuildHall object
    /// </summary>
    [Serializable]
    public class GuildHall : ObjectID, IClearable, IStringResolvable
    {
        public const string PROPNAME_NAMERID    = "NameRID";
        public const string PROPNAME_COST       = "Cost";
        public const string PROPNAME_RENT       = "Rent";
        public const string PROPNAME_NAME       = "Name";

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return base.ByteLength + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT; 
            } 
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            nameRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            cost = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            rent = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex; 
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(nameRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(cost), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(rent), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            nameRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            cost = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            rent = *((uint*)Buffer);
            Buffer += TypeSizes.INT;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = nameRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = cost;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = rent;
            Buffer += TypeSizes.INT;
        }
        #endregion

        protected uint nameRID;
        protected uint cost;
        protected uint rent;
        protected string name;

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public uint Cost
        {
            get { return cost; }
            set
            {
                if (cost != value)
                {
                    cost = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COST));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Rent
        {
            get { return rent; }
            set
            {
                if (rent != value)
                {
                    rent = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RENT));
                }
            }
        }

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
        public GuildHall() : base() { }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ID"></param>
        /// <param name="Count"></param>
        public GuildHall(string Name, uint ID, uint Count = 0)
            : base(ID, Count)
        {
            name = Name;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public GuildHall(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        /// <summary>
        /// Constructor by pointer parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe GuildHall(ref byte* Buffer)
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
                NameRID = 0;
                Cost = 0;
                Rent = 0;
                Name = String.Empty;                
            }
            else
            {
                nameRID = 0;
                cost = 0;
                rent = 0;
                name = String.Empty;
            }
        }

        public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_name;

            StringResources.TryGetValue(nameRID, out res_name);

            if (RaiseChangedEvent)
            {
                if (res_name != null) Name = res_name;
                else Name = String.Empty;
            }
            else
            {
                if (res_name != null) name = res_name;
                else name = String.Empty;
            }
        }
    }
}
