using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrcDotNet;

namespace Meridian59.Bot.IRC
{
    public class IRCAdminBotCommandHelp : IRCAdminBotCommand
    {
        public const string HELPSTRING = "Available commands are: ";
        public const string COMMAND = "@help";
        public const string RETURNTEXT = "@help";

        public override IRCAdminBotCommandType CommandType { get { return IRCAdminBotCommandType.Help; } }

        public IRCAdminBotCommandHelp()
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        /// <returns></returns>
        public override bool PerformCommand(string Admin, string Command, IRCBotClient Bot)
        {
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandHelp.HELPSTRING);
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandCheckReg.HELPSTRING);
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandRegister.HELPSTRING);
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandList.HELPSTRING);
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandAdd.HELPSTRING);
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandRemove.HELPSTRING);
            Bot.IrcClient.LocalUser.SendMessage(Admin, IRCAdminBotCommandEcho.HELPSTRING);

            return true;
        }
    }
}
