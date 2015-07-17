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
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected int currentTask = 0;

        #region Properties
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
        /// <param name="Reader"></param>
        public override void ReadXml(XmlReader Reader)
        {
            // read baseclass part
            base.ReadXml(Reader);

            // vars for reading
            string activetemplate;

            // bot
            Reader.ReadToFollowing(XMLTAG_BOT);
            activetemplate = Reader[XMLATTRIB_TEMPLATE];

            // templates
            Reader.ReadToFollowing(XMLTAG_TEMPLATES);
            if (Reader.ReadToDescendant(XMLTAG_TEMPLATE))
            {
                do
                {
                    Templates.Add(ReadTemplate(Reader));
                }               
                while (Reader.ReadToNextSibling(XMLTAG_TEMPLATE));
            }

            // find active one from templates
            ActiveTemplate = GetTemplateByName(activetemplate);                     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reader"></param>
        protected Template ReadTemplate(XmlReader Reader)
        {
            Template template = new Template();
            template.Name = Reader[XMLATTRIB_NAME];

            string type;
            string name;
            string target;
            string text;
            string where;
            string onmax;
            uint cap;
            uint duration;
            
            if (Reader.ReadToDescendant(XMLTAG_TASK))
            {
                do
                {
                    type = Reader[XMLATTRIB_TYPE].ToLower();

                    switch (type)
                    {
                        case XMLVALUE_CAST:
                            name = Reader[XMLATTRIB_NAME];
                            target = Reader[XMLATTRIB_TARGET];
                            where = Reader[XMLATTRIB_IN];
                            onmax = Reader[XMLATTRIB_ONMAX];

                            cap = (Reader[XMLATTRIB_CAP] == null) ? StatNumsValues.SKILLMAX : 
                                Math.Min(StatNumsValues.SKILLMAX, Convert.ToUInt32(Reader[XMLATTRIB_CAP]));

                            template.Tasks.Add(new BotTaskCast(name, target, where, onmax, cap));
                            break;

                        case XMLVALUE_USE:
                            name = Reader[XMLATTRIB_NAME];
                            template.Tasks.Add(new BotTaskUse(name));
                            break;

                        case XMLVALUE_REST:
                            template.Tasks.Add(new BotTaskRest());
                            break;

                        case XMLVALUE_STAND:
                            template.Tasks.Add(new BotTaskStand());
                            break;

                        case XMLVALUE_SLEEP:
                            duration = GameTick.MSINSECOND * Convert.ToUInt32(Reader[XMLATTRIB_DURATION]);
                            template.Tasks.Add(new BotTaskSleep(duration));
                            break;

                        case XMLVALUE_SAY:
                            text = Reader[XMLATTRIB_TEXT];
                            template.Tasks.Add(new BotTaskSay(text));
                            break;
                    }
                }
                while (Reader.ReadToNextSibling(XMLTAG_TASK));
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
