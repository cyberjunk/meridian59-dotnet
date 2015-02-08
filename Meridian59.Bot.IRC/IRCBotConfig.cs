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

namespace Meridian59.Bot.IRC
{
    /// <summary>
    /// Reads IRCBot configuration file
    /// </summary>
    public class IRCBotConfig : BotConfig
    {
        #region Constants
        protected const string XMLTAG_ADMINCOMMANDS     = "admincommands";
        protected const string XMLATTRIB_IRCSERVER      = "ircserver";        
        protected const string XMLATTRIB_IRCPORT        = "ircport";
        protected const string XMLATTRIB_CHANNEL        = "channel";
        protected const string XMLATTRIB_NICKNAME       = "nickname";
        protected const string XMLATTRIB_IRCPASSWORD    = "ircpassword";
        protected const string XMLATTRIB_CHATPREFIX     = "chatprefix";
        protected const string XMLATTRIB_MAXBURST       = "maxburst";
        protected const string XMLATTRIB_REFILL         = "refill";
        protected const string XMLATTRIB_BANNER         = "banner";
        #endregion

        #region Properties
        public string IRCServer { get; protected set; }
        public ushort IRCPort { get; protected set; }
        public string Channel { get; protected set; }
        public string NickName { get; protected set; }
        public string IRCPassword { get; protected set; }
        public string ChatPrefix { get; protected set; }
        public List<string> AdminCommands { get; protected set; }
        public uint MaxBurst { get; protected set; }
        public uint Refill { get; protected set; }
        public string Banner { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public IRCBotConfig()
            : base() { }
                        
        /// <summary>
        /// 
        /// </summary>
        protected override void InitPreConfig()
        {
            base.InitPreConfig();

            AdminCommands = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPastConfig()
        {
            base.InitPastConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reader"></param>
        public override void ReadXml(XmlReader Reader)
        {
            // read baseclass part
            base.ReadXml(Reader);

            // bot
            Reader.ReadToFollowing(XMLTAG_BOT);
            IRCServer = Reader[XMLATTRIB_IRCSERVER];
            IRCPort = Convert.ToUInt16(Reader[XMLATTRIB_IRCPORT]);
            Channel = Reader[XMLATTRIB_CHANNEL];
            NickName = Reader[XMLATTRIB_NICKNAME];
            IRCPassword = Reader[XMLATTRIB_IRCPASSWORD];
            ChatPrefix = Reader[XMLATTRIB_CHATPREFIX];
            MaxBurst = Convert.ToUInt32(Reader[XMLATTRIB_MAXBURST]);
            Refill = Convert.ToUInt32(Reader[XMLATTRIB_REFILL]);
            Banner = Reader[XMLATTRIB_BANNER];

            // admincommands list
            AdminCommands.Clear();
            Reader.ReadToFollowing(XMLTAG_ADMINCOMMANDS);
            if (Reader.ReadToDescendant(XMLTAG_ITEM))
            {
                do
                {
                    AdminCommands.Add(Reader[XMLATTRIB_NAME]);
                }
                while (Reader.ReadToNextSibling(XMLTAG_ITEM));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Writer"></param>
        public override void WriteXml(XmlWriter Writer)
        {
            base.WriteXml(Writer);
        }
    }
}
