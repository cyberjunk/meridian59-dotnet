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

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Common
{
    /// <summary>
    /// A basic 2D vector struct (32/64-Bit FP)
    /// </summary>
    public struct V2
    {
        const Real EPSILON = 0.000001f;

        /// <summary>
        /// The zero vector (0,0)
        /// </summary>
        public static V2 ZERO { get { return new V2(0.0f, 0.0f); } }

        /// <summary>
        /// UNITX (1,0)
        /// </summary>
        public static V2 UNITX { get { return new V2(1.0f, 0.0f); } }

        /// <summary>
        /// UNITY (0,1)
        /// </summary>
        public static V2 UNITY { get { return new V2(0.0f, 1.0f); } }

        /// <summary>
        /// The X component
        /// </summary>
        public Real X;

        /// <summary>
        /// The Y component
        /// </summary>
        public Real Y;

        /// <summary>
        /// X as a property
        /// </summary>
        public Real XProp { get { return X; } set { X = value; } }

        /// <summary>
        /// Y as a property
        /// </summary>
        public Real YProp { get { return Y; } set { Y = value; } }

        /// <summary>
        /// The length of the vector. Calculated on the fly.
        /// </summary>
        public Real Length
        {
            get { return (Real)Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// The squared length of the vector. Calcalulated on the fly.
        /// </summary>
        public Real LengthSquared
        {
            get { return X * X + Y * Y; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public V2(Real X, Real Y)
        {
            this.X = X;
            this.Y = Y;
        }

        #region Operators
        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static V2 operator +(V2 v1, V2 v2)
        {
            return new V2(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static V2 operator -(V2 v1, V2 v2)
        {
            return new V2(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static V2 operator -(V2 v1)
        {
            v1.X = -v1.X;
            v1.Y = -v1.Y;

            return v1;
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Real operator *(V2 v1, V2 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static V2 operator *(V2 v1, Real scalar)
        {
            return new V2(v1.X * scalar, v1.Y * scalar);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static V2 operator *(Real scalar, V2 v1)
        {
            return new V2(v1.X * scalar, v1.Y * scalar);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(V2 v1, V2 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(V2 v1, V2 v2)
        {
            return !v1.Equals(v2);
        }
        #endregion

        /// <summary>
        /// Overridden equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is V2)
                return Equals((V2)obj);

            return false;
        }

        /// <summary>
        /// Typed equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(V2 obj)
        {
            return 
                Math.Abs(obj.X-X) <= EPSILON && 
                Math.Abs(obj.Y-Y) <= EPSILON;
        }

        /// <summary>
        /// Overriden
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // to do ?

            return base.GetHashCode();
        }

        /// <summary>
        /// Create a new instance with same values
        /// </summary>
        /// <returns></returns>
        public V2 Clone()
        {
            return new V2(X, Y);
        }

        /// <summary>
        /// Returns a string like (0,0)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return '(' + X.ToString() + '/' + Y.ToString() + ')';
        }

        /// <summary>
        /// Scales the vector by scalar
        /// </summary>
        /// <param name="scalar"></param>
        public void Scale(Real scalar)
        {
            X *= scalar;
            Y *= scalar;          
        }

        /// <summary>
        /// Scales this V2 instance to given length
        /// </summary>
        /// <param name="newlength"></param>
        public void ScaleToLength(Real newlength)
        {
            Real len = Length;

            if (len > 0.0f)
            {
                Real quot = newlength / len;
                X *= quot;
                Y *= quot;
            }
        }

        /// <summary>
        /// Normalizes this V2 instance (length=1.0)
        /// </summary>
        public void Normalize()
        {
            ScaleToLength(1.0f);
        }

        /// <summary>
        /// Rotates this V2 instance anti-clockwise by given radian.
        /// </summary>
        /// <example>
        /// Rotating the unit-x (1, 0) vector by 90° (PI/2)
        /// creates the unit-y (0, 1) vector .
        /// </example>
        /// <param name="Radian">Rotate by this value.</param>
        public void Rotate(Real Radian)
        {
            Real cs = (Real)Math.Cos(Radian);
            Real sn = (Real)Math.Sin(Radian);
            Real px = X;
            Real py = Y;

            X = px * cs - py * sn;
            Y = px * sn + py * cs;
        }

        /// <summary>
        /// Returns the distance to another point
        /// </summary>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public Real DistanceTo(V2 Destination)
        {
            V2 diff = Destination - this;

            return diff.Length;
        }

        /// <summary>
        /// Returns the squared distance to another point
        /// </summary>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public Real DistanceToSquared(V2 Destination)
        {
            V2 diff = Destination - this;

            return diff.LengthSquared;
        }

        /// <summary>
        /// Returns the minimum distance of this instance to a
        /// line segment given by points P1, P2.
        /// Note: line segment != infinite line
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public Real MinDistanceToLineSegment(V2 P1, V2 P2)
        {
            V2 diff = P2 - P1;
            Real l2 = diff.LengthSquared;

            if (l2 == 0.0f) 
                return DistanceTo(P1);

            Real t = ((this - P1) * (P2 - P1)) / l2;

            if (t < 0.0f)
                return DistanceTo(P1);

            else if (t > 1.0f)
                return DistanceTo(P2);

            diff.Scale(t);
            V2 projection = P1 + diff;

            return DistanceTo(projection);
        }

        /// <summary>
        /// Returns the minimum squared distance of this instance to a
        /// line segment given by points P1, P2.
        /// Note: line segment != infinite line
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public Real MinSquaredDistanceToLineSegment(V2 P1, V2 P2)
        {
            V2 diff = P2 - P1;
            Real l2 = diff.LengthSquared;

            if (l2 == 0.0f)
                return DistanceToSquared(P1);

            Real t = ((this - P1) * (P2 - P1)) / l2;

            if (t < 0.0f)
                return DistanceToSquared(P1);

            else if (t > 1.0f)
                return DistanceToSquared(P2);

            diff.Scale(t);
            V2 projection = P1 + diff;

            return DistanceToSquared(projection);
        }

        /// <summary>
        /// Returns the side (-1 or 1) of this (point) instance
        /// for the line given by points P1 and P2.
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public int GetSide(V2 P1, V2 P2)
        {
            Real val = (P2.X - P1.X) * (Y - P1.Y) - (P2.Y - P1.Y) * (X - P1.X);

            if (val >= EPSILON)
                return 1;

            else if (val <= -EPSILON)
               return -1;

            else
                return 0;
        }

        /// <summary>
        /// Tests whether this V2 instance lies on a line segment
        /// given by points P1 and P2.
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public bool IsOnLineSegment(V2 P1, V2 P2)
        {
            // the point is not even on the infinite line given by P1P2
            if (GetSide(P1, P2) != 0)
                return false;

            // if on infinite line, must also be in boundingbox
            V2 min, max;
            min.X = Math.Min(P1.X, P2.X);
            min.Y = Math.Min(P1.Y, P2.Y);
            max.X = Math.Max(P1.X, P2.X);
            max.Y = Math.Max(P1.Y, P2.Y);

            return
                min.X <= X && X <= max.X &&
                min.Y <= Y && Y <= max.Y;
        }

        /// <summary>
        /// Returns a projection of this instance on another vector.
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        public V2 GetProjection(V2 Vector)
        {
            Real denom = Vector.LengthSquared;

            if (denom > 0.0f)
            {
                Real num = this * Vector;
                return Vector * (num / denom);
            }
            else
                return V2.ZERO;
        }

        /// <summary>
        /// Returns a vector with values (y, -x)
        /// </summary>
        /// <returns></returns>
        public V2 GetPerpendicular1()
        {
            return new V2(Y, -X);
        }

        /// <summary>
        /// Returns a vector with values (-y, x)
        /// </summary>
        /// <returns></returns>
        public V2 GetPerpendicular2()
        {
            return new V2(-Y, X);
        }

        /// <summary>
        /// Returns the scalar crossproduct of this vector
        /// and the parameter vector.
        /// </summary>
        /// <remarks>
        /// http://stackoverflow.com/questions/243945/calculating-a-2d-vectors-cross-product
        /// </remarks>
        /// <param name="v"></param>
        /// <returns></returns>
        public Real CrossProduct(V2 v)
        {
            return (X * v.Y) - (Y * v.X);
        }

        /// <summary>
        /// Multiplies components by 16 and subtracts an 1024 offset
        /// </summary>
        public void ConvertToROO()
        {
            X = X * 16f - 1024f;
            Y = Y * 16f - 1024f;
        }

        /// <summary>
        /// Divides components by 16 and adds an 64 offset
        /// </summary>
        public void ConvertToWorld()
        {
            X = X / 16f + 64f;
            Y = Y / 16f + 64f;
        }
    }
}
