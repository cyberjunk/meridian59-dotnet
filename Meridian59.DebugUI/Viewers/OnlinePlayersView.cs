using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;

namespace Meridian59.DebugUI
{
    public partial class OnlinePlayersView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public OnlinePlayerList DataSource
        {
            get
            {
                if (gridOnlinePlayers.DataSource != null)
                    return (OnlinePlayerList)gridOnlinePlayers.DataSource;
                else
                    return null;
            }
            set
            {
                gridOnlinePlayers.DataSource = value;
            }
        }

        public OnlinePlayersView()
        {
            InitializeComponent();
        }
    }
}
