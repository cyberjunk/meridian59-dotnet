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
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.Structs;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class ChangePasswordMessage : GameModeMessage
    {
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength
                    + TypeSizes.SHORT + 16  // hashlen + hash
                    + TypeSizes.SHORT + 16; // hashlen + hash
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes((ushort)16), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(PasswordHashOld.HASH1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHashOld.HASH2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHashOld.HASH3), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHashOld.HASH4), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes((ushort)16), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(PasswordHashNew.HASH1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHashNew.HASH2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHashNew.HASH3), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHashNew.HASH4), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            // passwordlen, always 0x10 = 16
            cursor += TypeSizes.SHORT;

            PasswordHash pwHashOld = new PasswordHash();
            pwHashOld.HASH1 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            pwHashOld.HASH2 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            pwHashOld.HASH3 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            pwHashOld.HASH4 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            PasswordHashOld = pwHashOld;

            // passwordlen, always 0x10 = 16
            cursor += TypeSizes.SHORT;

            PasswordHash pwHashNew = new PasswordHash();
            pwHashNew.HASH1 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            pwHashNew.HASH2 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            pwHashNew.HASH3 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            pwHashNew.HASH4 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            PasswordHashNew = pwHashNew;

            return cursor - StartIndex;
        }
        #endregion

        public PasswordHash PasswordHashOld { get; set; }
        public PasswordHash PasswordHashNew { get; set; }
        
        public ChangePasswordMessage(string PasswordOld, string PasswordNew) 
            : base(MessageTypeGameMode.ChangePassword)
        {           
            byte[] md5hashOld = MeridianMD5.ComputeMD5(PasswordOld);
            byte[] md5hashNew = MeridianMD5.ComputeMD5(PasswordNew);

            PasswordHash pwHashOld = new PasswordHash();
            pwHashOld.HASH1 = BitConverter.ToUInt32(md5hashOld, 0);
            pwHashOld.HASH2 = BitConverter.ToUInt32(md5hashOld, 4);
            pwHashOld.HASH3 = BitConverter.ToUInt32(md5hashOld, 8);
            pwHashOld.HASH4 = BitConverter.ToUInt32(md5hashOld, 12);
            
            PasswordHash pwHashNew = new PasswordHash();
            pwHashNew.HASH1 = BitConverter.ToUInt32(md5hashNew, 0);
            pwHashNew.HASH2 = BitConverter.ToUInt32(md5hashNew, 4);
            pwHashNew.HASH3 = BitConverter.ToUInt32(md5hashNew, 8);
            pwHashNew.HASH4 = BitConverter.ToUInt32(md5hashNew, 12);
            
            this.PasswordHashOld = pwHashOld;
            this.PasswordHashNew = pwHashNew;
        }

        public ChangePasswordMessage(byte[] MessageBuffer, int StartIndex = 0) 
            : base(MessageBuffer, StartIndex) { }               
    }
}
