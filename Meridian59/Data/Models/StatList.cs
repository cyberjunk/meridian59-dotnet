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
using Meridian59.Files;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Stat data list with your spells and skills.
    /// </summary>
    [Serializable]
    public class StatList : Stat, IUpdatable<StatList>
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_OBJECTID = "ObjectID";
        public const string PROPNAME_SKILLPOINTS = "SkillPoints";
        public const string PROPNAME_RESOURCEICONID = "ResourceIconID";

        public const string PROPNAME_RESOURCEICONNAME = "ResourceIconName";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                // 1 + 4 + 1 + 4 + 4 + 4
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;
            }
        }      
 
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
          
            Buffer[cursor] = (byte)Type;
            cursor++;

            Array.Copy(BitConverter.GetBytes(objectID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(skillPoints), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(resourceIconID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            // skip type
            cursor++;

            objectID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            skillPoints = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            resourceIconID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            Buffer[0] = (byte)Type;
            Buffer++;

            *((uint*)Buffer) = objectID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = skillPoints;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = resourceIconID;
            Buffer += TypeSizes.INT;            
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            // skip type
            Buffer++;

            objectID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            skillPoints = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            resourceIconID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;
        }        
        #endregion

        #region Fields
        protected uint objectID;
        protected uint skillPoints;
        protected uint resourceIconID;
        protected string resourceIconName;
        #endregion

        #region Properties
        /// <summary>
        /// The type of the stat
        /// </summary>
        public override StatType Type
        {
            get { return StatType.List; }
            
        }

        /// <summary>
        /// ID of the stat on the server.
        /// </summary>
        public uint ObjectID
        {
            get { return objectID; }
            set
            {
                if (objectID != value)
                {
                    objectID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_OBJECTID));
                }
            }
        }

        /// <summary>
        /// SkillPoints
        /// </summary>
        public uint SkillPoints
        {
            get { return skillPoints; }
            set
            {
                if (skillPoints != value)
                {
                    skillPoints = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLPOINTS));
                }
            }
        }

        /// <summary>
        /// Unique resource ID of the icon.
        /// </summary>
        public uint ResourceIconID
        {
            get { return resourceIconID; }
            set
            {
                if (resourceIconID != value)
                {
                    resourceIconID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEICONID));
                }
            }
        }

        /// <summary>
        /// Name of the resource icon.
        /// Set in ResolveStrings()
        /// </summary>     
        public string ResourceIconName
        {
            get { return resourceIconName; }
            set
            {
                if (resourceIconName != value)
                {
                    resourceIconName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEICONNAME));
                }
            }
        }        
        #endregion

        #region Constructors
        public StatList()
        {
            Clear(false);
        }

        public StatList(byte Num)
            : base(Num) { }

        public StatList(byte Num, uint ResourceNameID, uint ObjectID, uint SkillPoints, uint ResourceIconID)
            : base(Num, ResourceNameID)
        {
            this.objectID = ObjectID;
            this.skillPoints = SkillPoints;
            this.resourceIconID = ResourceIconID;
        }

        public StatList(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe StatList(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                ObjectID = 0;
                SkillPoints = 0;
                ResourceIconID = 0;
                ResourceIconName = String.Empty;
            }
            else
            {
                objectID = 0;
                skillPoints = 0;
                resourceIconID = 0;
                resourceIconName = String.Empty;
            } 
        }
        #endregion

        #region IStringResolvable
		public override void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            base.ResolveStrings(StringResources, RaiseChangedEvent);

            string res_icon;

			StringResources.TryGetValue(resourceIconID, out res_icon);

            if (RaiseChangedEvent)
            {           
                if (res_icon != null) ResourceIconName = res_icon;
                else ResourceIconName = String.Empty;
            }
            else
            {          
                if (res_icon != null) resourceIconName = res_icon;
                else resourceIconName = String.Empty;
            }
        }
        #endregion

        #region IResourceResolvable
        public override void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (resourceIconName != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    Resource = M59ResourceManager.GetObject(resourceIconName);
                }
                else
                {
                    resource = M59ResourceManager.GetObject(resourceIconName);
                }
            }
        }
        #endregion
        
        #region IUpdatable
        public void UpdateFromModel(StatList Model, bool RaiseChangedEvent)
        {
            base.UpdateFromModel(Model, RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                ObjectID = Model.ObjectID;
                SkillPoints = Model.SkillPoints;
                ResourceIconID = Model.ResourceIconID;                              
                ResourceIconName = Model.ResourceIconName;
            }
            else
            {
                objectID = Model.ObjectID;
                skillPoints = Model.SkillPoints;
                resourceIconID = Model.ResourceIconID;               
                resourceIconName = Model.ResourceIconName;
            }
        }
        #endregion
    }
}
