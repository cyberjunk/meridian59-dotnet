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
using Meridian59.Common.Enums;

namespace Meridian59.Common
{
    /// <summary>
    /// Base class for all configuration files
    /// </summary>
    public class Config : INotifyPropertyChanged
    {
        #region Constants
        public const string CONFIGFILE                          = "../configuration.xml";
        public const string CONFIGFILE_ALT                      = "configuration.xml";

        public const string PROPNAME_RESOURCESPATH              = "ResourcesPath";
        public const string PROPNAME_PRELOADROOMS               = "PreloadRooms";
        public const string PROPNAME_PRELOADOBJECTS             = "PreloadObjects";
        public const string PROPNAME_PRELOADROOMTEXTURES        = "PreloadRoomTextures";
        public const string PROPNAME_PRELOADSOUND               = "PreloadSound";
        public const string PROPNAME_PRELOADMUSIC               = "PreloadMusic";
        public const string PROPNAME_RESOURCESVERSION           = "ResourcesVersion";
        public const string PROPNAME_CONNECTIONS                = "Connections";
        public const string PROPNAME_SELECTEDCONNECTIONINDEX    = "SelectedConnectionIndex";
        public const string PROPNAME_LANGUAGE                   = "Language";
        public const string PROPNAME_ALIASES                    = "Aliases";

        public const string DEFAULTVAL_RESOURCES_PATH           = "../resources/";
        public const string DEFAULTVAL_RESOURCES_PATH_DEV       = "../../../../resources/";
        public const uint   DEFAULTVAL_RESOURCES_VERSION        = 10016;
        public const bool   DEFAULTVAL_RESOURCES_PRELOADROOMS   = false;
        public const bool   DEFAULTVAL_RESOURCES_PRELOADOBJECTS = false;
        public const bool   DEFAULTVAL_RESOURCES_PRELOADROOMTEX = false;
        public const bool   DEFAULTVAL_RESOURCES_PRELOADSOUND   = false;
        public const bool   DEFAULTVAL_RESOURCES_PRELOADMUSIC   = false;
        public const int    DEFAULTVAL_CONNECTIONS_SELECTEDINDEX= 0;
        public const string DEFAULTVAL_CONNECTIONS_NAME         = "";
        public const string DEFAULTVAL_CONNECTIONS_HOST         = "";
        public const ushort DEFAULTVAL_CONNECTIONS_PORT         = 5959;
        public const bool   DEFAULTVAL_CONNECTIONS_USEIPV6      = false;
        public const string DEFAULTVAL_CONNECTIONS_STRINGDICT   = "rsc0000.rsb";
        public const string DEFAULTVAL_CONNECTIONS_USERNAME     = "";
        public const string DEFAULTVAL_CONNECTIONS_PASSWORD     = "";
        public const string DEFAULTVAL_CONNECTIONS_CHARACTER    = "";
        public const string DEFAULTVAL_ALIASES_KEY              = "";
        public const string DEFAULTVAL_ALIASES_VALUE            = "";
        public const LanguageCode DEFAULTVAL_LANGUAGE           = LanguageCode.English;
        
        protected const string XMLTAG_CONFIGURATION             = "configuration";
        protected const string XMLTAG_RESOURCES                 = "resources";
        protected const string XMLTAG_CONNECTIONS               = "connections";
        protected const string XMLTAG_CONNECTION                = "connection";
        protected const string XMLTAG_IGNORELIST                = "ignorelist";
        protected const string XMLTAG_IGNORE                    = "ignore";
        protected const string XMLTAG_GROUPS                    = "groups";
        protected const string XMLTAG_GROUP                     = "group";
        protected const string XMLTAG_MEMBER                    = "member";
        protected const string XMLTAG_LANGUAGE                  = "language";
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
        protected string configFile;
        protected string configFileAlt;
        protected uint resourcesversion;
        protected string resourcespath;
        protected bool preloadrooms;
        protected bool preloadobjects;
        protected bool preloadroomtextures;
        protected bool preloadsound;
        protected bool preloadmusic;
        protected readonly BindingList<ConnectionInfo> connections = new BindingList<ConnectionInfo>();
        protected int selectedConnectionIndex;
        protected LanguageCode language;
        protected readonly KeyValuePairStringList aliases = new KeyValuePairStringList();
        #endregion

        #region Properties
        /// <summary>
        /// Returns the path/filename of the config file
        /// </summary>
        public string ConfigFile { get { return configFile; } }

        /// <summary>
        /// Returns the path/filename of the alternative config file
        /// </summary>
        public string ConfigFileAlt { get { return configFileAlt; } }

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
        /// Currently selected Language
        /// </summary>
        public LanguageCode Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_LANGUAGE));
                }
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
            this.configFile = String.Empty;
            this.configFileAlt = String.Empty;

            // keep aliases sorted by key
            aliases.SortByKey();
        }

        public static string GetFilenameFromCmdArgs(string[] args)
        {
            if (args == null)
                return null;

            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] != "-c")
                   continue;

                if (args[i + 1] != null &&
                    args[i + 1] != String.Empty)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        /// <summary>
        /// Loads the configuration using XmlReader.
        /// Tries ConfigFile first, then ConfiglFileAlt, otherwise uses defaults.
        /// </summary>
        public virtual void Load(string ConfigFile, string ConfigFileAlt)
        {
            this.configFile = ConfigFile;
            this.configFileAlt = ConfigFileAlt;

            XmlDocument doc = new XmlDocument();
            XmlNode node, node2;

            uint val_uint;
            bool val_bool;
            int val_int;
            ushort val_ushort;
            LanguageCode val_language;

            // check for ../configuration.xml existance
            if (File.Exists(ConfigFile))
            {
                // create xml reader
                XmlReader reader = XmlReader.Create(ConfigFile);

                // try parse file
                doc.Load(reader);
                reader.Close();
            }

            // try find configuration.xml at alternative location
            else if (File.Exists(ConfigFileAlt))
            {
                // create xml reader
                XmlReader reader = XmlReader.Create(ConfigFileAlt);

                // try parse file
                doc.Load(reader);
                reader.Close();
            }

            // no configuration found, use defaults
            else
            {
                // create empty rootnode
                doc.InsertBefore(doc.CreateElement(XMLTAG_CONFIGURATION), null);
                Logger.Log("Config", LogType.Info, "File '" + ConfigFile + "' and '" + ConfigFileAlt + "' not found. Using defaults.");
            }
            
            // clear some old
            connections.Clear();
            aliases.Clear();

            /******************************************************************************/           
            // PART I: RESOURCES
            /******************************************************************************/

            node = doc.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_RESOURCES);

            if (node != null)
            {
                // path when shipped to user (../resources)
                ResourcesPath = (node.Attributes[XMLATTRIB_PATH] != null) ? 
                    node.Attributes[XMLATTRIB_PATH].Value : DEFAULTVAL_RESOURCES_PATH;

                if (!Directory.Exists(ResourcesPath))
                {
                    // path when running from build-dir (../../../../resources)
                    ResourcesPath = (node.Attributes[XMLATTRIB_PATH] != null) ?
                        node.Attributes[XMLATTRIB_PATH].Value : DEFAULTVAL_RESOURCES_PATH_DEV;
                }

                ResourcesVersion = (node.Attributes[XMLATTRIB_VERSION] != null && UInt32.TryParse(node.Attributes[XMLATTRIB_VERSION].Value, out val_uint)) ? 
                    val_uint : DEFAULTVAL_RESOURCES_VERSION;

                PreloadRooms = (node.Attributes[XMLATTRIB_PRELOADROOMS] != null && Boolean.TryParse(node.Attributes[XMLATTRIB_PRELOADROOMS].Value, out val_bool)) ? 
                    val_bool : DEFAULTVAL_RESOURCES_PRELOADROOMS;

                PreloadObjects = (node.Attributes[XMLATTRIB_PRELOADOBJECTS] != null && Boolean.TryParse(node.Attributes[XMLATTRIB_PRELOADOBJECTS].Value, out val_bool)) ? 
                    val_bool : DEFAULTVAL_RESOURCES_PRELOADOBJECTS;

                PreloadRoomTextures = (node.Attributes[XMLATTRIB_PRELOADROOMTEXTURES] != null && Boolean.TryParse(node.Attributes[XMLATTRIB_PRELOADROOMTEXTURES].Value, out val_bool)) ? 
                    val_bool : DEFAULTVAL_RESOURCES_PRELOADROOMTEX;

                PreloadSound = (node.Attributes[XMLATTRIB_PRELOADSOUND] != null && Boolean.TryParse(node.Attributes[XMLATTRIB_PRELOADSOUND].Value, out val_bool)) ? 
                    val_bool : DEFAULTVAL_RESOURCES_PRELOADSOUND;

                PreloadMusic = (node.Attributes[XMLATTRIB_PRELOADMUSIC] != null && Boolean.TryParse(node.Attributes[XMLATTRIB_PRELOADMUSIC].Value, out val_bool)) ? 
                    val_bool : DEFAULTVAL_RESOURCES_PRELOADMUSIC;
            }
            else
            {
                ResourcesPath = (Directory.Exists(DEFAULTVAL_RESOURCES_PATH)) ? DEFAULTVAL_RESOURCES_PATH : DEFAULTVAL_RESOURCES_PATH_DEV;
                ResourcesVersion = DEFAULTVAL_RESOURCES_VERSION;
                PreloadRooms = DEFAULTVAL_RESOURCES_PRELOADROOMS;
                PreloadObjects = DEFAULTVAL_RESOURCES_PRELOADOBJECTS;
                PreloadRoomTextures = DEFAULTVAL_RESOURCES_PRELOADROOMTEX;
                PreloadSound = DEFAULTVAL_RESOURCES_PRELOADSOUND;
                PreloadMusic = DEFAULTVAL_RESOURCES_PRELOADMUSIC;
            }

            /******************************************************************************/
            // PART II: Connections
            /******************************************************************************/
            
            node = doc.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_CONNECTIONS);

            if (node != null && node.ChildNodes.Count > 0)
            {
                SelectedConnectionIndex = (node.Attributes[XMLATTRIB_SELECTEDINDEX] != null && Int32.TryParse(node.Attributes[XMLATTRIB_SELECTEDINDEX].Value, out val_int)) ?
                    val_int : DEFAULTVAL_CONNECTIONS_SELECTEDINDEX;
           
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_CONNECTION)
                        continue;

                    string name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                        child.Attributes[XMLATTRIB_NAME].Value : DEFAULTVAL_CONNECTIONS_NAME;

                    string host = (child.Attributes[XMLATTRIB_HOST] != null) ?
                        child.Attributes[XMLATTRIB_HOST].Value : DEFAULTVAL_CONNECTIONS_HOST;
#if !VANILLA && !OPENMERIDIAN
                    // Change old 112 host entry if present.
                    if (host.Equals("meridian112.arantis.eu"))
                        host = ConnectionInfo.CON112.Host;
#endif
                    ushort port = (child.Attributes[XMLATTRIB_PORT] != null && UInt16.TryParse(child.Attributes[XMLATTRIB_PORT].Value, out val_ushort)) ?
                        val_ushort : DEFAULTVAL_CONNECTIONS_PORT;

                    string stringdictionary = (child.Attributes[XMLATTRIB_STRINGDICTIONARY] != null) ?
                        child.Attributes[XMLATTRIB_STRINGDICTIONARY].Value : DEFAULTVAL_CONNECTIONS_STRINGDICT;

                    string username = (child.Attributes[XMLATTRIB_USERNAME] != null) ?
                        child.Attributes[XMLATTRIB_USERNAME].Value : DEFAULTVAL_CONNECTIONS_USERNAME;

                    string password = (child.Attributes[XMLATTRIB_PASSWORD] != null) ?
                        child.Attributes[XMLATTRIB_PASSWORD].Value : DEFAULTVAL_CONNECTIONS_PASSWORD;

                    string character = (child.Attributes[XMLATTRIB_CHARACTER] != null) ?
                        child.Attributes[XMLATTRIB_CHARACTER].Value : DEFAULTVAL_CONNECTIONS_CHARACTER;

                    // read ignorelist
                    List<string> ignorelist = new List<string>();                      
                    node2 = child.SelectSingleNode(XMLTAG_IGNORELIST);
                    if (node2 != null)
                    {
                        foreach (XmlNode subchild in node2.ChildNodes)
                        {
                            if (subchild.Name != XMLTAG_IGNORE)
                                continue;

                            if (subchild.Attributes[XMLATTRIB_NAME] == null)
                                continue;

                            ignorelist.Add(subchild.Attributes[XMLATTRIB_NAME].Value);
                        }
                    }

                    // read groups
                    List<Group> groups = new List<Group>();                                            
                    node2 = child.SelectSingleNode(XMLTAG_GROUPS);
                    if (node2 != null)
                    {
                        foreach (XmlNode subchild in node2.ChildNodes)
                        {
                            if (subchild.Name != XMLTAG_GROUP)
                                continue;

                            if (subchild.Attributes[XMLATTRIB_NAME] == null)
                                continue;

                            List<GroupMember> members = new List<GroupMember>();
                            foreach (XmlNode xmlMember in subchild.ChildNodes)
                            {
                                if (xmlMember.Name != XMLTAG_MEMBER)
                                    continue;

                                if (xmlMember.Attributes[XMLATTRIB_NAME] == null)
                                    continue;

                                members.Add(new GroupMember(xmlMember.Attributes[XMLATTRIB_NAME].Value));
                            }

                            groups.Add(new Group(subchild.Attributes[XMLATTRIB_NAME].Value, members));
                        }
                    }                  

                    // add connection
                    connections.Add(new ConnectionInfo(
                        name,
                        host,
                        port,
                        stringdictionary,
                        username,
                        password,
                        character,
                        ignorelist,
                        groups));
                }

                // Double check we have all available servers in connections list.
#if VANILLA
                if (!HasConnection(ConnectionInfo.CON101.Host, ConnectionInfo.CON101.Port))
                    connections.Add(ConnectionInfo.CON101);
                if (!HasConnection(ConnectionInfo.CON102.Host, ConnectionInfo.CON102.Port))
                    connections.Add(ConnectionInfo.CON102);
#elif OPENMERIDIAN
                if (!HasConnection(ConnectionInfo.CON103.Host, ConnectionInfo.CON103.Port))
                    connections.Add(ConnectionInfo.CON103);
                if (!HasConnection(ConnectionInfo.CON104.Host, ConnectionInfo.CON104.Port))
                    connections.Add(ConnectionInfo.CON104);
#else
                if (!HasConnection(ConnectionInfo.CON105.Host, ConnectionInfo.CON105.Port))
                    connections.Add(ConnectionInfo.CON105);
                if (!HasConnection(ConnectionInfo.CON106.Host, ConnectionInfo.CON106.Port))
                    connections.Add(ConnectionInfo.CON106);
                if (!HasConnection(ConnectionInfo.CON112.Host, ConnectionInfo.CON112.Port))
                    connections.Add(ConnectionInfo.CON112);
#endif
            }
            else
            {
                SelectedConnectionIndex = DEFAULTVAL_CONNECTIONS_SELECTEDINDEX;
#if VANILLA
                connections.Add(ConnectionInfo.CON101);
                connections.Add(ConnectionInfo.CON102);
#elif OPENMERIDIAN
                connections.Add(ConnectionInfo.CON103);
                connections.Add(ConnectionInfo.CON104);
#else
                connections.Add(ConnectionInfo.CON105);
                connections.Add(ConnectionInfo.CON106);
                connections.Add(ConnectionInfo.CON112);
#endif
            }

            /******************************************************************************/
            // PART III: Language
            /******************************************************************************/
            
            node = doc.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_LANGUAGE);

            if (node != null)
            {
                Language = (node.Attributes[XMLATTRIB_VALUE] != null && Enum.TryParse<LanguageCode>(node.Attributes[XMLATTRIB_VALUE].Value, out val_language)) ?
                    val_language : DEFAULTVAL_LANGUAGE;
            }
            else
            {
                Language = DEFAULTVAL_LANGUAGE;
            }

            /******************************************************************************/
            // PART IV: Aliases
            /******************************************************************************/
            
            node = doc.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_ALIASES);

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_ALIAS)
                        continue;

                    string key = (child.Attributes[XMLATTRIB_KEY] != null) ?
                        child.Attributes[XMLATTRIB_KEY].Value : DEFAULTVAL_ALIASES_KEY;

                    string val = (child.Attributes[XMLATTRIB_VALUE] != null) ?
                        child.Attributes[XMLATTRIB_VALUE].Value : DEFAULTVAL_ALIASES_VALUE;

                    aliases.Add(new KeyValuePairString(key, val));
                }
            }

            /******************************************************************************/

            // let deriving classes load their stuff
            ReadXml(doc);
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
            XmlWriter writer = XmlWriter.Create(ConfigFile, settings);

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

                // groups
                writer.WriteStartElement(XMLTAG_GROUPS);
                foreach (Group group in connections[i].Groups)
                {
                    writer.WriteStartElement(XMLTAG_GROUP);
                    writer.WriteAttributeString(XMLATTRIB_NAME, group.Name);

                    foreach (GroupMember member in group.Members)
                    {
                        writer.WriteStartElement(XMLTAG_MEMBER);
                        writer.WriteAttributeString(XMLATTRIB_NAME, member.Name);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            
            /******************************************************************************/
            // PART III: Language

            writer.WriteStartElement(XMLTAG_LANGUAGE);
            writer.WriteAttributeString(XMLATTRIB_VALUE, language.ToString());
            writer.WriteEndElement();

            /******************************************************************************/
            // PART IV: Aliases

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
        /// Checks if the given host and port can be found in the connections BindingList.
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        public virtual bool HasConnection(string Host, ushort Port)
        {
            foreach (ConnectionInfo connection in connections)
                if (connection.Host.Equals(Host) && connection.Port == Port)
                    return true;

            return false;
        }

        /// <summary>
        /// Does nothing by default, overwrite if necessary.
        /// </summary>
        /// <param name="Document"></param>
        public virtual void ReadXml(XmlDocument Document) { }

        /// <summary>
        /// Does nothing by default, overwrite if necessary.
        /// </summary>
        /// <param name="Writer"></param>
        public virtual void WriteXml(XmlWriter Writer) { }
    }
}
