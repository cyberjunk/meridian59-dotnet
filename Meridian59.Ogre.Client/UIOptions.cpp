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
		
		PreloadRooms = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADROOMS));
		PreloadRoomTextures = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADROOMTEXTURES));
		PreloadObjects = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADOBJECTS));
		PreloadSounds = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADSOUNDS));
		PreloadMusic = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADMUSIC));

		
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

		Display->setItemSelectState(Display->getListboxItemFromIndex(OgreClient::Singleton->Config->Display), true);
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
		
		PreloadRooms->setSelected(OgreClient::Singleton->Config->PreloadRooms);
		PreloadRoomTextures->setSelected(OgreClient::Singleton->Config->PreloadRoomTextures);
		PreloadObjects->setSelected(OgreClient::Singleton->Config->PreloadObjects);
		PreloadSounds->setSelected(OgreClient::Singleton->Config->PreloadSound);
		PreloadMusic->setSelected(OgreClient::Singleton->Config->PreloadMusic);

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

		OISKeyBinding^ keybinding = OgreClient::Singleton->Config->KeyBinding;
		::OIS::Keyboard* keyboard = ControllerInput::OISKeyboard;
		
		LearnMoveForward->setText(keyboard->getAsString(keybinding->MoveForward));
		LearnMoveBackward->setText(keyboard->getAsString(keybinding->MoveBackward));
		LearnMoveLeft->setText(keyboard->getAsString(keybinding->MoveLeft));
		LearnMoveRight->setText(keyboard->getAsString(keybinding->MoveRight));
		LearnRotateLeft->setText(keyboard->getAsString(keybinding->RotateLeft));
		LearnRotateRight->setText(keyboard->getAsString(keybinding->RotateRight));
		LearnWalk->setText(keyboard->getAsString(keybinding->Walk));
		LearnAutoMove->setText(keyboard->getAsString(keybinding->AutoMove));
		LearnNextTarget->setText(keyboard->getAsString(keybinding->NextTarget));
		LearnOpen->setText(keyboard->getAsString(keybinding->ReqGo));
		LearnClose->setText(keyboard->getAsString(keybinding->Close));

		LearnAction01->setText(keyboard->getAsString(keybinding->ActionButton01));
		LearnAction02->setText(keyboard->getAsString(keybinding->ActionButton02));
		LearnAction03->setText(keyboard->getAsString(keybinding->ActionButton03));
		LearnAction04->setText(keyboard->getAsString(keybinding->ActionButton04));
		LearnAction05->setText(keyboard->getAsString(keybinding->ActionButton05));
		LearnAction06->setText(keyboard->getAsString(keybinding->ActionButton06));
		LearnAction07->setText(keyboard->getAsString(keybinding->ActionButton07));
		LearnAction08->setText(keyboard->getAsString(keybinding->ActionButton08));
		LearnAction09->setText(keyboard->getAsString(keybinding->ActionButton09));
		LearnAction10->setText(keyboard->getAsString(keybinding->ActionButton10));
		LearnAction11->setText(keyboard->getAsString(keybinding->ActionButton11));
		LearnAction12->setText(keyboard->getAsString(keybinding->ActionButton12));
		LearnAction13->setText(keyboard->getAsString(keybinding->ActionButton13));
		LearnAction14->setText(keyboard->getAsString(keybinding->ActionButton14));
		LearnAction15->setText(keyboard->getAsString(keybinding->ActionButton15));
		LearnAction16->setText(keyboard->getAsString(keybinding->ActionButton16));
		LearnAction17->setText(keyboard->getAsString(keybinding->ActionButton17));
		LearnAction18->setText(keyboard->getAsString(keybinding->ActionButton18));
		LearnAction19->setText(keyboard->getAsString(keybinding->ActionButton19));
		LearnAction20->setText(keyboard->getAsString(keybinding->ActionButton20));
		LearnAction21->setText(keyboard->getAsString(keybinding->ActionButton21));
		LearnAction22->setText(keyboard->getAsString(keybinding->ActionButton22));
		LearnAction23->setText(keyboard->getAsString(keybinding->ActionButton23));
		LearnAction24->setText(keyboard->getAsString(keybinding->ActionButton24));
		LearnAction25->setText(keyboard->getAsString(keybinding->ActionButton25));
		LearnAction26->setText(keyboard->getAsString(keybinding->ActionButton26));
		LearnAction27->setText(keyboard->getAsString(keybinding->ActionButton27));
		LearnAction28->setText(keyboard->getAsString(keybinding->ActionButton28));
		LearnAction29->setText(keyboard->getAsString(keybinding->ActionButton29));
		LearnAction30->setText(keyboard->getAsString(keybinding->ActionButton30));
		LearnAction31->setText(keyboard->getAsString(keybinding->ActionButton31));
		LearnAction32->setText(keyboard->getAsString(keybinding->ActionButton32));
		LearnAction33->setText(keyboard->getAsString(keybinding->ActionButton33));
		LearnAction34->setText(keyboard->getAsString(keybinding->ActionButton34));
		LearnAction35->setText(keyboard->getAsString(keybinding->ActionButton35));
		LearnAction36->setText(keyboard->getAsString(keybinding->ActionButton36));
		LearnAction37->setText(keyboard->getAsString(keybinding->ActionButton37));
		LearnAction38->setText(keyboard->getAsString(keybinding->ActionButton38));
		LearnAction39->setText(keyboard->getAsString(keybinding->ActionButton39));
		LearnAction40->setText(keyboard->getAsString(keybinding->ActionButton40));
		LearnAction41->setText(keyboard->getAsString(keybinding->ActionButton41));
		LearnAction42->setText(keyboard->getAsString(keybinding->ActionButton42));
		LearnAction43->setText(keyboard->getAsString(keybinding->ActionButton43));
		LearnAction44->setText(keyboard->getAsString(keybinding->ActionButton44));
		LearnAction45->setText(keyboard->getAsString(keybinding->ActionButton45));
		LearnAction46->setText(keyboard->getAsString(keybinding->ActionButton46));
		LearnAction47->setText(keyboard->getAsString(keybinding->ActionButton47));
		LearnAction48->setText(keyboard->getAsString(keybinding->ActionButton48));

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
		DisableMipmaps->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisableMipmapsChanged));
		DisableNewRoomTextures->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisableNewRoomTexturesChanged));
		Disable3DModels->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisable3DModelsChanged));
		DisableNewSky->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisableNewSkyChanged));
		DisableWeather->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisableWeatherEffectsChanged));
		Particles->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnParticlesChanged));
		Decoration->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDecorationChanged));
		MusicVolume->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnMusicVolumeChanged));
		SoundVolume->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnSoundVolumeChanged));
		DisableLoopSounds->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnDisableLoopSoundsChanged));
		PreloadRooms->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreloadChanged));
		PreloadRoomTextures->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreloadChanged));
		PreloadObjects->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreloadChanged));
		PreloadSounds->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreloadChanged));
		PreloadMusic->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreloadChanged));

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

		LearnMoveForward->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnMoveBackward->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnMoveLeft->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnMoveRight->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnRotateLeft->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnRotateRight->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnWalk->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAutoMove->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnNextTarget->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnOpen->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnClose->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction01->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction02->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction03->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction04->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction05->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction06->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction07->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction08->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction09->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction10->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction11->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction12->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction13->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction14->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction15->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction16->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction17->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction18->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction19->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction20->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction21->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction22->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction23->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction24->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction25->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction26->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction27->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction28->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction29->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction30->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction31->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction32->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction33->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction34->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction35->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction36->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction37->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction38->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction39->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction40->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction41->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction42->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction43->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction44->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction45->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction46->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction47->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAction48->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));


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

	bool UICallbacks::Options::OnKeyLearnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
		const CEGUI::PushButton* btn	= (const CEGUI::PushButton*)args.window;
		::OIS::Keyboard* keyboard		= ControllerInput::OISKeyboard;
		OISKeyBinding^ keybinding		= OgreClient::Singleton->Config->KeyBinding;
		const std::string keystr		= keyboard->getAsString((::OIS::KeyCode)args.scancode);

		// basic movement
		if (btn == ControllerUI::Options::LearnMoveForward)
		{
			keybinding->MoveForward = (::OIS::KeyCode)args.scancode;			
			ControllerUI::Options::LearnMoveForward->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnMoveBackward)
		{
			keybinding->MoveBackward = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnMoveBackward->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnMoveLeft)
		{
			keybinding->MoveLeft = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnMoveLeft->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnMoveRight)
		{
			keybinding->MoveRight = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnMoveRight->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnRotateLeft)
		{
			keybinding->RotateLeft = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnRotateLeft->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnRotateRight)
		{
			keybinding->RotateRight = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnRotateRight->setText(keystr);
		}

		// others
		else if (btn == ControllerUI::Options::LearnWalk)
		{
			keybinding->Walk = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnWalk->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAutoMove)
		{
			keybinding->AutoMove = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAutoMove->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnNextTarget)
		{
			keybinding->NextTarget = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnNextTarget->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnSelfTarget)
		{
			keybinding->SelfTarget = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnSelfTarget->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnOpen)
		{
			keybinding->ReqGo = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnOpen->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnClose)
		{
			keybinding->Close = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnClose->setText(keystr);
		}

		// actionbuttons
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton01 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction01->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton02 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction02->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton03 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction03->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton04 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction04->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton05 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction05->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton06 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction06->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton07 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction07->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton08 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction08->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton09 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction09->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton10 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction10->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton11 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction11->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton12 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction12->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton13 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction13->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton14 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction14->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton15 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction15->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton16 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction16->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton17 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction17->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton18 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction18->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton19 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction19->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton20 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction20->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton21 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction21->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton22 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction22->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton23 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction23->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton24 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction24->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton25 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction25->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton26 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction26->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton27 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction27->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton28 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction28->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton29 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction29->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton30 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction30->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton31 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction31->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton32 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction32->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton33 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction33->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton34 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction34->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton35 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction35->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton36 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction36->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton37 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction37->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton38 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction38->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton39 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction39->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton40 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction40->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton41 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction41->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton42 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction42->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton43 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction43->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton44 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction44->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton45 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction45->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton46 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction46->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton47 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction47->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction01)
		{
			keybinding->ActionButton48 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction48->setText(keystr);
		}

		return true;
	};

	bool UICallbacks::Options::OnKeyLearnButtonClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn		= (const CEGUI::PushButton*)args.window;

		//if (btn == ControllerUI::Options::LearnMoveForward)
		//	ControllerUI::Options::LearnMoveForward->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, "Red");

		//else if (btn == ControllerUI::Options::Engine)
		//	ControllerUI::Options::TabControl->setSelectedTabAtIndex(1);

		return true;
	};

	bool UICallbacks::Options::OnDisplayChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		OgreClient::Singleton->Config->Display = combobox->getItemIndex(combobox->getSelectedItem());

		return true;
	};

	bool UICallbacks::Options::OnResolutionChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		OgreClient::Singleton->Config->Resolution = StringConvert::CEGUIToCLR(combobox->getText());

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

	bool UICallbacks::Options::OnDisableMipmapsChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->NoMipmaps = !btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnDisableNewRoomTexturesChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->DisableNewRoomTextures = !btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnDisable3DModelsChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->Disable3DModels = !btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnDisableNewSkyChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->DisableNewSky = !btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnDisableWeatherEffectsChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->DisableWeatherEffects = !btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnParticlesChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		OgreClient::Singleton->Config->WeatherParticles = (int)slider->getCurrentValue();

		return true;
	};

	bool UICallbacks::Options::OnDecorationChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		OgreClient::Singleton->Config->DecorationIntensity = (int)slider->getCurrentValue();

		return true;
	};

	bool UICallbacks::Options::OnMusicVolumeChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		OgreClient::Singleton->Config->MusicVolume = (int)slider->getCurrentValue();

		return true;
	};

	bool UICallbacks::Options::OnSoundVolumeChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		//OgreClient::Singleton->Config->SoundVolume = (int)slider->getCurrentValue();

		return true;
	};

	bool UICallbacks::Options::OnDisableLoopSoundsChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		OgreClient::Singleton->Config->DisableLoopSounds = btn->isSelected();

		return true;
	};

	bool UICallbacks::Options::OnPreloadChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* btn		= (const CEGUI::ToggleButton*)args.window;

		if (btn == ControllerUI::Options::PreloadRooms)
			OgreClient::Singleton->Config->PreloadRooms = btn->isSelected();

		else if (btn == ControllerUI::Options::PreloadRoomTextures)
			OgreClient::Singleton->Config->PreloadRoomTextures = btn->isSelected();

		else if (btn == ControllerUI::Options::PreloadObjects)
			OgreClient::Singleton->Config->PreloadObjects = btn->isSelected();

		else if (btn == ControllerUI::Options::PreloadSounds)
			OgreClient::Singleton->Config->PreloadSound = btn->isSelected();

		else if (btn == ControllerUI::Options::PreloadMusic)
			OgreClient::Singleton->Config->PreloadMusic = btn->isSelected();

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