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
using Meridian59.Files.ROO;
using Meridian59.Files;
using Meridian59.Common;
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Roominformation (MapID, Brigthness, ...)
    /// </summary>
    [Serializable]
    public class RoomInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<RoomInfo>, IStringResolvable, IResourceResolvable
    {
        #region Constants
        
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */
        
        public const string PROPNAME_AVATARID = "AvatarID";
        public const string PROPNAME_AVATAROVERLAYRID = "AvatarOverlayRID";
        public const string PROPNAME_AVATARNAMERID = "AvatarNameRID";
        public const string PROPNAME_ROOMID = "RoomID";
        public const string PROPNAME_ROOMFILERID = "RoomFileRID";
        public const string PROPNAME_ROOMNAMERID = "RoomNameRID";
        public const string PROPNAME_ROOMSECURITY = "RoomSecurity";
        public const string PROPNAME_AMBIENTLIGHT = "AmbientLight";
        public const string PROPNAME_AVATARLIGHT = "AvatarLight";
        public const string PROPNAME_BACKGROUNDFILERID = "BackgroundFileRID";
        public const string PROPNAME_WADINGSOUNDFILERID = "WadingSoundFileRID";
        public const string PROPNAME_FLAGS = "Flags";
        public const string PROPNAME_DEPTH1 = "Depth1";
        public const string PROPNAME_DEPTH2 = "Depth2";
        public const string PROPNAME_DEPTH3 = "Depth3";
        public const string PROPNAME_AVATAROVERLAY = "AvatarOverlay";
        public const string PROPNAME_AVATARNAME = "AvatarName";
        public const string PROPNAME_ROOMFILE = "RoomFile";
        public const string PROPNAME_ROOMNAME = "RoomName";
        public const string PROPNAME_BACKGROUNDFILE = "BackgroundFile";
        public const string PROPNAME_WADINGSOUNDFILE = "WadingSoundFile";
        public const string PROPNAME_RESOURCEROOM = "ResourceRoom";
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
                // Sum up length in bytes             
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + 
                    TypeSizes.INT + TypeSizes.BYTE + TypeSizes.BYTE + 
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(avatarID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(avatarOverlayRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(avatarNameRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(roomID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(roomFileRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(roomNameRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(roomSecurity), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = ambientLight;
            cursor++;

            Buffer[cursor] = avatarLight;
            cursor++;

            Array.Copy(BitConverter.GetBytes(backgroundFileRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(wadingSoundFileRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(flags.Value), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(depth1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(depth2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(depth3), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        
        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            avatarID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            avatarOverlayRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            avatarNameRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            roomID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            roomFileRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            roomNameRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            roomSecurity = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ambientLight = Buffer[cursor];
            cursor++;

            avatarLight = Buffer[cursor];
            cursor++;

            backgroundFileRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            wadingSoundFileRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            flags = new RoomInfoFlags(BitConverter.ToUInt32(Buffer, cursor));
            cursor += TypeSizes.INT;

            depth1 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            depth2 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            depth3 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = avatarID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = avatarOverlayRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = avatarNameRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = roomID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = roomFileRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = roomNameRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = roomSecurity;
            Buffer += TypeSizes.INT;

            Buffer[0] = ambientLight;
            Buffer++;

            Buffer[0] = avatarLight;
            Buffer++;

            *((uint*)Buffer) = backgroundFileRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = wadingSoundFileRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = flags.Value;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = depth1;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = depth2;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = depth3;
            Buffer += TypeSizes.INT;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            avatarID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            avatarOverlayRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            avatarNameRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            roomID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            roomFileRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            roomNameRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            roomSecurity = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            ambientLight = Buffer[0];
            Buffer++;

            avatarLight = Buffer[0];
            Buffer++;

            backgroundFileRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            wadingSoundFileRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            flags = new RoomInfoFlags(*((uint*)Buffer));
            Buffer += TypeSizes.INT;

            depth1 = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            depth2 = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            depth3 = *((uint*)Buffer);
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
        protected uint avatarID;
        protected uint avatarOverlayRID;
        protected uint avatarNameRID;
        protected uint roomID;
        protected uint roomFileRID;
        protected uint roomNameRID;
        protected uint roomSecurity;
        protected byte ambientLight;
        protected byte avatarLight;
        protected uint backgroundFileRID;
        protected uint wadingSoundFileRID;
        protected RoomInfoFlags flags;
        protected uint depth1;
        protected uint depth2;
        protected uint depth3;

        protected string avatarOverlay;
        protected string avatarName;
        protected string roomFile;
        protected string roomName;
        protected string backgroundFile;
        protected string wadingSoundFile;
        protected RooFile resourceRoom;
        #endregion

        #region Properties
        public uint AvatarID
        {
            get { return avatarID; }
            set
            {
                if (avatarID != value)
                {
                    avatarID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATARID));
                }
            }
        }
        public uint AvatarOverlayRID
        {
            get { return avatarOverlayRID; }
            set
            {
                if (avatarOverlayRID != value)
                {
                    avatarOverlayRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATAROVERLAYRID));
                }
            }
        }
        public uint AvatarNameRID
        {
            get { return avatarNameRID; }
            set
            {
                if (avatarNameRID != value)
                {
                    avatarNameRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATARNAMERID));
                }
            }
        }
        public uint RoomID
        {
            get { return roomID; }
            set
            {
                if (roomID != value)
                {
                    roomID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROOMID));
                }
            }
        }
        public uint RoomFileRID
        {
            get { return roomFileRID; }
            set
            {
                if (roomFileRID != value)
                {
                    roomFileRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROOMFILERID));
                }
            }
        }
        public uint RoomNameRID
        {
            get { return roomNameRID; }
            set
            {
                if (roomNameRID != value)
                {
                    roomNameRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROOMNAMERID));
                }
            }
        }
        public uint RoomSecurity
        {
            get { return roomSecurity; }
            set
            {
                if (roomSecurity != value)
                {
                    roomSecurity = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROOMSECURITY));
                }
            }
        }
        public byte AmbientLight
        {
            get { return ambientLight; }
            set
            {
                if (ambientLight != value)
                {
                    ambientLight = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AMBIENTLIGHT));
                }
            }
        }
        public byte AvatarLight
        {
            get { return avatarLight; }
            set
            {
                if (avatarLight != value)
                {
                    avatarLight = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATARLIGHT));
                }
            }
        }
        public uint BackgroundFileRID
        {
            get { return backgroundFileRID; }
            set
            {
                if (backgroundFileRID != value)
                {
                    backgroundFileRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_BACKGROUNDFILERID));
                }
            }
        }
        public uint WadingSoundFileRID
        {
            get { return wadingSoundFileRID; }
            set
            {
                if (wadingSoundFileRID != value)
                {
                    wadingSoundFileRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_WADINGSOUNDFILERID));
                }
            }
        }
        public RoomInfoFlags Flags
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
        public uint Depth1
        {
            get { return depth1; }
            set
            {
                if (depth1 != value)
                {
                    depth1 = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DEPTH1));
                }
            }
        }
        public uint Depth2
        {
            get { return depth2; }
            set
            {
                if (depth2 != value)
                {
                    depth2 = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DEPTH2));
                }
            }
        }
        public uint Depth3
        {
            get { return depth3; }
            set
            {
                if (depth3 != value)
                {
                    depth3 = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DEPTH3));
                }
            }
        }

        // Extended properties
        public string AvatarOverlay
        {
            get { return avatarOverlay; }
            set
            {
                if (avatarOverlay != value)
                {
                    avatarOverlay = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATAROVERLAY));
                }
            }
        }
        public string AvatarName
        {
            get { return avatarName; }
            set
            {
                if (avatarName != value)
                {
                    avatarName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATARNAME));
                }
            }
        }
        public string RoomFile
        {
            get { return roomFile; }
            set
            {
                if (roomFile != value)
                {
                    roomFile = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROOMFILE));
                }
            }
        }
        public string RoomName
        {
            get { return roomName; }
            set
            {
                if (roomName != value)
                {
                    roomName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ROOMNAME));
                }
            }
        }
        public string BackgroundFile
        {
            get { return backgroundFile; }
            set
            {
                if (backgroundFile != value)
                {
                    backgroundFile = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_BACKGROUNDFILE));
                }
            }
        }
        public string WadingSoundFile
        {
            get { return wadingSoundFile; }
            set
            {
                if (wadingSoundFile != value)
                {
                    wadingSoundFile = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_WADINGSOUNDFILE));
                }
            }
        }
        public RooFile ResourceRoom
        {
            get { return resourceRoom; }
            set
            {
                if (resourceRoom != value)
                {
                    resourceRoom = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEROOM));
                }
            }
        }
        #endregion

        #region Constructors
        public RoomInfo()
        {
            Clear(false);
        }

        public RoomInfo(
            uint AvatarID, 
            uint AvatarOverlayRID, 
            uint AvatarNameRID, 
            uint RoomID, 
            uint RoomFileRID, 
            uint RoomNameRID, 
            uint RoomSecurity, 
            byte AmbientLight,
            byte AvatarLight,
            uint BackgroundFileRID,
            uint WadingSoundFileRID,
            uint Flags,
            uint Depth1,
            uint Depth2,
            uint Depth3)
        {
            this.avatarID = AvatarID;
            this.avatarOverlayRID = AvatarOverlayRID;
            this.avatarNameRID = AvatarNameRID;
            this.roomID = RoomID;
            this.roomFileRID = RoomFileRID;
            this.roomNameRID = RoomNameRID;
            this.roomSecurity = RoomSecurity;
            this.ambientLight = AmbientLight;
            this.avatarLight = AvatarLight;
            this.backgroundFileRID = BackgroundFileRID;
            this.wadingSoundFileRID = WadingSoundFileRID;
            this.flags = new RoomInfoFlags(Flags);
            this.depth1 = Depth1;
            this.depth2 = Depth2;
            this.depth3 = Depth3;
        }

        public RoomInfo(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe RoomInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                AvatarID = 0;
                AvatarOverlayRID = 0;
                AvatarNameRID = 0;
                RoomID = 0;
                RoomFileRID = 0;
                RoomNameRID = 0;
                RoomSecurity = 0;
                AmbientLight = 0;
                AvatarLight = 0;
                BackgroundFileRID = 0;
                WadingSoundFileRID = 0;
                Flags = new RoomInfoFlags(0);
                Depth1 = 0;
                Depth2 = 0;
                Depth3 = 0;

                AvatarOverlay = String.Empty;
                AvatarName = String.Empty;
                RoomFile = String.Empty;
                BackgroundFile = String.Empty;
                WadingSoundFile = String.Empty;
            }
            else
            {
                avatarID = 0;
                avatarOverlayRID = 0;
                avatarNameRID = 0;
                roomID = 0;
                roomFileRID = 0;
                roomNameRID = 0;
                roomSecurity = 0;
                ambientLight = 0;
                avatarLight = 0;
                backgroundFileRID = 0;
                wadingSoundFileRID = 0;
                flags = new RoomInfoFlags(0);
                depth1 = 0;
                depth2 = 0;
                depth3 = 0;

                avatarOverlay = String.Empty;
                avatarName = String.Empty;
                roomFile = String.Empty;
                backgroundFile = String.Empty;
                wadingSoundFile = String.Empty;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(RoomInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                AvatarID = Model.AvatarID;
                AvatarOverlayRID = Model.AvatarOverlayRID;
                AvatarNameRID = Model.AvatarNameRID;
                RoomID = Model.RoomID;
                RoomFileRID = Model.RoomFileRID;
                RoomNameRID = Model.RoomNameRID;
                RoomSecurity = Model.RoomSecurity;
                AmbientLight = Model.AmbientLight;
                AvatarLight = Model.AvatarLight;
                BackgroundFileRID = Model.BackgroundFileRID;
                WadingSoundFileRID = Model.WadingSoundFileRID;
                Flags = Model.Flags;
                Depth1 = Model.Depth1;
                Depth2 = Model.Depth2;
                Depth3 = Model.Depth3;

                AvatarOverlay = Model.AvatarOverlay;
                AvatarName = Model.AvatarName;
                RoomFile = Model.RoomFile;
                RoomName = Model.RoomName;
                BackgroundFile = Model.BackgroundFile;
                WadingSoundFile = Model.WadingSoundFile;

                ResourceRoom = Model.ResourceRoom;
            }
            else
            {
                avatarID = Model.AvatarID;
                avatarOverlayRID = Model.AvatarOverlayRID;
                avatarNameRID = Model.AvatarNameRID;
                roomID = Model.RoomID;
                roomFileRID = Model.RoomFileRID;
                roomNameRID = Model.RoomNameRID;
                roomSecurity = Model.RoomSecurity;
                ambientLight = Model.AmbientLight;
                avatarLight = Model.AvatarLight;
                backgroundFileRID = Model.BackgroundFileRID;
                wadingSoundFileRID = Model.WadingSoundFileRID;
                flags = Model.Flags;
                depth1 = Model.Depth1;
                depth2 = Model.Depth2;
                depth3 = Model.Depth3;

                avatarOverlay = Model.AvatarOverlay;
                avatarName = Model.AvatarName;
                roomFile = Model.RoomFile;
                roomName = Model.RoomName;
                backgroundFile = Model.BackgroundFile;
                wadingSoundFile = Model.WadingSoundFile;

                resourceRoom = Model.ResourceRoom;
            }          
        }
        #endregion

        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string avatar_name;
            string avatar_overlay;
            string room_file;
            string room_name;
            string background_file;
            string wading_file;

			StringResources.TryGetValue(avatarOverlayRID, out avatar_overlay);
			StringResources.TryGetValue(avatarNameRID, out avatar_name);
			StringResources.TryGetValue(roomFileRID, out room_file);
			StringResources.TryGetValue(roomNameRID, out room_name);
			StringResources.TryGetValue(backgroundFileRID, out background_file);
			StringResources.TryGetValue(wadingSoundFileRID, out wading_file);

            if (RaiseChangedEvent)
            {
                if (avatar_name != null) AvatarName = avatar_name;
                else AvatarName = String.Empty;

                if (avatar_overlay != null) AvatarOverlay = avatar_overlay;
                else AvatarOverlay = String.Empty;

                if (room_file != null) RoomFile = room_file;
                else RoomFile = String.Empty;

                if (room_name != null) RoomName = room_name;
                else RoomName = String.Empty;

                if (background_file != null) BackgroundFile = background_file;
                else BackgroundFile = String.Empty;

                if (wading_file != null) WadingSoundFile = wading_file;
                else WadingSoundFile = String.Empty;
            }
            else
            {
                if (avatar_name != null) avatarName = avatar_name;
                else avatarName = String.Empty;

                if (avatar_overlay != null) avatarOverlay = avatar_overlay;
                else avatarOverlay = String.Empty;

                if (room_file != null) roomFile = room_file;
                else roomFile = String.Empty;

                if (room_name != null) roomName = room_name;
                else roomName = String.Empty;

                if (background_file != null) backgroundFile = background_file;
                else backgroundFile = String.Empty;

                if (wading_file != null) wadingSoundFile = wading_file;
                else wadingSoundFile = String.Empty;
            }
        }
        #endregion

        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (RoomFile != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    ResourceRoom = M59ResourceManager.GetRoom(RoomFile);
                }
                else
                {
                    resourceRoom = M59ResourceManager.GetRoom(RoomFile);
                }

                if (resourceRoom != null)
                    resourceRoom.ResolveResources(M59ResourceManager);
            }           
        }
        #endregion
    }
}
