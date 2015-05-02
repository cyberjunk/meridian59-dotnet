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
        public class PolygonEventArgs : EventArgs
        {
            public Polygon Polygon;

            public PolygonEventArgs(Polygon Polygon)
            {
                this.Polygon = Polygon;
            }
        }

        public static EventHandler<PolygonEventArgs> FoundNonConvexPolygon;

        public static RooBSPItem Build(RooFile Room)
        {
            if (Room == null)
                return null;
            
            BoundingBox2D box = Room.GetBoundingBox2D();
            
            Polygon poly = new Polygon();           
            poly.Add(box.Min);
            poly.Add(box.Min + new V2(box.Max.X - box.Min.X, 0f));
            poly.Add(box.Max);
            poly.Add(box.Max - new V2(box.Max.X - box.Min.X, 0f));

            // convert roomeditor walls to roowall
            //List<RooWall> convertedWalls = new List<RooWall>();
            //foreach (RooWallEditor editorwall in Room.WallsEditor)
            //    convertedWalls.Add(editorwall.ToRooWall(Room));

            RooBSPItem tree = BuildNode(Room.Walls, poly, 0);

            Room.BSPTree.Clear();
            FillNode(tree, Room.BSPTree);

            return tree;
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

        private static Tuple<List<RooWall>, List<RooWall>> SplitWalls(IEnumerable<RooWall> Walls, RooWall Splitter)
        {
            int side0, side1;
            List<RooWall> wallsRight = new List<RooWall>();
            List<RooWall> wallsLeft  = new List<RooWall>();

            foreach(RooWall wall in Walls)
            {
                side0 = wall.P1.GetSide(Splitter.P1, Splitter.P2);
                side1 = wall.P2.GetSide(Splitter.P1, Splitter.P2);

                // coincide =
                // both endpoints on wall
                if (side0 == 0 && side1 == 0)
                    continue;

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
                return new RooSubSector((ushort)Sector, Polygon);

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
            MathUtil.GetLineEquation2DCoefficients(splitter.P1, splitter.P2,
                out a, out b, out c);

            // create new splitter node
            RooPartitionLine node = new RooPartitionLine(
                Polygon.GetBoundingBox(), (int)a, (int)b, (int)c, 0, 0, (ushort)splitter.Num);

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
    }
}
