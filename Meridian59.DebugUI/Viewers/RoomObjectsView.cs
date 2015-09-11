using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;
using Meridian59.Files;
using Meridian59.Drawing2D;

namespace Meridian59.AdminUI
{
    public partial class RoomObjectsView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RoomObjectList DataSource
        {
            get
            {
                if (gridRoomObjects.DataSource != null)
                    return (RoomObjectList)gridRoomObjects.DataSource;
                else
                    return null;
            }
            set
            {
                gridRoomObjects.DataSource = value;
            }
        }

        ImageComposerGDI<RoomObject> imageComposer;

        public RoomObjectsView()
        {
            InitializeComponent();
            imageComposer = new ImageComposerGDI<RoomObject>();

            // attach handler when gamebitmap changes (animation)
            imageComposer.NewImageAvailable += gameBitmap_NewBitmap;                   
        }

        private void gridRoomObjects_SelectionChanged(object sender, EventArgs e)
        {
            if (gridRoomObjects.SelectedRows.Count > 0 && gridRoomObjects.SelectedRows[0].DataBoundItem != null)
            {
                RoomObject roomObject = (RoomObject)gridRoomObjects.SelectedRows[0].DataBoundItem;

                if (roomObject.Resource != null)
                {
                    // recreate gamebitmap
                    imageComposer.Width = (uint)picImage.Width;
                    imageComposer.Height = (uint)picImage.Height;

                    imageComposer.DataSource = roomObject;
                }

                roomObject.SubOverlays.SyncContext = SynchronizationContext.Current;
                roomObject.MotionSubOverlays.SyncContext = SynchronizationContext.Current;

                avAnimation.DataSource = null;
                avMotionAnimation.DataSource = null;
                avSubOverlayAnimation.DataSource = null;
                avMotionSubOverlayAnimation.DataSource = null;

                gridSubOverlays.DataSource = roomObject.SubOverlays;
                gridMotionSubOverlays.DataSource = roomObject.MotionSubOverlays;
                avAnimation.DataSource = roomObject.Animation;
                avMotionAnimation.DataSource = roomObject.MotionAnimation;
            }          
        }

        private void gridSubOverlays_SelectionChanged(object sender, EventArgs e)
        {
            SubOverlay selectedItem = gridSubOverlays.SelectedItem;
            
            if (selectedItem != null)
                avSubOverlayAnimation.DataSource = selectedItem.Animation;
        }

        private void gridMotionSubOverlays_SelectionChanged(object sender, EventArgs e)
        {
            SubOverlay selectedItem = gridMotionSubOverlays.SelectedItem;

            if (selectedItem != null)
                avMotionSubOverlayAnimation.DataSource = selectedItem.Animation;
        }

        private void gameBitmap_NewBitmap(object sender, EventArgs e)
        {
            picImage.Image = imageComposer.Image;
        }
    }
}
