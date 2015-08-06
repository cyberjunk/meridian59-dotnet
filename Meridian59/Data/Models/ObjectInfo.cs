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
    /// A set of information about a nonplayer object (when requesting to look at)
    /// </summary>
    [Serializable]
    public class ObjectInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<ObjectInfo>
    {        
        #region Constants
        public const string PROPNAME_OBJECTBASE = "ObjectBase";
        public const string PROPNAME_LOOKTYPE = "LookType";
        public const string PROPNAME_MESSAGE = "Message";
        public const string PROPNAME_INSCRIPTION = "Inscription";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
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
                int len = objectBase.ByteLength + TypeSizes.BYTE + message.ByteLength;

                if (LookType.IsEditable || LookType.IsInscribed)               
                    len += Inscription.ByteLength;
                
                return len;
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
           
            objectBase = new ObjectBase(true, Buffer, cursor);
            cursor += objectBase.ByteLength;

            lookType = new LookTypeFlags(Buffer[cursor]);
            cursor++;

            message = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += Message.ByteLength;

            // if there is an inscription, additionally read it
            if (LookType.IsEditable || LookType.IsInscribed)
            {
                inscription = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
                cursor += Inscription.ByteLength;
            }

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += objectBase.WriteTo(Buffer, cursor);

            Buffer[cursor] = (byte)LookType.Flags;
            cursor++;

            cursor += message.WriteTo(Buffer, cursor);

            // if there is an inscription, additionally write it
            if (LookType.IsEditable || LookType.IsInscribed)                    
                cursor += Inscription.WriteTo(Buffer, cursor);
            
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
        protected LookTypeFlags lookType;
        protected ServerString message;
        protected ServerString inscription;
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

        public LookTypeFlags LookType
        {
            get
            {
                return lookType;
            }
            set
            {
                if (lookType != value)
                {
                    lookType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LOOKTYPE));
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

        public ServerString Inscription
        {
            get
            {
                return inscription;
            }
            set
            {
                if (inscription != value)
                {
                    inscription = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_INSCRIPTION));
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
        public ObjectInfo()
        {
            Clear(false);
        }

        public ObjectInfo(ObjectBase ObjectBase, LookTypeFlags LookType, ServerString Message, ServerString Inscription)
        {
            objectBase = ObjectBase;
            lookType = LookType;
            message = Message;
            inscription = Inscription;
        }

		public ObjectInfo(StringDictionary StringResources, byte[] Buffer, int StartIndex = 0) 
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
                LookType = new LookTypeFlags();
                Message = new ServerString();
                Inscription = null;
                IsVisible = false;
            }
            else
            {
                objectBase = new ObjectBase();
                lookType = new LookTypeFlags();
                message = new ServerString();
                inscription = null;
                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(ObjectInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ObjectBase = Model.ObjectBase;
                LookType = Model.LookType;
                Message = Model.Message;
                Inscription = Model.Inscription;
                // don't isvisible
            }
            else
            {
                objectBase = Model.ObjectBase;
                lookType = Model.LookType;
                message = Model.Message;
                inscription = Model.Inscription;
                // don't isvisible
            }
        }
        #endregion
    }
}
