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
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;
using Meridian59.Common;
using System.Xml;

#if DRAWING
using System.Drawing;
#endif

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Files.BGF
{
    /// <summary>
    /// Access M59 .bgf files (BMP container with metainfo)
    /// </summary>
    public class BgfFile : IByteSerializable, IGameFile, IClearable, INotifyPropertyChanged
    {
        #region Constants
        public const uint SIGNATURE     = 0x11464742;
        public const uint VERSION9      = 9;
        public const uint VERSION10     = 10;
        public const int NAMELENGTH     = 32;

        public const string DEFAULTFILENAME         = "object";
        public const string ERRORFILEFORMAT         = "Unsupported fileformat.";        
        public const string PROPNAME_NAME           = "Name";
        public const string PROPNAME_VERSION        = "Version";
        public const string PROPNAME_SHRINKFACTOR   = "ShrinkFactor";
        public const string PROPNAME_FRAMES         = "Frames";
        public const string PROPNAME_FRAMESETS      = "FrameSets";
        public const string PROPNAME_FILENAME       = "Filename";
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
            get {
                
                // signature + version
                int len = TypeSizes.INT + TypeSizes.INT;
                
                // MAX_BITMAPNAME
                len += NAMELENGTH;

                // bitmapcount + bitmapgroupcount + maxindices + shrink
                len += TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;

                // frames
                foreach (BgfBitmap bgfBitmap in Frames)
                    len += bgfBitmap.ByteLength;

                // framesets
                foreach (BgfFrameSet frameSet in FrameSets)
                    len += frameSet.ByteLength;

                return len;
            }
        }
       
        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
        
            Array.Copy(BitConverter.GetBytes(SIGNATURE), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Version), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(Util.Encoding.GetBytes(name), 0, Buffer, cursor, name.Length);
            cursor += NAMELENGTH;
          
            Array.Copy(BitConverter.GetBytes(Frames.Count), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(FrameSets.Count), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(MaxIndices), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(ShrinkFactor), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            foreach(BgfBitmap bgfBitmap in Frames)
                cursor += bgfBitmap.WriteTo(Buffer, cursor);

            foreach(BgfFrameSet frameSet in FrameSets)
                cursor += frameSet.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            uint signature = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            if (signature == SIGNATURE)
            {
                Version = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                // version has to be V9 or higher
                if (version >= VERSION9)
                {
                    // name string has always fixed length
                    Name = Util.Encoding.GetString(Buffer, cursor, NAMELENGTH);
                    cursor += NAMELENGTH;

                    uint frameCount = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    uint frameSetCount = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    // skip max indices, we use dynamic getter
                    cursor += TypeSizes.INT;

                    ShrinkFactor = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    Frames.Clear();
                    Frames.Capacity = (int)frameCount;
                    for (uint i = 0; i < frameCount; i++)
                    {
                        BgfBitmap bgfBitmap = new BgfBitmap(i + 1, version, Buffer, cursor);
                        cursor += bgfBitmap.ByteLength;

                        Frames.Add(bgfBitmap);
                    }

                    FrameSets.Clear();
                    FrameSets.Capacity = (int)frameSetCount;
                    for (uint i = 0; i < frameSetCount; i++)
                    {
                        BgfFrameSet frameSet = new BgfFrameSet(i + 1, Buffer, cursor);
                        cursor += frameSet.ByteLength;

                        FrameSets.Add(frameSet);
                    }
                }
                else
                    throw new Exception("Wrong file version: " + version + " (expected " + VERSION9 + " or higher).");
            }
            else
                throw new Exception("Wrong file signature: " + signature + " (expected " + SIGNATURE + ").");

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

        #region IGameFile
        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="Filename">Full path and filename of file to load</param>
        /// <param name="Buffer">All bytes of the file to load, if null will load from disk</param>
        public void Load(string Filename, byte[] Buffer = null)
        {
            // save raw filename without path or extensions
            this.Filename = Path.GetFileNameWithoutExtension(Filename);

            // read all bytes at once to memory if not provided
            if (Buffer == null)
               Buffer = File.ReadAllBytes(Filename);

            // parse
            ReadFrom(Buffer, 0);
        }

        /// <summary>
        /// Save the file to disk
        /// </summary>
        /// <param name="Filename">Full path and filename of file to write</param>
        public void Save(string Filename)
        {
            File.WriteAllBytes(Filename, Bytes);
        }
        #endregion

        #region Fields
        protected string filename;
        protected uint version;
        protected string name;
        protected uint shrinkFactor = 1;
        protected readonly BaseList<BgfBitmap> frames = new BaseList<BgfBitmap>(20);
        protected readonly BaseList<BgfFrameSet> frameSets = new BaseList<BgfFrameSet>(20);
        #endregion

        #region Properties
        /// <summary>
        /// Filename without path or extension
        /// </summary>
        public string Filename
        {
            get { return filename; }
            set
            {
                if (filename != value)
                {
                    filename = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FILENAME));
                }
            }
        }

        /// <summary>
        /// A name which is stored inside the BGF.
        /// Max. 32 characters. All grdXXXXX.bgf have it set.
        /// </summary>
        public string Name 
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    if (value.Length > NAMELENGTH)
                        name = value.Substring(0, NAMELENGTH);
                    else
                        name = value;

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NAME));
                }
            }
        }

        /// <summary>
        /// Version of this BGF file. (V9=CRUSH, V10=ZLIB)
        /// </summary>
        public uint Version 
        { 
            get { return version; } 
            set 
            {
                if (version != value)
                {
                    version = value;

                    // update version on frames
                    // this might trigger a recompression
                    foreach (BgfBitmap bgf in Frames)
                        bgf.Version = value;

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VERSION));
                }
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public uint MaxIndices
        {
            get
            {
                int val = 0;

                if (FrameSets.Count > 0)
                {
                    val = FrameSets[0].FrameIndices.Count;

                    if (FrameSets.Count > 1)
                        for (int i = 1; i < FrameSets.Count; i++)                        
                            if (val < FrameSets[i].FrameIndices.Count)
                                val = FrameSets[i].FrameIndices.Count;
                        
                }

                return (uint)val;
            }
        }

        /// <summary>
        /// An integer scale-factor for included frames.
        /// The higher the resolution of the images, the higher the shrink usually.
        /// Dividing the imagesizes by this ShrinkFactor give the size of the texture in the world.
        /// Must be greater than zero.
        /// </summary>
        public uint ShrinkFactor 
        {
            get { return (shrinkFactor > 0) ? shrinkFactor : 1; }
            set
            {
                // turn 0 into 1
                if (value == 0)
                    value = 1;

                if (shrinkFactor != value)
                {
                    shrinkFactor = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SHRINKFACTOR));
                }
            }
        }

        /// <summary>
        /// The list with included images (BgfBitmap)
        /// </summary>
        public BaseList<BgfBitmap> Frames { get { return frames; } }
        
        /// <summary>
        /// The list with included framesets/groups
        /// </summary>
        public BaseList<BgfFrameSet> FrameSets { get { return frameSets; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public BgfFile()
        {            
            Clear(false);
        }

        /// <summary>
        /// Constructor from file
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="Buffer"></param>
        public BgfFile(string Filename, byte[] Buffer = null)
        {
            // get extension of filename
            string extension = Path.GetExtension(Filename).ToLower();

            // if it's .bgf load it
            if (extension == FileExtensions.BGF)
                Load(Filename, Buffer);

            else
                throw new Exception(ERRORFILEFORMAT);   
        }

        #endregion

        #region Methods
        /// <summary>
        /// Checks if a Frame (given by Index) is referenced in any FrameSet.
        /// </summary>
        /// <param name="Index">Index of frame to check</param>
        /// <returns></returns>
        public bool IsFrameIndexLinkedInFrameSet(int Index)
        {
            foreach (BgfFrameSet frameset in FrameSets)
                foreach (int value in frameset.FrameIndices)
                    if (value == Index)
                        return true;

            return false;
        }

        /// <summary>
        /// Updates all references of Frame given by OldIndex 
        /// to NewIndex in all FrameSets. Useful if you move
        /// a frame's position in the list and it has existing 
        /// references in FrameSets.
        /// </summary>
        /// <param name="OldIndex">The old index of the Frame</param>
        /// <param name="NewIndex">The new index of the Frame</param>
        public void UpdateFrameIndexInFrameSets(int OldIndex, int NewIndex)
        {
            foreach (BgfFrameSet frameset in FrameSets)
                for (int i = 0; i < frameset.FrameIndices.Count; i++ )
                    if (frameset.FrameIndices[i] == OldIndex)
                        frameset.FrameIndices[i] = NewIndex;
        }
       
        /// <summary>
        /// Iterates all BgfBitmaps and decompresses them if still compressed.
        /// </summary>
        public void DecompressAllSingle()
        {
            foreach (BgfBitmap frame in Frames)
                frame.IsCompressed = false;
        }

        /// <summary>
        /// Decompresses all still compressed frames in parallel.
        /// </summary>
        public void DecompressAllParallel()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.IsCompressed = false; });
        }

        /// <summary>
        /// Decompresses all still compressed frames either 
        /// parallel or single.
        /// </summary>
        public void DecompressAll()
        {
            if (frames.Count >= 8 && frames[0].IsCompressed)
                DecompressAllParallel();

            else
                DecompressAllSingle();
        }

        /// <summary>
        /// Iterates all BgfBitmaps and compresses them if decompressed.
        /// </summary>
        public void CompressAll()
        {
            foreach (BgfBitmap frame in Frames)
                frame.IsCompressed = true;
        }

        /// <summary>
        /// Returns the BgfBitmap frame for given Group and Angle.
        /// </summary>
        /// <param name="Group"></param>
        /// <param name="Angle"></param>
        /// <returns>BgfBitmap or null</returns>
        public BgfBitmap GetFrame(int Group, ushort Angle)
        {
            int index = GetFrameIndex(Group, Angle);

            if (index > -1)
                return Frames[index];
            else
                return null;
        }

        /// <summary>
        /// Resolves the index of a BgfBitmap frame for given Group and Angle
        /// from the FrameSets.
        /// </summary>
        /// <param name="Group"></param>
        /// <param name="Angle"></param>
        /// <returns>Index, -1 if not found</returns>
        public int GetFrameIndex(int Group, ushort Angle)
        {
            // index is zero based, group nr. isn't
            int groupindex = Group - 1;
            if (groupindex > -1 && FrameSets.Count > groupindex)
            {
                int entries = FrameSets[groupindex].FrameIndices.Count;

                if (entries > 0)
                {
                    Real frameanglewidth = (Real)GeometryConstants.MAXANGLE / (Real)entries;
                    int indexforangle = Convert.ToInt32((Real)Angle / frameanglewidth);

                    if (indexforangle >= FrameSets[groupindex].FrameIndices.Count)
                        indexforangle = 0;

                    int frameindex = FrameSets[groupindex].FrameIndices[indexforangle];

                    if (frameindex > -1)
                        return frameindex;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds a new frameset/group with increased num at the end
        /// </summary>
        public void AddFrameSet()
        {
            FrameSets.Add(
                new BgfFrameSet((uint)(FrameSets.Count + 1), new List<int>()));
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Filename = DEFAULTFILENAME;
                Version = VERSION10;
                Name = String.Empty;
                ShrinkFactor = 1;

                frames.Clear();
                frameSets.Clear();
            }
            else
            {
                filename = DEFAULTFILENAME;
                version = VERSION10;
                name = String.Empty;
                shrinkFactor = 1;

                frames.Clear();
                frameSets.Clear();
            }
        }
        #endregion

        /// <summary>
        /// Converts all frames from vale colortable to new color table.
        /// </summary>
        public void ConvertFromVale()
        {
            foreach (BgfBitmap f in Frames)
                f.ConvertFromVale();
        }

        /// <summary>
        /// Calls Cut() on all existing BgfBitmap instances
        /// </summary>
        public void Cut()
        {
            foreach (BgfBitmap f in frames)
                f.Cut();
        }

        /// <summary>
        /// Calls Cut() on all existing BgfBitmap instances
        /// using Parallel looping.
        /// </summary>
        public void CutParallel()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.Cut(); });
        }

        /// <summary>
        /// Calls GrayScaleByShade() on all existing BgfBitmap instances
        /// using Parallel looping.
        /// </summary>
        public void GrayScaleByShade()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.GrayScaleByShade(); });
        }

        /// <summary>
        /// Calls GrayScaleWeightedSum() on all existing BgfBitmap instances
        /// using Parallel looping.
        /// </summary>
        public void GrayScaleWeightedSum()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.GrayScaleWeightedSum(); });
        }

        /// <summary>
        /// Calls GrayScaleDesaturate() on all existing BgfBitmap instances
        /// using Parallel looping.
        /// </summary>
        public void GrayScaleDesaturate()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.GrayScaleDesaturate(); });
        }

        /// <summary>
        /// Calls GrayScaleDecompose() on all existing BgfBitmap instances
        /// using Parallel looping.
        /// </summary>
        public void GrayScaleDecompose()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.GrayScaleDecompose(); });
        }

        /// <summary>
        /// Calls RevertPixelDataToOriginal() on all existing BgfBitmap instances
        /// using Parallel looping.
        /// </summary>
        public void RevertPixelDataToOriginal()
        {
            Parallel.ForEach<BgfBitmap>(frames, frame => { frame.RevertPixelDataToOriginal(); });
        }
        #region BUILDDEPENDENT

#if DRAWING
        /// <summary>
        /// Loads this BgfFile instance from XML/BMP
        /// </summary>
        /// <param name="Filename"></param>
        public void LoadXml(string Filename)
        {
            string path = Path.GetDirectoryName(Filename);

            // save raw filename without path or extensions
            Filename = Path.GetFileNameWithoutExtension(Filename);

            // init XML reader
            XmlReader reader = XmlReader.Create(Filename);

            // rootnode
            reader.ReadToFollowing("bgf");
            uint version = Convert.ToUInt32(reader["version"]);
            if (version >= BgfFile.VERSION9)
            {
                ShrinkFactor = Convert.ToUInt32(reader["shrink"]);

                // skip maxindices, we use dynamic getter
                // MaxIndices = Convert.ToUInt32(reader["maxindices"]);

                // frames
                reader.ReadToFollowing("frames");
                int FrameCount = Convert.ToInt32(reader["count"]);
                Frames.Capacity = FrameCount;
                for (int i = 0; i < FrameCount; i++)
                {
                    reader.ReadToFollowing("frame");
                    uint width = Convert.ToUInt32(reader["width"]);
                    uint height = Convert.ToUInt32(reader["height"]);
                    int xoffset = Convert.ToInt32(reader["xoffset"]);
                    int yoffset = Convert.ToInt32(reader["yoffset"]);
                    string file = reader["file"];

                    // hotspots
                    reader.ReadToFollowing("hotspots");
                    byte hotspotcount = Convert.ToByte(reader["count"]);
                    List<BgfBitmapHotspot> hotspots = new List<BgfBitmapHotspot>(hotspotcount);
                    for (int j = 0; j < hotspotcount; j++)
                    {
                        reader.ReadToFollowing("hotspot");
                        sbyte index = Convert.ToSByte(reader["index"]);
                        int x = Convert.ToInt32(reader["x"]);
                        int y = Convert.ToInt32(reader["y"]);

                        BgfBitmapHotspot hotspot = new BgfBitmapHotspot(index, x, y);
                        hotspots.Add(hotspot);
                    }

                    // load bitmap from file
                    Bitmap bmp = (Bitmap)Image.FromFile(path + "/" + file);

                    byte[] pixelData = BgfBitmap.BitmapToPixelData(bmp);
                    BgfBitmap bgfBitmap = new BgfBitmap(
                        (uint)i + 1,
                        version,
                        width,
                        height,
                        xoffset,
                        yoffset,
                        hotspots,
                        false,
                        0,
                        pixelData);

                    // cleanp temporary bitmap
                    bmp.Dispose();

                    Frames.Add(bgfBitmap);
                }

                // framesets
                reader.ReadToFollowing("framesets");
                int FrameSetCount = Convert.ToInt32(reader["count"]);
                FrameSets.Capacity = FrameSetCount;
                for (int i = 0; i < FrameSetCount; i++)
                {
                    reader.ReadToFollowing("frameset");
                    string[] indices = reader["indices"].Split(' ');
                    List<int> intIndices = new List<int>();
                    foreach (string index in indices)
                        intIndices.Add(Convert.ToInt32(index));

                    BgfFrameSet bgfFrameSet = new BgfFrameSet((uint)i + 1, intIndices);

                    FrameSets.Add(bgfFrameSet);
                }
            }
            else
                throw new Exception("Wrong format version: " + version + " (expected " + BgfFile.VERSION9 + ").");
        }

        /// <summary>
        /// Exports this BgfFile instance to XML/BMP
        /// </summary>
        /// <param name="Filename"></param>
        public void WriteXml(string Filename)
        {
            string path = Path.GetDirectoryName(Filename);
            string filename = Path.GetFileNameWithoutExtension(Filename);

            // export BMPs
            for (int i = 0; i < Frames.Count; i++)
                File.WriteAllBytes(Path.Combine(path, filename + "-" + i + FileExtensions.BMP), Frames[i].PixelDataToBitmapBytes());

            // start writing XML
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            XmlWriter writer = XmlWriter.Create(Path.Combine(path, filename + FileExtensions.XML), settings);

            // begin
            writer.WriteStartDocument();
            writer.WriteStartElement("bgf");
            writer.WriteAttributeString("version", "10");
            writer.WriteAttributeString("shrink", ShrinkFactor.ToString());
            writer.WriteAttributeString("maxindices", MaxIndices.ToString());

            // frames
            writer.WriteStartElement("frames");
            writer.WriteAttributeString("count", Frames.Count.ToString());
            writer.WriteAttributeString("imageformat", "bmp");
            for (int i = 0; i < Frames.Count; i++)
            {
                writer.WriteStartElement("frame");
                writer.WriteAttributeString("width", Frames[i].Width.ToString());
                writer.WriteAttributeString("height", Frames[i].Height.ToString());
                writer.WriteAttributeString("xoffset", Frames[i].XOffset.ToString());
                writer.WriteAttributeString("yoffset", Frames[i].YOffset.ToString());

                writer.WriteAttributeString("file", Filename + "-" + i.ToString() + FileExtensions.BMP);

                // hotspots
                writer.WriteStartElement("hotspots");
                writer.WriteAttributeString("count", Frames[i].HotSpots.Count.ToString());
                for (int j = 0; j < Frames[i].HotSpots.Count; j++)
                {
                    writer.WriteStartElement("hotspot");
                    writer.WriteAttributeString("index", Frames[i].HotSpots[j].Index.ToString());
                    writer.WriteAttributeString("x", Frames[i].HotSpots[j].X.ToString());
                    writer.WriteAttributeString("y", Frames[i].HotSpots[j].Y.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // framesets
            writer.WriteStartElement("framesets");
            writer.WriteAttributeString("count", FrameSets.Count.ToString());
            for (int i = 0; i < FrameSets.Count; i++)
            {
                writer.WriteStartElement("frameset");
                string indices = String.Empty;
                for (int j = 0; j < FrameSets[i].FrameIndices.Count; j++)
                {
                    indices += FrameSets[i].FrameIndices[j] + " ";
                }
                indices = indices.TrimEnd();
                writer.WriteAttributeString("indices", indices);
                writer.WriteEndElement();
            }

            // end
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }
#endif

        #endregion
    }
}
