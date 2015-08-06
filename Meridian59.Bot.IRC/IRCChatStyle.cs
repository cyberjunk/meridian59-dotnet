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
        #endregion

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
