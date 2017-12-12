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
using Meridian59.Common;
using Meridian59.Common.Constants;

namespace Meridian59.Bot.Spell
{
    /// <summary>
    /// Reads SpellBot configuration file
    /// </summary>
    public class SpellBotConfig : BotConfig
    {
        #region Constants
        public new const string CONFIGFILE      = "../configuration.spellbot.xml";
        public new const string CONFIGFILE_ALT  = "configuration.spellbot.xml";

        public const string XMLTAG_TEMPLATES    = "templates";
        public const string XMLTAG_TEMPLATE     = "template";
        public const string XMLTAG_TASK         = "task";
        public const string XMLATTRIB_TYPE      = "type";
        public const string XMLATTRIB_TARGET    = "target";
        public const string XMLATTRIB_TEXT      = "text";
        public const string XMLATTRIB_DURATION  = "duration";
        public const string XMLATTRIB_IN        = "in";
        public const string XMLATTRIB_ONMAX     = "onmax";
        public const string XMLATTRIB_CAP       = "cap";
        public const string XMLATTRIB_TEMPLATE  = "template";
        public const string XMLVALUE_CAST       = "cast";
        public const string XMLVALUE_USE        = "use";
        public const string XMLVALUE_REST       = "rest";
        public const string XMLVALUE_STAND      = "stand";
        public const string XMLVALUE_SLEEP      = "sleep";
        public const string XMLVALUE_SAY        = "say";
        public const string XMLVALUE_ROOM       = "room";
        public const string XMLVALUE_INVENTORY  = "inventory";
        public const string XMLVALUE_QUIT       = "quit";
        public const string XMLVALUE_SKIP       = "skip";
        public const string XMLVALUE_SELF       = "self";

        public const string DEFAULTVAL_SPELLBOT_TEMPLATE = "";
        public const string DEFAULTVAL_SPELLBOT_TEMPLATENAME = "";
        public const string DEFAULTVAL_SPELLBOT_CAST_NAME = "";
        public const string DEFAULTVAL_SPELLBOT_CAST_TARGET = "";
        public const string DEFAULTVAL_SPELLBOT_CAST_WHERE = "";
        public const string DEFAULTVAL_SPELLBOT_CAST_ONMAX = "";
        public const uint DEFAULTVAL_SPELLBOT_SLEEP_DURATION = 1;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected int currentTask = 0;

        #region Properties
        public override string ConfigFile { get { return SpellBotConfig.CONFIGFILE; } }
        public override string ConfigFileAlt { get { return SpellBotConfig.CONFIGFILE_ALT; } }
        public Template ActiveTemplate { get; protected set; }
        public List<Template> Templates { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SpellBotConfig()
            : base() { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPreConfig()
        {
            base.InitPreConfig();

            Templates = new List<Template>();           
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
        /// <returns></returns>
        public BotTask GetNextTask()
        {
            BotTask returnValue = null;

            if (ActiveTemplate != null)
            {
                // get
                if (ActiveTemplate.Tasks.Count > currentTask)
                    returnValue = ActiveTemplate.Tasks[currentTask];

                // raise or reset
                if (ActiveTemplate.Tasks.Count > currentTask + 1)
                    currentTask++;

                else
                    currentTask = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Document"></param>
        public override void ReadXml(XmlDocument Document)
        {
            // read baseclass part
            base.ReadXml(Document);

            XmlNode node;

            // vars for reading
            string activetemplate;


            // bot

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT);

            if (node != null)
            {
                activetemplate = (node.Attributes[XMLATTRIB_TEMPLATE] != null) ?
                    node.Attributes[XMLATTRIB_TEMPLATE].Value : DEFAULTVAL_SPELLBOT_TEMPLATE;
            }
            else
            {
                activetemplate = DEFAULTVAL_SPELLBOT_TEMPLATE;
            }


            // templates
            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT + '/' + XMLTAG_TEMPLATES);

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_TEMPLATE)
                        continue;

                    Templates.Add(ReadTemplate(child));
                }
            }

            //

            // find active one from templates
            ActiveTemplate = GetTemplateByName(activetemplate);                     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reader"></param>
        protected Template ReadTemplate(XmlNode Reader)
        {
            Template template = new Template();

            template.Name = (Reader.Attributes[XMLATTRIB_NAME] != null) ?
                Reader.Attributes[XMLATTRIB_NAME].Value : DEFAULTVAL_SPELLBOT_TEMPLATENAME;
          
            string type;
            string name;
            string target;
            string text;
            string where;
            string onmax;
            uint cap;
            uint duration;

            foreach (XmlNode child in Reader.ChildNodes)
            {
                if (child.Name != XMLTAG_TASK)
                    continue;

                type = (child.Attributes[XMLATTRIB_TYPE] != null) ?
                    child.Attributes[XMLATTRIB_TYPE].Value : null;

                switch (type)
                {
                    case XMLVALUE_CAST:
                        name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                            child.Attributes[XMLATTRIB_NAME].Value : DEFAULTVAL_SPELLBOT_CAST_NAME;
                        target = (child.Attributes[XMLATTRIB_TARGET] != null) ?
                            child.Attributes[XMLATTRIB_TARGET].Value : DEFAULTVAL_SPELLBOT_CAST_TARGET;
                        where = (child.Attributes[XMLATTRIB_IN] != null) ?
                            child.Attributes[XMLATTRIB_IN].Value : DEFAULTVAL_SPELLBOT_CAST_WHERE;
                        onmax = (child.Attributes[XMLATTRIB_ONMAX] != null) ?
                            child.Attributes[XMLATTRIB_ONMAX].Value : DEFAULTVAL_SPELLBOT_CAST_ONMAX;
                        cap = (child.Attributes[XMLATTRIB_CAP] != null && UInt32.TryParse(child.Attributes[XMLATTRIB_CAP].Value, out cap)) ?
                            Math.Min(StatNumsValues.SKILLMAX, cap) : StatNumsValues.SKILLMAX;

                        template.Tasks.Add(new BotTaskCast(name, target, where, onmax, cap));
                        break;

                    case XMLVALUE_USE:
                        name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                            child.Attributes[XMLATTRIB_NAME].Value : DEFAULTVAL_SPELLBOT_CAST_NAME;
                        template.Tasks.Add(new BotTaskUse(name));
                        break;

                    case XMLVALUE_REST:
                        template.Tasks.Add(new BotTaskRest());
                        break;

                    case XMLVALUE_STAND:
                        template.Tasks.Add(new BotTaskStand());
                        break;

                    case XMLVALUE_SLEEP:
                        duration = (child.Attributes[XMLATTRIB_DURATION] != null && UInt32.TryParse(child.Attributes[XMLATTRIB_DURATION].Value, out duration)) ?
                            duration * GameTick.MSINSECOND : DEFAULTVAL_SPELLBOT_SLEEP_DURATION * GameTick.MSINSECOND;

                        template.Tasks.Add(new BotTaskSleep(duration));
                        break;

                    case XMLVALUE_SAY:
                        text = (child.Attributes[XMLATTRIB_TEXT] != null) ?
                            child.Attributes[XMLATTRIB_TEXT].Value : null;

                        if (text != null)
                            template.Tasks.Add(new BotTaskSay(text));
                        break;

                }
            }

            return template;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Writer"></param>
        public override void WriteXml(XmlWriter Writer)
        {
            base.WriteXml(Writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Template GetTemplateByName(string Name)
        {
            foreach (Template template in Templates)
                if (template.Name == Name)
                    return template;

            return null;
        }
    }
}
