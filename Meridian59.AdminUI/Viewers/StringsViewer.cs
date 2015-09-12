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
using System.Windows.Forms;
using Meridian59.Common;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class StringsViewer : UserControl
    {
        protected readonly StringList unfilteredData = new StringList(50000);
        protected readonly StringList filteredData = new StringList(50000);

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public StringDictionary DataSource
        {
            //get { return dataSource; }
            set
            {
                // clear our lists
                unfilteredData.Clear();
                filteredData.Clear();

                // add all to unfiltered first
                foreach (KeyValuePair<uint, string> entry in value)
                {
                    ResourceID stringEntry = new ResourceID();
                    stringEntry.Value = entry.Key;
                    stringEntry.Name = entry.Value;
                    unfilteredData.Add(stringEntry);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StringsViewer()
        {
            InitializeComponent();

            // display uses filtered list
            gridStrings.DataSource = filteredData;

            // hookup listener on unfiltered data
            unfilteredData.ListChanged += OnUnfilteredDataListChanged;
        }

        protected void OnUnfilteredDataListChanged(object sender, ListChangedEventArgs e)
        {
            switch(e.ListChangedType)
            {
                // add: check if it matches filter, possibly add to filtered list
                case ListChangedType.ItemAdded:
                    if (IsTextFilterMatch(unfilteredData[e.NewIndex]))
                    {
                        filteredData.Add(unfilteredData[e.NewIndex]);
                    }
                    break;

                // remove: try remove from filtered also
                case ListChangedType.ItemDeleted:
                    filteredData.Remove(unfilteredData.LastDeletedItem);
                    break;

                // reset: clear and add all
                case ListChangedType.Reset:
                    filteredData.Clear();
                    foreach (ResourceID id in unfilteredData)
                        if (IsTextFilterMatch(id))
                            filteredData.Add(id);
                    break;
            }
        }

        protected void OnFilterTextKeyUp(object sender, KeyEventArgs e)
        {
            Filter();
        }

        protected void Filter()
        {
            filteredData.Clear();

            foreach (ResourceID id in unfilteredData)
                if (IsTextFilterMatch(id))
                    filteredData.Add(id);
        }

        protected bool IsTextFilterMatch(ResourceID ID)
        {
            return
                txtFilterText.Text == String.Empty ||
                (ID != null && ID.Name != null &&
                ID.Name.Contains(txtFilterText.Text));
        }
    }
}
