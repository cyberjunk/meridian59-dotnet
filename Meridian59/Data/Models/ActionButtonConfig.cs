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
    /// 
    /// </summary>
    public class ActionButtonConfig : INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_NUM = "Num";
        public const string PROPNAME_BUTTONTYPE = "ButtonType";
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_DATA = "Data";
        public const string PROPNAME_LABEL = "Label";
        public const string PROPNAME_NUMOFSAMENAME = "NumOfSameName";
        #endregion

        public event EventHandler Activated;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        protected int num;
        protected ActionButtonType buttonType;
        protected string name;
        protected object data;
        protected string label;
        protected uint numOfSameName;

        #region Properties
        public int Num
        {
            get { return num; }
            set
            {
                if (num != value)
                {
                    num = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUM));
                }
            }
        }

        public ActionButtonType ButtonType
        {
            get { return buttonType; }
            set
            {
                if (buttonType != value)
                {
                    buttonType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_BUTTONTYPE));
                }
            }
        }

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

        public object Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DATA));
                }
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                if (label != value)
                {
                    // Display nothing instead of Key_0, because Key_0
                    // might be confusing for users.
                    if (value.Equals("Key_0"))
                        label = "";
                    else
                        label = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LABEL));
                }
            }
        }

        public uint NumOfSameName
        {
            get { return numOfSameName; }
            set
            {
                if (numOfSameName != value)
                {
                    numOfSameName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUMOFSAMENAME));
                }
            }
        }
        #endregion

        #region Constructors
        public ActionButtonConfig()
        {
            Clear(false);
        }

        public ActionButtonConfig(int Num, ActionButtonType ButtonType, string Name, object Data = null, string Label = null, uint NumOfSameName = 0)
        {
            num = Num;
            buttonType = ButtonType;
            numOfSameName = NumOfSameName;

            if (Label == null)
                Label = String.Empty;

            label = Label;

            if (ButtonType == ActionButtonType.Action)
            {
                data = GetAction(Name);
                name = ((AvatarAction)data).ToString();
            }
            else
            {              
                name = Name;
                data = Data;
            }
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Num = 1;
                ButtonType = ActionButtonType.Unset;
                Name = String.Empty;
                Data = null;
                NumOfSameName = 0;
            }
            else
            {
                num = 1;
                buttonType = ActionButtonType.Unset;
                name = String.Empty;
                data = null;
                numOfSameName = 0;
            }
        }
        #endregion

        protected void RemoveListener()
        {
            if (data == null)
                return;

            switch(buttonType)
            {
                case ActionButtonType.Item:
                case ActionButtonType.Spell:
                    ((ObjectBase)data).PropertyChanged -= OnObjectPropertyChanged;
                    break;
            }
        }

        public void SetToUnset()
        {
            RemoveListener();
            buttonType = ActionButtonType.Unset;
            name = String.Empty;
            numOfSameName = 0;
            Data = null;
        }

        public void SetToAction(AvatarAction Action)
        {
            RemoveListener();
            buttonType = ActionButtonType.Action;
            name = Action.ToString();
            numOfSameName = 0;
            Data = Action;
        }

        public void SetToSpell(SpellObject Spell)
        {
            if (Spell == null)
                return;

            // remove attached listener
            RemoveListener();

            buttonType = ActionButtonType.Spell;
            name = Spell.Name;
            numOfSameName = 0;

            Data = Spell;
            Spell.PropertyChanged += OnObjectPropertyChanged;
        }

        public void SetToSkill(SkillObject Skill)
        {
            if (Skill == null)
                return;

            // remove attached listener
            RemoveListener();

            buttonType = ActionButtonType.Skill;
            name = Skill.Name;
            numOfSameName = 0;

            Data = Skill;
            Skill.PropertyChanged += OnObjectPropertyChanged;
        }

        public void SetToItem(InventoryObject Item)
        {
            if (Item == null)
                return;

            RemoveListener();

            buttonType = ActionButtonType.Item;
            name = Item.Name;
            numOfSameName = Item.NumOfSameName;

            Data = Item;
            Item.PropertyChanged += OnObjectPropertyChanged;
        }

        public void SetToAlias(KeyValuePairString Item)
        {
            if (Item == null)
                return;

            RemoveListener();
            buttonType = ActionButtonType.Alias;
            name = Item.Key;
            numOfSameName = 0;
            Data = Item;
        }

        private void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case ObjectBase.PROPNAME_NAME:
                    Name = ((ObjectBase)sender).Name;
                    break;
            }
        }

        public void Activate()
        {
            if (Activated != null)
                Activated(this, new EventArgs());
        }

        /// <summary>
        /// Returns an AvatarAction from a string.
        /// </summary>
        /// <returns></returns>
        public static AvatarAction GetAction(string Name)
        {
            AvatarAction action = AvatarAction.None;               
            string lower = Name.ToLower();

            if (String.Equals(lower, AvatarAction.Attack.ToString().ToLower()))
            {
                action = AvatarAction.Attack;
            }
            else if (String.Equals(lower, AvatarAction.Dance.ToString().ToLower()))
            {
                action = AvatarAction.Dance;
            }
            else if (String.Equals(lower, AvatarAction.Loot.ToString().ToLower()))
            {
                action = AvatarAction.Loot;
            }
            else if (String.Equals(lower, AvatarAction.Point.ToString().ToLower()))
            {
                action = AvatarAction.Point;
            }
            else if (String.Equals(lower, AvatarAction.Rest.ToString().ToLower()))
            {
                action = AvatarAction.Rest;
            }
            else if (String.Equals(lower, AvatarAction.Buy.ToString().ToLower()))
            {
                action = AvatarAction.Buy;
            }
            else if (String.Equals(lower, AvatarAction.Inspect.ToString().ToLower()))
            {
                action = AvatarAction.Inspect;
            }
            else if (String.Equals(lower, AvatarAction.Trade.ToString().ToLower()))
            {
                action = AvatarAction.Trade;
            }
            else if (String.Equals(lower, AvatarAction.Activate.ToString().ToLower()))
            {
                action = AvatarAction.Activate;
            }
            else if (String.Equals(lower, AvatarAction.GuildInvite.ToString().ToLower()))
            {
                action = AvatarAction.GuildInvite;
            }
            else
            {
                action = AvatarAction.Wave;
            }

            return action;        
        }
    }
}
