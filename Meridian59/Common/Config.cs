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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel;
using Meridian59.Data.Models;
using System.Collections.Generic;
using System.Globalization;
using Meridian59.Data.Lists;

namespace Meridian59.Common
{
    /// <summary>
    /// Base class for all configuration files
    /// </summary>
    public class Config : IXmlSerializable, INotifyPropertyChanged
    {
        #region Constants
        public const string CONFIGFILE = "configuration.xml";

        public const string PROPNAME_RESOURCESPATH              = "ResourcesPath";
        public const string PROPNAME_PRELOADROOMS               = "PreloadRooms";
        public const string PROPNAME_PRELOADOBJECTS             = "PreloadObjects";
        public const string PROPNAME_PRELOADROOMTEXTURES        = "PreloadRoomTextures";
        public const string PROPNAME_PRELOADSOUND               = "PreloadSound";
        public const string PROPNAME_PRELOADMUSIC               = "PreloadMusic";
        public const string PROPNAME_RESOURCESVERSION           = "ResourcesVersion";
        public const string PROPNAME_CONNECTIONS                = "Connections";
        public const string PROPNAME_SELECTEDCONNECTIONINDEX    = "SelectedConnectionIndex";
        public const string PROPNAME_ALIASES                    = "Aliases";

        protected const string XMLTAG_CONFIGURATION             = "configuration";
        protected const string XMLTAG_RESOURCES                 = "resources";
        protected const string XMLTAG_CONNECTIONS               = "connections";
        protected const string XMLTAG_CONNECTION                = "connection";
        protected const string XMLTAG_IGNORELIST                = "ignorelist";
        protected const string XMLTAG_IGNORE                    = "ignore";
        protected const string XMLTAG_ALIASES                   = "aliases";
        protected const string XMLTAG_ALIAS                     = "alias";
        protected const string XMLATTRIB_VERSION                = "version";
        protected const string XMLATTRIB_PATH                   = "path";
        protected const string XMLATTRIB_PRELOADROOMS           = "preloadrooms";
        protected const string XMLATTRIB_PRELOADOBJECTS         = "preloadobjects";
        protected const string XMLATTRIB_PRELOADROOMTEXTURES    = "preloadroomtextures";
        protected const string XMLATTRIB_PRELOADSOUND           = "preloadsound";
        protected const string XMLATTRIB_PRELOADMUSIC           = "preloadmusic";
        protected const string XMLATTRIB_NAME                   = "name";
        protected const string XMLATTRIB_HOST                   = "host";
        protected const string XMLATTRIB_PORT                   = "port";
        protected const string XMLATTRIB_USEIPV6                = "useipv6";
        protected const string XMLATTRIB_STRINGDICTIONARY       = "stringdictionary";
        protected const string XMLATTRIB_USERNAME               = "username";
        protected const string XMLATTRIB_PASSWORD               = "password";
        protected const string XMLATTRIB_CHARACTER              = "character";
        protected const string XMLATTRIB_SELECTEDINDEX          = "selectedindex";
        protected const string XMLATTRIB_KEY                    = "key"; 
        protected const string XMLATTRIB_VALUE                  = "value";
        #endregion
        
        public static readonly NumberFormatInfo NumberFormatInfo = new NumberFormatInfo();
        
        #region Fields
        protected uint resourcesversion;
        protected string resourcespath;
        protected bool preloadrooms;
        protected bool preloadobjects;
        protected bool preloadroomtextures;
        protected bool preloadsound;
        protected bool preloadmusic;
        protected readonly BindingList<ConnectionInfo> connections = new BindingList<ConnectionInfo>();
        protected int selectedConnectionIndex;
        protected readonly KeyValuePairStringList aliases = new KeyValuePairStringList();
        #endregion

        #region Properties
        /// <summary>
        /// Version of Resources
        /// </summary>
        /// <remarks>
        /// This value can also be found in legacy meridian.ini
        /// It's the value behind Download=XXXXX.
        /// If you send a value below a server set minimum,
        /// you will be redirected to a resource update.
        /// </remarks>
        public uint ResourcesVersion
        {
            get { return resourcesversion; }
            set
            {
                if (resourcesversion != value)
                {
                    resourcesversion = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCESVERSION));
                }
            }
        }

        /// <summary>
        /// Path to resources root
        /// </summary>
        public string ResourcesPath
        {
            get { return resourcespath; }
            set
            {
                if (resourcespath != value)
                {
                    resourcespath = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCESPATH));
                }
            }
        }

        /// <summary>
        /// Whether to preload room files or not.
        /// </summary>
        public bool PreloadRooms
        {
            get { return preloadrooms; }
            set
            {
                if (preloadrooms != value)
                {
                    preloadrooms = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_PRELOADROOMS));
                }
            }
        }

        /// <summary>
        /// Whether to preload bgf objects or not.
        /// </summary>
        public bool PreloadObjects
        {
            get { return preloadobjects; }
            set
            {
                if (preloadobjects != value)
                {
                    preloadobjects = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_PRELOADOBJECTS));
                }
            }
        }

        /// <summary>
        /// Whether to preload room textures or not.
        /// </summary>
        public bool PreloadRoomTextures
        {
            get { return preloadroomtextures; }
            set
            {
                if (preloadroomtextures != value)
                {
                    preloadroomtextures = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_PRELOADROOMTEXTURES));
                }
            }
        }

        /// <summary>
        /// Whether to preload sounds or not.
        /// </summary>
        public bool PreloadSound
        {
            get { return preloadsound; }
            set
            {
                if (preloadsound != value)
                {
                    preloadsound = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_PRELOADSOUND));
                }
            }
        }

        /// <summary>
        /// Whether to preload music or not.
        /// </summary>
        public bool PreloadMusic
        {
            get { return preloadmusic; }
            set
            {
                if (preloadmusic != value)
                {
                    preloadmusic = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_PRELOADMUSIC));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BindingList<ConnectionInfo> Connections
        {
            get { return connections; }          
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedConnectionIndex
        {
            get { return selectedConnectionIndex; }
            set
            {
                if (selectedConnectionIndex != value)
                {
                    selectedConnectionIndex = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_SELECTEDCONNECTIONINDEX));
                }
            }
        }

        /// <summary>
        /// Returns the entry from Connections based on SelectedConnectionIndex.
        /// Or Null if this index does not exist.
        /// </summary>
        public ConnectionInfo SelectedConnectionInfo
        {
            get 
            {
                return (selectedConnectionIndex > -1 && connections.Count > selectedConnectionIndex) ?
                    connections[selectedConnectionIndex] : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public KeyValuePairStringList Aliases
        {
            get { return aliases; }
        }
        #endregion

        #region INotifyPropertyChanged
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Will raise PropertyChangedEvent if a listener exists.
        /// </summary>
        /// <param name="e"></param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        /// <summary>
        /// Static constructor
        /// </summary>
        static Config()
        {
            NumberFormatInfo.NumberDecimalSeparator = ".";
        }

        /// <summary>
        /// Constructor. Will load configuration file directly.
        /// </summary>
        public Config()
        {

            // keep aliases sorted by key
            aliases.SortByKey();

            // allow subclasses to init stuff before loading config file
            InitPreConfig();

            // load config file
            Load();

            // past load init
            InitPastConfig();
        }

        /// <summary>
        /// Overwrite with initialisation code supposed to run BEFORE Load()
        /// </summary>
        protected virtual void InitPreConfig() { }

        /// <summary>
        /// Overwrite with initialisation code supposed to run RIGHT AFTER Load()
        /// </summary>
        protected virtual void InitPastConfig() { }

        /// <summary>
        /// Loads the CONFIGFILE using XmlReader.
        /// Does nothing if the files does not exist.
        /// </summary>
        public virtual void Load()
        {
            if (!File.Exists(CONFIGFILE))
                return;
                
            // create xml reader
            XmlReader reader = XmlReader.Create(CONFIGFILE);
            
            // read rootnode
            if (!reader.ReadToFollowing(XMLTAG_CONFIGURATION))
                return;

            /******************************************************************************/           
            // PART I: RESOURCES

            if (!reader.ReadToFollowing(XMLTAG_RESOURCES))
                return;

            ResourcesPath       = reader[XMLATTRIB_PATH];
            ResourcesVersion    = Convert.ToUInt32(reader[XMLATTRIB_VERSION]);
            PreloadRooms        = Convert.ToBoolean(reader[XMLATTRIB_PRELOADROOMS]);
            PreloadObjects      = Convert.ToBoolean(reader[XMLATTRIB_PRELOADOBJECTS]);
            PreloadRoomTextures = Convert.ToBoolean(reader[XMLATTRIB_PRELOADROOMTEXTURES]);
            PreloadSound        = Convert.ToBoolean(reader[XMLATTRIB_PRELOADSOUND]);
            PreloadMusic        = Convert.ToBoolean(reader[XMLATTRIB_PRELOADMUSIC]);
            
            /******************************************************************************/
            // PART II: Connections

            if (!reader.ReadToFollowing(XMLTAG_CONNECTIONS))
                return;

            SelectedConnectionIndex = Convert.ToInt32(reader[XMLATTRIB_SELECTEDINDEX]);

            connections.Clear();
            if (reader.ReadToDescendant(XMLTAG_CONNECTION))
            {
                do
                {
                    string name             = reader[XMLATTRIB_NAME];
                    string host             = reader[XMLATTRIB_HOST];
                    ushort port             = Convert.ToUInt16(reader[XMLATTRIB_PORT]);
                    bool useipv6            = Convert.ToBoolean(reader[XMLATTRIB_USEIPV6]);
                    string stringdictionary = reader[XMLATTRIB_STRINGDICTIONARY];
                    string username         = reader[XMLATTRIB_USERNAME];
                    string password         = reader[XMLATTRIB_PASSWORD];
                    string character        = reader[XMLATTRIB_CHARACTER];

                    List<string> ignorelist = new List<string>();
                    if (reader.ReadToDescendant(XMLTAG_IGNORELIST))
                    {
                        if (reader.ReadToDescendant(XMLTAG_IGNORE))
                        {
                            do
                            {
                                ignorelist.Add(reader[XMLATTRIB_NAME]);
                            }
                            while (reader.ReadToNextSibling(XMLTAG_IGNORE));

                            reader.ReadEndElement();
                        }
                        else
                        {
                            reader.ReadStartElement(XMLTAG_IGNORELIST);
                        }

                        reader.ReadEndElement();
                    }

                    // add connection
                    connections.Add(new ConnectionInfo(
                        name,
                        host,
                        port,
                        useipv6,
                        stringdictionary,
                        username,
                        password,
                        character,
                        ignorelist));
                }
                while (reader.ReadToNextSibling(XMLTAG_CONNECTION));
            }

            /******************************************************************************/
            // PART III: Aliases

            if (!reader.ReadToFollowing(XMLTAG_ALIASES))
                return;

            if (reader.ReadToDescendant(XMLTAG_ALIAS))
            {
                do
                {
                    string key = reader[XMLATTRIB_KEY];
                    string val = reader[XMLATTRIB_VALUE];
                   
                    // add alias
                    aliases.Add(new KeyValuePairString(key, val));                
                }
                while (reader.ReadToNextSibling(XMLTAG_ALIAS));
            }

            /******************************************************************************/

            // let deriving classes load their stuff
            ReadXml(reader);

            // end
            reader.Close();
        }

        /// <summary>
        /// Saves the current configuration to CONFIGFILE.
        /// </summary>
        public virtual void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            // init writer
            XmlWriter writer = XmlWriter.Create(CONFIGFILE, settings);

            // begin
            writer.WriteStartDocument();
            writer.WriteStartElement(XMLTAG_CONFIGURATION);

            /******************************************************************************/
            // PART I: RESOURCES

            writer.WriteStartElement(XMLTAG_RESOURCES);
            writer.WriteAttributeString(XMLATTRIB_VERSION, ResourcesVersion.ToString());
            writer.WriteAttributeString(XMLATTRIB_PATH, ResourcesPath.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADROOMS, PreloadRooms.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADOBJECTS, PreloadObjects.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADROOMTEXTURES, PreloadRoomTextures.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADSOUND, PreloadSound.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADMUSIC, PreloadMusic.ToString().ToLower());
            writer.WriteEndElement();

            /******************************************************************************/
            // PART II: Connections

            writer.WriteStartElement(XMLTAG_CONNECTIONS);
            writer.WriteAttributeString(XMLATTRIB_SELECTEDINDEX, SelectedConnectionIndex.ToString());

            for (int i = 0; i < connections.Count; i++)
            {
                // connection
                writer.WriteStartElement(XMLTAG_CONNECTION);
                writer.WriteAttributeString(XMLATTRIB_NAME, connections[i].Name);
                writer.WriteAttributeString(XMLATTRIB_HOST, connections[i].Host);
                writer.WriteAttributeString(XMLATTRIB_PORT, connections[i].Port.ToString());
                writer.WriteAttributeString(XMLATTRIB_USEIPV6, connections[i].UseIPv6.ToString());
                writer.WriteAttributeString(XMLATTRIB_STRINGDICTIONARY, connections[i].StringDictionary);
                writer.WriteAttributeString(XMLATTRIB_USERNAME, connections[i].Username);
                writer.WriteAttributeString(XMLATTRIB_PASSWORD, String.Empty);
                writer.WriteAttributeString(XMLATTRIB_CHARACTER, connections[i].Character);

                // ignorelist
                writer.WriteStartElement(XMLTAG_IGNORELIST);
                for (int j = 0; j < connections[i].IgnoreList.Count; j++)
                {
                    writer.WriteStartElement(XMLTAG_IGNORE);
                    writer.WriteAttributeString(XMLATTRIB_NAME, connections[i].IgnoreList[j]);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            /******************************************************************************/
            // PART III: Aliases

            writer.WriteStartElement(XMLTAG_ALIASES);

            for (int i = 0; i < aliases.Count; i++)
            {
                // alias
                writer.WriteStartElement(XMLTAG_ALIAS);
                writer.WriteAttributeString(XMLATTRIB_KEY, aliases[i].Key);
                writer.WriteAttributeString(XMLATTRIB_VALUE, aliases[i].Value);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            /******************************************************************************/

            // let deriving classes write their stuff
            WriteXml(writer);

            // end
            writer.WriteEndElement();
            writer.WriteEndDocument();

            // close writer
            writer.Close();
        }
        
        /// <summary>
        /// Returns null, overwrite if necessary.
        /// </summary>
        /// <returns></returns>
        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Does nothing by default, overwrite if necessary.
        /// </summary>
        /// <param name="Reader"></param>
        public virtual void ReadXml(XmlReader Reader) { }

        /// <summary>
        /// Does nothing by default, overwrite if necessary.
        /// </summary>
        /// <param name="Writer"></param>
        public virtual void WriteXml(XmlWriter Writer) { }
    }
}
