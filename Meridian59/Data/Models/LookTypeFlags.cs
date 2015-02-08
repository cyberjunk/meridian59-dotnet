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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Wrapper around looktype flags
    /// </summary>
    [Serializable]
    public class LookTypeFlags
    {
        #region Bitmasks
        private const uint DF_EDITABLE  = 0x00000001;   // Item has inscription/description that can be edited
        private const uint DF_INSCRIBED = 0x00000002;   // Item has inscription/description
        #endregion

        protected uint flags;

        public uint Flags
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {
                    flags = value;                  
                }
            }
        }
        public LookTypeFlags(byte Flags = 0)
        {
            this.Flags = Flags;
        }

        public override string ToString()
        {
            return Flags.ToString();
        }

        #region Property Accessors
        /* 
         * Easy to use property accessors,
         * Check set: AND
         * Set: OR
         * Unset: AND NEG
         */
        
        public bool IsEditable
        {
            get { return (Flags & DF_EDITABLE) == DF_EDITABLE; }
            set 
            {
                if (value) Flags |= DF_EDITABLE;
                else Flags &= ~DF_EDITABLE;
            }
        }
        public bool IsInscribed
        {
            get { return (Flags & DF_INSCRIBED) == DF_INSCRIBED; }
            set
            {
                if (value) Flags |= DF_INSCRIBED;
                else Flags &= ~DF_INSCRIBED;
            }
        }
        #endregion
    }
}
