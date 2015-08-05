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

using System.Text;
using Meridian59.Common;
using Meridian59.Common.Enums;

namespace Meridian59.Protocol
{  
    /// <summary>
    /// Use to decode incoming Message types
    /// </summary>
    public class PIDecoder
    {
        #region Constants
        /// <summary>
        /// This string is used if the server transmits a zero resourceid.
        /// </summary>
        public const string FALLBACKSTRING = "BLAKSTON: Greenwich Q Zjiria";
        
        /// <summary>
        /// This value is involved in decoding.
        /// </summary>
        public const byte XORValue = 0xED;

        /// <summary>
        /// This value is involved in decoding.
        /// </summary>
        public const byte ANDValue = 0x7F;
        #endregion

        #region Fields
        /// <summary>
        /// The bytes of the local iterate-string in use.
        /// </summary>
        protected byte[] stringBytes;

        /// <summary>
        /// A cursor on the local iterate-string
        /// </summary>
        protected short cursor;

        /// <summary>
        /// The byte involved in the decoding of next PI
        /// </summary>
        protected uint currentDecodeByte;

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
        /// Constructor
        /// </summary>
        /// <param name="StringResources">A threadsafe dictionary used to resolve Meridian Strings from.</param>
		public PIDecoder(StringDictionary StringResources)
        {
            this.stringResources = StringResources;
            
            if (StringResources == null)
                stringBytes = Encoding.Default.GetBytes(FALLBACKSTRING);
            
            Reset();
        }

        #region Methods
        /// <summary>
        /// Reset the PIDecoder to initial state
        /// </summary>
        public void Reset()
        {
            currentDecodeByte = 0x00;
            cursor = 0;
            enabled = false;
        }

        /// <summary>
        /// Returns the decoded message type
        /// </summary>
        /// <param name="MessageType">Encoded message type</param>
        /// <returns></returns>
        public byte Decode(byte MessageType)
        {          
            // Decode the type with the current DecodeByte
            byte decodedPI = (byte)(((uint)MessageType ^ (uint)(currentDecodeByte & 0xFF)) & 0xFF);

            if (enabled)
            {
                // reset cursor if we reached end of iteration string
                if (cursor == stringBytes.Length)
                    cursor = 0;

                // Iterate the DecodeByte based on stringresource
                currentDecodeByte += ((uint)stringBytes[cursor] & ANDValue);

                // raise cursor
                cursor++;
            }

            return decodedPI;           
        }

        /// <summary>
        /// Update the PIDecoder from values attached to PingReply message
        /// </summary>
        /// <param name="NewDecodeByte">Is attached on PingReply</param>
        /// <param name="ResourceID">Is attached on PingReply</param>
        public void Update(byte NewDecodeByte, uint ResourceID)
        {
            // Update the decode byte
            currentDecodeByte = (uint)(NewDecodeByte ^ XORValue);

            // zero resourceid indicates use of fallbackstring
            if (ResourceID == 0)
                stringBytes = Encoding.Default.GetBytes(FALLBACKSTRING);

            else
            {
                string iterateString;

                // try to get the string from dictionary (ALWAYS english!)
                if (stringResources.TryGetValue(ResourceID, out iterateString, LanguageCode.English))
                {
                    stringBytes = Encoding.Default.GetBytes(iterateString);
                }
                else
                {
                    // THIS MOST LIKELY KILLS YOUR CONNECTION
                    // AND SHOULD NOT BE REACHED
                    stringBytes = Encoding.Default.GetBytes(FALLBACKSTRING);
                }
            }
               
            // Reset the cursor on the local iteration string
            cursor = 0;

            // Mark as enabled
            enabled = true;
        }
        #endregion
    }
}
