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
using System.Collections.Generic;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Polygon : List<V2>
    {
        /// <summary>
        /// True if the first three points of this polygon
        /// describe a valid triangle.
        /// </summary>
        /// <returns></returns>
        public bool IsPolygon()
        {
            return
                Count >= 3 &&
                !this[0].IsOnLineSegment(this[1], this[2]) &&
                !this[1].IsOnLineSegment(this[0], this[2]) &&
                !this[2].IsOnLineSegment(this[0], this[1]);
        }

        /// <summary>
        /// Checks if this polygon is convex.
        /// </summary>
        /// <remarks>
        /// http://stackoverflow.com/questions/471962/how-do-determine-if-a-polygon-is-complex-convex-nonconvex
        /// </remarks>
        /// <returns></returns>
        public bool IsConvexPolygon()
        {
            V2 v1, v2;
            int i, val, lastval;

            // will make sure there is at least a valid triangle
            if (!IsPolygon())
                return false;

            // first
            v1 = this[1] - this[0];
            v2 = this[2] - this[1];
            lastval = Math.Sign(v1.CrossProduct(v2));

            // middle
            for (i = 1; i < Count - 2; i++)
            {
                v1 = this[i + 1] - this[i];
                v2 = this[i + 2] - this[i + 1];
                val = Math.Sign(v1.CrossProduct(v2));

                if (val != lastval)
                    return false;

                lastval = val;
            }

            // last with first
            v1 = this[0] - this[Count-1];
            v2 = this[1] - this[0];
            val = Math.Sign(v1.CrossProduct(v2));

            if (val != lastval)
                return false;

            return true;
        }

        /// <summary>
        /// Removes points from the list, which share coordinates with their successor/predecessor
        /// </summary>
        /// <returns>Amount of removed points</returns>
        public int RemoveZeroEdges()
        {
            int removed = 0;

            // backward due to removing items
            for(int i = Count - 1; i >= 1; i--)
            {
                if (this[i] == this[i - 1])
                { 
                    RemoveAt(i);
                    removed++;
                }
            }

            return removed;
        }

        /*public Tuple<Polygon, Polygon> SplitConvexPolygon(V2 P1, V2 P2)
        {
            // make sure this is a convex polygon
            if (!IsConvexPolygon())
                return null;

            V2 intersect;
            LineInfiniteLineIntersectionType intersecttype;
            List<V2> intersections  = new List<V2>();
            List<V2> boundarypoints = new List<V2>();
            int numcoincides      = 0;
            
            // create left and right polygons
            Polygon polyRight = new Polygon();
            Polygon polyLeft = new Polygon();
            
            Tuple<Polygon, Polygon> splitted = 
                new Tuple<Polygon, Polygon>(polyRight, polyLeft);

            // test intersection of all polygon edges (finite lines) with infinite line p1p2
            for (int i = 0; i < Count - 1; i++)
            {
                intersecttype = MathUtil.IntersectLineInfiniteLine(this[i], this[i + 1], P1, P2, out intersect);

                // count intersection
                if (intersecttype == LineInfiniteLineIntersectionType.OneIntersection)
                    intersections.Add(intersect);

                // count boundarypoints (will be counted twice, for each of the two edges)
                else if (intersecttype == LineInfiniteLineIntersectionType.OneBoundaryPoint)
                    boundarypoints.Add(intersect);

                // count coincides
                else if (intersecttype == LineInfiniteLineIntersectionType.FullyCoincide)
                    numcoincides++;
            }

            // also test the edge from last point to first point
            intersecttype = MathUtil.IntersectLineInfiniteLine(this[Count-1], this[0], P1, P2, out intersect);
            if (intersecttype == LineInfiniteLineIntersectionType.OneIntersection)
                intersections.Add(intersect);
            else if (intersecttype == LineInfiniteLineIntersectionType.OneBoundaryPoint)
                boundarypoints.Add(intersect);
            else if (intersecttype == LineInfiniteLineIntersectionType.FullyCoincide)
                numcoincides++;

            // case (1): all on one side
            // (a) no intersection, no boundary point and no coincide edge (trivial)
            // (b) one edge is coincide with splitter
            //     =two edge-endpoints also touch it, endpoints of coincide edge are not counted
            // (c) exactly one polygon point is a boundarypoint on the splitter (both edge endpoints counted
            if ((numcoincides == 0 && intersections.Count == 0 && boundarypoints.Count == 0) ||
                (numcoincides == 1 && intersections.Count == 0 && boundarypoints.Count == 2) ||
                (numcoincides == 0 && intersections.Count == 0 && boundarypoints.Count == 2))
            {
                // determine side by finding first point not on splitter
                for (int i = 0; i < Count - 1; i++)
                {
                    int side = this[i].GetSide(P1, P2);

                    // add verything to right poly and return
                    if (side > 0)
                    {
                        polyRight.AddRange(this);
                        return splitted;
                    }

                    // add everything to left poly and return
                    else if (side < 0)
                    {
                        polyLeft.AddRange(this);
                        return splitted;
                    }
                }
            }

            // case (2): infinite line intersects two polygon edges
            else if (numcoincides == 0 && intersections.Count == 2 && boundarypoints.Count == 0)
            {
                // split up
            }

            // case (3): infinite line intersects two polygon vertices (4 endpoints lie on splitter)
            else if (numcoincides == 0 && intersections.Count == 0 && boundarypoints.Count == 4)
            {
                // split up
            }

            // case (3): infinite line intersects one polygon edge and one vertex (2 endpoints there)
            else if (numcoincides == 0 && intersections.Count == 1 && boundarypoints.Count == 2)
            {
                // split up
            }

            // something wrong
            return null;
        }*/
    }
}
