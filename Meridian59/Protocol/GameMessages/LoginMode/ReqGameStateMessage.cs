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
    /// Sent to the server by the client to request a switch from
    /// protocol mode 'login' to protocol mode 'game'.
    /// </summary>
    [Serializable]
    public class ReqGameStateMessage : LoginModeMessage
    {
        /// <summary>
        /// See P_CATCH in 'clientd3d/client.h'
        /// </summary>
        public const int CATCHVALUE = 3;

        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.INT + TypeSizes.INT + TypeSizes.SHORT + Hostname.Length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(DownloadVersion), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(VersionCheckValue), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Hostname.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Hostname), 0, Buffer, cursor, Hostname.Length);
            cursor += Hostname.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);

            DownloadVersion = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            VersionCheckValue = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Hostname = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            return cursor - StartIndex;
        }
        #endregion

        /// <summary>
        /// The resource version to tell the server.
        /// This is from old 'meridian.ini' value 'Download='
        /// </summary>
        public uint DownloadVersion { get; set; }

        /// <summary>
        /// Encoded Major/Minor/Catch Value.
        /// See EnterGame() in 'clientd3d/login.c'
        /// </summary>
        public int VersionCheckValue { get; set; }
        
        /// <summary>
        /// Hostname, set with "cheater" by default
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DownloadVersion"></param>
        /// <param name="MajorVersion"></param>
        /// <param name="MinorVersion"></param>
        /// <param name="Hostname"></param>
        public ReqGameStateMessage(uint DownloadVersion, byte MajorVersion, byte MinorVersion, string Hostname)
            : base(MessageTypeLoginMode.ReqGame)
        {
            this.DownloadVersion = DownloadVersion;
            this.Hostname = Hostname;    

            // calculate the version check value
            // see EnterGame() in 'clientd3d/login.c'
            this.VersionCheckValue = (((MajorVersion * 100) + MinorVersion) * CATCHVALUE) + CATCHVALUE;                              
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MessageBuffer"></param>
        /// <param name="StartIndex"></param>
        public ReqGameStateMessage(byte[] MessageBuffer, int StartIndex = 0) : 
            base(MessageBuffer, StartIndex) { }
    }
}
