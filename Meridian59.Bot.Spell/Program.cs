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
using System.IO;
using System.ServiceProcess;

using Meridian59.Common;
using Meridian59.Data;
using Meridian59.Files;

namespace Meridian59.Bot.Spell
{
    class Program
    {
        static void Main(string[] args)
        {
            // run as console app
            if (Environment.UserInteractive)
            {
                SpellBotClient spellBot = new SpellBotClient();

                // start it
                spellBot.Start(true);
            }

            // run as windows service
            else
            {
                // set workpath to .exe path (not default for services)
                // very important
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                // create bot-service wrapper
                ServiceBase serviceWrap =
                    new BotServiceWrapper<GameTick, ResourceManager, DataController, SpellBotConfig, SpellBotClient>();

                // run service
                ServiceBase.Run(serviceWrap);
            }
        }
    }
}
