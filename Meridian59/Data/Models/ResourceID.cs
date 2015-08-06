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
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A Meridian59 ResourceID notation.
    /// </summary>
    [Serializable]
    public class ResourceID : IByteSerializableFast, INotifyPropertyChanged, IStringResolvable, IClearable, IUpdatable<ResourceID>
    {
        #region Constants
        public const string PROPNAME_VALUE = "Value";
        public const string PROPNAME_NAME = "Name";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region Fields
        protected uint value;
        protected string name;
        #endregion

        #region Properties
        public uint Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VALUE));
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
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get
            {
                return TypeSizes.INT;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(value), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
            
            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            value = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = value;
            Buffer += TypeSizes.INT;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            value = *((uint*)Buffer);
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

        #region Constructors
        public ResourceID()
        {
            Clear(false);
        }
        
        public ResourceID(uint Value)
        {
            this.value = Value;
        }

        public ResourceID(byte[] Buffer, int StartIndex = 0)
        {
            this.ReadFrom(Buffer, StartIndex);
        }

        public unsafe ResourceID(ref byte* Buffer)
        {
            this.ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion

        #region IStringResolvable
		public virtual void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_name;

			StringResources.TryGetValue(value, out res_name);
            
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
        #endregion     

        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Value = 0;
                
                Name = String.Empty;
            }
            else
            {
                value = 0;

                name = String.Empty;
            }
        }
        #endregion

        #region IUpdatable
        public virtual void UpdateFromModel(ResourceID Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Value = Model.Value;
                
                Name = Model.Name;               
            }
            else
            {
                value = Model.Value;
                
                name = Model.Name;
            }
        }
        #endregion
    }
}
