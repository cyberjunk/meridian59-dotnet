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
        public const string CONFIGFILE_SHOPBOT     = "../configuration.shopbot.xml";
        public const string CONFIGFILE_SHOPBOT_ALT = "configuration.shopbot.xml";

        protected const string XMLTAG_OFFERLIST             = "offerlist";
        protected const string XMLTAG_BUYLIST               = "buylist";
        protected const string XMLATTRIB_COUNT              = "count";        
        protected const string XMLATTRIB_UNITPRICE          = "unitprice";
        protected const string XMLATTRIB_INTERVALBROADCAST  = "intervalbroadcast";
        protected const string XMLATTRIB_INTERVALSAY        = "intervalsay";
        protected const string XMLATTRIB_TELLONENTER        = "tellonenter";
        protected const string XMLATTRIB_CHATPREFIXSTRING   = "chatprefixstring";
        protected const string XMLATTRIB_SHOPNAME           = "shopname";

        public const uint   DEFAULTVAL_SHOPBOT_INTERVALBROADCAST    = 3600;
        public const uint   DEFAULTVAL_SHOPBOT_INTERVALSAY          = 1800;
        public const bool   DEFAULTVAL_SHOPBOT_TELLONENTER          = true;
        public const string DEFAULTVAL_SHOPBOT_CHATPREFIXSTRING     = "~B~r";
        public const string DEFAULTVAL_SHOPBOT_SHOPNAME             = "~B~rS~n~khop~B~rB~n~kot";
        public const string DEFAULTVAL_OFFERLIST_NAME               = "";
        public const uint   DEFAULTVAL_OFFERLIST_UNITPRICE          = 99999999;
        public const uint   DEFAULTVAL_OFFERLIST_AMOUNT             = 0;

        #endregion

        #region Properties
        public override string ConfigFile { get { return CONFIGFILE_SHOPBOT; } }
        public override string ConfigFileAlt { get { return CONFIGFILE_SHOPBOT_ALT; } }
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
        /// <param name="Document"></param>
        public override void ReadXml(XmlDocument Document)
        {
            // read baseclass part
            base.ReadXml(Document);
            
            uint val_uint;
            bool val_bool;
            XmlNode node;
            string name;
            uint unitprice;
            uint amount;

            OfferList.Clear();
            BuyList.Clear();

            // basics

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT);

            if (node != null)
            {
                IntervalBroadcast = (node.Attributes[XMLATTRIB_INTERVALBROADCAST] != null && UInt32.TryParse(node.Attributes[XMLATTRIB_INTERVALBROADCAST].Value, out val_uint)) ?
                    val_uint * GameTick.MSINSECOND : DEFAULTVAL_SHOPBOT_INTERVALBROADCAST * GameTick.MSINSECOND;

                IntervalSay = (node.Attributes[XMLATTRIB_INTERVALSAY] != null && UInt32.TryParse(node.Attributes[XMLATTRIB_INTERVALSAY].Value, out val_uint)) ?
                    val_uint * GameTick.MSINSECOND : DEFAULTVAL_SHOPBOT_INTERVALSAY * GameTick.MSINSECOND;

                TellOnEnter = (node.Attributes[XMLATTRIB_TELLONENTER] != null && Boolean.TryParse(node.Attributes[XMLATTRIB_TELLONENTER].Value, out val_bool)) ? 
                    val_bool : DEFAULTVAL_SHOPBOT_TELLONENTER;

                ChatPrefixString = (node.Attributes[XMLATTRIB_CHATPREFIXSTRING] != null) ?
                    node.Attributes[XMLATTRIB_CHATPREFIXSTRING].Value : DEFAULTVAL_SHOPBOT_CHATPREFIXSTRING;

                Shopname = (node.Attributes[XMLATTRIB_SHOPNAME] != null) ?
                    node.Attributes[XMLATTRIB_SHOPNAME].Value : DEFAULTVAL_SHOPBOT_SHOPNAME;
            }
            else
            {
                IntervalBroadcast = DEFAULTVAL_SHOPBOT_INTERVALBROADCAST * GameTick.MSINSECOND;
                IntervalSay = DEFAULTVAL_SHOPBOT_INTERVALSAY * GameTick.MSINSECOND;
                TellOnEnter = DEFAULTVAL_SHOPBOT_TELLONENTER;
                ChatPrefixString = DEFAULTVAL_SHOPBOT_CHATPREFIXSTRING;
                Shopname = DEFAULTVAL_SHOPBOT_SHOPNAME;
            }
            
            // offerlist

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT + '/' + XMLTAG_OFFERLIST);

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_ITEM)
                        continue;

                    name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                        child.Attributes[XMLATTRIB_NAME].Value : DEFAULTVAL_OFFERLIST_NAME;

                    unitprice = (child.Attributes[XMLATTRIB_UNITPRICE] != null && UInt32.TryParse(child.Attributes[XMLATTRIB_UNITPRICE].Value, out val_uint)) ?
                        val_uint : DEFAULTVAL_OFFERLIST_UNITPRICE;

                    amount = (child.Attributes[XMLATTRIB_COUNT] != null && UInt32.TryParse(child.Attributes[XMLATTRIB_COUNT].Value, out val_uint)) ?
                        val_uint : DEFAULTVAL_OFFERLIST_AMOUNT;

                    OfferList.Add(new ShopItem(name, unitprice, amount));
                }
            }

            // buylist

            node = Document.DocumentElement.SelectSingleNode(
                '/' + XMLTAG_CONFIGURATION + '/' + XMLTAG_BOT + '/' + XMLTAG_BUYLIST);

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name != XMLTAG_ITEM)
                        continue;

                    name = (child.Attributes[XMLATTRIB_NAME] != null) ?
                        child.Attributes[XMLATTRIB_NAME].Value : DEFAULTVAL_OFFERLIST_NAME;

                    unitprice = (child.Attributes[XMLATTRIB_UNITPRICE] != null && UInt32.TryParse(child.Attributes[XMLATTRIB_UNITPRICE].Value, out val_uint)) ?
                        val_uint : DEFAULTVAL_OFFERLIST_UNITPRICE;

                    amount = (child.Attributes[XMLATTRIB_COUNT] != null && UInt32.TryParse(child.Attributes[XMLATTRIB_COUNT].Value, out val_uint)) ?
                        val_uint : DEFAULTVAL_OFFERLIST_AMOUNT;

                    BuyList.Add(new ShopItem(name, unitprice, amount));
                }
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
