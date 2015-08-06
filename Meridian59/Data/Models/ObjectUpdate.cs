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
using System.Collections.Generic;
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;
using Meridian59.Files;
using Meridian59.Files.BGF;
using Meridian59.Common;
using Meridian59.Drawing2D;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// ObjectUpdate for an Object known to the client (like a RoomObject or InventoryObject)
    /// </summary>
    [Serializable]
    public class ObjectUpdate : ObjectID, IStringResolvable, IResourceResolvable
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_OVERLAYFILERID = "OverlayFileRID";
        public const string PROPNAME_NAMERID = "NameRID";
        public const string PROPNAME_FLAGS = "Flags";
        public const string PROPNAME_LIGHTFLAGS = "LightFlags";
        public const string PROPNAME_LIGHTINTENSITY = "LightIntensity";
        public const string PROPNAME_LIGHTCOLOR = "LightColor";
        public const string PROPNAME_FIRSTANIMATIONTYPE = "FirstAnimationType";
        public const string PROPNAME_COLORTRANSLATION = "ColorTranslation";
        public const string PROPNAME_EFFECT = "Effect";
        public const string PROPNAME_ANIMATION = "Animation";
        public const string PROPNAME_SUBOVERLAYS = "SubOverlays";
        public const string PROPNAME_MOTIONFIRSTANIMATIONTYPE = "MotionFirstAnimationType";
        public const string PROPNAME_MOTIONCOLORTRANSLATION = "MotionColorTranslation";
        public const string PROPNAME_MOTIONEFFECT = "MotionEffect";
        public const string PROPNAME_MOTIONANIMATION = "MotionAnimation";
        public const string PROPNAME_MOTIONSUBOVERLAYS = "MotionSubOverlays";
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_OVERLAYFILE = "OverlayFile";
        public const string PROPNAME_RESOURCE = "Resource";
        #endregion

        #region IByteSerializable
        public override int ByteLength
        { 
            get {
                int len = base.ByteLength + TypeSizes.INT + TypeSizes.INT + flags.ByteLength + TypeSizes.SHORT;

                if (lightFlags > 0)
                    len += TypeSizes.BYTE + TypeSizes.SHORT;

                if (firstAnimationType > 0)
                    len += TypeSizes.BYTE + TypeSizes.BYTE;

                len += animation.ByteLength;

                len += TypeSizes.BYTE;
                foreach (SubOverlay subOverlay in subOverlays)
                    len += subOverlay.ByteLength;
                    
                if (motionFirstAnimationType > 0)
                    len += TypeSizes.BYTE + TypeSizes.BYTE;

                len += motionAnimation.ByteLength;

                len += TypeSizes.BYTE;
                foreach (SubOverlay subOverlay in motionSubOverlays)
                    len += subOverlay.ByteLength;

                return len;
            } 
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);                            // ObjectID (4/8 bytes)           

            overlayFileRID = BitConverter.ToUInt32(Buffer, cursor);              // MainOverlayID (4 bytes)
            cursor += TypeSizes.INT;

            nameRID = BitConverter.ToUInt32(Buffer, cursor);                   // StringID (4 bytes)
            cursor += TypeSizes.INT;

            flags.ReadFrom(Buffer, cursor);                                    // Flags (n bytes)
            cursor += flags.ByteLength;

            lightFlags = BitConverter.ToUInt16(Buffer, cursor);                 // LightFlags (2 bytes)
            cursor += TypeSizes.SHORT;

            if (lightFlags > 0)
            {
                lightIntensity = Buffer[cursor];                                // LightIntensity (1 byte)
                cursor++;

                lightColor = BitConverter.ToUInt16(Buffer, cursor);             // LightColor (2 bytes)
                cursor += TypeSizes.SHORT;
            }

            if ((AnimationType)Buffer[cursor] == AnimationType.TRANSLATION)   // check if there is a translation or effect for object                         
            {
                firstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                colorTranslation = Buffer[cursor];                        // ColorTranslation (1 byte)
                cursor++;            
            }
            else if ((AnimationType)Buffer[cursor] == AnimationType.EFFECT)
            {
                firstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                effect = Buffer[cursor];                                  // Effect (1 byte)
                cursor++;
            }

            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                colorTranslation = ColorTransformation.FILTERWHITE90;   

            animation = Animation.ExtractAnimation(Buffer, cursor);       // Animation (n bytes)
            cursor += animation.ByteLength;

            byte subOverlaysCount = Buffer[cursor];                             // Suboverlaycount (1 byte)
            cursor++;

            subOverlays.Clear();

            for (byte i = 0; i < subOverlaysCount; i++)                         // ObjectSuboverlays (n bytes)
            {
                SubOverlay obj = new SubOverlay(Buffer, cursor);
                cursor += obj.ByteLength;

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    obj.ColorTranslation = ColorTransformation.FILTERWHITE90;

                subOverlays.Add(obj);                
            }

            if ((AnimationType)Buffer[cursor] == AnimationType.TRANSLATION)   // check if there is a translation or effect for roomobject      
            {
                motionFirstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                motionColorTranslation = Buffer[cursor];                    // ColorTranslation (1 byte)
                cursor++;
            }
            else if ((AnimationType)Buffer[cursor] == AnimationType.EFFECT)
            {
                motionFirstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                motionEffect = Buffer[cursor];                              // Effect (1 byte)
                cursor++;
            }

            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                motionColorTranslation = ColorTransformation.FILTERWHITE90; 

            motionAnimation = Animation.ExtractAnimation(Buffer, cursor);   // Animation (n bytes)
            cursor += motionAnimation.ByteLength;

            subOverlaysCount = Buffer[cursor];                                  // RoomObjectSubOverlaysCount (1 byte)
            cursor++;

            motionSubOverlays.Clear();

            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay obj = new SubOverlay(Buffer, cursor);
                cursor += obj.ByteLength;

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    obj.ColorTranslation = ColorTransformation.FILTERWHITE90;

                motionSubOverlays.Add(obj);                 
            }

            return cursor - StartIndex;   
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);                                             // ID (4/8 bytes)

            Array.Copy(BitConverter.GetBytes(overlayFileRID), 0, Buffer, cursor, TypeSizes.INT); // MainOverlayID (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(nameRID), 0, Buffer, cursor, TypeSizes.INT);      // StringID (4 bytes)
            cursor += TypeSizes.INT;

            cursor += flags.WriteTo(Buffer, cursor);                                            // Flags (n bytes)
            
            Array.Copy(BitConverter.GetBytes(lightFlags), 0, Buffer, cursor, TypeSizes.SHORT);  // LightFlags (2 bytes)
            cursor += TypeSizes.SHORT;

            if (lightFlags > 0)
            {
                Buffer[cursor] = lightIntensity;                                                    // LightIntensity (1 byte)
                cursor++;

                Array.Copy(BitConverter.GetBytes(lightColor), 0, Buffer, cursor, TypeSizes.SHORT);  // LightColor (2 bytes)
                cursor += TypeSizes.SHORT;
            }

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

            Buffer[cursor] = Convert.ToByte(subOverlays.Count);
            cursor++;

            foreach (SubOverlay overlay in subOverlays)
                cursor += overlay.WriteTo(Buffer, cursor);

            if (motionFirstAnimationType == AnimationType.TRANSLATION)
            {
                Buffer[cursor] = (byte)motionFirstAnimationType;
                cursor++;

                Buffer[cursor] = motionColorTranslation;
                cursor++;
            }
            else if (motionFirstAnimationType == AnimationType.EFFECT)
            {
                Buffer[cursor] = (byte)motionFirstAnimationType;
                cursor++;

                Buffer[cursor] = motionEffect;
                cursor++;
            }

            cursor += motionAnimation.WriteTo(Buffer, cursor);

            Buffer[cursor] = Convert.ToByte(motionSubOverlays.Count);
            cursor++;

            foreach (SubOverlay overlay in motionSubOverlays)
                cursor += overlay.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            overlayFileRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            nameRID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            flags.ReadFrom(ref Buffer);
            
            lightFlags = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            if (lightFlags > 0)
            {
                lightIntensity = Buffer[0];
                Buffer++;

                lightColor = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;
            }

            if ((AnimationType)Buffer[0] == AnimationType.TRANSLATION)
            {
                firstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                colorTranslation = Buffer[0];
                Buffer++;

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    colorTranslation = ColorTransformation.FILTERWHITE90;
            }
            else if ((AnimationType)Buffer[0] == AnimationType.EFFECT)
            {
                firstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                effect = Buffer[0];
                Buffer++;
            }

            animation = Animation.ExtractAnimation(ref Buffer);

            byte subOverlaysCount = Buffer[0];
            Buffer++;

            subOverlays.Clear();
            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay subOv = new SubOverlay(ref Buffer);

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    subOv.ColorTranslation = ColorTransformation.FILTERWHITE90;

                subOverlays.Add(subOv);
            }

            if ((AnimationType)Buffer[0] == AnimationType.TRANSLATION)
            {
                motionFirstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                motionColorTranslation = Buffer[0];
                Buffer++;

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    motionColorTranslation = ColorTransformation.FILTERWHITE90;
            }
            else if ((AnimationType)Buffer[0] == AnimationType.EFFECT)
            {
                motionFirstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                motionEffect = Buffer[0];
                Buffer++;
            }

            motionAnimation = Animation.ExtractAnimation(ref Buffer);

            subOverlaysCount = Buffer[0];
            Buffer++;

            motionSubOverlays.Clear();
            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay subOv = new SubOverlay(ref Buffer);

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    subOv.ColorTranslation = ColorTransformation.FILTERWHITE90;

                motionSubOverlays.Add(subOv);
            }
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((uint*)Buffer) = overlayFileRID;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = nameRID;
            Buffer += TypeSizes.INT;

            flags.WriteTo(ref Buffer);
                     
            *((ushort*)Buffer) = lightFlags;
            Buffer += TypeSizes.SHORT;

            if (lightFlags > 0)
            {
                Buffer[0] = lightIntensity;
                Buffer++;

                *((ushort*)Buffer) = lightColor;
                Buffer += TypeSizes.SHORT;
            }
            
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

            Buffer[0] = (byte)subOverlays.Count;
            Buffer++;

            foreach (SubOverlay overlay in subOverlays)
                overlay.WriteTo(ref Buffer);

            if (motionFirstAnimationType == AnimationType.TRANSLATION)
            {
                Buffer[0] = (byte)motionFirstAnimationType;
                Buffer++;

                Buffer[0] = motionColorTranslation;
                Buffer++;
            }
            else if (motionFirstAnimationType == AnimationType.EFFECT)
            {
                Buffer[0] = (byte)motionFirstAnimationType;
                Buffer++;

                Buffer[0] = motionEffect;
                Buffer++;
            }

            motionAnimation.WriteTo(ref Buffer);

            Buffer[0] = (byte)motionSubOverlays.Count;
            Buffer++;

            foreach (SubOverlay overlay in motionSubOverlays)
                overlay.WriteTo(ref Buffer);
        }

        #endregion

        #region Fields
        protected uint overlayFileRID;
        protected uint nameRID;
        protected readonly ObjectFlags flags = new ObjectFlags();
        protected ushort lightFlags;
        protected byte lightIntensity;
        protected ushort lightColor;
        protected AnimationType firstAnimationType;
        protected byte colorTranslation;
        protected byte effect;
        protected Animation animation;
        protected readonly BaseList<SubOverlay> subOverlays = new BaseList<SubOverlay>();
        protected AnimationType motionFirstAnimationType;
        protected byte motionColorTranslation;
        protected byte motionEffect;
        protected Animation motionAnimation;
        protected readonly BaseList<SubOverlay> motionSubOverlays = new BaseList<SubOverlay>();

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
        public ObjectFlags Flags
        {
            get { return flags; }            
        }
        public ushort LightFlags
        {
            get { return lightFlags; }
            set
            {
                if (lightFlags != value)
                {
                    lightFlags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_LIGHTFLAGS));
                }
            }
        }
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
        public BaseList<SubOverlay> SubOverlays
        {
            get { return subOverlays; }          
        }
        public AnimationType MotionFirstAnimationType
        {
            get { return motionFirstAnimationType; }
            set
            {
                if (motionFirstAnimationType != value)
                {
                    motionFirstAnimationType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONFIRSTANIMATIONTYPE));
                }
            }
        }
        public byte MotionColorTranslation
        {
            get { return motionColorTranslation; }
            set
            {
                if (motionColorTranslation != value)
                {
                    motionColorTranslation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONCOLORTRANSLATION));
                }
            }
        }
        public byte MotionEffect
        {
            get { return motionEffect; }
            set
            {
                if (motionEffect != value)
                {
                    motionEffect = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONEFFECT));
                }
            }
        }
        public Animation MotionAnimation
        {
            get
            {
                return motionAnimation;
            }
            set
            {
                if (motionAnimation != value)
                {
                    motionAnimation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONANIMATION));
                }
            }
        }
        public BaseList<SubOverlay> MotionSubOverlays
        {
            get
            {
                return motionSubOverlays;
            }           
        }

        //Extended Properties
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
        public ObjectUpdate() : base()
        {
        }

        public ObjectUpdate(
            uint ID, 
            uint Count = 0,             
            uint OverlayFileRID = 0, 
            uint NameRID = 0, 
            uint Flags = 0,
            ushort LightFlags = 0, 
            byte LightIntensity = 0, 
            ushort LightColor = 0, 
            AnimationType FirstAnimationType = 0, 
            byte ColorTranslation = 0, 
            byte Effect = 0, 
            Animation Animation = null, 
            IEnumerable<SubOverlay> SubOverlays = null,            
            AnimationType MotionFirstAnimationType = 0, 
            byte MotionColorTranslation = 0, 
            byte MotionEffect = 0, 
            Animation MotionAnimation = null, 
            IEnumerable<SubOverlay> MotionSubOverlays = null)
            : base(ID, Count)
        {
            this.overlayFileRID = OverlayFileRID;
            this.nameRID = NameRID;
            this.flags.Value = Flags;
            this.lightFlags = LightFlags;
            this.lightIntensity = LightIntensity;
            this.lightColor = LightColor;
            this.firstAnimationType = FirstAnimationType;
            this.colorTranslation = ColorTranslation;

            // special handling for secondtrans
            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
            {
                colorTranslation = ColorTransformation.FILTERWHITE90;

                foreach (SubOverlay subOv in SubOverlays)
                    subOv.ColorTranslation = ColorTransformation.FILTERWHITE90;
            }

            this.effect = Effect;
            this.animation = Animation;

            if (SubOverlays != null)
                this.subOverlays.AddRange(SubOverlays);

            this.motionFirstAnimationType = MotionFirstAnimationType;
            this.motionColorTranslation = MotionColorTranslation;

            // special handling for secondtrans
            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
            {
                motionColorTranslation = ColorTransformation.FILTERWHITE90;

                foreach (SubOverlay subOv in MotionSubOverlays)
                    subOv.ColorTranslation = ColorTransformation.FILTERWHITE90;
            }

            this.motionEffect = MotionEffect;
            this.motionAnimation = MotionAnimation;

            if (MotionSubOverlays != null)
                this.motionSubOverlays.AddRange(MotionSubOverlays);
        }

        public ObjectUpdate(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        public unsafe ObjectUpdate(ref byte* Buffer)
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
                LightFlags = 0;
                LightIntensity = 0;
                LightColor = 0;
                FirstAnimationType = 0;
                ColorTranslation = 0;
                Effect = 0;
                Animation = new AnimationNone();

                MotionFirstAnimationType = 0;
                MotionColorTranslation = 0;
                MotionEffect = 0;
                MotionAnimation = new AnimationNone();

                Name = String.Empty;
                OverlayFile = String.Empty;
            }
            else
            {
                overlayFileRID = 0;
                nameRID = 0;
                lightFlags = 0;
                lightIntensity = 0;
                lightColor = 0;
                firstAnimationType = 0;
                colorTranslation = 0;
                effect = 0;
                animation = new AnimationNone();
                
                motionFirstAnimationType = 0;
                motionColorTranslation = 0;
                motionEffect = 0;
                motionAnimation = new AnimationNone();
                
                name = String.Empty;
                overlayFile = String.Empty;
            }

            flags.Clear(RaiseChangedEvent);
            subOverlays.Clear();
            motionSubOverlays.Clear();
        }
        #endregion

        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_name;
            string res_mainoverlayname;

			StringResources.TryGetValue(nameRID, out res_name);
			StringResources.TryGetValue(overlayFileRID, out res_mainoverlayname);

            if (RaiseChangedEvent)
            {
                if (res_name != null) Name = res_name;
                else Name = String.Empty;

                if (res_mainoverlayname != null) OverlayFile = res_mainoverlayname;
                else OverlayFile = String.Empty;
            }
            else
            {
                if (res_name != null) name = res_name;
                else name = String.Empty;

                if (res_mainoverlayname != null) overlayFile = res_mainoverlayname;
                else overlayFile = String.Empty;
            }

            foreach (SubOverlay obj in subOverlays)
                obj.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (SubOverlay obj in motionSubOverlays)
                obj.ResolveStrings(StringResources, RaiseChangedEvent);
        }
        #endregion

        #region IResourceResolvable
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (OverlayFile != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    Resource = M59ResourceManager.GetObject(OverlayFile);

                    if (resource != null)
                    {
                        animation.GroupMax = resource.FrameSets.Count;
                        motionAnimation.GroupMax = resource.FrameSets.Count;
                    }
                }
                else
                {
                    resource = M59ResourceManager.GetObject(OverlayFile);

                    if (resource != null)
                    {
                        animation.GroupMax = resource.FrameSets.Count;
                        motionAnimation.GroupMax = resource.FrameSets.Count;
                    }
                }
            }

            foreach (SubOverlay obj in subOverlays)
                obj.ResolveResources(M59ResourceManager, false);

            foreach (SubOverlay obj in motionSubOverlays)
                obj.ResolveResources(M59ResourceManager, false);  
        }
        #endregion

        #region Methods
        public void DecompressResources()
        {
            if (Resource != null)
                Resource.DecompressAll();

            foreach (SubOverlay subOv in subOverlays)
            {
                if (subOv.Resource != null)
                    subOv.Resource.DecompressAll();
            }

            foreach (SubOverlay subOv in motionSubOverlays)
            {
                if (subOv.Resource != null)
                    subOv.Resource.DecompressAll();
            }
        }
        #endregion

    }
}
