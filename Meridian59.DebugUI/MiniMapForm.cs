using Meridian59.Drawing2D;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Meridian59.DebugUI
{
    public partial class MiniMapForm : Form
    {
        protected MiniMapGDI map;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public MiniMapGDI Map
        {
            get { return map; }
            set
            {
                map = value;

                if (value != null)
                {
                    map.ImageChanged += onImageChanged;
                }
            }
        }

        private void onImageChanged(object sender, EventArgs e)
        {
            picBox.Image = map.Image;
        }

        public MiniMapForm()
        {
            InitializeComponent();
        }

        private void MiniMapForm_Load(object sender, EventArgs e)
        {
            if (map != null)
            {
                map.SetDimension(this.Width, this.Height);              
            }
        }

        private void MiniMapForm_ResizeEnd(object sender, EventArgs e)
        {
            if (map != null)
            {
                map.SetDimension(this.Width, this.Height); 
            }
        }

        private void btnSetZoom_Click(object sender, EventArgs e)
        {
            Map.Zoom = Convert.ToSingle(txtZoom.Text);
        }
    }
}
