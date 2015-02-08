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
    /// Sphere position in M59 style
    /// </summary>
    [Serializable]
    public class LightShading : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<LightShading>
    {
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
                return TypeSizes.BYTE + spherePosition.ByteLength; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            lightIntensity = Buffer[cursor];
            cursor++;

            spherePosition = new SpherePosition(Buffer, cursor);
            cursor += spherePosition.ByteLength;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = lightIntensity;
            cursor++;

            cursor += spherePosition.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            lightIntensity = Buffer[0];
            Buffer++;

            spherePosition = new SpherePosition(ref Buffer);            
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {           
            Buffer[0] = lightIntensity;
            Buffer++;

            spherePosition.WriteTo(ref Buffer);          
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
        protected byte lightIntensity;
        protected SpherePosition spherePosition;
        #endregion

        #region Properties
        public byte LightIntensity
        {
            get
            {
                return lightIntensity;
            }
            set
            {
                if (lightIntensity != value)
                {
                    lightIntensity = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("LightIntensity"));
                }
            }
        }

        public SpherePosition SpherePosition
        {
            get
            {
                return spherePosition;
            }
            set
            {
                if (spherePosition != value)
                {
                    spherePosition = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("SpherePosition"));
                }
            }
        }
        #endregion

        #region Constructors
        public LightShading()
        {
            Clear(false);
        }

        public LightShading(byte LightIntensity, SpherePosition Position)
        {
            this.lightIntensity = LightIntensity;
            this.spherePosition = Position;
        }

        public LightShading(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe LightShading(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                LightIntensity = 0;
                SpherePosition = new SpherePosition(0, 0);
            }
            else
            {
                lightIntensity = 0;
                spherePosition = new SpherePosition(0, 0);
            }
        }
        #endregion

        #region IUpdatable
        public virtual void UpdateFromModel(LightShading Model, bool RaiseChangedEvent)
        {           
            if (RaiseChangedEvent)
            {
                LightIntensity = Model.LightIntensity;
                SpherePosition.UpdateFromModel(Model.SpherePosition, true);                
            }
            else
            {
                lightIntensity = Model.LightIntensity;
                spherePosition.UpdateFromModel(Model.SpherePosition, false);
            }
        }
        #endregion
    }
}
