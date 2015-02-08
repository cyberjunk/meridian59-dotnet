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
    /// This kind of data is used by the original WINDEU for linedefs/walls.
    /// </summary>
    [Serializable]
    public class RooWallEditor : IByteSerializableFast
    {
        #region IByteSerializable implementation
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

            X0 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y0 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            X1 = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y1 = BitConverter.ToInt32(Buffer, cursor);
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

            X0 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y0 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            X1 = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y1 = *((int*)Buffer);
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

        public short FileSideDef1 { get; set; }
        public short FileSideDef2 { get; set; }
        public short Side1XOffset { get; set; }
        public short Side2XOffset { get; set; }
        public short Side1YOffset { get; set; }
        public short Side2YOffset { get; set; }
        public short Side1Sector { get; set; }
        public short Side2Sector { get; set; }
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        
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

        public RooWallEditor(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe RooWallEditor(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
    }
}
