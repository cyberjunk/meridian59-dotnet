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
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandHelp.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandHelp.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandCheckReg.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandRegister.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandList.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandAdd.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandRemove.HELPSTRING);
            Bot.SendIRCMessage(Admin, IRCAdminBotCommandEcho.HELPSTRING);

            return true;
        }
    }
}
