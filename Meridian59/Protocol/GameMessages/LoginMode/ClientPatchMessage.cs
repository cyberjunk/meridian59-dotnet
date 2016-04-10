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
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Sent from the server to the client in response to a not accepted
    /// DownloadVersion in previous 'ReqGameStateMessage'.
    /// This signals a client patch.
    /// </summary>
    [Serializable]
    public class ClientPatchMessage : LoginModeMessage
    {    
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + ClientPatchInfo.ByteLength;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
            cursor += ClientPatchInfo.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ClientPatchInfo = new ClientPatchInfo(Buffer, cursor);
            cursor += ClientPatchInfo.ByteLength;

            return cursor - StartIndex;
        }

        public unsafe override void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);
            ClientPatchInfo = new ClientPatchInfo(ref Buffer);
        }

        public unsafe override void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);
            ClientPatchInfo.WriteTo(ref Buffer);
        }
        #endregion

        public ClientPatchInfo ClientPatchInfo { get; set; }

        public ClientPatchMessage(ClientPatchInfo ClientPatchInfo)
            : base(MessageTypeLoginMode.ClientPatch)
        {
            this.ClientPatchInfo = ClientPatchInfo;
        }

        public ClientPatchMessage(byte[] MessageBuffer, int StartIndex = 0) : 
            base(MessageBuffer, StartIndex) { }

        public unsafe ClientPatchMessage(ref byte* Buffer) :
            base(ref Buffer) { }
    }
}
