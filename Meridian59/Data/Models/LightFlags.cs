using System;
using System.ComponentModel;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class LightFlags : INotifyPropertyChanged
    {
        #region CONSTANTS
        public string PROPNAME_FLAGS = "Flags";
        #endregion

        #region Bitmasks
        private const uint LIGHT_FLAG_NONE      = 0x0000;
        private const uint LIGHT_FLAG_ON        = 0x0001;
        private const uint LIGHT_FLAG_DYNAMIC   = 0x0002;
        private const uint LIGHT_FLAG_WAVERING  = 0x0004;
        private const uint LIGHT_FLAG_HIGHLIGHT = 0x0008;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        protected uint flags;

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

        public LightFlags(uint Flags = 0)
        {
            this.Flags = Flags;
        }

        public override string ToString()
        {
            return Flags.ToString();
        }

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
    }
}
