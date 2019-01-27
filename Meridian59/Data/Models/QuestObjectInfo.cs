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
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A set of information about a quest object for NPC quest UI (object + 2 serverstring fields)
    /// </summary>
    [Serializable]
    public class QuestObjectInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<QuestObjectInfo>
    {
        #region Constants
        public const string PROPNAME_OBJECTBASE = "ObjectBase";
        public const string PROPNAME_DESCRIPTION = "Description";
        public const string PROPNAME_REQUIREMENTS = "Requirements";
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
                return objectBase.ByteLength + description.ByteLength + requirements.ByteLength;
            }
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            objectBase = new ObjectBase(true, Buffer, cursor);
            cursor += objectBase.ByteLength;

            description = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += Description.ByteLength;

            requirements = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += Requirements.ByteLength;

            return cursor - StartIndex;
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += objectBase.WriteTo(Buffer, cursor);

            cursor += description.WriteTo(Buffer, cursor);
            cursor += requirements.WriteTo(Buffer, cursor);

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
        protected ObjectBase objectBase;
        protected ServerString description;
        protected ServerString requirements;

        protected StringDictionary stringResources;
        #endregion

        #region Properties
        public ObjectBase ObjectBase
        {
            get
            {
                return objectBase;
            }
            set
            {
                if (objectBase != value)
                {
                    objectBase = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_OBJECTBASE));
                }
            }
        }

        /// <summary>
        /// String for quest description
        /// </summary>
        public ServerString Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DESCRIPTION));
                }
            }
        }

        /// <summary>
        /// String for requirements (or quest instructions)
        /// </summary>
        public ServerString Requirements
        {
            get { return requirements; }
            set
            {
                if (requirements != value)
                {
                    requirements = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_REQUIREMENTS));
                }
            }
        }
        #endregion

        #region Constructors
        public QuestObjectInfo()
        {
            Clear(false);
        }

        public QuestObjectInfo(ObjectBase ObjectBase, ServerString Description, ServerString Requirements)
        {
            objectBase = ObjectBase;
            description = Description;
            requirements = Requirements;
        }

        public QuestObjectInfo(StringDictionary StringResources, byte[] Buffer, int StartIndex = 0)
        {
            stringResources = StringResources;

            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ObjectBase = new ObjectBase();
                Description = new ServerString();
                Requirements = new ServerString();
            }
            else
            {
                objectBase = new ObjectBase();
                description = new ServerString();
                requirements = new ServerString();
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(QuestObjectInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ObjectBase = Model.ObjectBase;
                Description = new ServerString();
                Requirements = new ServerString();
            }
            else
            {
                objectBase = Model.ObjectBase;
                description = new ServerString();
                requirements = new ServerString();
            }
        }
        #endregion
    }
}
