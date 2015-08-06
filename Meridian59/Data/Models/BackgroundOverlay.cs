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
using Meridian59.Common.Constants;
using Meridian59.Files;
using Meridian59.Files.BGF;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A BackgroundOverlay
    /// </summary>
    [Serializable]
    public class BackgroundOverlay : ObjectID, IClearable, IStringResolvable, IResourceResolvable, IUpdatable<BackgroundOverlay>
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_OVERLAYFILERID = "OverlayFileRID";
        public const string PROPNAME_NAMERID = "NameRID";
        public const string PROPNAME_FIRSTANIMATIONTYPE = "FirstAnimationType";
        public const string PROPNAME_COLORTRANSLATION = "ColorTranslation";
        public const string PROPNAME_EFFECT = "Effect";
        public const string PROPNAME_ANIMATION = "Animation";
        public const string PROPNAME_ANGLE = "Angle";
        public const string PROPNAME_HEIGHT = "Height";
        public const string PROPNAME_OVERLAYFILE = "OverlayFile";
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_RESOURCE = "Resource";
        #endregion

        #region IByteSerializable
        public override int ByteLength
        {
            get 
            {
                int len = base.ByteLength + TypeSizes.INT + TypeSizes.INT;

                if (firstAnimationType > 0)
                    len += TypeSizes.BYTE + TypeSizes.BYTE;

                len += animation.ByteLength;
                len += TypeSizes.SHORT + TypeSizes.SHORT;

                return  len;
            }
        }
        
        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(overlayFileRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(nameRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            if (firstAnimationType == AnimationType.TRANSLATION)
            {
                Buffer[cursor] = (byte)firstAnimationType;
                cursor++;

                Buffer[cursor] = colorTranslation;
                cursor++;
            }
            else if (firstAnimationType == AnimationType.EFFECT)
            {
                Buffer[cursor] = (byte)firstAnimationType;
                cursor++;

                Buffer[cursor] = effect;
                cursor++;
            }

            cursor += animation.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(angle), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(height), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }
        
        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);
            
            nameRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            overlayFileRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            if ((AnimationType)Buffer[cursor] == AnimationType.TRANSLATION)                        
            {
                firstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                colorTranslation = Buffer[cursor];
                cursor++;
            }
            else if (((AnimationType)Buffer[cursor] == AnimationType.EFFECT))
            {
                firstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                effect = Buffer[cursor];
                cursor++;
            }

            animation = Animation.ExtractAnimation(Buffer, cursor);
            cursor += animation.ByteLength;

            angle = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            height = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = nameRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = overlayFileRID;
            Buffer += TypeSizes.INT;

            if (firstAnimationType == AnimationType.TRANSLATION)
            {
                Buffer[0] = (byte)firstAnimationType;
                Buffer++;

                Buffer[0] = colorTranslation;
                Buffer++;
            }
            else if (firstAnimationType == AnimationType.EFFECT)
            {
                Buffer[0] = (byte)firstAnimationType;
                Buffer++;

                Buffer[0] = effect;
                Buffer++;
            }

            animation.WriteTo(ref Buffer);

            *((ushort*)Buffer) = angle;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = height;
            Buffer += TypeSizes.SHORT;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            nameRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            overlayFileRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            if ((AnimationType)Buffer[0] == AnimationType.TRANSLATION)
            {
                firstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                colorTranslation = Buffer[0];
                Buffer++;
            }
            else if (((AnimationType)Buffer[0] == AnimationType.EFFECT))
            {
                firstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                effect = Buffer[0];
                Buffer++;
            }

            animation = Animation.ExtractAnimation(ref Buffer);

            angle = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            height = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;
        }

        #endregion

        #region Fields
        protected uint overlayFileRID;
        protected uint nameRID;
        protected AnimationType firstAnimationType;
        protected byte colorTranslation;
        protected byte effect;
        protected Animation animation;
        protected ushort angle;
        protected short height;

        protected string name;
        protected string overlayFile;
        protected BgfFile resource;
        #endregion

        #region Properties
        public uint OverlayFileRID
        {
            get { return overlayFileRID; }
            set
            {
                if (overlayFileRID != value)
                {
                    overlayFileRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_OVERLAYFILERID));
                }
            }
        }

        public uint NameRID
        {
            get { return nameRID; }
            set
            {
                if (nameRID != value)
                {
                    nameRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NAMERID));
                }
            }
        }

        

        public AnimationType FirstAnimationType
        {
            get { return firstAnimationType; }
            set
            {
                if (firstAnimationType != value)
                {
                    firstAnimationType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FIRSTANIMATIONTYPE));
                }
            }
        }

        public byte ColorTranslation
        {
            get { return colorTranslation; }
            set
            {
                if (colorTranslation != value)
                {
                    colorTranslation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COLORTRANSLATION));
                }
            }
        }

        public byte Effect
        {
            get { return effect; }
            set
            {
                if (effect != value)
                {
                    effect = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_EFFECT));
                }
            }
        }

        public Animation Animation
        {
            get { return animation; }
            set
            {
                if (animation != value)
                {
                    animation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));
                }
            }
        }

        public ushort Angle
        {
            get { return angle; }
            set
            {
                if (angle != value)
                {
                    angle = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANGLE));
                }
            }
        }

        public short Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEIGHT));
                }
            }
        }

        // Extended
        public string OverlayFile
        {
            get { return overlayFile; }
            set
            {
                if (overlayFile != value)
                {
                    overlayFile = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_OVERLAYFILE));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NAME));
                }
            }
        }

        public BgfFile Resource
        {
            get { return resource; }
            set
            {
                if (resource != value)
                {
                    resource = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCE));
                }
            }
        }

        #endregion

        #region Constructors
        public BackgroundOverlay() : base()
        {
        }

        public BackgroundOverlay(uint ObjectID, uint Count, uint ResourceID, uint StringID, 
            AnimationType FirstAnimationType, byte ColorTranslation, byte Effect, Animation Animation,
            ushort Angle, short Height)
            : base (ObjectID, Count)
        {
            this.overlayFileRID = ResourceID;
            this.nameRID = StringID;
            this.firstAnimationType = FirstAnimationType;
            this.colorTranslation = ColorTranslation;
            this.effect = Effect;
            this.angle = Angle;
            this.height = Height;
        }

        public BackgroundOverlay(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe BackgroundOverlay(ref byte* Buffer)
            : base(ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                OverlayFileRID = 0;
                NameRID = 0;
                FirstAnimationType = 0;
                ColorTranslation = 0;
                Effect = 0;
                Angle = 0;
                Height = 0;

                Name = String.Empty;
                OverlayFile = String.Empty;
            }
            else
            {
                overlayFileRID = 0;
                nameRID = 0;
                firstAnimationType = 0;
                colorTranslation = 0;
                effect = 0;
                angle = 0;
                height = 0;

                name = String.Empty;
                overlayFile = String.Empty;
            }
        }
        #endregion
        
        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {           
            string bg_res;
            string bg_name;

			StringResources.TryGetValue(nameRID, out bg_res);
			StringResources.TryGetValue(overlayFileRID, out bg_name);

            if (RaiseChangedEvent)
            {
                if (bg_res != null) OverlayFile = bg_res;
                else OverlayFile = String.Empty;

                if (bg_name != null) Name = bg_name;
                else Name = String.Empty;
            }
            else
            {
                if (bg_res != null) overlayFile = bg_res;
                else overlayFile = String.Empty;

                if (bg_name != null) name = bg_name;
                else name = String.Empty;
            }
        }
        #endregion

        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (Name != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    Resource = M59ResourceManager.GetObject(OverlayFile);
                }
                else
                {
                    resource = M59ResourceManager.GetObject(OverlayFile);
                }
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(BackgroundOverlay Model, bool RaiseChangedEvent)
        {            
            if (RaiseChangedEvent)
            {
                ID = Model.ID;
                Count = Model.Count;
                OverlayFileRID = Model.OverlayFileRID;
                NameRID = Model.NameRID;
                FirstAnimationType = Model.FirstAnimationType;
                ColorTranslation = Model.ColorTranslation;
                Effect = Model.Effect;
                Animation = Model.Animation;
                Angle = Model.Angle;
                Height = Model.Height;

                Name = Model.Name;
                OverlayFile = Model.OverlayFile;
            }
            else
            {
                id = Model.ID;
                count = Model.Count;
                overlayFileRID = Model.OverlayFileRID;
                nameRID = Model.NameRID;
                firstAnimationType = Model.FirstAnimationType;
                colorTranslation = Model.ColorTranslation;
                effect = Model.Effect;
                animation = Model.Animation;
                angle = Model.Angle;
                height = Model.Height;

                name = Model.Name;
                overlayFile = Model.OverlayFile;
            }
        }
        #endregion
    }
}
