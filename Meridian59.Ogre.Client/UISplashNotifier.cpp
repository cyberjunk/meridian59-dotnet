#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::SplashNotifier::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_SPLASHNOTIFIER_WINDOW));
		
		// create list with notifications
		notifications = gcnew ::System::Collections::Generic::List<::System::String^>();

		// attach listener to Data
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
        
		// attach listener to paralyze effect
		OgreClient::Singleton->Data->Effects->Paralyze->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnParalyzePropertyChanged);      
	};

	void ControllerUI::SplashNotifier::Destroy()
	{	 
		// detach listener from Data
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
        
		// detach listener from paralyze effect
		OgreClient::Singleton->Data->Effects->Paralyze->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnParalyzePropertyChanged);  		
	};

	void ControllerUI::SplashNotifier::UpdateNotification()
	{
		if (notifications->Count > 0)
        {
            // set last added notification
            Window->setText(StringConvert::CLRToCEGUI(notifications[notifications->Count - 1]));
				
            // show
			Window->show();
			Window->moveToFront();
        }
        else
		{
            Window->hide();
		}
	};

	void ControllerUI::SplashNotifier::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// resting
		if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_ISRESTING))
		{
			if (OgreClient::Singleton->Data->IsResting)
            {
                if (!notifications->Contains(UI_NOTIFICATION_RESTING))
                    notifications->Add(UI_NOTIFICATION_RESTING);
            }
            else
            {
                if (notifications->Contains(UI_NOTIFICATION_RESTING))
                    notifications->Remove(UI_NOTIFICATION_RESTING);
            }

			UpdateNotification();
		}

		// server saving
		else if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_ISWAITING))
		{
			if (OgreClient::Singleton->Data->IsWaiting)
            {
                if (!notifications->Contains(UI_NOTIFICATION_SAVING))
                    notifications->Add(UI_NOTIFICATION_SAVING);
            }
            else
            {
                if (notifications->Contains(UI_NOTIFICATION_SAVING))
                    notifications->Remove(UI_NOTIFICATION_SAVING);
            }
			
			UpdateNotification();
		}
	};

	void ControllerUI::SplashNotifier::OnParalyzePropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// isactive
		if (::System::String::Equals(e->PropertyName, EffectParalyze::PROPNAME_ISACTIVE))
		{
			if (OgreClient::Singleton->Data->Effects->Paralyze->IsActive)
            {
                if (!notifications->Contains(UI_NOTIFICATION_PARALYZED))
                    notifications->Add(UI_NOTIFICATION_PARALYZED);
            }
            else
            {
                if (notifications->Contains(UI_NOTIFICATION_PARALYZED))
                    notifications->Remove(UI_NOTIFICATION_PARALYZED);
            }

			UpdateNotification();
		}
	};
};};