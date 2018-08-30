#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::StatChangeWizard::Initialize()
   {
      StatChangeInfo^ info = OgreClient::Singleton->Data->StatChangeInfo;

      // setup references to children from xml nodes
      Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_STATCHANGEWIZARD_WINDOW));

      // Sub-windows
      AttributeWindow = static_cast<CEGUI::DefaultWindow*>(Window->getChild(UI_NAME_STATCHANGEWIZARD_ATTRIBUTEWINDOW));
      SchoolWindow = static_cast<CEGUI::DefaultWindow*>(Window->getChild(UI_NAME_STATCHANGEWIZARD_SCHOOLWINDOW));

      // Button at the bottom (apply changes)
      ButtonOK = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_STATCHANGEWIZARD_BUTTONOK));
      ButtonOK->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnButtonOKClicked));

      // Stats
      Might = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_MIGHT));
      Intellect = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_INTELLECT));
      Stamina = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_STAMINA));
      Agility = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_AGILITY));
      Mysticism = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_MYSTICISM));
      Aim = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_AIM));

      // Number of stat points we can allocate.
      AttributesAvailable = static_cast<CEGUI::ProgressBar*>(AttributeWindow->getChild(UI_NAME_STATCHANGEWIZARD_ATTRIBUTESAVAILABLE));

      // Schools
      ShalilleLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_SHALILLE));
      QorLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_QOR));
      KraananLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_KRAANAN));
      FarenLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_FAREN));
      RiijaLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_RIIJA));
      JalaLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_JALA));
      WCLevel = static_cast<CEGUI::ProgressBar*>(SchoolWindow->getChild(UI_NAME_STATCHANGEWIZARD_WC));

      // Attach listener for stats/schools changes
      OgreClient::Singleton->Data->StatChangeInfo->PropertyChanged += 
         gcnew PropertyChangedEventHandler(OnStatChangeInfoPropertyChanged);

      // subscribe attributes
      Might->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Might->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Might->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseWheel));
      Might->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      Intellect->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Intellect->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Intellect->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseWheel));
      Intellect->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      Stamina->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Stamina->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Stamina->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseWheel));
      Stamina->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      Agility->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Agility->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Agility->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseWheel));
      Agility->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      Mysticism->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Mysticism->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Mysticism->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseWheel));
      Mysticism->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      Aim->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Aim->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick));
      Aim->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnAttributeMouseWheel));
      Aim->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      AttributesAvailable->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));

      // subscribe school levels
      ShalilleLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      ShalilleLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      ShalilleLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      ShalilleLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      QorLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      QorLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      QorLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      QorLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      KraananLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      KraananLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      KraananLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      KraananLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      FarenLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      FarenLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      FarenLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      FarenLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      RiijaLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      RiijaLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      RiijaLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      RiijaLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      JalaLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      JalaLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      JalaLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      JalaLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));
      WCLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseMove, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      WCLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick));
      WCLevel->subscribeEvent(CEGUI::ProgressBar::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnSchoolMouseWheel));
      WCLevel->subscribeEvent(CEGUI::ProgressBar::EventProgressChanged, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnStatChangeProgressChange));

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::StatChangeWizard::OnWindowKeyUp));

      // Set initial attributes
      SetAttributeProgressbar(Might, info->Might);
      SetAttributeProgressbar(Intellect, info->Intellect);
      SetAttributeProgressbar(Stamina, info->Stamina);
      SetAttributeProgressbar(Agility, info->Agility);
      SetAttributeProgressbar(Mysticism, info->Mysticism);
      SetAttributeProgressbar(Aim, info->Aim);

      AttributesAvailable->setProgress(
         (float)info->AttributesAvailable
         / (float)StatChangeInfo::ATTRIBUTE_MAXSUM);
      AttributesAvailable->setText(
         CEGUI::PropertyHelper<unsigned int>::toString(info->AttributesAvailable)
         + " / " + CEGUI::PropertyHelper<unsigned int>::toString(StatChangeInfo::ATTRIBUTE_MAXSUM));

      // initial schools
      SetSchoolProgressbar(StatChangeWizard::ShalilleLevel, info->OrigLevelSha, info->LevelSha);
      SetSchoolProgressbar(StatChangeWizard::QorLevel, info->OrigLevelQor, info->LevelQor);
      SetSchoolProgressbar(StatChangeWizard::KraananLevel, info->OrigLevelKraanan, info->LevelKraanan);
      SetSchoolProgressbar(StatChangeWizard::FarenLevel, info->LevelFaren, info->OrigLevelFaren);
      SetSchoolProgressbar(StatChangeWizard::RiijaLevel, info->OrigLevelRiija, info->LevelRiija);
      SetSchoolProgressbar(StatChangeWizard::JalaLevel, info->OrigLevelJala, info->LevelJala);
      SetSchoolProgressbar(StatChangeWizard::WCLevel, info->OrigLevelWC, info->LevelWC);
   };

   void ControllerUI::StatChangeWizard::Destroy()
   {
      OgreClient::Singleton->Data->StatChangeInfo->PropertyChanged -= 
         gcnew PropertyChangedEventHandler(OnStatChangeInfoPropertyChanged);
   };

   void ControllerUI::StatChangeWizard::ApplyLanguage()
   {
   };

   void ControllerUI::StatChangeWizard::OnStatChangeInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      StatChangeInfo^ statsInfo = OgreClient::Singleton->Data->StatChangeInfo;

      /// ATTRIBUTES

      // might
      if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_MIGHT))
      {
         SetAttributeProgressbar(Might, statsInfo->Might);
      }

      // intellect
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_INTELLECT))
      {
         SetAttributeProgressbar(Intellect, statsInfo->Intellect);
      }

      // stamina
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_STAMINA))
      {
         SetAttributeProgressbar(Stamina, statsInfo->Stamina);
      }

      // agility
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_AGILITY))
      {
         SetAttributeProgressbar(Agility, statsInfo->Agility);
      }

      // mysticism
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_MYSTICISM))
      {
         SetAttributeProgressbar(Mysticism, statsInfo->Mysticism);
      }

      // aim
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_AIM))
      {
         SetAttributeProgressbar(Aim, statsInfo->Aim);
      }

      // attribute points available
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ATTRIBUTESAVAILABLE))
      {
         AttributesAvailable->setProgress((float)statsInfo->AttributesAvailable / (float)StatChangeInfo::ATTRIBUTE_MAXSUM);
         
         AttributesAvailable->setText(
            CEGUI::PropertyHelper<unsigned int>::toString(statsInfo->AttributesAvailable) + " / " + 
            CEGUI::PropertyHelper<unsigned int>::toString(StatChangeInfo::ATTRIBUTE_MAXSUM));
      }

      // Shal level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELSHA)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELSHA))
      {
         SetSchoolProgressbar(StatChangeWizard::ShalilleLevel, statsInfo->OrigLevelSha, statsInfo->LevelSha);
      }

      // Qor level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELQOR)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELQOR))
      {
         SetSchoolProgressbar(StatChangeWizard::QorLevel, statsInfo->OrigLevelQor, statsInfo->LevelQor);
      }

      // Kraanan level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELKRAANAN)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELKRAANAN))
      {
         SetSchoolProgressbar(StatChangeWizard::KraananLevel, statsInfo->OrigLevelKraanan, statsInfo->LevelKraanan);
      }

      // Faren level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELFAREN)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELFAREN))
      {
         SetSchoolProgressbar(StatChangeWizard::FarenLevel, statsInfo->OrigLevelFaren, statsInfo->LevelFaren);
      }

      // Riija level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELRIIJA)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELRIIJA))
      {
         SetSchoolProgressbar(StatChangeWizard::RiijaLevel, statsInfo->OrigLevelRiija, statsInfo->LevelRiija);
      }

      // Jala level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELJALA)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELJALA))
      {
         SetSchoolProgressbar(StatChangeWizard::JalaLevel, statsInfo->OrigLevelJala, statsInfo->LevelJala);
      }

      // WC level
      else if (CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_LEVELWC)
         || CLRString::Equals(e->PropertyName, StatChangeInfo::PROPNAME_ORIGLEVELWC))
      {
         SetSchoolProgressbar(StatChangeWizard::WCLevel, statsInfo->OrigLevelWC, statsInfo->LevelWC);
      }

      // isvisible
      else if (CLRString::Equals(e->PropertyName, ObjectInfo::PROPNAME_ISVISIBLE))
      {
         // set window visibility
         Window->setVisible(OgreClient::Singleton->Data->StatChangeInfo->IsVisible);
         Window->moveToFront();
      }
   };

   void ControllerUI::StatChangeWizard::SetAttributeProgressbar(CEGUI::ProgressBar* AttrBar, unsigned char Attr)
   {
      AttrBar->setProgress((float)Attr / (float)StatChangeInfo::ATTRIBUTE_MAXVALUE);

      AttrBar->setText(
         CEGUI::PropertyHelper<unsigned int>::toString(Attr) + " / " +
         CEGUI::PropertyHelper<unsigned int>::toString(StatChangeInfo::ATTRIBUTE_MAXVALUE));
   };

   void ControllerUI::StatChangeWizard::SetSchoolProgressbar(CEGUI::ProgressBar* SchoolBar, unsigned char OrigLevel, unsigned char Level)
   {
      if (OrigLevel)
      {
         SchoolBar->setProgress((float)Level / (float)OrigLevel);
         SchoolBar->setText(
            CEGUI::PropertyHelper<unsigned int>::toString(Level) + " / " +
            CEGUI::PropertyHelper<unsigned int>::toString(OrigLevel));
      }
      else
      {
         SchoolBar->setProgress(0.0f);
         SchoolBar->setText("0");
      }
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::StatChangeWizard::OnAttributeMouseMoveClick(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
      
      if ((args.sysKeys & CEGUI::LeftMouse) || (args.button == CEGUI::MouseButton::LeftButton))
      {
         // calc
         float mouse_x = args.position.d_x;
         float control_x = args.window->getPixelPosition().d_x;
         float control_width = (float)MathUtil::Max(args.window->getPixelSize().d_width, 0.1f);
         float p = (mouse_x - control_x) / control_width;
         unsigned int val = ::System::Convert::ToUInt32(p * StatChangeInfo::ATTRIBUTE_MAXVALUE);

         if (args.window == ControllerUI::StatChangeWizard::Might)
            OgreClient::Singleton->Data->StatChangeInfo->Might = val;

         else if (args.window == ControllerUI::StatChangeWizard::Intellect)
            OgreClient::Singleton->Data->StatChangeInfo->Intellect = val;

         else if (args.window == ControllerUI::StatChangeWizard::Stamina)
            OgreClient::Singleton->Data->StatChangeInfo->Stamina = val;

         else if (args.window == ControllerUI::StatChangeWizard::Agility)
            OgreClient::Singleton->Data->StatChangeInfo->Agility = val;

         else if (args.window == ControllerUI::StatChangeWizard::Mysticism)
            OgreClient::Singleton->Data->StatChangeInfo->Mysticism = val;

         else if (args.window == ControllerUI::StatChangeWizard::Aim)
            OgreClient::Singleton->Data->StatChangeInfo->Aim = val;
      }

      return true;
   };
   
   bool UICallbacks::StatChangeWizard::OnAttributeMouseWheel(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
      
      if (args.window == ControllerUI::StatChangeWizard::Might)
         OgreClient::Singleton->Data->StatChangeInfo->Might += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::Intellect)
         OgreClient::Singleton->Data->StatChangeInfo->Intellect += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::Stamina)
         OgreClient::Singleton->Data->StatChangeInfo->Stamina += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::Agility)
         OgreClient::Singleton->Data->StatChangeInfo->Agility += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::Mysticism)
         OgreClient::Singleton->Data->StatChangeInfo->Mysticism += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::Aim)
         OgreClient::Singleton->Data->StatChangeInfo->Aim += static_cast<unsigned char>(args.wheelChange);

      return true;
   };

   bool UICallbacks::StatChangeWizard::OnSchoolMouseMoveClick(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

      if ((args.sysKeys & CEGUI::LeftMouse) || (args.button == CEGUI::MouseButton::LeftButton))
      {
         // calc
         float mouse_x = args.position.d_x;
         float control_x = args.window->getPixelPosition().d_x;
         float control_width = (float)MathUtil::Max(args.window->getPixelSize().d_width, 0.1f);
         float p = (mouse_x - control_x) / control_width;
         unsigned int val = ::System::Convert::ToUInt32(p * StatChangeInfo::SCHOOL_MAXVALUE);

         if (args.window == ControllerUI::StatChangeWizard::ShalilleLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelSha = val;

         else if (args.window == ControllerUI::StatChangeWizard::QorLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelQor = val;

         else if (args.window == ControllerUI::StatChangeWizard::KraananLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelKraanan = val;

         else if (args.window == ControllerUI::StatChangeWizard::FarenLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelFaren = val;

         else if (args.window == ControllerUI::StatChangeWizard::RiijaLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelRiija = val;

         else if (args.window == ControllerUI::StatChangeWizard::JalaLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelJala = val;

         else if (args.window == ControllerUI::StatChangeWizard::WCLevel)
            OgreClient::Singleton->Data->StatChangeInfo->LevelWC = val;
      }

      return true;
   };

   bool UICallbacks::StatChangeWizard::OnSchoolMouseWheel(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

      if (args.window == ControllerUI::StatChangeWizard::ShalilleLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelSha += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::QorLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelQor += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::KraananLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelKraanan += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::FarenLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelFaren += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::RiijaLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelRiija += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::JalaLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelJala += static_cast<unsigned char>(args.wheelChange);

      else if (args.window == ControllerUI::StatChangeWizard::WCLevel)
         OgreClient::Singleton->Data->StatChangeInfo->LevelWC += static_cast<unsigned char>(args.wheelChange);

      return true;
   };

   bool UICallbacks::StatChangeWizard::OnStatChangeProgressChange(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
      ControllerUI::SetVUMeterColorFromProgress((CEGUI::ProgressBar*)args.window);

      return true;
   };

   bool UICallbacks::StatChangeWizard::OnButtonOKClicked(const CEGUI::EventArgs& e)
   {
      StatChangeInfo^ statsInfo = OgreClient::Singleton->Data->StatChangeInfo;

      if (statsInfo->Intellect < statsInfo->IntellectNeeded)
      {
         // show (should be OK button)
         ControllerUI::ConfirmPopup::ShowOK("Invalid intellect for number of schools!", 0);
      }
      else
      {
         // attach yes listener to confirm popup
         ControllerUI::ConfirmPopup::Confirmed += gcnew System::EventHandler(ControllerUI::StatChangeWizard::OnStatChangeConfirmed);

         // show
         ControllerUI::ConfirmPopup::ShowChoice("Are you sure you want to change your stats?", 0);
      }

      return true;
   };

   void ControllerUI::StatChangeWizard::OnStatChangeConfirmed(Object ^sender, ::System::EventArgs ^e)
   {
#ifndef VANILLA
      // Send request to server.
      OgreClient::Singleton->SendChangedStatsMessage();
#endif
      return;
   };

   bool UICallbacks::StatChangeWizard::OnWindowClosed(const CEGUI::EventArgs& e)
   {
      // set not visible in datalayer (view will react)
      OgreClient::Singleton->Data->StatChangeInfo->IsVisible = false;

      // mark GUIroot active
      ControllerUI::ActivateRoot();

      return true;
   }

   bool UICallbacks::StatChangeWizard::OnWindowKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

      // close window on ESC
      if (args.scancode == CEGUI::Key::Escape)
      {
         // clear (view will react)
         OgreClient::Singleton->Data->StatChangeInfo->IsVisible = false;

         // mark GUIroot active
         ControllerUI::ActivateRoot();
      }

      return true;
   }
};};
