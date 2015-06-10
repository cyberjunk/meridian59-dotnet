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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Meridian59.Common;

namespace Meridian59.Bot
{
    /// <summary>
    /// Reads bot configuration file
    /// </summary>
    public abstract class BotConfig : Config
    {
        #region Constants
        protected const string XMLTAG_BOT                   = "bot";
        protected const string XMLTAG_ADMINS                = "admins";
        protected const string XMLTAG_ITEM                  = "item";
        protected const string XMLATTRIB_MAJORVERSION       = "majorversion";
        protected const string XMLATTRIB_MINORVERSION       = "minorversion";
        protected const string XMLATTRIB_LOGFILE            = "logfile";
        #endregion

        #region Properties
        public byte MajorVersion { get; protected set; }
        public byte MinorVersion { get; protected set; }
        public string LogFile { get; protected set; }
        public List<String> Admins { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public BotConfig() : base()
        {
        }

        /// <summary>
        /// Override with init code called before configload in parentconstructor
        /// </summary>
        protected override void InitPreConfig()
        {
            Admins = new List<string>();
        }

        /// <summary>
        /// Override with init code called after configload
        /// </summary>
        protected override void InitPastConfig()
        {
        }

        /// <summary>
        /// Says whether a name is on the admin list
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool IsAdmin(string Name)
        {
            foreach (string s in Admins)
                if (String.Equals(Name, s))
                    return true;

            return false;
        }

        /// <summary>
        /// Whether a logfile is set or not
        /// </summary>
        /// <returns></returns>
        public bool HasLogFile()
        {
            return LogFile != null && LogFile != String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reader"></param>
        public override void ReadXml(XmlReader Reader)
        {
            base.ReadXml(Reader);
           
            // connection
            Reader.ReadToFollowing(XMLTAG_CONNECTION);
            MajorVersion = Convert.ToByte(Reader[XMLATTRIB_MAJORVERSION]);
            MinorVersion = Convert.ToByte(Reader[XMLATTRIB_MINORVERSION]);
            LogFile = Reader[XMLATTRIB_LOGFILE];

            // admins list
            Admins.Clear();
            Reader.ReadToFollowing(XMLTAG_ADMINS);
            if (Reader.ReadToDescendant(XMLTAG_ITEM))
            {
                do
                {
                    Admins.Add(Reader[XMLATTRIB_NAME]);
                }
                while (Reader.ReadToNextSibling(XMLTAG_ITEM));
            }
        }
    }
}
