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

#pragma once

#pragma managed(push, off)
#pragma managed(pop)

#include "XmlReaderExtensions.h"
#include "Util.h"
#include "StringConvert.h"

namespace Meridian59 { namespace Ogre
{
   using namespace ::System::Xml;
   using namespace ::Meridian59::Common::Enums;
   using namespace ::Meridian59::Data::Lists;

   /// <summary>
   /// 
   /// </summary>
   public ref class OgreClientConfig : public Config
   {
   public:
      literal int          DEFAULTVAL_ENGINE_DISPLAY                 = 0;
      literal CLRString^   DEFAULTVAL_ENGINE_RESOLUTION              = "1280 x 720 @ 32-bit colour";
      literal bool         DEFAULTVAL_ENGINE_WINDOWMODE              = true;
      literal bool         DEFAULTVAL_ENGINE_WINDOWFRAME             = true;
      literal bool         DEFAULTVAL_ENGINE_VSYNC                   = true;
      literal CLRString^   DEFAULTVAL_ENGINE_FSAA                    = "8";
      literal bool         DEFAULTVAL_ENGINE_NOMIPMAPS               = false;
      literal CLRString^   DEFAULTVAL_ENGINE_TEXFILTERING            = "Anisotropic x16";
      literal CLRString^   DEFAULTVAL_ENGINE_IMAGEBUILDER            = "GDI";
      literal CLRString^   DEFAULTVAL_ENGINE_BITMAPSCALING           = "Default";
      literal CLRString^   DEFAULTVAL_ENGINE_TEXQUALITY              = "Default";
      literal int          DEFAULTVAL_ENGINE_DECORATIONINTENSITY     = 20;
      literal bool         DEFAULTVAL_ENGINE_DISABLENEWROOMTEX       = false;
      literal bool         DEFAULTVAL_ENGINE_DISABLE3DMODELS         = false;
      literal bool         DEFAULTVAL_ENGINE_DISABLENEWSKY           = false;
      literal bool         DEFAULTVAL_ENGINE_DISABLEWEATHEREFFECTS   = false;
      literal float        DEFAULTVAL_ENGINE_BRIGHTNESSFACTOR        = 0.0f;
      literal int          DEFAULTVAL_ENGINE_WEATHERPARTICLES        = 5000;
      literal int          DEFAULTVAL_ENGINE_MUSICVOLUME             = 4;
      literal int          DEFAULTVAL_ENGINE_SOUNDVOLUME             = 10;
      literal bool         DEFAULTVAL_ENGINE_DISABLELOOPSOUNDS       = false;
      literal int          DEFAULTVAL_INPUT_MOUSEAIMSPEED            = 75;
      literal int          DEFAULTVAL_INPUT_MOUSEAIMDISTANCE         = 45;
      literal int          DEFAULTVAL_INPUT_KEYROTATESPEED           = 25;
      literal bool         DEFAULTVAL_INPUT_INVERTMOUSEY             = false;
      literal bool         DEFAULTVAL_INPUT_CAMERACOLLISIONS         = true;
      literal float        DEFAULTVAL_INPUT_CAMERADISTANCEMAX        = 1024.0f;
      literal float        DEFAULTVAL_INPUT_CAMERAPITCHMAX           = 0.25f;
      literal bool         DEFAULTVAL_UI_LOCKED                      = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYAVATAR            = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYTARGET            = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYMINIMAP           = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYROOMENCHANTMENTS  = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYCHAT              = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYINVENTORY         = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYSPELLS            = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYSKILLS            = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYACTIONS           = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYATTRIBUTES        = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYMAINBUTTONSLEFT   = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYMAINBUTTONSRIGHT  = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYACTIONBUTTONGRID  = true;
      literal bool         DEFAULTVAL_UI_VISIBILITYONLINEPLAYERS     = false;
      literal bool         DEFAULTVAL_UI_VISIBILITYROOMOBJECTS       = false;

      literal CLRString^ BUTTONTYPE_SPELL = "spell";
      literal CLRString^ BUTTONTYPE_SKILL = "skill";
      literal CLRString^ BUTTONTYPE_ACTION = "action";
      literal CLRString^ BUTTONTYPE_ITEM = "item";
      literal CLRString^ BUTTONTYPE_ALIAS = "alias";
      literal CLRString^ BUTTONTYPE_UNSET = "unset";

   protected:
      literal CLRString^ TAG_ENGINE = "engine";
      literal CLRString^ TAG_DISPLAY = "display";
      literal CLRString^ TAG_RESOLUTION = "resolution";
      literal CLRString^ TAG_WINDOWMODE = "windowmode";
      literal CLRString^ TAG_WINDOWFRAME = "windowframe";
      literal CLRString^ TAG_VSYNC = "vsync";
      literal CLRString^ TAG_FSAA = "fsaa";
      literal CLRString^ TAG_NOMIPMAPS = "nomipmaps";
      literal CLRString^ TAG_TEXTUREFILTERING = "texturefiltering";
      literal CLRString^ TAG_IMAGEBUILDER = "imagebuilder";
      literal CLRString^ TAG_BITMAPSCALING = "bitmapscaling";
      literal CLRString^ TAG_TEXTUREQUALITY = "texturequality";
      literal CLRString^ TAG_DECORATIONINTENSITY = "decorationintensity";
      literal CLRString^ TAG_DISABLENEWROOMTEXTURES = "disablenewroomtextures";
      literal CLRString^ TAG_DISABLENEWSKY = "disablenewsky";
      literal CLRString^ TAG_DISABLE3DMODELS = "disable3dmodels";
      literal CLRString^ TAG_DISABLEWEATHEREFFECTS = "disableweathereffects";
      literal CLRString^ TAG_BRIGHTNESSFACTOR = "brightnessfactor";
      literal CLRString^ TAG_WEATHERPARTICLES = "weatherparticles";
      literal CLRString^ TAG_MUSICVOLUME = "musicvolume";
      literal CLRString^ TAG_SOUNDVOLUME = "soundvolume";
      literal CLRString^ TAG_DISABLELOOPSOUNDS = "disableloopsounds";
      literal CLRString^ TAG_UI = "ui";
      literal CLRString^ TAG_LAYOUT = "layout";
      literal CLRString^ TAG_AVATAR = "avatar";
      literal CLRString^ TAG_TARGET = "target";
      literal CLRString^ TAG_MINIMAP = "minimap";
      literal CLRString^ TAG_ROOMENCHANTMENTS = "roomenchantments";
      literal CLRString^ TAG_CHAT = "chat";
      literal CLRString^ TAG_INVENTORY = "inventory";
      literal CLRString^ TAG_SPELLS = "spells";
      literal CLRString^ TAG_SKILLS = "skills";
      literal CLRString^ TAG_ACTIONS = "actions";
      literal CLRString^ TAG_ATTRIBUTES = "attributes";
      literal CLRString^ TAG_MAINBUTTONSLEFT = "mainbuttonsleft";
      literal CLRString^ TAG_MAINBUTTONSRIGHT = "mainbuttonsright";
      literal CLRString^ TAG_ACTIONBUTTONGRID = "actionbuttongrid";
      literal CLRString^ TAG_ONLINEPLAYERS = "onlineplayers";
      literal CLRString^ TAG_ROOMOBJECTS = "roomobjects";
      literal CLRString^ TAG_TARGETVISIBILITY = "targetvisibility";
      literal CLRString^ TAG_MINIMAPVISIBILITY = "minimapvisibility";
      literal CLRString^ TAG_CHATVISIBILITY = "chatvisibility";
      literal CLRString^ TAG_INVENTORYVISIBILITY = "inventoryvisibility";
      literal CLRString^ TAG_SPELLSVISIBILITY = "spellsvisibility";
      literal CLRString^ TAG_SKILLSVISIBILITY = "skillsvisibility";
      literal CLRString^ TAG_ACTIONSVISIBILITY = "actionsvisibility";
      literal CLRString^ TAG_ATTRIBUTESVISIBILITY = "attributesvisibility";
      literal CLRString^ TAG_ONLINEPLAYERSVISIBILITY = "onlineplayersvisibility";
      literal CLRString^ TAG_ROOMOBJECTSVISIBILITY = "roomobjectsvisibility";
      literal CLRString^ TAG_INPUT = "input";
      literal CLRString^ TAG_MOUSEAIMSPEED = "mouseaimspeed";
      literal CLRString^ TAG_MOUSEAIMDISTANCE = "mouseaimdistance";
      literal CLRString^ TAG_KEYROTATESPEED = "keyrotatespeed";
      literal CLRString^ TAG_INVERTMOUSEY = "invertmousey";
      literal CLRString^ TAG_CAMERACOLLISIONS = "cameracollisions";
      literal CLRString^ TAG_CAMERADISTANCEMAX = "cameradistancemax";
      literal CLRString^ TAG_CAMERAPITCHMAX = "camerapitchmax";
      literal CLRString^ TAG_KEYBINDING = "keybinding";
      literal CLRString^ TAG_MOVEFORWARD = "moveforward";
      literal CLRString^ TAG_MOVEBACKWARD = "movebackward";
      literal CLRString^ TAG_MOVELEFT = "moveleft";
      literal CLRString^ TAG_MOVERIGHT = "moveright";
      literal CLRString^ TAG_ROTATELEFT = "rotateleft";
      literal CLRString^ TAG_ROTATERIGHT = "rotateright";
      literal CLRString^ TAG_WALK = "walk";
      literal CLRString^ TAG_AUTOMOVE = "automove";
      literal CLRString^ TAG_NEXTTARGET = "nexttarget";
      literal CLRString^ TAG_SELFTARGET = "selftarget";
      literal CLRString^ TAG_OPEN = "open";
      literal CLRString^ TAG_CLOSE = "close";
      literal CLRString^ TAG_ACTION01 = "action01";
      literal CLRString^ TAG_ACTION02 = "action02";
      literal CLRString^ TAG_ACTION03 = "action03";
      literal CLRString^ TAG_ACTION04 = "action04";
      literal CLRString^ TAG_ACTION05 = "action05";
      literal CLRString^ TAG_ACTION06 = "action06";
      literal CLRString^ TAG_ACTION07 = "action07";
      literal CLRString^ TAG_ACTION08 = "action08";
      literal CLRString^ TAG_ACTION09 = "action09";
      literal CLRString^ TAG_ACTION10 = "action10";
      literal CLRString^ TAG_ACTION11 = "action11";
      literal CLRString^ TAG_ACTION12 = "action12";
      literal CLRString^ TAG_ACTION13 = "action13";
      literal CLRString^ TAG_ACTION14 = "action14";
      literal CLRString^ TAG_ACTION15 = "action15";
      literal CLRString^ TAG_ACTION16 = "action16";
      literal CLRString^ TAG_ACTION17 = "action17";
      literal CLRString^ TAG_ACTION18 = "action18";
      literal CLRString^ TAG_ACTION19 = "action19";
      literal CLRString^ TAG_ACTION20 = "action20";
      literal CLRString^ TAG_ACTION21 = "action21";
      literal CLRString^ TAG_ACTION22 = "action22";
      literal CLRString^ TAG_ACTION23 = "action23";
      literal CLRString^ TAG_ACTION24 = "action24";
      literal CLRString^ TAG_ACTION25 = "action25";
      literal CLRString^ TAG_ACTION26 = "action26";
      literal CLRString^ TAG_ACTION27 = "action27";
      literal CLRString^ TAG_ACTION28 = "action28";
      literal CLRString^ TAG_ACTION29 = "action29";
      literal CLRString^ TAG_ACTION30 = "action30";
      literal CLRString^ TAG_ACTION31 = "action31";
      literal CLRString^ TAG_ACTION32 = "action32";
      literal CLRString^ TAG_ACTION33 = "action33";
      literal CLRString^ TAG_ACTION34 = "action34";
      literal CLRString^ TAG_ACTION35 = "action35";
      literal CLRString^ TAG_ACTION36 = "action36";
      literal CLRString^ TAG_ACTION37 = "action37";
      literal CLRString^ TAG_ACTION38 = "action38";
      literal CLRString^ TAG_ACTION39 = "action39";
      literal CLRString^ TAG_ACTION40 = "action40";
      literal CLRString^ TAG_ACTION41 = "action41";
      literal CLRString^ TAG_ACTION42 = "action42";
      literal CLRString^ TAG_ACTION43 = "action43";
      literal CLRString^ TAG_ACTION44 = "action44";
      literal CLRString^ TAG_ACTION45 = "action45";
      literal CLRString^ TAG_ACTION46 = "action46";
      literal CLRString^ TAG_ACTION47 = "action47";
      literal CLRString^ TAG_ACTION48 = "action48";
	  
	  literal CLRString^ TAG_ACTION49 = "action49";
	  literal CLRString^ TAG_ACTION50 = "action50";
	  literal CLRString^ TAG_ACTION51 = "action51";
	  literal CLRString^ TAG_ACTION52 = "action52";
	  literal CLRString^ TAG_ACTION53 = "action53";
	  literal CLRString^ TAG_ACTION54 = "action54";
	  literal CLRString^ TAG_ACTION55 = "action55";
	  literal CLRString^ TAG_ACTION56 = "action56";
	  literal CLRString^ TAG_ACTION57 = "action57";
	  literal CLRString^ TAG_ACTION58 = "action58";
	  literal CLRString^ TAG_ACTION59 = "action59";
	  literal CLRString^ TAG_ACTION60 = "action60";

	  literal CLRString^ TAG_ACTIONBUTTONSETS = "actionbuttonsets";
      literal CLRString^ TAG_ACTIONBUTTONS = "actionbuttons";
      literal CLRString^ TAG_ACTIONBUTTON = "actionbutton";

      literal CLRString^ ATTRIB_COUNT = "count";
      literal CLRString^ ATTRIB_ENABLED = "enabled";
      literal CLRString^ ATTRIB_TYPE = "type";
      literal CLRString^ ATTRIB_PLAYER = "player";
      literal CLRString^ ATTRIB_NUM = "num";
      literal CLRString^ ATTRIB_RIGHTCLICKACTION = "rightclickaction";
      literal CLRString^ ATTRIB_NUMOFSAMENAME = "numofsamename";
      literal CLRString^ XMLATTRIB_XREL = "xrel";
      literal CLRString^ XMLATTRIB_XABS = "xabs";
      literal CLRString^ XMLATTRIB_YREL = "yrel";
      literal CLRString^ XMLATTRIB_YABS = "yabs";
      literal CLRString^ XMLATTRIB_WREL = "wrel";
      literal CLRString^ XMLATTRIB_WABS = "wabs";
      literal CLRString^ XMLATTRIB_HREL = "hrel";
      literal CLRString^ XMLATTRIB_HABS = "habs";
      literal CLRString^ XMLATTRIB_VSBL = "vsbl";
      literal CLRString^ XMLATTRIB_LOCKED = "locked";

   protected:
      ActionButtonType GetButtonType(CLRString^ ButtonType);
      void ReadUILayout(::CEGUI::URect* Layout, XmlNode^ Node, bool DoSize);

   public:
      int Display;
      CLRString^ Resolution;
      bool WindowMode;
      bool WindowFrame;
      bool VSync;
      CLRString^ FSAA;
      bool NoMipmaps;
      CLRString^ TextureFiltering;
      CLRString^ ImageBuilder;
      CLRString^ BitmapScaling;
      CLRString^ TextureQuality;
      int DecorationIntensity;
      bool DisableNewRoomTextures;
      bool Disable3DModels;
      bool DisableNewSky;
      bool DisableWeatherEffects;
      float BrightnessFactor;
      int WeatherParticles;
      float MusicVolume;
      float SoundVolume;
      bool DisableLoopSounds;
      OISKeyBinding^ KeyBinding;
      int MouseAimSpeed;
      int MouseAimDistance;
      int KeyRotateSpeed;
      bool InvertMouseY;
      bool CameraCollisions;
      float CameraDistanceMax;
      float CameraPitchMax;

      bool UILocked;

      bool UIVisibilityTarget;
      bool UIVisibilityMiniMap;
      bool UIVisibilityChat;
      bool UIVisibilityInventory;
      bool UIVisibilitySpells;
      bool UIVisibilitySkills;
      bool UIVisibilityActions;
      bool UIVisibilityAttributes;
      bool UIVisibilityOnlinePlayers;
      bool UIVisibilityRoomObjects;

      ::CEGUI::URect* UILayoutAvatar;
      ::CEGUI::URect* UILayoutTarget;
      ::CEGUI::URect* UILayoutMinimap;
      ::CEGUI::URect* UILayoutRoomEnchantments;
      ::CEGUI::URect* UILayoutChat;
      ::CEGUI::URect* UILayoutInventory;
      ::CEGUI::URect* UILayoutSpells;
      ::CEGUI::URect* UILayoutSkills;
      ::CEGUI::URect* UILayoutActions;
      ::CEGUI::URect* UILayoutAttributes;
      ::CEGUI::URect* UILayoutMainButtonsLeft;
      ::CEGUI::URect* UILayoutMainButtonsRight;
      ::CEGUI::URect* UILayoutActionButtons;
      ::CEGUI::URect* UILayoutOnlinePlayers;
      ::CEGUI::URect* UILayoutRoomObjects;

      ::System::Collections::Generic::List<ActionButtonList^>^ ActionButtonSets =
         gcnew ::System::Collections::Generic::List<ActionButtonList^>();

      OgreClientConfig();

      virtual void ReadXml(::System::Xml::XmlDocument^ Document) override;
      virtual void WriteXml(::System::Xml::XmlWriter^ Writer) override;

      ActionButtonList^ GetActionButtonSetByName(CLRString^ Name);
      void AddOrUpdateActionButtonSet(ActionButtonList^ Buttons);
   };
};};
