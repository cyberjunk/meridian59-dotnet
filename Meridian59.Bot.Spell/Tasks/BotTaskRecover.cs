using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meridian59.Bot.Spell
{
    public class BotTaskRecover : BotTask
    {
        public string Stat = String.Empty;

        public BotTaskRecover()
        {
        }

        public BotTaskRecover(string Stat)
        {
            this.Stat = Stat;
        }
    }
}
