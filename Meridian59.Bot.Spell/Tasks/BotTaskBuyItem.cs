using System;

namespace Meridian59.Bot.Spell
{
    /// <summary>
    /// A 'cast' item in the tasks list
    /// </summary>
    public class BotTaskBuy : BotTask
    {
        public string Name = String.Empty;
        public string Target = String.Empty;
        public uint Amount = 0;
        public uint MaxAmount = 0;

        public BotTaskBuy()
        {
        }

        public BotTaskBuy(string Name, string Target, uint Amount, uint MaxAmount)
        {
            this.Name = Name;
            this.Target = Target;
            this.Amount = Amount;
            this.MaxAmount = MaxAmount;
        }
    }

    public class BotTaskSelectNPC : BotTask
    {
        public string Name = String.Empty;

        public BotTaskSelectNPC()
        {
        }

        public BotTaskSelectNPC(string Name)
        {
            this.Name = Name;
        }
    }

    public class BotTaskSendBuy : BotTask
    {
        public BotTaskSendBuy()
        {
        }
    }

    public class BotTaskBuyItem : BotTask
    {
        public string Name = String.Empty;
        public uint Amount = 0;
        public uint MaxAmount = 0;

        public BotTaskBuyItem(string Name, uint Amount, uint MaxAmount)
        {
            this.Name = Name;
            this.Amount = Amount;
            this.MaxAmount = MaxAmount;
        }
    }
}
