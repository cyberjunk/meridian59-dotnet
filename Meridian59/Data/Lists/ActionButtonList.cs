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
using System.Collections.Generic;
using Meridian59.Common.Enums;
using Meridian59.Data.Models;

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionButtonList : BaseList<ActionButtonConfig>
    {
        public string PlayerName { get; set; }
              
        public ActionButtonList()
        {
            PlayerName = String.Empty;                  
        }

        public ActionButtonList(string PlayerName)
        {
            this.PlayerName = PlayerName;           
        }

        public bool HasPlayerName
        {
            get { return PlayerName != null && PlayerName != String.Empty; }
        }

        public ActionButtonConfig GetByNum(int Num)
        {
            foreach (ActionButtonConfig obj in this)
                if (obj.Num == Num)
                    return obj;

            return null;
        }

        public List<ActionButtonConfig> GetSpellButtons()
        {
            List<ActionButtonConfig> list = new List<ActionButtonConfig>();

            foreach (ActionButtonConfig button in this)           
                if (button.ButtonType == ActionButtonType.Spell)
                    list.Add(button);
            
            return list;
        }

        public List<ActionButtonConfig> GetItemButtons()
        {
            List<ActionButtonConfig> list = new List<ActionButtonConfig>();

            foreach (ActionButtonConfig button in this)
                if (button.ButtonType == ActionButtonType.Item)
                    list.Add(button);

            return list;
        }

        public List<ActionButtonConfig> GetSpellButtonsByName(string Spellname)
        {
            List<ActionButtonConfig> list = new List<ActionButtonConfig>();

            foreach (ActionButtonConfig button in GetSpellButtons())          
                if (String.Equals(button.Name, Spellname))
                    list.Add(button);

            return list;
        }

        public List<ActionButtonConfig> GetItemButtonsByName(string Itemname)
        {
            List<ActionButtonConfig> list = new List<ActionButtonConfig>();

            foreach (ActionButtonConfig button in GetItemButtons())
                if (String.Equals(button.Name, Itemname))
                    list.Add(button);

            return list;
        }

        /// <summary>
        /// Returns a new instance of a default button set
        /// </summary>
        public static ActionButtonList DEFAULT
        {
            get 
            {
                ActionButtonList buttons = new ActionButtonList();
                buttons.PlayerName = "Default";

                buttons.Add(new ActionButtonConfig(0, ActionButtonType.Action, AvatarAction.Activate.ToString().ToLower()));
                buttons.Add(new ActionButtonConfig(1, ActionButtonType.Action, AvatarAction.Attack.ToString().ToLower()));
                buttons.Add(new ActionButtonConfig(2, ActionButtonType.Action, AvatarAction.Rest.ToString().ToLower()));
                buttons.Add(new ActionButtonConfig(3, ActionButtonType.Spell, Common.Constants.ResourceStrings.Spells.Faren.MEDITATE));
                buttons.Add(new ActionButtonConfig(4, ActionButtonType.Spell, Common.Constants.ResourceStrings.Spells.Kraanan.CONVEYANCE));
                buttons.Add(new ActionButtonConfig(5, ActionButtonType.Action, AvatarAction.Loot.ToString().ToLower()));
                buttons.Add(new ActionButtonConfig(6, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(7, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(8, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(9, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(10, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(11, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(12, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(13, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(14, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(15, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(16, ActionButtonType.Action, AvatarAction.Buy.ToString().ToLower()));
                buttons.Add(new ActionButtonConfig(17, ActionButtonType.Action, AvatarAction.Trade.ToString().ToLower()));
                buttons.Add(new ActionButtonConfig(18, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(19, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(20, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(21, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(22, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(23, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(24, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(25, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(26, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(27, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(28, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(29, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(30, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(31, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(32, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(33, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(34, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(35, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(36, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(37, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(38, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(39, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(40, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(41, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(42, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(43, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(44, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(45, ActionButtonType.Spell, Common.Constants.ResourceStrings.Spells.Riija.BLINK));
                buttons.Add(new ActionButtonConfig(46, ActionButtonType.Spell, Common.Constants.ResourceStrings.Spells.Riija.PHASE));
                buttons.Add(new ActionButtonConfig(47, ActionButtonType.Action, AvatarAction.Inspect.ToString().ToLower()));

                buttons.Add(new ActionButtonConfig(48, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(49, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(50, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(51, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(52, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(53, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(54, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(55, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(56, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(57, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(58, ActionButtonType.Unset, String.Empty));
                buttons.Add(new ActionButtonConfig(59, ActionButtonType.Unset, String.Empty));

                return buttons;
            }
        }
    }
}
