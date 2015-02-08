using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Meridian59.DebugUI
{
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

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
        }
    }
}
