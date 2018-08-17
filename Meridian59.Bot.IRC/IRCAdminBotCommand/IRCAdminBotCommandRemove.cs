namespace Meridian59.Bot.IRC
{
    public class IRCAdminBotCommandRemove : IRCAdminBotCommand
    {
        public const string HELPSTRING = "@remove nickname: Remove a user from being able to use the bot.";
        public const string COMMAND = "@remove";

        public override IRCAdminBotCommandType CommandType { get { return IRCAdminBotCommandType.Remove; } }

        public IRCAdminBotCommandRemove()
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        /// <returns></returns>
        public override bool PerformCommand(string Admin, string Command, IRCBotClient Bot)
        {
            if (Bot.Config.AllowedUsers.Contains(Command))
            {
                Bot.Config.AllowedUsers.Remove(Command);
                Bot.SendIRCMessage(Admin, "Removed " + Command + " as a bot user.");
            }
            else
            {
                Bot.SendIRCMessage(Admin, "Could not find " + Command + " in bot user list.");
            }

            return true;
        }
    }
}
