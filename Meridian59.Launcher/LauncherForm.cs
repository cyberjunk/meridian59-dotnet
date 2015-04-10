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
using System.Drawing;
using System.Windows.Forms;
using Meridian59.Launcher.Models;
using Meridian59.Data.Models;

namespace Meridian59.Launcher
{
    public partial class LauncherForm : Form
    {
        public event EventHandler Exit;
        public event EventHandler ConnectRequest;

        protected bool dragging;
        protected Point pointClicked;
        protected Options options;

        public Options Options
        {
            get { return options; }
            set
            {
                options = value;

                cbServer.DataSource = options.Connections;
                cbServer.DisplayMember = ConnectionInfo.PROPNAME_NAME;
                cbServer.SelectedIndex = options.SelectedConnectionIndex;
            }
        }

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.DataSource = Options;
            optionsForm.ShowDialog();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void LauncherForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {
                dragging = false;
            }
        }

        private void LauncherForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));
                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);

                this.Location = pointMoveTo;
            } 
        }

        private void LauncherForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

            if (Exit != null)
                Exit(this, new EventArgs());
        }

        private void cbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbServer.SelectedIndex;

            if (index > -1)
            {
                ConnectionInfo info = Options.Connections[index];

                txtUsername.DataBindings.Clear();
                txtUsername.DataBindings.Add("Text", info, ConnectionInfo.PROPNAME_USERNAME);

                txtPassword.DataBindings.Clear();
                txtPassword.DataBindings.Add("Text", info, ConnectionInfo.PROPNAME_PASSWORD);

                // save lastselectedindex
                options.SelectedConnectionIndex = index;
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {            
            if (e.KeyCode == Keys.Return)
                Connect();          
        }

        private void Connect()
        {
            if (ConnectRequest != null)
            {
                SwitchEnabled();
                ConnectRequest(this, new EventArgs());
            }
        }

        public void SwitchEnabled()
        {
            // flip
            bool enabled = !btnConnect.Enabled;

            btnConnect.Enabled = enabled;
            btnOptions.Enabled = enabled;
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            cbServer.Enabled = enabled;
        }
    }
}
