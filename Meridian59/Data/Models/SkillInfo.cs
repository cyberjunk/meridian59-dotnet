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
    /// A set of information about a spell object (when requesting to look at)
    /// </summary>
    [Serializable]
    public class SkillInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<SkillInfo>
    {        
        #region Constants
        public const string PROPNAME_OBJECTBASE = "ObjectBase";
        public const string PROPNAME_MESSAGE = "Message";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        public const string PROPNAME_SCHOOLNAME = "SchoolName";
        public const string PROPNAME_SKILLLEVEL = "SkillLevel";
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
                return objectBase.ByteLength
                    + TypeSizes.BYTE + schoolName.ByteLength
                    + TypeSizes.BYTE + skillLevel.ByteLength
                    + TypeSizes.BYTE + message.ByteLength;
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
           
            objectBase = new ObjectBase(true, Buffer, cursor);
            cursor += objectBase.ByteLength;

            schoolName = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += SchoolName.ByteLength;
            skillLevel = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += SkillLevel.ByteLength;
            message = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += Message.ByteLength;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += objectBase.WriteTo(Buffer, cursor);
            cursor += schoolName.WriteTo(Buffer, cursor);
            cursor += skillLevel.WriteTo(Buffer, cursor);
            cursor += message.WriteTo(Buffer, cursor);

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
        protected ServerString message;
        protected ServerString schoolName;
        protected ServerString skillLevel;
        protected bool isVisible;

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

        public ServerString Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message != value)
                {
                    message = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MESSAGE));
                }
            }
        }

        public ServerString SchoolName
        {
            get
            {
                return schoolName;
            }
            set
            {
                if (schoolName != value)
                {
                    schoolName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SCHOOLNAME));
                }
            }
        }

        public ServerString SkillLevel
        {
            get
            {
                return skillLevel;
            }
            set
            {
                if (skillLevel != value)
                {
                    skillLevel = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SKILLLEVEL));
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
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
        public SkillInfo()
        {
            Clear(false);
        }

        public SkillInfo(ObjectBase ObjectBase, ServerString Message, ServerString SchoolName,
            ServerString SkillLevel, ServerString ManaCost, ServerString VigorCost)
        {
            objectBase = ObjectBase;
            message = Message;
            schoolName = SchoolName;
            skillLevel = SkillLevel;
        }

		public SkillInfo(StringDictionary StringResources, byte[] Buffer, int StartIndex = 0) 
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
                Message = new ServerString();
                SchoolName = new ServerString();
                SkillLevel = new ServerString();
                IsVisible = false;
            }
            else
            {
                objectBase = new ObjectBase();
                message = new ServerString();
                schoolName = new ServerString();
                skillLevel = new ServerString();
                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(SkillInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Message = Model.Message;
                SchoolName = Model.SchoolName;
                SkillLevel = Model.SkillLevel;
                ObjectBase = Model.ObjectBase;
                // don't isvisible
            }
            else
            {
                message = Model.Message;
                schoolName = Model.SchoolName;
                skillLevel = Model.SkillLevel;
                objectBase = Model.ObjectBase;
                // don't isvisible
            }
        }
        #endregion
    }
}
