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
using System.ComponentModel;
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Cumulated model for Effects in Meridian
    /// </summary>
    [Serializable]
    public class Effects : INotifyPropertyChanged, ITickable
    {
        #region Constants
        public const string PROPNAME_BLIND      = "Blind";
        public const string PROPNAME_PARALYZE   = "Paralyze";
        public const string PROPNAME_RAINING    = "Raining";
        public const string PROPNAME_SNOWING    = "Snowing";
        public const string PROPNAME_SAND       = "Sand";

        public const string PROPNAME_INVERT     = "Invert";
        public const string PROPNAME_PAIN       = "Pain";
        public const string PROPNAME_WHITEOUT   = "Whiteout";
        public const string PROPNAME_BLUR       = "Blur";
        public const string PROPNAME_FLASHXLAT  = "FlashXLat";
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected EffectBlind blind;
        protected EffectParalyze paralyze;
        protected EffectRaining raining;
        protected EffectSnowing snowing;
        protected EffectSand sand;

        protected EffectInvert invert;
        protected EffectPain pain;
        protected EffectWhiteOut whiteout;
        protected EffectBlur blur;
        protected EffectFlashXLat flashxlat;
        #endregion

        #region Properties
        /// <summary>
        /// The Blind effect
        /// </summary>
        public EffectBlind Blind
        {
            get { return blind; }
            protected set
            {
                if (blind != value)
                {
                    blind = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_BLIND));
                }
            }
        }

        /// <summary>
        /// The Paralyze/Hold effect
        /// </summary>
        public EffectParalyze Paralyze
        {
            get { return paralyze; }
            protected set
            {
                if (paralyze != value)
                {
                    paralyze = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PARALYZE));
                }
            }
        }

        /// <summary>
        /// The raining effect
        /// </summary>
        public EffectRaining Raining
        {
            get { return raining; }
            protected set
            {
                if (raining != value)
                {
                    raining = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RAINING));
                }
            }
        }

        /// <summary>
        /// The snowing effect
        /// </summary>
        public EffectSnowing Snowing
        {
            get { return snowing; }
            protected set
            {
                if (snowing != value)
                {
                    snowing = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SNOWING));
                }
            }
        }

        /// <summary>
        /// The sandstorm effect
        /// </summary>
        public EffectSand Sand
        {
            get { return sand; }
            protected set
            {
                if (sand != value)
                {
                    sand = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SAND));
                }
            }
        }

        /// <summary>
        /// The invert effect
        /// </summary>
        public EffectInvert Invert
        {
            get { return invert; }
            protected set
            {
                if (invert != value)
                {
                    invert = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_INVERT));
                }
            }
        }

        /// <summary>
        /// The pain effect
        /// </summary>
        public EffectPain Pain
        {
            get { return pain; }
            protected set
            {
                if (pain != value)
                {
                    pain = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PAIN));
                }
            }
        }

        /// <summary>
        /// The whiteout effect
        /// </summary>
        public EffectWhiteOut Whiteout
        {
            get { return whiteout; }
            protected set
            {
                if (whiteout != value)
                {
                    whiteout = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_WHITEOUT));
                }
            }
        }

        /// <summary>
        /// The blur effect
        /// </summary>
        public EffectBlur Blur
        {
            get { return blur; }
            protected set
            {
                if (blur != value)
                {
                    blur = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_BLUR));
                }
            }
        }

        /// <summary>
        /// The flash xlat effect
        /// </summary>
        public EffectFlashXLat FlashXLat
        {
            get { return flashxlat; }
            protected set
            {
                if (flashxlat != value)
                {
                    flashxlat = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLASHXLAT));
                }
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Effects()
        {
            blind = new EffectBlind();
            paralyze = new EffectParalyze();
            raining = new EffectRaining();
            snowing = new EffectSnowing();
            sand = new EffectSand();

            invert = new EffectInvert();
            pain = new EffectPain();
            whiteout = new EffectWhiteOut();
            blur = new EffectBlur();
            flashxlat = new EffectFlashXLat();

            Clear(false);
        }

        /// <summary>
        /// Call regularly to update durationeffect progress.
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(double Tick, double Span)
        {
            // update duration effects

            invert.Update(Tick);
            pain.Update(Tick);
            whiteout.Update(Tick);
            blur.Update(Tick);
            flashxlat.Update(Tick);
        }

        /// <summary>
        /// Handles a new effect information into the current state.
        /// </summary>
        /// <param name="Effect"></param>
        public void HandleEffect(Effect Effect)
        {
            switch (Effect.EffectType)
            {
                case EffectType.Invert:         // 1
                    invert.StartOrExtend(((DurationEffect)Effect).Duration);
                    break;

                case EffectType.Shake:          // 2
                    //shake.StartOrExtend(((DurationEffect)Effect).Duration);
                    break;

                case EffectType.Paralyze:       // 3
                    paralyze.IsActive = true;
                    break;

                case EffectType.Release:        // 4
                    paralyze.IsActive = false;
                    break;

                case EffectType.Blind:          // 5
                    blind.IsActive = true;
                    break;

                case EffectType.See:            // 6
                    blind.IsActive = false;
                    break;

                case EffectType.Pain:           // 7
                    pain.StartOrExtend(((DurationEffect)Effect).Duration);                    
                    break;

                case EffectType.Blur:           // 8
                    blur.StartOrExtend(((DurationEffect)Effect).Duration);
                    break;

                case EffectType.Raining:        // 9
                    raining.IsActive = true;
                    break;

                case EffectType.Snowing:        // 10
                    snowing.IsActive = true;
                    break;

                case EffectType.ClearWeather:   // 11
                    snowing.IsActive = false;
                    raining.IsActive = false;
                    break;

                case EffectType.Sand:           // 12
                    sand.IsActive = true;
                    break;

                case EffectType.ClearSand:      // 13
                    sand.IsActive = false;
                    break;

                case EffectType.Waver:          // 14
                    //waver.StartOrExtend(((DurationEffect)Effect).Duration);
                    break;

                case EffectType.FlashXLat:      // 15
                    flashxlat.XLat = ((EffectFlashXLat)Effect).XLat;
                    flashxlat.StartOrExtend(((DurationEffect)Effect).Duration);
                    break;

                case EffectType.WhiteOut:       // 16
                    whiteout.StartOrExtend(((DurationEffect)Effect).Duration);
                    break;

                case EffectType.XLatOverride:   // 17
                    // TODO?
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public void Clear(bool RaiseChangedEvent)
        {
            blind.Clear(RaiseChangedEvent);
            paralyze.Clear(RaiseChangedEvent);
            raining.Clear(RaiseChangedEvent);
            snowing.Clear(RaiseChangedEvent);
            sand.Clear(RaiseChangedEvent);

            invert.Clear(RaiseChangedEvent);
            pain.Clear(RaiseChangedEvent);
            whiteout.Clear(RaiseChangedEvent);
            blur.Clear(RaiseChangedEvent);
            flashxlat.Clear(RaiseChangedEvent);
        }
    }
}
