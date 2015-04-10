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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;

namespace Meridian59.Common
{
    /// <summary>
    /// Base class for all configuration files
    /// </summary>
    public class Config : IXmlSerializable, INotifyPropertyChanged
    {
        #region Constants
        public const string CONFIGFILE = "configuration.xml";
        
        public const string SUBPATHSTRINGS          = "strings";
        public const string SUBPATHROOMS            = "rooms";
        public const string SUBPATHROOMTEXTURES     = "bgftextures";
        public const string SUBPATHOBJECTS          = "bgfobjects";
        public const string SUBPATHSOUNDS           = "sounds";
        public const string SUBPATHMUSIC            = "music";
        public const string SUBPATHMAILS            = "mails";

        public const string PROPNAME_RESOURCESPATH          = "ResourcesPath";
        public const string PROPNAME_PRELOADROOMS           = "PreloadRooms";
        public const string PROPNAME_PRELOADOBJECTS         = "PreloadObjects";
        public const string PROPNAME_PRELOADROOMTEXTURES    = "PreloadRoomTextures";
        public const string PROPNAME_PRELOADSOUND           = "PreloadSound";
        public const string PROPNAME_PRELOADMUSIC           = "PreloadMusic";
        public const string PROPNAME_RESOURCESVERSION       = "ResourcesVersion";
        public const string PROPNAME_RESOURCEMANAGER        = "ResourceManager";
        public const string PROPNAME_COUNTROOMS             = "CountRooms";
        public const string PROPNAME_COUNTOBJECTS           = "CountObjects";
        public const string PROPNAME_COUNTROOMTEXTURES      = "CountRoomTextures";
        public const string PROPNAME_COUNTSOUNDS            = "CountSounds";
        public const string PROPNAME_COUNTMUSIC             = "CountMusic";

        protected const string XMLTAG_CONFIGURATION             = "configuration";
        protected const string XMLTAG_RESOURCES                 = "resources";
        protected const string XMLATTRIB_VERSION                = "version";
        protected const string XMLATTRIB_PATH                   = "path";
        protected const string XMLATTRIB_PRELOADROOMS           = "preloadrooms";
        protected const string XMLATTRIB_PRELOADOBJECTS         = "preloadobjects";
        protected const string XMLATTRIB_PRELOADROOMTEXTURES    = "preloadroomtextures";
        protected const string XMLATTRIB_PRELOADSOUND           = "preloadsound";
        protected const string XMLATTRIB_PRELOADMUSIC           = "preloadmusic";
        #endregion
      
        #region Fields
        protected Files.ResourceManager resourceManager;
        protected uint resourcesversion;
        protected string resourcespath;
        protected bool preloadrooms;
        protected bool preloadobjects;
        protected bool preloadroomtextures;
        protected bool preloadsound;
        protected bool preloadmusic;
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
        /// ResourceManager instance
        /// </summary>
        public Meridian59.Files.ResourceManager ResourceManager
        {
            get { return resourceManager; }
            set
            {
                if (resourceManager != value)
                {
                    resourceManager = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEMANAGER));
                }
            }
        }
        
        public int CountRooms
        {
            get { return (resourceManager != null) ? resourceManager.Rooms.Count : 0; }
        }

        public int CountObjects
        {
            get { return (resourceManager != null) ? resourceManager.Objects.Count : 0; }
        }

        public int CountRoomTextures
        {
            get { return (resourceManager != null) ? resourceManager.RoomTextures.Count : 0; }
        }

        public int CountSounds
        {
            get { return (resourceManager != null) ? resourceManager.Wavs.Count : 0; }
        }

        public int CountMusic
        {
            get { return (resourceManager != null) ? resourceManager.Music.Count : 0; }
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
        /// Constructor. Will load configuration file directly.
        /// </summary>
        public Config()
        {
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

            // rootnode
            reader.ReadToFollowing(XMLTAG_CONFIGURATION);

            // resources
            reader.ReadToFollowing(XMLTAG_RESOURCES);
            ResourcesPath       = reader[XMLATTRIB_PATH];
            ResourcesVersion    = Convert.ToUInt32(reader[XMLATTRIB_VERSION]);
            PreloadRooms        = Convert.ToBoolean(reader[XMLATTRIB_PRELOADROOMS]);
            PreloadObjects      = Convert.ToBoolean(reader[XMLATTRIB_PRELOADOBJECTS]);
            PreloadRoomTextures = Convert.ToBoolean(reader[XMLATTRIB_PRELOADROOMTEXTURES]);
            PreloadSound        = Convert.ToBoolean(reader[XMLATTRIB_PRELOADSOUND]);
            PreloadMusic        = Convert.ToBoolean(reader[XMLATTRIB_PRELOADMUSIC]);

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

            // resources
            writer.WriteStartElement(XMLTAG_RESOURCES);
            writer.WriteAttributeString(XMLATTRIB_VERSION, ResourcesVersion.ToString());
            writer.WriteAttributeString(XMLATTRIB_PATH, ResourcesPath.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADROOMS, PreloadRooms.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADOBJECTS, PreloadObjects.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADROOMTEXTURES, PreloadRoomTextures.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADSOUND, PreloadSound.ToString().ToLower());
            writer.WriteAttributeString(XMLATTRIB_PRELOADMUSIC, PreloadMusic.ToString().ToLower());
            writer.WriteEndElement();

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
