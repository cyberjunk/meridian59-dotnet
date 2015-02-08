/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Xml;
using Meridian59.Common;

namespace Meridian59.Bot.Shop
{
    /// <summary>
    /// Reads ShopBot configuration file
    /// </summary>
    public class ShopBotConfig : BotConfig
    {
        #region Constants
        protected const string XMLTAG_OFFERLIST             = "offerlist";
        protected const string XMLTAG_BUYLIST               = "buylist";
        protected const string XMLATTRIB_COUNT              = "count";        
        protected const string XMLATTRIB_UNITPRICE          = "unitprice";
        protected const string XMLATTRIB_INTERVALBROADCAST  = "intervalbroadcast";
        protected const string XMLATTRIB_INTERVALSAY        = "intervalsay";
        protected const string XMLATTRIB_TELLONENTER        = "tellonenter";
        protected const string XMLATTRIB_CHATPREFIXSTRING   = "chatprefixstring";
        protected const string XMLATTRIB_SHOPNAME           = "shopname";
        #endregion

        #region Properties
        public uint IntervalBroadcast { get; protected set; }
        public uint IntervalSay { get; protected set; }
        public bool TellOnEnter { get; protected set; }
        public string ChatPrefixString { get; protected set; }
        public string Shopname { get; protected set; }
        public List<ShopItem> OfferList { get; protected set; }
        public List<ShopItem> BuyList { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopBotConfig()
            : base() { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPreConfig()
        {
            base.InitPreConfig();

            OfferList = new List<ShopItem>();
            BuyList = new List<ShopItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPastConfig()
        {
            base.InitPastConfig();
        }

        /// <summary>
        /// Looks up an entry from the buylist.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Entry or NULL</returns>
        public ShopItem BuyListGetItemByName(string Name)
        {
            foreach (ShopItem obj in BuyList)
                if (obj.Name.ToLower() == Name.ToLower())               
                    return obj;

            return null;
        }

        /// <summary>
        /// Looks up an entry from the offerlist
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Entry or NULL</returns>
        public ShopItem OfferListGetItemByName(string Name)
        {
            foreach (ShopItem obj in OfferList)
                if (obj.Name.ToLower() == Name.ToLower())
                    return obj;

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reader"></param>
        public override void ReadXml(XmlReader Reader)
        {
            // read baseclass part
            base.ReadXml(Reader);

            // shopbot
            Reader.ReadToFollowing(XMLTAG_BOT);
            IntervalBroadcast = GameTick.MSINSECOND * Convert.ToUInt32(Reader[XMLATTRIB_INTERVALBROADCAST]);
            IntervalSay = GameTick.MSINSECOND * Convert.ToUInt32(Reader[XMLATTRIB_INTERVALSAY]);
            TellOnEnter = Convert.ToBoolean(Reader[XMLATTRIB_TELLONENTER]);
            ChatPrefixString = Reader[XMLATTRIB_CHATPREFIXSTRING];
            Shopname = Reader[XMLATTRIB_SHOPNAME];

            // vars for lists
            string name;
            uint unitprice;
            uint amount;

            // offerlist
            OfferList.Clear();
            Reader.ReadToFollowing(XMLTAG_OFFERLIST);
            if (Reader.ReadToDescendant(XMLTAG_ITEM))
            {
                do
                {
                    name = Reader[XMLATTRIB_NAME];
                    unitprice = Convert.ToUInt32(Reader[XMLATTRIB_UNITPRICE]);
                    amount = Convert.ToUInt32(Reader[XMLATTRIB_COUNT]);

                    OfferList.Add(new ShopItem(name, unitprice, amount));
                }
                while (Reader.ReadToNextSibling(XMLTAG_ITEM));
            }
            
            // buylist
            BuyList.Clear();
            Reader.ReadToFollowing(XMLTAG_BUYLIST);
            if (Reader.ReadToDescendant(XMLTAG_ITEM))
            {
                do
                {
                    name = Reader[XMLATTRIB_NAME];
                    unitprice = Convert.ToUInt32(Reader[XMLATTRIB_UNITPRICE]);
                    amount = Convert.ToUInt32(Reader[XMLATTRIB_COUNT]);

                    BuyList.Add(new ShopItem(name, unitprice, amount));
                }
                while (Reader.ReadToNextSibling(XMLTAG_ITEM));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Writer"></param>
        public override void WriteXml(XmlWriter Writer)
        {
            base.WriteXml(Writer);
        }
    }
}
