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

using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using System;

namespace Meridian59.Bot.IRC
{
    /// <summary>
    /// Extensions for the String class that allow replacing the first instance
    /// of a substring in a string, starting from pos 0 or given pos.
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Replaces the first instance of a search string in the string with another string.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Replaces the first instance of a search string in the string with another string.
        /// Starts searching at startPos.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace, int startPos)
        {
            int pos = text.IndexOf(search, startPos);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }

    /// <summary>
    /// Deals with IRC chatstyle
    /// </summary>
    public static class IRCChatStyle
    {
        #region Constants       
        public const string IRCCOLOR_UNDERLINE  = "\u001F";
        public const string IRCCOLOR_BOLD       = "\u0002";
        public const string IRCCOLOR_ITALIC     = "\u001D";
        public const string IRCCOLOR_START      = "\u0003";
        public const string IRCCOLOR_TERM       = "\u000F";
        public const string IRCCOLOR_WHITE      = "00";
        public const string IRCCOLOR_BLACK      = "01";
        public const string IRCCOLOR_BLUE       = "02";
        public const string IRCCOLOR_GREEN      = "03";
        public const string IRCCOLOR_RED        = "04";
        public const string IRCCOLOR_BROWN      = "05";
        public const string IRCCOLOR_PURPLE     = "06";
        public const string IRCCOLOR_ORANGE     = "07";
        public const string IRCCOLOR_YELLOW     = "08";
        public const string IRCCOLOR_LIGHTGREEN = "09";
        public const string IRCCOLOR_TEAL       = "10";
        public const string IRCCOLOR_LIGHTCYAN  = "11";
        public const string IRCCOLOR_LIGHTBLUE  = "12";
        public const string IRCCOLOR_LIGHTPINK  = "13";
        public const string IRCCOLOR_GREY       = "14";
        public const string IRCCOLOR_LIGHTGREY  = "15";

        // IRC to chat constants
        public const string SERVER_UNDERLINE = "~U";
        public const string SERVER_BOLD = "~B";
        public const string SERVER_ITALIC = "~I";
        public const string SERVER_CANCEL = "~n";
        public const string SERVER_WHITE = "~w";
        public const string SERVER_BLACK = "~k";
        public const string SERVER_BLUE = "~b";
        public const string SERVER_GREEN = "~g";
        public const string SERVER_RED = "~r";
        public const string SERVER_PURPLE = "~v";
        public const string SERVER_ORANGE = "~o";
        public const string SERVER_YELLOW = "~y";
        public const string SERVER_LIGHTGREEN = "~l";
        public const string SERVER_TEAL = "~t";
        public const string SERVER_AQUA = "~a";
        public const string SERVER_LIGHTPINK = "~p";
        public const string SERVER_LIGHTGREY = "~s";
        #endregion

        /// <summary>
        /// Converts IRC colors into server-displayable colors, for sending
        /// a server chat message from one server to another through IRC.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CreateChatMessageFromIRCMessage(string s)
        {
            int index;
            // Handle styles
            while ((index = s.IndexOf(IRCCOLOR_BOLD, 0)) > -1)
            {
                s = s.ReplaceFirst(IRCCOLOR_BOLD, SERVER_BOLD);
                s = s.ReplaceFirst(IRCCOLOR_TERM, SERVER_BOLD, index);
            }
            while ((index = s.IndexOf(IRCCOLOR_UNDERLINE, 0)) > -1)
            {
                s = s.ReplaceFirst(IRCCOLOR_UNDERLINE, SERVER_UNDERLINE);
                s = s.ReplaceFirst(IRCCOLOR_TERM, SERVER_UNDERLINE, index);
            }
            while ((index = s.IndexOf(IRCCOLOR_ITALIC, 0)) > -1)
            {
                s = s.ReplaceFirst(IRCCOLOR_ITALIC, SERVER_ITALIC);
                s = s.ReplaceFirst(IRCCOLOR_TERM, SERVER_ITALIC, index);
            }

            s = s.Replace(IRCCOLOR_START + IRCCOLOR_WHITE + "," + IRCCOLOR_GREY, SERVER_WHITE);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_BLACK + "," + IRCCOLOR_GREY, SERVER_BLACK);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_BLUE + "," + IRCCOLOR_GREY, SERVER_BLUE);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_GREEN + "," + IRCCOLOR_GREY, SERVER_GREEN);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_PURPLE + "," + IRCCOLOR_GREY, SERVER_PURPLE);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_RED + "," + IRCCOLOR_GREY, SERVER_RED);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTGREEN + "," + IRCCOLOR_GREY, SERVER_LIGHTGREEN);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_YELLOW + "," + IRCCOLOR_GREY, SERVER_YELLOW);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTPINK + "," + IRCCOLOR_GREY, SERVER_LIGHTPINK);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_ORANGE + "," + IRCCOLOR_GREY, SERVER_ORANGE);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTCYAN + "," + IRCCOLOR_GREY, SERVER_AQUA);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_TEAL + "," + IRCCOLOR_GREY, SERVER_TEAL);
            s = s.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTGREY + "," + IRCCOLOR_GREY, SERVER_LIGHTGREY);
            s = s.Replace(IRCCOLOR_TERM, String.Empty);

            return s;
        }

        /// <summary>
        /// Creates a colored IRC message string from a ChatMessage instance
        /// </summary>
        /// <param name="ChatMessage"></param>
        /// <returns></returns>
        public static string CreateIRCMessageFromChatMessage(ServerString ChatMessage)
        {
            // prefix
            string s = String.Empty;
  
            // insert styles as IRC chatstyles
            foreach (ChatStyle style in ChatMessage.Styles)
            {
                // bold
                if (style.IsBold)
                    s += IRCCOLOR_BOLD;

                // italic
                if (style.IsCursive)
                    s += IRCCOLOR_ITALIC;

                // underline
                if (style.IsUnderline)
                    s += IRCCOLOR_UNDERLINE;

                // init IRC color
                s += IRCCOLOR_START;

                // add color with lightgrey background
                switch (style.Color)
                {
                    case ChatColor.Black:
                        s += IRCCOLOR_BLACK + "," + IRCCOLOR_GREY;
                        break;

                    case ChatColor.Blue:
                        s += IRCCOLOR_BLUE + "," + IRCCOLOR_GREY;;
                        break;

                    case ChatColor.Green:
                        s += IRCCOLOR_GREEN + "," + IRCCOLOR_GREY;;
                        break;

                    case ChatColor.Purple:
                    case ChatColor.Violet: // no match
                        s += IRCCOLOR_PURPLE + "," + IRCCOLOR_GREY;;
                        break;

                    case ChatColor.Red:
                    case ChatColor.BrightRed: // no IRC match
                        s += IRCCOLOR_RED + "," + IRCCOLOR_GREY;;
                        break;

                    case ChatColor.White:
                        s += IRCCOLOR_WHITE + "," + IRCCOLOR_GREY;;
                        break;

                    case ChatColor.LightGreen:
                        s += IRCCOLOR_LIGHTGREEN + "," + IRCCOLOR_GREY; ;
                        break;

                    case ChatColor.Yellow:
                        s += IRCCOLOR_YELLOW + "," + IRCCOLOR_GREY; ;
                        break;

                    case ChatColor.Pink:
                    case ChatColor.Magenta: // no IRC match
                        s += IRCCOLOR_LIGHTPINK + "," + IRCCOLOR_GREY; ;
                        break;

                    case ChatColor.Orange:
                        s += IRCCOLOR_ORANGE + "," + IRCCOLOR_GREY; ;
                        break;

                    case ChatColor.Cyan:
                    case ChatColor.Aquamarine: // no IRC match
                        s += IRCCOLOR_LIGHTCYAN + "," + IRCCOLOR_GREY; ;
                        break;

                    case ChatColor.Teal:
                        s += IRCCOLOR_TEAL + "," + IRCCOLOR_GREY; ;
                        break;

                    case ChatColor.DarkGrey: // no IRC match
                        s += IRCCOLOR_LIGHTGREY + "," + IRCCOLOR_GREY; ;
                        break;
                }

                // now copy textchunk of this style to output
                s += ChatMessage.FullString.Substring(style.StartIndex, style.Length);

                // appendix
                s += IRCCOLOR_TERM;
            }

            // IRC does not allow \r \n and \0
            // since player messages should not contain them at all (except for shopbot)
            // we remove them here as a final step (don't do earlier!)
            // not worth the work to keeping the last chatstyle to the next irc message start
            s = s.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(Environment.NewLine, String.Empty).Replace("\0", String.Empty);

            // return
            return s;
        }

        /// <summary>
        /// Creates a colored string from a Prefix
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public static string GetPrefixString(string Prefix)
        {
            return
                IRCChatStyle.IRCCOLOR_BOLD + IRCChatStyle.IRCCOLOR_START + 
                IRCChatStyle.IRCCOLOR_WHITE + "," + IRCChatStyle.IRCCOLOR_GREY + 
                Prefix + ":" + IRCChatStyle.IRCCOLOR_TERM + " ";
        }
    }
}
