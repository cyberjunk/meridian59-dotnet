using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meridian59.Bot.Spell
{
    public class BotTaskMove : BotTask
    {
        public string Door = String.Empty;

        public BotTaskMove()
        {
        }

        public BotTaskMove(string Door)
        {
            this.Door = Door;
        }
    }
}
