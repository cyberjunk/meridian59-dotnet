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

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// Bowtie flags
    /// </summary>
    [Serializable]
    public class BowtieFlags : Flags
    {
        #region Bitmasks
        private const uint BT_BELOW_POS     = 1U;       // below wall is bowtie & positive sector is on top at endpoint 0
        private const uint BT_BELOW_NEG     = 2U;       // below wall is bowtie & negative sector is on top at endpoint 0
        private const uint BT_ABOVE_POS     = 4U;       // above wall is bowtie & positive sector is on top at endpoint 0
        private const uint BT_ABOVE_NEG     = 8U;       // above wall is bowtie & negative sector is on top at endpoint 0

        private const uint BT_MASK_BELOW_BOWTIE = 3U;   // mask to test for bowtie on below wall
        private const uint BT_MASK_ABOVE_BOWTIE = 12U;  // mask to test for bowtie on above wall
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value"></param>
        public BowtieFlags(uint Value = 0)
            : base(Value) { }

        /// <summary>
        /// Below wall is bowtie and positive sector is on top at endpoint 0
        /// </summary>
        public bool IsBelowPos
        {
            get { return (flags & BT_BELOW_POS) == BT_BELOW_POS; }
            set 
            {
                if (value) flags |= BT_BELOW_POS;
                else flags &= ~BT_BELOW_POS;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Below wall is bowtie and negative sector is on top at endpoint 0
        /// </summary>
        public bool IsBelowNeg
        {
            get { return (flags & BT_BELOW_NEG) == BT_BELOW_NEG; }
            set
            {
                if (value) flags |= BT_BELOW_NEG;
                else flags &= ~BT_BELOW_NEG;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Above wall is bowtie and positive sector is on top at endpoint 0
        /// </summary>
        public bool IsAbovePos
        {
            get { return (flags & BT_ABOVE_POS) == BT_ABOVE_POS; }
            set
            {
                if (value) flags |= BT_ABOVE_POS;
                else flags &= ~BT_ABOVE_POS;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Above wall is bowtie and negative sector is on top at endpoint 0
        /// </summary>
        public bool IsAboveNeg
        {
            get { return (flags & BT_ABOVE_NEG) == BT_ABOVE_NEG; }
            set
            {
                if (value) flags |= BT_ABOVE_NEG;
                else flags &= ~BT_ABOVE_NEG;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Mask for bowtie variants on below wall.
        /// Contains bits of IsBelowPos and IsBelowNeg.
        /// </summary>
        public bool IsBelowBowtie
        {
            get { return (flags & BT_MASK_BELOW_BOWTIE) == BT_MASK_BELOW_BOWTIE; }
            set
            {
                if (value) flags |= BT_MASK_BELOW_BOWTIE;
                else flags &= ~BT_MASK_BELOW_BOWTIE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }

        /// <summary>
        /// Mask for bowtie variants on above wall.
        /// Contains bits of IsAbovePos and IsAboveNeg
        /// </summary>
        public bool IsAboveBowtie
        {
            get { return (flags & BT_MASK_ABOVE_BOWTIE) == BT_MASK_ABOVE_BOWTIE; }
            set
            {
                if (value) flags |= BT_MASK_ABOVE_BOWTIE;
                else flags &= ~BT_MASK_ABOVE_BOWTIE;

                RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
            }
        }    
    }
}
