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
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Data.Lists;
using Meridian59.Files;
using Meridian59.Files.BGF;
using System.Collections.Generic;
using Meridian59.Drawing2D;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An often used base notation of an object (e.g. inventory)
    /// </summary>
    [Serializable]
    public class ObjectBase : ObjectID, IStringResolvable, IResourceResolvable, ILightOwner, ITickable
    {
        #region Constants
        public const ushort DEFAULTANGLE = 0;

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
        public const string PROPNAME_NAME = "Name"; 
        public const string PROPNAME_OVERLAYFILE = "OverlayFile";
        public const string PROPNAME_RESOURCE = "Resource";
        public const string PROPNAME_APPEARANCEHASH = "AppearanceHash";
        public const string PROPNAME_VIEWERANGLE = "ViewerAngle";
        #endregion
      
        #region IByteSerializable
        public override int ByteLength { 
            get { 
                int len = base.ByteLength + TypeSizes.INT + TypeSizes.INT + flags.ByteLength;

                if (HasLight)
                {
                    len += TypeSizes.SHORT;
                    
                    if (lightFlags > 0)
                        len += TypeSizes.BYTE + TypeSizes.SHORT;
                }
               
                if (firstAnimationType > 0)                
                    len += TypeSizes.BYTE + TypeSizes.BYTE;

                len += animation.ByteLength;

                len += TypeSizes.BYTE;
                foreach (SubOverlay subOverlay in subOverlays)
                    len += subOverlay.ByteLength;

                return len; 
            } 
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, StartIndex);
            
            overlayFileRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            nameRID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            flags.ReadFrom(Buffer, cursor);
            cursor += flags.ByteLength;

            if (HasLight)
            {
                lightFlags = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                if (lightFlags > 0)
                {
                    lightIntensity = Buffer[cursor];
                    cursor++;

                    lightColor = BitConverter.ToUInt16(Buffer, cursor);
                    cursor += TypeSizes.SHORT;
                }
            }

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

            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                colorTranslation = ColorTransformation.FILTERWHITE90;

            animation = Animation.ExtractAnimation(Buffer, cursor);
            animation.PropertyChanged += OnAnimationPropertyChanged;
            cursor += animation.ByteLength;

            byte subOverlaysCount = Buffer[cursor];
            cursor++;

            subOverlays.Clear();          
            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay obj = new SubOverlay(Buffer, cursor);
                cursor += obj.ByteLength;

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    obj.ColorTranslation = ColorTransformation.FILTERWHITE90;

                subOverlays.Add(obj);                
            }

            return cursor - StartIndex; 
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(overlayFileRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(nameRID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            cursor += flags.WriteTo(Buffer, cursor);
            
            if (HasLight)
            {
                Array.Copy(BitConverter.GetBytes(lightFlags), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;

                if (lightFlags > 0)
                {
                    Buffer[cursor] = lightIntensity;
                    cursor++;

                    Array.Copy(BitConverter.GetBytes(lightColor), 0, Buffer, cursor, TypeSizes.SHORT);
                    cursor += TypeSizes.SHORT;
                }
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

            if (HasLight)
            {
                lightFlags = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                if (lightFlags > 0)
                {
                    lightIntensity = Buffer[0];
                    Buffer++;

                    lightColor = *((ushort*)Buffer);
                    Buffer += TypeSizes.SHORT;
                }
            }

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

            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                colorTranslation = ColorTransformation.FILTERWHITE90;

            animation = Animation.ExtractAnimation(ref Buffer);
            animation.PropertyChanged += OnAnimationPropertyChanged;

            byte subOverlaysCount = Buffer[0];
            Buffer++;

            subOverlays.Clear();
            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay subOverlay = new SubOverlay(ref Buffer);

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    subOverlay.ColorTranslation = ColorTransformation.FILTERWHITE90;

                subOverlays.Add(subOverlay);
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

            if (HasLight)
            {
                *((ushort*)Buffer) = lightFlags;
                Buffer += TypeSizes.SHORT;

                if (lightFlags > 0)
                {
                    Buffer[0] = lightIntensity;
                    Buffer++;

                    *((ushort*)Buffer) = lightColor;
                    Buffer += TypeSizes.SHORT;
                }
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
        }
        #endregion

        #region Fields
        protected uint overlayFileRID;
        protected uint nameRID;
        protected ushort lightFlags;
        protected byte lightIntensity;
        protected ushort lightColor;
        protected AnimationType firstAnimationType;
        protected byte colorTranslation;
        protected byte effect;
        protected Animation animation;
        protected string name;
        protected string overlayFile;
        protected BgfFile resource;
        protected uint appearanceHash;
        protected bool appearanceChangeFlag;
        protected ushort viewerAngle;

        protected bool hasLight = true;
        protected readonly ObjectFlags flags = new ObjectFlags();
        protected readonly BaseList<SubOverlay> subOverlays = new BaseList<SubOverlay>();
        protected readonly Murmur3 hash = new Murmur3();
        #endregion

        #region Properties
        /// <summary>
        /// ObjectBase or deriving class instances can support a light or not.
        /// This does not reflect an active or inactive light on the object, but instead the general
        /// capability to carry a light, so it's not changeable and depends on the use case of this class.
        /// Affects the parser.
        /// </summary>
        public bool HasLight 
        { 
            get { return hasLight; }
        }

        /// <summary>
        /// The resource ID of the main overlay/image
        /// </summary>
        public uint OverlayFileRID
        {
            get { return overlayFileRID; }
            set
            {
                if (overlayFileRID != value)
                {
                    overlayFileRID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_OVERLAYFILERID));

                    // mark for possible appearance change
                    appearanceChangeFlag = true;
                }
            }
        }

        /// <summary>
        /// The resource ID of the object's name
        /// </summary>
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

        /// <summary>
        /// Some flags for object behaviour/appearance/...
        /// </summary>
        public ObjectFlags Flags
        {
            get { return flags; }           
        }

        /// <summary>
        /// Flags for a possibly attached lightsource
        /// </summary>
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

        /// <summary>
        /// An value for light intensity if light enabled.
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
        /// A 16-Bit (A1R5G5B5?) color of the light
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

        /// <summary>
        /// The type of the first animation,
        /// should be TRANSLATION or EFFECT.
        /// </summary>
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

        /// <summary>
        /// The color palette nr to use on main overlay,
        /// if FirstAnimationType is set to TRANSLATION
        /// </summary>
        public byte ColorTranslation
        {
            get { return colorTranslation; }
            set
            {
                if (colorTranslation != value)
                {
                    colorTranslation = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COLORTRANSLATION));

                    appearanceChangeFlag = true;
                }
            }
        }
        
        /// <summary>
        /// The effect value to use on main overlay
        /// if FirstAnimationType is set to EFFECT
        /// </summary>
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
        
        /// <summary>
        /// The 'real' Animation for the main overlay,
        /// should be NONE, ONCE or CYCLE.
        /// </summary>
        public Animation Animation
        {
            get { return animation; }
            set
            {
                if (value != null && animation != value)
                {
                    if (animation != null)
                        animation.PropertyChanged -= OnAnimationPropertyChanged;

                    animation = value;
                    animation.PropertyChanged += OnAnimationPropertyChanged;

                    // mark for possible appearance change
                    appearanceChangeFlag = true;

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));
                }
            }
        }

        /// <summary>
        /// The SubOverlays attached to the object (arms, legs, ...)
        /// </summary>
        public BaseList<SubOverlay> SubOverlays
        {
            get { return subOverlays; }
        }

        /// <summary>
        /// The resolved Name for NameRID
        /// </summary>
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

        /// <summary>
        /// The resolved Filename for OverlayFileRID
        /// </summary>
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

        /// <summary>
        /// The resolved resource for main overlay.
        /// </summary>
        public BgfFile Resource
        {
            get { return resource; }
            set
            {
                if (resource != value)
                {
                    resource = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCE));

                    // mark for possible appearance change
                    appearanceChangeFlag = true;
                }
            }
        }
        
        /// <summary>
        /// Index of Frame in resource for current ViewerAngle,
        /// or -1 if no resource or nonavailable Group.
        /// </summary>
        public int ViewerFrameIndex { get; protected set; }

        /// <summary>
        /// The frame of resource for current ViewerAngle,
        /// or null if not available.
        /// </summary>
        public BgfBitmap ViewerFrame { get; protected set; }

        /// <summary>
        /// An angle a viewer has on the object
        /// </summary>
        public ushort ViewerAngle
        {
            get { return viewerAngle; }
            set
            {
                // remove any full period from the value
                // e.g. turn 4098 into 2
                value = (ushort)(value % GeometryConstants.MAXANGLE);

                if (viewerAngle != value)
                {
                    viewerAngle = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VIEWERANGLE));

                    // mark for possible appearance change
                    appearanceChangeFlag = true;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ObjectBase()
            : base() 
        {
            // attach listeners
            subOverlays.ListChanged += OnSubOverlaysListChanged;
            flags.PropertyChanged += OnFlagsPropertyChanged;
        }

        /// <summary>
        /// Constructor by values
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Count"></param>
        /// <param name="OverlayFileRID"></param>
        /// <param name="NameRID"></param>
        /// <param name="Flags"></param>
        /// <param name="LightFlags"></param>
        /// <param name="LightIntensity"></param>
        /// <param name="LightColor"></param>
        /// <param name="FirstAnimationType"></param>
        /// <param name="ColorTranslation"></param>
        /// <param name="Effect"></param>
        /// <param name="Animation"></param>
        /// <param name="SubOverlays"></param>
        /// <param name="HasLight"></param>
        public ObjectBase(
            uint ID,
            uint Count,             
            uint OverlayFileRID,
            uint NameRID , 
            uint Flags,
            ushort LightFlags, 
            byte LightIntensity, 
            ushort LightColor, 
            AnimationType FirstAnimationType, 
            byte ColorTranslation, 
            byte Effect, 
            Animation Animation, 
            IEnumerable<SubOverlay> SubOverlays,            
            bool HasLight = true)
            : base(ID, Count)
        {
            // with light ?
            hasLight = HasLight;

            // basic
            overlayFileRID = OverlayFileRID;
            nameRID = NameRID;
            flags.Value = Flags;

            // light
            lightFlags = LightFlags;
            lightIntensity = LightIntensity;
            lightColor = LightColor;
            
            // first animation: colortranslation or effect
            firstAnimationType = FirstAnimationType;
            colorTranslation = ColorTranslation;
            effect = Effect;

            // special handling for secondtrans
            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
            {
                colorTranslation = ColorTransformation.FILTERWHITE90;

                foreach(SubOverlay subOv in SubOverlays)
                    subOv.ColorTranslation = ColorTransformation.FILTERWHITE90;
            }

            // animation
            animation = Animation;
            animation.PropertyChanged += OnAnimationPropertyChanged; 

            
            // suboverlays
            subOverlays.AddRange(SubOverlays);

            // attach listeners last (no need to refresh in contructor)
            subOverlays.ListChanged += OnSubOverlaysListChanged;
            flags.PropertyChanged += OnFlagsPropertyChanged;          
        }

        /// <summary>
        /// Constructor by parser.
        /// </summary>
        /// <param name="HasLight"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public ObjectBase(bool HasLight, byte[] Buffer, int StartIndex=0)
            : base()
        {
            // attach listeners
            subOverlays.ListChanged += OnSubOverlaysListChanged;
            flags.PropertyChanged += OnFlagsPropertyChanged;

            /* we need to set the HasLight property first, before start parsing,
               so we use the empty parentconstructor and start parsing manually */
            hasLight = HasLight;
            
            // start parsing
            ReadFrom(Buffer, StartIndex);                    
        }

        /// <summary>
        /// Constructor by pointerbased parser
        /// </summary>
        /// <param name="HasLight"></param>
        /// <param name="Buffer"></param>
        public unsafe ObjectBase(bool HasLight, ref byte* Buffer)
            : base()
        {
            // attach suboverlays listener
            subOverlays.ListChanged += OnSubOverlaysListChanged;
            flags.PropertyChanged += OnFlagsPropertyChanged;

            /* we need to set the HasLight property first, before start parsing,
               so we use the empty parentconstructor and start parsing manually */
            hasLight = HasLight;
            
            // start parsing
            ReadFrom(ref Buffer);
        }
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

                if (animation != null)
                    animation.PropertyChanged -= OnAnimationPropertyChanged;

                Animation = new AnimationNone();
                animation.PropertyChanged += OnAnimationPropertyChanged;

                Name = String.Empty;
                OverlayFile = String.Empty;
                Resource = null;
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

                if (animation != null)
                    animation.PropertyChanged -= OnAnimationPropertyChanged;

                animation = new AnimationNone();
                animation.PropertyChanged += OnAnimationPropertyChanged;

                name = String.Empty;
                overlayFile = String.Empty;
                resource = null;
            }

            flags.Clear(RaiseChangedEvent);                              
            subOverlays.Clear();
        }
        #endregion

        #region IUpdatable
        /// <summary>
        /// Updates values of this instance to values taken from parameter instance.
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="RaiseChangedEvent"></param>
        public override void UpdateFromModel(ObjectUpdate Model, bool RaiseChangedEvent)
        {
            base.UpdateFromModel(Model, RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                OverlayFileRID = Model.OverlayFileRID;
                NameRID = Model.NameRID;
                Flags.UpdateFromModel(Model.Flags, RaiseChangedEvent);
                LightFlags = Model.LightFlags;
                LightIntensity = Model.LightIntensity;
                LightColor = Model.LightColor;
                FirstAnimationType = Model.FirstAnimationType;
                ColorTranslation = Model.ColorTranslation;
                Effect = Model.Effect;

                if (Animation != null)
                    Animation.PropertyChanged -= OnAnimationPropertyChanged;

                Animation = Model.Animation;
                Animation.PropertyChanged += OnAnimationPropertyChanged;
                 
                subOverlays.Clear();
                subOverlays.AddRange(Model.SubOverlays);

                Name = Model.Name;
                OverlayFile = Model.OverlayFile;
                Resource = Model.Resource;
            }
            else
            {
                overlayFileRID = Model.OverlayFileRID;
                nameRID = Model.NameRID;
                Flags.UpdateFromModel(Model.Flags, RaiseChangedEvent);
                lightFlags = Model.LightFlags;
                lightIntensity = Model.LightIntensity;
                lightColor = Model.LightColor;
                firstAnimationType = Model.FirstAnimationType;
                colorTranslation = Model.ColorTranslation;
                effect = Model.Effect;

                if (animation != null)
                    animation.PropertyChanged -= OnAnimationPropertyChanged;

                animation = Model.Animation;
                animation.PropertyChanged += OnAnimationPropertyChanged;
                              
                subOverlays.Clear();
                subOverlays.AddRange(Model.SubOverlays);

                name = Model.Name;
                overlayFile = Model.OverlayFile;
                resource = Model.Resource;
            }

            // appearance update
            ProcessAppearance(true);
        }

        /// <summary>
        /// Updates values of this instance to values taken from parameter instance.
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="RaiseChangedEvent"></param>
        public override void UpdateFromModel(ObjectBase Model, bool RaiseChangedEvent)
        {
            base.UpdateFromModel(Model, RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                OverlayFileRID = Model.OverlayFileRID;
                NameRID = Model.NameRID;
                Flags.UpdateFromModel(Model.Flags, RaiseChangedEvent);
                LightFlags = Model.LightFlags;
                LightIntensity = Model.LightIntensity;
                LightColor = Model.LightColor;
                FirstAnimationType = Model.FirstAnimationType;
                ColorTranslation = Model.ColorTranslation;
                Effect = Model.Effect;
                
                if (Animation != null)
                    Animation.PropertyChanged -= OnAnimationPropertyChanged;

                Animation = Model.Animation;
                Animation.PropertyChanged += OnAnimationPropertyChanged;

                subOverlays.Clear();
                subOverlays.AddRange(Model.SubOverlays);
               
                Name = Model.Name;
                OverlayFile = Model.OverlayFile;
                Resource = Model.Resource;
            }
            else
            {
                overlayFileRID = Model.OverlayFileRID;
                nameRID = Model.NameRID;
                Flags.UpdateFromModel(Model.Flags, RaiseChangedEvent);
                lightFlags = Model.LightFlags;
                lightIntensity = Model.LightIntensity;
                lightColor = Model.LightColor;
                firstAnimationType = Model.FirstAnimationType;
                colorTranslation = Model.ColorTranslation;
                effect = Model.Effect;

                if (animation != null)
                    animation.PropertyChanged -= OnAnimationPropertyChanged;

                animation = Model.Animation;
                animation.PropertyChanged += OnAnimationPropertyChanged;

                subOverlays.Clear();
                subOverlays.AddRange(Model.SubOverlays);
                
                name = Model.Name;
                overlayFile = Model.OverlayFile;
                resource = Model.Resource;
            }
            
            // appearance update
            ProcessAppearance(true);
        }
        #endregion

        #region IStringResolvable
		public virtual void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
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
        }
        #endregion

        #region IResourceResolvable
        public virtual void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            if (OverlayFile != String.Empty)
            {
                if (RaiseChangedEvent)
                {
                    Resource = M59ResourceManager.GetObject(OverlayFile);
                }
                else
                {
                    resource = M59ResourceManager.GetObject(OverlayFile);
                }

                if (resource != null)                
                    animation.GroupMax = resource.FrameSets.Count;
            }

            foreach (SubOverlay obj in subOverlays)
                obj.ResolveResources(M59ResourceManager, false);

            // appearance update
            ProcessAppearance(true);
        }
        #endregion

        #region PropertyChanged listener
        /// <summary>
        /// Executed when animation triggered PropertyChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAnimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));

            // mark for redraw
            appearanceChangeFlag = true;
        }

        /// <summary>
        /// Executed when suboverlay triggered PropertyChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSubOverlayPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SUBOVERLAYS));

            // mark for redraw
            appearanceChangeFlag = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSubOverlaysListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    subOverlays.LastAddedItem.PropertyChanged += OnSubOverlayPropertyChanged;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SUBOVERLAYS));
                    break;

                case ListChangedType.ItemDeleted:
                    subOverlays.LastDeletedItem.PropertyChanged -= OnSubOverlayPropertyChanged;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SUBOVERLAYS));
                    break;
            }
        }
       
        /// <summary>
        /// Executed when flags triggered PropertyChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnFlagsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));

            // mark for redraw
            appearanceChangeFlag = true;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Processes the animations of the objects,
        /// call regularly in threadloop.
        /// May raise AppearanceChanged event.
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public virtual void Tick(double Tick, double Span)
        {
            // update animation
            Animation.Tick(Tick, Span);

            // update suboverlay animations
            foreach (SubOverlay subOverlay in SubOverlays)
                subOverlay.Animation.Tick(Tick, Span);

            // possibly update appearancehash and raise event
            ProcessAppearance();
        }

        /// <summary>
        /// Decompresses all not yet decompressed resources.
        /// </summary>
        public virtual void DecompressResources()
        {
            if (Resource != null)
                Resource.DecompressAll();

            foreach (SubOverlay subOv in subOverlays)         
                if (subOv.Resource != null)
                    subOv.Resource.DecompressAll();          
        }

        /// <summary>
        /// Returns SubOverlays.
        /// </summary>
        public virtual BaseList<SubOverlay> CurrentSubOverlays
        {
            get
            {
                return subOverlays;
            }
        }

        /// <summary>
        /// Try to find a SubOverlay with given Hotspot index
        /// </summary>
        /// <param name="Hotspot"></param>
        /// <returns></returns>
        public SubOverlay GetSubOverlayByHotspot(byte Hotspot)
        {
            foreach (SubOverlay subOv in CurrentSubOverlays)
                if (subOv.HotSpot == Hotspot)
                    return subOv;

            return null;
        }

        public bool HasEmblemDuke()
        {
            foreach (SubOverlay subOv in CurrentSubOverlays)
                if (subOv.Name == ResourceStrings.Others.DUKEEMBLEM ||
                    subOv.Name == ResourceStrings.Others.DUKEEMBLEM2)
                    return true;

            return false;
        }

        public bool HasEmblemPrincess()
        {
            foreach (SubOverlay subOv in CurrentSubOverlays)
                if (subOv.Name == ResourceStrings.Others.PRINCESSEMBLEM ||
                    subOv.Name == ResourceStrings.Others.PRINCESSEMBLEM2)
                    return true;

            return false;
        }

        public bool HasEmblemJonas()
        {
            foreach (SubOverlay subOv in CurrentSubOverlays)
                if (subOv.Name == ResourceStrings.Others.JONASEMBLEM ||
                    subOv.Name == ResourceStrings.Others.JONASEMBLEM2)
                    return true;

            return false;
        }
        #endregion

        #region Appearance
        /// <summary>
        /// A unique hash of the appearance of the object
        /// </summary>
        public uint AppearanceHash 
        {
            get { return appearanceHash; }
            protected set
            {
                if (appearanceHash != value)
                {
                    appearanceHash = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_APPEARANCEHASH));
                }
            }
        }

        /// <summary>
        /// Triggered when the appearance has changed
        /// </summary>
        public event EventHandler AppearanceChanged;

        /// <summary>
        /// Call to possibly raise an AppearanceChanged event
        /// if new AppearanceHash.
        /// </summary>
        /// <param name="Force"></param>
        protected virtual void ProcessAppearance(bool Force = false)
        {
            // if the object is flagged 
            // to have possibly changed appearance
            if (appearanceChangeFlag || Force)
            {
                // update used frame indices
                UpdateFrameIndices();

                // recalculate appearance hash
                uint newhash = GetAppearanceHash();

                // save new hash for appearance
                if (appearanceHash != newhash || Force)
                {
                    // update the suboverlays parents/hotspots
                    foreach (SubOverlay subOv in subOverlays)
                        subOv.UpdateHotspots(this, subOverlays);

                    // trigger event if set, not force overriden
                    if (appearanceHash != newhash)
                    {
                        AppearanceHash = newhash;
                        RaiseAppearanceChanged();
                    }
                }

                // reset appearancechanged status we processed it
                appearanceChangeFlag = false;
            }
        }

        /// <summary>
        /// Updates the ViewerFrameIndex for this instance
        /// and all suboverlays.
        /// </summary>
        protected virtual void UpdateFrameIndices()
        {
            if (resource != null)
            {
                ViewerFrameIndex = resource.GetFrameIndex(Animation.CurrentGroup, viewerAngle);

                if (ViewerFrameIndex > -1 && resource.Frames.Count > ViewerFrameIndex)
                    ViewerFrame = resource.Frames[ViewerFrameIndex];
                else
                    ViewerFrame = null;
            }
            else
            {
                ViewerFrameIndex = -1;
                ViewerFrame = null;
            }

            foreach (SubOverlay subOv in subOverlays)
                subOv.UpdateFrameIndices(viewerAngle);
        }

        public RenderInfo GetRenderInfo(
            Real Quality = 1.0f,
            bool ApplyYOffset = true,
            byte RootHotspotIndex = 0,
            bool ScalePow2 = false, 
            uint Width = 0, 
            uint Height = 0,
            bool CenterVertical = false,
            bool CenterHorizontal = false)
        {
            return new RenderInfo(this, ApplyYOffset, RootHotspotIndex, Quality, ScalePow2, Width, Height, CenterVertical, CenterHorizontal);
        }
       
        /// <summary>
        /// Creates a 32bit hash for the appearance of the object.
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetAppearanceHash()
        {
            hash.Reset();
            
            hash.Step((uint)overlayFileRID);
            //hash.Step((uint)flags.Flags);
            hash.Step((uint)colorTranslation);

            if (resource != null)
                hash.Step((uint)ViewerFrameIndex);

            foreach (SubOverlay obj in subOverlays)
                hash.Step((uint)obj.GetAppearanceHash(false));

            return hash.Finish();
        }

        /// <summary>
        /// Raise AppearanceChanged event if listener set.
        /// </summary>
        protected void RaiseAppearanceChanged()
        {
            if (AppearanceChanged != null)          
                AppearanceChanged(this, new EventArgs());           
        }
        #endregion
    }
}
