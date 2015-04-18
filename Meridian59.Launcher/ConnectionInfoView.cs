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

using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Launcher.Models;
using Meridian59.Data.Models;

namespace Meridian59.Launcher.Controls
{
    public partial class ConnectionInfoView : UserControl
    {
        private ConnectionInfo dataSource;
        private string[] stringDictionaries;

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public ConnectionInfo DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;

                // cleanup old databindings
                txtName.DataBindings.Clear();
                txtHost.DataBindings.Clear();
                numPort.DataBindings.Clear();
                chkUseIPv6.DataBindings.Clear();

                if (dataSource != null)
                {
                    txtName.DataBindings.Add("Text", DataSource, ConnectionInfo.PROPNAME_NAME);
                    txtHost.DataBindings.Add("Text", DataSource, ConnectionInfo.PROPNAME_HOST);
                    numPort.DataBindings.Add("Text", DataSource, ConnectionInfo.PROPNAME_PORT);
                    chkUseIPv6.DataBindings.Add("Checked", DataSource, ConnectionInfo.PROPNAME_USEIPV6);
                } 
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public string[] StringDictionaries
        {
            get { return stringDictionaries; }
            set
            {
                stringDictionaries = value;

                if (value != null)
                {
                    // set combobox entries from data
                    cbStringDictionary.DataSource = stringDictionaries;
                    cbStringDictionary.SelectedItem = dataSource.StringDictionary;
                }
            }
        }

        public ConnectionInfoView()
        {
            InitializeComponent();
        }

        private void cbStringDictionary_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            // update configmodel with selected dictionary
            dataSource.StringDictionary = (string)cbStringDictionary.SelectedItem;
        }
    }
}
