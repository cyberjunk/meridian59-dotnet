using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meridian59.Bot.Spell
{

    public class BotTaskEat :BotTask
    {
        public string Name = String.Empty;

        public BotTaskEat()
        {

        }

        public BotTaskEat(string Name)
        {
            this.Name = Name;
        }
    }
}
