#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::SplashNotifier::Initialize()
   {
      // setup references to children from xml nodes
      Window = static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_SPLASHNOTIFIER_WINDOW));

      Window->setAlwaysOnTop(true);
      Window->show();

      // must disable so not recognized as TopControl on hittest
      Window->disable();

      // create list with notifications
      notifications = gcnew ::System::Collections::Generic::List<CLRString^>();

      // attach listener to Data
      OgreClient::Singleton->Data->PropertyChanged += 
         gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
        
      // attach listener to paralyze effect
      OgreClient::Singleton->Data->Effects->Paralyze->PropertyChanged += 
         gcnew PropertyChangedEventHandler(OnParalyzePropertyChanged);

      // attach listener to avatar buffs
      OgreClient::Singleton->Data->AvatarBuffs->ListChanged +=
         gcnew ListChangedEventHandler(OnAvatarBuffsListChanged);
   };

   void ControllerUI::SplashNotifier::Destroy()
   {
      // detach listener from Data
      OgreClient::Singleton->Data->PropertyChanged -= 
         gcnew PropertyChangedEventHandler(OnDataPropertyChanged);

      // detach listener from paralyze effect
      OgreClient::Singleton->Data->Effects->Paralyze->PropertyChanged -= 
         gcnew PropertyChangedEventHandler(OnParalyzePropertyChanged);

      // detach listener to avatar buffs
      OgreClient::Singleton->Data->AvatarBuffs->ListChanged -=
         gcnew ListChangedEventHandler(OnAvatarBuffsListChanged);
   };

   void ControllerUI::SplashNotifier::ApplyLanguage()
   {
   };

   void ControllerUI::SplashNotifier::UpdateNotification()
   {
      if (notifications->Count > 0)
      {
         // set last added notification
         Window->setText(StringConvert::CLRToCEGUI(notifications[notifications->Count - 1]));
      }
      else
      {
         Window->setText(STRINGEMPTY);
      }
   };

   void ControllerUI::SplashNotifier::ShowNotification(CLRString^ Text)
   {
      if (!notifications->Contains(Text))
         notifications->Add(Text);

      UpdateNotification();
   };

   void ControllerUI::SplashNotifier::HideNotification(CLRString^ Text)
   {
      if (notifications->Contains(Text))
         notifications->Remove(Text);

      UpdateNotification();
   };

   void ControllerUI::SplashNotifier::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      // resting
      if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_ISRESTING))
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
      else if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_ISWAITING))
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
      if (CLRString::Equals(e->PropertyName, EffectParalyze::PROPNAME_ISACTIVE))
      {
         if (OgreClient::Singleton->Data->Effects->Paralyze->IsActive)
         {
            if (!notifications->Contains(UI_NOTIFICATION_PARALYZED)
               && !notifications->Contains(UI_NOTIFICATION_PHASED))
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

   void ControllerUI::SplashNotifier::OnAvatarBuffsListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      ObjectBase^ o;

      // buff added/removed, only want to know about phase
      switch (e->ListChangedType)
      {
      case ListChangedType::ItemAdded:
         o = OgreClient::Singleton->Data->AvatarBuffs[e->NewIndex];
         if (o->Name == "phase" || o->Name == "Ausstieg")
         {
            if (!notifications->Contains(UI_NOTIFICATION_PHASED))
               notifications->Add(UI_NOTIFICATION_PHASED);
         }
         UpdateNotification();
         break;

      case ListChangedType::ItemDeleted:
         o = OgreClient::Singleton->Data->AvatarBuffs->LastDeletedItem;
         if (o->Name == "phase" || o->Name == "Ausstieg")
         {
            if (notifications->Contains(UI_NOTIFICATION_PHASED))
               notifications->Remove(UI_NOTIFICATION_PHASED);

            // Might still have paralyze active, display appropriate notification.
            if (OgreClient::Singleton->Data->Effects->Paralyze->IsActive)
            {
               if (!notifications->Contains(UI_NOTIFICATION_PARALYZED))
                  notifications->Add(UI_NOTIFICATION_PARALYZED);
            }
         }
         UpdateNotification();
         break;
      }
   };
};};
