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

using Meridian59.Common.Enums;
using System;
using System.ComponentModel;

namespace Meridian59.Common
{
    /// <summary>
    /// An internal log message for debugging
    /// </summary>
    public class LogMessage : INotifyPropertyChanged
    {
        #region Constants
        public const string PROPNAME_MODULE = "Module";
        public const string PROPNAME_TYPE = "Type"; 
        public const string PROPNAME_MESSAGE = "Message";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected string module = String.Empty;
        protected LogType time = LogType.Info;
        protected string message = String.Empty;
        #endregion

        #region Properties
        public string Module
        {
            get { return module; }
            set
            {
                module = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MODULE));
            }
        }
        public LogType Type
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TYPE));
            }
        }
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MESSAGE));
            }
        }
        #endregion

        public LogMessage()
        {       
        }

        public LogMessage(string Module, LogType Type, string Message)
        {       
            this.Module = Module;
            this.Type = Type;
            this.Message = Message;
        }

        public override string ToString()
        {
            return Message;
        }      
    }
}
