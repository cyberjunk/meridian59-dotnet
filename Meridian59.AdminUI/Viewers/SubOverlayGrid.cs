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
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI
{
    /// <summary>
    /// View for Data.Lists.BaseList T=SubOverlay
    /// </summary>
    public partial class SubOverlayGrid : UserControl
    {
        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public BaseList<SubOverlay> DataSource
        {
            get
            {
                if (gridSubOverlays.DataSource != null)
                    return (BaseList<SubOverlay>)gridSubOverlays.DataSource;
                else
                    return null;
            }
            set 
            {
                gridSubOverlays.DataSource = value;
            }
        }

        /// <summary>
        /// Event will be raised when selection in the grid changes
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Currently selected item in the grid
        /// </summary>
        public SubOverlay SelectedItem
        {
            get
            {
                if (gridSubOverlays.SelectedRows.Count > 0)                
                    if (gridSubOverlays.SelectedRows[0].DataBoundItem != null)                   
                        return (SubOverlay)gridSubOverlays.SelectedRows[0].DataBoundItem;
                                  
                return null;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SubOverlayGrid()
        {
            InitializeComponent();
        }

        protected void OnGridSubOverlaysSelectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(sender, e);
        }
    }
}
