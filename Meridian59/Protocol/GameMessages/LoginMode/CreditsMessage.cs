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
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Early message sent by the server to the client to tell him the credits he has left. 
    /// Currently not used and set with fixed value.
    /// </summary>
    /// <remarks>
    /// See original 'blakserv/synched.c'.
    /// </remarks>
    [Serializable]
    public class CreditsMessage : LoginModeMessage
    {    
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.INT;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(Credits), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
         
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            Credits = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }
        #endregion

        /// <summary>
        /// Remaining credits
        /// </summary>
        public uint Credits { get; set; }
        
        public CreditsMessage(uint Credits)
            : base(MessageTypeLoginMode.Credits)
        {
            this.Credits = Credits;                       
        }

        public CreditsMessage(byte[] MessageBuffer, int StartIndex = 0) : 
            base(MessageBuffer, StartIndex) { }
    }
}
