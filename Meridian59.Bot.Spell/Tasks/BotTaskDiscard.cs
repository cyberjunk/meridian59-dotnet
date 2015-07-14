using System;

namespace Meridian59.Bot.Spell
{
    /// <summary>
    /// A 'use' item in the tasks list
    /// </summary>
    public class BotTaskDiscard : BotTask
    {
        public string Name = String.Empty;

        public BotTaskDiscard()
        {
        }

        public BotTaskDiscard(string Name)
        {
            this.Name = Name;
        }
    }

}