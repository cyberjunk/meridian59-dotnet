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
		Engine		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ENGINE));
		Input		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_INPUT));
		UI			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_UI));
		Aliases		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ALIASES));
		About		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ABOUT));

		// tabcontrol and tabs
		TabControl	= static_cast<CEGUI::TabControl*>(Window->getChild(UI_NAME_OPTIONS_TABCONTROL));		
		TabEngine	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABENGINE));
		TabInput	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABINPUT));
		TabUI		= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABUI));
		TabAliases	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABALIASES));
		TabAbout	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABABOUT));

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
		LearnSelfTarget		= static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_SELFTARGET));
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
		
		PreloadRooms	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADROOMS));
		PreloadRoomTextures = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADROOMTEXTURES));
		PreloadObjects	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADOBJECTS));
		PreloadSounds	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADSOUNDS));
		PreloadMusic	= static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADMUSIC));

		/******************************************************************************************************/

		// tabaliases
		ListAliases = static_cast<CEGUI::ItemListbox*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ALIASES));
		AliasKey = static_cast<CEGUI::Editbox*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ADDKEY));
		AliasValue = static_cast<CEGUI::Editbox*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ADDVALUE));
		AliasAddBtn = static_cast<CEGUI::PushButton*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ADD));

		/******************************************************************************************************/

		// tababout
		TabAboutTabControl = static_cast<CEGUI::TabControl*>(TabAbout->getChild(UI_NAME_OPTIONS_TABABOUT_TABCONTROL));
		TabAboutTabGeneral = static_cast<CEGUI::Window*>(TabAboutTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABGENERAL));
		TabAboutTabHistory = static_cast<CEGUI::Window*>(TabAboutTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY));

		// tababout - tabhistory
		TabAboutTabHistoryTabControl = static_cast<CEGUI::TabControl*>(TabAboutTabHistory->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABCONTROL));
		TabAboutTabHistoryTabEvolution = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABEVOLUTION));
		TabAboutTabHistoryTabResurrection = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABRESURRECTION));
		TabAboutTabHistoryTabDarkAuspices = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABDARKAUSPICES));
		TabAboutTabHistoryTabInsurrection = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABINSURRECTION));
		TabAboutTabHistoryTabRenaissance = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABRENAISSANCE));
		TabAboutTabHistoryTabRevelation = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABREVELATION));
		TabAboutTabHistoryTabValeOfSorrow = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABVALEOFSORROW));
		TabAboutTabHistoryTabTheInternetQuestBegins = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_TABTHEINTERNETQUESTBEGINS));

		TabAboutTabHistoryImageEvolution = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabEvolution->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGEEVOLUTION));
		TabAboutTabHistoryImageResurrection = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabResurrection->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGERESURRECTION));
		TabAboutTabHistoryImageDarkAuspices = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabDarkAuspices->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGEDARKAUSPICES));
		TabAboutTabHistoryImageInsurrection = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabInsurrection->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGEINSURRECTION));
		TabAboutTabHistoryImageRenaissance = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabRenaissance->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGERENAISSANCE));
		TabAboutTabHistoryImageRevelation = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabRevelation->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGEREVELATION));
		TabAboutTabHistoryImageValeOfSorrow = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabValeOfSorrow->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGEVALEOFSORROW));
		TabAboutTabHistoryImageTheInternetQuestBegins = static_cast<CEGUI::Window*>(TabAboutTabHistoryTabTheInternetQuestBegins->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY_IMAGETHEINTERNETQUESTBEGINS));

		/******************************************************************************************************/

		Ogre::TextureManager* texMan	= Ogre::TextureManager::getSingletonPtr();
		BgfFile^ aboutBgf				= OgreClient::Singleton->ResourceManager->GetObject("about.bgf");

		if (aboutBgf)
		{
			// Evolution
			if (aboutBgf->Frames->Count > 0)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/0";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[0], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageEvolution->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// Resurrection
			if (aboutBgf->Frames->Count > 1)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/1";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[1], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageResurrection->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// DarkAuspices
			if (aboutBgf->Frames->Count > 2)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/2";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[2], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageDarkAuspices->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// Insurrection
			if (aboutBgf->Frames->Count > 3)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/3";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[3], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageInsurrection->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// Renaissance
			if (aboutBgf->Frames->Count > 4)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/4";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[4], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageRenaissance->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// Revelation
			if (aboutBgf->Frames->Count > 5)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/5";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[5], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageRevelation->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// ValeOfSorrow
			if (aboutBgf->Frames->Count > 6)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/6";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[6], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageValeOfSorrow->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}

			// TheInternetQuestBegins
			if (aboutBgf->Frames->Count > 7)
			{
				const ::Ogre::String oStrName = "CEGUI/about.bgf/7";

				Util::CreateTextureA8R8G8B8(aboutBgf->Frames[7], oStrName, UI_RESGROUP_IMAGESETS, 0);
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
					TabAboutTabHistoryImageTheInternetQuestBegins->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}
		}


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
		MusicVolume->setMaxValue(10.0f);
		SoundVolume->setMaxValue(10.0f);

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

		Particles->setCurrentValue((float)OgreClient::Singleton->Config->WeatherParticles);
		Decoration->setCurrentValue((float)OgreClient::Singleton->Config->DecorationIntensity);
		MusicVolume->setCurrentValue(OgreClient::Singleton->Config->MusicVolume);
		SoundVolume->setCurrentValue(OgreClient::Singleton->Config->SoundVolume);
		DisableLoopSounds->setSelected(OgreClient::Singleton->Config->DisableLoopSounds);
		
		PreloadRooms->setSelected(OgreClient::Singleton->Config->PreloadRooms);
		PreloadRoomTextures->setSelected(OgreClient::Singleton->Config->PreloadRoomTextures);
		PreloadObjects->setSelected(OgreClient::Singleton->Config->PreloadObjects);
		PreloadSounds->setSelected(OgreClient::Singleton->Config->PreloadSound);
		PreloadMusic->setSelected(OgreClient::Singleton->Config->PreloadMusic);

		/******************************************************************************************************/
		/*                                  PREPARE / SET: ALIASES                                            */
		/******************************************************************************************************/

		for (int i = 0; i < OgreClient::Singleton->Config->Aliases->Count; i++)		
			AliasAdd(i);		

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
		
		LearnMoveForward->setText(keybinding->MoveForward == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->MoveForward));
		LearnMoveBackward->setText(keybinding->MoveBackward == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->MoveBackward));
		LearnMoveLeft->setText(keybinding->MoveLeft == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->MoveLeft));
		LearnMoveRight->setText(keybinding->MoveRight == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->MoveRight));
		LearnRotateLeft->setText(keybinding->RotateLeft == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->RotateLeft));
		LearnRotateRight->setText(keybinding->RotateRight == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->RotateRight));
		LearnWalk->setText(keybinding->Walk == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->Walk));
		LearnAutoMove->setText(keybinding->AutoMove == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->AutoMove));
		LearnNextTarget->setText(keybinding->NextTarget == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->NextTarget));
		LearnSelfTarget->setText(keybinding->SelfTarget == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->SelfTarget));
		LearnOpen->setText(keybinding->ReqGo == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ReqGo));
		LearnClose->setText(keybinding->Close == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->Close));

		LearnAction01->setText(keybinding->ActionButton01 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton01));
		LearnAction02->setText(keybinding->ActionButton02 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton02));
		LearnAction03->setText(keybinding->ActionButton03 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton03));
		LearnAction04->setText(keybinding->ActionButton04 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton04));
		LearnAction05->setText(keybinding->ActionButton05 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton05));
		LearnAction06->setText(keybinding->ActionButton06 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton06));
		LearnAction07->setText(keybinding->ActionButton07 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton07));
		LearnAction08->setText(keybinding->ActionButton08 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton08));
		LearnAction09->setText(keybinding->ActionButton09 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton09));
		LearnAction10->setText(keybinding->ActionButton10 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton10));
		LearnAction11->setText(keybinding->ActionButton11 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton11));
		LearnAction12->setText(keybinding->ActionButton12 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton12));
		LearnAction13->setText(keybinding->ActionButton13 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton13));
		LearnAction14->setText(keybinding->ActionButton14 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton14));
		LearnAction15->setText(keybinding->ActionButton15 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton15));
		LearnAction16->setText(keybinding->ActionButton16 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton16));
		LearnAction17->setText(keybinding->ActionButton17 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton17));
		LearnAction18->setText(keybinding->ActionButton18 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton18));
		LearnAction19->setText(keybinding->ActionButton19 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton19));
		LearnAction20->setText(keybinding->ActionButton20 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton20));
		LearnAction21->setText(keybinding->ActionButton21 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton21));
		LearnAction22->setText(keybinding->ActionButton22 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton22));
		LearnAction23->setText(keybinding->ActionButton23 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton23));
		LearnAction24->setText(keybinding->ActionButton24 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton24));
		LearnAction25->setText(keybinding->ActionButton25 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton25));
		LearnAction26->setText(keybinding->ActionButton26 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton26));
		LearnAction27->setText(keybinding->ActionButton27 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton27));
		LearnAction28->setText(keybinding->ActionButton28 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton28));
		LearnAction29->setText(keybinding->ActionButton29 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton29));
		LearnAction30->setText(keybinding->ActionButton30 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton30));
		LearnAction31->setText(keybinding->ActionButton31 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton31));
		LearnAction32->setText(keybinding->ActionButton32 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton32));
		LearnAction33->setText(keybinding->ActionButton33 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton33));
		LearnAction34->setText(keybinding->ActionButton34 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton34));
		LearnAction35->setText(keybinding->ActionButton35 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton35));
		LearnAction36->setText(keybinding->ActionButton36 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton36));
		LearnAction37->setText(keybinding->ActionButton37 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton37));
		LearnAction38->setText(keybinding->ActionButton38 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton38));
		LearnAction39->setText(keybinding->ActionButton39 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton39));
		LearnAction40->setText(keybinding->ActionButton40 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton40));
		LearnAction41->setText(keybinding->ActionButton41 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton41));
		LearnAction42->setText(keybinding->ActionButton42 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton42));
		LearnAction43->setText(keybinding->ActionButton43 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton43));
		LearnAction44->setText(keybinding->ActionButton44 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton44));
		LearnAction45->setText(keybinding->ActionButton45 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton45));
		LearnAction46->setText(keybinding->ActionButton46 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton46));
		LearnAction47->setText(keybinding->ActionButton47 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton47));
		LearnAction48->setText(keybinding->ActionButton48 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton48));

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
		Engine->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		Input->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		UI->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		Aliases->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
		About->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));

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
		LearnMoveForward->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnMoveBackward->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnMoveLeft->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnMoveRight->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnRotateLeft->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnRotateRight->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnWalk->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAutoMove->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnNextTarget->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnSelfTarget->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnOpen->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnClose->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction01->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction02->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction03->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction04->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction05->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction06->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction07->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction08->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction09->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction10->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction11->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction12->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction13->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction14->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction15->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction16->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction17->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction18->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction19->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction20->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction21->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction22->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction23->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction24->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction25->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction26->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction27->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction28->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction29->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction30->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction31->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction32->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction33->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction34->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction35->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction36->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction37->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction38->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction39->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction40->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction41->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction42->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction43->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction44->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction45->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction46->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction47->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
		LearnAction48->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));

		LearnMoveForward->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnMoveBackward->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnMoveLeft->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnMoveRight->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnRotateLeft->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnRotateRight->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnWalk->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnAutoMove->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnNextTarget->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
		LearnSelfTarget->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
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

		LearnMoveForward->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnMoveBackward->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnMoveLeft->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnMoveRight->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnRotateLeft->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnRotateRight->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnWalk->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAutoMove->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnNextTarget->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnSelfTarget->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnOpen->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnClose->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction01->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction02->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction03->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction04->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction05->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction06->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction07->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction08->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction09->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction10->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction11->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction12->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction13->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction14->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction15->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction16->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction17->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction18->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction19->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction20->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction21->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction22->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction23->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction24->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction25->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction26->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction27->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction28->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction29->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction30->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction31->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction32->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction33->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction34->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction35->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction36->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction37->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction38->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction39->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction40->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction41->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction42->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction43->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction44->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction45->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction46->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction47->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
		LearnAction48->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));

		/******************************************************************************************************/

		// subscribe other events
		RightClickAction->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnRightClickActionChanged));
		InvertMouseY->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnInvertMouseYChanged));
		MouseAimSpeed->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnMouseAimSpeedChanged));
		KeyRotateSpeed->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyRotateSpeedChanged));
		AliasAddBtn->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnAliasAddClicked));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup (esc for close)
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));

		// copy&paste for alias values
		AliasKey->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		AliasValue->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

		/******************************************************************************************************/

		// attach listener to config
		OgreClient::Singleton->Config->PropertyChanged +=
			gcnew PropertyChangedEventHandler(OnConfigPropertyChanged);

		// attach listener to aliases
		OgreClient::Singleton->Config->Aliases->ListChanged +=
			gcnew ListChangedEventHandler(OnAliasListChanged);
	};

	void ControllerUI::Options::Destroy()
	{
		// detach listener from config
		OgreClient::Singleton->Config->PropertyChanged -=
			gcnew PropertyChangedEventHandler(OnConfigPropertyChanged);

		// detach listener from aliases
		OgreClient::Singleton->Config->Aliases->ListChanged -=
			gcnew ListChangedEventHandler(OnAliasListChanged);
	};

	void ControllerUI::Options::OnConfigPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
	};

	void ControllerUI::Options::OnAliasListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch (e->ListChangedType)
		{
		case ::System::ComponentModel::ListChangedType::ItemAdded:
			AliasAdd(e->NewIndex);
			break;

		case ::System::ComponentModel::ListChangedType::ItemDeleted:
			AliasRemove(e->NewIndex);
			break;
		}
	};

	void ControllerUI::Options::AliasAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr	= CEGUI::WindowManager::getSingletonPtr();
		KeyValuePairString^ alias		= OgreClient::Singleton->Config->Aliases[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_ALIASLISTBOXITEM);

		// get children
		CEGUI::PushButton* del	= (CEGUI::PushButton*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_DELETE);
		CEGUI::Editbox* key		= (CEGUI::Editbox*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_KEY);
		CEGUI::Editbox* value	= (CEGUI::Editbox*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_VALUE);

		// set values
		key->setText(StringConvert::CLRToCEGUI(alias->Key));
		value->setText(StringConvert::CLRToCEGUI(alias->Value));

		// subscribe del event
		del->subscribeEvent(
			CEGUI::PushButton::EventClicked,
			CEGUI::Event::Subscriber(UICallbacks::Options::OnAliasDeleteClicked));

		// copy&paste for alias values
		key->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		key->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnAliasKeyAccepted));
		key->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnAliasKeyAccepted));
		value->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		value->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnAliasValueAccepted));
		value->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnAliasValueAccepted));

		// insert in ui-list
		if ((int)ListAliases->getItemCount() > Index)
			ListAliases->insertItem(widget, ListAliases->getItemFromIndex(Index));

		// or add
		else
			ListAliases->addItem(widget);

		// fix a bug with last item not visible
		ListAliases->notifyScreenAreaChanged(true);
	};

	void ControllerUI::Options::AliasRemove(int Index)
	{
		if ((int)ListAliases->getItemCount() > Index)
			ListAliases->removeItem(ListAliases->getItemFromIndex(Index));
	};

	bool UICallbacks::Options::OnCategoryButtonClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn		= (const CEGUI::PushButton*)args.window;

		if (btn == ControllerUI::Options::Engine)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(0);

		else if (btn == ControllerUI::Options::Input)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(1);
		
		else if (btn == ControllerUI::Options::UI)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(2);

		else if (btn == ControllerUI::Options::Aliases)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(3);
		
		else if (btn == ControllerUI::Options::About)
			ControllerUI::Options::TabControl->setSelectedTabAtIndex(4);

		return true;
	};

	bool UICallbacks::Options::OnKeyLearnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
		CEGUI::PushButton* btn			= (CEGUI::PushButton*)args.window;
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
		else if (btn == ControllerUI::Options::LearnAction02)
		{
			keybinding->ActionButton02 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction02->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction03)
		{
			keybinding->ActionButton03 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction03->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction04)
		{
			keybinding->ActionButton04 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction04->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction05)
		{
			keybinding->ActionButton05 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction05->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction06)
		{
			keybinding->ActionButton06 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction06->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction07)
		{
			keybinding->ActionButton07 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction07->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction08)
		{
			keybinding->ActionButton08 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction08->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction09)
		{
			keybinding->ActionButton09 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction09->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction10)
		{
			keybinding->ActionButton10 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction10->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction11)
		{
			keybinding->ActionButton11 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction11->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction12)
		{
			keybinding->ActionButton12 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction12->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction13)
		{
			keybinding->ActionButton13 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction13->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction14)
		{
			keybinding->ActionButton14 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction14->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction15)
		{
			keybinding->ActionButton15 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction15->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction16)
		{
			keybinding->ActionButton16 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction16->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction17)
		{
			keybinding->ActionButton17 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction17->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction18)
		{
			keybinding->ActionButton18 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction18->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction19)
		{
			keybinding->ActionButton19 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction19->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction20)
		{
			keybinding->ActionButton20 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction20->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction21)
		{
			keybinding->ActionButton21 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction21->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction22)
		{
			keybinding->ActionButton22 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction22->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction23)
		{
			keybinding->ActionButton23 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction23->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction24)
		{
			keybinding->ActionButton24 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction24->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction25)
		{
			keybinding->ActionButton25 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction25->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction26)
		{
			keybinding->ActionButton26 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction26->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction27)
		{
			keybinding->ActionButton27 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction27->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction28)
		{
			keybinding->ActionButton28 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction28->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction29)
		{
			keybinding->ActionButton29 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction29->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction30)
		{
			keybinding->ActionButton30 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction30->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction31)
		{
			keybinding->ActionButton31 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction31->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction32)
		{
			keybinding->ActionButton32 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction32->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction33)
		{
			keybinding->ActionButton33 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction33->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction34)
		{
			keybinding->ActionButton34 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction34->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction35)
		{
			keybinding->ActionButton35 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction35->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction36)
		{
			keybinding->ActionButton36 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction36->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction37)
		{
			keybinding->ActionButton37 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction37->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction38)
		{
			keybinding->ActionButton38 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction38->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction39)
		{
			keybinding->ActionButton39 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction39->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction40)
		{
			keybinding->ActionButton40 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction40->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction41)
		{
			keybinding->ActionButton41 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction41->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction42)
		{
			keybinding->ActionButton42 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction42->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction43)
		{
			keybinding->ActionButton43 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction43->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction44)
		{
			keybinding->ActionButton44 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction44->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction45)
		{
			keybinding->ActionButton45 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction45->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction46)
		{
			keybinding->ActionButton46 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction46->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction47)
		{
			keybinding->ActionButton47 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction47->setText(keystr);
		}
		else if (btn == ControllerUI::Options::LearnAction48)
		{
			keybinding->ActionButton48 = (::OIS::KeyCode)args.scancode;
			ControllerUI::Options::LearnAction48->setText(keystr);
		}

		// deactivate focus
		btn->deactivate();

		return true;
	};

	bool UICallbacks::Options::OnKeyLearnButtonClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		CEGUI::PushButton* btn				= (CEGUI::PushButton*)args.window;
		OISKeyBinding^ keybinding			= OgreClient::Singleton->Config->KeyBinding;

		// left click gives focus to button (assign handled in KeyUp)
		// also show notification
		if (args.button == ::CEGUI::MouseButton::LeftButton)
			ControllerUI::SplashNotifier::ShowNotification(UI_NOTIFICATION_PRESSAKEY);

		// right click clears assignment
		else if (args.button == ::CEGUI::MouseButton::RightButton)
		{
			// basic movement
			if (btn == ControllerUI::Options::LearnMoveForward)
			{
				keybinding->MoveForward = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnMoveForward->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnMoveBackward)
			{
				keybinding->MoveBackward = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnMoveBackward->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnMoveLeft)
			{
				keybinding->MoveLeft = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnMoveLeft->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnMoveRight)
			{
				keybinding->MoveRight = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnMoveRight->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnRotateLeft)
			{
				keybinding->RotateLeft = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnRotateLeft->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnRotateRight)
			{
				keybinding->RotateRight = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnRotateRight->setText(STRINGEMPTY);
			}

			// others
			else if (btn == ControllerUI::Options::LearnWalk)
			{
				keybinding->Walk = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnWalk->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAutoMove)
			{
				keybinding->AutoMove = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAutoMove->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnNextTarget)
			{
				keybinding->NextTarget = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnNextTarget->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnSelfTarget)
			{
				keybinding->SelfTarget = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnSelfTarget->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnOpen)
			{
				keybinding->ReqGo = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnOpen->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnClose)
			{
				keybinding->Close = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnClose->setText(STRINGEMPTY);
			}

			// actionbuttons
			else if (btn == ControllerUI::Options::LearnAction01)
			{
				keybinding->ActionButton01 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction01->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction02)
			{
				keybinding->ActionButton02 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction02->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction03)
			{
				keybinding->ActionButton03 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction03->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction04)
			{
				keybinding->ActionButton04 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction04->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction05)
			{
				keybinding->ActionButton05 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction05->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction06)
			{
				keybinding->ActionButton06 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction06->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction07)
			{
				keybinding->ActionButton07 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction07->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction08)
			{
				keybinding->ActionButton08 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction08->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction09)
			{
				keybinding->ActionButton09 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction09->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction10)
			{
				keybinding->ActionButton10 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction10->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction11)
			{
				keybinding->ActionButton11 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction11->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction12)
			{
				keybinding->ActionButton12 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction12->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction13)
			{
				keybinding->ActionButton13 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction13->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction14)
			{
				keybinding->ActionButton14 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction14->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction15)
			{
				keybinding->ActionButton15 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction15->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction16)
			{
				keybinding->ActionButton16 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction16->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction17)
			{
				keybinding->ActionButton17 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction17->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction18)
			{
				keybinding->ActionButton18 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction18->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction19)
			{
				keybinding->ActionButton19 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction19->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction20)
			{
				keybinding->ActionButton20 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction20->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction21)
			{
				keybinding->ActionButton21 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction21->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction22)
			{
				keybinding->ActionButton22 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction22->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction23)
			{
				keybinding->ActionButton23 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction23->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction24)
			{
				keybinding->ActionButton24 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction24->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction25)
			{
				keybinding->ActionButton25 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction25->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction26)
			{
				keybinding->ActionButton26 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction26->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction27)
			{
				keybinding->ActionButton27 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction27->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction28)
			{
				keybinding->ActionButton28 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction28->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction29)
			{
				keybinding->ActionButton29 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction29->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction30)
			{
				keybinding->ActionButton30 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction30->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction31)
			{
				keybinding->ActionButton31 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction31->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction32)
			{
				keybinding->ActionButton32 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction32->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction33)
			{
				keybinding->ActionButton33 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction33->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction34)
			{
				keybinding->ActionButton34 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction34->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction35)
			{
				keybinding->ActionButton35 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction35->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction36)
			{
				keybinding->ActionButton36 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction36->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction37)
			{
				keybinding->ActionButton37 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction37->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction38)
			{
				keybinding->ActionButton38 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction38->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction39)
			{
				keybinding->ActionButton39 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction39->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction40)
			{
				keybinding->ActionButton40 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction40->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction41)
			{
				keybinding->ActionButton41 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction41->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction42)
			{
				keybinding->ActionButton42 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction42->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction43)
			{
				keybinding->ActionButton43 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction43->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction44)
			{
				keybinding->ActionButton44 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction44->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction45)
			{
				keybinding->ActionButton45 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction45->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction46)
			{
				keybinding->ActionButton46 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction46->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction47)
			{
				keybinding->ActionButton47 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction47->setText(STRINGEMPTY);
			}
			else if (btn == ControllerUI::Options::LearnAction48)
			{
				keybinding->ActionButton48 = ::OIS::KeyCode::KC_UNASSIGNED;
				ControllerUI::Options::LearnAction48->setText(STRINGEMPTY);
			}
		}

		return true;
	};

	bool UICallbacks::Options::OnKeyLearnButtonDeactivated(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn		= (const CEGUI::PushButton*)args.window;

		ControllerUI::SplashNotifier::HideNotification(UI_NOTIFICATION_PRESSAKEY);

		return true;
	};

	bool UICallbacks::Options::OnDisplayChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		int newval = (int)combobox->getItemIndex(combobox->getSelectedItem());
		int oldval = OgreClient::Singleton->Config->Display;

		if (newval < 0 || oldval == newval)
			return true;

		OgreClient::Singleton->Config->Display = newval;
		OgreClient::Singleton->RecreateWindow = true;

		return true;
	};

	bool UICallbacks::Options::OnResolutionChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		::System::String^ oldval = OgreClient::Singleton->Config->Resolution;

		if (!newval || newval == STRINGEMPTY || oldval == newval)
			return true;

		OgreClient::Singleton->Config->Resolution = newval;
		OgreClient::Singleton->RecreateWindow = true;

		return true;
	};

	bool UICallbacks::Options::OnWindowModeChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb = (const CEGUI::ToggleButton*)args.window;

		bool newval = toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->WindowMode;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->WindowMode = newval;
		OgreClient::Singleton->RecreateWindow = true;

		return true;
	};

	bool UICallbacks::Options::OnWindowBordersChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb = (const CEGUI::ToggleButton*)args.window;

		bool newval = toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->WindowFrame;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->WindowFrame = newval;
		OgreClient::Singleton->RecreateWindow = true;

		return true;
	};

	bool UICallbacks::Options::OnVSyncChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb = (const CEGUI::ToggleButton*)args.window;

		bool newval = toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->VSync;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->VSync = newval;
		OgreClient::Singleton->RecreateWindow = true;

		return true;
	};

	bool UICallbacks::Options::OnFSAAChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		::System::String^ oldval = OgreClient::Singleton->Config->FSAA;

		if (oldval == newval)
			return true;
		
		OgreClient::Singleton->Config->FSAA = newval;
		OgreClient::Singleton->RecreateWindow = true;

		return true;
	};

	bool UICallbacks::Options::OnFilteringChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;
		::Ogre::MaterialManager* matMan		= ::Ogre::MaterialManager::getSingletonPtr();

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		::System::String^ oldval = OgreClient::Singleton->Config->TextureFiltering;

		if (oldval == newval)
			return true;

		// save in config
		OgreClient::Singleton->Config->TextureFiltering = newval;
		
		// apply
		if (::System::String::Equals(newval, "Off"))
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_NONE);

		else if (::System::String::Equals(newval, "Bilinear"))
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_BILINEAR);

		else if (::System::String::Equals(newval, "Trilinear"))
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_TRILINEAR);

		else if (::System::String::Equals(newval, "Anisotropic x4"))
		{
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
			matMan->setDefaultAnisotropy(4);
		}
		else if (::System::String::Equals(newval, "Anisotropic x16"))
		{
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
			matMan->setDefaultAnisotropy(16);
		}

		return true;
	};

	bool UICallbacks::Options::OnImageBuilderChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Combobox* combobox		= (const CEGUI::Combobox*)args.window;

		::System::String^ newval = StringConvert::CEGUIToCLR(combobox->getText());
		::System::String^ oldval = OgreClient::Singleton->Config->ImageBuilder;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->ImageBuilder = newval;
		// todo

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
		OgreClient::Singleton->Config->BitmapScaling = newval;

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
		const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

		bool newval = !toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->NoMipmaps;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->NoMipmaps = newval;

		// apply value
		::Ogre::TextureManager::getSingletonPtr()->setDefaultNumMipmaps(newval ? 0 : 5);

		return true;
	};

	bool UICallbacks::Options::OnDisableNewRoomTexturesChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

		bool newval = !toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->DisableNewRoomTextures;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->DisableNewRoomTextures = newval;
		// todo

		return true;
	};

	bool UICallbacks::Options::OnDisable3DModelsChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

		bool newval = !toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->Disable3DModels;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->Disable3DModels = newval;
		// todo

		return true;
	};

	bool UICallbacks::Options::OnDisableNewSkyChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

		bool newval = !toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->DisableNewSky;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->DisableNewSky = newval;
		
		if (newval)
			ControllerRoom::DestroyCaelum(); 
		else
			ControllerRoom::InitCaelum();

		ControllerRoom::UpdateSky();

		return true;
	};

	bool UICallbacks::Options::OnDisableWeatherEffectsChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

		bool newval = !toggleb->isSelected();
		bool oldval = OgreClient::Singleton->Config->DisableWeatherEffects;

		if (oldval == newval)
			return true;

		OgreClient::Singleton->Config->DisableWeatherEffects = newval;
		// todo

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

		OgreClient::Singleton->Config->MusicVolume = slider->getCurrentValue();

		// applies the new musicvolume on playing sound
		ControllerSound::AdjustMusicVolume();

		return true;
	};

	bool UICallbacks::Options::OnSoundVolumeChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Slider* slider			= (const CEGUI::Slider*)args.window;

		OgreClient::Singleton->Config->SoundVolume = slider->getCurrentValue();

		// applies the new soundvolume on playing sound
		ControllerSound::AdjustSoundVolume();

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

	bool UICallbacks::Options::OnAliasAddClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn = (const CEGUI::PushButton*)args.window;

		::CEGUI::String key = ControllerUI::Options::AliasKey->getText();
		::CEGUI::String val = ControllerUI::Options::AliasValue->getText();
	
		::System::String^ keyclr = StringConvert::CEGUIToCLR(key);
		::System::String^ valclr = StringConvert::CEGUIToCLR(val);

		keyclr = keyclr->Trim();
		valclr = valclr->Trim();

		if (keyclr == STRINGEMPTY || valclr == STRINGEMPTY)
			return true;

		// must not have this aliaskey already
		if (OgreClient::Singleton->Config->Aliases->GetIndexByKey(keyclr) != -1)
			return true;

		// add the new alias to config
		OgreClient::Singleton->Config->Aliases->Add(
			gcnew KeyValuePairString(keyclr, valclr));

		return true;
	};

	bool UICallbacks::Options::OnAliasDeleteClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		const CEGUI::PushButton* btn		= (const CEGUI::PushButton*)args.window;

		CEGUI::ItemListbox* list = ControllerUI::Options::ListAliases;

		size_t idx = list->getItemIndex((CEGUI::ItemEntry*)btn->getParent());

		OgreClient::Singleton->Config->Aliases->RemoveAt(idx);

		return true;
	};

	bool UICallbacks::Options::OnAliasKeyAccepted(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Editbox* box = (const CEGUI::Editbox*)args.window;

		CEGUI::ItemListbox* list = ControllerUI::Options::ListAliases;
		size_t idx = list->getItemIndex((CEGUI::ItemEntry*)box->getParent());

		if (OgreClient::Singleton->Config->Aliases->Count > (int)idx)
			OgreClient::Singleton->Config->Aliases[idx]->Key = StringConvert::CEGUIToCLR(box->getText());

		return true;
	};

	bool UICallbacks::Options::OnAliasValueAccepted(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		const CEGUI::Editbox* box = (const CEGUI::Editbox*)args.window;

		CEGUI::ItemListbox* list = ControllerUI::Options::ListAliases;
		size_t idx = list->getItemIndex((CEGUI::ItemEntry*)box->getParent());

		if (OgreClient::Singleton->Config->Aliases->Count > (int)idx)
			OgreClient::Singleton->Config->Aliases[idx]->Value = StringConvert::CEGUIToCLR(box->getText());

		return true;
	};
};};