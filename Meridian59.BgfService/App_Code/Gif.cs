using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using System.Linq;
using System.Runtime.InteropServices;

/// <summary>
/// GIF File Format implementation. Reads and writes data in a GIF.
/// </summary>
/// <remarks>
/// 1) GIF File Format: http://www.matthewflickinger.com/lab/whatsinagif/bits_and_bytes.asp
/// 2) Official GIF87a: https://www.w3.org/Graphics/GIF/spec-gif87.txt
/// 3) Official GIF89a: http://www6.uniovi.es/gifanim/gif89a.txt
/// 4) Netscape Looping: http://www.vurdalakov.net/misc/gif/netscape-looping-application-extension
/// </remarks>
public class Gif
{
    /// <summary>
    /// Returns next greater power of 2
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static uint NextPowerOf2(uint n)
    {
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        n++;

        return n;
    }

    /// <summary>
    /// GIF Extension Base Class
    /// </summary>
    public abstract class Extension
    {
        public enum Type : byte
        {
            PlainText       = 0x01,
            GraphicsControl = 0xF9,
            Comment         = 0xFE,
            Application     = 0xFF
        }

        public static Type Extract(BinaryReader Reader, out Extension Extension)
        {
            Type type = (Type)Reader.ReadByte();
            switch (type)
            {
                case Type.PlainText:        Extension = new ExtensionGeneric(Reader);         break; //todo
                case Type.GraphicsControl:  Extension = new ExtensionGraphicsControl(Reader); break;
                case Type.Comment:          Extension = new ExtensionGeneric(Reader);         break; //todo
                case Type.Application:      Extension = ExtensionApplication.Extract(Reader); break;
                default:                    Extension = new ExtensionGeneric(Reader);         break;
            }
            return type;
        }

        public abstract void Read(BinaryReader Reader);
        public abstract void Write(BinaryWriter Writer);

    }

    /// <summary>
    /// Generic/Unknown Extension
    /// </summary>
    public class ExtensionGeneric : Extension
    {
        public byte ExtensionType;
        public readonly List<byte[]> Chunks = new List<byte[]>();
        public ExtensionGeneric(BinaryReader Reader)
        {
            Read(Reader);
        }
        public override void Read(BinaryReader Reader)
        {
            while (true)
            {
                byte len = Reader.ReadByte();
                if (len > 0)
                {
                    Chunks.Add(Reader.ReadBytes(len));
                }
                else
                    break;
            }
        }
        public override void Write(BinaryWriter Writer)
        {
            Writer.Write((byte)Introducer.Extension);
            Writer.Write((byte)ExtensionType);
            foreach (byte[] chunk in Chunks)
            {
                Writer.Write((byte)chunk.Length);
                Writer.Write(chunk);
            }
            Writer.Write((byte)0x00);
        }
    }

    /// <summary>
    /// GraphicsControl Extension
    /// </summary>
    public class ExtensionGraphicsControl : Extension
    {
        public struct Flags
        {
            static class Bitmasks
            {
                public const uint TRANSPARENTCOLORFLAG = 0x01; // BITS 0
                public const uint USERINPUTFLAG = 0x02; // BITS 1
                public const uint DISPOSALMETHOD = 0x1C; // BITS 2-4
                public const uint RESERVEDFORFUTUREUSE = 0xE0; // BITS 5-7
            }
            public enum DisposalMethods
            {
                NotSpecified = 0,
                DoNotDispose = 1,
                RestoreToBackground = 2,
                RestoreToPrevious = 3
            }
            public uint Value { get; set; }
            public bool IsTransparentColor
            {
                get { return (Value & Bitmasks.TRANSPARENTCOLORFLAG) == Bitmasks.TRANSPARENTCOLORFLAG; }
                set
                {
                    if (value) Value |= Bitmasks.TRANSPARENTCOLORFLAG;
                    else Value &= ~Bitmasks.TRANSPARENTCOLORFLAG;
                }
            }
            public bool IsUserInput
            {
                get { return (Value & Bitmasks.USERINPUTFLAG) == Bitmasks.USERINPUTFLAG; }
                set
                {
                    if (value) Value |= Bitmasks.USERINPUTFLAG;
                    else Value &= ~Bitmasks.USERINPUTFLAG;

                }
            }
            public DisposalMethods DisposalMethod
            {
                get { return (DisposalMethods)((Value & Bitmasks.DISPOSALMETHOD) >> 2); }
                set
                {
                    Value &= ~Bitmasks.DISPOSALMETHOD;
                    Value |= (((uint)value << 2) & Bitmasks.DISPOSALMETHOD);
                }
            }
        }

        public Flags Packed;
        public ushort DelayTime;
        public byte TransparentColorIndex;

        public ExtensionGraphicsControl() { }
        public ExtensionGraphicsControl(BinaryReader Reader)
        {
            Read(Reader);
        }

        public override void Read(BinaryReader Reader)
        {
            if (Reader.ReadByte() == 0x04)
            {
                Packed.Value = Reader.ReadByte();
                DelayTime = Reader.ReadUInt16();
                TransparentColorIndex = Reader.ReadByte();
            }
            else
                throw new Exception("Unexpected Length of GraphicsControl extension");

            if (Reader.ReadByte() != 0x00)
                throw new Exception("Failed to read zero termination in GraphicsControl extension");
        }

        public override void Write(BinaryWriter Writer)
        {
            Writer.Write((byte)Introducer.Extension);
            Writer.Write((byte)Extension.Type.GraphicsControl);
            Writer.Write((byte)0x04);
            Writer.Write((byte)Packed.Value);
            Writer.Write((ushort)DelayTime);
            Writer.Write((byte)TransparentColorIndex);
            Writer.Write((byte)0x00);
        }
    }

    /// <summary>
    /// Application Extension
    /// </summary>
    public abstract class ExtensionApplication : Extension
    {
        public static ExtensionApplication Extract(BinaryReader Reader)
        {
            if (Reader.ReadByte() == 0x0B)
            {
                byte[] identifier = Reader.ReadBytes(8);
                byte[] authenticationCode = Reader.ReadBytes(3);

                if (identifier.SequenceEqual(ExtensionNetscape.ID_NETSCAPE2) &&
                    authenticationCode.SequenceEqual(ExtensionNetscape.CODE_NETSCAPE2))
                {
                    return new ExtensionNetscape(Reader);
                }
                else
                    return new ExtensionApplicationGeneric(Reader, identifier, authenticationCode);
            }
            else
                throw new Exception("Unexpected length of Application extension");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtensionApplicationGeneric : ExtensionApplication
    {
        public readonly byte[] Identifier;
        public readonly byte[] AuthenticationCode;
        public readonly List<byte[]> Chunks = new List<byte[]>();

        public ExtensionApplicationGeneric() { }
        public ExtensionApplicationGeneric(BinaryReader Reader, byte[] Identifier, byte[] AuthenticationCode)
        {
            this.Identifier = Identifier;
            this.AuthenticationCode = AuthenticationCode;
            Read(Reader);
        }
        public override void Read(BinaryReader Reader)
        {
            while (true)
            {
                byte len = Reader.ReadByte();
                if (len > 0)
                {
                    Chunks.Add(Reader.ReadBytes(len));
                }
                else
                    break;
            }
        }
        public override void Write(BinaryWriter Writer)
        {
            Writer.Write((byte)Introducer.Extension);
            Writer.Write((byte)Extension.Type.Application);
            Writer.Write((byte)0x0B);
            Writer.Write(Identifier);
            Writer.Write(AuthenticationCode);
            foreach (byte[] chunk in Chunks)
            {
                Writer.Write((byte)chunk.Length);
                Writer.Write(chunk);
            }
            Writer.Write((byte)0x00);
        }
    }

    /// <summary>
    /// NETSCAPE2.0 Application Extension with REPEAT count
    /// </summary>
    public class ExtensionNetscape : ExtensionApplication
    {
        public static readonly byte[] ID_NETSCAPE2 = new byte[] { 0x4E, 0x45, 0x54, 0x53, 0x43, 0x41, 0x50, 0x45 };
        public static readonly byte[] CODE_NETSCAPE2 = new byte[] { 0x32, 0x2E, 0x30 };
        public ushort Repeat;
        public ExtensionNetscape(BinaryReader Reader)
        {
            Read(Reader);
        }
        public ExtensionNetscape(ushort Repeat) : base()
        {
            this.Repeat = Repeat;
        }
        public override void Read(BinaryReader Reader)
        {
            if (Reader.ReadByte() == 0x03)
            {
                if (Reader.ReadByte() == 0x01)
                {
                    Repeat = Reader.ReadUInt16();
                }
                else
                    throw new Exception("Unexpected ID constant in Netscape extension data");
            }
            else
                throw new Exception("Unexpected length of Netscape extension data");

            if (Reader.ReadByte() != 0x00)
                throw new Exception("Failed to read zero termination in Netscape extension");
        }
        public override void Write(BinaryWriter Writer)
        {
            Writer.Write((byte)Introducer.Extension);
            Writer.Write((byte)Extension.Type.Application);
            Writer.Write((byte)0x0B);
            Writer.Write(ID_NETSCAPE2);
            Writer.Write(CODE_NETSCAPE2);
            Writer.Write((byte)0x03);
            Writer.Write((byte)0x01);
            Writer.Write((ushort)Repeat);
            Writer.Write((byte)0x00);
        }
    }

    /// <summary>
    /// GIF Frame
    /// </summary>
    public class Frame
    {
        public struct Flags
        {
            static class Bitmasks
            {
                public const uint SIZEOFLOCALCOLORTABLE = 0x07; // BITS 0-2
                public const uint RESERVEDFORFUTUREUSE  = 0x18; // BITS 3-4
                public const uint SORTFLAG              = 0x20; // BITS 5
                public const uint INTERLACEFLAG         = 0x40; // BITS 6
                public const uint LOCALCOLORTABLEFLAG   = 0x80; // BITS 7
            }
            public uint Value { get; set; }
            public bool IsSort
            {
                get { return (Value & Bitmasks.SORTFLAG) == Bitmasks.SORTFLAG; }
                set
                {
                    if (value) Value |= Bitmasks.SORTFLAG;
                    else Value &= ~Bitmasks.SORTFLAG;
                }
            }
            public bool IsInterlace
            {
                get { return (Value & Bitmasks.INTERLACEFLAG) == Bitmasks.INTERLACEFLAG; }
                set
                {
                    if (value) Value |= Bitmasks.INTERLACEFLAG;
                    else Value &= ~Bitmasks.INTERLACEFLAG;

                }
            }
            public bool IsLocalColorTable
            {
                get { return (Value & Bitmasks.LOCALCOLORTABLEFLAG) == Bitmasks.LOCALCOLORTABLEFLAG; }
                set
                {
                    if (value) Value |= Bitmasks.LOCALCOLORTABLEFLAG;
                    else Value &= ~Bitmasks.LOCALCOLORTABLEFLAG;

                }
            }
            public uint SizeOfLocalColorTable
            {
                get { return (Value & Bitmasks.SIZEOFLOCALCOLORTABLE); }
                set
                {
                    Value &= ~Bitmasks.SIZEOFLOCALCOLORTABLE;
                    Value |= (value & Bitmasks.SIZEOFLOCALCOLORTABLE);
                }
            }
        }

        public ushort Left;
        public ushort Top;
        public ushort Width;
        public ushort Height;
        public Flags Packed;
        public byte[] ColorTable;
        public byte MinLZWCodeSize;
        public readonly List<byte[]> Chunks = new List<byte[]>();
        public ExtensionGraphicsControl GraphicsControl;

        public Frame(BinaryReader Reader)
        {
            Read(Reader);
        }

        public Frame(byte[] Pixels, int Width, int Height, uint[] Palette, uint ColorCount, LZWEncoder Encoder, ushort Delay, int TransparentColorIndex = -1)
        {
            if (Pixels == null)
                throw new ArgumentException("Pixels can't be null.");

            if (Width <= 0 || Height <= 0)
                throw new ArgumentException("Width and Height must be greater 0.");

            if (Palette == null || Palette.Length > 256)
                throw new ArgumentException("Palette null or more than 256 entries.");

            if (ColorCount > Palette.Length)
                throw new ArgumentException("ColorCount bigger than Palette size.");

            if (Encoder == null)
                throw new ArgumentException("Encoder can't be null.");

            // save provided dimension
            this.Width = (ushort)Width;
            this.Height = (ushort)Height;

            // encode pixels using argument encoder instance
            Encoder.Encode(Pixels, Chunks, Width, Height);
            MinLZWCodeSize = 8;

            // determine size of colortable to use
            // get next bigger power of 2 value of used colorcount
            uint palSize2 = NextPowerOf2(ColorCount);

            // use and create local color table with determined size
            Packed.IsLocalColorTable = true;
            Packed.SizeOfLocalColorTable = (uint)Math.Max((int)Math.Log(palSize2, 2.0) - 1, 0);
            ColorTable = new byte[palSize2 * 3];

            // fill color table
            int i = 0;
            for(uint j = 0; j < ColorCount; j++)
            {
                uint c = Palette[j];
                ColorTable[i] = (byte)((c & 0x00FF0000) >> 16); i++;
                ColorTable[i] = (byte)((c & 0x0000FF00) >> 8); i++;
                ColorTable[i] = (byte)(c & 0x000000FF); i++;
            }

            // set graphics control extension
            GraphicsControl = new ExtensionGraphicsControl();
            GraphicsControl.DelayTime = Delay;
            GraphicsControl.Packed.IsUserInput = false;
            GraphicsControl.Packed.DisposalMethod =
                ExtensionGraphicsControl.Flags.DisposalMethods.RestoreToBackground;

            // possibly set transparent color index and enable transparency
            if (TransparentColorIndex > -1 && TransparentColorIndex < 256)
            {
                GraphicsControl.Packed.IsTransparentColor = true;
                GraphicsControl.TransparentColorIndex = (byte)TransparentColorIndex;
            }
        }

        public void Read(BinaryReader Reader)
        {
            Left   = Reader.ReadUInt16();
            Top    = Reader.ReadUInt16();
            Width  = Reader.ReadUInt16();
            Height = Reader.ReadUInt16();
            Packed.Value = Reader.ReadByte();

            if (Packed.IsLocalColorTable)
            {
                int len = 3 * (int)Math.Pow(2, Packed.SizeOfLocalColorTable + 1);
                ColorTable = Reader.ReadBytes(len);
            }
            else
                ColorTable = new byte[0];

            MinLZWCodeSize = Reader.ReadByte();
            while (true)
            {
                byte len = Reader.ReadByte();
                if (len > 0)
                {
                    Chunks.Add(Reader.ReadBytes(len));
                }
                else
                    break;
            }
        }
        public void Write(BinaryWriter Writer)
        {
            Writer.Write((byte)Introducer.ImageDescriptor);
            Writer.Write((ushort)Left);
            Writer.Write((ushort)Top);
            Writer.Write((ushort)Width);
            Writer.Write((ushort)Height);
            Writer.Write((byte)Packed.Value);

            if (Packed.IsLocalColorTable)
                Writer.Write(ColorTable);

            Writer.Write((byte)MinLZWCodeSize);
            foreach (byte[] chunk in Chunks)
            {
                Writer.Write((byte)chunk.Length);
                Writer.Write(chunk);
            }
            Writer.Write((byte)0x00);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    static class Constants
    {
        public static readonly byte[] SIGNATURE = new byte[3] { 0x47, 0x49, 0x46 }; // GIF
        public static readonly byte[] VERSION_87A = new byte[3] { 0x38, 0x37, 0x61 }; // 87a
        public static readonly byte[] VERSION_89A = new byte[3] { 0x38, 0x39, 0x61 }; // 89a
    }

    enum Introducer : byte
    {
        Extension = 0x21,
        ImageDescriptor = 0x2C,
        Trailer = 0x3B
    }

    public struct Flags
    {
        static class Bitmasks
        {
            public const uint SIZEOFGLOBALCOLORTABLE = 0x07; // BITS 0-2
            public const uint SORTFLAG = 0x08; // BITS 3
            public const uint COLORRESOLUTION = 0x70; // BITS 4-6
            public const uint GLOBALCOLORTABLEFLAG = 0x80; // BITS 7

        }
        public uint Value { get; set; }
        public bool IsGlobalColorTable
        {
            get { return (Value & Bitmasks.GLOBALCOLORTABLEFLAG) == Bitmasks.GLOBALCOLORTABLEFLAG; }
            set
            {
                if (value) Value |= Bitmasks.GLOBALCOLORTABLEFLAG;
                else Value &= ~Bitmasks.GLOBALCOLORTABLEFLAG;
            }
        }
        public bool IsSortFlag
        {
            get { return (Value & Bitmasks.SORTFLAG) == Bitmasks.SORTFLAG; }
            set
            {
                if (value) Value |= Bitmasks.SORTFLAG;
                else Value &= ~Bitmasks.SORTFLAG;
            }
        }
        public uint SizeOfColorTable
        {
            get { return Value & Bitmasks.SIZEOFGLOBALCOLORTABLE; }
            set
            {
                Value &= ~Bitmasks.SIZEOFGLOBALCOLORTABLE;
                Value |= (value & Bitmasks.SIZEOFGLOBALCOLORTABLE);
            }
        }
        public uint ColorResolution
        {
            get { return (Value & Bitmasks.COLORRESOLUTION) >> 4; }
            set
            {
                Value &= ~Bitmasks.COLORRESOLUTION;
                Value |= ((value << 4) & Bitmasks.COLORRESOLUTION);
            }
        }
        public uint EntriesInColorTable { get { return (uint)Math.Pow(2.0, SizeOfColorTable + 1); } }
        public uint ByteSizeOfColorTable { get { return 3U * EntriesInColorTable; } }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////

    public ushort CanvasWidth;
    public ushort CanvasHeight;
    public Flags Packed;
    public byte BackgroundColorIndex;
    public byte PixelAspectRatio;
    public byte[] GlobalColorTable;
    public ExtensionApplication ApplicationExtension;
    public readonly List<Frame> Frames = new List<Frame>();
    public readonly List<Extension> Extensions = new List<Extension>();

    /// <summary>
    /// Constructor
    /// </summary>
    public Gif(ushort Width, ushort Height, ushort Repeat = 0)
    {
        CanvasWidth = Width;
        CanvasHeight = Height;
        Packed.Value = 0;
        GlobalColorTable = new byte[0];
        ApplicationExtension = new ExtensionNetscape(Repeat);
    }

    /// <summary>
    /// Constructor by Filename
    /// </summary>
    /// <param name="Filename"></param>
    public Gif(string Filename)
    {
        // try open the file
        Stream stream = new FileStream(
            Filename, FileMode.Open);

        // parse it
        Read(stream);

        // cleanup
        stream.Close();
        stream.Dispose();
    }

    /// <summary>
    /// Constructor by Stream
    /// </summary>
    /// <param name="Stream"></param>
    public Gif(Stream Stream)
    {
        Read(Stream);
    }

    /// <summary>
    /// Reads this Gif from a Stream
    /// </summary>
    /// <param name="Stream"></param>
    public void Read(Stream Stream)
    {
        BinaryReader reader = new BinaryReader(
            Stream, Encoding.ASCII, true);

        //////////////////////////////////////////////
        // HEADER
        //////////////////////////////////////////////
        byte[] buffer = new byte[3];

        // signature
        reader.Read(buffer, 0, 3);

        if (!buffer.SequenceEqual(Constants.SIGNATURE))
            throw new Exception("Not a GIF file");

        // version
        reader.Read(buffer, 0, 3);

        // check version
        if (!buffer.SequenceEqual(Constants.VERSION_89A) &&
            !buffer.SequenceEqual(Constants.VERSION_87A))
        {
            throw new Exception("Unsupported Version");
        }

        //////////////////////////////////////////////
        // LOGICAL SCREEN DESCRIPTOR DATA
        //////////////////////////////////////////////

        // width & height
        CanvasWidth = reader.ReadUInt16();
        CanvasHeight = reader.ReadUInt16();

        // other data
        Packed.Value = reader.ReadByte();
        BackgroundColorIndex = reader.ReadByte();
        PixelAspectRatio = reader.ReadByte();

        //////////////////////////////////////////////
        // GLOBAL COLOR TABLE (OPTIONAL)
        //////////////////////////////////////////////
        GlobalColorTable = (Packed.IsGlobalColorTable) ?
            reader.ReadBytes((int)Packed.ByteSizeOfColorTable) :
            new byte[0];

        //////////////////////////////////////////////
        // DYNAMIC CHUNKS (OPTIONAL, EXCEPT ONE FRAME)
        //////////////////////////////////////////////
        ExtensionGraphicsControl lastGraphicsControl = null;
        while (true)
        {
            byte introducer = reader.ReadByte();

            // EXTENSION
            if (introducer == (byte)Introducer.Extension)
            {
                // try parse it
                Extension extension;
                Extension.Type type = Extension.Extract(reader, out extension);

                // GraphicsControl
                if (type == Extension.Type.GraphicsControl)
                {
                    // save it for next frame
                    if (lastGraphicsControl == null)
                        lastGraphicsControl = (ExtensionGraphicsControl)extension;

                    // not good, we got another graphicscontrol extension
                    // without having read a frame since then...
                    else
                    {
                        // discard previous one, save this one
                        lastGraphicsControl = (ExtensionGraphicsControl)extension;
                    }
                }

                // Application
                else if (type == Extension.Type.Application)
                    ApplicationExtension = (ExtensionApplication)extension;

                // Generic
                else
                    Extensions.Add(extension);
            }

            // IMAGEDESCRIPTOR
            else if (introducer == (byte)Introducer.ImageDescriptor)
            {
                // parse frame data
                Frame frame = new Frame(reader);

                // attach the last GraphicsControl extension that was
                // not used yet and last read before this frame
                if (lastGraphicsControl != null)
                {
                    frame.GraphicsControl = lastGraphicsControl;
                    lastGraphicsControl = null;
                }

                // store frame
                Frames.Add(frame);
            }

            // TRAILER (PARSING SUCCESSFUL)
            else if (introducer == (byte)Introducer.Trailer)
                break;

            // UNKNOWN
            else
                throw new Exception("Unknown Dynamic Chunk Introducer");
        }

        reader.Dispose();
    }

    /// <summary>
    /// Writes this Gif to a Stream
    /// </summary>
    /// <param name="Stream"></param>
    public void Write(Stream Stream)
    {
        // create binary writer
        BinaryWriter writer = new BinaryWriter(
            Stream, Encoding.ASCII, true);

        //////////////////////////////////////////////
        // HEADER
        //////////////////////////////////////////////
        writer.Write(Constants.SIGNATURE);
        writer.Write(Constants.VERSION_89A);

        //////////////////////////////////////////////
        // LOGICAL SCREEN DESCRIPTOR
        //////////////////////////////////////////////

        // width & height
        writer.Write((ushort)CanvasWidth);
        writer.Write((ushort)CanvasHeight);

        // other data
        writer.Write((byte)Packed.Value);
        writer.Write((byte)BackgroundColorIndex);
        writer.Write((byte)PixelAspectRatio);

        //////////////////////////////////////////////
        // GLOBAL COLOR TABLE (OPTIONAL)
        //////////////////////////////////////////////
        if (Packed.IsGlobalColorTable)
            writer.Write(GlobalColorTable);

        //////////////////////////////////////////////
        // APPLICATION EXTENSION (OPTIONAL)
        //////////////////////////////////////////////
        if (ApplicationExtension != null)
            ApplicationExtension.Write(writer);

        //////////////////////////////////////////////
        // OTHER EXTENSIONS (OPTIONAL)
        //////////////////////////////////////////////
        foreach (Extension extension in Extensions)
            extension.Write(writer);

        //////////////////////////////////////////////
        // FRAMES
        //////////////////////////////////////////////
        foreach (Frame frame in Frames)
        {
            // write graphicscontrol extension
            // before frame if there is any
            if (frame.GraphicsControl != null)
                frame.GraphicsControl.Write(writer);

            // write frame
            frame.Write(writer);
        }

        //////////////////////////////////////////////
        // TRAILER
        //////////////////////////////////////////////
        writer.Write((byte)Introducer.Trailer);
        writer.Dispose();
    }

    public void Read(string Filename)
    {
        // try open the file
        Stream stream = new FileStream(Filename, FileMode.Open);

        // parse it
        Read(stream);

        // cleanup
        stream.Close();
        stream.Dispose();
    }

    public void Write(string Filename)
    {
        // try to create/overwrite file
        FileStream stream = new FileStream(Filename, FileMode.Create);

        // write gif
        Write(stream);

        stream.Flush();
        stream.Close();
        stream.Dispose();
    }

    /// <summary>
    /// LZW Compression for GIF
    /// </summary>
    /// <remarks>
    /// Created from misc. sources on the internet
    /// GIFCOMPR.C GIF Image compression routines
    /// David Rowley (mgardi@watdcsu.waterloo.edu)
    /// Based on: compress.c - File compression ala IEEE Computer, June 1984.
    /// By Authors:  Spencer W. Thomas      (decvax!harpo!utah-cs!utah-gr!thomas)
    ///              James A. Woods         (decvax!ihnp4!ames!jaw)
    ///              Joe Orost              (decvax!vax135!petsd!joe)
    /// </remarks>
    public class LZWEncoder
    {
        private const int BITS = 12;
        private const int HSIZE = 5003; // 80% occupancy
        private const int EOF = -1;
        private const int MAXBITS = BITS;         // user settable max # bits/code
        private const int MAXMAXCODE = 1 << BITS; // should NEVER generate this code
        private const int COLORDEPTH = 8;
        private const int CLEARCODE = 1 << COLORDEPTH;
        private const int EOFCODE = CLEARCODE + 1;
        private const int HSHIFT = 4;

        private static readonly int[] MASKS =
        {
            0x0000, 0x0001, 0x0003, 0x0007, 0x000F,
            0x001F, 0x003F, 0x007F, 0x00FF, 0x01FF,
            0x03FF, 0x07FF, 0x0FFF, 0x1FFF, 0x3FFF,
            0x7FFF, 0xFFFF
        };

        private static int MaxCode(int n_bits) { return (1 << n_bits) - 1; }

        private readonly int[] htab = new int[HSIZE];
        private readonly int[] codetab = new int[HSIZE];
        private readonly byte[] accum = new byte[256];

        private int curPixel;
        private int n_bits;    // number of bits/code
        private int maxcode;   // maximum code, given n_bits
        private int free_ent;  // first unused entry
        private bool clear_flg;
        private int cur_accum;
        private int cur_bits;
        private int a_count;

        public LZWEncoder()
        {
        }

        public void Encode(byte[] pixels, List<byte[]> chunks, int imgW, int imgH)
        {
            // save/reset some values
            curPixel = 0;
            cur_bits = 0;
            cur_accum = 0;

            // Set up the necessary values
            clear_flg = false;
            n_bits = COLORDEPTH + 1;
            maxcode = MaxCode(n_bits);
            free_ent = CLEARCODE + 2;

            // clear packet
            a_count = 0;

            //////////////////////////////////

            int fcode;
            int i;
            int c;
            int ent;
            int disp;

            ent = NextPixel(pixels);
            ResetCodeTable();
            Output(CLEARCODE, chunks);

           outer_loop:
            while ((c = NextPixel(pixels)) != EOF)
            {
                fcode = (c << MAXBITS) + ent;

                // xor hashing
                i = (c << HSHIFT) ^ ent;

                if (htab[i] == fcode)
                {
                    ent = codetab[i];
                    continue;
                }
                else if (htab[i] >= 0) // non-empty slot
                {
                    // secondary hash (after G. Knott)
                    disp = HSIZE - i;

                    if (i == 0)
                        disp = 1;

                    do
                    {
                        if ((i -= disp) < 0)
                            i += HSIZE;

                        if (htab[i] == fcode)
                        {
                            ent = codetab[i];
                            goto outer_loop;
                        }
                    }
                    while (htab[i] >= 0);
                }

                Output(ent, chunks);
                ent = c;

                if (free_ent < MAXMAXCODE)
                {
                    // code -> hashtable
                    codetab[i] = free_ent++;
                    htab[i] = fcode;
                }
                else
                    ClearTable(chunks);
            }

            // Put out the final code.
            Output(ent, chunks);
            Output(EOFCODE, chunks);
        }

        private void Add(byte c, List<byte[]> chunks)
        {
            accum[a_count++] = c;

            // buffer full, flush it
            if (a_count >= 255)
                Flush(chunks);
        }

        private void ClearTable(List<byte[]> chunks)
        {
            ResetCodeTable();

            free_ent = CLEARCODE + 2;
            clear_flg = true;

            Output(CLEARCODE, chunks);
        }

        private void ResetCodeTable()
        {
            for (int i = 0; i < HSIZE; ++i)
                htab[i] = -1;
        }

        private void Flush(List<byte[]> Chunks)
        {
            if (a_count > 0)
            {
                byte[] chunk = new byte[a_count];
                Array.Copy(accum, chunk, a_count);
                Chunks.Add(chunk);
                a_count = 0;
            }
        }

        private int NextPixel(byte[] pixAry)
        {
            if (curPixel >= pixAry.Length)
                return EOF;

            byte pixel = pixAry[curPixel];
            curPixel++;

            return pixel;
        }

        private void Output(int code, List<byte[]> chunks)
        {
            cur_accum &= MASKS[cur_bits];

            if (cur_bits > 0)
                cur_accum |= (code << cur_bits);
            else
                cur_accum = code;

            cur_bits += n_bits;

            while (cur_bits >= 8)
            {
                Add((byte)(cur_accum & 0xff), chunks);
                cur_accum >>= 8;
                cur_bits -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if (free_ent > maxcode || clear_flg)
            {
                if (clear_flg)
                {
                    maxcode = MaxCode(n_bits = COLORDEPTH + 1);
                    clear_flg = false;
                }
                else
                {
                    ++n_bits;
                    if (n_bits == MAXBITS)
                        maxcode = MAXMAXCODE;
                    else
                        maxcode = MaxCode(n_bits);
                }
            }

            if (code == EOFCODE)
            {
                // At EOF, write the rest of the buffer.
                while (cur_bits > 0)
                {
                    Add((byte)(cur_accum & 0xff), chunks);
                    cur_accum >>= 8;
                    cur_bits -= 8;
                }

                Flush(chunks);
            }
        }
    }
}
