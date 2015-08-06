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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Files;
using Meridian59.Files.BGF;
using Meridian59.Common;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Suboverlay information (hotspot, color, ...)
    /// </summary>
    [Serializable]
    public class SubOverlay : IByteSerializableFast, INotifyPropertyChanged, IClearable, IStringResolvable, IResourceResolvable
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_RESOURCEID = "ResourceID";
        public const string PROPNAME_ANIMATION = "Animation";
        public const string PROPNAME_HOTSPOT = "HotSpot";
        public const string PROPNAME_FIRSTANIMATIONTYPE = "FirstAnimationType";
        public const string PROPNAME_COLORTRANSLATION = "ColorTranslation";
        public const string PROPNAME_EFFECT = "Effect";
        public const string PROPNAME_NAME = "Name";
        public const string PROPNAME_RESOURCE = "Resource";   
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
                // Sum up lengths
                int len = TypeSizes.INT + TypeSizes.BYTE;

                // possibly ColorTranslation OR Effect, not BOTH, or none at all)
                if (firstAnimationType > 0)
                    len += TypeSizes.BYTE + TypeSizes.BYTE;

                len += animation.ByteLength;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            // write ID (4 bytes)
            Array.Copy(BitConverter.GetBytes(resourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            // write hotspotindex (1 byte)
            Buffer[cursor] = hotSpot;
            cursor++;

            // possibly write first animation (if set)
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

            // write animation (n bytes)
            cursor += animation.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            this.resourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            this.hotSpot = Buffer[cursor];
            cursor++;

            if ((AnimationType)Buffer[cursor] == AnimationType.TRANSLATION)           // check if the next byte is animationtype                           
            {                                                                           // TRANSLATION or EFFECT
                firstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                colorTranslation = Buffer[cursor];                                      // Translation (1 byte)
                cursor++;
            }
            else if (((AnimationType)Buffer[cursor] == AnimationType.EFFECT))
            {
                firstAnimationType = (AnimationType)Buffer[cursor];
                cursor++;

                effect = Buffer[cursor];                                                // Effect (1 byte)
                cursor++;
            }

            // extract animation
            this.animation = Animation.ExtractAnimation(Buffer, cursor);
            animation.PropertyChanged += OnAnimationPropertyChanged;
            cursor += this.animation.ByteLength;

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((uint*)Buffer) = resourceID;
            Buffer += TypeSizes.INT;

            Buffer[0] = hotSpot;
            Buffer++;

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
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            resourceID = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            hotSpot = Buffer[0];
            Buffer++;

            if ((AnimationType)Buffer[0] == AnimationType.TRANSLATION)        // check if the next byte is animationtype                           
            {
                firstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                colorTranslation = Buffer[0];
                Buffer++;
            }
            else if ((AnimationType)Buffer[0] == AnimationType.EFFECT)
            {
                firstAnimationType = (AnimationType)Buffer[0];
                Buffer++;

                effect = Buffer[0];
                Buffer++;
            }

            animation = Animation.ExtractAnimation(ref Buffer);
            animation.PropertyChanged += OnAnimationPropertyChanged;
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
        protected uint resourceID;
        protected byte hotSpot;
        protected AnimationType firstAnimationType;
        protected byte colorTranslation;
        protected byte effect;
        protected Animation animation;
        protected string name;
        protected BgfFile resource;
        protected Murmur3 hash = new Murmur3();          
        #endregion

        #region Properties
        /// <summary>
        /// The ResourceID in stringdictionary
        /// </summary>
        public uint ResourceID
        {
            get { return resourceID; }
            set
            {
                if (resourceID != value)
                {
                    resourceID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOURCEID));
                }
            }
        }

        /// <summary>
        /// Attached animation
        /// </summary>
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

        /// <summary>
        /// Index of hotspot to attach
        /// </summary>
        public byte HotSpot
        {
            get { return hotSpot; }
            set
            {
                if (hotSpot != value)
                {
                    hotSpot = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HOTSPOT));
                }
            }
        }
        
        /// <summary>
        /// First animation type (either effect or color)
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
        /// Color palette index to apply
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
                }
            }
        }
        
        /// <summary>
        /// Effect index
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
        /// Resolved name
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
        /// Resolved resource
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
                }
            }
        }

        /// <summary>
        /// Current BgfBitmap frame index in resource for front view
        /// </summary>
        public int FrontFrameIndex { get; protected set; }

        /// <summary>
        /// Current BgfBitmap frame index in resource for viewer view
        /// </summary>
        public int ViewerFrameIndex { get; protected set; }

        /// <summary>
        /// Current BgfBitmap in resource for front view
        /// </summary>
        public BgfBitmap FrontFrame { get; protected set; }

        /// <summary>
        /// Current BgfBitmap in resource for viewer view
        /// </summary>
        public BgfBitmap ViewerFrame { get; protected set; }

        /// <summary>
        /// The SubOverlay this is a child of in frontview.
        /// Used in double nested cases (subov on subov).
        /// Otherwise main overlay is parent.
        /// </summary>
        public SubOverlay FrontParent { get; protected set; }

        /// <summary>
        /// The SubOverlay this is a child of in viewerview.
        /// Used in double nested cases (subov on subov).
        /// Otherwise main overlay is parent
        /// </summary>
        public SubOverlay ViewerParent { get; protected set; }

        /// <summary>
        /// The hotspot this is attached to in frontview.
        /// </summary>
        public BgfBitmapHotspot FrontHotspot { get; protected set; }

        /// <summary>
        /// The hotspot this is attached to in viewerview.
        /// </summary>
        public BgfBitmapHotspot ViewerHotspot { get; protected set; }

        #endregion

        #region Constructors
        public SubOverlay()
        {
            Clear(false);
        }

        public SubOverlay(uint ResourceID, Animation LinkedAnimation, byte HotSpotIndex, byte ColorTranslation, byte Effect)
        {
            this.resourceID = ResourceID;
            this.animation = LinkedAnimation;
            this.hotSpot = HotSpotIndex;

            if ((ColorTranslation != 0x00) && (Effect == 0x00))
            {
                firstAnimationType = AnimationType.TRANSLATION;
                this.colorTranslation = ColorTranslation;
            }
            else if ((ColorTranslation == 0x00) && (Effect != 0x00))
            {
                firstAnimationType = AnimationType.EFFECT;
                this.effect = Effect;
            }
            else if ((ColorTranslation == 0x00) && (Effect == 0x00))
            {
                firstAnimationType = AnimationType.TRANSLATION;
                this.colorTranslation = 0x00;
            }
            else
                throw new Exception("Don't set Translation AND Effect at same time");
        }

        public SubOverlay(byte[] Buffer, int StartIndex = 0)
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe SubOverlay(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }

        #endregion

        #region IClearable
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                ResourceID = 0;
                HotSpot = 0;
                FirstAnimationType = 0;
                ColorTranslation = 0;
                Effect = 0;
                Animation = new AnimationNone();
                Name = String.Empty;
            }
            else
            {
                resourceID = 0;
                hotSpot = 0;
                firstAnimationType = 0;
                colorTranslation = 0;
                effect = 0;
                animation = new AnimationNone();
                name = String.Empty;
            }
        }
        #endregion

        #region IStringResolvable
		public virtual void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_name;

			StringResources.TryGetValue(resourceID, out res_name);
            
            if (RaiseChangedEvent)
            {
                if (res_name != null) Name = res_name;
                else Name = String.Empty;               
            }
            else
            {
                if (res_name != null) name = res_name;
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
                    Resource = M59ResourceManager.GetObject(Name);                                                         
                }
                else
                {
                    resource = M59ResourceManager.GetObject(Name);
                }

                if (resource != null)
                {
                    animation.GroupMax = resource.FrameSets.Count;
                }
            }
        }
        #endregion

        /// <summary>
        /// Creates a 32bit hash for the appearance of the suboverlay.
        /// </summary>
        /// <param name="UseViewerFrame">Whether to use ViewerFrame or FrontFrame</param>
        /// <returns></returns>
        public uint GetAppearanceHash(bool UseViewerFrame)
        {
            hash.Reset();

            hash.Step((uint)resourceID);
            hash.Step((uint)colorTranslation);
            hash.Step((uint)hotSpot);

            if (UseViewerFrame)           
                hash.Step((uint)ViewerFrameIndex);
            else
                hash.Step((uint)FrontFrameIndex);

            return hash.Finish();
        }

        /// <summary>
        /// Updates the Viewer-, FrontFrame and indices
        /// if resource available.
        /// </summary>
        /// <param name="ViewerAngle">An viewer angle on the suboverlay.</param>
        public void UpdateFrameIndices(ushort ViewerAngle)
        {
            if (resource != null)
            {
                FrontFrameIndex = resource.GetFrameIndex(Animation.CurrentGroup, ObjectBase.DEFAULTANGLE);
                ViewerFrameIndex = resource.GetFrameIndex(Animation.CurrentGroup, ViewerAngle);

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
        }

        /// <summary>
        /// Updates the ViewerParent and ViewerHotspot property.
        /// </summary>
        /// <param name="Root">The object this suboverlay belongs to</param>
        /// <param name="SubOverlays">The current suboverlays of the object</param>
        public void UpdateHotspots(ObjectBase Root, IList<SubOverlay> SubOverlays)
        {
            FrontParent = null;
            FrontHotspot = null;
            ViewerParent = null;
            ViewerHotspot = null;

            // try find hotspot on active mainoverlay frame
            if (Root.ViewerFrame != null)           
                ViewerHotspot = Root.ViewerFrame.FindHotspot(hotSpot);
           
            // if not found
            if (ViewerHotspot == null)
            {
                // try find on suboverlays
                foreach (SubOverlay subOv in SubOverlays)
                {
                    if (subOv.ViewerFrame != null)
                    {
                        ViewerHotspot = subOv.ViewerFrame.FindHotspot(hotSpot);
                        if (ViewerHotspot != null)
                        {
                            ViewerParent = subOv;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the ViewerParent, ViewerHotspot and
        /// FrontParent and FrontHotspot properties.
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="SubOverlays"></param>
        public void UpdateHotspots(RoomObject Root, IList<SubOverlay> SubOverlays)
        {
            // do viewer parts
            UpdateHotspots((ObjectBase)Root, SubOverlays);

            // try find hotspot on active mainoverlay frame
            if (Root.FrontFrame != null)
                FrontHotspot = Root.FrontFrame.FindHotspot(hotSpot);

            // if not found
            if (FrontHotspot == null)
            {
                // try find on suboverlays
                foreach (SubOverlay subOv in SubOverlays)
                {
                    if (subOv.FrontFrame != null)
                    {
                        FrontHotspot = subOv.FrontFrame.FindHotspot(hotSpot);
                        if (FrontHotspot != null)
                        {
                            FrontParent = subOv;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle animation change.
        /// Raise propertychange for animation property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnAnimationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ANIMATION));
        }

        /// <summary>
        /// Overwritten. Returns ResourceID value as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return resourceID.ToString();
        }

        /// <summary>
        /// Set of information for rendering this SubOverlay.
        /// </summary>
        public class RenderInfo
        {
            public V2 Origin;
            public V2 Size;
            public BgfBitmap Bgf;
            public HotSpotType HotspotType;
            public SubOverlay SubOverlay;

            public RenderInfo()
            {
                Origin = new V2(0.0f, 0.0f);
                Size = new V2(0.0f, 0.0f);
                Bgf = null;
                HotspotType = HotSpotType.HOTSPOT_NONE;
                SubOverlay = null;
            }

            public void Scale(Real Value)
            {
                Origin.Scale(Value);
                Size.Scale(Value);
            }
        }
    }
}
