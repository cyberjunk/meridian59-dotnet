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
using System.Text;
using System.Threading;
using Meridian59.Common.Enums;

namespace Meridian59.Common
{
    /// <summary>
    /// Provides asynchronous writing to logfiles.
    /// In case these logfiles are not writable, there will be no logging.
    /// </summary>
    public static class Logger
    {
        public const string MODULENAME  = "Logger";
        public const string LOGFILE     = "Meridian59.log";
        public const string LOGFILECHAT = "Chat.log";
        public const int SLEEPTIME      = 100;
        public const int COLSIZETIME    = 13;
        public const int COLSIZEMODULE  = 20;
        public const int COLSIZETYPE    = 8;
        public const int COLSIZEMESSAGE = 50;

        private static volatile bool isRunning;
        private static Thread workThread;
        private static StreamWriter logStream;
        private static StreamWriter logStreamChat;
        private static LockingQueue<LogMessage> inputQueue = new LockingQueue<LogMessage>();
        private static LockingQueue<string> inputQueueChat = new LockingQueue<string>();

        /// <summary>
        /// Starts the internal workthread for the logwriting.
        /// Does not do anything if already started.
        /// </summary>
        public static void Start()
        {
            if (!isRunning)
            {
                // mark running
                isRunning = true;

                // start workthread
                workThread = new Thread(new ThreadStart(ThreadProc));
                workThread.IsBackground = true;
                workThread.Start();
            }
        }

        /// <summary>
        /// Async marks the internal logthread for stopping.
        /// May take a bit until it finally ends.
        /// </summary>
        public static void Stop()
        {
            isRunning = false;
        }

        /// <summary>
        /// Enqueues a log message to be written to main logfile.
        /// Only if the Logger is started.
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="Type"></param>
        /// <param name="Message"></param>
        public static void Log(string Module, LogType Type, string Message)
        {
            if (isRunning && inputQueue != null)
                inputQueue.Enqueue(new LogMessage(Module, Type, Message));
        }

        /// <summary>
        /// Enqueues a log message to be written to main chat logfile.
        /// Only if the Logger is started.
        /// </summary>
        /// <param name="Text"></param>
        public static void LogChat(string Text)
        {
            if (isRunning && inputQueueChat != null)
                inputQueueChat.Enqueue(String.Copy(Text));
        }

        /// <summary>
        /// Internal thread procedure
        /// </summary>
        private static void ThreadProc()
        {
            // try to initiate the logfiles
            // fails for second startup due to writelock
            // so anything below the try catch has to nullcheck the streams!
            try
            {

                // create textlog stream
                logStream = new StreamWriter(LOGFILE, false, Encoding.Default);
                logStream.AutoFlush = true;

                // create textlog stream for chat
                logStreamChat = new StreamWriter(LOGFILECHAT, false, Encoding.Default);
                logStreamChat.AutoFlush = true;
            }
            catch (Exception) { }

            // (possibly) write startup headers
            WriteHeader();
            WriteHeaderChat();

            // (possibly) write logger start here
            Log(MODULENAME, LogType.Info, "Starting logger.");

            // start processing loop
            while (isRunning)
            {
                LogMessage item;
                string text;

                // process all pending items
                while (inputQueue.TryDequeue(out item))
                    WriteLog(item);

                // process all pending items for chat
                while (inputQueueChat.TryDequeue(out text))
                    WriteLogChat(text);

                // sleep
                Thread.Sleep(SLEEPTIME);
            }

            if (logStream != null)
            {
                logStream.Close();
                logStream = null;
            }

            if (logStreamChat != null)
            {
                logStreamChat.Close();
                logStreamChat = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        private static void WriteLog(LogMessage Item)
        {
            if (logStream != null)
            {
                string strTime = DateTime.Now.ToLongTimeString();
                string strModule = Item.Module.ToString();
                string strType = Item.Type.ToString();

                if (strTime.Length > COLSIZETIME - 1)
                    strTime = strTime.Substring(0, COLSIZETIME - 1 - 3) + "...";

                if (strModule.Length > COLSIZEMODULE - 1)
                    strModule = strModule.Substring(0, COLSIZEMODULE - 1 - 3) + "...";
                
                if (strType.Length > COLSIZETYPE - 1)
                    strType = strType.Substring(0, COLSIZETYPE - 1 - 3) + "...";

                strTime = strTime.PadRight(COLSIZETIME);
                strModule = strModule.PadRight(COLSIZEMODULE);
                strType = strType.PadRight(COLSIZETYPE);

                logStream.WriteLine(strTime + strModule + strType + Item.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        private static void WriteLogChat(string Text)
        {
            if (logStreamChat != null)            
                logStreamChat.WriteLine(Text);            
        }

        /// <summary>
        /// 
        /// </summary>
        private static void WriteHeader()
        {
            if (logStream != null)
            {               
                string s =
                    "+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+" + Environment.NewLine +
                    "+                                      Meridian 59                                        +" + Environment.NewLine +
                    "+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+" + Environment.NewLine;

                logStream.Write(s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void WriteHeaderChat()
        {
            if (logStreamChat != null)
            {
                string s =
                    "+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+" + Environment.NewLine +
                    "+                                   Meridian 59 - Chatlog                                 +" + Environment.NewLine +
                    "+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+" + Environment.NewLine;

                logStreamChat.Write(s);
            }
        }
    }
}
