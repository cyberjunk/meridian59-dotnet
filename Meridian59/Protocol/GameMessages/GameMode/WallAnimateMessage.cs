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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Tells the client to animate a wall (i.e. change texture)
    /// </summary>
    public class WallAnimateMessage : GameModeMessage
    {       
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.SHORT + Animation.ByteLength + TypeSizes.BYTE;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
                
            Array.Copy(BitConverter.GetBytes(SideDefServerID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            cursor += Animation.WriteTo(Buffer, cursor);

            Buffer[cursor] = (byte)Action;
            cursor++;
                  
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            SideDefServerID = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Animation = Animation.ExtractAnimation(Buffer, cursor);
            cursor += Animation.ByteLength;

            Action = (RoomAnimationAction)Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);
            
            *((ushort*)Buffer) = SideDefServerID;
            Buffer += TypeSizes.SHORT;

            Animation.WriteTo(ref Buffer);

            Buffer[0] = (byte)Action;
            Buffer++;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            SideDefServerID = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Animation = Animation.ExtractAnimation(ref Buffer);          

            Action = (RoomAnimationAction)Buffer[0];
            Buffer++;
        }
        #endregion

        public ushort SideDefServerID { get; set; }
        public Animation Animation { get; set; }
        public RoomAnimationAction Action { get; set; }

        public WallAnimateMessage(ushort SideDefServerID, Animation Animation, RoomAnimationAction Action) 
            : base(MessageTypeGameMode.WallAnimate)
        {
            this.SideDefServerID = SideDefServerID;
            this.Animation = Animation;
            this.Action = Action;                                          
        }

        public WallAnimateMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }

        public unsafe WallAnimateMessage(ref byte* Buffer)
            : base(ref Buffer) { }
    }
}
