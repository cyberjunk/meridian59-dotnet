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
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Events;
using Meridian59.Protocol.Enums;
using Meridian59.Common;

namespace Meridian59.Protocol
{
    /// <summary>
    /// MessageController for a client implementation
    /// </summary>
    public class MessageControllerClient : MessageController
    {
        /// <summary>
        /// Reader for incoming data
        /// </summary>
        protected MessageParser recvReader;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StringResources"></param>
        public MessageControllerClient(StringDictionary StringResources)
            : base(StringResources) { }

        /// <summary>
        /// Reset to initial state
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            // detach events from old instance
            if (recvReader != null)
            {
                recvReader.MessageAvailable -= new MessageBufferEventHandler(OnRecvReaderProcessMessage);
                recvReader.MismatchMessageLengthFound -= new MismatchMessageLengthFoundEventHandler(OnRecvReaderMismatchMessageLengthFound);
                recvReader.SplittedMessageFound -= new SplittedMessageFoundEventHandler(OnRecvReaderSplittedPacketFound);
                recvReader.CompletingSplittedMessage -= new CompletingSplittedMessageEventHandler(OnRecvReaderCompletingSplittedPacket);
            }

            // create new instance
            recvReader = new MessageParser();
            recvReader.MessageAvailable += new MessageBufferEventHandler(OnRecvReaderProcessMessage);
            recvReader.MismatchMessageLengthFound += new MismatchMessageLengthFoundEventHandler(OnRecvReaderMismatchMessageLengthFound);
            recvReader.SplittedMessageFound += new SplittedMessageFoundEventHandler(OnRecvReaderSplittedPacketFound);
            recvReader.CompletingSplittedMessage += new CompletingSplittedMessageEventHandler(OnRecvReaderCompletingSplittedPacket);
        }

        /// <summary>
        /// Process available data in a stream
        /// </summary>
        /// <param name="Stream"></param>
        /// <param name="Available">How many bytes can be read from Stream</param>
        public void ReadRecv(Stream Stream, int Available)
        {
            recvReader.Read(Stream, Available);
        }

        /// <summary>
        /// Handle a message from the messagebuffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRecvReaderProcessMessage(object sender, MessageBufferEventArgs e)
        {
            if (e.Length > GameMessage.HEADERLENGTH)
            {
                // save encoded type
                byte encodedType = e.MessageBuffer[GameMessage.HEADERLENGTH];

                // decode type
                e.MessageBuffer[GameMessage.HEADERLENGTH] = PIDecoder.Decode(encodedType);

                try
                {
                    // mark as incoming message
                    e.Direction = MessageDirection.ServerToClient;

                    // parse the message to a typed instance
                    GameMessage typedMessage = ExtractMessage(e);
                    
                    // set encoded pi and memoryaddres
                    typedMessage.EncryptedPI = encodedType;
                    typedMessage.MemoryStartAddress = e.MemoryAddress;

                    // examine serversave value
                    CheckServerSave(typedMessage);

                    // trigger event for this new message
                    OnNewMessageAvailable(new GameMessageEventArgs(typedMessage));
                }
                catch (Exception Error)
                {
                    byte[] dump = new byte[e.Length];
                    Array.Copy(e.MessageBuffer, 0, dump, 0, e.Length);
                    OnHandlerError(new HandlerErrorEventArgs(dump, Error.Message));
                }
            }
            else
            {
                OnEmptyPacketFound(new EmptyMessageFoundEventArgs());
            }
        }
       
        /// <summary>
        /// Eventhandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRecvReaderMismatchMessageLengthFound(object sender, MismatchMessageLengthFoundEventArgs e)
        {
            // raise our own event
            OnMismatchPacketLENFound(e);
        }

        /// <summary>
        /// Eventhandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRecvReaderCompletingSplittedPacket(object sender, CompletingSplittedMessageEventArgs e)
        {
            // raise our own event
            OnCompletingSplittedPacket(e);
        }

        /// <summary>
        /// Eventhandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRecvReaderSplittedPacketFound(object sender, SplittedMessageFoundEventArgs e)
        {
            // raise our own event
            OnSplittedPacketFound(e);
        }
    }
}
