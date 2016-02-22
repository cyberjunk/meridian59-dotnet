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
    /// Several 2D grids as stored in ROO files
    /// </summary>
    [Serializable]
    public class RooGrids : IByteSerializableFast
    {
        #region Constants
        protected const byte MASK_NORTH = 1;
        protected const byte MASK_NORTH_EAST = 1 << 1;
        protected const byte MASK_EAST = 1 << 2;
        protected const byte MASK_SOUTH_EAST = 1 << 3;
        protected const byte MASK_SOUTH = 1 << 4;
        protected const byte MASK_SOUTH_WEST = 1 << 5;
        protected const byte MASK_WEST = 1 << 6;
        protected const byte MASK_NORTH_WEST = 1 << 7;

        protected const byte ROOM_FLAG_WALKABLE = 0x01;
        #endregion

        #region IByteSerializable
        public virtual int ByteLength 
        {
            get 
            {
                if (RooVersion >= RooFile.VERSIONNOGRIDS)
                    return 0;

                // rows + cols
                int len = TypeSizes.INT + TypeSizes.INT;

                // movementgrid
                len += (Rows * Columns);

                // flaggrid
                len += (Rows * Columns);

                // monstergrid
                if (RooVersion >= RooFile.VERSIONMONSTERGRID)
                    len += (Rows * Columns);

                if (RooVersion >= RooFile.VERSIONHIGHRESGRID)
                {
                    // rows + cols
                    len += TypeSizes.INT + TypeSizes.INT;

                    // highresgrid uses 4 byte per square
                    len += RowsHighRes * ColumnsHighRes * TypeSizes.INT;
                }
                return len;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            if (RooVersion >= RooFile.VERSIONNOGRIDS)
                return 0;

            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Rows), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Columns), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // write movement grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Buffer[cursor] = GridMovement[i, j];
                    cursor++;
                }
            }

            // write flag grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Buffer[cursor] = GridFlag[i, j];
                    cursor++;
                }
            }

            // write monster grid
            if (RooVersion >= RooFile.VERSIONMONSTERGRID)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        Buffer[cursor] = GridMonster[i, j];
                        cursor++;
                    }
                }
            }

            // write highres grid
            if (RooVersion >= RooFile.VERSIONHIGHRESGRID)
            {
                Array.Copy(BitConverter.GetBytes(RowsHighRes), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(ColumnsHighRes), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                for (int i = 0; i < RowsHighRes; i++)
                {
                    for (int j = 0; j < ColumnsHighRes; j++)
                    {
                        Array.Copy(BitConverter.GetBytes(GridHighRes[i, j]), 0, Buffer, cursor, TypeSizes.INT);
                        cursor += TypeSizes.INT;
                    }
                }
            }

            return cursor - StartIndex;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            if (RooVersion >= RooFile.VERSIONNOGRIDS)
                return;

            *((int*)Buffer) = Rows;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Columns;
            Buffer += TypeSizes.INT;

            // write movement grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Buffer[0] = GridMovement[i, j];
                    Buffer++;
                }
            }

            // write flag grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Buffer[0] = GridFlag[i, j];
                    Buffer++;
                }
            }

            // write monster grid
            if (RooVersion >= RooFile.VERSIONMONSTERGRID)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        Buffer[0] = GridMonster[i, j];
                        Buffer++;
                    }
                }
            }

            // write highres grid
            if (RooVersion >= RooFile.VERSIONHIGHRESGRID)
            {
                *((int*)Buffer) = RowsHighRes;
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = ColumnsHighRes;
                Buffer += TypeSizes.INT;

                for (int i = 0; i < RowsHighRes; i++)
                {
                    for (int j = 0; j < ColumnsHighRes; j++)
                    {
                        *((uint*)Buffer) = GridHighRes[i, j];
                        Buffer += TypeSizes.INT;
                    }
                }
            }
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            if (RooVersion >= RooFile.VERSIONNOGRIDS)
                return 0;

            int cursor = StartIndex;

            Rows = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Columns = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
                     
            // init grids
            GridMovement = new byte[Rows, Columns];
            GridFlag = new byte[Rows, Columns];
            GridMonster = new byte[Rows, Columns];

            // load movement grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridMovement[i, j] = Buffer[cursor];
                    cursor++;
                }
            }

            // load flag grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridFlag[i, j] = Buffer[cursor];
                    cursor++;
                }
            }

            // load monster grid
            if (RooVersion >= RooFile.VERSIONMONSTERGRID)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        GridMonster[i, j] = Buffer[cursor];
                        cursor++;
                    }
                }
            }

            // load highres grid
            if (RooVersion >= RooFile.VERSIONHIGHRESGRID)
            {
                RowsHighRes = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                ColumnsHighRes = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                GridHighRes = new uint[RowsHighRes, ColumnsHighRes];

                for (int i = 0; i < RowsHighRes; i++)
                {
                    for (int j = 0; j < ColumnsHighRes; j++)
                    {
                        GridHighRes[i, j] = BitConverter.ToUInt32(Buffer, cursor);
                        cursor += TypeSizes.INT;
                    }
                }
            }

            return cursor - StartIndex;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            if (RooVersion >= RooFile.VERSIONNOGRIDS)
                return;

            Rows = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Columns = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            // init grids
            GridMovement = new byte[Rows, Columns];
            GridFlag = new byte[Rows, Columns];
            GridMonster = new byte[Rows, Columns];

            // load movement grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridMovement[i, j] = Buffer[0];
                    Buffer++;
                }
            }

            // load flag grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridFlag[i, j] = Buffer[0];
                    Buffer++;
                }
            }

            // load monster grid
            if (RooVersion >= RooFile.VERSIONMONSTERGRID)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        GridMonster[i, j] = Buffer[0];
                        Buffer++;
                    }
                }
            }

            // load highres grid
            if (RooVersion >= RooFile.VERSIONHIGHRESGRID)
            {
                RowsHighRes = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                ColumnsHighRes = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                GridHighRes = new uint[RowsHighRes, ColumnsHighRes];

                for (int i = 0; i < RowsHighRes; i++)
                {
                    for (int j = 0; j < ColumnsHighRes; j++)
                    {
                        GridHighRes[i, j] = *((uint*)Buffer);
                        Buffer += TypeSizes.INT;
                    }
                }
            }
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
        /// Controls which grids are used/written/read
        /// </summary>
        public uint RooVersion { get; protected set; }

        /// <summary>
        /// Rows count of the 2D grids
        /// </summary>
        public int Rows { get; protected set; }

        /// <summary>
        /// Columns count of the 2D grids
        /// </summary>
        public int Columns { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[,] GridMovement { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[,] GridFlag { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[,] GridMonster { get; protected set; }

        /// <summary>
        /// Rows count of the high resolution grid
        /// </summary>
        public int RowsHighRes { get; protected set; }

        /// <summary>
        /// Columns count of the high resolution grid
        /// </summary>
        public int ColumnsHighRes { get; protected set; }

        /// <summary>
        /// New high resolution grid with heights
        /// </summary>
        public uint[,] GridHighRes { get; protected set; }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Rows"></param>
        /// <param name="Columns"></param>
        /// <param name="RowsHighRes"></param>
        /// <param name="ColumnsHighRes"></param>
        public RooGrids(
            uint RooVersion,
            int Rows,
            int Columns,
            int RowsHighRes,
            int ColumnsHighRes)
        {
            this.RooVersion = RooVersion;
            this.Rows = Rows;
            this.Columns = Columns;
            this.RowsHighRes = RowsHighRes;
            this.ColumnsHighRes = ColumnsHighRes;

            // init grids
            GridMovement = new byte[Rows, Columns];
            GridFlag = new byte[Rows, Columns];
            GridMonster = new byte[Rows, Columns];
            GridHighRes = new uint[RowsHighRes, ColumnsHighRes];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RooGrids(uint RooVersion, byte[] Buffer, int StartIndex = 0)
        {
            this.RooVersion = RooVersion;

            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        public unsafe RooGrids(uint RooVersion, ref byte* Buffer)
        {
            this.RooVersion = RooVersion;

            ReadFrom(ref Buffer);
        }
        #endregion

        #region Visualisation
#if DRAWING
        public void WriteGridMonster(RooFile file, string Filename)
        {
            System.Drawing.Bitmap bmp = new
                System.Drawing.Bitmap(file.Grids.Columns * 3, file.Grids.Rows * 3);

            int x = 1;
            int y = 1;
            for (int i = 0; i < file.Grids.Rows; i++)
            {
                x = 1;

                for (int j = 0; j < file.Grids.Columns; j++)
                {
                    if (file.Grids.IsWalkable(i, j))
                        bmp.SetPixel(x, y, System.Drawing.Color.White);
                    else
                        bmp.SetPixel(x, y, System.Drawing.Color.Black);

                    if (file.Grids.IsWalkNorthMonster(i, j))
                        bmp.SetPixel(x, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x, y - 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkNorthEastMonster(i, j))
                        bmp.SetPixel(x + 1, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y - 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkEastMonster(i, j))
                        bmp.SetPixel(x + 1, y, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthEastMonster(i, j))
                        bmp.SetPixel(x + 1, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthMonster(i, j))
                        bmp.SetPixel(x, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthWestMonster(i, j))
                        bmp.SetPixel(x - 1, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkWestMonster(i, j))
                        bmp.SetPixel(x - 1, y, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkNorthWestMonster(i, j))
                        bmp.SetPixel(x - 1, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y - 1, System.Drawing.Color.Red);


                    x += 3;
                }

                y += 3;
            }

            bmp.Save(Filename);
            bmp.Dispose();
        }

        public void WriteGridMove(RooFile file, string Filename)
        {
            System.Drawing.Bitmap bmp =
                new System.Drawing.Bitmap(file.Grids.Columns * 3, file.Grids.Rows * 3);

            int x = 1;
            int y = 1;
            for (int i = 0; i < file.Grids.Rows; i++)
            {
                x = 1;

                for (int j = 0; j < file.Grids.Columns; j++)
                {
                    if (file.Grids.IsWalkable(i, j))
                        bmp.SetPixel(x, y, System.Drawing.Color.White);
                    else
                        bmp.SetPixel(x, y, System.Drawing.Color.Black);

                    if (file.Grids.IsWalkNorth(i, j))
                        bmp.SetPixel(x, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x, y - 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkNorthEast(i, j))
                        bmp.SetPixel(x + 1, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y - 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkEast(i, j))
                        bmp.SetPixel(x + 1, y, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthEast(i, j))
                        bmp.SetPixel(x + 1, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouth(i, j))
                        bmp.SetPixel(x, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthWest(i, j))
                        bmp.SetPixel(x - 1, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkWest(i, j))
                        bmp.SetPixel(x - 1, y, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkNorthWest(i, j))
                        bmp.SetPixel(x - 1, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y - 1, System.Drawing.Color.Red);


                    x += 3;
                }

                y += 3;
            }

            bmp.Save(Filename);
            bmp.Dispose();
        }

        public void WriteGridHighRes(RooFile file, string Filename)
        {
            System.Drawing.Bitmap bmp = 
                new System.Drawing.Bitmap(file.Grids.ColumnsHighRes * 3, file.Grids.RowsHighRes * 3);

            int x = 1;
            int y = 1;
            for (int i = 0; i < file.Grids.RowsHighRes; i++)
            {
                x = 1;

                for (int j = 0; j < file.Grids.ColumnsHighRes; j++)
                {
                    if (file.Grids.IsWalkableHighRes(i, j))
                        bmp.SetPixel(x, y, System.Drawing.Color.White);
                    else
                        bmp.SetPixel(x, y, System.Drawing.Color.Black);

                    if (file.Grids.IsWalkNorthHighRes(i, j))
                        bmp.SetPixel(x, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x, y - 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkNorthEastHighRes(i, j))
                        bmp.SetPixel(x + 1, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y - 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkEastHighRes(i, j))
                        bmp.SetPixel(x + 1, y, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthEastHighRes(i, j))
                        bmp.SetPixel(x + 1, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x + 1, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthHighRes(i, j))
                        bmp.SetPixel(x, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkSouthWestHighRes(i, j))
                        bmp.SetPixel(x - 1, y + 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y + 1, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkWestHighRes(i, j))
                        bmp.SetPixel(x - 1, y, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y, System.Drawing.Color.Red);

                    if (file.Grids.IsWalkNorthWestHighRes(i, j))
                        bmp.SetPixel(x - 1, y - 1, System.Drawing.Color.Green);
                    else
                        bmp.SetPixel(x - 1, y - 1, System.Drawing.Color.Red);


                    x += 3;
                }

                y += 3;
            }

            bmp.Save(Filename);
            bmp.Dispose();
        }
#endif
        #endregion

        #region GridFlag
        public bool IsWalkable(int Row, int Col)
        {
            return 
                (GridFlag[Row, Col] & ROOM_FLAG_WALKABLE) == ROOM_FLAG_WALKABLE;            
        }
        public void SetWalkable(int Row, int Col, bool Value)
        {          
            if (Value) 
                GridFlag[Row, Col] |= ROOM_FLAG_WALKABLE;

            else 
                GridFlag[Row, Col] = (byte)(GridFlag[Row, Col] & ~ROOM_FLAG_WALKABLE);            
        }
        #endregion

        #region GridMovement
        public bool IsWalkNorth(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_NORTH) == MASK_NORTH;
        }
        public void SetWalkNorth(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_NORTH;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_NORTH);
        }

        public bool IsWalkNorthEast(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_NORTH_EAST) == MASK_NORTH_EAST;
        }
        public void SetWalkNorthEast(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_NORTH_EAST;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_NORTH_EAST);
        }

        public bool IsWalkEast(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_EAST) == MASK_EAST;
        }
        public void SetWalkEast(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_EAST;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_EAST);
        }

        public bool IsWalkSouthEast(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_SOUTH_EAST) == MASK_SOUTH_EAST;
        }
        public void SetWalkSouthEast(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_SOUTH_EAST;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_SOUTH_EAST);
        }

        public bool IsWalkSouth(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_SOUTH) == MASK_SOUTH;
        }
        public void SetWalkSouth(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_SOUTH;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_SOUTH);
        }

        public bool IsWalkSouthWest(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_SOUTH_WEST) == MASK_SOUTH_WEST;
        }
        public void SetWalkSouthWest(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_SOUTH_WEST;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_SOUTH_WEST);
        }

        public bool IsWalkWest(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_WEST) == MASK_WEST;
        }
        public void SetWalkWest(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_WEST;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_WEST);
        }

        public bool IsWalkNorthWest(int Row, int Col)
        {
            return
                (GridMovement[Row, Col] & MASK_NORTH_WEST) == MASK_NORTH_WEST;
        }
        public void SetWalkNorthWest(int Row, int Col, bool Value)
        {
            if (Value)
                GridMovement[Row, Col] |= MASK_NORTH_WEST;

            else
                GridMovement[Row, Col] = (byte)(GridMovement[Row, Col] & ~MASK_NORTH_WEST);
        }
        #endregion

        #region GridMonster
        public bool IsWalkNorthMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_NORTH) == MASK_NORTH;
        }
        public void SetWalkNorthMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_NORTH;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_NORTH);
        }

        public bool IsWalkNorthEastMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_NORTH_EAST) == MASK_NORTH_EAST;
        }
        public void SetWalkNorthEastMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_NORTH_EAST;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_NORTH_EAST);
        }

        public bool IsWalkEastMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_EAST) == MASK_EAST;
        }
        public void SetWalkEastMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_EAST;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_EAST);
        }

        public bool IsWalkSouthEastMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_SOUTH_EAST) == MASK_SOUTH_EAST;
        }
        public void SetWalkSouthEastMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_SOUTH_EAST;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_SOUTH_EAST);
        }

        public bool IsWalkSouthMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_SOUTH) == MASK_SOUTH;
        }
        public void SetWalkSouthMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_SOUTH;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_SOUTH);
        }

        public bool IsWalkSouthWestMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_SOUTH_WEST) == MASK_SOUTH_WEST;
        }
        public void SetWalkSouthWestMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_SOUTH_WEST;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_SOUTH_WEST);
        }

        public bool IsWalkWestMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_WEST) == MASK_WEST;
        }
        public void SetWalkWestMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_WEST;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_WEST);
        }

        public bool IsWalkNorthWestMonster(int Row, int Col)
        {
            return
                (GridMonster[Row, Col] & MASK_NORTH_WEST) == MASK_NORTH_WEST;
        }
        public void SetWalkNorthWestMonster(int Row, int Col, bool Value)
        {
            if (Value)
                GridMonster[Row, Col] |= MASK_NORTH_WEST;

            else
                GridMonster[Row, Col] = (byte)(GridMonster[Row, Col] & ~MASK_NORTH_WEST);
        }
        #endregion

        #region GridHighRes
        public bool IsWalkableHighRes(int Row, int Col)
        {
            return
                (GridHighRes[Row, Col] & ROOM_FLAG_WALKABLE) == ROOM_FLAG_WALKABLE;
        }
        public void SetWalkableHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= ROOM_FLAG_WALKABLE;

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~ROOM_FLAG_WALKABLE);
        }


        public bool IsWalkNorthHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_NORTH) == MASK_NORTH;
        }
        public void SetWalkNorthHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_NORTH << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_NORTH << 1));
        }

        public bool IsWalkNorthEastHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_NORTH_EAST) == MASK_NORTH_EAST;
        }
        public void SetWalkNorthEastHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_NORTH_EAST << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_NORTH_EAST << 1));
        }

        public bool IsWalkEastHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_EAST) == MASK_EAST;
        }
        public void SetWalkEastHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_EAST << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_EAST << 1));
        }

        public bool IsWalkSouthEastHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_SOUTH_EAST) == MASK_SOUTH_EAST;
        }
        public void SetWalkSouthEastHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_SOUTH_EAST << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_SOUTH_EAST << 1));
        }

        public bool IsWalkSouthHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_SOUTH) == MASK_SOUTH;
        }
        public void SetWalkSouthHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_SOUTH << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_SOUTH << 1));
        }

        public bool IsWalkSouthWestHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_SOUTH_WEST) == MASK_SOUTH_WEST;
        }
        public void SetWalkSouthWestHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_SOUTH_WEST << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_SOUTH_WEST << 1));
        }

        public bool IsWalkWestHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_WEST) == MASK_WEST;
        }
        public void SetWalkWestHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_WEST << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_WEST << 1));
        }

        public bool IsWalkNorthWestHighRes(int Row, int Col)
        {
            return
                ((GridHighRes[Row, Col] >> 1) & MASK_NORTH_WEST) == MASK_NORTH_WEST;
        }
        public void SetWalkNorthWestHighRes(int Row, int Col, bool Value)
        {
            if (Value)
                GridHighRes[Row, Col] |= (MASK_NORTH_WEST << 1);

            else
                GridHighRes[Row, Col] = (uint)(GridHighRes[Row, Col] & ~(MASK_NORTH_WEST << 1));
        }
        #endregion
    }
}
