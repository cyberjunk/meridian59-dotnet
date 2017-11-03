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
using Meridian59.Protocol.Events;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Reads the bytes of game message from raw byte[] udp datagrams
    /// Subscribe the events!
    /// </summary>
    public class MessageParserUDP
    {
        #region Events
        /// <summary>
        /// Event when a complete message is available in the buffer.
        /// </summary>
        public event MessageBufferEventHandler MessageAvailable;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageParserUDP()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process a chunk (byte[]) of data with attached MemoryPosition.
        /// Use for reading external/hooked traffic.
        /// </summary>
        /// <param name="Buffer">Chunk of data to process</param>
        /// <param name="MemoryAddress"></param>
        /// <param name="Available"></param>
        public void Read(byte[] Buffer, IntPtr MemoryAddress, int Available)
        {
            OnProcessMessage(new MessageBufferEventArgs(Buffer, Available, 
                MemoryAddress, MessageDirection.ServerToClient, false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnProcessMessage(MessageBufferEventArgs e)
        {
            if (MessageAvailable != null)
                MessageAvailable(this, e);
        }
        #endregion
    }
}
