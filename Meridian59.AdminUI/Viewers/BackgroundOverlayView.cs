using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    public partial class BackgroundOverlayView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public BackgroundOverlayList DataSource
        {
            get
            {
                if (gridObjects.DataSource != null)
                    return (BackgroundOverlayList)gridObjects.DataSource;
                else
                    return null;
            }
            set
            {
                gridObjects.DataSource = value;
            }
        }

        public BackgroundOverlayView()
        {
            InitializeComponent();
        }

        private void gridObjects_SelectionChanged(object sender, EventArgs e)
        {
            if (gridObjects.SelectedRows.Count > 0 && gridObjects.SelectedRows[0].DataBoundItem != null)
            {
                BackgroundOverlay bgOverlay = (BackgroundOverlay)gridObjects.SelectedRows[0].DataBoundItem;

                avAnimation.DataSource = null;
                avAnimation.DataSource = bgOverlay.Animation;
            }
        }
    }
}
