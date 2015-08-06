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
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Files;
using Meridian59.Files.BGF;
using Meridian59.Files.ROO;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A projectile like a fireball or an arrow.
    /// </summary>
    [Serializable]
    public class Projectile : IByteSerializable, INotifyPropertyChanged, IClearable, IResourceResolvable, IStringResolvable, ILightOwner, ITickable
    {
        #region Constants

        protected const Real EPSILON2 = 1.0f;
               
        public const string PROPNAME_OVERLAYFILERID = "OverlayFileRID";
        public const string PROPNAME_FLAGS = "Flags";
        public const string PROPNAME_LIGHTFLAGS = "LightFlags";
        public const string PROPNAME_LIGHTINTENSITY = "LightIntensity";
        public const string PROPNAME_LIGHTCOLOR = "LightColor";
        public const string PROPNAME_FIRSTANIMATIONTYPE = "FirstAnimationType";
        public const string PROPNAME_COLORTRANSLATION = "ColorTranslation";
        public const string PROPNAME_EFFECT = "Effect";
        public const string PROPNAME_ANIMATION = "Animation";
        public const string PROPNAME_SOURCE = "Source";
        public const string PROPNAME_TARGET = "Target";
        public const string PROPNAME_SPEED = "Speed";
        public const string PROPNAME_OVERLAYFILE = "OverlayFile";
        public const string PROPNAME_RESOURCE = "Resource";
        public const string PROPNAME_SOURCEOBJECT = "SourceObject";
        public const string PROPNAME_TARGETOBJECT = "TargetObject";
        public const string PROPNAME_ISFINISHED = "IsFinished";
        public const string PROPNAME_ISMOVING = "IsMoving";
        public const string PROPNAME_POSITION3D = "Position3D";
        public const string PROPNAME_APPEARANCEHASH = "AppearanceHash";
        public const string PROPNAME_VIEWERANGLE = "ViewerAngle";
        public const string PROPNAME_ID = "ID";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public int ByteLength { 
            get { 
                int len = TypeSizes.INT;
               
                if (firstAnimationType > 0)                
                    len += TypeSizes.BYTE + TypeSizes.BYTE;

                len += animation.ByteLength;

                len += source.ByteLength;
                len += target.ByteLength;

                len += TypeSizes.BYTE;

                len += TypeSizes.SHORT + TypeSizes.SHORT;

                if (lightFlags > 0)
                    len += TypeSizes.BYTE + TypeSizes.SHORT;

                return len; 
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
           
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
            animation.PropertyChanged += animation_PropertyChanged;
            cursor += animation.ByteLength;

            source = new ObjectID(Buffer, cursor);
            cursor += source.ByteLength;

            target = new ObjectID(Buffer, cursor);
            cursor += target.ByteLength;

            speed = Buffer[cursor];
            cursor++;

            flags = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            lightFlags = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            if (lightFlags > 0)
            {
                lightIntensity = Buffer[cursor];
                cursor++;

                lightColor = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;
            }

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            Array.Copy(BitConverter.GetBytes(overlayFileRID), 0, Buffer, cursor, TypeSizes.INT);
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

            cursor += source.WriteTo(Buffer, cursor);
            cursor += target.WriteTo(Buffer, cursor);

            Buffer[cursor] = speed;
            cursor++;

            Array.Copy(BitConverter.GetBytes(flags), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(lightFlags), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            if (lightFlags > 0)
            {
                Buffer[cursor] = lightIntensity;
                cursor++;

                Array.Copy(BitConverter.GetBytes(lightColor), 0, Buffer, cursor, TypeSizes.SHORT);
                cursor += TypeSizes.SHORT;
            }

            return cursor - StartIndex;
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
        protected static uint nextID = 0;   // internal ID iterator

        protected uint overlayFileRID;
        protected AnimationType firstAnimationType;
        protected byte colorTranslation;
        protected byte effect;
        protected Animation animation;
        protected ObjectID source;
        protected ObjectID target;
        protected byte speed;
        protected ushort flags;
        protected ushort lightFlags;
        protected byte lightIntensity;
        protected ushort lightColor;

        protected V3 position3D;
        protected bool isMoving;
        protected ushort viewerAngle;

        protected bool isFinished;
        protected string overlayFile;
        protected BgfFile resource;
        protected bool appearanceChanged;
        protected uint appearanceHash;
        protected RoomObject sourceObject;
        protected RoomObject targetObject;
        protected Murmur3 hash = new Murmur3();
        protected object userdata;
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
                animation = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));
            }
        }
        public ObjectID Source
        {
            get { return source; }
            set
            {
                if (source != value)
                {
                    source = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SOURCE));
                }
            }
        }
        public ObjectID Target
        {
            get { return target; }
            set
            {
                if (target != value)
                {
                    target = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TARGET));
                }
            }
        }
        public byte Speed
        {
            get { return speed; }
            set
            {
                if (speed != value)
                {
                    speed = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPEED));
                }
            }
        }
        public ushort Flags
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
        
        //Extended

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

        public RoomObject SourceObject
        {
            get { return sourceObject; }
            set
            {
                if (sourceObject != value)
                {
                    sourceObject = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SOURCEOBJECT));
                }
            }
        }

        public RoomObject TargetObject
        {
            get { return targetObject; }
            set
            {
                if (targetObject != value)
                {
                    targetObject = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TARGETOBJECT));
                }
            }
        }

        public V2 Position2D
        {
            get
            {
                return new V2(Position3D.X, Position3D.Z);
            }
        }

        /// <summary>
        /// An angle a viewer has on the projectile
        /// </summary>
        public ushort ViewerAngle
        {
            get { return viewerAngle; }
            protected set
            {
                if (viewerAngle != value)
                {
                    viewerAngle = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VIEWERANGLE));
                }
            }
        }

        /// <summary>
        /// Index of Frame in resource for ViewerAngle
        /// or -1 if no resource or nonavailable Group.
        /// </summary>
        public int ViewerFrameIndex { get; protected set; }

        /// <summary>
        /// The frame of resource for current ViewerAngle
        /// or null if not available.
        /// </summary>
        public BgfBitmap ViewerFrame { get; protected set; }

        public uint ID { get; protected set; }

        /// <summary>
        /// User attached undefined data
        /// </summary>
        public object UserData
        {
            get { return userdata; }
            set
            {
                if (userdata != value)
                {
                    userdata = value;
                    //OnPropertyChanged(new PropertyChangedEventArgs("UserData"));
                }
            }
        }
        #endregion

        #region Constructors
        public Projectile()
        {
            ID = nextID;
            nextID++;

            Clear(false);
        }

        public Projectile(
            uint ResourceID,
            AnimationType FirstObjectAnimationType, 
            byte ColorTranslation, 
            byte Effect, 
            Animation Animation, 
            ObjectID Source, 
            ObjectID Targe,
            byte Speed,
            ushort Flags,
            ushort LightFlags, 
            byte LightIntensity, 
            ushort LightColor)
        {
            ID = nextID;
            nextID++;

            this.overlayFileRID = ResourceID;
            
            this.firstAnimationType = FirstObjectAnimationType;
            this.colorTranslation = ColorTranslation;
            this.effect = Effect;

            this.animation = Animation;

            this.source = Source;
            this.target = Target;

            this.flags = Flags;
           
            this.lightFlags = LightFlags;
            this.lightIntensity = LightIntensity;
            this.lightColor = LightColor;                    
        }

        public Projectile(byte[] Buffer, int StartIndex=0)
        {
            ID = nextID;
            nextID++;

            ReadFrom(Buffer, StartIndex);                    
        }

        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                OverlayFileRID = 0;
                FirstAnimationType = 0;
                ColorTranslation = 0;
                Effect = 0;
                Animation = new AnimationNone();
                Source = new ObjectID(0);
                Target = new ObjectID(0);
                Speed = 0;
                Flags = 0;
                LightFlags = 0;
                LightIntensity = 0;
                LightColor = 0;

                OverlayFile = String.Empty;
            }
            else
            {
                overlayFileRID = 0;
                firstAnimationType = 0;
                colorTranslation = 0;
                effect = 0;
                animation = new AnimationNone();
                source = new ObjectID(0);
                target = new ObjectID(0);
                speed = 0;
                flags = 0;
                lightFlags = 0;
                lightIntensity = 0;
                lightColor = 0;

                overlayFile = String.Empty;
            }
        }
        #endregion

        #region IStringResolvable
		public void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_mainoverlayname;

			StringResources.TryGetValue(overlayFileRID, out res_mainoverlayname);

            if (RaiseChangedEvent)
            {           
                if (res_mainoverlayname != null) OverlayFile = res_mainoverlayname;
                else OverlayFile = String.Empty;
            }
            else
            {            
                if (res_mainoverlayname != null) overlayFile = res_mainoverlayname;
                else overlayFile = String.Empty;
            }
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
                        animation.GroupMax = resource.FrameSets.Count;
                }
                else
                {
                    resource = M59ResourceManager.GetObject(OverlayFile);

                    if (resource != null)
                        animation.GroupMax = resource.FrameSets.Count;
                }
            }
        }
        #endregion

        #region IMovable
        public V3 Position3D
        {
            get { return position3D; }
            set
            {
                if (!position3D.Equals(value))
                {
                    position3D = value;
                    appearanceChanged = true;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POSITION3D));
                }
            }
        }
        
        public bool IsMoving
        {
            get { return isMoving; }
            set
            {
                if (isMoving != value)
                {
                    isMoving = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISMOVING));
                    appearanceChanged = true;
                }
            }
        }

        public void StartMove()
        {
            if (targetObject == null || sourceObject == null)
                return;

            IsMoving = true;
        }

        public void UpdatePosition(double TickSpan, RooFile RooFile)
        {
            if (!isMoving)
                return;

            // source->destination delta vector and squared distance (in 3D)
            V3   dvector   = targetObject.Position3D - position3D;
            Real distance2 = dvector.LengthSquared;

            // end not yet reached?
            if (distance2 > EPSILON2)
            {
                if (Speed != (byte)MovementSpeed.Teleport)
                {
                    // normalise to get plain direction
                    dvector.Normalize();

                    // process another step based on time delta
                    V3 step = dvector * (Real)speed * (Real)TickSpan * GeometryConstants.PROJECTILEMOVEBASECOEFF;

                    // check if this step is greater than distance left
                    if (step.LengthSquared > distance2)
                    {
                        // directly update for teleport-speed zero
                        position3D.X = TargetObject.Position3D.X;
                        position3D.Y = TargetObject.Position3D.Y;
                        position3D.Z = TargetObject.Position3D.Z;

                        // movement finished
                        IsMoving = false;
                    }
                    else
                    {
                        // do the step
                        position3D.X += step.X;
                        position3D.Y += step.Y;
                        position3D.Z += step.Z;
                    }
                }
                else
                {
                    // directly update for teleport-speed zero
                    position3D.X = TargetObject.Position3D.X;
                    position3D.Y = TargetObject.Position3D.Y;
                    position3D.Z = TargetObject.Position3D.Z;
                }

                // trigger changed event
                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POSITION3D));
            }
            else               
                IsMoving = false;              
            
        }
        #endregion

        #region ITickable
        /// <summary>
        /// Processes the animations of the projectile,
        /// call regularly in threadloop.
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(double Tick, double Span)
        {
            // update animation
            Animation.Tick(Tick, Span);

            // process possibly new appearance
            ProcessAppearance();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Recalculates the ViewerAngle property based on a viewer's position.
        /// </summary>
        /// <param name="ViewerPosition"></param>
        public void UpdateViewerAngle(V2 ViewerPosition)
        {
            V2 direction = TargetObject.Position2D - Position2D;
            direction.Normalize();

            ushort angle = MathUtil.GetAngleForDirection(direction);

            // update viewer angle
            ViewerAngle = MathUtil.GetAngle(ViewerPosition, Position2D, angle);

            // mark for possible appearance change
            appearanceChanged = true;
        }

        /// <summary>
        /// Tries to resolve the source and target RoomObject
        /// references from RoomObjects parameter, also sets the start position.
        /// </summary>
        /// <param name="RoomObjects"></param>
        /// <param name="RaiseChangedEvent"></param>
        public void ResolveSourceTarget(IList<RoomObject> RoomObjects, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                foreach (RoomObject roomObject in RoomObjects)
                {
                    if (roomObject.ID == source.ID)
                    {
                        SourceObject = roomObject;
                        Position3D = SourceObject.Position3D.Clone();
                    }

                    else if (roomObject.ID == target.ID)
                        TargetObject = roomObject;                   
                }
            }
            else
            {
                foreach (RoomObject roomObject in RoomObjects)
                {
                    if (roomObject.ID == source.ID)
                    {
                        sourceObject = roomObject;
                        position3D = sourceObject.Position3D.Clone();
                    }
                    else if (roomObject.ID == target.ID)
                        targetObject = roomObject;
                }
            }
        }

        /// <summary>
        /// Executed when animation changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void animation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));

            // mark for redraw
            appearanceChanged = true;
        }

        /// <summary>
        /// Decompresses all not yet decompressed resources.
        /// </summary>
        public void DecompressResources()
        {
            if (Resource != null)
                Resource.DecompressAll();
        }
        #endregion

        #region Appearance
        /// <summary>
        /// A unique hash of the appearance of the projectile
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
        protected void ProcessAppearance()
        {
            // if the object is flagged 
            // to have possibly changed appearance
            if (appearanceChanged)
            {
                // update used frame indices
                UpdateFrameIndices();

                // recalculate appearance hash
                uint newhash = GetAppearanceHash();

                // save new hash for appearance
                if (appearanceHash != newhash)
                {
                    AppearanceHash = newhash;

                    // check if event listener exists and hash is different
                    if (AppearanceChanged != null)
                    {
                        // trigger event for a real appearancechange
                        AppearanceChanged(this, new EventArgs());
                    }
                }

                // reset appearancechanged status we processed it
                appearanceChanged = false;
            }
        }

        /// <summary>
        /// Updates the ViewerFrameIndex for this instance.
        /// </summary>
        public virtual void UpdateFrameIndices()
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
        }

        /// <summary>
        /// Creates a 32bit hash for the appearance of the projectile.
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetAppearanceHash()
        {
            hash.Reset();

            hash.Step((uint)overlayFileRID);
            hash.Step((uint)flags);
            hash.Step((uint)colorTranslation);

            if (resource != null)
                hash.Step((uint)ViewerFrameIndex);

            return hash.Finish();
        }
        #endregion
    }
}
