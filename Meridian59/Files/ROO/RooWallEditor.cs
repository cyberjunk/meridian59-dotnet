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
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;
using Meridian59.Common;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// This class implements a wall as it is saved and shown by the old roomedit.
    /// </summary>
    /// <remarks>
    /// They significantly differ from RooWall:
    ///  (a) They can start or end at negative coordinates
    ///  (b) Their Y axis/coordinate is flipped
    ///  (c) Their scale is 1:64 rather than 1:1024
    ///  (d) They can get split up into two or more RooWall in BSP building
    /// </remarks>
    [Serializable]
    public class RooWallEditor : IByteSerializableFast
    {
        #region IByteSerializable
        public int ByteLength 
        {
            get 
            { 
                return TypeSizes.SHORT + TypeSizes.SHORT 
                    + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT
                    + TypeSizes.SHORT + TypeSizes.SHORT
                    + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT; 
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(FileSideDef1), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(FileSideDef2), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Side1XOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Side2XOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Side1YOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Side2YOffset), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Side1Sector), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Side2Sector), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(X0), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y0), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(X1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((short*)Buffer) = FileSideDef1;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = FileSideDef2;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = Side1XOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = Side2XOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = Side1YOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = Side2YOffset;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = Side1Sector;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = Side2Sector;
            Buffer += TypeSizes.SHORT;

            *((int*)Buffer) = X0;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Y0;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = X1;
            Buffer += TypeSizes.INT;
            
            *((int*)Buffer) = Y1;
            Buffer += TypeSizes.INT;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            FileSideDef1 = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            FileSideDef2 = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Side1XOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Side2XOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Side1YOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Side2YOffset = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Side1Sector = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Side2Sector = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            // roomedit uses 16 bit coordinates for own walls, roo format has 32 bit
            // early editors write garbage into the high 16 bit (fix is up..)
            // so we ignore them here for now

            X0 = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y0 = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.INT;

            X1 = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y1 = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.INT;
            
            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            FileSideDef1 = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            FileSideDef2 = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            Side1XOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            Side2XOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            Side1YOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            Side2YOffset = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            Side1Sector = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            Side2Sector = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            // roomedit uses 16 bit coordinates for own walls, roo format has 32 bit
            // early editors write garbage into the high 16 bit (fix is up..)
            // so we ignore them here for now

            X0 = *((short*)Buffer);  
            Buffer += TypeSizes.INT;

            Y0 = *((short*)Buffer);
            Buffer += TypeSizes.INT;

            X1 = *((short*)Buffer);
            Buffer += TypeSizes.INT;

            Y1 = *((short*)Buffer);
            Buffer += TypeSizes.INT;
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

        protected V2 p0;
        protected V2 p1;

        public int Num { get; set; }
        public short FileSideDef1 { get; set; }
        public short FileSideDef2 { get; set; }
        public short Side1XOffset { get; set; }
        public short Side2XOffset { get; set; }
        public short Side1YOffset { get; set; }
        public short Side2YOffset { get; set; }
        public short Side1Sector { get; set; }
        public short Side2Sector { get; set; }

        /// <summary>
        /// Start point of this line
        /// </summary>
        public V2 P0 { get { return p0; } set { p0 = value; } }

        /// <summary>
        /// End point of this line.
        /// </summary>
        public V2 P1 { get { return p1; } set { p1 = value; } }

        /// <summary>
        /// Returns or sets X component of P0 as integer
        /// </summary>
        public int X0 { get { return (int)p0.X; } set { p0.X = value; } }

        /// <summary>
        /// Returns or sets Y component of P0 as integer
        /// </summary>
        public int Y0 { get { return (int)p0.Y; } set { p0.Y = value; } }

        /// <summary>
        /// Returns or sets X component of P1 as integer
        /// </summary>
        public int X1 { get { return (int)p1.X; } set { p1.X = value; } }

        /// <summary>
        /// Returns or sets Y component of P1 as integer
        /// </summary>
        public int Y1 { get { return (int)p1.Y; } set { p1.Y = value; } }
        
        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="FileSideDef1"></param>
        /// <param name="FileSideDef2"></param>
        /// <param name="Side1XOffset"></param>
        /// <param name="Side2XOffset"></param>
        /// <param name="Side1YOffset"></param>
        /// <param name="Side2YOffset"></param>
        /// <param name="Side1Sector"></param>
        /// <param name="Side2Sector"></param>
        /// <param name="X0"></param>
        /// <param name="Y0"></param>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        public RooWallEditor(
            short FileSideDef1, short FileSideDef2,
            short Side1XOffset, short Side2XOffset,
            short Side1YOffset, short Side2YOffset,
            short Side1Sector, short Side2Sector,
            int X0, int Y0,
            int X1, int Y1)
        {
            this.FileSideDef1 = FileSideDef1;
            this.FileSideDef2 = FileSideDef2;
            this.Side1XOffset = Side1XOffset;
            this.Side2XOffset = Side2XOffset;
            this.Side1YOffset = Side1YOffset;
            this.Side2YOffset = Side2YOffset;
            this.Side1Sector = Side1Sector;
            this.Side2Sector = Side2Sector;
            this.X0 = X0;
            this.Y0 = Y0;
            this.X1 = X1;
            this.Y1 = Y1;            
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RooWallEditor(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Constructor by native parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe RooWallEditor(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }

        /// <summary>
        /// Creates a RooWall instance based on this RooWallEditor instance.
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Room"></param>
        /// <returns></returns>
        public RooWall ToRooWall(uint RooVersion, RooFile Room)
        {
            if (Room == null)
                return null;

            V2 q1, q2;

            // first try get boundingbox as defined by 'Things'
            BoundingBox2D box = Room.GetBoundingBox2DFromThings();
            
            // no thingsbox? build based on editorwalls
            if (box == BoundingBox2D.NULL)
                box = Room.GetBoundingBox2D(false);
   
            // 1) Convert from 1:64 to 1:1024
            // 2) Modify coordinate system (y-axis different)
            q1.X = (P0.X - box.Min.X) * 16f;
            q1.Y = (box.Max.Y - P0.Y) * 16f;
            q2.X = (P1.X - box.Min.X) * 16f;
            q2.Y = (box.Max.Y - P1.Y) * 16f;

            // sidenum in editorwall is  0 to n ( 0=unset)
            // sectnum in editorwall is -1 to n (-1=unset)

            RooWall wall = new RooWall(
                RooVersion,
                0,
                (ushort)this.FileSideDef1, // no +1
                (ushort)this.FileSideDef2, // no +1
                q1, 
                q2,
                Side1XOffset, 
                Side2XOffset, 
                Side1YOffset, 
                Side2YOffset,
                (ushort)(Side1Sector + 1),  // +1 mapping 
                (ushort)(Side2Sector + 1)); // +1 mapping

            // now resolve the object references from indices
            // and fill in heights
            wall.ResolveIndices(Room);
            wall.CalculateWallSideHeights();

            // done
            return wall;
        }
    }
}
