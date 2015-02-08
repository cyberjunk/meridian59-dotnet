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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An object for the second spelllist with more info.
    /// </summary>
    [Serializable]
    public class SpellObject : ObjectBase
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_TARGETSCOUNT = "TargetsCount";
        public const string PROPNAME_SCHOOLTYPE = "SchoolType";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get {
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.BYTE;
            }
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);           

            targetsCount = Buffer[cursor];
            cursor++;

            schoolType = (SchoolType)Buffer[cursor];
            cursor++;

            return cursor - StartIndex;   
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            cursor += base.WriteTo(Buffer, StartIndex);                                 // ID (4/8 bytes)

            Buffer[cursor] = targetsCount;
            cursor++;

            Buffer[cursor] = (byte)schoolType;
            cursor++;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            targetsCount = Buffer[0];
            Buffer++;

            schoolType = (SchoolType)Buffer[0];
            Buffer++;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            Buffer[0] = targetsCount;
            Buffer++;

            Buffer[0] = (byte)schoolType;
            Buffer++;
        }
        #endregion

        #region Fields
        protected byte targetsCount;
        protected SchoolType schoolType;
        #endregion

        #region Properties

        /// <summary>
        /// How many concurrent targets this spell can have
        /// </summary>
        public byte TargetsCount
        {
            get { return targetsCount; }
            set
            {
                if (targetsCount != value)
                {
                    targetsCount = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TARGETSCOUNT));
                }
            }
        }

        /// <summary>
        /// Type of school this spell belongs to
        /// </summary>
        public SchoolType SchoolType
        {
            get { return schoolType; }
            set
            {
                if (schoolType != value)
                {
                    schoolType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SCHOOLTYPE));
                }
            }
        }
        #endregion

        #region Constructors
        public SpellObject() : base()
        {
        }

        public SpellObject(
            uint ID, 
            uint Count,            
            uint OverlayFileRID,
            uint NameRID, 
            uint Flags,
            ushort LightFlags, 
            byte LightIntensity, 
            ushort LightColor, 
            AnimationType FirstAnimationType, 
            byte ColorTranslation, 
            byte Effect, 
            Animation Animation, 
            BaseList<SubOverlay> SubOverlays,                      
            byte TargetsCount,
            SchoolType SchoolType)           
            : base(
                ID, Count, 
                OverlayFileRID, NameRID, Flags, 
                LightFlags, LightIntensity, LightColor, 
                FirstAnimationType, ColorTranslation, Effect, 
                Animation, SubOverlays)
        {
            this.targetsCount = TargetsCount;
            this.schoolType = SchoolType;        
        }

        public SpellObject(byte[] Buffer, int StartIndex = 0)
            : base(true, Buffer, StartIndex) { }

        public unsafe SpellObject(ref byte* Buffer)
            : base(true, ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                TargetsCount = 0;
                SchoolType = 0;                
            }
            else
            {
                targetsCount = 0;
                schoolType = 0;  
            }
        }
        #endregion
    }
}
