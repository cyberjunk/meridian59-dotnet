using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrcDotNet;

namespace Meridian59.Bot.IRC
{
    public class IRCAdminBotCommandAdd : IRCAdminBotCommand
    {
        public const string HELPSTRING = "@add nickname: Enable a user to use the bot (broadcasts only).";
        public const string COMMAND = "@add";

        public override IRCAdminBotCommandType CommandType { get { return IRCAdminBotCommandType.Add; } }

        public IRCAdminBotCommandAdd()
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        /// <returns></returns>
        public override bool PerformCommand(string Admin, string Command, IRCBotClient Bot)
        {
            if (!Bot.Config.AllowedUsers.Contains(Command))
            {
                Bot.Config.AllowedUsers.Add(Command);
                Bot.IrcClient.LocalUser.SendMessage(Admin, "Added " + Command + " as a bot user.");
            }
            else
            {
                Bot.IrcClient.LocalUser.SendMessage(Admin, Command + " is already on the bot user list.");
            }

            return true;
        }
    }
}
