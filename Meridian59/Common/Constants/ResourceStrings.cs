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

using Meridian59.Common.Enums;
using System.Collections.Generic;

namespace Meridian59.Common.Constants
{
    /// <summary>
    /// A set of important M59 strings
    /// </summary>
    public static class ResourceStrings
    {
        /// <summary>
        /// All kind of items
        /// </summary>
        public static class Items
        {
            /// <summary>
            /// Strings of armors
            /// </summary>
            public static class Armor
            {                
                public const string PLATE    = "plate armor";
                public const string NERUDITE = "nerudite armor";
                public const string SCALE    = "scale armor";
                public const string CHAIN    = "chain armor";
                public const string LEATHER  = "leather armor";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, PLATE) ||
                        string.Equals(Value, NERUDITE) ||
                        string.Equals(Value, SCALE) ||
                        string.Equals(Value, CHAIN) ||
                        string.Equals(Value, LEATHER));
                }                
            }

            /// <summary>
            /// Strings of shields
            /// </summary>
            public static class Shields
            {
                public const string HERALD = "herald shield";
                public const string KNIGHT = "knight's shield";
                public const string ORC    = "orc shield";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, HERALD) ||
                        string.Equals(Value, KNIGHT) ||
                        string.Equals(Value, ORC));
                }
            }

            /// <summary>
            /// Strings of helms
            /// </summary>
            public static class Helms
            {
                public const string CIRCLET     = "circlet";
                public const string IVYCIRCLET  = "ivy circlet";
                public const string HELM        = "helm";
                public const string MSH         = "magic spirit helmet";
                
                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, CIRCLET) ||
                        string.Equals(Value, IVYCIRCLET) ||
                        string.Equals(Value, HELM) ||
                        string.Equals(Value, MSH));
                }
            }

            /// <summary>
            /// Strings of weapons
            /// </summary>
            public static class Weapons
            {
                public const string NERUDITEBOW = "nerudite bow";
                public const string MAGICBOW    = "magic bow";
                public const string BATTLEBOW   = "battle bow";
                public const string LONGBOW     = "long bow";
                public const string RIIJASWORD  = "sword of Riija";
                public const string GOLDSWORD   = "gold sword";
                public const string MYSTICSWORD = "mystic sword";
                public const string LONGSWORD   = "long sword";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, NERUDITEBOW) ||
                        string.Equals(Value, MAGICBOW) ||
                        string.Equals(Value, BATTLEBOW) ||
                        string.Equals(Value, LONGBOW) ||
                        string.Equals(Value, RIIJASWORD) ||
                        string.Equals(Value, GOLDSWORD) ||
                        string.Equals(Value, MYSTICSWORD) ||
                        string.Equals(Value, LONGSWORD));
                }
            }

            /// <summary>
            /// Strings of food
            /// </summary>
            public static class Food
            {
                public const string CHOCOLATEMINT       = "chocolate mint";
                public const string INKYCAPMUSHROOM     = "Inky-cap mushroom";
                public const string WHEELOFCHEESE       = "wheel of cheese";
                public const string LOAFOFBREAD         = "loaf of bread";
                public const string MUGOFSTOUT          = "mug of stout";
                public const string WATERSKIN           = "water skin";
                public const string GOBLETOFWINE        = "goblet of wine";
                public const string GOBLETOFALE         = "goblet of Ale";
                public const string BUNCHOFGRAPES       = "bunch of grapes";
                public const string EDIBLEMUSHROOM      = "edible mushroom";
                public const string MEATPIE             = "meat pie";
                public const string COPPERPEKONCHMUGS   = "copper pekonch mugs";
                public const string BOWLOFSOUP          = "bowl of soup";
                public const string BOWLOFSTEW          = "bowl of stew";
                public const string FORTUNECOOKIE       = "fortune cookie";
                public const string SLICEOFPORK         = "slice of pork";
                public const string TURKEYLEG           = "turkey leg";
                public const string DRUMSTICK           = "drumstick";
                public const string SPIDEREYE           = "spider eye";
                public const string APPLE               = "apple";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, CHOCOLATEMINT) ||
                        string.Equals(Value, INKYCAPMUSHROOM) ||
                        string.Equals(Value, WHEELOFCHEESE) ||
                        string.Equals(Value, LOAFOFBREAD) ||
                        string.Equals(Value, MUGOFSTOUT) ||
                        string.Equals(Value, WATERSKIN) ||
                        string.Equals(Value, GOBLETOFWINE) ||
                        string.Equals(Value, GOBLETOFALE) ||
                        string.Equals(Value, BUNCHOFGRAPES) ||
                        string.Equals(Value, EDIBLEMUSHROOM) ||
                        string.Equals(Value, MEATPIE) ||
                        string.Equals(Value, COPPERPEKONCHMUGS) ||
                        string.Equals(Value, BOWLOFSOUP) ||
                        string.Equals(Value, BOWLOFSTEW) ||
                        string.Equals(Value, FORTUNECOOKIE) ||
                        string.Equals(Value, SLICEOFPORK) ||
                        string.Equals(Value, TURKEYLEG) ||
                        string.Equals(Value, DRUMSTICK) ||
                        string.Equals(Value, SPIDEREYE) ||
                        string.Equals(Value, APPLE));
                }

                /// <summary>
                /// Lookup nutrition / gain in vigor from here. Do not edit at runtime.
                /// </summary>
                public static readonly Dictionary<string, byte> NUTRITIONS = new Dictionary<string, byte>() {
                    {CHOCOLATEMINT,5},      {INKYCAPMUSHROOM,50},   {WHEELOFCHEESE,30},
                    {LOAFOFBREAD,20},       {MUGOFSTOUT,6},         {WATERSKIN,3},       
                    {GOBLETOFWINE,6},       {GOBLETOFALE,3},        {BUNCHOFGRAPES,7},  
                    {EDIBLEMUSHROOM,5},     {MEATPIE,30},           {COPPERPEKONCHMUGS,3},
                    {BOWLOFSOUP,9},         {BOWLOFSTEW,15},        {FORTUNECOOKIE,1}, 
                    {SLICEOFPORK,9},        {TURKEYLEG,15},         {DRUMSTICK,9},
                    {SPIDEREYE,9},          {APPLE,10},             };               
            }

            /// <summary>
            /// Strings of god gifts
            /// </summary>
            public static class GodGifts
            {                
                public const string FEATHEREDCHOKER = "feathered choker";
                public const string DIAMONDPENDANT  = "diamond pendant";
                public const string BRASSMEDALLION  = "brass medallion";
                public const string STEELTORC       = "steel torc";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, FEATHEREDCHOKER) ||
                        string.Equals(Value, DIAMONDPENDANT) ||
                        string.Equals(Value, BRASSMEDALLION) ||
                        string.Equals(Value, STEELTORC));
                }
            }

            /// <summary>
            /// Strings of other equipment
            /// </summary>
            public static class Equipment
            {
                public const string DISCIPLEROBES   = "robes of the disciple";
                public const string PANTS           = "pants";
                public const string TRUELUTE        = "true lute";
                public const string LUTE            = "lute";
                public const string JALANECKLACE    = "necklace of Jala";
                public const string GAUNTLETS       = "gauntlets";
                public const string JEWELOFFROZ     = "Jewel of Froz";
                public const string LIGHTJERKIN     = "light jerkin";
            }

            /// <summary>
            /// Strings of wands
            /// </summary>
            public static class Wands
            {
                public const string WANDOFHEALING = "wand of healing";
                public const string WANDOFVAMPIRE = "wand of vampiric shock";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, WANDOFHEALING) ||
                        string.Equals(Value, WANDOFVAMPIRE));
                }
            }

            /// <summary>
            /// Others
            /// </summary>
            public static class Others
            {
                public const string GNARLEDSTAFF = "gnarled staff";
                public const string SHILLING = "shilling";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, GNARLEDSTAFF) ||
                        string.Equals(Value, SHILLING));
                }
            }
        }

        /// <summary>
        /// All spellnames
        /// </summary>
        public static class Spells
        {
            public static class Qor
            {
                public const string INVISIBILITY = "invisibility";
                public const string HOLD         = "hold";
                public const string BLIND        = "blind";
                
                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, INVISIBILITY) ||
                        string.Equals(Value, HOLD) ||
                        string.Equals(Value, BLIND));
                }
            }

            public static class Shalille
            {
                public const string DAZZLE      = "dazzle";
                public const string PURGE       = "purge";
                public const string RESISTACID  = "resist acid";
                public const string TRUCE       = "truce";
                public const string MINORHEAL   = "minor heal";
                public const string HOSPICE     = "hospice";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, DAZZLE) ||
                        string.Equals(Value, PURGE) ||
                        string.Equals(Value, RESISTACID) ||
                        string.Equals(Value, TRUCE) ||
                        string.Equals(Value, MINORHEAL) ||
                        string.Equals(Value, HOSPICE));
                }
            }

            public static class Kraanan
            {
                public const string BLESS           = "bless";
                public const string SUPERSTRENGTH   = "super strength";
                public const string RESISTPOISON    = "resist poison";
                public const string DEFLECT         = "deflect";
                public const string DISCORDANCE     = "discordance";
                public const string ARMOROFGORT     = "armor of Gort";
                public const string RESISTMAGIC     = "resist magic";
                public const string MAGICSHIELD     = "magic shield";
                public const string EAGLEEYES       = "eagle eyes";
                public const string FREEACTION      = "free action";
                public const string ANTIMAGICAURA   = "anti-magic aura";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, BLESS) ||
                        string.Equals(Value, SUPERSTRENGTH) ||
                        string.Equals(Value, RESISTPOISON) ||
                        string.Equals(Value, DEFLECT) ||
                        string.Equals(Value, DISCORDANCE) ||
                        string.Equals(Value, ARMOROFGORT) ||
                        string.Equals(Value, RESISTMAGIC) ||
                        string.Equals(Value, MAGICSHIELD) ||
                        string.Equals(Value, EAGLEEYES) ||
                        string.Equals(Value, FREEACTION) ||
                        string.Equals(Value, ANTIMAGICAURA));
                }
            }

            public static class Faren
            {
            }

            public static class Riija
            {
                public const string SHADOWFORM  = "shadowform";
                public const string ELUSION     = "elusion";
                public const string BLINK       = "blink";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, SHADOWFORM) ||
                        string.Equals(Value, ELUSION) ||
                        string.Equals(Value, BLINK));
                }
            }

            public static class Jala
            {
                public const string JIG             = "jig";
                public const string INVIGORATE      = "invigorate";
                public const string MANACONVERGENCE = "mana convergence";
                public const string SPELLBANE       = "spellbane";
                public const string CONCILIATION    = "conciliation";

                public static bool Is(string Value)
                {
                    return (Value != null) && (
                        string.Equals(Value, JIG) ||
                        string.Equals(Value, INVIGORATE) ||
                        string.Equals(Value, MANACONVERGENCE) ||
                        string.Equals(Value, SPELLBANE) ||
                        string.Equals(Value, CONCILIATION));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Sounds
        {
            public const string OUCHMALE1 = "ouchm1.wav";
            public const string OUCHMALE2 = "ouchm2.wav";
            public const string OUCHMALE3 = "ouchm3.wav";
            public const string OUCHMALE4 = "ouchm4.wav";
            public const string OUCHFEMALE1 = "ouchf1.wav";
            public const string OUCHFEMALE2 = "ouchf2.wav";
            public const string OUCHFEMALE3 = "ouchf3.wav";
            public const string OUCHFEMALE4 = "ouchf4.wav";

            public static HealthStatus IsOuch(string Value)
            {
                switch (Value)
                {
                    case OUCHMALE1:
                    case OUCHFEMALE1:
                        return HealthStatus.Green;

                    case OUCHMALE2:
                    case OUCHFEMALE2:
                        return HealthStatus.Yellow;

                    case OUCHMALE3:
                    case OUCHFEMALE3:
                        return HealthStatus.Orange;

                    case OUCHMALE4:
                    case OUCHFEMALE4:
                        return HealthStatus.Red;

                    default:
                        return HealthStatus.Unknown;
                }
            }

            public static bool Is(string Value)
            {
                return (Value != null) && (
                    string.Equals(Value, OUCHMALE1) ||
                    string.Equals(Value, OUCHMALE2) ||
                    string.Equals(Value, OUCHMALE3) ||
                    string.Equals(Value, OUCHMALE4) ||
                    string.Equals(Value, OUCHFEMALE1) ||
                    string.Equals(Value, OUCHFEMALE2) ||
                    string.Equals(Value, OUCHFEMALE3) ||
                    string.Equals(Value, OUCHFEMALE4));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Rooms
        {
            public const string TOSFORGT = "tosforgt.roo";
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Others
        {
            public const string PRINCESSEMBLEM  = "gshprov.bgf";
            public const string PRINCESSEMBLEM2 = "gshprbk.bgf";
            public const string DUKEEMBLEM      = "gshdaov.bgf";
            public const string DUKEEMBLEM2     = "gshdabk.bgf";
            public const string JONASEMBLEM     = "gshreov.bgf";
            public const string JONASEMBLEM2    = "gshrebk.bgf";

        }
    }
}
