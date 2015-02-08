using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Models;
using Meridian59.Common.Enums;

namespace Meridian59.DebugUI
{
    public partial class AnimationView : UserControl
    {
        private Animation dataSource;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public Animation DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;

                // cleanup old databindings
                lblType.DataBindings.Clear();
                lblValue1.DataBindings.Clear();
                lblValue2.DataBindings.Clear();
                lblValue3.DataBindings.Clear();
                lblValue4.DataBindings.Clear();

                if (dataSource != null)
                {
                    // setup new bindings
                    lblType.DataBindings.Add("Text", DataSource, Animation.PROPNAME_ANIMATIONTYPE);

                    switch (value.AnimationType)
                    {
                        case AnimationType.NONE:
                            lblValue1.DataBindings.Add("Text", dataSource, AnimationNone.PROPNAME_GROUP);
                            lblValue1Desc.Text = AnimationNone.PROPNAME_GROUP;
                            lblTypeDesc.Visible = true;
                            lblType.Visible = true;
                            lblValue1Desc.Visible = true;
                            lblValue1.Visible = true;
                            lblValue2Desc.Visible = false;
                            lblValue2.Visible = false;
                            lblValue3Desc.Visible = false;
                            lblValue3.Visible = false;
                            lblValue4Desc.Visible = false;
                            lblValue4.Visible = false;
                            break;

                        case AnimationType.CYCLE:
                            lblValue1.DataBindings.Add("Text", dataSource, AnimationCycle.PROPNAME_PERIOD);
                            lblValue1Desc.Text = AnimationCycle.PROPNAME_PERIOD;
                            lblValue2.DataBindings.Add("Text", dataSource, AnimationCycle.PROPNAME_GROUPLOW);
                            lblValue2Desc.Text = AnimationCycle.PROPNAME_GROUPLOW;
                            lblValue3.DataBindings.Add("Text", dataSource, AnimationCycle.PROPNAME_GROUPHIGH);
                            lblValue3Desc.Text = AnimationCycle.PROPNAME_GROUPHIGH;
                            lblTypeDesc.Visible = true;
                            lblType.Visible = true;
                            lblValue1Desc.Visible = true;
                            lblValue1.Visible = true;
                            lblValue2Desc.Visible = true;
                            lblValue2.Visible = true;
                            lblValue3Desc.Visible = true;
                            lblValue3.Visible = true;
                            lblValue4Desc.Visible = false;
                            lblValue4.Visible = false;
                            break;

                        case AnimationType.ONCE:
                            lblValue1.DataBindings.Add("Text", dataSource, AnimationOnce.PROPNAME_PERIOD);
                            lblValue1Desc.Text = AnimationOnce.PROPNAME_PERIOD;
                            lblValue2.DataBindings.Add("Text", dataSource, AnimationOnce.PROPNAME_GROUPLOW);
                            lblValue2Desc.Text = AnimationOnce.PROPNAME_GROUPLOW;
                            lblValue3.DataBindings.Add("Text", dataSource, AnimationOnce.PROPNAME_GROUPHIGH);
                            lblValue3Desc.Text = AnimationOnce.PROPNAME_GROUPHIGH;
                            lblValue4.DataBindings.Add("Text", dataSource, AnimationOnce.PROPNAME_GROUPFINAL);
                            lblValue4Desc.Text = AnimationOnce.PROPNAME_GROUPFINAL;
                            lblTypeDesc.Visible = true;
                            lblType.Visible = true;
                            lblValue1Desc.Visible = true;
                            lblValue1.Visible = true;
                            lblValue2Desc.Visible = true;
                            lblValue2.Visible = true;
                            lblValue3Desc.Visible = true;
                            lblValue3.Visible = true;
                            lblValue4Desc.Visible = true;
                            lblValue4.Visible = true;
                            break;
                    }
                }
                else
                {
                    lblTypeDesc.Visible = false;
                    lblType.Visible = false;
                    lblValue1Desc.Visible = false;
                    lblValue1.Visible = false;
                    lblValue2Desc.Visible = false;
                    lblValue2.Visible = false;
                    lblValue3Desc.Visible = false;
                    lblValue3.Visible = false;
                    lblValue4Desc.Visible = false;
                    lblValue4.Visible = false;
                }
            }
        }
        
        public AnimationView()
        {
            InitializeComponent();
        }
    }
}
