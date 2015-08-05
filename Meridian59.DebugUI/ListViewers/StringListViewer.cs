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
using Meridian59.Data.Lists;
using Meridian59.DebugUI.Generic;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Meridian59.Data.Models;
using System.ComponentModel;
using Meridian59.Common;

namespace Meridian59.DebugUI.ListViewers
{
    public class StringListViewer : GroupBox
    {
        private StringList unfilteredDataSource = new StringList();
        private StringList filteredDataSource = new StringList();

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public StringDictionary DataSource
        {
            //get { return dataSource; }
            set {

                filteredDataSource = new StringList();
                foreach(KeyValuePair<uint, string> entry in value)
                {
                    ResourceID stringEntry = new ResourceID();
                    stringEntry.Value = entry.Key;
                    stringEntry.Name = entry.Value;
                    filteredDataSource.Add(stringEntry);
                }

                unfilteredDataSource = filteredDataSource;
                gridStringList.DataSource = filteredDataSource;
                lblCount.Text = filteredDataSource.Count.ToString() + "/" + unfilteredDataSource.Count.ToString();            
                txtStringFilter.Text = String.Empty;
                txtIDFilterDec.Text = String.Empty;

                if (value.Count > 0)
                {
                    txtIDFilterDec.Minimum = filteredDataSource.GetMinimumValue();
                    txtIDFilterDec.Maximum = filteredDataSource.GetMaximumValue();
                }      

            }
        }

        

        public DataGridView gridStringList = new DataGridView();

        private SplitContainer splitContainer = new SplitContainer();

        private TableLayoutPanel tblPanel = new TableLayoutPanel();

        private Label lblStringFilterDesc = new Label();
        private TextBox txtStringFilter = new TextBox();
        private Label lblIDFilterDecDesc = new Label();
        private NumericUpDown txtIDFilterDec = new NumericUpDown();
        private Label lblIDFilterHexDesc = new Label();
        private HexTextBox txtIDFilterHex = new HexTextBox();       
        private Label lblCountDesc = new Label();
        private Label lblCount = new Label();
          
        // DataGrid columns
        private DataGridViewColumn colID = new DataGridViewTextBoxColumn();
        private DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();

        // ConstantStrings
        private const string strID = "Value";
        private const string strName = "Name";
        private const string strCountDesc = "Entries";
        private const string strIDFilterDecDesc = "Jump (ID)";
        private const string strIDFilterHexDesc = "Jump (hexID, l.e.)";
        private const string strStringFilterDesc = "Filter by String";

        // Constructor
        public StringListViewer()
        {
            this.Dock = DockStyle.Fill;
            splitContainer.Dock = DockStyle.Fill;
            txtIDFilterDec.Maximum = UInt32.MaxValue;
            //DataSource = new StringList();

            CreateGrid();
            CreateHeaderControls();
            
            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.SplitterDistance = 40;
            splitContainer.FixedPanel = FixedPanel.Panel1;

            splitContainer.Panel2.Controls.Add(gridStringList);
            
            //gridStringList.DataSource = DataSource;

            txtStringFilter.TextChanged += new EventHandler(txtStringFilter_TextChanged);
            txtIDFilterDec.ValueChanged += new EventHandler(txtIDFilterDec_ValueChanged);
            txtIDFilterHex.TextChanged += new EventHandler(txtIDFilterHex_TextChanged);

            gridStringList.UserAddedRow += new DataGridViewRowEventHandler(gridStringList_UserAddedRow);
         
            this.Controls.Add(splitContainer);
        }

        private void gridStringList_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            txtIDFilterDec.Minimum = filteredDataSource.GetMinimumValue();
            txtIDFilterDec.Maximum = filteredDataSource.GetMaximumValue();
        }

        private void txtIDFilterDec_ValueChanged(object sender, EventArgs e)
        {
            uint id = Convert.ToUInt32(txtIDFilterDec.Value);
            if ((id < filteredDataSource.GetMaximumValue()) && (id > filteredDataSource.GetMinimumValue()))
            {
                int index = filteredDataSource.GetIndexByValue(id);
                if (index > -1)
                    gridStringList.FirstDisplayedScrollingRowIndex = index;
            }
        }

        private void txtIDFilterHex_TextChanged(object sender, EventArgs e)
        {        
            try
            {
                uint id = 0;
                switch (txtIDFilterHex.Value.Length)
                {
                    case 1:
                        id = txtIDFilterHex.Value[0];
                        break;

                    case 2: case 3:
                        id = BitConverter.ToUInt16(txtIDFilterHex.Value, 0);
                        break;

                    case 4:
                        id = BitConverter.ToUInt32(txtIDFilterHex.Value, 0);
                        break;

                }

                if ((id < filteredDataSource.GetMaximumValue()) && (id > filteredDataSource.GetMinimumValue()))
                {
                    int index = filteredDataSource.GetIndexByValue(id);
                    if (index > -1)
                        gridStringList.FirstDisplayedScrollingRowIndex = filteredDataSource.GetIndexByValue(id);
                }
            }
            finally { }
                               
        }

        private void txtStringFilter_TextChanged(object sender, EventArgs e)
        {
            IEnumerable<ResourceID> filteredList = unfilteredDataSource.GetItemsBySubstring(txtStringFilter.Text);

            if (gridStringList.DataSource == null)           
                gridStringList.DataSource = new StringList();
                           
            StringList list = (StringList)gridStringList.DataSource;
            list.Clear();

            foreach (ResourceID id in filteredList)
                list.Add(id);
           
            txtIDFilterDec.Maximum = list.GetMaximumValue();
            txtIDFilterDec.Minimum = list.GetMinimumValue();

            lblCount.Text = list.Count.ToString() + "/" + unfilteredDataSource.Count.ToString();         
        }

        private void CreateHeaderControls()
        {
            lblCountDesc.Text = strCountDesc;
            lblIDFilterDecDesc.Text = strIDFilterDecDesc;
            lblIDFilterHexDesc.Text = strIDFilterHexDesc;
            lblStringFilterDesc.Text = strStringFilterDesc;

            txtIDFilterHex.MaxBytesLength = 4;

            CreateHeaderTable();
            splitContainer.Panel1.Controls.Add(tblPanel);
        }

        private void CreateHeaderTable()
        {
            tblPanel.Dock = DockStyle.Fill;
            tblPanel.RowCount = 2;
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 15));

            tblPanel.ColumnCount = 4;
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

            tblPanel.Controls.Add(lblIDFilterDecDesc, 0, 0);
            tblPanel.Controls.Add(txtIDFilterDec, 0, 1);

            tblPanel.Controls.Add(lblIDFilterHexDesc, 1, 0);
            tblPanel.Controls.Add(txtIDFilterHex, 1, 1);

            tblPanel.Controls.Add(lblStringFilterDesc, 2, 0);
            tblPanel.Controls.Add(txtStringFilter, 2, 1);

            tblPanel.Controls.Add(lblCountDesc, 3, 0);
            tblPanel.Controls.Add(lblCount, 3, 1);
        }

        private void CreateGrid()
        {
            // Set basic properties of the DataGridView
            gridStringList.Dock = DockStyle.Fill;
            gridStringList.ReadOnly = false;
            gridStringList.AllowUserToAddRows = true;
            gridStringList.AllowUserToDeleteRows = true;
            gridStringList.AllowUserToResizeRows = true;
            gridStringList.EditMode = DataGridViewEditMode.EditOnEnter;
            gridStringList.RowHeadersVisible = false;
            gridStringList.MultiSelect = false;
            gridStringList.AutoGenerateColumns = false;
            gridStringList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Set many properties equal to constant strings
            colID.DataPropertyName = colID.Name = colID.HeaderText = strID;
            colName.DataPropertyName = colName.Name = colName.HeaderText = strName;

            colName.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            gridStringList.ColumnHeadersVisible = true;
            
            // Column widths
            colID.Width = 100;
            colName.Width = 100;
            
            // AutoSize modi
            colID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            // Column styles
            colID.Visible = true;
            colName.Visible = true;
            
            // Add columns
            gridStringList.Columns.Add(colID);
            gridStringList.Columns.Add(colName);           

            // Bind events
            
        }
    }
}
