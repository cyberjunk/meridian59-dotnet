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
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A M59 mail
    /// </summary>
    [Serializable]
    public class Mail : IByteSerializable, INotifyPropertyChanged, IClearable, IXmlSerializable
    {
        #region Constants
        public const byte MAXRECIPIENTS         = 20;
        public const string PROPNAME_NUM        = "Num";
        public const string PROPNAME_SENDER     = "Sender";
        public const string PROPNAME_TIMESTAMP  = "Timestamp";
        public const string PROPNAME_RECIPIENTS = "Recipients";
        public const string PROPNAME_MESSAGE    = "Message";
        public const string PROPNAME_TITLE      = "Title";

        public const string TAG_MAIL        = "mail";
        public const string TAG_RECIPIENTS  = "recipients";
        public const string TAG_RECIPIENT   = "recipient";
        public const string TAG_TEXT        = "text";       
        public const string ATTRIB_SENDER   = "sender";
        public const string ATTRIB_TIMESTAMP= "timestamp";
        public const string ATTRIB_NAME     = "name";
        public const string ATTRIB_TITLE    = "title";
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
                int len = TypeSizes.INT + TypeSizes.SHORT + sender.Length + TypeSizes.INT;

                len += TypeSizes.SHORT;
                foreach (string s in recipients)
                    len += TypeSizes.SHORT + s.Length;

                len += message.ByteLength;

                return len; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            num = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;
            
            sender = Encoding.Default.GetString(Buffer, cursor, strlen);
            cursor += strlen;

            timestamp = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ushort listlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            recipients = new List<string>();
                
            // get recipients
            for (int i = 0; i < listlen; i++)
            {
                strlen = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                string recipient = Encoding.Default.GetString(Buffer, cursor, strlen);
                cursor += strlen;

                recipients.Add(recipient);
            }

            // get message
            message = new ServerString(ChatMessageType.ObjectChatMessage, stringResources, Buffer, cursor);
            cursor += message.ByteLength;

            // Now ugly:
            // For some reason the title is encoded into the message body,
            // and not a separate field. It also includes hardcoded "Subject:" string
            string tmp1, tmp2;
            Mail.ParseTitle(message.FullString, out tmp1, out tmp2);

            // update values from parsed results (error: String.Empty)
            title = tmp1;
            message.FullString = tmp2;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(num), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(sender.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(sender), 0, Buffer, cursor, sender.Length);
            cursor += sender.Length;

            Array.Copy(BitConverter.GetBytes(timestamp), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(recipients.Count)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (string s in recipients)
            {
                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(s.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                Array.Copy(Encoding.Default.GetBytes(s), 0, Buffer, cursor, s.Length);
                cursor += s.Length;
            }

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
        protected uint num;
        protected string sender;
        protected uint timestamp;
        protected List<string> recipients;
        protected ServerString message;
        protected string title;

		protected StringDictionary stringResources;
        #endregion

        #region Properties
        public uint Num
        {
            get
            {
                return num;
            }
            set
            {
                if (num != value)
                {
                    num = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUM));
                }
            }
        }

        public string Sender
        {
            get
            {
                return sender;
            }
            set
            {
                if (sender != value)
                {
                    sender = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SENDER));
                }
            }
        }

        public uint Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                if (timestamp != value)
                {
                    timestamp = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TIMESTAMP));
                }
            }
        }

        public List<string> Recipients
        {
            get
            {
                return recipients;
            }
            set
            {
                if (recipients != value)
                {
                    recipients = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RECIPIENTS));
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

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TITLE));
                }
            }
        }
        #endregion

        #region Constructors
        public Mail()
        {
            Clear(false);
        }

        public Mail(uint Num, string Sender, uint TimeStamp, List<string> Recipients, ServerString Message, string Title)
        {
            num = Num;
            sender = Sender;
            timestamp = TimeStamp;
            recipients = Recipients;
            message = Message;
            title = Title;
        }

		public Mail(StringDictionary StringResources, byte[] Buffer, int StartIndex = 0) 
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
                Num = 0;
                Sender = String.Empty;
                Timestamp = 0;
                Recipients = new List<string>(10);
                Message = new ServerString();
                Title = String.Empty;
            }
            else
            {
                num = 0;
                sender = String.Empty;
                timestamp = 0;
                recipients = new List<string>(10);
                message = new ServerString();
                title = String.Empty;
            }
        }
        #endregion

        #region Xml Save/Load
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader Reader)
        {
            // rootnode
            Reader.ReadToFollowing(TAG_MAIL);
            
            // read attributes
            Sender = Reader[ATTRIB_SENDER];
            Timestamp = Convert.ToUInt32(Reader[ATTRIB_TIMESTAMP]);
            Title = Reader[ATTRIB_TITLE];

            // recipients
            Reader.ReadToFollowing(TAG_RECIPIENTS);
            if (Reader.ReadToDescendant(TAG_RECIPIENT))
            {
                do
                {
                    Recipients.Add(Reader[ATTRIB_NAME]);
                }
                while (Reader.ReadToNextSibling(TAG_RECIPIENT));
            }

            // text
            Reader.ReadToFollowing(TAG_TEXT);
            
            // construct chatmessage instance dummy
            Message = new ServerString();
            Message.FullString = Reader.ReadString();
        }

        public void WriteXml(XmlWriter Writer)
        {
            // begin
            Writer.WriteStartDocument();
            Writer.WriteStartElement(TAG_MAIL);

            // write properties as attributes
            Writer.WriteAttributeString(ATTRIB_SENDER, Sender);
            Writer.WriteAttributeString(ATTRIB_TIMESTAMP, Timestamp.ToString());
            Writer.WriteAttributeString(ATTRIB_TITLE, Title);
           
            // write recipients start
            Writer.WriteStartElement(TAG_RECIPIENTS);

            // write each single one
            foreach (string s in Recipients)
            {
                // write recipient start
                Writer.WriteStartElement(TAG_RECIPIENT);

                // write name attribute
                Writer.WriteAttributeString(ATTRIB_NAME, s);
            
                // write recipient end          
                Writer.WriteEndElement();
            }

            // write recipients end
            Writer.WriteEndElement();

            // write text
            Writer.WriteStartElement(TAG_TEXT);
            Writer.WriteString(Message.FullString);
            Writer.WriteEndElement();

            // end
            Writer.WriteEndElement();
            Writer.WriteEndDocument();
        }

        /// <summary>
        /// Load from XML file.
        /// </summary>
        /// <returns>False if an exception was raised.</returns>
        public bool Load(string File)
        {
            bool returnValue = false;
            try
            {
                string str = Path.GetFileNameWithoutExtension(File);

                // parse filename to num
                if (UInt32.TryParse(str, out num))
                {
                    XmlReader reader = XmlReader.Create(File);
                    ReadXml(reader);

                    reader.Close();
                    returnValue = true;
                }              
            }
            catch (Exception) { }

            return returnValue;
        }

        /// <summary>
        /// Save to XML file
        /// </summary>
        /// <returns>False if an exception was raised.</returns>
        public bool Save(string File)
        {
            bool returnValue = false;

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";

                XmlWriter writer = XmlWriter.Create(File, settings);
                WriteXml(writer);

                writer.Close();
                returnValue = true;
            }
            catch (Exception) { }

            return returnValue;
        }

        /// <summary>
        /// Creates a filename for the mail based on Num.
        /// Example: 00001.xml
        /// </summary>
        /// <returns></returns>
        public string GetFilename()
        {
            // start with pure number, e.g. 5
            string str = Num.ToString();

            // add 0 prefixed until number has length of 5
            int loops = 5 - str.Length;
            for (int i = 0; i < loops; i++)
                str = str.Insert(0, "0");

            // add extension
            str += FileExtensions.XML;

            return str;
        }
        #endregion

        /// <summary>
        /// Message title is encoded into the message text.
        /// It's the first line until the first line break.
        /// It also contains a string: "Subject: ", which depends
        /// on the language of the client sending it.
        /// </summary>
        /// <param name="FullText">Full input text</param>
        /// <param name="Title">Out variable storing parsed title. Default: String.Empty</param>
        /// <param name="Text">Out variable storing parsed text. Default: String.Empty</param>
        /// <returns>Successful or not</returns>
        public static bool ParseTitle(string FullText, out string Title, out string Text)
        {
            // default out returns
            Title = String.Empty;
            Text = String.Empty;

            try
            {
                // check
                if (FullText == null)
                    return false;

                // find first linebreak
                int pos = FullText.IndexOf("\n");

                // check
                if (pos < 0)
                    return false;

                // read substrings
                Title = FullText.Substring(0, pos);
                Text = FullText.Substring(pos + 1);

                // get position of ': ' from "Subject:" or "Betreff:" or ..
                pos = Title.IndexOf(": ");

                // remove "subject" part from title
                if (pos >= 0)
                    Title = Title.Substring(pos + 2);
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
