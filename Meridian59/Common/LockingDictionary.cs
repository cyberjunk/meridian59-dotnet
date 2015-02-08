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

namespace Meridian59.Common
{
    /// <summary>
    /// A multithread-safe dictionary allowing multiple threads
    /// to request and insert entries from and to the dictionary.
    /// </summary>
    /// <remarks>
    /// This class flips its internal implementation based
    /// on the preprocessor flag CONCURRENT.
    /// If set it uses ConcurrentDictionary class from CLR,
    /// otherwise a custom implementation.
    /// </remarks>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LockingDictionary<TKey, TValue>

#if CONCURRENT
        : System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>
    {
        public LockingDictionary()
            : base()
        {
        }

        public LockingDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }
    }

#else
    {
        /// <summary>
        /// Basic dictionary class from .net
        /// </summary>
        protected Dictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Constructor
        /// </summary>
        public LockingDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LockingDictionary(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public void AddOrUpdate(TKey Key, TValue Value)
        {
            // lock
            lock (dictionary)
            {
                // add
                if (!dictionary.ContainsKey(Key))
                {
                    dictionary.Add(Key, Value);
                }

                // update
                else
                {
                    dictionary[Key] = Value;
                }
            }
        }

        public bool TryGetValue(TKey Key, out TValue Value)
        {
            // default returnvalue
            bool returnValue = false;

            // empty/null default
            Value = default(TValue);

            // lock
            lock (dictionary)
            {
                // get item
                returnValue = dictionary.TryGetValue(Key, out Value);
            }

            return returnValue;
        }

        public bool TryRemove(TKey Key, out TValue Value)
        {
            // default returnvalue
            bool returnValue = false;

            // empty/null default
            Value = default(TValue);

            // lock
            lock (dictionary)
            {
                // remove item
                returnValue = dictionary.TryGetValue(Key, out Value);
                
                if (returnValue)
                    dictionary.Remove(Key);
            }

            return returnValue;
        }
    }
#endif

}
