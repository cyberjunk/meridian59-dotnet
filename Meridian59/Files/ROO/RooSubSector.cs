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
using Meridian59.Common;
using Meridian59.Common.Constants;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// A SubSector is a tree leaf in BSP-Tree
    /// </summary>
    [Serializable]
    public class RooSubSector : RooBSPItem
    {       
        #region IByteSerializable
        public override int ByteLength 
        {
            get { 
                int len = base.ByteLength + TypeSizes.SHORT;
                
                // vertices are saved as INT32 for < v14 or FLOAT32 for >= v14
                len += TypeSizes.SHORT;
                foreach (V2 vertex in Vertices)
                    len += TypeSizes.INT + TypeSizes.INT;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(SectorNum), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Vertices.Count)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                foreach (V2 vertex in Vertices)
                {
                    Array.Copy(BitConverter.GetBytes(Convert.ToInt32(vertex.X)), 0, Buffer, cursor, TypeSizes.INT);
                    cursor += TypeSizes.INT;

                    Array.Copy(BitConverter.GetBytes(Convert.ToInt32(vertex.Y)), 0, Buffer, cursor, TypeSizes.INT);
                    cursor += TypeSizes.INT;
                }
            }
            else
            {
                foreach (V2 vertex in Vertices)
                {
                    Array.Copy(BitConverter.GetBytes((float)vertex.X), 0, Buffer, cursor, TypeSizes.FLOAT);
                    cursor += TypeSizes.FLOAT;

                    Array.Copy(BitConverter.GetBytes((float)vertex.Y), 0, Buffer, cursor, TypeSizes.FLOAT);
                    cursor += TypeSizes.FLOAT;
                }
            }

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((ushort*)Buffer) = SectorNum;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = (ushort)Vertices.Count;
            Buffer += TypeSizes.SHORT;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                foreach (V2 vertex in Vertices)
                {
                    *((int*)Buffer) = Convert.ToInt32(vertex.X);
                    Buffer += TypeSizes.INT;

                    *((int*)Buffer) = Convert.ToInt32(vertex.Y);
                    Buffer += TypeSizes.INT;
                }
            }
            else
            {
                foreach (V2 vertex in Vertices)
                {
                    *((float*)Buffer) = (float)vertex.X;
                    Buffer += TypeSizes.FLOAT;

                    *((float*)Buffer) = (float)vertex.Y;
                    Buffer += TypeSizes.FLOAT;
                }
            }
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            SectorNum = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Vertices = new Polygon();

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                for (int i = 0; i < len; i++)
                {
                    int x = BitConverter.ToInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    int y = BitConverter.ToInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    Vertices.Add(new V2(x, y));
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    float x = BitConverter.ToSingle(Buffer, cursor);
                    cursor += TypeSizes.FLOAT;

                    float y = BitConverter.ToSingle(Buffer, cursor);
                    cursor += TypeSizes.FLOAT;

                    Vertices.Add(new V2(x, y));
                }
            }

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            SectorNum = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Vertices = new Polygon();

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                for (int i = 0; i < len; i++)
                {
                    int x = *((int*)Buffer);
                    Buffer += TypeSizes.INT;

                    int y = *((int*)Buffer);
                    Buffer += TypeSizes.INT;

                    Vertices.Add(new V2(x, y));
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    float x = *((float*)Buffer);
                    Buffer += TypeSizes.FLOAT;

                    float y = *((float*)Buffer);
                    Buffer += TypeSizes.FLOAT;

                    Vertices.Add(new V2(x, y));
                }
            }
        }
        #endregion

        /// <summary>
        /// This is a a 'leaf' type node / subclass.
        /// </summary>
        public override NodeType Type { get { return RooBSPItem.NodeType.Leaf; } }
        
        /// <summary>
        /// The 1 based num of the sector this leaf is part of.
        /// </summary>
        public ushort SectorNum { get; set; }
        
        /// <summary>
        /// The polygon points describing this leaf.
        /// </summary>
        public Polygon Vertices { get; set; }

        /// <summary>
        /// Reference to the Sector this leaf is part of.
        /// Will be resolved from SectorNum within ResolveIndices().
        /// </summary>
        public RooSector Sector { get; set; }

        public V3[] FloorP;
        public V2[] FloorUV;
        public V3 FloorNormal;
        public V3[] CeilingP;
        public V2[] CeilingUV;
        public V3 CeilingNormal;

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="SectorNum"></param>
        /// <param name="Vertices"></param>
        public RooSubSector(uint RooVersion, ushort SectorNum, Polygon Vertices) 
            : base(RooVersion)
        {
            this.SectorNum = SectorNum; 
            this.Vertices = Vertices;

            if (Vertices != null && Vertices.Count > 0)            
                boundingBox = Vertices.GetBoundingBox();
            
            else
                boundingBox = BoundingBox2D.NULL;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RooSubSector(uint RooVersion, byte[] Buffer, int StartIndex = 0)
            : base(RooVersion, Buffer, StartIndex) { }

        /// <summary>
        /// Constructor by native parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        public unsafe RooSubSector(uint RooVersion, ref byte* Buffer)
            : base(RooVersion, ref Buffer) { }

        /// <summary>
        /// Resolve all index references to object references
        /// </summary>
        /// <param name="RooFile"></param>
        public override void ResolveIndices(RooFile RooFile)
        {
            // indices properties are not zero-based, but the arrays/lists are
            // get reference to parent Sector
            if (SectorNum > 0 &&
                RooFile.Sectors.Count > SectorNum - 1)
            {
                Sector = RooFile.Sectors[SectorNum - 1];
            }
        }

        /// <summary>
        /// Updates the P, UV and Normal properties for either floor or ceiling.
        /// Fills in current 3D data for the subsector vertices.
        /// Note: Z component is the height here.
        /// </summary>
        /// <param name="IsFloor">Whether to create for floor or ceiling</param>
        /// <param name="Scale">Optional additional scale to apply.</param>
        public void UpdateVertexData(bool IsFloor, Real Scale = 1.0f)
        {
            const Real INV64 = 1.0f / (Real)(64 << 4);  // from old code..

            V3 normal;
            V3[] p          = new V3[Vertices.Count];
            V2[] uv         = new V2[Vertices.Count];
            Real left        = 0;
            Real top         = 0;
            Real oneOverC   = 0.0f;

            RooSectorSlopeInfo slopeInfo = 
                (IsFloor) ? Sector.SlopeInfoFloor : Sector.SlopeInfoCeiling;
            
            /*****************************************************************/

            // find most top left vertex and get its coordinates
            if (slopeInfo != null)
            {
                left = (int)slopeInfo.X0;
                top = (int)slopeInfo.Y0;
                oneOverC = 1.0f / slopeInfo.C;
            }
            else
            {
                for (int i = 0; i < Vertices.Count; i++)
                {
                    if (Vertices[i].X < left)
                        left = Vertices[i].X;

                    if (Vertices[i].Y < top)
                        top = Vertices[i].Y;
                }
            }

            /*****************************************************************/

            for (int count = 0; count < Vertices.Count; count++)
            {
                // 1: Fill in vertex coordinates
                if (slopeInfo != null)
                {
                    p[count].X = Vertices[count].X;
                    p[count].Y = Vertices[count].Y;
                    p[count].Z = (-slopeInfo.A * p[count].X - slopeInfo.B * p[count].Y - slopeInfo.D) *oneOverC;
                }
                else
                {
                    p[count].X = Vertices[count].X;
                    p[count].Y = Vertices[count].Y;
                    p[count].Z = (IsFloor) ? (Sector.FloorHeight * 16.0f) : (Sector.CeilingHeight * 16.0f);                   
                }
              
                // 2.1: UV with slope
                if (slopeInfo != null)
                {
                    V3 intersectTop;
                    V3 intersectLeft;          
                    V3 vectorU;
                    V3 vectorV;
                    V3 vector;
                    Real distance;
                    Real U, temp;
                    
                    // calc distance from top line (vector u)
                    U = ((p[count].X - slopeInfo.P0.X) * (slopeInfo.P1.X - slopeInfo.P0.X)) +
                        ((p[count].Z - slopeInfo.P0.Z) * (slopeInfo.P1.Z - slopeInfo.P0.Z)) +
                        ((p[count].Y - slopeInfo.P0.Y) * (slopeInfo.P1.Y - slopeInfo.P0.Y));

                    temp = ((slopeInfo.P1.X - slopeInfo.P0.X) * (slopeInfo.P1.X - slopeInfo.P0.X)) +
                        ((slopeInfo.P1.Z - slopeInfo.P0.Z) * (slopeInfo.P1.Z - slopeInfo.P0.Z)) +
                        ((slopeInfo.P1.Y - slopeInfo.P0.Y) * (slopeInfo.P1.Y - slopeInfo.P0.Y));

                    if (temp == 0.0f)
                        temp = 1.0f;

                    U /= temp;

                    intersectTop.X = slopeInfo.P0.X + U * (slopeInfo.P1.X - slopeInfo.P0.X);
                    intersectTop.Z = slopeInfo.P0.Z + U * (slopeInfo.P1.Z - slopeInfo.P0.Z);
                    intersectTop.Y = slopeInfo.P0.Y + U * (slopeInfo.P1.Y - slopeInfo.P0.Y);

                    uv[count].X = (Real)System.Math.Sqrt(
                                    (p[count].X - intersectTop.X) * (p[count].X - intersectTop.X) +
                                    (p[count].Z - intersectTop.Z) * (p[count].Z - intersectTop.Z) +
                                    (p[count].Y - intersectTop.Y) * (p[count].Y - intersectTop.Y));

                    // calc distance from left line (vector v)
                    U = ((p[count].X - slopeInfo.P0.X) * (slopeInfo.P2.X - slopeInfo.P0.X)) +
                        ((p[count].Z - slopeInfo.P0.Z) * (slopeInfo.P2.Z - slopeInfo.P0.Z)) +
                        ((p[count].Y - slopeInfo.P0.Y) * (slopeInfo.P2.Y - slopeInfo.P0.Y));

                    temp = ((slopeInfo.P2.X - slopeInfo.P0.X) * (slopeInfo.P2.X - slopeInfo.P0.X)) +
                        ((slopeInfo.P2.Z - slopeInfo.P0.Z) * (slopeInfo.P2.Z - slopeInfo.P0.Z)) +
                        ((slopeInfo.P2.Y - slopeInfo.P0.Y) * (slopeInfo.P2.Y - slopeInfo.P0.Y));

                    if (temp == 0.0f)
                        temp = 1.0f;

                    U /= temp;

                    intersectLeft.X = slopeInfo.P0.X + U * (slopeInfo.P2.X - slopeInfo.P0.X);
                    intersectLeft.Z = slopeInfo.P0.Z + U * (slopeInfo.P2.Z - slopeInfo.P0.Z);
                    intersectLeft.Y = slopeInfo.P0.Y + U * (slopeInfo.P2.Y - slopeInfo.P0.Y);

                    uv[count].Y = (Real)System.Math.Sqrt(
                                    (p[count].X - intersectLeft.X) * (p[count].X - intersectLeft.X) +
                                    (p[count].Z - intersectLeft.Z) * (p[count].Z - intersectLeft.Z) +
                                    (p[count].Y - intersectLeft.Y) * (p[count].Y - intersectLeft.Y));

                    uv[count].X += (Sector.TextureY << 4) / 2.0f;
                    uv[count].Y += (Sector.TextureX << 4) / 2.0f;

                    vectorU.X = slopeInfo.P1.X - slopeInfo.P0.X;
                    vectorU.Z = slopeInfo.P1.Z - slopeInfo.P0.Z;
                    vectorU.Y = slopeInfo.P1.Y - slopeInfo.P0.Y;

                    distance = (Real)System.Math.Sqrt((vectorU.X * vectorU.X) + (vectorU.Y * vectorU.Y));

                    if (distance == 0.0f)
                        distance = 1.0f;

                    vectorU.X /= distance;
                    vectorU.Z /= distance;
                    vectorU.Y /= distance;

                    vectorV.X = slopeInfo.P2.X - slopeInfo.P0.X;
                    vectorV.Z = slopeInfo.P2.Z - slopeInfo.P0.Z;
                    vectorV.Y = slopeInfo.P2.Y - slopeInfo.P0.Y;

                    distance = (Real)System.Math.Sqrt((vectorV.X * vectorV.X) + (vectorV.Y * vectorV.Y));

                    if (distance == 0.0f)
                        distance = 1.0f;

                    vectorV.X /= distance;
                    vectorV.Z /= distance;
                    vectorV.Y /= distance;

                    vector.X = p[count].X - slopeInfo.P0.X;
                    vector.Y = p[count].Y - slopeInfo.P0.Y;

                    distance = (Real)System.Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));

                    if (distance == 0)
                        distance = 1.0f;

                    vector.X /= distance;
                    vector.Y /= distance;

                    if (((vector.X * vectorU.X) +
                        (vector.Y * vectorU.Y)) <= 0)
                        uv[count].Y = -uv[count].Y;

                    if (((vector.X * vectorV.X) +
                        (vector.Y * vectorV.Y)) > 0)
                        uv[count].X = -uv[count].X;
                }

                // 2.2: UV without slope
                else
                {
                    uv[count].X = System.Math.Abs(Vertices[count].Y - top) - (Sector.TextureY << 4);
                    uv[count].Y = System.Math.Abs(Vertices[count].X - left) - (Sector.TextureX << 4);
                }

                uv[count].X *= INV64;
                uv[count].Y *= INV64;

                // apply additional userscale
                p[count] *= Scale;
            }

            /*****************************************************************/

            // Calculate the normal
            if (slopeInfo != null)
            {
                // if the sector is sloped, we get the normal from slopeinfo
                normal.X = slopeInfo.A;
                normal.Y = slopeInfo.B;
                normal.Z = slopeInfo.C;

                normal.Normalize();
            }
            else if (IsFloor)
            {
                // default normal for non sloped floor
                normal.X = 0.0f;
                normal.Y = 0.0f;
                normal.Z = 1.0f;
            }
            else
            {
                // default normal for non sloped ceilings
                normal.X = 0.0f;
                normal.Y = 0.0f;
                normal.Z = -1.0f;
            }

            /*****************************************************************/

            // save in class properties depending on floor/ceiling
            if (IsFloor)
            {
                FloorP = p;
                FloorUV = uv;
                FloorNormal = normal;
            }
            else
            {
                CeilingP = p;
                CeilingUV = uv;
                CeilingNormal = normal;
            }
        }
    }
}
