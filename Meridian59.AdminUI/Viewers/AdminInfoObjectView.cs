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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Models.AdminInfoObject
    /// </summary>
    public partial class AdminInfoObjectView : UserControl
    {
        protected AdminInfoObject dataSource;

        /// <summary>
        /// Raised when Close button is clicked
        /// </summary>
        public event EventHandler Close;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public AdminInfoObject DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    dataSource = value;

                    gridProperties.DataSource = dataSource.Properties;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminInfoObjectView()
        {
            InitializeComponent();
        }

        protected void OnCloseClick(object sender, EventArgs e)
        {
            if (Close != null)
                Close(this, new EventArgs());
        }
    }
}
