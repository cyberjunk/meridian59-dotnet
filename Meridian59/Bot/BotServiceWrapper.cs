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
using System.ServiceProcess;
using System.Threading;

using Meridian59.Common;
using Meridian59.Files;
using Meridian59.Data;

namespace Meridian59.Bot
{
#if WINCLR && (X86 || X64)
    /// <summary>
    /// Wraps a BotClient or deriving client class instance
    /// into a Windows service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="X"></typeparam>
    public class BotServiceWrapper<T, R, D, K, X> : ServiceBase
        where T : GameTick, new()
        where R : ResourceManager, new()
        where D : DataController, new()
        where K : BotConfig, new()
        where X : BotClient<T, R, D, K>, new()
    {
        /// <summary>
        /// This thread will run the client
        /// </summary>
        protected Thread workThread;

        /// <summary>
        /// Flag for our recreate-client loop
        /// </summary>
        protected volatile bool isRunning;

        /// <summary>
        /// This client will be executed by the service
        /// </summary>
        protected X client;

        /// <summary>
        /// Executed when service starts.
        /// Do not lock here!
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            
            // create workthread which executes the client
            workThread = new Thread(new ThreadStart(ThreadProc));
            workThread.Start();
        }

        /// <summary>
        /// Executed when service is requested to stop
        /// </summary>
        protected override void OnStop()
        {
            // mark client as not running
            // this will end the loop started by .Start()
            if (client != null)
                client.IsRunning = false;

            // make sure we exit our own recreate client
            // in case of errors/exits threadloop
            isRunning = false;

            // base handler
            base.OnStop();
        }

        /// <summary>
        /// Executed by internal thread.
        /// This thread runs the client.
        /// </summary>
        protected void ThreadProc()
        {
            isRunning = true;

            uint exceptionCounter = 0;

            while (isRunning)
            {
                try
                {
                    // create client instance
                    client = new X();

                    // mark it as a service instance
                    client.IsService = true;

                    // start it (locks with a while loop)
                    client.Start();

                    // sleep 30s before restart after "normal exit"
                    Thread.Sleep(30000);
                }
                catch (Exception)
                {
                    exceptionCounter++;

                    // try stop the client if it's instanced
                    if (client != null)
                        client.IsRunning = false;

                    // sleep 3 seconds to possibly log shutdown stuff
                    Thread.Sleep(3000);

                    // make sure logger also ends
                    Logger.Stop();

                    // more than 1000 exceptions?
                    // shut down the service
                    // else wait to recreate client instance
                    if (exceptionCounter > 1000)
                    {
                        isRunning = false;
                        ExitCode = 1;

                        Stop();
                    }
                    else
                    {
                        // sleep 30s before retry
                        Thread.Sleep(30000);
                    }
                }
            }           
        }
    }
#endif
}
