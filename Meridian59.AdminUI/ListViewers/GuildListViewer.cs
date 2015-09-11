/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.DebugUI".

 "Meridian59.DebugUI" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59.DebugUI" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59.DebugUI".
 If not, see http://www.gnu.org/licenses/.
*/

using System.Windows.Forms;
using System.Drawing;
using Meridian59.Data.Models;
using Meridian59.Data.Lists;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.GameMessages;
using System.ComponentModel;

namespace Meridian59.AdminUI.ListViewers
{
    public class GuildListViewer : Panel
    {
        public event GameMessageEventHandler PacketSend;

        private SplitContainer splitMain = new SplitContainer();
        private TableLayoutPanel tblMain = new TableLayoutPanel();
        private DataGridView dgGuilds = new DataGridView();
        private ListBox listDeclaredYouAlly = new ListBox();
        private ListBox listDeclaredYouEnemy = new ListBox();
        private ListBox listYouDeclaredAlly = new ListBox();
        private ListBox listYouDeclaredEnemy = new ListBox();

        private Label lblDeclaredYouAlly = new Label();
        private Label lblDeclaredYouEnemy = new Label();
        private Label lblYouDeclaredAlly = new Label();
        private Label lblYouDeclaredEnemy = new Label();


        private DiplomacyInfo data = new DiplomacyInfo();

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public DiplomacyInfo DataSource
        {
            get { return data; }
            set {
                data = value;
                
                dgGuilds.DataSource = value.Guilds;
                listDeclaredYouAlly.DataSource = value.DeclaredYouAllyList;
                listDeclaredYouEnemy.DataSource = value.DeclaredYouEnemyList;
                listYouDeclaredAlly.DataSource = value.YouDeclaredAllyList;
                listYouDeclaredEnemy.DataSource = value.YouDeclaredEnemyList;            
            }
        }

        //dgRoomObjects columns
        private DataGridViewColumn colID = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colCount = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colName = new DataGridViewTextBoxColumn();
        
        // Strings used by dgRoomObjects
        private const string strID = "ID";
        private const string strCount = "Count";
        private const string strName = "Name";
        
        private Button btnDeclareNeutralToAlly = new Button();
        private Button btnDeclareNeutralToEnemy = new Button();
        private Button btnDeclareAllyToNeutral = new Button();
        private Button btnDeclareEnemyToNeutral = new Button();

        private Button btnRequestGuilds = new Button();
        

        public GuildListViewer()
        {
            this.Dock = DockStyle.Fill;
            this.data = new DiplomacyInfo();

            this.splitMain.Dock = DockStyle.Fill;
            this.splitMain.Orientation = Orientation.Vertical;
            splitMain.SplitterDistance = 80;
            splitMain.FixedPanel = FixedPanel.None;
            splitMain.IsSplitterFixed = false;

            CreateTable();
            CreateGuildsGrid();
            CreateListBoxes();
            CreateLabels();
            CreateButtons();
            

            this.Controls.Add(splitMain);

            
        }

        private void CreateButtons()
        {
            btnDeclareNeutralToAlly.Text = "Set N->A";           
            btnDeclareNeutralToAlly.Click += new System.EventHandler(btnDeclareNeutralToAlly_Click);
            btnDeclareNeutralToAlly.Location = new Point(300, 0);

            btnDeclareNeutralToEnemy.Text = "Set N->E";
            btnDeclareNeutralToEnemy.Click += new System.EventHandler(btnDeclareNeutralToEnemy_Click);
            btnDeclareNeutralToEnemy.Location = new Point(300, 100);

            btnDeclareAllyToNeutral.Text = "Set A->N";
            btnDeclareAllyToNeutral.Click += new System.EventHandler(btnDeclareAllyToNeutral_Click);
            btnDeclareAllyToNeutral.Location = new Point(300, 100);

            btnDeclareEnemyToNeutral.Text = "Set E->";
            btnDeclareEnemyToNeutral.Click += new System.EventHandler(btnDeclareEnemyToNeutral_Click);
            btnDeclareEnemyToNeutral.Location = new Point(300, 100);

            btnRequestGuilds.Text = "Request";
            btnRequestGuilds.Click += new System.EventHandler(btnRequestGuilds_Click);
            btnRequestGuilds.Location = new Point(300, 100);

            tblMain.Controls.Add(btnDeclareNeutralToAlly, 0, 4);
            tblMain.Controls.Add(btnDeclareNeutralToEnemy, 1, 4);
            tblMain.Controls.Add(btnDeclareAllyToNeutral, 0, 5);
            tblMain.Controls.Add(btnDeclareEnemyToNeutral, 1, 5);
            tblMain.Controls.Add(btnRequestGuilds, 0, 6);

        }

        private void CreateLabels()
        {
            lblDeclaredYouAlly.Text = "Them ALLY You"; 
            lblDeclaredYouEnemy.Text = "Them ENEMY You";
            lblYouDeclaredAlly.Text = "You ALLY them";
            lblYouDeclaredEnemy.Text = "You ENEMY them";

            tblMain.Controls.Add(lblDeclaredYouAlly, 0, 0);
            tblMain.Controls.Add(lblDeclaredYouEnemy, 1, 0);
            tblMain.Controls.Add(lblYouDeclaredAlly, 0, 2);
            tblMain.Controls.Add(lblYouDeclaredEnemy, 1, 2);
        }

        private void CreateTable()
        {
            tblMain.Dock = DockStyle.Fill;
            tblMain.Height = 300;
            tblMain.RowCount = 7;
            tblMain.ColumnCount = 2;

            splitMain.Panel2.Controls.Add(tblMain);
        }

        private void CreateGuildsGrid()
        {
            dgGuilds.DataSource = new BaseList<GuildEntry>();
            
            // Set basic properties of the DataGridView
            dgGuilds.Dock = DockStyle.Fill;
            dgGuilds.ReadOnly = true;
            dgGuilds.AllowUserToAddRows = false;
            dgGuilds.AllowUserToDeleteRows = false;
            dgGuilds.AllowUserToResizeRows = false;
            dgGuilds.RowHeadersVisible = false;
            dgGuilds.MultiSelect = false;
            dgGuilds.AutoGenerateColumns = false;
            dgGuilds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgGuilds.ColumnHeadersVisible = true;
            dgGuilds.Location = new Point(0, 0);

            // Set many properties equal to constant strings
            colID.DataPropertyName    = colID.Name  = colID.HeaderText = strID;
            colCount.DataPropertyName = colCount.Name = strCount;
            colName.DataPropertyName = colName.Name = strName;
            
            colCount.HeaderText = strCount;
            colName.HeaderText = strName;
            
            // enable sorting for columns
            colID.SortMode = DataGridViewColumnSortMode.Automatic;
         
            // Column widths
            colID.Width = 80;
            colCount.Width = 50;
            colName.MinimumWidth = 150;
            
            // AutoSize modi
            colID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            // Column styles
            // colDescription.DefaultCellStyle.BackColor = Color.Wheat;
            colID.Visible = true;
            colCount.Visible = true;
            colName.Visible = true;
            
            // Add columns
            dgGuilds.Columns.Add(colID);
            dgGuilds.Columns.Add(colCount);
            dgGuilds.Columns.Add(colName);
            
            this.splitMain.Panel1.Controls.Add(dgGuilds);
        }

        private void CreateListBoxes()
        {
            listDeclaredYouAlly.Location = new Point(0, 0);
            listDeclaredYouAlly.DisplayMember = "ID";
            listDeclaredYouAlly.ValueMember = "ID";
            listDeclaredYouAlly.Dock = DockStyle.Fill;

            listDeclaredYouEnemy.Location = new Point(100, 0);
            listDeclaredYouEnemy.DisplayMember = "ID";
            listDeclaredYouEnemy.ValueMember = "ID";
            listDeclaredYouEnemy.Dock = DockStyle.Fill;

            listYouDeclaredAlly.Location = new Point(500, 0);
            listYouDeclaredAlly.DisplayMember = "ID";
            listYouDeclaredAlly.ValueMember = "ID";
            listYouDeclaredAlly.Dock = DockStyle.Fill;

            listYouDeclaredEnemy.Location = new Point(600, 0);
            listYouDeclaredEnemy.DisplayMember = "ID";
            listYouDeclaredEnemy.ValueMember = "ID";
            listYouDeclaredEnemy.Dock = DockStyle.Fill;

            tblMain.Controls.Add(listDeclaredYouAlly, 0, 1);
            tblMain.Controls.Add(listDeclaredYouEnemy, 1, 1);
            tblMain.Controls.Add(listYouDeclaredAlly, 0, 3);
            tblMain.Controls.Add(listYouDeclaredEnemy, 1, 3);
        }

        private void btnDeclareNeutralToAlly_Click(object sender, System.EventArgs e)
        {           
            if ((PacketSend != null) && (dgGuilds.SelectedRows.Count > 0))
            {
                GuildEntry entry = (GuildEntry)dgGuilds.SelectedRows[0].DataBoundItem;
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildMakeAlliance(new ObjectID(entry.ID)), null)));
            }
        }

        private void btnDeclareNeutralToEnemy_Click(object sender, System.EventArgs e)
        {
            if ((PacketSend != null) && (dgGuilds.SelectedRows.Count > 0))
            {
                GuildEntry entry = (GuildEntry)dgGuilds.SelectedRows[0].DataBoundItem;
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildMakeEnemy(new ObjectID(entry.ID)), null)));
            }
        }

        private void btnDeclareAllyToNeutral_Click(object sender, System.EventArgs e)
        {
            if ((PacketSend != null) && (dgGuilds.SelectedRows.Count > 0))
            {
                GuildEntry entry = (GuildEntry)dgGuilds.SelectedRows[0].DataBoundItem;
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildEndAlliance(new ObjectID(entry.ID)), null)));
            }
        }

        private void btnDeclareEnemyToNeutral_Click(object sender, System.EventArgs e)
        {
            if ((PacketSend != null) && (dgGuilds.SelectedRows.Count > 0))
            {
                GuildEntry entry = (GuildEntry)dgGuilds.SelectedRows[0].DataBoundItem;
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildEndEnemy(new ObjectID(entry.ID)), null)));
            }
        }

        private void btnRequestGuilds_Click(object sender, System.EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildGuildListReq(), null)));
        }
    }
}
