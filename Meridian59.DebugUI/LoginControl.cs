/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.DebugUI".

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
using System.Drawing;

namespace Meridian59.DebugUI
{
    public class LoginControl : GroupBox
    {
        public event EventHandler ConnectRequest;
        public event EventHandler DisconnectRequest;

        public string Hostname
        {
            get { return txtHostname.Text; }
            set { txtHostname.Text = value; }
        }
        public ushort Hostport
        {
            get { return Convert.ToUInt16(txtHostport.Value); }
            set { txtHostport.Value = value; }
        }
        public string Username
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }
        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        private TextBox txtHostname = new TextBox();
        private NumericUpDown txtHostport = new NumericUpDown();
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private TableLayoutPanel tblMain = new TableLayoutPanel();
        private Label lblHostname = new Label();
        private Label lblHostport = new Label();
        private Label lblUsername = new Label();
        private Label lblPassword = new Label();
        private Button btnConnectDisconnect = new Button();

        private bool connectedState;
        public bool ConnectedState {
            get { return connectedState; }
            set
            {
                connectedState = value;

                if (value)
                {
                    btnConnectDisconnect.Text = "Disconnect";
                    txtHostname.Enabled = txtHostport.Enabled = txtUsername.Enabled = txtPassword.Enabled = false;
                    if (ConnectRequest != null) ConnectRequest(this, new EventArgs());
                }
                else
                {
                    btnConnectDisconnect.Text = "Connect";
                    txtHostname.Enabled = txtHostport.Enabled = txtUsername.Enabled = txtPassword.Enabled = true;
                    if (DisconnectRequest != null) DisconnectRequest(this, new EventArgs());
                }
            }
        }
        public LoginControl()
        {
            lblHostname.Text = "Hostname";
            lblHostport.Text = "Port";
            lblUsername.Text = "Username";
            lblPassword.Text = "Password";
            btnConnectDisconnect.Text = "Connect";

            txtHostname.Text = "meridian103.meridian59.com";
            txtHostport.Maximum = UInt16.MaxValue;
            txtHostport.Value = 5903;
           
            txtPassword.PasswordChar = Convert.ToChar("*");

            lblHostname.Width = lblUsername.Width = lblPassword.Width = lblHostport.Width = 60;                
            lblHostname.AutoSize = lblUsername.AutoSize = lblPassword.AutoSize = lblHostport.AutoSize = false;
            lblHostname.TextAlign = lblUsername.TextAlign = lblPassword.TextAlign = lblHostport.TextAlign = ContentAlignment.MiddleLeft;
            txtHostname.Width = txtUsername.Width = txtPassword.Width = txtHostport.Width = btnConnectDisconnect.Width = 140;          
            txtHostname.Height = txtUsername.Height = txtPassword.Height = txtHostport.Height = btnConnectDisconnect.Height = 25;
            
            tblMain.RowCount = 4;
            tblMain.ColumnCount = 2;

            tblMain.Controls.Add(lblUsername, 0, 0);
            tblMain.Controls.Add(txtUsername, 1, 0);
            tblMain.Controls.Add(lblPassword, 0, 1);
            tblMain.Controls.Add(txtPassword, 1, 1);
            tblMain.Controls.Add(lblHostname, 0, 2);
            tblMain.Controls.Add(txtHostname, 1, 2);
            tblMain.Controls.Add(lblHostport, 0, 3);
            tblMain.Controls.Add(txtHostport, 1, 3);
            tblMain.Controls.Add(btnConnectDisconnect, 1, 4);
            
            tblMain.Dock = DockStyle.Fill;
            tblMain.Padding = new Padding(2);

            RowStyle defaultRowStyle;
            for (int i = 0; i < tblMain.RowCount; i++)
            {
                defaultRowStyle = new RowStyle(SizeType.Absolute, 25);
                tblMain.RowStyles.Add(defaultRowStyle);
            }

            btnConnectDisconnect.Click += new EventHandler(btnConnectDisconnect_Click);
            
            this.Controls.Add(tblMain);      
        }
     
        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            switch (btnConnectDisconnect.Text)
            {
                case "Connect":
                    ConnectedState = true;
                    break;

                case "Disconnect":
                    ConnectedState = false;
                    break;
            }                      
        }
    }
}
