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
using System.Collections.Generic;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An object for the second spelllist with more info.
    /// </summary>
    [Serializable]
    public class SkillObject : ObjectBase
    {
        public const string PROPNAME_TARGETSCOUNT   = "TargetsCount";
        public const string PROPNAME_SCHOOLTYPE     = "SchoolType";
        public const string PROPNAME_ISACTIVESKILL  = "IsActiveSkill";

        protected byte targetsCount;
        protected SchoolType schoolType;
        protected bool isActiveSkill;

        #region IByteSerializable
        public override int ByteLength { 
            get {
                return base.ByteLength
#if !VANILLA && !OPENMERIDIAN
                    + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE
#endif
                    ;
            }
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);
#if !VANILLA && !OPENMERIDIAN
            targetsCount = Buffer[cursor];
            cursor++;

            schoolType = (SchoolType)Buffer[cursor];
            cursor++;

            isActiveSkill = Convert.ToBoolean(Buffer[cursor]);
            cursor++;
#endif
            return cursor - StartIndex;   
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            cursor += base.WriteTo(Buffer, StartIndex);                                 // ID (4/8 bytes)
#if !VANILLA && !OPENMERIDIAN
            Buffer[cursor] = targetsCount;
            cursor++;

            Buffer[cursor] = (byte)schoolType;
            cursor++;

            Buffer[cursor] = Convert.ToByte(isActiveSkill);
            cursor++;
#endif
            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);
#if !VANILLA && !OPENMERIDIAN
            targetsCount = Buffer[0];
            Buffer++;

            schoolType = (SchoolType)Buffer[0];
            Buffer++;

            isActiveSkill = Convert.ToBoolean(Buffer[0]);
            Buffer++;
#endif
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);
#if !VANILLA && !OPENMERIDIAN
            Buffer[0] = targetsCount;
            Buffer++;

            Buffer[0] = (byte)schoolType;
            Buffer++;

            Buffer[0] = Convert.ToByte(isActiveSkill);
            Buffer++;
#endif
        }
        #endregion

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


        /// <summary>
        /// Whether skill is 'active' (performable) or not
        /// </summary>
        public bool IsActiveSkill
        {
            get { return isActiveSkill; }
            set
            {
                if (isActiveSkill != value)
                {
                    isActiveSkill = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISACTIVESKILL));
                }
            }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public SkillObject() : base() { }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Count"></param>
        /// <param name="OverlayFileRID"></param>
        /// <param name="NameRID"></param>
        /// <param name="Flags"></param>
        /// <param name="LightingInfo"></param>
        /// <param name="FirstAnimationType"></param>
        /// <param name="ColorTranslation"></param>
        /// <param name="Effect"></param>
        /// <param name="Animation"></param>
        /// <param name="SubOverlays"></param>
        /// <param name="TargetsCount"></param>
        /// <param name="SchoolType"></param>
        /// <param name="IsActiveSkill"></param>
        public SkillObject(
            uint ID,
            uint Count,
            uint OverlayFileRID,
            uint NameRID,
            uint Flags,
            LightingInfo LightingInfo,
            AnimationType FirstAnimationType,
            byte ColorTranslation,
            byte Effect,
            Animation Animation,
            IEnumerable<SubOverlay> SubOverlays,
            byte TargetsCount,
            SchoolType SchoolType,
            bool IsActiveSkill)
            : base(
                ID, Count, OverlayFileRID, NameRID, Flags,
                LightingInfo, FirstAnimationType,
                ColorTranslation, Effect, Animation, SubOverlays)
        {
            this.targetsCount = TargetsCount;
            this.schoolType = SchoolType;
            this.isActiveSkill = IsActiveSkill;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public SkillObject(byte[] Buffer, int StartIndex = 0)
            : base(true, Buffer, StartIndex) { }

        /// <summary>
        /// Constructor by pointer parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe SkillObject(ref byte* Buffer)
            : base(true, ref Buffer) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                TargetsCount = 0;
                SchoolType = 0;
                IsActiveSkill = false;
            }
            else
            {
                targetsCount = 0;
                schoolType = 0;
                isActiveSkill = false;
            }
        }
    }
}
