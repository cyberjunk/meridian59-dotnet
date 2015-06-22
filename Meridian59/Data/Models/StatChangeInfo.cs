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
        public const string PROPNAME_MIGHT          = "Might";
        public const string PROPNAME_INTELLECT      = "Intellect";
        public const string PROPNAME_STAMINA        = "Stamina";
        public const string PROPNAME_AGILITY        = "Agility";
        public const string PROPNAME_MYSTICISM      = "Mysticism";
        public const string PROPNAME_AIM            = "Aim";
        public const string PROPNAME_LEVELSHA       = "LevelSha";
        public const string PROPNAME_LEVELQOR       = "LevelQor";
        public const string PROPNAME_LEVELKRAANAN   = "LevelKraanan";
        public const string PROPNAME_LEVELFAREN     = "LevelFaren";
        public const string PROPNAME_LEVELRIIJA     = "LevelRiija";
        public const string PROPNAME_LEVELJALA      = "LevelJala";
        public const string PROPNAME_LEVELWC        = "LevelWC";

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

            LevelSha = Buffer[cursor];
            cursor++;

            LevelQor = Buffer[cursor];
            cursor++;

            LevelKraanan = Buffer[cursor];
            cursor++;

            LevelFaren = Buffer[cursor];
            cursor++;

            LevelRiija = Buffer[cursor];
            cursor++;

            LevelJala = Buffer[cursor];
            cursor++;

            LevelWC = Buffer[cursor];
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
        protected byte levelSha;
        protected byte levelQor;
        protected byte levelKraanan;
        protected byte levelFaren;
        protected byte levelRiija;
        protected byte levelJala;
        protected byte levelWC;

        protected bool isVisible;
        #endregion

        #region Properties
        public byte Might
        {
            get
            {
                return might;
            }
            set
            {
                if (might != value)
                {
                    might = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MIGHT));
                }
            }
        }

        public byte Intellect
        {
            get
            {
                return intellect;
            }
            set
            {
                if (intellect != value)
                {
                    intellect = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_INTELLECT));
                }
            }
        }

        public byte Stamina
        {
            get
            {
                return stamina;
            }
            set
            {
                if (stamina != value)
                {
                    stamina = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_STAMINA));
                }
            }
        }

        public byte Agility
        {
            get
            {
                return agility;
            }
            set
            {
                if (agility != value)
                {
                    agility = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AGILITY));
                }
            }
        }

        public byte Mysticism
        {
            get
            {
                return mysticism;
            }
            set
            {
                if (mysticism != value)
                {
                    mysticism = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MYSTICISM));
                }
            }
        }

        public byte Aim
        {
            get
            {
                return aim;
            }
            set
            {
                if (aim != value)
                {
                    aim = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_AIM));
                }
            }
        }

        public byte LevelSha
        {
            get
            {
                return levelSha;
            }
            set
            {
                if (levelSha != value)
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
                if (levelQor != value)
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
                if (levelKraanan != value)
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
                if (levelFaren != value)
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
                if (levelRiija != value)
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
                if (levelJala != value)
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
                if (levelWC != value)
                {
                    levelWC = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LEVELWC));
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
                levelSha = Model.LevelSha;
                levelQor = Model.levelQor;
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
