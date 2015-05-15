#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{		
	OgreClientConfig::OgreClientConfig()
	{
	};

	void OgreClientConfig::InitPreConfig()
	{
		Config::InitPreConfig();
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
		Display = ::System::Convert::ToInt32(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_RESOLUTION);
		Resolution = Reader[ATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_WINDOWMODE);
		WindowMode = ::System::Convert::ToBoolean(Reader[ATTRIB_ENABLED]);

		Reader->ReadToFollowing(TAG_WINDOWFRAME);
		WindowFrame = ::System::Convert::ToBoolean(Reader[ATTRIB_ENABLED]);

		Reader->ReadToFollowing(TAG_VSYNC);
		VSync = ::System::Convert::ToBoolean(Reader[ATTRIB_ENABLED]);

		Reader->ReadToFollowing(TAG_FSAA);
		FSAA = Reader[ATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_NOMIPMAPS);
		NoMipmaps = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_TEXTUREFILTERING);
		TextureFiltering = Reader[ATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_IMAGEBUILDER);
		ImageBuilder = Reader[ATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_BITMAPSCALING);
		BitmapScaling = Reader[ATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_TEXTUREQUALITY);
		TextureQuality = Reader[ATTRIB_VALUE];

		Reader->ReadToFollowing(TAG_DECORATIONINTENSITY);
		DecorationIntensity = ::System::Convert::ToInt32(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLENEWROOMTEXTURES);
		DisableNewRoomTextures = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLE3DMODELS);
		Disable3DModels = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLENEWSKY);
		DisableNewSky = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLEWEATHEREFFECTS);
		DisableWeatherEffects = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_WEATHERPARTICLES);
		WeatherParticles = ::System::Convert::ToInt32(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_MUSICVOLUME);
		MusicVolume = ::System::Convert::ToSingle(Reader[ATTRIB_VALUE]);
		
		Reader->ReadToFollowing(TAG_SOUNDVOLUME);
		SoundVolume = ::System::Convert::ToSingle(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_DISABLELOOPSOUNDS);
		DisableLoopSounds = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);
		
		/******************************************************************************/
		// input

		Reader->ReadToFollowing(TAG_INPUT);
		Reader->ReadToFollowing(TAG_MOUSEAIMSPEED);
		MouseAimSpeed = ::System::Convert::ToInt32(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_KEYROTATESPEED);
		KeyRotateSpeed = ::System::Convert::ToInt32(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_INVERTMOUSEY);
		InvertMouseY = ::System::Convert::ToBoolean(Reader[ATTRIB_VALUE]);

		Reader->ReadToFollowing(TAG_KEYBINDING);

		KeyBinding = gcnew OISKeyBinding();
		KeyBinding->RightClickAction = ::System::Convert::ToInt32(Reader[ATTRIB_RIGHTCLICKACTION]);

		// movement
		Reader->ReadToFollowing(TAG_MOVEFORWARD);
		KeyBinding->MoveForward = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_MOVEBACKWARD);
		KeyBinding->MoveBackward = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_MOVELEFT);
		KeyBinding->MoveLeft = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_MOVERIGHT);
		KeyBinding->MoveRight = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ROTATELEFT);
		KeyBinding->RotateLeft = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ROTATERIGHT);
		KeyBinding->RotateRight = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		// modifiers
		Reader->ReadToFollowing(TAG_WALK);
		KeyBinding->Walk = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_AUTOMOVE);
		KeyBinding->AutoMove = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		// targetting
		Reader->ReadToFollowing(TAG_NEXTTARGET);
		KeyBinding->NextTarget = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_SELFTARGET);
		KeyBinding->SelfTarget = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		// others
		Reader->ReadToFollowing(TAG_OPEN);
		KeyBinding->ReqGo = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_CLOSE);
		KeyBinding->Close = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		// actions
		Reader->ReadToFollowing(TAG_ACTION01);
		KeyBinding->ActionButton01 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION02);
		KeyBinding->ActionButton02 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION03);
		KeyBinding->ActionButton03 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION04);
		KeyBinding->ActionButton04 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION05);
		KeyBinding->ActionButton05 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION06);
		KeyBinding->ActionButton06 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION07);
		KeyBinding->ActionButton07 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION08);
		KeyBinding->ActionButton08 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION09);
		KeyBinding->ActionButton09 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION10);
		KeyBinding->ActionButton10 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION11);
		KeyBinding->ActionButton11 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION12);
		KeyBinding->ActionButton12 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION13);
		KeyBinding->ActionButton13 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION14);
		KeyBinding->ActionButton14 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION15);
		KeyBinding->ActionButton15 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION16);
		KeyBinding->ActionButton16 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION17);
		KeyBinding->ActionButton17 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION18);
		KeyBinding->ActionButton18 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION19);
		KeyBinding->ActionButton19 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION20);
		KeyBinding->ActionButton20 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION21);
		KeyBinding->ActionButton21 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION22);
		KeyBinding->ActionButton22 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION23);
		KeyBinding->ActionButton23 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION24);
		KeyBinding->ActionButton24 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION25);
		KeyBinding->ActionButton25 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION26);
		KeyBinding->ActionButton26 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION27);
		KeyBinding->ActionButton27 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION28);
		KeyBinding->ActionButton28 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION29);
		KeyBinding->ActionButton29 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION30);
		KeyBinding->ActionButton30 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION31);
		KeyBinding->ActionButton31 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION32);
		KeyBinding->ActionButton32 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION33);
		KeyBinding->ActionButton33 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION34);
		KeyBinding->ActionButton34 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION35);
		KeyBinding->ActionButton35 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION36);
		KeyBinding->ActionButton36 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION37);
		KeyBinding->ActionButton37 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION38);
		KeyBinding->ActionButton38 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION39);
		KeyBinding->ActionButton39 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION40);
		KeyBinding->ActionButton40 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION41);
		KeyBinding->ActionButton41 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION42);
		KeyBinding->ActionButton42 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION43);
		KeyBinding->ActionButton43 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION44);
		KeyBinding->ActionButton44 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION45);
		KeyBinding->ActionButton45 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION46);
		KeyBinding->ActionButton46 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION47);
		KeyBinding->ActionButton47 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

		Reader->ReadToFollowing(TAG_ACTION48);
		KeyBinding->ActionButton48 = (::OIS::KeyCode)::System::Convert::ToUInt32(Reader[ATTRIB_KEY]);

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
		Writer->WriteAttributeString(ATTRIB_VALUE, Display.ToString());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_RESOLUTION);
		Writer->WriteAttributeString(ATTRIB_VALUE, Resolution);
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
		Writer->WriteAttributeString(ATTRIB_VALUE, FSAA);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_NOMIPMAPS);
		Writer->WriteAttributeString(ATTRIB_VALUE, NoMipmaps.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_TEXTUREFILTERING);
		Writer->WriteAttributeString(ATTRIB_VALUE, TextureFiltering);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_IMAGEBUILDER);
		Writer->WriteAttributeString(ATTRIB_VALUE, ImageBuilder);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_BITMAPSCALING);
		Writer->WriteAttributeString(ATTRIB_VALUE, BitmapScaling);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_TEXTUREQUALITY);
		Writer->WriteAttributeString(ATTRIB_VALUE, TextureQuality);
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DECORATIONINTENSITY);
		Writer->WriteAttributeString(ATTRIB_VALUE, DecorationIntensity.ToString());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLENEWROOMTEXTURES);
		Writer->WriteAttributeString(ATTRIB_VALUE, DisableNewRoomTextures.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLE3DMODELS);
		Writer->WriteAttributeString(ATTRIB_VALUE, Disable3DModels.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLENEWSKY);
		Writer->WriteAttributeString(ATTRIB_VALUE, DisableNewSky.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLEWEATHEREFFECTS);
		Writer->WriteAttributeString(ATTRIB_VALUE, DisableWeatherEffects.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_WEATHERPARTICLES);
		Writer->WriteAttributeString(ATTRIB_VALUE, WeatherParticles.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_MUSICVOLUME);
		Writer->WriteAttributeString(ATTRIB_VALUE, MusicVolume.ToString());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_SOUNDVOLUME);
		Writer->WriteAttributeString(ATTRIB_VALUE, SoundVolume.ToString());
		Writer->WriteEndElement();

		Writer->WriteStartElement(TAG_DISABLELOOPSOUNDS);
		Writer->WriteAttributeString(ATTRIB_VALUE, DisableLoopSounds.ToString()->ToLower());
		Writer->WriteEndElement();

		Writer->WriteEndElement();

		/******************************************************************************/
		// input

		Writer->WriteStartElement(TAG_INPUT);

		// mouseaimspeed
		Writer->WriteStartElement(TAG_MOUSEAIMSPEED);
		Writer->WriteAttributeString(ATTRIB_VALUE, MouseAimSpeed.ToString());
		Writer->WriteEndElement();

		// keyrotatespeed
		Writer->WriteStartElement(TAG_KEYROTATESPEED);
		Writer->WriteAttributeString(ATTRIB_VALUE, KeyRotateSpeed.ToString());
		Writer->WriteEndElement();

		// invertmousey
		Writer->WriteStartElement(TAG_INVERTMOUSEY);
		Writer->WriteAttributeString(ATTRIB_VALUE, InvertMouseY.ToString());
		Writer->WriteEndElement();

		// keybinding
		Writer->WriteStartElement(TAG_KEYBINDING);
		Writer->WriteAttributeString(ATTRIB_RIGHTCLICKACTION, KeyBinding->RightClickAction.ToString());

		Writer->WriteStartElement(TAG_MOVEFORWARD);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->MoveForward).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_MOVEBACKWARD);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->MoveBackward).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_MOVELEFT);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->MoveLeft).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_MOVERIGHT);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->MoveRight).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ROTATELEFT);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->RotateLeft).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ROTATERIGHT);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->RotateRight).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_WALK);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->Walk).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_AUTOMOVE);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->AutoMove).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_NEXTTARGET);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->NextTarget).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_SELFTARGET);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->SelfTarget).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_OPEN);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ReqGo).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_CLOSE);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->Close).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION01);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton01).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION02);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton02).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION03);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton03).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION04);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton04).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION05);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton05).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION06);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton06).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION07);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton07).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION08);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton08).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION09);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton09).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION10);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton10).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION11);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton11).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION12);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton12).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION13);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton13).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION14);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton14).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION15);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton15).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION16);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton16).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION17);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton17).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION18);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton18).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION19);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton19).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION20);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton20).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION21);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton21).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION22);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton22).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION23);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton23).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION24);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton24).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION25);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton25).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION26);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton26).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION27);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton27).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION28);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton28).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION29);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton29).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION30);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton30).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION31);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton31).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION32);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton32).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION33);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton33).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION34);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton34).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION35);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton35).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION36);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton36).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION37);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton37).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION38);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton38).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION39);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton39).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION40);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton40).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION41);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton41).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION42);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton42).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION43);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton43).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION44);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton44).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION45);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton45).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION46);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton46).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION47);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton47).ToString());
		Writer->WriteEndElement();
		Writer->WriteStartElement(TAG_ACTION48);
		Writer->WriteAttributeString(ATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton48).ToString());
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