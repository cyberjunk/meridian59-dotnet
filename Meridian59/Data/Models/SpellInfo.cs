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
    public class SpellInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<SpellInfo>
    {        
        #region Constants
        public const string PROPNAME_OBJECTBASE = "ObjectBase";
        public const string PROPNAME_MESSAGE = "Message";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        public const string PROPNAME_SCHOOLNAME = "SchoolName";
        public const string PROPNAME_SPELLLEVEL = "SpellLevel";
        public const string PROPNAME_MANACOST = "ManaCost";
        public const string PROPNAME_VIGORCOST = "VigorCost";
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
                    + TypeSizes.BYTE + spellLevel.ByteLength
                    + TypeSizes.BYTE + manaCost.ByteLength
                    + TypeSizes.BYTE + vigorCost.ByteLength
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
            spellLevel = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += SpellLevel.ByteLength;
            manaCost = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += ManaCost.ByteLength;
            vigorCost = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += VigorCost.ByteLength;
            message = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += Message.ByteLength;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += objectBase.WriteTo(Buffer, cursor);
            cursor += schoolName.WriteTo(Buffer, cursor);
            cursor += spellLevel.WriteTo(Buffer, cursor);
            cursor += manaCost.WriteTo(Buffer, cursor);
            cursor += vigorCost.WriteTo(Buffer, cursor);
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
        protected ServerString spellLevel;
        protected ServerString manaCost;
        protected ServerString vigorCost;
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

        public ServerString SpellLevel
        {
            get
            {
                return spellLevel;
            }
            set
            {
                if (spellLevel != value)
                {
                    spellLevel = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPELLLEVEL));
                }
            }
        }

        public ServerString ManaCost
        {
            get
            {
                return manaCost;
            }
            set
            {
                if (manaCost != value)
                {
                    manaCost = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MANACOST));
                }
            }
        }

        public ServerString VigorCost
        {
            get
            {
                return vigorCost;
            }
            set
            {
                if (vigorCost != value)
                {
                    vigorCost = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VIGORCOST));
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
        public SpellInfo()
        {
            Clear(false);
        }

        public SpellInfo(ObjectBase ObjectBase, ServerString Message, ServerString SchoolName,
            ServerString SpellLevel, ServerString ManaCost, ServerString VigorCost)
        {
            objectBase = ObjectBase;
            message = Message;
            schoolName = SchoolName;
            spellLevel = SpellLevel;
            manaCost = ManaCost;
            vigorCost = VigorCost;
        }

		public SpellInfo(StringDictionary StringResources, byte[] Buffer, int StartIndex = 0) 
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
                SpellLevel = new ServerString();
                ManaCost = new ServerString();
                VigorCost = new ServerString();
                IsVisible = false;
            }
            else
            {
                objectBase = new ObjectBase();
                message = new ServerString();
                schoolName = new ServerString();
                spellLevel = new ServerString();
                manaCost = new ServerString();
                vigorCost = new ServerString();
                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(SpellInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Message = Model.Message;
                SchoolName = Model.SchoolName;
                SpellLevel = Model.SpellLevel;
                ManaCost = Model.ManaCost;
                VigorCost = Model.VigorCost;
                ObjectBase = Model.ObjectBase;
                // don't isvisible
            }
            else
            {
                message = Model.Message;
                schoolName = Model.SchoolName;
                spellLevel = Model.SpellLevel;
                manaCost = Model.ManaCost;
                vigorCost = Model.VigorCost;
                objectBase = Model.ObjectBase;
                // don't isvisible
            }
        }
        #endregion
    }
}
