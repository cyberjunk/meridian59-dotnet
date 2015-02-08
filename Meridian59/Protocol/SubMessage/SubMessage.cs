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
using Meridian59.Common.Interfaces;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.SubMessage
{
    /// <summary>
    /// A SubMessage is basically a GameMessage embedded into another Message, like "SystemMessage".
    /// In this case the Messagetype of SubMessage is the next following byte after the MessageType.
    /// </summary>
    [Serializable]
    public abstract class SubMessage : IByteSerializable
    {
        public abstract byte SubMessageType { get; }

        #region IByteSerializable implementation
        public abstract int ByteLength { get; }        
        public abstract int WriteTo(byte[] Buffer, int StartIndex=0);
        public abstract int ReadFrom(byte[] Buffer, int StartIndex=0);
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

        public static SubMessage ExtractSubMessage(byte[] Buffer, int StartIndex)
        {
            SubMessage returnValue = null;

            // try to parse the submessage
            switch ((MessageTypeGameMode)Buffer[StartIndex])
            {
                case MessageTypeGameMode.NewCharInfo:
                    returnValue = new SubMessageNewCharInfo(Buffer, StartIndex);
                    break;

                case MessageTypeGameMode.SendCharInfo:
                    returnValue = new SubMessageSendCharInfo(Buffer, StartIndex);
                    break;
             
                default:
                    returnValue = new SubMessageGeneric(Buffer, StartIndex);
                    break;
            }

            return returnValue;
        }
    }
}
