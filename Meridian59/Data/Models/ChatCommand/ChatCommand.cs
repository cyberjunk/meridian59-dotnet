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
                case ChatCommandSay.KEY1:
                case ChatCommandSay.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandSay(text);
                    }
                    break;
                
                case ChatCommandEmote.KEY1:
                case ChatCommandEmote.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandEmote(text);
                    }
                    break;
                
                case ChatCommandYell.KEY1:
                case ChatCommandYell.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandYell(text);
                    }
                    break;

                case ChatCommandBroadcast.KEY1:
                case ChatCommandBroadcast.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandBroadcast(text);
                    }
                    break;

                case ChatCommandGuild.KEY1:
                case ChatCommandGuild.KEY2:
                    if (splitted.Length > 1)
                    {
                        text = String.Join(DELIMITER.ToString(), splitted, 1, splitted.Length - 1);
                        returnValue = new ChatCommandGuild(text);
                    }
                    break;

                case ChatCommandTell.KEY1:
                case ChatCommandTell.KEY2:
                    returnValue = ParseTell(splitted, lower, DataController);                                           
                    break;
                
                case ChatCommandCast.KEY1:
                case ChatCommandCast.KEY2:
                    returnValue = ParseCast(splitted, lower, DataController);
                    break;

                case ChatCommandDeposit.KEY1:
                    if (splitted.Length == 2)
                    {
                        uint amount = 0;

                        if (UInt32.TryParse(splitted[1], out amount))
                            returnValue = new ChatCommandDeposit(amount);
                    }
                    break;

                case ChatCommandWithDraw.KEY1:
                    if (splitted.Length == 2)
                    {
                        uint amount = 0;

                        if (UInt32.TryParse(splitted[1], out amount))
                            returnValue = new ChatCommandWithDraw(amount);
                    }     
                    break;

                case ChatCommandSuicide.KEY1:
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
                    returnValue = new ChatCommandRest();
                    break;

                case ChatCommandStand.KEY1:
                    returnValue = new ChatCommandStand();
                    break;

                case ChatCommandQuit.KEY1:
                    returnValue = new ChatCommandQuit();
                    break;
#if !VANILLA
                case ChatCommandTempSafe.KEY1:
                    returnValue = ParseTempSafe(splitted, lower, DataController);
                    break;

                case ChatCommandGrouping.KEY1:
                    returnValue = ParseGrouping(splitted, lower, DataController);
                    break;
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
                {
                    // startindex of actual text
                    int idx = Words[0].Length + quote.Item2 + 2;

                    // correct tell
                    if (idx < Text.Length)
                    {
                        command = new ChatCommandTell(
                            player.ID, Text.Substring(idx, Text.Length - idx));
                    }

                    // empty text
                    else
                    {
                        Data.ChatMessages.Add(ServerString.GetServerStringForString(
                            "Can't send empty message."));
                    }
                }

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
                while (list.Count > 1 && num < Words.Length - 1)
                {
                    prefix += DELIMITER + Words[num];
                    list = Data.OnlinePlayers.GetItemsByNamePrefix(prefix);
                    num++;
                }

                if (list.Count == 1)
                {
                    // startindex of actual text
                    int idx = Words[0].Length + prefix.Length + 2;

                    if (idx < Text.Length)
                    {
                        command = new ChatCommandTell(
                            list[0].ID, Text.Substring(idx, Text.Length - idx));
                    }
                    else
                    {
                        // empty text
                        Data.ChatMessages.Add(ServerString.GetServerStringForString(
                            "Can't send empty message."));
                    }
                }

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
#endif
    }
}
