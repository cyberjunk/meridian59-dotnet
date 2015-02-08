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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Data of GuildAsk UC
    /// </summary>
    [Serializable]
    public class GuildAskData : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<GuildAskData>
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */
        public const string PROPNAME_COSTNORMAL = "CostNormal";
        public const string PROPNAME_COSTSECRET = "CostSecret";
        public const string PROPNAME_ISVISIBLE  = "IsVisible";
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
                return TypeSizes.INT + TypeSizes.INT;
            }
        }      
 
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(costNormal), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(costSecret), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            costNormal = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            costSecret = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }       
        
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = costNormal;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = costSecret;
            Buffer += TypeSizes.INT; 
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            costNormal = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            costSecret = *((uint*)Buffer);
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
        protected uint costNormal;        
        protected uint costSecret;
        protected bool isVisible;
        #endregion

        #region Properties
        public uint CostNormal
        {
            get { return costNormal; }
            set
            {
                if (costNormal != value)
                {
                    costNormal = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COSTNORMAL));
                }
            }
        }        
        public uint CostSecret
        {
            get { return costSecret; }
            set
            {
                if (costSecret != value)
                {
                    costSecret = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COSTSECRET));
                }
            }
        }
        public bool IsVisible
        {
            get { return isVisible; }
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
        public GuildAskData()
        {
            Clear(false);
        }
        
        public GuildAskData(uint CostNormal, uint CostSecret)
        {
            costNormal = CostNormal;
            costSecret = CostSecret;
        }

        public GuildAskData(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe GuildAskData(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {           
            if (RaiseChangedEvent)
            {
                CostNormal = 0;
                CostSecret = 0;
                IsVisible = false;
            }
            else
            {
                costNormal = 0;
                costSecret = 0;
                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(GuildAskData Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                CostNormal = Model.CostNormal;
                CostSecret = Model.CostSecret;
                
                // dont touch visible
            }
            else
            {
                costNormal = Model.CostNormal;
                costSecret = Model.CostSecret;

                // dont touch visible
            }
        }
        #endregion
    }
}
