#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Chat::Initialize()
	{
		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_CHAT_WINDOW));
		Text		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_CHAT_TEXT));
      TextPlain = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_CHAT_TEXTPLAIN));
      Input		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_CHAT_INPUT));
		Scrollbar	= static_cast<CEGUI::Scrollbar*>(Text->getChildAtIdx(1));
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
		
		// subscribe scroll
		Scrollbar->subscribeEvent(CEGUI::Scrollbar::EventThumbTrackStarted, CEGUI::Event::Subscriber(UICallbacks::Chat::OnThumbTrackStarted));
		Scrollbar->subscribeEvent(CEGUI::Scrollbar::EventThumbTrackEnded, CEGUI::Event::Subscriber(UICallbacks::Chat::OnThumbTrackEnded));
      ScrollbarPlain->subscribeEvent(CEGUI::Scrollbar::EventThumbTrackStarted, CEGUI::Event::Subscriber(UICallbacks::Chat::OnThumbTrackStarted));
      ScrollbarPlain->subscribeEvent(CEGUI::Scrollbar::EventThumbTrackEnded, CEGUI::Event::Subscriber(UICallbacks::Chat::OnThumbTrackEnded));

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
	};

	void ControllerUI::Chat::Tick(double Tick, double Span)
	{
		if (OgreClient::Singleton->GameTick->CanChatUpdate() || ChatForceRenew)
		{
			if (Queue->Count > 0 || DeleteCounter > 0)
			{
            ::CEGUI::Window* wnd = PlainMode ? Chat::TextPlain : Chat::Text;

				// append new
				if (Queue->Count > 0)
				{
					// get first
					ServerString^ msg = Queue->Dequeue();
					CEGUI::String& str = GetChatString(msg);

					// append next ones
					while(Queue->Count > 0)
					{
						msg = Queue->Dequeue();			
						str = str.append(GetChatString(msg));
					}
				
               // set fully
               if (ChatForceRenew)
                  wnd->setText(str);

               // append
               else
                  wnd->appendText(str);


				}

            if (ChatForceRenew)
               DeleteCounter = 0;
            
            else
            {
               size_t idx = CEGUI::String::npos;

               // remove
               while (DeleteCounter > 0)
               {
                  idx = wnd->getText().find("\n", idx + 1);
                  DeleteCounter--;
               }

               if (idx != CEGUI::String::npos)
                  wnd->eraseText(0, idx + 1);
            }

				// save update tick
				OgreClient::Singleton->GameTick->DidChatUpdate();
			}

         ChatForceRenew = false;
		}
	};

	::CEGUI::String ControllerUI::Chat::GetChatString(ServerString^ ChatMessage)
	{
      if (PlainMode)
         return StringConvert::CLRToCEGUI(ChatMessage->FullString + ::System::Environment::NewLine);

		// text with CEGUI markup and escapes
		::System::String^ text = ::System::String::Empty;

		// walk style definitions
		for each (ChatStyle^ style in ChatMessage->Styles)
		{
			// add fontselection
			// normal
			if (!style->IsBold && !style->IsCursive)			
				text += UI_TAGFONT_NORMAL;
			
			// bold not italic
			else if (style->IsBold && !style->IsCursive)			
				text += UI_TAGFONT_BOLD;
				
			// not bold but italic
			else if (!style->IsBold && style->IsCursive)			
				text += UI_TAGFONT_ITALIC;

			// bold and italic
			else if (style->IsBold && style->IsCursive)			
				text += UI_TAGFONT_BOLDITALIC;				
			
			// add color
			switch(style->Color)
			{
				case ChatColor::Black:
					text += UI_TAGCOLOR_BLACK;
					break;

				case ChatColor::Blue:
					text += UI_TAGCOLOR_BLUE;
					break;

				case ChatColor::Green:
					text += UI_TAGCOLOR_CHATGREEN;
					break;

				case ChatColor::Purple:
					text += UI_TAGCOLOR_CHATPURPLE;
					break;
				
				case ChatColor::Red:
					text += UI_TAGCOLOR_CHATRED;
					break;

				case ChatColor::White:
					text += UI_TAGCOLOR_WHITE;
					break;
#ifndef VANILLA
				case ChatColor::BrightRed:
					text += UI_TAGCOLOR_CHATBRIGHTRED;
					break;

				case ChatColor::LightGreen:
					text += UI_TAGCOLOR_CHATLIGHTGREEN;
					break;

				case ChatColor::Yellow:
					text += UI_TAGCOLOR_CHATYELLOW;
					break;

				case ChatColor::Pink:
					text += UI_TAGCOLOR_CHATPINK;
					break;

				case ChatColor::Orange:
					text += UI_TAGCOLOR_CHATORANGE;
					break;

				case ChatColor::Aquamarine:
					text += UI_TAGCOLOR_CHATAQUAMARINE;
					break;

				case ChatColor::Cyan:
					text += UI_TAGCOLOR_CHATCYAN;
					break;

				case ChatColor::Teal:
					text += UI_TAGCOLOR_CHATTEAL;
					break;

				case ChatColor::DarkGrey:
					text += UI_TAGCOLOR_CHATDARKGREY;
					break;

				case ChatColor::Violet:
					text += UI_TAGCOLOR_CHATVIOLET;
					break;

				case ChatColor::Magenta:
					text += UI_TAGCOLOR_CHATMAGENTA;
					break;
#endif
			}

			// get substring for this style
			::System::String^ str = ChatMessage->FullString->Substring(style->StartIndex, style->Length);

			// replace "\" with "\\"	
			// and "[" with "\["
			str = str->Replace("\\", "\\\\");
			str = str->Replace("[", "\\[");

			// add textpart
			text += str;		
		}

		// append line break
		text += ::System::Environment::NewLine;

		// return as CEGUI string
		return StringConvert::CLRToCEGUI(text);
	};

	void ControllerUI::Chat::OnChatMessagesListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::Reset:
				Chat::Text->setText(STRINGEMPTY);
				DeleteCounter = 0;
				break;

			case ::System::ComponentModel::ListChangedType::ItemAdded:
				Queue->Enqueue(OgreClient::Singleton->Data->ChatMessages[e->NewIndex]);
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				DeleteCounter++;		
				break;
		}
	};

	bool UICallbacks::Chat::OnKeyDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& e2 = static_cast<const CEGUI::KeyEventArgs&>(e);
		CEGUI::KeyEventArgs& args	= const_cast<CEGUI::KeyEventArgs&>(e2);
		CEGUI::Editbox* chatInput	= ControllerUI::Chat::Input;
		const CEGUI::String text	= chatInput->getText();				
		
		::System::Collections::Generic::List<::System::String^>^ chatCommandHistory =
			OgreClient::Singleton->Data->ChatCommandHistory;

		// base handler for copy&paste clipboard
		bool handled = UICallbacks::OnCopyPasteKeyDown(e);	
		::System::String^ str;

		switch(args.scancode)
		{
			case CEGUI::Key::Scan::Return:
			case CEGUI::Key::Scan::NumpadEnter:
				// exec chatcommand
				OgreClient::Singleton->ExecChatCommand(
					StringConvert::CEGUIToCLR(text));

				// clear input
				chatInput->setText(STRINGEMPTY);
				
				// reset chatcommand history scrollindex
				OgreClient::Singleton->Data->ChatCommandHistoryIndex = -1;

				// focus back game
				ControllerUI::ActivateRoot();
							
				handled = true;
				break;

			case CEGUI::Key::Scan::Escape:
				// reset chatcommand history scrollindex
				OgreClient::Singleton->Data->ChatCommandHistoryIndex = -1;

				ControllerUI::ActivateRoot();
				handled = true;
				break;

			case CEGUI::Key::ArrowUp:
				str = OgreClient::Singleton->Data->ChatCommandHistoryGetNext();

				if (str)
				{
					::CEGUI::String ceguistr = StringConvert::CLRToCEGUI(str);

					chatInput->setText(ceguistr);
					chatInput->setCaretIndex(ceguistr.length());
				}
            handled = true;
				break;

			case CEGUI::Key::ArrowDown:
				str = OgreClient::Singleton->Data->ChatCommandHistoryGetPrevious();

				if (str)
				{
					::CEGUI::String ceguistr = StringConvert::CLRToCEGUI(str);

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

   bool UICallbacks::Chat::OnThumbTrackStarted(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& e2 = static_cast<const CEGUI::WindowEventArgs&>(e);
      
      // pick scrollbar for text mode
      //::CEGUI::Scrollbar* bar = ControllerUI::Chat::PlainMode ? 
      //   ControllerUI::Chat::ScrollbarPlain : 
      //   ControllerUI::Chat::Scrollbar;

      // disable autoscroll on both until we
      // re-enable it possibly in OnThumTrackEnded
      ControllerUI::Chat::ScrollbarPlain->setEndLockEnabled(false);
      ControllerUI::Chat::Scrollbar->setEndLockEnabled(false);

      return true;
   };

   bool UICallbacks::Chat::OnThumbTrackEnded(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& e2 = static_cast<const CEGUI::WindowEventArgs&>(e);

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

   bool UICallbacks::Chat::OnTextClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& e2 = static_cast<const CEGUI::MouseEventArgs&>(e);

      // only for right mousebutton
      if (e2.button != ::CEGUI::MouseButton::RightButton)
         return true;

      // get both chat controls and their scrollbars
      ::CEGUI::Window* chat = ControllerUI::Chat::Text;
      ::CEGUI::Window* chatPlain = ControllerUI::Chat::TextPlain;
      ::CEGUI::Scrollbar* barNormal = ControllerUI::Chat::Scrollbar;
      ::CEGUI::Scrollbar* barPlain = ControllerUI::Chat::ScrollbarPlain;

      // flip plain mode and force update on next Tick()
      ControllerUI::Chat::PlainMode = !ControllerUI::Chat::PlainMode;
      ControllerUI::Chat::ChatForceRenew = true;

      if (ControllerUI::Chat::PlainMode)
      {
         // show one, hide the other
         chat->setVisible(false);
         chatPlain->setVisible(true);

         // transfer current scroll state to other
         barPlain->setScrollPosition(barNormal->getScrollPosition());
         barPlain->setPageSize(barNormal->getPageSize());
         barPlain->setDocumentSize(barNormal->getDocumentSize());
      }
      else
      {
         // show one, hide the other
         chat->setVisible(true);
         chatPlain->setVisible(false);

         // transfer current scroll state to other
         barNormal->setScrollPosition(barPlain->getScrollPosition());
         barNormal->setPageSize(barPlain->getPageSize());
         barNormal->setDocumentSize(barPlain->getDocumentSize());
      }

      //add all chat history again for new mode
      for each(ServerString^ msg in OgreClient::Singleton->Data->ChatMessages)
         ControllerUI::Chat::Queue->Enqueue(msg);

      return true;
   };

};};
