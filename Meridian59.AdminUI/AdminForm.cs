/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.AdminUI".

 "Meridian59.DebugUI" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59.DebugUI" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59.DebugUI".
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
                        // old              
                        gamePacketViewer.DataSource = data.GameMessageLog;
                        guildMemberListViewer.DataSource = data.GuildInfo;
                        guildShieldsViewer.DataSource = data.GuildShieldInfo.Shields;
                        guildListViewer.DataSource = data.DiplomacyInfo;

                        // refactored controls
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

                    if (resourceManager != null)
                        stringListViewer.DataSource = resourceManager.StringResources;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminForm()
        {
            InitializeComponent();

            guildMemberListViewer.PacketSend += new GameMessageEventHandler(OnGamePacketViewerPacketSend);
            guildListViewer.PacketSend += new GameMessageEventHandler(OnGamePacketViewerPacketSend);
        }

        protected void OnGamePacketViewerPacketSend(object sender, GameMessageEventArgs e)
        {
            if (PacketSend != null) 
                PacketSend(this, e);
        }

        protected void gamePacketViewer_PacketLogChanged(object sender, PacketLogChangeEventArgs e)
        {
            if (PacketLogChanged != null) 
                PacketLogChanged(this, e);
        }

        protected void btnRequestSkills_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendStatsMessage(StatGroup.Skills)));          
        }

        protected void btnRequestSpells_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendStatsMessage(StatGroup.Spells)));  
        }

        protected void btnRequestCondition_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendStatsMessage(StatGroup.Condition)));
        }

        protected void btnRequestAttributes_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendStatsMessage(StatGroup.Attributes)));
        }

        protected void btnRequestPlayerBuffs_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendEnchantmentsMessage(BuffType.AvatarBuff)));
        }

        protected void btnRequestRoomBuffs_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendEnchantmentsMessage(BuffType.RoomBuff)));
        }

        protected void btnRequestSpellObjects_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new SendSpellsMessage()));
        }

        protected void btnRequestInventory_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new ReqInventoryMessage()));
        }

        protected void btnRequestGuildShields_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildShieldListReq(), null)));
        }

        protected void btnLeaveGuild_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildRenounce(), null)));
        }

        protected void btnDisbandGuild_Click(object sender, EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildDisband(), null)));
        }
    }
}
