using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Meridian59.Data.Models;
using Meridian59.Common.Enums;
using Meridian59.Drawing2D;

namespace Meridian59.BgfEditor.Forms
{
    public partial class Viewer : Form
    {
        protected RoomObject dataSource;

        /// <summary>
        /// The model to be shown in the View
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RoomObject DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                if (dataSource != value)
                {
                    if (dataSource != null)
                    {
                        dataSource.PropertyChanged -= OnDataSourcePropertyChanged;

                        if (dataSource.Resource != null)
                            dataSource.Resource.FrameSets.ListChanged -= OnResourceGroupsListChanged;
                    }

                    dataSource = value;
                    picAnimation.DataSource = value;

                    if (dataSource != null)
                    {
                        dataSource.PropertyChanged += OnDataSourcePropertyChanged;

                        if (dataSource.Resource != null)
                            dataSource.Resource.FrameSets.ListChanged += OnResourceGroupsListChanged;
                    }
                }
            }
        }

        public Viewer()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // use cycle by default
            cbType.SelectedIndex = 1;

            RefreshGroups();

            // set datasource on animation viewer
            //picAnimation.DataSource = Program.RoomObject;
            picAnimation.PropertyChanged += OnAnimationViewerPropertyChanged;

            // add binding for trackbar on zoom
            //trackZoom.DataBindings.Add(
            //    "Value", picAnimation, "Zoom", true, DataSourceUpdateMode.OnPropertyChanged);

            // add binding for trackbar on viewerangle
            trackAngle.DataBindings.Add(
                "Value", Program.RoomObject, RoomObject.PROPNAME_VIEWERANGLE, true, DataSourceUpdateMode.OnPropertyChanged);

            // add binding for color            
            cbPalette.DataBindings.Add(
                "SelectedIndex", Program.RoomObject, RoomObject.PROPNAME_COLORTRANSLATION, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void SetAnimation()
        {
            ushort group = (cbGroup.SelectedItem == null) ? (ushort)1 : (ushort)((int)cbGroup.SelectedItem);
            ushort low   = (cbLow.SelectedItem == null)   ? (ushort)1 : (ushort)((int)cbLow.SelectedItem);
            ushort high  = (cbHigh.SelectedItem == null)  ? (ushort)1 : (ushort)((int)cbHigh.SelectedItem);
            ushort final = (cbFinal.SelectedItem == null) ? (ushort)1 : (ushort)((int)cbFinal.SelectedItem);

            uint period  = Convert.ToUInt32(numInterval.Value);
            int groupmax = (dataSource.Resource != null) ? dataSource.Resource.FrameSets.Count : 1;

            Animation anim;

            switch (cbType.SelectedIndex)
            {
                // cycle
                case 1:
                    anim = new AnimationCycle(period, low, high);
                    anim.GroupMax = groupmax;
                    break;

                // once
                case 2:
                    anim = new AnimationOnce(period, low, high, final);
                    anim.GroupMax = groupmax;
                    break;

                // none (and others)
                default:
                    anim = new AnimationNone(group);
                    anim.GroupMax = groupmax;
                    break;
            }

            // set color
            dataSource.FirstAnimationType = AnimationType.TRANSLATION;
            dataSource.ColorTranslation = Convert.ToByte(cbPalette.SelectedIndex);

            // set anim
            dataSource.Animation = anim;
        }


        protected void RefreshGroups()
        {
            // clear high/low items
            cbGroup.Items.Clear();
            cbHigh.Items.Clear();
            cbLow.Items.Clear();
            cbFinal.Items.Clear();

            if (dataSource == null || dataSource.Resource == null)
                return;

            // add available group nums to comboboxes
            for (int i = 0; i < dataSource.Resource.FrameSets.Count; i++)
            {
                cbGroup.Items.Add(i + 1);
                cbHigh.Items.Add(i + 1);
                cbLow.Items.Add(i + 1);
                cbFinal.Items.Add(i + 1);
            }

            if (cbGroup.Items.Count > 0)
                cbGroup.SelectedIndex = 0;

            if (cbHigh.Items.Count > 0)
                cbHigh.SelectedIndex = 0;

            if (cbLow.Items.Count > 0)
                cbLow.SelectedIndex = 0;

            if (cbFinal.Items.Count > 0)
                cbFinal.SelectedIndex = 0;
        }

        protected void OnAnimationViewerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            trackZoom.Value = picAnimation.Zoom;
        }

        protected void OnResourceGroupsListChanged(object sender, ListChangedEventArgs e)
        {
            RefreshGroups();
        }

        protected void OnDataSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case RoomObject.PROPNAME_RESOURCE:
                    if (dataSource.Resource != null)
                        dataSource.Resource.FrameSets.ListChanged += OnResourceGroupsListChanged;
                    break;
            }
        }

        protected void OnTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    cbGroup.Enabled = true;
                    cbHigh.Enabled = false;
                    cbLow.Enabled = false;
                    cbFinal.Enabled = false;
                    numInterval.Enabled = false;
                    break;

                case 1:
                    cbGroup.Enabled = false;
                    cbHigh.Enabled = true;
                    cbLow.Enabled = true;
                    cbFinal.Enabled = false;
                    numInterval.Enabled = true;
                    break;

                case 2:
                    cbGroup.Enabled = false;
                    cbHigh.Enabled = true;
                    cbLow.Enabled = true;
                    cbFinal.Enabled = true;
                    numInterval.Enabled = true;
                    break;
            }

            SetAnimation();
        }

        protected void OnGroupSelectedIndexChanged(object sender, EventArgs e)
        {
            SetAnimation();
        }

        protected void OnIntervalChanged(object sender, EventArgs e)
        {
            SetAnimation();
        }

        protected void OnUseOffsetChanged(object sender, EventArgs e)
        {
            picAnimation.UseOffset = chkUseOffset.Checked;
            dataSource.MarkForAppearanceChange();
            picAnimation.RefreshImage();
        }

        protected void OnZoomValueChanged(object sender, EventArgs e)
        {
            picAnimation.Zoom = trackZoom.Value;
            picAnimation.Refresh();
        }
    }
}
