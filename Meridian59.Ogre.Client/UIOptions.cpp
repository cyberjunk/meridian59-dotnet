#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Options::Initialize()
	{
		/******************************************************************************************************/
		/*                                       GET UI ELEMENTS                                              */
		/******************************************************************************************************/

		// setup references to children from xml nodes
		Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_OPTIONS_WINDOW));

		// category buttons
		Connections = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_CONNECTIONS));
		Engine		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ENGINE));
		Input		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_INPUT));
		Resources	= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_RESOURCES));

		// tabcontrol and tabs
		TabControl		= static_cast<CEGUI::TabControl*>(Window->getChild(UI_NAME_OPTIONS_TABCONTROL));		
		TabConnections	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABCONNECTIONS));
		TabEngine		= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABENGINE));
		TabInput		= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABINPUT));
		TabResources	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABRESOURCES));

		/******************************************************************************************************/

		// tabinput children
		TabInputTabControl = static_cast<CEGUI::TabControl*>(TabInput->getChild(UI_NAME_OPTIONS_TABINPUT_TABCONTROL));
		TabInputTabGeneral = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL));
		TabInputTabActionButtons1 = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1));
		TabInputTabActionButtons2 = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2));

		// tabinput - tabgeneral
		LearnMoveForward	= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVEFORWARD));
		LearnMoveBackward	= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVEBACKWARD));
		LearnMoveLeft		= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVELEFT));
		LearnMoveRight		= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVERIGHT));
		LearnRotateLeft		= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_ROTATELEFT));
		LearnRotateRight	= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_ROTATERIGHT));
		LearnWalk			= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_WALK));
		LearnAutoMove		= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_AUTOMOVE));
		LearnNextTarget		= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_NEXTTARGET));
		LearnOpen			= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_OPEN));
		LearnClose			= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_CLOSE));
		MouseAimSpeed		= static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOUSEAIMSPEED));
		KeyRotateSpeed		= static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_KEYROTATESPEED));
		RightClickAction	= static_cast<CEGUI::Combobox*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_RIGHTCLICKACTION));
		InvertMouseY		= static_cast<CEGUI::ToggleButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_INVERTMOUSEY));

		// tabinput - tabactionbuttons1
		LearnAction01 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON01));
		LearnAction02 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON02));
		LearnAction03 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON03));
		LearnAction04 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON04));
		LearnAction05 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON05));
		LearnAction06 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON06));
		LearnAction07 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON07));
		LearnAction08 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON08));
		LearnAction09 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON09));
		LearnAction10 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON10));
		LearnAction11 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON11));
		LearnAction12 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON12));
		LearnAction13 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON13));
		LearnAction14 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON14));
		LearnAction15 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON15));
		LearnAction16 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON16));
		LearnAction17 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON17));
		LearnAction18 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON18));
		LearnAction19 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON19));
		LearnAction20 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON20));
		LearnAction21 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON21));
		LearnAction22 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON22));
		LearnAction23 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON23));
		LearnAction24 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons1->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1_BUTTON24));

		// tabinput - tabactionbuttons2
		LearnAction25 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON25));
		LearnAction26 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON26));
		LearnAction27 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON27));
		LearnAction28 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON28));
		LearnAction29 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON29));
		LearnAction30 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON30));
		LearnAction31 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON31));
		LearnAction32 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON32));
		LearnAction33 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON33));
		LearnAction34 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON34));
		LearnAction35 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON35));
		LearnAction36 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON36));
		LearnAction37 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON37));
		LearnAction38 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON38));
		LearnAction39 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON39));
		LearnAction40 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON40));
		LearnAction41 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON41));
		LearnAction42 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON42));
		LearnAction43 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON43));
		LearnAction44 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON44));
		LearnAction45 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON45));
		LearnAction46 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON46));
		LearnAction47 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON47));
		LearnAction48 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons2->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2_BUTTON48));

		/******************************************************************************************************/

		// tabengine
		Display			= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISPLAY));
		Resolution		= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_RESOLUTION));
		WindowMode		= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_WINDOWMODE));
		WindowBorders	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_WINDOWBORDERS));
		VSync			= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_VSYNC));
		FSAA			= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_FSAA));
		Filtering		= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_FILTERING));
		ImageBuilder	= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_IMAGEBUILDER));
		ScalingQuality	= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_SCALINGQUALITY));
		TextureQuality	= static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_TEXTUREQUALITY));
		DisableMipmaps = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLEMIPMAPS));
		DisableNewRoomTextures = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLENEWROOMTEXTURES));
		Disable3DModels = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLE3DMODELS));
		DisableNewSky	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLENEWSKY));
		DisableWeather	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLEWEATHER));
		Particles		= static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PARTICLES));
		Decoration		= static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DECORATION));
		MusicVolume		= static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_MUSICVOLUME));
		SoundVolume		= static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_SOUNDVOLUME));
		DisableLoopSounds = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLELOOPSOUNDS));

		
		/******************************************************************************************************/
		/*                                  PREPARE / SET: ENGINE                                             */
		/******************************************************************************************************/

		// Filtering values
		Filtering->addItem(new::CEGUI::ListboxTextItem("Off"));
		Filtering->addItem(new::CEGUI::ListboxTextItem("Bilinear"));
		Filtering->addItem(new::CEGUI::ListboxTextItem("Trilinear"));
		Filtering->addItem(new::CEGUI::ListboxTextItem("Anisotropic x4"));
		Filtering->addItem(new::CEGUI::ListboxTextItem("Anisotropic x16"));

		// ImageBuilder values
		ImageBuilder->addItem(new::CEGUI::ListboxTextItem("GDI"));
		ImageBuilder->addItem(new::CEGUI::ListboxTextItem("DirectDraw"));

		// ScalingQuality values
		ScalingQuality->addItem(new::CEGUI::ListboxTextItem("Low"));
		ScalingQuality->addItem(new::CEGUI::ListboxTextItem("Default"));
		ScalingQuality->addItem(new::CEGUI::ListboxTextItem("High"));

		// TextureQuality values
		TextureQuality->addItem(new::CEGUI::ListboxTextItem("Low"));
		TextureQuality->addItem(new::CEGUI::ListboxTextItem("Default"));
		TextureQuality->addItem(new::CEGUI::ListboxTextItem("High"));

		// maxvalues for particles & decoration sliders
		Particles->setMaxValue(50000.0f);
		Decoration->setMaxValue(100.0f);

		/******************************************************************************************************/

		Ogre::ConfigOptionMap map = OgreClient::Singleton->RenderSystem->getConfigOptions();
		Ogre::ConfigOptionMap::iterator result;
		Ogre::ConfigOption option;

		result = map.find("Rendering Device");
		if (result != map.end())
		{
			option = result->second;
			for (Ogre::StringVector::iterator i(option.possibleValues.begin()), iEnd(option.possibleValues.end()); i != iEnd; ++i)
				Display->addItem(new::CEGUI::ListboxTextItem(i->c_str()));
		}

		result = map.find("Video Mode");
		if (result != map.end())
		{
			option = result->second;
			for (Ogre::StringVector::iterator i(option.possibleValues.begin()), iEnd(option.possibleValues.end()); i != iEnd; ++i)
				Resolution->addItem(new::CEGUI::ListboxTextItem(i->c_str()));
		}

		result = map.find("FSAA");
		if (result != map.end())
		{
			option = result->second;
			for (Ogre::StringVector::iterator i(option.possibleValues.begin()), iEnd(option.possibleValues.end()); i != iEnd; ++i)
				FSAA->addItem(new::CEGUI::ListboxTextItem(i->c_str()));
		}

		Display->setSelection(OgreClient::Singleton->Config->Display, OgreClient::Singleton->Config->Display);
		Resolution->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->Resolution));

		WindowMode->setSelected(OgreClient::Singleton->Config->WindowMode);
		WindowBorders->setSelected(OgreClient::Singleton->Config->WindowFrame);
		VSync->setSelected(OgreClient::Singleton->Config->VSync);

		FSAA->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->FSAA));
		Filtering->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->TextureFiltering));
		ImageBuilder->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->ImageBuilder));
		ScalingQuality->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->BitmapScaling));
		TextureQuality->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->TextureQuality));
		
		Display->selectListItemWithEditboxText();
		Resolution->selectListItemWithEditboxText();
		FSAA->selectListItemWithEditboxText();
		Filtering->selectListItemWithEditboxText();
		ImageBuilder->selectListItemWithEditboxText();
		ScalingQuality->selectListItemWithEditboxText();
		TextureQuality->selectListItemWithEditboxText();
		
		DisableMipmaps->setSelected(!OgreClient::Singleton->Config->NoMipmaps);
		DisableNewRoomTextures->setSelected(!OgreClient::Singleton->Config->DisableNewRoomTextures);
		Disable3DModels->setSelected(!OgreClient::Singleton->Config->Disable3DModels);
		DisableNewSky->setSelected(!OgreClient::Singleton->Config->DisableNewSky);
		DisableWeather->setSelected(!OgreClient::Singleton->Config->DisableWeatherEffects);

		Particles->setCurrentValue(OgreClient::Singleton->Config->WeatherParticles);
		Decoration->setCurrentValue(OgreClient::Singleton->Config->DecorationIntensity);
		MusicVolume->setCurrentValue(OgreClient::Singleton->Config->MusicVolume);
		//SoundVolume->setCurrentValue(OgreClient::Singleton->Config->SoundVolume);

		DisableLoopSounds->setSelected(OgreClient::Singleton->Config->DisableLoopSounds);

		/******************************************************************************************************/
		/*                                  PREPARE / SET: INPUT                                              */
		/******************************************************************************************************/

		MouseAimSpeed->setMaxValue(100.0f);
		KeyRotateSpeed->setMaxValue(100.0f);

		for (int i = 1; i < 10; i++)
			RightClickAction->addItem(new ::CEGUI::ListboxTextItem("Action 0" + ::CEGUI::PropertyHelper<int>::toString(i), i));

		for (int i = 10; i <= 48; i++)
			RightClickAction->addItem(new ::CEGUI::ListboxTextItem("Action " + ::CEGUI::PropertyHelper<int>::toString(i), i));
		
		/******************************************************************************************************/

		LearnMoveForward->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->MoveForward.ToString()));
		LearnMoveBackward->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->MoveBackward.ToString()));
		LearnMoveLeft->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->MoveLeft.ToString()));
		LearnMoveRight->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->MoveRight.ToString()));
		LearnRotateLeft->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->RotateLeft.ToString()));
		LearnRotateRight->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->RotateRight.ToString()));
		LearnWalk->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->Walk.ToString()));
		LearnAutoMove->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->AutoMove.ToString()));
		LearnNextTarget->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->NextTarget.ToString()));
		LearnOpen->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ReqGo.ToString()));
		LearnClose->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->Close.ToString()));

		LearnAction01->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton01.ToString()));
		LearnAction02->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton02.ToString()));
		LearnAction03->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton03.ToString()));
		LearnAction04->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton04.ToString()));
		LearnAction05->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton05.ToString()));
		LearnAction06->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton06.ToString()));
		LearnAction07->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton07.ToString()));
		LearnAction08->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton08.ToString()));
		LearnAction09->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton09.ToString()));
		LearnAction10->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton10.ToString()));
		LearnAction11->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton11.ToString()));
		LearnAction12->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton12.ToString()));
		LearnAction13->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton13.ToString()));
		LearnAction14->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton14.ToString()));
		LearnAction15->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton15.ToString()));
		LearnAction16->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton16.ToString()));
		LearnAction17->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton17.ToString()));
		LearnAction18->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton18.ToString()));
		LearnAction19->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton19.ToString()));
		LearnAction20->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton20.ToString()));
		LearnAction21->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton21.ToString()));
		LearnAction22->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton22.ToString()));
		LearnAction23->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton23.ToString()));
		LearnAction24->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton24.ToString()));
		LearnAction25->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton25.ToString()));
		LearnAction26->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton26.ToString()));
		LearnAction27->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton27.ToString()));
		LearnAction28->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton28.ToString()));
		LearnAction29->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton29.ToString()));
		LearnAction30->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton30.ToString()));
		LearnAction31->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton31.ToString()));
		LearnAction32->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton32.ToString()));
		LearnAction33->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton33.ToString()));
		LearnAction34->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton34.ToString()));
		LearnAction35->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton35.ToString()));
		LearnAction36->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton36.ToString()));
		LearnAction37->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton37.ToString()));
		LearnAction38->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton38.ToString()));
		LearnAction39->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton39.ToString()));
		LearnAction40->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton40.ToString()));
		LearnAction41->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton41.ToString()));
		LearnAction42->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton42.ToString()));
		LearnAction43->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton43.ToString()));
		LearnAction44->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton44.ToString()));
		LearnAction45->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton45.ToString()));
		LearnAction46->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton46.ToString()));
		LearnAction47->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton47.ToString()));
		LearnAction48->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->KeyBinding->ActionButton48.ToString()));

		MouseAimSpeed->setCurrentValue((float)OgreClient::Singleton->Config->MouseAimSpeed);
		KeyRotateSpeed->setCurrentValue((float)OgreClient::Singleton->Config->KeyRotateSpeed);
		InvertMouseY->setSelected(OgreClient::Singleton->Config->InvertMouseY);

		int idx = OgreClient::Singleton->Config->KeyBinding->RightClickAction - 1;
		RightClickAction->getListboxItemFromIndex(idx)->setSelected(true);

		// select
		CEGUI::ListboxItem* itm = RightClickAction->getListboxItemFromIndex(idx);
		itm->setSelected(true);

		// set selected text
		RightClickAction->setText(itm->getText());

		/******************************************************************************************************/
		/*                                       SET CEGUI EVENTS                                             */
		/******************************************************************************************************/

		// subscribe category buttons
		Connections->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		Engine->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		Input->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		Resources->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));

		/******************************************************************************************************/
		
		Display->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisplayChanged));
		Resolution->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnResolutionChanged));
		WindowMode->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnWindowModeChanged));
		WindowBorders->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnWindowBordersChanged));
		VSync->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnVSyncChanged));

		FSAA->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnFSAAChanged));
		Filtering->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnFilteringChanged));
		ImageBuilder->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnImageBuilderChanged));
		ScalingQuality->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnScalingQualityChanged));
		TextureQuality->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnTextureQualityChanged));


		/******************************************************************************************************/

		// hookup keylearn button events
		LearnMoveForward->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnMoveBackward->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnMoveLeft->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnMoveRight->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnRotateLeft->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnRotateRight->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnWalk->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAutoMove->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnNextTarget->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnOpen->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnClose->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction01->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction02->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction03->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction04->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction05->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction06->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction07->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction08->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction09->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction10->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction11->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction12->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction13->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction14->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction15->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction16->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction17->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction18->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction19->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction20->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction21->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction22->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction23->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction24->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction25->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction26->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction27->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction28->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction29->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction30->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction31->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction32->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction33->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction34->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction35->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction36->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction37->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction38->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction39->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction40->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction41->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction42->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction43->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction44->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction45->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction46->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction47->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction48->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));

		/******************************************************************************************************/

		// subscribe other events
		RightClickAction->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnRightClickActionChanged));
		InvertMouseY->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnInvertMouseYChanged));
		MouseAimSpeed->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnMouseAimSpeedChanged));
		KeyRotateSpeed->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyRotateSpeedChanged));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		//Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Guild::OnWindowKeyUp));


		/******************************************************************************************************/

		// attach listener to config
		OgreClient::Singleton->Config->PropertyChanged +=
			gcnew PropertyChangedEventHandler(OnConfigPropertyChanged);
	};

	void ControllerUI::Options::Destroy()
	{
		// detach listener from config
		OgreClient::Singleton->Config->PropertyChanged -=
			gcnew PropertyChangedEventHandler(OnConfigPropertyChanged);

	};

	void ControllerUI::Options::OnConfigPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
	};

	bool UICallbacks::Options::OnCategoryButtonClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn		= (const CEGUI::PushButton*)args.window;

		if (btn == ControllerUI::Options::Connections)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(0);

		else if (btn == ControllerUI::Options::Engine)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(1);

		else if (btn == ControllerUI::Options::Input)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(2);

		else if (btn == ControllerUI::Options::Resources)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(3);

		return true;
	};

	bool UICallbacks::Options::OnKeyLearnButtonClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn		= (const CEGUI::PushButton*)args.window;

		//if (btn == ControllerUI::Options::LearnMoveForward)
		//	ControllerUI::Options::TabControl->setSelectedTabAtIndex(0);

		//else if (btn == ControllerUI::Options::Engine)
		//	ControllerUI::Options::TabControl->setSelectedTabAtIndex(1);

		return true;
	};

	bool UICallbacks::Options::OnDisplayChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		//OgreClient::Singleton->Config->Display = combobox->getSelectionStartIndex();;

		return true;
	};

	bool UICallbacks::Options::OnResolutionChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		//OgreClient::Singleton->Config->Resolution = combobox->getSelectionStartIndex();;

		return true;
	};

	bool UICallbacks::Options::OnWindowModeChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb = (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->WindowMode = toggleb->isSelected();
		
		/*OgreClient::Singleton->RenderWindow->setFullscreen(toggleb->isSelected(),
			OgreClient::Singleton->RenderWindow->getWidth(),
			OgreClient::Singleton->RenderWindow->getHeight());*/

		return true;
	};

	bool UICallbacks::Options::OnWindowBordersChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb = (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->WindowFrame = toggleb->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnVSyncChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb = (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->VSync = toggleb->isSelected();
		//OgreClient::Singleton->RenderWindow->setVSyncEnabled(toggleb->isSelected());
		//OgreClient::Singleton->RenderWindow->setVSyncInterval(1);
		
		return true;
	};

	bool UICallbacks::Options::OnFSAAChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		//OgreClient::Singleton->Config->FSAA = combobox->getSelectionStartIndex();;

		return true;
	};

	bool UICallbacks::Options::OnFilteringChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		//OgreClient::Singleton->Config->TextureFiltering = combobox->getSelectionStartIndex();;

		return true;
	};

	bool UICallbacks::Options::OnImageBuilderChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		OgreClient::Singleton->Config->ImageBuilder = StringConvert::CEGUIToCLR(combobox->getText());

		return true;
	};

	bool UICallbacks::Options::OnScalingQualityChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		::System::String^ oldval = OgreClient::Singleton->Config->BitmapScaling;

		if (oldval == newval)
			return true;

		// save new value
		OgreClient::Singleton->Config->BitmapScaling = StringConvert::CEGUIToCLR(combobox->getText());

		// apply new value
		if (newval == "Low")		
			ImageBuilder::GDI::InterpolationMode = ::System::Drawing::Drawing2D::InterpolationMode::NearestNeighbor;
		
		else if (newval == "Default")		
			ImageBuilder::GDI::InterpolationMode = ::System::Drawing::Drawing2D::InterpolationMode::Default;
		
		else if (newval == "High")		
			ImageBuilder::GDI::InterpolationMode = ::System::Drawing::Drawing2D::InterpolationMode::HighQualityBicubic;
		
		return true;
	};

	bool UICallbacks::Options::OnTextureQualityChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		::System::String^ oldval = OgreClient::Singleton->Config->TextureQuality;

		if (oldval == newval)
			return true;

		// save new value
		OgreClient::Singleton->Config->TextureQuality = newval;

		// apply new value
		if (newval == "Low")
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality		 = 0.25f; // used in RemoteNode2D		
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality		 = 0.25f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality		 = 0.25f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.25f; // used in CEGUI
		}
		else if (newval == "Default")
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality		 = 0.5f; // used in RemoteNode2D
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality		 = 0.5f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality		 = 0.5f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.5f; // used in CEGUI
		}
		else if (newval == "High")
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality		 = 1.0f; // used in RemoteNode2D
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality		 = 1.0f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality		 = 1.0f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 1.0f; // used in CEGUI
		}

		return true;
	};

	bool UICallbacks::Options::OnRightClickActionChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		OgreClient::Singleton->Config->KeyBinding->RightClickAction = (int)combobox->getSelectedItem()->getID();

		return true;
	};

	bool UICallbacks::Options::OnInvertMouseYChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->InvertMouseY = btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnMouseAimSpeedChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		OgreClient::Singleton->Config->MouseAimSpeed = (int)slider->getCurrentValue();

		return true;
	};

	bool UICallbacks::Options::OnKeyRotateSpeedChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		OgreClient::Singleton->Config->KeyRotateSpeed = (int)slider->getCurrentValue();

		return true;
	};
};};