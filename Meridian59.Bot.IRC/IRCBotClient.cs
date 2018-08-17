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
using System.Collections.Generic;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Data;
using Meridian59.Data.Models;
using Meridian59.Files;
using ChatSharp;
using ChatSharp.Events;
using System.Text.RegularExpressions;

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
        /// Flood protection for sending IRC messages.
        /// </summary>
        public IRCQueryQueue IRCSendQueue { get; protected set; }

        /// <summary>
        /// WhoX queries to be sent.
        /// </summary>
        public WhoXQueryQueue WhoXQueryQueue { get; protected set; }

        public Dictionary<string, bool> UserRegistration { get; protected set; }

        public bool DisplayMessages { get; set; }

        public List<string> RecentAdmins { get; protected set; }

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
            IRCSendQueue = new IRCQueryQueue();
            WhoXQueryQueue = new WhoXQueryQueue();

            // Whether bot echoes to IRC or not.
            DisplayMessages = true;

            // Create list for keeping track of user registration.
            UserRegistration = new Dictionary<string, bool>();

            // Init list of recent admins to send a command.
            RecentAdmins = new List<string>();

            // create an IRC client instance, with connection info
            IrcClient = new IrcClient(Config.IRCServer,
                new IrcUser(Config.NickName, Config.NickName, Config.IRCPassword, Config.NickName));

            // TODO: does ChatSharp use something like this?
            //IrcClient.FloodPreventer = new FloodPreventer(Config.MaxBurst, Config.Refill);
            
            // hook up IRC client event handlers
            // beware! these are executed by the internal workthread
            // of the library.          
            IrcClient.ConnectionComplete += OnIrcClientConnected;

            // ChatSharp doesn't have fine-grained error handling
            // to my knowledge, likely we have to disconnect on any
            // network error.
            IrcClient.NetworkError += OnIrcClientDisconnected;

            IrcClient.WhoxReceived += OnWhoXReplyReceived;

            // log IRC connecting
            Log("SYS", "Connecting IRC to " + Config.IRCServer + ":" + Config.IRCPort);

            // connect the lib internally (this is async)
            IrcClient.ConnectAsync();
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
                IrcClient.ConnectionComplete -= OnIrcClientConnected;
                IrcClient.NetworkError -= OnIrcClientDisconnected;
                IrcClient.WhoxReceived -= OnWhoXReplyReceived;
                IrcClient.UserJoinedChannel -= OnUserJoined;
                IrcClient.UserPartedChannel -= OnUserLeft;
                IrcClient.ChannelListRecieved -= OnUsersListReceived;
                IrcClient.ChannelMessageRecieved -= OnMessageReceived;
                IrcClient.UserMessageRecieved -= OnLocalUserMessageReceived;

                IrcClient.Quit();
                IrcClient = null;

                IrcChannel = null;
            }

            RecentAdmins.Clear();

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
        /// Sends Message to IrcChannel, splitting if necessary to avoid
        /// IRC character limit. Uses a conservative split limit (280)
        /// due to color codes seeming to shorten the max length from 450
        /// on EsperNet.
        /// </summary>
        /// <param name="Message"></param>
        private void SendSplitIRCChannelServerString(ServerString Message)
        {
            // Must have somewhere to send.
            if (IrcChannel == null)
                return;

            // build a str to log
            // this has a prefix (e.g. "103: ") and a m59 message
            string chatstr =
                IRCChatStyle.GetPrefixString(Config.ChatPrefix) +
                IRCChatStyle.CreateIRCMessageFromChatMessage(Message);

            // try to log the chatmessage to IRC
            // Short path
            if (chatstr.Length < 280)
            {
                SendIRCChannelMessage(chatstr);
                return;
            }

            try
            {
                string lastStyle = string.Empty;
                int count = 0;
                while (chatstr.Length > 0 && count++ < 8)
                {
                    string substr = chatstr.Truncate(IRCChatStyle.GetGoodTruncateIndex(chatstr, 280));
                    SendIRCChannelMessage(lastStyle + substr);
                    chatstr = chatstr.Substring(substr.Length);
                    lastStyle = IRCChatStyle.GetLastChatStyleString(substr);
                }
            }
            catch (Exception e)
            {
                Log("ERROR", "Failed string splitting: " + e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleSaidMessage(SaidMessage Message)
        {
            base.HandleSaidMessage(Message);

            // log chat from players to IRC
            if (DisplayMessages)
                SendSplitIRCChannelServerString(Message.Message);
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
            if (DisplayMessages &&
                s.IndexOf(prefix) == 0 && 
                s.IndexOf(ignore1) == -1)
            {
                SendSplitIRCChannelServerString(Message.Message);
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
            if (IrcChannel != null)
            {
                // IRC does not allow \r \n \0 
                // so we remove \0 and remove \r\n by splitting up into lines               
                string[] lines = Util.SplitLinebreaks(Message.Message.Replace("\0", String.Empty));

                // Only want to send to any admins who've sent a command since last GC.
                // Other admins don't need to know about GC.
                List<string> recipients;
                bool clearRecent = false;
                if (Util.IsGarbageCollectMessage(lines[0]))
                {
                    recipients = RecentAdmins;
                    clearRecent = true;
                }
                else
                {
                    recipients = Config.Admins;
                }

                // forward line by line
                foreach (string line in lines)
                {
                    // forward this line to each admin
                    foreach (string admin in recipients)
                    {
                        // try get channeluser by name
                        IrcUser usr = GetChannelUser(admin);

                        // online? send!
                        if (usr != null)
                            SendIRCMessage(admin, line);
                    }
                }
                if (clearRecent)
                {
                    RecentAdmins.Clear();
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

            // Try send an available WhoXQuery.
            if (WhoXQueryQueue.TryDequeue(out string command))
                SendWhoXQuery(command);

            if (IRCSendQueue.TryDequeue(out IrcMessage ircMessage))
                IrcClient.SendMessage(ircMessage.Message, ircMessage.Name);

            // execute the chatcommands - this is all which work in the 3d client also
            // like tell, say, broadcast - but also "dm", "getplayer" etc.
            while (ChatCommandQueue.TryDequeue(out command))            
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
        protected IrcUser GetChannelUser(string Username)
        {
            // check
            if (IrcChannel == null)
                return null;

            // lookup sender from channel users
            foreach (IrcUser usr in IrcChannel.Users)
                if (String.Equals(usr.Nick, Username))
                    return usr;
            
            // default
            return null;
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
        /// SASL registration - can join channel on connection.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientConnected(object sender, EventArgs e)
        {
            Log("SYS", LOG_IRCCONNECTED);

            IrcClient.UserJoinedChannel += OnUserJoined;
            IrcClient.UserPartedChannel += OnUserLeft;
            IrcClient.ChannelListRecieved += OnUsersListReceived;
            IrcClient.ChannelMessageRecieved += OnMessageReceived;

            // TODO
            // Should UserMessageRecieved be PrivateMessageRecieved?
            IrcClient.UserMessageRecieved += OnLocalUserMessageReceived;

            // join the IRC channel
            IrcClient.Channels.Join(Config.Channel);
        }

        /// <summary>
        /// Executed when IRC client connect failed.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnIrcClientConnectFailed(object sender, SocketErrorEventArgs e)
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
        protected void OnIrcClientProtocolError(object sender, SocketErrorEventArgs e)
        {
        }

        /// <summary>
        /// Executed when IRC client joined the channel.
        /// Beware: This is executed by a different thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLocalUserJoinedChannel(object sender, ChannelUserEventArgs e)
        {
            // save reference to channel
            IrcChannel = e.Channel;

            string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (Config.SelectedConnectionInfo != null)
            {
                // build the welcome-string
                string welcomestr =
                    "Meridian 59 IRC BOT (" + ver + ") - Player " + Config.SelectedConnectionInfo.Character + " on host " + 
                    Config.SelectedConnectionInfo.Host + ":" + Config.SelectedConnectionInfo.Port;

                // try to log the chatmessage to IRC
                SendIRCChannelMessage(welcomestr);
            }
        }

        /// <summary>
        /// Invoked when a channel message was received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            if (e.PrivateMessage == null || e.PrivateMessage.Message == null)
                return;

            string message = e.PrivateMessage.Message;
            string messageSource = e.PrivateMessage.User.Nick;

            // Ignore message whose length is just @prefix + @ + space length.
            if (message.Length <= Config.ChatPrefix.Length + 1 + 1)
                return;

            // try get channeluser by name
            IrcUser usr = GetChannelUser(messageSource);

            if (usr == null)
                return;

            string s = String.Empty;
            string banner = String.Empty;
            string ignoreSystemRegex = String.Empty;
            bool relayMsg = false;

            // Relay messages from allowed chatbots
            foreach (var relayBot in Config.RelayBots)
            {
                if (relayBot.Name.Contains(messageSource))
                {
                    // Sanity check for our own name
                    if (messageSource.Equals(Config.NickName))
                        return;

                    // Convert the IRC colors back to server styles/colors.
                    s = IRCChatStyle.CreateChatMessageFromIRCMessage(message);

                    // Ignore any messages containing...
                    if (!String.IsNullOrEmpty(relayBot.IgnoreAllRegex) && (Regex.Match(s, relayBot.IgnoreAllRegex)).Success)
                        return;

                    // Banner is the second string in the tuple.
                    banner = relayBot.Banner;
                    ignoreSystemRegex = relayBot.IgnoreSystemRegex;

                    relayMsg = true;
                    break;

                }
            }

            if (!relayMsg)
            {
                // Ignore messages without @prefix start.
                if (!relayMsg && message.IndexOf("@" + Config.ChatPrefix) != 0)
                    return;

                // only allowed users and admins can use the bot
                if (!(Config.AllowedUsers.Contains(messageSource)
                        || Config.Admins.Contains(messageSource)))
                {
                    // respond user he can't use this feature
                    SendIRCChannelMessage(messageSource + " you can't use this feature.");

                    return;
                }

                if (!IsUserRegistered(messageSource))
                {
                    // Must have registered nickname.
                    SendIRCChannelMessage(messageSource + " you must register your nickname to use this feature.");

                    return;
                }

                // now remove the @103 start
                s = message.Substring(Config.ChatPrefix.Length + 1 + 1);
            }

            // used delimiter
            const char delimiter = ' ';
            // split up into words
            string[] words = s.Split(delimiter);

            if (words.Length > 0)
            {
                if (Util.IsDisallowedDMCommand(words[0]))
                {
                    SendIRCChannelMessage(messageSource + " you can't use this feature. This incident has been logged.");
                    foreach (string admin in Config.Admins)
                    {
                        // try get channeluser by name
                        usr = GetChannelUser(admin);

                        // online? send!
                        if (usr != null)
                            SendIRCMessage(admin, String.Format("IRC User {0} has attempted to use a DM command", messageSource));
                    }
                    return;
                }
                else if (words.Length > 3 && relayMsg)
                {
                    // First word is the server's header (e.g. 103:) so don't use it.
                    if (words[1].Contains("[###]"))
                    {
                        // Ignore system messages containing....
                        if (!String.IsNullOrEmpty(ignoreSystemRegex) && (Regex.Match(s, ignoreSystemRegex)).Success)
                            return;

                        // Adjust the color codes to display [###] correctly, drop the
                        // existing [###] and add a fixed one here. Doesn't seem to be
                        // possible to fix the one in the message itself.
                        s = "dm gqemote " + banner + " ~U[###]~n ~U";
                        s += String.Join(delimiter.ToString(), words, 2, words.Length - 2);
                    }
                    else if ((Regex.Match(s, @"(broadcasts,|teilt allen)")).Success)
                    {
                        // Add server header, echo message.
                        s = "dm gqemote " + banner + " ~n~w";
                        s += String.Join(delimiter.ToString(), words, 1, words.Length - 1);
                    }
                    else if (words[1].Contains("You") && words[2].Contains("broadcast,"))
                    {
                        // Echo messages that the other bot broadcasts to its server.
                        s = "dm gqemote " + banner + " ~n~wHelp broadcasts,";
                        s += String.Join(delimiter.ToString(), words, 3, words.Length - 3);
                    }
                    else if (words.Length > 5 && words[1].Contains("Du") && words[3].Contains("allen"))
                    {
                        // German bot broadcasting to its server, echo the German translation.
                        s = "dm gqemote " + banner + " ~n~wHelp teilt allen mit,";
                        s += String.Join(delimiter.ToString(), words, 5, words.Length - 5);
                    }
                }
                else
                {
                    switch (words[0])
                    {
                        case ChatCommandBroadcast.KEY1:
                        case ChatCommandBroadcast.KEY2:

                            // keep first word
                            s = String.Join(delimiter.ToString(), words, 0, 1);

                            // insert banner + name
                            s += delimiter + Config.Banner;
                            s += messageSource + ": ~n";

                            // add rest
                            s += String.Join(delimiter.ToString(), words, 1, words.Length - 1);
                            break;

                        case ChatCommandTell.KEY1:
                        case ChatCommandTell.KEY2:

                            // keep first two word
                            s = String.Join(delimiter.ToString(), words, 0, 2);

                            // insert banner + name
                            s += delimiter + Config.Banner;
                            s += messageSource + ": ~n";

                            // add rest
                            s += String.Join(delimiter.ToString(), words, 2, words.Length - 2);
                            break;
                    }
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
        protected void OnLocalUserMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            if (e.PrivateMessage == null || e.PrivateMessage.Message == null)
                return;

            string message = e.PrivateMessage.Message;
            string messageSender = e.PrivateMessage.Source;

            // try get channeluser by name
            IrcUser usr = GetChannelUser(messageSender);

            if (usr == null)
                return;

            // only allow ADMINS from config
            if (!Config.Admins.Contains(messageSender))
            {
                // respond user he can't use this feature
                SendIRCMessage(messageSender,
                    messageSender + " you can't use the admin console - Only admins allowed.");
                return;
            }

            // Must be registered.
            if (!IsUserRegistered(messageSender))
            {
                // respond user he can't use this feature
                SendIRCMessage(messageSender,
                    messageSender + " you can't use the admin console - nickname must be registered.");
                return;
            }

            // invalid
            if (message == null || message == String.Empty)
                return;

            Log("ADM", messageSender + " used the command " + message);

            // Bot admin command?
            if (message.StartsWith("@"))
            {
                // Returns false if nothing handled the admin command.
                if (!IRCAdminBotCommand.ParseAdminCommand(messageSender, message, this))
                    SendIRCMessage(messageSender,
                        "Couldn't find a handler for admin command " + message);

                return;
            }

            // get first space in text
            int firstspace = message.IndexOf(' ');
                    
            // either use first part up to first space or full text if no space
            string comtype = 
                (firstspace > 0) ? message.Substring(0, firstspace) : comtype = message;

            // check if commandtype is allowed
            if (Config.AdminCommands.Contains(comtype))
            {
                // Record this admin as recent.
                if (!RecentAdmins.Contains(messageSender))
                    RecentAdmins.Add(messageSender);
                // enqueue it for execution
                AdminCommandQueue.Enqueue(message);
            }
            else
            {
                // respond admin he can't use this admincommand
                SendIRCMessage(messageSender,
                    messageSender + " you can't use the admin command '" + comtype + "'");
            }
        }

        /// <summary>
        /// Invoked when a user joins the IRC channel.
        /// Calls a nick registration check on the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnUserJoined(object sender, ChannelUserEventArgs e)
        {
            if (e.User.Nick == Config.NickName)
            {
                OnLocalUserJoinedChannel(sender, e);
                return;
            }
            // Perform WhoIs query to determine registration status on user.
            UserRegCheck(e.User.Nick);
        }

        /// <summary>
        /// Invoked when a user leaves the IRC channel.
        /// Removes the user from the user registration list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnUserLeft(object sender, ChannelUserEventArgs e)
        {
            RemoveUserRegistration(e.User.Nick);
        }

        protected void OnUsersListReceived(object sender, ChannelEventArgs e)
        {
            SendWhoXQuery(IrcChannel.Name);
        }

        /// <summary>
        /// Invoked when a RPL_WHOSPCRPL message is received from IRC server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnWhoXReplyReceived(object sender, WhoxReceivedEventArgs e)
        {
            if (e == null || e.WhoxResponse == null)
                return;

            foreach (ExtendedWho extendedWho in e.WhoxResponse)
            {
                if (extendedWho.User != null
                    && extendedWho.User.Account != null
                    && extendedWho.User.Nick != null)
                {

                    Log("IRC", "Received WhoX response for " + extendedWho.User.Nick);
                    AddUserRegistration(extendedWho.User.Nick, true);
                }
            }
        }
        #endregion

        #region User nick registrations
        /// <summary>
        /// Returns the registration status of user with nickname Name.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool IsUserRegistered(string Name)
        {
            UserRegistration.TryGetValue(Name, out bool isRegistered);

            return isRegistered;
        }

        /// <summary>
        /// Adds a user nickname registration to list.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="IsRegistered"></param>
        private void AddUserRegistration(string Name, bool IsRegistered)
        {
            if (UserRegistration.TryGetValue(Name, out bool isAlreadyRegistered))
                UserRegistration[Name] = IsRegistered;
            else
                UserRegistration.Add(Name, IsRegistered);
        }

        /// <summary>
        /// Removes a user nickname registration from list.
        /// </summary>
        /// <param name="Name"></param>
        private void RemoveUserRegistration(string Name)
        {
            if (UserRegistration.ContainsKey(Name))
                UserRegistration.Remove(Name);
        }

        /// <summary>
        /// Enqueue a message to send a WhoX query for the nickname.
        /// and a nickname.
        /// </summary>
        /// <param name="Name"></param>
        public void UserRegCheck(string Name)
        {
            if (Name != string.Empty)
            {
                WhoXQueryQueue.Enqueue(Name);
            }
        }

        /// <summary>
        /// Send a WhoX (who extended) query to IRC server, with
        /// %a flag for returning logged-in account status.
        /// </summary>
        /// <param name="Message"></param>
        private void SendWhoXQuery(string Name)
        {
            IrcClient.Who(Name, WhoxFlag.AccountName, WhoxField.QueryType, null);
        }

        /// <summary>
        /// Sends a private message to a user.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Message"></param>
        public void SendIRCMessage(string Username, string Message)
        {
            IRCSendQueue.Enqueue(Username, Message);
            //IrcClient.SendMessage(Message, Username);
        }

        /// <summary>
        /// Sends a message to the IRC channel.
        /// </summary>
        /// <param name="Message"></param>
        public void SendIRCChannelMessage(string Message)
        {
            SendIRCMessage(IrcChannel.Name, Message);
        }
        #endregion
    }

}
