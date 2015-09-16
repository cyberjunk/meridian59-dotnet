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
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A set of information for avatar selection with MOTD.
    /// </summary>
    [Serializable]
    public class WelcomeInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<WelcomeInfo>
    {        
        #region Constants
        public const string PROPNAME_CHARACTERS = "Characters";
        public const string PROPNAME_MOTD = "MOTD";
        public const string PROPNAME_ADS = "Ads";
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
                int len = 0;

                // charcountlen + chars
                len += TypeSizes.SHORT;
                foreach (CharSelectItem obj in characters)
                    len += obj.ByteLength;

                // welcomemsglen + welcomemsg
                len += TypeSizes.SHORT + motd.Length;

                // adscountlen + ads   
                len += TypeSizes.BYTE;
                foreach (CharSelectAd obj in ads)
                    len += obj.ByteLength;

                return len;
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
           
            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            for (ushort i = 0; i < len; i++)
            {
                Characters.Add(new CharSelectItem(Buffer, cursor));
                cursor += Characters[i].ByteLength;
            }

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            MOTD = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += MOTD.Length;

            byte blen = Buffer[cursor];
            cursor++;

            for (byte i = 0; i < blen; i++)
            {
                Ads.Add(new CharSelectAd(Buffer, cursor));
                cursor += Ads[i].ByteLength;
            }

            return cursor - StartIndex;
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Characters.Count)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (CharSelectItem obj in Characters)
                cursor += obj.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(MOTD.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(MOTD), 0, Buffer, cursor, MOTD.Length);
            cursor += MOTD.Length;

            Buffer[cursor] = Convert.ToByte(Ads.Count);
            cursor++;

            foreach (CharSelectAd obj in Ads)
                cursor += obj.WriteTo(Buffer, cursor);

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
        protected readonly BaseList<CharSelectItem> characters = new BaseList<CharSelectItem>();
        protected string motd;
        protected readonly BaseList<CharSelectAd> ads = new BaseList<CharSelectAd>();
        #endregion

        #region Properties
        public BaseList<CharSelectItem> Characters
        {
            get
            {
                return characters;
            }
        }

        public string MOTD
        {
            get
            {
                return motd;
            }
            set
            {
                if (motd != value)
                {
                    motd = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTD));
                }
            }
        }

        public BaseList<CharSelectAd> Ads
        {
            get
            {
                return ads;
            }
        }
        #endregion

        #region Constructors
        public WelcomeInfo()
        {
            Clear(false);
        }

        public WelcomeInfo(IEnumerable<CharSelectItem> Characters, IEnumerable<CharSelectAd> Ads, string MOTD)
        {
            characters.AddRange(Characters);
            ads.AddRange(Ads);
            motd = MOTD;
        }

        public WelcomeInfo(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }    
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Characters.Clear();
                Ads.Clear();
                MOTD = String.Empty;
            }
            else
            {
                characters.Clear();
                ads.Clear();
                motd = String.Empty;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(WelcomeInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                characters.Clear();
                ads.Clear();

                foreach(CharSelectItem itm in Model.Characters)
                    characters.Add(itm);

                foreach(CharSelectAd ad in Model.Ads)
                    ads.Add(ad);

                MOTD = Model.MOTD;
            }
            else
            {
                characters.Clear();
                ads.Clear();

                foreach(CharSelectItem itm in Model.Characters)
                    characters.Add(itm);

                foreach(CharSelectAd ad in Model.Ads)
                    ads.Add(ad);

                motd = Model.MOTD;
            }
        }
        #endregion
    }
}
