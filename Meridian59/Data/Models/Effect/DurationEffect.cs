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
using Meridian59.Common.Constants;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An Effect which has a Duration.
    /// </summary>
    [Serializable]
    public abstract class DurationEffect : Effect
    {
        public const string PROPNAME_DURATION = "Duration";
        public const string PROPNAME_PROGRESS = "Progress";

        #region IByteSerializable
        public override int ByteLength { 
            get { 
                return base.ByteLength + TypeSizes.INT; } }
        
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
          
            Array.Copy(BitConverter.GetBytes(Duration), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            Duration = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;
            
            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = Duration;  
            Buffer += TypeSizes.INT;            
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            Duration = *((uint*)Buffer);
            Buffer += TypeSizes.INT;            
        }
        #endregion

        protected uint duration = 0;
        protected Real progress = 0.0f;
        protected double startTick = 0;
        
        /// <summary>
        /// The duration the effect lasts.
        /// </summary>
        public uint Duration 
        {
            get { return duration; }
            set
            {
                if (duration != value)
                {
                    duration = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DURATION));
                }
            }
        }

        /// <summary>
        /// This value is between 0.0 (playback start)
        /// and 1.0 (playback finished).
        /// </summary>
        public Real Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PROGRESS));
                }
            }
        }

        public DurationEffect(EffectType EffectType, uint Duration = 0)
            : base(EffectType)
        {
            this.Duration = Duration;           
        }

        public DurationEffect(byte[] Buffer, int StartIndex)
            : base(Buffer, StartIndex) { }

        public unsafe DurationEffect(ref byte* Buffer)
            : base(ref Buffer) { }

        public void Update(double CurrentTick)
        {
            if (isActive)
            {
                // catch first execution to set starttick
                if (startTick == 0)
                    startTick = CurrentTick;

                else
                {
                    double span = CurrentTick - startTick;

                    // still playing
                    if (span < duration)
                        Progress = (Real)span / (Real)duration;

                    // finished
                    else
                    {
                        Progress = 1.0f;
                        IsActive = false;
                    }
                }
            }
        }

        public void StartOrExtend(uint Value)
        {
            // extend duration if already active
            if (isActive)            
                Duration += Value;
            
            // start new effect
            else
            {
                Duration = Value;
                Progress = 0.0f;
                IsActive = true;
                startTick = 0;                
            }
        }

        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Progress = 0.0f;
                startTick = 0;
                Duration = 0;
            }
            else
            {
                progress = 0.0f;
                startTick = 0;
                duration = 0;
            }
        }

        public override string ToString()
        {
            return "DurationEffect";
        }
    }
}
