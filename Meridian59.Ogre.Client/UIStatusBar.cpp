#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::StatusBar::Initialize()
   {
      // setup references to children from xml nodes
      Window            = static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_STATUSBAR_WINDOW));
      FPSDescription    = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_FPSDESC));
      FPSValue          = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_FPSVAL));
      RTTDescription    = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_RTTDESC));
      RTTValue          = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_RTTVAL));
      PlayersDescription = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_PLAYERSDESC));
      PlayersValue      = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_PLAYERSVAL));
      MoodDescription   = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_MOODDESC));
      MoodHappy         = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODHAPPY));
      MoodNeutral       = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODNEUTRAL));
      MoodSad           = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODSAD));
      MoodAngry         = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODANGRY));
      SafetyDescription = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_SAFETYDESC));
      SafetyValue       = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_SAFETYVAL));
      MTimeDescription  = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_MTIMEDESC));
      MTimeValue        = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_MTIMEVAL));
      RoomDescription   = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_ROOMDESC));
      RoomValue         = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_ROOMVAL));
      Reset             = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_RESET));
      Lock              = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_LOCK));

      // attach listener to Data
      OgreClient::Singleton->Data->PropertyChanged += 
         gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnDataPropertyChanged);

      // attach listener to Preference Flags
      OgreClient::Singleton->Data->ClientPreferences->PropertyChanged +=
         gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnClientPreferencesChanged);

      // attach listener to roominformation
      OgreClient::Singleton->Data->RoomInformation->PropertyChanged += 
         gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnRoomInformationPropertyChanged);

      // attach listener to onlineplayers
      OgreClient::Singleton->Data->OnlinePlayers->ListChanged += 
         gcnew ListChangedEventHandler(&ControllerUI::StatusBar::OnOnlinePlayersListChanged);

      // subscribe mood clicks
      MoodHappy->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnMoodHappyClicked));
      MoodNeutral->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnMoodNeutralClicked));
      MoodSad->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnMoodSadClicked));
      MoodAngry->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnMoodAngryClicked));

      // subscribe safety click
      SafetyValue->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnSafetyClicked));

      // subscribe players click
      PlayersValue->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnPlayersClicked));

      // subscribe ui reset click
      Reset->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnResetClicked));

      // subscribe ui lock click
      Lock->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnLockClicked));

      // subscribe fps click
      FPSValue->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatusBar::OnFPSClicked));
   };

   void ControllerUI::StatusBar::Destroy()
   {
      // detach listener from Data
      OgreClient::Singleton->Data->PropertyChanged -= 
         gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnDataPropertyChanged);

      // detach listener from Preference Flags
      OgreClient::Singleton->Data->ClientPreferences->PropertyChanged -=
         gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnClientPreferencesChanged);

      // detach listener from roominformation
      OgreClient::Singleton->Data->RoomInformation->PropertyChanged -= 
         gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnRoomInformationPropertyChanged);

      // detach listener from onlineplayers
      OgreClient::Singleton->Data->OnlinePlayers->ListChanged -= 
         gcnew ListChangedEventHandler(&ControllerUI::StatusBar::OnOnlinePlayersListChanged);
   };

   void ControllerUI::StatusBar::ApplyLanguage()
   {
      const bool LOCKED = OgreClient::Singleton->Config->UILocked;

      // Set Tooltips for selected language
      FPSDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::FPS_TOOLTIP));
      FPSValue->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::FPS_TOOLTIP));
      RTTDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::PING_TOOLTIP));
      RTTValue->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::PING_TOOLTIP));
      PlayersDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::PLAYERCOUNT_TOOLTIP));
      PlayersValue->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::PLAYERCOUNT_TOOLTIP));
      MoodDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::MOOD_TOOLTIP));
      MoodHappy->setTooltipText(GetLangTooltipMood(LANGSTR_TOOLTIP_MOOD::HAPPY));
      MoodNeutral->setTooltipText(GetLangTooltipMood(LANGSTR_TOOLTIP_MOOD::NEUTRAL));
      MoodSad->setTooltipText(GetLangTooltipMood(LANGSTR_TOOLTIP_MOOD::SAD));
      MoodAngry->setTooltipText(GetLangTooltipMood(LANGSTR_TOOLTIP_MOOD::ANGRY));
      SafetyDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::SAFETY_TOOLTIP));
      SafetyValue->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::SAFETY_TOOLTIP));
      MTimeDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::TIME_TOOLTIP));
      MTimeValue->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::TIME_TOOLTIP));
      RoomDescription->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::ROOM_TOOLTIP));
      RoomValue->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::ROOM_TOOLTIP));

      if (LOCKED)
      {
         Lock->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UILOCKED_TOOLTIP));
         Reset->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UIRESETLOCKED_TOOLTIP));
      }
      else
      {
         Lock->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UIUNLOCKED_TOOLTIP));
         Reset->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UIRESET_TOOLTIP));
      }
      
      ControllerUI::StatusBar::Reset->setDisabled(LOCKED);
      ControllerUI::ApplyLock();
      // Set Descriptions for selected language
      /*FPSDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::FPS_DESCRIPTION));
      RTTDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::PING_DESCRIPTION));
      PlayersDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::PLAYERCOUNT_DESCRIPTION));
      MoodDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::MOOD_DESCRIPTION));
      SafetyDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::SAFETY_DESCRIPTION));
      MTimeDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::TIME_DESCRIPTION));
      RoomDescription->setText(GetLangDescriptionStatusBar(LANGSTR_DESCRIPTION_STATUSBAR::ROOM_DESCRIPTION));*/
   };

   void ControllerUI::StatusBar::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      // update FPS/TPS
      if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_TPS))
      {
         unsigned int tps = OgreClient::Singleton->Data->TPS;

         // set color
         if (tps >= FPSValues::GOOD)
            FPSValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_PALEGREEN);

         else if (tps >= FPSValues::OK)
            FPSValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_YELLOW);

         else if (tps >= FPSValues::BAD)
            FPSValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_ORANGE);

         else
            FPSValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_DARKRED);

         // set value
         FPSValue->setText(::CEGUI::PropertyHelper<unsigned int>::toString(tps));
      }

      // update RTT
      else if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_RTT))
      {
         unsigned int rtt = OgreClient::Singleton->Data->RTT;

         // set color
         if (rtt <= RTTValues::GOOD)
            RTTValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_PALEGREEN);

         else if (rtt <= RTTValues::OK)
            RTTValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_YELLOW);

         else if (rtt <= RTTValues::BAD)
            RTTValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_ORANGE);

         else
            RTTValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, UI_COLOR_DARKRED);

         // set value
         RTTValue->setText(::CEGUI::PropertyHelper<unsigned int>::toString(rtt));
      }

      // update Meridian Time
      else if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_MERIDIANTIME))
      {
         MTimeValue->setText(
            StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->MeridianTime.ToShortTimeString()));
      }
   };

   void ControllerUI::StatusBar::OnClientPreferencesChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      if (CLRString::Equals(e->PropertyName, PreferencesFlags::PROPNAME_FLAGS))
      {
         const bool isSafetyOff = OgreClient::Singleton->Data->ClientPreferences->IsSafetyOff;
         const CEGUI::String& color = (isSafetyOff ? UI_COLOR_DARKRED : UI_COLOR_PALEGREEN);
         const CEGUI::String& val = (isSafetyOff ? GetLangLabel(LANGSTR::OFF) : GetLangLabel(LANGSTR::ON));
         SafetyValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, color);
         SafetyValue->setProperty(UI_PROPNAME_HOVERTEXTCOLOUR, color);
         SafetyValue->setText(val);
      }
   };

   void ControllerUI::StatusBar::OnRoomInformationPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      if (CLRString::Equals(e->PropertyName, RoomInfo::PROPNAME_ROOMNAME))
      {
         RoomValue->setText(
            StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->RoomInformation->RoomName));
      }
   };

   void ControllerUI::StatusBar::OnOnlinePlayersListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      PlayersValue->setText(
         ::CEGUI::PropertyHelper<unsigned int>::toString(OgreClient::Singleton->Data->OnlinePlayers->Count));
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::StatusBar::OnMoodHappyClicked(const CEGUI::EventArgs& e)
   {
      OgreClient::Singleton->SendActionMessage(ActionType::Happy);
      return true;
   };

   bool UICallbacks::StatusBar::OnMoodNeutralClicked(const CEGUI::EventArgs& e)
   {
      OgreClient::Singleton->SendActionMessage(ActionType::Neutral);
      return true;
   };

   bool UICallbacks::StatusBar::OnMoodSadClicked(const CEGUI::EventArgs& e)
   {
      OgreClient::Singleton->SendActionMessage(ActionType::Sad);
      return true;
   };

   bool UICallbacks::StatusBar::OnMoodAngryClicked(const CEGUI::EventArgs& e)
   {
      OgreClient::Singleton->SendActionMessage(ActionType::Angry);
      return true;
   };

   bool UICallbacks::StatusBar::OnSafetyClicked(const CEGUI::EventArgs& e)
   {
      OgreClient::Singleton->Data->ClientPreferences->IsSafetyOff
         = !OgreClient::Singleton->Data->ClientPreferences->IsSafetyOff;

#if VANILLA
      OgreClient::Singleton->SendUserCommandSafetyMessage(
         !OgreClient::Singleton->Data->ClientPreferences->IsSafetyOff);
#else
      OgreClient::Singleton->SendUserCommandSendPreferences();
#endif

       return true;
   };

   bool UICallbacks::StatusBar::OnPlayersClicked(const CEGUI::EventArgs& e)
   {
      // toggle visibility of the onlineplayers window
      ControllerUI::ToggleVisibility(ControllerUI::OnlinePlayers::Window);

      return true;
   };

   bool UICallbacks::StatusBar::OnResetClicked(const CEGUI::EventArgs& e)
   {
      ControllerUI::ResetLayout();

      return true;
   };

   bool UICallbacks::StatusBar::OnLockClicked(const CEGUI::EventArgs& e)
   {
      // flip lock in config
      OgreClient::Singleton->Config->UILocked = 
         !OgreClient::Singleton->Config->UILocked;

      ControllerUI::StatusBar::Reset->setDisabled(OgreClient::Singleton->Config->UILocked);

      // update ui locking
      ControllerUI::ApplyLock();

      return true;
   };

   bool UICallbacks::StatusBar::OnFPSClicked(const CEGUI::EventArgs& e)
   {
      // flip lock in config
      if (!ControllerUI::Stats::Window->isVisible())
         ControllerUI::Stats::Window->show();

      else
         ControllerUI::Stats::Window->hide();

      return true;
   };
};};
