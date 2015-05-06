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

using Meridian59.Common.Constants;
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
    /// Different cases two finite line segments can intersect.
    /// </summary>
    /// <remarks>
    /// NoIntersection: No intersection or boundary point
    /// OneIntersection: Exactly one intersection
    /// OneBoundaryPoint: Exactly one boundary point
    /// FullyCoincide: Finite lines are exactly equal
    /// PartiallyCoincide: Finite lines partially overlap
    /// </remarks>
    public enum LineLineIntersectionType
    {
        NoIntersection, OneIntersection, OneBoundaryPoint, FullyCoincide, PartiallyCoincide
    }

    /// <summary>
    /// Different cases a finite line segment can intersect with an infinite line.
    /// </summary>
    /// <remarks>
    /// NoIntersection: No intersection or boundary point
    /// OneIntersection: Exactly one intersection
    /// OneBoundaryPoint: Exactly one boundary point
    /// FullyCoincide: Finite line segment is included in infinite line
    /// </remarks>
    public enum LineInfiniteLineIntersectionType
    {
        NoIntersection, OneIntersection, OneBoundaryPoint, FullyCoincide
    }

    /// <summary>
    /// Contains some static math util functions
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// Random generator
        /// </summary>
        public static readonly Random Random = new Random();

        /// <summary>
        /// Converts a M59 FixedPoint value (8 fract bits) to a float
        /// </summary>
        /// <param name="FPValue"></param>
        /// <returns></returns>
        public static Real M59FPToFloat(int FPValue)
        {
            return (Real)FPValue / (Real)(1 << 8);
        }

        /// <summary>
        /// Converts a float to a M59 FixedPoint value (8 fract bits)
        /// </summary>
        /// <param name="FloatValue"></param>
        /// <returns></returns>
        public static int FloatToM59FP(Real FloatValue)
        {
            return (int)Math.Round(FloatValue * (Real)(1 << 8));
        }

        /// <summary>
        /// Converts angle units to radian.
        /// </summary>
        /// <param name="Value">Angle units (0 - 4096)</param>
        /// <returns>Radian (0 - 2PI)</returns>
        public static Real BinaryAngleToRadian(ushort Value)
        {
            return (Real)Value * GeometryConstants.M59ANGLETORADQUOT;
        }

        /// <summary>
        /// Converts radian to angle units.
        /// </summary>
        /// <param name="Value">Radian (0 - 2PI)</param>
        /// <returns>Angle units (0 - 4096)</returns>
        public static ushort RadianToBinaryAngle(Real Value)
        {
            return Convert.ToUInt16(Value * GeometryConstants.RADTOM59ANGLEQUOT);
        }

        /// <summary>
        /// Returns next greater power of 2
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static uint NextPowerOf2(uint n)
        {
            n--;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            n++;

            return n;
        }

        /// <summary>
        /// Returns next greater multiple of 4 of n,
        /// or n, if n is a multiple of 4.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static uint NextMultipleOf4(uint n)
        {
            return (n + 3U) & ~0x03U;
        }

        /// <summary>
        /// Returns the maximum of two floats.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Real Max(Real a, Real b)
        {
            return (a > b) ? a : b;
        }

        /// <summary>
        /// Returns the minimum of two floats.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Real Min(Real a, Real b)
        {
            return (a < b) ? a : b;
        }

        /// <summary>
        /// Returns the Greatest Common Divisor
        /// </summary>
        /// <param name="a">Number a</param>
        /// <param name="b">Number b</param>
        /// <returns></returns>
        public static long GCD(long a, long b)
        {
            long tmp;
            while (b != 0)
            {
                tmp = b;
                b = a % b;
                a = tmp;
            }

            return a;
        }

        /// <summary>
        /// Returns the Greatest Common Divisor
        /// </summary>
        /// <param name="a">Number a</param>
        /// <param name="b">Number b</param>
        /// <returns></returns>
        public static int GCD(int a, int b)
        {
            int tmp;
            while (b != 0)
            {
                tmp = b;
                b = a % b;
                a = tmp;
            }

            return a;
        }

        /// <summary>
        /// Returns the Least Common Multiple
        /// </summary>
        /// <param name="a">Number a</param>
        /// <param name="b">Number b</param>
        /// <returns></returns>
        public static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }

        /// <summary>
        /// Returns the Least Common Multiple
        /// </summary>
        /// <param name="a">Number a</param>
        /// <param name="b">Number b</param>
        /// <returns></returns>
        public static int LCM(int a, int b)
        {
            return (a * b) / GCD(a, b);
        }

        /// <summary>
        /// Returns a random point within a 2D triangle.
        /// </summary>
        /// <remarks>
        /// See:
        /// http://math.stackexchange.com/questions/18686/uniform-random-point-in-triangle
        /// </remarks>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns>Random point</returns>
        public static V2 RandomPointInTriangle(V2 A, V2 B, V2 C)
        {
            // create two randoms within [0,1]
            Real rnd1 = (Real)Random.NextDouble();
            Real rnd2 = (Real)Random.NextDouble();

            // get rootsqrt
            Real sqrt_rnd1 = (Real)Math.Sqrt(rnd1);
            
            // coefficients
            Real coeff1 = 1 - sqrt_rnd1;
            Real coeff2 = sqrt_rnd1 * (1 - rnd2);
            Real coeff3 = rnd2 * sqrt_rnd1;

            // return random point
            return new V2(
                coeff1 * A.X + coeff2 * B.X + coeff3 * C.X,
                coeff1 * A.Y + coeff2 * B.Y + coeff3 * C.Y);
        }

        /// <summary>
        /// Returns the area of the 2D triangle.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static Real TriangleArea(V2 A, V2 B, V2 C)
        {
            V2 AB = B - A;
            V2 AC = C - A;

            return 0.5f * AB.CrossProduct(AC);
        }

        /// <summary>
        /// Checks a finite line segment for intersection with infinite line.
        /// </summary>
        /// <param name="P1">First point of finite line segment</param>
        /// <param name="P2">Second point of finite line segment</param>
        /// <param name="Q1">First point in infinte line</param>
        /// <param name="Q2">Second point in infinite line</param>
        /// <param name="Intersect"></param>
        /// <returns></returns>
        public static LineInfiniteLineIntersectionType IntersectLineInfiniteLine(V2 P1, V2 P2, V2 Q1, V2 Q2, out V2 Intersect)
        {
            Intersect.X = 0.0f;
            Intersect.Y = 0.0f;

            /*****************************************************************/

            int sideP1 = P1.GetSide(Q1, Q2);
            int sideP2 = P2.GetSide(Q1, Q2);

            // case (a): no intersection
            // both finite line endpoints are on the same side of the infinite line
            if (sideP1 == sideP2 && sideP1 != 0)
            {
                return LineInfiniteLineIntersectionType.NoIntersection;
            }

            // case (b): real intersection (not only touch)
            // both finite line endpoints are on different sides
            if (sideP1 != sideP2 && sideP1 != 0 && sideP2 != 0)
            {
                // variant 2
                Real denom = (P1.X - P2.X) * (Q1.Y - Q2.Y) - (P1.Y - P2.Y) * (Q1.X - Q2.X);                
                Real num;
                
                num = (P1.X * P2.Y - P1.Y * P2.X) * (Q1.X - Q2.X) - (P1.X - P2.X) * (Q1.X * Q2.Y - Q1.Y * Q2.X);                
                Intersect.X = num / denom;

                num = (P1.X * P2.Y - P1.Y * P2.X) * (Q1.Y - Q2.Y) - (P1.Y - P2.Y) * (Q1.X * Q2.Y - Q1.Y * Q2.X);
                Intersect.Y = num / denom;

                return LineInfiniteLineIntersectionType.OneIntersection;
            }

            // case (c): fully coincide
            // both finite line endpoints are on the infinite line
            if (sideP1 == 0 && sideP2 == 0)
            {
                Intersect.X = P1.X;
                Intersect.Y = P1.Y;

                return LineInfiniteLineIntersectionType.FullyCoincide;
            }
           
            // case (d):
            // finite line endpoint P1 touches infinite line
            if (sideP1 != sideP2 && sideP1 == 0)
            {
                Intersect.X = P1.X;
                Intersect.Y = P1.Y;

                return LineInfiniteLineIntersectionType.OneBoundaryPoint;
            }

            // case (d):
            // finite line endpoint P2 touches infinite line
            if (sideP1 != sideP2 && sideP2 == 0)
            {
                Intersect.X = P2.X;
                Intersect.Y = P2.Y;

                return LineInfiniteLineIntersectionType.OneBoundaryPoint;
            }

            // huh? something wrong?
            throw new Exception("Unknown intersection");
        }

        /// <summary>
        /// Checks two finite line segments for intersection.
        /// </summary>
        /// <param name="P1">First point of first line segment</param>
        /// <param name="P2">Second point of first line segment</param>
        /// <param name="Q1">First point of second line segment</param>
        /// <param name="Q2">Second point of second line segment</param>
        /// <param name="Intersect">Intersection point</param>
        /// <returns>LineLineIntersectionType</returns>
        public static LineLineIntersectionType IntersectLineLine(V2 P1, V2 P2, V2 Q1, V2 Q2, out V2 Intersect)
        {
            Intersect.X = 0.0f;
            Intersect.Y = 0.0f;

            /*****************************************************************/

            // fully coincide, lines are equal
            if ((P1 == Q1 && P2 == Q2) || (P1 == Q2 && P2 == Q1))
            {
                // use one point as intersection
                Intersect.X = P1.X;
                Intersect.Y = P1.Y;

                return LineLineIntersectionType.FullyCoincide;
            }

            /*****************************************************************/

            V2 b = P2 - P1;
            V2 d = Q2 - Q1;
            Real bDotDPerp = b.X * d.Y - b.Y * d.X;

            /******************************************************************
             *                 SPECIAL CASE: b dot d == 0                     *
             ******************************************************************/

            // three subcases:
            // (a) no intersect: parallel or on same infinite line but don't overlap
            // (b) partially coincide with many intersections
            // (c) touch each other at one endpoint
            if (bDotDPerp == 0.0f)
            {
                bool isP1onQ1Q2 = P1.IsOnLineSegment(Q1, Q2);
                bool isP2onQ1Q2 = P2.IsOnLineSegment(Q1, Q2);
                bool isQ1onP1P2 = Q1.IsOnLineSegment(P1, P2);
                bool isQ2onP1P2 = Q2.IsOnLineSegment(P1, P2);

                // subcase (a): p1p2 and q1q2 don't share a point
                if (!isP1onQ1Q2 && !isP2onQ1Q2 && !isQ1onP1P2 && !isQ2onP1P2)
                {
                    return LineLineIntersectionType.NoIntersection;
                }

                // subcase (b): P1P2 fully inside Q1Q2
                if (isP1onQ1Q2 && isP2onQ1Q2)
                {
                    // use p1 for intersection
                    Intersect.X = P1.X;
                    Intersect.Y = P1.Y;

                    return LineLineIntersectionType.PartiallyCoincide;
                }

                // subcase (b): Q1Q2 fully inside P1P2
                if (isQ1onP1P2 && isQ2onP1P2)
                {
                    // use p1 for intersection
                    Intersect.X = Q1.X;
                    Intersect.Y = Q1.Y;

                    return LineLineIntersectionType.PartiallyCoincide;
                }

                // subcase (c): touch at P1
                if (isP1onQ1Q2 && !isP2onQ1Q2)
                {
                    // use p1 for intersection
                    Intersect.X = P1.X;
                    Intersect.Y = P1.Y;

                    return LineLineIntersectionType.OneBoundaryPoint;
                }

                // subcase (c): touch at P2
                if (!isP1onQ1Q2 && isP2onQ1Q2)
                {
                    // use p1 for intersection
                    Intersect.X = P2.X;
                    Intersect.Y = P2.Y;

                    return LineLineIntersectionType.OneBoundaryPoint;
                }

                // subcase (c): touch at Q1
                if (isQ1onP1P2 && !isQ2onP1P2)
                {
                    // use p1 for intersection
                    Intersect.X = Q1.X;
                    Intersect.Y = Q1.Y;

                    return LineLineIntersectionType.OneBoundaryPoint;
                }

                // subcase (c): touch at Q2
                if (!isQ1onP1P2 && isQ2onP1P2)
                {
                    // use p1 for intersection
                    Intersect.X = Q2.X;
                    Intersect.Y = Q2.Y;

                    return LineLineIntersectionType.OneBoundaryPoint;
                }
            }

            /******************************************************************
             *             DEFAULT CASE: INFINITE LINES CROSS                 *
             ******************************************************************/
           
            V2 c = Q1 - P1;
            Real t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0.0f || t > 1.0f)
                return LineLineIntersectionType.NoIntersection;

            Real u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0.0f || u > 1.0f)
                return LineLineIntersectionType.NoIntersection;

            // -- finite line segments cross or boundary point! --

            b.Scale(t);
            Intersect = P1 + b;

            // if the intersection point is also the endpoint of
            // one of the finite lines, it's a boundary point
            if (Intersect == P1 || Intersect == P2 || Intersect == Q1 || Intersect == Q2)          
                return LineLineIntersectionType.OneBoundaryPoint;
            
            // true intersection
            return LineLineIntersectionType.OneIntersection;
        }

        /// <summary>
        /// Calculates the FIRST (entry) intersection point of a line and a circle.
        /// </summary>
        /// <remarks>
        /// http://stackoverflow.com/questions/1073336/circle-line-collision-detection
        /// </remarks>
        /// <param name="LineStart"></param>
        /// <param name="LineEnd"></param>
        /// <param name="CircleCenter"></param>
        /// <param name="Radius"></param>
        /// <param name="Intersect"></param>
        /// <returns>True if interesction, false if none.</returns>
        public static bool IntersectLineCircle(V2 LineStart, V2 LineEnd, V2 CircleCenter, Real Radius, out V2 Intersect)
        {
            // default output param
            Intersect.X = 0;
            Intersect.Y = 0;
            
            // returnval
            bool intersects = false;

            V2 d = LineEnd - LineStart;
            V2 f = LineStart - CircleCenter;

            Real a = d * d;
            Real b = 2 * (f * d);
            Real c = (f * f) - (Radius * Radius);
            Real discriminant = b * b - 4 * a * c;
            
            if (discriminant >= 0)
            {
                discriminant = (Real)Math.Sqrt(discriminant);

                Real t1 = (-b - discriminant) / (2 * a);
                Real t2 = (-b + discriminant) / (2 * a);

                if (t1 >= 0 && t1 <= 1)
                {
                    intersects = true;
                    Intersect = LineStart + (t1 * d);
                }
                else if (t2 >= 0 && t2 <= 1)
                {
                    intersects = true;
                    Intersect = LineStart + (t2 * d);
                }      
            }

            return intersects;
        }

        /// <summary>
        /// Converts radian angle to V2 direction
        /// </summary>
        /// <param name="Angle">Direction in angle 0-2pi</param>
        /// <returns>Direction vector with length 1</returns>
        public static V2 GetDirectionForRadian(Real Angle)
        {
            // start with unitx vector length 1
            V2 direction = V2.UNITX;

            // rotate by given angle
            direction.Rotate(Angle);

            return direction;
        }

        /// <summary>
        /// Converts V2 direction to radian angle.
        /// </summary>
        /// <param name="Direction">Direction vector</param>
        /// <returns>Value in [0..2PI]</returns>
        public static Real GetRadianForDirection(V2 Direction)
        {
            // normalize
            Direction.Normalize();

            // get angle between x-axis and direction in radian.
            Real cosphi = Direction * V2.UNITX;
            Real phi = (Real)Math.Acos(cosphi);

            // phi is always in [0..PI], not in [0..2PI]
            // this increases to full range [0..2PI]
            if (Direction.Y < 0)
                phi = GeometryConstants.TWOPI - phi;

            return phi;
        }

        /// <summary>
        /// Converts V2 direction to M59 angle.
        /// </summary>
        /// <example>
        ///    V2           M59
        /// -------------------------   
        /// (1,   0)      = 0
        /// (1,   1)      = 512
        /// (0,   1)      = 1024
        /// (-1,  1)      = 1536
        /// (-1,  0)      = 2048
        /// (-1, -1)      = 2560
        /// (0,  -1)      = 3072
        /// (1,  -1)      = 3584
        /// (1,  -0.0001) = 4096
        /// </example>
        /// <param name="Direction">Direction vector</param>        
        /// <returns>Value in [0..4096]</returns>
        public static ushort GetAngleForDirection(V2 Direction)
        {
            return RadianToBinaryAngle(
                GetRadianForDirection(Direction));
        }

        /// <summary>
        /// Gets a delta M59 angle based on positions and angle.
        /// </summary>
        /// <example>
        /// 
        /// Values:
        ///     T = (1, 1)
        ///     V = (4, 1)
        ///     A = 1024
        ///  => R = 1024 (90°)
        /// ------------------------
        ///     y
        ///     ^
        ///     |  ^   {R=90°}
        ///     |  | 
        ///   1 |  T--[TV]-->V
        ///     |
        ///     |-------------->x
        ///        1         4
        /// </example>
        /// <param name="V">Position 1</param>
        /// <param name="T">Position 2</param>
        /// <param name="Angle">Angle</param>
        /// <param name="GetSmallOne">Returns 2PI-ANGLE for ANGLE>PI, if set to true.</param>
        /// <returns>Value in [0..4096] or [0..2048], if GetSmallOne is true</returns>
        public static ushort GetAngle(V2 V, V2 T, ushort Angle, bool GetSmallOne = false)
        {
            // get vector from T to V
            V2 TV = V - T;

            // get angle for this deltavector as direction
            ushort objangle = GetAngleForDirection(TV);

            // the difference in angles
            int deltaangle = Angle - objangle;

            // if it's negative
            if (deltaangle < 0)
                deltaangle = GeometryConstants.MAXANGLE + deltaangle;

            // if we want the smaller one and it's bigger than 180°
            // use the difference to 360°
            if (GetSmallOne && deltaangle > GeometryConstants.HALFMAXANGLE)
                deltaangle = GeometryConstants.MAXANGLE - deltaangle;

            // return
            return (ushort)deltaangle;
        }
    }
}
