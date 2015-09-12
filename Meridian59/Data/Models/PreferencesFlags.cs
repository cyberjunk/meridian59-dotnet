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

#if !VANILLA

using System;
using System.ComponentModel;
using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Client preference flags.
    /// </summary>
    [Serializable]
    public class PreferencesFlags : Flags, IUpdatable<PreferencesFlags>
    {
        #region Bitmasks
        /// Flags for client preference options, e.g. safety.
        private const uint CF_SAFETY_OFF = 0x00000001; // Player has safety off
        private const uint CF_TEMPSAFE = 0x00000002; // Player has temp safety on death activated
        private const uint CF_GROUPING = 0x00000004; // Player is grouping
        private const uint CF_AUTOLOOT = 0x00000008; // Player is automatically picking up loot
        private const uint CF_AUTOCOMBINE = 0x00000010; // Player automatically combines spell items
        private const uint CF_REAGENTBAG = 0x00000020; // Player automatically puts items into reagent bag
        private const uint CF_SPELLPOWER = 0x00000040; // Player gets spellpower readout from cast spells
        #endregion

        #region Constants
        public new const string PROPNAME_FLAGS = "Flags";
        #endregion

        #region Properties
        /// <summary>
        /// Whether safety is enabled or not
        /// </summary>
        ///
        public bool IsSafety
        {
           get { return (flags & CF_SAFETY_OFF) == CF_SAFETY_OFF; }
           set
           {
              if (value) Value |= CF_SAFETY_OFF;
              else Value &= ~CF_SAFETY_OFF;
           }
        }

        /// <summary>
        /// Whether tempsafe is enabled or not
        /// </summary>
        public bool TempSafe
        {
           get { return (flags & CF_TEMPSAFE) == CF_TEMPSAFE; }
           set
           {
              if (value) Value |= CF_TEMPSAFE;
              else Value &= ~CF_TEMPSAFE;
           }
        }

        /// <summary>
        /// Whether grouping is enabled or not
        /// </summary>
        public bool Grouping
        {
           get { return (flags & CF_GROUPING) == CF_GROUPING; }
           set
           {
              if (value) Value |= CF_GROUPING;
              else Value &= ~CF_GROUPING;
           }
        }

        /// <summary>
        /// Whether reagent bag use is enabled or not
        /// </summary>
        public bool ReagentBag
        {
           get { return (flags & CF_REAGENTBAG) == CF_REAGENTBAG; }
           set
           {
              if (value) Value |= CF_REAGENTBAG;
              else Value &= ~CF_REAGENTBAG;
           }
        }

        /// <summary>
        /// Whether autolooting items is enabled or not
        /// </summary>
        public bool AutoLoot
        {
           get { return (flags & CF_AUTOLOOT) == CF_AUTOLOOT; }
           set
           {
              if (value) Value |= CF_AUTOLOOT;
              else Value &= ~CF_AUTOLOOT;
           }
        }

        /// <summary>
        /// Whether autocombining spellitems is enabled or not
        /// </summary>
        public bool AutoCombine
        {
           get { return (flags & CF_AUTOCOMBINE) == CF_AUTOCOMBINE; }
           set
           {
              if (value) Value |= CF_AUTOCOMBINE;
              else Value &= ~CF_AUTOCOMBINE;
           }
        }

        /// <summary>
        /// Whether to display spellpower for spell cast or not
        /// </summary>
        public bool SpellPower
        {
           get { return (flags & CF_SPELLPOWER) == CF_SPELLPOWER; }
           set
           {
              if (value) Value |= CF_SPELLPOWER;
              else Value &= ~CF_SPELLPOWER;
           }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Old flags</param>
        public PreferencesFlags(
            uint Value = 0)
            : base(Value)
        {
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public PreferencesFlags(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        public unsafe PreferencesFlags(ref byte* Buffer)
            : base(ref Buffer) { }
        #endregion

        #region IUpdatable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Flags"></param>
        /// <param name="RaiseChangedEvent"></param>
        public void UpdateFromModel(PreferencesFlags Flags, bool RaiseChangedEvent)
        {
            if (Flags == null)
                return;

            if (RaiseChangedEvent)
            {
                Value = Flags.Value;
            }
            else
            {
                flags = Flags.Value;
            }
        }
        #endregion
    }
}
#endif