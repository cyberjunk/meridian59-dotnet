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

using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Models.RoomInfo
    /// </summary>
    public partial class RoomInfoView : UserControl
    {
        protected RoomInfo dataSource;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RoomInfo DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;

                // cleanup old databindings
                lblAvatarID.DataBindings.Clear();
                lblAvatarOverlayRID.DataBindings.Clear();
                lblAvatarNameRID.DataBindings.Clear();
                lblRoomID.DataBindings.Clear();
                lblRoomFileRID.DataBindings.Clear();
                lblRoomNameRID.DataBindings.Clear();
                lblRoomSecurity.DataBindings.Clear();
                lblAmbientLight.DataBindings.Clear();
                lblAvatarLight.DataBindings.Clear();
                lblBackgroundFileRID.DataBindings.Clear();
                lblWadingSoundFileRID.DataBindings.Clear();
                lblFlags.DataBindings.Clear();
                lblDepth1.DataBindings.Clear();
                lblDepth2.DataBindings.Clear();
                lblDepth3.DataBindings.Clear();

                lblAvatarOverlay.DataBindings.Clear();
                lblAvatarName.DataBindings.Clear();
                lblRoomFile.DataBindings.Clear();
                lblRoomName.DataBindings.Clear();
                lblBackgroundFile.DataBindings.Clear();
                lblWadingSoundFile.DataBindings.Clear();

                if (dataSource != null)
                {
                    lblAvatarID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AVATARID);
                    lblAvatarOverlayRID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AVATAROVERLAYRID);
                    lblAvatarNameRID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AVATARNAMERID);
                    lblRoomID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_ROOMID);
                    lblRoomFileRID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_ROOMFILERID);
                    lblRoomNameRID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_ROOMNAMERID);
                    lblRoomSecurity.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_ROOMSECURITY);
                    lblAmbientLight.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AMBIENTLIGHT);
                    lblAvatarLight.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AVATARLIGHT);
                    lblBackgroundFileRID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_BACKGROUNDFILERID);
                    lblWadingSoundFileRID.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_WADINGSOUNDFILERID);
                    lblFlags.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_FLAGS);
                    lblDepth1.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_DEPTH1);
                    lblDepth2.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_DEPTH2);
                    lblDepth3.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_DEPTH3);

                    lblAvatarOverlay.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AVATAROVERLAY);
                    lblAvatarName.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_AVATARNAME);
                    lblRoomFile.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_ROOMFILE);
                    lblRoomName.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_ROOMNAME);
                    lblBackgroundFile.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_BACKGROUNDFILE);
                    lblWadingSoundFile.DataBindings.Add("Text", DataSource, RoomInfo.PROPNAME_WADINGSOUNDFILE);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RoomInfoView()
        {
            InitializeComponent();

            Disposed += OnDisposed;
        }

        protected void OnDisposed(object sender, System.EventArgs e)
        {
            // cleanup old databindings
            lblAvatarID.DataBindings.Clear();
            lblAvatarOverlayRID.DataBindings.Clear();
            lblAvatarNameRID.DataBindings.Clear();
            lblRoomID.DataBindings.Clear();
            lblRoomFileRID.DataBindings.Clear();
            lblRoomNameRID.DataBindings.Clear();
            lblRoomSecurity.DataBindings.Clear();
            lblAmbientLight.DataBindings.Clear();
            lblAvatarLight.DataBindings.Clear();
            lblBackgroundFileRID.DataBindings.Clear();
            lblWadingSoundFileRID.DataBindings.Clear();
            lblFlags.DataBindings.Clear();
            lblDepth1.DataBindings.Clear();
            lblDepth2.DataBindings.Clear();
            lblDepth3.DataBindings.Clear();

            lblAvatarOverlay.DataBindings.Clear();
            lblAvatarName.DataBindings.Clear();
            lblRoomFile.DataBindings.Clear();
            lblRoomName.DataBindings.Clear();
            lblBackgroundFile.DataBindings.Clear();
            lblWadingSoundFile.DataBindings.Clear();

        }
    }
}
