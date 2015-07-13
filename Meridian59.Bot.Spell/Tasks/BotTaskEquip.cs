using System;

namespace Meridian59.Bot.Spell
{
    /// <summary>
    /// A 'use' item in the tasks list
    /// </summary>
    public class BotTaskEquip : BotTask
    {
        public string Name = String.Empty;

        public BotTaskEquip()
        {
        }

        public BotTaskEquip(string Name)
        {
            this.Name = Name;
        }
    }

}
