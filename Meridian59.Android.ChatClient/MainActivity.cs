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
using System.ComponentModel;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;

using Meridian59.Common.Enums;
using Meridian59.Data.Models;
using Meridian59.Data.Controller;

namespace Meridian59.Android.ChatClient
{
    /// <summary>
    /// The main activity representing the UI
    /// </summary>
    [Activity(Label = "Meridian 59", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        #region Constants
        public const string TITLELOGIN = "Login";
        public const string TITLESELECTAVATAR = "Select avatar";
        public const string TITLEINFO = "Info";
        public const string TITLECHAT = "Chat";
        public const string TITLEWHOLIST = "Who's online";
        public const string ERROR = "Error";
        public const string OK = "Ok";
        public const string ERRORNETWORK = "No connection or network error.";
        public const string ERRORSDCARD = "Required SDCARD content not found.";
        public const string ERRORCREDENTIALS = "Wrong username or password.";
        public const string ERRORAPPVERSION = "Your app version doesn't match the server. Fix your version.xml";
        public const string ERRORRESOURCEVERSION = "Your resource version doesn't match the server. Fix your version.xml";
        #endregion

        #region Fields/Properties
        protected volatile int layout;
        protected ChatClient chatClient;
        public IMenu Menu { get; protected set; }
        public int Layout
        {
            get { return layout; }
            set { SetLayout(value); }
        }
        #endregion
       
        #region UI elements
        public EditText txtUsername { get { return FindViewById<EditText>(Resource.Id.txtUsername); } }
        public EditText txtPassword { get { return FindViewById<EditText>(Resource.Id.txtPassword); } }
        public EditText txtHost { get { return FindViewById<EditText>(Resource.Id.txtHost); } }
        public EditText txtPort { get { return FindViewById<EditText>(Resource.Id.txtPort); } }
        public Button btnLogin { get { return FindViewById<Button>(Resource.Id.btnLogin); } }
        public Spinner cbSelect { get { return FindViewById<Spinner>(Resource.Id.cbSelect); } }
        public Button btnSelect { get { return FindViewById<Button>(Resource.Id.btnSelect); } }
        public ListView listChat { get { return FindViewById<ListView>(Resource.Id.listChat); } }
        public ListView listPlayers { get { return FindViewById<ListView>(Resource.Id.listPlayers); } }
        public TextView txtLatency { get { return FindViewById<TextView>(Resource.Id.txtLatency); } }
        public TextView txtRoom { get { return FindViewById<TextView>(Resource.Id.txtRoom); } }
        public TextView txtHP { get { return FindViewById<TextView>(Resource.Id.txtHP); } }
        public TextView txtMP { get { return FindViewById<TextView>(Resource.Id.txtMP); } }
        public TextView txtVigor { get { return FindViewById<TextView>(Resource.Id.txtVigor); } }
        public Button btnRest { get { return FindViewById<Button>(Resource.Id.btnRest); } }
        #endregion

        /// <summary>
        /// Constructor for MainActivity
        /// </summary>
        public MainActivity()
            : base()
        {
        }

        #region Activity overrides
        /// <summary>
        /// Start override
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();
        }
       
        /// <summary>
        /// Create override
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // create and start chatclient with manual ticks
            chatClient = new ChatClient(this);
            chatClient.Start(false);
            
            // show login content at start
            Layout = Resource.Layout.Login;
        }

        /// <summary>
        /// Destroy override
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // shutdown
            chatClient.IsRunning = false;
            chatClient.MessageEnrichment.IsRunning = false;
            chatClient.NetworkClient.Disconnect();
        }

        /// <summary>
        /// Inflates the custom menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // create menu
            MenuInflater.Inflate(Resource.Layout.Menu, menu);
            Menu = menu;

            // adjust menu to current layout
            SetMenu(layout);

            return true;
        }

        /// <summary>
        /// Executed when user selected an option from menu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_disconnect:
                    chatClient.NetworkClient.Disconnect();
                    Layout = Resource.Layout.Login;
                    return true;
                
                case Resource.Id.menu_wholist:
                    Layout = Resource.Layout.WhoList;
                    return true;

                case Resource.Id.menu_info:
                    Layout = Resource.Layout.Info;
                    return true;

                case Resource.Id.menu_chat:
                    Layout = Resource.Layout.Chat;
                    return true;
               
                case Resource.Id.menu_send:
                    ShowChatPrompt();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Shows prompt to enter chat
        /// </summary>
        public void ShowChatPrompt()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Send text");
            
            // Set an EditText view to get user input 
            EditText input = new EditText(this);
            alert.SetView(input);

            alert.SetPositiveButton("Send", (sender, e) =>
            {
                ChatCommand chatCommand = ChatCommand.Parse(input.Text, chatClient.DataController);

                // handle chatcommand
                if (chatCommand != null)
                {
                    switch (chatCommand.CommandType)
                    {
                        case ChatCommandType.Say:
                            ChatCommandSay chatCommandSay = (ChatCommandSay)chatCommand;
                            chatClient.SendSayToMessage(ChatTransmissionType.Normal, chatCommandSay.Text);
                            break;

                        case ChatCommandType.Yell:
                            ChatCommandYell chatCommandYell = (ChatCommandYell)chatCommand;
                            chatClient.SendSayToMessage(ChatTransmissionType.Yell, chatCommandYell.Text);
                            break;

                        case ChatCommandType.Broadcast:
                            ChatCommandBroadcast chatCommandBroadcast = (ChatCommandBroadcast)chatCommand;
                            chatClient.SendSayToMessage(ChatTransmissionType.Everyone, chatCommandBroadcast.Text);
                            break;

                        case ChatCommandType.Guild:
                            ChatCommandGuild chatCommandGuild = (ChatCommandGuild)chatCommand;
                            chatClient.SendSayToMessage(ChatTransmissionType.Guild, chatCommandGuild.Text);
                            break;

                        case ChatCommandType.Tell:
                            ChatCommandTell chatCommandTell = (ChatCommandTell)chatCommand;
                            chatClient.SendSayGroupMessage(new uint[] { chatCommandTell.TargetID }, chatCommandTell.Text);
                            break;
                    }
                }
            });

            alert.SetNegativeButton("Cancel", (sender, e) =>
            {

            });

            alert.Show();
        }

        /// <summary>
        /// Shows error about non existant SDCARD resources
        /// </summary>
        public void ShowSDCARDError()
        {
            // display error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(ERROR);
            builder.SetMessage(ERRORSDCARD);
            builder.SetPositiveButton(OK, (sender, e) => { Finish(); });
            builder.Show();
        }

        /// <summary>
        /// Shows error when login credentials had been wrong
        /// </summary>
        public void ShowCredentialsError()
        {
            // display error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(ERROR);
            builder.SetMessage(ERRORCREDENTIALS);
            builder.SetPositiveButton(OK, (sender, e) => { });
            builder.Show();

            // reenable the login controls
            ToggleLoginEnabled();
        }

        /// <summary>
        /// Shows error when server expected different client version
        /// </summary>
        public void ShowAppVersionError()
        {
            // display error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(ERROR);
            builder.SetMessage(ERRORAPPVERSION);
            builder.SetPositiveButton(OK, (sender, e) => { });
            builder.Show();

            // reenable the login controls
            ToggleLoginEnabled();
        }

        /// <summary>
        /// Shows error when server expected different resource version
        /// </summary>
        public void ShowResourceVersionError()
        {
            // display error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(ERROR);
            builder.SetMessage(ERRORRESOURCEVERSION);
            builder.SetPositiveButton(OK, (sender, e) => { });
            builder.Show();

            // reenable the login controls
            ToggleLoginEnabled();
        }

        /// <summary>
        /// Shows error with network interface
        /// </summary>
        public void ShowNetworkError()
        {
            // display error
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle(ERROR);
            builder.SetMessage(ERRORNETWORK);
            builder.SetPositiveButton(OK, (sender, e) =>
            {
                // switch back to login
                Layout = Resource.Layout.Login;

                // reenable the login controls
                ToggleLoginEnabled();
            });
            
            builder.Show();
        }

        /// <summary>
        /// Executed once each Chatclient thread cycle
        /// </summary>
        public void Update()
        {
            switch (layout)
            { 
                case Resource.Layout.Info:
                    txtLatency.Text = chatClient.NetworkClient.Latency.ToString() + " ms";
                    break;                   
            }            
        }

        /// <summary>
        /// Executed when information in datalayer for room changed
        /// </summary>
        /// <param name="e"></param>
        public void ProcessRoomInformationChange(PropertyChangedEventArgs e)
        {
            if (layout == Resource.Layout.Info)
            {
                switch (e.PropertyName)
                {
                    case RoomInfo.PROPNAME_ROOMNAME:
                        txtRoom.Text = chatClient.DataController.RoomInformation.RoomName;
                        break;
                }
            }
        }

        /// <summary>
        /// Executed when information about avatar condition changed
        /// </summary>
        public void ProcessAvatarConditionChange()
        {
            if (layout == Resource.Layout.Info)
            {
                RefreshCondition();
            }
        }
        
        /// <summary>
        /// Sets the layout to the given layout value
        /// </summary>
        /// <param name="value"></param>
        protected void SetLayout(int value)
        {
            // cleanup old
            switch (layout)
            {
                case Resource.Layout.Login:
                    btnLogin.Click -= btnLogin_Click;
                    break;

                case Resource.Layout.SelectAvatar:
                    btnSelect.Click -= btnSelect_Click;
                    break;

                case Resource.Layout.Info:
                    btnRest.Click -= btnRest_Click;
                    break;
            }

            // update
            SetContentView(value);
            SetMenu(value);
            layout = value;

            // process new layout
            switch (layout)
            {
                case Resource.Layout.Login:
                    Title = TITLELOGIN;
                    btnLogin.Click += btnLogin_Click;
                    txtUsername.Enabled = true;
                    txtPassword.Enabled = true;
                    txtHost.Enabled = true;
                    txtPort.Enabled = true;
                    btnLogin.Enabled = true;
                    break;

                case Resource.Layout.SelectAvatar:
                    Title = TITLESELECTAVATAR;
                    btnSelect.Click += btnSelect_Click;
                    break;

                case Resource.Layout.Info:
                    Title = TITLEINFO;
                    btnRest.Click += btnRest_Click;
                    RefreshInfo();
                    break;

                case Resource.Layout.Chat:
                    Title = TITLECHAT;
                    listChat.Adapter = chatClient.ChatAdapter;
                    break;

                case Resource.Layout.WhoList:
                    Title = TITLEWHOLIST;
                    listPlayers.Adapter = chatClient.OnlinePlayersAdapter;
                    break;
            }
        }

        /// <summary>
        /// Updates the menu to fit the given layout value
        /// </summary>
        /// <param name="value"></param>
        protected void SetMenu(int value)
        {
            if (Menu != null)
            {
                // process new layout
                switch (value)
                {
                    case Resource.Layout.Login:
                        Menu.FindItem(Resource.Id.menu_disconnect).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_info).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_chat).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_send).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_wholist).SetEnabled(false);
                        break;

                    case Resource.Layout.SelectAvatar:
                        Menu.FindItem(Resource.Id.menu_disconnect).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_info).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_chat).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_send).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_wholist).SetEnabled(false);
                        break;

                    case Resource.Layout.Chat:
                        Menu.FindItem(Resource.Id.menu_disconnect).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_info).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_chat).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_send).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_wholist).SetEnabled(true);
                        break;

                    case Resource.Layout.WhoList:
                        Menu.FindItem(Resource.Id.menu_disconnect).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_info).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_chat).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_send).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_wholist).SetEnabled(false);
                        break;

                    case Resource.Layout.Info:
                        Menu.FindItem(Resource.Id.menu_disconnect).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_info).SetEnabled(false);
                        Menu.FindItem(Resource.Id.menu_chat).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_send).SetEnabled(true);
                        Menu.FindItem(Resource.Id.menu_wholist).SetEnabled(true);
                        break;
                }
            }
        }

        /// <summary>
        /// Refresh all values in "Info" layout
        /// </summary>
        protected void RefreshInfo()
        {
            if (Layout == Resource.Layout.Info)
            {
                // update condition values
                RefreshCondition();

                // update latency value
                txtLatency.Text = chatClient.NetworkClient.Latency.ToString() + " ms";

                // update room name
                txtRoom.Text = chatClient.DataController.RoomInformation.RoomName;
            }
        }

        /// <summary>
        /// Updates the HP, MP, VIG values from datalayer
        /// </summary>
        protected void RefreshCondition()
        {
            if (Layout == Resource.Layout.Info)
            {
                DataController dataController = chatClient.DataController;

                if (dataController.AvatarCondition.Count > 0)
                    txtHP.Text = dataController.AvatarCondition[0].ValueCurrent.ToString() + "/" +
                        dataController.AvatarCondition[0].ValueMaximum;

                if (dataController.AvatarCondition.Count > 1)
                    txtMP.Text = dataController.AvatarCondition[1].ValueCurrent.ToString() + "/" +
                        dataController.AvatarCondition[1].ValueMaximum;

                if (dataController.AvatarCondition.Count > 2)
                    txtVigor.Text = dataController.AvatarCondition[2].ValueCurrent.ToString() + "/" +
                        dataController.AvatarCondition[2].ValueRenderMax;
            }
        }

        /// <summary>
        /// Disables or enables the login controls
        /// </summary>
        protected void ToggleLoginEnabled()
        {
            txtUsername.Enabled = !txtUsername.Enabled;
            txtPassword.Enabled = !txtPassword.Enabled;
            txtHost.Enabled = !txtHost.Enabled;
            txtPort.Enabled = !txtPort.Enabled;
            btnLogin.Enabled = !btnLogin.Enabled;
        }

        /// <summary>
        /// Login button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // initialize the connection to the server
            chatClient.NetworkClient.Connect(txtHost.Text, Convert.ToUInt16(txtPort.Text));

            // disable user-controls to avoid multiple clicks
            ToggleLoginEnabled();
        }

        /// <summary>
        /// Select avatar button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            // get item the user selected
            CharSelectItem item = chatClient.CharSelectItemAdapter[cbSelect.SelectedItemPosition];

            // send a use-this-character message
            chatClient.SendUseCharacterMessage(new ObjectID(item.ID), true);

            // switch to chat (initial after login)
            Layout = Resource.Layout.Chat;
        }

        /// <summary>
        /// Rest/Stand button click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRest_Click(object sender, EventArgs e)
        {
            if (chatClient.DataController.IsResting)
            {
                // request to stand
                chatClient.SendUserCommandStand();

                // update button
                Button btn = (Button)sender;
                btn.Text = "Rest";
            }
            else
            {
                // request to rest
                chatClient.SendUserCommandRest();

                // update button
                Button btn = (Button)sender;
                btn.Text = "Stand";
            }
        }
        #endregion
    }
}

