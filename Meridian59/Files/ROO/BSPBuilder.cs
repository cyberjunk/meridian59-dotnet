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
using Meridian59.Common;
using System.Collections.Generic;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    public static class BSPBuilder
    {
        private static RooFile room;

        public class PolygonEventArgs : EventArgs
        {
            public Polygon Polygon;

            public PolygonEventArgs(Polygon Polygon)
            {
                this.Polygon = Polygon;
            }
        }

        public static EventHandler<PolygonEventArgs> FoundNonConvexPolygon;

        public static EventHandler BuildStarted;

        public static void Build(RooFile Room)
        {
            if (Room == null)
                return;

            room = Room;

            if (BuildStarted != null)
                BuildStarted(null, new EventArgs());

            ///////////////////////////////////////////////////////////////

            BoundingBox2D box = Room.GetBoundingBox2D(true);
            
            Polygon poly = new Polygon();           
            poly.Add(box.Min);
            poly.Add(box.Min + new V2(box.Max.X - box.Min.X, 0f));
            poly.Add(box.Max);
            poly.Add(box.Max - new V2(box.Max.X - box.Min.X, 0f));
            
            ///////////////////////////////////////////////////////////////

            // clean up old data from room
            Room.Walls.Clear();
            Room.BSPTree.Clear();
            foreach (RooSector sector in Room.Sectors)
            {
                sector.Walls.Clear();
                sector.Sides.Clear();
            }

            // convert roomeditor walls to roowall
            for (int i = 0; i < Room.WallsEditor.Count; i++)
            {
                RooWall wall = Room.WallsEditor[i].ToRooWall(RooFile.VERSIONHIGHRESGRID, Room);
                Room.Walls.Add(wall);
            }

            ///////////////////////////////////////////////////////////////

            RooBSPItem tree = BuildNode(Room.Walls, poly, 0);

            ///////////////////////////////////////////////////////////////

            FillNode(tree, Room.BSPTree);
            SetNums(Room.BSPTree);
        }

        /// <summary>
        /// Returns the A, B, C coefficients of the general 2D line equation: Ax+By+C=0
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        public static void GetLineEquation2DCoefficients(V2 P1, V2 P2, out Real A, out Real B, out Real C)
        {
            A = P2.Y - P1.Y;
            B = P1.X - P2.X;

            // if the float values represent full integers
            // try to reduce the fraction
            int intdy = (int)A;
            int intdx = (int)B;
            if (A == (Real)intdy && B == (Real)intdx)
            {
                int gcd = MathUtil.GCD(intdx, intdy);

                if (gcd != 0)
                {
                    A /= (Real)gcd;
                    B /= (Real)gcd;
                }
            }

            C = A * P1.X + B * P1.Y;
        }

        private static RooWall ChooseSplitter(IEnumerable<RooWall> Walls)
        {
            RooWall best_splitter = null;
            int best_count = -1;      // Minimum # of walls in bigger side of best wall
            int best_splits = 999999; // # of splits for best root so far
            int max_count;
            int side0, side1;

            foreach (RooWall splitter in Walls)
            {
                int pos = 0;
                int neg = 0;
                int splits = 0;

                foreach (RooWall wall in Walls)
                {                  
                    side0 = wall.P1.GetSide(splitter.P1, splitter.P2);
                    side1 = wall.P2.GetSide(splitter.P1, splitter.P2);

                    // If both on same side, or one is on line, no split needed
                    if (side0 * side1 >= 0)
                    {
                        // In plane of root?
                        if (side0 == 0 && side1 == 0)
                            continue;

                        // On + side of root?
                        if (side0 > 0 || side1 > 0)
                        {
                            pos++;
                            continue;
                        }

                        // On - side of root
                        neg++;
                        continue;
                    }

                    // Split--one on each side
	                pos++;
	                neg++;
	                splits++;
                }

                max_count = Math.Max(pos, neg);

                if (max_count < best_count || 
                    (max_count == best_count && splits < best_splits) ||
	                best_count == -1)
                {
	                best_count = max_count;
	                best_splitter = splitter;
	                best_splits = splits;
                }
            }

            return best_splitter;
        }

        private static Tuple<List<RooWall>, List<RooWall>> SplitWalls(List<RooWall> Walls, RooWall Splitter)
        {
            int side0, side1;
            List<RooWall> wallsRight = new List<RooWall>();
            List<RooWall> wallsLeft  = new List<RooWall>();
            
            // backwards due to removing items
            for(int i = Walls.Count - 1 ; i >= 0; i--)
            {
                RooWall wall = Walls[i];

                if (wall == Splitter)
                    continue;

                side0 = wall.P1.GetSide(Splitter.P1, Splitter.P2);
                side1 = wall.P2.GetSide(Splitter.P1, Splitter.P2);

                // coincide =
                // both endpoints on wall
                if (side0 == 0 && side1 == 0)
                {
                    // attach at end of linked list if not already included
                    bool add = true;
                    RooWall next = Splitter;
                    while (next.NextWallInPlane != null) 
                    {
                        // already in there
                        if (next.NextWallInPlane == wall)
                        {
                            add = false;
                            break;
                        }

                        next = next.NextWallInPlane;
                    }

                    if (add)
                        next.NextWallInPlane = wall;
                    
                    continue;
                }
                
                // on right side =
                // both endpoints right side, or one right, one on line
                if (side0 <= 0 && side1 <= 0)
                {
                    wallsRight.Add(wall);
                    continue;
                }

                // on left side
                // both endpoints left side, or one left, one on line
                if (side0 >= 0 && side1 >= 0)
                {
                    wallsLeft.Add(wall);
                    continue;
                }

                // intersection - split the wall
                Tuple<RooWall, RooWall> splitWall = wall.Split(Splitter.P1, Splitter.P2);

                // remove old wall and add new chunks
                // second first so first will be first
                int idx = room.Walls.IndexOf(wall);
                room.Walls.RemoveAt(idx);
                room.Walls.Insert(idx, splitWall.Item2);
                room.Walls.Insert(idx, splitWall.Item1);

                if (splitWall != null)
                {
                    // if p1 was on right side and p2 on left side
                    // add the segment p1 to intersect to right walls
                    // add the segment intersect to p2 to left walls
                    if (side0 < 0 && side1 > 0)
                    {
                        wallsRight.Add(splitWall.Item1);
                        wallsLeft.Add(splitWall.Item2);
                    }
                    // other way round
                    else
                    {
                        wallsLeft.Add(splitWall.Item1);
                        wallsRight.Add(splitWall.Item2);
                    }
                }
                else
                    return null; // WTF?
            }

            return new Tuple<List<RooWall>, List<RooWall>>(wallsRight, wallsLeft);
        }

        private static RooBSPItem BuildNode(List<RooWall> Walls, Polygon Polygon, int Sector)
        {
            if (Polygon.Count == 0 || (Walls.Count == 0 && Sector == 0))
                return null;

            Polygon.RemoveZeroEdges();

            if (!Polygon.IsConvexPolygon())
            {
                if (FoundNonConvexPolygon != null)
                    FoundNonConvexPolygon(null, new PolygonEventArgs(Polygon));

                //return null; // WTF ?
            }

            // No walls left ==> leaf
            if (Walls.Count == 0)
            {
                RooSubSector leaf = new RooSubSector(RooFile.VERSIONHIGHRESGRID, (ushort)Sector, Polygon);

                // fills in sector reference
                leaf.ResolveIndices(room);
                
                return leaf;
            }

            // get best splitter of remaining walls
            RooWall splitter = ChooseSplitter(Walls);

            if (splitter == null)
                return null; // WTF ?

            // split up walls into right/left
            Tuple<List<RooWall>, List<RooWall>> splitWalls = 
                SplitWalls(Walls, splitter);
            
            // split up polygon into right/left
            Tuple<Polygon, Polygon> splitPolygons = 
                Polygon.SplitConvexPolygon(splitter.P1, splitter.P2);

            Real a, b, c;
            GetLineEquation2DCoefficients(splitter.P1, splitter.P2,
                out a, out b, out c);

            // create new splitter node
            RooPartitionLine node = new RooPartitionLine(RooFile.VERSIONHIGHRESGRID,
                Polygon.GetBoundingBox(), a, b, c, 0, 0, (ushort)splitter.Num);

            // fills in wall reference
            node.Wall = splitter;

            // recursively descend to children
            node.LeftChild = BuildNode(splitWalls.Item1, splitPolygons.Item1, splitter.LeftSectorNum);
            node.RightChild  = BuildNode(splitWalls.Item2, splitPolygons.Item2, splitter.RightSectorNum);

            return node;
        }

        private static void FillNode(RooBSPItem Node, List<RooBSPItem> NodeList)
        {
            if (Node == null)
                return;

            NodeList.Add(Node);
            
            if (Node.Type == RooBSPItem.NodeType.Node)
            {
                RooPartitionLine node = (RooPartitionLine)Node;

                FillNode(node.RightChild, NodeList);
                FillNode(node.LeftChild, NodeList);
            }
        }

        private static void SetNums(List<RooBSPItem> Nodes)
        {
            int idx;
            RooPartitionLine splitter;

            // set wall nums
            for (int i = 0; i < room.Walls.Count; i++)
            {
                RooWall wall = room.Walls[i];
                wall.Num = i + 1;

                if (wall.NextWallInPlane != null)
                {
                    // set 1-based num (0=unset)
                    idx = room.Walls.IndexOf(wall.NextWallInPlane);

                    wall.NextWallNumInPlane = (short)((idx >= 0) ? idx + 1 : 0);
                }
            }

            foreach(RooBSPItem node in Nodes)
            {
                if (node.Type == RooBSPItem.NodeType.Node)
                {
                    splitter = (RooPartitionLine)node;

                    if (splitter.RightChild != null)
                    {
                        idx = Nodes.IndexOf(splitter.RightChild);

                        // set 1-based num (0=unset)
                        splitter.Right = (ushort)((idx >= 0) ? idx + 1 : 0);
                    }

                    if (splitter.LeftChild != null)
                    {
                        idx = Nodes.IndexOf(splitter.LeftChild);

                        // set 1-based num (0=unset)
                        splitter.Left = (ushort)((idx >= 0) ? idx + 1 : 0);
                    }

                    if (splitter.Wall != null)
                    {
                        idx = room.Walls.IndexOf(splitter.Wall);

                        // set 1-based num (0=unset)
                        splitter.WallReference = (ushort)((idx >= 0) ? idx + 1 : 0);
                    }
                }
            }  
        }
    }
}
