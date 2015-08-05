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

using Meridian59.Common;
using System.Text;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Encodes message types
    /// </summary>
    public class PIEncoder
    {
        #region Constants
        /// <summary>
        /// This string is usually used as local iterate-string (it's resource ID (20194) is transmitted in PingReply Packet).
        /// If the class can't access the stringlist to resolve the local iterate-strings from this will be used as fallback
        /// </summary>
        public const string StaticFallbackHashString = "BLAKSTON: Greenwich Q Zjiria";

        /// <summary>
        /// 
        /// </summary>
        public const uint ResourceID = 0;

        /// <summary>
        /// 
        /// </summary>
        public const byte SeedByte = 0;
        
        /// <summary>
        /// This value is involved.
        /// </summary>
        public const byte XORValue = 0xED;

        /// <summary>
        /// This value is involved.
        /// </summary>
        public const byte ANDValue = 0x7F;
        #endregion

        #region Fields
        /// <summary>
        /// The bytes of the local iterate-string in use (+ 0x00 termination)
        /// </summary>
        protected byte[] hashString;

        /// <summary>
        /// A cursor on the local iterate-string
        /// </summary>
        protected short cursor;

        /// <summary>
        /// A flag wether or not we received at least one PingReply with an Decode-Byte
        /// </summary>
        protected bool enabled;

        /// <summary>
        /// Stores reference to StringResources used to get local-iterate strings from (ID proposed in PingReply packet)
        /// </summary>
		protected StringDictionary stringResources;
        #endregion

        /// <summary>
        /// The byte involved in the decoding of next PI
        /// </summary>
        public byte CurrentEncodeByte { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StringResources">A threadsafe dictionary used to resolve Meridian Strings from.</param>
		public PIEncoder(StringDictionary StringResources)
        {
            this.stringResources = StringResources;
            
            if (StringResources == null)
                hashString = Encoding.Default.GetBytes(StaticFallbackHashString);
            
            Reset();
        }

        /// <summary>
        /// Reset the PIEncoder to initial state
        /// </summary>
        public void Reset()
        {
            CurrentEncodeByte = 0x00;
            cursor = 0;
            enabled = false;
        }

        /// <summary>
        /// Encode the Message type of a GameMessage based on the PIEncoder state
        /// </summary>
        /// <param name="MessageType">The GameMessage to encode type</param>
        public byte Encode(byte MessageType)
        {          
            // Encode the PI with the current EncodeByte
            byte encodedPI = (byte)(MessageType ^ CurrentEncodeByte);

            if (enabled)
            {
                // reset cursor if we reached end of iteration string
                if (cursor == hashString.Length)
                    cursor = 0;

                // Iterate the DecodeByte based on stringresource
                CurrentEncodeByte += (byte)(hashString[cursor] & ANDValue);

                // raise cursor
                cursor++;
            }

            return encodedPI;           
        }

        /// <summary>
        /// Update the PIDecoder from values attached to PingReply message
        /// </summary>
        public void Update()
        {
            // Set new EncodeByte
            CurrentEncodeByte = SeedByte ^ XORValue;

            // Update the local iteration string
            hashString = Encoding.Default.GetBytes(StaticFallbackHashString);

            // Reset the cursor on the local iteration string
            cursor = 0;

            // Mark as enabled
            enabled = true;                                   
        }
    }
}
