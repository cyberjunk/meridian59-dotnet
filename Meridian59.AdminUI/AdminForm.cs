/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.AdminUI".

 "Meridian59.AdminUI" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59.AdminUI" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59.AdminUI".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Windows.Forms;
using Meridian59.Data;
using Meridian59.AdminUI.Events;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.GameMessages;
using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using Meridian59.Files;

namespace Meridian59.AdminUI
{
    /// <summary>
    /// The Admin UI window
    /// </summary>
    public partial class AdminForm : Form
    {
        protected DataController data;
        protected ResourceManager resourceManager;

        public event GameMessageEventHandler PacketSend;
        public event PacketLogChangeEventHandler PacketLogChanged;

        /// <summary>
        /// A DataController instance providing all models for the views.
        /// </summary>
        public DataController DataController
        {
            get { return data; }
            set
            {
                if (data != value)
                { 
                    data = value;

                    if (data != null)
                    {
                        adminInfoView.DataSource = data.AdminInfo;
                        diplomacyInfoViewer.DataSource = data.DiplomacyInfo;
                        guildInfoViewer.DataSource = data.GuildInfo;
                        gameMessageViewer.DataSource = data.GameMessageLog;
                        statsConditionView.DataSource = data.AvatarCondition;
                        statsAttributesView.DataSource = data.AvatarAttributes;
                        statsSkillsView.DataSource = data.AvatarSkills;
                        statsSpellsView.DataSource = data.AvatarSpells;
                        roomObjectsViewer.DataSource = data.RoomObjects;
                        inventoryObjectView.DataSource = data.InventoryObjects;
                        spellsView.DataSource = data.SpellObjects;
                        avatarBuffsView.DataSource = data.AvatarBuffs;
                        roomBuffsView.DataSource = data.RoomBuffs;
                        onlinePlayersView.DataSource = data.OnlinePlayers;
                        backgroundOverlayView.DataSource = data.BackgroundOverlays;
                        roomInfoView.DataSource = data.RoomInformation;
                        lightShadingView.DataSource = data.LightShading;
                        backgroundMusicView.DataSource = data.BackgroundMusic;
                        chatViewer.DataSource = data.ChatMessages;
                    }
                }
            }
        }

        /// <summary>
        /// The 'ResourceManager' instance providing access
        /// to resource files such as BGF.
        /// </summary>
        public ResourceManager ResourceManager
        {
            get { return resourceManager; }
            set
            {
                if (resourceManager != value)
                {
                    resourceManager = value;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminForm()
        {
            InitializeComponent();
        }

        protected void OnSubControlPacketSend(object sender, GameMessageEventArgs e)
        {
            if (PacketSend != null) 
                PacketSend(this, e);
        }

        protected void OnGameMessageViewerPacketLogChanged(object sender, PacketLogChangeEventArgs e)
        {
            if (PacketLogChanged != null) 
                PacketLogChanged(this, e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Dispose();
        }
    }
}
