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
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Data.Models;
using Meridian59.Files.BGF;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// Represents a side of a wall.
    /// </summary>
    [Serializable]
    public class RooSideDef : IByteSerializableFast, IResourceResolvable
    {
        // from roomanim.h:
        // "Number of milliseconds per pixel scrolled for various scrolling texture speeds"
        protected const int SCROLL_SLOW_PERIOD      = 96; // SCROLL_WALL_SLOW_PERIOD
        protected const int SCROLL_MEDIUM_PERIOD    = 32; // SCROLL_WALL_MEDIUM_PERIOD
        protected const int SCROLL_FAST_PERIOD      = 8;  // SCROLL_WALL_FAST_PERIOD

        #region IByteSerializable
        public int ByteLength 
        {
            get 
            { 
                return TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT +
                    TypeSizes.INT + TypeSizes.BYTE; 
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(ServerID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(MiddleTexture), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(UpperTexture), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(LowerTexture), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Flags.Value), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Buffer[cursor] = Speed;
            cursor++;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((short*)Buffer) = ServerID;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = MiddleTexture;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = UpperTexture;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = LowerTexture;
            Buffer += TypeSizes.SHORT;

            *((uint*)Buffer) = Flags.Value;
            Buffer += TypeSizes.INT;

            Buffer[0] = Speed;
            Buffer++;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            ServerID = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            MiddleTexture = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            UpperTexture = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            LowerTexture = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Flags = new RooSideDefFlags(BitConverter.ToUInt32(Buffer, cursor));
            cursor += TypeSizes.INT;

            Speed = Buffer[cursor];
            cursor++;

            // create animation if any
            CreateAnimation();

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ServerID = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            MiddleTexture = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            UpperTexture = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            LowerTexture = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Flags = new RooSideDefFlags(*((uint*)Buffer));
            Buffer += TypeSizes.INT;

            Speed = Buffer[0];
            Buffer++;

            // create animation if any
            CreateAnimation();
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

        public event WallTextureChangedEventHandler TextureChanged;

        #region Properties
        public int Num { get; set; }
        public short ServerID { get; set; }
        public ushort MiddleTexture { get; set; }
        public ushort UpperTexture { get; set; }
        public ushort LowerTexture { get; set; }
        public RooSideDefFlags Flags { get; set; }
        public byte Speed { get; set; }

        public BgfFile ResourceUpper { get; protected set; }
        public BgfFile ResourceMiddle { get; protected set; }
        public BgfFile ResourceLower { get; protected set; }

        public object UserData { get; set; }

        public V2 SpeedUpper { get; protected set; }
        public V2 SpeedMiddle { get; protected set; }
        public V2 SpeedLower { get; protected set; }

        public string TextureNameUpper { get; protected set; }

        public string TextureNameMiddle { get; protected set; }

        public string TextureNameLower { get; protected set; }

        public string MaterialNameUpper { get; protected set; }
        public string MaterialNameMiddle { get; protected set; }
        public string MaterialNameLower { get; protected set; }

        public BgfBitmap TextureUpper { get; protected set; }
        public BgfBitmap TextureMiddle { get; protected set; }
        public BgfBitmap TextureLower { get; protected set; }
        #endregion

        #region Animation
        protected Animation animation;
        public Animation Animation
        {
            get { return animation; }
            set
            {
                if (animation != value)
                {
                    if (animation != null)
                        animation.PropertyChanged -= OnAnimationPropertyChanged;

                    animation = value;

                    if (animation != null)                    
                        animation.PropertyChanged += OnAnimationPropertyChanged;

                    // update
                    SetMiddleTexture(MiddleTexture, ResourceMiddle);

                    //if (TextureChanged != null)
                    //    TextureChanged(this, new WallTextureChangedEventArgs(this, WallPartType.Middle));                   
                }
            }
        }

        protected void OnAnimationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // update middle texture without animation reset
            SetMiddleTexture(MiddleTexture, ResourceMiddle, false);

            // others ?
        }

        protected void CreateAnimation()
        {
            // setup cycle animation
            if (Speed != 0)
            {
                int period = 10000 / Speed;
                this.Animation = new AnimationCycle((uint)period, 1, 1);
            }
            // scroll animation
            else if (Flags.ScrollSpeed != TextureScrollSpeed.NONE)
            {
                Speed = (byte)Flags.ScrollSpeed;
                // we handle this with internal mogre texture scrolling atm
            }
        }
        #endregion

        #region Constructors
        public RooSideDef()
        {
            this.Flags = new RooSideDefFlags();
        }

        public RooSideDef(
            short ServerID, 
            ushort MiddleTexture, 
            ushort UpperTexture, 
            ushort LowerTexture,
            uint Flags, 
            byte Speed)
        {
            this.ServerID = ServerID;
            this.MiddleTexture = MiddleTexture;
            this.UpperTexture = UpperTexture;
            this.LowerTexture = LowerTexture;
            this.Flags = new RooSideDefFlags(Flags);
            this.Speed = Speed;

            SpeedUpper = V2.ZERO;
            SpeedMiddle = V2.ZERO;
            SpeedLower = V2.ZERO;
        }

        public RooSideDef(byte[] Buffer, int StartIndex = 0)
        {
            SpeedUpper = V2.ZERO;
            SpeedMiddle = V2.ZERO;
            SpeedLower = V2.ZERO;

            ReadFrom(Buffer, StartIndex);
        }

        public unsafe RooSideDef(ref byte* Buffer)
        {
            SpeedUpper = V2.ZERO;
            SpeedMiddle = V2.ZERO;
            SpeedLower = V2.ZERO;

            ReadFrom(ref Buffer);
        }
        #endregion

        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            ushort group = (animation != null) ? animation.CurrentGroup : (ushort)1;
            int frameidx = 0;
           
            /******************************************************************************/

            if (UpperTexture > 0)
                ResourceUpper = M59ResourceManager.GetRoomTexture(UpperTexture);

            if (MiddleTexture > 0)
                ResourceMiddle = M59ResourceManager.GetRoomTexture(MiddleTexture);

            if (LowerTexture > 0)
                ResourceLower = M59ResourceManager.GetRoomTexture(LowerTexture);

            /******************************************************************************/

            if (animation != null)
            {
                // note: possibly need animation instance for each parttype?

                if (ResourceMiddle != null)
                    animation.GroupMax = ResourceMiddle.FrameSets.Count;

                /*else if (ResourceLower != null)
                    animation.GroupMax = ResourceLower.FrameSets.Count;

                else if (ResourceUpper != null)
                    animation.GroupMax = ResourceUpper.FrameSets.Count;*/
            }    

            /******************************************************************************/

            if (ResourceUpper != null && ResourceUpper.Frames.Count > 0)
            {
                frameidx = ResourceUpper.GetFrameIndex(group, 0);
                frameidx = Math.Max(frameidx, 0);

                if (ResourceUpper.Frames.Count > frameidx)
                {
                    TextureUpper = ResourceUpper.Frames[frameidx];
                    SpeedUpper = GetWallScrollSpeed(TextureUpper.Width, TextureUpper.Height);
                }
                else
                {
                    TextureUpper = null;
                    SpeedUpper = V2.ZERO;
                }

                TextureNameUpper = RooFile.GetNameForTexture(
                    ResourceUpper, frameidx);

                MaterialNameUpper = RooFile.GetNameForMaterial(
                    ResourceUpper, frameidx, SpeedUpper);
            }
            else
            {
                TextureUpper = null;
                SpeedUpper = V2.ZERO;
                TextureNameUpper = null;
                MaterialNameUpper = null;
            }

            /******************************************************************************/

            if (ResourceMiddle != null && ResourceMiddle.Frames.Count > 0)
            {
                frameidx = ResourceMiddle.GetFrameIndex(group, 0);
                frameidx = Math.Max(frameidx, 0);

                if (ResourceMiddle.Frames.Count > frameidx)
                {
                    TextureMiddle = ResourceMiddle.Frames[frameidx];
                    SpeedMiddle = GetWallScrollSpeed(TextureMiddle.Width, TextureMiddle.Height);
                }
                else
                {
                    TextureMiddle = null;
                    SpeedMiddle = V2.ZERO;
                }
                
                TextureNameMiddle = RooFile.GetNameForTexture(
                    ResourceMiddle, frameidx);

                MaterialNameMiddle = RooFile.GetNameForMaterial(
                    ResourceMiddle, frameidx, SpeedMiddle);
            }
            else
            {
                TextureMiddle = null;
                SpeedMiddle = V2.ZERO;
                TextureNameMiddle = null;
                MaterialNameMiddle = null;
            }

            /******************************************************************************/

            if (ResourceLower != null && ResourceLower.Frames.Count > 0)
            {
                frameidx = ResourceLower.GetFrameIndex(group, 0);
                frameidx = Math.Max(frameidx, 0);

                if (ResourceLower.Frames.Count > frameidx)
                {
                    TextureLower = ResourceLower.Frames[frameidx];
                    SpeedLower = GetWallScrollSpeed(TextureLower.Width, TextureLower.Height);
                }
                else
                {
                    TextureLower = null;
                    SpeedLower = V2.ZERO;
                }

                TextureNameLower = RooFile.GetNameForTexture(
                    ResourceLower, frameidx);

                MaterialNameLower = RooFile.GetNameForMaterial(
                    ResourceLower, frameidx, SpeedLower);
            }
            else
            {
                TextureLower = null;
                SpeedLower = V2.ZERO;
                TextureNameLower = null;
                MaterialNameLower = null;
            }
        }
        #endregion

        /// <summary>
        /// Sets lower texture to another num
        /// </summary>
        /// <param name="TextureNum"></param>
        /// <param name="TextureFile"></param>
        public void SetLowerTexture(ushort TextureNum, BgfFile TextureFile)
        {
            ushort group = (animation != null) ? animation.CurrentGroup : (ushort)1;
            int frameidx = 0;
            string oldmaterial = MaterialNameLower;
       
            LowerTexture = TextureNum;
            ResourceLower = TextureFile;

            if (ResourceLower != null && ResourceLower.Frames.Count > 0)
            { 
                frameidx = ResourceLower.GetFrameIndex(group, 0);
                frameidx = Math.Max(frameidx, 0);

                if (ResourceLower.Frames.Count > frameidx)
                {
                    TextureLower = ResourceLower.Frames[frameidx];
                    SpeedLower = GetWallScrollSpeed(TextureLower.Width, TextureLower.Height);
                }
                else
                {
                    TextureLower = null;
                    SpeedLower = V2.ZERO;
                }

                TextureNameLower = RooFile.GetNameForTexture(
                    ResourceLower, frameidx);

                MaterialNameLower = RooFile.GetNameForMaterial(
                    ResourceLower, frameidx, SpeedLower);
            }
            else
            {
                TextureLower = null;
                SpeedLower = V2.ZERO;
                TextureNameLower = null;
                MaterialNameLower = null;
            }

            if (TextureChanged != null)
                TextureChanged(this, new WallTextureChangedEventArgs(this, WallPartType.Lower, oldmaterial));
        }

        /// <summary>
        /// Sets middle texture to another num
        /// </summary>
        /// <param name="TextureNum"></param>
        /// <param name="TextureFile"></param>
        /// <param name="ResetAnimation"></param>
        public void SetMiddleTexture(ushort TextureNum, BgfFile TextureFile, bool ResetAnimation = true)
        {
            ushort group = (animation != null) ? animation.CurrentGroup : (ushort)1;
            int frameidx = 0;
            string oldmaterial = MaterialNameMiddle;

            MiddleTexture = TextureNum;
            ResourceMiddle = TextureFile;

            if (ResourceMiddle != null && ResourceMiddle.Frames.Count > 0)
            {
                // possibly reset animation
                if (animation != null && ResetAnimation)
                { 
                    animation.SetValues(1, ResourceMiddle.FrameSets.Count, false);
                    group = animation.CurrentGroup;
                }

                frameidx = ResourceMiddle.GetFrameIndex(group, 0);
                frameidx = Math.Max(frameidx, 0);

                if (ResourceMiddle.Frames.Count > frameidx)
                {
                    TextureMiddle = ResourceMiddle.Frames[frameidx];
                    SpeedMiddle = GetWallScrollSpeed(TextureMiddle.Width, TextureMiddle.Height);
                }
                else
                {
                    TextureMiddle = null;
                    SpeedMiddle = V2.ZERO;
                }

                TextureNameMiddle = RooFile.GetNameForTexture(
                    ResourceMiddle, frameidx);

                MaterialNameMiddle = RooFile.GetNameForMaterial(
                    ResourceMiddle, frameidx, SpeedMiddle);
            }
            else
            {
                TextureMiddle = null;
                SpeedMiddle = V2.ZERO;
                TextureNameMiddle = null;
                MaterialNameMiddle = null;
            }

            if (TextureChanged != null)
                TextureChanged(this, new WallTextureChangedEventArgs(this, WallPartType.Middle, oldmaterial));
        }

        /// <summary>
        /// Sets lower texture to another num
        /// </summary>
        /// <param name="TextureNum"></param>
        /// <param name="TextureFile"></param>
        public void SetUpperTexture(ushort TextureNum, BgfFile TextureFile)
        {
            ushort group = (animation != null) ? animation.CurrentGroup : (ushort)1;
            int frameidx = 0;
            string oldmaterial = MaterialNameUpper;

            UpperTexture = TextureNum;
            ResourceUpper = TextureFile;

            if (ResourceUpper != null && ResourceUpper.Frames.Count > 0)
            {
                frameidx = ResourceUpper.GetFrameIndex(group, 0);
                frameidx = Math.Max(frameidx, 0);

                if (ResourceUpper.Frames.Count > frameidx)
                {
                    TextureUpper = ResourceUpper.Frames[frameidx];
                    SpeedUpper = GetWallScrollSpeed(TextureUpper.Width, TextureUpper.Height);
                }
                else
                {
                    TextureUpper = null;
                    SpeedUpper = V2.ZERO;
                }

                TextureNameUpper = RooFile.GetNameForTexture(
                    ResourceUpper, frameidx);

                MaterialNameUpper = RooFile.GetNameForMaterial(
                    ResourceUpper, frameidx, SpeedUpper);
            }
            else
            {
                TextureUpper = null;
                SpeedUpper = V2.ZERO;
                TextureNameUpper = null;
                MaterialNameUpper = null;
            }

            if (TextureChanged != null)
                TextureChanged(this, new WallTextureChangedEventArgs(this, WallPartType.Upper, oldmaterial));
        }

        /// <summary>
        /// Returns ogre U, V scrollspeed for given SideFlags and bgf
        /// </summary>
        /// <param name="TextureWidth"></param>
        /// <param name="TextureHeight"></param>
        /// <returns></returns>
        protected V2 GetWallScrollSpeed(uint TextureWidth, uint TextureHeight)
        {
            V2 sp       = V2.ZERO;
            Real length = 0.0f;
             
            // no scrolling (0/0) if
            // 1) no texture size
            // 2) scrollspeed set to zero
            // 3) floor and floorscrolling off
            // 4) ceiling and ceilingscrolling off
            if (TextureWidth == 0 || 
                TextureHeight == 0 ||
                Flags.ScrollSpeed == TextureScrollSpeed.NONE)
                return sp;
           
            // build direction
            switch (Flags.ScrollDirection)
            {
                case TextureScrollDirection.N:
                    sp.Y += 0;
                    sp.X += -1;
                    length = TextureHeight;
                    break;

                case TextureScrollDirection.NE:
                    sp.Y += 1;
                    sp.X += -1;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;

                case TextureScrollDirection.E:
                    sp.Y += 1;
                    sp.X += 0;
                    length = TextureWidth;
                    break;

                case TextureScrollDirection.SE:
                    sp.Y += 1;
                    sp.X += 1;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;

                case TextureScrollDirection.S:
                    sp.Y += 0;
                    sp.X += 1;
                    length = TextureHeight;
                    break;

                case TextureScrollDirection.SW:
                    sp.Y += -1;
                    sp.X += 1;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;

                case TextureScrollDirection.W:
                    sp.Y += -1;
                    sp.X += 0;
                    length = TextureWidth;
                    break;

                case TextureScrollDirection.NW:
                    sp.Y += -1;
                    sp.X += -1;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;
            }

            // multiply the amount of pixels
            // with the milliseconds required to scroll by 1 pixel
            // to get the time it requires to scroll the whole texture.
            switch (Flags.ScrollSpeed)
            {
                case TextureScrollSpeed.NONE:
                    length = 0.0f;
                    sp = V2.ZERO;
                    break;

                case TextureScrollSpeed.SLOW:
                    length *= SCROLL_SLOW_PERIOD;
                    break;

                case TextureScrollSpeed.MEDIUM:
                    length *= SCROLL_MEDIUM_PERIOD;
                    break;

                case TextureScrollSpeed.FAST:
                    length *= SCROLL_FAST_PERIOD;
                    break;
            }

            // scale the scrolldirection to match the given speed
            // ogre vectorlen 1.0f is 1 full texture scroll in 1 second
            if (length > 0.0f)
                sp.ScaleToLength(1000.0f / length);
            
            return sp;
        }        
    }
}
