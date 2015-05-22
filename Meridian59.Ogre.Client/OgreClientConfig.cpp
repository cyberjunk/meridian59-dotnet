#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{		
	OgreClientConfig::OgreClientConfig() : Config()
	{
		// be warned:
		// base constructor will already start reading, use InitPreConfig() instead
	};

	void OgreClientConfig::InitPreConfig()
	{
		Config::InitPreConfig();

		UILayoutAvatar		= new ::CEGUI::URect();
		UILayoutTarget		= new ::CEGUI::URect();
		UILayoutMinimap		= new ::CEGUI::URect();
		UILayoutChat		= new ::CEGUI::URect();
		UILayoutInventory	= new ::CEGUI::URect();
		UILayoutSpells		= new ::CEGUI::URect();
		UILayoutSkills		= new ::CEGUI::URect();
		UILayoutActions		= new ::CEGUI::URect();
		UILayoutAttributes	= new ::CEGUI::URect();
		UILayoutMainButtonsLeft  = new ::CEGUI::URect();
		UILayoutMainButtonsRight = new ::CEGUI::URect();
		UILayoutActionButtons	= new ::CEGUI::URect();
		UILayoutOnlinePlayers	= new ::CEGUI::URect();
		UILayoutRoomObjects		= new ::CEGUI::URect();
	};

	void OgreClientConfig::InitPastConfig()
	{
		Config::InitPastConfig();
	};

	void OgreClientConfig::ReadXml(::System::Xml::XmlReader^ Reader)
	{
		Config::ReadXml(Reader);

		int count;

		/******************************************************************************/
		// engine

		Reader->ReadToFollowing(TAG_ENGINE);
		Reader->ReadToFollowing(TAG_DISPLAY);
		Display = ::System::Convert::ToInt32(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_RESOLUTION);
		Resolution = Reader[XMLATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_WINDOWMODE);
		WindowMode = ::System::Convert::ToBoolean(Reader[ATTRIB_ENABLED]);

		Reader->ReadToFollowing(TAG_WINDOWFRAME);
		WindowFrame = ::System::Convert::ToBoolean(Reader[ATTRIB_ENABLED]);

		Reader->ReadToFollowing(TAG_VSYNC);
		VSync = ::System::Convert::ToBoolean(Reader[ATTRIB_ENABLED]);

		Reader->ReadToFollowing(TAG_FSAA);
		FSAA = Reader[XMLATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_NOMIPMAPS);
		NoMipmaps = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_TEXTUREFILTERING);
		TextureFiltering = Reader[XMLATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_IMAGEBUILDER);
		ImageBuilder = Reader[XMLATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_BITMAPSCALING);
		BitmapScaling = Reader[XMLATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_TEXTUREQUALITY);
		TextureQuality = Reader[XMLATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_DECORATIONINTENSITY);
		DecorationIntensity = ::System::Convert::ToInt32(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLENEWROOMTEXTURES);
		DisableNewRoomTextures = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLE3DMODELS);
		Disable3DModels = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLENEWSKY);
		DisableNewSky = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLEWEATHEREFFECTS);
		DisableWeatherEffects = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_WEATHERPARTICLES);
		WeatherParticles = ::System::Convert::ToInt32(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_MUSICVOLUME);
		MusicVolume = ::System::Convert::ToSingle(Reader[XMLATTRIB_VALUE], Config::NumberFormatInfo);
		
		Reader->ReadToFollowing(TAG_SOUNDVOLUME);
		SoundVolume = ::System::Convert::ToSingle(Reader[XMLATTRIB_VALUE], Config::NumberFormatInfo);

		Reader->ReadToFollowing(TAG_DISABLELOOPSOUNDS);
		DisableLoopSounds = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);
		
		/******************************************************************************/
		// ui

		Reader->ReadToFollowing(TAG_UI);		
		Reader->ReadToFollowing(TAG_LAYOUT);

		// avatar
		Reader->ReadToFollowing(TAG_AVATAR);
		UILayoutAvatar->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutAvatar->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo), 
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));
		
		// target
		Reader->ReadToFollowing(TAG_TARGET);
		UILayoutTarget->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutTarget->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// minimap
		Reader->ReadToFollowing(TAG_MINIMAP);
		UILayoutMinimap->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutMinimap->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// chat
		Reader->ReadToFollowing(TAG_CHAT);
		UILayoutChat->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutChat->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// inventory
		Reader->ReadToFollowing(TAG_INVENTORY);
		UILayoutInventory->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutInventory->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// spells
		Reader->ReadToFollowing(TAG_SPELLS);
		UILayoutSpells->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutSpells->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// skills
		Reader->ReadToFollowing(TAG_SKILLS);
		UILayoutSkills->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutSkills->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// actions
		Reader->ReadToFollowing(TAG_ACTIONS);
		UILayoutActions->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutActions->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// attributes
		Reader->ReadToFollowing(TAG_ATTRIBUTES);
		UILayoutAttributes->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutAttributes->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// mainbuttonsleft
		Reader->ReadToFollowing(TAG_MAINBUTTONSLEFT);
		UILayoutMainButtonsLeft->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutMainButtonsLeft->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// mainbuttonsright
		Reader->ReadToFollowing(TAG_MAINBUTTONSRIGHT);
		UILayoutMainButtonsRight->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutMainButtonsRight->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// actionbuttons
		Reader->ReadToFollowing(TAG_ACTIONBUTTONGRID);
		UILayoutActionButtons->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutActionButtons->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// onlineplayers
		Reader->ReadToFollowing(TAG_ONLINEPLAYERS);
		UILayoutOnlinePlayers->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutOnlinePlayers->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		// roomobjects
		Reader->ReadToFollowing(TAG_ROOMOBJECTS);
		UILayoutRoomObjects->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_XREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_XABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_YREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_YABS], Config::NumberFormatInfo))));
		UILayoutRoomObjects->setSize(::CEGUI::Size<::CEGUI::UDim>(
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_WREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_WABS], Config::NumberFormatInfo)),
			::CEGUI::UDim(::System::Convert::ToSingle(Reader[XMLATTRIB_HREL], Config::NumberFormatInfo),
			::System::Convert::ToSingle(Reader[XMLATTRIB_HABS], Config::NumberFormatInfo))));

		/******************************************************************************/
		// input

		Reader->ReadToFollowing(TAG_INPUT);
		Reader->ReadToFollowing(TAG_MOUSEAIMSPEED);
		MouseAimSpeed = ::System::Convert::ToInt32(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_KEYROTATESPEED);
		KeyRotateSpeed = ::System::Convert::ToInt32(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_INVERTMOUSEY);
		InvertMouseY = ::System::Convert::ToBoolean(Reader[XMLATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_KEYBINDING);

		KeyBinding = gcnew OISKeyBinding();
		KeyBinding->RightClickAction = ::System::Convert::ToInt32(Reader[ATTRIB_RIGHTCLICKACTION]);

		// movement
		Reader->ReadToFollowing(TAG_MOVEFORWARD);
		KeyBinding->MoveForward = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_MOVEBACKWARD);
		KeyBinding->MoveBackward = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_MOVELEFT);
		KeyBinding->MoveLeft = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_MOVERIGHT);
		KeyBinding->MoveRight = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ROTATELEFT);
		KeyBinding->RotateLeft = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ROTATERIGHT);
		KeyBinding->RotateRight = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		// modifiers
		Reader->ReadToFollowing(TAG_WALK);
		KeyBinding->Walk = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_AUTOMOVE);
		KeyBinding->AutoMove = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		// targetting
		Reader->ReadToFollowing(TAG_NEXTTARGET);
		KeyBinding->NextTarget = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_SELFTARGET);
		KeyBinding->SelfTarget = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		// others
		Reader->ReadToFollowing(TAG_OPEN);
		KeyBinding->ReqGo = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_CLOSE);
		KeyBinding->Close = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		// actions
		Reader->ReadToFollowing(TAG_ACTION01);
		KeyBinding->ActionButton01 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION02);
		KeyBinding->ActionButton02 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION03);
		KeyBinding->ActionButton03 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION04);
		KeyBinding->ActionButton04 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION05);
		KeyBinding->ActionButton05 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION06);
		KeyBinding->ActionButton06 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION07);
		KeyBinding->ActionButton07 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION08);
		KeyBinding->ActionButton08 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION09);
		KeyBinding->ActionButton09 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION10);
		KeyBinding->ActionButton10 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION11);
		KeyBinding->ActionButton11 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION12);
		KeyBinding->ActionButton12 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION13);
		KeyBinding->ActionButton13 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION14);
		KeyBinding->ActionButton14 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION15);
		KeyBinding->ActionButton15 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION16);
		KeyBinding->ActionButton16 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION17);
		KeyBinding->ActionButton17 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION18);
		KeyBinding->ActionButton18 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION19);
		KeyBinding->ActionButton19 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION20);
		KeyBinding->ActionButton20 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION21);
		KeyBinding->ActionButton21 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION22);
		KeyBinding->ActionButton22 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION23);
		KeyBinding->ActionButton23 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION24);
		KeyBinding->ActionButton24 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION25);
		KeyBinding->ActionButton25 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION26);
		KeyBinding->ActionButton26 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION27);
		KeyBinding->ActionButton27 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION28);
		KeyBinding->ActionButton28 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION29);
		KeyBinding->ActionButton29 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION30);
		KeyBinding->ActionButton30 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION31);
		KeyBinding->ActionButton31 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION32);
		KeyBinding->ActionButton32 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION33);
		KeyBinding->ActionButton33 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION34);
		KeyBinding->ActionButton34 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION35);
		KeyBinding->ActionButton35 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION36);
		KeyBinding->ActionButton36 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION37);
		KeyBinding->ActionButton37 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION38);
		KeyBinding->ActionButton38 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION39);
		KeyBinding->ActionButton39 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION40);
		KeyBinding->ActionButton40 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION41);
		KeyBinding->ActionButton41 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION42);
		KeyBinding->ActionButton42 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION43);
		KeyBinding->ActionButton43 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION44);
		KeyBinding->ActionButton44 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION45);
		KeyBinding->ActionButton45 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION46);
		KeyBinding->ActionButton46 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION47);
		KeyBinding->ActionButton47 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION48);
		KeyBinding->ActionButton48 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[XMLATTRIB_KEY]);

		/******************************************************************************/
		// input

		// actionbuttonsets
		Reader->ReadToFollowing(TAG_ACTIONBUTTONSETS);
		count = ::System::Convert::ToInt32(Reader[ATTRIB_COUNT]);
		for (int i = 0; i < count; i++)
		{
			ActionButtonList^ buttonSet = gcnew ActionButtonList();

			// actionbuttons
			Reader->ReadToFollowing(TAG_ACTIONBUTTONS);
			buttonSet->PlayerName = Reader[ATTRIB_PLAYER];
			int num = ::System::Convert::ToInt32(Reader[ATTRIB_COUNT]);
			for (int j = 0; j < num; j++)
			{
				// button
				Reader->ReadToFollowing(TAG_ACTIONBUTTON);

				buttonSet->Add(gcnew ActionButtonConfig(
					::System::Convert::ToInt32(Reader[ATTRIB_NUM]),
					GetButtonType(Reader[ATTRIB_TYPE]),
					Reader[XMLATTRIB_NAME],
					nullptr,
					nullptr,
					::System::Convert::ToUInt32(Reader[ATTRIB_NUMOFSAMENAME])));
			}

			// add buttonset to known ones
			ActionButtonSets->Add(buttonSet);
		}
	};

	void OgreClientConfig::WriteXml(::System::Xml::XmlWriter^ Writer)
	{
		Config::WriteXml(Writer);

		/******************************************************************************/
		// engine

		Writer->WriteStartElement(TAG_ENGINE);

		Writer->WriteStartElement(TAG_DISPLAY);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, Display.ToString());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_RESOLUTION);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, Resolution);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_WINDOWMODE);
		Writer->WriteAttributeString(ATTRIB_ENABLED, WindowMode.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_WINDOWFRAME);
		Writer->WriteAttributeString(ATTRIB_ENABLED, WindowFrame.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_VSYNC);
		Writer->WriteAttributeString(ATTRIB_ENABLED, VSync.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_FSAA);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, FSAA);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_NOMIPMAPS);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, NoMipmaps.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_TEXTUREFILTERING);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, TextureFiltering);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_IMAGEBUILDER);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, ImageBuilder);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_BITMAPSCALING);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, BitmapScaling);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_TEXTUREQUALITY);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, TextureQuality);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DECORATIONINTENSITY);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, DecorationIntensity.ToString());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLENEWROOMTEXTURES);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableNewRoomTextures.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLE3DMODELS);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, Disable3DModels.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLENEWSKY);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableNewSky.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLEWEATHEREFFECTS);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableWeatherEffects.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_WEATHERPARTICLES);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, WeatherParticles.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_MUSICVOLUME);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, MusicVolume.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_SOUNDVOLUME);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, SoundVolume.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLELOOPSOUNDS);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableLoopSounds.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteEndElement();

		/******************************************************************************/
		// ui

		Writer->WriteStartElement(TAG_UI);
		Writer->WriteStartElement(TAG_LAYOUT);

		// avatar
		Writer->WriteStartElement(TAG_AVATAR);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutAvatar->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutAvatar->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutAvatar->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutAvatar->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutAvatar->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutAvatar->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutAvatar->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutAvatar->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// target
		Writer->WriteStartElement(TAG_TARGET);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutTarget->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutTarget->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutTarget->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutTarget->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutTarget->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutTarget->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutTarget->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutTarget->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// minimap
		Writer->WriteStartElement(TAG_MINIMAP);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutMinimap->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutMinimap->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutMinimap->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutMinimap->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutMinimap->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutMinimap->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutMinimap->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutMinimap->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// chat
		Writer->WriteStartElement(TAG_CHAT);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutChat->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutChat->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutChat->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutChat->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutChat->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutChat->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutChat->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutChat->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// inventory
		Writer->WriteStartElement(TAG_INVENTORY);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutInventory->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutInventory->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutInventory->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutInventory->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutInventory->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutInventory->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutInventory->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutInventory->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// spells
		Writer->WriteStartElement(TAG_SPELLS);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutSpells->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutSpells->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutSpells->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutSpells->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutSpells->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutSpells->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutSpells->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutSpells->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// skills
		Writer->WriteStartElement(TAG_SKILLS);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutSkills->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutSkills->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutSkills->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutSkills->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutSkills->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutSkills->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutSkills->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutSkills->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// actions
		Writer->WriteStartElement(TAG_ACTIONS);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutActions->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutActions->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutActions->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutActions->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutActions->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutActions->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutActions->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutActions->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// attributes
		Writer->WriteStartElement(TAG_ATTRIBUTES);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutAttributes->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutAttributes->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutAttributes->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutAttributes->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutAttributes->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutAttributes->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutAttributes->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutAttributes->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// mainbuttonsleft
		Writer->WriteStartElement(TAG_MAINBUTTONSLEFT);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutMainButtonsLeft->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutMainButtonsLeft->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutMainButtonsLeft->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutMainButtonsLeft->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutMainButtonsLeft->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutMainButtonsLeft->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutMainButtonsLeft->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutMainButtonsLeft->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// mainbuttonsrights
		Writer->WriteStartElement(TAG_MAINBUTTONSRIGHT);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutMainButtonsRight->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutMainButtonsRight->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutMainButtonsRight->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutMainButtonsRight->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutMainButtonsRight->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutMainButtonsRight->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutMainButtonsRight->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutMainButtonsRight->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// actionbuttons
		Writer->WriteStartElement(TAG_ACTIONBUTTONGRID);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutActionButtons->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutActionButtons->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutActionButtons->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutActionButtons->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutActionButtons->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutActionButtons->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutActionButtons->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutActionButtons->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// onlineplayers
		Writer->WriteStartElement(TAG_ONLINEPLAYERS);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutOnlinePlayers->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutOnlinePlayers->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutOnlinePlayers->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutOnlinePlayers->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutOnlinePlayers->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutOnlinePlayers->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutOnlinePlayers->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutOnlinePlayers->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		// roomobjects
		Writer->WriteStartElement(TAG_ROOMOBJECTS);
		Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutRoomObjects->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutRoomObjects->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutRoomObjects->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutRoomObjects->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutRoomObjects->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutRoomObjects->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutRoomObjects->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
		Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutRoomObjects->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
		Writer->WriteEndElement();

		Writer->WriteEndElement();
		Writer->WriteEndElement();

		/******************************************************************************/
		// input

		Writer->WriteStartElement(TAG_INPUT);

		// mouseaimspeed
		Writer->WriteStartElement(TAG_MOUSEAIMSPEED);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, MouseAimSpeed.ToString());
		Writer->WriteEndElement();

		// keyrotatespeed
		Writer->WriteStartElement(TAG_KEYROTATESPEED);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, KeyRotateSpeed.ToString());
		Writer->WriteEndElement();

		// invertmousey
		Writer->WriteStartElement(TAG_INVERTMOUSEY);
		Writer->WriteAttributeString(XMLATTRIB_VALUE, InvertMouseY.ToString());
		Writer->WriteEndElement();

		// keybinding
		Writer->WriteStartElement(TAG_KEYBINDING);
		Writer->WriteAttributeString(ATTRIB_RIGHTCLICKACTION, KeyBinding->RightClickAction.ToString());

		Writer->WriteStartElement(TAG_MOVEFORWARD);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveForward).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_MOVEBACKWARD);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveBackward).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_MOVELEFT);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveLeft).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_MOVERIGHT);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveRight).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ROTATELEFT);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->RotateLeft).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ROTATERIGHT);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->RotateRight).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_WALK);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->Walk).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_AUTOMOVE);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->AutoMove).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_NEXTTARGET);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->NextTarget).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_SELFTARGET);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->SelfTarget).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_OPEN);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ReqGo).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_CLOSE);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->Close).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION01);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton01).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION02);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton02).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION03);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton03).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION04);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton04).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION05);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton05).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION06);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton06).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION07);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton07).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION08);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton08).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION09);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton09).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION10);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton10).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION11);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton11).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION12);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton12).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION13);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton13).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION14);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton14).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION15);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton15).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION16);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton16).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION17);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton17).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION18);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton18).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION19);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton19).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION20);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton20).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION21);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton21).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION22);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton22).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION23);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton23).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION24);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton24).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION25);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton25).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION26);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton26).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION27);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton27).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION28);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton28).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION29);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton29).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION30);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton30).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION31);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton31).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION32);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton32).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION33);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton33).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION34);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton34).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION35);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton35).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION36);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton36).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION37);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton37).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION38);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton38).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION39);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton39).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION40);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton40).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION41);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton41).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION42);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton42).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION43);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton43).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION44);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton44).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION45);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton45).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION46);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton46).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION47);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton47).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION48);
		Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton48).ToString());
		Writer->WriteEndElement();

		// keybinding end
		Writer->WriteEndElement();

		// actionbuttonsets
		Writer->WriteStartElement(TAG_ACTIONBUTTONSETS);
		Writer->WriteAttributeString(ATTRIB_COUNT, ActionButtonSets->Count.ToString());

		for (int i = 0; i < ActionButtonSets->Count; i++)
		{
			ActionButtonList^ set = ActionButtonSets[i];

			// actionbuttons
			Writer->WriteStartElement(TAG_ACTIONBUTTONS);
			Writer->WriteAttributeString(ATTRIB_PLAYER, set->PlayerName);
			Writer->WriteAttributeString(ATTRIB_COUNT, set->Count.ToString());

			for (int j = 0; j < set->Count; j++)
			{
				// actionbutton
				Writer->WriteStartElement(TAG_ACTIONBUTTON);
				Writer->WriteAttributeString(ATTRIB_NUM, set[j]->Num.ToString());
				Writer->WriteAttributeString(ATTRIB_TYPE, set[j]->ButtonType.ToString()->ToLower());
				Writer->WriteAttributeString(XMLATTRIB_NAME, set[j]->Name->ToLower());
				Writer->WriteAttributeString(ATTRIB_NUMOFSAMENAME, set[j]->NumOfSameName.ToString());
				Writer->WriteEndElement();
			}

			// actionbuttons end
			Writer->WriteEndElement();
		}

		// actionbuttonsets end
		Writer->WriteEndElement();

		// input end
		Writer->WriteEndElement();
	};

	ActionButtonType OgreClientConfig::GetButtonType(::System::String^ ButtonType)
	{
		if (::System::String::Equals(ButtonType, BUTTONTYPE_SPELL))
			return ActionButtonType::Spell;

		else if (::System::String::Equals(ButtonType, BUTTONTYPE_ACTION))
			return ActionButtonType::Action;

		else if (::System::String::Equals(ButtonType, BUTTONTYPE_ITEM))
			return ActionButtonType::Item;

		else
			return ActionButtonType::Unset;
	};

	ActionButtonList^ OgreClientConfig::GetActionButtonSetByName(::System::String^ Name)
	{
		for each(ActionButtonList^ set in ActionButtonSets)
			if (::System::String::Equals(set->PlayerName, Name))
				return set;

		return nullptr;
	};
};};