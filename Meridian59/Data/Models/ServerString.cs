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
    public class ServerString : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const char VARIABLEFLAG                  = '%';
		public const char ORDERFLAG						= '$';
        public const char INTEGERFLAG                   = 'i';
        public const char INTEGERFLAG2                  = 'd';
        public const char STRINGRESOURCELITERALFLAG     = 's';
        public const char EMBEDDEDSTRINGLITERALFLAG     = 'q';
#if !VANILLA        
        public const char STRINGRESOURCERECURSIVEFLAG   = 'r';
#endif        
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

		protected StringDictionary stringResources;      
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

            InlineVariable var;
            string strval;
			string tempstr;
            int intval;
            uint uintval;
            int index;
            
			// read root resource ID
            resourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

			// This is the parser-iteration in which we go through the string-building process,
			// but only do enough of it to get the info we need to read parameters from buffer.
			// Parameters are ordered on the buffer like they appear in ENGLISH.
			// Todo: Change serverside to specify amount and type of parameters to be read.

			// get root resource-string in english
			if (!stringResources.TryGetValue(resourceID, out tempstr, LanguageCode.English))
				tempstr = String.Empty;

			// string building process
			index = HasVariable(tempstr);
			while (index > -1)
			{
				while (index > -1)
				{
					// which type of inline var
					switch (tempstr[index + 1])
					{
						case INTEGERFLAG:
						case INTEGERFLAG2:
							var = new InlineVariable(InlineVariableType.Integer, Buffer, cursor);
							intval = (int)var.Data;
							cursor += var.ByteLength;

							Variables.Add(var);
							
							// remove the %i, %d, no handling pass 1
							tempstr = tempstr.Remove(index, 2);						
							break;

						case EMBEDDEDSTRINGLITERALFLAG:
							var = new InlineVariable(InlineVariableType.String, Buffer, cursor);
							strval = (string)var.Data;
							cursor += var.ByteLength;

							Variables.Add(var);

							// remove the %q, no handling in pass 1
							tempstr = tempstr.Remove(index, 2);
							break;

						case STRINGRESOURCELITERALFLAG:
							var = new InlineVariable(InlineVariableType.Resource, Buffer, cursor);
							uintval = (uint)var.Data;
							cursor += var.ByteLength;

							Variables.Add(var);

							if (!stringResources.TryGetValue(uintval, out strval, LanguageCode.English))
								strval = String.Empty;

							// remove the %s, and insert the string,
							// but skip its content in this inner while()                     
							tempstr = tempstr.Remove(index, 2);
							tempstr = tempstr.Insert(index, strval);

							// skip
							index += strval.Length;
							break;

#if !VANILLA
						case STRINGRESOURCERECURSIVEFLAG:
							var = new InlineVariable(InlineVariableType.Resource, Buffer, cursor);
							uintval = (uint)var.Data;
							cursor += var.ByteLength;

							Variables.Add(var);

							if (!stringResources.TryGetValue(uintval, out strval, LanguageCode.English))
								strval = String.Empty;

							// remove the %r, insert the content (so we process it next)
							tempstr = tempstr.Remove(index, 2);
							tempstr = tempstr.Insert(index, strval);
							break;
#endif
						default:
							break;
					}

					// see if there is more inline vars to the right of current index
					// this makes sure we don't yet process nested and already inserted %s
					index = HasVariable(tempstr, index);
				}

				// start from the beginning again, this will process nested %s
				index = HasVariable(tempstr);
			}

			// now build the user-language based FullString using parameters
			BuildString();

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
			InlineVariable var;
			string strval;
			string tempstr;
			int intval;
			uint uintval;
			int index;

			// read root resource ID
			resourceID = *((uint*)Buffer);
			Buffer += TypeSizes.INT;

			// This is the parser-iteration in which we go through the string-building process,
			// but only do enough of it to get the info we need to read parameters from buffer.
			// Parameters are ordered on the buffer like they appear in ENGLISH.
			// Todo: Change serverside to specify amount and type of parameters to be read.

			// get root resource-string in english
			if (!stringResources.TryGetValue(resourceID, out tempstr, LanguageCode.English))
				tempstr = String.Empty;

			// string building process
			index = HasVariable(tempstr);
			while (index > -1)
			{
				while (index > -1)
				{
					// which type of inline var
					switch (tempstr[index + 1])
					{
						case INTEGERFLAG:
						case INTEGERFLAG2:
							var = new InlineVariable(InlineVariableType.Integer, ref Buffer);
							intval = (int)var.Data;

							Variables.Add(var);

							// remove the %i, %d, no handling pass 1
							tempstr = tempstr.Remove(index, 2);
							break;

						case EMBEDDEDSTRINGLITERALFLAG:
							var = new InlineVariable(InlineVariableType.String, ref Buffer);
							strval = (string)var.Data;

							Variables.Add(var);

							// remove the %q, no handling in pass 1
							tempstr = tempstr.Remove(index, 2);
							break;

						case STRINGRESOURCELITERALFLAG:
							var = new InlineVariable(InlineVariableType.Resource, ref Buffer);
							uintval = (uint)var.Data;

							Variables.Add(var);

							if (!stringResources.TryGetValue(uintval, out strval, LanguageCode.English))
								strval = String.Empty;

							// remove the %s, and insert the string,
							// but skip its content in this inner while()                     
							tempstr = tempstr.Remove(index, 2);
							tempstr = tempstr.Insert(index, strval);

							// skip
							index += strval.Length;
							break;

#if !VANILLA
						case STRINGRESOURCERECURSIVEFLAG:
							var = new InlineVariable(InlineVariableType.Resource, ref Buffer);
							uintval = (uint)var.Data;

							Variables.Add(var);

							if (!stringResources.TryGetValue(uintval, out strval, LanguageCode.English))
								strval = String.Empty;

							// remove the %r, insert the content (so we process it next)
							tempstr = tempstr.Remove(index, 2);
							tempstr = tempstr.Insert(index, strval);
							break;
#endif
						default:
							break;
					}

					// see if there is more inline vars to the right of current index
					// this makes sure we don't yet process nested and already inserted %s
					index = HasVariable(tempstr, index);
				}

				// start from the beginning again, this will process nested %s
				index = HasVariable(tempstr);
			}

			// now build the user-language based FullString using parameters
			BuildString();
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

        public ServerString()
        {
            this.chatMessageType = ChatMessageType.SystemMessage;
            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();

            Clear(false);
        }

        public ServerString(ChatMessageType MessageType)
        {
            this.chatMessageType = MessageType;
            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();

            Clear(false);
        }

        public ServerString(
            ChatMessageType MessageType,
			StringDictionary StringResources, 
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

        public ServerString(
            ChatMessageType MessageType,
			StringDictionary StringResources, 
            byte[] Buffer, 
            int StartIndex = 0)
        {
            this.chatMessageType = MessageType;
            this.stringResources = StringResources;
                     
            Variables = new List<InlineVariable>();
            Styles = new List<ChatStyle>();
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe ServerString(
            ChatMessageType MessageType,
			StringDictionary StringResources,
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
        /// <param name="StartIndex"></param>
        /// <returns>index of the next inline variable (-1 if none found)</returns>
        protected int HasVariable(string String, int StartIndex = 0)
        {
            // turn neg. indices to 0
            StartIndex = Math.Max(0, StartIndex);

            for (int i = StartIndex; i < String.Length - 1; i++)
            {
                if (String[i] == VARIABLEFLAG &&
                    (
                    String[i + 1] == INTEGERFLAG ||
                    String[i + 1] == INTEGERFLAG2 || 
                    String[i + 1] == EMBEDDEDSTRINGLITERALFLAG || 
                    String[i + 1] == STRINGRESOURCELITERALFLAG
#if !VANILLA
                    || String[i + 1] == STRINGRESOURCERECURSIVEFLAG
#endif
                    ))

                    return i;
            }

            return -1;
        }

		/// <summary>
		/// Builds the 'FullString' property based on 
		/// the available parameters in 'Variables',
		/// the root resource id and the user-language dependent strings.
		/// </summary>
		protected void BuildString()
		{
			int index, varindex;
			InlineVariable var;
			string strval;
			int intval;
			uint uintval;
			List<int> q_indices = new List<int>();
			List<string> q_strings = new List<string>();
			
			// In this iteration we will use the parsed 'Variables' to actually build
			// the string in the user-lanuage, honoring the $ordering.

			// resolve root resource string in user-language
			if (!stringResources.TryGetValue(resourceID, out resourceName))
				resourceName = String.Empty;

			// will store fully constructed string
			fullString = String.Copy(resourceName);

			// specifies the index of next variable to use
			varindex = 0;

			// build string
			index = HasVariable(fullString);
			while (index > -1)
			{
				while (index > -1)
				{
					// which type of inline var
					switch (fullString[index + 1])
					{
						// %i or %d are 4 byte integers. Their value should be inserted as a string.
						case INTEGERFLAG:
						case INTEGERFLAG2:
							var = Variables[varindex];
							intval = (int)var.Data; // todo check type
							varindex++;

							string intstr = intval.ToString();

							// remove the %i, %d and insert the integer directly
							fullString = fullString.Remove(index, 2);
							fullString = fullString.Insert(index, intstr);

							// adjust stringliteral indices right to this index
							for (int i = 0; i < q_indices.Count; i++)
								if (q_indices[i] > index)
									q_indices[i] = q_indices[i] + intstr.Length - 2;
							break;

						// %q is a server-sent string which is directly attached to the message and not looked up from rsb
						// if it contains any %vars itself, these MUST NOT be resolved further (won't be available in params)
						case EMBEDDEDSTRINGLITERALFLAG:
							var = Variables[varindex];
							strval = (string)var.Data;
							varindex++;

							// remove the %q, save string and position for insert later
							fullString = fullString.Remove(index, 2);
							q_strings.Add(strval);
							q_indices.Add(index);

							// adjust stringliteral indices right to this index
							for (int i = 0; i < q_indices.Count; i++)
								if (q_indices[i] > index)
									q_indices[i] = q_indices[i] - 2;
							break;

						// %s is a server-sent string resource id. It must be resolved from .rsb
						// If it contains any %vars itself, these sub-vars must be resolved AFTER any next-vars.
						case STRINGRESOURCELITERALFLAG:
							var = Variables[varindex];
							uintval = (uint)var.Data;
							varindex++;

							if (!stringResources.TryGetValue(uintval, out strval))
								strval = String.Empty;

							// remove the %s, and insert the string,
							// but skip its content in this inner while()                     
							fullString = fullString.Remove(index, 2);
							fullString = fullString.Insert(index, strval);

							// adjust stringliteral indices right to this index
							for (int i = 0; i < q_indices.Count; i++)
								if (q_indices[i] > index)
									q_indices[i] = q_indices[i] + strval.Length - 2;

							// skip
							index += strval.Length;

							break;

#if !VANILLA
						// %r is a server-sent string resource id. It must be resolved from .rsb
						// If it contains any %vars itself, these sub-vars must be resolved BEFORE any next-vars.
						case STRINGRESOURCERECURSIVEFLAG:
							var = Variables[varindex];
							uintval = (uint)var.Data;
							varindex++;

							if (!stringResources.TryGetValue(uintval, out strval))
								strval = String.Empty;

							// remove the %r, insert the content (so we process it next)
							fullString = fullString.Remove(index, 2);
							fullString = fullString.Insert(index, strval);

							// adjust stringliteral indices right to this index
							for (int i = 0; i < q_indices.Count; i++)
								if (q_indices[i] > index)
									q_indices[i] = q_indices[i] + strval.Length - 2;
							break;
#endif
						default:
							break;
					}

					// see if there is more inline vars to the right of current index
					// this makes sure we don't yet process nested and already inserted %s
					index = HasVariable(fullString, index);
				}

				// start from the beginning again, this will process nested %s
				index = HasVariable(fullString);
			}

			// Now finally add the %q stringliterals:
			// MUST iterate backwards (right to left in string)
			// so these inserts don't invalidate the other indices
			for (int i = q_indices.Count - 1; i >= 0; i--)
				fullString = fullString.Insert(q_indices[i], q_strings[i]);

			// extract and remove the inline styles (~B ...)
			Styles = ChatStyle.GetStyles(fullString, chatMessageType);
			fullString = ChatStyle.RemoveInlineStyles(fullString);
		}

        public override string ToString()
        {
            return fullString;
        }

        /// <summary>
        /// Creates a local 'ServerString' instance for given text and
        /// style settings.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="IsBold"></param>
        /// <param name="IsCursive"></param>
        /// <param name="IsUnderline"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static ServerString GetServerStringForString(
            string Text, 
            bool IsBold = false, 
            bool IsCursive = false, 
            bool IsUnderline = false,
            ChatColor Color = ChatColor.Red)
        {         
            ChatStyle style = new ChatStyle(
                0, Text.Length, IsBold, IsCursive, IsUnderline, Color);
            
            ServerString message = new ServerString();
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
