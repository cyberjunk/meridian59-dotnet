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
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Sent by the server to the client as positive response to LoginMessage.
    /// </summary>
    [Serializable]
    public class LoginOKMessage : LoginModeMessage
    {
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
#if !VANILLA && !OPENMERIDIAN
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.INT;
#else
                return base.ByteLength + TypeSizes.BYTE;
#endif
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, StartIndex);

            Buffer[cursor] = (byte)AccountType;
            cursor++;

#if !VANILLA && !OPENMERIDIAN
            Array.Copy(BitConverter.GetBytes(SessionID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
#endif
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            AccountType = (AccountType)Buffer[cursor];
            cursor++;

#if !VANILLA && !OPENMERIDIAN
            SessionID = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
#endif
            return cursor - StartIndex;
        }
#endregion
       
        /// <summary>
        /// Account type of this user.
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Blakserv session ID.
        /// NOT available in VANILLA and OPENMERIDIAN
        /// </summary>
        public int SessionID { get; set; }

        public LoginOKMessage(AccountType AccountType, int SessionID)
            : base(MessageTypeLoginMode.LoginOK)
        {           
            this.AccountType = AccountType;
            this.SessionID = SessionID;
        }

        public LoginOKMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }       
    }
}
