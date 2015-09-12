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
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Lists.SpellObjectList
    /// </summary>
    public partial class SpellsView : UserControl
    {
        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public SpellObjectList DataSource
        {
            get
            {
                if (gridSpells.DataSource != null)
                    return (SpellObjectList)gridSpells.DataSource;
                else
                    return null;
            }
            set
            {
                gridSpells.DataSource = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpellsView()
        {
            InitializeComponent();
        }

        protected void OnGridSpellsSelectionChanged(object sender, EventArgs e)
        {
            if (gridSpells.SelectedRows.Count > 0 && 
                gridSpells.SelectedRows[0].DataBoundItem != null)
            {
                SpellObject spellObject = (SpellObject)gridSpells.SelectedRows[0].DataBoundItem;

                spellObject.SubOverlays.SyncContext = SynchronizationContext.Current;

                gridSubOverlays.DataSource = spellObject.SubOverlays;               
                avAnimation.DataSource = spellObject.Animation;              
            }
        }
    }
}
