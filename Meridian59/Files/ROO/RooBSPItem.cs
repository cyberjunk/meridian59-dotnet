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

namespace Meridian59.Files.ROO
{
    [Serializable]
    public abstract class RooBSPItem : IByteSerializableFast, IRooIndicesResolvable
    {
        /// <summary>
        /// Different types of nodes in a tree.
        /// Node = Has at least one child.
        /// Leaf = Has no children
        /// </summary>
        public enum NodeType : byte
        {
            Node = 0x01, Leaf=0x02
        }

        #region IByteSerializable
        public virtual int ByteLength
        {
            get { return TypeSizes.BYTE + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT; }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (byte)Type;
            cursor++;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(boundingBox.Min.X)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(boundingBox.Min.Y)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(boundingBox.Max.X)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(boundingBox.Max.Y)), 0, Buffer, cursor, TypeSizes.INT);
                cursor += TypeSizes.INT;
            }
            else
            {
                Array.Copy(BitConverter.GetBytes((float)boundingBox.Min.X), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)boundingBox.Min.Y), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)boundingBox.Max.X), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;

                Array.Copy(BitConverter.GetBytes((float)boundingBox.Max.Y), 0, Buffer, cursor, TypeSizes.FLOAT);
                cursor += TypeSizes.FLOAT;
            }

            return cursor - StartIndex;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)Type;
            Buffer++;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                *((int*)Buffer) = Convert.ToInt32(boundingBox.Min.X);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(boundingBox.Min.Y);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(boundingBox.Max.X);
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = Convert.ToInt32(boundingBox.Max.Y);
                Buffer += TypeSizes.INT;
            }
            else
            {
                *((float*)Buffer) = (float)boundingBox.Min.X;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)boundingBox.Min.Y;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)boundingBox.Max.X;
                Buffer += TypeSizes.FLOAT;

                *((float*)Buffer) = (float)boundingBox.Max.Y;
                Buffer += TypeSizes.FLOAT;
            }
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            // no need to read type
            cursor++;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                boundingBox.Min.X = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                boundingBox.Min.Y = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                boundingBox.Max.X = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                boundingBox.Max.Y = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;
            }
            else
            {
                boundingBox.Min.X = BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                boundingBox.Min.Y = BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                boundingBox.Max.X = BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;

                boundingBox.Max.Y = BitConverter.ToSingle(Buffer, cursor);
                cursor += TypeSizes.FLOAT;
            }

            return cursor - StartIndex;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            // no need to read type
            Buffer++;

            if (RooVersion < RooFile.VERSIONFLOATCOORDS)
            {
                boundingBox.Min.X = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                boundingBox.Min.Y = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                boundingBox.Max.X = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                boundingBox.Max.Y = *((int*)Buffer);
                Buffer += TypeSizes.INT;
            }
            else
            {
                boundingBox.Min.X = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                boundingBox.Min.Y = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                boundingBox.Max.X = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;

                boundingBox.Max.Y = *((float*)Buffer);
                Buffer += TypeSizes.FLOAT;
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

        protected BoundingBox2D boundingBox;

        /// <summary>
        /// RooVersion of the fileformat. Important for parsers.
        /// </summary>
        public uint RooVersion { get; set; }

        /// <summary>
        /// Abstract. Type to set by deriving subclasses.
        /// </summary>
        public abstract NodeType Type { get; }

        /// <summary>
        /// A 2D boundingbox of this BSP node (splitter or leaf).
        /// </summary>
        public BoundingBox2D BoundingBox { get { return boundingBox; } set { boundingBox = value; } }

        /// <summary>
        /// BoundingBox minimum X of this node (or leaf).
        /// </summary>
        public int X1 { get { return (int)boundingBox.Min.X; } set { boundingBox.Min.X = value; } }

        /// <summary>
        /// BoundingBox minimum Y of this node (or leaf).
        /// </summary>
        public int Y1 { get { return (int)boundingBox.Min.Y; } set { boundingBox.Min.Y = value; } }

        /// <summary>
        /// BoundingBox maximum X of this node (or leaf).
        /// </summary>
        public int X2 { get { return (int)boundingBox.Max.X; } set { boundingBox.Max.X = value; } }

        /// <summary>
        /// BoundingBox maximum Y of this node (or leaf).
        /// </summary>
        public int Y2 { get { return (int)boundingBox.Max.Y; } set { boundingBox.Max.Y = value; } }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="RooVersion"></param>
        public RooBSPItem(uint RooVersion)
        {
            this.RooVersion = RooVersion;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RooBSPItem(uint RooVersion, byte[] Buffer, int StartIndex = 0)
        {
            this.RooVersion = RooVersion;

            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Constructor by native parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        public unsafe RooBSPItem(uint RooVersion, ref byte* Buffer)
        {
            this.RooVersion = RooVersion;

            ReadFrom(ref Buffer);
        }

        /// <summary>
        /// Abstract. Must be implemented.
        /// </summary>
        /// <param name="RooFile"></param>
        public abstract void ResolveIndices(RooFile RooFile);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        public static RooBSPItem ExtractBSPItem(uint RooVersion, byte[] Buffer, int StartIndex)
        {          
            switch ((NodeType)Buffer[StartIndex])
            {
                case NodeType.Node:
                    return new RooPartitionLine(RooVersion, Buffer, StartIndex);
                    
                case NodeType.Leaf:
                    return new RooSubSector(RooVersion, Buffer, StartIndex);

                default:
                    throw new Exception("Unsupported BSPItem type: "+Buffer[StartIndex]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        public static unsafe RooBSPItem ExtractBSPItem(uint RooVersion, ref byte* Buffer)
        {
            switch ((NodeType)Buffer[0])
            {
                case NodeType.Node:
                    return new RooPartitionLine(RooVersion, ref Buffer);

                case NodeType.Leaf:
                    return new RooSubSector(RooVersion, ref Buffer);

                default:
                    throw new Exception("Unsupported BSPItem type: " + Buffer[0]);
            }
        }
    }
}
