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
using Meridian59.Protocol.Events;
using Meridian59.Protocol.GameMessages;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Models.AdminInfo
    /// </summary>
    public partial class AdminInfoView : UserControl
    {
        /// <summary>
        /// Will be raised when the object wants to send a GameMessage instance
        /// </summary>
        public event GameMessageEventHandler MessageSend;

        protected AdminInfo dataSource;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public AdminInfo DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    if (dataSource != null)
                    {
                        dataSource.ServerResponses.ListChanged -= OnServerResponsesListChanged;
                        dataSource.TrackedObjects.ListChanged -= OnTrackedObjectsListChanged;
                    }

                    dataSource = value;

                    if (dataSource != null)
                    {
                        dataSource.ServerResponses.ListChanged += OnServerResponsesListChanged;
                        dataSource.TrackedObjects.ListChanged += OnTrackedObjectsListChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminInfoView()
        {
            InitializeComponent();

            // add default commands
            txtCommand.AutoCompleteCustomSource.AddRange(AdminInfo.DEFAULTCOMMANDS);
        }

        protected void OnCommandKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        protected void OnCommandKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                string s = txtCommand.Text.TrimEnd().TrimStart();

                ReqAdminMessage msg = new ReqAdminMessage(s);

                if (MessageSend != null)
                    MessageSend(this, new GameMessageEventArgs(msg));

                // track it in autocompletion
                txtCommand.AutoCompleteCustomSource.Add(s);

                // prepare for next command
                txtCommand.Clear();
            }
        }

        protected void OnCommandKeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        protected void OnServerResponsesListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    txtConsole.AppendText(dataSource.ServerResponses[e.NewIndex] + Environment.NewLine);
                    txtConsole.ScrollToCaret();
                    break;

                case ListChangedType.Reset:
                    txtConsole.Clear();
                    break;
            }
        }

        protected void OnTrackedObjectsListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    AdminInfoObject obj = dataSource.TrackedObjects[e.NewIndex];
                    
                    // create view usercontrol
                    AdminInfoObjectView view = new AdminInfoObjectView();
                    view.Dock = DockStyle.Fill;
                    view.DataSource = dataSource.TrackedObjects[e.NewIndex];
                    view.Close += OnAdminInfoObjectViewClose;

                    // create tabpage and add view
                    TabPage tabPage = new TabPage();
                    tabPage.Text = "(" + obj.ID.ToString() + ") " + obj.ClassName;
                    tabPage.Controls.Add(view);
                    
                    // add tabpage an set active
                    tabObjects.TabPages.Add(tabPage);
                    tabObjects.SelectedTab = tabPage;
                    break;

                case ListChangedType.ItemDeleted:
                    break;

                case ListChangedType.Reset:
                    tabObjects.TabPages.Clear();
                    break;
            }
        }

        protected void OnAdminInfoObjectViewClose(object sender, EventArgs e)
        {
            foreach(TabPage tabPage in tabObjects.TabPages)
                if (tabPage.Controls.Contains((AdminInfoObjectView)sender))               
                    tabObjects.TabPages.Remove(tabPage);        
        }

        protected void OnClearLogClick(object sender, EventArgs e)
        {
            dataSource.ServerResponses.Clear();
        }
    }
}
