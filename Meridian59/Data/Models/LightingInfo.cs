using System;
using System.ComponentModel;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Info about an objects lighting.
    /// </summary>
    [Serializable]
    public class LightingInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<LightingInfo>
    {
        #region Constants
        public const string PROPNAME_FLAGS = "Flags";
        public const string PROPNAME_LIGHTINTENSITY = "LightIntensity";
        public const string PROPNAME_LIGHTCOLOR = "LightColor";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region IByteSerializable
        public int ByteLength
        {
            get
            {
                // LightFlags (short)
                int len = TypeSizes.SHORT;

                // If object has lighting, other two properties are set.
                // intensity(byte) + color(short).
                if (flags.IsLightOn)
                    len += TypeSizes.BYTE + TypeSizes.SHORT;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(flags.Flags)), 0, Buffer, cursor, TypeSizes.SHORT); // LightFlags (2 bytes).
            cursor += TypeSizes.SHORT;

            if (flags.IsLightOn)
            {
                Array.Copy(BitConverter.GetBytes(Convert.ToByte(lightIntensity)), 0, Buffer, cursor, TypeSizes.BYTE); // Intensity (1 byte).
                cursor += TypeSizes.BYTE;

                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(lightColor)), 0, Buffer, cursor, TypeSizes.SHORT); // Color (2 bytes).
                cursor += TypeSizes.SHORT;
            }

            return ByteLength;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            flags = new LightFlags(BitConverter.ToUInt16(Buffer, cursor));       // Flags (2 bytes).
            cursor += TypeSizes.SHORT;

            if (flags.IsLightOn)
            {
                lightIntensity = Buffer[cursor]; // Intensity, (1 byte).
                cursor++;

                lightColor = BitConverter.ToUInt16(Buffer, cursor); // Color (2 bytes).
                cursor += TypeSizes.SHORT;
            }

            return ByteLength;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {

            flags = new LightFlags(*((ushort*)Buffer));
            Buffer += TypeSizes.SHORT;

            if (flags.IsLightOn)
            {
                lightIntensity = Buffer[0];
                Buffer++;

                lightColor = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;
            }
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = flags.Flags;
            Buffer += TypeSizes.SHORT;

            if (flags.IsLightOn)
            {
                Buffer[0] = lightIntensity;
                Buffer++;

                *((uint*)Buffer) = lightColor;
                Buffer += TypeSizes.SHORT;
            }
        }

        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
        }
        #endregion

        #region Fields
        protected LightFlags flags;
        protected byte lightIntensity;
        protected ushort lightColor;
        #endregion

        #region Properties
        /// <summary>
        /// Light flags, e.g. on, dynamic, wavering
        /// </summary>
        public LightFlags Flags
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }

        /// <summary>
        /// Intensity of light (0-255).
        /// </summary>
        public byte LightIntensity
        {
            get { return lightIntensity; }
            set
            {
                if (lightIntensity != value)
                {
                    lightIntensity = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LIGHTINTENSITY));
                }
            }
        }

        /// <summary>
        /// A 16-Bit(A1R5G5B5?) color of the light.
        /// </summary>
        public ushort LightColor
        {
            get { return lightColor; }
            set
            {
                if (lightColor != value)
                {
                    lightColor = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LIGHTCOLOR));
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public LightingInfo()
        {
            Clear(false);
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="Flags"></param>
        /// <param name="LightIntensity"></param>
        /// <param name="LightColor"></param>
        public LightingInfo(LightFlags Flags, byte LightIntensity, ushort LightColor)
        {
            flags = Flags;
            lightIntensity = LightIntensity;
            lightColor = LightColor;
        }

        /// <summary>
        /// Constructor by managed parser
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="startIndex"></param>
        public LightingInfo(byte[] RawData, int startIndex = 0)
        {
            ReadFrom(RawData, startIndex);
        }

        /// <summary>
        /// Constructor by pointer parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe LightingInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a string like (0,0,0)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return '(' + flags.ToString() + '/' + lightIntensity.ToString() + '/' + lightColor.ToString() + ')';
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Flags = new LightFlags();
                LightIntensity = 0;
                LightColor = 0;

            }
            else
            {
                flags = new LightFlags();
                lightIntensity = 0;
                lightColor = 0;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(LightingInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Flags = Model.Flags;
                LightIntensity = Model.LightIntensity;
                LightColor = Model.LightColor;
            }
            else
            {
                flags = Model.Flags;
                lightIntensity = Model.LightIntensity;
                lightColor = Model.LightColor;
            }
        }
        #endregion
    }


}
