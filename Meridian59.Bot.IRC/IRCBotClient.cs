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
using System.ComponentModel;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Data;
using Meridian59.Data.Models;
using Meridian59.Files;
using IrcDotNet;

namespace Meridian59.Bot.IRC
{
    /// <summary>
    /// A client which acts as an IRC bridge
    /// </summary>
    public class IRCBotClient : BotClient<GameTick, ResourceManager, DataController, IRCBotConfig>
    {
        #region Constants
        protected const string LOG_IRCCONNECTERROR      = "Could not connect to IRC server.";
        protected const string LOG_IRCCONNECTED         = "Connected to IRC server.";
        protected const string LOG_IRCDISCONNECTERROR   = "Lost connection to IRC server.";
        #endregion

        /// <summary>
        /// The IRC client from the lib
        /// </summary>
        public IrcClient IrcClient { get; protected set; }
        
        /// <summary>
        /// Once successfully connected and joined,
        /// this will store a reference to the channel the bot is in.
        /// </summary>
        public IrcChannel IrcChannel { get; protected set; }

        /// <summary>
        /// ChatCommands received from IRC channel to be forwarded to M59 will be inserted here by the IRC lib
        /// thread and read by the bot mainthread later.
        /// </summary>
        public LockingQueue<string> ChatCommandQueue { get; protected set; }

        /// <summary>
        /// AdminCommands received from IRC private message to be forwarded to M59 will be inserted here by the IRC lib
        /// thread and read by the bot mainthread later.
        /// </summary>
        public LockingQueue<string> AdminCommandQueue { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public IRCBotClient()
            : base()
        {
                  
        }

        /// <summary>
        /// Overwritten Init method.
        /// This method connects to the M59 server.
        /// So we connect to IRC here as well.
        /// </summary>
        public override void Init()
        {
            // base handler connecting to m59 server
            base.Init();

            // create IRC command queues
            ChatCommandQueue = new LockingQueue<string>();
            AdminCommandQueue = new LockingQueue<string>();

            // create an IRC client instance
            IrcClient = new IrcClient();
            IrcClient.FloodPreventer = new FloodPreventer(Config.MaxBurst, Config.Refill);
            
            // hook up IRC client event handlers
            // beware! these are executed by the internal workthread
            // of the library.          
            IrcClient.Connected += OnIrcClientConnected;
            IrcClient.ConnectFailed += OnIrcClientConnectFailed;
            IrcClient.Disconnected += OnIrcClientDisconnected;
            IrcClient.Registered += OnIrcClientRegistered;
            IrcClient.ProtocolError += OnIrcClientProtocolError;

            // build our IRC connection info
            IrcUserRegistrationInfo regInfo = new IrcUserRegistrationInfo();
            regInfo.UserName = Config.NickName;
            regInfo.RealName = Config.NickName;
            regInfo.NickName = Config.NickName;
            
            // if password is set
            if (!String.Equals(Config.IRCPassword, String.Empty))
                regInfo.Password = Config.IRCPassword;

            // log IRC connecting
            Log("SYS", "Connecting IRC to " + Config.IRCServer + ":" + Config.IRCPort);

            // connect the lib internally (this is async)
            IrcClient.Connect(Config.IRCServer, Config.IRCPort, false, regInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Cleanup()
        {
            // cleanup IRC client
            if (IrcClient != null)
            {
                // detach event handlers
                IrcClient.Connected -= OnIrcClientConnected;
                IrcClient.ConnectFailed -= OnIrcClientConnectFailed;
                IrcClient.Disconnected -= OnIrcClientDisconnected;
                IrcClient.Registered -= OnIrcClientRegistered;
                IrcClient.ProtocolError -= OnIrcClientProtocolError;
               
                IrcClient.Disconnect();
                IrcClient.Dispose();
                IrcClient = null;

                IrcChannel = null;
            }

            base.Cleanup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandlePlayerMessage(PlayerMessage Message)
        {
            base.HandlePlayerMessage(Message);

            // make sure we're resting to have mana for broadcasts
            SendUserCommandRest();  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleSaidMessage(SaidMessage Message)
        {
            base.HandleSaidMessage(Message);

            // log chat from players to IRC
            if (IrcClient.IsRegistered && IrcChannel != null)
            {
                // build a str to log
                // this has a prefix (e.g. "103: ") and a m59 message
                string chatstr = 
                    IRCChatStyle.GetPrefixString(Config.ChatPrefix) + 
                    IRCChatStyle.CreateIRCMessageFromChatMessage(Message.Message);

                // try to log the chatmessage to IRC
                IrcClient.LocalUser.SendMessage(IrcChannel, chatstr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleMessageMessage(MessageMessage Message)
        {
            base.HandleMessageMessage(Message);

            string s = Message.Message.FullString;
            const string prefix = "[###]";
            const string ignore1 = "Your safety is now";
            
            // starts with prefix and does not contain ignores
            if (s.IndexOf(prefix) == 0 && 
                s.IndexOf(ignore1) == -1 &&
                IrcClient.IsRegistered && 
                IrcChannel != null)
            {
                // build a str to log
                // this has a prefix (e.g. "103: ") and a m59 message
                string chatstr =
                    IRCChatStyle.GetPrefixString(Config.ChatPrefix) +
                    IRCChatStyle.CreateIRCMessageFromChatMessage(Message.Message);

                // try to log the chatmessage to IRC
                IrcClient.LocalUser.SendMessage(IrcChannel, chatstr);             
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleAdminMessage(AdminMessage Message)
        {
            base.HandleAdminMessage(Message);

            // no proper message text?
            if (Message.Message == null || String.Equals(Message.Message, String.Empty))
                return;

            // check if irc is available
            if (IrcClient.IsRegistered && IrcChannel != null)              
            {
                // IRC does not allow \r \n \0 
                // so we remove \0 and remove \r\n by splitting up into lines               
                string[] lines = SplitLinebreaks(Message.Message.Replace("\0", String.Empty));

                // forward line by line
                foreach (string line in lines)
                {
                    // forward this line to each admin
                    foreach (string admin in Config.Admins)
                    {
                        // try get channeluser by name
                        IrcChannelUser usr = GetChannelUser(admin);

                        // online? send!
                        if (usr != null)
                            IrcClient.LocalUser.SendMessage(admin, line);
                    }
                }
            }
        }

        /// <summary>
        /// Main handler for commands received from M59.
        /// Not used in IRC bot.
        /// </summary>
        /// <param name="PartnerID"></param>
        /// <param name="Words">First element is command name</param>
        protected override void ProcessCommand(uint PartnerID, string[] Words)
        {         
        }
        
        /// <summary>
        /// Overrides Update from BaseClient to trigger advertising in intervals.
        /// </summary>
        public override void Update()
        {
            base.Update();

            string command;

            // execute the chatcommands - this is all which work in the 3d client also
            // like tell, say, broadcast - but also "dm", "getplayer" etc.
            while(ChatCommandQueue.TryDequeue(out command))            
                ExecChatCommand(command);

            // these are admin textcommands without a prefix, just plain was written in adminconsole
            while (AdminCommandQueue.TryDequeue(out command))
                SendReqAdminMessage(command);           
        }

        /// <summary>
        /// Returns a ChannelUser object if the name was found
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        protected IrcChannelUser GetChannelUser(string Username)
        {
            // check
            if (IrcChannel == null)
                return null;

            // lookup sender from channel users
            foreach (IrcChannelUser usr in IrcChannel.Users)
                if (String.Equals(usr.User.NickName, Username))                
                    return usr;
            
            // default
            return null;
        }

        /// <summary>
        /// Utility: IRC does not allow line breaks - 
        /// so we have to split up into lines first.
        /// </summary>
        /// <returns></returns>
        public static string[] SplitLinebreaks(string Text)
        {            
            // replace \r\n linebreaks (adminconsole) with \n
            string s = Text.Replace("\r\n", "\n");

            // remove single \r
            s = s.Replace("\r", String.Empty);

            // now split into lines by \n
            return s.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        #region IRC client events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientDisconnected(object sender, EventArgs e)
        {
            // close connection and exit  
            if (ServerConnection != null)
                ServerConnection.Disconnect();
            
            IsRunning = false;
            IrcChannel = null;

            Log("ERROR", LOG_IRCDISCONNECTERROR);
        }

        /// <summary>
        /// Executed when IRC client got a new connection.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientConnected(object sender, EventArgs e)
        {
            Log("SYS", LOG_IRCCONNECTED);
        }

        /// <summary>
        /// Executed when IRC client registered the nickname.
        /// You can't do anything before this.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientRegistered(object sender, EventArgs e)
        {
            IrcClient.LocalUser.JoinedChannel += OnLocalUserJoinedChannel;
            IrcClient.LocalUser.MessageReceived += OnLocalUserMessageReceived;
            
            // join the IRC channel
            IrcClient.Channels.Join(Config.Channel);
        }

        /// <summary>
        /// Executed when IRC client connect failed.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientConnectFailed(object sender, IrcErrorEventArgs e)
        {
            // close connection and exit         
            ServerConnection.Disconnect();
            IsRunning = false;
            IrcChannel = null;

            Log("ERROR", LOG_IRCCONNECTERROR);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientProtocolError(object sender, IrcProtocolErrorEventArgs e)
        {
        }

        /// <summary>
        /// Executed when IRC client joined the channel.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLocalUserJoinedChannel(object sender, IrcChannelEventArgs e)
        {
            // save reference to channel
            IrcChannel = e.Channel;

            // hook up new message listener
            e.Channel.MessageReceived += OnMessageReceived;

            string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (Config.SelectedConnectionInfo != null)
            {
                // build the welcome-string
                string welcomestr =
                    "Meridian 59 IRC BOT (" + ver + ") - Player " + Config.SelectedConnectionInfo.Character + " on host " + 
                    Config.SelectedConnectionInfo.Host + ":" + Config.SelectedConnectionInfo.Port;

                // try to log the chatmessage to IRC
                IrcClient.LocalUser.SendMessage(IrcChannel, welcomestr);
            }
        }

        /// <summary>
        /// Invoked when a channel message was received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMessageReceived(object sender, IrcMessageEventArgs e)
        {
            // ignore messages without @prefix start or which are only @prefix + @ + space length
            if (e.Text == null || e.Text.IndexOf("@" + Config.ChatPrefix) != 0 || e.Text.Length <= Config.ChatPrefix.Length + 1 + 1)
                return;

            // try get channeluser by name
            IrcChannelUser usr = GetChannelUser(e.Source.Name);

            if (usr == null)
                return;

            // only allow operators
            if (!usr.Modes.Contains('o'))
            {
                // respond user he can't use this feature
                IrcClient.LocalUser.SendMessage(
                    IrcChannel,
                    e.Source.Name + " you can't use this feature. Only operators allowed.");

                return;
            }
           
            // now remove the @103 start
            string s = e.Text.Substring(Config.ChatPrefix.Length + 1 + 1);
            
            // used delimiter
            const char delimiter = ' ';
            
            // split up into words
            string[] words = s.Split(delimiter);

            if (words.Length > 0)
            {
                switch(words[0])
                {
                    case ChatCommandBroadcast.KEY1:
                    case ChatCommandBroadcast.KEY2:

                        // keep first word
                        s = String.Join(delimiter.ToString(), words, 0, 1);

                        // insert banner + name
                        s += delimiter + Config.Banner;
                        s += e.Source.Name + ": ~n";

                        // add rest
                        s += String.Join(delimiter.ToString(), words, 1, words.Length - 1);
                        break;

                    case ChatCommandTell.KEY1:
                    case ChatCommandTell.KEY2:

                        // keep first two word
                        s = String.Join(delimiter.ToString(), words, 0, 2);

                        // insert banner + name
                        s += delimiter + Config.Banner;
                        s += e.Source.Name + ": ~n";

                        // add rest
                        s += String.Join(delimiter.ToString(), words, 2, words.Length - 2);
                        break;
                }
            }

            // enqueue it for execution
            ChatCommandQueue.Enqueue(s);        
        }

        /// <summary>
        /// Invoked when a private message was received.
        /// Private messages = admin console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLocalUserMessageReceived(object sender, IrcMessageEventArgs e)
        {            
            // only allow ADMINS from config
            if (Config.Admins.Contains(e.Source.Name))
            {
                // invalid
                if (e.Text == null || e.Text == String.Empty)
                    return;

                // get first space in text
                int firstspace = e.Text.IndexOf(' ');
                    
                // either use first part up to first space or full text if no space
                string comtype = 
                    (firstspace > 0) ? e.Text.Substring(0, firstspace) : comtype = e.Text;

                // check if commandtype is allowed
                if (Config.AdminCommands.Contains(comtype))
                {
                    // enqueue it for execution
                    AdminCommandQueue.Enqueue(e.Text);
                }
                else
                {
                    // respond admin he can't use this admincommand
                    IrcClient.LocalUser.SendMessage(
                        e.Source.Name,
                        e.Source.Name + " you can't use the admin command '" + comtype + "'");
                }                              
            }
            else
            {
                // respond user he can't use this feature
                IrcClient.LocalUser.SendMessage(
                    e.Source.Name,
                    e.Source.Name + " you can't use the admin console - Only admins allowed.");
            }
        }

        #endregion       
    }
}
