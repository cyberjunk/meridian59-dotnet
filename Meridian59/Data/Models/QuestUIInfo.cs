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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A set of information for the NPC quest window.
    /// </summary>
    [Serializable]
    public class QuestUIInfo : INotifyPropertyChanged, IClearable
    {        
        #region Constants
        public const string PROPNAME_QUESTGIVER = "QuestGiver";
        public const string PROPNAME_QUESTLIST = "QuestList";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region Fields
        protected ObjectBase questGiver;
        protected readonly BaseList<QuestObjectInfo> questList = new BaseList<QuestObjectInfo>();
        protected bool isVisible;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ObjectBase QuestGiver
        {
            get
            {
                return questGiver;
            }
            set
            {
                if (questGiver != value)
                {
                    questGiver = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_QUESTGIVER));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseList<QuestObjectInfo> QuestList { get { return questList; } }

        /// <summary>
        /// 
        /// </summary>
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
        public QuestUIInfo()
        {
            Clear(false);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                QuestGiver = null;
                QuestList.Clear();
                IsVisible = false;
            }
            else
            {
                QuestGiver = null;
                questList.Clear();
                isVisible = false;
            }
        }
        #endregion
    }
}
