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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A Meridian59 ID notation. It's the current listindex on the server, changing with server GC.
    /// It's 28-Bit only, 4 high Bits are flag whether it's a single or multi/stackable object.
    /// </summary>
    [Serializable]
    public class ObjectID : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<ObjectUpdate>, IUpdatable<ObjectBase>
    {
        #region Constants
        public const uint UINT28MAX = 0x0FFFFFFFU;      // uint28 maximum = max id
        private const uint FLAGMASK = 0xF0000000U;
        private const uint MULTIOBJ = 0x10000000U;       
        private const byte SINGLELEN = 4;
        private const byte MULTILEN = 8;

        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_ID = "ID";
        public const string PROPNAME_COUNT = "Count";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion
        
        #region Fields
        protected uint id;
        protected uint count;
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get { return (count > 0) ? MULTILEN : SINGLELEN; }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            // possibly add mulitcount flag
            uint encodedID = id;           
            if (count > 0)
                encodedID = SetMultiCountFlag(encodedID);

            Array.Copy(BitConverter.GetBytes(encodedID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            if (Count > 0)
            {
                Array.Copy(BitConverter.GetBytes(Count), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;
            }

            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            uint encid = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            id = ExtractID(encid);                          // read ID but don't care about 4 high bits            
            uint flags = ExtractIDFlag(encid);              // get the 4 high bits at their origin position

            if (flags == MULTIOBJ)
            {
                count = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;
            }
            else
            {                
                count = 0;
            }

            return cursor - StartIndex;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            // possibly add mulitcount flag
            uint encodedID = id;
            if (count > 0)
                encodedID = SetMultiCountFlag(encodedID);

            *((uint*)Buffer) = encodedID;
            Buffer += TypeSizes.INT;

            if (Count > 0)
            {               
                *((uint*)Buffer) = Count;
                Buffer += TypeSizes.INT;
            }           
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            uint encid = *((uint*)Buffer);
            Buffer += TypeSizes.INT;
            
            id = ExtractID(encid);                          // read ID but don't care about 4 high bits            
            uint flags = ExtractIDFlag(encid);              // get the 4 high bits at their origin position
            
            if (flags == MULTIOBJ)
            {              
                count = *((uint*)Buffer);
                Buffer += TypeSizes.INT;                 
            }
            else
            {
                count = 0;
            }           
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
       
        #region Properties
  
        /// <summary>
        /// The unique ID of the object on the server.
        /// </summary>
        public uint ID 
        {
            get { return id; }
            set
            {
                // too big value, use maximum
                if (value > UINT28MAX)
                    value = UINT28MAX;
               
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ID));
                }              
            }
        }
        
        /// <summary>
        /// The amount of the item.
        /// 0 if not stackable.
        /// </summary>
        public uint Count 
        {
            get { return count; }
            set
            {
                if (count != value)
                {
                    count = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COUNT));
                }
            }
        }

        /// <summary>
        /// True if count is greater than zero (stackable).
        /// </summary>
        public bool IsStackable
        {
            get { return Count > 0; }
        }
        #endregion

        #region Constructors
        public ObjectID() 
        {
            Clear(false);
        }

        public ObjectID(uint ID, uint Count = 0)
        {
            this.id = ID;
            this.count = Count;                      
        }

        public ObjectID(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe ObjectID(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion
       
        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ID = 0;
                Count = 0;
            }
            else
            {
                id = 0;
                count = 0;
            } 
        }
        #endregion

        #region IUpdatable
        public virtual void UpdateFromModel(ObjectUpdate Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ID = Model.ID;
                Count = Model.count;
            }
            else
            {
                id = Model.ID;
                count = Model.Count;
            }
        }

        public virtual void UpdateFromModel(ObjectBase Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ID = Model.ID;
                Count = Model.count;
            }
            else
            {
                id = Model.ID;
                count = Model.Count;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return ID.ToString();
        }

        protected uint ExtractID(uint EncodedID)
        {
            return EncodedID & UINT28MAX;
        }

        protected uint ExtractIDFlag(uint EncodedID)
        {
            return EncodedID & FLAGMASK;
        }

        protected uint SetMultiCountFlag(uint ID)
        {
            return ID | MULTIOBJ;
        }
        #endregion

        #region Static
        public static bool IsValid(uint ID)
        {
            return (ID <= UINT28MAX);
        }
        #endregion
    }
}
