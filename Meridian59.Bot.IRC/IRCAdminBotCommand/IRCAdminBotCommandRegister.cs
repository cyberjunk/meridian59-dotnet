using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrcDotNet;

namespace Meridian59.Bot.IRC
{
    public class IRCAdminBotCommandRegister : IRCAdminBotCommand
    {
        public const string HELPSTRING = "@register nickname: Force bot to do a nickname registration check on IRC server.";
        public const string COMMAND = "@register";

        public override IRCAdminBotCommandType CommandType { get { return IRCAdminBotCommandType.Register; } }

        public IRCAdminBotCommandRegister()
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        /// <returns></returns>
        public override bool PerformCommand(string Admin, string Command, IRCBotClient Bot)
        {
            Bot.UserRegCheck(Command);
            Bot.IrcClient.LocalUser.SendMessage(Admin, "Performing reg check on " + Command);

            return true;
        }
    }
}
