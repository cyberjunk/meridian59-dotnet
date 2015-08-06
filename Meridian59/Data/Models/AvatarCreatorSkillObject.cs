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
    /// Contains information about a selectable skill during avatar creation wizard.
    /// </summary>
    [Serializable]
    public class AvatarCreatorSkillObject : IByteSerializableFast, INotifyPropertyChanged, IClearable, IStringResolvable
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_EXTRAID = "ExtraID";
        public const string PROPNAME_SKILLNAMEID = "SkillNameID";
        public const string PROPNAME_SKILLDESCRIPTIONID = "SkillDescriptionID";
        public const string PROPNAME_SKILLCOST = "SkillCost";
        public const string PROPNAME_SKILLNAME = "SkillName";
        public const string PROPNAME_SKILLDESCRIPTION = "SkillDescription";
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
                // 4 + 4 + 4 + 4
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT; 
            } 
        }      
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(extraID), 0, Buffer, cursor, TypeSizes.INT);               // ExtraID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(skillNameID), 0, Buffer, cursor, TypeSizes.INT);           // SkillNameID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(skillDescriptionID), 0, Buffer, cursor, TypeSizes.INT);    // SkillDescriptionID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(skillCost), 0, Buffer, cursor, TypeSizes.INT);             // SkillCost (4 bytes)
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        public int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            extraID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            skillNameID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            skillDescriptionID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            skillCost = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = extraID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = skillNameID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = skillDescriptionID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = skillCost;
            Buffer += TypeSizes.INT;
        }
        public unsafe void ReadFrom(ref byte* Buffer)
        {
            extraID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            skillNameID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            skillDescriptionID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            skillCost = *((uint*)Buffer);
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
        protected uint extraID;
        protected uint skillNameID;
        protected uint skillDescriptionID;
        protected uint skillCost;

        protected string skillName;
        protected string skillDescription;
        #endregion

        #region Properties
        public uint ExtraID
        {
            get { return extraID; }
            set
            {
                if (extraID != value)
                {
                    extraID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_EXTRAID));
                }
            }
        }

        public uint SkillNameID
        {
            get { return skillNameID; }
            set
            {
                if (skillNameID != value)
                {
                    skillNameID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLNAMEID));
                }
            }
        }

        public uint SkillDescriptionID
        {
            get { return skillDescriptionID; }
            set
            {
                if (skillDescriptionID != value)
                {
                    skillDescriptionID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLDESCRIPTIONID));
                }
            }
        }

        public uint SkillCost
        {
            get { return skillCost; }
            set
            {
                if (skillCost != value)
                {
                    skillCost = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLCOST));
                }
            }
        }

        // extended
        public string SkillName
        {
            get { return skillName; }
            set
            {
                if (skillName != value)
                {
                    skillName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLNAME));
                }
            }
        }

        public string SkillDescription
        {
            get { return skillDescription; }
            set
            {
                if (skillDescription != value)
                {
                    skillDescription = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLDESCRIPTION));
                }
            }
        }
        #endregion

        #region Constructors
        public AvatarCreatorSkillObject()
        {
            Clear(false);
        }

        public AvatarCreatorSkillObject(uint ExtraID, uint SkillNameID, uint SkillDescriptionID, uint SkillCost)
        {
            this.extraID = ExtraID;
            this.skillNameID = SkillNameID;
            this.skillDescriptionID = SkillDescriptionID;
            this.skillCost = SkillCost;             
        }

        public AvatarCreatorSkillObject(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe AvatarCreatorSkillObject(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ExtraID = 0;
                SkillNameID = 0;
                SkillDescriptionID = 0;
                SkillCost = 0;

                SkillName = String.Empty;
                SkillDescription = String.Empty;
            }
            else
            {
                extraID = 0;
                skillNameID = 0;
                skillDescriptionID = 0;
                skillCost = 0;

                skillName = String.Empty;
                skillDescription = String.Empty;
            }            
        }
        #endregion

        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string skill_name;
            string skill_description;

			StringResources.TryGetValue(skillNameID, out skill_name);
			StringResources.TryGetValue(skillDescriptionID, out skill_description);

            if (RaiseChangedEvent)
            {
                if (skill_name != null) SkillName = skill_name;
                else SkillName = String.Empty;

                if (skill_description != null) SkillDescription = skill_description;
                else SkillDescription = String.Empty;
            }
            else
            {
                if (skill_name != null) skillName = skill_name;
                else skillName = String.Empty;

                if (skill_description != null) skillDescription = skill_description;
                else skillDescription = String.Empty;
            }
        }
        #endregion 
    }
}
