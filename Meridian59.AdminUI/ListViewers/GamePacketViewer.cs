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

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Meridian59.AdminUI.Generic;
using Meridian59.AdminUI.Events;
using Meridian59.AdminUI.CustomDataGridColumns;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.GameMessages;
using Meridian59.Data.Lists;

namespace Meridian59.AdminUI.ListViewers
{
    
    public class GamePacketViewer : Panel
    {
        public event GameMessageEventHandler PacketSend;
        public event PacketLogChangeEventHandler PacketLogChanged;

        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public BaseList<GameMessage> DataSource
        {
            get { return (BaseList<GameMessage>)gridMonitor.DataSource; }
            set { 
                gridMonitor.DataSource = value;
                if (value != null)
                    value.ListChanged += new ListChangedEventHandler(value_ListChanged);
            }
        }

        // UI Controls
        private SplitContainer splitContainer = new SplitContainer();
        private DataGridView gridMonitor = new DataGridView();
        public CheckBox chkRecv = new CheckBox();
        public CheckBox chkSend = new CheckBox();
        public CheckBox chkPings = new CheckBox();
        private CheckBox chkLEN = new CheckBox();
        private CheckBox chkCRC = new CheckBox();
        private CheckBox chkSS = new CheckBox();
        private CheckBox chkPI = new CheckBox();
        private CheckBox chkDATA = new CheckBox();
        private CheckBox chkAutoScroll = new CheckBox();
        private HexTextBox txtSendPacket = new HexTextBox();
        private Button btnSend = new Button();
        private Button btnClear = new Button();
        private Label lblSend = new Label();

        // DataGrid columns
        private InOutColumn colTrafficDirection = new InOutColumn();
        private DataGridViewColumn colDescription = new DataGridViewTextBoxColumn();
        private ByteColumn colLEN1 = new ByteColumn();
        private ByteColumn colCRC = new ByteColumn();
        private ByteColumn colLEN2 = new ByteColumn();
        private ByteColumn colSS = new ByteColumn();
        private ByteColumn colPI = new ByteColumn();
        private ByteColumn colDATA = new ByteColumn();
        
        // ConstantStrings      
        private const string strTrafficDirection = "TransferDirection";
        private const string strDescription = "Description";
        private const string strLEN1 = "LEN1Bytes";
        private const string strCRC = "CRCBytes";
        private const string strLEN2 = "LEN2Bytes";
        private const string strSS = "HeaderSS";
        private const string strPI = "PI";
        private const string strDATA = "DATABytes";
        private const string strASCII = "ASCII";

        private const string strLEN1Desc = "LEN1";
        private const string strCRCDesc = "CRC";
        private const string strLEN2Desc = "LEN2";
        private const string strSSDesc = "SS";
        private const string strDATADesc = "DATA";
        private const string strPIDesc = "PI";
        
        private void value_ListChanged(object sender, ListChangedEventArgs e)
        {
            if ((chkAutoScroll.Checked) && (gridMonitor.Rows.Count > 2))
                gridMonitor.FirstDisplayedScrollingRowIndex = gridMonitor.Rows.Count - 1;
        }

        // Constructor
        public GamePacketViewer()
        {
            this.DataSource = new BaseList<GameMessage>();
            this.Dock = DockStyle.Fill;
            
            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.FixedPanel = FixedPanel.Panel1;
            splitContainer.SplitterDistance = 60;
            splitContainer.Dock = DockStyle.Fill;

            // CheckBox Row1
            chkRecv.Location = new Point(10, 0);
            chkSend.Location = new Point(70, 0);
            chkPings.Location = new Point(130, 0);
            chkLEN.Location = new Point(190, 0);
            chkCRC.Location = new Point(240, 0);
            chkSS.Location = new Point(290, 0);
            chkPI.Location = new Point(340, 0);
            chkDATA.Location = new Point(390, 0);
            chkAutoScroll.Location = new Point(510, 0);
            
            // Row2
            btnClear.Location = new Point(10, 30);
            lblSend.Location = new Point(130, 35);
            txtSendPacket.Location = new Point(230, 30);
            btnSend.Location = new Point(500, 30);

            chkRecv.Width = 60;
            chkSend.Width = 60;
            chkPings.Width = 60;
            chkLEN.Width = 50;
            chkCRC.Width = 50;
            chkSS.Width = 50;
            chkPI.Width = 50;
            chkDATA.Width = 60;
            chkAutoScroll.Width = 100;
            btnClear.Width = 80;
            lblSend.Width = 100;
            btnSend.Width = 80;
            txtSendPacket.Width = 250;
            
            btnClear.Height = 22;
            btnSend.Height = 22;

            btnClear.Text = "Clear";
            lblSend.Text = "Send PacketBody:";
            btnSend.Text = "Send";

            CreateCheckBoxes();
            CreateMonitorGrid();

            // Attach event handlers
            btnClear.Click += new EventHandler(btnClear_Click);
            btnSend.Click += new EventHandler(btnSend_Click);
            chkLEN.CheckedChanged += new EventHandler(ChkBoxColumnFilterChanged);
            chkCRC.CheckedChanged += new EventHandler(ChkBoxColumnFilterChanged);
            chkSS.CheckedChanged += new EventHandler(ChkBoxColumnFilterChanged);
            chkPI.CheckedChanged += new EventHandler(ChkBoxColumnFilterChanged);
            chkDATA.CheckedChanged += new EventHandler(ChkBoxColumnFilterChanged);

            chkPings.CheckedChanged += new EventHandler(chkFilter_CheckedChanged);
            chkSend.CheckedChanged += new EventHandler(chkFilter_CheckedChanged);
            chkRecv.CheckedChanged += new EventHandler(chkFilter_CheckedChanged);

            // Add controls
            splitContainer.Panel1.Controls.Add(chkRecv);
            splitContainer.Panel1.Controls.Add(chkSend);
            splitContainer.Panel1.Controls.Add(chkPings);
            splitContainer.Panel1.Controls.Add(chkLEN);
            splitContainer.Panel1.Controls.Add(chkCRC);
            splitContainer.Panel1.Controls.Add(chkSS);
            splitContainer.Panel1.Controls.Add(chkPI);
            splitContainer.Panel1.Controls.Add(chkDATA);
            splitContainer.Panel1.Controls.Add(chkAutoScroll);
            splitContainer.Panel1.Controls.Add(btnClear);
            splitContainer.Panel1.Controls.Add(txtSendPacket);
            splitContainer.Panel1.Controls.Add(btnSend);
            splitContainer.Panel1.Controls.Add(lblSend);
            splitContainer.Panel2.Controls.Add(gridMonitor);
            this.Controls.Add(splitContainer);
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (PacketLogChanged != null)
                PacketLogChanged(this, new PacketLogChangeEventArgs(chkSend.Checked, chkRecv.Checked, chkPings.Checked));
        }    

        private void btnSend_Click(object sender, EventArgs e)
        {
            
            try
            {
                byte[] packetbody = txtSendPacket.GetBinaryValue();
                byte[] packetdata = new byte[GameMessage.HEADERLENGTH + packetbody.Length];
                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(packetbody.Length)), 0, packetdata, 0, 2);
                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(packetbody.Length)), 0, packetdata, 4, 2);
                Array.Copy(packetbody, 0, packetdata, GameMessage.HEADERLENGTH, packetbody.Length);

                GenericGameMessage packet = new GenericGameMessage(packetdata);
                
                if (PacketSend != null) PacketSend(this, new GameMessageEventArgs(packet));
            }
            catch(Exception)
            {
                
            }
        }
        
        protected void ChkBoxColumnFilterChanged(object sender, EventArgs e)
        {
            string controlName = ((CheckBox)sender).Name;
            bool isChecked = ((CheckBox)sender).Checked;
            
            switch(controlName)
            {
                case "chkLEN":
                    gridMonitor.Columns[strLEN1].Visible = isChecked;
                    gridMonitor.Columns[strLEN2].Visible = isChecked;
                    break;

                case "chkCRC":
                    gridMonitor.Columns[strCRC].Visible = isChecked;
                    break;

                case "chkSS":
                    gridMonitor.Columns[strSS].Visible = isChecked;
                    break;

                case "chkPI":
                    gridMonitor.Columns[strPI].Visible = isChecked;
                    break;

                case "chkDATA":
                    gridMonitor.Columns[strDATA].Visible = isChecked;
                    break;

                case "chkASCII":
                    gridMonitor.Columns[strASCII].Visible = isChecked;
                    break;
            }
        }
 
        private void btnClear_Click(object sender, EventArgs e)
        {
            DataSource.Clear();
        }
    
        private void CreateCheckBoxes()
        {
            // set checkbox names
            chkRecv.Name = "chkRecv";
            chkSend.Name = "chkSend";
            chkPings.Name = "chkPings";
            chkLEN.Name = "chkLEN";
            chkCRC.Name = "chkCRC";
            chkSS.Name = "chkSS";
            chkPI.Name = "chkPI";
            chkDATA.Name = "chkDATA";
            chkAutoScroll.Name = "chkAutoScroll";

            // set checkbox labels
            chkRecv.Text = "RECV";
            chkSend.Text = "SEND";
            chkPings.Text = "PINGS";
            chkLEN.Text = "LEN";
            chkCRC.Text = "CRC";
            chkSS.Text = "SS";
            chkPI.Text = "PI";
            chkDATA.Text = "DATA";
            chkAutoScroll.Text = "AutoScroll";

            // apply default checked state
            chkRecv.Checked = false;
            chkSend.Checked = false;
            chkPings.Checked = false;
            chkLEN.Checked = true;
            chkCRC.Checked = true;
            chkSS.Checked = true;
            chkPI.Checked = true;
            chkDATA.Checked = true;
            chkAutoScroll.Checked = false;
        }

        private void CreateMonitorGrid()
        {
            // Set basic properties of the DataGridView
            gridMonitor.Dock = DockStyle.Fill;
            gridMonitor.ReadOnly = true;
            gridMonitor.AllowUserToAddRows = false;
            gridMonitor.AllowUserToDeleteRows = false;
            gridMonitor.AllowUserToResizeRows = false;
            gridMonitor.RowHeadersVisible = false;
            gridMonitor.AutoGenerateColumns = false;

            // Set many properties equal to constant strings
            colDescription.DataPropertyName = colDescription.Name   = colDescription.HeaderText = strDescription;
            colLEN1.DataPropertyName        = colLEN1.Name          = strLEN1;
            colCRC.DataPropertyName         = colCRC.Name           = strCRC;
            colLEN2.DataPropertyName        = colLEN2.Name          = strLEN2;
            colSS.DataPropertyName          = colSS.Name            = strSS;
            colPI.DataPropertyName          = colPI.Name            = strPI;
            colDATA.DataPropertyName        = colDATA.Name          = strDATA;
            
            colLEN1.HeaderText = strLEN1Desc;
            colCRC.HeaderText = strCRCDesc;
            colLEN2.HeaderText = strLEN2Desc;
            colSS.HeaderText = strSSDesc;
            colPI.HeaderText = strPIDesc;
            colDATA.HeaderText = strDATADesc;
            
            // the only one don't matching all strings
            colTrafficDirection.DataPropertyName = colTrafficDirection.Name = strTrafficDirection;
            colTrafficDirection.HeaderText = String.Empty;
     
            // Column widths
            colTrafficDirection.Width = 40;
            colDescription.Width = 100;
            colLEN1.Width = 40;
            colCRC.Width = 40;
            colLEN2.Width = 40;
            colSS.Width = 30;
            colPI.Width = 30;
            colDATA.Width = 200;
            
            // Set visible columns based on checkboxes
            colTrafficDirection.Visible = true;
            colDescription.Visible = true;
            colLEN1.Visible = chkLEN.Checked;
            colCRC.Visible = chkCRC.Checked;
            colSS.Visible = chkSS.Checked;
            colPI.Visible = chkPI.Checked;
            colDATA.Visible = chkDATA.Checked;
            
            // AutoSize modi
            colTrafficDirection.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colLEN1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCRC.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colLEN2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSS.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colPI.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDATA.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            // Column styles
            colDescription.DefaultCellStyle.BackColor = Color.Wheat;
            colLEN1.DefaultCellStyle.BackColor = Color.PaleGreen;
            colCRC.DefaultCellStyle.BackColor = Color.PaleGreen;
            colLEN2.DefaultCellStyle.BackColor = Color.PaleGreen;
            colSS.DefaultCellStyle.BackColor = Color.PaleGreen;
            colPI.DefaultCellStyle.BackColor = Color.Wheat;
            colDATA.DefaultCellStyle.BackColor = Color.PaleGoldenrod;
            
            // Add columns
            gridMonitor.Columns.Add(colTrafficDirection);
            gridMonitor.Columns.Add(colDescription);
            gridMonitor.Columns.Add(colLEN1);
            gridMonitor.Columns.Add(colCRC);
            gridMonitor.Columns.Add(colLEN2);
            gridMonitor.Columns.Add(colSS);
            gridMonitor.Columns.Add(colPI);
            gridMonitor.Columns.Add(colDATA);         
        }
    }
}
