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

using Meridian59.Common.Enums;
using Meridian59.Common.Interfaces;
using Meridian59.Data.Models;
using Meridian59.Files;
using Meridian59.Data.Lists;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Meridian59.Common;

namespace Meridian59.Launcher.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Options : Config, IClearable
    {
        #region Constants

        public const int    DEFAULT_DISPLAY             = 0;
        public const string DEFAULT_RESOLUTION          = "1024 x 768";
        public const bool   DEFAULT_WINDOWMODE          = false;
        public const bool   DEFAULT_VSYNC               = false;
        public const string DEFAULT_FSAA                = "2";
        public const bool   DEFAULT_NOMIPMAPS           = false;
        public const bool   DEFAULT_WINDOWFRAMEENABLED  = true;
        public const string DEFAULT_TEXTUREFILTERING    = "Bilinear";
        public const string DEFAULT_IMAGEBUILDER        = "GDI";
        public const string DEFAULT_BITMAPSCALING       = "Default";
        public const string DEFAULT_TEXTUREQUALITY      = "Default";
        public const bool   DEFAULT_DISABLE3DMODELS     = false;
        public const bool   DEFAULT_DISABLEROOMTEXTURES = false;
        public const bool   DEFAULT_DISABLENEWSKY       = false;
        public const bool   DEFAULT_DISABLEWEATHEREFFECTS = false;
        public const int    DEFAULT_WEATHERPARTICLES    = 1;
        public const int    DEFAULT_MUSICVOLUME         = 10;
        public const bool   DEFAULT_DISABLELOOPSOUNDS   = false;
        public const int    DEFAULT_MOUSEAIMSPEED       = 25;
        public const int    DEFAULT_KEYROTATESPEED      = 25;
        public const bool   DEFAULT_INVERTMOUSEY        = false;

        public const string PROPNAME_DISPLAY                = "Display";
        public const string PROPNAME_RESOLUTION             = "Resolution";
        public const string PROPNAME_WINDOWMODE             = "WindowMode";
        public const string PROPNAME_WINDOWFRAME            = "WindowFrame";
        public const string PROPNAME_VSYNC                  = "VSync";
        public const string PROPNAME_FSAA                   = "FSAA";
        public const string PROPNAME_NOMIPMAPS              = "NoMipmaps";
        public const string PROPNAME_TEXTUREFILTERING       = "TextureFiltering";
        public const string PROPNAME_IMAGEBUILDER           = "ImageBuilder";
        public const string PROPNAME_BITMAPSCALING          = "BitmapScaling";
        public const string PROPNAME_TEXTUREQUALITY         = "TextureQuality";
        public const string PROPNAME_DECORATIONINTENSITY    = "DecorationIntensity";
        public const string PROPNAME_DECORATIONQUALITY      = "DecorationQuality";
        public const string PROPNAME_DISABLENEWROOMTEXTURES = "DisableNewRoomTextures";
        public const string PROPNAME_DISABLE3DMODELS        = "Disable3DModels";
        public const string PROPNAME_DISABLENEWSKY          = "DisableNewSky";
        public const string PROPNAME_DISABLEWEATHEREFFECTS  = "DisableWeatherEffects";
        public const string PROPNAME_WEATHERPARTICLES       = "WeatherParticles";

        public const string PROPNAME_MUSICVOLUME            = "MusicVolume";
        public const string PROPNAME_DISABLELOOPSOUNDS      = "DisableLoopSounds";
        public const string PROPNAME_KEYBINDING             = "KeyBinding";
        public const string PROPNAME_MOUSEAIMSPEED          = "MouseAimSpeed";
        public const string PROPNAME_KEYROTATESPEED         = "KeyRotateSpeed";
        public const string PROPNAME_INVERTMOUSEY           = "InvertMouseY";

        public const string PROPNAME_ACTIONBUTTONSETS           = "ActionButtonSets";

        public const string BUTTONTYPE_SPELL    = "spell";
        public const string BUTTONTYPE_ACTION   = "action";
        public const string BUTTONTYPE_ITEM     = "item";
        public const string BUTTONTYPE_UNSET    = "unset";
        #endregion

        #region XML constants        
        protected const string TAG_ENGINE           = "engine";
        protected const string TAG_DISPLAY          = "display";
        protected const string TAG_RESOLUTION       = "resolution";
        protected const string TAG_WINDOWMODE       = "windowmode";
        protected const string TAG_WINDOWFRAME      = "windowframe";
        protected const string TAG_VSYNC            = "vsync";
        protected const string TAG_FSAA             = "fsaa";
        protected const string TAG_NOMIPMAPS        = "nomipmaps";
        protected const string TAG_TEXTUREFILTERING = "texturefiltering";
        protected const string TAG_IMAGEBUILDER     = "imagebuilder";
        protected const string TAG_BITMAPSCALING    = "bitmapscaling";
        protected const string TAG_TEXTUREQUALITY   = "texturequality";
        protected const string TAG_DECORATIONINTENSITY = "decorationintensity";
        protected const string TAG_DECORATIONQUALITY = "decorationquality";
        protected const string TAG_DISABLENEWROOMTEXTURES = "disablenewroomtextures";
        protected const string TAG_DISABLENEWSKY    = "disablenewsky";
        protected const string TAG_DISABLE3DMODELS  = "disable3dmodels";
        protected const string TAG_DISABLEWEATHEREFFECTS = "disableweathereffects";
        protected const string TAG_WEATHERPARTICLES = "weatherparticles";
        protected const string TAG_MUSICVOLUME      = "musicvolume";
        protected const string TAG_DISABLELOOPSOUNDS = "disableloopsounds";
        protected const string TAG_INPUT            = "input";
        protected const string TAG_MOUSEAIMSPEED    = "mouseaimspeed";
        protected const string TAG_KEYROTATESPEED   = "keyrotatespeed";
        protected const string TAG_INVERTMOUSEY     = "invertmousey";
        protected const string TAG_KEYBINDING       = "keybinding";
        protected const string TAG_MOVEFORWARD      = "moveforward";
        protected const string TAG_MOVEBACKWARD     = "movebackward";
        protected const string TAG_MOVELEFT         = "moveleft";
        protected const string TAG_MOVERIGHT        = "moveright";
        protected const string TAG_ROTATELEFT       = "rotateleft";
        protected const string TAG_ROTATERIGHT      = "rotateright";
        protected const string TAG_WALK             = "walk";
        protected const string TAG_AUTOMOVE         = "automove";
        protected const string TAG_NEXTTARGET       = "nexttarget";
        protected const string TAG_SELFTARGET       = "selftarget";
        protected const string TAG_OPEN             = "open";
        protected const string TAG_CLOSE            = "close";
        protected const string TAG_ACTION01         = "action01";
        protected const string TAG_ACTION02         = "action02";
        protected const string TAG_ACTION03         = "action03";
        protected const string TAG_ACTION04         = "action04";
        protected const string TAG_ACTION05         = "action05";
        protected const string TAG_ACTION06         = "action06";
        protected const string TAG_ACTION07         = "action07";
        protected const string TAG_ACTION08         = "action08";
        protected const string TAG_ACTION09         = "action09";
        protected const string TAG_ACTION10         = "action10";
        protected const string TAG_ACTION11         = "action11";
        protected const string TAG_ACTION12         = "action12";
        protected const string TAG_ACTION13         = "action13";
        protected const string TAG_ACTION14         = "action14";
        protected const string TAG_ACTION15         = "action15";
        protected const string TAG_ACTION16         = "action16";
        protected const string TAG_ACTION17         = "action17";
        protected const string TAG_ACTION18         = "action18";
        protected const string TAG_ACTION19         = "action19";
        protected const string TAG_ACTION20         = "action20";
        protected const string TAG_ACTION21         = "action21";
        protected const string TAG_ACTION22         = "action22";
        protected const string TAG_ACTION23         = "action23";
        protected const string TAG_ACTION24         = "action24";
        protected const string TAG_ACTION25         = "action25";
        protected const string TAG_ACTION26         = "action26";
        protected const string TAG_ACTION27         = "action27";
        protected const string TAG_ACTION28         = "action28";
        protected const string TAG_ACTION29         = "action29";
        protected const string TAG_ACTION30         = "action30";
        protected const string TAG_ACTION31         = "action31";
        protected const string TAG_ACTION32         = "action32";
        protected const string TAG_ACTION33         = "action33";
        protected const string TAG_ACTION34         = "action34";
        protected const string TAG_ACTION35         = "action35";
        protected const string TAG_ACTION36         = "action36";
        protected const string TAG_ACTION37         = "action37";
        protected const string TAG_ACTION38         = "action38";
        protected const string TAG_ACTION39         = "action39";
        protected const string TAG_ACTION40         = "action40";
        protected const string TAG_ACTION41         = "action41";
        protected const string TAG_ACTION42         = "action42";
        protected const string TAG_ACTION43         = "action43";
        protected const string TAG_ACTION44         = "action44";
        protected const string TAG_ACTION45         = "action45";
        protected const string TAG_ACTION46         = "action46";
        protected const string TAG_ACTION47         = "action47";
        protected const string TAG_ACTION48         = "action48";
        protected const string TAG_ACTIONBUTTONSETS = "actionbuttonsets";
        protected const string TAG_ACTIONBUTTONS    = "actionbuttons";
        protected const string TAG_ACTIONBUTTON     = "actionbutton";

        protected const string ATTRIB_COUNT                 = "count";
        protected const string ATTRIB_ENABLED               = "enabled";
        protected const string ATTRIB_VALUE                 = "value";
        protected const string ATTRIB_KEY                   = "key";
        protected const string ATTRIB_TYPE                  = "type";
        protected const string ATTRIB_PLAYER                = "player";
        protected const string ATTRIB_NUM                   = "num";
        protected const string ATTRIB_RIGHTCLICKACTION      = "rightclickaction";
        protected const string ATTRIB_NUMOFSAMENAME         = "numofsamename";
        #endregion

        #region Fields
        protected int display;
        protected string resolution;
        protected bool windowMode;
        protected bool windowFrame;
        protected bool vsync;
        protected string fsaa;
        protected bool nomipmaps;
        protected string textureFiltering;
        protected string imageBuilder;
        protected string bitmapScaling;
        protected string textureQuality;
        protected int decorationIntensity;
        protected int decorationQuality;
        protected bool disableNewRoomTextures;
        protected bool disable3DModels;
        protected bool disableNewSky;
        protected bool disableWeatherEffects;
        protected int weatherParticles;
        protected int musicVolume;
        protected bool disableLoopSounds;
        protected KeyBinding keyBinding;
        protected int mouseAimSpeed;
        protected int keyRotateSpeed;
        protected bool invertMouseY;

        protected KeysConverter keyConverter = new KeysConverter();
        protected List<ActionButtonList> actionButtonSets = new List<ActionButtonList>();
        #endregion

        #region Properties
        public int Display
        {
            get { return display; }
            set
            {
                if (display != value)
                {
                    display = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DISPLAY));
                }
            }
        }

        public string Resolution
        {
            get { return resolution; }
            set
            {
                if (resolution != value)
                {
                    resolution = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_RESOLUTION));
                }
            }
        }

        public bool WindowMode
        {
            get { return windowMode; }
            set
            {
                if (windowMode != value)
                {
                    windowMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_WINDOWMODE));
                }
            }
        }

        public bool WindowFrame
        {
            get { return windowFrame; }
            set
            {
                if (windowFrame != value)
                {
                    windowFrame = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_WINDOWFRAME));
                }
            }
        }

        public bool VSync
        {
            get { return vsync; }
            set
            {
                if (vsync != value)
                {
                    vsync = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_VSYNC));
                }
            }
        }

        public string FSAA
        {
            get { return fsaa; }
            set
            {
                if (fsaa != value)
                {
                    fsaa = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_FSAA));
                }
            }
        }

        public bool NoMipmaps
        {
            get { return nomipmaps; }
            set
            {
                if (nomipmaps != value)
                {
                    nomipmaps = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_NOMIPMAPS));
                }
            }
        }

        public string TextureFiltering
        {
            get { return textureFiltering; }
            set
            {
                if (textureFiltering != value)
                {
                    textureFiltering = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_TEXTUREFILTERING));
                }
            }
        }

        public string ImageBuilder
        {
            get { return imageBuilder; }
            set
            {
                if (imageBuilder != value)
                {
                    imageBuilder = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_IMAGEBUILDER));
                }
            }
        }

        public string BitmapScaling
        {
            get { return bitmapScaling; }
            set
            {
                if (bitmapScaling != value)
                {
                    bitmapScaling = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_BITMAPSCALING));
                }
            }
        }

        public string TextureQuality
        {
            get { return textureQuality; }
            set
            {
                if (textureQuality != value)
                {
                    textureQuality = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_TEXTUREQUALITY));
                }
            }
        }

        public int DecorationIntensity
        {
            get { return decorationIntensity; }
            set
            {
                if (decorationIntensity != value)
                {
                    decorationIntensity = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DECORATIONINTENSITY));
                }
            }
        }

        public int DecorationQuality
        {
            get { return decorationQuality; }
            set
            {
                if (decorationQuality != value)
                {
                    decorationQuality = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DECORATIONQUALITY));
                }
            }
        }

        public bool DisableNewRoomTextures
        {
            get { return disableNewRoomTextures; }
            set
            {
                if (disableNewRoomTextures != value)
                {
                    disableNewRoomTextures = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DISABLENEWROOMTEXTURES));
                }
            }
        }

        public bool Disable3DModels
        {
            get { return disable3DModels; }
            set
            {
                if (disable3DModels != value)
                {
                    disable3DModels = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DISABLE3DMODELS));
                }
            }
        }

        public bool DisableNewSky
        {
            get { return disableNewSky; }
            set
            {
                if (disableNewSky != value)
                {
                    disableNewSky = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DISABLENEWSKY));
                }
            }
        }

        public bool DisableWeatherEffects
        {
            get { return disableWeatherEffects; }
            set
            {
                if (disableWeatherEffects != value)
                {
                    disableWeatherEffects = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DISABLEWEATHEREFFECTS));
                }
            }
        }

        public int WeatherParticles
        {
            get { return weatherParticles; }
            set
            {
                if (weatherParticles != value)
                {
                    weatherParticles = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_WEATHERPARTICLES));
                }
            }
        }

        public int MusicVolume
        {
            get { return musicVolume; }
            set
            {
                if (musicVolume != value)
                {
                    musicVolume = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_MUSICVOLUME));
                }
            }
        }

        public bool DisableLoopSounds
        {
            get { return disableLoopSounds; }
            set
            {
                if (disableLoopSounds != value)
                {
                    disableLoopSounds = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_DISABLELOOPSOUNDS));
                }
            }
        }

        public KeyBinding KeyBinding
        {
            get { return keyBinding; }
            set
            {
                if (keyBinding != value)
                {
                    keyBinding = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_KEYBINDING));
                }
            }
        }

        public int MouseAimSpeed
        {
            get { return mouseAimSpeed; }
            set
            {
                if (mouseAimSpeed != value)
                {
                    mouseAimSpeed = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_MOUSEAIMSPEED));
                }
            }
        }

        public int KeyRotateSpeed
        {
            get { return keyRotateSpeed; }
            set
            {
                if (keyRotateSpeed != value)
                {
                    keyRotateSpeed = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_KEYROTATESPEED));
                }
            }
        }
        public bool InvertMouseY
        {
            get { return invertMouseY; }
            set
            {
                if (invertMouseY != value)
                {
                    invertMouseY = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_INVERTMOUSEY));
                }
            }
        }    

        public List<ActionButtonList> ActionButtonSets
        {
            get { return actionButtonSets; }
            set
            {
                if (actionButtonSets != value)
                {
                    actionButtonSets = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(PROPNAME_ACTIONBUTTONSETS));
                }
            }
        }
        #endregion

        public Options() : base()
        {
        }

        public void Clear(bool RaiseChangedEvent)
        {
            if (!RaiseChangedEvent)
            {
                display = DEFAULT_DISPLAY;
                resolution = DEFAULT_RESOLUTION;
                windowMode = DEFAULT_WINDOWMODE;
                windowFrame = DEFAULT_WINDOWFRAMEENABLED;
                vsync = DEFAULT_VSYNC;
                fsaa = DEFAULT_FSAA;
                nomipmaps = DEFAULT_NOMIPMAPS;
                textureFiltering = DEFAULT_TEXTUREFILTERING;
                keyBinding = KeyBinding.DEFAULT;
                imageBuilder = DEFAULT_IMAGEBUILDER;
                bitmapScaling = DEFAULT_BITMAPSCALING;
                textureQuality = DEFAULT_TEXTUREQUALITY;
                disableNewRoomTextures = DEFAULT_DISABLEROOMTEXTURES;
                disable3DModels = DEFAULT_DISABLE3DMODELS;
                disableNewSky = DEFAULT_DISABLENEWSKY;
                musicVolume = DEFAULT_MUSICVOLUME;
                disableLoopSounds = DEFAULT_DISABLELOOPSOUNDS;
                mouseAimSpeed = DEFAULT_MOUSEAIMSPEED;
                keyRotateSpeed = DEFAULT_KEYROTATESPEED;

                actionButtonSets.Clear();
                connections.Clear();
            }
            else
            {
                Display = DEFAULT_DISPLAY;
                Resolution = DEFAULT_RESOLUTION;
                WindowMode = DEFAULT_WINDOWMODE;
                WindowFrame = DEFAULT_WINDOWFRAMEENABLED;
                VSync = DEFAULT_VSYNC;
                FSAA = DEFAULT_FSAA;
                NoMipmaps = DEFAULT_NOMIPMAPS;
                TextureFiltering = DEFAULT_TEXTUREFILTERING;
                KeyBinding = KeyBinding.DEFAULT;
                ImageBuilder = DEFAULT_IMAGEBUILDER;
                BitmapScaling = DEFAULT_BITMAPSCALING;
                TextureQuality = DEFAULT_TEXTUREQUALITY;
                DisableNewRoomTextures = DEFAULT_DISABLEROOMTEXTURES;
                Disable3DModels = DEFAULT_DISABLE3DMODELS;
                DisableNewSky = DEFAULT_DISABLENEWSKY;
                MusicVolume = DEFAULT_MUSICVOLUME;
                DisableLoopSounds = DEFAULT_DISABLELOOPSOUNDS;
                MouseAimSpeed = DEFAULT_MOUSEAIMSPEED;
                KeyRotateSpeed = DEFAULT_KEYROTATESPEED;

                ActionButtonSets.Clear();
                Connections.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPreConfig()
        {
            base.InitPreConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPastConfig()
        {
            base.InitPastConfig();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Set for this playername or NULL</returns>
        public ActionButtonList GetActionButtonSetByName(string Name)
        {
            foreach (ActionButtonList set in ActionButtonSets)           
                if (String.Equals(set.PlayerName, Name))               
                    return set;

            return null;
        }

        protected ActionButtonType GetButtonType(string ButtonType)
        {
            switch (ButtonType)
            {
                case BUTTONTYPE_SPELL:
                    return ActionButtonType.Spell;

                case BUTTONTYPE_ACTION:
                    return ActionButtonType.Action;

                case BUTTONTYPE_ITEM:
                    return ActionButtonType.Item;

                default:
                    return ActionButtonType.Unset;
            }
        }

        public override void ReadXml(XmlReader Reader)
        {
            base.ReadXml(Reader);

            int count;
           
            /******************************************************************************/

            // engine
            Reader.ReadToFollowing(TAG_ENGINE);
            Reader.ReadToFollowing(TAG_DISPLAY);
            int display_tmp = Convert.ToInt32(Reader[ATTRIB_VALUE]);
            
            Reader.ReadToFollowing(TAG_RESOLUTION);
            string resolution_tmp = Reader[ATTRIB_VALUE];
            
            Reader.ReadToFollowing(TAG_WINDOWMODE);
            bool wndmode = Convert.ToBoolean(Reader[ATTRIB_ENABLED]);

            Reader.ReadToFollowing(TAG_WINDOWFRAME);
            bool wndframeenabled = Convert.ToBoolean(Reader[ATTRIB_ENABLED]);

            Reader.ReadToFollowing(TAG_VSYNC);
            bool vsync = Convert.ToBoolean(Reader[ATTRIB_ENABLED]);

            Reader.ReadToFollowing(TAG_FSAA);
            string fsaa_tmp = Reader[ATTRIB_VALUE];
            
            Reader.ReadToFollowing(TAG_NOMIPMAPS);
            string nomipmaps_tmp = Reader[ATTRIB_VALUE];

            Reader.ReadToFollowing(TAG_TEXTUREFILTERING);
            string anisop_tmp = Reader[ATTRIB_VALUE];

            Reader.ReadToFollowing(TAG_IMAGEBUILDER);
            string imagebuilder = Reader[ATTRIB_VALUE];

            Reader.ReadToFollowing(TAG_BITMAPSCALING);
            string bitmapscaling = Reader[ATTRIB_VALUE];

            Reader.ReadToFollowing(TAG_TEXTUREQUALITY);
            string texturequality = Reader[ATTRIB_VALUE];

            Reader.ReadToFollowing(TAG_DECORATIONINTENSITY);
            int decorationintensity = Convert.ToInt32(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_DECORATIONQUALITY);
            int decorationquality = Convert.ToInt32(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_DISABLENEWROOMTEXTURES);
            bool disablenewroomtextures = Convert.ToBoolean(Reader[ATTRIB_VALUE]);
            
            Reader.ReadToFollowing(TAG_DISABLE3DMODELS);
            bool disable3dmodels = Convert.ToBoolean(Reader[ATTRIB_VALUE]);
            
            Reader.ReadToFollowing(TAG_DISABLENEWSKY);
            bool disablenewsky = Convert.ToBoolean(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_DISABLEWEATHEREFFECTS);
            bool disableweathereffects = Convert.ToBoolean(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_WEATHERPARTICLES);
            int weatherparticles = Convert.ToInt32(Reader[ATTRIB_VALUE]);
            
            Reader.ReadToFollowing(TAG_MUSICVOLUME);
            int musicvolume = Convert.ToInt32(Reader[ATTRIB_VALUE]);
            
            Reader.ReadToFollowing(TAG_DISABLELOOPSOUNDS);
            bool disableloopsounds = Convert.ToBoolean(Reader[ATTRIB_VALUE]);

            // set fields
            Display = display_tmp;
            Resolution = resolution_tmp;
            WindowMode = wndmode;
            WindowFrame = wndframeenabled;
            FSAA = fsaa_tmp;
            VSync = vsync;
            NoMipmaps = Convert.ToBoolean(nomipmaps_tmp);
            TextureFiltering = anisop_tmp;
            ImageBuilder = imagebuilder;
            BitmapScaling = bitmapscaling;
            TextureQuality = texturequality;
            DecorationIntensity = decorationintensity;
            DecorationQuality = decorationquality;
            DisableNewRoomTextures = disablenewroomtextures;
            Disable3DModels = disable3dmodels;
            DisableNewSky = disablenewsky;
            DisableWeatherEffects = disableweathereffects;
            WeatherParticles = weatherparticles;
            MusicVolume = musicvolume;
            DisableLoopSounds = disableloopsounds;

            // input
            Reader.ReadToFollowing(TAG_INPUT);
            Reader.ReadToFollowing(TAG_MOUSEAIMSPEED);
            MouseAimSpeed = Convert.ToInt32(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_KEYROTATESPEED);
            KeyRotateSpeed = Convert.ToInt32(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_INVERTMOUSEY);
            InvertMouseY = Convert.ToBoolean(Reader[ATTRIB_VALUE]);

            Reader.ReadToFollowing(TAG_KEYBINDING);
                      
            keyBinding = new KeyBinding();
            keyBinding.RightClickAction = Convert.ToInt32(Reader[ATTRIB_RIGHTCLICKACTION]);

            // movement
            Reader.ReadToFollowing(TAG_MOVEFORWARD);            
            keyBinding.MoveForward = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_MOVEBACKWARD);
            keyBinding.MoveBackward = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_MOVELEFT);
            keyBinding.MoveLeft = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_MOVERIGHT);
            keyBinding.MoveRight = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ROTATELEFT);
            keyBinding.RotateLeft = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ROTATERIGHT);
            keyBinding.RotateRight = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            // modifiers
            Reader.ReadToFollowing(TAG_WALK);
            keyBinding.Walk = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_AUTOMOVE);
            keyBinding.AutoMove = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            // targetting
            Reader.ReadToFollowing(TAG_NEXTTARGET);
            keyBinding.NextTarget = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_SELFTARGET);
            keyBinding.SelfTarget = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            // others
            Reader.ReadToFollowing(TAG_OPEN);
            keyBinding.ReqGo = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_CLOSE);
            keyBinding.Close = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            // actions
            Reader.ReadToFollowing(TAG_ACTION01);
            keyBinding.ActionButton01 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION02);
            keyBinding.ActionButton02 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION03);
            keyBinding.ActionButton03 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION04);
            keyBinding.ActionButton04 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION05);
            keyBinding.ActionButton05 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION06);
            keyBinding.ActionButton06 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION07);
            keyBinding.ActionButton07 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION08);
            keyBinding.ActionButton08 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION09);
            keyBinding.ActionButton09 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION10);
            keyBinding.ActionButton10 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION11);
            keyBinding.ActionButton11 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION12);
            keyBinding.ActionButton12 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION13);
            keyBinding.ActionButton13 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION14);
            keyBinding.ActionButton14 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION15);
            keyBinding.ActionButton15 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION16);
            keyBinding.ActionButton16 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION17);
            keyBinding.ActionButton17 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION18);
            keyBinding.ActionButton18 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION19);
            keyBinding.ActionButton19 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION20);
            keyBinding.ActionButton20 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION21);
            keyBinding.ActionButton21 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION22);
            keyBinding.ActionButton22 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION23);
            keyBinding.ActionButton23 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION24);
            keyBinding.ActionButton24 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION25);
            keyBinding.ActionButton25 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION26);
            keyBinding.ActionButton26 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION27);
            keyBinding.ActionButton27 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION28);
            keyBinding.ActionButton28 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION29);
            keyBinding.ActionButton29 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION30);
            keyBinding.ActionButton30 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION31);
            keyBinding.ActionButton31 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION32);
            keyBinding.ActionButton32 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION33);
            keyBinding.ActionButton33 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION34);
            keyBinding.ActionButton34 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION35);
            keyBinding.ActionButton35 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION36);
            keyBinding.ActionButton36 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION37);
            keyBinding.ActionButton37 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION38);
            keyBinding.ActionButton38 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION39);
            keyBinding.ActionButton39 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION40);
            keyBinding.ActionButton40 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            Reader.ReadToFollowing(TAG_ACTION41);
            keyBinding.ActionButton41 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION42);
            keyBinding.ActionButton42 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION43);
            keyBinding.ActionButton43 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION44);
            keyBinding.ActionButton44 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION45);
            keyBinding.ActionButton45 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION46);
            keyBinding.ActionButton46 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION47);
            keyBinding.ActionButton47 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);
            
            Reader.ReadToFollowing(TAG_ACTION48);
            keyBinding.ActionButton48 = (Keys)keyConverter.ConvertFrom(Reader[ATTRIB_KEY]);

            // actionbuttonsets
            Reader.ReadToFollowing(TAG_ACTIONBUTTONSETS);
            count = Convert.ToInt32(Reader[ATTRIB_COUNT]);
            for (int i = 0; i < count; i++)
            {
                ActionButtonList buttonSet = new ActionButtonList();

                // actionbuttons
                Reader.ReadToFollowing(TAG_ACTIONBUTTONS);
                buttonSet.PlayerName = Reader[ATTRIB_PLAYER];
                int num = Convert.ToInt32(Reader[ATTRIB_COUNT]);
                for (int j = 0; j < num; j++)
                {
                    // button
                    Reader.ReadToFollowing(TAG_ACTIONBUTTON);
                    
                    buttonSet.Add(new ActionButtonConfig(
                        Convert.ToInt32(Reader[ATTRIB_NUM]), 
                        GetButtonType(Reader[ATTRIB_TYPE]), 
                        Reader[XMLATTRIB_NAME], 
                        null, 
                        null,
                        Convert.ToUInt32(Reader[ATTRIB_NUMOFSAMENAME])));
                }
                
                // add buttonset to known ones
                actionButtonSets.Add(buttonSet);
            }
        }

        public override void WriteXml(XmlWriter Writer)
        {
            base.WriteXml(Writer);

            // engine
            Writer.WriteStartElement(TAG_ENGINE);

            Writer.WriteStartElement(TAG_DISPLAY);
            Writer.WriteAttributeString(ATTRIB_VALUE, display.ToString());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_RESOLUTION);
            Writer.WriteAttributeString(ATTRIB_VALUE, resolution);
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_WINDOWMODE);
            Writer.WriteAttributeString(ATTRIB_ENABLED, windowMode.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_WINDOWFRAME);
            Writer.WriteAttributeString(ATTRIB_ENABLED, windowFrame.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_VSYNC);
            Writer.WriteAttributeString(ATTRIB_ENABLED, vsync.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_FSAA);
            Writer.WriteAttributeString(ATTRIB_VALUE, fsaa);
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_NOMIPMAPS);
            Writer.WriteAttributeString(ATTRIB_VALUE, nomipmaps.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_TEXTUREFILTERING);
            Writer.WriteAttributeString(ATTRIB_VALUE, textureFiltering);
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_IMAGEBUILDER);
            Writer.WriteAttributeString(ATTRIB_VALUE, imageBuilder);
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_BITMAPSCALING);
            Writer.WriteAttributeString(ATTRIB_VALUE, bitmapScaling);
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_TEXTUREQUALITY);
            Writer.WriteAttributeString(ATTRIB_VALUE, textureQuality);
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_DECORATIONINTENSITY);
            Writer.WriteAttributeString(ATTRIB_VALUE, decorationIntensity.ToString());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_DECORATIONQUALITY);
            Writer.WriteAttributeString(ATTRIB_VALUE, decorationQuality.ToString());
            Writer.WriteEndElement();
            
            Writer.WriteStartElement(TAG_DISABLENEWROOMTEXTURES);
            Writer.WriteAttributeString(ATTRIB_VALUE, disableNewRoomTextures.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_DISABLE3DMODELS);
            Writer.WriteAttributeString(ATTRIB_VALUE, disable3DModels.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_DISABLENEWSKY);
            Writer.WriteAttributeString(ATTRIB_VALUE, disableNewSky.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_DISABLEWEATHEREFFECTS);
            Writer.WriteAttributeString(ATTRIB_VALUE, disableWeatherEffects.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_WEATHERPARTICLES);
            Writer.WriteAttributeString(ATTRIB_VALUE, weatherParticles.ToString().ToLower());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_MUSICVOLUME);
            Writer.WriteAttributeString(ATTRIB_VALUE, musicVolume.ToString());
            Writer.WriteEndElement();

            Writer.WriteStartElement(TAG_DISABLELOOPSOUNDS);
            Writer.WriteAttributeString(ATTRIB_VALUE, disableLoopSounds.ToString().ToLower());
            Writer.WriteEndElement();

            // engine end
            Writer.WriteEndElement();

            // input
            Writer.WriteStartElement(TAG_INPUT);

            // mouseaimspeed
            Writer.WriteStartElement(TAG_MOUSEAIMSPEED);          
            Writer.WriteAttributeString(ATTRIB_VALUE, mouseAimSpeed.ToString());
            Writer.WriteEndElement();

            // keyrotatespeed
            Writer.WriteStartElement(TAG_KEYROTATESPEED);
            Writer.WriteAttributeString(ATTRIB_VALUE, keyRotateSpeed.ToString());
            Writer.WriteEndElement();

            // invertmousey
            Writer.WriteStartElement(TAG_INVERTMOUSEY);
            Writer.WriteAttributeString(ATTRIB_VALUE, invertMouseY.ToString());
            Writer.WriteEndElement();

            // keybinding
            Writer.WriteStartElement(TAG_KEYBINDING);
            Writer.WriteAttributeString(ATTRIB_RIGHTCLICKACTION, keyBinding.RightClickAction.ToString());
            
            Writer.WriteStartElement(TAG_MOVEFORWARD);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.MoveForward.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_MOVEBACKWARD);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.MoveBackward.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_MOVELEFT);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.MoveLeft.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_MOVERIGHT);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.MoveRight.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ROTATELEFT);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.RotateLeft.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ROTATERIGHT);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.RotateRight.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_WALK);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.Walk.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_AUTOMOVE);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.AutoMove.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_NEXTTARGET);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.NextTarget.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_SELFTARGET);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.SelfTarget.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_OPEN);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ReqGo.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_CLOSE);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.Close.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION01);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton01.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION02);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton02.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION03);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton03.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION04);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton04.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION05);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton05.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION06);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton06.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION07);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton07.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION08);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton08.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION09);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton09.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION10);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton10.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION11);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton11.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION12);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton12.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION13);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton13.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION14);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton14.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION15);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton15.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION16);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton16.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION17);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton17.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION18);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton18.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION19);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton19.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION20);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton20.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION21);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton21.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION22);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton22.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION23);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton23.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION24);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton24.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION25);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton25.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION26);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton26.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION27);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton27.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION28);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton28.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION29);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton29.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION30);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton30.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION31);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton31.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION32);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton32.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION33);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton33.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION34);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton34.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION35);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton35.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION36);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton36.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION37);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton37.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION38);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton38.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION39);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton39.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION40);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton40.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION41);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton41.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION42);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton42.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION43);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton43.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION44);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton44.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION45);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton45.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION46);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton46.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION47);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton47.ToString());
            Writer.WriteEndElement();
            Writer.WriteStartElement(TAG_ACTION48);
            Writer.WriteAttributeString(ATTRIB_KEY, keyBinding.ActionButton48.ToString());
            Writer.WriteEndElement();

            // keybinding end
            Writer.WriteEndElement();

            // actionbuttonsets
            Writer.WriteStartElement(TAG_ACTIONBUTTONSETS);
            Writer.WriteAttributeString(ATTRIB_COUNT, actionButtonSets.Count.ToString());

            for (int i = 0; i < actionButtonSets.Count; i++)
            {
                // actionbuttons
                Writer.WriteStartElement(TAG_ACTIONBUTTONS);
                Writer.WriteAttributeString(ATTRIB_PLAYER, actionButtonSets[i].PlayerName);
                Writer.WriteAttributeString(ATTRIB_COUNT, actionButtonSets[i].Count.ToString());

                for (int j = 0; j < actionButtonSets[i].Count; j++)
                {
                    // actionbutton
                    Writer.WriteStartElement(TAG_ACTIONBUTTON);
                    Writer.WriteAttributeString(ATTRIB_NUM, actionButtonSets[i][j].Num.ToString());
                    Writer.WriteAttributeString(ATTRIB_TYPE, actionButtonSets[i][j].ButtonType.ToString().ToLower());
                    Writer.WriteAttributeString(XMLATTRIB_NAME, actionButtonSets[i][j].Name.ToLower());
                    Writer.WriteAttributeString(ATTRIB_NUMOFSAMENAME, actionButtonSets[i][j].NumOfSameName.ToString());
                    Writer.WriteEndElement();
                }

                // actionbuttons end
                Writer.WriteEndElement();
            }
            
            // actionbuttonsets end
            Writer.WriteEndElement();

            // input end
            Writer.WriteEndElement();
        }
    }
}
