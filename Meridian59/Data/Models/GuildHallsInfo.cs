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
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class GuildHallsInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<GuildHallsInfo>
    {
        #region Constants
        public const string PROPNAME_GUILDS = "GuildsHalls";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public int ByteLength
        {
            get
            {
                // guild halls list
                int len = TypeSizes.SHORT;
                foreach (GuildHall entry in guildHalls)
                    len += entry.ByteLength;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(guildHalls.Count)), 0, Buffer, cursor, TypeSizes.SHORT);                 // GuildsListLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (GuildHall entry in guildHalls)
                cursor += entry.WriteTo(Buffer, cursor);

            return ByteLength;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);                 // GuildsLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            guildHalls.Clear();
            guildHalls.Capacity = len;
            for (ushort i = 0; i < len; i++)
            {
                GuildHall obj = new GuildHall(Buffer, cursor);                // GuildHall (n bytes)
                guildHalls.Add(obj);
                cursor += obj.ByteLength;
            }

            return ByteLength;
        }
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((ushort*)Buffer) = (ushort)guildHalls.Count;
            Buffer += TypeSizes.SHORT;

            foreach (GuildHall entry in guildHalls)
                entry.WriteTo(ref Buffer);
        }
        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            guildHalls.Clear();
            guildHalls.Capacity = len;
            for (ushort i = 0; i < len; i++)
                guildHalls.Add(new GuildHall(ref Buffer));
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
        protected readonly BaseList<GuildHall> guildHalls = new BaseList<GuildHall>();
        protected bool isVisible;
        #endregion

        #region Properties
        public BaseList<GuildHall> GuildHalls
        {
            get { return guildHalls; }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISVISIBLE));
                }
            }
        }
        #endregion

        #region Constructors
        public GuildHallsInfo()
        {
            Clear(false);
        }

        public GuildHallsInfo(IEnumerable<GuildHall> GuildHalls)
        {
            guildHalls.AddRange(GuildHalls);
        }

        public GuildHallsInfo(byte[] RawData, int startIndex = 0)
        {
            ReadFrom(RawData, startIndex);
        }

        public unsafe GuildHallsInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                GuildHalls.Clear();
            }
            else
            {
                guildHalls.Clear();
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(GuildHallsInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                GuildHalls.Clear();
                foreach (GuildHall obj in Model.GuildHalls)
                    GuildHalls.Add(obj);
                IsVisible = false;

            }
            else
            {
                guildHalls.Clear();
                foreach (GuildHall obj in Model.GuildHalls)
                    guildHalls.Add(obj);
                isVisible = false;
            }
        }
        #endregion
    }
}
