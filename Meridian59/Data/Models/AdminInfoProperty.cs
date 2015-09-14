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
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A property of a blakserv object described by
    /// Name (like 'piRow'), Type (like 'INT') and Value (like '22')
    /// </summary>
    public class AdminInfoProperty : INotifyPropertyChanged, IClearable
    {
        public const string PROPNAME_PROPERTYNAME  = "PropertyName";
        public const string PROPNAME_PROPERTYTYPE  = "PropertyType";
        public const string PROPNAME_PROPERTYVALUE = "PropertyValue";

        public event PropertyChangedEventHandler PropertyChanged;

        protected string propertyName;
        protected string propertyType;
        protected string propertyValue;

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set
            {
                if (propertyName != value)
                {
                    propertyName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PROPERTYNAME));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyType
        {
            get { return propertyType; }
            set
            {
                if (propertyType != value)
                {
                    propertyType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PROPERTYTYPE));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyValue
        {
            get { return propertyValue; }
            set
            {
                if (propertyValue != value)
                {
                    propertyValue = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PROPERTYVALUE));
                }
            }
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public AdminInfoProperty()
        {
            Clear(false);
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyType"></param>
        /// <param name="PropertyValue"></param>
        public AdminInfoProperty(string PropertyName, string PropertyType, string PropertyValue)
        {
            propertyName = PropertyName;
            propertyType = PropertyType;
            propertyValue = PropertyValue;
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                PropertyName = String.Empty;
                PropertyType = String.Empty;
                PropertyValue = String.Empty;
            }
            else
            {
                propertyName = String.Empty;
                propertyType = String.Empty;
                propertyValue = String.Empty;
            }
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
    }
}
