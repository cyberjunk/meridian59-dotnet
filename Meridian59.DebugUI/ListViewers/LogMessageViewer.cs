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
using Meridian59.Data.Lists;
using Meridian59.Common;
using System.ComponentModel;

namespace Meridian59.AdminUI.ListViewers
{
    public class LogMessageViewer : Panel
    {
        private DataGridView dgLogMessages = new DataGridView();
        private GroupBox groupBox = new GroupBox();

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public BaseList<LogMessage> DataSource
        {
            get { return (BaseList<LogMessage>)dgLogMessages.DataSource; }
            set {
                dgLogMessages.DataSource = value; }
        }

        private SplitContainer splitMain = new SplitContainer();
        
        //dgRoomObjects columns
        private DataGridViewColumn colTime = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colModule = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colMessage = new DataGridViewTextBoxColumn();
        
        // Strings used by dgRoomObjects
        private const string strTime = "Time";
        private const string strModule = "Module";
        private const string strMessage = "Message";
        
        public LogMessageViewer()
        {
            this.Dock = DockStyle.Fill;
            CreateSplit();   
            CreateGrid();
        }

        private void CreateSplit()
        {
            groupBox.Dock = DockStyle.Fill;
            splitMain.Orientation = Orientation.Horizontal;
            splitMain.FixedPanel = FixedPanel.Panel1;
            splitMain.SplitterDistance = 30;
            splitMain.Dock = DockStyle.Fill;
            groupBox.Controls.Add(splitMain);
            this.Controls.Add(groupBox);
        }

        private void CreateGrid()
        {
            dgLogMessages.DataSource = new BaseList<LogMessage>();
            
            // Set basic properties of the DataGridView
            dgLogMessages.Dock = DockStyle.Fill;
            dgLogMessages.ReadOnly = true;
            dgLogMessages.AllowUserToAddRows = false;
            dgLogMessages.AllowUserToDeleteRows = false;
            dgLogMessages.AllowUserToResizeRows = true;
            dgLogMessages.RowHeadersVisible = false;
            dgLogMessages.MultiSelect = false;
            dgLogMessages.AutoGenerateColumns = false;
            dgLogMessages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgLogMessages.ColumnHeadersVisible = true; 

            // Set many properties equal to constant strings
            colTime.DataPropertyName    = colTime.Name  = colTime.HeaderText = strTime;
            colModule.DataPropertyName = colModule.Name = colModule.HeaderText = strModule;
            colMessage.DataPropertyName = colMessage.Name = colMessage.HeaderText = strMessage;
            
            //colModule.HeaderText = strModule;
            colMessage.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //((DataGridViewTextBoxColumn)colMessage).Row
            // enable sorting for columns
            //colTime.SortMode = DataGridViewColumnSortMode.Automatic;
         
            // Column widths
            colTime.Width = 100;
            colModule.Width = 100;
            colMessage.Width = 100;
            

            // AutoSize modi
            colTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colModule.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            

            // Column styles
            // colDescription.DefaultCellStyle.BackColor = Color.Wheat;
            colTime.Visible = true;
            colModule.Visible = true;
            colMessage.Visible = true;
            
            // Add columns
            dgLogMessages.Columns.Add(colTime);
            dgLogMessages.Columns.Add(colModule);
            dgLogMessages.Columns.Add(colMessage);

            splitMain.Panel2.Controls.Add(dgLogMessages);
            //this.Controls.Add(dgLogMessages);
        }
    }
}
