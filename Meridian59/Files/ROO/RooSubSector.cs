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

            FloorP = new V3[Vertices.Count];
            FloorUV = new V2[Vertices.Count];
            CeilingP = new V3[Vertices.Count];
            CeilingUV = new V2[Vertices.Count];

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

            FloorP = new V3[Vertices.Count];
            FloorUV = new V2[Vertices.Count];
            CeilingP = new V3[Vertices.Count];
            CeilingUV = new V2[Vertices.Count];
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
            this.FloorP = new V3[0];
            this.FloorUV = new V2[0];
            this.CeilingP = new V3[0];
            this.CeilingUV = new V2[0];

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
                Sector.Leafs.Add(this);
            }
        }

        /// <summary>
        /// Updates the P and UV properties for either floor or ceiling.
        /// Fills in current 3D data for the subsector vertices.
        /// Note: Z component is the height here.
        /// </summary>
        /// <param name="IsFloor">Whether to create for floor or ceiling</param>
        public void UpdateVertexData(bool IsFloor)
        {
            const Real INV64 = 1.0f / (Real)(64 << 4);  // from old code..

            Real left     = 0;
            Real top      = 0;
            Real oneOverC = 0.0f;

            V3[] p;
            V2[] uv;
            RooSectorSlopeInfo slopeInfo;

            if (IsFloor)
            {
                p = FloorP;
                uv = FloorUV;
                slopeInfo = Sector.SlopeInfoFloor;
            }
            else
            {
                p = CeilingP;
                uv = CeilingUV;
                slopeInfo = Sector.SlopeInfoCeiling;
            }

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
                    V2 vectorU;
                    V2 vectorV;
                    V2 vector;
                    Real distance;
                    Real u, temp;

                    ///////////////////////////////////////////////

                    V3 d;
                    V3 d1 = p[count]     - slopeInfo.P0;
                    V3 d2 = slopeInfo.P1 - slopeInfo.P0;
                    V3 d3 = slopeInfo.P2 - slopeInfo.P0;

                    // calc distance from top line (vector u)
                    u    = d1 * d2;
                    temp = d2 * d2;

                    if (temp == 0.0f)
                        temp = 1.0f;

                    u /= temp;
                    intersectTop = slopeInfo.P0 + u * d2;
                    d = p[count] - intersectTop;
                    uv[count].X = d.Length;

                    // calc distance from left line (vector v)
                    u    = d1 * d3;
                    temp = d3 * d3;

                    if (temp == 0.0f)
                        temp = 1.0f;

                    u /= temp;
                    intersectLeft = slopeInfo.P0 + u * d3;
                    d = p[count] - intersectLeft;
                    uv[count].Y = d.Length;

                    ///////////////////////////////////////////////

                    // add
                    uv[count].X += (Sector.TextureY << 4) / 2.0f;
                    uv[count].Y += (Sector.TextureX << 4) / 2.0f;

                    // vectorU
                    vectorU.X = slopeInfo.P1.X - slopeInfo.P0.X;
                    vectorU.Y = slopeInfo.P1.Y - slopeInfo.P0.Y;
                    distance  = vectorU.Length;

                    if (distance == 0.0f)
                        distance = 1.0f;

                    vectorU *= (1.0f / distance);

                    // vectorV
                    vectorV.X = slopeInfo.P2.X - slopeInfo.P0.X;
                    vectorV.Y = slopeInfo.P2.Y - slopeInfo.P0.Y;
                    distance  = vectorV.Length;
                    
                    if (distance == 0.0f)
                        distance = 1.0f;

                    vectorV *= (1.0f / distance);

                    // vector
                    vector.X = p[count].X - slopeInfo.P0.X;
                    vector.Y = p[count].Y - slopeInfo.P0.Y;
                    distance = vector.Length;

                    if (distance == 0)
                        distance = 1.0f;

                    vector *= (1.0f / distance);

                    // apply on uv
                    if ((vector * vectorU) <= 0)
                        uv[count].Y = -uv[count].Y;

                    if ((vector * vectorV) > 0)
                        uv[count].X = -uv[count].X;
                }

                // 2.2: UV without slope
                else
                {
                    uv[count].X = System.Math.Abs(Vertices[count].Y - top) - (Sector.TextureY << 4);
                    uv[count].Y = System.Math.Abs(Vertices[count].X - left) - (Sector.TextureX << 4);
                }

                // scale uv
                uv[count] *= INV64;

                // apply additional userscale
                //p[count] *= Scale;
            }
        }

        /// <summary>
        /// Updates FloorNormal and CeilingNormal properties.
        /// </summary>
        public void UpdateNormals()
        {
            if (Sector == null)
                return;

            // floor
            if (Sector.SlopeInfoFloor != null)
            {
                FloorNormal.X = Sector.SlopeInfoFloor.A;
                FloorNormal.Y = Sector.SlopeInfoFloor.B;
                FloorNormal.Z = Sector.SlopeInfoFloor.C;
                FloorNormal.Normalize();
            }
            else
            {
                FloorNormal.X = 0.0f;
                FloorNormal.Y = 0.0f;
                FloorNormal.Z = 1.0f;
            }

            // ceiling
            if (Sector.SlopeInfoCeiling != null)
            {
                CeilingNormal.X = Sector.SlopeInfoCeiling.A;
                CeilingNormal.Y = Sector.SlopeInfoCeiling.B;
                CeilingNormal.Z = Sector.SlopeInfoCeiling.C;
                CeilingNormal.Normalize();
            }
            else
            {
                CeilingNormal.X = 0.0f;
                CeilingNormal.Y = 0.0f;
                CeilingNormal.Z = -1.0f;
            }
        }
    }
}
