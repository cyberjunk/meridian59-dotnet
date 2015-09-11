using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI
{
    public partial class SubOverlayGrid : UserControl
    {
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

        public event EventHandler SelectionChanged;

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

        public SubOverlayGrid()
        {
            InitializeComponent();
        }

        private void gridSubOverlays_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(sender, e);
        }
    }
}
