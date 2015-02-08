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
using System.Xml;
using System.Threading;
using System.ComponentModel;

using Android.OS;

using Meridian59.Files;
using Meridian59.Files.ROO;
using Meridian59.Protocol.GameMessages;
using Meridian59.Android.ChatClient.Adapters;

namespace Meridian59.Android.ChatClient
{
    /// <summary>
    /// Android M59 ChatClient
    /// </summary>
    public class ChatClient : BaseClient
    {
        #region Constants
        public const int TICKINTERVALMS = 100;
        public const string SDCARDFOLDER = "meridian59";

        public static readonly string WORKPATH = 
            global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/" + SDCARDFOLDER + "/";

        public static readonly string VERSIONFILE = WORKPATH + "version.xml";
        public static readonly string STRINGSFILE = WORKPATH + "rsc0000.rsb";
        #endregion

        #region Fields
        protected Thread thread;
        protected MainActivity ui;
        protected byte versionmajor;
        protected byte versionminor;
        protected uint versionresources;
        #endregion

        #region Properties
        public ChatMessageAdapter ChatAdapter { get; protected set; }
        public OnlinePlayersAdapter OnlinePlayersAdapter { get; protected set; }
        public CharSelectItemAdapter CharSelectItemAdapter { get; protected set; }
        public override byte AppVersionMajor { get { return versionmajor; } }
        public override byte AppVersionMinor { get { return versionminor; } }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MainActivity"></param>
        public ChatClient(MainActivity MainActivity)
            : base()
        {
            // save reference to UI
            ui = MainActivity;

            // check if necessary SDCARD stuff exists
            if (VerifySDCard())
            {
                // load versions from xml
                ReadVersionsFromFile();

                // create data adapters for model layer
                ChatAdapter = new ChatMessageAdapter(DataController.ChatMessages, ui);
                OnlinePlayersAdapter = new OnlinePlayersAdapter(DataController.OnlinePlayers, ui);

                // hook up some event handlers
                DataController.RoomInformation.PropertyChanged += RoomInformation_PropertyChanged;
                DataController.AvatarCondition.ListChanged += AvatarCondition_ListChanged;

                // workaround for monodroid because there is no proper access to mainthread:
                // spawn a thread that forces mainthread to process a tick
                thread = new Thread(ThreadProc);
                thread.Start();
            }
            else
            {
                ui.ShowSDCARDError();
            }
        }

        /// <summary>
        /// Overridden init
        /// </summary>
        public override void Init()
        {
            InitLegacyResources();
        }

        /// <summary>
        /// Overriden initlegacyresources
        /// </summary>
        public override void InitLegacyResources()
        {
            // initialize the resourcemanagerconfig
            ResourceManagerConfig config = new ResourceManagerConfig(
                versionresources,
                false, false, false, false, false, false,
                STRINGSFILE,
                WORKPATH + "rooms",
                WORKPATH + "objects",
                WORKPATH + "roomtextures",
                WORKPATH + "wavs");
            
            // init the legacy resources
            ResourceManager.InitConfig(config);
        }

        /// <summary>
        /// Executed once per app cycle
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (ui != null)
            {
                ui.Update();
            }
        }

        /// <summary>
        /// Executed by internal thread to force ticks on mainthread
        /// </summary>
        protected void ThreadProc()
        {
            while (ui != null)
            {
                // force the mainthread to process a tick
                ui.RunOnUiThread(() => { Tick(); });

                // sleep some time to not lock UI thread
                Thread.Sleep(TICKINTERVALMS);
            }
        }

        /// <summary>
        /// Verifies the necessary folders and files on SDCARD exist
        /// </summary>
        /// <returns></returns>
        protected bool VerifySDCard()
        {
            if (Directory.Exists(WORKPATH) &&
                File.Exists(VERSIONFILE) &&
                File.Exists(STRINGSFILE))
            
                return true;
            
            else
                return false;
        }

        /// <summary>
        /// Reads App and Resource versions from file
        /// </summary>
        protected void ReadVersionsFromFile()
        {
            XmlReader reader = XmlReader.Create(VERSIONFILE);

            reader.ReadToFollowing("version");
            versionmajor = Convert.ToByte(reader["major"]);
            versionminor = Convert.ToByte(reader["minor"]);
            versionresources = Convert.ToUInt32(reader["resources"]);

            reader.Close();
        }

        /// <summary>
        /// Executed when login credentials were wrong
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleLoginFailedMessage(LoginFailedMessage Message)
        {
            base.HandleLoginFailedMessage(Message);
           
            ui.ShowCredentialsError();
        }

        /// <summary>
        /// Executed when resource version mismatched server
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleDownloadMessage(DownloadMessage Message)
        {            
            ui.ShowResourceVersionError();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleGetClientMessage(GetClientMessage Message)
        {           
            ui.ShowAppVersionError();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleCharactersMessage(CharactersMessage Message)
        {
            CharSelectItemAdapter = new CharSelectItemAdapter(Message.Characters, ui);
        
            ui.Layout = Resource.Layout.SelectAvatar;
            ui.cbSelect.Adapter = CharSelectItemAdapter;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        protected override void HandleGetLoginMessage(GetLoginMessage Message)
        {
            SendLoginMessage(ui.txtUsername.Text, ui.txtPassword.Text);
        }

        protected void RoomInformation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            ui.ProcessRoomInformationChange(e);                     
        }

        protected void AvatarCondition_ListChanged(object sender, ListChangedEventArgs e)
        {
            
            ui.ProcessAvatarConditionChange();                    
        }

        protected override void HandleNetworkClientException(Exception Error)
        {
            ui.ShowNetworkError();
        }

        #region Unused overrides
        protected override void DoWallSideChange(RooSideDef ChangedWallSide)
        {

        }

        protected override void HandleLoginModeMessageMessage(LoginModeMessageMessage Message)
        {

        }

        protected override void HandleLoginOKMessage(LoginOKMessage Message)
        {

        }
        #endregion
    }
}
