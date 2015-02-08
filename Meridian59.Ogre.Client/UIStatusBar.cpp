#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::StatusBar::Initialize()
	{
		// setup references to children from xml nodes
		Window				= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_STATUSBAR_WINDOW));
		FPSDescription		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_FPSDESC));
		FPSValue			= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_FPSVAL));
		RTTDescription		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_RTTDESC));
		RTTValue			= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_RTTVAL));
		PlayersDescription	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_PLAYERSDESC));
		PlayersValue		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_PLAYERSVAL));
		MoodDescription		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_MOODDESC));
		MoodHappy			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODHAPPY));
		MoodNeutral			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODNEUTRAL));
		MoodSad				= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODSAD));
		MoodAngry			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_MOODANGRY));
		SafetyDescription	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_SAFETYDESC));
		SafetyValue			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATUSBAR_SAFETYVAL));
		MTimeDescription	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_MTIMEDESC));
		MTimeValue			= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_MTIMEVAL));
		RoomDescription		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_ROOMDESC));
		RoomValue			= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATUSBAR_ROOMVAL));
		
		// attach listener to Data
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnDataPropertyChanged);
           
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
	};

	void ControllerUI::StatusBar::Destroy()
	{	 
		// detach listener from Data
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnDataPropertyChanged);
           
		// detach listener from roominformation
		OgreClient::Singleton->Data->RoomInformation->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerUI::StatusBar::OnRoomInformationPropertyChanged);
           
		// detach listener from onlineplayers
		OgreClient::Singleton->Data->OnlinePlayers->ListChanged -= 
			gcnew ListChangedEventHandler(&ControllerUI::StatusBar::OnOnlinePlayersListChanged); 		
	};

	void ControllerUI::StatusBar::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// update FPS/TPS
		if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_TPS))
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
		else if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_RTT))
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
		else if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_MERIDIANTIME))
		{
			MTimeValue->setText(
				StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->MeridianTime.ToShortTimeString()));
		}

		// update safety
		else if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_ISSAFETY))
		{
			const bool isSafety = OgreClient::Singleton->Data->IsSafety;
			const CEGUI::String color = (isSafety ? UI_COLOR_PALEGREEN : UI_COLOR_DARKRED);
			const CEGUI::String val = (isSafety ? "On" : "Off");

			SafetyValue->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, color);
			SafetyValue->setProperty(UI_PROPNAME_HOVERTEXTCOLOUR, color);
			SafetyValue->setText(val);
		}
	};
	
	void ControllerUI::StatusBar::OnRoomInformationPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (::System::String::Equals(e->PropertyName, RoomInfo::PROPNAME_ROOMNAME))
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
		OgreClient::Singleton->SendUserCommandSafetyMessage(
			!OgreClient::Singleton->Data->IsSafety);

		return true;
	};

	bool UICallbacks::StatusBar::OnPlayersClicked(const CEGUI::EventArgs& e)
	{
		// toggle visibility of the onlineplayers window
		ControllerUI::ToggleVisibility(ControllerUI::OnlinePlayers::Window);

		return true;
	};
};};