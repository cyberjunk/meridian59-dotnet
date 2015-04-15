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
        #region IByteSerializable
        public int ByteLength 
        {
            get 
            {
                return TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT +
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT +
                    TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT +
                    TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(ServerID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightSideReference), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftSideReference), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(X1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(X2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(ClientLength), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightXOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftXOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightYOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftYOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RightSectorReference), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LeftSectorReference), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {            
            *((short*)Buffer) = ServerID;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = RightSideReference;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = LeftSideReference;
            Buffer += TypeSizes.SHORT;

            *((int*)Buffer) = X1;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Y1;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = X2;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Y2;
            Buffer += TypeSizes.INT;

            *((ushort*)Buffer) = ClientLength;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = RightXOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = LeftXOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = RightYOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = LeftYOffset;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = RightSectorReference;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = LeftSectorReference;
            Buffer += TypeSizes.SHORT;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            ServerID = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightSideReference = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftSideReference = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            X1 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y1 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            X2 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y2 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ClientLength = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightXOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftXOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightYOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftYOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            RightSectorReference = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LeftSectorReference = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ServerID = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightSideReference = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftSideReference = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            X1 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y1 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            X2 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y2 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            ClientLength = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightXOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftXOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightYOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftYOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            RightSectorReference = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            LeftSectorReference = *((ushort*)Buffer);
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
        
        /// <summary>
        /// Number of this wall (1 based)
        /// </summary>
        public int Num { get; set; }
        
        /// <summary>
        /// ID on the server (0 if none)
        /// </summary>
        public short ServerID { get; set; }
        
        /// <summary>
        /// Index of right(pos) side in array/list
        /// </summary>
        public ushort RightSideReference { get; set; }
        
        /// <summary>
        /// Index of left(neg) side in array/list
        /// </summary>
        public ushort LeftSideReference { get; set; }

        // first point (from)
        public int X1 { get; set; }
        public int Y1 { get; set; }

        // second point (to)
        public int X2 { get; set; }
        public int Y2 { get; set; }

        // length
        public ushort ClientLength { get; set; }

        // texture offsets
        public short RightXOffset { get; set; }
        public short LeftXOffset { get; set; }
        public short RightYOffset { get; set; }
        public short LeftYOffset { get; set; }

        // indices of "sector" instances
        public ushort RightSectorReference { get; set; }
        public ushort LeftSectorReference { get; set; }

        public RooSector RightSector { get; set; }
        public RooSector LeftSector { get; set; }
        public RooSideDef RightSide { get; set; }
        public RooSideDef LeftSide { get; set; }

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
        public Real Z0 { get; set; }      /* height of bottom of lower wall */
        public Real Z1 { get; set; }      /* height of top of lower wall / bottom of normal wall */
        public Real Z2 { get; set; }      /* height of top of normal wall / bottom of upper wall */
        public Real Z3 { get; set; }      /* height of top of upper wall */

        // Z-coordinates at (X2,Y2)
        public Real ZZ0 { get; set; }     /* height of bottom of lower wall */
        public Real ZZ1 { get; set; }     /* height of top of lower wall / bottom of normal wall */
        public Real ZZ2 { get; set; }     /* height of top of normal wall / bottom of upper wall */
        public Real ZZ3 { get; set; }     /* height of top of upper wall */

        #endregion

        public BowtieFlags BowtieFlags { get; set; }

        #endregion

        #region Constructors
        public RooWall(short ServerID, 
            ushort RightSideReference, ushort LeftSideReference, 
            int X1, int Y1, int X2, int Y2, ushort ClientLength, 
            short RightXOffset, short LeftXOffset, short RightYOffset, short LeftYOffset,
            ushort RightSectorReference, ushort LeftSectorReference)
        {
            this.ServerID = ServerID;
            this.RightSideReference = RightSideReference;
            this.LeftSideReference = LeftSideReference;
            this.X1 = X1;
            this.Y1 = Y1;
            this.X2 = X2;
            this.Y2 = Y2;
            this.ClientLength = ClientLength;
            this.RightXOffset = RightXOffset;
            this.LeftXOffset = LeftXOffset;
            this.RightYOffset = RightYOffset;
            this.LeftYOffset = LeftYOffset;
            this.RightSectorReference = RightSectorReference;
            this.LeftSectorReference = LeftSectorReference;

            this.BowtieFlags = new BowtieFlags();
        }

        public RooWall(byte[] Buffer, int StartIndex = 0)
        {
            BowtieFlags = new BowtieFlags();
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe RooWall(ref byte* Buffer)
        {
            BowtieFlags = new BowtieFlags();
            ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates object references from the indices for easier access.
        /// </summary>
        /// <param name="RooFile"></param>
        public void ResolveIndices(RooFile RooFile)
        {
            // indices properties are not zero-based, but the arrays/lists are

            // get reference to right SectorDef
            if (RightSectorReference > 0 && 
                RooFile.Sectors.Count > RightSectorReference - 1)
            {
                RightSector = RooFile.Sectors[RightSectorReference - 1];

                // save as adjacent wall
                if (!RightSector.Walls.Contains(this))
                    RightSector.Walls.Add(this);
            }

            // get reference to left SectorDef
            if (LeftSectorReference > 0 &&
                RooFile.Sectors.Count > LeftSectorReference - 1)
            {
                LeftSector = RooFile.Sectors[LeftSectorReference - 1];

                // save as adjacent wall
                if (!LeftSector.Walls.Contains(this))
                    LeftSector.Walls.Add(this);
            }

            // get reference to right SideDef
            if (RightSideReference > 0 &&
                RooFile.SideDefs.Count > RightSideReference - 1)
            {
                RightSide = RooFile.SideDefs[RightSideReference - 1];

                // save as adjacent side
                if (RightSector != null && !RightSector.Sides.Contains(RightSide))
                    RightSector.Sides.Add(RightSide);

                // save as adjacent side
                if (LeftSector != null && !LeftSector.Sides.Contains(RightSide))
                    LeftSector.Sides.Add(RightSide);
            }

            // get reference to left SideDef
            if (LeftSideReference > 0 &&
                RooFile.SideDefs.Count > LeftSideReference - 1)
            {
                LeftSide = RooFile.SideDefs[LeftSideReference - 1];

                // save as adjacent side
                if (RightSector != null && !RightSector.Sides.Contains(LeftSide))
                    RightSector.Sides.Add(LeftSide);

                // save as adjacent side
                if (LeftSector != null && !LeftSector.Sides.Contains(LeftSide))
                    LeftSector.Sides.Add(LeftSide);
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
                Z0 = Z1 = 0.0f;
                Z2 = Z3 = (Real)GeometryConstants.FINENESS;
                ZZ0 = ZZ1 = 0.0f;
                ZZ2 = ZZ3 = (Real)GeometryConstants.FINENESS;
                return;
            }

            // only left sector? use heights from there and return
            if (RightSector == null)
            {
                Z0 = Z1 = LeftSector.CalculateFloorHeight(X1, Y1);
                Z2 = Z3 = LeftSector.CalculateCeilingHeight(X1, Y1);
                ZZ0 = ZZ1 = LeftSector.CalculateFloorHeight(X2, Y2);
                ZZ2 = ZZ3 = LeftSector.CalculateCeilingHeight(X2, Y2);
                return;
            }

            // only right sector? use heights from there and return
            if (LeftSector == null)
            {
                Z0 = Z1 = RightSector.CalculateFloorHeight(X1, Y1);
                Z2 = Z3 = RightSector.CalculateCeilingHeight(X1, Y1);
                ZZ0 = ZZ1 = RightSector.CalculateFloorHeight(X2, Y2);
                ZZ2 = ZZ3 = RightSector.CalculateCeilingHeight(X2, Y2);
                return;
            }

            // --  finally, if there are both sectors available ---

            // start with the floor handling
            Real S1_height0 = RightSector.CalculateFloorHeight(X1, Y1);
            Real S2_height0 = LeftSector.CalculateFloorHeight(X1, Y1);
            Real S1_height1 = RightSector.CalculateFloorHeight(X2, Y2);
            Real S2_height1 = LeftSector.CalculateFloorHeight(X2, Y2);

            // S1 is above S2 at first endpoint
            if (S1_height0 > S2_height0)
            {
                if (S1_height1 >= S2_height1)
                {
                    // normal wall - S1 higher at both ends
                    BowtieFlags.Value =  0;

                    Z1 = S1_height0;
                    ZZ1 = S1_height1;
                    Z0 = S2_height0;
                    ZZ0 = S2_height1;
                }
                else
                {
                    // bowtie handling
                    BowtieFlags.IsBelowPos = true;

                    // no extra zNeg here
                    Z1 = S1_height0;
                    ZZ1 = S2_height1;
                    Z0 = S2_height0;
                    ZZ0 = S1_height1;
                }
            }

            // S2 above S1 at first endpoint
            else
            {                
                if (S2_height1 >= S1_height1)
                {
                    // normal wall - S2 higher at both ends
                    BowtieFlags.Value = 0;

                    Z1 = S2_height0;
                    ZZ1 = S2_height1;
                    Z0 = S1_height0;
                    ZZ0 = S1_height1;
                }
                else
                {
                    // bowtie handling
                    BowtieFlags.IsBelowNeg = true;

                    // no extra zNeg here
                    Z1 = S2_height0;
                    ZZ1 = S1_height1;
                    Z0 = S1_height0;
                    ZZ0 = S2_height1;

                }
            }

            // start with ceiling handling
            S1_height0 = RightSector.CalculateCeilingHeight(X1, Y1);
            S2_height0 = LeftSector.CalculateCeilingHeight(X1, Y1);
            S1_height1 = RightSector.CalculateCeilingHeight(X2, Y2);
            S2_height1 = LeftSector.CalculateCeilingHeight(X2, Y2);

            if (S1_height0 > S2_height0)
            {
                if (S1_height1 >= S2_height1)
                {
                    // normal wall - S1 higher at both ends
                    //wall->bowtie_bits &= (BYTE)~BT_ABOVE_BOWTIE; // Clear above bowtie bits
                    BowtieFlags.IsAboveBowtie = false;

                    Z3 = S1_height0;
                    ZZ3 = S1_height1;
                    Z2 = S2_height0;
                    ZZ2 = S2_height1;
                }
                else
                {
                    // bowtie - see notes on bowties above
                    //wall->bowtie_bits |= (BYTE)BT_ABOVE_POS; // positive sector is on top at endpoint 0
                    BowtieFlags.IsAbovePos = true;

                    Z3 = S1_height0;
                    ZZ3 = S2_height1;
                    Z2 = S2_height0;
                    ZZ2 = S1_height1;
                }
            }
            else
            {
                if (S2_height1 >= S1_height1)
                {
                    // normal wall - S2 higher at both ends
                    //wall->bowtie_bits &= (BYTE)~BT_ABOVE_BOWTIE;
                    BowtieFlags.IsAboveBowtie = false;

                    Z3 = S2_height0;
                    ZZ3 = S2_height1;
                    Z2 = S1_height0;
                    ZZ2 = S1_height1;
                }
                else
                {
                    // bowtie - see notes on bowties above
                    //wall->bowtie_bits |= (BYTE)BT_ABOVE_NEG; // negative sector is on top at endpoint 0
                    BowtieFlags.IsAboveNeg = true;

                    Z3 = S2_height0;
                    ZZ3 = S1_height1;
                    Z2 = S1_height0;
                    ZZ2 = S2_height1;
                }
            }
        }

        #endregion

        #region V2 / Renderstuff

        /// <summary>
        /// Gets first point of wall (2D)
        /// </summary>
        /// <returns></returns>
        public V2 GetP1()
        {
            return new V2(X1, Y1);
        }

        /// <summary>
        /// Gets second point of wall (2D)
        /// </summary>
        /// <returns></returns>
        public V2 GetP2()
        {
            return new V2(X2, Y2);
        }

        /// <summary>
        /// Gets line segment of wall (2D)
        /// </summary>
        /// <returns></returns>
        public V2 GetP1P2()
        {
            V2 P1 = GetP1();
            V2 P2 = GetP2();

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
        public bool IsBlocking(V3 Start, V2 End, Real PlayerHeight)
        {
            V2 P1 = GetP1();
            V2 P2 = GetP2();

            V2 Start2D = new V2(Start.X, Start.Z);

            // calculate the sides of the points (returns -1, 0 or 1)
            int startside = Start2D.GetSide(P1, P2);
            int endside = End.GetSide(P1, P2);

            // if points are not on same side
            // the infinite lines cross
            if (startside != endside)
            {
                // verify also the finite line segments cross
                V2 intersect;
                
                if (MathUtil.IntersectLineLine(Start2D, End, P1, P2, out intersect))
                {
                    // verify the side we've crossed is flaggged as "nonpassable"
                    // if so, we actually have a collision
                    if ((startside < 0 && LeftSide != null && !LeftSide.Flags.IsPassable) ||
                        (startside > 0 && RightSide != null && !RightSide.Flags.IsPassable))
                    {
                        return true;
                    }

                    // still check the stepheight from oldheight to new floor if passable
                    // for too high steps                    
                    Real endheight = 0.0f;
                    Real diff;

                    if (endside <= 0 && LeftSector != null)
                        endheight = LeftSector.CalculateFloorHeight((int)End.X, (int)End.Y, true);

                    else if (endside > 0 && RightSector != null)
                        endheight = RightSector.CalculateFloorHeight((int)End.X, (int)End.Y, true);

                    diff = endheight - Start.Y;

                    // diff is bigger than max. step height, we have a collision
                    if (diff > GeometryConstants.MAXSTEPHEIGHT)                   
                        return true;

                    // check the ceiling heights
                    if (endside <= 0 && LeftSector != null)
                        endheight = LeftSector.CalculateCeilingHeight((int)End.X, (int)End.Y);

                    else if (endside > 0 && RightSector != null)
                        endheight = RightSector.CalculateCeilingHeight((int)End.X, (int)End.Y);

                    // diff is bigger than max. step height, we have a collision
                    if (endheight < Start.Y + PlayerHeight)
                        return true;
                }
            }

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
            V2 P1 = GetP1();
            V2 P2 = GetP2();

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
                
                if (MathUtil.IntersectLineLine(Start2D, End2D, P1, P2, out intersect))
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
                    Real h3 = (Z3 + ZZ3) / 2.0f;
                    Real h2 = (Z2 + ZZ2) / 2.0f;
                    Real h1 = (Z1 + ZZ1) / 2.0f;
                    Real h0 = (Z0 + ZZ0) / 2.0f;
                    bool a, b;
                    
                    // test upper part
                    a = (LeftSide != null && LeftSide.ResourceUpper != null && (LeftSide.Flags.IsNoLookThrough || !LeftSide.Flags.IsTransparent));
                    b = (RightSide != null && RightSide.ResourceUpper != null && (RightSide.Flags.IsNoLookThrough || !RightSide.Flags.IsTransparent));
                    if ((a || b) &&
                        rayheight < h3 &&
                        rayheight > h2)
                        return true;

                    // test middle part
                    a = (LeftSide != null && LeftSide.ResourceMiddle != null && (LeftSide.Flags.IsNoLookThrough || !LeftSide.Flags.IsTransparent));
                    b = (RightSide != null && RightSide.ResourceMiddle != null && (RightSide.Flags.IsNoLookThrough || !RightSide.Flags.IsTransparent));
                    if ((a || b) &&
                        rayheight < h2 &&
                        rayheight > h1)
                        return true;

                    // test lower part (nolookthrough)
                    a = (LeftSide != null && LeftSide.ResourceLower != null);
                    b = (RightSide != null && RightSide.ResourceLower != null);
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
                RI.P0.X = X1;
                RI.P3.X = X2;
                RI.P1.X = X1;
                RI.P2.X = X2;

                RI.P0.Y = Y1;
                RI.P3.Y = Y2;
                RI.P1.Y = Y1;
                RI.P2.Y = Y2;

                flags = RightSide.Flags;
                xoffset = RightXOffset;
                yoffset = RightYOffset;

                switch (PartType)
                {
                    case WallPartType.Upper:
                        RI.P0.Z = Z3;
                        RI.P3.Z = ZZ3;
                        RI.P1.Z = Z2;
                        RI.P2.Z = ZZ2;
                        drawTopDown = !RightSide.Flags.IsAboveBottomUp;
                        break;

                    case WallPartType.Middle:
                        RI.P0.Z = Z2;
                        RI.P3.Z = ZZ2;
                        RI.P1.Z = Z1;
                        RI.P2.Z = ZZ1;
                        drawTopDown = RightSide.Flags.IsNormalTopDown;
                        break;

                    case WallPartType.Lower:
                        RI.P0.Z = Z1;
                        RI.P3.Z = ZZ1;
                        RI.P1.Z = Z0;
                        RI.P2.Z = ZZ0;
                        drawTopDown = RightSide.Flags.IsBelowTopDown;
                        break;
                }
            }
            else
            {
                RI.P0.X = X2;
                RI.P3.X = X1;
                RI.P1.X = X2;
                RI.P2.X = X1;

                RI.P0.Y = Y2;
                RI.P3.Y = Y1;
                RI.P1.Y = Y2;
                RI.P2.Y = Y1;

                flags = LeftSide.Flags;
                xoffset = LeftXOffset;
                yoffset = LeftYOffset;

                switch (PartType)
                {
                    case WallPartType.Upper:
                        RI.P0.Z = ZZ3;
                        RI.P3.Z = Z3;
                        RI.P1.Z = ZZ2;
                        RI.P2.Z = Z2;
                        drawTopDown = !LeftSide.Flags.IsAboveBottomUp;
                        break;

                    case WallPartType.Middle:
                        RI.P0.Z = ZZ2;
                        RI.P3.Z = Z2;
                        RI.P1.Z = ZZ1;
                        RI.P2.Z = Z1;
                        drawTopDown = LeftSide.Flags.IsNormalTopDown;
                        break;

                    case WallPartType.Lower:
                        RI.P0.Z = ZZ1;
                        RI.P3.Z = Z1;
                        RI.P1.Z = ZZ0;
                        RI.P2.Z = Z0;
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
            Real u2 = u1 + ((Real)ClientLength * (Real)TexShrink * invHeight);

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
                    RI.UV1.Y -= ((Real)RI.P1.Y - bottom) * (Real)TexShrink * invWidthFudge;
                    RI.UV2.Y -= ((Real)RI.P2.Y - bottom) * (Real)TexShrink * invWidthFudge;
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
        /// RenderInformation for a PART of a RooWall
        /// </summary>
        public struct VertexData
        {
            public V3 P0, P1, P2, P3;
            public V2 UV0, UV1, UV2, UV3;
            public V3 Normal;
        }

        #endregion

    }
}
