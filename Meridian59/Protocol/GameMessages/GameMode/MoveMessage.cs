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
using Meridian59.Protocol.Enums;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Tells the client about an updated position of a RoomObject
    /// </summary>
    public class MoveMessage : GameModeMessage
    {
        /// <summary>
        /// The upper bit of the transferred speed byte
        /// </summary>
        protected const byte ROTATETODEST = 0x80;

        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
#if !VANILLA && !OPENMERIDIAN
                return base.ByteLength + TypeSizes.INT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE + TypeSizes.SHORT;
#else
                return base.ByteLength + TypeSizes.INT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE;
#endif
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(ObjectID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(NewCoordinateY), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(NewCoordinateX), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = (byte)(ROTATETODEST | (byte)MovementSpeed);
            cursor++;

#if !VANILLA && !OPENMERIDIAN
            Array.Copy(BitConverter.GetBytes(Angle), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;
#endif
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ObjectID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            NewCoordinateY = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            NewCoordinateX = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            MovementSpeed = (MovementSpeed)(Buffer[cursor] & ~ROTATETODEST);
            RotateToDestination = ((Buffer[cursor] & ROTATETODEST) == ROTATETODEST);
            cursor++;

#if !VANILLA && !OPENMERIDIAN
            Angle = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;
#endif
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = ObjectID;
            Buffer += TypeSizes.INT;

            *((ushort*)Buffer) = NewCoordinateY;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = NewCoordinateX;
            Buffer += TypeSizes.SHORT;

            Buffer[0] = (byte)(ROTATETODEST | (byte)MovementSpeed);
            Buffer++;

#if !VANILLA && !OPENMERIDIAN
            *((ushort*)Buffer) = Angle;
            Buffer += TypeSizes.SHORT;
#endif
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            ObjectID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            NewCoordinateY = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            NewCoordinateX = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            MovementSpeed = (MovementSpeed)(Buffer[0] & ~ROTATETODEST);
            RotateToDestination = ((Buffer[0] & ROTATETODEST) == ROTATETODEST);
            Buffer++;

#if !VANILLA && !OPENMERIDIAN
            Angle = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;
#endif
        }

        #endregion

        public uint ObjectID { get; set; }
        public ushort NewCoordinateY { get; set; }
        public ushort NewCoordinateX { get; set; }
        public MovementSpeed MovementSpeed { get; set; }
        public ushort Angle { get; set; }

        /// <summary>
        /// Tells whether the object should also be rotated
        /// to face the destination of the move.
        /// </summary>
        public bool RotateToDestination { get; set; }

        public MoveMessage(uint ObjectID, ushort NewCoordinateX, ushort NewCoordinateY, MovementSpeed MovementSpeed, ushort Angle, bool RotateToDestination = false) 
            : base(MessageTypeGameMode.Move)
        {
            this.ObjectID = ObjectID;
            this.NewCoordinateY = NewCoordinateY;
            this.NewCoordinateX = NewCoordinateX;
            this.MovementSpeed = MovementSpeed;
            this.RotateToDestination = RotateToDestination;
            this.Angle = Angle;
        }

        public MoveMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe MoveMessage(ref byte* Buffer)
            : base(ref Buffer) { }
    }
}
