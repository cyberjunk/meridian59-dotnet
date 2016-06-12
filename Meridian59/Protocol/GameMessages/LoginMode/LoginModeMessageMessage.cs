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
using System.Text;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Common;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// A message sent from server to client, (usually) carrying an errortext in protocol mode 'login'.
    /// Can also request the client to do an action.
    /// </summary>
    [Serializable]
    public class LoginModeMessageMessage : LoginModeMessage
    {
        public const byte LA_NOTHING = 0;
        public const byte LA_LOGOFF = 1;

        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.SHORT + Message.Length + TypeSizes.BYTE;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Message.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Message), 0, Buffer, cursor, Message.Length);
            cursor += Message.Length;

            Buffer[cursor] = (byte)Action;
            cursor++;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Message = Util.Encoding.GetString(Buffer, cursor, strlen);
            cursor += strlen;

            Action = Buffer[cursor];
            cursor++;

            return cursor - StartIndex;
        }
        #endregion

        /// <summary>
        /// Message textx
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Action the client should execute when receiving this message.
        /// Either LA_NOTHING or LA_LOGOFF constants in this class.
        /// </summary>
        public byte Action { get; set; }

        public LoginModeMessageMessage(string Message, byte Action)
            : base(MessageTypeLoginMode.Message)
        {
            this.Message = Message;
            this.Action = Action;                        
        }

        public LoginModeMessageMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }       
    }
}
