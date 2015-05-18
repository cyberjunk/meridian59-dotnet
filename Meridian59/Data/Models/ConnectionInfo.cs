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
using System.Collections.Generic;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Set of data representing a connection to a M59 server.
    /// </summary>
    public class ConnectionInfo : INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const ushort DEFAULTPORT     = 5959;
        public const string PROPNAME_NAME   = "Name";
        public const string PROPNAME_HOST   = "Host";
        public const string PROPNAME_PORT   = "Port";
        public const string PROPNAME_USEIPV6 = "UseIPv6";
        public const string PROPNAME_STRINGDICTIONARY   = "StringDictionary";
        public const string PROPNAME_USERNAME           = "Username";
        public const string PROPNAME_PASSWORD           = "Password";
        public const string PROPNAME_CHARACTER          = "Character";
        public const string PROPNAME_IGNORELIST         = "IgnoreList";
        #endregion

        #region Fields
        protected string name;
        protected string host;
        protected ushort port;
        protected bool useIPv6;
        protected string stringdictionary;
        protected string username;
        protected string password;
        protected string character;
        protected List<string> ignoreList;
        #endregion

        #region Properties
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

        public string Host
        {
            get { return host; }
            set
            {
                if (host != value)
                {
                    host = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HOST));
                }
            }
        }

        public ushort Port
        {
            get { return port; }
            set
            {
                if (port != value)
                {
                    port = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PORT));
                }
            }
        }

        public bool UseIPv6
        {
            get { return useIPv6; }
            set
            {
                if (useIPv6 != value)
                {
                    useIPv6 = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_USEIPV6));
                }
            }
        }

        public string StringDictionary
        {
            get { return stringdictionary; }
            set
            {
                if (stringdictionary != value)
                {
                    stringdictionary = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_STRINGDICTIONARY));
                }
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_USERNAME));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PASSWORD));
                }
            }
        }

        public string Character
        {
            get { return character; }
            set
            {
                if (character != value)
                {
                    character = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CHARACTER));
                }
            }
        }

        public List<string> IgnoreList
        {
            get { return ignoreList; }
            protected set
            {
                if (ignoreList != value)
                {
                    ignoreList = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_IGNORELIST));
                }
            }
        } 
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        public ConnectionInfo() 
        {
            ignoreList = new List<string>();

            Clear(false);
        }

        public ConnectionInfo(
            string Name, 
            string Host, 
            ushort Port,
            bool UseIPv6,
            string StringDictionary, 
            string Username,
            string Password,
            string Character,
            IEnumerable<string> IgnoreList)
        {
            ignoreList = new List<string>();

            name = Name;
            host = Host;
            port = Port;
            useIPv6 = UseIPv6;
            stringdictionary = StringDictionary;
            username = Username;
            password = Password;
            character = Character;

            foreach (string s in IgnoreList)
                ignoreList.Add(s);
        }

        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Name = String.Empty;
                Host = String.Empty;
                Port = DEFAULTPORT;
                UseIPv6 = false;
                StringDictionary = String.Empty;
                Username = String.Empty;
                Password = String.Empty;
                Character = String.Empty;
                IgnoreList.Clear();
            }
            else
            {
                name = String.Empty;
                host = String.Empty;
                port = DEFAULTPORT;
                useIPv6 = false;
                stringdictionary = String.Empty;
                username = String.Empty;
                password = String.Empty;
                character = String.Empty;
                IgnoreList.Clear();
            }
        }
    }
}
