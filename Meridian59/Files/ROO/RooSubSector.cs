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
                
                // vertices
                len += TypeSizes.SHORT;
                foreach (RooVertex vertex in Vertices)
                    len += vertex.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(SectorDefReference), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Vertices.Count)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (RooVertex vertex in Vertices)
                cursor += vertex.WriteTo(Buffer, cursor);
            
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((ushort*)Buffer) = SectorDefReference;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = (ushort)Vertices.Count;
            Buffer += TypeSizes.SHORT;

            foreach (RooVertex vertex in Vertices)
                vertex.WriteTo(ref Buffer);
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            SectorDefReference = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Vertices = new List<RooVertex>(len);
            for(int i = 0; i < len; i++)
            {
                RooVertex vertex = new RooVertex(Buffer, cursor);
                cursor += vertex.ByteLength;

                Vertices.Add(vertex);
            }             
            
            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            SectorDefReference = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Vertices = new List<RooVertex>(len);
            for (int i = 0; i < len; i++)                      
                Vertices.Add(new RooVertex(ref Buffer));             
        }
        #endregion

        public override byte Type { get { return RooBSPItem.SubSectorType; } }
        public ushort SectorDefReference { get; set; }
        public List<RooVertex> Vertices { get; set; }

        public RooSector Sector { get; set; }

        public RooSubSector( 
            int X1, int Y1, int X2, int Y2,
            ushort SectorDefReference,
            List<RooVertex> Vertices)
            : base(X1, Y1, X2, Y2)
        {          
            this.SectorDefReference = SectorDefReference; 
            this.Vertices = Vertices;                  
        }

        public RooSubSector(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe RooSubSector(ref byte* Buffer)
            : base(ref Buffer) { }

        /// <summary>
        /// Resolve all index references to object references
        /// </summary>
        /// <param name="RooFile"></param>
        public override void ResolveIndices(RooFile RooFile)
        {
            // indices properties are not zero-based, but the arrays/lists are
            // get reference to parent Sector
            if (SectorDefReference > 0 &&
                RooFile.Sectors.Count > SectorDefReference - 1)
            {
                Sector = RooFile.Sectors[SectorDefReference - 1];
            }
        }

        /// <summary>
        /// Creates RenderInfo data (containing points and uv coordinates).
        /// Note: Z component is the height.
        /// </summary>
        /// <param name="IsFloor">Whether to create for floor or ceiling</param>
        /// <param name="Scale">Optional additional scale to apply.</param>
        /// <returns></returns>
        public RenderInfo GetRenderInfo(bool IsFloor, Real Scale = 1.0f)
        {
            // get possible slopeinfo either for floor or ceiling
            RooSectorSlopeInfo slopeInfo = null;
            if (IsFloor) slopeInfo = Sector.SlopeInfoFloor;
            if (!IsFloor) slopeInfo = Sector.SlopeInfoCeiling;

            // initialize renderinfo keeping points and uv coordinates
            RenderInfo RI = new RenderInfo(Vertices.Count);
            V3[] P = RI.P;
            V2[] UV = RI.UV;

            // initialize some more vars
            V3 intersectTop = new V3(0, 0, 0);
            V3 intersectLeft = new V3(0, 0, 0);
            int left = 0;
            int top = 0;
            Real inv64 = 1.0f / (Real)(64 << 4);
            Real oneOverC = 0.0f;

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

            // Start UV calculation
            for (int count = 0; count < Vertices.Count; count++)
            {
                // fill in point coordinates
                // this time we use the z as z and flip y/z (mogre!) when triangulating
                // instead of changing all the following ported code
                if (slopeInfo != null)
                {
                    P[count].X = (Real)Vertices[count].X;
                    P[count].Y = (Real)Vertices[count].Y;
                    P[count].Z = (-slopeInfo.A * P[count].X - slopeInfo.B * P[count].Y - slopeInfo.D) * oneOverC;
                }
                else
                {
                    P[count].X = (Real)Vertices[count].X;
                    P[count].Y = (Real)Vertices[count].Y;

                    if (IsFloor)
                        P[count].Z = (Real)(Sector.FloorHeight * 16.0f);
                    else
                        P[count].Z = (Real)(Sector.CeilingHeight * 16.0f);
                }

                Real U, temp;

                // start uv
                if (slopeInfo != null)
                {
                    V3 vectorU = new V3(0, 0, 0);
                    V3 vectorV = new V3(0, 0, 0);
                    V3 vector = new V3(0, 0, 0);
                    Real distance;

                    // calc distance from top line (vector u)
                    U = ((P[count].X - slopeInfo.P0.X) * (slopeInfo.P1.X - slopeInfo.P0.X)) +
                        ((P[count].Z - slopeInfo.P0.Z) * (slopeInfo.P1.Z - slopeInfo.P0.Z)) +
                        ((P[count].Y - slopeInfo.P0.Y) * (slopeInfo.P1.Y - slopeInfo.P0.Y));

                    temp = ((slopeInfo.P1.X - slopeInfo.P0.X) * (slopeInfo.P1.X - slopeInfo.P0.X)) +
                        ((slopeInfo.P1.Z - slopeInfo.P0.Z) * (slopeInfo.P1.Z - slopeInfo.P0.Z)) +
                        ((slopeInfo.P1.Y - slopeInfo.P0.Y) * (slopeInfo.P1.Y - slopeInfo.P0.Y));

                    if (temp == 0)
                        temp = 1.0f;

                    U /= temp;

                    intersectTop.X = slopeInfo.P0.X + U * (slopeInfo.P1.X - slopeInfo.P0.X);
                    intersectTop.Z = slopeInfo.P0.Z + U * (slopeInfo.P1.Z - slopeInfo.P0.Z);
                    intersectTop.Y = slopeInfo.P0.Y + U * (slopeInfo.P1.Y - slopeInfo.P0.Y);

                    UV[count].X = (Real)System.Math.Sqrt(
                                    (P[count].X - intersectTop.X) * (P[count].X - intersectTop.X) +
                                    (P[count].Z - intersectTop.Z) * (P[count].Z - intersectTop.Z) +
                                    (P[count].Y - intersectTop.Y) * (P[count].Y - intersectTop.Y));

                    // calc distance from left line (vector v)
                    U = ((P[count].X - slopeInfo.P0.X) * (slopeInfo.P2.X - slopeInfo.P0.X)) +
                        ((P[count].Z - slopeInfo.P0.Z) * (slopeInfo.P2.Z - slopeInfo.P0.Z)) +
                        ((P[count].Y - slopeInfo.P0.Y) * (slopeInfo.P2.Y - slopeInfo.P0.Y));

                    temp = ((slopeInfo.P2.X - slopeInfo.P0.X) * (slopeInfo.P2.X - slopeInfo.P0.X)) +
                        ((slopeInfo.P2.Z - slopeInfo.P0.Z) * (slopeInfo.P2.Z - slopeInfo.P0.Z)) +
                        ((slopeInfo.P2.Y - slopeInfo.P0.Y) * (slopeInfo.P2.Y - slopeInfo.P0.Y));

                    if (temp == 0)
                        temp = 1.0f;

                    U /= temp;

                    intersectLeft.X = slopeInfo.P0.X + U * (slopeInfo.P2.X - slopeInfo.P0.X);
                    intersectLeft.Z = slopeInfo.P0.Z + U * (slopeInfo.P2.Z - slopeInfo.P0.Z);
                    intersectLeft.Y = slopeInfo.P0.Y + U * (slopeInfo.P2.Y - slopeInfo.P0.Y);

                    UV[count].Y = (Real)System.Math.Sqrt(
                                    (P[count].X - intersectLeft.X) * (P[count].X - intersectLeft.X) +
                                    (P[count].Z - intersectLeft.Z) * (P[count].Z - intersectLeft.Z) +
                                    (P[count].Y - intersectLeft.Y) * (P[count].Y - intersectLeft.Y));

                    UV[count].X += (Sector.TextureY << 4) / 2.0f;
                    UV[count].Y += (Sector.TextureX << 4) / 2.0f;

                    vectorU.X = slopeInfo.P1.X - slopeInfo.P0.X;
                    vectorU.Z = slopeInfo.P1.Z - slopeInfo.P0.Z;
                    vectorU.Y = slopeInfo.P1.Y - slopeInfo.P0.Y;

                    distance = (Real)System.Math.Sqrt((vectorU.X * vectorU.X) + (vectorU.Y * vectorU.Y));

                    if (distance == 0)
                        distance = 1.0f;

                    vectorU.X /= distance;
                    vectorU.Z /= distance;
                    vectorU.Y /= distance;

                    vectorV.X = slopeInfo.P2.X - slopeInfo.P0.X;
                    vectorV.Z = slopeInfo.P2.Z - slopeInfo.P0.Z;
                    vectorV.Y = slopeInfo.P2.Y - slopeInfo.P0.Y;

                    distance = (Real)System.Math.Sqrt((vectorV.X * vectorV.X) + (vectorV.Y * vectorV.Y));

                    if (distance == 0)
                        distance = 1.0f;

                    vectorV.X /= distance;
                    vectorV.Z /= distance;
                    vectorV.Y /= distance;

                    vector.X = P[count].X - slopeInfo.P0.X;
                    vector.Y = P[count].Y - slopeInfo.P0.Y;

                    distance = (Real)System.Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));

                    if (distance == 0)
                        distance = 1.0f;

                    vector.X /= distance;
                    vector.Y /= distance;

                    if (((vector.X * vectorU.X) +
                        (vector.Y * vectorU.Y)) <= 0)
                        UV[count].Y = -UV[count].Y;

                    if (((vector.X * vectorV.X) +
                        (vector.Y * vectorV.Y)) > 0)
                        UV[count].X = -UV[count].X;
                }
                else
                {
                    UV[count].X = (Real)System.Math.Abs(Vertices[count].Y - top) - (Sector.TextureY << 4);
                    UV[count].Y = (Real)System.Math.Abs(Vertices[count].X - left) - (Sector.TextureX << 4);
                }

                UV[count].X *= inv64;
                UV[count].Y *= inv64;

                // apply additional userscale
                P[count] *= Scale;
            }

            // Calculate the normal
            if (slopeInfo != null)
            {
                // if the sector is sloped, we get the normal from slopeinfo
                RI.Normal.X = slopeInfo.A;
                RI.Normal.Y = slopeInfo.B;
                RI.Normal.Z = slopeInfo.C;

                RI.Normal.Normalize();
            }
            else if (IsFloor)
            {
                // default normal for non sloped floor
                RI.Normal.X = 0.0f;
                RI.Normal.Y = 0.0f;
                RI.Normal.Z = 1.0f;
            }
            else
            {
                // default normal for non sloped ceilings
                RI.Normal.X = 0.0f;
                RI.Normal.Y = 0.0f;
                RI.Normal.Z = -1.0f;
            }
            
            return RI;
        }

        /// <summary>
        /// RenderInformation for a SubSector
        /// </summary>
        public class RenderInfo
        {
            public V3[] P;
            public V2[] UV;
            public V3 Normal;

            public RenderInfo(int Size)
            {
                this.P = new V3[Size];
                this.UV = new V2[Size];
                this.Normal = new V3(0.0f, 0.0f, 0.0f);

                for (int i = 0; i < Size; i++)
                {
                    P[i] = new V3(0.0f, 0.0f, 0.0f);
                    UV[i] = new V2(0.0f, 0.0f);
                }
            }
        }
    }
}
