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
        /// Returns true if two polygons are exactly identical.
        /// Note: The ordering/start of points also matters here.
        /// If two polygons contain the same points but indices are shifted,
        /// the return would be a false.
        /// </summary>
        /// <param name="Polygon"></param>
        /// <returns></returns>
        public bool IsIdentical(Polygon Polygon)
        {
            // no points
            if (Polygon.Count == 0 && Count == 0)
                return true;

            // not same amount of points
            if (Polygon.Count != Count)
                return false;

            // points must match exactly
            for (int i = 0; i < Count; i++)
                if (this[i] != Polygon[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Returns true if two polygons contain the
        /// same points. The ordering does not matter.
        /// Note: This can return true for two polygons,
        /// which use the same points, but have different shape
        /// due to different ordering of these points.
        /// </summary>
        /// <param name="Polygon"></param>
        /// <returns></returns>
        public bool HasSamePoints(Polygon Polygon)
        {
            // no points
            if (Polygon.Count == 0 && Count == 0)
                return true;

            // not same amount of points
            if (Polygon.Count != Count)
                return false;

            // point must be existant in both polys
            for (int i = 0; i < Count; i++)
                if (!this.Contains(Polygon[i]))
                    return false;

            return true;
        }

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

        /// <summary>
        /// Searches for the first edge the new Point lies on.
        /// If it's not a polygon vertex, it's added at according index.
        /// </summary>
        /// <param name="Point"></param>
        /// <returns>True if the point was added.</returns>
        public bool AddPointOnEdge(V2 Point)
        {
            // walk the poly edges
            for (int i = 0; i < Count - 1; i++)
            {
                if (Point.IsOnLineSegment(this[i], this[i + 1]) &&
                    Point != this[i] && Point != this[i+1])
                {
                    this.Insert(i + 1, Point);                   
                    return true;
                }
            }

            // on edge between last and first point
            if (Point.IsOnLineSegment(this[Count-1], this[0]) &&
                Point != this[Count-1] && Point != this[0])
            {
                this.Add(Point);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds points from this polygon starting at FromIndex up to including ToIndex
        /// into a Target Polygon. If FromIndex is bigger than ToIndex, it will iterate
        /// over the zero index (2 chunks: FromIndex->end, 0->ToIndex).
        /// </summary>
        /// <param name="FromIndex"></param>
        /// <param name="ToIndex"></param>
        /// <param name="Target"></param>
        protected void AddIndexRangeToOtherPolygon(int FromIndex, int ToIndex, Polygon Target)
        {
            // seamless range
            if (FromIndex < ToIndex)
            {
                for (int i = FromIndex; i <= ToIndex; i++)
                    Target.Add(this[i]);
            }
        
            // splitted range
            else if (FromIndex > ToIndex)
            {
                // from last to end
                for (int i = FromIndex; i < Count; i++)
                    Target.Add(this[i]);

                // from 0 to first
                for (int i = 0; i <= ToIndex; i++)
                    Target.Add(this[i]);
            }
        }

        /// <summary>
        /// Splits this polygon using a infinite line given by P1 (start) and P2 (end).
        /// Will check for convexity first.
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns>
        /// Null in case the polygon is not convex or something went wrong.
        /// A tuple with Item1 (right) and Item2 (left) polygon.
        /// </returns>
        public Tuple<Polygon, Polygon> SplitConvexPolygon(V2 P1, V2 P2)
        {
            // make sure this is a convex polygon
            if (!IsConvexPolygon())
                return null;
            
            /***************************************************************/

            Polygon poly;
            V2 intersect;
            LineInfiniteLineIntersectionType intersecttype;
            List<V2> intersections  = new List<V2>();
            List<V2> boundarypoints = new List<V2>();
            int numcoincides      = 0;
            int side;

            // create left and right polygons
            Polygon polyRight = new Polygon();
            Polygon polyLeft = new Polygon();

            /***************************************************************/

            // test intersection of all polygon edges (finite lines) with infinite line p1p2
            for (int i = 0; i < Count - 1; i++)
            {
                intersecttype = MathUtil.IntersectLineInfiniteLine(this[i], this[i + 1], P1, P2, out intersect);

                // intersection
                if (intersecttype == LineInfiniteLineIntersectionType.OneIntersection &&
                    !intersections.Contains(intersect))               
                        intersections.Add(intersect);
                
                // boundarypoint
                // note: in case we tried to add a boundarypoint which is already there
                // we in fact have a polygon vertex intersection, could add it there and change handling..
                else if (intersecttype == LineInfiniteLineIntersectionType.OneBoundaryPoint &&
                    !boundarypoints.Contains(intersect))               
                        boundarypoints.Add(intersect);
                
                // count coincides
                else if (intersecttype == LineInfiniteLineIntersectionType.FullyCoincide)
                    numcoincides++;
            }

            // also test the edge from last point to first point
            intersecttype = MathUtil.IntersectLineInfiniteLine(this[Count-1], this[0], P1, P2, out intersect);
            if (intersecttype == LineInfiniteLineIntersectionType.OneIntersection &&
                !intersections.Contains(intersect))
                    intersections.Add(intersect);
            else if (intersecttype == LineInfiniteLineIntersectionType.OneBoundaryPoint &&
                !boundarypoints.Contains(intersect))
                    boundarypoints.Add(intersect);
            else if (intersecttype == LineInfiniteLineIntersectionType.FullyCoincide)
                numcoincides++;

            /***************************************************************/
          
            bool case1 = 
                (numcoincides == 0 && intersections.Count == 0 && boundarypoints.Count == 0) ||
                (numcoincides == 1 && intersections.Count == 0 && boundarypoints.Count == 2) ||
                (numcoincides == 0 && intersections.Count == 0 && boundarypoints.Count == 1);

            bool case2a = 
                (numcoincides == 0 && intersections.Count == 2 && boundarypoints.Count == 0);

            bool case2b =
                (numcoincides == 0 && intersections.Count == 0 && boundarypoints.Count == 2);

            bool case2c = 
                (numcoincides == 0 && intersections.Count == 1 && boundarypoints.Count == 1);

            // case (1): all on one side
            // (a) no intersection, no boundary point and no coincide edge (trivial)
            // (b) one edge is coincide with splitter
            // (c) exactly one polygon point is a boundarypoint on the splitter
            if (case1)
            {
                // determine side by finding first point not on splitter
                for (int i = 0; i < Count; i++)
                {
                    side = this[i].GetSide(P1, P2);

                    // add verything to right poly and return
                    if (side < 0)
                    {
                        polyRight.AddRange(this);
                        return new Tuple<Polygon, Polygon>(polyRight, polyLeft);
                    }

                    // add everything to left poly and return
                    else if (side > 0)
                    {
                        polyLeft.AddRange(this);
                        return new Tuple<Polygon, Polygon>(polyRight, polyLeft);
                    }
                }

                return null; // WTF?
            }

            // case (2)
            else if (case2a || case2b || case2c)
            {
                int idx  = -1;
                int idx1 = -1;
                int idx2 = -1;

                // case (2a): infinite line intersects two polygon edges
                // -> add both intersection vertices, then use them
                if (case2a)
                {
                    AddPointOnEdge(intersections[0]);
                    AddPointOnEdge(intersections[1]);
                    idx1 = IndexOf(intersections[0]);
                    idx2 = IndexOf(intersections[1]);
                }

                // case (2b): infinite line intersects two poly vertices
                // -> use them directly
                else if (case2b)
                {
                    idx1 = IndexOf(boundarypoints[0]);
                    idx2 = IndexOf(boundarypoints[1]);
                }

                // case (2c): infinite line intersects one polygon edge and one vertex
                else if (case2c)
                {
                    AddPointOnEdge(intersections[0]);
                    idx1 = IndexOf(intersections[0]);
                    idx2 = IndexOf(boundarypoints[0]);
                }

                if (idx1 < 0 || idx2 < 0 || idx1 == idx2)
                    return null; // WTF?

                // now add points to each poly

                // poly 1
                // use side of next point
                idx = (idx1 + 1 < Count) ? idx1 + 1 : 0;
                side = this[idx].GetSide(P1, P2);

                if (side < 0) poly = polyRight;
                else if (side > 0) poly = polyLeft;
                else
                    return null; // WTF?

                AddIndexRangeToOtherPolygon(idx1, idx2, poly);

                // poly 2
                // use side of next point
                idx = (idx2 + 1 < Count) ? idx2 + 1 : 0;
                side = this[idx].GetSide(P1, P2);

                if (side < 0) poly = polyRight;
                else if (side > 0) poly = polyLeft;
                else
                    return null; // WTF?

                AddIndexRangeToOtherPolygon(idx2, idx1, poly);

                // return both new polys
                return new Tuple<Polygon, Polygon>(polyRight, polyLeft);
            }
            else
                return null; // WTF?
        }
    }
}
