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
using Meridian59.Data.Lists;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminInfoObject : INotifyPropertyChanged, IClearable, IUpdatable<AdminInfoObject>
    {
        #region Constants
        public const string PROPNAME_ID = "ID";
        public const string PROPNAME_CLASSNAME = "ClassName";
        public const string PROPNAME_PROPERTIES = "Properties";
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected uint id;
        protected string className;
        protected readonly BaseList<AdminInfoProperty> properties = new BaseList<AdminInfoProperty>();

        /// <summary>
        /// 
        /// </summary>
        public uint ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ID));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClassName
        {
            get { return className; }
            set
            {
                if (className != value)
                {
                    className = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CLASSNAME));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseList<AdminInfoProperty> Properties { get { return properties; } }       

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public AdminInfoObject()
        {
            Clear(false);
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        public AdminInfoObject(uint ID, string ClassName, IEnumerable<AdminInfoProperty> Properties)
        {
            id = ID;
            className = ClassName;
            properties.AddRange(Properties);
        }
 
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ID = 0;
                ClassName = String.Empty;

                properties.Clear();
            }
            else
            {
                id = 0;
                className = String.Empty;

                properties.Clear();
            }
        }

        public static AdminInfoObject TryParse(string Text)
        {
            Regex regex;
            MatchCollection matches;
            uint id;
            string className;
            AdminInfoProperty[] props;

            // 1) Check if this is an object response

            regex = new Regex(@":< OBJECT (?<id>\d*) is CLASS (?<classname>.*)");
            matches = regex.Matches(Text);
            
            // must have exactly one occurence if it is what we're looking for
            if (matches.Count != 1)
                return null;

            // get values for id and classname
            Group gID = matches[0].Groups["id"];
            Group gClassName = matches[0].Groups["classname"];

            if (!UInt32.TryParse(gID.ToString(), out id))
                id = 0;

            className = gClassName.ToString();
            
            // 2) Parse out properties

            regex = new Regex(@": (?<propname>\w*)\s*=\s(?<proptype>[\w$]*)\s(?<propvalue>\w*)");
            matches = regex.Matches(Text);

            props = new AdminInfoProperty[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                props[i] = new AdminInfoProperty(                
                    matches[i].Groups["propname"].ToString(),
                    matches[i].Groups["proptype"].ToString(),
                    matches[i].Groups["propvalue"].ToString());
            }

            // 3) Return instance
            return new AdminInfoObject(id, className, props);
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        public void UpdateFromModel(AdminInfoObject Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ID = Model.ID;
                ClassName = Model.ClassName;

                properties.Clear();
                properties.AddRange(Model.Properties);
            }
            else
            {
                id = Model.ID;
                className = Model.ClassName;

                properties.Clear();
                properties.AddRange(Model.Properties);
            }
        }
    }
}
