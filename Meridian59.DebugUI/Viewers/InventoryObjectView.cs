using System;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Drawing2D;
using Meridian59.Files;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.DebugUI
{
    public partial class InventoryObjectView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public InventoryObjectList DataSource
        {
            get
            {
                if (gridObjects.DataSource != null)
                    return (InventoryObjectList)gridObjects.DataSource;
                else
                    return null;
            }
            set
            {
                gridObjects.DataSource = value;
            }
        }

        public string Title
        {
            get { return groupObjects.Text; }
            set
            {
                groupObjects.Text = value;
            }
        }

        protected ImageComposerGDI<ObjectBase> gameBitmap;

        public InventoryObjectView()
        {
            InitializeComponent();

            // recreate gamebitmap
            gameBitmap = new ImageComposerGDI<ObjectBase>();
            gameBitmap.Width = (uint)picImage.Width;
            gameBitmap.Height = (uint)picImage.Height;

            // attach handler when gamebitmap changes (animation)
            gameBitmap.NewImageAvailable += gameBitmap_NewBitmap;
        }

        private void gridObjects_SelectionChanged(object sender, EventArgs e)
        {
            if (gridObjects.SelectedRows.Count > 0 && gridObjects.SelectedRows[0].DataBoundItem != null)
            {
                ObjectBase objectBase = (ObjectBase)gridObjects.SelectedRows[0].DataBoundItem;

                if (objectBase.Resource != null)
                {
                    gameBitmap.DataSource = objectBase;
                }

                objectBase.SubOverlays.SyncContext = SynchronizationContext.Current;
                
                avAnimation.DataSource = null;
                avSubOverlayAnimation.DataSource = null;
                
                gridSubOverlays.DataSource = objectBase.SubOverlays;
                avAnimation.DataSource = objectBase.Animation;
            }  
        }

        private void gameBitmap_NewBitmap(object sender, EventArgs e)
        {
            picImage.Image = gameBitmap.Image;
        }

        private void gridSubOverlays_SelectionChanged(object sender, EventArgs e)
        {
            SubOverlay selectedItem = gridSubOverlays.SelectedItem;

            if (selectedItem != null)
                avSubOverlayAnimation.DataSource = selectedItem.Animation;
        }
    }
}
