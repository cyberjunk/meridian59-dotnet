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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An keyvalue pair with both strings
    /// </summary>
    [Serializable]
    public class KeyValuePairString : INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_KEY    = "Key";
        public const string PROPNAME_VALUE  = "Value";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region Fields
        protected string key;
        protected string value;
        #endregion

        #region Properties
        public string Key
        {
            get { return key; }
            set
            {
                if (this.key != value)
                {
                    this.key = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_KEY));
                }
            }
        }

        public string Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VALUE));
                }
            }
        }
        #endregion

        #region Constructors
        public KeyValuePairString()
        {
            Clear(false);
        }
        
        public KeyValuePairString(string Key, string Value)
        {
            this.key = Key;
            this.value = Value;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion

        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Key = String.Empty;               
                Value = String.Empty;
            }
            else
            {
                key = String.Empty;
                value = String.Empty;
            }
        }
        #endregion
    }
}
