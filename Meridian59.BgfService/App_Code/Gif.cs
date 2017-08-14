using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


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
    public Gif(ushort Width, ushort Height, ushort Repeat = 0, byte ColorResolution = 7)
    {
        CanvasWidth = Width;
        CanvasHeight = Height;
        Packed.ColorResolution = ColorResolution;
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

    public void AddFrame(Image Image, ushort Delay)
    {
        // create memorystream to hold temporary gif
        MemoryStream memStream = new MemoryStream();

        // use .NET GIF encoder to create a compatible frame
        Image.Save(memStream, ImageFormat.Gif);
        //Image.Save("test2.gif", ImageFormat.Gif);

        // reset streamposition for reading
        memStream.Position = 0;

        // parse the generated gif
        Gif gif = new Gif(memStream);

        // must have at least one frame in there
        if (gif.Frames.Count > 0)
        {
            Frame frame = gif.Frames[0];

            // possibly create a GraphicsControl extension for this frame
            if (frame.GraphicsControl == null)
                frame.GraphicsControl = new ExtensionGraphicsControl();

            // attach global color palette as local
            if (!frame.Packed.IsLocalColorTable)
            {
                if (gif.Packed.IsGlobalColorTable)
                {
                    frame.ColorTable = gif.GlobalColorTable;
                    frame.Packed.SizeOfLocalColorTable = gif.Packed.SizeOfColorTable;
                    //frame.GraphicsControl.TransparentColorIndex = gif.BackgroundColorIndex;
                }
                else
                    throw new Exception("No ColorTable in .NET encoded GIF");
            }

            // enable local colortable and set delay
            frame.Packed.IsLocalColorTable = true;
            frame.GraphicsControl.DelayTime = Delay;

            // set flags
            frame.GraphicsControl.Packed.IsTransparentColor = true;
            frame.GraphicsControl.Packed.IsUserInput = false;
            frame.GraphicsControl.Packed.DisposalMethod =
                ExtensionGraphicsControl.Flags.DisposalMethods.RestoreToBackground;

            // add the frame to our gif
            Frames.Add(frame);
        }
        else
            throw new Exception("No Frame in .NET encoded GIF");

        memStream.Close();
        memStream.Dispose();
    }
}
