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
using System.ComponentModel;
using System.Drawing;
using Meridian59.AdminUI.DataGridColumns;
using Meridian59.Data.Lists;
using Meridian59.Data.Models;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.GameMessages;

namespace Meridian59.AdminUI.ListViewers
{
    public class GuildMemberListViewer : Panel
    {
        public event GameMessageEventHandler PacketSend;

        private DataGridView dgGuildMembers = new DataGridView();
        
        private GuildInfo data;
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public GuildInfo DataSource
        {
            get { return data; }
            set {
                data = value;
                value.GuildMembers.ListChanged += new ListChangedEventHandler(value_ListChanged);
                dgGuildMembers.DataSource = value.GuildMembers;

                lblGuildName.DataBindings.Clear();
                lblGuildName.DataBindings.Add("Text", DataSource, "GuildName", false, DataSourceUpdateMode.OnPropertyChanged);
              
                chkPasswordSet.DataBindings.Clear();
                chkPasswordSet.DataBindings.Add("Checked", DataSource, "PasswordSetFlag", false, DataSourceUpdateMode.OnPropertyChanged);
                
                txtChestPassword.DataBindings.Clear();
                txtChestPassword.DataBindings.Add("Text", DataSource, "ChestPassword", false, DataSourceUpdateMode.OnPropertyChanged);

                lblGuildID.DataBindings.Clear();
                lblGuildID.DataBindings.Add("Text", DataSource.GuildID, "ID", false, DataSourceUpdateMode.OnPropertyChanged);

                lblSupportedMemberID.DataBindings.Clear();
                lblSupportedMemberID.DataBindings.Add("Text", DataSource.SupportedMember, "ID", false, DataSourceUpdateMode.OnPropertyChanged);

                lblFlags.DataBindings.Clear();
                lblFlags.DataBindings.Add("Text", DataSource, "Flags", false, DataSourceUpdateMode.OnPropertyChanged);

                lblMaleRank1.DataBindings.Clear();
                lblMaleRank1.DataBindings.Add("Text", DataSource, "Rank1Male", false, DataSourceUpdateMode.OnPropertyChanged);

                lblFemaleRank1.DataBindings.Clear();
                lblFemaleRank1.DataBindings.Add("Text", DataSource, "Rank1Female", false, DataSourceUpdateMode.OnPropertyChanged);

                lblMaleRank2.DataBindings.Clear();
                lblMaleRank2.DataBindings.Add("Text", DataSource, "Rank2Male", false, DataSourceUpdateMode.OnPropertyChanged);

                lblFemaleRank2.DataBindings.Clear();
                lblFemaleRank2.DataBindings.Add("Text", DataSource, "Rank2Female", false, DataSourceUpdateMode.OnPropertyChanged);

                lblMaleRank3.DataBindings.Clear();
                lblMaleRank3.DataBindings.Add("Text", DataSource, "Rank3Male", false, DataSourceUpdateMode.OnPropertyChanged);

                lblFemaleRank3.DataBindings.Clear();
                lblFemaleRank3.DataBindings.Add("Text", DataSource, "Rank3Female", false, DataSourceUpdateMode.OnPropertyChanged);

                lblMaleRank4.DataBindings.Clear();
                lblMaleRank4.DataBindings.Add("Text", DataSource, "Rank4Male", false, DataSourceUpdateMode.OnPropertyChanged);

                lblFemaleRank4.DataBindings.Clear();
                lblFemaleRank4.DataBindings.Add("Text", DataSource, "Rank4Female", false, DataSourceUpdateMode.OnPropertyChanged);
                
                lblMaleRank5.DataBindings.Clear();
                lblMaleRank5.DataBindings.Add("Text", DataSource, "Rank5Male", false, DataSourceUpdateMode.OnPropertyChanged);

                lblFemaleRank5.DataBindings.Clear();
                lblFemaleRank5.DataBindings.Add("Text", DataSource, "Rank5Female", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void value_ListChanged(object sender, ListChangedEventArgs e)
        {
         
        }

        private SplitContainer splitMain = new SplitContainer();

        //dgRoomObjects columns
        private DataGridViewColumn colID = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colCount = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colName = new DataGridViewTextBoxColumn();
        private DataGridViewColumn colRank = new HexColumn();
        private DataGridViewColumn colGender = new HexColumn();
        
        // Strings used by dgRoomObjects
        private const string strID = "ID";
        private const string strCount = "Count";
        private const string strName = "Name";
        private const string strRank = "Rank";
        private const string strGender = "Gender";

        private Button btnRequestInfo = new Button();
        private Button btnSupportMember = new Button();

        private Label lblGuildNameDesc = new Label();
        private Label lblGuildName = new Label();
        private CheckBox chkPasswordSet = new CheckBox();
        private TextBox txtChestPassword = new TextBox();
        private Label lblGuildIDDesc = new Label();
        private Label lblGuildID = new Label();
        private Label lblSupportedMemberDesc = new Label();
        private Label lblSupportedMemberID = new Label();
        private Label lblFlagsDesc = new Label();
        private Label lblFlags = new Label();

        private Label lblMaleRanksDesc = new Label();
        private Label lblFemaleRanksDesc = new Label();
        private Label lblMaleRank1 = new Label();
        private Label lblFemaleRank1 = new Label();
        private Label lblMaleRank2 = new Label();
        private Label lblFemaleRank2 = new Label();
        private Label lblMaleRank3 = new Label();
        private Label lblFemaleRank3 = new Label();
        private Label lblMaleRank4 = new Label();
        private Label lblFemaleRank4 = new Label();
        private Label lblMaleRank5 = new Label();
        private Label lblFemaleRank5 = new Label();

        public GuildMemberListViewer()
        {
            this.data = new GuildInfo();
            CreateGrid();
            CreateInfo();


            splitMain.Orientation = Orientation.Horizontal;
            splitMain.Dock = DockStyle.Fill;
            splitMain.SplitterDistance = 10;
            splitMain.FixedPanel = FixedPanel.None;
            splitMain.IsSplitterFixed = false;

            
            this.Controls.Add(splitMain);
            this.Dock = DockStyle.Fill;
            
        }

        private void CreateGrid()
        {
            dgGuildMembers.DataSource = new GuildMemberList();
            
            // Set basic properties of the DataGridView
            dgGuildMembers.Dock = DockStyle.Fill;
            dgGuildMembers.ReadOnly = true;
            dgGuildMembers.AllowUserToAddRows = false;
            dgGuildMembers.AllowUserToDeleteRows = false;
            dgGuildMembers.AllowUserToResizeRows = false;
            dgGuildMembers.RowHeadersVisible = false;
            dgGuildMembers.MultiSelect = false;
            dgGuildMembers.AutoGenerateColumns = false;
            dgGuildMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgGuildMembers.ColumnHeadersVisible = true; 

            // Set many properties equal to constant strings
            colID.DataPropertyName    = colID.Name  = colID.HeaderText = strID;
            colCount.DataPropertyName = colCount.Name = strCount;
            colName.DataPropertyName = colName.Name = strName;
            colRank.DataPropertyName = colRank.Name = strRank;
            colGender.DataPropertyName = colGender.Name = strGender;
            
            colCount.HeaderText = strCount;
            colRank.HeaderText = strRank;
            colGender.HeaderText = strGender;
            colName.HeaderText = strName;
            

            // enable sorting for columns
            colID.SortMode = DataGridViewColumnSortMode.Automatic;
         
            // Column widths
            colID.Width = 80;
            colCount.Width = 50;
            colName.MinimumWidth = 150;
            colRank.Width = 50;
            colGender.Width = 50;
            

            // AutoSize modi
            colID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRank.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colGender.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            

            // Column styles
            // colDescription.DefaultCellStyle.BackColor = Color.Wheat;
            colID.Visible = true;
            colCount.Visible = true;
            colName.Visible = true;
            colRank.Visible = true;
            colGender.Visible = true;
            

            // Add columns
            dgGuildMembers.Columns.Add(colID);
            dgGuildMembers.Columns.Add(colCount);
            dgGuildMembers.Columns.Add(colName);
            dgGuildMembers.Columns.Add(colRank);
            dgGuildMembers.Columns.Add(colGender);
            
            this.splitMain.Panel2.Controls.Add(dgGuildMembers);
        }

        private void CreateInfo()
        {
            btnRequestInfo.Text = "Request";
            btnRequestInfo.Height = 20;
            btnRequestInfo.Width = 100;
            btnRequestInfo.Click += new System.EventHandler(btnRequestInfo_Click);
            btnRequestInfo.Location = new Point(0, 100);

            btnSupportMember.Text = "Support";
            btnSupportMember.Height = 20;
            btnSupportMember.Width = 100;
            btnSupportMember.Click += new System.EventHandler(btnSupportMember_Click);
            btnSupportMember.Location = new Point(100, 100);

            lblGuildNameDesc.Text = "GuildName";
            lblGuildNameDesc.Width = 100;
            lblGuildNameDesc.Height = 20;
            lblGuildNameDesc.Location = new Point(0, 0);
            
            lblGuildName.Width = 100;
            lblGuildName.Height = 20;
            lblGuildName.Location = new Point(100, 0);

            chkPasswordSet.Width = 100;
            chkPasswordSet.Height = 20;
            chkPasswordSet.Text = "ChestPW";
            chkPasswordSet.Location = new Point(0, 20);

            txtChestPassword.Width = 100;
            txtChestPassword.Height = 20;
            txtChestPassword.Location = new Point(100, 20);

            lblGuildIDDesc.Text = "GuildID";
            lblGuildIDDesc.Width = 100;
            lblGuildIDDesc.Height = 20;
            lblGuildIDDesc.Location = new Point(0, 40);
            
            lblGuildID.Width = 100;
            lblGuildID.Height = 20;
            lblGuildID.Location = new Point(100, 40);

            lblSupportedMemberDesc.Text = "SupportedMember";
            lblSupportedMemberDesc.Width = 100;
            lblSupportedMemberDesc.Height = 20;
            lblSupportedMemberDesc.Location = new Point(0, 60);

            lblSupportedMemberID.Width = 100;
            lblSupportedMemberID.Height = 20;
            lblSupportedMemberID.Location = new Point(100, 60);

            lblFlagsDesc.Text = "Flags";
            lblFlagsDesc.Width = 100;
            lblFlagsDesc.Height = 20;
            lblFlagsDesc.Location = new Point(0, 80);

            lblFlags.Width = 100;
            lblFlags.Height = 20;
            lblFlags.Location = new Point(100, 80);

            lblMaleRanksDesc.Width = 100;
            lblMaleRanksDesc.Height = 20;
            lblMaleRanksDesc.Text = "Male";
            lblMaleRanksDesc.Location = new Point(200, 0);

            lblFemaleRanksDesc.Width = 100;
            lblFemaleRanksDesc.Height = 20;
            lblFemaleRanksDesc.Text = "Female";
            lblFemaleRanksDesc.Location = new Point(300, 0);

            lblMaleRank1.Width = 100;
            lblMaleRank1.Height = 20;
            lblMaleRank1.Location = new Point(200, 20);

            lblFemaleRank1.Width = 100;
            lblFemaleRank1.Height = 20;
            lblFemaleRank1.Location = new Point(300, 20);

            lblMaleRank2.Width = 100;
            lblMaleRank2.Height = 20;
            lblMaleRank2.Location = new Point(200, 40);

            lblFemaleRank2.Width = 100;
            lblFemaleRank2.Height = 20;
            lblFemaleRank2.Location = new Point(300, 40);

            lblMaleRank3.Width = 100;
            lblMaleRank3.Height = 20;
            lblMaleRank3.Location = new Point(200, 60);

            lblFemaleRank3.Width = 100;
            lblFemaleRank3.Height = 20;
            lblFemaleRank3.Location = new Point(300, 60);

            lblMaleRank4.Width = 100;
            lblMaleRank4.Height = 20;
            lblMaleRank4.Location = new Point(200, 80);

            lblFemaleRank4.Width = 100;
            lblFemaleRank4.Height = 20;
            lblFemaleRank4.Location = new Point(300, 80);

            lblMaleRank5.Width = 100;
            lblMaleRank5.Height = 20;
            lblMaleRank5.Location = new Point(200, 100);

            lblFemaleRank5.Width = 100;
            lblFemaleRank5.Height = 20;
            lblFemaleRank5.Location = new Point(300, 100);

            this.splitMain.Panel1.Controls.Add(btnRequestInfo);
            this.splitMain.Panel1.Controls.Add(btnSupportMember);  
            this.splitMain.Panel1.Controls.Add(lblGuildNameDesc);
            this.splitMain.Panel1.Controls.Add(lblGuildName);
            this.splitMain.Panel1.Controls.Add(chkPasswordSet);
            this.splitMain.Panel1.Controls.Add(txtChestPassword);
            this.splitMain.Panel1.Controls.Add(lblGuildIDDesc);
            this.splitMain.Panel1.Controls.Add(lblGuildID);
            this.splitMain.Panel1.Controls.Add(lblSupportedMemberDesc);
            this.splitMain.Panel1.Controls.Add(lblSupportedMemberID);
            this.splitMain.Panel1.Controls.Add(lblFlagsDesc);
            this.splitMain.Panel1.Controls.Add(lblFlags);
            this.splitMain.Panel1.Controls.Add(lblMaleRanksDesc);
            this.splitMain.Panel1.Controls.Add(lblFemaleRanksDesc);
            this.splitMain.Panel1.Controls.Add(lblMaleRank1);
            this.splitMain.Panel1.Controls.Add(lblFemaleRank1);
            this.splitMain.Panel1.Controls.Add(lblMaleRank2);
            this.splitMain.Panel1.Controls.Add(lblFemaleRank2);
            this.splitMain.Panel1.Controls.Add(lblMaleRank3);
            this.splitMain.Panel1.Controls.Add(lblFemaleRank3);
            this.splitMain.Panel1.Controls.Add(lblMaleRank4);
            this.splitMain.Panel1.Controls.Add(lblFemaleRank4);
            this.splitMain.Panel1.Controls.Add(lblMaleRank5);
            this.splitMain.Panel1.Controls.Add(lblFemaleRank5);
        }

        private void btnSupportMember_Click(object sender, System.EventArgs e)
        {
            if ((PacketSend != null) && (dgGuildMembers.SelectedRows.Count > 0))
            {
                GuildMemberEntry entry = (GuildMemberEntry)dgGuildMembers.SelectedRows[0].DataBoundItem;
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildVote(new ObjectID(entry.ID)), null)));
            }
        }

        private void btnRequestInfo_Click(object sender, System.EventArgs e)
        {
            if (PacketSend != null)
                PacketSend(this, new GameMessageEventArgs(new UserCommandMessage(new UserCommandGuildInfoReq(), null)));
        }
    }
}
