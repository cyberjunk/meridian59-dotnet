namespace Meridian59.Bot.IRC
{
    public abstract class IRCAdminBotCommand
    {
        public abstract IRCAdminBotCommandType CommandType { get; }

        public static bool ParseAdminCommand(string Admin, string Command, IRCBotClient Bot)
        {
            string[] splitCommand = Command.Split(' ');

            IRCAdminBotCommand botCommand = null;

            switch (splitCommand[0])
            {
                case IRCAdminBotCommandHelp.COMMAND:
                    botCommand = new IRCAdminBotCommandHelp();
                    break;
                case IRCAdminBotCommandCheckReg.COMMAND:
                    botCommand = new IRCAdminBotCommandCheckReg();
                    break;
                case IRCAdminBotCommandRegister.COMMAND:
                    botCommand = new IRCAdminBotCommandRegister();
                    break;
                case IRCAdminBotCommandList.COMMAND:
                    botCommand = new IRCAdminBotCommandList();
                    break;
                case IRCAdminBotCommandAdd.COMMAND:
                    botCommand = new IRCAdminBotCommandAdd();
                    break;
                case IRCAdminBotCommandRemove.COMMAND:
                    botCommand = new IRCAdminBotCommandRemove();
                    break;
                case IRCAdminBotCommandEcho.COMMAND:
                    botCommand = new IRCAdminBotCommandEcho();
                    break;
            }

            if (botCommand == null)
                return false;

            return botCommand.PerformCommand(Admin, Command.Substring(splitCommand[0].Length).TrimStart(' '), Bot);
        }

        public abstract bool PerformCommand(string Admin, string Command, IRCBotClient Bot);
    }
}
