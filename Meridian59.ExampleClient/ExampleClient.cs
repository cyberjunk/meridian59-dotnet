using System.Windows.Forms;
using Meridian59.Files;
using Meridian59.DebugUI;
using Meridian59.Data.Models;
using Meridian59.Protocol.GameMessages;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Events;
using System.IO;
using Meridian59.Files.ROO;
using System;
using System.Net.Sockets;
using Meridian59.Client;
using Meridian59.Common;
using Meridian59.Data;

namespace Meridian59.ExampleClient
{
    public class ExampleClient : SingletonClient<GameTick, ResourceManager, DataController, Config, ExampleClient>
    {
        #region Fields
        /// <summary>
        /// The debug window
        /// </summary>
        protected DebugForm DebugForm;
        
        /// <summary>
        /// The mainform with the login/char selection
        /// </summary>
        protected MainForm MainForm;
        #endregion

        #region Properties
        /// <summary>
        /// The major application version of this implementation.
        /// Must match major version of meridian.exe of the server you're connecting to.
        /// </summary>
        public override byte AppVersionMajor { get { return 70; } }

        /// <summary>
        /// The minor application version of this implementation.
        /// Must match minor version of meridian.exe of the server you're connecting to.
        /// </summary>
        public override byte AppVersionMinor { get { return 1; } }
        #endregion

        #region Constructors
        public ExampleClient()
            : base()
        {  
            // Initialize the DebugForm
            DebugForm = new DebugForm();
            DebugForm.DataController = Data;
            DebugForm.ResourceManager = ResourceManager;
            DebugForm.PacketSend += DebugForm_PacketSend;
            DebugForm.PacketLogChanged += DebugForm_PacketLogChanged;
            DebugForm.Closed += DebugForm_Closed;
            DebugForm.Show();

            // Initialize the MainForm
            MainForm = new MainForm();
            MainForm.LoginControl.ConnectRequest += LoginControl_ConnectRequest;
            MainForm.LoginControl.DisconnectRequest += LoginControl_DisconnectRequest;
            MainForm.Closed += MainForm_Closed;
            MainForm.ButtonSelectCharacter.Click += ButtonSelectCharacter_Click;
            MainForm.Show();

        }
        #endregion

        #region Event handlers
        private void DebugForm_PacketSend(object sender, GameMessageEventArgs e)
        {
            // send the user created generic message
            ServerConnection.SendQueue.Enqueue(e.Message);
        }

        private void DebugForm_PacketLogChanged(object sender, DebugUI.Events.PacketLogChangeEventArgs e)
        {
            // update the setings in the datacontroller
            Data.LogIncomingMessages = e.LogIncoming;
            Data.LogOutgoingMessages = e.LogOutgoing;
            Data.LogPingMessages = e.LogPings;

            // tell networkclient to potentially loopback sent messages
            ServerConnection.IsOutgoingPacketLogEnabled = e.LogOutgoing;
        }

        private void DebugForm_Closed(object sender, System.EventArgs e)
        {
            // stop the application
            IsRunning = false;
        }

        private void LoginControl_ConnectRequest(object sender, System.EventArgs e)
        {
            ResourceManager.SelectStringDictionary("rsc0000.rsb");

            // start connect to server
            ServerConnection.Connect(MainForm.LoginControl.Hostname, MainForm.LoginControl.Hostport);
        }

        private void LoginControl_DisconnectRequest(object sender, System.EventArgs e)
        {
            // disconnect
            ServerConnection.Disconnect();
        }

        private void MainForm_Closed(object sender, System.EventArgs e)
        {
            // stop the application
            IsRunning = false;
        }

        private void ButtonSelectCharacter_Click(object sender, System.EventArgs e)
        {
            CharSelectItem item = (CharSelectItem)MainForm.CharacterList.SelectedItem;

            // send "UseCharacterMessage" and request basic gameinfo
            SendUseCharacterMessage(new ObjectID(item.ID), true);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Add code which should not run in constructor,
        /// but before entering the app loop here.
        /// </summary>
        public override void Init()
        {
            Config.ResourcesPath = "./resources/";

            base.Init();
        }

        /// <summary>
        /// Custom application loop code
        /// </summary>
        public override void Update()
        {
            // internal updatestep
            base.Update();

            // handle all gui form events (button clicks etc.)
            Application.DoEvents();
        }

        /// <summary>
        /// Handle exceptions from network client
        /// </summary>
        /// <param name="Error"></param>
        protected override void OnServerConnectionException(Exception Error)
        {
            // tell user about unknown exception
            MessageBox.Show(Error.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly, false);

            // reset connectionstate (networkclient closes on exception)
            MainForm.LoginControl.ConnectedState = false;

            // clear character items
            MainForm.CharacterList.Items.Clear();
        }

        /// <summary>
        /// Custom cleanup code for anything not part of baseclient class.
        /// </summary>
        protected override void Cleanup()
        {
            base.Cleanup();

            if (DebugForm != null)
            {
                DebugForm.Close();
                DebugForm = null;
            }

            if (MainForm != null)
            {
                MainForm.Close();
                MainForm = null;
            }
        }
        #endregion

        #region LoginModeMessage handlers
        /// <summary>
        /// Handler for "GetClient" message, your minor/major versions don't match server.
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleGetClientMessage(GetClientMessage Message)
        {
            // tell user about mismatching major/minor version
            MessageBox.Show(APPVERSIONMISMATCH);
            
            // close connection, we're not going to download the proposed meridian.exe
            ServerConnection.Disconnect();

            // reset logincontrol
            MainForm.LoginControl.ConnectedState = false;
        }

        /// <summary>
        /// Handler for "GetLogin" message, returns account credentials
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleGetLoginMessage(GetLoginMessage Message)
        {
            SendLoginMessage(MainForm.LoginControl.Username, MainForm.LoginControl.Password);
        }

        /// <summary>
        /// Handler for a successful verification of login credentials
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleLoginOKMessage(LoginOKMessage Message)
        {
            // nothing really to do in the example client
        }

        /// <summary>
        /// Handler for a invalid credentials return
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleLoginFailedMessage(LoginFailedMessage Message)
        {
            // call base handler
            base.HandleLoginFailedMessage(Message);

            // tell user about wrong credentials
            MessageBox.Show(WRONGCREDENTIALS);

            // reset state of logincontrol
            MainForm.LoginControl.ConnectedState = false;
        }

        /// <summary>
        /// Custom message as login result (i.e. banned/maintenance)
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleLoginModeMessageMessage(LoginModeMessageMessage Message)
        {
            MessageBox.Show(Message.Message);

            MainForm.LoginControl.ConnectedState = false;
        }

        /// <summary>
        /// Handler for a resource download request from server (your DownloadVersion mismatches)
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleDownloadMessage(DownloadMessage Message)
        {
            base.HandleDownloadMessage(Message);

            // tell user about mismatching resources version
            MessageBox.Show(RESVERSIONMISMATCH);

            // close connection, we're not going to download the proposed new resource files
            ServerConnection.Disconnect();

            // reset logincontrol
            MainForm.LoginControl.ConnectedState = false;
        }
        #endregion

        #region GameMode message handlers
        /// <summary>
        /// Handler for "Characters" message, sets available characters on gui.
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleCharactersMessage(CharactersMessage Message)
        {
            // add characters to list
            MainForm.CharacterList.Items.Clear();
            MainForm.CharacterList.Items.AddRange(Message.WelcomeInfo.Characters.ToArray());
        }
        #endregion        
    }
}
