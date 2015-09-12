/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.AdminUI".

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
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// View for Data.Models.PlayMusic
    /// </summary>
    public partial class BackgroundMusicView : UserControl
    {
        protected PlayMusic dataSource;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public PlayMusic DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    dataSource = value;

                    // cleanup old databindings
                    lblRID.DataBindings.Clear();
                    lblFile.DataBindings.Clear();

                    if (dataSource != null)
                    {
                        // setup new bindings
                        lblRID.DataBindings.Add("Text", DataSource, PlayMusic.PROPNAME_RESOURCEID);
                        lblFile.DataBindings.Add("Text", DataSource, PlayMusic.PROPNAME_RESOURCENAME);
                    }
                    else
                    {
                        lblRID.Text = String.Empty;
                        lblFile.Text = String.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundMusicView()
        {
            InitializeComponent();
        }
    }
}
