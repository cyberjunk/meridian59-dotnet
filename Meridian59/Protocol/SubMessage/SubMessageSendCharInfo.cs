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
using Meridian59.Common.Constants;

namespace Meridian59.Protocol.SubMessage
{
    [Serializable]
    public class SubMessageSendCharInfo : SubMessage
    {
        public override byte SubMessageType { get { return (byte)MessageTypeGameMode.SendCharInfo; } }

        #region IByteSerializable implementation
        public override int ByteLength { 
            get { 
                // CommandType
                return TypeSizes.BYTE;
            }
        }      
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            Buffer[cursor] = (byte)SubMessageType;                           // Type     (1 byte)
            cursor++;

            return ByteLength;
        }
        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if (Buffer[cursor] != SubMessageType)
                throw new Exception("Wrong 1.Byte (type) for AvatarCreateRequestGeneratorData");
            else
            {
                cursor++;                                           // Type     (1 byte)  
            }

            return ByteLength;
        }      
        #endregion

        public SubMessageSendCharInfo()
        {            
        }

        public SubMessageSendCharInfo(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
