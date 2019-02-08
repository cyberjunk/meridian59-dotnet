#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::Mail::Initialize()
   {
      // setup references to children from xml nodes
      Window      = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_MAIL_WINDOW));
      List        = static_cast<CEGUI::MultiColumnList*>(Window->getChild(UI_NAME_MAIL_LIST));
      Create      = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAIL_CREATE));
      Respond     = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAIL_RESPOND));
      RespondAll  = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAIL_RESPONDALL));
      Refresh     = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAIL_REFRESH));
      Delete      = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAIL_DELETE));
      Text        = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_MAIL_TEXT));

      List->setShowVertScrollbar(true);
      List->setSelectionMode(CEGUI::MultiColumnList::SelectionMode::RowSingle);
      List->setUserSortControlEnabled(false);

      // create columns
      List->addColumn("Num", 0, CEGUI::UDim(0.0f, 50));
      List->addColumn("Sender", 1, CEGUI::UDim(0.5f, -86));
      List->addColumn("Subject", 2, CEGUI::UDim(0.5f, -86));
      List->addColumn("Date", 3, CEGUI::UDim(0.0f, 105));

      // attach listener to mails list
      OgreClient::Singleton->ResourceManager->Mails->ListChanged += 
         gcnew ListChangedEventHandler(OnMailsListChanged);

      // mails have been loaded before UI init, so create the ones already loaded from disk
      for(int i = 0; i < OgreClient::Singleton->ResourceManager->Mails->Count; i++)
         MailAdd(i);

      // subscribe buttons
      Create->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Mail::OnCreateClicked));
      Respond->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Mail::OnRespondClicked));
      RespondAll->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Mail::OnRespondAllClicked));
      Refresh->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Mail::OnRefreshClicked));
      Delete->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Mail::OnDeleteClicked));

      // subscribe keydown on text
      Text->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

      // subscribe selection change
      List->subscribeEvent(CEGUI::MultiColumnList::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::Mail::OnSelectionChanged));
      List->subscribeEvent(CEGUI::MultiColumnList::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Mail::OnKeyUp));

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::Mail::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
   };

   void ControllerUI::Mail::Destroy()
   {
      // detach listener from mails list
      OgreClient::Singleton->ResourceManager->Mails->ListChanged -= 
         gcnew ListChangedEventHandler(OnMailsListChanged);
   };

   void ControllerUI::Mail::ApplyLanguage()
   {
      Window->setText(GetLangWindowTitle(LANGSTR_WINDOW_TITLE::MAIL));
   };

   void ControllerUI::Mail::OnMailsListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      switch(e->ListChangedType)
      {
         case ::System::ComponentModel::ListChangedType::ItemAdded:
            MailAdd(e->NewIndex);
            break;

         case ::System::ComponentModel::ListChangedType::ItemDeleted:
            MailRemove(e->NewIndex);
            break;

         case ::System::ComponentModel::ListChangedType::ItemChanged:
            MailChange(e->NewIndex);
            break;
      }
   };

   void ControllerUI::Mail::MailAdd(int Index)
   {
      Data::Models::Mail^ obj = OgreClient::Singleton->ResourceManager->Mails[Index];

      CEGUI::ListboxTextItem* itmNum = new CEGUI::ListboxTextItem(
         StringConvert::CLRToCEGUI(obj->Num.ToString()));

      CEGUI::ListboxTextItem* itmSender = new CEGUI::ListboxTextItem(
         StringConvert::CLRToCEGUI(obj->Sender));

      CEGUI::ListboxTextItem* itmTitle = new CEGUI::ListboxTextItem(
         StringConvert::CLRToCEGUI(obj->Title));

      ::System::DateTime time = MeridianDate::ToDateTime(obj->Timestamp);

      CEGUI::ListboxTextItem* itmDate = new CEGUI::ListboxTextItem(
         StringConvert::CLRToCEGUI(time.ToShortDateString() + " " + time.ToShortTimeString()));

      itmNum->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
      itmSender->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
      itmTitle->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
      itmDate->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");

      itmNum->setSelectionColours(CEGUI::Colour(0xFF444444));
      itmSender->setSelectionColours(CEGUI::Colour(0xFF444444));
      itmTitle->setSelectionColours(CEGUI::Colour(0xFF444444));
      itmDate->setSelectionColours(CEGUI::Colour(0xFF444444));

      // insert widget in ui-list
      if ((int)List->getRowCount() > Index)
         List->insertRow(Index);

      // or add
      else
         List->addRow();

      List->setItem(itmNum, 0, Index); 
      List->setItem(itmSender, 1, Index);
      List->setItem(itmTitle, 2, Index); 
      List->setItem(itmDate, 3, Index);
   };

   void ControllerUI::Mail::MailRemove(int Index)
   {
      // check
      if ((int)List->getRowCount() > Index)
         List->removeRow(Index);
   };

   void ControllerUI::Mail::MailChange(int Index)
   {
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::Mail::OnSelectionChanged(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
      CEGUI::MultiColumnList* list = ControllerUI::Mail::List;
      CEGUI::ListboxItem* itm = list->getFirstSelectedItem();

      if (itm != nullptr)
      {
         // get index
         unsigned int index = list->getItemRowIndex(itm);

         // get text from datamodel
         CLRString^ text = 
            OgreClient::Singleton->ResourceManager->Mails[index]->Message->FullString;

         // set on view
         ControllerUI::Mail::Text->setText(StringConvert::CLRToCEGUI(text));
      }

      return true;
   };

   bool UICallbacks::Mail::OnCreateClicked(const CEGUI::EventArgs& e)
   {
      // show empty compose window
      ControllerUI::MailCompose::Window->setVisible(true);
      ControllerUI::MailCompose::Window->moveToFront();

      return true;
   }

   bool UICallbacks::Mail::OnRespondClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
      CEGUI::MultiColumnList* list = ControllerUI::Mail::List;
      MailList^ mails = OgreClient::Singleton->ResourceManager->Mails;

      // need selection to be able to respond
      if (list->getSelectedCount() > 0)
      {
         // get selection
         CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
         unsigned int index = list->getItemRowIndex(itm);

         if (mails->Count > (int)index)
         {
            Data::Models::Mail^ mail = mails[index];

            // set title
            bool addReply = !(mail->Title->StartsWith("Re:") || mail->Title->StartsWith("Aw:"));
            ControllerUI::MailCompose::HeadLine->setText(
               StringConvert::CLRToCEGUI((addReply ? "Re: " : "") + mail->Title));

            // set sender as recipient
            ControllerUI::MailCompose::Recipients->setText(
               StringConvert::CLRToCEGUI(mail->Sender));

            // make sure no error is shown
            ControllerUI::MailCompose::Error->setVisible(false);

            // show prefilled compose window
            ControllerUI::MailCompose::Window->setVisible(true);
            ControllerUI::MailCompose::Window->moveToFront();
         }
      }

      return true;
   }

   bool UICallbacks::Mail::OnRespondAllClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      CEGUI::MultiColumnList* list = ControllerUI::Mail::List;
      MailList^ mails = OgreClient::Singleton->ResourceManager->Mails;

      // need selection to be able to respond
      if (list->getSelectedCount() > 0)
      {
         // get selection
         CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
         unsigned int index = list->getItemRowIndex(itm);

         if (mails->Count > (int)index)
         {
            Data::Models::Mail^ mail = mails[index];

            // set title
            ControllerUI::MailCompose::HeadLine->setText(
               StringConvert::CLRToCEGUI("Re: " + mail->Title));

            // build recipients list, start with sender
            CLRString^ recipients = mail->Sender;

            // add all recipients
            for(int i = 0; i < mail->Recipients->Count; i++)
               recipients += "," + mail->Recipients[i];

            // set recipients
            ControllerUI::MailCompose::Recipients->setText(
               StringConvert::CLRToCEGUI(recipients));

            // set empty text
            ControllerUI::MailCompose::Text->setText(STRINGEMPTY);

            // make sure no error is shown
            ControllerUI::MailCompose::Error->setVisible(false);

            // show prefilled compose window
            ControllerUI::MailCompose::Window->setVisible(true);
            ControllerUI::MailCompose::Window->moveToFront();
         }
      }
      return true;
   }

   bool UICallbacks::Mail::OnDeleteClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
      CEGUI::MultiColumnList* list = ControllerUI::Mail::List;
      MailList^ mails = OgreClient::Singleton->ResourceManager->Mails;

      // need selection to be able to respond
      if (list->getSelectedCount() > 0)
      {
         // get selection
         CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
         unsigned int index = list->getItemRowIndex(itm);

         if (mails->Count > (int)index)
         {
            mails->RemoveAt(index);
         }
      }

      return true;
   }

   bool UICallbacks::Mail::OnRefreshClicked(const CEGUI::EventArgs& e)
   {
      // re-request mails
      OgreClient::Singleton->SendReqGetMail();

      return true;
   }

   bool UICallbacks::Mail::OnKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
      CEGUI::MultiColumnList* list = ControllerUI::Mail::List;		
      CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
      int count = (int)list->getRowCount();

      if (itm != nullptr)
      {
         int index = (int)list->getItemRowIndex(itm);

         if (args.scancode == CEGUI::Key::Scan::ArrowUp && index > 0)
         {
            // unselect
            list->setItemSelectState(CEGUI::MCLGridRef(index, 0), false);
            list->setItemSelectState(CEGUI::MCLGridRef(index, 1), false);
            list->setItemSelectState(CEGUI::MCLGridRef(index, 2), false);
            list->setItemSelectState(CEGUI::MCLGridRef(index, 3), false);

            // select
            list->setItemSelectState(CEGUI::MCLGridRef(index-1, 0), true);
            list->setItemSelectState(CEGUI::MCLGridRef(index-1, 1), true);
            list->setItemSelectState(CEGUI::MCLGridRef(index-1, 2), true);
            list->setItemSelectState(CEGUI::MCLGridRef(index-1, 3), true);

            // make sure new selected row is visible (scroll if necessary)
            list->ensureRowIsVisible(index - 1);
         }
         else if (args.scancode == CEGUI::Key::Scan::ArrowDown && index < count - 1)
         {
            // unselect
            list->setItemSelectState(CEGUI::MCLGridRef(index, 0), false);
            list->setItemSelectState(CEGUI::MCLGridRef(index, 1), false);
            list->setItemSelectState(CEGUI::MCLGridRef(index, 2), false);
            list->setItemSelectState(CEGUI::MCLGridRef(index, 3), false);

            // select
            list->setItemSelectState(CEGUI::MCLGridRef(index+1, 0), true);
            list->setItemSelectState(CEGUI::MCLGridRef(index+1, 1), true);
            list->setItemSelectState(CEGUI::MCLGridRef(index+1, 2), true);
            list->setItemSelectState(CEGUI::MCLGridRef(index+1, 3), true);

            // make sure new selected row is visible (scroll if necessary)
            list->ensureRowIsVisible(index + 1);
         }
      }

      return true;
   };

   bool UICallbacks::Mail::OnWindowClosed(const CEGUI::EventArgs& e)
   {
      ControllerUI::Mail::Window->hide();

      // mark GUIroot active
      ControllerUI::ActivateRoot();

      return true;
   }
};};
