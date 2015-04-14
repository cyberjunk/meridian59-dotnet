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
using Meridian59.Common.Enums;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// An object in your inventory.
    /// </summary>
    public class InventoryObject : ObjectBase
    {
        #region Constants
        public const string PROPNAME_ISINUSE        = "IsInUse";
        public const string PROPNAME_NUMOFSAMENAME  = "NumOfSameName";
        #endregion

        #region Fields
        protected bool isInUse;
        protected uint numOfSameName;
        #endregion

        #region Properties
        /// <summary>
        /// Whether the item is currently in use or not.
        /// </summary>
        public bool IsInUse
        {
            get { return isInUse; }
            set
            {
                if (isInUse != value)
                {
                    isInUse = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISINUSE));
                    appearanceChangeFlag = true;
                }
            }
        }

        /// <summary>
        /// Used to keep track if this is the first (0) , second (1), ... of
        /// the item-name within a collection it is contained in.
        /// </summary>
        public uint NumOfSameName
        {
            get { return numOfSameName; }
            set
            {
                if (numOfSameName != value)
                {
                    numOfSameName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NUMOFSAMENAME));
                }
            }
        }
        #endregion

        /// <summary>
        /// Empty constructor
        /// </summary>
        public InventoryObject()
            : base() { }

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
        /// <param name="IsInUse"></param>
        public InventoryObject(
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
            bool IsInUse
            )            
            : base(
                ID, Count, 
                OverlayFileRID, NameRID, Flags, 
                LightFlags, LightIntensity, LightColor, 
                FirstAnimationType, ColorTranslation, Effect, Animation, SubOverlays)
        {
            this.isInUse = IsInUse;            
        }

        /// <summary>
        /// Constructor by parser
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public InventoryObject(byte[] Buffer, int StartIndex = 0)
            : base(true, Buffer, StartIndex) { }

        /// <summary>
        /// Constructor by pointerbased parser
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe InventoryObject(ref byte* Buffer)
            : base(true, ref Buffer) { }

        #region IClearable
        public override void Clear(bool RaiseChangedEvent)
        {
            base.Clear(RaiseChangedEvent);

            if (RaiseChangedEvent)
            {
                IsInUse = false;
                NumOfSameName = 0;
            }
            else
            {
                isInUse = false;
                numOfSameName = 0;
            }
        }
        #endregion

        /// <summary>
        /// Creates a 32bit hash for the appearance of the InventoryObject.
        /// </summary>
        /// <returns></returns>
        protected override uint GetAppearanceHash()
        {
            hash.Reset(base.GetAppearanceHash());

            hash.Step(Convert.ToUInt32(isInUse));
            
            return hash.Finish();
        }
    }
}
