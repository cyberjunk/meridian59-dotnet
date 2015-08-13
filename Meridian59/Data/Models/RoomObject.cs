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
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;
using Meridian59.Files.ROO;
using Meridian59.Files.BGF;
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
    /// A server created element in the room.
    /// </summary>
    [Serializable]
    public class RoomObject : ObjectBase
    {
        #region Constants

        protected const uint HEALTHSTATUSRESETDELAY = 5000;

        public const string PROPNAME_COORDINATEY = "CoordinateY";
        public const string PROPNAME_COORDINATEX = "CoordinateX";
        public const string PROPNAME_POSITION3D = "Position3D";
        public const string PROPNAME_ANGLE = "Angle";
        public const string PROPNAME_ANGLEUNITS = "AngleUnits";
        public const string PROPNAME_MOTIONFIRSTANIMATIONTYPE = "MotionFirstAnimationType";
        public const string PROPNAME_MOTIONCOLORTRANSLATION = "MotionColorTranslation";
        public const string PROPNAME_MOTIONEFFECT = "MotionEffect";
        public const string PROPNAME_MOTIONANIMATION = "MotionAnimation";
        public const string PROPNAME_MOTIONSUBOVERLAYS = "MotionSubOverlays";
        public const string PROPNAME_HEALTHSTATUS = "HealthStatus";
        public const string PROPNAME_HEALTHSTATUSLASTUPDATE = "HealthStatusLastUpdate";
        public const string PROPNAME_ISMOVING = "IsMoving";       
        public const string PROPNAME_HORIZONTALSPEED = "HorizontalSpeed";
        public const string PROPNAME_VERTICALSPEED = "VerticalSpeed";
        public const string PROPNAME_ISTARGET = "IsTarget";
        public const string PROPNAME_ISHIGHLIGHTED = "IsHighlighted";
        public const string PROPNAME_VIEWERAPPEARANCEHASH = "ViewerAppearanceHash";
        #endregion

        #region IByteSerializable
        public override int ByteLength { 
            get {
                int len = base.ByteLength + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT;

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

            cursor += base.ReadFrom(Buffer, StartIndex);         
           
            ushort coordinateY = BitConverter.ToUInt16(Buffer, cursor);          
            cursor += TypeSizes.SHORT;

            ushort coordinateX = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            position3D = new V3(coordinateX, 0.0f, coordinateY);

            angle = MathUtil.BinaryAngleToRadian(BitConverter.ToUInt16(Buffer, cursor));
            cursor += TypeSizes.SHORT;

            if ((AnimationType)Buffer[cursor] == AnimationType.TRANSLATION)                   // check if there is a colortranslation or effect as 1. anim     
            {
                motionFirstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                motionColorTranslation = Buffer[cursor];                    // RoomObjectColorTranslation (1 byte)
                cursor++;
            }
            else if (((AnimationType)Buffer[cursor] == AnimationType.EFFECT))
            {
                motionFirstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                motionEffect = Buffer[cursor];                              // RoomObjectEffect (1 byte)
                cursor++;
            }

            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                motionColorTranslation = ColorTransformation.FILTERWHITE90;

            motionAnimation = Animation.ExtractAnimation(Buffer, cursor);   // Animation (n bytes)
            cursor += motionAnimation.ByteLength;

            motionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;

            byte subOverlaysCount = Buffer[cursor];                             // RoomObjectSubOverlaysCount (1 byte)
            cursor++;

            motionSubOverlays.Clear();
            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay obj = new SubOverlay(Buffer, cursor);
                cursor += obj.ByteLength;
                
                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    obj.ColorTranslation = ColorTransformation.FILTERWHITE90;

                obj.PropertyChanged += OnMotionSubOverlayPropertyChanged;
                
                motionSubOverlays.Add(obj);
            }

            return cursor - StartIndex;   
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            cursor += base.WriteTo(Buffer, StartIndex);

            Array.Copy(BitConverter.GetBytes(CoordinateY), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(CoordinateX), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(AngleUnits), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

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

            ushort coordinateY = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            ushort coordinateX = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            position3D = new V3(coordinateX, 0.0f, coordinateY);

            angle = MathUtil.BinaryAngleToRadian(*((ushort*)Buffer));
            Buffer += TypeSizes.SHORT;

            if ((AnimationType)Buffer[0] == AnimationType.TRANSLATION)
            {
                motionFirstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                motionColorTranslation = Buffer[0];
                Buffer++;
            }
            else if (((AnimationType)Buffer[0] == AnimationType.EFFECT))
            {
                motionFirstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                motionEffect = Buffer[0];
                Buffer++;
            }

            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                motionColorTranslation = ColorTransformation.FILTERWHITE90;

            motionAnimation = Animation.ExtractAnimation(ref Buffer);
            motionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;

            byte subOverlaysCount = Buffer[0];
            Buffer++;

            motionSubOverlays.Clear();
            for (byte i = 0; i < subOverlaysCount; i++)
            {
                SubOverlay subov = new SubOverlay(ref Buffer);

                if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
                    subov.ColorTranslation = ColorTransformation.FILTERWHITE90;

                subov.PropertyChanged += OnMotionSubOverlayPropertyChanged;
                motionSubOverlays.Add(subov);
            }
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((ushort*)Buffer) = CoordinateY;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = CoordinateX;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = AngleUnits;
            Buffer += TypeSizes.SHORT;

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
        protected V3 position3D;
        protected Real angle;
        protected AnimationType motionFirstAnimationType;
        protected byte motionColorTranslation;
        protected byte motionEffect;
        protected Animation motionAnimation;
        protected HealthStatus healthStatus;
        protected DateTime healthStatusLastUpdate;
        protected bool isMoving;
        protected Real horizontalSpeed;
        protected bool isTarget;
        protected bool isHighlighted;
        protected RooSubSector subSector;
        protected Real verticalSpeed;
        protected uint viewerAppearanceHash;
        protected object userdata;
        
        protected readonly BaseList<SubOverlay> motionSubOverlays = new BaseList<SubOverlay>();
        #endregion

        #region Properties
        /// <summary>
        /// 2D (UInt16) of the Z component of Position3D
        /// </summary>
        public ushort CoordinateY
        {
            get 
            { 
                ushort val = 0;

                // use only positive values
                if (position3D.Z > 0.0f)
                    val = Convert.ToUInt16(position3D.Z);

                return val;
            }
        }
       
        /// <summary>
        /// 2D (UInt16) of X component of Position3D
        /// </summary>
        public ushort CoordinateX
        {
            get 
            {
                ushort val = 0;

                // use only positive values
                if (position3D.X > 0.0f)
                    val = Convert.ToUInt16(position3D.X);

                return val;
            }
        }
        
        /// <summary>
        /// The orientation of the RoomObject in radian.
        /// This is the clockwise measured angle between world x-Axis and object's x-Axis,
        /// assuming the object's y-Axis (height) is always parallel to world's y-Axis.
        /// Stays within bounds 0 to 2PI.
        /// </summary>
        public Real Angle
        {
            get { return angle; }
            set
            {
                // remove any full period from the value
                value = (Real)(value % GeometryConstants.TWOPI);

                // map - to +
                if (value < 0)
                    value = GeometryConstants.TWOPI + value;

                // check if changed
                if (angle != value)
                {
                    // update
                    angle = value;

                    // trigger event
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANGLE));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANGLEUNITS));

                    // orientation changes may result in other frames to use
                    appearanceChangeFlag = true;
                }
            }
        }

        /// <summary>
        /// The orientation of the RoomObject in M59 angle units.
        /// This is converted from Angle property.
        /// Always stays within bounds 0 to 4096.
        /// </summary>
        public ushort AngleUnits
        {
            get { return MathUtil.RadianToBinaryAngle(angle); }
            set
            {
                // remove any full period from the value
                // e.g. turn 4098 into 2
                value = (ushort)(value % GeometryConstants.MAXANGLE);

                // convert to radian
                Real radian = MathUtil.BinaryAngleToRadian(value);

                if (angle != radian)
                {
                    // update radian angle
                    angle = radian;

                    // trigger event
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANGLE));
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANGLEUNITS));

                    // orientation changes may result in other frames to use
                    appearanceChangeFlag = true;
                }
            }
        }

        /// <summary>
        /// The type of the MotionAnimation
        /// </summary>
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
        
        /// <summary>
        /// The color palette for mainoverlay in case of motions
        /// </summary>
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
        
        /// <summary>
        /// The effect for mainoverlay in case of motions
        /// </summary>
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

        /// <summary>
        /// The animation to use in case object is moving
        /// </summary>
        public Animation MotionAnimation
        {
            get
            {
                return motionAnimation;
            }
            set
            {
                if (value != null && motionAnimation != value)
                {
                    if (motionAnimation != null)
                        motionAnimation.PropertyChanged -= OnMotionAnimationPropertyChanged;

                    motionAnimation = value;
                    motionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONANIMATION));
                }
            }
        }
        
        /// <summary>
        /// The SubOverlays to use in case object is moving
        /// </summary>
        public BaseList<SubOverlay> MotionSubOverlays
        {
            get { return motionSubOverlays; }          
        }

        /// <summary>
        /// Currently estimated health-status of the roomobject,
        /// either unknown or based on the last received ouch sound.
        /// </summary>
        public HealthStatus HealthStatus
        {
            get { return healthStatus; }
            set
            {
                if (healthStatus != value)
                {
                    healthStatus = value;
                    healthStatusLastUpdate = DateTime.Now;

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEALTHSTATUS));
                }
            }
        }

        /// <summary>
        /// Internal time we last updated the health status.
        /// </summary>
        public DateTime HealthStatusLastUpdate
        {
            get { return healthStatusLastUpdate; }
            set
            {
                if (healthStatusLastUpdate != value)
                {
                    healthStatusLastUpdate = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEALTHSTATUSLASTUPDATE));
                }
            }
        }
        
        /// <summary>
        /// Whether this is the current target or not
        /// </summary>
        public bool IsTarget
        {
            get { return isTarget; }
            set
            {
                if (isTarget != value)
                {
                    isTarget = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISTARGET));
                }
            }
        }

        /// <summary>
        /// Whether this is object is highlighted or not (e.g. mouseover)
        /// </summary>
        public bool IsHighlighted
        {
            get { return isHighlighted; }
            set
            {
                if (isHighlighted != value)
                {
                    isHighlighted = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISHIGHLIGHTED));
                }
            }
        }

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

        /// <summary>
        /// The 2D position of the object on groundplane (x=x, y=z)
        /// </summary>
        public V2 Position2D
        {
            get
            {
                return new V2(Position3D.X, Position3D.Z);
            }
        }

        /// <summary>
        /// Whether this is our avatar or not
        /// </summary>
        public bool IsAvatar { get; set; }

        /// <summary>
        /// Index of Frame in resource for angle = 0,
        /// or -1 if no resource or nonavailable Group.
        /// </summary>
        public int FrontFrameIndex { get; protected set; }

        /// <summary>
        /// The frame of resource for a frontal view on the object
        /// or null if not available.
        /// </summary>
        public BgfBitmap FrontFrame { get; protected set; }

        /// <summary>
        /// Returns either SubOverlays or MotionSubOverlys property,
        /// depending on IsMoving state.
        /// </summary>
        public override BaseList<SubOverlay> CurrentSubOverlays
        {
            get
            {
                if (isMoving) return motionSubOverlays;
                else return subOverlays;
            }
        }

        /// <summary>
        /// The Roo SubSector this object is contained in or NULL
        /// </summary>
        public RooSubSector SubSector 
        {
            get { return subSector; }
            protected set { subSector = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public RoomObject() 
            : base() 
        {
            // attach motionsuboverlays listener
            motionSubOverlays.ListChanged += OnMotionSubOverlaysListChanged;
        }

        public RoomObject(
            uint ID,
            uint Count,           
            uint OverlayFileRID,
            uint NameRID, 
            uint Flags,
            ushort LightFlags, 
            byte LightIntensity, 
            ushort LightColor, 
            AnimationType FirstAnimationType, 
            byte ColorTranslation, 
            byte Effect, 
            Animation Animation, 
            BaseList<SubOverlay> SubOverlays,            
            V3 Position3D,
            ushort Angle, 
            AnimationType MotionFirstAnimationType, 
            byte MotionColorTranslation, 
            byte MotionEffect, 
            Animation MotionAnimation, 
            IEnumerable<SubOverlay> MotionSubOverlays)            
            : base(
                ID, Count, 
                OverlayFileRID, NameRID, Flags, 
                LightFlags, LightIntensity, LightColor, 
                FirstAnimationType, ColorTranslation, Effect, Animation, SubOverlays)
        {
            // attach motionsuboverlays listener
            motionSubOverlays.ListChanged += OnMotionSubOverlaysListChanged;

            // coordinates & angle
            position3D = Position3D;
            angle = Angle;

            // roomobject stuff (like object)
            motionFirstAnimationType = MotionFirstAnimationType;
            motionColorTranslation = MotionColorTranslation;
            motionEffect = MotionEffect;
            motionAnimation = MotionAnimation;
            motionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;

            // special handling for secondtrans
            if (flags.Drawing == ObjectFlags.DrawingType.SecondTrans)
            {
                motionColorTranslation = ColorTransformation.FILTERWHITE90;

                foreach (SubOverlay subOv in MotionSubOverlays)
                    subOv.ColorTranslation = ColorTransformation.FILTERWHITE90;
            }

            // motionsuboverlays
            motionSubOverlays.AddRange(MotionSubOverlays);
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public RoomObject(byte[] Buffer, int StartIndex=0)
            : base(true, Buffer, StartIndex) 
        {
            // attach motionsuboverlays listener
            motionSubOverlays.ListChanged += OnMotionSubOverlaysListChanged;
        }

        /// <summary>
        /// Constructor by pointerbased parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe RoomObject(ref byte* Buffer)
            : base(true, ref Buffer) 
        {
            // attach motionsuboverlays listener
            motionSubOverlays.ListChanged += OnMotionSubOverlaysListChanged;
        }

        #endregion

        #region IClearable
        /// <summary>
        /// Resets the object to default state/values
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                Position3D = new V3(0, 0, 0);
                Angle = 0.0f;
                MotionFirstAnimationType = 0;
                MotionColorTranslation = 0;
                MotionEffect = 0;

                if (motionAnimation != null)
                    motionAnimation.PropertyChanged -= OnMotionAnimationPropertyChanged;

                MotionAnimation = new AnimationNone();
                MotionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;

                motionSubOverlays.Clear();
                
                IsMoving = false;
                IsTarget = false;
                IsHighlighted = false;
                VerticalSpeed = 0.0f;
            }
            else
            {
                position3D = new V3(0, 0, 0);
                angle = 0.0f;
                motionFirstAnimationType = 0;
                motionColorTranslation = 0;
                motionEffect = 0;

                if (motionAnimation != null)
                    motionAnimation.PropertyChanged -= OnMotionAnimationPropertyChanged;

                motionAnimation = new AnimationNone();
                motionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;

                motionSubOverlays.Clear();
                
                isMoving = false;
                isTarget = false;
                isHighlighted = false;
                verticalSpeed = 0.0f;
            }
        }
        #endregion

        #region IUpdatable
        public override void UpdateFromModel(ObjectUpdate Model, bool RaiseChangedEvent)
        {
            base.UpdateFromModel(Model, RaiseChangedEvent);

            if (RaiseChangedEvent)
            {      
                MotionFirstAnimationType = Model.MotionFirstAnimationType;
                MotionColorTranslation = Model.MotionColorTranslation;
                MotionEffect = Model.MotionEffect;

                if (MotionAnimation != null)
                    MotionAnimation.PropertyChanged -= OnMotionAnimationPropertyChanged;

                MotionAnimation = Model.MotionAnimation;
                MotionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;
                
                MotionSubOverlays.Clear();
                MotionSubOverlays.AddRange(Model.MotionSubOverlays);
            }
            else
            {
                motionFirstAnimationType = Model.MotionFirstAnimationType;
                motionColorTranslation = Model.MotionColorTranslation;
                motionEffect = Model.MotionEffect;

                if (motionAnimation != null)
                    motionAnimation.PropertyChanged -= OnMotionAnimationPropertyChanged;

                motionAnimation = Model.MotionAnimation;
                motionAnimation.PropertyChanged += OnMotionAnimationPropertyChanged;
                
                foreach (SubOverlay obj in motionSubOverlays)
                    obj.PropertyChanged -= OnMotionSubOverlayPropertyChanged;

                motionSubOverlays.Clear();
                motionSubOverlays.AddRange(Model.MotionSubOverlays);
            }

            // update appearance
            ProcessAppearance(true);
        }
        #endregion

        #region IStringResolvable
		public override void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            base.ResolveStrings(StringResources, RaiseChangedEvent);

            foreach (SubOverlay obj in motionSubOverlays)
                obj.ResolveStrings(StringResources, RaiseChangedEvent);
        }
        #endregion

        #region IResourceResolvable
        public override void ResolveResources(Files.ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            // no base
            // base.ResolveResources(M59ResourceManager, RaiseChangedEvent);

            if (OverlayFile != String.Empty)
            {
                if (RaiseChangedEvent)               
                    Resource = M59ResourceManager.GetObject(OverlayFile);
                
                else             
                    resource = M59ResourceManager.GetObject(OverlayFile);
                
                if (resource != null)
                {
                    animation.GroupMax = resource.FrameSets.Count;
                    motionAnimation.GroupMax = resource.FrameSets.Count;
                }
            }

            foreach (SubOverlay obj in subOverlays)
                obj.ResolveResources(M59ResourceManager, false);

            foreach (SubOverlay obj in motionSubOverlays)
                obj.ResolveResources(M59ResourceManager, false);

            // appearance update
            ProcessAppearance(true);
        }
        #endregion

        #region IMovable

        /// <summary>
        /// Current 3D position of the RoomObject
        /// </summary>
        public V3 Position3D
        {
            get { return position3D; }
            set
            {
                if (!position3D.Equals(value))
                {
                    position3D = value;
                    appearanceChangeFlag = true;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POSITION3D));
                }
            }
        }

        /// <summary>
        /// Whether or not the object is currently moving
        /// </summary>
        public bool IsMoving
        {
            get { return isMoving; }
            set
            {
                if (isMoving != value)
                {
                    isMoving = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISMOVING));
                    appearanceChangeFlag = true;
                }
            }
        }

        /// <summary>
        /// The vertical speed of the object (falling, ...)
        /// </summary>
        public Real VerticalSpeed
        {
            get { return verticalSpeed; }
            set
            {
                if (verticalSpeed != value)
                {
                    verticalSpeed = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VERTICALSPEED));
                }
            }
        }

        /// <summary>
        /// The horizontal speed of the object
        /// </summary>
        public Real HorizontalSpeed
        {
            get { return horizontalSpeed; }
            set
            {
                if (horizontalSpeed != value)
                {
                    horizontalSpeed = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HORIZONTALSPEED));
                }
            }
        }

        /// <summary>
        /// The destination of last/current movement in 2D
        /// </summary>
        public V2 MoveDestination { get; protected set; }

        /// <summary>
        /// The start of the last/current movement in 2D
        /// </summary>
        public V2 MoveStart { get; protected set; }

        /// <summary>
        /// Updates the position (including height) of the RoomObject in a timedelta based step
        /// closer to the Destination.
        /// </summary>
        /// <param name="TickSpan">Elapsed ms since last call</param>
        /// <param name="RoomInfo">Server sent room information, also has loaded reference to ROO</param>
        public void UpdatePosition(double TickSpan, RoomInfo RoomInfo)
        {
            if (RoomInfo == null || RoomInfo.ResourceRoom == null)
                return;

            if (IsMoving)
            {
                // source, destination, source->destination (on plane, y=0)
                V2 SD = MoveDestination - Position2D;

                // squared distance to destination left
                Real distance2 = SD.LengthSquared;

                // when is a destination considered to be reached
                const Real epsilon2 = 0.000001f;

                // whether we need an update in the end
                bool positionChanged = false;

                // end not yet reached? process another step based on time delta
                bool endReached = (distance2 < epsilon2);
                if (!endReached)
                {
                    positionChanged = true;

                    if (horizontalSpeed != (Real)MovementSpeed.Teleport)
                    {
                        // normalise
                        SD.Normalize();

                        // the step-vector to do this frame
                        V2 step = SD * horizontalSpeed * (Real)TickSpan * GeometryConstants.MOVEBASECOEFF;

                        // check if this step is greater than target distance
                        // if so use distance left in one step.
                        if (step.LengthSquared > distance2)
                        {
                            step.Normalize();
                            step *= (Real)System.Math.Sqrt(distance2);
                        }

                        // apply the step on plane (y=0)
                        position3D.X += step.X;
                        position3D.Z += step.Y;
                    }
                    else
                    {
                        // directly update for teleport (i.e. blink, hold-move setback)
                        position3D.X = MoveDestination.X;
                        position3D.Z = MoveDestination.Y;
                    }
                }

                // get height at destination from roo
                // convert to ROO coordinates
                Real xint = (Position3D.X - 64.0f) * 16.0f;
                Real yint = (Position3D.Z - 64.0f) * 16.0f;

                // get height from ROO and update subsector reference
                Real oldheight = Position3D.Y;

#if VANILLA
                // note: IsHanging overlaps with some playertypes in VANILLA
                Real newheight = (Flags.IsHanging && !Flags.IsPlayer) ?
#else
                Real newheight = (Flags.IsHanging) ?                   
#endif
                    (Real)(RoomInfo.ResourceRoom.GetHeightAt(xint, yint, out subSector, false, false) * 0.0625f) :
                    (Real)(RoomInfo.ResourceRoom.GetHeightAt(xint, yint, out subSector, true, true) * 0.0625f);

                // see if server overrides this depth type completely
                // to another sector height
                if (subSector != null && subSector.Sector != null)
                {
                    RooSector sector = subSector.Sector;

                    if (RoomInfo.Flags.IsOverrideDepth1 && sector.Flags.SectorDepth == RooSectorFlags.DepthType.Depth1)
                        newheight = RoomInfo.Depth1;

                    else if (RoomInfo.Flags.IsOverrideDepth2 && sector.Flags.SectorDepth == RooSectorFlags.DepthType.Depth2)
                        newheight = RoomInfo.Depth2;

                    else if (RoomInfo.Flags.IsOverrideDepth3 && sector.Flags.SectorDepth == RooSectorFlags.DepthType.Depth3)
                        newheight = RoomInfo.Depth3;                
                }

                if (oldheight != newheight)
                    positionChanged = true;

                bool falling = (newheight < oldheight);
                if (!falling)
                {
                    // no falling? step up immediately
                    position3D.Y = newheight;
                    verticalSpeed = 0.0f;
                }
                else
                {
                    // linear fake gravity
                    position3D.Y += ((Real)TickSpan * verticalSpeed);
                    verticalSpeed += ((Real)TickSpan * GeometryConstants.GRAVITYACCELERATION);
                }

                // no more processing of this node necessary
                if (!falling && endReached)
                {
                    IsMoving = false;                    
                }

                // if we moved somehow, trigger changed event
                if (positionChanged)
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POSITION3D));
            }
        }

        /// <summary>
        /// Instantly updates the height from ROO
        /// </summary>
        /// <param name="RoomInfo"></param>
        public void UpdateHeightPosition(RoomInfo RoomInfo)
        {
            if (RoomInfo == null || RoomInfo.ResourceRoom == null)
                return;

            // get height at destination from roo
            // convert to ROO coordinates
            Real xint = (Position3D.X - 64.0f) * 16.0f;
            Real yint = (Position3D.Z - 64.0f) * 16.0f;

            // get height from ROO
            Real oldheight = Position3D.Y;

#if VANILLA
            // note: IsHanging overlaps with some playertypes
            Real newheight = (Flags.IsHanging && !Flags.IsPlayer) ?
#else
            Real newheight = (Flags.IsHanging) ?
#endif
                (Real)(RoomInfo.ResourceRoom.GetHeightAt(xint, yint, out subSector, false, false) * 0.0625f) :
                (Real)(RoomInfo.ResourceRoom.GetHeightAt(xint, yint, out subSector, true, true) * 0.0625f);

            // see if server overrides this depth type completely
            // to another sector height
            if (subSector != null && subSector.Sector != null)
            {
                RooSector sector = subSector.Sector;

                if (RoomInfo.Flags.IsOverrideDepth1 && sector.Flags.SectorDepth == RooSectorFlags.DepthType.Depth1)
                    newheight = RoomInfo.Depth1;

                else if (RoomInfo.Flags.IsOverrideDepth2 && sector.Flags.SectorDepth == RooSectorFlags.DepthType.Depth2)
                    newheight = RoomInfo.Depth2;

                else if (RoomInfo.Flags.IsOverrideDepth3 && sector.Flags.SectorDepth == RooSectorFlags.DepthType.Depth3)
                    newheight = RoomInfo.Depth3;
            }

            // update immediately
            position3D.Y = newheight;

            // trigger changed event
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_POSITION3D));
        }

        /// <summary>
        /// Sets a destination and marks the object as moving,
        /// the movement will be processed step by step in UpdatePosition().
        /// </summary>
        /// <param name="Destination"></param>
        /// <param name="Speed"></param>
        public void StartMoveTo(V2 Destination, byte Speed)
        {           
            // dynamic speed adjustment for objects not ourself             
            // kicks in if we receive a new move-destination before our
            // playback of the old one has finished
            // currently just 1:1 ported from the old client
            // subject to be improved.
            if (!IsAvatar)
            {
                V2 moveNew = Destination - Position2D;

                // don't start a move to the old location
                if (moveNew.LengthSquared < 1.0f)
                    return;

                if (isMoving)
                {
                    if (horizontalSpeed < (Real)Speed)
                        horizontalSpeed = Speed;

                    Real moveNewLen = moveNew.Length;
                                   
                    // too far away from last destination, increase speed
                    if (moveNewLen > (1.0f / (Real)GeometryConstants.FINENESS))
                    {
                        V2   moveOld    = MoveDestination - MoveStart;
                        Real moveOldLen = (Real)Math.Max(moveOld.Length, 0.00001f);

                        Real ratio = moveNewLen / moveOldLen;

                        if (ratio > 1.0f)
                            HorizontalSpeed *= ratio;
                    }                 
                }             
            }

            MoveStart = Position2D;
            MoveDestination = Destination;
            HorizontalSpeed = Speed;
            IsMoving = true;           
        }

        #endregion

        #region PropertyChanged listener
        /// <summary>
        /// Executed when MotionAnimation changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMotionAnimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONANIMATION));

            // mark for redraw when not moving
            if (!isMoving)
                appearanceChangeFlag = true;
        }

        /// <summary>
        /// Executed when a MotionSuboverlay changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMotionSubOverlayPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOTIONSUBOVERLAYS));

            // mark for redraw when moving
            if (isMoving)             
                appearanceChangeFlag = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMotionSubOverlaysListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    motionSubOverlays.LastAddedItem.PropertyChanged += OnMotionSubOverlayPropertyChanged;
                    break;

                case ListChangedType.ItemDeleted:
                    motionSubOverlays.LastDeletedItem.PropertyChanged -= OnMotionSubOverlayPropertyChanged;
                    break;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Processes continuous changes of the objects,
        /// call regularly in threadloop.
        /// May raise AppearanceChanged event and others.
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public override void Tick(double Tick, double Span)
        {
            // don't use base or 
            // appearancechanged will be triggered too early
            //base.Update(CurrentTick);

            // update animation
            animation.Tick(Tick, Span);

            // update suboverlay animations
            foreach (SubOverlay subOverlay in SubOverlays)
                subOverlay.Animation.Tick(Tick, Span);

            // update motionanimation
            motionAnimation.Tick(Tick, Span);

            // update motion suboverlay animations
            foreach (SubOverlay subOverlay in MotionSubOverlays)
                subOverlay.Animation.Tick(Tick, Span);

            // reset health status if we haven't received a new
            // ouch for more than X ms
            if ((healthStatus != HealthStatus.Unknown) &&
                ((DateTime.Now - healthStatusLastUpdate).TotalMilliseconds > HEALTHSTATUSRESETDELAY))
            {
                HealthStatus = HealthStatus.Unknown;
            }

            // possibly update appearancehash and raise event
            ProcessAppearance();          
        }

        /// <summary>
        /// Recalculates the ViewerAngle property based on a viewer's position.
        /// Call this when your viewer changes his position.
        /// </summary>
        /// <param name="ViewerPosition"></param>
        public void UpdateViewerAngle(V2 ViewerPosition)
        {
            // update viewer angle
            ViewerAngle = MathUtil.GetAngle(ViewerPosition, Position2D, AngleUnits);
        }

        /// <summary>
        /// Current 3D position converted to ROO format
        /// </summary>
        public V3 GetROOPosition3D()
        {
            V3 rooPos = position3D.Clone();
            rooPos.ConvertToROO();

            return rooPos;
        }

        /// <summary>
        /// Decompresses all not yet decompressed resources.
        /// </summary>
        public override void DecompressResources()
        {
            base.DecompressResources();
            
            foreach (SubOverlay subOv in motionSubOverlays)          
                if (subOv.Resource != null)
                    subOv.Resource.DecompressAll();            
        }

        /// <summary>
        /// Gets the distance between this and another RoomObject
        /// </summary>
        /// <param name="RoomObject"></param>
        /// <returns></returns>
        public Real GetDistance(RoomObject RoomObject)
        {
            return GetDistance(this, RoomObject);
        }

        /// <summary>
        /// Gets the squared distance between this and another RoomObject
        /// </summary>
        /// <param name="RoomObject"></param>
        /// <returns></returns>
        public Real GetDistanceSquared(RoomObject RoomObject)
        {
            return GetDistanceSquared(this, RoomObject);
        }
        
        /// <summary>
        /// Gets the angle that needs to bet set 
        /// to make this RoomObject
        /// look at another RoomObject
        /// </summary>
        /// <param name="LookAt"></param>
        /// <returns></returns>
        public ushort GetAngleTo(RoomObject LookAt)
        {
            V2 dir = LookAt.Position2D - this.Position2D;
            return MathUtil.GetAngleForDirection(dir);
        }

        /// <summary>
        /// Returns the smaller angle between this RoomObject
        /// and its angle and another RoomObject.
        /// </summary>
        /// <param name="RoomObject"></param>
        /// <returns></returns>
        public ushort GetAngleBetween(RoomObject RoomObject)
        {
            return MathUtil.GetAngle(RoomObject.Position2D, Position2D, AngleUnits, true);
        }

        /// <summary>
        /// Checks if this object is visible from a position
        /// in a room.
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Room"></param>
        /// <returns></returns>
        public bool IsVisibleFrom(V3 Position, RooFile Room)
        {
            if (Room != null)
            {
                // roo format variants
                V3 rooStart = Position.Clone();
                V3 rooEnd = Position3D.Clone();

                // add offset for playerheight to not use the groundheight
                rooStart.Y += GeometryConstants.PLAYERHEIGHT;
                rooEnd.Y += GeometryConstants.PLAYERHEIGHT;

                // convert to ROO format
                rooStart.ConvertToROO();
                rooEnd.ConvertToROO();

                // verify the object is visible
                return Room.VerifySight(rooStart, rooEnd);
            }
            else
                return false;
        }

        /// <summary>
        /// Returns all visible RoomObjects within given distance
        /// from this instance.
        /// </summary>
        /// <param name="RoomObjects">Objects to check</param>
        /// <param name="Room">Room used for collision</param>
        /// <param name="BackRadius">Max. distance behind</param>
        /// <param name="FrontRadius">Max. distance in front</param>
        /// <param name="AddAvatar">If also to add avatar roomobject</param>
        /// <returns></returns>
        public List<RoomObject> GetObjectsWithinDistance(IEnumerable<RoomObject> RoomObjects, RooFile Room, Real BackRadius, Real FrontRadius, bool AddAvatar)
        {
            List<RoomObject> list = new List<RoomObject>();

            if (RoomObjects != null && Room != null)
            {
                Real backradius2 = BackRadius * BackRadius;
                Real frontradius2 = FrontRadius * FrontRadius;
                ushort angle;
                Real dist2;

                foreach (RoomObject obj in RoomObjects)
                {
                    // don't add ourself and don't add avatar if not said so
                    if (obj != this && (AddAvatar || !obj.IsAvatar))
                    {
                        // get squared distance to object
                        dist2 = GetDistanceSquared(obj);

                        // skip everything that is farer way than front radius
                        if (dist2 <= frontradius2)
                        {
                            // get smaller angle between our orientation and the direction
                            // to the other object
                            angle = GetAngleBetween(obj);

                            // either in front of us and within frontradius
                            // or behind of us and and within backradius
                            bool infront = (angle < GeometryConstants.QUARTERMAXANGLE && dist2 <= frontradius2);
                            bool behind = (angle >= GeometryConstants.QUARTERMAXANGLE && dist2 <= backradius2);

                            // add it if also visible
                            if ((infront || behind) && obj.IsVisibleFrom(Position3D, Room))
                                list.Add(obj);
                        }
                    }
                }
            }

            return list;
        }

        #endregion

        #region Appearance

        /// <summary>
        /// Triggered when the viewerappearance has changed
        /// </summary>
        public event EventHandler ViewerAppearanceChanged;

        /// <summary>
        /// Mark the object to have changed appearance,
        /// forces a redraw.
        /// </summary>
        public void MarkForAppearanceChange()
        {
            appearanceChangeFlag = true;
        }

        /// <summary>
        /// A unique hash of the viewer's appearance of the object
        /// </summary>
        public uint ViewerAppearanceHash
        {
            get { return viewerAppearanceHash; }
            protected set
            {
                if (viewerAppearanceHash != value)
                {
                    viewerAppearanceHash = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_VIEWERAPPEARANCEHASH));
                }
            }
        }

        /// <summary>
        /// Creates a 32bit hash for the appearance of the RoomObject
        /// in frontview.
        /// </summary>
        /// <returns></returns>
        protected override uint GetAppearanceHash()
        {
            hash.Reset();

            hash.Step((uint)overlayFileRID);
            //hash.Step((uint)flags.Flags);

            if (isMoving)
            {
                hash.Step((uint)motionColorTranslation);

                if (resource != null)
                    hash.Step((uint)FrontFrameIndex);

                foreach (SubOverlay obj in motionSubOverlays)
                    hash.Step((uint)obj.GetAppearanceHash(false));
            }
            else
            {
                hash.Step((uint)colorTranslation);

                if (resource != null)
                    hash.Step((uint)FrontFrameIndex);

                foreach (SubOverlay obj in subOverlays)
                    hash.Step((uint)obj.GetAppearanceHash(false));
            }

            return hash.Finish();
        }

        /// <summary>
        /// Creates a 32bit hash for the appearance of the RoomObject
        /// for the current viewer.
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetViewerAppearanceHash()
        {
            hash.Reset();

            hash.Step((uint)overlayFileRID);
            //hash.Step((uint)flags.Flags);

            if (isMoving)
            {
                hash.Step((uint)motionColorTranslation);

                if (resource != null)
                    hash.Step((uint)ViewerFrameIndex);

                foreach (SubOverlay obj in motionSubOverlays)
                    hash.Step((uint)obj.GetAppearanceHash(true));
            }
            else
            {
                hash.Step((uint)colorTranslation);

                if (resource != null)
                    hash.Step((uint)ViewerFrameIndex);

                foreach (SubOverlay obj in subOverlays)
                    hash.Step((uint)obj.GetAppearanceHash(true));
            }

            return hash.Finish();
        }

        /// <summary>
        /// Call to possibly raise an AppearanceChanged 
        /// and ViewerAppearanceChanged event,
        /// if new AppearanceHash.
        /// </summary>
        /// <param name="Force"></param>
        protected override void ProcessAppearance(bool Force = false)
        {
            // no base
            // base.ProcessAppearance();

            // if the object is flagged to have possibly changed appearance
            if (appearanceChangeFlag || Force)
            {
                // update used frame indices
                UpdateFrameIndices();

                // recalculate appearance hashes
                uint apphash = GetAppearanceHash();
                uint vapphash = GetViewerAppearanceHash();

                if (apphash != appearanceHash || vapphash != viewerAppearanceHash || Force)
                {
                    // update the suboverlays parents/hotspots
                    foreach (SubOverlay subOv in subOverlays)
                        subOv.UpdateHotspots(this, subOverlays);

                    // update the motionsuboverlays parents/hotspots
                    foreach (SubOverlay subOv in motionSubOverlays)
                        subOv.UpdateHotspots(this, motionSubOverlays);

                    // trigger event if set, not force overriden
                    if (apphash != appearanceHash)
                    {
                        AppearanceHash = apphash;
                        RaiseAppearanceChanged();
                    }

                    // trigger event if set, not force overriden
                    if (vapphash != viewerAppearanceHash)
                    {
                        ViewerAppearanceHash = vapphash;
                        RaiseViewerAppearanceChanged();
                    }
                }
                
                // reset appearancechanged status we processed it
                appearanceChangeFlag = false;
            }
        }

        /// <summary>
        /// Updates the FrontFrameIndex and ViewerFrameIndex
        /// of resource and all suboverlays based on the current ViewerAngle property.
        /// </summary>
        protected override void UpdateFrameIndices()
        {
            // no base
            // base.UpdateFrameIndices();

            if (resource != null)
            {
                if (!isMoving)
                {
                    FrontFrameIndex = resource.GetFrameIndex(Animation.CurrentGroup, ObjectBase.DEFAULTANGLE);
                    ViewerFrameIndex = resource.GetFrameIndex(Animation.CurrentGroup, viewerAngle);
                }
                else
                {
                    FrontFrameIndex = resource.GetFrameIndex(MotionAnimation.CurrentGroup, ObjectBase.DEFAULTANGLE);
                    ViewerFrameIndex = resource.GetFrameIndex(MotionAnimation.CurrentGroup, viewerAngle);
                }

                if (FrontFrameIndex > -1 && resource.Frames.Count > FrontFrameIndex)
                    FrontFrame = resource.Frames[FrontFrameIndex];
                else
                    FrontFrame = null;

                if (ViewerFrameIndex > -1 && resource.Frames.Count > ViewerFrameIndex)
                    ViewerFrame = resource.Frames[ViewerFrameIndex];
                else
                    ViewerFrame = null;
            }
            else
            {
                FrontFrameIndex = -1;
                ViewerFrameIndex = -1;
                FrontFrame = null;
                ViewerFrame = null;
            }

            foreach (SubOverlay subOv in subOverlays)
                subOv.UpdateFrameIndices(viewerAngle);

            foreach (SubOverlay subOv in motionSubOverlays)
                subOv.UpdateFrameIndices(viewerAngle);
        }

        /// <summary>
        /// Raise AppearanceChanged event if listener
        /// </summary>
        protected void RaiseViewerAppearanceChanged()
        {
            if (ViewerAppearanceChanged != null)
                ViewerAppearanceChanged(this, new EventArgs());
        }
        #endregion

        public RenderInfo GetRenderInfo(
            bool UseViewerFrame = false,
            Real Quality = 1.0f,
            bool ApplyYOffset = true,
            byte RootHotspotIndex = 0,
            bool ScalePow2 = false,
            uint Width = 0,
            uint Height = 0,
            bool CenterVertical = false,
            bool CenterHorizontal = false)
        {
            return new RenderInfo(this, UseViewerFrame, ApplyYOffset, RootHotspotIndex, Quality, ScalePow2, Width, Height, CenterVertical, CenterHorizontal);
        }

        #region Static
        /// <summary>
        /// Static
        /// </summary>
        /// <param name="ObjectA"></param>
        /// <param name="ObjectB"></param>
        /// <returns></returns>
        public static Real GetDistanceSquared(RoomObject ObjectA, RoomObject ObjectB)
        {
			V3 AB = ObjectB.Position3D - ObjectA.Position3D;

            return AB.LengthSquared;
        }

        /// <summary>
        /// Static
        /// </summary>
        /// <param name="ObjectA"></param>
        /// <param name="ObjectB"></param>
        /// <returns></returns>
        public static Real GetDistance(RoomObject ObjectA, RoomObject ObjectB)
        {
            return (Real)Math.Sqrt(GetDistanceSquared(ObjectA, ObjectB));
        }
        #endregion
    }
}
