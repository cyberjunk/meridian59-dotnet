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
using System.Collections.Concurrent;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Files;
using Meridian59.Files.BGF;
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Info about a texture change on a room element (wall/floors/...)
    /// </summary>
    [Serializable]
    public class TextureChangeInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable, IResourceResolvable
    {
        #region Constants
        public const string PROPNAME_SERVERID = "ServerID";
        public const string PROPNAME_TEXTURENUM = "TextureNum";
        public const string PROPNAME_FLAGS = "Flags";
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
                // 2 + 2 + 1
                return TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(serverID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(textureNum), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = (byte)flags.Flags;
            cursor++;

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            serverID = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            textureNum = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            flags = new ChangeFlags(Buffer[cursor]);
            cursor++;

            return cursor - StartIndex;
        }

        public unsafe virtual void WriteTo(ref byte* Buffer)
        {
            *((ushort*)Buffer) = serverID;
            Buffer += TypeSizes.SHORT; 

            *((ushort*)Buffer) = textureNum;          
            Buffer += TypeSizes.SHORT;

            Buffer[0] = (byte)flags.Flags;
            Buffer++;
        }

        public unsafe virtual void ReadFrom(ref byte* Buffer)
        {
            serverID = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;  

            textureNum = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            flags = new ChangeFlags(Buffer[0]);
            Buffer++;         
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
        protected ushort serverID;
        protected ushort textureNum;       
        protected ChangeFlags flags;        
        protected BgfFile resource;
        #endregion

        #region Properties

        /// <summary>
        /// ServerID of the roomelement to change.
        /// </summary>
        public ushort ServerID
        {
            get { return serverID; }
            set
            {
                if (serverID != value)
                {
                    serverID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SERVERID));
                }
            }
        }
        
        /// <summary>
        /// New texture num.
        /// </summary>
        public ushort TextureNum
        {
            get { return textureNum; }
            set
            {
                if (textureNum != value)
                {
                    textureNum = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TEXTURENUM));
                }
            }
        }
        
        /// <summary>
        /// Flags for the changes.
        /// Defines what kind of element.
        /// </summary>
        public ChangeFlags Flags
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {                   
                    flags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// Resource of the new texture.
        /// Set by ResolveResources().
        /// </summary>
        public BgfFile Resource
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
        public TextureChangeInfo()
        {
            Clear(false);
        }
        
        public TextureChangeInfo(ushort ServerID, ushort TextureNum, byte Flags)
        {
            serverID = ServerID;
            textureNum = TextureNum;
            flags = new ChangeFlags(Flags);
        }

        public TextureChangeInfo(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe TextureChangeInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ServerID = 0;
                TextureNum = 0;              
                Flags.Flags = 0;
            }
            else
            {
                serverID = 0;
                textureNum = 0;
                flags.Flags = 0;
            }
        }
        #endregion
       
        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {          
            if (RaiseChangedEvent)
            {
                Resource = M59ResourceManager.GetRoomTexture(textureNum);
            }
            else
            {
                resource = M59ResourceManager.GetRoomTexture(textureNum);
            }            
        }
        #endregion

        /// <summary>
        /// Wrapper around 8 !! bit sidedef flags
        /// </summary>
        [Serializable]
        public class ChangeFlags
        {
            #region Bitmasks
            private const uint CTF_ABOVEWALL    = 0x01;     // Change above wall texture
            private const uint CTF_NORMALWALL   = 0x02;     // Change normal wall texture
            private const uint CTF_BELOWWALL    = 0x04;     // Change below wall texture
            private const uint CTF_FLOOR        = 0x08;     // Change floor texture
            private const uint CTF_CEILING      = 0x10;     // Change ceiling texture
            #endregion

            public uint Flags { get; set; }

            public ChangeFlags(byte Flags = 0)
            {
                this.Flags = Flags;
            }

            public override string ToString()
            {
                return Flags.ToString();
            }

            #region Property Accessors
            /* 
             * Easy to use property accessors,
             * Check set: AND
             * Set: OR
             * Unset: AND NEG
             */

            public bool IsAboveWall
            {
                get { return (Flags & CTF_ABOVEWALL) == CTF_ABOVEWALL; }
                set
                {
                    if (value) Flags |= CTF_ABOVEWALL;
                    else Flags &= ~CTF_ABOVEWALL;
                }
            }
            public bool IsNormalWall
            {
                get { return (Flags & CTF_NORMALWALL) == CTF_NORMALWALL; }
                set
                {
                    if (value) Flags |= CTF_NORMALWALL;
                    else Flags &= ~CTF_NORMALWALL;
                }
            }
            public bool IsBelowWall
            {
                get { return (Flags & CTF_BELOWWALL) == CTF_BELOWWALL; }
                set
                {
                    if (value) Flags |= CTF_BELOWWALL;
                    else Flags &= ~CTF_BELOWWALL;
                }
            }
            public bool IsFloor
            {
                get { return (Flags & CTF_FLOOR) == CTF_FLOOR; }
                set
                {
                    if (value) Flags |= CTF_FLOOR;
                    else Flags &= ~CTF_FLOOR;
                }
            }
            public bool IsCeiling
            {
                get { return (Flags & CTF_CEILING) == CTF_CEILING; }
                set
                {
                    if (value) Flags |= CTF_CEILING;
                    else Flags &= ~CTF_CEILING;
                }
            }
            #endregion
        }
    }
}
