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
using System.Collections.Generic;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else
using Real = System.Single;
#endif

namespace Meridian59.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Polygon : List<V2>
    {
        /// <summary>
        /// True if this polygon has at least 3 different points.
        /// None of them equal or on the line given by other two.
        /// Note: Points must be ordered.
        /// </summary>
        /// <returns></returns>
        public bool IsPolygon()
        {
            return
                Count >= 3 &&
                !this[0].IsOnLineSegment(this[1], this[2]) &&
                !this[1].IsOnLineSegment(this[0], this[2]) &&
                !this[2].IsOnLineSegment(this[0], this[1]);
        }
    }
}
