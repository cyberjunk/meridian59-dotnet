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
using System.IO;
using System.Collections.Generic;
using Meridian59.Common.Interfaces;

namespace Meridian59.Files
{
    /// <summary>
    /// Access M59 .bsf files (PNG container)
    /// </summary>
    public class BsfFile : IGameFile, IByteSerializable
    {
        /// <summary>
        /// Default PNG start signature
        /// </summary>
        public static byte[] PngStartSignature = new byte[8] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        
        /// <summary>
        /// Default PNG end signature
        /// </summary>
        public static byte[] PngEndSignature = new byte[8] { 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82 };

        #region IByteSerializable implementation
        public int ByteLength
        {
            get {
                int len = 0;

                foreach (byte[] png in Images)
                    len += png.Length;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            foreach(byte[] png in Images)
            {
                Array.Copy(png, 0, Buffer, cursor, png.Length);
                cursor += png.Length;
            }

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            int lastPNGStartOffset = 0;

            images.Clear();

            // start splitting up PNG files from container
            while (cursor < Buffer.Length)
            {
                // check if start of PNG signature, if so save offset and go ahead
                if (IsSignature(Buffer, cursor, PngStartSignature))
                {
                    lastPNGStartOffset = cursor;
                    cursor += PngStartSignature.Length;
                }
                // check if end of PNG signature, if so save offset and extract stream
                else if (IsSignature(Buffer, cursor, PngEndSignature))
                {
                    cursor += PngEndSignature.Length;
                    
                    byte[] pngFile = new byte[cursor - lastPNGStartOffset];
                    Array.Copy(Buffer, lastPNGStartOffset, pngFile, 0, cursor - lastPNGStartOffset);
                    Images.Add(pngFile);                  

                }
                else
                    cursor++;
            }

            return cursor - StartIndex;
        }

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

        #region IGameFile implementation
        public void Load(string FilePath, byte[] Buffer = null)
        {
            // save raw filename without path or extensions
            this.Filename = Path.GetFileNameWithoutExtension(FilePath);

            if (File.Exists(FilePath))
            {
                if (Buffer == null)
                  Buffer = File.ReadAllBytes(FilePath);
                ReadFrom(Buffer, 0);
            }
            else
                throw new FileNotFoundException();
        }

        public void Save(string FilePath)
        {
            // write file
            File.WriteAllBytes(FilePath, Bytes);
        }

        public string Filename { get; set; }
        #endregion

        protected readonly List<byte[]> images = new List<byte[]>();

        public List<byte[]> Images { get { return images; } }
        
        #region Constructors
        public BsfFile()
        {
        }

        public BsfFile(string Filename, byte[] Buffer = null)
        {
            Load(Filename);
        }

        public BsfFile(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        private bool IsSignature(byte[] Buffer, int StartIndex, byte[] signature)
        {
            bool returnVal = false;
            if (Buffer.Length > StartIndex + 7)
            {
                if ((Buffer[StartIndex] == signature[0]) &&
                    (Buffer[StartIndex + 1] == signature[1]) &&
                    (Buffer[StartIndex + 2] == signature[2]) &&
                    (Buffer[StartIndex + 3] == signature[3]) &&
                    (Buffer[StartIndex + 4] == signature[4]) &&
                    (Buffer[StartIndex + 5] == signature[5]) &&
                    (Buffer[StartIndex + 6] == signature[6]) &&
                    (Buffer[StartIndex + 7] == signature[7]))
                    returnVal = true;    
            }
            return returnVal;
        }          
    }
}
