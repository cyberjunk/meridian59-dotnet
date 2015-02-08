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
    /// A multithread-safe queue allowing multiple threads
    /// to enqueue and dequeue items in a FIFO order.
    /// Often used in a producer/consumer pattern.
    /// </summary>
    /// <remarks>
    /// This class flips its internal implementation based
    /// on the preprocessor flag CONCURRENT.
    /// If set it uses ConcurrentQueue class from CLR,
    /// otherwise a custom implementation.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class LockingQueue<T>

/* If the CLR is supposed to have a System.Collections.Concurrent namespace
 * containing a ConcurrentQueue class, derive/use it. */
#if CONCURRENT
        : System.Collections.Concurrent.ConcurrentQueue<T>
    {   
    }

/* If no System.Collections.Concurrent namespace
 * containing ConcurrentQueue is available in the CLR, use our own implementation */
#else
    {
        /// <summary>
        /// Basic queue class from .net
        /// </summary>
        protected Queue<T> queue;

        /// <summary>
        /// Constructor
        /// </summary>
        public LockingQueue()
        {
            queue = new Queue<T>();
        }

        public void Enqueue(T Item)
        {
            lock (queue)
            {
                // enqueue item
                queue.Enqueue(Item);
            }
        }

        public bool TryDequeue(out T Item)
        {
            bool returnValue = false;
            Item = default(T);

            lock (queue)
            {
                if (queue.Count > 0)
                {
                    // dequeue item
                    Item = queue.Dequeue();

                    //
                    returnValue = true;
                }
            }

            return returnValue;
        }
    }
#endif

}
