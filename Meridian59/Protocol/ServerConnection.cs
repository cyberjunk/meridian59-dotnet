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
using System.Timers;
using System.Threading;
using System.Net.Sockets;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Enums;
using Meridian59.Protocol.Events;

namespace Meridian59.Protocol
{
    /// <summary>
    /// Provides a connection established from a client to a server.
    /// This class has its own workthread with an async communication queue pattern.
    /// </summary>
    public class ServerConnection : IDisposable
    {
        #region Constants
        /// <summary>
        /// ModuleName
        /// </summary>
        protected const string MODULENAME = "ServerConnection";

        /// <summary>
        /// Ping interval in milliseconds.
        /// </summary>
        protected const int PINGINTERVAL = 5000;

        /// <summary>
        /// Time in ms for listenthread to sleep
        /// </summary>
        protected const int SLEEPTIME = 2;

        /// <summary>
        /// Time in ms to sleep after call to disconnect to let netthread end
        /// </summary>
        protected const int DISCONNECTSLEEP = 200;
        #endregion

        #region Fields
        /// <summary>
        /// Hostname or IP of server
        /// </summary>
        protected string serverAddress;

        /// <summary>
        /// TCP port of connection
        /// </summary>
        protected ushort serverPort;

        /// <summary>
        /// If true will use IPv6
        /// </summary>
        protected bool useIPv6;

        /// <summary>
        /// Handles the TCP connection
        /// </summary>
        protected Socket socket;
        
        /// <summary>
        /// TCP stream between client and server
        /// </summary>
        protected NetworkStream tcpStream;
        
        /// <summary>
        /// Loop condition for listener thread
        /// </summary>
        protected volatile bool isRunning;

        /// <summary>
        /// Current latency 
        /// </summary>
        protected volatile int rtt;

        /// <summary>
        /// Current connectionstate
        /// </summary>
        protected volatile ConnectionState connectionState;

        /// <summary>
        /// 
        /// </summary>
		protected StringDictionary stringResources;

        /// <summary>
        /// Timer for regularly sending pings in Game ProtocolMode
        /// </summary>
        protected System.Timers.Timer timPing;

        /// <summary>
        /// Internal network-listener thread
        /// </summary>
        protected Thread workThread;

        /// <summary>
        /// The packetcontroller used for parsing.
        /// </summary>
        protected MessageControllerClient messageController;

        /// <summary>
        /// Saves the time the last ping was sent (for delta calculation)
        /// </summary>
        protected DateTime lastPingSent;

        #endregion

        #region Properties
        /// <summary>
        /// Current connectionstate
        /// </summary>
        public ConnectionState ConnectionState { get { return connectionState; } }

        /// <summary>
        /// Round-Trip-Time measured in milliseconds between last ping sent and its received pingreply
        /// </summary>
        public int RTT { get { return rtt; } }

        /// <summary>
        /// The tcp-listener thread will put fully parsed/typed packets on this queue
        /// you should regularly take off the packets from here and process them by your thread
        /// </summary>
        public LockingQueue<GameMessage> ReceiveQueue { get; protected set; }

        /// <summary>
        /// Insert packets to be sent in here.
        /// </summary>
        public LockingQueue<GameMessage> SendQueue { get; protected set; }
        
        /// <summary>
        /// Whether or not put fully sent packets back in a queue (debug)
        /// </summary>
        public bool IsOutgoingPacketLogEnabled { get; set; }
       
        /// <summary>
        /// If enabled, this packetqueue contains all outgoing packets you
        /// inserted in sendqueue and that were actually sent (for logging purposes with crc/len/pi)
        /// </summary>
        public LockingQueue<GameMessage> OutgoingPacketLog { get; protected set; }

        /// <summary>
        /// Contains exceptions raised by internal thread.
        /// </summary>
        public LockingQueue<Exception> ExceptionQueue { get; protected set; }

        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="StringResources">The StringResources to use.</param>
		public ServerConnection(StringDictionary StringResources)
        {           
            // init the queues (receive, send, logs, exceptions)
            ReceiveQueue = new LockingQueue<GameMessage>();
            SendQueue = new LockingQueue<GameMessage>();
            ExceptionQueue = new LockingQueue<Exception>();

            // the debugqueue to give back sent packets (with crc/...) if activated
            OutgoingPacketLog = new LockingQueue<GameMessage>();

            // save reference to string dictionary
            stringResources = StringResources;

            // setup the packetcontroller
            messageController = new MessageControllerClient(StringResources);

            // hook up listeners
            messageController.MessageAvailable += new GameMessageEventHandler(OnMessageControllerNewMessageAvailable);
            messageController.ServerSaveChanged += new ServerSaveChangedEventHandler(OnMessageControllerNewServerSave);
            messageController.HandlerError += new HandlerErrorEventHandler(OnMessageControllerHandlerError);
            messageController.ProtocolModeChanged += new EventHandler(OnMessageControllerProtocolModeChanged);
            
            // setup the ping timer
            timPing = new System.Timers.Timer();
            timPing.Enabled = false;
            timPing.Interval = PINGINTERVAL;
            timPing.Elapsed += new ElapsedEventHandler(OnPingTimerElapsed);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ServerConnection()
        {
            Dispose(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Connect this instance to a Meridian 59 Server.
        /// This will spawn the internal workthread and also
        /// disconnect and cleanup first, if already connected.
        /// </summary>
        /// <param name="ServerAddr">IP or DNS to connect to</param>
        /// <param name="Port">TCP-Port to connect to</param>
        /// <param name="UseIPv6">If true connection will be made using IPv6 instead of IPv4</param>
        public void Connect(string ServerAddr, ushort Port, bool UseIPv6 = false)
        {
            // make sure we're disconnected
            Disconnect();
         
            // save connection settings
            serverAddress = ServerAddr;
            serverPort = Port;
            useIPv6 = UseIPv6;

            // make log entry
            Logger.Log(MODULENAME, LogType.Info, "Connecting to " + serverAddress + ":" + serverPort.ToString());
            
            // Start workthread
            workThread = new Thread(new ThreadStart(ThreadProc));
            workThread.IsBackground = true;
            workThread.Start();
        }

        /// <summary>
        /// Close connection. This will block for some time to let the async
        /// network thread exit its loop.
        /// </summary>
        public void Disconnect()
        {
            if (isRunning || !(ConnectionState == ConnectionState.Offline))
            {
                // make log entry
                Logger.Log(MODULENAME, LogType.Info, "Disconnecting.");

                // mark disconnected
                connectionState = ConnectionState.Offline;

                // stop workthread and timer
                isRunning = false;
                timPing.Stop();

                // sleep to let networkthread catch isRunning=False
                Thread.Sleep(DISCONNECTSLEEP);
            }
        }   
    
        /// <summary>
        /// Sends a GameMessage instance to the connected server.
        /// </summary>
        /// <param name="Message">The GameMessage instance to be sent</param>
        /// <param name="Flush">Flush TCP buffer or not</param>
        protected void Send(GameMessage Message, bool Flush = true)
        {           
            // use MessageController to sign the message with valid CRC and SS
            messageController.SignMessage(Message);
            
            // serialize message to bytes
            byte[] MessageBytes = Message.Bytes;
            
            // write the message bytes to stream
            tcpStream.Write(MessageBytes, 0, MessageBytes.Length);
            
            // only flush if we should do so
            if (Flush)
                tcpStream.Flush();

            // track sending of UseCharacter message
            if ((MessageTypeGameMode)Message.PI == MessageTypeGameMode.UseCharacter)
            {
                // set state
                connectionState = ConnectionState.Playing;

                // log
                Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Playing");
            }

            // loop back game message for logging purposes
            if (IsOutgoingPacketLogEnabled)
                OutgoingPacketLog.Enqueue(Message);                       
        }

        /// <summary>
        /// Use this with SendBuffered to finally flush the sendbuffer
        /// </summary>
        protected void Flush()
        {
            if (tcpStream != null)
                tcpStream.Flush();             
        }

        /// <summary>
        /// The internal thread procedure, handling recv and send.
        /// </summary>
        protected void ThreadProc()
        {
            AddressFamily ipv = useIPv6 ? AddressFamily.InterNetworkV6 : 
                AddressFamily.InterNetwork;
            
            // cleanup old socket if any
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }

            // reset the packetcontroller
            messageController.Reset();

            // clean pending messages/exceptions from queues
            GameMessage message;
            Exception error;
            while (SendQueue.TryDequeue(out message)) ;
            while (ReceiveQueue.TryDequeue(out message)) ;
            while (ExceptionQueue.TryDequeue(out error)) ;

            // init a new Socket 
            socket = new Socket(ipv, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;

            // try connect to server
            try { socket.Connect(serverAddress, serverPort); }
            catch (Exception Error) { ExceptionQueue.Enqueue(Error); }

            // don't go on if no connection
            if (socket.Connected)
            {
                // initialize the socket stream
                tcpStream = new NetworkStream(socket);

                // mark running
                isRunning = true;

                // start thread loop
                // this can be broken by calling Disconnect()                
                while (isRunning)
                {
                    try
                    {
                        bool doSend = false;
                        
                        // processing pending messages to send
                        while (SendQueue.TryDequeue(out message))
                        {
                            Send(message, false);
                            doSend = true;
                        }

                        // call flush ourself here
                        // so we can send multiple messages above
                        if (doSend)
                            Flush();

                        // read
                        if (socket.Available > 0)
                            messageController.ReadRecv(tcpStream, socket.Available);

                        // avoid 100% cpu usage
                        Thread.Sleep(SLEEPTIME);
                    }
                    catch (Exception Error)
                    {
                        // log the exception
                        ExceptionQueue.Enqueue(Error);

                        // stop thread
                        isRunning = false;
                    }
                }
            }

            // cleanup
            if (socket != null)
            {
                socket.Close();             
            }
            
            // reset references
            socket = null;
            tcpStream = null;
            workThread = null;
        }

        /// <summary>
        /// The timer will send a required PingPacket every 5 seconds by using this function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnPingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // make sure to stop
            timPing.Stop();

            // sanity check cause this is async
            if (isRunning)
            {
                // enqueue a ping message for sending
                SendQueue.Enqueue(new PingMessage());

                // save time ping was sent
                lastPingSent = DateTime.Now;

                // start again
                timPing.Start();
            }
        }

        #endregion

        #region MessageController events
        /// <summary>
        /// Handler in case the MessageController throws an error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMessageControllerHandlerError(object sender, HandlerErrorEventArgs e)
        {
            string error =  
                e.ExceptionMessage + Environment.NewLine +
                "PacketDump:" + Environment.NewLine + 
                BitConverter.ToString(e.MessageDump) + Environment.NewLine;

            Logger.Log(MODULENAME, LogType.Error, error);
        }

        /// <summary>
        /// Handler in case the MessageController signals a new serversave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMessageControllerNewServerSave(object sender, ServerSaveChangedEventArgs e)
        {
            Logger.Log(MODULENAME, LogType.Info, 
                "ServerSaveChange, Old:" + e.OldServerSave + " New:" + e.NewServerSave);
        }

        /// <summary>
        /// Handler for protocolmode change in MessageController
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMessageControllerProtocolModeChanged(object sender, EventArgs e)
        {
            switch (messageController.Mode)
            {
                case ProtocolMode.Login:
                    timPing.Stop();
                    break;

                case ProtocolMode.Game:
                    timPing.Start();
                    break;
            }
        }

        /// <summary>
        /// Process new GameMessage from MessageController
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMessageControllerNewMessageAvailable(object sender, GameMessageEventArgs e)
        {
            // Internally process some of the messages  that are linked to the connection
            switch (messageController.Mode)
            {
                case ProtocolMode.Login:
                    HandleLoginModeMessage(e.Message);
                    break;

                case ProtocolMode.Game:
                    HandleGameModeMessage(e.Message);
                    break;
            }
               
            // Add the message to the received Message queue
            ReceiveQueue.Enqueue(e.Message);
        }

        #endregion

        #region MessageHandlers
        /// <summary>
        /// Internally handle some LoginModeMessages
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleLoginModeMessage(GameMessage Message)
        {
            switch ((MessageTypeLoginMode)Message.PI)
            {
                case MessageTypeLoginMode.GetLogin:
                    HandleGetLoginMessage((GetLoginMessage)Message);
                    break;

                case MessageTypeLoginMode.GetClient:
                    HandleGetClientMessage((GetClientMessage)Message);
                    break;

                case MessageTypeLoginMode.LoginOK:
                    HandleLoginOKMessage((LoginOKMessage)Message);
                    break;

                case MessageTypeLoginMode.Game:
                    HandleGameStateMessage((GameStateMessage)Message);
                    break;
            }
        }

        /// <summary>
        /// Internally handle some GameModeMessages
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleGameModeMessage(GameMessage Message)
        {
            switch ((MessageTypeGameMode)Message.PI)
            {
                case MessageTypeGameMode.EchoPing:
                    rtt = (DateTime.Now - lastPingSent).Milliseconds;
                    break;

                case MessageTypeGameMode.Wait:
                    timPing.Stop();
                    connectionState = ConnectionState.Waiting;
                    
                    Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Waiting");
                    break;

                case MessageTypeGameMode.Unwait:
                    timPing.Start();
                    connectionState = ConnectionState.Playing;

                    Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Playing");
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleGetLoginMessage(GetLoginMessage Message)
        {
            // set connection state
            connectionState = ConnectionState.Connected;

            // log
            Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Connected");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleGetClientMessage(GetClientMessage Message)
        {
            // set connection state
            connectionState = ConnectionState.Authenticated;

            // log
            Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Authenticated");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleLoginOKMessage(LoginOKMessage Message)
        {
            // set connection state
            connectionState = ConnectionState.Authenticated;

            // log
            Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Authenticated");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleGameStateMessage(GameStateMessage Message)
        {
            // set connection state
            connectionState = ConnectionState.Online;

            // log
            Logger.Log(MODULENAME, LogType.Info, "ConnectionState: Online");

            // start pinging
            timPing.Enabled = true;
        }
        #endregion

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (isRunning)
                isRunning = false;

            if (disposing)
            {
                if (timPing != null)
                {
                    timPing.Dispose();
                    timPing = null;
                }

                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
            }                
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
