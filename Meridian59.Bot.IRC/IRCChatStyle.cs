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

using Meridian59.Common;
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
        public const string IRCCOLOR_STRIKEOUT  = "\u001E";
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
        public const string SERVER_STRIKEOUT = "~S";
        public const string SERVER_LINK = "~L";
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
        public static string CreateChatMessageFromIRCMessage(string Text)
        {
            int index;
            // Handle styles
            while ((index = Text.IndexOf(IRCCOLOR_BOLD, 0)) > -1)
            {

                Text = Text.ReplaceFirst(IRCCOLOR_BOLD, SERVER_BOLD);
                Text = Text.ReplaceFirst(IRCCOLOR_BOLD, SERVER_BOLD);
                Text = Text.ReplaceFirst(IRCCOLOR_TERM, SERVER_BOLD, index);
            }
            while ((index = Text.IndexOf(IRCCOLOR_UNDERLINE, 0)) > -1)
            {
                Text = Text.ReplaceFirst(IRCCOLOR_UNDERLINE, SERVER_UNDERLINE);
                Text = Text.ReplaceFirst(IRCCOLOR_TERM, SERVER_UNDERLINE, index);
            }
            while ((index = Text.IndexOf(IRCCOLOR_ITALIC, 0)) > -1)
            {
                Text = Text.ReplaceFirst(IRCCOLOR_ITALIC, SERVER_ITALIC);
                Text = Text.ReplaceFirst(IRCCOLOR_TERM, SERVER_ITALIC, index);
            }
            while ((index = Text.IndexOf(IRCCOLOR_STRIKEOUT, 0)) > -1)
            {
                Text = Text.ReplaceFirst(IRCCOLOR_STRIKEOUT, SERVER_STRIKEOUT);
                Text = Text.ReplaceFirst(IRCCOLOR_TERM, SERVER_STRIKEOUT, index);
            }
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_WHITE + "," + IRCCOLOR_GREY, SERVER_WHITE);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_BLACK + "," + IRCCOLOR_GREY, SERVER_BLACK);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_BLUE + "," + IRCCOLOR_GREY, SERVER_BLUE);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_GREEN + "," + IRCCOLOR_GREY, SERVER_GREEN);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_PURPLE + "," + IRCCOLOR_GREY, SERVER_PURPLE);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_RED + "," + IRCCOLOR_GREY, SERVER_RED);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTGREEN + "," + IRCCOLOR_GREY, SERVER_LIGHTGREEN);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_YELLOW + "," + IRCCOLOR_GREY, SERVER_YELLOW);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTPINK + "," + IRCCOLOR_GREY, SERVER_LIGHTPINK);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_ORANGE + "," + IRCCOLOR_GREY, SERVER_ORANGE);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTCYAN + "," + IRCCOLOR_GREY, SERVER_AQUA);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_TEAL + "," + IRCCOLOR_GREY, SERVER_TEAL);
            Text = Text.Replace(IRCCOLOR_START + IRCCOLOR_LIGHTGREY + "," + IRCCOLOR_GREY, SERVER_LIGHTGREY);
            Text = Text.Replace(IRCCOLOR_TERM, String.Empty);

            return Text;
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
                
                // strikeout
                if (style.IsStrikeout)
                    s += IRCCOLOR_STRIKEOUT;

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

        /// <summary>
        /// Returns a string containing the last chat color and style used in Message.
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string GetLastChatStyleString(string Message)
        {
            int lastColorIndex = Message.LastIndexOf(IRCCOLOR_START);

            // Handle not finding anything, or truncated style.
            if (lastColorIndex == -1 || Message.Length < lastColorIndex + 5)
                return IRCCOLOR_START + IRCCOLOR_WHITE + "," + IRCCOLOR_GREY;

            int chatStyleLen = (IRCCOLOR_GREY.Length * 2) + IRCCOLOR_START.Length + ",".Length;

            int styleLen = IRCCOLOR_BOLD.Length;
            // Check for styles.
            if (lastColorIndex - Message.LastIndexOf(IRCCOLOR_BOLD) < styleLen * 4)
            {
                lastColorIndex -= styleLen;
                chatStyleLen += styleLen;
            }
            if (lastColorIndex - Message.LastIndexOf(IRCCOLOR_ITALIC) < styleLen * 4)
            {
                lastColorIndex -= styleLen;
                chatStyleLen += styleLen;
            }
            if (lastColorIndex - Message.LastIndexOf(IRCCOLOR_UNDERLINE) < styleLen * 4)
            {
                lastColorIndex -= styleLen;
                chatStyleLen += styleLen;
            }
            if (lastColorIndex - Message.LastIndexOf(IRCCOLOR_STRIKEOUT) < styleLen * 4)
            {
                lastColorIndex -= styleLen;
                chatStyleLen += styleLen;
            }

            return Message.Substring(lastColorIndex, chatStyleLen);
        }

        /// <summary>
        /// Checks for a potentially better truncation index than StartIndex
        /// in Message. Used so that a game chat message destined for IRC
        /// does not get split in the middle of a color code or word.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        public static int GetGoodTruncateIndex(string Message, int StartIndex)
        {
            // Length of Message must be larger than StartIndex, otherwise
            // use the whole string.
            if (Message.Length <= StartIndex)
                return Message.Length;

            // Ideally truncate on a space (only ' ').
            int lastSpace = Message.LastIndexOf(" ", StartIndex);

            // No space to truncate at? Use StartIndex.
            if (lastSpace <= 0)
                return StartIndex;

            // Handle cases where the message might be a giant word. Just
            // use StartIndex so as to not split into too many strings.
            if (StartIndex - lastSpace > 50)
                return StartIndex;

            // Truncate on the space
            return lastSpace;
        }
    }
}
