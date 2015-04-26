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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Files.ROO;

namespace Meridian59.RooViewer.UI
{
    public partial class RooSubSectorsViewer : UserControl
    {
        public event EventHandler SelectedItemChanged;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooSubSector SelectedItem
        {
            get
            {
                if (dataGrid == null ||
                    dataGrid.DataSource == null ||
                    dataGrid.SelectedRows.Count == 0 ||
                    dataGrid.SelectedRows[0].DataBoundItem == null)
                {
                    return null;
                }
                else
                {
                    return (RooSubSector)dataGrid.SelectedRows[0].DataBoundItem;
                }
            }
            set
            {
                if (value == null)
                    dataGrid.ClearSelection();

                else
                { 
                    foreach (DataGridViewRow row in dataGrid.Rows)
                    {
                        if (row.DataBoundItem == value)
                        {
                            row.Selected = true;
                            dataGrid.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public List<RooSubSector> DataSource
        {
            get { return (dataGrid.DataSource != null) ? (List<RooSubSector>)dataGrid.DataSource : null; }
            set
            {
                if (dataGrid.DataSource != value)
                {
                    dataGrid.DataSource = value;
                    dataGrid.ClearSelection();
                }
            }
        }

        public RooSubSectorsViewer()
        {
            InitializeComponent();
            
            dataGrid.AutoGenerateColumns = false;
            dataGrid.SelectionChanged += OnDataGridSelectionChanged;
        }

        protected void OnDataGridSelectionChanged(object sender, EventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, new EventArgs());
        }
    }
}
