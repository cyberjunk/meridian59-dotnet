using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;

namespace Meridian59.AdminUI.Viewers
{
    public partial class StatsNumericView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public StatNumericList DataSource
        {
            get
            {
                if (gridStats.DataSource != null)
                    return (StatNumericList)gridStats.DataSource;
                else
                    return null;
            }
            set
            {
                gridStats.DataSource = value;
            }
        }

        public StatsNumericView()
        {
            InitializeComponent();
        }
    }
}
