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
		literal int    DEFAULT_DISPLAY = 0;
		literal ::System::String^ DEFAULT_RESOLUTION = "1024 x 768";
		literal bool   DEFAULT_WINDOWMODE = false;
		literal bool   DEFAULT_VSYNC = false;
		literal ::System::String^ DEFAULT_FSAA = "2";
		literal bool   DEFAULT_NOMIPMAPS = false;
		literal bool   DEFAULT_WINDOWFRAMEENABLED = true;
		literal ::System::String^ DEFAULT_TEXTUREFILTERING = "Bilinear";
		literal ::System::String^ DEFAULT_IMAGEBUILDER = "GDI";
		literal ::System::String^ DEFAULT_BITMAPSCALING = "Default";
		literal ::System::String^ DEFAULT_TEXTUREQUALITY = "Default";
		literal bool   DEFAULT_DISABLE3DMODELS = false;
		literal bool   DEFAULT_DISABLEROOMTEXTURES = false;
		literal bool   DEFAULT_DISABLENEWSKY = false;
		literal bool   DEFAULT_DISABLEWEATHEREFFECTS = false;
		literal int    DEFAULT_WEATHERPARTICLES = 1;
		literal int    DEFAULT_MUSICVOLUME = 10;
		literal bool   DEFAULT_DISABLELOOPSOUNDS = false;
		literal int    DEFAULT_MOUSEAIMSPEED = 25;
		literal int    DEFAULT_KEYROTATESPEED = 25;
		literal bool   DEFAULT_INVERTMOUSEY = false;

		literal ::System::String^ BUTTONTYPE_SPELL = "spell";
		literal ::System::String^ BUTTONTYPE_ACTION = "action";
		literal ::System::String^ BUTTONTYPE_ITEM = "item";
		literal ::System::String^ BUTTONTYPE_UNSET = "unset";

	protected:
		literal ::System::String^ TAG_ENGINE = "engine";
		literal ::System::String^ TAG_DISPLAY = "display";
		literal ::System::String^ TAG_RESOLUTION = "resolution";
		literal ::System::String^ TAG_WINDOWMODE = "windowmode";
		literal ::System::String^ TAG_WINDOWFRAME = "windowframe";
		literal ::System::String^ TAG_VSYNC = "vsync";
		literal ::System::String^ TAG_FSAA = "fsaa";
		literal ::System::String^ TAG_NOMIPMAPS = "nomipmaps";
		literal ::System::String^ TAG_TEXTUREFILTERING = "texturefiltering";
		literal ::System::String^ TAG_IMAGEBUILDER = "imagebuilder";
		literal ::System::String^ TAG_BITMAPSCALING = "bitmapscaling";
		literal ::System::String^ TAG_TEXTUREQUALITY = "texturequality";
		literal ::System::String^ TAG_DECORATIONINTENSITY = "decorationintensity";
		literal ::System::String^ TAG_DISABLENEWROOMTEXTURES = "disablenewroomtextures";
		literal ::System::String^ TAG_DISABLENEWSKY = "disablenewsky";
		literal ::System::String^ TAG_DISABLE3DMODELS = "disable3dmodels";
		literal ::System::String^ TAG_DISABLEWEATHEREFFECTS = "disableweathereffects";
		literal ::System::String^ TAG_WEATHERPARTICLES = "weatherparticles";
		literal ::System::String^ TAG_MUSICVOLUME = "musicvolume";
		literal ::System::String^ TAG_SOUNDVOLUME = "soundvolume";
		literal ::System::String^ TAG_DISABLELOOPSOUNDS = "disableloopsounds";
		literal ::System::String^ TAG_UI = "ui";
		literal ::System::String^ TAG_LAYOUT = "layout";
		literal ::System::String^ TAG_AVATAR = "avatar";
		literal ::System::String^ TAG_TARGET = "target";
		literal ::System::String^ TAG_MINIMAP = "minimap";
		literal ::System::String^ TAG_CHAT = "chat";
		literal ::System::String^ TAG_INVENTORY = "inventory";
		literal ::System::String^ TAG_SPELLS = "spells";
		literal ::System::String^ TAG_SKILLS = "skills";
		literal ::System::String^ TAG_ACTIONS = "actions";
		literal ::System::String^ TAG_ATTRIBUTES = "attributes";
		literal ::System::String^ TAG_MAINBUTTONSLEFT = "mainbuttonsleft";
		literal ::System::String^ TAG_MAINBUTTONSRIGHT = "mainbuttonsright";
		literal ::System::String^ TAG_ACTIONBUTTONGRID = "actionbuttongrid";
		literal ::System::String^ TAG_ONLINEPLAYERS = "onlineplayers";
		literal ::System::String^ TAG_ROOMOBJECTS = "roomobjects";
		literal ::System::String^ TAG_INPUT = "input";
		literal ::System::String^ TAG_MOUSEAIMSPEED = "mouseaimspeed";
		literal ::System::String^ TAG_KEYROTATESPEED = "keyrotatespeed";
		literal ::System::String^ TAG_INVERTMOUSEY = "invertmousey";
		literal ::System::String^ TAG_KEYBINDING = "keybinding";
		literal ::System::String^ TAG_MOVEFORWARD = "moveforward";
		literal ::System::String^ TAG_MOVEBACKWARD = "movebackward";
		literal ::System::String^ TAG_MOVELEFT = "moveleft";
		literal ::System::String^ TAG_MOVERIGHT = "moveright";
		literal ::System::String^ TAG_ROTATELEFT = "rotateleft";
		literal ::System::String^ TAG_ROTATERIGHT = "rotateright";
		literal ::System::String^ TAG_WALK = "walk";
		literal ::System::String^ TAG_AUTOMOVE = "automove";
		literal ::System::String^ TAG_NEXTTARGET = "nexttarget";
		literal ::System::String^ TAG_SELFTARGET = "selftarget";
		literal ::System::String^ TAG_OPEN = "open";
		literal ::System::String^ TAG_CLOSE = "close";
		literal ::System::String^ TAG_ACTION01 = "action01";
		literal ::System::String^ TAG_ACTION02 = "action02";
		literal ::System::String^ TAG_ACTION03 = "action03";
		literal ::System::String^ TAG_ACTION04 = "action04";
		literal ::System::String^ TAG_ACTION05 = "action05";
		literal ::System::String^ TAG_ACTION06 = "action06";
		literal ::System::String^ TAG_ACTION07 = "action07";
		literal ::System::String^ TAG_ACTION08 = "action08";
		literal ::System::String^ TAG_ACTION09 = "action09";
		literal ::System::String^ TAG_ACTION10 = "action10";
		literal ::System::String^ TAG_ACTION11 = "action11";
		literal ::System::String^ TAG_ACTION12 = "action12";
		literal ::System::String^ TAG_ACTION13 = "action13";
		literal ::System::String^ TAG_ACTION14 = "action14";
		literal ::System::String^ TAG_ACTION15 = "action15";
		literal ::System::String^ TAG_ACTION16 = "action16";
		literal ::System::String^ TAG_ACTION17 = "action17";
		literal ::System::String^ TAG_ACTION18 = "action18";
		literal ::System::String^ TAG_ACTION19 = "action19";
		literal ::System::String^ TAG_ACTION20 = "action20";
		literal ::System::String^ TAG_ACTION21 = "action21";
		literal ::System::String^ TAG_ACTION22 = "action22";
		literal ::System::String^ TAG_ACTION23 = "action23";
		literal ::System::String^ TAG_ACTION24 = "action24";
		literal ::System::String^ TAG_ACTION25 = "action25";
		literal ::System::String^ TAG_ACTION26 = "action26";
		literal ::System::String^ TAG_ACTION27 = "action27";
		literal ::System::String^ TAG_ACTION28 = "action28";
		literal ::System::String^ TAG_ACTION29 = "action29";
		literal ::System::String^ TAG_ACTION30 = "action30";
		literal ::System::String^ TAG_ACTION31 = "action31";
		literal ::System::String^ TAG_ACTION32 = "action32";
		literal ::System::String^ TAG_ACTION33 = "action33";
		literal ::System::String^ TAG_ACTION34 = "action34";
		literal ::System::String^ TAG_ACTION35 = "action35";
		literal ::System::String^ TAG_ACTION36 = "action36";
		literal ::System::String^ TAG_ACTION37 = "action37";
		literal ::System::String^ TAG_ACTION38 = "action38";
		literal ::System::String^ TAG_ACTION39 = "action39";
		literal ::System::String^ TAG_ACTION40 = "action40";
		literal ::System::String^ TAG_ACTION41 = "action41";
		literal ::System::String^ TAG_ACTION42 = "action42";
		literal ::System::String^ TAG_ACTION43 = "action43";
		literal ::System::String^ TAG_ACTION44 = "action44";
		literal ::System::String^ TAG_ACTION45 = "action45";
		literal ::System::String^ TAG_ACTION46 = "action46";
		literal ::System::String^ TAG_ACTION47 = "action47";
		literal ::System::String^ TAG_ACTION48 = "action48";
		literal ::System::String^ TAG_ACTIONBUTTONSETS = "actionbuttonsets";
		literal ::System::String^ TAG_ACTIONBUTTONS = "actionbuttons";
		literal ::System::String^ TAG_ACTIONBUTTON = "actionbutton";

		literal ::System::String^ ATTRIB_COUNT = "count";
		literal ::System::String^ ATTRIB_ENABLED = "enabled";
		literal ::System::String^ ATTRIB_TYPE = "type";
		literal ::System::String^ ATTRIB_PLAYER = "player";
		literal ::System::String^ ATTRIB_NUM = "num";
		literal ::System::String^ ATTRIB_RIGHTCLICKACTION = "rightclickaction";
		literal ::System::String^ ATTRIB_NUMOFSAMENAME = "numofsamename";
		literal ::System::String^ XMLATTRIB_XREL = "xrel";
		literal ::System::String^ XMLATTRIB_XABS = "xabs";
		literal ::System::String^ XMLATTRIB_YREL = "yrel";
		literal ::System::String^ XMLATTRIB_YABS = "yabs";
		literal ::System::String^ XMLATTRIB_WREL = "wrel";
		literal ::System::String^ XMLATTRIB_WABS = "wabs";
		literal ::System::String^ XMLATTRIB_HREL = "hrel";
		literal ::System::String^ XMLATTRIB_HABS = "habs";

	protected:
		ActionButtonType GetButtonType(::System::String^ ButtonType);

	public:
		int Display;
		::System::String^ Resolution;
		bool WindowMode;
		bool WindowFrame;
		bool VSync;
		::System::String^ FSAA;
		bool NoMipmaps;
		::System::String^ TextureFiltering;
		::System::String^ ImageBuilder;
		::System::String^ BitmapScaling;
		::System::String^ TextureQuality;
		int DecorationIntensity;
		bool DisableNewRoomTextures;
		bool Disable3DModels;
		bool DisableNewSky;
		bool DisableWeatherEffects;
		int WeatherParticles;
		float MusicVolume;
		float SoundVolume;
		bool DisableLoopSounds;
		OISKeyBinding^ KeyBinding;
		int MouseAimSpeed;
		int KeyRotateSpeed;
		bool InvertMouseY;

		::CEGUI::URect* UILayoutAvatar;
		::CEGUI::URect* UILayoutTarget;
		::CEGUI::URect* UILayoutMinimap;
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

		virtual void InitPreConfig() override;
		virtual void InitPastConfig() override;
		virtual void ReadXml(::System::Xml::XmlReader^ Reader) override;
		virtual void WriteXml(::System::Xml::XmlWriter^ Writer) override;

		ActionButtonList^ GetActionButtonSetByName(::System::String^ Name);
	};
};};