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
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Data;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A command parsed from a user input / text (i.e. say)
    /// </summary>
    [Serializable]
    public abstract class ChatCommand
    {
        public const char DELIMITER = ' ';
        public const char QUOTECHAR = '\"';
        protected const string ON   = "on";
        protected const string OFF  = "off";

        public abstract ChatCommandType CommandType { get; }
       
        /// <summary>
        /// Parses a ChatCommand instance from string input and available data.
        /// </summary>
        /// <param name="Text">Text input to parse</param>
        /// <param name="DataController">Reference to DataController instance</param>
        /// <param name="Config">Reference to Config instance</param>
        /// <returns></returns>
        public static ChatCommand Parse(string Text, DataController DataController, Config Config)
        {            
            string command          = null;
            string lower            = null;
            string text             = null;
            string alias            = null;
            string[] splitted       = null;
            ChatCommand returnValue = null;
            
            /**********************************************************************/
            // checks
            
            if (Text == null || DataController == null || Config == null)
                return null;

            lower = Text.Trim();

            if (String.Equals(lower, String.Empty))
                return null;

            /**********************************************************************/
            // resolve aliases

            int idx = lower.IndexOf(DELIMITER);
            if (idx == -1)
            {
                // only one word which might be an alias
                KeyValuePairString aliascmd = Config.Aliases.GetItemByKey(lower);

                if (aliascmd != null)
                    lower = aliascmd.Value;
            }
            else
            {
                // first word might be alias, but there is more
                alias = lower.Substring(0, idx);

                KeyValuePairString aliascmd = Config.Aliases.GetItemByKey(alias);

                if (aliascmd != null)
                    lower = aliascmd.Value + lower.Substring(idx + 1); 
            }

            /**********************************************************************/

            // split up by delimiter
            splitted = lower.Split(DELIMITER);

            // check
            if (splitted.Length == 0)
                return null;

            // command is first argument
            command = splitted[0];

            // select command
            switch (command)
            {
                case ChatCommandMacro.KEY1:
                case ChatCommandMacro.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandMacro(text);
                    }
                    break;

                case ChatCommandSay.KEY1:
                case ChatCommandSay.KEY2:
                case ChatCommandSay.KEY3:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandSay(text);
                    }
                    break;
                
                case ChatCommandEmote.KEY1:
                case ChatCommandEmote.KEY2:
                case ChatCommandEmote.KEY3:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandEmote(text);
                    }
                    break;
                
                case ChatCommandYell.KEY1:
                case ChatCommandYell.KEY2:
                case ChatCommandYell.KEY3:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandYell(text);
                    }
                    break;

                case ChatCommandBroadcast.KEY1:
                case ChatCommandBroadcast.KEY2:
                case ChatCommandBroadcast.KEY3:
                case ChatCommandBroadcast.KEY4:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandBroadcast(text);
                    }
                    break;

                case ChatCommandGuild.KEY1:
                case ChatCommandGuild.KEY2:
                case ChatCommandGuild.KEY3:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandGuild(text);
                    }
                    break;

                case ChatCommandAppeal.KEY1:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandAppeal(text);
                    }
                    break;

                case ChatCommandTell.KEY1:
                case ChatCommandTell.KEY2:
                case ChatCommandTell.KEY3:
                    returnValue = ParseTell(splitted, lower, DataController);                                           
                    break;
                
                case ChatCommandCast.KEY1:
                case ChatCommandCast.KEY2:
                case ChatCommandCast.KEY3:
                case ChatCommandCast.KEY4:
                    returnValue = ParseCast(splitted, lower, DataController);
                    break;

                case ChatCommandDeposit.KEY1:
                case ChatCommandDeposit.KEY2:
                    if (splitted.Length == 2)
                    {
                        uint amount = 0;

                        if (UInt32.TryParse(splitted[1], out amount))
                            returnValue = new ChatCommandDeposit(amount);
                    }
                    break;

                case ChatCommandWithDraw.KEY1:
                case ChatCommandWithDraw.KEY2:
                case ChatCommandWithDraw.KEY3:
                    if (splitted.Length == 2)
                    {
                        uint amount = 0;

                        if (UInt32.TryParse(splitted[1], out amount))
                            returnValue = new ChatCommandWithDraw(amount);
                    }     
                    break;

                case ChatCommandBalance.KEY1:
                case ChatCommandBalance.KEY2:
                    if (splitted.Length == 1)
                    {
                        returnValue = new ChatCommandBalance();
                    }
                    break;

                case ChatCommandSuicide.KEY1:
                case ChatCommandSuicide.KEY2:
                    if (splitted.Length == 1)
                    {
                        returnValue = new ChatCommandSuicide();
                    }
                    break;

                case ChatCommandDM.KEY1:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandDM(text);
                    }
                    break;

                case ChatCommandGo.KEY1:
                case ChatCommandGo.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandGo(text);
                    }
                    break;

                case ChatCommandGoPlayer.KEY1:
                    returnValue = ParseGoPlayer(splitted, lower, DataController);                  
                    break;

                case ChatCommandGetPlayer.KEY1:
                    returnValue = ParseGetPlayer(splitted, lower, DataController);
                    break;

                case ChatCommandRest.KEY1:
                case ChatCommandRest.KEY2:
                    returnValue = new ChatCommandRest();
                    break;

                case ChatCommandStand.KEY1:
                case ChatCommandStand.KEY2:
                    returnValue = new ChatCommandStand();
                    break;

                case ChatCommandQuit.KEY1:
                    returnValue = new ChatCommandQuit();
                    break;

                case ChatCommandDance.KEY1:
                case ChatCommandDance.KEY2:
                    returnValue = new ChatCommandDance();
                    break;

                case ChatCommandPoint.KEY1:
                case ChatCommandPoint.KEY2:
                    returnValue = new ChatCommandPoint();
                    break;

                case ChatCommandWave.KEY1:
                case ChatCommandWave.KEY2:
                    returnValue = new ChatCommandWave();
                    break;
#if !VANILLA
                case ChatCommandTempSafe.KEY1:
                    returnValue = ParseTempSafe(splitted, lower, DataController);
                    break;

                case ChatCommandGrouping.KEY1:
                    returnValue = ParseGrouping(splitted, lower, DataController);
                    break;

                case ChatCommandAutoLoot.KEY1:
                    returnValue = ParseAutoLoot(splitted, lower, DataController);
                    break;

                case ChatCommandAutoCombine.KEY1:
                    returnValue = ParseAutoCombine(splitted, lower, DataController);
                    break;

                case ChatCommandReagentBag.KEY1:
                    returnValue = ParseReagentBag(splitted, lower, DataController);
                    break;

                case ChatCommandSpellPower.KEY1:
                    returnValue = ParseSpellPower(splitted, lower, DataController);
                    break;

                case ChatCommandTime.KEY1:
                case ChatCommandTime.KEY2:
                    if (splitted.Length == 1)
                    {
                        returnValue = new ChatCommandTime();
                    }
                    break;
#if !OPENMERIDIAN
                case ChatCommandInvite.KEY1:
                case ChatCommandInvite.KEY2:
                case ChatCommandInvite.KEY3:
                    returnValue = ParseInvite(splitted, lower, DataController);
                    break;

                case ChatCommandPerform.KEY1:
                case ChatCommandPerform.KEY2:
                case ChatCommandPerform.KEY3:
                case ChatCommandPerform.KEY4:
                case ChatCommandPerform.KEY5:
                    returnValue = ParsePerform(splitted, lower, DataController);
                    break;
#endif
#endif
            }
            
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandTell ParseTell(string[] Words, string Text, DataController Data)
        {
            Tuple<int, int, string> quote   = null;
            ChatCommandTell command         = null;
            OnlinePlayer player             = null;
            Group group                     = null;
            string prefix                   = null;
            List<OnlinePlayer> list         = null;
            List<Group> listGroups          = null;
            int num                         = 0;
            int sum                         = 0;

            if (Words == null || Words.Length < 2)
                return null;

            // extract quoted name if second word starts with "          
            // this is necessary to not care about quoted text (t someone "yes yes")
            // but only for quoted names (t "mister x" hello!)
            if (Words[1].Length > 0 && Words[1][0] == QUOTECHAR)
                quote = Text.GetQuote();

            /********* QUOTED NAME *********/
            if (quote != null)
            {
                // try get exact player and group match for quoted name
                player = Data.OnlinePlayers.GetItemByName(quote.Item3);
                group = Data.Groups.GetItemByName(quote.Item3);

                // player or group match
                if (group != null || player != null)
                {
                    // startindex of actual text
                    int idx = Words[0].Length + quote.Item2 + 2;

                    // correct tell
                    if (idx < Text.Length)
                    {
                        string sendtext = Text.Substring(idx, Text.Length - idx);

                        // prefer group match
                        if (group != null)
                        {
                            // collect ids from online-player list
                            List<uint> ids = new List<uint>();
                            foreach (GroupMember m in group.Members)
                            {
                                OnlinePlayer p = Data.OnlinePlayers.GetItemByName(m.Name);

                                if (p != null)
                                    ids.Add(p.ID);
                            }

                            // at least one person is online
                            if (ids.Count > 0)
                            {
                                command = new ChatCommandTell(ids.ToArray(), sendtext);
                            }

                            // no one of the group is online
                            else
                            {
                                Data.ChatMessages.Add(ServerString.GetServerStringForString(
                                    "No member of the group " + group.Name + " is online."));
                            }
                        }

                        // player match
                        else if (player != null)
                        {                                             
                            command = new ChatCommandTell(player.ID, sendtext);                      
                        }
                    }

                    // empty text
                    else
                    {
                        Data.ChatMessages.Add(ServerString.GetServerStringForString(
                            "Can't send empty message."));
                    }
                }
                
                // no player or group with that name
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player or group with name: " + quote.Item3));
                }
            }

            /********* UNQUOTED NAME *********/
            else
            {
                prefix = Words[1];
                list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);
                listGroups = Data.Groups.GetItemsByNamePrefix(prefix);

                // extend prefix with more words
                // until there is only one or zero matches found
                // or until there is only one more word left (supposed minimal text)
                num = 2;
                sum = list.Count + listGroups.Count;
                while (sum > 1 && num < Words.Length - 1)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);
                    listGroups = Data.Groups.GetItemsByNamePrefix(prefix);
                    sum = list.Count + listGroups.Count;
                    num++;
                }

                if (sum == 1)
                {
                    // startindex of actual text
                    int idx = Words[0].Length + prefix.Length + 2;

                    if (idx < Text.Length)
                    {
                        string sendtext = Text.Substring(idx, Text.Length - idx);

                        // to player
                        if (list.Count == 1)
                        {
                            command = new ChatCommandTell(list[0].ID, sendtext);
                        }

                        // to group
                        else if (listGroups.Count == 1)
                        {
                            // collect ids from online-player list
                            List<uint> ids = new List<uint>();
                            foreach (GroupMember m in listGroups[0].Members)
                            {
                                OnlinePlayer p = Data.OnlinePlayers.GetItemByName(m.Name);

                                if (p != null)
                                    ids.Add(p.ID);
                            }

                            // at least one person is online
                            if (ids.Count > 0)
                            {
                                command = new ChatCommandTell(ids.ToArray(), sendtext);
                            }

                            // no one of the group is online
                            else
                            {                              
                                Data.ChatMessages.Add(ServerString.GetServerStringForString(
                                    "No member of the group " + listGroups[0].Name + " is online."));
                            }
                        }
                    }
                    else
                    {
                        // empty text
                        Data.ChatMessages.Add(ServerString.GetServerStringForString(
                            "Can't send empty message."));
                    }
                }

                // still more than one player or group with max. prefix
                else if (sum > 1)
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "More than one player or group with prefix: " + prefix));
                }

                // no player or group with that prefix
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player or group with prefix: " + prefix));
                }              
            }

            return command;
        }

        /// <summary>
        /// Very much like ParseTell (copy'n'paste)!
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandGoPlayer ParseGoPlayer(string[] Words, string Text, DataController Data)
        {
            Tuple<int, int, string> quote   = null;
            ChatCommandGoPlayer command     = null;
            OnlinePlayer player             = null;
            string prefix                   = null;
            List<OnlinePlayer> list         = null;
            int num                         = 0;

            if (Words == null || Words.Length < 2)
                return null;

            // extract quoted name if second word starts with "          
            // this is necessary to not care about quoted text (t someone "yes yes")
            // but only for quoted names (t "mister x" hello!)
            if (Words[1].Length > 0 && Words[1][0] == QUOTECHAR)
                quote = Text.GetQuote();

            /********* QUOTED NAME *********/
            if (quote != null)
            {
                // try get exact match for quoted name
                player = Data.OnlinePlayers.GetItemByName(quote.Item3);

                if (player != null)               
                    command = new ChatCommandGoPlayer(player.ID);                   
                
                // no player with that name
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player with name: " + quote.Item3));
                }
            }

            /********* UNQUOTED NAME *********/
            else
            {
                prefix = Words[1];
                list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);

                // extend prefix with more words
                // until there is only one or zero matches found
                // or until there is only one more word left (supposed minimal text)
                num = 2;
                while (list.Count > 1 && num < Words.Length)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);
                    num++;
                }

                if (list.Count == 1)              
                    command = new ChatCommandGoPlayer(list[0].ID);                   
                
                // still more than one player with max. prefix
                else if (list.Count > 1)
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "More than one player with prefix: " + prefix));
                }

                // no player with that prefix
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player with prefix: " + prefix));
                }
            }

            return command;
        }
        
        /// <summary>
        /// Almost exactly like ParseGoPlayer
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandGetPlayer ParseGetPlayer(string[] Words, string Text, DataController Data)
        {
            Tuple<int, int, string> quote = null;
            ChatCommandGetPlayer command = null;
            OnlinePlayer player = null;
            string prefix = null;
            List<OnlinePlayer> list = null;
            int num = 0;

            if (Words == null || Words.Length < 2)
                return null;

            // extract quoted name if second word starts with "          
            // this is necessary to not care about quoted text (t someone "yes yes")
            // but only for quoted names (t "mister x" hello!)
            if (Words[1].Length > 0 && Words[1][0] == QUOTECHAR)
                quote = Text.GetQuote();

            /********* QUOTED NAME *********/
            if (quote != null)
            {
                // try get exact match for quoted name
                player = Data.OnlinePlayers.GetItemByName(quote.Item3);

                if (player != null)
                    command = new ChatCommandGetPlayer(player.ID);

                // no player with that name
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player with name: " + quote.Item3));
                }
            }

            /********* UNQUOTED NAME *********/
            else
            {
                prefix = Words[1];
                list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);

                // extend prefix with more words
                // until there is only one or zero matches found
                // or until there is only one more word left (supposed minimal text)
                num = 2;
                while (list.Count > 1 && num < Words.Length)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);
                    num++;
                }

                if (list.Count == 1)
                    command = new ChatCommandGetPlayer(list[0].ID);

                // still more than one player with max. prefix
                else if (list.Count > 1)
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "More than one player with prefix: " + prefix));
                }

                // no player with that prefix
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player with prefix: " + prefix));
                }
            }

            return command;
        }

        /// <summary>
        /// Almost exactly like ParseGoPlayer
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandCast ParseCast(string[] Words, string Text, DataController Data)
        {
            Tuple<int, int, string> quote   = null;
            ChatCommandCast command         = null;
            SpellObject spell              = null;
            string prefix                   = null;
            List<SpellObject> list          = null;
            int num                         = 0;

            if (Words == null || Words.Length < 2)
                return null;

            // extract quoted name if second word starts with "          
            // this is necessary to not care about quoted text (t someone "yes yes")
            // but only for quoted names (t "mister x" hello!)
            if (Words[1].Length > 0 && Words[1][0] == QUOTECHAR)
                quote = Text.GetQuote();

            /********* QUOTED NAME *********/
            if (quote != null)
            {
                // try get exact match for quoted name
                spell = Data.SpellObjects.GetItemByName(quote.Item3, false);

                if (spell != null)
                    command = new ChatCommandCast(spell);

                // no player with that name
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No spell with name: " + quote.Item3));
                }
            }

            /********* UNQUOTED NAME *********/
            else
            {
                prefix = Words[1];
                list = Data.SpellObjects.GetItemsByNamePrefix(prefix);

                // extend prefix with more words
                // until there is only one or zero matches found
                // or until there is only one more word left (supposed minimal text)
                num = 2;
                while (list.Count > 1 && num < Words.Length)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.SpellObjects.GetItemsByNamePrefix(prefix);
                    num++;
                }

                if (list.Count == 1)
                    command = new ChatCommandCast(list[0]);

                // still more than one player with max. prefix
                else if (list.Count > 1)
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "More than one spell with prefix: " + prefix));
                }

                // no spell with that prefix
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No spell with prefix: " + prefix));
                }
            }

            return command;
        }

#if !VANILLA
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandTempSafe ParseTempSafe(string[] Words, string Text, DataController Data)
        {
            ChatCommandTempSafe command = null;

            if (Words == null || Words.Length < 2)
                return command;

            string text = String.Join(DELIMITER.ToString(), Words, 1, Words.Length - 1);

            if (text != null)
                text = text.Trim();
   
            switch (text)
            {
                case ON:
                    command = new ChatCommandTempSafe(true);
                    break;

                case OFF:
                    command = new ChatCommandTempSafe(false);
                    break;
            }

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandGrouping ParseGrouping(string[] Words, string Text, DataController Data)
        {
            ChatCommandGrouping command = null;

            if (Words == null || Words.Length < 2)
                return command;

            string text = String.Join(DELIMITER.ToString(), Words, 1, Words.Length - 1);

            if (text != null)
                text = text.Trim();
   
            switch (text)
            {
                case ON:
                    command = new ChatCommandGrouping(true);
                    break;

                case OFF:
                    command = new ChatCommandGrouping(false);
                    break;
            }

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandAutoLoot ParseAutoLoot(string[] Words, string Text, DataController Data)
        {
           ChatCommandAutoLoot command = null;

           if (Words == null || Words.Length < 2)
              return command;

           string text = String.Join(DELIMITER.ToString(), Words, 1, Words.Length - 1);

           if (text != null)
              text = text.Trim();

           switch (text)
           {
              case ON:
                 command = new ChatCommandAutoLoot(true);
                 break;

              case OFF:
                 command = new ChatCommandAutoLoot(false);
                 break;
           }

           return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandAutoCombine ParseAutoCombine(string[] Words, string Text, DataController Data)
        {
           ChatCommandAutoCombine command = null;

           if (Words == null || Words.Length < 2)
              return command;

           string text = String.Join(DELIMITER.ToString(), Words, 1, Words.Length - 1);

           if (text != null)
              text = text.Trim();

           switch (text)
           {
              case ON:
                 command = new ChatCommandAutoCombine(true);
                 break;

              case OFF:
                 command = new ChatCommandAutoCombine(false);
                 break;
           }

           return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandReagentBag ParseReagentBag(string[] Words, string Text, DataController Data)
        {
           ChatCommandReagentBag command = null;

           if (Words == null || Words.Length < 2)
              return command;

           string text = String.Join(DELIMITER.ToString(), Words, 1, Words.Length - 1);

           if (text != null)
              text = text.Trim();

           switch (text)
           {
              case ON:
                 command = new ChatCommandReagentBag(true);
                 break;

              case OFF:
                 command = new ChatCommandReagentBag(false);
                 break;
           }

           return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandSpellPower ParseSpellPower(string[] Words, string Text, DataController Data)
        {
           ChatCommandSpellPower command = null;

           if (Words == null || Words.Length < 2)
              return command;

           string text = String.Join(DELIMITER.ToString(), Words, 1, Words.Length - 1);

           if (text != null)
              text = text.Trim();

           switch (text)
           {
              case ON:
                 command = new ChatCommandSpellPower(true);
                 break;

              case OFF:
                 command = new ChatCommandSpellPower(false);
                 break;
           }

           return command;
        }

#if !OPENMERIDIAN
        /// <summary>
        /// Very much like ParseGoPlayer (copy'n'paste)!
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandInvite ParseInvite(string[] Words, string Text, DataController Data)
        {
            Tuple<int, int, string> quote = null;
            ChatCommandInvite command = null;
            OnlinePlayer player = null;
            string prefix = null;
            List<OnlinePlayer> list = null;
            int num = 0;

            if (Words == null || Words.Length < 2)
                return null;

            // extract quoted name if second word starts with "          
            // this is necessary to not care about quoted text (t someone "yes yes")
            // but only for quoted names (t "mister x" hello!)
            if (Words[1].Length > 0 && Words[1][0] == QUOTECHAR)
                quote = Text.GetQuote();

            /********* QUOTED NAME *********/
            if (quote != null)
            {
                // try get exact match for quoted name
                player = Data.OnlinePlayers.GetItemByName(quote.Item3);

                if (player != null)
                    command = new ChatCommandInvite(player.ID);

                // no player with that name
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player with name: " + quote.Item3));
                }
            }

            /********* UNQUOTED NAME *********/
            else
            {
                prefix = Words[1];
                list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);

                // extend prefix with more words
                // until there is only one or zero matches found
                // or until there is only one more word left (supposed minimal text)
                num = 2;
                while (list.Count > 1 && num < Words.Length)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);
                    num++;
                }

                if (list.Count == 1)
                    command = new ChatCommandInvite(list[0].ID);

                // still more than one player with max. prefix
                else if (list.Count > 1)
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "More than one player with prefix: " + prefix));
                }

                // no player with that prefix
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No player with prefix: " + prefix));
                }
            }

            return command;
        }

        /// <summary>
        /// Almost exactly like ParseGoPlayer (copied from ParseCast)
        /// </summary>
        /// <param name="Words"></param>
        /// <param name="Text"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        protected static ChatCommandPerform ParsePerform(string[] Words, string Text, DataController Data)
        {
            Tuple<int, int, string> quote = null;
            ChatCommandPerform command = null;
            SkillObject skill = null;
            string prefix = null;
            List<SkillObject> list = null;
            int num = 0;

            if (Words == null || Words.Length < 2)
                return null;

            // extract quoted name if second word starts with "          
            // this is necessary to not care about quoted text (t someone "yes yes")
            // but only for quoted names (t "mister x" hello!)
            if (Words[1].Length > 0 && Words[1][0] == QUOTECHAR)
                quote = Text.GetQuote();

            /********* QUOTED NAME *********/
            if (quote != null)
            {
                // try get exact match for quoted name
                skill = Data.SkillObjects.GetItemByName(quote.Item3, false);

                if (skill != null)
                    command = new ChatCommandPerform(skill);

                // no skill with that name
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No skill with name: " + quote.Item3));
                }
            }

            /********* UNQUOTED NAME *********/
            else
            {
                prefix = Words[1];
                list = Data.SkillObjects.GetItemsByNamePrefix(prefix);

                // extend prefix with more words
                // until there is only one or zero matches found
                // or until there is only one more word left (supposed minimal text)
                num = 2;
                while (list.Count > 1 && num < Words.Length)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.SkillObjects.GetItemsByNamePrefix(prefix);
                    num++;
                }

                if (list.Count == 1)
                    command = new ChatCommandPerform(list[0]);

                // still more than one skill with max. prefix
                else if (list.Count > 1)
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "More than one skill with prefix: " + prefix));
                }

                // no skill with that prefix
                else
                {
                    Data.ChatMessages.Add(ServerString.GetServerStringForString(
                        "No skill with prefix: " + prefix));
                }
            }

            return command;
        }
#endif
#endif
    }
}
