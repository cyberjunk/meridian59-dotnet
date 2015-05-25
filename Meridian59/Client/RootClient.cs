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
using System.Threading;

using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Data;
using Meridian59.Drawing2D;
using Meridian59.Files;
using Meridian59.Files.ROO;
using Meridian59.Protocol.GameMessages;

namespace Meridian59.Client
{  
    /// <summary>
    /// The root class of the client-class hierarchy.
    /// Just a very basic skeleton witout a real server-connection.
    /// </summary>
    /// <remarks>
    /// The generics are supposed to allow you to use the abstract
    /// client class-hierarchy, but plug in your own/modified, deriving module-implementations.
    /// </remarks>
    /// <typeparam name="T">Type of GameTick or deriving class</typeparam>
    /// <typeparam name="R">Type of ResourceManager or deriving class</typeparam>
    /// <typeparam name="D">Type of DataController or deriving class</typeparam>
    /// <typeparam name="C">Type of Config or deriving class</typeparam>
    public abstract class RootClient<T, R, D, C> 
        where T:GameTick, new()
        where R:ResourceManager, new()
        where D:DataController, new()
        where C:Config, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public const string MODULENAME = "Client";
        
        /// <summary>
        /// 
        /// </summary>
        protected const string CHARDLL = "char.dll";
        
        /// <summary>
        /// Sleep this long at the end of Tick()
        /// </summary>
        protected static int SLEEPTIME = 0;

        /// <summary>
        /// If this turns false the application quits
        /// </summary>
        public volatile bool IsRunning;

        /// <summary>
        /// The mainthread that created the client instance
        /// </summary>
        public Thread MainThread { get; protected set; }

        /// <summary>
        /// Current GameTick data
        /// </summary>
        public T GameTick { get; protected set; }

        /// <summary>
        /// Legacy ResourceManager
        /// </summary>
        public R ResourceManager { get; protected set; }

        /// <summary>
        /// DataController
        /// </summary>
        public D Data { get; protected set; }

        /// <summary>
        /// Configuration
        /// </summary>
        public C Config { get; protected set; }

        /// <summary>
        /// Shortcut to roomdata in DataController
        /// </summary>
        public RooFile CurrentRoom { get { return Data.RoomInformation.ResourceRoom; } }

        /// <summary>
        /// An outfit helper
        /// </summary>
        public Outfitter Outfitter { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RootClient()
        {
            // startup async logging
            Logger.Start();

            // log client startup
            Logger.Log(MODULENAME, LogType.Info, "Initializing client");

            // save reference to mainthread
            MainThread = Thread.CurrentThread;

            // init tick info
            GameTick = new T();

            // check if system supports high resolution ticks (at least 1ms precision)
            if (!GameTick.IsHighResolution)
                Logger.Log(MODULENAME, LogType.Warning, "System does not support high resolution ticks.");
           
#if DRAWING
            // Initialize GDI variants of colorpalettes if System.Drawing available
            PalettesGDI.Initialize();
#endif

            // Initialize legacy resources (bgf, roo, ...)
            ResourceManager = new R();

            // Initialize DataController
            Data = new D();

            // read in config
            Config = new C();

            // Initialize Outfitter
            Outfitter = new Outfitter(Data.InventoryObjects);
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="AutoTick">Set this to false to call Tick() manually and not lock the thread.</param>
        public void Start(bool AutoTick = true)
        {
            // log client startup
            Logger.Log(MODULENAME, LogType.Info, "Starting client");

            // set running flag
            IsRunning = true;

            // custom initialisation code
            Init();

            // sleep a ms to get an initial tickspan
            Thread.Sleep(1);

            // check whether to lock this thread
            // = AutoTick
            if (AutoTick)
            {
                // start mainthread loop
                while (IsRunning)
                {
                    Tick();
                }

                // cleanup
                Cleanup();
            }
        }

        /// <summary>
        /// Process a game tick.
        /// Call this yourself only if you had set AutoTick to false
        /// when executed Start()
        /// </summary>
        public void Tick()
        {
            // init new gametick
            GameTick.Tick();

            // process tick
            Update();

            //if (GameTick.Span > 33)
            //    Logger.Log(MODULENAME, LogType.Warning, "Tick span was " + GameTick.Span.ToString() + " ms.");

            // sleep
            Thread.Sleep(SLEEPTIME);
        }

        /// <summary>
        /// Overwrite with additional startup code. This is not part of the object construction.
        /// It will be called by Start() before entering the application loop.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Implement this with your code for each tick/update.
        /// </summary>
        public abstract void Update();
        
        /// <summary>
        /// Cleanup, runs after loop exits
        /// </summary>
        protected virtual void Cleanup()
        {
            Logger.Stop();
        }

        /// <summary>
        /// Top level handler for a new GameMessage from server
        /// </summary>
        /// <param name="Message"></param>
        protected virtual void HandleGameMessage(GameMessage Message)
        {
            /* First determine the type, then call the sub-handlers
             * Some few messages don't have typed classes yet,
             * so they appear as GenericGameMessage
             */

            if (!Message.HasEmptyBody)
            {
                if (Message is LoginModeMessage)
                {
                    // always handle in datalayer first to have updated info
                    Data.HandleIncomingLoginModeMessage(Message);

                    // own handlers
                    HandleLoginModeMessage((LoginModeMessage)Message);
                }
                else if (Message is GameModeMessage)
                {
                    // always handle in datalayer first to have updated info
                    Data.HandleIncomingGameModeMessage(Message);

                    // let roomfile handle message for changes
                    if (CurrentRoom != null)
                        CurrentRoom.HandleGameModeMessage((GameModeMessage)Message);

                    // own handlers
                    HandleGameModeMessage((GameModeMessage)Message);
                }

                // GenericGameMessage
                else
                {
                    // just for logging purposes
                    Data.HandleIncomingGameModeMessage(Message);
                }
            }
        }

        /// <summary>
        /// Will be executed for any new LoginMode message from the server
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleLoginModeMessage(LoginModeMessage Message);

        /// <summary>
        /// Will be executed for any new GameMode message from the server
        /// </summary>
        /// <param name="Message"></param>
        protected abstract void HandleGameModeMessage(GameModeMessage Message);

        /// <summary>
        /// Sends a GameMessage
        /// </summary>
        /// <param name="Message"></param>
        public abstract void SendGameMessage(GameMessage Message);
    }
}
