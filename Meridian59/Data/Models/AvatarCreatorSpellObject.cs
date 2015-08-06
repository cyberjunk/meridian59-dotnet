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
    /// Contains information about a selectable spell during avatar creation wizard.
    /// </summary>
    [Serializable]
    public class AvatarCreatorSpellObject : IByteSerializableFast, INotifyPropertyChanged, IClearable, IStringResolvable
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_EXTRAID = "ExtraID";
        public const string PROPNAME_SPELLNAMEID = "SpellNameID";
        public const string PROPNAME_SPELLDESCRIPTIONID = "SpellDescriptionID";
        public const string PROPNAME_SPELLCOST = "SpellCost";
        public const string PROPNAME_SCHOOLTYPE = "SchoolType";
        public const string PROPNAME_SPELLNAME = "SpellName";
        public const string PROPNAME_SPELLDESCRIPTION = "SpellDescription";
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
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.BYTE;
            } 
        }      
        
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(extraID), 0, Buffer, cursor, TypeSizes.INT);               // ExtraID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(spellNameID), 0, Buffer, cursor, TypeSizes.INT);           // SpellNameID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(spellDescriptionID), 0, Buffer, cursor, TypeSizes.INT);    // SpellDescriptionID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(spellCost), 0, Buffer, cursor, TypeSizes.INT);             // SpellCost (4 bytes)
            cursor += TypeSizes.INT;

            Buffer[cursor] = (byte)schoolType;                                                          // SchoolType (1 bytes)           
            cursor++;

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            extraID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            spellNameID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            spellDescriptionID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            spellCost = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            schoolType = (SchoolType)Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = extraID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = spellNameID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = spellDescriptionID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = spellCost;
            Buffer += TypeSizes.INT;

            Buffer[0] = (byte)schoolType;          
            Buffer++;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            extraID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            spellNameID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            spellDescriptionID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            spellCost = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            schoolType = (SchoolType)Buffer[0];
            Buffer++;
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
        protected uint spellNameID;
        protected uint spellDescriptionID;
        protected uint spellCost;
        protected SchoolType schoolType;

        protected string spellName;
        protected string spellDescription;
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

        public uint SpellNameID
        {
            get { return spellNameID; }
            set
            {
                if (spellNameID != value)
                {
                    spellNameID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLNAMEID));
                }
            }
        }

        public uint SpellDescriptionID
        {
            get { return spellDescriptionID; }
            set
            {
                if (spellDescriptionID != value)
                {
                    spellDescriptionID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLDESCRIPTIONID));
                }
            }
        }

        public uint SpellCost
        {
            get { return spellCost; }
            set
            {
                if (spellCost != value)
                {
                    spellCost = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLCOST));
                }
            }
        }

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

        // extended
        public string SpellName
        {
            get { return spellName; }
            set
            {
                if (spellName != value)
                {
                    spellName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLNAME));
                }
            }
        }
        public string SpellDescription
        {
            get { return spellDescription; }
            set
            {
                if (spellDescription != value)
                {
                    spellDescription = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLDESCRIPTION));
                }
            }
        }
        #endregion

        #region Constructors
        public AvatarCreatorSpellObject()
        {
            Clear(false);
        }

        public AvatarCreatorSpellObject(uint ExtraID, uint SpellNameID, uint SpellDescriptionID, uint SpellCost, SchoolType SchoolType)
        {
            this.extraID = ExtraID;
            this.spellNameID = SpellNameID;
            this.spellDescriptionID = SpellDescriptionID;
            this.spellCost = SpellCost;
            this.schoolType = SchoolType;
        }

        public AvatarCreatorSpellObject(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe AvatarCreatorSpellObject(ref byte* Buffer)
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
                SpellNameID = 0;
                SpellDescriptionID = 0;
                SpellCost = 0;
                SchoolType = 0;

                SpellName = String.Empty;
                SpellDescription = String.Empty;
            }
            else
            {
                extraID = 0;
                spellNameID = 0;
                spellDescriptionID = 0;
                spellCost = 0;

                spellName = String.Empty;
                spellDescription = String.Empty;
            }
        }
        #endregion

        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string spell_name;
            string spell_description;

			StringResources.TryGetValue(spellNameID, out spell_name);
			StringResources.TryGetValue(spellDescriptionID, out spell_description);

            if (RaiseChangedEvent)
            {
                if (spell_name != null) SpellName = spell_name;
                else SpellName = String.Empty;

                if (spell_description != null) SpellDescription = spell_description;
                else SpellDescription = String.Empty;
            }
            else
            {
                if (spell_name != null) spellName = spell_name;
                else spellName = String.Empty;

                if (spell_description != null) spellDescription = spell_description;
                else spellDescription = String.Empty;               
            }
        }
        #endregion 
    }
}
