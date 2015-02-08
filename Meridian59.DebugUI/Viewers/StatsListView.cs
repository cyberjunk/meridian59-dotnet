using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;

namespace Meridian59.DebugUI.Viewers
{
    public partial class StatsListView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public SkillList DataSource
        {
            get
            {
                if (gridStats.DataSource != null)
                    return (SkillList)gridStats.DataSource;
                else
                    return null;
            }
            set
            {
                gridStats.DataSource = value;
            }
        }

        public StatsListView()
        {
            InitializeComponent();
        }
    }
}
