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
    [Serializable]
    public class RooVertex : IByteSerializableFast
    {
        #region IByteSerializable
        public int ByteLength 
        {
            get { return TypeSizes.INT + TypeSizes.INT; }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(X), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Y), 0, Buffer, cursor, TypeSizes.INT);        
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((int*)Buffer) = X;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Y;
            Buffer += TypeSizes.INT;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            X = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Y = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            X = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Y = *((int*)Buffer);
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

        public int X { get; set; }
        public int Y { get; set; }
      
        public RooVertex(int X, int Y)
        {
            this.X = X;
            this.Y = Y;          
        }

        public RooVertex(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe RooVertex(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
    }
}
