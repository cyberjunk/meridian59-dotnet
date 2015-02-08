using System;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;

namespace Meridian59.DebugUI.Viewers
{
    public partial class SpellsView : UserControl
    {
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public SpellObjectList DataSource
        {
            get
            {
                if (gridSpells.DataSource != null)
                    return (SpellObjectList)gridSpells.DataSource;
                else
                    return null;
            }
            set
            {
                gridSpells.DataSource = value;
            }
        }

        public SpellsView()
        {
            InitializeComponent();
        }

        private void gridSpells_SelectionChanged(object sender, EventArgs e)
        {
            if (gridSpells.SelectedRows.Count > 0 && gridSpells.SelectedRows[0].DataBoundItem != null)
            {
                SpellObject spellObject = (SpellObject)gridSpells.SelectedRows[0].DataBoundItem;

                spellObject.SubOverlays.SyncContext = SynchronizationContext.Current;
                
                avAnimation.DataSource = null;
                avSubOverlayAnimation.DataSource = null;
                
                gridSubOverlays.DataSource = spellObject.SubOverlays;               
                avAnimation.DataSource = spellObject.Animation;              
            }
        }
    }
}
