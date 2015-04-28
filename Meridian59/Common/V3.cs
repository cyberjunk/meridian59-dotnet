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

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Common
{
    /// <summary>
    /// A basic 3D vector struct (32/64-Bit FP)
    /// </summary>
    public struct V3
    {
        /// <summary>
        /// The zero vector (0,0,0)
        /// </summary>
        public static V3 ZERO { get { return new V3(0.0f, 0.0f, 0.0f); } }
        
        /// <summary>
        /// UNITX (1,0,0)
        /// </summary>
        public static V3 UNITX { get { return new V3(1.0f, 0.0f, 0.0f); } }

        /// <summary>
        /// UNITY (0,1,0)
        /// </summary>
        public static V3 UNITY { get { return new V3(0.0f, 1.0f, 0.0f); } }

        /// <summary>
        /// UNITZ (0,0,1)
        /// </summary>
        public static V3 UNITZ { get { return new V3(0.0f, 0.0f, 1.0f); } }
        
        /// <summary>
        /// The X component
        /// </summary>
        public Real X;

        /// <summary>
        /// The Y component
        /// </summary>
        public Real Y;

        /// <summary>
        /// The Z component
        /// </summary>
        public Real Z;

        /// <summary>
        /// X as a property
        /// </summary>
        public Real XProp { get { return X; } set { X = value; } }

        /// <summary>
        /// Y as a property
        /// </summary>
        public Real YProp { get { return Y; } set { Y = value; } }

        /// <summary>
        /// Z as a property
        /// </summary>
        public Real ZProp { get { return Z; } set { Z = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public V3(Real X, Real Y, Real Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        #region Operators
        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static V3 operator + (V3 v1, V3 v2)
        {
            return new V3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static V3 operator - (V3 v1, V3 v2)
        {
            return new V3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static V3 operator - (V3 v1)
        {
            v1.X = -v1.X;
            v1.Y = -v1.Y;
            v1.Z = -v1.Z;

            return v1;
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Real operator *(V3 v1, V3 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z); 
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static V3 operator *(V3 v1, Real scalar)
        {
            return new V3(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static V3 operator *(Real scalar, V3 v1)
        {
            return new V3(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(V3 v1, V3 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Implemented operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(V3 v1, V3 v2)
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
            if (obj is V3)
                return Equals((V3)obj);

            return false;
        }

        /// <summary>
        /// Typed equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(V3 obj)
        {
            return (obj.X == X && obj.Y == Y && obj.Z == Z);
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
        public V3 Clone()
        {
            return new V3(X, Y, Z);
        }

        /// <summary>
        /// Returns the length of the vector
        /// </summary>
        public Real Length
        {
            get { return (Real)Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// Returns the squared length of the vector
        /// </summary>
        public Real LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        /// <summary>
        /// Returns the crossproduct of this
        /// and another V3 vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public V3 CrossProduct(V3 v)
        {
            return new V3(
                Y * v.Z - Z * v.Y,
                Z * v.X - X * v.Z,
                X * v.Y - Y * v.X);
        }

        /// <summary>
        /// Returns a string like (0,0,0)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return '(' + X.ToString() + '/' + Y.ToString() + '/' + Z.ToString() + ')'; 
        }

        /// <summary>
        /// Scales the vector by scalar
        /// </summary>
        /// <param name="scalar"></param>
        public void Scale(Real scalar)
        {
            X = X * scalar;
            Y = Y * scalar;
            Z = Z * scalar;
        }

        /// <summary>
        /// Scales the vector to given length
        /// </summary>
        /// <param name="newlength"></param>
        public void ScaleToLength(Real newlength)
        {
            Real len = Length;
            
            if (len > 0)
            {
                Real quot = newlength / len;
                X *= quot;
                Y *= quot;
                Z *= quot;
            }
        }

        /// <summary>
        /// Scales the vector to length 1.0
        /// </summary>
        public void Normalize()
        {
            ScaleToLength(1.0f);
        }

        /// <summary>
        /// Multiplies components by 16 and subtracts an 1024 offset
        /// </summary>
        public void ConvertToROO()
        {
            X = X * 16f - 1024f;
            Y = Y * 16f;
            Z = Z * 16f - 1024f;
        }

        /// <summary>
        /// Divides components by 16 and adds an 64 offset
        /// </summary>
        public void ConvertToWorld()
        {
            X = X / 16f + 64f;
            Y = Y / 16f;
            Z = Z / 16f + 64f;
        }
    }
}
