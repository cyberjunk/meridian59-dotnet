using System;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;

namespace Meridian59.AdminUI.Viewers
{
    public partial class BackgroundMusicView : UserControl
    {
        private PlayMusic dataSource;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public PlayMusic DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;

                // cleanup old databindings
                lblRID.DataBindings.Clear();
                lblFile.DataBindings.Clear();
                
                if (dataSource != null)
                {
                    // setup new bindings
                    lblRID.DataBindings.Add("Text", DataSource, PlayMusic.PROPNAME_RESOURCEID);
                    lblFile.DataBindings.Add("Text", DataSource, PlayMusic.PROPNAME_RESOURCENAME);                    
                }
                else
                {
                    lblRID.Text = String.Empty;
                    lblFile.Text = String.Empty;                   
                }
            }
        }

        public BackgroundMusicView()
        {
            InitializeComponent();
        }
    }
}
