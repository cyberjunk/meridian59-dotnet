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
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Structs;

namespace Meridian59.Protocol
{ 
    /// <summary>
    /// Implements encoding of message CRC
    /// </summary>
    public class CRCCreator
    {
        #region Constants
        public const uint CONST1 = 9301U;
        public const uint CONST2 = 49297U;
        public const uint CONST3 = 233280U;
        #endregion

        /// <summary>
        /// Currently used hash
        /// </summary>
        public HashTable CurrentHashTable;

        /// <summary>
        /// Last used hash
        /// </summary>
        public HashTable LastHashTable;
            
        /// <summary>
        /// Constructor
        /// </summary>
        public CRCCreator()
        {
            Reset();
        }

        /// <summary>
        /// Resets the CRC creator to initial state.
        /// </summary>
        public void Reset()
        {
            CurrentHashTable.HASH1 = 0;
            CurrentHashTable.HASH2 = 0;
            CurrentHashTable.HASH3 = 0;
            CurrentHashTable.HASH4 = 0;
            CurrentHashTable.HASH5 = 0;
        }

        /// <summary>
        /// Advances the HashTable to new values without creating a complete packetCRC
        /// DO NOT call this for an outgoing packet if you have already called "CreatePacketCRC"
        /// You would advance hashtable twice.
        /// This simply is a low cost variant for sniffed packets and advancing a mirrored/selfcreated hashtable in background.
        /// </summary>
        public void AdvanceHashTable()
        {
            // Construct/iterate to new HashMap (see disassembly for formula)
            LastHashTable = CurrentHashTable;

            CurrentHashTable.HASH1 = ((CurrentHashTable.HASH1 * CONST1) + CONST2) % CONST3;
            CurrentHashTable.HASH2 = ((CurrentHashTable.HASH2 * CONST1) + CONST2) % CONST3;
            CurrentHashTable.HASH3 = ((CurrentHashTable.HASH3 * CONST1) + CONST2) % CONST3;
            CurrentHashTable.HASH4 = ((CurrentHashTable.HASH4 * CONST1) + CONST2) % CONST3;
            CurrentHashTable.HASH5 = ((CurrentHashTable.HASH5 * CONST1) + CONST2) % CONST3;
        }
   
        /// <summary>
        /// Sets (and returns) a valid CRC on a GameMessage (and advances the hash if not in testmode).
        /// </summary>
        /// <param name="Message">Message to set CRC in header</param>
        /// <param name="BodyCRC">The plain CRC32</param>
        /// <param name="TestMode">If set to false, hash is not iterated</param>
        /// <returns>Shortened and encoded CRC also set on Message</returns>
        public ushort CreatePacketCRC(GameMessage Message, out ushort BodyCRC, bool TestMode = false)
        {
            // create a generic CRC32 of message body      
            uint crc32 = Crc32.Compute(Message.BodyBytes);
            
            // use first 2 bytes only
            BodyCRC = (ushort)crc32;

            // iterate hash
            HashTable newHashTable = new HashTable();
            newHashTable.HASH1 = ((CurrentHashTable.HASH1 * CONST1) + CONST2) % CONST3;
            newHashTable.HASH2 = ((CurrentHashTable.HASH2 * CONST1) + CONST2) % CONST3;
            newHashTable.HASH3 = ((CurrentHashTable.HASH3 * CONST1) + CONST2) % CONST3;
            newHashTable.HASH4 = ((CurrentHashTable.HASH4 * CONST1) + CONST2) % CONST3;
            newHashTable.HASH5 = ((CurrentHashTable.HASH5 * CONST1) + CONST2) % CONST3;
            uint ptr = newHashTable.HASH5 & 3;
            
            // select the right part to use
            uint HashToUse = 0;
            if (ptr == 0) HashToUse = newHashTable.HASH1;
            else if (ptr == 1) HashToUse = newHashTable.HASH2;
            else if (ptr == 2) HashToUse = newHashTable.HASH3;
            else if (ptr == 3) HashToUse = newHashTable.HASH4;
            else if (ptr == 4) HashToUse = newHashTable.HASH5;
           
            // do encryption of plain CRC32
            ushort shiftedPI = (ushort)((sbyte)Message.PI << 4);
            uint packetCRC = HashToUse ^ shiftedPI ^ Message.Header.BodyLength ^ BodyCRC;
            
            // set encoded and shortened CRC on message
            Message.Header.HeaderCRC = (ushort)packetCRC;

            // write back new Hashtable if not testMode
            if (!TestMode)
            {
                LastHashTable = CurrentHashTable;
                CurrentHashTable = newHashTable;  
            }

            return Message.Header.HeaderCRC;
        }

        /// <summary>
        /// Sets (and returns) a valid CRC on a GameMessage for UDP
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public ushort CreatePacketCRCUDP(GameMessage Message)
        {
            // create a generic CRC32 of message body
            uint crc32 = Crc32.Compute(Message.BodyBytes);

            // no scrambling on UDP

            // set encoded and shortened CRC on message
            Message.Header.HeaderCRC = (ushort)crc32;

            return Message.Header.HeaderCRC;
        }
    }
}
