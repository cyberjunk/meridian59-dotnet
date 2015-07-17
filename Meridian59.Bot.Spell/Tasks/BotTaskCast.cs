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

using Meridian59.Common.Constants;
using System;

namespace Meridian59.Bot.Spell
{
    /// <summary>
    /// A 'cast' item in the tasks list
    /// </summary>
    public class BotTaskCast : BotTask
    {
        public string Name = String.Empty;
        public string Target = String.Empty;
        public string Where = String.Empty;
        public string OnMax = String.Empty;
        public uint Cap = StatNumsValues.SKILLMAX;

        public BotTaskCast()
        {
        }

        public BotTaskCast(string Name, string Target, string Where, string OnMax, uint Cap)
        {
            this.Name = Name;
            this.Target = Target;
            this.Where = Where;
            this.OnMax = OnMax;
            this.Cap = Cap;
        }
    }
}
