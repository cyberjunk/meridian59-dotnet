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
    /// Provides conversion for Meridian 32-Bit timestamps.
    /// </summary>
    public static class MeridianDate
    {
        /// <summary>
        /// A static seconds offset from UNIXTIMESTAMP ZERO
        /// to a newer timestamp (currently january 2014).
        /// Because M59 timestamps are only 32 bit.
        /// See 'DateFromSeconds()' in 'newsread.c'.
        /// </summary>
#if VANILLA
        public const uint OFFSET = 1400000000;
#else
        public const uint OFFSET = 1388534400;
#endif

        /// <summary>
        /// How many m59 seconds elapse during one real second
        /// </summary>
        public const int M59SECONDSPERSECOND = 86400 / 7200;

        /// <summary>
        /// DateTime representing the UNIX timestamp zero.
        /// </summary>
        public static readonly DateTime UNIXZERO = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        
        /// <summary>
        /// The Meridian ZERO time
        /// </summary>
        public static readonly DateTime MERIDIANZERO = UNIXZERO.AddSeconds(OFFSET);
        
        /// <summary>
        /// Converts a meridian timestamp into DateTime
        /// </summary>
        /// <param name="MeridianDate"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(uint MeridianDate)
        {
            return MERIDIANZERO.AddSeconds(MeridianDate);
        }

        /// <summary>
        /// Converts a DateTime to meridian timestamp
        /// </summary>
        /// <param name="DateTime"></param>
        /// <returns></returns>
        public static uint ToMeridianDate(DateTime DateTime)
        {
            // the timespan since meridian ZERO
            TimeSpan span = DateTime - MERIDIANZERO;

            return Convert.ToUInt32(span.TotalSeconds);
        }

        /// <summary>
        /// Returns the current time in Meridian 59
        /// with dummy date set to 1/1/1.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMeridianTime()
        {
            // the current global time
            DateTime now = DateTime.UtcNow;

            // work around daylight saving time +-1
            if (!now.IsDaylightSavingTime())
                now = now.AddHours(1.0);

            // sum up real seconds elapsed since last m59 midnight
            // which happens every 2 hours
            int secs = (now.Hour % 2) * 60 * 60;
            secs += now.Minute * 60;
            secs += now.Second;

            // scale to M59 secnods
            secs *= M59SECONDSPERSECOND;

            // get m59 hours, minutes and seconds
            int hours = secs / 3600;
            int minutes = (secs % 3600) / 60;
            int seconds = (secs % 3600) % 60;

            // return m59 time set with a dummy date
            return new DateTime(1, 1, 1, hours, minutes, seconds);
        }
    }
}
