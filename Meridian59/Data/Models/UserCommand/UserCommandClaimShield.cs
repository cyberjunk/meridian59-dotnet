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

namespace Meridian59.Data.Models
{
    [Serializable]
    public class UserCommandClaimShield : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.ClaimShield; } }

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                // CommandType + Color1 + Color2 + Design + ReallyClaim
                return TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE+ TypeSizes.BYTE;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;
            cursor++;

            Buffer[cursor] = Color1;
            cursor++;

            Buffer[cursor] = Color2;
            cursor++;

            Buffer[cursor] = Design;
            cursor++;

            Buffer[cursor] = ReallyClaim;
            cursor++;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if ((UserCommandType)Buffer[cursor] != CommandType)
                throw new Exception(ERRORWRONGTYPEBYTE);
            else
            {
                cursor++; 

                Color1 = Buffer[cursor];
                cursor++;

                Color2 = Buffer[cursor];
                cursor++;

                Design = Buffer[cursor];
                cursor++;

                ReallyClaim = Buffer[cursor];
                cursor++;
            }

            return cursor - StartIndex;
        }      
        #endregion

        public byte Color1 { get; set; }
        public byte Color2 { get; set; }
        public byte Design { get; set; }
        public byte ReallyClaim { get; set; }

        public UserCommandClaimShield(byte Color1, byte Color2, byte Design, byte ReallyClaim)
        {
            this.Color1 = Color1;
            this.Color2 = Color2;
            this.Design = Design;
            this.ReallyClaim = ReallyClaim;
        }

        public UserCommandClaimShield(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
