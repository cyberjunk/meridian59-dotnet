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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

using Meridian59.Files.ROO;
using Meridian59.Common;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.RooViewer.UI
{
    public partial class Map : UserControl
    {
        protected const float DEFAULTZOOM = 64.0f;

        public event EventHandler SelectedSubSectorChanged;
        public event EventHandler SelectedSectorChanged;

        protected RooFile room;
        protected float zoom;
        protected V2 center;
        protected V2 boxMin;
        protected V2 boxMax;
        protected Point oldMousePosition;

        protected RooPartitionLine selectedPartitionLine;
        protected RooSubSector selectedSubSector;
        protected RooSector selectedSector;
        protected RooWall selectedWall;
        protected RooWallEditor selectedWallEditor;
        protected RooSideDef selectedSide;

        protected List<Polygon> bspBuilderNonConvexPolygons = new List<Polygon>();

        protected Pen penWhite1 = new Pen(Color.White, 1f);
        protected Pen penRed1 = new Pen(Color.Red, 1f);
        protected Pen penRed2 = new Pen(Color.Red, 2f);
        protected Pen penBlue1 = new Pen(Color.Blue, 1f);
        protected Pen penBlue2 = new Pen(Color.Blue, 2f);
        protected Pen penLightBlue1 = new Pen(Color.LightBlue, 1f);
        protected Pen penOrange1 = new Pen(Color.Orange, 1f);
        protected Pen penOrange2 = new Pen(Color.Orange, 2f);
        protected Pen penViolet3 = new Pen(Color.Violet, 3f);

        protected SolidBrush brushSolidGreen = new SolidBrush(Color.Green);
        protected SolidBrush brushSolidPaleGreen = new SolidBrush(Color.PaleGreen);
        protected SolidBrush brushSolidRed = new SolidBrush(Color.Red);

        protected Font fontDefault = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooFile Room
        {
            get { return room; }
            set 
            {
                if (room != value)
                {
                    room = value;

                    BoundingBox2D bBox = room.GetBoundingBox2D();
                    center.X = bBox.Min.X + 0.5f * (bBox.Max.X - bBox.Min.X);
                    center.Y = bBox.Min.Y + 0.5f * (bBox.Max.Y - bBox.Min.Y);
                    
                    bspBuilderNonConvexPolygons.Clear();

                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooPartitionLine SelectedPartitionLine
        {
            get { return selectedPartitionLine; }
            set
            {
                if (selectedPartitionLine != value)
                {
                    selectedPartitionLine = value;
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooSubSector SelectedSubSector
        {
            get { return selectedSubSector; }
            set
            {
                if (selectedSubSector != value)
                {
                    selectedSubSector = value;
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooSector SelectedSector
        {
            get { return selectedSector; }
            set
            {
                if (selectedSector != value)
                {
                    selectedSector = value;
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooWall SelectedWall
        {
            get { return selectedWall; }
            set
            {
                if (selectedWall != value)
                {
                    selectedWall = value;
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooWallEditor SelectedWallEditor
        {
            get { return selectedWallEditor; }
            set
            {
                if (selectedWallEditor != value)
                {
                    selectedWallEditor = value;
                    Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public RooSideDef SelectedSide
        {
            get { return selectedSide; }
            set
            {
                if (selectedSide != value)
                {
                    selectedSide = value;
                    Invalidate();
                }
            }
        }

        public float Zoom
        {
            get { return zoom; }
            protected set 
            {
                if (zoom != value && value > 0.0f)
                    zoom = value;
            }
        }

        public float ZoomInv
        {
            get { return 1.0f / zoom; }
        }

        public V2 Center
        {
            get { return center; }
            set 
            {
                if (center.X != value.X && center.Y != value.Y)
                {
                    center.X = value.X;
                    center.Y = value.Y;

                    Invalidate();
                }
            }
        }

        public Map()
        {
            zoom = DEFAULTZOOM;
            oldMousePosition = MousePosition;
            
            // don't flicker in OnPaint
            DoubleBuffered = true;

            InitializeComponent();

            BSPBuilder.FoundNonConvexPolygon += OnBSPBuilderFoundNonConvexPolygon;
            BSPBuilder.BuildStarted += OnBSPBuilderBuildStarted;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(Color.Black);

            DrawElements(e.Graphics);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (oldMousePosition == e.Location)
                return;

            float mapx = ((float)e.Location.X / ZoomInv + (float)boxMin.X);
            float mapy = ((float)e.Location.Y / ZoomInv + (float)boxMin.Y);

            lblMouseCoordinates.Text = '(' + ((int)mapx).ToString() + '/' + ((int)mapy).ToString() + ')';
            
            if (e.Button == MouseButtons.Right)
            { 
                center.X += (oldMousePosition.X - e.Location.X) * zoom;
                center.Y += (oldMousePosition.Y - e.Location.Y) * zoom;

                Invalidate();
            }

            oldMousePosition = e.Location;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            // step big
            if (zoom > 16.0f)
            {
                Zoom -= (float)e.Delta * 0.05f;           
            }
            // step small
            else
            {
                Zoom -= (float)e.Delta * 0.01f;
            }
            
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (room == null)
                return;

            float mapx = ((float)e.Location.X / ZoomInv + (float)boxMin.X);
            float mapy = ((float)e.Location.Y / ZoomInv + (float)boxMin.Y);

            if (e.Button == MouseButtons.Left)
            {
                RooSubSector oldSubSector   = selectedSubSector;
                RooSector oldSector         = selectedSector;

                selectedSubSector   = Room.GetSubSectorAt((int)mapx, (int)mapy);
                selectedSector      = (selectedSubSector != null) ? selectedSubSector.Sector : null;
                
                if (oldSubSector != selectedSubSector || 
                    oldSector != selectedSector)
                {
                    Invalidate();

                    if (oldSector != selectedSector && SelectedSectorChanged != null)
                        SelectedSectorChanged(this, new EventArgs());
                    
                    if (oldSubSector != selectedSubSector && SelectedSubSectorChanged != null)
                        SelectedSubSectorChanged(this, new EventArgs());
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            const float STEP = 32.0f;

            switch(e.KeyChar)
            {
                case 'w': center.Y -= STEP * zoom; break;
                case 'a': center.X -= STEP * zoom; break;
                case 's': center.Y += STEP * zoom; break;
                case 'd': center.X += STEP * zoom; break;
            }

            Invalidate();
        }

        protected void OnVertexMismatchesCheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected void OnUseEditorWallsCheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected void OnVertHortLinesCheckedChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        public new void Invalidate()
        {
            float deltax = 0.5f * zoom * (float)Width;
            float deltay = 0.5f * zoom * (float)Height;
 
            // update box boundaries (box = what to draw from map)
            boxMin.X = center.X - deltax;
            boxMin.Y = center.Y - deltay;
            boxMax.X = center.X + deltax;
            boxMax.Y = center.Y + deltay;

            base.Invalidate();
        }

        protected void DrawElements(Graphics G)
        {
            Point[] points;

            if (room == null || G == null)
                return;

            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
            G.SmoothingMode = SmoothingMode.HighQuality;

            /**********************************************************************************/

            // fill selected sector
            if (selectedSector != null)
            {
                foreach (RooSubSector obj in Room.BSPTreeLeaves)
                {
                    if (obj.Sector != selectedSector)
                        continue;

                    points = new Point[obj.Vertices.Count];

                    for (int i = 0; i < obj.Vertices.Count; i++)
                    {
                        points[i].X = (int)(((float)obj.Vertices[i].X - boxMin.X) * ZoomInv);
                        points[i].Y = (int)(((float)obj.Vertices[i].Y - boxMin.Y) * ZoomInv);
                    }

                    G.FillPolygon(brushSolidGreen, points);
                }
            }

            /**********************************************************************************/

            // fill selected subsector
            if (selectedSubSector != null)
            {
                points = new Point[selectedSubSector.Vertices.Count];

                for (int i = 0; i < selectedSubSector.Vertices.Count; i++)
                {
                    points[i].X = (int)(((float)selectedSubSector.Vertices[i].X - boxMin.X) * ZoomInv);
                    points[i].Y = (int)(((float)selectedSubSector.Vertices[i].Y - boxMin.Y) * ZoomInv);
                }

                G.FillPolygon(brushSolidPaleGreen, points);               
            }

            // draw polys in non convex poly list from bspbuilder
            foreach (Polygon poly in bspBuilderNonConvexPolygons)
            {
                points = new Point[poly.Count];

                for (int i = 0; i < poly.Count; i++)
                {
                    points[i].X = (int)(((float)poly[i].X - boxMin.X) * ZoomInv);
                    points[i].Y = (int)(((float)poly[i].Y - boxMin.Y) * ZoomInv);

                    G.DrawRectangle(penWhite1, points[i].X - 1, points[i].Y - 1, 2, 2);
                }

                G.FillPolygon(brushSolidRed, points);
            }

            /**********************************************************************************/

            if (chkUseEditorWalls.Checked || selectedWallEditor != null)
            {
                // draw editor walls
                foreach (RooWallEditor rld in Room.WallsEditor)
                {
                    // convert to roowall
                    RooWall wall = rld.ToRooWall(RooFile.VERSIONHIGHRESGRID, Room);

                    // selected walleditor
                    if (selectedWallEditor == rld)
                        DrawWall(G, wall, penViolet3, false, penLightBlue1);

                    // normal wall only if not drawn by editorwall
                    else if (chkUseEditorWalls.Checked)
                        DrawWall(G, wall, penWhite1, false, penLightBlue1);
                }
            }

            // draw client walls
            foreach (RooWall rld in Room.Walls)
            {
                // selected wall
                if (selectedWall == rld)
                    DrawWall(G, rld, penRed2, false, penLightBlue1);

                // selected side
                else if (selectedSide != null && (rld.LeftSide == selectedSide || rld.RightSide == selectedSide))
                    DrawWall(G, rld, penOrange2, false, penLightBlue1);

                // selected partitionline
                else if (selectedPartitionLine != null && (rld == selectedPartitionLine.Wall))
                    DrawWall(G, rld, penBlue2, true, penLightBlue1);

                // default
                else if (!chkUseEditorWalls.Checked)
                    DrawWall(G, rld, penWhite1, false, penLightBlue1);
            }

            /**********************************************************************************/

            if (chkVertexMismatches.Checked)
            { 
                List<Tuple<RooSubSector, V2, float>> vertexmismatch;
                foreach(RooSubSector subsect in Room.BSPTreeLeaves)
                {                    
                    foreach(V2 vertex in subsect.Vertices)
                    {
                        vertexmismatch = Program.FindVertexMismatches(vertex);

                        if (vertexmismatch.Count > 0)
                        {
                            float transx1 = ((float)vertex.X - (float)boxMin.X) * ZoomInv;
                            float transy1 = ((float)vertex.Y - (float)boxMin.Y) * ZoomInv;

                            Pen pen = (vertexmismatch[0].Item3 == 2) ? penRed1 : penOrange1;

                            G.DrawRectangle(pen, transx1 - 5, transy1 - 5, 10, 10);                       
                        }
                    }
                }
            }
        }

        protected void DrawWall(Graphics G, RooWall Wall, Pen Pen, bool Infinite, Pen PenInfinite)
        {
            V2 p1p2;

            // transform points to match world of pixeldrawing
            float transx1 = (float)(Wall.P1.X - boxMin.X) * ZoomInv;
            float transy1 = (float)(Wall.P1.Y - boxMin.Y) * ZoomInv;
            float transx2 = (float)(Wall.P2.X - boxMin.X) * ZoomInv;
            float transy2 = (float)(Wall.P2.Y - boxMin.Y) * ZoomInv;

            // draw extensions of line
            if (Infinite)
            {
                V2 p1 = new V2(transx1, transy1);
                V2 p2 = new V2(transx2, transy2);
                p1p2 = p2 - p1;

                p2 += 1000f * p1p2;
                p1 -= 1000f * p1p2;

                G.DrawLine(PenInfinite, (float)p1.X, (float)p1.Y, (float)p2.X, (float)p2.Y);
            }

            // check if this is an issue line (almost horizontal or vertical, but not fully)
            p1p2 = Wall.GetP1P2();
            if (chkVertHortLines.Checked && p1p2.X != 0.0f && p1p2.Y != 0.0f)
            {
                Real m = (Real)(p1p2.Y / p1p2.X);

                if ((m > -0.125f && m < 0.125f) ||
                    (m > 8.0f || m < -8.0f))
                        Pen = penRed2;
            }
            
            // draw line
            G.DrawLine(Pen, transx1, transy1, transx2, transy2);           
        }

        protected void OnBSPBuilderFoundNonConvexPolygon(object sender, BSPBuilder.PolygonEventArgs e)
        {
            bspBuilderNonConvexPolygons.Add(e.Polygon);
            Invalidate();
        }

        protected void OnBSPBuilderBuildStarted(object sender, EventArgs e)
        {
            bspBuilderNonConvexPolygons.Clear();
        }

    }
}
