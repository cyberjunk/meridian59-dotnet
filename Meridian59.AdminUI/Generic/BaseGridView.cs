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
using System.Windows.Forms;
using System.ComponentModel;

namespace Meridian59.AdminUI.Generic
{
    /// <summary>
    /// This class just extends the basic .net 'DataGridView' with different
    /// defaultvalues for properties such as 'ReadOnly', 'Dock' and others.
    /// </summary>
    public class BaseGridView : DataGridView
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(DockStyle.Fill), Browsable(true)]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(DataGridViewSelectionMode.FullRowSelect), Browsable(true)]
        public new DataGridViewSelectionMode SelectionMode
        {
            get
            {
                return base.SelectionMode;
            }
            set
            {
                base.SelectionMode = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false), Browsable(true)]
        public new bool AutoGenerateColumns
        {
            get
            {
                return base.AutoGenerateColumns;
            }

            set
            {
                base.AutoGenerateColumns = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false), Browsable(true)]
        public new bool AllowUserToAddRows
        {
            get
            {
                return base.AllowUserToAddRows;
            }

            set
            {
                base.AllowUserToAddRows = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false), Browsable(true)]
        public new bool AllowUserToDeleteRows
        {
            get
            {
                return base.AllowUserToDeleteRows;
            }

            set
            {
                base.AllowUserToDeleteRows = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false), Browsable(true)]
        public new bool AllowUserToResizeRows
        {
            get
            {
                return base.AllowUserToResizeRows;
            }

            set
            {
                base.AllowUserToResizeRows = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false), Browsable(true)]
        public new bool RowHeadersVisible
        {
            get
            {
                return base.RowHeadersVisible;
            }

            set
            {
                base.RowHeadersVisible = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false), Browsable(true)]
        public new bool MultiSelect
        {
            get
            {
                return base.MultiSelect;
            }

            set
            {
                base.MultiSelect = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true), Browsable(true)]
        public new bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }

            set
            {
                base.ReadOnly = value;
            }
        }

        public BaseGridView()
            : base()
        {
            this.Dock = DockStyle.Fill;
            this.AutoGenerateColumns = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.RowHeadersVisible = false;
            this.MultiSelect = false;
            this.ReadOnly = true;
            this.AllowUserToResizeRows = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
        }
    }
}
