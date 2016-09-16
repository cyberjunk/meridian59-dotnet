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
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A model for first person weapon/shield/... overlays
    /// </summary>
    [Serializable]
    public class PlayerOverlay : ObjectBase
    {
        #region Constants
        /* 
         * These constants are used in databinding and avoid nasty and slow reflection calls
         * Make sure to keep them in sync with the actual property names.
         */

        public const string PROPNAME_RENDERPOSITION = "RenderPosition";       
        #endregion

        #region IByteSerializable
        public override int ByteLength
        { 
            get { 
                return TypeSizes.BYTE + base.ByteLength; 
            }
        }
    
        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            renderPosition = (PlayerOverlayHotspot)Buffer[cursor];
            cursor++;
            
            cursor += base.ReadFrom(Buffer, cursor);
            
            return cursor - StartIndex; 
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (byte)renderPosition;
            cursor++;
            
            cursor += base.WriteTo(Buffer, cursor);
         
            return cursor - StartIndex;
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            renderPosition = (PlayerOverlayHotspot)Buffer[0];
            Buffer++;

            base.ReadFrom(ref Buffer);
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)renderPosition;
            Buffer++;

            base.WriteTo(ref Buffer);
        }
        #endregion

        #region Fields
        protected PlayerOverlayHotspot renderPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Hotspot index where to show overlay
        /// </summary>
        public PlayerOverlayHotspot RenderPosition
        {
            get { return renderPosition; }
            set
            {
                if (renderPosition != value)
                {
                    renderPosition = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RENDERPOSITION));
                }
            }
        }  
        #endregion

        #region Constructors
        public PlayerOverlay() 
            : base() 
        {
            hasLight = false;
        }

        public PlayerOverlay(
            uint ID,
            uint Count,
            uint OverlayFileRID,
            uint NameRID,
            uint Flags,
            LightingInfo LightingInfo,
            AnimationType FirstAnimationType,
            byte ColorTranslation,
            byte Effect,
            Animation Animation,
            BaseList<SubOverlay> SubOverlays,
            PlayerOverlayHotspot RenderPosition)
            : base(
                ID, Count, OverlayFileRID, NameRID, Flags,
                LightingInfo, FirstAnimationType,
                ColorTranslation, Effect,
                Animation, SubOverlays, false)
        {
            renderPosition = RenderPosition;
        }

        public PlayerOverlay(byte[] Buffer, int StartIndex = 0)
            : base(false, Buffer, StartIndex) { }

        public unsafe PlayerOverlay(ref byte* Buffer)
            : base(false, ref Buffer) { }

        #endregion

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                RenderPosition = 0;
            }
            else
            {
                renderPosition = 0;
            }
        }
        #endregion        
    }
}
