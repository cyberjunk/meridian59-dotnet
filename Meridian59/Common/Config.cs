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
        /// <summary>
        /// Filename of configuration to load
        /// </summary>
        public const string CONFIGFILE = "configuration.xml";
        
        protected const string XMLTAG_CONFIGURATION = "configuration";
        
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

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
            XmlReader xmlReader = XmlReader.Create(CONFIGFILE);

            // load from xml file
            ReadXml(xmlReader);

            // end
            xmlReader.Close();
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
