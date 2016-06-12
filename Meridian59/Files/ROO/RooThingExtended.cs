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
using System.Text;

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// Extended RooThing with more info (new, barely used)
    /// </summary>
    [Serializable]
    public class RooThingExtended : RooThing
    {
        protected const int COMMENTSLENGTH = 64;

        #region IByteSerializable
        public override int ByteLength 
        {
            get 
            {
                return base.ByteLength +
                    TypeSizes.INT + TypeSizes.INT +
                    TypeSizes.INT + TypeSizes.INT +
                    TypeSizes.INT + TypeSizes.INT + 
                    COMMENTSLENGTH;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Type), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Angle), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(When), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(ExitPositionX), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(ExitPositionY), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Flags), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(Util.Encoding.GetBytes(Comments), 0, Buffer, cursor, Comments.Length);
            cursor += COMMENTSLENGTH;
          
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            *((int*)Buffer) = Type;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Angle;
            Buffer += TypeSizes.INT;

            base.WriteTo(ref Buffer);

            *((int*)Buffer) = When;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = ExitPositionX;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = ExitPositionY;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = Flags;
            Buffer += TypeSizes.INT;

            fixed (char* pString = Comments)
            {
                int a, b; bool c;
                Util.Encoding.GetEncoder().Convert(pString, Comments.Length, Buffer, COMMENTSLENGTH, true, out a, out b, out c);
                Buffer += COMMENTSLENGTH;
            }  
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Type = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Angle = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            cursor += base.ReadFrom(Buffer, cursor);

            When = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ExitPositionX = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ExitPositionY = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Flags = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Comments = Util.Encoding.GetString(Buffer, cursor, COMMENTSLENGTH);
            cursor += COMMENTSLENGTH;

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            Type = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Angle = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            base.ReadFrom(ref Buffer);

            When = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            ExitPositionX = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            ExitPositionY = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Flags = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            Comments = new string((sbyte*)Buffer, 0, COMMENTSLENGTH);
            Comments = Comments.Replace("\0", String.Empty);

            Buffer += COMMENTSLENGTH;
        }
        #endregion

        public int Type { get; set; }
        public int Angle { get; set; }
        public int When { get; set; }
        public int ExitPositionX { get; set; }
        public int ExitPositionY { get; set; }
        public int Flags { get; set; }
        public string Comments { get; set; }

        public RooThingExtended(
            int Type,
            int Angle,
            int PositionX,
            int PositionY,
            int When,
            int ExitPositionX,
            int ExitPositionY,
            int Flags,
            string Comments)
            : base(PositionX, PositionY)
        {
            this.Type = Type;
            this.Angle = Angle;
            this.When = When;
            this.ExitPositionX = ExitPositionX;
            this.ExitPositionY = ExitPositionY;
            this.Flags = Flags;

            // handle comments

            if (Comments == null)
                this.Comments = String.Empty;

            else if (Comments.Length > COMMENTSLENGTH)
                this.Comments = Comments.Substring(0, COMMENTSLENGTH);

            else
                this.Comments = Comments;
        }

        public RooThingExtended(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe RooThingExtended(ref byte* Buffer)
            : base(ref Buffer) { }
    }
}
