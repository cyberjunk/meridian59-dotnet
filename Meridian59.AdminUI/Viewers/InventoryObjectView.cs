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
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Drawing2D;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI
{
    /// <summary>
    /// View for Data.Models.InventoryObjectList
    /// </summary>
    public partial class InventoryObjectView : UserControl
    {
        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public InventoryObjectList DataSource
        {
            get
            {
                if (gridObjects.DataSource != null)
                    return (InventoryObjectList)gridObjects.DataSource;
                else
                    return null;
            }
            set
            {
                gridObjects.DataSource = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get { return groupObjects.Text; }
            set
            {
                groupObjects.Text = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryObjectView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Triggered when another inventory object is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnGridObjectsSelectionChanged(object sender, EventArgs e)
        {
            if (gridObjects.SelectedRows.Count > 0 && 
                gridObjects.SelectedRows[0].DataBoundItem != null)
            {
                InventoryObject objectBase = (InventoryObject)gridObjects.SelectedRows[0].DataBoundItem;
                objectBase.SubOverlays.SyncContext = SynchronizationContext.Current;

                // update imagecomposer
                pictureBox.DataSource = objectBase;
                
                gridSubOverlays.DataSource = objectBase.SubOverlays;
                avAnimation.DataSource = objectBase.Animation;
            }  
        }

        /// <summary>
        /// Triggered when another subovleray is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnGridSubOverlaysSelectionChanged(object sender, EventArgs e)
        {
            SubOverlay selectedItem = gridSubOverlays.SelectedItem;

            avSubOverlayAnimation.DataSource = 
                (selectedItem != null) ? selectedItem.Animation : null;
        }
    }
}
