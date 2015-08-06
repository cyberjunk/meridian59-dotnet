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

using System.Windows.Forms;
using System.ComponentModel;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.DebugUI.ListViewers
{
    public class ServerChatMessageViewer : Panel
    {
        private DataGridView dgChatMessages = new DataGridView();

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public BaseList<ServerString> DataSource
        {
            get { return (BaseList<ServerString>)dgChatMessages.DataSource; }
            set {

                value.ListChanged += new ListChangedEventHandler(value_ListChanged);
                dgChatMessages.DataSource = value; }
        }

        private void value_ListChanged(object sender, ListChangedEventArgs e)
        {
         
        }

        private SplitContainer splitMain = new SplitContainer();
        
        //dgRoomObjects columns
        private DataGridViewColumn colResourceID = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colResourceName = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colFullString = new DataGridViewTextBoxColumn();
        
        // Strings used by dgRoomObjects
        private const string strResourceID = "ResourceID";
        private const string strResourceName = "ResourceName";
        private const string strFullString = "FullString";
        

        public ServerChatMessageViewer()
        {
            this.Dock = DockStyle.Fill;
            
            CreateGrid();
        }

        private void CreateGrid()
        {
            dgChatMessages.DataSource = new BaseList<ServerString>(5);

            // Set basic properties of the DataGridView
            dgChatMessages.Dock = DockStyle.Fill;
            dgChatMessages.ReadOnly = true;
            dgChatMessages.AllowUserToAddRows = false;
            dgChatMessages.AllowUserToDeleteRows = false;
            dgChatMessages.AllowUserToResizeRows = false;
            dgChatMessages.RowHeadersVisible = false;
            dgChatMessages.MultiSelect = false;
            dgChatMessages.AutoGenerateColumns = false;
            dgChatMessages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgChatMessages.ColumnHeadersVisible = true; 

            // Set many properties equal to constant strings
            colResourceID.DataPropertyName      = colResourceID.Name    = strResourceID;
            colResourceName.DataPropertyName    = colResourceName.Name  = strResourceName;
            colFullString.DataPropertyName      = colFullString.Name    = strFullString;
            
            colResourceID.HeaderText = "ResourceID";
            colResourceName.HeaderText = "Name";
            colFullString.HeaderText = "Message";
                      
            // enable sorting for columns
            colResourceID.SortMode = DataGridViewColumnSortMode.Automatic;
         
            // Column widths
            colResourceID.Width = 80;
            colResourceName.MinimumWidth = 200;
            colFullString.MinimumWidth = 200;
            

            // AutoSize modi
            colResourceID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colResourceName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colFullString.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            

            // Column styles
            // colDescription.DefaultCellStyle.BackColor = Color.Wheat;
            colResourceID.Visible = true;
            colResourceName.Visible = true;
            colFullString.Visible = true;
            

            // Add columns
            dgChatMessages.Columns.Add(colResourceID);
            dgChatMessages.Columns.Add(colResourceName);
            dgChatMessages.Columns.Add(colFullString);
            
            this.Controls.Add(dgChatMessages);
        }
    }
}
