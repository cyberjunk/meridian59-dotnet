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
using System.Collections.Generic;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A group of Meridian 59 players.
    /// </summary>
    [Serializable]
    public class Group : INotifyPropertyChanged, IClearable
    {
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_MEMBERS = "Members";

        public event PropertyChangedEventHandler PropertyChanged;

        protected string name;
        protected readonly GroupMemberList members = new GroupMemberList();

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

        public GroupMemberList Members
        {
            get { return members; }         
        }

        public Group()
        {
            members.SortByName();
            Clear(false);
        }
        
        public Group(string Name)
        {
            this.name = Name;
            members.SortByName();
        }

        public Group(string Name, IEnumerable<GroupMember> Members)
        {
            this.name = Name;
            members.SortByName();
            members.AddRange(Members);
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
                members.Clear();
            }
            else
            {
                name = String.Empty;
                members.Clear();
            }
        }
    }
}
