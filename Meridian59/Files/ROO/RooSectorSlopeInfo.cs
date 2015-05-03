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
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    [Serializable]
    public class RooSectorSlopeInfo : IByteSerializableFast
    {
        private const int PAYLOADSIZE = 6;

        #region IByteSerializable implementation
        public int ByteLength 
        {
            get 
            { 
                return TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT
                    + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT
                    + (3 * PAYLOADSIZE);
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                Array.Copy(BitConverter.GetBytes(MathUtil.FloatToM59FP(A)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(MathUtil.FloatToM59FP(B)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(MathUtil.FloatToM59FP(C)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(MathUtil.FloatToM59FP(D)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;
            }
            else
            {
                Array.Copy(BitConverter.GetBytes((float)A), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)B), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)C), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)D), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;
            }

            Array.Copy(BitConverter.GetBytes(X0), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y0), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(TextureAngle), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // skip 3*6 unused bytes (vertex indices for roomeditor)
            cursor += (3 * PAYLOADSIZE);

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                *((int*)Buffer) = MathUtil.FloatToM59FP(A);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = MathUtil.FloatToM59FP(B);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = MathUtil.FloatToM59FP(C);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = MathUtil.FloatToM59FP(D);
                Buffer += TypeSizes.INT;
            }
            else
            {
                *((float*)Buffer) = (float)A;
                Buffer += TypeSizes.INT;

                *((float*)Buffer) = (float)B;
                Buffer += TypeSizes.INT;

                *((float*)Buffer) = (float)C;
                Buffer += TypeSizes.INT;

                *((float*)Buffer) = (float)D;
                Buffer += TypeSizes.INT;
            }

            *((int*)Buffer) = X0;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Y0;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = TextureAngle;
            Buffer += TypeSizes.INT;

            // skip 3*6 unused bytes (vertex indices for roomeditor)
            Buffer += (3 * PAYLOADSIZE);
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                A = MathUtil.M59FPToFloat(BitConverter.ToInt32(Buffer, cursor));
                cursor += TypeSizes.INT;

                B = MathUtil.M59FPToFloat(BitConverter.ToInt32(Buffer, cursor));
                cursor += TypeSizes.INT;

                C = MathUtil.M59FPToFloat(BitConverter.ToInt32(Buffer, cursor));
                cursor += TypeSizes.INT;

                D = MathUtil.M59FPToFloat(BitConverter.ToInt32(Buffer, cursor));
                cursor += TypeSizes.INT;
            }
            else
            {
                A = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                B = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                C = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                D = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;
            }

            X0 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y0 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            TextureAngle = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            // skip 3*6 unused bytes (vertex indices for roomeditor)
            cursor += (3 * PAYLOADSIZE);

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                A = MathUtil.M59FPToFloat(*((int*)Buffer));
                Buffer += TypeSizes.INT;

                B = MathUtil.M59FPToFloat(*((int*)Buffer));
                Buffer += TypeSizes.INT;

                C = MathUtil.M59FPToFloat(*((int*)Buffer));
                Buffer += TypeSizes.INT;

                D = MathUtil.M59FPToFloat(*((int*)Buffer));
                Buffer += TypeSizes.INT;
            }
            else
            {
                A = (Real)(*((float*)Buffer));
                Buffer += TypeSizes.FLOAT;

                B = (Real)(*((float*)Buffer));
                Buffer += TypeSizes.FLOAT;

                C = (Real)(*((float*)Buffer));
                Buffer += TypeSizes.FLOAT;

                D = (Real)(*((float*)Buffer));
                Buffer += TypeSizes.FLOAT;
            }

            X0 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y0 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            TextureAngle = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            // skip 3*6 unused bytes (vertex indices for roomeditor)
            Buffer += (3 * PAYLOADSIZE);
        }

        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
            set
            {
                ReadFrom(value);
            }
        }
        #endregion

        public uint RooVersion { get; set; }

        // plane equation constants (normal-vector variant)
        // (n1,n2,n3) * (x,y,z) - d = 0
        // Note: These values including X/Y are FixedPoint arithemtic, not int!
        // The fixed point precision is 8.
        public Real A { get; set; } // n1
        public Real B { get; set; } // n2
        public Real C { get; set; } // n3
        public Real D { get; set; } // d

        // x & y of texture origin
        public int X0 { get; set; }
        public int Y0 { get; set; }
        
        // this is planar angle between x axis of texture & x axis of world
        public int TextureAngle { get; set; }

        // calculated points
        public V3 P0 { get; set; } /* texture origin */
        public V3 P1 { get; set; } /* u axis end point */
        public V3 P2 { get; set; } /* v axis end point */
        public V3 TextureOrientation { get; set; }

        public RooSectorSlopeInfo(uint RooVersion)
        {
            this.RooVersion = RooVersion;
            this.A = 0;
            this.B = 0;
            this.C = 0;
            this.D = 0;
            this.X0 = 0;
            this.Y0 = 0;
            this.TextureAngle = 0;
        }

        public RooSectorSlopeInfo(
            uint RooVersion,
            int A, int B, int C, int D,
            int X, int Y, int TextureAngle)
        {
            this.RooVersion = RooVersion;
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
            this.X0 = X;
            this.Y0 = Y;
            this.TextureAngle = TextureAngle;
            
            Calculate();
        }

        public RooSectorSlopeInfo(uint RoVersion, byte[] Buffer, int StartIndex = 0)
        {
            this.RooVersion = RooVersion;
            ReadFrom(Buffer, StartIndex);
            Calculate();
        }

        public unsafe RooSectorSlopeInfo(uint RooVersion, ref byte* Buffer)
        {
            this.RooVersion = RooVersion;
            ReadFrom(ref Buffer);
            Calculate();
        }

        public void Calculate()
        {
            // texture orientation
            Real radangle = MathUtil.BinaryAngleToRadian((ushort)TextureAngle);
            Real texorientx = (Real)System.Math.Cos(radangle);
            Real texorienty = (Real)System.Math.Sin(radangle);
            Real texorientz = 0;
            TextureOrientation = new V3(texorientx, texorienty, texorientz);

            // generate other endpoints from plane normal, texture origin, and texture
            //  orientation which determine the orientation of the texture's u v space
            //  in the 3d world's x, y, z space
            
            // plane normal
            V3 planeNormal = new V3(A, B, C);

            // first point
            // calculate z of texture origin from x, y, and plane equation            
            Real z = (-A * X0 - B * Y0 - D) / C;
            P0 = new V3(X0, Y0, z);
                      
            // cross normal with texture orientation to get vector perpendicular to texture
            //  orientation and normal = v axis direction
            V3 v2 = planeNormal.CrossProduct(TextureOrientation);
            v2.ScaleToLength(GeometryConstants.FINENESS);

            // cross normal with v axis direction vector to get vector perpendicular to v axis
            //  and normal = u axis direction vector
            V3 v1 = v2.CrossProduct(planeNormal);
            v1.ScaleToLength(GeometryConstants.FINENESS);

            // add vectors to origin to get endpoints
            P1 = P0 + v1;
            P2 = P0 + v2;
        }
    }
}
