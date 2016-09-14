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

        #region Bitmasks
        private const uint LIGHT_FLAG_NONE = 0x0000;
        private const uint LIGHT_FLAG_ON = 0x0001;
        private const uint LIGHT_FLAG_DYNAMIC = 0x0002;
        private const uint LIGHT_FLAG_WAVERING = 0x0004;
        private const uint LIGHT_FLAG_HIGHLIGHT = 0x0008;
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
                // Light flags (short)
                int len = TypeSizes.SHORT;

                // If object has lighting, other two properties are set.
                // intensity(byte) + color(short).
                if (IsLightOn)
                    len += TypeSizes.BYTE + TypeSizes.SHORT;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(flags)), 0, Buffer, cursor, TypeSizes.SHORT); // Light flags (2 bytes).
            cursor += TypeSizes.SHORT;

            if (IsLightOn)
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

            flags = BitConverter.ToUInt16(Buffer, cursor); // Flags (2 bytes).
            cursor += TypeSizes.SHORT;

            if (IsLightOn)
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

            flags = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            if (IsLightOn)
            {
                lightIntensity = Buffer[0];
                Buffer++;

                lightColor = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;
            }
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = flags;
            Buffer += TypeSizes.SHORT;

            if (IsLightOn)
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
        protected uint flags;
        protected byte lightIntensity;
        protected ushort lightColor;
        #endregion

        #region Properties
        /// <summary>
        /// Light flags, e.g. on, dynamic, wavering
        /// </summary>
        public uint Flags
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

        #region Property Accessors
        public bool IsLightOn
        {
            get { return (Flags & LIGHT_FLAG_ON) == LIGHT_FLAG_ON; }
            set
            {
                if (value) Flags |= LIGHT_FLAG_ON;
                else Flags &= ~LIGHT_FLAG_ON;
            }
        }

        public bool IsLightDynamic
        {
            get { return (Flags & LIGHT_FLAG_DYNAMIC) == LIGHT_FLAG_DYNAMIC; }
            set
            {
                if (value) Flags |= LIGHT_FLAG_DYNAMIC;
                else Flags &= ~LIGHT_FLAG_DYNAMIC;
            }
        }

        public bool IsLightWavering
        {
            get { return (Flags & LIGHT_FLAG_WAVERING) == LIGHT_FLAG_WAVERING; }
            set
            {
                if (value) Flags |= LIGHT_FLAG_WAVERING;
                else Flags &= ~LIGHT_FLAG_WAVERING;
            }
        }

        public bool IsLightHighlight
        {
            get { return (Flags & LIGHT_FLAG_HIGHLIGHT) == LIGHT_FLAG_HIGHLIGHT; }
            set
            {
                if (value) Flags |= LIGHT_FLAG_HIGHLIGHT;
                else Flags &= ~LIGHT_FLAG_HIGHLIGHT;
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
        public LightingInfo(uint Flags, byte LightIntensity, ushort LightColor)
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
                Flags = 0;
                LightIntensity = 0;
                LightColor = 0;

            }
            else
            {
                flags = 0;
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
