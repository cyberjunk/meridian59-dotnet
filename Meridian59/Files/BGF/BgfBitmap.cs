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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO.Compression;
using Meridian59.Common;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;
using Meridian59.Drawing2D;
using Meridian59.Properties;
using Meridian59.Native;
using ComponentAce.Compression.Libs.zlib;

#if DRAWING
using System.Drawing;
using System.Drawing.Imaging;
#endif

namespace Meridian59.Files.BGF
{    
    /// <summary>
    /// A single frame/image within a BGF file
    /// </summary>
    public class BgfBitmap : IByteSerializable, INotifyPropertyChanged, IClearable
    {
        #region Constants
        protected const string ERRORIMAGEFORMAT         = "Invalid Bitmap. Either not BMP or not 8bppIndexed.";
        protected const string ERRORCRUSHPLATFORM       = "Crusher is only supported in x86 builds on Windows.";        
        protected const int COMPRESSEDLENFORUNCOMPRESSED = 0;
        protected const ushort BMPSIGNATURE             = 0x4D42;
        protected const ushort BMPCOLORPLANES           = 1;
        protected const ushort BMPBPP                   = 8;
        protected const uint BMPCOMPRESSION             = 0;
        protected const uint BMPPPMHORIZ                = 0;
        protected const uint BMPPPMVERTIC               = 0;
        protected const uint BMPCOLORSPALETTE           = 0;
        protected const uint BMPNUMIMPORTANTCOLORS      = 0;
        protected const int BMPHEADERLEN                = 14;
        protected const int DIBHEADERLEN                = 40;

        public const string PROPNAME_NUM                = "Num";
        public const string PROPNAME_VERSION            = "Version";
        public const string PROPNAME_ISCOMPRESSED       = "IsCompressed";
        public const string PROPNAME_COMPRESSEDLENGTH   = "CompressedLength";
        public const string PROPNAME_UNCOMPRESSEDLENGTH = "UncompressedLength";
        public const string PROPNAME_WIDTH              = "Width";
        public const string PROPNAME_HEIGHT             = "Height";
        public const string PROPNAME_XOFFSET            = "XOffset";
        public const string PROPNAME_YOFFSET            = "YOffset";
        public const string PROPNAME_HOTSPOTS           = "HotSpots";
        public const string PROPNAME_POW2WIDTH          = "Pow2Width";
        public const string PROPNAME_POW2HEIGHT         = "Pow2Height";
        public const string PROPNAME_MULTIPLE4WIDTH     = "Multiple4Width";
        public const string PROPNAME_PIXELDATA          = "PixelData";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, e);
        }
        #endregion
        
        #region IByteSerializable
        public int ByteLength
        {
            get
            {
                int len = TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;
                
                len += TypeSizes.BYTE;              
                foreach(BgfBitmapHotspot obj in HotSpots)
                    len += obj.ByteLength;

                len += TypeSizes.BYTE + TypeSizes.INT;

                if (!IsCompressed)
                    len += UncompressedLength;
                else
                    len += CompressedLength;

                return len;                 
            }
        }     

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Width), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Height), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(XOffset), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(YOffset), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = Convert.ToByte(HotSpots.Count);
            cursor++;

            foreach (BgfBitmapHotspot hotspot in HotSpots)
                cursor += hotspot.WriteTo(Buffer, cursor);

            Buffer[cursor] = Convert.ToByte(IsCompressed);
            cursor++;

            // always uncompressed length here, if not compressed 0 or -1
            Array.Copy(BitConverter.GetBytes(CompressedLength), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            if (IsCompressed)
            {
                Array.Copy(PixelData, 0, Buffer, cursor, CompressedLength);
                cursor += CompressedLength;
            }
            else
            {
                Array.Copy(PixelData, 0, Buffer, cursor, UncompressedLength);
                cursor += CompressedLength;
            }

            return ByteLength;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Width = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            Height = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            XOffset = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            YOffset = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            byte HotSpotCount = Buffer[cursor];
            cursor++;

            HotSpots.Clear();
            HotSpots.Capacity = HotSpotCount;
            for (int i = 0; i < HotSpotCount; i++)
            {
                BgfBitmapHotspot hotspot = new BgfBitmapHotspot(Buffer, cursor);
                cursor += hotspot.ByteLength;

                HotSpots.Add(hotspot);
            }

            isCompressed = BitConverter.ToBoolean(Buffer, cursor);
            cursor++;

            CompressedLength = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            if (IsCompressed)
            {
                PixelData = new byte[CompressedLength];
                Array.Copy(Buffer, cursor, PixelData, 0, CompressedLength);
                cursor += CompressedLength;              
            }
            else
            {
                PixelData = new byte[UncompressedLength];
                Array.Copy(Buffer, cursor, PixelData, 0, UncompressedLength);
                cursor += UncompressedLength;            
            }
            return ByteLength;
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

        #region Fields
        protected uint num;
        protected uint version;
        protected uint width;
        protected uint height;
        protected int xoffset;
        protected int yoffset;
        protected bool isCompressed;
        protected int compressedLength;
        protected uint pow2width;
        protected uint pow2height;
        protected uint multiple4width;
        protected BaseList<BgfBitmapHotspot> hotspots = new BaseList<BgfBitmapHotspot>();
        protected byte[] pixeldata;
        #endregion

        #region Properties
        /// <summary>
        /// Num of this frame, 1 based
        /// </summary>
        public uint Num
        {
            get { return num; }
            set
            {
                if (num != value)
                {
                    num = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUM));
                }
            }
        }

        /// <summary>
        /// The BGF file version of this BgfBitmap.
        /// Can trigger an uncompress/recompress when being set with different values.
        /// </summary>
        public uint Version 
        { 
            get { return version; } 
            set 
            {
                if (version != value)
                {
                    // check if compression needs a switch
                    bool switchCompression = PixelData != null && isCompressed && (
                        (version > BgfFile.VERSION9) && (value < BgfFile.VERSION10) ||      // zlib -> crush
                        (version < BgfFile.VERSION10) && (value > BgfFile.VERSION9));       // crush -> zlib
                    
                    // recompress
                    if (switchCompression)
                    {
                        // decompress using old version
                        IsCompressed = false;

                        // update version
                        version = value;

                        // compress using new version
                        IsCompressed = true;
                    }
                    else
                    {
                        // just update version
                        version = value;
                    }

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VERSION));                    
                }
            } 
        }
        
        /// <summary>
        /// Width of the image in pixels
        /// </summary>
        public uint Width
        {
            get { return width; }
            set 
            {
                if (width != value)
                {
                    width = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_WIDTH));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_UNCOMPRESSEDLENGTH));
                    
                    Pow2Width = MathUtil.NextPowerOf2(width);
                    Multiple4Width = MathUtil.NextMultipleOf4(width);
                }
            }
        }

        /// <summary>
        /// Height of the image in pixels
        /// </summary>
        public uint Height 
        { 
            get { return height; }
            set 
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEIGHT));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_UNCOMPRESSEDLENGTH));

                    Pow2Height = MathUtil.NextPowerOf2(height);
                }
            } 
        }
        
        /// <summary>
        /// XOffset to apply
        /// </summary>
        public int XOffset 
        {
            get { return xoffset; }
            set
            {
                if (xoffset != value)
                {
                    xoffset = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_XOFFSET));
                }
            }
        }

        /// <summary>
        /// YOffset to apply
        /// </summary>
        public int YOffset 
        {
            get { return yoffset; }
            set
            {
                if (yoffset != value)
                {
                    yoffset = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_YOFFSET));
                }
            }
        }

        /// <summary>
        /// List of hotspots this frame has
        /// </summary>
        public BaseList<BgfBitmapHotspot> HotSpots 
        {
            get { return hotspots; }
            protected set
            {
                if (hotspots != value)
                {
                    hotspots = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HOTSPOTS));
                }
            }
        }

        /// <summary>
        /// Compressed size (if compression enabled)
        /// </summary>
        public int CompressedLength 
        {
            get { return compressedLength; }
            protected set
            {
                if (compressedLength != value)
                {
                    compressedLength = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COMPRESSEDLENGTH));
                }
            } 
        }
   
        /// <summary>
        /// Uncompressed size (which is simply Width * Height)
        /// </summary>
        public int UncompressedLength { get { return (int)(Width * Height); } }

        /// <summary>
        /// The next greater power of 2 of Width.
        /// Updated when Width property is set.
        /// </summary>
        public uint Pow2Width
        {
            get { return pow2width; }
            protected set
            {
                if (pow2width != value)
                {
                    pow2width = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POW2WIDTH));
                }
            }
        }

        /// <summary>
        /// The next greater power of 2 of Height.
        /// Updated when Height property is set.
        /// </summary>
        public uint Pow2Height
        {
            get { return pow2height; }
            protected set
            {
                if (pow2height != value)
                {
                    pow2height = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POW2HEIGHT));
                }
            }
        }

        /// <summary>
        /// The next greater multiple 4 of width.
        /// Updated when Width property is set.
        /// </summary>
        public uint Multiple4Width
        {
            get { return multiple4width; }
            protected set
            {
                if (multiple4width != value)
                {
                    multiple4width = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MULTIPLE4WIDTH));
                }
            }
        }

        /// <summary>
        /// Whether or not the PixelData is compressed (change it to compress/decompress)
        /// </summary>
        public bool IsCompressed
        {
            get { return isCompressed; }
            set
            {
                if (isCompressed != value)
                {
                    if (value)
                    {
                        // compress PixelData
                        PixelData = Compress(PixelData);

                        // Update compressedlength & compressedflag
                        CompressedLength = PixelData.Length;
                        isCompressed = true;

                    }
                    else
                    {
                        // decompress PixelData
                        PixelData = Decompress(PixelData);

                        // Update compressedlength & compressedflag
                        CompressedLength = COMPRESSEDLENFORUNCOMPRESSED;
                        isCompressed = false;
                    }

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISCOMPRESSED));
                }
            }
        }
        
        /// <summary>
        /// This keeps the PixelData of this BgfBitmap instance as it is saved in BGF files.
        /// Either compressed or uncompressed, WITHOUT stride-bytes and up-side-down.
        /// Each byte represents a pixel and the value is the index of the pixel's color in the palette.
        /// </summary>
        public byte[] PixelData
        {
            get { return pixeldata; }
            set
            {
                if (pixeldata != value)
                {
                    pixeldata = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PIXELDATA));
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public BgfBitmap()
        {
            Clear(false);
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="Version"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="XOffset"></param>
        /// <param name="YOffset"></param>
        /// <param name="HotSpots"></param>
        /// <param name="IsCompressed"></param>
        /// <param name="CompressedLength"></param>
        /// <param name="PixelData"></param>
        public BgfBitmap(
            uint Num,
            uint Version,
            uint Width, 
            uint Height,
            int XOffset,
            int YOffset,
            IEnumerable<BgfBitmapHotspot> HotSpots,
            bool IsCompressed,
            int CompressedLength,
            byte[] PixelData)
        {
            num = Num;
            version = Version;
            this.Width = Width;     // update pow2/multiple4 also
            this.Height = Height;   // update pow2 also
            xoffset = XOffset;
            yoffset = YOffset;            
            isCompressed = IsCompressed;
            compressedLength = CompressedLength;
            pixeldata = PixelData;

            // hotspots to own list
            foreach (BgfBitmapHotspot obj in HotSpots)
                hotspots.Add(obj);
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="Version"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public BgfBitmap(uint Num, uint Version, byte[] Buffer, int StartIndex = 0)
        {
            num = Num;

            // set version
            version = Version;

            // parse bytes
            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a compressed byte[] of PixelData argument
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public byte[] Compress(byte[] Data)
        {           
            // allocate a buffer with uncompressed length to write compressed stream to
            byte[] tempBuffer = new byte[UncompressedLength];           
            int compressedLength;

            // ZLIB
            if (version > BgfFile.VERSION9)
            {
                // init streams
                MemoryStream destStream = new MemoryStream(tempBuffer, true);
                ZOutputStream destZ = new ZOutputStream(destStream, zlibConst.Z_BEST_COMPRESSION);

                // compress
                destZ.Write(Data, 0, Data.Length);
                destZ.Flush();
                destZ.finish();

                // update compressed length
                compressedLength = (int)destZ.TotalOut;

                // cleanup
                destStream.Dispose();
                destZ.Dispose();
            }
            // CRUSH
            else
            {
#if WINCLR && X86
                // compress to tempBuffer
                compressedLength = Crush32.Compress(Data, 0, tempBuffer, 0, (int)UncompressedLength);   
#else
                throw new Exception(ERRORCRUSHPLATFORM);
#endif
            }

            // copy all bytes we actually used from tempBuffer to new PixelData
            byte[] newPixelData = new byte[compressedLength];
            Array.Copy(tempBuffer, 0, newPixelData, 0, compressedLength);

            return newPixelData;
        }

        /// <summary>
        /// Returns a decompressed byte[] of PixelData argument
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public byte[] Decompress(byte[] Data)
        {
            byte[] decompressedPixelData = new byte[UncompressedLength];

            // ZLIB
            if (version > BgfFile.VERSION9)
            {
                // init sourcestream
                MemoryStream srcStream = new MemoryStream(Data, false);

                // must skip two bytes not part of deflate but used by zlib
                srcStream.ReadByte();
                srcStream.ReadByte();

                // init .net decompressor
                DeflateStream destZ = new DeflateStream(srcStream, CompressionMode.Decompress);

                // decompress
                destZ.Read(decompressedPixelData, 0, UncompressedLength);

                // cleanup                
                destZ.Dispose();
                srcStream.Dispose();
            }
            // CRUSH
            else
            {
#if WINCLR && X86
                // decompress
                Crush32.Decompress(Data, 0, decompressedPixelData, 0, (int)UncompressedLength, CompressedLength);
#else
                throw new Exception(ERRORCRUSHPLATFORM);
#endif
            }

            // set decompressed array to pixeldata
            return decompressedPixelData;
        }

        /// <summary>
        /// Old Decompress method not using .NET compressor
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public byte[] DecompressOld(byte[] Data)
        {
            byte[] decompressedPixelData = new byte[UncompressedLength];

            // ZLIB
            if (version > BgfFile.VERSION9)
            {
                // init streams
                MemoryStream destStream = new MemoryStream(decompressedPixelData, true);
                ZOutputStream destZ = new ZOutputStream(destStream);

                // decompress
                destZ.Write(Data, 0, Data.Length);
                destZ.Flush();
                destZ.finish();

                // cleanup
                destStream.Dispose();
                destZ.Dispose();
            }
            // CRUSH
            else
            {
#if WINCLR && X86
                // decompress
                Crush32.Decompress(Data, 0, decompressedPixelData, 0, (int)UncompressedLength, CompressedLength);
#else
                throw new Exception(ERRORCRUSHPLATFORM);
#endif                
            }

            // set decompressed array to pixeldata
            return decompressedPixelData;
        }
  
        /// <summary>
        /// Returns the Hotspot matching given Index.
        /// This is an absolute compare: -1 equals 1
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public BgfBitmapHotspot FindHotspot(byte Index)
        {
            foreach (BgfBitmapHotspot hotspot in HotSpots)
                if (Math.Abs(hotspot.Index) == Index)
                    return hotspot;

            return null;
        }
        
        /// <summary>
        /// Creates the full byte[] of a BMP file representing this BgfBitmap.
        /// Including BMP, DIB header and M59 default colorpalette.
        /// Warning: This is only useful when writing to BMP files.
        /// </summary>
        /// <returns>byte[] of a BMP file</returns>
        public byte[] PixelDataToBitmapBytes()
        {
            uint pixeldatasize = Multiple4Width * Height;
            uint pixelsOffset =  (uint)(BMPHEADERLEN + DIBHEADERLEN + Resources.BitmapColorTable.Length);
            uint filesize = pixelsOffset + pixeldatasize;
            
            // allocate a byte[] to store the bitmap
            byte[] bitmapBytes = new byte[filesize];
            int cursor = 0;

            // --- BMP HEADER --- 
            Array.Copy(BitConverter.GetBytes(BMPSIGNATURE), 0, bitmapBytes, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(filesize), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // skip 4 bytes value
            cursor += TypeSizes.INT; 

            Array.Copy(BitConverter.GetBytes(pixelsOffset), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // --- DIB HEADER ---
            Array.Copy(BitConverter.GetBytes(DIBHEADERLEN), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Width), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Height), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(BMPCOLORPLANES), 0, bitmapBytes, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(BMPBPP), 0, bitmapBytes, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(BMPCOMPRESSION), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(UncompressedLength), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(BMPPPMHORIZ), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(BMPPPMVERTIC), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(BMPCOLORSPALETTE), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(BMPNUMIMPORTANTCOLORS), 0, bitmapBytes, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // --- COLORTABLE ---
            Array.Copy(Resources.BitmapColorTable, 0, bitmapBytes, cursor, Resources.BitmapColorTable.Length);
            cursor += Resources.BitmapColorTable.Length;

            // --- PIXELS ---
            FillPixelDataTo(bitmapBytes, pixelsOffset, true);
            cursor += (int)pixeldatasize;

            return bitmapBytes;
        }

        /// <summary>
        /// Writes the default 8-Bit PixelData (indices) to a target byte[],
        /// adding stride zeros and can flip the rows.
        /// </summary>
        /// <param name="Target">Array to write to</param>
        /// <param name="StartIndex">Index to start writing in Target argument</param>
        /// <param name="FlipRows">Whether to make first pixelrow the last</param>
        public void FillPixelDataTo(byte[] Target, uint StartIndex, bool FlipRows)
        {
            // possibly decompress first
            if (IsCompressed)
                IsCompressed = false;

            // define offsets
            uint lastoffset = StartIndex + (Multiple4Width * Height);
            uint readoffset = 0;

            if (FlipRows)
            {
                // set target for first loop to first pixel of last row               
                StartIndex = lastoffset - Multiple4Width;

                // loop through rows             
                for (int i = 0; i < Height; i++)
                {                                       
                    // copy the pixelrow
                    Wrapper.CopyMem(PixelData, (int)readoffset, Target, (int)StartIndex, Width);

                    // move cursors to new rowstarts
                    readoffset += Width;
                    StartIndex -= Multiple4Width;
                }
            }
            else
            {
                // loop through rows             
                for (int i = 0; i < Height; i++)
                {
                    // copy the pixelrow
                    Wrapper.CopyMem(PixelData, (int)readoffset, Target, (int)StartIndex, Width);

                    // move cursors to new rowstarts
                    readoffset += Width;
                    StartIndex += Multiple4Width;
                }
            }
        }

        /// <summary>
        /// Writes the default 8-Bit PixelData (indices) to a target pointer.
        /// Adds stride zeros and can flip rows.
        /// </summary>
        /// <param name="Target">A memory pointer to start writing to.</param>
        /// <param name="FlipRows">Whether to make the first pixelrow the last.</param>
        /// <param name="UseMultiple4Width">Whether the target holds additional bytes to make rows multiple of 4</param>
        public void FillPixelDataTo(IntPtr Target, bool FlipRows, bool UseMultiple4Width = false)
        {           
            // possibly decompress first
            if (IsCompressed)
                IsCompressed = false;

            // define offsets
            IntPtr lastoffset;
            uint readoffset = 0;
            int toadd;
           
            if (UseMultiple4Width)
            {
                lastoffset = Target + ((int)Multiple4Width * (int)Height);
                toadd = (int)Multiple4Width;
            }
            else
            {
                lastoffset = Target + ((int)Width * (int)Height);
                toadd = (int)Width;
            }
            
            if (FlipRows)
            {
                if (UseMultiple4Width)
                {
                    // set target for first loop to first pixel of last row
                    Target = lastoffset - (int)Multiple4Width;                  
                }
                else
                {
                    // set target for first loop to first pixel of last row
                    Target = lastoffset - (int)Width;                  
                }
                                
                // loop through rows             
                for (int i = 0; i < Height; i++)
                {
                    // copy the pixelrow
                    Wrapper.CopyMem(PixelData, (int)readoffset, Target, (uint)Width);

                    // increase readoffset to next row
                    readoffset += Width;
                    Target -= toadd;
                }
            }
            else
            {
                if (!UseMultiple4Width || (Width & 3) == 0)
                {
                    // copy the pixelrow
                    Wrapper.CopyMem(PixelData, (int)readoffset, Target, (uint)(Width * Height));
                }
                else
                {
                    // loop through rows             
                    for (int i = 0; i < Height; i++)
                    {
                        // copy the pixelrow
                        Wrapper.CopyMem(PixelData, (int)readoffset, Target, (uint)Width);

                        // move cursors to new rowstarts
                        readoffset += Width;
                        Target += (int)Multiple4Width;
                    }
                }
            }          
        }

        /// <summary>
        /// Returns the pixels of this BgfBitmap with stride
        /// as A1R5G5B5 values.
        /// Requires initialized ColorTransformation.Provider.
        /// </summary>
        /// <param name="Palette">color palette index to use</param>
        /// <param name="Multiple4Stride"></param>
        /// <returns>16-Bit Pixels</returns>
        public ushort[] GetPixelDataAsA1R5G5B5(byte Palette = 0, bool Multiple4Stride = false)
        {
            uint stride, pixels;

            // multiple-of-4 stride
            if (Multiple4Stride)
            {
                stride = Multiple4Width - Width;
                pixels = Multiple4Width * Height;
            }
            else
            {
                stride = 0;
                pixels = Width * Height;
            }

            // possibly decompress first
            if (IsCompressed)
                IsCompressed = false;

            uint[] colPalette = ColorTransformation.Palettes[Palette];

            // allocate pixeldata
            ushort[] pixeldata16bpp = new ushort[pixels];

            // walk rows
            uint sourceindex = 0;
            uint targetindex = 0;
            for (int i = 0; i < Height; i++)
            {
                // walk pixels of row
                for (int j = 0; j < Width; j++)
                {
                    // get color from table for this pixel
                    uint intcolor = colPalette[PixelData[sourceindex]];

                    pixeldata16bpp[targetindex] = (ushort)
                        (((intcolor & 0x80000000) >> 16) |
                        ((intcolor & 0x00F80000) >> 9) |
                        ((intcolor & 0x0000F800) >> 6) |
                        ((intcolor & 0x000000F8) >> 3));

                    // raise sourceindex and targetindex
                    sourceindex++;
                    targetindex++;
                }

                // skip stride bytes at end of target row
                targetindex += stride;
            }

            return pixeldata16bpp;
        }

        /// <summary>
        /// Returns the pixels of this BgfBitmap with stride
        /// as A8R8G8B8 values.
        /// Requires initialized ColorTransformation.Provider.
        /// </summary>
        /// <param name="Palette">Color Palette to use</param>
        /// <param name="Multiple4Stride">Whether to add empty pixels for multiple of 4</param>
        /// <returns></returns>
        public uint[] GetPixelDataAsA8R8G8B8(byte Palette = 0, bool Multiple4Stride = false)
        {
            uint stride, pixels;

            // multiple-of-4 stride
            if (Multiple4Stride)
            {
                stride = Multiple4Width - Width;
                pixels = Multiple4Width * Height;
            }
            else
            {
                stride = 0;
                pixels = Width * Height;
            }

            // possibly decompress first
            if (IsCompressed)
                IsCompressed = false;

            // get colorpalette to use
            uint[] colPalette = ColorTransformation.Palettes[Palette];

            // allocate pixeldata
            uint[] pixeldata32bpp = new uint[pixels];

            // walk rows
            uint sourceindex = 0;
            uint targetindex = 0;
            for (int i = 0; i < Height; i++)
            {
                // walk pixels of row
                for (int j = 0; j < Width; j++)
                {
                    // get color from table for this pixel and write color to output
                    pixeldata32bpp[targetindex] = colPalette[PixelData[sourceindex]];

                    // raise sourceindex and targetindex
                    sourceindex++;
                    targetindex++;
                }

                // skip stride bytes at end of target row
                targetindex += stride;
            }

            return pixeldata32bpp;
        }

        /// <summary>
        /// Writes the pixels of this BgfBitmap as A8R8G8B8 values to a raw pointer.
        /// Requires initialized ColorTransformation palettes.
        /// </summary>
        /// <param name="Buffer">A buffer with 32bit pixel-values</param>
        /// <param name="Palette">Palette index to use</param>
        /// <param name="RowWidth">
        /// How many pixels a row in the targetbuffer has.
        /// Must be bigger or equal the BgfBitmap Width
        /// </param>
        public unsafe void FillPixelDataAsA8R8G8B8(uint* Buffer, byte Palette, uint RowWidth)
        {
            // rowstride to skip
            uint rightstride = RowWidth - Width;

            // possibly decompress first
            if (IsCompressed)
                IsCompressed = false;

            // select palette
            uint[] colorPal = ColorTransformation.Palettes[Palette];

            // walk rows
            uint sourceindex = 0;
            uint targetindex = 0;
            for (uint i = 0; i < Height; i++)
            {
                // walk pixels of row
                for (uint j = 0; j < Width; j++)
                {
                    // get color from table for this pixel and write color to output
                    Buffer[targetindex] = colorPal[PixelData[sourceindex]];

                    // raise sourceindex and targetindex
                    sourceindex++;
                    targetindex++;
                }

                // skip empty part from pow2scale
                targetindex += rightstride;
            }
        }

        /// <summary>
        /// Copy'n'Paste of FillPixelDataAsA8R8G8B8, but always using
        /// a modified DefaultPalette with black transparency color.
        /// Used in the OgreClient to avoid issues with alpha_to_coverage
        /// </summary>
        /// <param name="Buffer">A buffer with 32bit pixel-values</param>
        /// <param name="RowWidth">
        /// How many pixels a row in the targetbuffer has.
        /// Must be bigger or equal the BgfBitmap Width
        /// </param>
        public unsafe void FillPixelDataAsA8R8G8B8TransparencyBlack(uint* Buffer, uint RowWidth)
        {
            // rowstride to skip
            uint rightstride = RowWidth - Width;

            // possibly decompress first
            if (IsCompressed)
                IsCompressed = false;

            // select palette
            uint[] colorPal = ColorTransformation.DefaultPaletteBlackTransparency;

            // walk rows
            uint sourceindex = 0;
            uint targetindex = 0;
            for (uint i = 0; i < Height; i++)
            {
                // walk pixels of row
                for (uint j = 0; j < Width; j++)
                {
                    // get color from table for this pixel and write color to output
                    Buffer[targetindex] = colorPal[PixelData[sourceindex]];

                    // raise sourceindex and targetindex
                    sourceindex++;
                    targetindex++;
                }

                // skip empty part from pow2scale
                targetindex += rightstride;
            }
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Version = BgfFile.VERSION10;
                Width = 0;
                Height = 0;
                XOffset = 0;
                YOffset = 0;
                IsCompressed = false;
                CompressedLength = 0;
                PixelData = new byte[0];

                hotspots.Clear();
            }
            else
            {
                version = BgfFile.VERSION10;
                width = 0;
                height = 0;
                xoffset = 0;
                yoffset = 0;
                isCompressed = false;
                compressedLength = 0;
                pixeldata = new byte[0];

                hotspots.Clear();
            }
        }
        #endregion

        #region Static
        /// <summary>
        /// Blank, transparent dummy (1 pixel, 1x1)
        /// 0xFE = transparent color index
        /// </summary>
        public static BgfBitmap BLANK =
            new BgfBitmap(1, BgfFile.VERSION10, 1, 1, 0, 0, new List<BgfBitmapHotspot>(), false, 0, new byte[] { 0xFE });
        #endregion

        #region BUILDDEPENDENT

#if DRAWING
        /// <summary>
        /// Verifies the Bitmap argument is a 8bpp indexed BMP
        /// </summary>
        /// <param name="Bitmap"></param>
        /// <returns></returns>
        public static bool IsValid(Bitmap Bitmap)
        {
            bool returnValue = true;

            // check if it's BMP format
            if (!Bitmap.RawFormat.Equals(ImageFormat.Bmp))
                returnValue = false;

            // make sure it's 8bpp indexed
            if (Bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                returnValue = false;

            return returnValue;
        }

        /// <summary>
        /// Creates a PixelData byte[] without stride from a 256 indexedcolor Bitmap instance
        /// </summary>
        /// <returns>PixelData byte[] like it's stored in binary BGF</returns>
        public static byte[] BitmapToPixelData(Bitmap Bitmap)
        {
            // difference to next multiple of 4
            uint stride = MathUtil.NextMultipleOf4((uint)Bitmap.Width) - (uint)Bitmap.Width;

            // allocate array for pixeldata
            byte[] pixels = new byte[Bitmap.Width * Bitmap.Height];

            // lock this area
            Rectangle lockRectangle = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);

            // lock bits
            BitmapData pixelData = Bitmap.LockBits(
                lockRectangle, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            // remove additional 0x00 because of "BMP row multiple of 4"
            IntPtr readoffset = pixelData.Scan0;
            int writeoffset = 0;
            for (uint i = 0; i < Bitmap.Height; i++)
            {
                Marshal.Copy(readoffset, pixels, writeoffset, Bitmap.Width);

                readoffset += pixelData.Stride;
                writeoffset += Bitmap.Width;
            }

            // unlock bits
            Bitmap.UnlockBits(pixelData);

            return pixels;
        }

        /// <summary>
        /// Creates a 256 color Bitmap from BgfBitmap.
        /// Requires initialized ColorTransformation.Provider.
        /// </summary>
        /// <param name="Palette">Palette index for ColorTransformation.Provider</param>
        /// <returns></returns>
        public Bitmap GetBitmap(byte Palette = 0)
        {
            // create bitmap instance
            Bitmap bitmap = new Bitmap((int)Width, (int)Height, PixelFormat.Format8bppIndexed);

            // lock bits
            BitmapData bits = bitmap.LockBits(
                new Rectangle(0, 0, (int)Width, (int)Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            // fill with indexed pixeldata
            FillPixelDataTo(bits.Scan0, false, true);

            // unlock & set palette
            bitmap.UnlockBits(bits);
            bitmap.Palette = PalettesGDI.Palettes[Palette];

            return bitmap;
        }

        /// <summary>
        /// Sets this BgfBitmap to a given 256 color Bitmap
        /// </summary>
        public void SetBitmap(Bitmap Bitmap)
        {
            // validate (8bit indexed?)
            if (!IsValid(Bitmap))
                throw new Exception(BgfBitmap.ERRORIMAGEFORMAT);

            // Update Width/Height values
            Width = (uint)Bitmap.Width;
            Height = (uint)Bitmap.Height;

            // update the pixelData property from the bitmap
            PixelData = BitmapToPixelData(Bitmap);

            // finally possibly compress the pixeldata if compression is activated
            if (IsCompressed)
                PixelData = Compress(PixelData);
        }

        /// <summary>
        /// Converts a server ushort LightColor to a CLR RGB color
        /// </summary>
        /// <param name="LightColor">16-Bit lightcolor value (i.e. in class objectbase)</param>
        /// <returns></returns>
        public static Color LightColorToCLRRGB(ushort LightColor)
        {
            // decode color from ushort (see d3drender.c for formulas)
            int r = ((LightColor >> 10) & 31) * 255 / 31;
            int g = ((LightColor >> 5) & 31) * 255 / 31;
            int b = (LightColor & 31) * 255 / 31;
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Converts a GrayScale value (0-255) to a CLR RGB color
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Color LightIntensityToCLRRGB(byte Value)
        {
            return Color.FromArgb(Value, Value, Value);
        }
#endif
        #endregion
    }
}
