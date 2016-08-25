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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Accepts incoming TCP connections and enqueues the
    /// TcpClient instances into a threadsafe queue
    /// </summary>
    public class ConnectionListener
    {
        protected volatile bool isRunning;

        /// <summary>
        /// Internal workthread accepting tcp connections
        /// </summary>
        protected readonly Thread thread;
        
        /// <summary>
        /// Will bind to TCP port and listen for incoming connections
        /// </summary>
        protected readonly TcpListener listener;
        
        /// <summary>
        /// Contains the incoming connections which must be served
        /// </summary>
        public readonly LockingQueue<TcpClient> IncomingConnections = new LockingQueue<TcpClient>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Port"></param>
        public ConnectionListener(ushort Port)
        {
            // create listener
            listener = new TcpListener(IPAddress.IPv6Any, Port);
            
            // set ipv6 socket to dualstack so it can handle our IPv4 connections too
            //listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

            // mark running
            isRunning = true;

            // create and start workerthread
            thread = new Thread(ThreadProc);            
            thread.Start();
        }

        /// <summary>
        /// Stops listening for incoming connections
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }

        /// <summary>
        /// Executed by internal worker thread
        /// </summary>
        protected void ThreadProc()
        {
            // start listening for tcp connections        
            listener.Start();

            // loop
            while (isRunning)
            {
                // wait for connection request (will block)
                TcpClient tcpClient = listener.AcceptTcpClient();

                // enqueue new connection
                IncomingConnections.Enqueue(tcpClient);
            }

            // stop listening for tcp connections
            listener.Stop();
        }
    }
}
