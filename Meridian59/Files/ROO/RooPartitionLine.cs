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
    /// A PartitionLine is a tree node in BSP-Tree.
    /// </summary>
    [Serializable]
    public class RooPartitionLine : RooBSPItem
    {       
        #region IByteSerializable
        public override int ByteLength 
        {
            get { 
                return base.ByteLength + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT 
                    + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT; }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            { 
                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(A)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(B)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(C)), 0, Buffer, cursor, TypeSizes.INT);
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
            }

            Array.Copy(BitConverter.GetBytes(Right), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Left), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(WallReference), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                *((int*)Buffer) = Convert.ToInt32(A);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(B);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(C);
                Buffer += TypeSizes.INT;
            }
            else
            {
                *((float*)Buffer) = (float)A;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)B;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)C;
                Buffer += TypeSizes.FLOAT;
            }

            *((ushort*)Buffer) = Right;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = Left;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = WallReference;
            Buffer += TypeSizes.SHORT;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                A = (Real)BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                B = (Real)BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                C = (Real)BitConverter.ToInt32(Buffer, cursor);
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
            }

            Right = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Left = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            WallReference = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                A = (Real)(*((int*)Buffer));
                Buffer += TypeSizes.INT;

                B = (Real)(*((int*)Buffer));
                Buffer += TypeSizes.INT;

                C = (Real)(*((int*)Buffer));
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
            }

            Right = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Left = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            WallReference = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;
        }

        #endregion

        #region Properties
        /// <summary>
        /// PartitionLineType for RooPartitionLine
        /// </summary>
        public override NodeType Type { get { return RooBSPItem.NodeType.Node; } }

        /// <summary>
        /// 'a' variable for line equation ax+by+c=0
        /// </summary>
        public Real A { get; set; }

        /// <summary>
        /// 'b' variable for line equation ax+by+c=0
        /// </summary>
        public Real B { get; set; }

        /// <summary>
        /// 'c' variable for line equation ax+by+c=0
        /// </summary>
        public Real C { get; set; }
        
        /// <summary>
        /// Index of right child
        /// </summary>
        public ushort Right { get; set; }

        /// <summary>
        /// Index of left child
        /// </summary>
        public ushort Left { get; set; }
        
        /// <summary>
        /// Index of wall used as splitter
        /// </summary>
        public ushort WallReference { get; set; }

        /// <summary>
        /// Reference to wall used as splitter or NULL.
        /// Will be filled in ResolveIndices().
        /// </summary>
        public RooWall Wall { get; set; }

        /// <summary>
        /// Reference to right child.
        /// Will be filled in ResolveIndices().
        /// </summary>
        public RooBSPItem RightChild { get; set; }

        /// <summary>
        /// Reference to left child.
        /// Will be filled in ResolveIndices().
        /// </summary>
        public RooBSPItem LeftChild { get; set; }

        #endregion

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="BoundingBox"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="Right"></param>
        /// <param name="Left"></param>
        /// <param name="LineDefReference"></param>
        public RooPartitionLine(
            uint RooVersion,
            BoundingBox2D BoundingBox,
            Real A, Real B, Real C,
            ushort Right, ushort Left, 
            ushort LineDefReference) : base(RooVersion)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.Right = Right;
            this.Left = Left;
            this.WallReference = LineDefReference;
            this.BoundingBox = BoundingBox;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RooPartitionLine(uint RooVersion, byte[] Buffer, int StartIndex = 0)
            : base(RooVersion, Buffer, StartIndex) { }

        /// <summary>
        /// Constructor by native parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        public unsafe RooPartitionLine(uint RooVersion, ref byte* Buffer)
            : base(RooVersion, ref Buffer) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RooFile"></param>
        public override void ResolveIndices(RooFile RooFile)
        {
            // indices properties are not zero-based, but the arrays/lists are

            // get reference to parent wall
            if (WallReference > 0 &&
                RooFile.Walls.Count > WallReference - 1)
            {
                Wall = RooFile.Walls[WallReference - 1];
            }

            // get right tree child
            if (Right > 0 &&
                RooFile.BSPTree.Count > Right - 1)
            {
                RightChild = RooFile.BSPTree[Right - 1];
            }

            // get left tree child
            if (Left > 0 &&
                RooFile.BSPTree.Count > Left - 1)
            {
                LeftChild = RooFile.BSPTree[Left - 1];
            }
        }

        /// <summary>
        /// Returns the distance of point P from this infinite splitter line.
        /// Uses the line equation coefficients from properties (A,B,C).
        /// Sign of value gives the side.
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public Real GetDistance(V2 P)
        {
            return A * P.X + B * P.Y + C;
        }
    }
}
