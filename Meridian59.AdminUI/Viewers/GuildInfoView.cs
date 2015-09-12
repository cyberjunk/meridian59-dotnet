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
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Models.GuildInfo
    /// </summary>
    public partial class GuildInfoView : UserControl
    {
        protected GuildInfo dataSource;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public GuildInfo DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    dataSource = value;

                    gridMembers.DataSource = dataSource.GuildMembers;

                    //

                    lblGuildName.DataBindings.Clear();
                    lblGuildName.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_GUILDNAME);

                    lblGuildID.DataBindings.Clear();
                    lblGuildID.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_GUILDID);

                    lblSupportedMember.DataBindings.Clear();
                    lblSupportedMember.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_SUPPORTEDMEMBER);

                    lblFlags.DataBindings.Clear();
                    lblFlags.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_FLAGS);

                    lblPassword.DataBindings.Clear();
                    lblPassword.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_CHESTPASSWORD);

                    //

                    lblMaleRank1.DataBindings.Clear();
                    lblMaleRank1.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK1MALE);

                    lblMaleRank2.DataBindings.Clear();
                    lblMaleRank2.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK2MALE);

                    lblMaleRank3.DataBindings.Clear();
                    lblMaleRank3.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK3MALE);

                    lblMaleRank4.DataBindings.Clear();
                    lblMaleRank4.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK4MALE);

                    lblMaleRank5.DataBindings.Clear();
                    lblMaleRank5.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK5MALE);

                    //

                    lblFemaleRank1.DataBindings.Clear();
                    lblFemaleRank1.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK1FEMALE);

                    lblFemaleRank2.DataBindings.Clear();
                    lblFemaleRank2.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK2FEMALE);

                    lblFemaleRank3.DataBindings.Clear();
                    lblFemaleRank3.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK3FEMALE);

                    lblFemaleRank4.DataBindings.Clear();
                    lblFemaleRank4.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK4FEMALE);

                    lblFemaleRank5.DataBindings.Clear();
                    lblFemaleRank5.DataBindings.Add("Text", dataSource, GuildInfo.PROPNAME_RANK5FEMALE);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GuildInfoView()
        {
            InitializeComponent();
        }
    }
}
