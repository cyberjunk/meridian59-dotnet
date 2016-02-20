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
        protected const string XMLTAG_RELAYBOTS         = "relaybots";
        protected const string XMLATTRIB_IRCSERVER      = "ircserver";
        protected const string XMLATTRIB_IRCPORT        = "ircport";
        protected const string XMLATTRIB_CHANNEL        = "channel";
        protected const string XMLATTRIB_NICKNAME       = "nickname";
        protected const string XMLATTRIB_IRCPASSWORD    = "ircpassword";
        protected const string XMLATTRIB_CHATPREFIX     = "chatprefix";
        protected const string XMLATTRIB_MAXBURST       = "maxburst";
        protected const string XMLATTRIB_REFILL         = "refill";
        protected const string XMLATTRIB_BANNER         = "banner";
        protected const string XMLATTRIB_IGNOREREGEX    = "ignoreregex";

        public const string DEFAULTVAL_IRCBOT_IRCSERVER     = "irc.esper.net";
        public const ushort DEFAULTVAL_IRCBOT_IRCPORT       = 7000;
        public const string DEFAULTVAL_IRCBOT_CHANNEL       = "#Meridian59";
        public const string DEFAULTVAL_IRCBOT_NICKNAME      = "M59-IRC-Bot";
        public const string DEFAULTVAL_IRCBOT_IRCPASSWORD   = "";
        public const string DEFAULTVAL_IRCBOT_CHATPREFIX    = "";
        public const uint   DEFAULTVAL_IRCBOT_MAXBURST      = 10;
        public const uint   DEFAULTVAL_IRCBOT_REFILL        = 1000;
        public const string DEFAULTVAL_IRCBOT_BANNER        = "";
        #endregion

        #region Properties
        public string IRCServer { get; protected set; }
        public ushort IRCPort { get; protected set; }
        public string Channel { get; protected set; }
        public string NickName { get; protected set; }
        public string IRCPassword { get; protected set; }
        public string ChatPrefix { get; protected set; }
        public List<string> AdminCommands { get; protected set; }
        public List<RelayConfig> RelayBots { get; protected set; }
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
            RelayBots = new List<RelayConfig>();
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
        /// <param name="Document"></param>
        public override void ReadXml(XmlDocument Document)
        {
            // read baseclass part
            base.ReadXml(Document);

            ushort val_ushort;
            uint val_uint;
            XmlNode node;

            AdminCommands.Clear();
            RelayBots.Clear();

            // bot

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT);

            if (node != null)
            {
                IRCServer = (node.Attributes[XMLATTRIB_IRCSERVER] != null) ?
                    node.Attributes[XMLATTRIB_IRCSERVER].Value : DEFAULTVAL_IRCBOT_IRCSERVER;

                IRCPort = (node.Attributes[XMLATTRIB_IRCPORT] != null && UInt16.TryParse(node.Attributes[XMLATTRIB_IRCPORT].Value, out val_ushort)) ?
                    val_ushort : DEFAULTVAL_IRCBOT_IRCPORT;

                Channel = (node.Attributes[XMLATTRIB_CHANNEL] != null) ?
                    node.Attributes[XMLATTRIB_CHANNEL].Value : DEFAULTVAL_IRCBOT_CHANNEL;

                NickName = (node.Attributes[XMLATTRIB_NICKNAME] != null) ?
                    node.Attributes[XMLATTRIB_NICKNAME].Value : DEFAULTVAL_IRCBOT_NICKNAME;

                IRCPassword = (node.Attributes[XMLATTRIB_IRCPASSWORD] != null) ?
                    node.Attributes[XMLATTRIB_IRCPASSWORD].Value : DEFAULTVAL_IRCBOT_IRCPASSWORD;

                ChatPrefix = (node.Attributes[XMLATTRIB_CHATPREFIX] != null) ?
                    node.Attributes[XMLATTRIB_CHATPREFIX].Value : DEFAULTVAL_IRCBOT_CHATPREFIX;

                MaxBurst = (node.Attributes[XMLATTRIB_MAXBURST] != null && UInt32.TryParse(node.Attributes[XMLATTRIB_MAXBURST].Value, out val_uint)) ?
                    val_uint : DEFAULTVAL_IRCBOT_MAXBURST;

                Refill = (node.Attributes[XMLATTRIB_REFILL] != null && UInt32.TryParse(node.Attributes[XMLATTRIB_REFILL].Value, out val_uint)) ?
                    val_uint : DEFAULTVAL_IRCBOT_REFILL;

                Banner = (node.Attributes[XMLATTRIB_BANNER] != null) ?
                    node.Attributes[XMLATTRIB_BANNER].Value : DEFAULTVAL_IRCBOT_BANNER;
            }      
            else
            {
                IRCServer = DEFAULTVAL_IRCBOT_IRCSERVER;
                IRCPort = DEFAULTVAL_IRCBOT_IRCPORT;
                Channel = DEFAULTVAL_IRCBOT_CHANNEL;
                NickName = DEFAULTVAL_IRCBOT_NICKNAME;
                IRCPassword = DEFAULTVAL_IRCBOT_IRCPASSWORD;
                ChatPrefix = DEFAULTVAL_IRCBOT_CHATPREFIX;
                MaxBurst = DEFAULTVAL_IRCBOT_MAXBURST;
                Refill = DEFAULTVAL_IRCBOT_REFILL;
                Banner = DEFAULTVAL_IRCBOT_BANNER;
            }

            // admincommands list

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT + '/' + XMLTAG_ADMINCOMMANDS);

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_ITEM)
                        continue;

                    string name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                        child.Attributes[XMLATTRIB_NAME].Value : null;

                    if (name != null)
                        AdminCommands.Add(name);
                }
            }

            // Relay bots (bots we pass on messages from)

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT + '/' + XMLTAG_RELAYBOTS);

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_ITEM)
                        continue;

                    string name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                        child.Attributes[XMLATTRIB_NAME].Value : null;
                    string banner = (child.Attributes[XMLATTRIB_BANNER] != null) ?
                        child.Attributes[XMLATTRIB_BANNER].Value : null;
                    string regex = (child.Attributes[XMLATTRIB_IGNOREREGEX].Value != null) ?
                        child.Attributes[XMLATTRIB_IGNOREREGEX].Value : null;
                    if (name != null && banner != null && regex != null)
                        RelayBots.Add(new RelayConfig(name,banner,regex));
                }
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
