#if USING_IRCDOTNET

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
using Meridian59.Common;
using IrcDotNet;
using System.Diagnostics;

namespace Meridian59.Bot.IRC
{
    /// <summary>
    /// Represents a flood protector that throttles data sent by the client.
    /// </summary>
    public class FloodPreventer : IIrcFloodPreventer
    {
        /// <summary>
        /// Saves the last tick
        /// </summary>
        protected long tickLast = 0;
        
        /// <summary>
        /// Divide 'ElapsedTicks' of StopWatch by this to get milliseconds.
        /// </summary>
        protected static readonly long MSTICKDIVISOR = Stopwatch.Frequency / 1000;

        /// <summary>
        /// Stopwatch object, usually high resolution on most CLR
        /// </summary>
        protected Stopwatch watch = new Stopwatch();

        /// <summary>
        /// How many more messages can be sent directly.
        /// </summary>
        public uint Available { get; protected set; }

        /// <summary>
        /// The maximum number of messages that can be sent in a burst.
        /// This also limits Available.
        /// </summary>
        public uint MaxBurst { get; protected set; }

        /// <summary>
        /// Milliseconds until a new available message is added.
        /// </summary>
        public uint FillDelay { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MaxBurst">The maximum number of messages that can be sent in a burst.</param>
        /// <param name="FillDelay">Milliseconds until a new available message is added.</param>
        public FloodPreventer(uint MaxBurst, uint FillDelay)
        {
            this.Available = MaxBurst;
            this.MaxBurst = MaxBurst;
            this.FillDelay = FillDelay;

            // start watch
            watch.Start();
        }

#region IIrcFloodPreventer
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetSendDelay()
        {
            // get tick for this threadloop in milliseconds
            long tick = watch.ElapsedTicks / MSTICKDIVISOR;
          
            // how long since we last called this
            long span = tick - tickLast;

            // increase available amount based on filldelay and elapsed span
            // but do not exceed maxburst
            Available = Math.Min(MaxBurst, Available + ((uint)span / FillDelay));

            // save the tick as last one
            tickLast = tick;

            // got some available? no need to delay
            if (Available > 0)
                return 0;

            // wait until next filldelay will be elapsed
            else
            {
                return Math.Min(Math.Max((long)FillDelay - span, 0), FillDelay);
            }
                
        }

        /// <summary>
        /// Called when a message was sent.
        /// </summary>
        public void HandleMessageSent()
        {
            // decrement available value
            if (Available > 0)
                Available--; 
        }

#endregion
    }
}
#endif