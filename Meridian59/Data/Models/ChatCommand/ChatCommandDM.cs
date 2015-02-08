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
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class ChatCommandDM : ChatCommand
    {
        public const string KEY1 = "dm";

        public override ChatCommandType CommandType { get { return ChatCommandType.DM; } }
        public string Text { get; set; }

        public ChatCommandDM()
        {
            Text = String.Empty;
        }

        public ChatCommandDM(string Text)
        {
            this.Text = Text;
        }
    }
}
