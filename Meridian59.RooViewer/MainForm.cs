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


using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Meridian59.Files.ROO;
using Meridian59.Common;

namespace Meridian59.RooViewer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooFile Room
        {
            get
            {
                return Program.Room;
            }

            set
            {               
                viewerRooWalls.DataSource = value.Walls;
                viewerRooWallsEditor.DataSource = value.WallsEditor;
                viewerRooSides.DataSource = value.SideDefs;
                viewerRooSectors.DataSource = value.Sectors;
                viewerRooPartitionLines.DataSource = value.BSPTreeNodes;
                viewerRooSubSectors.DataSource = value.BSPTreeLeaves;

                map.Room = value;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        protected void OnMenuFileOpen(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void OnMenuFileSave(object sender, EventArgs e)
        {
            Program.SaveRoom(openFileDialog.FileName);
        }

        protected void OnMenuExtractAllUsedTextures(object sender, EventArgs e)
        {
            if (Room == null)
                return;

            // let pick outputfolder
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                Program.ExtractAllTextures(folderBrowser.SelectedPath);
            }
        }

        protected void OnOpenFileDialogFileOk(object sender, CancelEventArgs e)
        {
            Program.OpenRoom(openFileDialog.FileName);
        }

        protected void OnViewerRooSectorsSelectedItemChanged(object sender, EventArgs e)
        {
            map.SelectedSector = viewerRooSectors.SelectedItem;
        }

        protected void OnViewerRooSubSectorsSelectedItemChanged(object sender, EventArgs e)
        {
            if (viewerRooSubSectors.SelectedItem != null)
            {
                viewerRooVertices.DataSource = viewerRooSubSectors.SelectedItem.Vertices;
                map.SelectedSubSector = viewerRooSubSectors.SelectedItem;
            }
            else
            { 
                viewerRooVertices.DataSource = null;
                map.SelectedSubSector = null;
            }
        }

        protected void OnViewerRooWallsSelectedItemChanged(object sender, EventArgs e)
        {
            map.SelectedWall = viewerRooWalls.SelectedItem;
        }

        protected void OnViewerRooSidesSelectedItemChanged(object sender, EventArgs e)
        {
            map.SelectedSide = viewerRooSides.SelectedItem;
        }

        protected void OnViewerRooPartitionLinesSelectedItemChanged(object sender, EventArgs e)
        {
            map.SelectedPartitionLine = viewerRooPartitionLines.SelectedItem;
        }

        protected void OnViewerRooWallsEditorSelectedItemChanged(object sender, EventArgs e)
        {
            map.SelectedWallEditor = viewerRooWallsEditor.SelectedItem;
        }

        protected void OnMapSelectedSectorChanged(object sender, EventArgs e)
        {
            viewerRooSectors.SelectedItem = map.SelectedSector;
        }

        protected void OnMapSelectedSubSectorChanged(object sender, EventArgs e)
        {
            viewerRooSubSectors.SelectedItem = map.SelectedSubSector;
        }

        protected void OnMenuRebuildBSPTreeClick(object sender, EventArgs e)
        {
            BSPBuilder.Build(Room);

            viewerRooPartitionLines.DataSource = null;
            viewerRooPartitionLines.DataSource = Room.BSPTreeNodes;

            viewerRooSubSectors.DataSource = null;
            viewerRooSubSectors.DataSource = Room.BSPTreeLeaves;

            viewerRooWalls.DataSource = null;
            viewerRooWalls.DataSource = Room.Walls;
        }

        private void OnBSPBuilderFoundNonConvexPolygon(object sender, Polygon e)
        {

        }
    }
}
