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
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.Structs;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// First message sent from the client to the server in response to AP_GETLOGIN.
    /// Used to identify himself (including account credentials).
    /// </summary>
    /// <remarks>
    /// See 'clientd3d/login.c' and 'blakserv/synched.c'
    /// </remarks>
    [Serializable]
    public class LoginMessage : LoginModeMessage
    {
        #region Constants
        public const ushort CPUTYPE_386     = 386;
        public const ushort CPUTYPE_486     = 486;
        public const ushort CPUTYPE_PENTIUM = 586;
        public const ushort WINTYPE_WIN31   = 0;
        public const ushort WINTYPE_WIN98   = 1;
        public const ushort WINTYPE_NT      = 2;
        #endregion

        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + TypeSizes.BYTE + TypeSizes.BYTE +
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT +
                    TypeSizes.INT + TypeSizes.SHORT + TypeSizes.SHORT +
                    TypeSizes.SHORT + TypeSizes.SHORT +
                    TypeSizes.INT + TypeSizes.INT +
                    TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.SHORT +
                    TypeSizes.SHORT + Username.Length +
#if VANILLA
                    TypeSizes.SHORT + 16; // password hashlen + hash
#else
                    TypeSizes.SHORT + 16 + // password hashlen + hash
                    TypeSizes.SHORT + 16; // rsbhashlen + hash
#endif
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Buffer[cursor] = MajorClientVersion;
            cursor++;

            Buffer[cursor] = MinorClientVersion;
            cursor++;

            Array.Copy(BitConverter.GetBytes(WindowsType), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(WindowsMajorVersion), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(WindowsMinorVersion), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(RamSize), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(CpuType), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(ClientExecutableCRC), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(HorizontalSystemResolution), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(VerticalSystemResolution), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Display), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Bandwidth), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = ColorDepth;
            cursor++;

            Buffer[cursor] = PartnerNr;
            cursor++;

            Array.Copy(BitConverter.GetBytes(Unused), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;           

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Username.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Username), 0, Buffer, cursor, Username.Length);
            cursor += Username.Length;

            Array.Copy(BitConverter.GetBytes((ushort)16), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(PasswordHash.HASH1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHash.HASH2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHash.HASH3), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(PasswordHash.HASH4), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
#if !VANILLA
            Array.Copy(BitConverter.GetBytes((ushort)16), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(RsbHash.HASH1), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(RsbHash.HASH2), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(RsbHash.HASH3), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(RsbHash.HASH4), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;
#endif
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            MajorClientVersion = Buffer[cursor];
            cursor++;

            MinorClientVersion = Buffer[cursor];
            cursor++;

            WindowsType = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            WindowsMajorVersion = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            WindowsMinorVersion = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            RamSize = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            CpuType = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ClientExecutableCRC = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            HorizontalSystemResolution = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            VerticalSystemResolution = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Display = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Bandwidth = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            ColorDepth = Buffer[cursor];
            cursor++;

            PartnerNr = Buffer[cursor];
            cursor++;

            Unused = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Username = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            // passwordlen, always 0x10 = 16
            cursor += TypeSizes.SHORT;

            Hash128Bit hash = new Hash128Bit();
            hash.HASH1 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            hash.HASH2 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            hash.HASH3 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            hash.HASH4 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            PasswordHash = hash;
#if !VANILLA
            // rsbhash len, always 0x10 = 16
            cursor += TypeSizes.SHORT;

            Hash128Bit rsbHash = new Hash128Bit();
            rsbHash.HASH1 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            rsbHash.HASH2 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            rsbHash.HASH3 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            rsbHash.HASH4 = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            RsbHash = rsbHash;
#endif
            return cursor - StartIndex;
        }
        #endregion

        /// <summary>
        /// Major program version of the Meridian 59 client trying to login
        /// </summary>
        public byte MajorClientVersion { get; set; }

        /// <summary>
        /// Minor program version of the Meridian 59 client trying to login
        /// </summary>
        public byte MinorClientVersion { get; set; }

        /// <summary>
        /// Basic type of windows.
        /// </summary>
        public uint WindowsType { get; set; }
        
        /// <summary>
        /// Major windows version.
        /// </summary>
        public uint WindowsMajorVersion { get; set; }
        
        /// <summary>
        /// Minor windows version.
        /// </summary>
        public uint WindowsMinorVersion { get; set; }

        /// <summary>
        /// Amount of RAM the client computer has.
        /// Not working correctly: 32-Bit Integer has become too small for RAM > 4GB.
        /// </summary>
        public uint RamSize { get; set; }

        /// <summary>
        /// CPU type.
        /// </summary>
        /// <remarks>
        /// In original code CpuType is merged with ClientExecutable in a 32-Bit integer
        /// </remarks>
        public ushort CpuType { get; set; }

        /// <summary>
        /// Used to transmit a CRC checksum of the old 'meridian.exe'.
        /// Probably designed to catch hackers, but the value is ignored on the serverside.
        /// </summary>
        /// <remarks>
        /// In original code CpuType is merged with ClientExecutable in a 32-Bit integer
        /// </remarks>
        public ushort ClientExecutableCRC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ushort HorizontalSystemResolution { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public ushort VerticalSystemResolution { get; set; }

        /// <summary>
        /// This is an encoded value built from the resolution
        /// and the colordepth. Likely redunant with other properties.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public uint Display { get; set; }

        /// <summary>
        /// Used to transmit user-bandwidth.
        /// Not active right now.
        /// </summary>
        public uint Bandwidth { get; set; }

        /// <summary>
        /// ColorDepth of the client computer.
        /// </summary>
        /// <remarks>
        /// The original code encodes this with 'PartnerNr' and 'Unused' into an integer
        /// </remarks>
        public byte ColorDepth { get; set; }

        /// <summary>
        /// An ID used to recognize old M59 resellers/partners.
        /// Not required.
        /// </summary>
        /// <remarks>
        /// The original code encodes this with 'PartnerNr' and 'Unused' into an integer
        /// </remarks>
        public byte PartnerNr { get; set; }

        /// <summary>
        /// An ID used to recognize old M59 resellers/partners.
        /// Not required.
        /// </summary>
        /// <remarks>
        /// The original code encodes this with 'PartnerNr' and 'Unused' into an integer
        /// </remarks>
        public ushort Unused { get; set; }

        /// <summary>
        /// Username of the account to login.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Sets the password to use.
        /// This will update property 'PasswordHash'
        /// </summary>
        public string Password
        {
            set
            {
                if (value != null)
                {
                    // recalculate the hash
                    byte[] md5hash = MeridianMD5.ComputeMD5(value);

                    Hash128Bit pwHash = new Hash128Bit();
                    pwHash.HASH1 = BitConverter.ToUInt32(md5hash, 0);
                    pwHash.HASH2 = BitConverter.ToUInt32(md5hash, 4);
                    pwHash.HASH3 = BitConverter.ToUInt32(md5hash, 8);
                    pwHash.HASH4 = BitConverter.ToUInt32(md5hash, 12);

                    this.PasswordHash = pwHash;
                }
            }
        }

        /// <summary>
        /// M59 MD5 of the last set of property 'Password'
        /// </summary>
        public Hash128Bit PasswordHash { get; protected set; }

        /// <summary>
        /// MD5 hash of the .rsb string resource file.
        /// Server compares with live version on connection and
        /// may trigger a client update.
        /// </summary>
        public Hash128Bit RsbHash { get; set; }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="RsbHash"></param>
        /// <param name="MajorClientVersion"></param>
        /// <param name="MinorClientVersion"></param>
        /// <param name="WindowsType"></param>
        /// <param name="WindowsMajorVersion"></param>
        /// <param name="WindowsMinorVersion"></param>
        /// <param name="RamSize"></param>
        /// <param name="CpuType"></param>
        /// <param name="ClientExecutableCRC"></param>
        /// <param name="HorizontalSystemResolution"></param>
        /// <param name="VerticalSystemResolution"></param>
        /// <param name="Display"></param>
        /// <param name="Bandwidth"></param>
        /// <param name="ColorDepth"></param>
        /// <param name="PartnerNr"></param>
        /// <param name="Unused"></param>
        public LoginMessage(
            string Username, 
            string Password,
            Hash128Bit RsbHash,
            byte MajorClientVersion, 
            byte MinorClientVersion,
            uint WindowsType = WINTYPE_NT, 
            uint WindowsMajorVersion = 0, 
            uint WindowsMinorVersion = 0,
            uint RamSize = 0,
            ushort CpuType = CPUTYPE_PENTIUM, 
            ushort ClientExecutableCRC = 0, 
            ushort HorizontalSystemResolution = 0, 
            ushort VerticalSystemResolution = 0, 
            uint Display = 0,
            uint Bandwidth = 0, 
            byte ColorDepth = 0, 
            byte PartnerNr = 0, 
            ushort Unused = 0) 
            : base(MessageTypeLoginMode.Login)
        {
            this.MajorClientVersion = MajorClientVersion;
            this.MinorClientVersion = MinorClientVersion;
            
            this.WindowsType = WindowsType;
            this.WindowsMajorVersion = WindowsMajorVersion;
            this.WindowsMinorVersion = WindowsMinorVersion;
            
            this.RamSize = RamSize;
            this.CpuType = CpuType;
            this.ClientExecutableCRC = ClientExecutableCRC;
            
            this.HorizontalSystemResolution = HorizontalSystemResolution;
            this.VerticalSystemResolution = VerticalSystemResolution;
            this.Display = Display;
            this.Bandwidth = Bandwidth;
            this.ColorDepth = ColorDepth;
            this.PartnerNr = PartnerNr;
            this.Unused = Unused;
            this.RsbHash = RsbHash;
            this.Username = Username;
            this.Password = Password;           
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="MessageBuffer"></param>
        /// <param name="StartIndex"></param>
        public LoginMessage(byte[] MessageBuffer, int StartIndex = 0) 
            : base(MessageBuffer, StartIndex) { }               
    }
}
