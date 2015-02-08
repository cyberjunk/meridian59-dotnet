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
using System.IO;
using Meridian59.Protocol.Events;
using Meridian59.Common.Constants;
using Meridian59.Protocol.GameMessages;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Reads the bytes of game message from raw byte[] chunks or a Stream instance.
    /// Subscribe the events!
    /// </summary>
    public class MessageParser
    {
        #region Events
        /// <summary>
        /// Event when a complete message is available in the buffer.
        /// </summary>
        public event MessageBufferEventHandler MessageAvailable;

        /// <summary>
        /// Event when a splitted message appears
        /// </summary>
        public event SplittedMessageFoundEventHandler SplittedMessageFound;

        /// <summary>
        /// Event when a splitted message is continued
        /// </summary>
        public event CompletingSplittedMessageEventHandler CompletingSplittedMessage;

        /// <summary>
        /// Event when two lengths in header don't match
        /// </summary>
        public event MismatchMessageLengthFoundEventHandler MismatchMessageLengthFound;
        #endregion

        #region Fields
        /// <summary>
        /// Buffer to store exactly one complete Message in bytes
        /// </summary>
        protected byte[] messageBuffer;

        /// <summary>
        /// Cursor on "MessageBuffer" to currently write
        /// </summary>
        protected int cursor;
        
        /// <summary>
        /// Flag which readmode to chose
        /// </summary>
        protected bool readingHeader;

        /// <summary>
        /// The BodyLength of the next/currently read message
        /// </summary>
        protected ushort nextBodyLength;

        /// <summary>
        /// Cursor on a chunk of data
        /// </summary>
        protected int processed;

        /// <summary>
        /// Base memory address of the last bufferchunk
        /// Only used by unmanaged read.
        /// </summary>
        protected IntPtr baseMemoryAddress;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageParser()
        {
            Reset();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reset to initial state.
        /// </summary>
        public void Reset()
        {
            // initalize a buffer for a message with maximum size
            messageBuffer = new byte[ushort.MaxValue + GameMessage.HEADERLENGTH];
            
            // initial parsermode
            readingHeader = true;

            // inital cursor states
            cursor = 0;
            processed = 0;

            // default memory address
            baseMemoryAddress = (IntPtr)0;
        }

        /// <summary>
        /// Process a chunk (byte[]) of data with attached MemoryPosition.
        /// Use for reading external/hooked traffic.
        /// </summary>
        /// <param name="Buffer">Chunk of data to process</param>
        /// <param name="MemoryAddress"></param>
        /// <param name="Available"></param>
        public void Read(byte[] Buffer, IntPtr MemoryAddress, int Available)
        {
            // reset address of this buffer
            this.baseMemoryAddress = MemoryAddress;
            
            // reset processedbytes counter
            this.processed = 0;

            using (MemoryStream memStream = new MemoryStream(Buffer, 0, Available))
            {
                Read(memStream, Available);
            }          
        }

        /// <summary>
        /// Process available bytes on a Stream object.
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="Available"></param>
        public void Read(Stream Stream, int Available)
        {
            if (readingHeader)
            {
                // How much till we have complete header?
                int missingheader = GameMessage.HEADERLENGTH - cursor;
                if (Available < missingheader)
                {
                    // Read header chunk only
                    Stream.Read(messageBuffer, cursor, Available);
                    cursor += Available;

                    OnSplittedPacketFound(new SplittedMessageFoundEventArgs(
                        (int)baseMemoryAddress, processed + Available, processed, Available));

                    // < end of buffer >                   
                }
                else
                {
                    // If we can finish the header this time
                    Stream.Read(messageBuffer, cursor, missingheader);
                    cursor += missingheader;
                    processed += missingheader;
                    Available -= missingheader;

                    ushort len1 = BitConverter.ToUInt16(messageBuffer, 0);
                    ushort len2 = BitConverter.ToUInt16(messageBuffer, TypeSizes.SHORT + TypeSizes.SHORT);

                    if (len1 == len2)
                    {
                        nextBodyLength = len1;

                        // check if we can already read the body of the message
                        if (Available >= nextBodyLength)
                        {
                            // if cursor is smaller than processed, we're doing 2./3. message in same buffer
                            // so it's memory addr gets increased
                            IntPtr MemoryAddress = baseMemoryAddress;
                            if (cursor < processed)
                                MemoryAddress += processed - GameMessage.HEADERLENGTH;

                            // copy body to messagebuffer
                            Stream.Read(messageBuffer, cursor, nextBodyLength);
                            cursor += nextBodyLength;
                            Available -= nextBodyLength;
                            processed += nextBodyLength;

                            // handle the complete message (and reset cursor)
                            OnProcessMessage(new MessageBufferEventArgs(messageBuffer, cursor, MemoryAddress));
                            cursor = 0;

                            // if there are still bytes left, recursively start reading again
                            // otherwise we're done with that chunk)
                            if (Available > 0)
                                Read(Stream, Available);

                            // < end of buffer >
                        }
                        else
                        {
                            OnSplittedPacketFound(new SplittedMessageFoundEventArgs(
                                (int)baseMemoryAddress, processed + Available, processed, Available));

                            // copy at least all we got to messagebuffer
                            Stream.Read(messageBuffer, cursor, Available);
                            cursor += Available;
                            processed += Available;

                            // next time we jump to processing a body directly
                            readingHeader = false;
                        }
                    }
                    else
                    {
                        // Lengths don't match
                        OnMismatchMessageLengthFound(new MismatchMessageLengthFoundEventArgs(
                            len1, len2, new byte[0]));
                    }
                }
            }
            // ------------------------------------------------------------------------------------------------------------------
            else
            {
                IntPtr MemoryAddress = baseMemoryAddress;
                MemoryAddress -= cursor; 
                
                OnCompletingSplittedPacket(new CompletingSplittedMessageEventArgs(
                    (int)baseMemoryAddress, processed + Available, processed, (int)MemoryAddress, GameMessage.HEADERLENGTH + nextBodyLength));

                // rest of body this time ?
                int MessageLength = GameMessage.HEADERLENGTH + nextBodyLength;
                if (cursor + Available >= MessageLength)
                {                                     
                    // copy rest of body to messagebuffer
                    int missingbytes = MessageLength - cursor;
                    Stream.Read(messageBuffer, cursor, missingbytes);                   
                    processed += missingbytes;
                    cursor += missingbytes;
                    Available -= missingbytes;

                    // handle the complete message (and reset cursor)
                    OnProcessMessage(new MessageBufferEventArgs(messageBuffer, cursor, MemoryAddress));
                    cursor = 0;

                    // next time we process header again
                    readingHeader = true;

                    // if there are still bytes left, recursively start reading again
                    // otherwise end
                    if (Available > 0)
                        Read(Stream, Available);

                    // < end of buffer >
                }
                else
                {
                    // read what we can get of the messagebody
                    Stream.Read(messageBuffer, cursor, Available);
                    cursor += Available;

                    // message is split across a 2./3. buffer
                    OnSplittedPacketFound(new SplittedMessageFoundEventArgs(
                        (int)baseMemoryAddress, processed + Available, processed, cursor));
                    
                    // < end of buffer >
                }
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnSplittedPacketFound(SplittedMessageFoundEventArgs e)
        {
            if (SplittedMessageFound != null)
                SplittedMessageFound(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnCompletingSplittedPacket(CompletingSplittedMessageEventArgs e)
        {
            if (CompletingSplittedMessage != null)
                CompletingSplittedMessage(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnMismatchMessageLengthFound(MismatchMessageLengthFoundEventArgs e)
        {
            if (MismatchMessageLengthFound != null)
                MismatchMessageLengthFound(this, e);
        }
        #endregion
    }
}
