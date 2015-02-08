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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// M59 chat message model
    /// </summary>
    public class ChatMessage : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const char VARIABLEFLAG       = '%';
        public const char INTEGERFLAG        = 'i';
        public const char INTEGERFLAG2       = 'd'; // might be decimal one day
        public const char STRINGRESOURCEFLAG = 's';
        public const char EMBEDDEDSTRINGFLAG = 'q';
        
        public const string PROPNAME_CHATMESSAGETYPE = "ChatMessageType";
        public const string PROPNAME_RESOURCEID = "ResourceID";
        public const string PROPNAME_RESOURCENAME = "ResourceName";
        public const string PROPNAME_FULLSTRING = "FullString";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected ChatMessageType chatMessageType;
        protected uint resourceID;

        protected string resourceName;
        protected string fullString;

        protected LockingDictionary<uint, string> stringResources;      
        #endregion

        #region Properties
        /// <summary>
        /// Attached variables
        /// </summary>
        public List<InlineVariable> Variables { get; protected set; }
        
        /// <summary>
        /// Styles of messageparts.
        /// </summary>
        public List<ChatStyle> Styles { get; protected set; }

        /// <summary>
        /// Type of this chatmessage
        /// </summary>
        public ChatMessageType ChatMessageType
        {
            get { return chatMessageType; }
            set
            {
                if (chatMessageType != value)
                {
                    chatMessageType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CHATMESSAGETYPE));
                }
            }
        }

        /// <summary>
        /// Main ResourceID of the message
        /// </summary>
        public uint ResourceID
        {
            get { return resourceID; }
            set
            {
                if (resourceID != value)
                {
                    resourceID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEID));
                }
            }
        }

        /// <summary>
        /// Main string
        /// </summary>
        public string ResourceName
        {
            get { return resourceName; }
            set
            {
                if (resourceName != value)
                {
                    resourceName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCENAME));
                }
            }
        }

        /// <summary>
        /// Resolved and constructed full string.
        /// </summary>
        public string FullString
        {
            get { return fullString; }
            set
            {
                fullString = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FULLSTRING));
            }
        }
        #endregion

        #region IByteSerializable
        public virtual int ByteLength {
            get {
                int len = TypeSizes.INT;

                foreach (InlineVariable obj in Variables)
                    len += obj.ByteLength;

                return len;
            }
        }     
        public virtual int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(resourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            foreach (InlineVariable obj in Variables)
                cursor += obj.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }
        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            resourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            string resourceName;
            stringResources.TryGetValue(resourceID, out resourceName);
            this.resourceName = resourceName;

            // start filling up variables (example: %s) with values
            // their values are attached to the packet/buffer and make it a variable length
            // also there might be more than one iteration necessary as varibles may be nested into variable-strings
            fullString = String.Copy(resourceName);

            int index = HasVariable(fullString);
            while (index > -1)
            {                              
                // which type of inline var
                switch (fullString[index + 1])
                {
                    case INTEGERFLAG:
                    case INTEGERFLAG2:
                        InlineVariable var1 = new InlineVariable(InlineVariableType.Integer, Buffer, cursor);
                        cursor += var1.ByteLength;

                        Variables.Add(var1);
                        int j = (int)var1.Data;

                        // remove the %i and insert the integer
                        fullString = fullString.Remove(index, 2);
                        fullString = fullString.Insert(index, j.ToString());
                        break;

                    case EMBEDDEDSTRINGFLAG:
                        InlineVariable var2 = new InlineVariable(InlineVariableType.String, Buffer, cursor);
                        cursor += var2.ByteLength;

                        Variables.Add(var2);                             
                        string s = (string)var2.Data;

                        // remove the %q and insert the string
                        fullString = fullString.Remove(index, 2);
                        fullString = fullString.Insert(index, s);                      
                        break;

                    case STRINGRESOURCEFLAG:
                        InlineVariable var3 = new InlineVariable(InlineVariableType.Resource, Buffer, cursor);
                        cursor += var3.ByteLength;

                        Variables.Add(var3);
                        uint resourceid = (uint)var3.Data;

                        // try to get it from strings
                        string resourcestr;
                        stringResources.TryGetValue(resourceid, out resourcestr);
                                                               
                        // still null? use dummy
                        if (resourcestr == null)
                            resourcestr = String.Empty;

                        // remove the %s and insert the resourcestr
                        fullString = fullString.Remove(index, 2);
                        fullString = fullString.Insert(index, resourcestr);
                        break;

                    default:
                        break;
                }
                               
                // check if there is more inline vars (may start loop again)
                index = HasVariable(fullString);
            }

            // extract and remove the inline styles (~B ...)
            Styles = ChatStyle.GetStyles(fullString, chatMessageType);
            fullString = ChatStyle.RemoveInlineStyles(fullString);

            return cursor - StartIndex;
        }
        public unsafe virtual void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = resourceID;
            Buffer += TypeSizes.INT;

            foreach (InlineVariable obj in Variables)
                obj.WriteTo(ref Buffer);
        }
        public unsafe virtual void ReadFrom(ref byte* Buffer)
        {
            resourceID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            string resourceName;
            stringResources.TryGetValue(resourceID, out resourceName);
            this.resourceName = resourceName;

            // start filling up variables (example: %s) with values
            // their values are attached to the packet/buffer and make it a variable length
            // also there might be more than one iteration necessary as varibles may be nested into variable-strings
            fullString = String.Copy(resourceName);

            int index = HasVariable(fullString);
            while (index > -1)
            {
                // which type of inline var
                switch (fullString[index + 1])
                {
                    case INTEGERFLAG:
                    case INTEGERFLAG2:
                        InlineVariable var1 = new InlineVariable(InlineVariableType.Integer, ref Buffer);
                        
                        Variables.Add(var1);
                        int j = (int)var1.Data;

                        // remove the %i and insert the integer
                        fullString = fullString.Remove(index, 2);
                        fullString = fullString.Insert(index, j.ToString());
                        break;

                    case EMBEDDEDSTRINGFLAG:
                        InlineVariable var2 = new InlineVariable(InlineVariableType.String, ref Buffer);
                        
                        Variables.Add(var2);
                        string s = (string)var2.Data;

                        // remove the %q and insert the string
                        fullString = fullString.Remove(index, 2);
                        fullString = fullString.Insert(index, s);
                        break;

                    case STRINGRESOURCEFLAG:
                        InlineVariable var3 = new InlineVariable(InlineVariableType.Resource, ref Buffer);
                        
                        Variables.Add(var3);
                        uint resourceid = (uint)var3.Data;

                        // try to get it from strings
                        string resourcestr;
                        stringResources.TryGetValue(resourceid, out resourcestr);

                        // still null? use dummy
                        if (resourcestr == null)
                            resourcestr = String.Empty;

                        // remove the %s and insert the resourcestr
                        fullString = fullString.Remove(index, 2);
                        fullString = fullString.Insert(index, resourcestr);
                        break;

                    default:
                        break;
                }

                // check if there is more inline vars (may start loop again)
                index = HasVariable(fullString);
            }

            // extract and remove the inline styles (~B ...)
            Styles = ChatStyle.GetStyles(fullString, chatMessageType);
            fullString = ChatStyle.RemoveInlineStyles(fullString);
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
      
        #region Constructors

        public ChatMessage()
        {
            this.chatMessageType = ChatMessageType.SystemMessage;
            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();

            Clear(false);
        }

        public ChatMessage(ChatMessageType MessageType)
        {
            this.chatMessageType = MessageType;
            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();

            Clear(false);
        }

        public ChatMessage(
            ChatMessageType MessageType, 
            LockingDictionary<uint, string> StringResources, 
            uint ResourceID, 
            List<InlineVariable> Variables,
            List<ChatStyle> Styles)
        {
            this.chatMessageType = MessageType;            
            this.stringResources = StringResources;
            this.Variables = Variables;
            this.Styles = Styles;

            this.resourceID = ResourceID;       
        }

        public ChatMessage(
            ChatMessageType MessageType, 
            LockingDictionary<uint, string> StringResources, 
            byte[] Buffer, 
            int StartIndex = 0)
        {
            this.chatMessageType = MessageType;
            this.stringResources = StringResources;
                     
            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe ChatMessage(
            ChatMessageType MessageType, 
            LockingDictionary<uint, string> StringResources,
            ref byte* Buffer) 
        {
            this.chatMessageType = MessageType;
            this.stringResources = StringResources;

            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();
            ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks whether a string still contains an inline variable
        /// </summary>
        /// <param name="String">string to check</param>
        /// <returns>index of the next inline variable (-1 if none found)</returns>
        protected int HasVariable(string String)
        {
            for (int i = 0; i < String.Length - 1; i++)
            {
                if (String[i] == VARIABLEFLAG &&
                    (
                    String[i + 1] == INTEGERFLAG ||
                    String[i + 1] == INTEGERFLAG2 || 
                    String[i + 1] == EMBEDDEDSTRINGFLAG || 
                    String[i + 1] == STRINGRESOURCEFLAG
                    ))

                    return i;
            }

            return -1;
        }

        public override string ToString()
        {
            return fullString;
        }

        /// <summary>
        /// Creates a local ChatMessage instance for given text and
        /// style settings.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="IsBold"></param>
        /// <param name="IsCursive"></param>
        /// <param name="IsUnderline"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static ChatMessage GetChatMessageForString(
            string Text, 
            bool IsBold = false, 
            bool IsCursive = false, 
            bool IsUnderline = false,
            ChatColor Color = ChatColor.Red)
        {         
            ChatStyle style = new ChatStyle(
                0, Text.Length, IsBold, IsCursive, IsUnderline, Color);
            
            ChatMessage message = new ChatMessage();
            message.FullString = Text;
            message.Styles.Add(style);
                
            return message;
        }
        #endregion

        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ResourceID = 0;
                ResourceName = String.Empty;
                FullString = String.Empty;
                Variables.Clear();
                Styles.Clear();
            }
            else
            {
                resourceID = 0;
                resourceName = String.Empty;
                fullString = String.Empty;
                Variables.Clear();
                Styles.Clear();
            }
        }
        #endregion
    }
}
