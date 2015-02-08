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

namespace Meridian59.Common.Interfaces
{
    /// <summary>
    /// Interface for serializing class instances to and from raw bytes.
    /// </summary>
    public interface IByteSerializable
    {
        /// <summary>
        /// The amount of bytes the byte-representation requires.
        /// </summary>
        int ByteLength { get; }

        /// <summary>
        /// Serialized bytes of the class implementing this.
        /// </summary>
        byte[] Bytes { get; }

        /// <summary>
        /// Write the bytes to a Buffer starting at StartIndex
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        int WriteTo(byte[] Buffer, int StartIndex = 0);
        
        /// <summary>
        /// Reads the bytes from a Buffer starting at StartIndex
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        /// <returns></returns>
        int ReadFrom(byte[] Buffer, int StartIndex = 0);
    }
}
