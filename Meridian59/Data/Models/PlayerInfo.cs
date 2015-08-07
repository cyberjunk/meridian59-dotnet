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
using System.Text;
using System.ComponentModel;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A set of information about a player object (when requesting to look at)
    /// </summary>
    [Serializable]
    public class PlayerInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<PlayerInfo>
    {        
        #region Constants
        public const string PROPNAME_OBJECTBASE = "ObjectBase";
        public const string PROPNAME_ISEDITABLE = "IsEditable";
        public const string PROPNAME_MESSAGE = "Message";
        public const string PROPNAME_TITLES = "Titles";
        public const string PROPNAME_WEBSITE = "Website";
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
                return objectBase.ByteLength + TypeSizes.BYTE + message.ByteLength
#if !VANILLA
					+ titles.ByteLength
#else
                    + TypeSizes.SHORT + titles.Length
#endif
					+ TypeSizes.SHORT + website.Length;
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
			ushort strlen;

            objectBase = new ObjectBase(true, Buffer, cursor);
            cursor += objectBase.ByteLength;

            isEditable = Convert.ToBoolean(Buffer[cursor]);
            cursor++;

            message = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += Message.ByteLength;

#if !VANILLA
			titles = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
			cursor += titles.ByteLength;
#else
            strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Titles = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen;
#endif
            strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Website = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += objectBase.WriteTo(Buffer, cursor);

            Buffer[cursor] = Convert.ToByte(IsEditable);
            cursor++;

            cursor += message.WriteTo(Buffer, cursor);

#if !VANILLA
			cursor += titles.WriteTo(Buffer, cursor);
#else
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Titles.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(Titles), 0, Buffer, cursor, Titles.Length);
            cursor += Titles.Length;
#endif
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Website.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(Website), 0, Buffer, cursor, Website.Length);
            cursor += Website.Length;
            
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
        protected bool isEditable;
        protected ServerString message;
#if !VANILLA
		protected ServerString titles;
#else
        protected string titles;
#endif
		protected string website;
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

        public bool IsEditable
        {
            get
            {
                return isEditable;
            }
            set
            {
                if (isEditable != value)
                {
                    isEditable = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISEDITABLE));
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
#if !VANILLA
		public ServerString Titles
		{
			get
			{
				return titles;
			}
			set
			{
				if (titles != value)
				{
					titles = value;
					RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TITLES));
				}
			}
		}
#else
        public string Titles
        {
            get
            {
                return titles;
            }
            set
            {
                if (titles != value)
                {
                    titles = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TITLES));
                }
            }
        }
#endif
        public string Website
        {
            get
            {
                return website;
            }
            set
            {
                if (website != value)
                {
                    website = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_WEBSITE));
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
        public PlayerInfo()
        {
            Clear(false);
        }
#if !VANILLA
		public PlayerInfo(ObjectBase ObjectBase, bool IsEditable, ServerString Message, ServerString Titles, string Website)
		{
			objectBase = ObjectBase;
			isEditable = IsEditable;
			message = Message;
			titles = Titles;
			website = Website;
		}
#else
        public PlayerInfo(ObjectBase ObjectBase, bool IsEditable, ServerString Message, string Titles, string Website)
        {
            objectBase = ObjectBase;
            isEditable = IsEditable;
            message = Message;
            titles = Titles;
            website = Website;
        }
#endif
		public PlayerInfo(StringDictionary StringResources, byte[] Buffer, int StartIndex = 0) 
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
                IsEditable = false;
                Message = new ServerString();
#if !VANILLA
				Titles = new ServerString();
#else
                Titles = String.Empty;
#endif
                Website = String.Empty;
                IsVisible = false;
            }
            else
            {
                objectBase = new ObjectBase();
                isEditable = false;
                message = new ServerString();
#if !VANILLA
				titles = new ServerString();
#else
                titles = String.Empty;
#endif
                website = String.Empty;
                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(PlayerInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ObjectBase = Model.ObjectBase;
                IsEditable = Model.IsEditable;
                Message = Model.Message;
                Titles = Model.Titles;
                Website = Model.Website;
                // no visible update
            }
            else
            {
                objectBase = Model.ObjectBase;
                isEditable = Model.IsEditable;
                message = Model.Message;
                titles = Model.Titles;
                website = Model.Website;
                // no visible update
            }
        }
        #endregion
    }
}
