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

namespace Meridian59.Common
{
    /// <summary>
    /// Murmur3, a fast 32-Bit hash translated from
    /// http://en.wikipedia.org/wiki/MurmurHash
    /// </summary>
    public class Murmur3
    {
        private const byte R1 = 15;
        private const byte R2 = 13;
        private const uint C1 = 0xcc9e2d51;
        private const uint C2 = 0x1b873593;
        private const uint C3 = 0x85ebca6b;
        private const uint C4 = 0xc2b2ae35; 
        private const uint M = 5;
        private const uint N = 0xe6546b64;

        private byte len;
        private uint hash;

        /// <summary>
        /// Constructor - Create new hash
        /// </summary>
        /// <param name="Seed">A previously hash to continue</param>
        public Murmur3(uint Seed = 0)
        {
            Reset(Seed);
        }

        /// <summary>
        /// Compute in another value/step into hashvalue
        /// </summary>
        /// <param name="Value"></param>
        public void Step(uint Value)
        {
            Value = Value * C1;
            Value = (Value << R1) | (Value >> (32 - R1));
            Value = Value * C2;

            hash = hash ^ Value;
            hash = (hash << R2) | (hash >> (32 - R2));
            hash = hash * M + N;

            len++;
        }

        /// <summary>
        /// Resets the hasher to the given seed
        /// </summary>
        /// <param name="Seed"></param>
        public void Reset(uint Seed = 0)
        {
            hash = Seed;
            len = 0;
        }

        /// <summary>
        /// Finish hash computation
        /// </summary>
        /// <returns>Hash value</returns>
        public uint Finish()
        {
            hash = hash ^ len;

            hash = hash ^ (hash >> 16);
            hash = hash * C3;
            hash = hash ^ (hash >> 13);
            hash = hash * C4;
            hash = hash ^ (hash >> 16);

            return hash;
        }
    }   
}
