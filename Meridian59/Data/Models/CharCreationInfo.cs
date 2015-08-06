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
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;
using Meridian59.Files;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Data.Lists;
using System.Collections.Generic;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Info for the character creation wizard.
    /// </summary>
    [Serializable]
    public class CharCreationInfo : 
        INotifyPropertyChanged, IByteSerializable, IStringResolvable,
        IResourceResolvable, IClearable, IUpdatable<CharCreationInfo>
    {
        #region Constants
        public const uint ATTRIBUTE_MINVALUE                = 1;
        public const uint ATTRIBUTE_MAXVALUE                = 50; 
        public const uint ATTRIBUTE_DEFAULT                 = 25;        
        public const uint ATTRIBUTE_MAXSUM                  = 200;
        public const uint SKILLPOINTS_MAXSUM                = 45;
        public const uint SLOTID_DEFAULT                    = 0;
        public const byte HAIRCOLOR_DEFAULT                 = 0;
        public const byte SKINCOLOR_DEFAULT                 = 0;
        public const string AVATARNAME_DEFAULT              = "";
        public const string AVATARDESCRIPTION_DEFAULT       = "";
        public const Gender GENDER_DEFAULT                  = Gender.Male;
        public const bool ISDATAOK_DEFAULT                  = true;
        public const string PROPNAME_HAIRCOLORS             = "HairColors";
        public const string PROPNAME_SKINCOLORS             = "SkinColors";
        public const string PROPNAME_MALEHAIRIDS            = "MaleHairIDs";
        public const string PROPNAME_MALESKULLID            = "MaleSkullID";
        public const string PROPNAME_MALEEYEIDS             = "MaleEyeIDs";
        public const string PROPNAME_MALENOSEIDS            = "MaleNoseIDs";
        public const string PROPNAME_MALEMOUTHIDS           = "MaleMouthIDs";
        public const string PROPNAME_FEMALEHAIRIDS          = "FemaleHairIDs";
        public const string PROPNAME_FEMALESKULLID          = "FemaleSkullID";
        public const string PROPNAME_FEMALEEYEIDS           = "FemaleEyeIDs";
        public const string PROPNAME_FEMALENOSEIDS          = "FemaleNoseIDs";
        public const string PROPNAME_FEMALEMOUTHIDS         = "FemaleMouthIDs";
        public const string PROPNAME_SPELLS                 = "Spells";
        public const string PROPNAME_SKILLS                 = "Skills";
        public const string PROPNAME_SELECTEDSPELLS         = "Spells";
        public const string PROPNAME_SELECTEDSKILLS         = "Skills";
        public const string PROPNAME_EXAMPLEMODEL           = "ExampleModel";
        public const string PROPNAME_MIGHT                  = "Might";
        public const string PROPNAME_INTELLECT              = "Intellect";
        public const string PROPNAME_STAMINA                = "Stamina";
        public const string PROPNAME_AGILITY                = "Agility";
        public const string PROPNAME_MYSTICISM              = "Mysticism";
        public const string PROPNAME_AIM                    = "Aim";
        public const string PROPNAME_ATTRIBUTESCURRENT      = "AttributesCurrent";
        public const string PROPNAME_ATTRIBUTESAVAILABLE    = "AttributesAvailable";
        public const string PROPNAME_SKILLPOINTSCURRENT     = "SkillPointsCurrent";
        public const string PROPNAME_SKILLPOINTSAVAILABLE   = "SkillPointsAvailable";
        public const string PROPNAME_SLOTID                 = "SlotID";
        public const string PROPNAME_GENDER                 = "Gender";
        public const string PROPNAME_HAIRCOLOR              = "HairColor";
        public const string PROPNAME_SKINCOLOR              = "SkinColor";
        public const string PROPNAME_AVATARNAME             = "AvatarName";
        public const string PROPNAME_AVATARDESCRIPTION      = "AvatarDescription";
        public const string PROPNAME_ISDATAOK               = "IsDataOK";
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
                //  blacklistcharslen + blacklistchars + skincolorslen + skincolors
                int length = 
                    TypeSizes.BYTE + HairColors.Length + 
                    TypeSizes.BYTE + SkinColors.Length;

                // male stuff
                length += TypeSizes.INT + (TypeSizes.INT * MaleHairIDs.Length);
                length += TypeSizes.INT;
                length += TypeSizes.INT + (TypeSizes.INT * MaleEyeIDs.Length);
                length += TypeSizes.INT + (TypeSizes.INT * MaleNoseIDs.Length);
                length += TypeSizes.INT + (TypeSizes.INT * MaleMouthIDs.Length);

                // female stuff
                length += TypeSizes.INT + (TypeSizes.INT * FemaleHairIDs.Length);
                length += TypeSizes.INT;
                length += TypeSizes.INT + (TypeSizes.INT * FemaleEyeIDs.Length);
                length += TypeSizes.INT + (TypeSizes.INT * FemaleNoseIDs.Length);
                length += TypeSizes.INT + (TypeSizes.INT * FemaleMouthIDs.Length);

                // spellslen + spells
                length += TypeSizes.INT;
                for (int i = 0; i < Spells.Count; i++)
                    length += Spells[i].ByteLength;

                // skillslen + skills
                length += TypeSizes.INT;
                for (int i = 0; i < Skills.Count; i++)
                    length += Skills[i].ByteLength;
               
                return length;
            }
        }
        
        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = Convert.ToByte(HairColors.Length);
            cursor++;

            Array.Copy(HairColors, 0, Buffer, cursor, HairColors.Length);
            cursor += HairColors.Length;

            Buffer[cursor] = Convert.ToByte(SkinColors.Length);
            cursor++;

            Array.Copy(SkinColors, 0, Buffer, cursor, SkinColors.Length);
            cursor += SkinColors.Length;

            // MALE parts

            Array.Copy(BitConverter.GetBytes(MaleHairIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < MaleHairIDs.Length; i++)           
                cursor += MaleHairIDs[i].WriteTo(Buffer, cursor);

            cursor += MaleSkullID.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(MaleEyeIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < MaleEyeIDs.Length; i++)           
                cursor += MaleEyeIDs[i].WriteTo(Buffer, cursor);
            
            Array.Copy(BitConverter.GetBytes(MaleNoseIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < MaleNoseIDs.Length; i++)          
                cursor += MaleNoseIDs[i].WriteTo(Buffer, cursor);
            
            Array.Copy(BitConverter.GetBytes(MaleMouthIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < MaleMouthIDs.Length; i++)           
                cursor += MaleMouthIDs[i].WriteTo(Buffer, cursor);
            

            // FEMALE parts

            Array.Copy(BitConverter.GetBytes(FemaleHairIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < FemaleHairIDs.Length; i++)           
                cursor += FemaleHairIDs[i].WriteTo(Buffer, cursor);

            cursor += FemaleSkullID.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(FemaleEyeIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < FemaleEyeIDs.Length; i++)
                cursor += FemaleEyeIDs[i].WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(FemaleNoseIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < FemaleNoseIDs.Length; i++)
                cursor += FemaleNoseIDs[i].WriteTo(Buffer, cursor);
            
            Array.Copy(BitConverter.GetBytes(FemaleMouthIDs.Length), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < FemaleMouthIDs.Length; i++)            
                cursor += FemaleMouthIDs[i].WriteTo(Buffer, cursor);
            

            // SPELLS
            Array.Copy(BitConverter.GetBytes(Spells.Count), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < Spells.Count; i++)
                cursor += Spells[i].WriteTo(Buffer, cursor);

            // SKILLS
            Array.Copy(BitConverter.GetBytes(Skills.Count), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            for (int i = 0; i < Skills.Count; i++)
                cursor += Skills[i].WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            byte blen = Buffer[cursor];
            cursor++;

            HairColors = new byte[blen];
            Array.Copy(Buffer, cursor, HairColors, 0, blen);
            cursor += blen;

            blen = Buffer[cursor];
            cursor++;

            SkinColors = new byte[blen];
            Array.Copy(Buffer, cursor, SkinColors, 0, blen);
            cursor += blen;

            // MALE parts
            int ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            MaleHairIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                MaleHairIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            MaleSkullID = new ResourceIDBGF(Buffer, cursor);
            cursor += TypeSizes.INT;

            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            MaleEyeIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                MaleEyeIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            MaleNoseIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                MaleNoseIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            MaleMouthIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                MaleMouthIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            // FEMALE parts
            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            FemaleHairIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                FemaleHairIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            FemaleSkullID = new ResourceIDBGF(Buffer, cursor);
            cursor += TypeSizes.INT;

            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            FemaleEyeIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                FemaleEyeIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            FemaleNoseIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                FemaleNoseIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            FemaleMouthIDs = new ResourceIDBGF[ilen];
            for (int i = 0; i < ilen; i++)
            {
                FemaleMouthIDs[i] = new ResourceIDBGF(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            // SPELLS
            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Spells.Clear();
            for (int i = 0; i < ilen; i++)
            {
                AvatarCreatorSpellObject spellObj = 
                    new AvatarCreatorSpellObject(Buffer, cursor);
                
                cursor += spellObj.ByteLength;

                Spells.Add(spellObj);
            }

            // SKILLS
            ilen = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Skills.Clear();
            for (int i = 0; i < ilen; i++)
            {
                AvatarCreatorSkillObject skillObj = 
                    new AvatarCreatorSkillObject(Buffer, cursor);

                cursor += skillObj.ByteLength;

                Skills.Add(skillObj);               
            }

            return cursor - StartIndex;
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
        protected byte[] hairColors;
        protected byte[] skinColors;
        protected ResourceIDBGF[] maleHairIDs;
        protected ResourceIDBGF maleSkullID;
        protected ResourceIDBGF[] maleEyeIDs;
        protected ResourceIDBGF[] maleNoseIDs;
        protected ResourceIDBGF[] maleMouthIDs;
        protected ResourceIDBGF[] femaleHairIDs;
        protected ResourceIDBGF femaleSkullID;
        protected ResourceIDBGF[] femaleEyeIDs;
        protected ResourceIDBGF[] femaleNoseIDs;
        protected ResourceIDBGF[] femaleMouthIDs;
        protected BaseList<AvatarCreatorSpellObject> spells;
        protected BaseList<AvatarCreatorSkillObject> skills;
        protected BaseList<AvatarCreatorSpellObject> selectedSpells;
        protected BaseList<AvatarCreatorSkillObject> selectedSkills;
        protected ObjectBase exampleModel;
        protected uint might = ATTRIBUTE_DEFAULT;
        protected uint intellect = ATTRIBUTE_DEFAULT;
        protected uint stamina = ATTRIBUTE_DEFAULT;
        protected uint agility = ATTRIBUTE_DEFAULT;
        protected uint mysticism = ATTRIBUTE_DEFAULT;
        protected uint aim = ATTRIBUTE_DEFAULT;
        protected uint slotID = SLOTID_DEFAULT;
        protected Gender gender = GENDER_DEFAULT;
        protected byte hairColor = HAIRCOLOR_DEFAULT;
        protected byte skinColor = SKINCOLOR_DEFAULT;
        protected string avatarName = AVATARNAME_DEFAULT;
        protected string avatarDescription = AVATARDESCRIPTION_DEFAULT;
        protected bool isDataOK = ISDATAOK_DEFAULT;
        #endregion

        #region Properties
        public byte[] HairColors
        {
            get { return hairColors; }
            set
            {
                if (hairColors != value)
                {
                    hairColors = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HAIRCOLORS));
                }
            }
        }

        public byte[] SkinColors
        {
            get { return skinColors; }
            set
            {
                if (skinColors != value)
                {
                    skinColors = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKINCOLORS));
                }
            }
        }

        public ResourceIDBGF[] MaleHairIDs
        {
            get { return maleHairIDs; }
            set
            {
                if (maleHairIDs != value)
                {
                    maleHairIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MALEHAIRIDS));
                }
            }
        }

        public ResourceIDBGF MaleSkullID
        {
            get { return maleSkullID; }
            set
            {
                if (maleSkullID != value)
                {
                    maleSkullID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MALESKULLID));
                }
            }
        }

        public ResourceIDBGF[] MaleEyeIDs
        {
            get { return maleEyeIDs; }
            set
            {
                if (maleEyeIDs != value)
                {
                    maleEyeIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MALEEYEIDS));
                }
            }
        }

        public ResourceIDBGF[] MaleNoseIDs
        {
            get { return maleNoseIDs; }
            set
            {
                if (maleNoseIDs != value)
                {
                    maleNoseIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MALENOSEIDS));
                }
            }
        }

        public ResourceIDBGF[] MaleMouthIDs
        {
            get { return maleMouthIDs; }
            set
            {
                if (maleMouthIDs != value)
                {
                    maleMouthIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MALEMOUTHIDS));
                }
            }
        }

        public ResourceIDBGF[] FemaleHairIDs
        {
            get { return femaleHairIDs; }
            set
            {
                if (femaleHairIDs != value)
                {
                    femaleHairIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FEMALEHAIRIDS));
                }
            }
        }

        public ResourceIDBGF FemaleSkullID
        {
            get { return femaleSkullID; }
            set
            {
                if (femaleSkullID != value)
                {
                    femaleSkullID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FEMALESKULLID));
                }
            }
        }

        public ResourceIDBGF[] FemaleEyeIDs
        {
            get { return femaleEyeIDs; }
            set
            {
                if (femaleEyeIDs != value)
                {
                    femaleEyeIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FEMALEEYEIDS));
                }
            }
        }

        public ResourceIDBGF[] FemaleNoseIDs
        {
            get { return femaleNoseIDs; }
            set
            {
                if (femaleNoseIDs != value)
                {
                    femaleNoseIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FEMALENOSEIDS));
                }
            }
        }

        public ResourceIDBGF[] FemaleMouthIDs
        {
            get { return femaleMouthIDs; }
            set
            {
                if (femaleMouthIDs != value)
                {
                    femaleMouthIDs = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FEMALEMOUTHIDS));
                }
            }
        }

        public BaseList<AvatarCreatorSpellObject> Spells
        {
            get { return spells; }
            set
            {
                if (spells != value)
                {
                    spells = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLS));
                }
            }
        }

        public BaseList<AvatarCreatorSkillObject> Skills
        {
            get { return skills; }
            set
            {
                if (skills != value)
                {
                    skills = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLS));
                }
            }
        }

        public BaseList<AvatarCreatorSpellObject> SelectedSpells
        {
            get { return selectedSpells; }
            set
            {
                if (selectedSpells != value)
                {
                    selectedSpells = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SELECTEDSPELLS));
                }
            }
        }

        public BaseList<AvatarCreatorSkillObject> SelectedSkills
        {
            get { return selectedSkills; }
            set
            {
                if (selectedSkills != value)
                {
                    selectedSkills = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SELECTEDSKILLS));
                }
            }
        }

        public ObjectBase ExampleModel
        {
            get { return exampleModel; }
            set
            {
                if (exampleModel != value)
                {
                    exampleModel = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_EXAMPLEMODEL));
                }
            }
        }

        public uint Might
        {
            get { return might; }
            set
            {
                if (might != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < might || (int)AttributesAvailable + (int)might - (int)value >= 0))
                {
                    might = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MIGHT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint Intellect
        {
            get { return intellect; }
            set
            {
                if (intellect != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < intellect || (int)AttributesAvailable + (int)intellect - (int)value >= 0))
                {
                    intellect = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_INTELLECT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint Stamina
        {
            get { return stamina; }
            set
            {
                if (stamina != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < stamina || (int)AttributesAvailable + (int)stamina - (int)value >= 0))
                {
                    stamina = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_STAMINA));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint Agility
        {
            get { return agility; }
            set
            {
                if (agility != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < agility || (int)AttributesAvailable + (int)agility - (int)value >= 0))
                {
                    agility = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AGILITY));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint Mysticism
        {
            get { return mysticism; }
            set
            {
                if (mysticism != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < mysticism || (int)AttributesAvailable + (int)mysticism - (int)value >= 0))
                {
                    mysticism = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MYSTICISM));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint Aim
        {
            get { return aim; }
            set
            {
                if (aim != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < aim || (int)AttributesAvailable + (int)aim - (int)value >= 0))
                {
                    aim = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AIM));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint AttributesCurrent
        {
            get { return might + intellect + stamina + agility + mysticism + aim; }
        }

        public uint AttributesAvailable
        {
            get { return ATTRIBUTE_MAXSUM - Math.Min(AttributesCurrent, ATTRIBUTE_MAXSUM); }
        }

        public uint SkillPointsCurrent
        {
            get 
            { 
                uint val = 0;
                foreach (AvatarCreatorSpellObject obj in SelectedSpells)
                    val += obj.SpellCost;

                foreach (AvatarCreatorSkillObject obj in SelectedSkills)
                    val += obj.SkillCost;

                return val;
            }
        }

        public uint SkillPointsAvailable
        {
            get
            {
                return SKILLPOINTS_MAXSUM - SkillPointsCurrent;
            }
        }

        /// <summary>
        /// Not transferred, has to be set when invoking the wizard with a char slot.
        /// Can be used to create the avatar at the end.
        /// </summary>
        public uint SlotID
        {
            get { return slotID; }
            set
            {
                if (slotID != value)
                {
                    slotID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SLOTID));
                }
            }
        }

        /// <summary>
        /// Not transferred.
        /// Use SetExampleModel() to set.
        /// </summary>
        public Gender Gender
        {
            get { return gender; }
        }

        /// <summary>
        /// Not transferred.
        /// Use SetExampleModel() to set.
        /// </summary>
        public byte HairColor
        {
            get { return hairColor; }
        }

        /// <summary>
        /// Not transferred.
        /// Use SetExampleModel() to set.
        /// </summary>
        public byte SkinColor
        {
            get { return skinColor; }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public string AvatarDescription
        {
            get { return avatarDescription; }
            set
            {
                if (avatarDescription != value)
                {
                    avatarDescription = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AVATARDESCRIPTION));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDataOK
        {
            get { return isDataOK; }
            set
            {
                if (isDataOK != value)
                {
                    isDataOK = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISDATAOK));
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public CharCreationInfo()
        {
            ExampleModel = new ObjectBase();
            
            // init list instances
            spells = new BaseList<AvatarCreatorSpellObject>(30);
            skills = new BaseList<AvatarCreatorSkillObject>(30);
            selectedSpells = new BaseList<AvatarCreatorSpellObject>(5);
            selectedSkills = new BaseList<AvatarCreatorSkillObject>(5);

            Clear(false);
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="HairColors"></param>
        /// <param name="SkinColors"></param>
        /// <param name="MaleHairIDs"></param>
        /// <param name="MaleSkullID"></param>
        /// <param name="MaleEyeIDs"></param>
        /// <param name="MaleNoseIDs"></param>
        /// <param name="MaleMouthIDs"></param>
        /// <param name="FemaleHairIDs"></param>
        /// <param name="FemaleSkullID"></param>
        /// <param name="FemaleEyeIDs"></param>
        /// <param name="FemaleNoseIDs"></param>
        /// <param name="FemaleMouthIDs"></param>
        /// <param name="Spells"></param>
        /// <param name="Skills"></param>
        public CharCreationInfo(
            byte[] HairColors, 
            byte[] SkinColors,
            ResourceIDBGF[] MaleHairIDs, 
            ResourceIDBGF MaleSkullID, 
            ResourceIDBGF[] MaleEyeIDs, 
            ResourceIDBGF[] MaleNoseIDs, 
            ResourceIDBGF[] MaleMouthIDs,
            ResourceIDBGF[] FemaleHairIDs, 
            ResourceIDBGF FemaleSkullID, 
            ResourceIDBGF[] FemaleEyeIDs, 
            ResourceIDBGF[] FemaleNoseIDs, 
            ResourceIDBGF[] FemaleMouthIDs,
            IEnumerable<AvatarCreatorSpellObject> Spells,
            IEnumerable<AvatarCreatorSkillObject> Skills)
        {           
            hairColors = HairColors;
            skinColors = SkinColors;
            maleHairIDs = MaleHairIDs;
            maleSkullID = MaleSkullID;
            maleEyeIDs = MaleEyeIDs;
            maleNoseIDs = MaleNoseIDs;
            maleMouthIDs = MaleMouthIDs;
            femaleHairIDs = FemaleHairIDs;
            femaleSkullID = FemaleSkullID;
            femaleEyeIDs = FemaleEyeIDs;
            femaleNoseIDs = FemaleNoseIDs;
            femaleMouthIDs = FemaleMouthIDs;
            
            // init list instances
            spells = new BaseList<AvatarCreatorSpellObject>(30);
            skills = new BaseList<AvatarCreatorSkillObject>(30);
            selectedSpells = new BaseList<AvatarCreatorSpellObject>(5);
            selectedSkills = new BaseList<AvatarCreatorSkillObject>(5);

            spells.AddRange(Spells);
            skills.AddRange(Skills);

            ExampleModel = new ObjectBase();         
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public CharCreationInfo(byte[] Buffer, int StartIndex = 0)
        {
            ExampleModel = new ObjectBase();

            // init list instances
            spells = new BaseList<AvatarCreatorSpellObject>(30);
            skills = new BaseList<AvatarCreatorSkillObject>(30);
            selectedSpells = new BaseList<AvatarCreatorSpellObject>(5);
            selectedSkills = new BaseList<AvatarCreatorSkillObject>(5);

            ReadFrom(Buffer, StartIndex); 
        }
        #endregion

        #region IStringResolvable
		public virtual void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            // male 

            foreach (ResourceIDBGF obj in maleHairIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

			maleSkullID.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in maleEyeIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in maleNoseIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in maleMouthIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

            // female 

            foreach (ResourceIDBGF obj in femaleHairIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

			femaleSkullID.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in femaleEyeIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in femaleNoseIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in femaleMouthIDs)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);

            // spells/skills
            
            foreach (AvatarCreatorSpellObject obj in spells)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);
            
            foreach (AvatarCreatorSkillObject obj in skills)
				obj.ResolveStrings(StringResources, RaiseChangedEvent);
        }
        #endregion

        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            // male 

            foreach (ResourceIDBGF obj in maleHairIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            maleSkullID.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in maleEyeIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in maleNoseIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in maleMouthIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            // female 

            foreach (ResourceIDBGF obj in femaleHairIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            femaleSkullID.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in femaleEyeIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in femaleNoseIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            foreach (ResourceIDBGF obj in femaleMouthIDs)
                obj.ResolveResources(M59ResourceManager, RaiseChangedEvent);
           
            // exampledata dummy-body

            ExampleModel.OverlayFile = "btc.bgf";
            ExampleModel.ResolveResources(M59ResourceManager, false);                  
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                HairColors = new byte[0];
                SkinColors = new byte[0];
                MaleHairIDs = new ResourceIDBGF[0];
                MaleSkullID = new ResourceIDBGF();
                MaleEyeIDs = new ResourceIDBGF[0];
                MaleNoseIDs = new ResourceIDBGF[0];
                MaleMouthIDs = new ResourceIDBGF[0];
                FemaleHairIDs = new ResourceIDBGF[0];
                FemaleSkullID = new ResourceIDBGF();
                FemaleEyeIDs = new ResourceIDBGF[0];
                FemaleNoseIDs = new ResourceIDBGF[0];
                FemaleMouthIDs = new ResourceIDBGF[0];
                Spells.Clear();
                Skills.Clear();
                SelectedSpells.Clear();
                SelectedSkills.Clear();
                
                Might = ATTRIBUTE_DEFAULT;
                Intellect = ATTRIBUTE_DEFAULT;
                Stamina = ATTRIBUTE_DEFAULT;
                Agility = ATTRIBUTE_DEFAULT;
                Mysticism = ATTRIBUTE_DEFAULT;
                Aim = ATTRIBUTE_DEFAULT;
                SlotID = SLOTID_DEFAULT;
            }
            else
            {
                hairColors = new byte[0];
                skinColors = new byte[0];
                maleHairIDs = new ResourceIDBGF[0];
                maleSkullID = new ResourceIDBGF();
                maleEyeIDs = new ResourceIDBGF[0];
                maleNoseIDs = new ResourceIDBGF[0];
                maleMouthIDs = new ResourceIDBGF[0];
                femaleHairIDs = new ResourceIDBGF[0];
                femaleSkullID = new ResourceIDBGF();
                femaleEyeIDs = new ResourceIDBGF[0];
                femaleNoseIDs = new ResourceIDBGF[0];
                femaleMouthIDs = new ResourceIDBGF[0];
                spells.Clear();
                skills.Clear();
                selectedSpells.Clear();
                selectedSkills.Clear();

                might = ATTRIBUTE_DEFAULT;
                intellect = ATTRIBUTE_DEFAULT;
                stamina = ATTRIBUTE_DEFAULT;
                agility = ATTRIBUTE_DEFAULT;
                mysticism = ATTRIBUTE_DEFAULT;
                aim = ATTRIBUTE_DEFAULT;
                slotID = SLOTID_DEFAULT;
            }
        }
        #endregion

        public void UpdateFromModel(CharCreationInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                HairColors = Model.HairColors;
                SkinColors = Model.SkinColors;
                MaleHairIDs = Model.MaleHairIDs;
                MaleSkullID = Model.MaleSkullID;
                MaleEyeIDs = Model.MaleEyeIDs;
                MaleNoseIDs = Model.MaleNoseIDs;
                MaleMouthIDs = Model.MaleMouthIDs;
                FemaleHairIDs = Model.FemaleHairIDs;
                FemaleSkullID = Model.FemaleSkullID;
                FemaleEyeIDs = Model.FemaleEyeIDs;
                FemaleNoseIDs = Model.FemaleNoseIDs;
                FemaleMouthIDs = Model.FemaleMouthIDs;

                Spells.Clear();
                Spells.AddRange(Model.Spells);

                Skills.Clear();
                Skills.AddRange(Model.Skills);

                SelectedSpells.Clear();
                SelectedSpells.AddRange(Model.SelectedSpells);

                SelectedSkills.Clear();
                SelectedSkills.AddRange(Model.SelectedSkills);

                AvatarName = Model.AvatarName;
                AvatarDescription = Model.AvatarDescription;
                Might = Model.Might;
                Intellect = Model.Intellect;
                Stamina = Model.Stamina;
                Agility = Model.Agility;
                Mysticism = Model.Mysticism;
                Aim = Model.Aim;

                // raise change on values
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSCURRENT));
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSAVAILABLE));

                // don't update slotid
            }
            else
            {
                hairColors = Model.HairColors;
                skinColors = Model.SkinColors;
                maleHairIDs = Model.MaleHairIDs;
                maleSkullID = Model.MaleSkullID;
                maleEyeIDs = Model.MaleEyeIDs;
                maleNoseIDs = Model.MaleNoseIDs;
                maleMouthIDs = Model.MaleMouthIDs;
                femaleHairIDs = Model.FemaleHairIDs;
                femaleSkullID = Model.FemaleSkullID;
                femaleEyeIDs = Model.FemaleEyeIDs;
                femaleNoseIDs = Model.FemaleNoseIDs;
                femaleMouthIDs = Model.FemaleMouthIDs;
                
                spells.Clear();
                spells.AddRange(Model.Spells);

                skills.Clear();
                skills.AddRange(Model.Skills);

                selectedSpells.Clear();
                selectedSpells.AddRange(Model.SelectedSpells);

                selectedSkills.Clear();
                selectedSkills.AddRange(Model.SelectedSkills);

                avatarName = Model.AvatarName;
                avatarDescription = Model.AvatarDescription;
                might = Model.Might;
                intellect = Model.Intellect;
                stamina = Model.Stamina;
                agility = Model.Agility;
                mysticism = Model.Mysticism;
                aim = Model.Aim;

                // don't update slotid              
            }

            // update example model instance
            ExampleModel.UpdateFromModel(Model.ExampleModel, RaiseChangedEvent);
        }

        public void DecompressResources()
        {
            // male 

            foreach (ResourceIDBGF obj in maleHairIDs)
                if (obj.Resource != null) 
                    obj.Resource.DecompressAll();

            if (maleSkullID.Resource != null)
                maleSkullID.Resource.DecompressAll();

            foreach (ResourceIDBGF obj in maleEyeIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();

            foreach (ResourceIDBGF obj in maleNoseIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();

            foreach (ResourceIDBGF obj in maleMouthIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();

            // female 

            foreach (ResourceIDBGF obj in femaleHairIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();

            if (femaleSkullID.Resource != null)
                femaleSkullID.Resource.DecompressAll();

            foreach (ResourceIDBGF obj in femaleEyeIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();

            foreach (ResourceIDBGF obj in femaleNoseIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();

            foreach (ResourceIDBGF obj in femaleMouthIDs)
                if (obj.Resource != null)
                    obj.Resource.DecompressAll();
        }

        public void SelectSkill(uint ExtraID)
        {
            foreach (AvatarCreatorSkillObject obj in Skills)
            {
                if (obj.ExtraID == ExtraID && (obj.SkillCost <= SkillPointsAvailable))
                {
                    // move from spells to selectespells
                    Skills.Remove(obj);
                    SelectedSkills.Add(obj);

                    // raise change on values
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSAVAILABLE));

                    break;
                }
            }
        }

        public void DeselectSkill(uint ExtraID)
        {
            foreach (AvatarCreatorSkillObject obj in SelectedSkills)
            {
                if (obj.ExtraID == ExtraID)
                {
                    // move from spells to selectespells
                    SelectedSkills.Remove(obj);
                    Skills.Add(obj);

                    // raise change on values
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSAVAILABLE));

                    break;
                }
            }
        }

        public void SelectSpell(uint ExtraID)
        {
            foreach (AvatarCreatorSpellObject obj in Spells)
            {
                if (obj.ExtraID == ExtraID && (obj.SpellCost <= SkillPointsAvailable))
                {
                    // move from spells to selectespells
                    Spells.Remove(obj);
                    SelectedSpells.Add(obj);

                    // raise change on values
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSAVAILABLE));

                    break;
                }
            }
        }

        public void DeselectSpell(uint ExtraID)
        {
            foreach (AvatarCreatorSpellObject obj in SelectedSpells)
            {
                if (obj.ExtraID == ExtraID)
                {
                    // move from spells to selectespells
                    SelectedSpells.Remove(obj);
                    Spells.Add(obj);

                    // raise change on values
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTSAVAILABLE));

                    break;
                }
            }
        }

        /// <summary>
        /// Returns all stat values as array.
        /// </summary>
        /// <returns></returns>
        public uint[] GetAttributesArray()
        {
            return new uint[] { might, intellect, stamina, agility, mysticism, aim };
        }

        /// <summary>
        /// Returns all "ExtraID" values from instances in SelectedSpells.
        /// </summary>
        /// <returns></returns>
        public uint[] GetSelectedSpellIDs()
        {
            uint[] ids = new uint[SelectedSpells.Count];
            for (int i = 0; i < SelectedSpells.Count; i++)         
                ids[i] = SelectedSpells[i].ExtraID;

            return ids;            
        }

        /// <summary>
        /// Returns all "ExtraID" values from instances in SelectedSkills.
        /// </summary>
        /// <returns></returns>
        public uint[] GetSelectedSkillIDs()
        {
            uint[] ids = new uint[SelectedSkills.Count];
            for (int i = 0; i < SelectedSkills.Count; i++)
                ids[i] = SelectedSkills[i].ExtraID;

            return ids;
        }

        /// <summary>
        /// Returns resource IDs or NULL
        /// </summary>
        /// <returns></returns>
        public uint[] GetSelectedResourceIDs()
        {
            // get suboverlays
            SubOverlay subOvSkull = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.HEAD);
            SubOverlay subOvHair = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.HAIR);
            SubOverlay subOvEyes = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.EYES);
            SubOverlay subOvNose = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.NOSE);
            SubOverlay subOvMouth = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.MOUTH);

            // all values available
            if (subOvSkull != null &&
                subOvHair != null &&
                subOvEyes != null &&
                subOvNose != null &&
                subOvMouth != null)
            {
                uint[] ids = new uint[5];
                ids[0] = subOvSkull.ResourceID;
                ids[1] = subOvHair.ResourceID;
                ids[2] = subOvEyes.ResourceID;
                ids[3] = subOvNose.ResourceID;
                ids[4] = subOvMouth.ResourceID;

                return ids;
            }
            else
                return null;
        }

        /// <summary>
        /// Sets attributes such as Might, Stamina, ... to a
        /// predefined set suitable for mages.
        /// </summary>
        public void SetAttributesToMage()
        {
            // set to 0 first
            Might = Intellect = Stamina = Agility = Mysticism = Aim = ATTRIBUTE_MINVALUE;
#if !VANILLA
            Might       = 40;
            Intellect   = 50;
            Stamina     = 45;
            Agility     = 15;
            Mysticism   = 45;
            Aim         = 5;
#else           
            Might       = 35;
            Intellect   = 40;
            Stamina     = 50;
            Agility     = 15;
            Mysticism   = 45;
            Aim         = 15;
#endif
        }

        /// <summary>
        /// Sets attributes such as Might, Stamina, ... to a
        /// predefined set suitable for warriors.
        /// </summary>
        public void SetAttributesToWarrior()
        {
            // set to 0 first
            Might = Intellect = Stamina = Agility = Mysticism = Aim = ATTRIBUTE_MINVALUE;
#if !VANILLA
            Might       = 40;
            Intellect   = 10;
            Stamina     = 50;
            Agility     = 40;
            Mysticism   = 10;
            Aim         = 50;
#else           
            Might       = 50;
            Intellect   = 10;
            Stamina     = 50;
            Agility     = 30;
            Mysticism   = 10;
            Aim         = 50;
#endif
        }

        /// <summary>
        /// Sets attributes such as Might, Stamina, ... to a
        /// predefined set suitable for hybrids.
        /// </summary>
        public void SetAttributesToHybrid()
        {
            // set to 0 first
            Might = Intellect = Stamina = Agility = Mysticism = Aim = ATTRIBUTE_MINVALUE;
#if !VANILLA
            Might       = 40;
            Intellect   = 25;
            Stamina     = 50;
            Agility     = 15;
            Mysticism   = 35;
            Aim         = 35;
#else           
            Might       = 40;
            Intellect   = 25;
            Stamina     = 35;
            Agility     = 25;
            Mysticism   = 35;
            Aim         = 40;
#endif
        }

        /// <summary>
        /// Adjusts the ExampleModel instance
        /// </summary>
        /// <param name="Gender"></param>
        /// <param name="SkinColorIndex"></param>
        /// <param name="HairColorIndex"></param>
        /// <param name="HairIndex"></param>
        /// <param name="EyesIndex"></param>
        /// <param name="NoseIndex"></param>
        /// <param name="MouthIndex"></param>
        public void SetExampleModel(
            Gender Gender = Gender.Male,
            int SkinColorIndex = 0,
            int HairColorIndex = 0,
            int HairIndex = 0,
            int EyesIndex = 0,
            int NoseIndex = 0,
            int MouthIndex = 0)
        {
            gender = Gender;
            skinColor = (skinColors.Length > SkinColorIndex) ? skinColors[SkinColorIndex] : (byte)0;
            hairColor = (hairColors.Length > HairColorIndex) ? hairColors[HairColorIndex] : (byte)0;

            ResourceIDBGF skullID;
            ResourceIDBGF[] hairIDs;
            ResourceIDBGF[] eyeIDs;
            ResourceIDBGF[] noseIDs;
            ResourceIDBGF[] mouthIDs;

            SubOverlay subOvSkull;
            SubOverlay subOvHair;
            SubOverlay subOvEyes;
            SubOverlay subOvNose;
            SubOverlay subOvMouth;

            // 1. select set based on gender
            if (Gender == Gender.Male)
            {
                skullID = maleSkullID;
                hairIDs = maleHairIDs;
                eyeIDs = maleEyeIDs;
                noseIDs = maleNoseIDs;
                mouthIDs = maleMouthIDs;
            }
            else
            {
                skullID = femaleSkullID;
                hairIDs = femaleHairIDs;
                eyeIDs = femaleEyeIDs;
                noseIDs = femaleNoseIDs;
                mouthIDs = femaleMouthIDs;
            }

            // 2. get suboverlays
            subOvSkull = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.HEAD);
            subOvHair = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.HAIR);
            subOvEyes = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.EYES);
            subOvNose = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.NOSE);
            subOvMouth = ExampleModel.GetSubOverlayByHotspot((byte)KnownHotspot.MOUTH);

            // 3. create not yet existing
            if (subOvSkull == null)
            {
                subOvSkull = new SubOverlay();
                subOvSkull.HotSpot = (byte)KnownHotspot.HEAD;
                subOvSkull.ColorTranslation = 4;
                ExampleModel.SubOverlays.Add(subOvSkull);
            }

            if (subOvHair == null)
            {
                subOvHair = new SubOverlay();
                subOvHair.HotSpot = (byte)KnownHotspot.HAIR;
                ExampleModel.SubOverlays.Add(subOvHair);
            }
            
            if (subOvEyes == null)
            {
                subOvEyes = new SubOverlay();
                subOvEyes.HotSpot = (byte)KnownHotspot.EYES;
                ExampleModel.SubOverlays.Add(subOvEyes);
            }
            
            if (subOvNose == null)
            {
                subOvNose = new SubOverlay();
                subOvNose.HotSpot = (byte)KnownHotspot.NOSE;
                ExampleModel.SubOverlays.Add(subOvNose);
            }
            
            if (subOvMouth == null)
            {
                subOvMouth = new SubOverlay();
                subOvMouth.HotSpot = (byte)KnownHotspot.MOUTH;
                ExampleModel.SubOverlays.Add(subOvMouth);
            }

            // 4. set values
            subOvSkull.Name = skullID.Name;
            subOvSkull.ResourceID = skullID.Value;
            subOvSkull.Resource = skullID.Resource;
            subOvSkull.ColorTranslation = (skinColors.Length > SkinColorIndex) ? skinColors[SkinColorIndex] : (byte)0;

            subOvHair.Name = (hairIDs.Length > HairIndex) ? hairIDs[HairIndex].Name : String.Empty;
            subOvHair.ResourceID = (hairIDs.Length > HairIndex) ? hairIDs[HairIndex].Value : 0;
            subOvHair.Resource = (hairIDs.Length > HairIndex) ? hairIDs[HairIndex].Resource : null;
            subOvHair.ColorTranslation = (hairColors.Length > HairColorIndex) ? hairColors[HairColorIndex] : (byte)0;

            subOvEyes.Name = (eyeIDs.Length > EyesIndex) ? eyeIDs[EyesIndex].Name : String.Empty;
            subOvEyes.ResourceID = (eyeIDs.Length > EyesIndex) ? eyeIDs[EyesIndex].Value : 0;
            subOvEyes.Resource = (eyeIDs.Length > EyesIndex) ? eyeIDs[EyesIndex].Resource : null;
            subOvEyes.ColorTranslation = (skinColors.Length > SkinColorIndex) ? skinColors[SkinColorIndex] : (byte)0;

            subOvNose.Name = (noseIDs.Length > NoseIndex) ? noseIDs[NoseIndex].Name : String.Empty;
            subOvNose.ResourceID = (noseIDs.Length > NoseIndex) ? noseIDs[NoseIndex].Value : 0;
            subOvNose.Resource = (noseIDs.Length > NoseIndex) ? noseIDs[NoseIndex].Resource : null;
            subOvNose.ColorTranslation = (skinColors.Length > SkinColorIndex) ? skinColors[SkinColorIndex] : (byte)0;

            subOvMouth.Name = (mouthIDs.Length > MouthIndex) ? mouthIDs[MouthIndex].Name : String.Empty;
            subOvMouth.ResourceID = (mouthIDs.Length > MouthIndex) ? mouthIDs[MouthIndex].Value : 0;
            subOvMouth.Resource = (mouthIDs.Length > MouthIndex) ? mouthIDs[MouthIndex].Resource : null;
            subOvMouth.ColorTranslation = (skinColors.Length > SkinColorIndex) ? skinColors[SkinColorIndex] : (byte)0;

            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GENDER));                
        }
    }
}
