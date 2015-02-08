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
        private Options dataSource;

        public OptionsForm()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// The DataSource to display
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(true)]
        public Options DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;

                dataSource.PropertyChanged += dataSource_PropertyChanged;

                cbConnections.DataSource = DataSource.Connections;
                cbConnections.DisplayMember = ConnectionInfo.PROPNAME_NAME;
                cbConnections.SelectedIndex = DataSource.LastConnectionIndex;

                // Engine tab
                SetDisplay();
                //SetResolution(cbDisplay.SelectedText); // is done on trigger in SetDisplay
                SetFSAA(dataSource.FSAA);
                SetTextureFiltering(dataSource.TextureFiltering);
                SetImageBuilder(dataSource.ImageBuilder);
                SetBitmapScaling(dataSource.BitmapScaling);
                SetTextureQuality(dataSource.TextureQuality);

                chkDisableRoomTextures.DataBindings.Clear();
                chkDisableRoomTextures.DataBindings.Add("Checked", DataSource, Options.PROPNAME_DISABLENEWROOMTEXTURES);

                chkDisable3DModels.DataBindings.Clear();
                chkDisable3DModels.DataBindings.Add("Checked", DataSource, Options.PROPNAME_DISABLE3DMODELS);

                chkDisableNewSky.DataBindings.Clear();
                chkDisableNewSky.DataBindings.Add("Checked", DataSource, Options.PROPNAME_DISABLENEWSKY);

                chkWindowMode.DataBindings.Clear();
                chkWindowMode.DataBindings.Add("Checked", DataSource, Options.PROPNAME_WINDOWMODE);

                chkWindowFrame.DataBindings.Clear();
                chkWindowFrame.DataBindings.Add("Checked", DataSource, Options.PROPNAME_WINDOWFRAME);
                
                chkVSync.DataBindings.Clear();
                chkVSync.DataBindings.Add("Checked", DataSource, Options.PROPNAME_VSYNC);
                
                chkMipmaps.DataBindings.Clear();
                chkMipmaps.DataBindings.Add("Checked", DataSource, Options.PROPNAME_NOMIPMAPS);
                
                chkDisableLoopSounds.DataBindings.Clear();
                chkDisableLoopSounds.DataBindings.Add("Checked", DataSource, Options.PROPNAME_DISABLELOOPSOUNDS);

                chkDisableWeatherEffects.DataBindings.Clear();
                chkDisableWeatherEffects.DataBindings.Add("Checked", DataSource, Options.PROPNAME_DISABLEWEATHEREFFECTS);
                
                tbWeatherParticles.Value = dataSource.WeatherParticles;
                tbMusicVol.Value = dataSource.MusicVolume;
                tbDecorationIntensity.Value = dataSource.DecorationIntensity;
                tbDecorationQuality.Value = dataSource.DecorationQuality;

                numResourceVersion.DataBindings.Clear();
                numResourceVersion.DataBindings.Add("Value", DataSource, Options.PROPNAME_RESOURCESVERSION);

                txtResourcePath.DataBindings.Clear();
                txtResourcePath.DataBindings.Add("Text", DataSource, Options.PROPNAME_RESOURCESPATH);

                lblCountRooms.DataBindings.Clear();
                lblCountRooms.DataBindings.Add("Text", DataSource, Options.PROPNAME_COUNTROOMS);

                lblCountObjects.DataBindings.Clear();
                lblCountObjects.DataBindings.Add("Text", DataSource, Options.PROPNAME_COUNTOBJECTS);
                
                lblCountRoomTextures.DataBindings.Clear();
                lblCountRoomTextures.DataBindings.Add("Text", DataSource, Options.PROPNAME_COUNTROOMTEXTURES);

                lblCountSounds.DataBindings.Clear();
                lblCountSounds.DataBindings.Add("Text", DataSource, Options.PROPNAME_COUNTSOUNDS);

                lblCountMusic.DataBindings.Clear();
                lblCountMusic.DataBindings.Add("Text", DataSource, Options.PROPNAME_COUNTMUSIC);
              
                chkPreloadRooms.DataBindings.Clear();
                chkPreloadRooms.DataBindings.Add("Checked", DataSource, Options.PROPNAME_PRELOADROOMS);

                chkPreloadObjects.DataBindings.Clear();
                chkPreloadObjects.DataBindings.Add("Checked", DataSource, Options.PROPNAME_PRELOADOBJECTS);

                chkPreloadRoomTextures.DataBindings.Clear();
                chkPreloadRoomTextures.DataBindings.Add("Checked", DataSource, Options.PROPNAME_PRELOADROOMTEXTURES);

                chkPreloadSound.DataBindings.Clear();
                chkPreloadSound.DataBindings.Add("Checked", DataSource, Options.PROPNAME_PRELOADSOUND);

                chkPreloadMusic.DataBindings.Clear();
                chkPreloadMusic.DataBindings.Add("Checked", DataSource, Options.PROPNAME_PRELOADMUSIC);

                // Input tab
                cbRightClickAction.SelectedIndex = dataSource.KeyBinding.RightClickAction - 1;

                tbMouseAimSpeed.Value = dataSource.MouseAimSpeed;
                tbKeyRotateSpeed.Value = dataSource.KeyRotateSpeed;

                btnLearnMoveForward.Text = dataSource.KeyBinding.MoveForward.ToString();
                btnLearnMoveBackward.Text = dataSource.KeyBinding.MoveBackward.ToString();
                btnLearnMoveLeft.Text = dataSource.KeyBinding.MoveLeft.ToString();
                btnLearnMoveRight.Text = dataSource.KeyBinding.MoveRight.ToString();

                btnLearnRotateLeft.Text = dataSource.KeyBinding.RotateLeft.ToString();
                btnLearnRotateRight.Text = dataSource.KeyBinding.RotateRight.ToString();

                btnLearnWalk.Text = dataSource.KeyBinding.Walk.ToString();
                btnLearnAutoMove.Text = dataSource.KeyBinding.AutoMove.ToString();
                btnLearnNextTarget.Text = dataSource.KeyBinding.NextTarget.ToString();
                btnLearnSelfTarget.Text = dataSource.KeyBinding.SelfTarget.ToString();
                btnLearnOpen.Text = dataSource.KeyBinding.ReqGo.ToString();

                btnLearnAction01.Text = dataSource.KeyBinding.ActionButton01.ToString();
                btnLearnAction02.Text = dataSource.KeyBinding.ActionButton02.ToString();
                btnLearnAction03.Text = dataSource.KeyBinding.ActionButton03.ToString();
                btnLearnAction04.Text = dataSource.KeyBinding.ActionButton04.ToString();
                btnLearnAction05.Text = dataSource.KeyBinding.ActionButton05.ToString();
                btnLearnAction06.Text = dataSource.KeyBinding.ActionButton06.ToString();
                btnLearnAction07.Text = dataSource.KeyBinding.ActionButton07.ToString();
                btnLearnAction08.Text = dataSource.KeyBinding.ActionButton08.ToString();
                btnLearnAction09.Text = dataSource.KeyBinding.ActionButton09.ToString();
                btnLearnAction10.Text = dataSource.KeyBinding.ActionButton10.ToString();
                btnLearnAction11.Text = dataSource.KeyBinding.ActionButton11.ToString();
                btnLearnAction12.Text = dataSource.KeyBinding.ActionButton12.ToString();
                btnLearnAction13.Text = dataSource.KeyBinding.ActionButton13.ToString();
                btnLearnAction14.Text = dataSource.KeyBinding.ActionButton14.ToString();
                btnLearnAction15.Text = dataSource.KeyBinding.ActionButton15.ToString();
                btnLearnAction16.Text = dataSource.KeyBinding.ActionButton16.ToString();
                btnLearnAction17.Text = dataSource.KeyBinding.ActionButton17.ToString();
                btnLearnAction18.Text = dataSource.KeyBinding.ActionButton18.ToString();
                btnLearnAction19.Text = dataSource.KeyBinding.ActionButton19.ToString();
                btnLearnAction20.Text = dataSource.KeyBinding.ActionButton20.ToString();
                btnLearnAction21.Text = dataSource.KeyBinding.ActionButton21.ToString();
                btnLearnAction22.Text = dataSource.KeyBinding.ActionButton22.ToString();
                btnLearnAction23.Text = dataSource.KeyBinding.ActionButton23.ToString();
                btnLearnAction24.Text = dataSource.KeyBinding.ActionButton24.ToString();
                btnLearnAction25.Text = dataSource.KeyBinding.ActionButton25.ToString();
                btnLearnAction26.Text = dataSource.KeyBinding.ActionButton26.ToString();
                btnLearnAction27.Text = dataSource.KeyBinding.ActionButton27.ToString();
                btnLearnAction28.Text = dataSource.KeyBinding.ActionButton28.ToString();
                btnLearnAction29.Text = dataSource.KeyBinding.ActionButton29.ToString();
                btnLearnAction30.Text = dataSource.KeyBinding.ActionButton30.ToString();
                btnLearnAction31.Text = dataSource.KeyBinding.ActionButton31.ToString();
                btnLearnAction32.Text = dataSource.KeyBinding.ActionButton32.ToString();
                btnLearnAction33.Text = dataSource.KeyBinding.ActionButton33.ToString();
                btnLearnAction34.Text = dataSource.KeyBinding.ActionButton34.ToString();
                btnLearnAction35.Text = dataSource.KeyBinding.ActionButton35.ToString();
                btnLearnAction36.Text = dataSource.KeyBinding.ActionButton36.ToString();
                btnLearnAction37.Text = dataSource.KeyBinding.ActionButton37.ToString();
                btnLearnAction38.Text = dataSource.KeyBinding.ActionButton38.ToString();
                btnLearnAction39.Text = dataSource.KeyBinding.ActionButton39.ToString();
                btnLearnAction40.Text = dataSource.KeyBinding.ActionButton40.ToString();
                btnLearnAction41.Text = dataSource.KeyBinding.ActionButton41.ToString();
                btnLearnAction42.Text = dataSource.KeyBinding.ActionButton42.ToString();
                btnLearnAction43.Text = dataSource.KeyBinding.ActionButton43.ToString();
                btnLearnAction44.Text = dataSource.KeyBinding.ActionButton44.ToString();
                btnLearnAction45.Text = dataSource.KeyBinding.ActionButton45.ToString();
                btnLearnAction46.Text = dataSource.KeyBinding.ActionButton46.ToString();
                btnLearnAction47.Text = dataSource.KeyBinding.ActionButton47.ToString();
                btnLearnAction48.Text = dataSource.KeyBinding.ActionButton48.ToString();
            }
        }

        private void dataSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Options.PROPNAME_DECORATIONINTENSITY:
                    tbDecorationIntensity.Value = dataSource.DecorationIntensity;
                    break;

                case Options.PROPNAME_DECORATIONQUALITY:
                    tbDecorationQuality.Value = dataSource.DecorationQuality;
                    break;

                case Options.PROPNAME_MUSICVOLUME:
                    tbMusicVol.Value = dataSource.MusicVolume;
                    break;

                case Options.PROPNAME_MOUSEAIMSPEED:
                    tbMouseAimSpeed.Value = dataSource.MouseAimSpeed;
                    break;

                case Options.PROPNAME_KEYROTATESPEED:
                    tbKeyRotateSpeed.Value = dataSource.KeyRotateSpeed;
                    break;

                case Options.PROPNAME_WEATHERPARTICLES:
                    tbWeatherParticles.Value = dataSource.WeatherParticles;
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

            if (cbDisplay.Items.Count > dataSource.Display)           
                cbDisplay.SelectedIndex = dataSource.Display;          
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

                if (String.Equals(val, dataSource.Resolution))
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

        private void onLearnButtonClick(object sender, EventArgs e)
        {
            keyLearnForm = new KeyLearnForm();
            keyLearnForm.LinkedButton = (Button)sender;
            keyLearnForm.Options = dataSource;
            keyLearnForm.KeyLearned += keyLearnForm_KeyLearned;
            keyLearnForm.ShowDialog();
        }

        private void keyLearnForm_KeyLearned(object sender, KeyEventArgs e)
        {
            if (keyLearnForm.LinkedButton == btnLearnMoveForward)
            {
                dataSource.KeyBinding.MoveForward = e.KeyCode;
                btnLearnMoveForward.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnMoveBackward)
            {
                dataSource.KeyBinding.MoveBackward = e.KeyCode;
                btnLearnMoveBackward.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnMoveLeft)
            {
                dataSource.KeyBinding.MoveLeft = e.KeyCode;
                btnLearnMoveLeft.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnMoveRight)
            {
                dataSource.KeyBinding.MoveRight = e.KeyCode;
                btnLearnMoveRight.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnRotateLeft)
            {
                dataSource.KeyBinding.RotateLeft = e.KeyCode;
                btnLearnRotateLeft.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnRotateRight)
            {
                dataSource.KeyBinding.RotateRight = e.KeyCode;
                btnLearnRotateRight.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnWalk)
            {
                dataSource.KeyBinding.Walk = e.KeyCode;
                btnLearnWalk.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAutoMove)
            {
                dataSource.KeyBinding.AutoMove = e.KeyCode;
                btnLearnAutoMove.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnNextTarget)
            {
                dataSource.KeyBinding.NextTarget = e.KeyCode;
                btnLearnNextTarget.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnSelfTarget)
            {
                dataSource.KeyBinding.SelfTarget = e.KeyCode;
                btnLearnSelfTarget.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnOpen)
            {
                dataSource.KeyBinding.ReqGo = e.KeyCode;
                btnLearnOpen.Text = e.KeyCode.ToString();
            }
          
            else if (keyLearnForm.LinkedButton == btnLearnAction01)
            {
                dataSource.KeyBinding.ActionButton01 = e.KeyCode;
                btnLearnAction01.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction02)
            {
                dataSource.KeyBinding.ActionButton02 = e.KeyCode;
                btnLearnAction02.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction03)
            {
                dataSource.KeyBinding.ActionButton03 = e.KeyCode;
                btnLearnAction03.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction04)
            {
                dataSource.KeyBinding.ActionButton04 = e.KeyCode;
                btnLearnAction04.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction05)
            {
                dataSource.KeyBinding.ActionButton05 = e.KeyCode;
                btnLearnAction05.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction06)
            {
                dataSource.KeyBinding.ActionButton06 = e.KeyCode;
                btnLearnAction06.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction07)
            {
                dataSource.KeyBinding.ActionButton07 = e.KeyCode;
                btnLearnAction07.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction08)
            {
                dataSource.KeyBinding.ActionButton08 = e.KeyCode;
                btnLearnAction08.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction09)
            {
                dataSource.KeyBinding.ActionButton09 = e.KeyCode;
                btnLearnAction09.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction10)
            {
                dataSource.KeyBinding.ActionButton10 = e.KeyCode;
                btnLearnAction10.Text = e.KeyCode.ToString();
            }

            else if (keyLearnForm.LinkedButton == btnLearnAction11)
            {
                dataSource.KeyBinding.ActionButton11 = e.KeyCode;
                btnLearnAction11.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction12)
            {
                dataSource.KeyBinding.ActionButton12 = e.KeyCode;
                btnLearnAction12.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction13)
            {
                dataSource.KeyBinding.ActionButton13 = e.KeyCode;
                btnLearnAction13.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction14)
            {
                dataSource.KeyBinding.ActionButton14 = e.KeyCode;
                btnLearnAction14.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction15)
            {
                dataSource.KeyBinding.ActionButton15 = e.KeyCode;
                btnLearnAction15.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction16)
            {
                dataSource.KeyBinding.ActionButton16 = e.KeyCode;
                btnLearnAction16.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction17)
            {
                dataSource.KeyBinding.ActionButton17 = e.KeyCode;
                btnLearnAction17.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction18)
            {
                dataSource.KeyBinding.ActionButton18 = e.KeyCode;
                btnLearnAction18.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction19)
            {
                dataSource.KeyBinding.ActionButton19 = e.KeyCode;
                btnLearnAction19.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction20)
            {
                dataSource.KeyBinding.ActionButton20 = e.KeyCode;
                btnLearnAction20.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction21)
            {
                dataSource.KeyBinding.ActionButton21 = e.KeyCode;
                btnLearnAction21.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction22)
            {
                dataSource.KeyBinding.ActionButton22 = e.KeyCode;
                btnLearnAction22.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction23)
            {
                dataSource.KeyBinding.ActionButton23 = e.KeyCode;
                btnLearnAction23.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction24)
            {
                dataSource.KeyBinding.ActionButton24 = e.KeyCode;
                btnLearnAction24.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction25)
            {
                dataSource.KeyBinding.ActionButton25 = e.KeyCode;
                btnLearnAction25.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction26)
            {
                dataSource.KeyBinding.ActionButton26 = e.KeyCode;
                btnLearnAction26.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction27)
            {
                dataSource.KeyBinding.ActionButton27 = e.KeyCode;
                btnLearnAction27.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction28)
            {
                dataSource.KeyBinding.ActionButton28 = e.KeyCode;
                btnLearnAction28.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction29)
            {
                dataSource.KeyBinding.ActionButton29 = e.KeyCode;
                btnLearnAction29.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction30)
            {
                dataSource.KeyBinding.ActionButton30 = e.KeyCode;
                btnLearnAction30.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction31)
            {
                dataSource.KeyBinding.ActionButton31 = e.KeyCode;
                btnLearnAction31.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction32)
            {
                dataSource.KeyBinding.ActionButton32 = e.KeyCode;
                btnLearnAction32.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction33)
            {
                dataSource.KeyBinding.ActionButton33 = e.KeyCode;
                btnLearnAction33.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction34)
            {
                dataSource.KeyBinding.ActionButton34 = e.KeyCode;
                btnLearnAction34.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction35)
            {
                dataSource.KeyBinding.ActionButton35 = e.KeyCode;
                btnLearnAction35.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction36)
            {
                dataSource.KeyBinding.ActionButton36 = e.KeyCode;
                btnLearnAction36.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction37)
            {
                dataSource.KeyBinding.ActionButton37 = e.KeyCode;
                btnLearnAction37.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction38)
            {
                dataSource.KeyBinding.ActionButton38 = e.KeyCode;
                btnLearnAction38.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction39)
            {
                dataSource.KeyBinding.ActionButton39 = e.KeyCode;
                btnLearnAction39.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction40)
            {
                dataSource.KeyBinding.ActionButton40 = e.KeyCode;
                btnLearnAction40.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction41)
            {
                dataSource.KeyBinding.ActionButton41 = e.KeyCode;
                btnLearnAction41.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction42)
            {
                dataSource.KeyBinding.ActionButton42 = e.KeyCode;
                btnLearnAction42.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction43)
            {
                dataSource.KeyBinding.ActionButton43 = e.KeyCode;
                btnLearnAction43.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction44)
            {
                dataSource.KeyBinding.ActionButton44 = e.KeyCode;
                btnLearnAction44.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction45)
            {
                dataSource.KeyBinding.ActionButton45 = e.KeyCode;
                btnLearnAction45.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction46)
            {
                dataSource.KeyBinding.ActionButton46 = e.KeyCode;
                btnLearnAction46.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction47)
            {
                dataSource.KeyBinding.ActionButton47 = e.KeyCode;
                btnLearnAction47.Text = e.KeyCode.ToString();
            }
            else if (keyLearnForm.LinkedButton == btnLearnAction48)
            {
                dataSource.KeyBinding.ActionButton48 = e.KeyCode;
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
                connectionInfoView1.DataSource = DataSource.Connections[cbConnections.SelectedIndex];

                string[] dictionaryFiles = Directory.GetFiles(dataSource.ResourcesPath + "/Legacy/", "*.rsb");
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
            dataSource.FSAA = (string)cbFSAA.Items[cbFSAA.SelectedIndex];
        }

        private void cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.Resolution = (string)cbResolution.Items[cbResolution.SelectedIndex];
        }

        private void cbTextureFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.TextureFiltering = (string)cbTextureFiltering.Items[cbTextureFiltering.SelectedIndex];
        }

        private void cbImageBuilder_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.ImageBuilder = (string)cbImageBuilder.Items[cbImageBuilder.SelectedIndex];
        }

        private void cbBitmapScaling_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.BitmapScaling = (string)cbBitmapScaling.Items[cbBitmapScaling.SelectedIndex];
        }

        private void cbTextureQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.TextureQuality = (string)cbTextureQuality.Items[cbTextureQuality.SelectedIndex];
        }

        private void cbRightClickAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.KeyBinding.RightClickAction = cbRightClickAction.SelectedIndex + 1;
        }

        private void cbDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataSource.Display = cbDisplay.SelectedIndex;

            string devicename = (string)cbDisplay.SelectedItem;
            SetResolution(devicename);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dataSource.Save();
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataSource.Connections.RemoveAt(cbConnections.SelectedIndex);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ConnectionInfo newEntry = new ConnectionInfo(
                "NewEntry", 
                "host.example.com", 
                5959, 
                "rsc0000.rsb", 
                "username",
                new string[0]);

            dataSource.Connections.Add(newEntry);
            cbConnections.SelectedItem = newEntry;               
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            dataSource.Load();
            dataSource.InitResourceManager();
            DataSource = dataSource;
        }

        private void btnSelectResourcePath_Click(object sender, EventArgs e)
        {
            DialogResult result = fbResourcePath.ShowDialog();

            if (result == DialogResult.OK)
            {
                string path = fbResourcePath.SelectedPath;

                if (Directory.Exists(path + "/" + Options.SUBPATHOBJECTS) &&
                    Directory.Exists(path + "/" + Options.SUBPATHROOMS) &&
                    Directory.Exists(path + "/" + Options.SUBPATHROOMTEXTURES) &&
                    Directory.Exists(path + "/" + Options.SUBPATHSOUNDS))
                {
                    dataSource.ResourcesPath = path;
                    
                    // load updated config to resourcemanager
                    dataSource.InitResourceManager();
                }
                else
                {
                    string message = "The folder you've selected is missing at least one of these subfolders: " + Environment.NewLine + Environment.NewLine +
                        Options.SUBPATHOBJECTS + Environment.NewLine +
                        Options.SUBPATHROOMS + Environment.NewLine +
                        Options.SUBPATHROOMTEXTURES + Environment.NewLine +
                        Options.SUBPATHSOUNDS + Environment.NewLine;

                    // tell user about missing path
                    MessageBox.Show(message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly, false);
                }
            }
        }

        private void tbMusicVol_ValueChanged(object sender, EventArgs e)
        {
            dataSource.MusicVolume = tbMusicVol.Value;
        }

        private void tbDecorationIntensity_ValueChanged(object sender, EventArgs e)
        {
            dataSource.DecorationIntensity = tbDecorationIntensity.Value;
        }

        private void tbDecorationQuality_ValueChanged(object sender, EventArgs e)
        {
            dataSource.DecorationQuality = tbDecorationQuality.Value;
        }

        private void tbMouseAimSpeed_ValueChanged(object sender, EventArgs e)
        {
            dataSource.MouseAimSpeed = tbMouseAimSpeed.Value;
        }

        private void tbKeyRotateSpeed_ValueChanged(object sender, EventArgs e)
        {
            dataSource.KeyRotateSpeed = tbKeyRotateSpeed.Value;
        }

        private void tbWeatherParticles_ValueChanged(object sender, EventArgs e)
        {
            dataSource.WeatherParticles = tbWeatherParticles.Value;
        }
    }
}
