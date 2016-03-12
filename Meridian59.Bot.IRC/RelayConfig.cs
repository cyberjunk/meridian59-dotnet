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
        public string IgnoreSystemRegex { get; set; }
        public string IgnoreAllRegex { get; set; }

        public RelayConfig(string name, string banner, string sysregex, string allregex)
        {
            Name = name;
            Banner = banner;
            IgnoreSystemRegex = sysregex;
            IgnoreAllRegex = allregex;
        }
    }
}
