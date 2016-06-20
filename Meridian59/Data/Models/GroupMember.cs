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
    /// A member (m59 player) of a Group.
    /// </summary>
    [Serializable]
    public class GroupMember : INotifyPropertyChanged, IClearable
    {
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_ISONLINE = "IsOnline";

        public event PropertyChangedEventHandler PropertyChanged;

        protected string name;
        protected bool isOnline;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NAME));
                }
            }
        }

        public bool IsOnline
        {
            get { return isOnline; }
            set
            {
                if (this.isOnline != value)
                {
                    this.isOnline = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISONLINE));
                }
            }
        }

        public GroupMember()
        {
            Clear(false);
        }

        public GroupMember(string Name, bool IsOnline = false)
        {
            this.name = Name;
            this.isOnline = IsOnline;
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {              
                Name = String.Empty;
                IsOnline = false;
            }
            else
            {
                name = String.Empty;
                isOnline = false;
            }
        }
    }
}
