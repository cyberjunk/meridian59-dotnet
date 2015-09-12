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
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Events;
using Meridian59.AdminUI.Events;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Lists.BaseList T=GameMessage
    /// </summary>
    public partial class GameMessageView : UserControl
    {
        /// <summary>
        /// Will be raised when a custom message should be sent.
        /// </summary>
        public event GameMessageEventHandler PacketSend;

        /// <summary>
        /// Will be raised when the network logger filters should be updated.
        /// </summary>
        public event PacketLogChangeEventHandler PacketLogChanged;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public BaseList<GameMessage> DataSource
        {
            get
            {
                if (gridMessages.DataSource != null)
                    return (BaseList<GameMessage>)gridMessages.DataSource;
                else
                    return null;
            }
            set
            {
                if (value != gridMessages.DataSource)
                {
                    // detach old listener
                    if (DataSource != null)                   
                        DataSource.ListChanged -= OnGameMessageListChanged;
                  
                    gridMessages.DataSource = value;

                    // attach listener
                    if (value != null)
                        value.ListChanged += OnGameMessageListChanged;                   
                }               
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameMessageView()
        {
            InitializeComponent();
        }

        protected void OnGameMessageListChanged(object sender, ListChangedEventArgs e)
        {
            // autoscroll to newly added message
            if (chkAutoScroll.Checked && gridMessages.Rows.Count > 2)
                gridMessages.FirstDisplayedScrollingRowIndex = gridMessages.Rows.Count - 1;
        }

        protected void OnColumnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            bool ischecked = ((CheckBox)sender).Checked;

            if (sender == chkLength)
            {
                colLEN1.Visible = ischecked;
                colLEN2.Visible = ischecked;
            }
            else if (sender == chkCRC)
            {
                colCRC.Visible = ischecked;
            }
            else if (sender == chkServerSave)
            {
                colSS.Visible = ischecked;
            }
            else if (sender == chkPI)
            {
                colPI.Visible = ischecked;
            }
            else if (sender == chkData)
            {
                colData.Visible = ischecked;
            }
        }

        protected void OnMessageFilterCheckedChanged(object sender, EventArgs e)
        {
            if (PacketLogChanged != null)
                PacketLogChanged(this, new PacketLogChangeEventArgs(chkSend.Checked, chkReceive.Checked, chkPings.Checked));
        }

        protected void OnSendClick(object sender, EventArgs e)
        {
            // build a gamemessage from the provided messagetype and body
            byte[] body = txtMessageBody.GetBinaryValue();
            byte[] msgbytes = new byte[GameMessage.HEADERLENGTH + body.Length];
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(body.Length)), 0, msgbytes, 0, 2);
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(body.Length)), 0, msgbytes, 4, 2);
            Array.Copy(body, 0, msgbytes, GameMessage.HEADERLENGTH, body.Length);

            // create generic instance from raw bytes
            GenericGameMessage message = new GenericGameMessage(msgbytes);

            // send it by raising event
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(message));
        }

        protected void OnClearClick(object sender, EventArgs e)
        {
            DataSource.Clear();
        }
    }
}
