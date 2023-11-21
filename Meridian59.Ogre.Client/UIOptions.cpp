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
      Engine   = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ENGINE));
      Input    = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_INPUT));
      GamePlay = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_GAMEPLAY));
      Aliases  = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ALIASES));
      Groups   = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_GROUPS));
      About    = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_OPTIONS_ABOUT));

      // tabcontrol and tabs
      TabControl	= static_cast<CEGUI::TabControl*>(Window->getChild(UI_NAME_OPTIONS_TABCONTROL));
      TabEngine	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABENGINE));
      TabInput	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABINPUT));
      TabGamePlay = static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABGAMEPLAY));
      TabAliases	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABALIASES));
      TabGroups	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABGROUPS));
      TabAbout	= static_cast<CEGUI::Window*>(TabControl->getChild(UI_NAME_OPTIONS_TABABOUT));

      /******************************************************************************************************/

      // tabinput children
      TabInputTabControl = static_cast<CEGUI::TabControl*>(TabInput->getChild(UI_NAME_OPTIONS_TABINPUT_TABCONTROL));
      TabInputTabGeneral = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL));
      TabInputTabActionButtons1 = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS1));
      TabInputTabActionButtons2 = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS2));
      TabInputTabActionButtons3 = static_cast<CEGUI::Window*>(TabInputTabControl->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3));

      // tabinput - tabgeneral
      LearnMoveForward  = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVEFORWARD));
      LearnMoveBackward = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVEBACKWARD));
      LearnMoveLeft     = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVELEFT));
      LearnMoveRight    = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOVERIGHT));
      LearnRotateLeft   = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_ROTATELEFT));
      LearnRotateRight  = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_ROTATERIGHT));
      LearnWalk         = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_WALK));
      LearnAutoMove	   = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_AUTOMOVE));
      LearnNextTarget   = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_NEXTTARGET));
      LearnSelfTarget   = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_SELFTARGET));
      LearnOpen         = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_OPEN));
      LearnClose        = static_cast<CEGUI::PushButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_CLOSE));
      MouseAimSpeed     = static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOUSEAIMSPEED));
      MouseAimDistance  = static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_MOUSEAIMDISTANCE));
      CameraDistanceMax = static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_CAMERADISTANCEMAX));
      CameraPitchMax    = static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_CAMERAPITCHMAX));
      InvertMouseY      = static_cast<CEGUI::ToggleButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_INVERTMOUSEY));
      CameraCollisions  = static_cast<CEGUI::ToggleButton*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_CAMERACOLLISIONS));
      KeyRotateSpeed    = static_cast<CEGUI::Slider*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_KEYROTATESPEED));
      RightClickAction  = static_cast<CEGUI::Combobox*>(TabInputTabGeneral->getChild(UI_NAME_OPTIONS_TABINPUT_TABGENERAL_RIGHTCLICKACTION));
      
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

      // tabinput - tabactionbuttons3
      LearnAction49 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON49));
      LearnAction50 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON50));
      LearnAction51 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON51));
      LearnAction52 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON52));
      LearnAction53 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON53));
      LearnAction54 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON54));
      LearnAction55 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON55));
      LearnAction56 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON56));
      LearnAction57 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON57));
      LearnAction58 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON58));
      LearnAction59 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON59));
      LearnAction60 = static_cast<CEGUI::PushButton*>(TabInputTabActionButtons3->getChild(UI_NAME_OPTIONS_TABINPUT_TABACTIONBUTTONS3_BUTTON60));
	 
      /******************************************************************************************************/

      // tabengine
      Display           = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISPLAY));
      Resolution        = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_RESOLUTION));
      WindowMode        = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_WINDOWMODE));
      WindowBorders     = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_WINDOWBORDERS));
      VSync             = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_VSYNC));
      FSAA              = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_FSAA));
      Filtering         = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_FILTERING));
      ImageBuilder      = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_IMAGEBUILDER));
      ScalingQuality    = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_SCALINGQUALITY));
      TextureQuality    = static_cast<CEGUI::Combobox*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_TEXTUREQUALITY));
      DisableMipmaps    = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLEMIPMAPS));
      DisableNewRoomTextures = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLENEWROOMTEXTURES));
      Disable3DModels   = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLE3DMODELS));
      DisableNewSky     = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLENEWSKY));
      DisableWeather    = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLEWEATHER));
      Brightness        = static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_BRIGHTNESS));
      Particles         = static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PARTICLES));
      Decoration        = static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DECORATION));
      MusicVolume       = static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_MUSICVOLUME));
      SoundVolume       = static_cast<CEGUI::Slider*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_SOUNDVOLUME));
      DisableLoopSounds = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_DISABLELOOPSOUNDS));

      PreloadRooms      = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADROOMS));
      PreloadRoomTextures = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADROOMTEXTURES));
      PreloadObjects    = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADOBJECTS));
      PreloadSounds     = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADSOUNDS));
      PreloadMusic      = static_cast<CEGUI::ToggleButton*>(TabEngine->getChild(UI_NAME_OPTIONS_TABENGINE_PRELOADMUSIC));

      /******************************************************************************************************/

      // tabgameplay
      Language    = static_cast<CEGUI::Combobox*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_LANGUAGE));
      Safety      = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_SAFETY));
      Grouping    = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_GROUPING));
      SpellPower  = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_SPELLPOWER));
      ReagentBag  = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_REAGENTBAG));
      TempSafe    = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_TEMPSAFE));
      AutoLoot    = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_AUTOLOOT));
      AutoCombine = static_cast<CEGUI::ToggleButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_AUTOCOMBINE));
      OldPasswordDescription = static_cast<CEGUI::Window*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_OLDPASSWORDDESC));
      OldPassword = static_cast<CEGUI::Editbox*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_OLDPASSWORD));
      NewPasswordDescription = static_cast<CEGUI::Window*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_NEWPASSWORDDESC));
      NewPassword = static_cast<CEGUI::Editbox*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_NEWPASSWORD));
      ConfirmPasswordDescription = static_cast<CEGUI::Window*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_CONFIRMPASSWORDDESC));
      ConfirmPassword = static_cast<CEGUI::Editbox*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_CONFIRMPASSWORD));
      ChangePassword = static_cast<CEGUI::PushButton*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_CHANGEPASSWORD));
      SettingsDisabledDescription = static_cast<CEGUI::Window*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_SETTINGSDISABLEDDESCRIPTION));
      ChangePasswordDisabledDescription = static_cast<CEGUI::Window*>(TabGamePlay->getChild(UI_NAME_OPTIONS_TABGAMEPLAY_CHANGEPASSWORDDISABLEDDESCRIPTION));

      /******************************************************************************************************/

      // tabaliases
      ListAliases = static_cast<CEGUI::ItemListbox*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ALIASES));
      AliasKey = static_cast<CEGUI::Editbox*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ADDKEY));
      AliasValue = static_cast<CEGUI::Editbox*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ADDVALUE));
      AliasAddBtn = static_cast<CEGUI::PushButton*>(TabAliases->getChild(UI_NAME_OPTIONS_TABALIASES_ADD));

      /******************************************************************************************************/

      // tabgroups
      DisabledDescription = static_cast<CEGUI::Window*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_DISABLEDDESCRIPTION));
      GroupName         = static_cast<CEGUI::Editbox*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_GROUPNAME));
      MemberName        = static_cast<CEGUI::Editbox*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_MEMBERNAME));
      AddGroup          = static_cast<CEGUI::PushButton*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_ADDGROUP));
      AddMember         = static_cast<CEGUI::PushButton*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_ADDMEMBER));
      NewGroup          = static_cast<CEGUI::Window*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_NEWGROUP));
      NewMember         = static_cast<CEGUI::Window*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_NEWMEMBER));
      GroupsDescription = static_cast<CEGUI::Window*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_GROUPSDESCRIPTION));
      MembersDescription = static_cast<CEGUI::Window*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_MEMBERSDESCRIPTION));
      ListGroups        = static_cast<CEGUI::ItemListbox*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_GROUPS));
      ListMembers       = static_cast<CEGUI::ItemListbox*>(TabGroups->getChild(UI_NAME_OPTIONS_TABGROUPS_MEMBERS));

      /******************************************************************************************************/

      // tababout
      TabAboutTabControl = static_cast<CEGUI::TabControl*>(TabAbout->getChild(UI_NAME_OPTIONS_TABABOUT_TABCONTROL));
      TabAboutTabGeneral = static_cast<CEGUI::Window*>(TabAboutTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABGENERAL));
      TabAboutTabHistory = static_cast<CEGUI::Window*>(TabAboutTabControl->getChild(UI_NAME_OPTIONS_TABABOUT_TABHISTORY));

      // tababout - tabgeneral
      TabAboutTabGeneralVersion = static_cast<CEGUI::Window*>(TabAboutTabGeneral->getChild(UI_NAME_OPTIONS_TABABOUT_TABGENERAL_VERSION));
      TabAboutTabGeneralDistributors = static_cast<CEGUI::Window*>(TabAboutTabGeneral->getChild(UI_NAME_OPTIONS_TABABOUT_TABGENERAL_DISTRIBUTORS));

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

      Ogre::TextureManager& texMan  = Ogre::TextureManager::getSingleton();
      BgfFile^ aboutBgf             = OgreClient::Singleton->ResourceManager->GetObject("about.bgf");

      if (aboutBgf)
      {
         // Evolution
         if (aboutBgf->Frames->Count > 0)
         {
            const ::Ogre::String oStrName = "CEGUI/about.bgf/0";

            Util::CreateTextureA8R8G8B8(aboutBgf->Frames[0], oStrName, UI_RESGROUP_IMAGESETS, 0);
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
            TexturePtr texPtr = texMan.getByName(oStrName);

            if (texPtr)
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
      ImageBuilder->addItem(new::CEGUI::ListboxTextItem("Native"));

      // ScalingQuality values
      ScalingQuality->addItem(new::CEGUI::ListboxTextItem("Low"));
      ScalingQuality->addItem(new::CEGUI::ListboxTextItem("Default"));
      ScalingQuality->addItem(new::CEGUI::ListboxTextItem("High"));

      // TextureQuality values
      TextureQuality->addItem(new::CEGUI::ListboxTextItem("Low"));
      TextureQuality->addItem(new::CEGUI::ListboxTextItem("Default"));
      TextureQuality->addItem(new::CEGUI::ListboxTextItem("High"));

      // maxvalues for options sliders
      Brightness->setMaxValue(0.8f);
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
      WindowMode->setEnabled(false); // disabled until fullscreen is supported
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

      Brightness->setCurrentValue((float)OgreClient::Singleton->Config->BrightnessFactor);
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
      /*                                  PREPARE / SET: GAMEPLAY                                           */
      /******************************************************************************************************/

      // add available languages
      Language->addItem(new::CEGUI::ListboxTextItem(StringConvert::CLRToCEGUI(LanguageCode::English.ToString())));
      Language->addItem(new::CEGUI::ListboxTextItem(StringConvert::CLRToCEGUI(LanguageCode::German.ToString())));

      // set the current one from config
      Language->setText(StringConvert::CLRToCEGUI(OgreClient::Singleton->Config->Language.ToString()));
      Language->selectListItemWithEditboxText();

      // these are disabled until switched enabled by event
      Safety->setEnabled(false);
      Grouping->setEnabled(false);
      SpellPower->setEnabled(false);
      ReagentBag->setEnabled(false);
      TempSafe->setEnabled(false);
      AutoLoot->setEnabled(false);
      AutoCombine->setEnabled(false);

      /******************************************************************************************************/
      /*                                  PREPARE / SET: ALIASES                                            */
      /******************************************************************************************************/

      for (int i = 0; i < OgreClient::Singleton->Config->Aliases->Count; i++)		
         AliasAdd(i);

      /******************************************************************************************************/
      /*                                  PREPARE / SET: GROUPS                                             */
      /******************************************************************************************************/

      for (int i = 0; i < OgreClient::Singleton->Data->Groups->Count; i++)
         GroupAdd(i);

      /******************************************************************************************************/
      /*                                  PREPARE / SET: INPUT                                              */
      /******************************************************************************************************/

      MouseAimSpeed->setMaxValue(100.0f);
      MouseAimSpeed->setClickStep(5.0f);
      MouseAimDistance->setMaxValue(100.0f);
      MouseAimDistance->setClickStep(5.0f);
      CameraDistanceMax->setMaxValue(8.0f * (float)OgreClientConfig::DEFAULTVAL_INPUT_CAMERADISTANCEMAX);
      CameraDistanceMax->setClickStep(128.0f);
      CameraPitchMax->setMaxValue(1.0f);
      CameraPitchMax->setClickStep(0.1f);
      KeyRotateSpeed->setMaxValue(100.0f);

      for (int i = 1; i < 10; i++)
         RightClickAction->addItem(new ::CEGUI::ListboxTextItem("Action 0" + ::CEGUI::PropertyHelper<int>::toString(i), i));

      for (int i = 10; i <= 60; i++)
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
      LearnAction49->setText(keybinding->ActionButton49 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton49));
      LearnAction50->setText(keybinding->ActionButton50 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton50));
      LearnAction51->setText(keybinding->ActionButton51 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton51));
      LearnAction52->setText(keybinding->ActionButton52 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton52));
      LearnAction53->setText(keybinding->ActionButton53 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton53));
      LearnAction54->setText(keybinding->ActionButton54 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton54));
      LearnAction55->setText(keybinding->ActionButton55 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton55));
      LearnAction56->setText(keybinding->ActionButton56 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton56));
      LearnAction57->setText(keybinding->ActionButton57 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton57));
      LearnAction58->setText(keybinding->ActionButton58 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton58));
      LearnAction59->setText(keybinding->ActionButton59 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton59));
      LearnAction60->setText(keybinding->ActionButton60 == KC_UNASSIGNED ? STRINGEMPTY : keyboard->getAsString(keybinding->ActionButton60));

      MouseAimSpeed->setCurrentValue((float)OgreClient::Singleton->Config->MouseAimSpeed);
      MouseAimDistance->setCurrentValue((float)OgreClient::Singleton->Config->MouseAimDistance);
      CameraDistanceMax->setCurrentValue((float)OgreClient::Singleton->Config->CameraDistanceMax);
      CameraPitchMax->setCurrentValue((float)OgreClient::Singleton->Config->CameraPitchMax);
      CameraCollisions->setSelected(OgreClient::Singleton->Config->CameraCollisions);
      InvertMouseY->setSelected(OgreClient::Singleton->Config->InvertMouseY);
      KeyRotateSpeed->setCurrentValue((float)OgreClient::Singleton->Config->KeyRotateSpeed);

      int idx = OgreClient::Singleton->Config->KeyBinding->RightClickAction - 1;
      RightClickAction->getListboxItemFromIndex(idx)->setSelected(true);

      // select
      CEGUI::ListboxItem* itm = RightClickAction->getListboxItemFromIndex(idx);
      itm->setSelected(true);

      // set selected text
      RightClickAction->setText(itm->getText());

      /******************************************************************************************************/
      /*                                  PREPARE / SET: ABOUT                                              */
      /******************************************************************************************************/

      // set version from assembly data
      TabAboutTabGeneralVersion->setText("OgreClient " + StringConvert::CLRToCEGUI(
         ::System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Version->ToString()));

      // set distributor of this binary
#if DISTRIBUTOR_MERIDIANNEXT && !VANILLA && !OPENMERIDIAN
      TabAboutTabGeneralDistributors->setText(
         "Server 105 | US | meridiannext.com\nServer 112 | EU | meridian59.de");
#elif DISTRIBUTOR_VANILLA && VANILLA
      TabAboutTabGeneralDistributors->setText(
         "Server 101 | US | meridian59.com\nServer 102 | US | meridian59.com");
#elif DISTRIBUTOR_OPENMERIDIAN && OPENMERIDIAN
      TabAboutTabGeneralDistributors->setText(
         "Server 103 | US | openmeridian.org"););
#else
      TabAboutTabGeneralDistributors->setText("Unknown Distributor\nCustom Version");
      TabAboutTabGeneralDistributors->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_RED);
#endif

      /******************************************************************************************************/
      /*                                       SET CEGUI EVENTS                                             */
      /******************************************************************************************************/

      // subscribe category buttons
      Engine->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
      Input->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
      GamePlay->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
      Aliases->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
      Groups->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnCategoryButtonClicked));
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
      Brightness->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnBrightnessChanged));
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
      // gameplay events

      Language->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnLanguageChanged));

      // use click event here instead of select-change, to avoid conflicts with code-raised clearing which should not trigger same behaviour
      Safety->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      Grouping->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      SpellPower->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      ReagentBag->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      TempSafe->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      AutoLoot->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      AutoCombine->subscribeEvent(CEGUI::ToggleButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnPreferencesCheckboxClicked));
      
      // change password boxes/button
      OldPassword->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnOldPasswordKeyUp));
      NewPassword->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnNewPasswordKeyUp));
      ConfirmPassword->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnConfirmPasswordKeyUp));
      ChangePassword->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnChangePasswordClicked));
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
      LearnAction49->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction50->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction51->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction52->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction53->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction54->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction55->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction56->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction57->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction58->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction59->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));
      LearnAction60->subscribeEvent(CEGUI::PushButton::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonClicked));

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
      LearnAction49->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction50->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction51->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction52->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction53->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction54->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction55->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction56->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction57->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction58->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction59->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));
      LearnAction60->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnKeyUp));

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
      LearnAction49->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction50->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction51->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction52->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction53->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction54->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction55->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction56->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction57->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction58->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction59->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));
      LearnAction60->subscribeEvent(CEGUI::PushButton::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnKeyLearnButtonDeactivated));

      /******************************************************************************************************/
      // groups events

      AddGroup->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupAddClicked));
      AddMember->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Options::OnMemberAddClicked));

      GroupName->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupNameKeyDown));
      MemberName->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::Options::OnMemberNameKeyDown));

      // subscribe selection change of list
      ListGroups->subscribeEvent(CEGUI::ItemListbox::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupsSelectionChanged));

      /******************************************************************************************************/

      // subscribe other events
      RightClickAction->subscribeEvent(CEGUI::Combobox::EventListSelectionAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnRightClickActionChanged));
      InvertMouseY->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnInvertMouseYChanged));
      CameraCollisions->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnCameraCollisionsChanged));
      MouseAimSpeed->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnMouseAimSpeedChanged));
      MouseAimDistance->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnMouseAimDistanceChanged));
      CameraDistanceMax->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnCameraDistanceMaxChanged));
      CameraPitchMax->subscribeEvent(CEGUI::Slider::EventValueChanged, CEGUI::Event::Subscriber(UICallbacks::Options::OnCameraPitchMaxChanged));
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

      // attach listener to groups
      OgreClient::Singleton->Data->Groups->ListChanged +=
         gcnew ListChangedEventHandler(OnGroupsListChanged);

      // attach listener to data
      OgreClient::Singleton->Data->PropertyChanged +=
         gcnew PropertyChangedEventHandler(OnDataPropertyChanged);

      // attach listener to clientpreferences
      OgreClient::Singleton->Data->ClientPreferences->PropertyChanged +=
         gcnew PropertyChangedEventHandler(OnClientPreferencesPropertyChanged);
   };

   void ControllerUI::Options::Destroy()
   {
      // detach listener from config
      OgreClient::Singleton->Config->PropertyChanged -=
         gcnew PropertyChangedEventHandler(OnConfigPropertyChanged);

      // detach listener from aliases
      OgreClient::Singleton->Config->Aliases->ListChanged -=
         gcnew ListChangedEventHandler(OnAliasListChanged);

      // detach listener from groups
      OgreClient::Singleton->Data->Groups->ListChanged -=
         gcnew ListChangedEventHandler(OnGroupsListChanged);

      // detach listener from data
      OgreClient::Singleton->Data->PropertyChanged -=
         gcnew PropertyChangedEventHandler(OnDataPropertyChanged);

      // detach listener from clientpreferences
      OgreClient::Singleton->Data->ClientPreferences->PropertyChanged -=
         gcnew PropertyChangedEventHandler(OnClientPreferencesPropertyChanged);
   };

   void ControllerUI::Options::ApplyLanguage()
   {
      Window->setText(GetLangWindowTitle(LANGSTR_WINDOW_TITLE::OPTIONS));
   };

   void ControllerUI::Options::OnConfigPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
   };

   void ControllerUI::Options::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_UIMODE))
      {
         UIMode mode = OgreClient::Singleton->Data->UIMode;

         // switch the groups options visibility
         DisabledDescription->setVisible(mode != UIMode::Playing);
         GroupName->setVisible(mode == UIMode::Playing);
         MemberName->setVisible(mode == UIMode::Playing);
         GroupsDescription->setVisible(mode == UIMode::Playing);
         MembersDescription->setVisible(mode == UIMode::Playing);
         AddGroup->setVisible(mode == UIMode::Playing);
         AddMember->setVisible(mode == UIMode::Playing);
         NewGroup->setVisible(mode == UIMode::Playing);
         NewMember->setVisible(mode == UIMode::Playing);
         ListGroups->setVisible(mode == UIMode::Playing);
         ListMembers->setVisible(mode == UIMode::Playing);

         // switch the game options visibility
         SettingsDisabledDescription->setVisible(mode != UIMode::Playing);
         Safety->setVisible(mode == UIMode::Playing);
         Grouping->setVisible(mode == UIMode::Playing);
         SpellPower->setVisible(mode == UIMode::Playing);
         ReagentBag->setVisible(mode == UIMode::Playing);
         TempSafe->setVisible(mode == UIMode::Playing);
         AutoLoot->setVisible(mode == UIMode::Playing);
         AutoCombine->setVisible(mode == UIMode::Playing);
         ChangePasswordDisabledDescription->setVisible(mode != UIMode::Playing);
         OldPasswordDescription->setVisible(mode == UIMode::Playing);
         OldPassword->setVisible(mode == UIMode::Playing);
         NewPasswordDescription->setVisible(mode == UIMode::Playing);
         NewPassword->setVisible(mode == UIMode::Playing);
         ConfirmPasswordDescription->setVisible(mode == UIMode::Playing);
         ConfirmPassword->setVisible(mode == UIMode::Playing);
         ChangePassword->setVisible(mode == UIMode::Playing);
      }
   };

   void ControllerUI::Options::OnClientPreferencesPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      if (CLRString::Equals(e->PropertyName, PreferencesFlags::PROPNAME_ENABLED))
      {
         Safety->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
#if !VANILLA
         // these only get enabled for openmeridian
         Grouping->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
         SpellPower->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
         ReagentBag->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
         TempSafe->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
         AutoLoot->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
         AutoCombine->setEnabled(OgreClient::Singleton->Data->ClientPreferences->Enabled);
#endif
      }
      else
      {
         // update checkboxes to datalayer values
         Safety->setSelected(!OgreClient::Singleton->Data->ClientPreferences->IsSafetyOff);
         Grouping->setSelected(OgreClient::Singleton->Data->ClientPreferences->Grouping);
         SpellPower->setSelected(OgreClient::Singleton->Data->ClientPreferences->SpellPower);
         ReagentBag->setSelected(OgreClient::Singleton->Data->ClientPreferences->ReagentBag);
         TempSafe->setSelected(OgreClient::Singleton->Data->ClientPreferences->TempSafe);
         AutoLoot->setSelected(OgreClient::Singleton->Data->ClientPreferences->AutoLoot);
         AutoCombine->setSelected(OgreClient::Singleton->Data->ClientPreferences->AutoCombine);
      }
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

   void ControllerUI::Options::OnGroupsListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      switch (e->ListChangedType)
      {
      case ::System::ComponentModel::ListChangedType::ItemAdded:
         GroupAdd(e->NewIndex);
         break;

      case ::System::ComponentModel::ListChangedType::ItemDeleted:
         GroupRemove(e->NewIndex);
         break;
      }
   };

   void ControllerUI::Options::OnMembersListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      switch (e->ListChangedType)
      {
      case ::System::ComponentModel::ListChangedType::ItemAdded:
         MemberAdd(e->NewIndex);
         break;

      case ::System::ComponentModel::ListChangedType::ItemDeleted:
         MemberRemove(e->NewIndex);
         break;
      }
   };

   void ControllerUI::Options::AliasAdd(int Index)
   {
      CEGUI::WindowManager* wndMgr  = CEGUI::WindowManager::getSingletonPtr();
      KeyValuePairString^ alias     = OgreClient::Singleton->Config->Aliases[Index];

      // create widget (item)
      CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
         UI_WINDOWTYPE_ALIASLISTBOXITEM);

      // get children
      CEGUI::DragContainer* drag = (CEGUI::DragContainer*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_DRAG);
      CEGUI::PushButton* del     = (CEGUI::PushButton*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_DELETE);
      CEGUI::Editbox* key        = (CEGUI::Editbox*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_KEY);
      CEGUI::Editbox* value      = (CEGUI::Editbox*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_ALIAS_VALUE);

      // set tooltip of dragable icon
      drag->setTooltipText("Drag&Drop me on the Button Grid!");

      // get image in dragger
      CEGUI::Window* icon = drag->getChildAtIdx(0);

      // set alias icon
      icon->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_ALIAS_ICON);

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

   void ControllerUI::Options::GroupAdd(int Index)
   {
      CEGUI::WindowManager& wndMgr  = CEGUI::WindowManager::getSingleton();
      Group^ group                  = OgreClient::Singleton->Data->Groups[Index];

      // create widget (item)
      CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr.createWindow(
         UI_WINDOWTYPE_GROUPLISTBOXITEM);

      // get children
      CEGUI::Window* value	= (CEGUI::Editbox*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_GROUP_VALUE);
      CEGUI::PushButton* del	= (CEGUI::PushButton*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_GROUP_DELETE);

      // set values
      value->setText(StringConvert::CLRToCEGUI(group->Name));

      // subscribe del event
      del->subscribeEvent(
         CEGUI::PushButton::EventClicked,
         CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupDeleteClicked));

      // copy&paste for name value
      //value->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
      //value->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupValueAccepted));
      //value->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupValueAccepted));

      // insert in ui-list
      if ((int)ListGroups->getItemCount() > Index)
         ListGroups->insertItem(widget, ListGroups->getItemFromIndex(Index));

      // or add
      else
         ListGroups->addItem(widget);

      // fix a bug with last item not visible
      ListGroups->notifyScreenAreaChanged(true);
   }

   void ControllerUI::Options::GroupRemove(int Index)
   {
      if ((int)ListGroups->getItemCount() > Index)
      {
         CEGUI::ItemEntry* entry = ListGroups->getItemFromIndex(Index);

         if (entry->isSelected())
         {
            // remove old ui entries
            int count = (int)ListMembers->getItemCount();
            for (int i = count - 1; i >= 0; i--)
               ControllerUI::Options::MemberRemove((int)i);

            CurrentGroup = nullptr;
         }

         ListGroups->removeItem(entry);
      }
   };

   void ControllerUI::Options::MemberAdd(int Index)
   {
      if (CurrentGroup == nullptr)
         return;

      CEGUI::WindowManager& wndMgr  = CEGUI::WindowManager::getSingleton();
      GroupMember^ member           = CurrentGroup->Members[Index];

      // create widget (item)
      CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr.createWindow(
         UI_WINDOWTYPE_GROUPMEMBERLISTBOXITEM);

      // get children
      CEGUI::Window* value = (CEGUI::Editbox*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_GROUPMEMBER_VALUE);
      CEGUI::PushButton* del = (CEGUI::PushButton*)widget->getChildAtIdx(UI_OPTIONS_CHILDINDEX_GROUPMEMBER_DELETE);

      // set values
      value->setText(StringConvert::CLRToCEGUI(member->Name));

      // subscribe del event
      del->subscribeEvent(
         CEGUI::PushButton::EventClicked,
         CEGUI::Event::Subscriber(UICallbacks::Options::OnMemberDeleteClicked));

      // copy&paste for name value
      //value->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
      //value->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupValueAccepted));
      //value->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Options::OnGroupValueAccepted));

      // insert in ui-list
      if ((int)ListMembers->getItemCount() > Index)
         ListMembers->insertItem(widget, ListMembers->getItemFromIndex(Index));

      // or add
      else
         ListMembers->addItem(widget);

      // fix a bug with last item not visible
      ListMembers->notifyScreenAreaChanged(true);
   }

   void ControllerUI::Options::MemberRemove(int Index)
   {
      if ((int)ListMembers->getItemCount() > Index)
         ListMembers->removeItem(ListMembers->getItemFromIndex(Index));
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::Options::OnCategoryButtonClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn        = (const CEGUI::PushButton*)args.window;

      if (btn == ControllerUI::Options::Engine)
         ControllerUI::Options::TabControl->setSelectedTabAtIndex(0);

      else if (btn == ControllerUI::Options::Input)
         ControllerUI::Options::TabControl->setSelectedTabAtIndex(1);

      else if (btn == ControllerUI::Options::GamePlay)
         ControllerUI::Options::TabControl->setSelectedTabAtIndex(2);

      else if (btn == ControllerUI::Options::Aliases)
         ControllerUI::Options::TabControl->setSelectedTabAtIndex(3);

      else if (btn == ControllerUI::Options::Groups)
         ControllerUI::Options::TabControl->setSelectedTabAtIndex(4);

      else if (btn == ControllerUI::Options::About)
         ControllerUI::Options::TabControl->setSelectedTabAtIndex(5);

      return true;
   };

   bool UICallbacks::Options::OnKeyLearnKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
      CEGUI::PushButton* btn          = (CEGUI::PushButton*)args.window;
      ::OIS::Keyboard* keyboard       = ControllerInput::OISKeyboard;
      OISKeyBinding^ keybinding       = OgreClient::Singleton->Config->KeyBinding;
      const std::string& keystr       = keyboard->getAsString((::OIS::KeyCode)args.scancode);

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
         OgreClient::Singleton->Data->ActionButtons[0]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction02)
      {
         keybinding->ActionButton02 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction02->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[1]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction03)
      {
         keybinding->ActionButton03 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction03->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[2]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction04)
      {
         keybinding->ActionButton04 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction04->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[3]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction05)
      {
         keybinding->ActionButton05 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction05->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[4]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction06)
      {
         keybinding->ActionButton06 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction06->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[5]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction07)
      {
         keybinding->ActionButton07 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction07->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[6]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction08)
      {
         keybinding->ActionButton08 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction08->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[7]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction09)
      {
         keybinding->ActionButton09 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction09->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[8]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction10)
      {
         keybinding->ActionButton10 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction10->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[9]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction11)
      {
         keybinding->ActionButton11 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction11->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[10]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction12)
      {
         keybinding->ActionButton12 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction12->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[11]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction13)
      {
         keybinding->ActionButton13 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction13->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[12]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction14)
      {
         keybinding->ActionButton14 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction14->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[13]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction15)
      {
         keybinding->ActionButton15 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction15->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[14]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction16)
      {
         keybinding->ActionButton16 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction16->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[15]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction17)
      {
         keybinding->ActionButton17 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction17->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[16]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction18)
      {
         keybinding->ActionButton18 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction18->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[17]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction19)
      {
         keybinding->ActionButton19 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction19->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[18]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction20)
      {
         keybinding->ActionButton20 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction20->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[19]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction21)
      {
         keybinding->ActionButton21 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction21->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[20]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction22)
      {
         keybinding->ActionButton22 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction22->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[21]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction23)
      {
         keybinding->ActionButton23 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction23->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[22]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction24)
      {
         keybinding->ActionButton24 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction24->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[23]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction25)
      {
         keybinding->ActionButton25 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction25->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[24]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction26)
      {
         keybinding->ActionButton26 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction26->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[25]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction27)
      {
         keybinding->ActionButton27 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction27->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[26]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction28)
      {
         keybinding->ActionButton28 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction28->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[27]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction29)
      {
         keybinding->ActionButton29 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction29->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[28]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction30)
      {
         keybinding->ActionButton30 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction30->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[29]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction31)
      {
         keybinding->ActionButton31 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction31->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[30]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction32)
      {
         keybinding->ActionButton32 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction32->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[31]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction33)
      {
         keybinding->ActionButton33 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction33->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[32]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction34)
      {
         keybinding->ActionButton34 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction34->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[33]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction35)
      {
         keybinding->ActionButton35 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction35->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[34]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction36)
      {
         keybinding->ActionButton36 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction36->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[35]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction37)
      {
         keybinding->ActionButton37 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction37->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[36]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction38)
      {
         keybinding->ActionButton38 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction38->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[37]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction39)
      {
         keybinding->ActionButton39 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction39->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[38]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction40)
      {
         keybinding->ActionButton40 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction40->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[39]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction41)
      {
         keybinding->ActionButton41 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction41->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[40]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction42)
      {
         keybinding->ActionButton42 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction42->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[41]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction43)
      {
         keybinding->ActionButton43 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction43->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[42]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction44)
      {
         keybinding->ActionButton44 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction44->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[43]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction45)
      {
         keybinding->ActionButton45 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction45->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[44]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction46)
      {
         keybinding->ActionButton46 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction46->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[45]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction47)
      {
         keybinding->ActionButton47 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction47->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[46]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction48)
      {
         keybinding->ActionButton48 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction48->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[47]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction49)
      {
         keybinding->ActionButton49 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction49->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[48]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction50)
      {
         keybinding->ActionButton50 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction50->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[49]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction51)
      {
         keybinding->ActionButton51 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction51->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[50]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction52)
      {
         keybinding->ActionButton52 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction52->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[51]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction53)
      {
         keybinding->ActionButton53 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction53->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[52]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction54)
      {
         keybinding->ActionButton54 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction54->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[53]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction55)
      {
         keybinding->ActionButton55 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction55->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[54]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction56)
      {
         keybinding->ActionButton56 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction56->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[55]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction57)
      {
         keybinding->ActionButton57 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction57->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[56]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction58)
      {
         keybinding->ActionButton58 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction58->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[57]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction59)
      {
         keybinding->ActionButton59 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction59->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[58]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }
      else if (btn == ControllerUI::Options::LearnAction60)
      {
         keybinding->ActionButton60 = (::OIS::KeyCode)args.scancode;
         ControllerUI::Options::LearnAction60->setText(keystr);
         OgreClient::Singleton->Data->ActionButtons[59]->Label
            = StringConvert::CEGUIToCLR(keystr);
      }

      // deactivate focus
      btn->deactivate();

      return true;
   };

   bool UICallbacks::Options::OnKeyLearnButtonClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = (const CEGUI::MouseEventArgs&)e;
      CEGUI::PushButton* btn            = (CEGUI::PushButton*)args.window;
      OISKeyBinding^ keybinding         = OgreClient::Singleton->Config->KeyBinding;

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
         else if (btn == ControllerUI::Options::LearnAction49)
         {
            keybinding->ActionButton49 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction49->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction50)
         {
            keybinding->ActionButton50 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction50->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction51)
         {
            keybinding->ActionButton51 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction51->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction52)
         {
            keybinding->ActionButton52 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction52->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction53)
         {
            keybinding->ActionButton53 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction53->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction54)
         {
            keybinding->ActionButton54 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction54->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction55)
         {
            keybinding->ActionButton55 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction55->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction56)
         {
            keybinding->ActionButton56 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction56->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction57)
         {
            keybinding->ActionButton57 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction57->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction58)
         {
            keybinding->ActionButton58 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction58->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction59)
         {
            keybinding->ActionButton59 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction59->setText(STRINGEMPTY);
         }
         else if (btn == ControllerUI::Options::LearnAction60)
         {
            keybinding->ActionButton60 = ::OIS::KeyCode::KC_UNASSIGNED;
            ControllerUI::Options::LearnAction60->setText(STRINGEMPTY);
         }
      }

      return true;
   };

   bool UICallbacks::Options::OnKeyLearnButtonDeactivated(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn        = (const CEGUI::PushButton*)args.window;

      ControllerUI::SplashNotifier::HideNotification(UI_NOTIFICATION_PRESSAKEY);

      return true;
   };

   bool UICallbacks::Options::OnDisplayChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;

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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;

      CLRString^ newval = StringConvert::CEGUIToCLR(combobox->getText());
      CLRString^ oldval = OgreClient::Singleton->Config->Resolution;

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

      CLRString^ newval = StringConvert::CEGUIToCLR(combobox->getText());
      CLRString^ oldval = OgreClient::Singleton->Config->FSAA;

      if (oldval == newval)
         return true;

      OgreClient::Singleton->Config->FSAA = newval;
      OgreClient::Singleton->RecreateWindow = true;

      return true;
   };

   bool UICallbacks::Options::OnFilteringChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;
      ::Ogre::MaterialManager& matMan     = ::Ogre::MaterialManager::getSingleton();

      CLRString^ newval = StringConvert::CEGUIToCLR(combobox->getText());
      CLRString^ oldval = OgreClient::Singleton->Config->TextureFiltering;

      if (oldval == newval)
         return true;

      // save in config
      OgreClient::Singleton->Config->TextureFiltering = newval;

      // apply
      if (CLRString::Equals(newval, "Off"))
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_NONE);

      else if (CLRString::Equals(newval, "Bilinear"))
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_BILINEAR);

      else if (CLRString::Equals(newval, "Trilinear"))
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_TRILINEAR);

      else if (CLRString::Equals(newval, "Anisotropic x4"))
      {
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
         matMan.setDefaultAnisotropy(4);
      }
      else if (CLRString::Equals(newval, "Anisotropic x16"))
      {
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
         matMan.setDefaultAnisotropy(16);
      }

      return true;
   };

   bool UICallbacks::Options::OnImageBuilderChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;

      CLRString^ newval = StringConvert::CEGUIToCLR(combobox->getText());
      CLRString^ oldval = OgreClient::Singleton->Config->ImageBuilder;

      if (oldval == newval)
         return true;

      OgreClient::Singleton->Config->ImageBuilder = newval;
      // todo

      return true;
   };

   bool UICallbacks::Options::OnScalingQualityChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;

      CLRString^ newval = StringConvert::CEGUIToCLR(combobox->getText());
      CLRString^ oldval = OgreClient::Singleton->Config->BitmapScaling;

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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;

      CLRString^ newval = StringConvert::CEGUIToCLR(combobox->getText());
      CLRString^ oldval = OgreClient::Singleton->Config->TextureQuality;

      if (oldval == newval)
         return true;

      // save new value
      OgreClient::Singleton->Config->TextureQuality = newval;

      // apply new value
      if (newval == "Low")
      {
         ImageComposerOgre<RoomObject^>::DefaultQuality       = 0.25f; // used in RemoteNode2D
         ImageComposerCEGUI<ObjectBase^>::DefaultQuality      = 0.25f; // used in CEGUI
         ImageComposerCEGUI<RoomObject^>::DefaultQuality      = 0.25f; // used in CEGUI
         ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.25f; // used in CEGUI
      }
      else if (newval == "Default")
      {
         ImageComposerOgre<RoomObject^>::DefaultQuality       = 0.5f; // used in RemoteNode2D
         ImageComposerCEGUI<ObjectBase^>::DefaultQuality      = 0.5f; // used in CEGUI
         ImageComposerCEGUI<RoomObject^>::DefaultQuality      = 0.5f; // used in CEGUI
         ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.5f; // used in CEGUI
      }
      else if (newval == "High")
      {
         ImageComposerOgre<RoomObject^>::DefaultQuality       = 1.0f; // used in RemoteNode2D
         ImageComposerCEGUI<ObjectBase^>::DefaultQuality      = 1.0f; // used in CEGUI
         ImageComposerCEGUI<RoomObject^>::DefaultQuality      = 1.0f; // used in CEGUI
         ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 1.0f; // used in CEGUI
      }

      return true;
   };

   bool UICallbacks::Options::OnDisableMipmapsChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

      bool newval = !toggleb->isSelected();
      bool oldval = OgreClient::Singleton->Config->DisableNewSky;

      if (oldval == newval)
         return true;

      OgreClient::Singleton->Config->DisableNewSky = newval;

      ControllerRoom::DestroyCaelum(); 
      ControllerRoom::InitCaelum();
      ControllerRoom::UpdateSky();

      return true;
   };

   bool UICallbacks::Options::OnDisableWeatherEffectsChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* toggleb  = (const CEGUI::ToggleButton*)args.window;

      bool newval = !toggleb->isSelected();
      bool oldval = OgreClient::Singleton->Config->DisableWeatherEffects;

      if (oldval == newval)
         return true;

      OgreClient::Singleton->Config->DisableWeatherEffects = newval;
      // todo

      return true;
   };

   bool UICallbacks::Options::OnBrightnessChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->BrightnessFactor = (float)slider->getCurrentValue();
      ControllerRoom::AdjustAmbientLight();

      return true;
   };

   bool UICallbacks::Options::OnParticlesChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider         = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->WeatherParticles = (int)slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnDecorationChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider         = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->DecorationIntensity = (int)slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnMusicVolumeChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider         = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->MusicVolume = slider->getCurrentValue();

      // applies the new musicvolume on playing sound
      ControllerSound::AdjustMusicVolume();

      return true;
   };

   bool UICallbacks::Options::OnSoundVolumeChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider         = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->SoundVolume = slider->getCurrentValue();

      // applies the new soundvolume on playing sound
      ControllerSound::AdjustSoundVolume();

      return true;
   };

   bool UICallbacks::Options::OnDisableLoopSoundsChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* btn      = (const CEGUI::ToggleButton*)args.window;

      OgreClient::Singleton->Config->DisableLoopSounds = btn->isSelected();

      return true;
   };

   bool UICallbacks::Options::OnPreloadChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* btn      = (const CEGUI::ToggleButton*)args.window;

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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox     = (const CEGUI::Combobox*)args.window;

      OgreClient::Singleton->Config->KeyBinding->RightClickAction = (int)combobox->getSelectedItem()->getID();

      return true;
   };

   bool UICallbacks::Options::OnInvertMouseYChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* btn      = (const CEGUI::ToggleButton*)args.window;

      OgreClient::Singleton->Config->InvertMouseY = btn->isSelected();

      return true;
   };

   bool UICallbacks::Options::OnCameraCollisionsChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* btn = (const CEGUI::ToggleButton*)args.window;

      OgreClient::Singleton->Config->CameraCollisions = btn->isSelected();

      return true;
   };

   bool UICallbacks::Options::OnMouseAimSpeedChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider         = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->MouseAimSpeed = (int)slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnMouseAimDistanceChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->MouseAimDistance = (int)slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnCameraDistanceMaxChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->CameraDistanceMax = slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnCameraPitchMaxChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->CameraPitchMax = slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnKeyRotateSpeedChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Slider* slider         = (const CEGUI::Slider*)args.window;

      OgreClient::Singleton->Config->KeyRotateSpeed = (int)slider->getCurrentValue();

      return true;
   };

   bool UICallbacks::Options::OnAliasAddClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn = (const CEGUI::PushButton*)args.window;

      const ::CEGUI::String& key = ControllerUI::Options::AliasKey->getText();
      const ::CEGUI::String& val = ControllerUI::Options::AliasValue->getText();

      CLRString^ keyclr = StringConvert::CEGUIToCLR(key);
      CLRString^ valclr = StringConvert::CEGUIToCLR(val);

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
      const CEGUI::WindowEventArgs& args  = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn        = (const CEGUI::PushButton*)args.window;

      CEGUI::ItemListbox* list = ControllerUI::Options::ListAliases;

      int idx = (int)list->getItemIndex((CEGUI::ItemEntry*)btn->getParent());

      OgreClient::Singleton->Config->Aliases->RemoveAt(idx);

      return true;
   };

   bool UICallbacks::Options::OnAliasKeyAccepted(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Editbox* box = (const CEGUI::Editbox*)args.window;

      CEGUI::ItemListbox* list = ControllerUI::Options::ListAliases;
      CEGUI::ItemEntry* entry = (CEGUI::ItemEntry*)box->getParent();

      size_t idx = list->getItemIndex(entry);

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

   bool UICallbacks::Options::OnGroupAddClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn       = (const CEGUI::PushButton*)args.window;
      CEGUI::ItemListbox* list           = ControllerUI::Options::ListGroups;
      GroupList^ grplist                 = OgreClient::Singleton->Data->Groups;

      // get groupname from ui-element and convert to clr
      const ::CEGUI::String& grpname = ControllerUI::Options::GroupName->getText();
      CLRString^ nameclr             = StringConvert::CEGUIToCLR(grpname);

      // remove whitespaces
      nameclr = nameclr->Trim();

      if (nameclr == STRINGEMPTY)
         return true;

      // clear text
      ControllerUI::Options::GroupName->setText(STRINGEMPTY);

      // must not have this group already
      if (grplist->GetItemByName(nameclr, false) != nullptr)
         return true;

      // add the new group to data
      Group^ newgrp = gcnew Group(nameclr);
      grplist->Add(newgrp);

      size_t index = (size_t)grplist->IndexOf(newgrp);
      list->clearAllSelections();
      list->selectRange(index, index);
      list->ensureItemIsVisibleVert(*list->getItemFromIndex(index));

      return true;
   };

   bool UICallbacks::Options::OnGroupDeleteClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn       = (const CEGUI::PushButton*)args.window;
      const CEGUI::ItemListbox* list     = ControllerUI::Options::ListGroups;

      // get index
      size_t idx = list->getItemIndex((CEGUI::ItemEntry*)btn->getParent());

      // remove from model-list
      OgreClient::Singleton->Data->Groups->RemoveAt((int)idx);

      return true;
   };

   bool UICallbacks::Options::OnMemberAddClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn       = (const CEGUI::PushButton*)args.window;
      CEGUI::ItemListbox* list           = ControllerUI::Options::ListMembers;
      Group^ currentGroup                = ControllerUI::Options::CurrentGroup;

      // get membername from ui-element and conver to clr
      const ::CEGUI::String& membername = ControllerUI::Options::MemberName->getText();
      CLRString^ nameclr                = StringConvert::CEGUIToCLR(membername);

      // remove whitespaces
      nameclr = nameclr->Trim();

      if (nameclr == STRINGEMPTY)
         return true;

      // clear text
      ControllerUI::Options::MemberName->setText(STRINGEMPTY);

      // no group selected
      if (currentGroup == nullptr)
         return true;

      // must not have this name already
      if (currentGroup->Members->GetItemByName(nameclr, false) != nullptr)
         return true;

      // create new groupmember data entry
      GroupMember^ newmember = gcnew GroupMember(nameclr, false);

      // add the new member to group
      currentGroup->Members->Add(newmember);

      size_t index = (size_t)currentGroup->Members->IndexOf(newmember);
      list->ensureItemIsVisibleVert(*list->getItemFromIndex(index));

      return true;
   };

   bool UICallbacks::Options::OnMemberDeleteClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::PushButton* btn       = (const CEGUI::PushButton*)args.window;
      const CEGUI::ItemListbox* list     = ControllerUI::Options::ListMembers;
      Group^ currentGroup                = ControllerUI::Options::CurrentGroup;

      // must have a group selected
      if (currentGroup == nullptr)
         return true;

      // get index
      size_t idx = list->getItemIndex((CEGUI::ItemEntry*)btn->getParent());

      // remove from model-list
      currentGroup->Members->RemoveAt((int)idx);

      return true;
   };

   bool UICallbacks::Options::OnGroupsSelectionChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::ItemListbox* groups = ControllerUI::Options::ListGroups;
      CEGUI::ItemListbox* members      = ControllerUI::Options::ListMembers;
      CEGUI::ItemEntry* item           = groups->getFirstSelectedItem();
      GroupList^ groupsData            = OgreClient::Singleton->Data->Groups;

      // detach listener and remove old members
      if (ControllerUI::Options::CurrentGroup != nullptr)
      {
         // detach listener from CurrentGroup Members
         ControllerUI::Options::CurrentGroup->Members->ListChanged -=
            gcnew ListChangedEventHandler(ControllerUI::Options::OnMembersListChanged);

         // remove old ui entries
         int count = (int)members->getItemCount();
         for (int i = count - 1; i >= 0; i--)
            ControllerUI::Options::MemberRemove((int)i);
      }

      // no selection anymore
      if (!item)
      {
         ControllerUI::Options::CurrentGroup = nullptr;

         // remove old ui entries
         int count = (int)members->getItemCount();
         for (int i = count - 1; i >= 0; i--)
            ControllerUI::Options::MemberRemove((int)i);
      }
      else
      {
         // get selected ui index
         int index = (int)groups->getItemIndex(item);

         // get the datamodel for index and keep a reference
         ControllerUI::Options::CurrentGroup = groupsData[index];

         // create new ui entries
         for (int i = 0; i < ControllerUI::Options::CurrentGroup->Members->Count; i++)
            ControllerUI::Options::MemberAdd(i);

         // attach listener from CurrentGroup Members
         ControllerUI::Options::CurrentGroup->Members->ListChanged +=
            gcnew ListChangedEventHandler(ControllerUI::Options::OnMembersListChanged);
      }

      return true;
   };

   bool UICallbacks::Options::OnGroupNameKeyDown(const CEGUI::EventArgs& e)
   {
      // handle copy&paste generically
      if (UICallbacks::OnCopyPasteKeyDown(e))
         return true;

      // handle other keys
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
      bool handled = false;

      switch (args.scancode)
      {
      case CEGUI::Key::Scan::Return:
         OnGroupAddClicked(::CEGUI::WindowEventArgs(ControllerUI::Options::AddGroup));
         handled = true;
         break;
      }

      return handled;
   };

   bool UICallbacks::Options::OnMemberNameKeyDown(const CEGUI::EventArgs& e)
   {
      // handle copy&paste generically
      if (UICallbacks::OnCopyPasteKeyDown(e))
         return true;

      // handle other keys
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
      bool handled = false;

      switch (args.scancode)
      {
      case CEGUI::Key::Scan::Return:
         OnMemberAddClicked(::CEGUI::WindowEventArgs(ControllerUI::Options::AddMember));
         handled = true;
         break;
      }

      return handled;
   };

   bool UICallbacks::Options::OnLanguageChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::Combobox* combobox = (const CEGUI::Combobox*)args.window;

      CLRString^ strval = StringConvert::CEGUIToCLR(combobox->getText());	
      LanguageCode newval;

      // something wrong
      if (!::System::Enum::TryParse<LanguageCode>(strval, newval))
         return true;

      // unchanged
      if (OgreClient::Singleton->Config->Language == newval)
         return true;

      // set new language
      OgreClient::Singleton->Config->Language = newval;
      OgreClient::Singleton->ResourceManager->StringResources->Language = newval;

      // apply language
      ControllerUI::ApplyLanguage();

      // resolve again all rsb strings on existing datamodels
      OgreClient::Singleton->Data->ResolveStrings(OgreClient::Singleton->ResourceManager->StringResources, true);

      return true;
   };

   bool UICallbacks::Options::OnPreferencesCheckboxClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      const CEGUI::ToggleButton* btn = (const CEGUI::ToggleButton*)args.window;

      // don't do anything if not ingame with a toon/preferences enabled
      if (!OgreClient::Singleton->Data->ClientPreferences->Enabled)
         return true;

#if !VANILLA
      if (btn == ControllerUI::Options::Safety)
         OgreClient::Singleton->Data->ClientPreferences->IsSafetyOff = !btn->isSelected();

      else if (btn == ControllerUI::Options::Grouping)
         OgreClient::Singleton->Data->ClientPreferences->Grouping = btn->isSelected();

      else if (btn == ControllerUI::Options::SpellPower)
         OgreClient::Singleton->Data->ClientPreferences->SpellPower = btn->isSelected();

      else if (btn == ControllerUI::Options::ReagentBag)
         OgreClient::Singleton->Data->ClientPreferences->ReagentBag = btn->isSelected();

      else if (btn == ControllerUI::Options::TempSafe)
         OgreClient::Singleton->Data->ClientPreferences->TempSafe = btn->isSelected();

      else if (btn == ControllerUI::Options::AutoLoot)
         OgreClient::Singleton->Data->ClientPreferences->AutoLoot = btn->isSelected();

      else if (btn == ControllerUI::Options::AutoCombine)
         OgreClient::Singleton->Data->ClientPreferences->AutoCombine = btn->isSelected();

      // send updated values to server
      OgreClient::Singleton->SendUserCommandSendPreferences();
#else
      if (btn == ControllerUI::Options::Safety)
         OgreClient::Singleton->SendUserCommandSafetyMessage(btn->isSelected());
#endif

      return true;
   };

   bool UICallbacks::Options::OnOldPasswordKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
      const CEGUI::Editbox* editbox = (const CEGUI::Editbox*)args.window;

      if (args.scancode == ::CEGUI::Key::Scan::Tab)
      {
         ControllerUI::Options::NewPassword->activate();
      }

      return true;
   };

   bool UICallbacks::Options::OnNewPasswordKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
      const CEGUI::Editbox* editbox = (const CEGUI::Editbox*)args.window;

      if (args.scancode == ::CEGUI::Key::Scan::Tab)
      {
         ControllerUI::Options::ConfirmPassword->activate();
      }

      return true;
   };

   bool UICallbacks::Options::OnConfirmPasswordKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
      const CEGUI::Editbox* editbox = (const CEGUI::Editbox*)args.window;

      if (args.scancode == ::CEGUI::Key::Scan::Tab
         && !ControllerUI::Options::ChangePassword->isDisabled())
      {
         ControllerUI::Options::ChangePassword->activate();
      }

      return true;
   };

   bool UICallbacks::Options::OnChangePasswordClicked(const CEGUI::EventArgs& e)
   {
      CLRString^ oldPassword = StringConvert::CEGUIToCLR(ControllerUI::Options::OldPassword->getText());
      CLRString^ newPassword = StringConvert::CEGUIToCLR(ControllerUI::Options::NewPassword->getText());
      CLRString^ confirmPassword = StringConvert::CEGUIToCLR(ControllerUI::Options::ConfirmPassword->getText());

      if (oldPassword == "" || newPassword == "" || confirmPassword == "")
      {
         ControllerUI::ConfirmPopup::ShowOK("Please fill out all password fields.", 0, false);

         return true;
      }

      if (oldPassword != OgreClient::Singleton->Config->SelectedConnectionInfo->Password)
      {
         ControllerUI::ConfirmPopup::ShowOK("Old password incorrect.", 0, false);

         return true;
      }

      if (newPassword != confirmPassword)
      {
         ControllerUI::ConfirmPopup::ShowOK("New passwords do not match.", 0, false);

         return true;
      }

      if (oldPassword == newPassword)
      {
         ControllerUI::ConfirmPopup::ShowOK("New password is same as old password.", 0, false);

         return true;
      }

      OgreClient::Singleton->SendReqChangePassword(oldPassword, newPassword);

      OgreClient::Singleton->Config->SelectedConnectionInfo->Password = newPassword;

      return true;
   };
};};
