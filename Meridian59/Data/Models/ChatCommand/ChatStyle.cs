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
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Defines a style for a part of a chat string
    /// </summary>
    public class ChatStyle
    {
        public const char MARKER1           = '~';
        public const char MARKER2           = '`';
        public const char STYLEBOLD         = 'B';
        public const char STYLECURSIVE      = 'I';
        public const char STYLEUNDERLINE    = 'U';
        public const char STYLENORMAL       = 'n';
#if !VANILLA && !OPENMERIDIAN
        public const char STYLESTRIKEOUT    = 'S';
        public const char STYLELINK         = 'L';
#endif

        public int StartIndex { get; set; }
        public int Length { get; set; }
        public bool IsBold { get; set; }
        public bool IsCursive { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsStrikeout { get; set; }
        public bool IsLink { get; set; }
        public ChatColor Color { get; set; }

        public ChatStyle()
        {
            StartIndex = 0;
            Length = 0;
            IsBold = false;
            IsCursive = false;
            IsUnderline = false;
            IsStrikeout = false;
            IsLink = false;
            Color = ChatColor.Black;
        }

        public ChatStyle(int StartIndex, int Length, bool IsBold, bool IsCursive,
            bool IsUnderline, bool IsStrikeout, bool IsLink, ChatColor Color)
        {
            this.StartIndex = StartIndex;
            this.Length = Length;
            this.IsBold = IsBold;
            this.IsCursive = IsCursive;
            this.IsUnderline = IsUnderline;
            this.IsStrikeout = IsStrikeout;
            this.IsLink = IsLink;
            this.Color = Color;
        }

        /// <summary>
        /// Changes properties based on a known StyleChar
        /// </summary>
        /// <param name="StyleCharacter"></param>
        /// <param name="MessageType"></param>
        public void ProcessStyleCharacter(char StyleCharacter, ChatMessageType MessageType)
        {
            // Colors
            if (Enum.IsDefined(typeof(ChatColor), (int)StyleCharacter))
            {
                Color = (ChatColor)StyleCharacter;
                return;
            }

            switch (StyleCharacter)
            {
                // modifiers
                case STYLEBOLD:
                    IsBold = !IsBold;
                    break;

                case STYLECURSIVE:
                    IsCursive = !IsCursive;
                    break;
                
                case STYLEUNDERLINE:
                    IsUnderline = !IsUnderline;
                    break;

#if !VANILLA && !OPENMERIDIAN
                case STYLESTRIKEOUT:
                    IsStrikeout = !IsStrikeout;
                    break;

                case STYLELINK:
                    IsLink = !IsLink;
                    break;
#endif

                case STYLENORMAL:
                    switch (MessageType)
                    {
                        case ChatMessageType.ObjectChatMessage:
                            Color = ChatColor.White;
                            break;

                        case ChatMessageType.ServerChatMessage:
                            Color = ChatColor.Purple;
                            break;

                        case ChatMessageType.SystemMessage:
                            Color = ChatColor.Blue;
                            break;

                        default:
                            Color = ChatColor.White;
                            break;
                    }
                    
                    IsBold = false;
                    IsCursive = false;
                    IsUnderline = false;
                    IsStrikeout = false;
                    IsLink = false;
                    break;
            }
        }

        #region Static
        /// <summary>
        /// Checks whether there is a known inline style character pair
        /// in given Text at given Position
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        public static bool IsInlineStyle(string Text, int Position)
        {
            if (Text.Length > Position + 1 &&
                IsMarker(Text[Position]) &&
                IsStyleChar(Text[Position + 1]))
                          
                return true;
            
            else return false;
        }

        /// <summary>
        /// Tells whether given Character is a MARKER
        /// for beginning of inline formatting tag.
        /// </summary>
        /// <param name="Character"></param>
        /// <returns></returns>
        public static bool IsMarker(char Character)
        {
            return
                Character == MARKER1 || 
                Character == MARKER2;
        }

        /// <summary>
        /// Checks if a character is one of the characters
        /// known inline style characters after MARKER
        /// </summary>
        /// <param name="Character"></param>
        /// <returns></returns>
        public static bool IsStyleChar(char Character)
        {
            // Colors
            if (Enum.IsDefined(typeof(ChatColor), (int)Character))
                return true;

            if (Character == STYLEBOLD ||
                Character == STYLECURSIVE ||
                Character == STYLEUNDERLINE ||
                Character == STYLENORMAL
#if VANILLA || OPENMERIDIAN
                )
#else
                ||
                Character == STYLESTRIKEOUT ||
                Character == STYLELINK)
#endif

                return true;

            else return false;
        }

        /// <summary>
        /// Returns the index of a given index without any
        /// characters belonging to inline styles before it.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static int GetIndexWithoutInlineStyle(string Text, int Index)
        {
            int returnval = Index;

            for (int i = 0; i < Index - 1; i++)
            {
                if (IsInlineStyle(Text, i))
                {
                    // skip the next char, it's already processed
                    i++;

                    // subtract two inline characters
                    returnval -= 2;
                }
            }

            return returnval;
        }

        /// <summary>
        /// Returns a cleaned up variant of Text without any inline text styles
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string RemoveInlineStyles(string Text)
        {
            string s = Text;

            // replace colors using marker1 and marker2
            foreach (ChatColor chatColor in Enum.GetValues(typeof(ChatColor)))
            {
                s = s.Replace(new string(new char[] { MARKER1, (char)chatColor }), String.Empty);
                s = s.Replace(new string(new char[] { MARKER2, (char)chatColor }), String.Empty);
            }

            // replace marker1 + style

            s = s.Replace(new string(new char[] { MARKER1, STYLEBOLD }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER1, STYLECURSIVE }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER1, STYLEUNDERLINE }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER1, STYLENORMAL }), String.Empty);
#if !VANILLA && !OPENMERIDIAN
            s = s.Replace(new string(new char[] { MARKER1, STYLESTRIKEOUT }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER1, STYLELINK }), String.Empty);
#endif
            // replace marker2 + style

            s = s.Replace(new string(new char[] { MARKER2, STYLEBOLD }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER2, STYLECURSIVE }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER2, STYLEUNDERLINE }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER2, STYLENORMAL }), String.Empty);

#if !VANILLA && !OPENMERIDIAN
            s = s.Replace(new string(new char[] { MARKER2, STYLESTRIKEOUT }), String.Empty);
            s = s.Replace(new string(new char[] { MARKER2, STYLELINK }), String.Empty);
#endif

            return s;
        }

        /// <summary>
        /// Returns a list of TextStyles for a given Text
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="MessageType"></param>
        /// <returns></returns>
        public static List<ChatStyle> GetStyles(string Text, ChatMessageType MessageType)
        {
            // styles we will parse
            List<ChatStyle> list = new List<ChatStyle>();

            // the startcolor depends on messagetype
            ChatColor startColor;
            switch (MessageType)
            {
                case ChatMessageType.ServerChatMessage:
#if !VANILLA
                    startColor = ChatColor.Violet;
#else
                    startColor = ChatColor.Purple;
#endif
                    break;

                case ChatMessageType.SystemMessage:
                    startColor = ChatColor.Blue;
                    break;

                case ChatMessageType.ObjectChatMessage:
                default:
                    startColor = ChatColor.White;
                    break;
            }

            // first/default style
            ChatStyle style = new ChatStyle(0, 0, false, false, false, false, false, startColor);
            list.Add(style);
           
            // walk the characters up to the lastone-1
            for (int i = 0; i < Text.Length - 1; i++)
            {
                // check if this is an inline style
                if (IsInlineStyle(Text, i))
                {
                    // get the index of this new style in a string without inline styles
                    int index = GetIndexWithoutInlineStyle(Text, i);

                    // if this index is bigger than our last one
                    // we have a new style to create
                    if (index > style.StartIndex)
                    {
                        // set length in last style
                        style.Length = index - style.StartIndex;
                        
                        // create a new style definition
                        // use the existing one to start from
                        style = new ChatStyle(index, 0, style.IsBold, style.IsCursive, style.IsUnderline,
                            style.IsStrikeout, style.IsLink, style.Color);

                        // process this stylechar
                        style.ProcessStyleCharacter(Text[i + 1], MessageType);
                        list.Add(style);
                    }
                    else
                    {
                        // process this inline style in existing style
                        style.ProcessStyleCharacter(Text[i + 1], MessageType);
                    }

                    // skip the next character, we already processed it
                    i++;
                }
            }

            // set length in last style
            style.Length = GetIndexWithoutInlineStyle(Text, Text.Length) - style.StartIndex;

            return list;
        }
        #endregion
    }
}
