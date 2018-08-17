namespace Meridian59.Bot.IRC
{
    public class IRCAdminBotCommandList : IRCAdminBotCommand
    {
        public const string HELPSTRING = "@list users, @list admins and @list registered: List the appropriate data.";
        public const string COMMAND = "@list";

        public override IRCAdminBotCommandType CommandType { get { return IRCAdminBotCommandType.List; } }

        public IRCAdminBotCommandList()
        {
        }

        /// <summary>
        /// Performs command
        /// </summary>
        /// <returns></returns>
        public override bool PerformCommand(string Admin, string Command, IRCBotClient Bot)
        {
            switch (Command)
            {
                case "admins":
                    Bot.SendIRCMessage(Admin,
                        "Bot admins: " + string.Join(", ", Bot.Config.Admins));
                    return true;
                case "users":
                    Bot.SendIRCMessage(Admin,
                        "Bot users: " + string.Join(", ", Bot.Config.AllowedUsers));
                    return true;
                case "registered":
                    Bot.SendIRCMessage(Admin,
                        "Registered users: " + string.Join(", ", Bot.UserRegistration.Keys));
                    return true;
            }

            return false;
        }
    }
}
