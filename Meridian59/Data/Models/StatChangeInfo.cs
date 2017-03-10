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
using System.ComponentModel;
using Meridian59.Common.Interfaces;
using Meridian59.Data.Lists;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// The underlying data shown in a stat change dialog
    /// </summary>
    [Serializable]
    public class StatChangeInfo : INotifyPropertyChanged, IClearable, IByteSerializable, IUpdatable<StatChangeInfo>
    {
        #region Constants
        public const uint ATTRIBUTE_MINVALUE = 1;
        public const uint ATTRIBUTE_MAXVALUE = 50;
        public const uint ATTRIBUTE_MAXSUM = 200;
        public const uint SCHOOL_MINVALUE = 0;
        public const uint SCHOOL_MAXVALUE = 6;

        public const string PROPNAME_MIGHT          = "Might";
        public const string PROPNAME_INTELLECT      = "Intellect";
        public const string PROPNAME_STAMINA        = "Stamina";
        public const string PROPNAME_AGILITY        = "Agility";
        public const string PROPNAME_MYSTICISM      = "Mysticism";
        public const string PROPNAME_AIM            = "Aim";
        public const string PROPNAME_ATTRIBUTESCURRENT = "AttributesCurrent";
        public const string PROPNAME_ATTRIBUTESAVAILABLE = "AttributesAvailable";
        public const string PROPNAME_LEVELSHA       = "LevelSha";
        public const string PROPNAME_LEVELQOR       = "LevelQor";
        public const string PROPNAME_LEVELKRAANAN   = "LevelKraanan";
        public const string PROPNAME_LEVELFAREN     = "LevelFaren";
        public const string PROPNAME_LEVELRIIJA     = "LevelRiija";
        public const string PROPNAME_LEVELJALA      = "LevelJala";
        public const string PROPNAME_LEVELWC        = "LevelWC";
        public const string PROPNAME_ORIGLEVELSHA = "OrigLevelSha";
        public const string PROPNAME_ORIGLEVELQOR = "OrigLevelQor";
        public const string PROPNAME_ORIGLEVELKRAANAN = "OrigLevelKraanan";
        public const string PROPNAME_ORIGLEVELFAREN = "OrigLevelFaren";
        public const string PROPNAME_ORIGLEVELRIIJA = "OrigLevelRiija";
        public const string PROPNAME_ORIGLEVELJALA = "OrigLevelJala";
        public const string PROPNAME_ORIGLEVELWC = "OrigLevelWC";
        public const string PROPNAME_ISVISIBLE      = "IsVisible";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public int ByteLength
        {
            get
            {
                // 13x 1 byte value
                return TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE +
                    TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE;
            }
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Might = Buffer[cursor];
            cursor++;

            Intellect = Buffer[cursor];
            cursor++;

            Stamina = Buffer[cursor];
            cursor++;

            Agility = Buffer[cursor];
            cursor++;
            
            Mysticism = Buffer[cursor];
            cursor++;

            Aim = Buffer[cursor];
            cursor++;

            OrigLevelSha = Buffer[cursor];
            LevelSha = OrigLevelSha;
            cursor++;

            OrigLevelQor = Buffer[cursor];
            LevelQor = OrigLevelQor;
            cursor++;

            OrigLevelKraanan = Buffer[cursor];
            LevelKraanan = OrigLevelKraanan;
            cursor++;

            OrigLevelFaren = Buffer[cursor];
            LevelFaren = OrigLevelFaren;
            cursor++;

            OrigLevelRiija = Buffer[cursor];
            LevelRiija = OrigLevelRiija;
            cursor++;

            OrigLevelJala = Buffer[cursor];
            LevelJala = OrigLevelJala;
            cursor++;

            OrigLevelWC = Buffer[cursor];
            LevelWC = origLevelWC;
            cursor++;

            return cursor - StartIndex;
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = Might;
            cursor++;

            Buffer[cursor] = Intellect;
            cursor++;

            Buffer[cursor] = Stamina;
            cursor++;

            Buffer[cursor] = Agility;
            cursor++;

            Buffer[cursor] = Mysticism;
            cursor++;

            Buffer[cursor] = Aim;
            cursor++;

            Buffer[cursor] = LevelSha;
            cursor++;

            Buffer[cursor] = LevelQor;
            cursor++;

            Buffer[cursor] = LevelKraanan;
            cursor++;

            Buffer[cursor] = LevelFaren;
            cursor++;

            Buffer[cursor] = LevelRiija;
            cursor++;

            Buffer[cursor] = LevelJala;
            cursor++;

            Buffer[cursor] = LevelWC;
            cursor++;

            return cursor - StartIndex;
        }

        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
        }
        #endregion

        #region Fields
        protected byte might;
        protected byte intellect;
        protected byte stamina;
        protected byte agility;
        protected byte mysticism;
        protected byte aim;
        protected byte availableStats;
        protected byte levelSha;
        protected byte levelQor;
        protected byte levelKraanan;
        protected byte levelFaren;
        protected byte levelRiija;
        protected byte levelJala;
        protected byte levelWC;

        // Original Levels
        protected byte origLevelSha;
        protected byte origLevelQor;
        protected byte origLevelKraanan;
        protected byte origLevelFaren;
        protected byte origLevelRiija;
        protected byte origLevelJala;
        protected byte origLevelWC;

        protected bool isVisible;
        #endregion

        #region Properties
        public byte Might
        {
            get { return might; }
            set
            {
                if (might != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < might || (int)AttributesAvailable + (int)might - (int)value >= 0))
                {
                    might = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MIGHT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public byte Intellect
        {
            get { return intellect; }
            set
            {
                if (intellect != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < intellect || (int)AttributesAvailable + (int)intellect - (int)value >= 0))
                {
                    intellect = (value >= IntellectNeeded) ? value : (byte)IntellectNeeded;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_INTELLECT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public byte Stamina
        {
            get { return stamina; }
            set
            {
                if (stamina != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < stamina || (int)AttributesAvailable + (int)stamina - (int)value >= 0))
                {
                    stamina = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_STAMINA));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public byte Agility
        {
            get { return agility; }
            set
            {
                if (agility != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < agility || (int)AttributesAvailable + (int)agility - (int)value >= 0))
                {
                    agility = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AGILITY));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public byte Mysticism
        {
            get { return mysticism; }
            set
            {
                if (mysticism != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < mysticism || (int)AttributesAvailable + (int)mysticism - (int)value >= 0))
                {
                    mysticism = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MYSTICISM));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public byte Aim
        {
            get { return aim; }
            set
            {
                if (aim != value && value >= ATTRIBUTE_MINVALUE && value <= ATTRIBUTE_MAXVALUE &&
                   (value < aim || (int)AttributesAvailable + (int)aim - (int)value >= 0))
                {
                    aim = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AIM));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESCURRENT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ATTRIBUTESAVAILABLE));
                }
            }
        }

        public uint AttributesCurrent
        {
            get { return (uint) (might + intellect + stamina + agility + mysticism + aim); }
        }

        public uint AttributesAvailable
        {
            get { return ATTRIBUTE_MAXSUM - Math.Min(AttributesCurrent, ATTRIBUTE_MAXSUM); }
        }

        public byte LevelSha
        {
            get
            {
                return levelSha;
            }
            set
            {
                if (levelSha != value && value <= origLevelSha)
                {
                    levelSha = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELSHA));
                }
            }
        }

        public byte LevelQor
        {
            get
            {
                return levelQor;
            }
            set
            {
                if (levelQor != value && value <= origLevelQor)
                {
                    levelQor = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELQOR));
                }
            }
        }

        public byte LevelKraanan
        {
            get
            {
                return levelKraanan;
            }
            set
            {
                if (levelKraanan != value && value <= origLevelKraanan)
                {
                    levelKraanan = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELKRAANAN));
                }
            }
        }

        public byte LevelFaren
        {
            get
            {
                return levelFaren;
            }
            set
            {
                if (levelFaren != value && value <= origLevelFaren)
                {
                    levelFaren = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELFAREN));
                }
            }
        }

        public byte LevelRiija
        {
            get
            {
                return levelRiija;
            }
            set
            {
                if (levelRiija != value && value <= origLevelRiija)
                {
                    levelRiija = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELRIIJA));
                }
            }
        }

        public byte LevelJala
        {
            get
            {
                return levelJala;
            }
            set
            {
                if (levelJala != value && value <= origLevelJala)
                {
                    levelJala = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELJALA));
                }
            }
        }

        public byte LevelWC
        {
            get
            {
                return levelWC;
            }
            set
            {
                if (levelWC != value && value <= origLevelWC)
                {
                    levelWC = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELWC));
                }
            }
        }

       public byte OrigLevelSha
        {
            get
            {
                return origLevelSha;
            }
            protected set
            {
                if (origLevelSha != value)
                {
                    origLevelSha = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELSHA));
                }
            }
        }

       public byte OrigLevelQor
        {
            get
            {
                return origLevelQor;
            }
            protected set
            {
                if (origLevelQor != value)
                {
                    origLevelQor = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELQOR));
                }
            }
        }

       public byte OrigLevelKraanan
        {
            get
            {
                return origLevelKraanan;
            }
            protected set
            {
                if (origLevelKraanan != value)
                {
                    origLevelKraanan = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELKRAANAN));
                }
            }
        }

       public byte OrigLevelFaren
        {
            get
            {
                return origLevelFaren;
            }
            protected set
            {
                if (origLevelFaren != value)
                {
                    origLevelFaren = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELFAREN));
                }
            }
        }

       public byte OrigLevelRiija
        {
            get
            {
                return origLevelRiija;
            }
            protected set
            {
                if (origLevelRiija != value)
                {
                    origLevelRiija = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELRIIJA));
                }
            }
        }

       public byte OrigLevelJala
        {
            get
            {
                return origLevelJala;
            }
            protected set
            {
                if (origLevelJala != value)
                {
                    origLevelJala = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELJALA));
                }
            }
        }

       public byte OrigLevelWC
        {
            get
            {
                return origLevelWC;
            }
            protected set
            {
                if (origLevelWC != value)
                {
                    origLevelWC = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ORIGLEVELWC));
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISVISIBLE));
                }
            }
        }

        /// <summary>
        /// Returns total number of levels.
        /// </summary>
        public int TotalLevels
        {
            get { return (LevelSha + LevelQor + LevelKraanan + LevelFaren + LevelRiija + LevelJala + LevelWC); }
        }

        /// <summary>
        /// Returns current number of schools level 1 or greater.
        /// </summary>
        public int SchoolCount
        {
            get
            {
                int schoolCount = 0;
                if (LevelSha > 0)
                    ++schoolCount;
                if (LevelQor > 0)
                    ++schoolCount;
                if (LevelKraanan > 0)
                    ++schoolCount;
                if (LevelFaren > 0)
                    ++schoolCount;
                if (LevelRiija > 0)
                    ++schoolCount;
                if (LevelJala > 0)
                    ++schoolCount;
                if (LevelWC > 0)
                    ++schoolCount;
                return schoolCount;
            }
        }

        /// <summary>
        /// Returns the amount of intellect needed for the schools
        /// and levels we have set.
        /// </summary>
        public int IntellectNeeded
        {
            get
            {
                return (TotalLevels <= 8) ? 1 : (TotalLevels - SchoolCount - 8) * 5;
            }
        }
        #endregion

        #region Constructors
        public StatChangeInfo()
        {
            Clear(false);
        }

        public StatChangeInfo(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Might = 0;
                Intellect = 0;
                Stamina = 0;
                Agility = 0;
                Mysticism = 0;
                Aim = 0;
                LevelSha = 0;
                LevelQor = 0;
                LevelKraanan = 0;
                LevelFaren = 0;
                LevelRiija = 0;
                LevelJala = 0;
                LevelWC = 0;

                IsVisible = false;
            }
            else
            {
                might = 0;
                intellect = 0;
                stamina = 0;
                agility = 0;
                mysticism = 0;
                aim = 0;
                levelSha = 0;
                levelQor = 0;
                levelKraanan = 0;
                levelFaren = 0;
                levelRiija = 0;
                levelJala = 0;
                levelWC = 0;

                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(StatChangeInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Might = Model.Might;
                Intellect = Model.Intellect;
                Stamina = Model.Stamina;
                Agility = Model.Agility;
                Mysticism = Model.Mysticism;
                Aim = Model.Aim;

                // Set original levels first.
                OrigLevelSha = Model.LevelSha;
                OrigLevelQor = Model.LevelQor;
                OrigLevelKraanan = Model.LevelKraanan;
                OrigLevelFaren = Model.LevelFaren;
                OrigLevelRiija = Model.LevelRiija;
                OrigLevelJala = Model.LevelJala;
                OrigLevelWC = Model.LevelWC;

                LevelSha = Model.LevelSha;
                LevelQor = Model.levelQor;
                LevelKraanan = Model.LevelKraanan;
                LevelFaren = Model.LevelFaren;
                LevelRiija = Model.LevelRiija;
                LevelJala = Model.LevelJala;
                LevelWC = Model.LevelWC;
            }
            else
            {
                might = Model.Might;
                intellect = Model.Intellect;
                stamina = Model.Stamina;
                agility = Model.Agility;
                mysticism = Model.Mysticism;
                aim = Model.Aim;

                // Set original levels first.
                origLevelSha = Model.LevelSha;
                origLevelQor = Model.LevelQor;
                origLevelKraanan = Model.LevelKraanan;
                origLevelFaren = Model.LevelFaren;
                origLevelRiija = Model.LevelRiija;
                origLevelJala = Model.LevelJala;
                origLevelWC = Model.LevelWC;

                levelSha = Model.LevelSha;
                levelQor = Model.LevelQor;
                levelKraanan = Model.LevelKraanan;
                levelFaren = Model.LevelFaren;
                levelRiija = Model.LevelRiija;
                levelJala = Model.LevelJala;
                levelWC = Model.LevelWC;
            }
        }
        #endregion
    }
}
