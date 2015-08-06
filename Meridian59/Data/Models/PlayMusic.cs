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
    /// Info for Music playback
    /// </summary>
    [Serializable]
    public class PlayMusic : IByteSerializableFast, INotifyPropertyChanged, IStringResolvable, IResourceResolvable, IClearable, IUpdatable<PlayMusic>
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */
        public const string PROPNAME_RESOURCEID = "ResourceID";
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
                return TypeSizes.INT;
            }
        }      
 
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(resourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            resourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
                     
            return cursor - StartIndex;
        }       
        
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = resourceID;
            Buffer += TypeSizes.INT;          
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            resourceID = *((uint*)Buffer);
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
        public PlayMusic()
        {
            Clear(false);
        }
        
        public PlayMusic(uint ResourceID)
        {
            resourceID = ResourceID;            
        }

        public PlayMusic(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe PlayMusic(ref byte* Buffer)
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
                ResourceName = String.Empty;
                Resource = null;
            }
            else
            {
                resourceID = 0;
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
                if (res_name != null) ResourceName = res_name.Replace(FileExtensions.MID, FileExtensions.MP3);               
                else ResourceName = String.Empty;
            }
            else
            {
                if (res_name != null) resourceName = res_name.Replace(FileExtensions.MID, FileExtensions.MP3);
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
                    Resource = M59ResourceManager.GetMusicFile(ResourceName);
                }
                else
                {
                    resource = M59ResourceManager.GetMusicFile(ResourceName);
                }
            }
        }
        #endregion      
 
        #region IUpdatable
        public void UpdateFromModel(PlayMusic Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ResourceID = Model.ResourceID;
                ResourceName = Model.ResourceName;
                Resource = Model.Resource;
            }
            else
            {
                resourceID = Model.ResourceID;
                resourceName = Model.ResourceName;
                resource = Model.Resource;
            }
        }
        #endregion
    }
}
