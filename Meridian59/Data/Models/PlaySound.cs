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
using Meridian59.Files;
using Meridian59.Common;
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Info for Sound playback
    /// </summary>
    [Serializable]
    public class PlaySound : IByteSerializableFast, INotifyPropertyChanged, IStringResolvable, IResourceResolvable, IClearable
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */
        public const string PROPNAME_RESOURCEID = "ResourceID";
        public const string PROPNAME_ID = "ID";
        public const string PROPNAME_FLAGS = "PlayFlags";
        public const string PROPNAME_ROW = "Row";
        public const string PROPNAME_COLUMN = "Column";
        public const string PROPNAME_RADIUS = "Radius";
        public const string PROPNAME_MAXVOLUME = "MaxVolume";
        public const string PROPNAME_RESOURCENAME = "ResourceName";
        public const string PROPNAME_RESOURCE = "Resource";
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
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.BYTE + 
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;
            }
        }      
 
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(resourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(id), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = playflags.PlayFlags;
            cursor++;

            Array.Copy(BitConverter.GetBytes(row), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(column), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(radius), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(maxVolume), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            resourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
           
            id = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
           
            playflags = new PlaySound.Flags(Buffer[cursor]);
            cursor++;

            row = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            column = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            radius = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            maxVolume = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }       
        
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = resourceID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = id;
            Buffer += TypeSizes.INT;
  
            Buffer[0] = playflags.PlayFlags;
            Buffer++;

            *((int*)Buffer) = row;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = column;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = radius;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = maxVolume;
            Buffer += TypeSizes.INT;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            resourceID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            id = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            playflags = new PlaySound.Flags(Buffer[0]);
            Buffer++;

            row = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            column = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            radius = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            maxVolume = *((int*)Buffer);
            Buffer += TypeSizes.INT;
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
        protected uint resourceID;
        protected uint id;
        protected PlaySound.Flags playflags;
        protected int row;
        protected int column;
        protected int radius;
        protected int maxVolume;

        protected string resourceName;
        protected Tuple<IntPtr, uint> resource;
        #endregion

        #region Properties
        public uint ResourceID
        {
            get { return resourceID; }
            set
            {
                if (resourceID != value)
                {
                    resourceID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEID));
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
        public PlaySound.Flags PlayFlags
        {
            get { return playflags; }
            set
            {
                if (playflags != value)
                {
                    playflags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }
        public int Row
        {
            get { return row; }
            set
            {
                if (row != value)
                {
                    row = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROW));
                }
            }
        }
        public int Column
        {
            get { return column; }
            set
            {
                if (column != value)
                {
                    column = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COLUMN));
                }
            }
        }
        public int Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RADIUS));
                }
            }
        }
        public int MaxVolume
        {
            get { return maxVolume; }
            set
            {
                if (maxVolume != value)
                {
                    maxVolume = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MAXVOLUME));
                }
            }
        }
        public string ResourceName
        {
            get { return resourceName; }
            set
            {
                if (resourceName != value)
                {
                    resourceName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCENAME));
                }
            }
        }
        public Tuple<IntPtr, uint> Resource
        {
            get { return resource; }
            set
            {
                if (resource != value)
                {
                    resource = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCE));
                }
            }
        }
        #endregion

        #region Constructors
        public PlaySound()
        {
            Clear(false);
        }
        
        public PlaySound(uint ResourceID, uint ID, PlaySound.Flags Flags, int Row, int Column, int Radius, int MaxVolume)
        {
            resourceID = ResourceID;
            id = ID;
            playflags = Flags;
            row = Row;
            column = Column;
            radius = Radius;
            maxVolume = MaxVolume;
        }

        public PlaySound(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe PlaySound(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {           
            if (RaiseChangedEvent)
            {
                ResourceID = 0;
                ID = 0;
                PlayFlags = new Flags(0);
                Row = 0;
                Column = 0;
                Radius = 0;
                MaxVolume = 0;

                ResourceName = String.Empty;
                Resource = null;
            }
            else
            {
                resourceID = 0;
                id = 0;
                playflags = new Flags(0);
                row = 0;
                column = 0;
                radius = 0;
                maxVolume = 0;

                resourceName = String.Empty;
                resource = null;
            }
        }
        #endregion

        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_name;

			StringResources.TryGetValue(resourceID, out res_name);

            if (RaiseChangedEvent)
            {
                if (res_name != null) ResourceName = res_name;
                else ResourceName = String.Empty;
            }
            else
            {
                if (res_name != null) resourceName = res_name;
                else resourceName = String.Empty;
            }
        }
        #endregion

        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (ResourceName != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    Resource = M59ResourceManager.GetWavFile(ResourceName);
                }
                else
                {
                    resource = M59ResourceManager.GetWavFile(ResourceName);
                }
            }
        }
        #endregion

        /// <summary>
        /// Nested class. Flags for Playback.
        /// </summary>
        public class Flags
        {
            public byte PlayFlags
            {
                get
                {
                    return (byte)intvalue;
                }

                set
                {
                    intvalue = value;
                }
            }


            private uint intvalue;

            #region Bitmasks
            protected const uint SF_LOOP = 0x01;            // Loop sound until player leaves room
            protected const uint SF_RANDOM_PITCH = 0x02;    // Choose a random pitch for sound
            protected const uint SF_RANDOM_PLACE = 0x04;    // Kod already chose a random position
            #endregion

            public Flags(byte Flags)
            {
                this.PlayFlags = Flags;
            }

            #region Property Accessors
            /* 
             * Easy to use property accessors,
             * Check set: AND
             * Set: OR
             * Unset: AND NEG
             */
        
            public bool IsLoop
            {
                get { return (intvalue & SF_LOOP) == SF_LOOP; }
                set 
                {
                    if (value) intvalue |= SF_LOOP;
                    else intvalue &= ~SF_LOOP;
                }
            }

            public bool IsRandomPitch
            {
                get { return (intvalue & SF_RANDOM_PITCH) == SF_RANDOM_PITCH; }
                set
                {
                    if (value) intvalue |= SF_RANDOM_PITCH;
                    else intvalue &= ~SF_RANDOM_PITCH;
                }
            }

            public bool IsRandomPlace
            {
                get { return (intvalue & SF_RANDOM_PLACE) == SF_RANDOM_PLACE; }
                set
                {
                    if (value) intvalue |= SF_RANDOM_PLACE;
                    else intvalue &= ~SF_RANDOM_PLACE;
                }
            }
            #endregion
        }
    }
}
