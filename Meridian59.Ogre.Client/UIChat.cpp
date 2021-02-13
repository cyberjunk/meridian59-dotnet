#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::Chat::Initialize()
   {
      // setup references to children from xml nodes
      Window         = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_CHAT_WINDOW));
      Text           = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_CHAT_TEXT));
      TextPlain      = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_CHAT_TEXTPLAIN));
      Input          = static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_CHAT_INPUT));
      Scrollbar      = static_cast<CEGUI::Scrollbar*>(Text->getChildAtIdx(1));
      ScrollbarPlain = TextPlain->getVertScrollbar();

      // set window layout from config
      Window->setPosition(OgreClient::Singleton->Config->UILayoutChat->getPosition());
      Window->setSize(OgreClient::Singleton->Config->UILayoutChat->getSize());

      // set autoscroll on text at start
      Scrollbar->setEndLockEnabled(true);
      ScrollbarPlain->setEndLockEnabled(true);

      // set maximum textlength for chatinput
      Input->setMaxTextLength((size_t)BlakservStringLengths::MAX_CHAT_LEN);

      TextPlain->setVerticalAlignment(::CEGUI::VerticalAlignment::VA_TOP);
      TextPlain->setHorizontalAlignment(::CEGUI::HorizontalAlignment::HA_LEFT);
      TextPlain->setEnsureCaretVisible(false);

      // attach listener to chatmessage list
      OgreClient::Singleton->Data->ChatMessages->ListChanged += 
         gcnew ListChangedEventHandler(&ControllerUI::Chat::OnChatMessagesListChanged);
            
      // subscribe key-up event
      Input->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::Chat::OnKeyDown));
      Input->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Chat::OnKeyUp));

      // subscribe scroll
      Scrollbar->subscribeEvent(CEGUI::Scrollbar::EventScrollPositionChanged, CEGUI::Event::Subscriber(UICallbacks::Chat::OnScrollPositionChanged));
      ScrollbarPlain->subscribeEvent(CEGUI::Scrollbar::EventScrollPositionChanged, CEGUI::Event::Subscriber(UICallbacks::Chat::OnScrollPositionChanged));

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));

      // subscribe click
      Text->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Chat::OnTextClicked));
      TextPlain->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Chat::OnTextClicked));

      // subscripe copy paste handler on plain chattext
      TextPlain->subscribeEvent(CEGUI::Window::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

      // create queue for chatmessage to write at next chatupdate tick
      Queue = gcnew ::System::Collections::Generic::Queue<::Meridian59::Data::Models::ServerString^>();
   };

   void ControllerUI::Chat::Destroy()
   {	
      OgreClient::Singleton->Data->ChatMessages->ListChanged -= 
         gcnew ListChangedEventHandler(&ControllerUI::Chat::OnChatMessagesListChanged); 

      // reset delete counter
      DeleteCounter = 0;

      // cleanup queue
      Queue->Clear();	
      delete Queue;
      Queue = nullptr;
   };

   void ControllerUI::Chat::ApplyLanguage()
   {
      ChatForceRenew = true;
   };

   void ControllerUI::Chat::Tick(double Tick, double Span)
   {
      // nothing to do
      if (!OgreClient::Singleton->GameTick->CanChatUpdate() && !ChatForceRenew)
         return;

      // some special handling for a forced renew first
      if (ChatForceRenew)
      {
         // remove old/pending
         Queue->Clear();

         // add all chat history from datamodels again
         for each(ServerString^ msg in OgreClient::Singleton->Data->ChatMessages)
            Queue->Enqueue(msg);

         // flip active chat control based on plain mode flag
         if (PlainMode)
         {
            // show one, hide the other
            Text->setVisible(false);
            TextPlain->setVisible(true);

            // transfer current scroll state to other
            ScrollbarPlain->setScrollPosition(Scrollbar->getScrollPosition());
            ScrollbarPlain->setPageSize(Scrollbar->getPageSize());
            ScrollbarPlain->setDocumentSize(Scrollbar->getDocumentSize());
         }
         else
         {
            // show one, hide the other
            Text->setVisible(true);
            TextPlain->setVisible(false);

            // transfer current scroll state to other
            Scrollbar->setScrollPosition(ScrollbarPlain->getScrollPosition());
            Scrollbar->setPageSize(ScrollbarPlain->getPageSize());
            Scrollbar->setDocumentSize(ScrollbarPlain->getDocumentSize());
         }
      }

      // pick current chat control
      ::CEGUI::Window* wnd = PlainMode ? TextPlain : Text;

      // append new ones first
      if (Queue->Count > 0)
      {
         // build string first, get first
         ServerString^ msg = Queue->Dequeue();
         CEGUI::String& str = Util::GetChatString(msg, PlainMode);

         // append next ones
         while (Queue->Count > 0)
         {
            msg = Queue->Dequeue();
            str = str.append(Util::GetChatString(msg, PlainMode));
         }

         // recreate cegui text completely
         if (ChatForceRenew)
            wnd->setText(str);

         // append to existing cegui text
         else
            wnd->appendText(str);
      }

      // reset delete counter for full recreate
      if (ChatForceRenew)
         DeleteCounter = 0;

      // otherwise remove lines according to delete counter
      // assumption: one chatmessage = chars between \n
      else
      {
         size_t idx = CEGUI::String::npos;

         // find index for that many line breaks
         while (DeleteCounter > 0)
         {
            idx = wnd->getText().find("\n", idx + 1);
            DeleteCounter--;
         }

         // remove lines
         if (idx != CEGUI::String::npos)
            wnd->eraseText(0, idx + 1);
      }

      // save update tick
      OgreClient::Singleton->GameTick->DidChatUpdate();

      // unset force chat update flag
      ChatForceRenew = false;
   };

   void ControllerUI::Chat::OnChatMessagesListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      switch(e->ListChangedType)
      {
         case ::System::ComponentModel::ListChangedType::Reset:
            ChatForceRenew = true;
            break;

         case ::System::ComponentModel::ListChangedType::ItemAdded:
            Queue->Enqueue(OgreClient::Singleton->Data->ChatMessages[e->NewIndex]);
            break;

         case ::System::ComponentModel::ListChangedType::ItemDeleted:
            DeleteCounter++;
            break;
      }
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::Chat::OnKeyDown(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& e2 = static_cast<const CEGUI::KeyEventArgs&>(e);
      CEGUI::KeyEventArgs& args     = const_cast<CEGUI::KeyEventArgs&>(e2);
      CEGUI::Editbox* chatInput     = ControllerUI::Chat::Input;
      //const CEGUI::String& text     = chatInput->getText();
      //CLRString^ textCLR     = StringConvert::CEGUIToCLR(text);

      ::System::Collections::Generic::List<CLRString^>^ chatCommandHistory =
         OgreClient::Singleton->Data->ChatCommandHistory;

      // base handler for copy&paste clipboard
      bool handled = UICallbacks::OnCopyPasteKeyDown(e);

      // used in some cases
      CLRString^ str;

      switch(args.scancode)
      {
         case CEGUI::Key::Scan::Return:
         case CEGUI::Key::Scan::NumpadEnter:
            handled = true; // see keyup
            break;

         case CEGUI::Key::Scan::Escape:
            handled = true; // see keyup
            break;

         case CEGUI::Key::ArrowUp:
            str = OgreClient::Singleton->Data->ChatCommandHistoryGetNext();
            if (str)
            {
               ::CEGUI::String& ceguistr = StringConvert::CLRToCEGUI(str);
               chatInput->setText(ceguistr);
               chatInput->setCaretIndex(ceguistr.length());
            }
            handled = true;
            break;

         case CEGUI::Key::ArrowDown:
            str = OgreClient::Singleton->Data->ChatCommandHistoryGetPrevious();
            if (str)
            {
               ::CEGUI::String& ceguistr = StringConvert::CLRToCEGUI(str);
               chatInput->setText(ceguistr);
               chatInput->setCaretIndex(ceguistr.length());
            }
            else
            {
               chatInput->setText(STRINGEMPTY);
            }
            handled = true;
            break;
      }

      if (handled)
         args.handled++;

      return handled;
   };

   bool UICallbacks::Chat::OnKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& e2 = static_cast<const CEGUI::KeyEventArgs&>(e);
      CEGUI::KeyEventArgs& args = const_cast<CEGUI::KeyEventArgs&>(e2);
      CEGUI::Editbox* chatInput = ControllerUI::Chat::Input;
      const CEGUI::String& text = chatInput->getText();

      ::System::Collections::Generic::List<CLRString^>^ chatCommandHistory =
         OgreClient::Singleton->Data->ChatCommandHistory;

      // base handler for copy&paste clipboard
      bool handled = false;

      // used in some cases
      CLRString^ str;

      switch (args.scancode)
      {
      case CEGUI::Key::Scan::Return:
      case CEGUI::Key::Scan::NumpadEnter:
         str = StringConvert::CEGUIToCLR(text);
         chatInput->setText(STRINGEMPTY);
         ControllerUI::ActivateRoot();
         OgreClient::Singleton->ExecChatCommand(str);
         OgreClient::Singleton->Data->ChatCommandHistoryIndex = -1;
         handled = true;
         break;

      case CEGUI::Key::Scan::Escape:
         ControllerUI::ActivateRoot();
         OgreClient::Singleton->Data->ChatCommandHistoryIndex = -1;
         handled = true;
         break;

      case CEGUI::Key::ArrowUp:
         handled = true; // see keydown
         break;

      case CEGUI::Key::ArrowDown:
         handled = true; // see keydown
         break;
      }

      if (handled)
         args.handled++;

      return handled;
   };

   bool UICallbacks::Chat::OnTextClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& e2 = static_cast<const CEGUI::MouseEventArgs&>(e);

      // only for right mousebutton
      if (e2.button != ::CEGUI::MouseButton::RightButton)
         return true;

      // flip plain mode and force update on next Tick()
      ControllerUI::Chat::PlainMode = !ControllerUI::Chat::PlainMode;
      ControllerUI::Chat::ChatForceRenew = true;

      return true;
   };

   bool UICallbacks::Chat::OnScrollPositionChanged(const CEGUI::EventArgs& e)
   {
      // pick scrollbar for text mode
      ::CEGUI::Scrollbar* bar = ControllerUI::Chat::PlainMode ?
         ControllerUI::Chat::ScrollbarPlain :
         ControllerUI::Chat::Scrollbar;

      ::Ogre::Real pos = bar->getScrollPosition();
      ::Ogre::Real docsize = bar->getDocumentSize();
      ::Ogre::Real pagesize = bar->getPageSize();
      ::Ogre::Real vl = docsize - pagesize;

      // reenable autoscroll if user scrolled back to the bottom
      if (::Ogre::Math::Abs(vl - pos) < 5.0f)
      {
         ControllerUI::Chat::ScrollbarPlain->setEndLockEnabled(true);
         ControllerUI::Chat::Scrollbar->setEndLockEnabled(true);
      }
      else
      {
         ControllerUI::Chat::ScrollbarPlain->setEndLockEnabled(false);
         ControllerUI::Chat::Scrollbar->setEndLockEnabled(false);
      }

      return true;
   };

};};
