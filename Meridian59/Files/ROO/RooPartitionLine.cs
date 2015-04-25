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
using Meridian59.Common.Interfaces;
using System;

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

            Array.Copy(BitConverter.GetBytes(A), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(B), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(C), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

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

            *((int*)Buffer) = A;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = B;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = C;
            Buffer += TypeSizes.INT;

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

            A = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            B = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            C = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

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

            A = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            B = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            C = *((int*)Buffer);
            Buffer += TypeSizes.INT;

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
        public override byte Type { get { return RooBSPItem.PartitionLineType; } }
        
        /// <summary>
        /// 'a' variable for line equation ax+bc+c=0
        /// </summary>
        public int A { get; set; }

        /// <summary>
        /// 'b' variable for line equation ax+bc+c=0
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// 'c' variable for line equation ax+bc+c=0
        /// </summary>
        public int C { get; set; }
        
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

        public RooPartitionLine( 
            int X1, int Y1, int X2, int Y2,
            int A, int B, int C,
            ushort Right, ushort Left, 
            ushort LineDefReference)
            : base(X1, X2, Y1, Y2)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.Right = Right;
            this.Left = Left;
            this.WallReference = LineDefReference;            
        }

        public RooPartitionLine(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe RooPartitionLine(ref byte* Buffer)
            : base(ref Buffer) { }

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
    }
}
