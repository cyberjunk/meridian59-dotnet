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
using Meridian59.Common.Constants;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Common
{
    /// <summary>
    /// A three dimensional boundingbox described
    /// by a minimum and a maxmimum point.
    /// </summary>
    public struct BoundingBox3D
    {
        /// <summary>
        /// A box with both points zero.
        /// </summary>
        public static BoundingBox3D NULL { get { return new BoundingBox3D(V3.ZERO, V3.ZERO); } }

        /// <summary>
        /// The Minimum
        /// </summary>
        public V3 Min;

        /// <summary>
        /// The Maximum
        /// </summary>
        public V3 Max;

        /// <summary>
        /// Constructor using two initial points.
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        public BoundingBox3D(V3 P1, V3 P2)
        {
            // these initial values make sure
            // any first point will replace them
            this.Min.X = Real.MaxValue;
            this.Min.Y = Real.MaxValue;
            this.Min.Z = Real.MaxValue;
            this.Max.X = Real.MinValue;
            this.Max.Y = Real.MinValue;
            this.Max.Z = Real.MinValue;

            ExtendByPoint(P1);
            ExtendByPoint(P2);
        }

        /// <summary>
        /// Adjusts the current min, max accordingly in case
        /// the new point lies outside the current box.
        /// </summary>
        /// <param name="Point"></param>
        public void ExtendByPoint(V3 Point)
        {
            if (Point.X < Min.X) Min.X = Point.X;
            if (Point.X > Max.X) Max.X = Point.X;
            if (Point.Y < Min.Y) Min.Y = Point.Y;
            if (Point.Y > Max.Y) Max.Y = Point.Y;
            if (Point.Z < Min.Z) Min.Z = Point.Z;
            if (Point.Z > Max.Z) Max.Z = Point.Z;
        }

        /// <summary>
        /// Possibly extends the Min and Max by the
        /// Min and Max of parameter.
        /// </summary>
        /// <param name="Box"></param>
        public void ExtendByBoundingBox(BoundingBox3D Box)
        {
            ExtendByPoint(Box.Min);
            ExtendByPoint(Box.Max);
        }

        /// <summary>
        /// True if all components of Min are
        /// smaller than the components of Max.
        /// No valid box if both are the same (point) or
        /// have same value in a component (line).
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return (Min.X < Max.X && Min.Y < Max.Y && Min.Z < Max.Z);
        }

        /// <summary>
        /// Overridden equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is BoundingBox3D)
                return Equals((BoundingBox3D)obj);

            return false;
        }

        /// <summary>
        /// Typed equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(BoundingBox3D obj)
        {
            return (obj.Min == Min && obj.Max == Max);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool operator ==(BoundingBox3D box1, BoundingBox3D box2)
        {
            return box1.Equals(box2);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool operator !=(BoundingBox3D box1, BoundingBox3D box2)
        {
            return !box1.Equals(box2);
        }

        /// <summary>
        /// Overriden
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
