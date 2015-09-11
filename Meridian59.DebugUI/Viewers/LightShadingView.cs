using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    public partial class LightShadingView : UserControl
    {
        private LightShading dataSource;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public LightShading DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;

                // cleanup old databindings
                lblLightIntensity.DataBindings.Clear();
                lblAngle.DataBindings.Clear();
                lblHeight.DataBindings.Clear();
                
                if (dataSource != null)
                {
                    // setup new bindings
                    lblLightIntensity.DataBindings.Add("Text", DataSource, "LightIntensity");
                    lblAngle.DataBindings.Add("Text", DataSource.SpherePosition, "Angle");
                    lblHeight.DataBindings.Add("Text", DataSource.SpherePosition, "Height");
                }
                else
                {
                    lblLightIntensity.Text = String.Empty;
                    lblAngle.Text = String.Empty;
                    lblHeight.Text = String.Empty;
                }
            }
        }

        public LightShadingView()
        {
            InitializeComponent();
        }
    }
}
