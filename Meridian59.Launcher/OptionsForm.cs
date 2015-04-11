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
using System.Drawing;
using System.Windows.Forms;
using Meridian59.Launcher.Models;
using System.Collections.Generic;
using System.IO;
using Meridian59.Data.Models;
using Meridian59.Files;
using Meridian59.Native;

namespace Meridian59.Launcher
{
    public partial class OptionsForm : Form
    {
        protected bool dragging;
        protected Point pointClicked;
        protected KeyLearnForm keyLearnForm;
        private Options config;
        private ResourceManager resourceManager;

        public OptionsForm()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// The config datasource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public Options Config
        {
            get { return config; }
            set
            {
                config = value;

                config.PropertyChanged += dataSource_PropertyChanged;

                cbConnections.DataSource = Config.Connections;
                cbConnections.DisplayMember = ConnectionInfo.PROPNAME_NAME;
                cbConnections.SelectedIndex = Config.SelectedConnectionIndex;

                // Engine tab
                SetDisplay();
                //SetResolution(cbDisplay.SelectedText); // is done on trigger in SetDisplay
                SetFSAA(config.FSAA);
                SetTextureFiltering(config.TextureFiltering);
                SetImageBuilder(config.ImageBuilder);
                SetBitmapScaling(config.BitmapScaling);
                SetTextureQuality(config.TextureQuality);

                chkDisableRoomTextures.DataBindings.Clear();
                chkDisableRoomTextures.DataBindings.Add("Checked", Config, Options.PROPNAME_DISABLENEWROOMTEXTURES);

                chkDisable3DModels.DataBindings.Clear();
                chkDisable3DModels.DataBindings.Add("Checked", Config, Options.PROPNAME_DISABLE3DMODELS);

                chkDisableNewSky.DataBindings.Clear();
                chkDisableNewSky.DataBindings.Add("Checked", Config, Options.PROPNAME_DISABLENEWSKY);

                chkWindowMode.DataBindings.Clear();
                chkWindowMode.DataBindings.Add("Checked", Config, Options.PROPNAME_WINDOWMODE);

                chkWindowFrame.DataBindings.Clear();
                chkWindowFrame.DataBindings.Add("Checked", Config, Options.PROPNAME_WINDOWFRAME);
                
                chkVSync.DataBindings.Clear();
                chkVSync.DataBindings.Add("Checked", Config, Options.PROPNAME_VSYNC);
                
                chkMipmaps.DataBindings.Clear();
                chkMipmaps.DataBindings.Add("Checked", Config, Options.PROPNAME_NOMIPMAPS);
                
                chkDisableLoopSounds.DataBindings.Clear();
                chkDisableLoopSounds.DataBindings.Add("Checked", Config, Options.PROPNAME_DISABLELOOPSOUNDS);

                chkDisableWeatherEffects.DataBindings.Clear();
                chkDisableWeatherEffects.DataBindings.Add("Checked", Config, Options.PROPNAME_DISABLEWEATHEREFFECTS);
                
                tbWeatherParticles.Value = config.WeatherParticles;
                tbMusicVol.Value = config.MusicVolume;
                tbDecorationIntensity.Value = config.DecorationIntensity;
                tbDecorationQuality.Value = config.DecorationQuality;

                numResourceVersion.DataBindings.Clear();
                numResourceVersion.DataBindings.Add("Value", Config, Options.PROPNAME_RESOURCESVERSION);

                txtResourcePath.DataBindings.Clear();
                txtResourcePath.DataBindings.Add("Text", Config, Options.PROPNAME_RESOURCESPATH);

                
              
                chkPreloadRooms.DataBindings.Clear();
                chkPreloadRooms.DataBindings.Add("Checked", Config, Options.PROPNAME_PRELOADROOMS);

                chkPreloadObjects.DataBindings.Clear();
                chkPreloadObjects.DataBindings.Add("Checked", Config, Options.PROPNAME_PRELOADOBJECTS);

                chkPreloadRoomTextures.DataBindings.Clear();
                chkPreloadRoomTextures.DataBindings.Add("Checked", Config, Options.PROPNAME_PRELOADROOMTEXTURES);

                chkPreloadSound.DataBindings.Clear();
                chkPreloadSound.DataBindings.Add("Checked", Config, Options.PROPNAME_PRELOADSOUND);

                chkPreloadMusic.DataBindings.Clear();
                chkPreloadMusic.DataBindings.Add("Checked", Config, Options.PROPNAME_PRELOADMUSIC);

                // Input tab
                cbRightClickAction.SelectedIndex = config.KeyBinding.RightClickAction - 1;

                tbMouseAimSpeed.Value = config.MouseAimSpeed;
                tbKeyRotateSpeed.Value = config.KeyRotateSpeed;

                chkInvertMouseY.DataBindings.Clear();
                chkInvertMouseY.DataBindings.Add("Checked", Config, Options.PROPNAME_INVERTMOUSEY);

                btnLearnMoveForward.Text = config.KeyBinding.MoveForward.ToString();
                btnLearnMoveBackward.Text = config.KeyBinding.MoveBackward.ToString();
                btnLearnMoveLeft.Text = config.KeyBinding.MoveLeft.ToString();
                btnLearnMoveRight.Text = config.KeyBinding.MoveRight.ToString();

                btnLearnRotateLeft.Text = config.KeyBinding.RotateLeft.ToString();
                btnLearnRotateRight.Text = config.KeyBinding.RotateRight.ToString();

                btnLearnWalk.Text = config.KeyBinding.Walk.ToString();
                btnLearnAutoMove.Text = config.KeyBinding.AutoMove.ToString();
                btnLearnNextTarget.Text = config.KeyBinding.NextTarget.ToString();
                btnLearnSelfTarget.Text = config.KeyBinding.SelfTarget.ToString();
                btnLearnOpen.Text = config.KeyBinding.ReqGo.ToString();

                btnLearnAction01.Text = config.KeyBinding.ActionButton01.ToString();
                btnLearnAction02.Text = config.KeyBinding.ActionButton02.ToString();
                btnLearnAction03.Text = config.KeyBinding.ActionButton03.ToString();
                btnLearnAction04.Text = config.KeyBinding.ActionButton04.ToString();
                btnLearnAction05.Text = config.KeyBinding.ActionButton05.ToString();
                btnLearnAction06.Text = config.KeyBinding.ActionButton06.ToString();
                btnLearnAction07.Text = config.KeyBinding.ActionButton07.ToString();
                btnLearnAction08.Text = config.KeyBinding.ActionButton08.ToString();
                btnLearnAction09.Text = config.KeyBinding.ActionButton09.ToString();
                btnLearnAction10.Text = config.KeyBinding.ActionButton10.ToString();
                btnLearnAction11.Text = config.KeyBinding.ActionButton11.ToString();
                btnLearnAction12.Text = config.KeyBinding.ActionButton12.ToString();
                btnLearnAction13.Text = config.KeyBinding.ActionButton13.ToString();
                btnLearnAction14.Text = config.KeyBinding.ActionButton14.ToString();
                btnLearnAction15.Text = config.KeyBinding.ActionButton15.ToString();
                btnLearnAction16.Text = config.KeyBinding.ActionButton16.ToString();
                btnLearnAction17.Text = config.KeyBinding.ActionButton17.ToString();
                btnLearnAction18.Text = config.KeyBinding.ActionButton18.ToString();
                btnLearnAction19.Text = config.KeyBinding.ActionButton19.ToString();
                btnLearnAction20.Text = config.KeyBinding.ActionButton20.ToString();
                btnLearnAction21.Text = config.KeyBinding.ActionButton21.ToString();
                btnLearnAction22.Text = config.KeyBinding.ActionButton22.ToString();
                btnLearnAction23.Text = config.KeyBinding.ActionButton23.ToString();
                btnLearnAction24.Text = config.KeyBinding.ActionButton24.ToString();
                btnLearnAction25.Text = config.KeyBinding.ActionButton25.ToString();
                btnLearnAction26.Text = config.KeyBinding.ActionButton26.ToString();
                btnLearnAction27.Text = config.KeyBinding.ActionButton27.ToString();
                btnLearnAction28.Text = config.KeyBinding.ActionButton28.ToString();
                btnLearnAction29.Text = config.KeyBinding.ActionButton29.ToString();
                btnLearnAction30.Text = config.KeyBinding.ActionButton30.ToString();
                btnLearnAction31.Text = config.KeyBinding.ActionButton31.ToString();
                btnLearnAction32.Text = config.KeyBinding.ActionButton32.ToString();
                btnLearnAction33.Text = config.KeyBinding.ActionButton33.ToString();
                btnLearnAction34.Text = config.KeyBinding.ActionButton34.ToString();
                btnLearnAction35.Text = config.KeyBinding.ActionButton35.ToString();
                btnLearnAction36.Text = config.KeyBinding.ActionButton36.ToString();
                btnLearnAction37.Text = config.KeyBinding.ActionButton37.ToString();
                btnLearnAction38.Text = config.KeyBinding.ActionButton38.ToString();
                btnLearnAction39.Text = config.KeyBinding.ActionButton39.ToString();
                btnLearnAction40.Text = config.KeyBinding.ActionButton40.ToString();
                btnLearnAction41.Text = config.KeyBinding.ActionButton41.ToString();
                btnLearnAction42.Text = config.KeyBinding.ActionButton42.ToString();
                btnLearnAction43.Text = config.KeyBinding.ActionButton43.ToString();
                btnLearnAction44.Text = config.KeyBinding.ActionButton44.ToString();
                btnLearnAction45.Text = config.KeyBinding.ActionButton45.ToString();
                btnLearnAction46.Text = config.KeyBinding.ActionButton46.ToString();
                btnLearnAction47.Text = config.KeyBinding.ActionButton47.ToString();
                btnLearnAction48.Text = config.KeyBinding.ActionButton48.ToString();
            }
        }
        
        /// <summary>
        /// The resourcemanager datasource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public ResourceManager ResourceManager
        {
            get { return resourceManager; }
            set
            {
                resourceManager = value;

                SetResourceCounts();
            }
        }

        private void dataSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Options.PROPNAME_DECORATIONINTENSITY:
                    tbDecorationIntensity.Value = config.DecorationIntensity;
                    break;

                case Options.PROPNAME_DECORATIONQUALITY:
                    tbDecorationQuality.Value = config.DecorationQuality;
                    break;

                case Options.PROPNAME_MUSICVOLUME:
                    tbMusicVol.Value = config.MusicVolume;
                    break;

                case Options.PROPNAME_MOUSEAIMSPEED:
                    tbMouseAimSpeed.Value = config.MouseAimSpeed;
                    break;

                case Options.PROPNAME_KEYROTATESPEED:
                    tbKeyRotateSpeed.Value = config.KeyRotateSpeed;
                    break;

                case Options.PROPNAME_WEATHERPARTICLES:
                    tbWeatherParticles.Value = config.WeatherParticles;
                    break;
            }
        }

        private void SetDisplay()
        {
            cbDisplay.Items.Clear();

            List<Windows.User32.DISPLAY_DEVICE> screenInfos = Windows.User32.GetScreens();
            for (int i = 0; i < screenInfos.Count; i++)
            {
                Windows.User32.DISPLAY_DEVICE screenInfo = screenInfos[i];
                string val = screenInfo.DeviceName;
                
                cbDisplay.Items.Add(val);
            }

            if (cbDisplay.Items.Count > config.Display)           
                cbDisplay.SelectedIndex = config.Display;          
        }

        private void SetResolution(string devicename)
        {
            cbResolution.Items.Clear();

            List<Windows.User32.DEVMODE> screenInfos = Windows.User32.GetScreenInfo(devicename);
            for(int i = 0; i < screenInfos.Count; i++)
            {
                Windows.User32.DEVMODE screenInfo = screenInfos[i];
                string val = screenInfo.dmPelsWidth.ToString() + " x " + screenInfo.dmPelsHeight.ToString();
                cbResolution.Items.Add(val);

                if (String.Equals(val, config.Resolution))
                    cbResolution.SelectedIndex = i;
            }
        }

        private void SetFSAA(string fsaa)
        {
            for(int i = 0; i < cbFSAA.Items.Count; i++)
            {
                string s = (string)cbFSAA.Items[i];

                if (String.Equals(s, fsaa))
                {
                    cbFSAA.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetTextureFiltering(string texturefiltering)
        {
            for (int i = 0; i < cbTextureFiltering.Items.Count; i++)
            {
                string s = (string)cbTextureFiltering.Items[i];

                if (String.Equals(s, texturefiltering))
                {
                    cbTextureFiltering.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetImageBuilder(string imagebuilder)
        {
            for (int i = 0; i < cbImageBuilder.Items.Count; i++)
            {
                string s = (string)cbImageBuilder.Items[i];

                if (String.Equals(s, imagebuilder))
                {
                    cbImageBuilder.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetBitmapScaling(string bitmapscaling)
        {
            for (int i = 0; i < cbBitmapScaling.Items.Count; i++)
            {
                string s = (string)cbBitmapScaling.Items[i];

                if (String.Equals(s, bitmapscaling))
                {
                    cbBitmapScaling.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetTextureQuality(string texturequality)
        {
            for (int i = 0; i < cbTextureQuality.Items.Count; i++)
            {
                string s = (string)cbTextureQuality.Items[i];

                if (String.Equals(s, texturequality))
                {
                    cbTextureQuality.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetResourceCounts()
        {
            if (resourceManager != null)
            { 
                lblCountRooms.Text = resourceManager.Rooms.Count.ToString();
                lblCountObjects.Text = resourceManager.Objects.Count.ToString();
                lblCountRoomTextures.Text = resourceManager.RoomTextures.Count.ToString();
                lblCountSounds.Text = resourceManager.Wavs.Count.ToString();
                lblCountMusic.Text = resourceManager.Music.Count.ToString();
            }
        }

        private void onLearnButtonClick(object sender, EventArgs e)
        {
            keyLearnForm = new KeyLearnForm();
            keyLearnForm.LinkedButton = (Button)sender;
            keyLearnForm.Options = config;
            keyLearnForm.KeyLearned += keyLearnForm_KeyLearned;
            keyLearnForm.ShowDialog();
        }

        private void keyLearnForm_KeyLearned(object sender, KeyEventArgs e)
        {
            if (keyLearnForm.LinkedButton == btnLearnMoveForward)
            {
                config.KeyBinding.MoveForward = e.KeyCode;
                btnLearnMoveForward.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnMoveBackward)
            {
                config.KeyBinding.MoveBackward = e.KeyCode;
                btnLearnMoveBackward.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnMoveLeft)
            {
                config.KeyBinding.MoveLeft = e.KeyCode;
                btnLearnMoveLeft.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnMoveRight)
            {
                config.KeyBinding.MoveRight = e.KeyCode;
                btnLearnMoveRight.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnRotateLeft)
            {
                config.KeyBinding.RotateLeft = e.KeyCode;
                btnLearnRotateLeft.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnRotateRight)
            {
                config.KeyBinding.RotateRight = e.KeyCode;
                btnLearnRotateRight.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnWalk)
            {
                config.KeyBinding.Walk = e.KeyCode;
                btnLearnWalk.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAutoMove)
            {
                config.KeyBinding.AutoMove = e.KeyCode;
                btnLearnAutoMove.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnNextTarget)
            {
                config.KeyBinding.NextTarget = e.KeyCode;
                btnLearnNextTarget.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnSelfTarget)
            {
                config.KeyBinding.SelfTarget = e.KeyCode;
                btnLearnSelfTarget.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnOpen)
            {
                config.KeyBinding.ReqGo = e.KeyCode;
                btnLearnOpen.Text = e.KeyCode.ToString();
            }
          
            else if (keyLearnForm.LinkedButton == btnLearnAction01)
            {
                config.KeyBinding.ActionButton01 = e.KeyCode;
                btnLearnAction01.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction02)
            {
                config.KeyBinding.ActionButton02 = e.KeyCode;
                btnLearnAction02.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction03)
            {
                config.KeyBinding.ActionButton03 = e.KeyCode;
                btnLearnAction03.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction04)
            {
                config.KeyBinding.ActionButton04 = e.KeyCode;
                btnLearnAction04.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction05)
            {
                config.KeyBinding.ActionButton05 = e.KeyCode;
                btnLearnAction05.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction06)
            {
                config.KeyBinding.ActionButton06 = e.KeyCode;
                btnLearnAction06.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction07)
            {
                config.KeyBinding.ActionButton07 = e.KeyCode;
                btnLearnAction07.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction08)
            {
                config.KeyBinding.ActionButton08 = e.KeyCode;
                btnLearnAction08.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction09)
            {
                config.KeyBinding.ActionButton09 = e.KeyCode;
                btnLearnAction09.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction10)
            {
                config.KeyBinding.ActionButton10 = e.KeyCode;
                btnLearnAction10.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction11)
            {
                config.KeyBinding.ActionButton11 = e.KeyCode;
                btnLearnAction11.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction12)
            {
                config.KeyBinding.ActionButton12 = e.KeyCode;
                btnLearnAction12.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction13)
            {
                config.KeyBinding.ActionButton13 = e.KeyCode;
                btnLearnAction13.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction14)
            {
                config.KeyBinding.ActionButton14 = e.KeyCode;
                btnLearnAction14.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction15)
            {
                config.KeyBinding.ActionButton15 = e.KeyCode;
                btnLearnAction15.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction16)
            {
                config.KeyBinding.ActionButton16 = e.KeyCode;
                btnLearnAction16.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction17)
            {
                config.KeyBinding.ActionButton17 = e.KeyCode;
                btnLearnAction17.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction18)
            {
                config.KeyBinding.ActionButton18 = e.KeyCode;
                btnLearnAction18.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction19)
            {
                config.KeyBinding.ActionButton19 = e.KeyCode;
                btnLearnAction19.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction20)
            {
                config.KeyBinding.ActionButton20 = e.KeyCode;
                btnLearnAction20.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction21)
            {
                config.KeyBinding.ActionButton21 = e.KeyCode;
                btnLearnAction21.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction22)
            {
                config.KeyBinding.ActionButton22 = e.KeyCode;
                btnLearnAction22.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction23)
            {
                config.KeyBinding.ActionButton23 = e.KeyCode;
                btnLearnAction23.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction24)
            {
                config.KeyBinding.ActionButton24 = e.KeyCode;
                btnLearnAction24.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction25)
            {
                config.KeyBinding.ActionButton25 = e.KeyCode;
                btnLearnAction25.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction26)
            {
                config.KeyBinding.ActionButton26 = e.KeyCode;
                btnLearnAction26.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction27)
            {
                config.KeyBinding.ActionButton27 = e.KeyCode;
                btnLearnAction27.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction28)
            {
                config.KeyBinding.ActionButton28 = e.KeyCode;
                btnLearnAction28.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction29)
            {
                config.KeyBinding.ActionButton29 = e.KeyCode;
                btnLearnAction29.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction30)
            {
                config.KeyBinding.ActionButton30 = e.KeyCode;
                btnLearnAction30.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction31)
            {
                config.KeyBinding.ActionButton31 = e.KeyCode;
                btnLearnAction31.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction32)
            {
                config.KeyBinding.ActionButton32 = e.KeyCode;
                btnLearnAction32.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction33)
            {
                config.KeyBinding.ActionButton33 = e.KeyCode;
                btnLearnAction33.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction34)
            {
                config.KeyBinding.ActionButton34 = e.KeyCode;
                btnLearnAction34.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction35)
            {
                config.KeyBinding.ActionButton35 = e.KeyCode;
                btnLearnAction35.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction36)
            {
                config.KeyBinding.ActionButton36 = e.KeyCode;
                btnLearnAction36.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction37)
            {
                config.KeyBinding.ActionButton37 = e.KeyCode;
                btnLearnAction37.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction38)
            {
                config.KeyBinding.ActionButton38 = e.KeyCode;
                btnLearnAction38.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction39)
            {
                config.KeyBinding.ActionButton39 = e.KeyCode;
                btnLearnAction39.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction40)
            {
                config.KeyBinding.ActionButton40 = e.KeyCode;
                btnLearnAction40.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction41)
            {
                config.KeyBinding.ActionButton41 = e.KeyCode;
                btnLearnAction41.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction42)
            {
                config.KeyBinding.ActionButton42 = e.KeyCode;
                btnLearnAction42.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction43)
            {
                config.KeyBinding.ActionButton43 = e.KeyCode;
                btnLearnAction43.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction44)
            {
                config.KeyBinding.ActionButton44 = e.KeyCode;
                btnLearnAction44.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction45)
            {
                config.KeyBinding.ActionButton45 = e.KeyCode;
                btnLearnAction45.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction46)
            {
                config.KeyBinding.ActionButton46 = e.KeyCode;
                btnLearnAction46.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction47)
            {
                config.KeyBinding.ActionButton47 = e.KeyCode;
                btnLearnAction47.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction48)
            {
                config.KeyBinding.ActionButton48 = e.KeyCode;
                btnLearnAction48.Text = e.KeyCode.ToString();
            }
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;

            if (btnSender == btnConnections)
                tabMain.SelectedIndex = 0;

            else if (btnSender == btnEngine)
                tabMain.SelectedIndex = 1;

            else if (btnSender == btnInput)
                tabMain.SelectedIndex = 2;

            else if (btnSender == btnResources)
                tabMain.SelectedIndex = 3;
        }

        private void OptionsForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {
                dragging = false;
            }
        }

        private void OptionsForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));

                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);

                this.Location = pointMoveTo;
            }
        }

        private void OptionsForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConnections.SelectedIndex > -1)
            {
                connectionInfoView1.DataSource = Config.Connections[cbConnections.SelectedIndex];

                string[] dictionaryFiles = Directory.GetFiles(config.ResourcesPath + "/strings/", "*.rsb");
                for (int i = 0; i < dictionaryFiles.Length; i++)
                    dictionaryFiles[i] = Path.GetFileName(dictionaryFiles[i]);

                connectionInfoView1.StringDictionaries = dictionaryFiles;
            }
            else
            {
                connectionInfoView1.DataSource = null;
                connectionInfoView1.StringDictionaries = null;
            }
        }

        private void cbFSAA_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.FSAA = (string)cbFSAA.Items[cbFSAA.SelectedIndex];
        }

        private void cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.Resolution = (string)cbResolution.Items[cbResolution.SelectedIndex];
        }

        private void cbTextureFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.TextureFiltering = (string)cbTextureFiltering.Items[cbTextureFiltering.SelectedIndex];
        }

        private void cbImageBuilder_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.ImageBuilder = (string)cbImageBuilder.Items[cbImageBuilder.SelectedIndex];
        }

        private void cbBitmapScaling_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.BitmapScaling = (string)cbBitmapScaling.Items[cbBitmapScaling.SelectedIndex];
        }

        private void cbTextureQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.TextureQuality = (string)cbTextureQuality.Items[cbTextureQuality.SelectedIndex];
        }

        private void cbRightClickAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.KeyBinding.RightClickAction = cbRightClickAction.SelectedIndex + 1;
        }

        private void cbDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.Display = cbDisplay.SelectedIndex;

            string devicename = (string)cbDisplay.SelectedItem;
            SetResolution(devicename);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            config.Save();
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            config.Connections.RemoveAt(cbConnections.SelectedIndex);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ConnectionInfo newEntry = new ConnectionInfo(
                "NewEntry", 
                "host.example.com", 
                5959, 
                "rsc0000.rsb", 
                "username",
                String.Empty,
                new string[0]);

            config.Connections.Add(newEntry);
            cbConnections.SelectedItem = newEntry;               
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            config.Load();

            // init the legacy resources
            resourceManager.Init(
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHSTRINGS,
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHROOMS,
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHOBJECTS,
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHROOMTEXTURES,
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHSOUNDS,
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHMUSIC,
                config.ResourcesPath + "/" + Meridian59.Common.Config.SUBPATHMAILS);

            Config = config;
        }

        private void btnSelectResourcePath_Click(object sender, EventArgs e)
        {
            if (resourceManager == null || config == null)
                return;

            DialogResult result = fbResourcePath.ShowDialog();

            if (result == DialogResult.OK)
            {
                string path         = fbResourcePath.SelectedPath;
                string pathStrings  = path + "/" + Options.SUBPATHSTRINGS;
                string pathObjects  = path + "/" + Options.SUBPATHOBJECTS; 
                string pathRooms    = path + "/" + Options.SUBPATHROOMS;
                string pathRoomTextures = path + "/" + Options.SUBPATHROOMTEXTURES;
                string pathSounds   = path + "/" + Options.SUBPATHSOUNDS;
                string pathMusic    = path + "/" + Options.SUBPATHMUSIC;
                string pathMails    = path + "/" + Options.SUBPATHMAILS;

                if (Directory.Exists(pathStrings) &&
                    Directory.Exists(pathRooms) &&
                    Directory.Exists(pathObjects) &&
                    Directory.Exists(pathRoomTextures) &&
                    Directory.Exists(pathSounds) &&
                    Directory.Exists(pathMusic) &&
                    Directory.Exists(pathMails))
                {
                    config.ResourcesPath = path;
                    
                    // init new resource folders
                    resourceManager.Init(
                        pathStrings,
                        pathRooms,
                        pathObjects,                        
                        pathRoomTextures,
                        pathSounds,
                        pathMusic,
                        pathMails);
                }
                else
                {
                    string message = "The folder you've selected is missing at least one of these subfolders: " + 
                        Environment.NewLine + Environment.NewLine +
                        Options.SUBPATHSTRINGS + Environment.NewLine +
                        Options.SUBPATHROOMS + Environment.NewLine +
                        Options.SUBPATHOBJECTS + Environment.NewLine +
                        Options.SUBPATHROOMTEXTURES + Environment.NewLine +
                        Options.SUBPATHSOUNDS + Environment.NewLine +
                        Options.SUBPATHMUSIC + Environment.NewLine +
                        Options.SUBPATHMAILS + Environment.NewLine;

                    // tell user about missing path
                    MessageBox.Show(message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly, false);
                }
            }
        }

        private void tbMusicVol_ValueChanged(object sender, EventArgs e)
        {
            config.MusicVolume = tbMusicVol.Value;
        }

        private void tbDecorationIntensity_ValueChanged(object sender, EventArgs e)
        {
            config.DecorationIntensity = tbDecorationIntensity.Value;
        }

        private void tbDecorationQuality_ValueChanged(object sender, EventArgs e)
        {
            config.DecorationQuality = tbDecorationQuality.Value;
        }

        private void tbMouseAimSpeed_ValueChanged(object sender, EventArgs e)
        {
            config.MouseAimSpeed = tbMouseAimSpeed.Value;
        }

        private void tbKeyRotateSpeed_ValueChanged(object sender, EventArgs e)
        {
            config.KeyRotateSpeed = tbKeyRotateSpeed.Value;
        }

        private void tbWeatherParticles_ValueChanged(object sender, EventArgs e)
        {
            config.WeatherParticles = tbWeatherParticles.Value;
        }
    }
}
