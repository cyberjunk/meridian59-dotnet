using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meridian59.Bot.IRC
{
    public class RelayConfig
    {
        public string Name { get; set; }
        public string Banner { get; set; }
        public string Regex { get; set; }

        public RelayConfig(string name, string banner, string regex)
        {
            Name = name;
            Banner = banner;
            Regex = regex;
        }
    }
}
