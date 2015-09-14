/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using Meridian59.Common.Enums;
using Meridian59.Data.Lists;
using Meridian59.Files.RSB;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Meridian59.RsbEditor
{
    public partial class MainForm : Form
    {
        protected readonly StringList filteredData = new StringList(50000);
        protected readonly RsbFile currentFile = new RsbFile();

        public MainForm()
        {
            InitializeComponent();
            
            // listen for changes in the actual rsb file strings,
            // so that we adjust our filtered data
            currentFile.StringResources.ListChanged += OnStringResourcesListChanged;

            // set datasource to filtereddata
            gridStrings.AutoGenerateColumns = false;
            gridStrings.DataSource = filteredData;

            // use language enum as datasource
            cbFilterLanguage.DataSource = Enum.GetValues(typeof(LanguageCode));

            // set initial values
            lblEntries.Text = currentFile.StringResources.Count.ToString();
            lblShown.Text = filteredData.Count.ToString();
            numVersion.Value = currentFile.Version;

            // load file passed by arguments
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
                LoadFile(args[1]);
        }

        public void LoadFile(string File)
        {
            currentFile.Load(File);
            numVersion.Value = currentFile.Version;
        }

        public void SaveFile(string File)
        {
            currentFile.Save(File);
        }

        protected void Filter()
        {
            // backwards due to modify by remove
            for (int i = filteredData.Count - 1 ; i >= 0; i--)
                if (!IsFilterMatch(filteredData[i]))
                    filteredData.RemoveAt(i);

            foreach (RsbResourceID obj in currentFile.StringResources)
                if (IsFilterMatch(obj) && !filteredData.Contains(obj))
                    filteredData.Add(obj);

            /*filteredData.Clear();

            foreach (RsbResourceID obj in currentFile.StringResources)
                filteredData.Add(obj);*/

            lblShown.Text = filteredData.Count.ToString();
        }

        protected bool IsFilterMatch(RsbResourceID ID)
        {
            return
                (!chkFilterID.Checked || ID.ID == numFilterID.Value) &&
                (!chkFilterLanguage.Checked || ID.Language == (LanguageCode)cbFilterLanguage.SelectedValue) &&
                (!chkFilterText.Checked || txtFilterText.Text.Length < 3 || ID.Text.Contains(txtFilterText.Text));
        }

        protected void OnStringResourcesListChanged(object sender, ListChangedEventArgs e)
        {
            switch(e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    if (IsFilterMatch(currentFile.StringResources[e.NewIndex]))
                        filteredData.Add(currentFile.StringResources[e.NewIndex]);
                    lblEntries.Text = currentFile.StringResources.Count.ToString();
                    lblShown.Text = filteredData.Count.ToString();
                    break;

                case ListChangedType.ItemDeleted:
                    filteredData.Remove(currentFile.StringResources.LastDeletedItem);
                    lblEntries.Text = currentFile.StringResources.Count.ToString();
                    lblShown.Text = filteredData.Count.ToString();
                    break;

                case ListChangedType.Reset:
                    filteredData.Clear();
                    foreach (RsbResourceID obj in currentFile.StringResources)
                        if (IsFilterMatch(obj))
                            filteredData.Add(obj);
                    lblEntries.Text = currentFile.StringResources.Count.ToString();
                    lblShown.Text = filteredData.Count.ToString();
                    break;
            }
        }

        protected void OnVersionValueChanged(object sender, EventArgs e)
        {
            currentFile.Version = Convert.ToUInt32(numVersion.Value);
        }

        protected void OnMenuOpenClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)            
                LoadFile(openFileDialog.FileName);            
        }

        protected void OnMenuSaveAsClick(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                SaveFile(saveFileDialog.FileName);
        }


        protected void OnFilterCheckedChanged(object sender, EventArgs e)
        {
            if (sender == chkFilterID)
            { 
                numFilterID.Enabled = chkFilterID.Checked;               
                Filter();
            }
            else if (sender == chkFilterLanguage)
            {
                cbFilterLanguage.Enabled = chkFilterLanguage.Checked;
                Filter();
            }
            else if (sender == chkFilterText)
            {
                txtFilterText.Enabled = chkFilterText.Checked;

                if (!chkFilterText.Checked || (chkFilterText.Checked && txtFilterText.Text.Length > 2))
                    Filter();
            }
        }

        protected void OnFilterIDValueChanged(object sender, EventArgs e)
        {
            Filter();
        }

        protected void OnFilterTextChanged(object sender, EventArgs e)
        {
            if (txtFilterText.Text.Length > 2)
                Filter();
        }

        protected void OnFilterLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }
    }
}
