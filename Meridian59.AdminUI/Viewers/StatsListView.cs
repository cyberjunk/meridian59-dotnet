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

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Lists.SkillList
    /// </summary>
    public partial class StatsListView : UserControl
    {
        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public SkillList DataSource
        {
            get
            {
                if (gridStats.DataSource != null)
                    return (SkillList)gridStats.DataSource;
                else
                    return null;
            }
            set
            {
                gridStats.DataSource = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StatsListView()
        {
            InitializeComponent();
        }
    }
}
