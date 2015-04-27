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

using Meridian59.Files.ROO;
using Meridian59.Common;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

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
        protected RooSideDef selectedSide;

        protected Pen penWhite1 = new Pen(Color.White, 1f);
        protected Pen penRed1 = new Pen(Color.Red, 1f);
        protected Pen penRed2 = new Pen(Color.Red, 2f);
        protected Pen penBlue1 = new Pen(Color.Blue, 1f);
        protected Pen penBlue2 = new Pen(Color.Blue, 2f);
        protected Pen penLightBlue1 = new Pen(Color.LightBlue, 1f);
        protected Pen penOrange1 = new Pen(Color.Orange, 1f);
        protected Pen penOrange2 = new Pen(Color.Orange, 2f);
        
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

                    Tuple<V3, V3> bBox = room.GetBoundingBox();
                    center.X = bBox.Item1.X + 0.5f * (bBox.Item2.X - bBox.Item1.X);
                    center.Y = bBox.Item1.Y + 0.5f * (bBox.Item2.Y - bBox.Item1.Y);

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


            // draw lines
            foreach (RooWall rld in Room.Walls)
            {                
                // expressions whether point is in rectangle
                bool isX1inScope = ((float)rld.X1 >= boxMin.X && (float)rld.X1 <= boxMax.X);
                bool isY1inScope = ((float)rld.Y1 >= boxMin.Y && (float)rld.Y1 <= boxMax.Y);
                bool isX2inScope = ((float)rld.X2 >= boxMin.X && (float)rld.X2 <= boxMax.X);
                bool isY2inScope = ((float)rld.Y2 >= boxMin.Y && (float)rld.Y2 <= boxMax.Y);

                // if at least one of the line points is in the mapscope, draw the line
                if ((isX1inScope && isY1inScope) || (isX2inScope && isY2inScope))
                {
                    // transform points to match world of pixeldrawing
                    float transx1 = ((float)rld.X1 - (float)boxMin.X) * ZoomInv;
                    float transy1 = ((float)rld.Y1 - (float)boxMin.Y) * ZoomInv;
                    float transx2 = ((float)rld.X2 - (float)boxMin.X) * ZoomInv;
                    float transy2 = ((float)rld.Y2 - (float)boxMin.Y) * ZoomInv;

                    Pen pen = penWhite1;

                    // selected wall
                    if (selectedWall == rld)
                    {
                        G.DrawLine(penRed2, transx1, transy1, transx2, transy2);
                    }
                    
                    // bsp splitter
                    else if (selectedPartitionLine != null &&
                            (rld == selectedPartitionLine.Wall))
                    {
                        V2 p1 = new V2(transx1, transy1);
                        V2 p2 = new V2(transx2, transy2);
                        V2 p1p2 = p2 - p1;
                        
                        p2 += 1000f * p1p2;
                        p1 -= 1000f * p1p2;

                        G.DrawLine(penLightBlue1, (float)p1.X, (float)p1.Y, (float)p2.X, (float)p2.Y);
                        G.DrawLine(penBlue2, transx1, transy1, transx2, transy2);                       
                    }

                    // selected side
                    else if (selectedSide != null &&
                            (rld.LeftSide == selectedSide || rld.RightSide == selectedSide))
                    {
                        G.DrawLine(penOrange2, transx1, transy1, transx2, transy2);
                    }

                    // normal wall
                    else
                    {
                        G.DrawLine(penWhite1, transx1, transy1, transx2, transy2); 
                    }                                          
                }
            }

            //
            if (chkVertexMismatches.Checked)
            { 
                List<Tuple<RooSubSector, RooVertex, int>> vertexmismatch;
                foreach(RooSubSector subsect in Room.BSPTreeLeaves)
                {                    
                    foreach(RooVertex vertex in subsect.Vertices)
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
    }
}
