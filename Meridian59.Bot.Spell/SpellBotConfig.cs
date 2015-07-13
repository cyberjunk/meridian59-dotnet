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
        public const string XMLATTRIB_AMOUNT    = "amount";
        public const string XMLATTRIB_INVMAX    = "invmax";
        public const string XMLATTRIB_TEMPLATE  = "template";
        public const string XMLATTRIB_STAT      = "stat";
        public const string XMLATTRIB_CAP       = "cap";
        public const string XMLVALUE_CAST       = "cast";
        public const string XMLVALUE_EAT        = "eat";
        public const string XMLVALUE_USE        = "use";
        public const string XMLVALUE_EQUIP      = "equip";
        public const string XMLVALUE_DISCARD    = "discard";
        public const string XMLVALUE_BUY        = "buy";
        public const string XMLVALUE_REST       = "rest";
        public const string XMLVALUE_STAND      = "stand";
        public const string XMLVALUE_SLEEP      = "sleep";
        public const string XMLVALUE_SAY        = "say";
        public const string XMLVALUE_ROOM       = "room";
        public const string XMLVALUE_INVENTORY  = "inventory";
        public const string XMLVALUE_QUIT       = "quit";
        public const string XMLVALUE_SKIP       = "skip";
        public const string XMLVALUE_MULTICAST  = "multicast";
        public const string XMLVALUE_STARTLOOP  = "loop";
        public const string XMLVALUE_ENDLOOP    = "endloop";
        public const string XMLVALUE_RECOVER    = "recover";

        public const string XMLVALUE_MOVE       = "move";
        public const string XMLATTRIB_DOOR      = "door";

        //public const string XMLVALUE_SELNPC     = "selectnpc";
        //public const string XMLVALUE_SENDBUY    = "sendbuy";
        //public const string XMLVALUE_BUYITEM    = "buyitem";

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

            int loopOpenCnt = 0;
            LinkedList<uint> loopAmountStack = new LinkedList<uint>();
            LinkedList<LinkedList<BotTask>> dynTaskListStack = new LinkedList<LinkedList<BotTask>>();

            string type;
            string name;
            string target;
            string text;
            string where;
            string onmax;
            string stat;
            uint cap;
            uint amount;
            uint duration;
            uint maxamount;

            string door;

            // Parse the BotSkript to Template
            if (Reader.ReadToDescendant(XMLTAG_TASK))
            {
                do
                {
                    type = Reader[XMLATTRIB_TYPE].ToLower();

                    #region switch
                    switch (type)
                    {
                        case XMLVALUE_STARTLOOP:
                            ++loopOpenCnt;
                            loopAmountStack.AddLast(Convert.ToUInt32(Reader[XMLATTRIB_AMOUNT]));
                            dynTaskListStack.AddLast(new LinkedList<BotTask>());
                            break;

                        case XMLVALUE_ENDLOOP:
                            if (loopOpenCnt > 0)
                            {
                                for (int i = 0; i < loopAmountStack.Last.Value ; ++i)
                                    template.Tasks.AddRange(dynTaskListStack.Last.Value);
                                --loopOpenCnt;
                                loopAmountStack.RemoveLast();
                                dynTaskListStack.RemoveLast();
                            }
                            break;

                        case XMLVALUE_CAST:
                            name = Reader[XMLATTRIB_NAME];
                            target = Reader[XMLATTRIB_TARGET];
                            where = Reader[XMLATTRIB_IN];
                            onmax = Reader[XMLATTRIB_ONMAX];
                            cap = Convert.ToUInt32(Reader[XMLATTRIB_CAP]);

                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskCast(name, target, where, onmax, cap));
                            else
                                template.Tasks.Add(new BotTaskCast(name, target, where, onmax, cap));
                            break;

                        case XMLVALUE_BUY:
                            name = Reader[XMLATTRIB_NAME];
                            target = Reader[XMLATTRIB_TARGET];
                            amount = Convert.ToUInt32(Reader[XMLATTRIB_AMOUNT]);
                            maxamount = Convert.ToUInt32(Reader[XMLATTRIB_INVMAX]);

                            if (loopOpenCnt > 0)
                            {
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSelectNPC(target));
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSleep(GameTick.MSINSECOND));
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSendBuy());
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSleep(GameTick.MSINSECOND));
                                dynTaskListStack.Last.Value.AddLast(new BotTaskBuyItem(name, amount, maxamount));
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSleep(GameTick.MSINSECOND));
                            }
                            else
                            {
                                template.Tasks.Add(new BotTaskSelectNPC(target));
                                template.Tasks.Add(new BotTaskSleep(GameTick.MSINSECOND));
                                template.Tasks.Add(new BotTaskSendBuy());
                                template.Tasks.Add(new BotTaskSleep(GameTick.MSINSECOND));
                                template.Tasks.Add(new BotTaskBuyItem(name, amount, maxamount));
                                template.Tasks.Add(new BotTaskSleep(GameTick.MSINSECOND));
                            }
                            break;

                        case XMLVALUE_MULTICAST:
                            name = Reader[XMLATTRIB_NAME];
                            target = Reader[XMLATTRIB_TARGET];
                            where = Reader[XMLATTRIB_IN];
                            onmax = Reader[XMLATTRIB_ONMAX];
                            cap = Convert.ToUInt32(Reader[XMLATTRIB_CAP]);
                            amount = Convert.ToUInt32(Reader[XMLATTRIB_AMOUNT]);
                            duration = GameTick.MSINSECOND * Convert.ToUInt32(Reader[XMLATTRIB_DURATION]);

                            if (loopOpenCnt > 0)
                                for (int i = 0; i < amount; ++i)
                                {
                                    dynTaskListStack.Last.Value.AddLast(new BotTaskCast(name, target, where, onmax, cap));
                                    dynTaskListStack.Last.Value.AddLast(new BotTaskSleep(duration));
                                }
                            else
                                for (int i = 0; i < amount; ++i)
                                {
                                    template.Tasks.Add(new BotTaskCast(name, target, where, onmax, cap));
                                    template.Tasks.Add(new BotTaskSleep(duration));
                                }
                            break;

                        case XMLVALUE_EAT:
                            name = Reader[XMLATTRIB_NAME];
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskEat(name));
                            else
                                template.Tasks.Add(new BotTaskEat(name));
                            break;

                        case XMLVALUE_USE:
                            name = Reader[XMLATTRIB_NAME];
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskUse(name));
                            else
                                template.Tasks.Add(new BotTaskUse(name));
                            break;

                        case XMLVALUE_EQUIP:
                            name = Reader[XMLATTRIB_NAME];
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskEquip(name));
                            else
                                template.Tasks.Add(new BotTaskEquip(name));
                            break;

                        case XMLVALUE_DISCARD:
                            name = Reader[XMLATTRIB_NAME];
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskDiscard(name));
                            else
                                template.Tasks.Add(new BotTaskDiscard(name));
                            break;

                        case XMLVALUE_REST:
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskRest());
                            else
                                template.Tasks.Add(new BotTaskRest());
                            break;

                        case XMLVALUE_RECOVER:
                            stat = Reader[XMLATTRIB_STAT];
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskRecover(stat));
                            else
                                template.Tasks.Add(new BotTaskRecover(stat));
                            break;

                        case XMLVALUE_STAND:
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskStand());
                            else
                                template.Tasks.Add(new BotTaskStand());
                            break;

                        case XMLVALUE_SLEEP:
                            duration = GameTick.MSINSECOND * Convert.ToUInt32(Reader[XMLATTRIB_DURATION]);
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSleep(duration));
                            else
                                template.Tasks.Add(new BotTaskSleep(duration));
                            break;

                        case XMLVALUE_SAY:
                            text = Reader[XMLATTRIB_TEXT];
                            if (loopOpenCnt > 0)
                                dynTaskListStack.Last.Value.AddLast(new BotTaskSay(text));
                            else
                                template.Tasks.Add(new BotTaskSay(text));
                            break;

                        //case XMLVALUE_MOVE :
                        //    door = Reader[XMLATTRIB_DOOR];
                        //    template.Tasks.Add(new BotTaskMove(door));
                        //    template.Tasks.Add(new BotTaskSleep(GameTick.MSINSECOND * 20));
                        //    break;

                        //case XMLVALUE_SELNPC:
                        //    name = Reader[XMLATTRIB_NAME];
                        //    template.Tasks.Add(new BotTaskSelectNPC(name));
                        //    break;

                        //case XMLVALUE_SENDBUY:
                        //    template.Tasks.Add(new BotTaskSendBuy());
                        //    break;

                        //case XMLVALUE_BUYITEM:
                        //    name = Reader[XMLATTRIB_NAME];
                        //    amount = Convert.ToUInt32(Reader[XMLATTRIB_AMOUNT]);
                        //    template.Tasks.Add(new BotTaskBuyItem(name, amount));
                        //    break;
                    }
                    #endregion
                }
                while (Reader.ReadToNextSibling(XMLTAG_TASK));

            }

            if (loopOpenCnt != 0)
                throw new Exception("Template contains illegal loop structure!");

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
