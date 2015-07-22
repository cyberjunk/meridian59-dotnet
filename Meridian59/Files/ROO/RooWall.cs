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

using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Files.BGF;
using System;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// A wall on the map.
    /// See original "WallData" struct and bspload.c
    /// </summary>
    [Serializable]
    public class RooWall : IByteSerializableFast, IRooIndicesResolvable
    {
        /// <summary>
        /// Stores 3D VertexData for a side-part of a RooWall
        /// </summary>
        public struct VertexData
        {
            public V3 P0, P1, P2, P3;
            public V2 UV0, UV1, UV2, UV3;
            public V3 Normal;
        }

        #region IByteSerializable
        public int ByteLength 
        {
            get 
            {
                ushort lClientLength = (RooVersion < RooFile.VERSIONFLOATCOORDS) ? TypeSizes.SHORT : TypeSizes.FLOAT;

                return 
                    TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT +
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT +
                    lClientLength + 
                    TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + 
                    TypeSizes.SHORT + TypeSizes.SHORT;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(NextWallNumInPlane), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightSideNum), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftSideNum), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(P1.X)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(P1.Y)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(P2.X)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(P2.Y)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(ClientLength)), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;
            }
            else
            {
                Array.Copy(BitConverter.GetBytes((float)P1.X), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)P1.Y), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)P2.X), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)P2.Y), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)ClientLength), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;
            }

            Array.Copy(BitConverter.GetBytes(RightXOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftXOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightYOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftYOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightSectorNum), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftSectorNum), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {            
            *((short*)Buffer) = NextWallNumInPlane;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = RightSideNum;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = LeftSideNum;
            Buffer += TypeSizes.SHORT;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                *((int*)Buffer) = Convert.ToInt32(P1.X);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(P1.Y);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(P2.X);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(P2.Y);
                Buffer += TypeSizes.INT;

                *((ushort*)Buffer) = Convert.ToUInt16(ClientLength);
                Buffer += TypeSizes.SHORT;
            }
            else
            {
                *((float*)Buffer) = (float)P1.X;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)P1.Y;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)P2.X;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)P2.Y;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)ClientLength;
                Buffer += TypeSizes.FLOAT;
            }

            *((short*)Buffer) = RightXOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = LeftXOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = RightYOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = LeftYOffset;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = RightSectorNum;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = LeftSectorNum;
            Buffer += TypeSizes.SHORT;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            NextWallNumInPlane = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightSideNum = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftSideNum = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                p1.X = (Real)BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                p1.Y = (Real)BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                p2.X = (Real)BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                p2.Y = (Real)BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                ClientLength = (Real)BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;
            }
            else
            {
                p1.X = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                p1.Y = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                p2.X = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                p2.Y = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                ClientLength = (Real)BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;
            }

            RightXOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftXOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightYOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftYOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightSectorNum = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftSectorNum = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            NextWallNumInPlane = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightSideNum = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftSideNum = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                p1.X = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                p1.Y = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                p2.X = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                p2.Y = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                ClientLength = (Real)(*((ushort*)Buffer));
                Buffer += TypeSizes.SHORT;
            }
            else
            {
                p1.X = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                p1.Y = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                p2.X = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                p2.Y = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                ClientLength = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;
            }

            RightXOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftXOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightYOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftYOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightSectorNum = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftSectorNum = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;
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

        #region Properties
        protected V2 p1;
        protected V2 p2;

        /// <summary>
        /// 
        /// </summary>
        public uint RooVersion { get; set; }

        /// <summary>
        /// Number of this wall (1 based)
        /// </summary>
        public int Num { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public short NextWallNumInPlane { get; set; }

        /// <summary>
        /// First wall point (source)(oldclient FINENESS 1:1024)
        /// </summary>
        public V2 P1 { get { return p1; } set { p1 = value; } }

        /// <summary>
        /// Value of P1.X converted from and to integer
        /// </summary>
        public int X1 { get { return (int)p1.X; } set { p1.X = value; } }

        /// <summary>
        /// Value of P1.Y converted from and to integer
        /// </summary>
        public int Y1 { get { return (int)p1.Y; } set { p1.Y = value; } }

        /// <summary>
        /// Second wall point (dest)(oldclient FINENESS 1:1024)
        /// </summary>
        public V2 P2 { get { return p2; } set { p2 = value; } }

        /// <summary>
        /// Value of P2.X converted from and to integer
        /// </summary>
        public int X2 { get { return (int)p2.X; } set { p2.X = value; } }

        /// <summary>
        /// Value of P2.Y converted from and to integer
        /// </summary>
        public int Y2 { get { return (int)p2.Y; } set { p2.Y = value; } }

        /// <summary>
        /// Length of the wall in server FINENESS (1:64).
        /// Note: Rather use the length of vector P1P2.
        /// </summary>
        public Real ClientLength { get; set; }

        /// <summary>
        /// XOffset of the texture on the right side
        /// </summary>
        public short RightXOffset { get; set; }
        
        /// <summary>
        /// YOffset of the texture on the right side
        /// </summary>
        public short RightYOffset { get; set; }
        
        /// <summary>
        /// XOffset of the texture on the left side
        /// </summary>
        public short LeftXOffset { get; set; }

        /// <summary>
        /// YOffset of the texture on the left side
        /// </summary>
        public short LeftYOffset { get; set; }

        /// <summary>
        /// Num (1 based) of sector to the right of this wall.
        /// 0 is unset.
        /// </summary>
        public ushort RightSectorNum { get; set; }
        
        /// <summary>
        /// Num (1 based) of the sector to the left of this wall.
        /// 0 is unset.
        /// </summary>
        public ushort LeftSectorNum { get; set; }

        /// <summary>
        /// Num (1 based) of the right side of this wall.
        /// 0 is unset.
        /// </summary>
        public ushort RightSideNum { get; set; }

        /// <summary>
        /// Num (1 based) of the left of left side of this wall.
        /// 0 is unset.
        /// </summary>
        public ushort LeftSideNum { get; set; }

        /// <summary>
        /// Sector to the right of the wall.
        /// Will be resolved from RightSectorNum within ResolveIndices().
        /// </summary>
        public RooSector RightSector { get; set; }

        /// <summary>
        /// Sector to the left of the wall.
        /// Will be resolved from LeftSectorNum within ResolveIndices().
        /// </summary>
        public RooSector LeftSector { get; set; }

        /// <summary>
        /// Right side of the wall.
        /// Will be resolved from RightSideNum within ResolveIndices().
        /// </summary>
        public RooSideDef RightSide { get; set; }

        /// <summary>
        /// Left side of the wall.
        /// Will be resolved from LeftSideNum within ResolveIndices().
        /// </summary>
        public RooSideDef LeftSide { get; set; }

        /// <summary>
        /// Flags describing special intersection cases.
        /// </summary>
        public BowtieFlags BowtieFlags { get; set; }

        /// <summary>
        /// Next wall on same infinite line.
        /// Will be resolved from NextWallNumInPlane within ResolveIndices().
        /// </summary>
        public RooWall NextWallInPlane { get; set; }

        #region Z-coordinates
        /* 
         * There is an overall of 8 z-coordinates possible for a wall given by
         * its adjacents sectorheights/ceilings, so except for the
         * (X1,Y1), (X2,Y2) these values are not part of the ROO data
         *
         *  S1     S2
         * ------        (z3 / zz3)
         *      |
         *      -----    (z2 / zz2)
         *
         *      -----    (z1 / zz1)
         *      |
         * ------        (z0 / zz0)     
         * 
         */

        // Z-coordinates at (X1,Y1) 
        protected Real z0;      /* height of bottom of lower wall */
        protected Real z1;      /* height of top of lower wall / bottom of normal wall */
        protected Real z2;      /* height of top of normal wall / bottom of upper wall */
        protected Real z3;      /* height of top of upper wall */

        // Z-coordinates at (X2,Y2)
        protected Real zz0;    /* height of bottom of lower wall */
        protected Real zz1;    /* height of top of lower wall / bottom of normal wall */
        protected Real zz2;     /* height of top of normal wall / bottom of upper wall */
        protected Real zz3;     /* height of top of upper wall */

        protected Real z0Neg;       /* height of bottom of lower wall */
        protected Real z1Neg;       /* height of top of lower wall / bottom of normal wall */
        protected Real zz0Neg;      /* height of bottom of lower wall */
        protected Real zz1Neg;      /* height of top of lower wall / bottom of normal wall */
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="ServerID">Sometimes also called UserID, used to reference wall by server.</param>
        /// <param name="RightSideNum">Num of the right side of the wall (1=first, 0=unset)</param>
        /// <param name="LeftSideNum">Num of the left side of the wall (1=first, 0=unset)</param>
        /// <param name="P1">2D coordinates of startpoint, must be in 1:1024 units</param>
        /// <param name="P2">2D coordinates of endpoint, must be in 1:1024 units</param>
        /// <param name="RightXOffset"></param>
        /// <param name="LeftXOffset"></param>
        /// <param name="RightYOffset"></param>
        /// <param name="LeftYOffset"></param>
        /// <param name="RightSectorNum">Num of the sector right to the wall (1=first, 0=unset)</param>
        /// <param name="LeftSectorNum">Num of the sector left to the wall (1=first, 0=unset)</param>
        public RooWall(
            uint RooVersion,
            short ServerID, 
            ushort RightSideNum, 
            ushort LeftSideNum, 
            V2 P1, 
            V2 P2, 
            short RightXOffset, 
            short LeftXOffset, 
            short RightYOffset, 
            short LeftYOffset,
            ushort RightSectorNum, 
            ushort LeftSectorNum)
        {
            this.RooVersion = RooVersion;
            this.NextWallNumInPlane = ServerID;
            this.RightSideNum = RightSideNum;
            this.LeftSideNum = LeftSideNum;
            this.P1 = P1;
            this.P2 = P2;

            // set clientlength stored in 1:64 units (convert from 1:1024)
            this.ClientLength = (P1-P2).Length * 0.0625f;

            this.RightXOffset = RightXOffset;
            this.LeftXOffset = LeftXOffset;
            this.RightYOffset = RightYOffset;
            this.LeftYOffset = LeftYOffset;
            this.RightSectorNum = RightSectorNum;
            this.LeftSectorNum = LeftSectorNum;

            this.BowtieFlags = new BowtieFlags();
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RooWall(uint RooVersion, byte[] Buffer, int StartIndex = 0)
        {
            this.RooVersion = RooVersion;
            BowtieFlags = new BowtieFlags();
            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Constructor by pointerbased parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        public unsafe RooWall(uint RooVersion, ref byte* Buffer)
        {
            this.RooVersion = RooVersion;
            BowtieFlags = new BowtieFlags();
            ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates object references from the Sector/Side Num references.
        /// </summary>
        /// <param name="RooFile"></param>
        public void ResolveIndices(RooFile RooFile)
        {
            // num properties are not zero-based, but the arrays are

            // get reference to right SectorDef
            if (RightSectorNum > 0 && 
                RooFile.Sectors.Count > RightSectorNum - 1)
            {
                RightSector = RooFile.Sectors[RightSectorNum - 1];

                // save as adjacent wall
                if (!RightSector.Walls.Contains(this))
                    RightSector.Walls.Add(this);
            }

            // get reference to left SectorDef
            if (LeftSectorNum > 0 &&
                RooFile.Sectors.Count > LeftSectorNum - 1)
            {
                LeftSector = RooFile.Sectors[LeftSectorNum - 1];

                // save as adjacent wall
                if (!LeftSector.Walls.Contains(this))
                    LeftSector.Walls.Add(this);
            }

            // get reference to right SideDef
            if (RightSideNum > 0 &&
                RooFile.SideDefs.Count > RightSideNum - 1)
            {
                RightSide = RooFile.SideDefs[RightSideNum - 1];

                // save as adjacent side
                if (RightSector != null && !RightSector.Sides.Contains(RightSide))
                    RightSector.Sides.Add(RightSide);

                // save as adjacent side
                if (LeftSector != null && !LeftSector.Sides.Contains(RightSide))
                    LeftSector.Sides.Add(RightSide);
            }

            // get reference to left SideDef
            if (LeftSideNum > 0 &&
                RooFile.SideDefs.Count > LeftSideNum - 1)
            {
                LeftSide = RooFile.SideDefs[LeftSideNum - 1];

                // save as adjacent side
                if (RightSector != null && !RightSector.Sides.Contains(LeftSide))
                    RightSector.Sides.Add(LeftSide);

                // save as adjacent side
                if (LeftSector != null && !LeftSector.Sides.Contains(LeftSide))
                    LeftSector.Sides.Add(LeftSide);
            }

            // get reference to next wall in same plane
            if (NextWallNumInPlane > 0 &&
                RooFile.Walls.Count > NextWallNumInPlane - 1)
            {
                NextWallInPlane = RooFile.Walls[NextWallNumInPlane - 1];
            }
        }

        /// <summary>
        /// Fills in the Z coordinates for the wall.
        /// Make sure to call "ResolveIndices" before!
        /// Taken from "SetWallHeights" in bspload.c
        /// </summary>
        public void CalculateWallSideHeights()
        {
            // no sectors? we're screwed, use defaults and return
            if (RightSector == null && LeftSector == null)
            {
                z0 = z1 = 0.0f;
                z2 = z3 = (Real)GeometryConstants.FINENESS;
                zz0 = zz1 = 0.0f;
                zz2 = zz3 = (Real)GeometryConstants.FINENESS;
                return;
            }

            // only left sector? use heights from there and return
            if (RightSector == null)
            {
                z0 = z1 = LeftSector.CalculateFloorHeight(P1.X, P1.Y);
                z2 = z3 = LeftSector.CalculateCeilingHeight(P1.X, P1.Y);
                zz0 = zz1 = LeftSector.CalculateFloorHeight(P2.X, P2.Y);
                zz2 = zz3 = LeftSector.CalculateCeilingHeight(P2.X, P2.Y);
                return;
            }

            // only right sector? use heights from there and return
            if (LeftSector == null)
            {
                z0 = z1 = RightSector.CalculateFloorHeight(P1.X, P1.Y);
                z2 = z3 = RightSector.CalculateCeilingHeight(P1.X, P1.Y);
                zz0 = zz1 = RightSector.CalculateFloorHeight(P2.X, P2.Y);
                zz2 = zz3 = RightSector.CalculateCeilingHeight(P2.X, P2.Y);
                return;
            }

            // --  finally, if there are both sectors available ---

            // start with the floor handling
            Real S1_height0 = RightSector.CalculateFloorHeight(P1.X, P1.Y);
            Real S2_height0 = LeftSector.CalculateFloorHeight(P1.X, P1.Y);
            Real S1_height1 = RightSector.CalculateFloorHeight(P2.X, P2.Y);
            Real S2_height1 = LeftSector.CalculateFloorHeight(P2.X, P2.Y);

            // S1 is above S2 at first endpoint
            if (S1_height0 > S2_height0)
            {
                if (S1_height1 >= S2_height1)
                {
                    // normal wall - S1 higher at both ends
                    BowtieFlags.Value =  0;

                    z1 = S1_height0;
                    zz1 = S1_height1;
                    z0 = S2_height0;
                    zz0 = S2_height1;
                }
                else
                {
                    // bowtie handling
                    BowtieFlags.IsBelowPos = true;

                    // this is the variant for gD3DEnabled in the old code
                    z1 = S1_height0;
                    zz1 = S1_height1;
                    z0 = S2_height0;
                    zz0 = S1_height1;

                    z1Neg = S2_height0;
                    zz1Neg = S2_height1;
                    z0Neg = S2_height0;
                    zz0Neg = S1_height1;

                    // other variant
                    /*z1 = S1_height0;
                    zz1 = S2_height1;
                    z0 = S2_height0;
                    zz0 = S1_height1;*/
                }
            }

            // S2 above S1 at first endpoint
            else
            {                
                if (S2_height1 >= S1_height1)
                {
                    // normal wall - S2 higher at both ends
                    BowtieFlags.Value = 0;

                    z1 = S2_height0;
                    zz1 = S2_height1;
                    z0 = S1_height0;
                    zz0 = S1_height1;
                }
                else
                {
                    // bowtie handling
                    BowtieFlags.IsBelowNeg = true;

                    // this is the variant for gD3DEnabled in the old code
                    z1 = S1_height0;
                    zz1 = S1_height1;
                    z0 = S1_height0;
                    zz0 = S2_height1;

                    z1Neg = S2_height0;
                    zz1Neg = S2_height1;
                    z0Neg = S1_height0;
                    zz0Neg = S2_height1;

                    // other variant
                    /*z1 = S2_height0;
                    zz1 = S1_height1;
                    z0 = S1_height0;
                    zz0 = S2_height1;*/
                }
            }

            // start with ceiling handling
            S1_height0 = RightSector.CalculateCeilingHeight(P1.X, P1.Y);
            S2_height0 = LeftSector.CalculateCeilingHeight(P1.X, P1.Y);
            S1_height1 = RightSector.CalculateCeilingHeight(P2.X, P2.Y);
            S2_height1 = LeftSector.CalculateCeilingHeight(P2.X, P2.Y);

            if (S1_height0 > S2_height0)
            {
                if (S1_height1 >= S2_height1)
                {
                    // normal wall - S1 higher at both ends
                    //wall->bowtie_bits &= (BYTE)~BT_ABOVE_BOWTIE; // Clear above bowtie bits
                    BowtieFlags.IsAboveBowtie = false;

                    z3 = S1_height0;
                    zz3 = S1_height1;
                    z2 = S2_height0;
                    zz2 = S2_height1;
                }
                else
                {
                    // bowtie - see notes on bowties above
                    //wall->bowtie_bits |= (BYTE)BT_ABOVE_POS; // positive sector is on top at endpoint 0
                    BowtieFlags.IsAbovePos = true;

                    z3 = S1_height0;
                    zz3 = S2_height1;
                    z2 = S2_height0;
                    zz2 = S1_height1;
                }
            }
            else
            {
                if (S2_height1 >= S1_height1)
                {
                    // normal wall - S2 higher at both ends
                    //wall->bowtie_bits &= (BYTE)~BT_ABOVE_BOWTIE;
                    BowtieFlags.IsAboveBowtie = false;

                    z3 = S2_height0;
                    zz3 = S2_height1;
                    z2 = S1_height0;
                    zz2 = S1_height1;
                }
                else
                {
                    // bowtie - see notes on bowties above
                    //wall->bowtie_bits |= (BYTE)BT_ABOVE_NEG; // negative sector is on top at endpoint 0
                    BowtieFlags.IsAboveNeg = true;

                    z3 = S2_height0;
                    zz3 = S1_height1;
                    z2 = S1_height0;
                    zz2 = S2_height1;
                }
            }
        }
        #endregion

        #region V2 / Renderstuff       
        /// <summary>
        /// Gets line segment of wall (2D)
        /// </summary>
        /// <returns></returns>
        public V2 GetP1P2()
        {
            return P2 - P1;
        }

        /// <summary>
        /// Checks if this wall blocks a
        /// move between Start and End
        /// </summary>
        /// <param name="Start">A 3D location</param>
        /// <param name="End">A 2D location</param>
        /// <param name="PlayerHeight">Height of the player for ceiling collisions</param>
        /// <returns></returns>
        public bool IsBlockingMove(V3 Start, V2 End, Real PlayerHeight)
        {          
            // get distance of end to finite line segment
            Real distEnd = End.MinSquaredDistanceToLineSegment(P1, P2);

            // end is far enough away, no block
            if (distEnd >= GeometryConstants.WALLMINDISTANCE2)
                return false;

            /*************************************************************************/
            // end is too 'too' close to wall
            
            V2 start2D      = new V2(Start.X, Start.Z);
            int startside   = start2D.GetSide(P1, P2);
            int endside     = End.GetSide(P1, P2);
            Real endheight;
            ushort bgfbmp;

            /*************************************************************************/
            // allow moving away from wall

            if (startside == endside)
            { 
                Real distStart = start2D.MinSquaredDistanceToLineSegment(P1, P2);

                if (distEnd > distStart)
                    return false;
            }

            /*************************************************************************/
            // prevent moving through non-passable side

            if ((startside < 0 && LeftSide != null && !LeftSide.Flags.IsPassable) ||
                (startside > 0 && RightSide != null && !RightSide.Flags.IsPassable))
                return true;         

            /*************************************************************************/
            // check step-height

            endheight = 0.0f;

            if (startside >= 0)
            {
                endheight = (LeftSector != null) ? LeftSector.CalculateFloorHeight(End.X, End.Y, true) : 0.0f;
                bgfbmp = (RightSide != null) ? RightSide.LowerTexture : (ushort)0;
            }
            else
            {
                endheight = (RightSector != null) ? RightSector.CalculateFloorHeight(End.X, End.Y, true) : 0.0f;
                bgfbmp = (LeftSide != null) ? LeftSide.LowerTexture : (ushort)0;
            }

            if (bgfbmp > 0 && (endheight - Start.Y > GeometryConstants.MAXSTEPHEIGHT))
                return true;
            
            /*************************************************************************/
            // check head collision with ceiling

            endheight = 0.0f;

            if (startside >= 0)
            {
                endheight = (LeftSector != null) ? LeftSector.CalculateCeilingHeight(End.X, End.Y) : 0.0f;
                bgfbmp = (RightSide != null) ? RightSide.UpperTexture : (ushort)0;
            }
            else
            {
                endheight = (RightSector != null) ? RightSector.CalculateCeilingHeight(End.X, End.Y) : 0.0f;
                bgfbmp = (LeftSide != null) ? LeftSide.UpperTexture : (ushort)0;
            }

            if (bgfbmp > 0 && (endheight < Start.Y + PlayerHeight))
                return true;

            /*************************************************************************/

            return false;
        }
        
        /// <summary>
        /// Checks if this wall blocks
        /// a view ray from Start to End
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns>True if blocked, false if OK</returns>
        public bool IsBlockingSight(V3 Start, V3 End)
        {
            // 2D
            V2 Start2D = new V2(Start.X, Start.Z);
            V2 End2D = new V2(End.X, End.Z);

            // calculate the sides of the points (returns -1, 0 or 1)
            int startside = Start2D.GetSide(P1, P2);
            int endside = End2D.GetSide(P1, P2);

            // if points are not on same side
            // the infinite lines cross
            if (startside != endside)
            {
                // verify also the finite line segments cross
                V2 intersect;
                LineLineIntersectionType intersecttype = 
                    MathUtil.IntersectLineLine(Start2D, End2D, P1, P2, out intersect);

                if (intersecttype == LineLineIntersectionType.OneIntersection ||
                    intersecttype == LineLineIntersectionType.OneBoundaryPoint ||
                    intersecttype == LineLineIntersectionType.FullyCoincide ||
                    intersecttype == LineLineIntersectionType.PartiallyCoincide)
                {
                    // the vector/ray between start and end
                    V3 diff = End - Start;
                    
                    // solve 3d linear equation to get rayheight R2
                    // (height of possible intersection)
                    // 
                    // ( R1 )   (P1)            (Q1)
                    // ( R2 ) = (P2) + lambda * (Q2)
                    // ( R3 )   (P3)            (Q3)
                    // 
                    // using:
                    // R = intersect
                    // P = Start
                    // Q = diff (Direction)

                    // get lambda scale
                    Real lambda = 1.0f;
                    if (diff.X != 0)
                        lambda = (intersect.X - Start.X) / diff.X;

                    else if (diff.Z != 0)
                        lambda = (intersect.Y - Start.Z) / diff.Z;

                    // calculate the rayheight based on linear 3d equation
                    Real rayheight = Start.Y + lambda * diff.Y;

                    // compare height with wallheights
                    // use average of both endpoints (in case its sloped)
                    // do not care about the sides
                    Real h3 = (z3 + zz3) / 2.0f;
                    Real h2 = (z2 + zz2) / 2.0f;
                    Real h1 = (z1 + zz1) / 2.0f;
                    Real h0 = (z0 + zz0) / 2.0f;
                    bool a, b;
                    
                    // test upper part
                    a = (startside <= 0 && LeftSide != null && LeftSide.ResourceUpper != null && (LeftSide.Flags.IsNoLookThrough || !LeftSide.Flags.IsTransparent));
                    b = (startside >= 0 && RightSide != null && RightSide.ResourceUpper != null && (RightSide.Flags.IsNoLookThrough || !RightSide.Flags.IsTransparent));
                    if ((a || b) &&
                        rayheight < h3 &&
                        rayheight > h2)
                        return true;

                    // test middle part
                    a = (startside <= 0 && LeftSide != null && LeftSide.ResourceMiddle != null && (LeftSide.Flags.IsNoLookThrough || !LeftSide.Flags.IsTransparent));
                    b = (startside >= 0 && RightSide != null && RightSide.ResourceMiddle != null && (RightSide.Flags.IsNoLookThrough || !RightSide.Flags.IsTransparent));
                    if ((a || b) &&
                        rayheight < h2 &&
                        rayheight > h1)
                        return true;

                    // test lower part (nolookthrough)
                    a = (startside <= 0 && LeftSide != null && LeftSide.ResourceLower != null);
                    b = (startside >= 0 && RightSide != null && RightSide.ResourceLower != null);
                    if ((a || b) &&
                        rayheight < h1 &&
                        rayheight > h0)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Modifies a 2D vector to slide along this wall
        /// Based on projection.
        /// </summary>
        /// <param name="Begin"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public V2 SlideAlong(V2 Begin, V2 End)
        {
            V2 diff = End - Begin;
            V2 projection = diff.GetProjection(GetP1P2());
            return Begin + projection;         
        }

        /// <summary>
        /// Returns ready to display render information for a sidepart of this wall.
        /// Note: Z component is the height.
        /// </summary>
        /// <param name="PartType">Which part (upper, middle, lower)</param>
        /// <param name="IsLeftSide">Left or Right side</param>
        /// <param name="TexWidth">Texture width</param>
        /// <param name="TexHeight">Texture height</param>
        /// <param name="TexShrink">Texture shrink</param>
        /// <param name="Scale">Additional scale for vertices</param>
        /// <returns></returns>
        public VertexData GetVertexData(WallPartType PartType, bool IsLeftSide, int TexWidth, int TexHeight, int TexShrink, Real Scale = 1.0f)
        {
            VertexData RI = new VertexData();
            bool drawTopDown = true;
            RooSideDefFlags flags;
            int xoffset = 0;
            int yoffset = 0;

            // fill vars based on left or right side
            if (!IsLeftSide)
            {
                RI.P0.X = P1.X;
                RI.P3.X = P2.X;
                RI.P1.X = P1.X;
                RI.P2.X = P2.X;

                RI.P0.Y = P1.Y;
                RI.P3.Y = P2.Y;
                RI.P1.Y = P1.Y;
                RI.P2.Y = P2.Y;

                flags = RightSide.Flags;
                xoffset = RightXOffset;
                yoffset = RightYOffset;

                switch (PartType)
                {
                    case WallPartType.Upper:
                        RI.P0.Z = z3;
                        RI.P3.Z = zz3;
                        RI.P1.Z = z2;
                        RI.P2.Z = zz2;
                        drawTopDown = !RightSide.Flags.IsAboveBottomUp;
                        break;

                    case WallPartType.Middle:
                        RI.P0.Z = z2;
                        RI.P3.Z = zz2;
                        RI.P1.Z = z1;
                        RI.P2.Z = zz1;
                        drawTopDown = RightSide.Flags.IsNormalTopDown;
                        break;

                    case WallPartType.Lower:
                        if (BowtieFlags.IsBelowPos || BowtieFlags.IsBelowNeg)
                        {
                            RI.P0.Z = z1Neg;
                            RI.P3.Z = zz1Neg;
                        }
                        else
                        {
                            RI.P0.Z = z1;
                            RI.P3.Z = zz1;
                        }

                        RI.P1.Z = z0;
                        RI.P2.Z = zz0;
                        drawTopDown = RightSide.Flags.IsBelowTopDown;
                        break;
                }
            }
            else
            {
                RI.P0.X = P2.X;
                RI.P3.X = P1.X;
                RI.P1.X = P2.X;
                RI.P2.X = P1.X;

                RI.P0.Y = P2.Y;
                RI.P3.Y = P1.Y;
                RI.P1.Y = P2.Y;
                RI.P2.Y = P1.Y;

                flags = LeftSide.Flags;
                xoffset = LeftXOffset;
                yoffset = LeftYOffset;

                switch (PartType)
                {
                    case WallPartType.Upper:
                        RI.P0.Z = zz3;
                        RI.P3.Z = z3;
                        RI.P1.Z = zz2;
                        RI.P2.Z = z2;
                        drawTopDown = !LeftSide.Flags.IsAboveBottomUp;
                        break;

                    case WallPartType.Middle:
                        RI.P0.Z = zz2;
                        RI.P3.Z = z2;
                        RI.P1.Z = zz1;
                        RI.P2.Z = z1;
                        drawTopDown = LeftSide.Flags.IsNormalTopDown;
                        break;

                    case WallPartType.Lower:
                        RI.P0.Z = zz1;
                        RI.P3.Z = z1;
                        RI.P1.Z = zz0;
                        RI.P2.Z = z0;
                        drawTopDown = LeftSide.Flags.IsBelowTopDown;
                        break;
                }
            }

            // scales
            Real invWidth = 1.0f / (Real)TexWidth;
            Real invHeight = 1.0f / (Real)TexHeight;
            Real invWidthFudge = 1.0f / (Real)(TexWidth << 4);
            Real invHeightFudge = 1.0f / (Real)(TexHeight << 4);

            // Start with UV calculation, see d3drender.c ---           
            Real u1 = (Real)xoffset * (Real)TexShrink * invHeight;
            Real u2 = u1 + (ClientLength * (Real)TexShrink * invHeight);

            // set U
            RI.UV0.X = u1;
            RI.UV1.X = u1;
            RI.UV3.X = u2;
            RI.UV2.X = u2;

            // calculate V
            int bottom, top;
            if (!drawTopDown)
            {
                if (RI.P1.Z == RI.P2.Z)
                    bottom = (int)RI.P1.Z;
                else
                {
                    bottom = (int)MathUtil.Min(RI.P1.Z, RI.P2.Z);
                    bottom = bottom & ~(GeometryConstants.FINENESS - 1);
                }

                if (RI.P0.Z == RI.P3.Z)
                    top = (int)RI.P0.Z;
                else
                {
                    top = (int)MathUtil.Max(RI.P0.Z, RI.P3.Z);
                    top = (top + GeometryConstants.FINENESS - 1) & ~(GeometryConstants.FINENESS - 1);
                }

                if (RI.P1.Z == RI.P2.Z)
                {
                    RI.UV1.Y = 1.0f - ((Real)yoffset * (Real)TexShrink * invWidth);
                    RI.UV2.Y = 1.0f - ((Real)yoffset * (Real)TexShrink * invWidth);
                }
                else
                {
                    RI.UV1.Y = 1.0f - ((Real)yoffset * (Real)TexShrink * invWidth);
                    RI.UV2.Y = 1.0f - ((Real)yoffset * (Real)TexShrink * invWidth);
                    RI.UV1.Y -= ((Real)RI.P1.Z - bottom) * (Real)TexShrink * invWidthFudge;
                    RI.UV2.Y -= ((Real)RI.P2.Z - bottom) * (Real)TexShrink * invWidthFudge;
                }

                RI.UV0.Y = RI.UV1.Y - ((Real)(RI.P0.Z - RI.P1.Z) * (Real)TexShrink * invWidthFudge);
                RI.UV3.Y = RI.UV2.Y - ((Real)(RI.P3.Z - RI.P2.Z) * (Real)TexShrink * invWidthFudge);
            }

            // else, need to place tex origin at top left
            else
            {
                if (RI.P0.Z == RI.P3.Z)
                    top = (int)RI.P0.Z;
                else
                {
                    top = (int)MathUtil.Max(RI.P0.Z, RI.P3.Z);
                    top = (top + GeometryConstants.FINENESS - 1) & ~(GeometryConstants.FINENESS - 1);
                }

                if (RI.P1.Z == RI.P2.Z)
                    bottom = (int)RI.P1.Z;
                else
                {
                    bottom = (int)MathUtil.Min(RI.P1.Z, RI.P2.Z);
                    bottom = bottom & ~(GeometryConstants.FINENESS - 1);
                }

                if (RI.P0.Z == RI.P3.Z)
                {
                    RI.UV0.Y = 0.0f;
                    RI.UV3.Y = 0.0f;
                }
                else
                {
                    RI.UV0.Y = ((Real)top - RI.P0.Z) * (Real)TexShrink * invWidthFudge;
                    RI.UV3.Y = ((Real)top - RI.P3.Z) * (Real)TexShrink * invWidthFudge;
                }

                RI.UV0.Y -= ((Real)(yoffset * TexShrink) * invWidth);
                RI.UV3.Y -= ((Real)(yoffset * TexShrink) * invWidth);

                RI.UV1.Y = RI.UV0.Y + ((RI.P0.Z - RI.P1.Z) * (Real)TexShrink * invWidthFudge);
                RI.UV2.Y = RI.UV3.Y + ((RI.P3.Z - RI.P2.Z) * (Real)TexShrink * invWidthFudge);
            }

            // backwards
            if (flags.IsBackwards)
            {
                Real temp;

                temp = RI.UV3.X;
                RI.UV3.X = RI.UV0.X;
                RI.UV0.X = temp;

                temp = RI.UV2.X;
                RI.UV2.X = RI.UV1.X;
                RI.UV1.X = temp;
            }

            // no vtile 
            // seems to apply only to middle parts, at least for bottom it creates strange holes
            if (flags.IsNoVTile && PartType == WallPartType.Middle)
            {
                if (RI.UV0.Y < 0.0f)
                {
                    Real tex, wall, ratio, temp;

                    tex = RI.UV1.Y - RI.UV0.Y;
                    if (tex == 0)
                        tex = 1.0f;
                    temp = -RI.UV0.Y;
                    ratio = temp / tex;

                    wall = RI.P0.Z - RI.P1.Z;
                    temp = wall * ratio;
                    RI.P0.Z -= temp;
                    RI.UV0.Y = 0.0f;
                }
                if (RI.UV3.Y < 0.0f)
                {
                    Real tex, wall, ratio, temp;

                    tex = RI.UV2.Y - RI.UV3.Y;
                    if (tex == 0)
                        tex = 1.0f;
                    temp = -RI.UV3.Y;
                    ratio = temp / tex;

                    wall = RI.P3.Z - RI.P2.Z;
                    temp = wall * ratio;
                    RI.P3.Z -= temp;
                    RI.UV3.Y = 0.0f;
                }

                RI.P1.Z -= 16.0f;
                RI.P2.Z -= 16.0f;
            }

            RI.UV0.Y += 1.0f / TexWidth;
            RI.UV3.Y += 1.0f / TexWidth;
            RI.UV1.Y -= 1.0f / TexWidth;
            RI.UV2.Y -= 1.0f / TexWidth;

            // scale by user scale
            RI.P0 *= Scale;
            RI.P3 *= Scale;
            RI.P1 *= Scale;
            RI.P2 *= Scale;

            // calculate the normal
            V3 P0P1 = (RI.P1 - RI.P0);
            V3 P0P2 = (RI.P2 - RI.P0);
            RI.Normal = P0P1.CrossProduct(P0P2);
            RI.Normal.Normalize();
            RI.Normal = -RI.Normal;
            return RI;
        }

        /// <summary>
        /// Splits this wall into two using infinite line given by Q1Q2.
        /// </summary>
        /// <param name="Q1"></param>
        /// <param name="Q2"></param>
        /// <returns>Item1: P1 to I. Item2: I to P2</returns>
        public Tuple<RooWall, RooWall> Split(V2 Q1, V2 Q2)
        {
            V2 intersect;

            // intersect this wall (finite line) with the infinite line given by Q1Q2
            LineInfiniteLineIntersectionType intersecttype = 
                MathUtil.IntersectLineInfiniteLine(P1, P2, Q1, Q2, out intersect);

            // must have a real intersection, not only boundarypoint or even coincide
            if (intersecttype != LineInfiniteLineIntersectionType.OneIntersection)
                return null;

            /*****************************************************************/

            // 1) Piece from P1 to intersection
            RooWall wall1 = new RooWall(
                RooVersion,
                NextWallNumInPlane,
                RightSideNum,
                LeftSideNum,
                P1,
                intersect,
                RightXOffset,  // readjust below
                LeftXOffset,   // readjust below
                RightYOffset,  // readjust below
                LeftYOffset,   // readjust below
                RightSectorNum,
                LeftSectorNum
                );

            // also keep references of old wall
            wall1.RightSector = RightSector;
            wall1.LeftSector = LeftSector;
            wall1.RightSide = RightSide;
            wall1.LeftSide = LeftSide;
            wall1.BowtieFlags = BowtieFlags;
            wall1.CalculateWallSideHeights();

            /*****************************************************************/

            // 2) Piece from intersection to P2
            RooWall wall2 = new RooWall(
                RooVersion,
                NextWallNumInPlane,
                RightSideNum,
                LeftSideNum,
                intersect,
                P2,
                RightXOffset,  // readjust below
                LeftXOffset,   // readjust below
                RightYOffset,  // readjust below
                LeftYOffset,   // readjust below
                RightSectorNum,
                LeftSectorNum
                );

            // also keep references of old wall
            wall2.RightSector = RightSector;
            wall2.LeftSector = LeftSector;
            wall2.RightSide = RightSide;
            wall2.LeftSide = LeftSide;
            wall2.BowtieFlags = BowtieFlags;
            wall2.CalculateWallSideHeights();

            /*****************************************************************/

            // 3) Readjust texture offsets to accoutn for split

            // RightSide
            if (wall1.RightSide != null && wall1.RightSide.Flags.IsBackwards)
                wall1.RightXOffset += (short)wall2.ClientLength;
            else wall2.RightXOffset += (short)wall1.ClientLength;

            // LeftSide (Do this backwards, because client exchanges vertices of negative walls)
            if (wall1.LeftSide != null && wall1.LeftSide.Flags.IsBackwards)
                wall2.LeftXOffset += (short)wall1.ClientLength;
            else wall1.LeftXOffset += (short)wall2.ClientLength;

            /*****************************************************************/

            return new Tuple<RooWall, RooWall>(wall1, wall2);
        }
        #endregion
    }
}
