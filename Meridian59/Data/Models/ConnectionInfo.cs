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
        public const string PROPNAME_STRINGDICTIONARY   = "StringDictionary";
        public const string PROPNAME_USERNAME           = "Username";
        public const string PROPNAME_PASSWORD           = "Password";
        public const string PROPNAME_CHARACTER          = "Character";
        public const string PROPNAME_IGNORELIST         = "IgnoreList";
        public const string PROPNAME_GROUPS             = "Groups";
        #endregion

        #region Hardcoded ConnectionInfos
#if VANILLA
        public static readonly ConnectionInfo CON101 = new ConnectionInfo("101", "meridian101.meridian59.com",  5901, "rsc0000-101.rsb", "", "", "", null);
        public static readonly ConnectionInfo CON102 = new ConnectionInfo("102", "meridian102.meridian59.com",  5902, "rsc0000-101.rsb", "", "", "", null);
#elif OPENMERIDIAN
        public static readonly ConnectionInfo CON103 = new ConnectionInfo("103", "meridian103.openmeridian.org",5903, "rsc0000-103.rsb", "", "", "", null);
        public static readonly ConnectionInfo CON104 = new ConnectionInfo("104", "meridian104.openmeridian.org",5904, "rsc0000-104.rsb", "", "", "", null);
#else
        public static readonly ConnectionInfo CON105 = new ConnectionInfo("105", "meridian105.meridiannext.com",5905, "rsc0000-105.rsb", "", "", "", null);
        public static readonly ConnectionInfo CON106 = new ConnectionInfo("106", "meridian106.meridiannext.com",5906, "rsc0000-106.rsb", "", "", "", null);
        public static readonly ConnectionInfo CON112 = new ConnectionInfo("112", "meridian112.meridian59.de",   5912, "rsc0000-112.rsb", "", "", "", null);
#endif
        #endregion

        #region Fields
        protected string name;
        protected string host;
        protected ushort port;
        protected string stringdictionary;
        protected string username;
        protected string password;
        protected string character;
        protected readonly List<string> ignoreList = new List<string>();
        protected readonly List<Group> groups = new List<Group>();
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
        }

        public List<Group> Groups
        {
            get { return groups; }
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
            Clear(false);
        }

        public ConnectionInfo(
            string Name, 
            string Host, 
            ushort Port,
            string StringDictionary, 
            string Username,
            string Password,
            string Character,
            IEnumerable<string> IgnoreList = null,
            IEnumerable<Group> Groups = null)
        {
            name = Name;
            host = Host;
            port = Port;
            stringdictionary = StringDictionary;
            username = Username;
            password = Password;
            character = Character;

            if (IgnoreList != null)
                ignoreList.AddRange(IgnoreList);

            if (Groups != null)
                groups.AddRange(Groups);
        }

        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Name = String.Empty;
                Host = String.Empty;
                Port = DEFAULTPORT;

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
                stringdictionary = String.Empty;
                username = String.Empty;
                password = String.Empty;
                character = String.Empty;
                IgnoreList.Clear();
            }
        }
    }
}
