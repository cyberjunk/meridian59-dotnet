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
using Meridian59.Files.BGF;
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A M59 stat (like spellist entry, avatar status, ...)
    /// </summary>
    [Serializable]
    public abstract class Stat : IByteSerializableFast, INotifyPropertyChanged, IClearable, IStringResolvable, IResourceResolvable, IUpdatable<Stat>
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_NUM = "Num";
        public const string PROPNAME_RESOURCEID = "ResourceID";
        public const string PROPNAME_RESOURCENAME = "ResourceName";
        public const string PROPNAME_RESOURCE = "Resource";

        /// <summary>
        /// The offset to preread the type when parsing
        /// </summary>
        public const int TypeOffset = 5;

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public virtual int ByteLength { 
            get { 
                // 1 + 4
                return TypeSizes.BYTE + TypeSizes.INT;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = num;
            cursor++;

            Array.Copy(BitConverter.GetBytes(resourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
          
            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            num = Buffer[cursor];
            cursor++;

            resourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public unsafe virtual void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = num;
            Buffer++;

            *((uint*)Buffer) = resourceID;          
            Buffer += TypeSizes.INT;           
        }

        public unsafe virtual void ReadFrom(ref byte* Buffer)
        {
            num = Buffer[0];
            Buffer++;
            
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
        protected byte num;
        protected uint resourceID;        
        protected string resourceName;
        protected BgfFile resource;
        #endregion

        #region Properties

        /// <summary>
        /// The type of the stat
        /// </summary>
        public abstract StatType Type { get; }       

        /// <summary>
        /// Num of the stat.
        /// </summary>
        public byte Num
        {
            get { return num; }
            set
            {
                if (num != value)
                {
                    num = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUM));
                }
            }
        }

        /// <summary>
        /// The unique ID of the resource.
        /// </summary>
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
        
        /// <summary>
        /// The name.
        /// Set in ResolveStrings()
        /// </summary>
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

        /// <summary>
        /// The resource.
        /// Set in ResolveResources()
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
        public Stat()
        {
            Clear(false);
        }
        
        public Stat(byte Num)
        {
            this.num = Num;
        }

        public Stat(byte Num, uint ResourceID)
        {
            this.num = Num;
            this.resourceID = ResourceID;      
        }

        public Stat(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe Stat(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Num = 0;
                ResourceID = 0;               
                ResourceName = String.Empty;
            }
            else
            {
                num = 0;
                resourceID = 0;               
                resourceName = String.Empty;
            }
        }
        #endregion

        #region IStringResolvable
		public virtual void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
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
        public abstract void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent);
        #endregion

        #region IUpdatable
        public void UpdateFromModel(Stat Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Num = Model.Num;
                ResourceID = Model.ResourceID;                
                ResourceName = Model.ResourceName;                
            }
            else
            {
                num = Model.Num;
                resourceID = Model.ResourceID;                
                resourceName = Model.ResourceName;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Extract a stat object from byte array
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        public static Stat ExtractStat(byte[] Buffer, int StartIndex = 0)
        {
            Stat returnValue = null;

            // try to parse the stat
            switch ((StatType)Buffer[StartIndex + TypeOffset])
            {
                case StatType.Numeric:
                    returnValue = new StatNumeric(Buffer, StartIndex);
                    break;

                case StatType.List:
                    returnValue = new StatList(Buffer, StartIndex);
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Extract a stat object from byte pointer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public static unsafe Stat ExtractStat(ref byte* Buffer)
        {
            Stat returnValue = null;

            // try to parse the stat
            switch ((StatType)Buffer[TypeOffset])
            {
                case StatType.Numeric:
                    returnValue = new StatNumeric(ref Buffer);
                    break;

                case StatType.List:
                    returnValue = new StatList(ref Buffer);
                    break;
            }

            return returnValue;
        }
        #endregion
    }
}
