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
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Common;
using Meridian59.Files.ROO;
using Meridian59.Common.Enums;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.Files.RSB
{
    /// <summary>
    /// Use to access M59 rsb files (string lists)
    /// </summary>
    public class RsbFile : IGameFile, IByteSerializable, IXmlSerializable, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const uint SIGNATURE = 0x01435352;
		public const uint VERSION5  = 0x00000005;
        public const uint VERSION4  = 0x00000004;
        public const uint VERSION3  = 0x00000003;

        public static readonly byte[] PASSWORDV3 = 
            { 0x2F, 0xC6, 0x46, 0xDA, 0x20, 0x0E, 0x9F, 0xF9, 0x00 };

        public const string DEFAULTFILENAME             = "rsc0000.rsb";
		public const uint DEFAULTVERSION				= VERSION5;
		public const string PROPNAME_VERSION			= "Version";
        public const string PROPNAME_FILENAME           = "Filename";
        public const string PROPNAME_STRINGRESOURCES    = "StringResources";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion
        
        #region IByteSerializable
        public int ByteLength
        {
            get 
            {
                // header
                int len = TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;

                // strings
                foreach (RsbResourceID entry in stringResources)
                    len += entry.ByteLength;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(SIGNATURE), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
            
            // note: we can't write v3 here, would have to encrypt
            // so trying to write opened v3 files will result in v4 files
            Array.Copy(BitConverter.GetBytes(Math.Max(version, VERSION4)), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            int count = StringResources.Count;

            // old versions cant store other languages
            // they won't write them to the file below
            if (version < VERSION5)
            {
                count = 0;
                foreach (RsbResourceID obj in StringResources)
                    if (obj.Language == LanguageCode.English)
                        count++;
            }

            Array.Copy(BitConverter.GetBytes(count), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            foreach (RsbResourceID entry in StringResources)            
                cursor += entry.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            uint sig= BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            if (sig == SIGNATURE)
            {
                version = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                uint entries = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                // decrypt old versions first (v4 first unencryted)
                // note: this might need different passwords for other versions
				if (version <= VERSION3)
                {
#if WINCLR && X86
                    uint streamlength = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    uint expectedresponse = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

					Crush32.Decrypt(Buffer, cursor, (int)streamlength, version, expectedresponse, PASSWORDV3);                   
#else
                    throw new Exception(RooFile.ERRORCRUSHPLATFORM);
#endif
                }

                // now load strings                  
                StringResources.Clear();
                StringResources.Capacity = (int)entries + 100;
                for (int i = 0; i < entries; i++)
                {
                    RsbResourceID entry = new RsbResourceID(version, Buffer, cursor);
                    cursor += entry.ByteLength;

					StringResources.Add(entry);
                }               
            }
            else
                throw new Exception("Wrong RSC file signature: " + sig + " (expected " + SIGNATURE + ").");
           
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

            set
            {
                ReadFrom(value);
            }
        }
        #endregion

        #region IGameFile
        /// <summary>
        /// Load string resources from file
        /// </summary>
        /// <param name="Filename">Full path and filename of string resource file</param>
        public void Load(string Filename)
        {
            // save raw filename without path or extensions
            this.Filename = Path.GetFileNameWithoutExtension(Filename);
          
            byte[] fileBytes = File.ReadAllBytes(Filename);
            ReadFrom(fileBytes, 0);           
        }

        /// <summary>
        /// Save string resources to .rsb file
        /// </summary>
        /// <param name="Filename">Full path and filename of string resource file</param>
        public void Save(string Filename)
        {
            File.WriteAllBytes(Filename, Bytes);
        }    
        #endregion

        #region IXmlSerializable
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(string Filename)
        {
            this.Filename = Path.GetFileNameWithoutExtension(Filename);

            XmlReader reader = XmlReader.Create(Filename);
            ReadXml(reader);
        }

        public void ReadXml(XmlReader reader)
        {
            // rootnode
            reader.ReadToFollowing("rsb");
            
            // strings
            reader.ReadToFollowing("strings");
            int framecount = Convert.ToInt32(reader["count"]);

            for (int i = 0; i < framecount; i++)
            {
                reader.ReadToFollowing("string");
                uint id = Convert.ToUInt32(reader["id"]);
				LanguageCode lang = LanguageCode.English; //todo
                string resource = reader.ReadString();
				stringResources.Add(new RsbResourceID(id, resource, version, lang));
            }
        }

        public void WriteXml(string Filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            XmlWriter writer = XmlWriter.Create(Filename, settings);
            WriteXml(writer);
        }

        public void WriteXml(XmlWriter writer)
        {
            // begin
            writer.WriteStartDocument();
            writer.WriteStartElement("rsb");
            
            // strings
            writer.WriteStartElement("strings");
            writer.WriteAttributeString("count", StringResources.Count.ToString());
            foreach(RsbResourceID entry in StringResources)
            {
                writer.WriteStartElement("string");
                writer.WriteAttributeString("id", entry.ID.ToString());
                writer.WriteString(entry.Text);               
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // end
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }
        #endregion

        #region Fields
		protected uint version;
        protected string filename;
        protected readonly StringList stringResources = new StringList();
        #endregion

        #region Properties
		/// <summary>
		/// Fileformat version
		/// </summary>
		public uint Version
		{
			get { return version; }
			set
			{
				if (version != value)
				{
					version = value;

                    foreach (RsbResourceID obj in StringResources)
                        obj.RsbVersion = version;

					RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VERSION));
				}
			}
		}

        /// <summary>
        /// Filename without path or extension
        /// </summary>
        public string Filename
        {
            get { return filename; }
            set
            {
                if (filename != value)
                {
                    filename = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FILENAME));
                }
            }
        }

        /// <summary>
        /// A key/value pair dictionary with resource/string IDs.
        /// </summary>
        public StringList StringResources 
        {
            get { return stringResources; }            
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public RsbFile()
        {
            Clear(false);
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="StringResources"></param>
		public RsbFile(IEnumerable<RsbResourceID> StringResources)
        {
            filename = DEFAULTFILENAME;

            stringResources.AddRange(StringResources);
        }

        /// <summary>
        /// Constructor by file
        /// </summary>
        /// <param name="Filename"></param>
        public RsbFile(string Filename)
        {
            string extension = Path.GetExtension(Filename);           
            if (extension == FileExtensions.RSB)
            {
                Load(Filename);
            }
            else if (extension == FileExtensions.XML)
            {
                ReadXml(Filename);
            }   
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Filename = DEFAULTFILENAME;
				Version = DEFAULTVERSION;
                stringResources.Clear();
            }
            else
            {
                filename = DEFAULTFILENAME;
				version = DEFAULTVERSION;
                stringResources.Clear();
            }
        }
        #endregion
    }
}
