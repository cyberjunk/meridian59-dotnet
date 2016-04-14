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
using System.Security.Cryptography;

namespace Meridian59.Common
{
    /// <summary>
    /// Provides MD5 hashes in M59 style (replaced zero bytes).
    /// </summary>
    public static class MeridianMD5
    {
        /// <summary>
        /// MD5 creator from .NET
        /// </summary>
        private static readonly MD5 md5 = MD5.Create();

        /// <summary>
        /// Generates a MD5 in M59 style from bytes input
        /// </summary>
        /// <param name="Input">Bytes to generate a M59 MD5 from</param>
        /// <returns>MD5 bytes with replaced 0x00</returns>
        public static byte[] ComputeMD5(byte[] Input)
        {
            // get MD5
            byte[] bytes = md5.ComputeHash(Input);

            // replace zero bytes with 0x01 to work around misinterpreting
            // them as termination zeros
            for (int i = 0; i < bytes.Length; i++)
                if (bytes[i] == 0x00)
                    bytes[i] = 0x01;

            return bytes;
        }

        /// <summary>
        /// Generates an MD5 from bytes input
        /// </summary>
        /// <param name="Input">A string to generate an MD5 from</param>
        /// <returns>MD5 bytes with no modification</returns>
        public static byte[] ComputeGenericMD5(byte[] Input)
        {
            // get MD5
            byte[] bytes = md5.ComputeHash(Input);

            return bytes;
        }

        /// <summary>
        /// Generates a MD5 in M59 style from string input
        /// </summary>
        /// <param name="Input">A string to generate a M59 MD5 from</param>
        /// <returns>MD5 bytes with replaced 0x00</returns>
        public static byte[] ComputeMD5(string Input)
        {
            // get bytes of string
            byte[] bytes = Encoding.Default.GetBytes(Input);

            return ComputeMD5(bytes);         
        }      
    }
}
