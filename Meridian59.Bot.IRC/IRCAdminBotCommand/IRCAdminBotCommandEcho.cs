using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrcDotNet;

namespace Meridian59.Bot.IRC
{
    public class IRCAdminBotCommandEcho : IRCAdminBotCommand
    {
        public const string HELPSTRING = "@echo on, @echo off, @echo status: Toggle whether bot echoes game chat to IRC, or display status.";
        public const string COMMAND = "@echo";

        public override IRCAdminBotCommandType CommandType { get { return IRCAdminBotCommandType.Echo; } }

        public IRCAdminBotCommandEcho()
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        /// <returns></returns>
        public override bool PerformCommand(string Admin, string Command, IRCBotClient Bot)
        {
            switch(Command)
            {
                case "on":
                    if (Bot.DisplayMessages)
                    {
                        Bot.IrcClient.LocalUser.SendMessage(Admin, "IRC echo is already on!.");
                    }
                    else
                    {
                        Bot.DisplayMessages = true;
                        Bot.IrcClient.LocalUser.SendMessage(Admin, "Set IRC echo on.");
                    }
                    break;
                case "off":
                    if (!Bot.DisplayMessages)
                    {
                        Bot.IrcClient.LocalUser.SendMessage(Admin, "IRC echo is already off!.");
                    }
                    else
                    {
                        Bot.DisplayMessages = false;
                        Bot.IrcClient.LocalUser.SendMessage(Admin, "Set IRC echo off.");
                    }
                    break;
                default:
                    Bot.IrcClient.LocalUser.SendMessage(Admin, "Echo is currently " + (Bot.DisplayMessages ? "on" : "off"));
                    break;
            }

            return true;
        }
    }
}
