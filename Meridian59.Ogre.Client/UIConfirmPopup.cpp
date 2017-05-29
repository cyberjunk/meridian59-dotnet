#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::ConfirmPopup::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::DefaultWindow*>(guiRoot->getChild(UI_NAME_CONFIRMPOPUP_WINDOW));
		SubWindow = static_cast<CEGUI::FrameWindow*>(Window->getChild(UI_NAME_CONFIRMPOPUP_SUBWINDOW));
		Text	= static_cast<CEGUI::Window*>(SubWindow->getChild(UI_NAME_CONFIRMPOPUP_TEXT));
		Yes		= static_cast<CEGUI::PushButton*>(SubWindow->getChild(UI_NAME_CONFIRMPOPUP_YES));
		No		= static_cast<CEGUI::PushButton*>(SubWindow->getChild(UI_NAME_CONFIRMPOPUP_NO));
		OK = static_cast<CEGUI::PushButton*>(SubWindow->getChild(UI_NAME_CONFIRMPOPUP_OK));

		// subscribe key event
		Yes->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnYesClicked));
		
		// subscribe No button
		No->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnNoClicked));
		
		// subscribe OK button (uses Yes/Confirmed button handler)
		OK->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnYesClicked));
      
      // subscribe key up to trigger yes/no based on focused window
      Window->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnKeyUp));
      Yes->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnKeyUp));
      No->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnKeyUp));
      OK->subscribeEvent(CEGUI::PushButton::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnKeyUp));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));
	};

	void ControllerUI::ConfirmPopup::Destroy()
	{
	};

	void ControllerUI::ConfirmPopup::ApplyLanguage()
	{
	};

	// Makes the Yes/No buttons visible, sets ID.
	void ControllerUI::ConfirmPopup::ShowChoice(const ::CEGUI::String& text, uint id)
	{
      Mode = ConfirmPopup::DialogMode::YesNo;

		// set text
		Text->setText(text);

		// set ID
		ID = id;

		// set buttons
		Yes->setVisible(true);
		No->setVisible(true);
		OK->setVisible(false);

		// show popup
		Window->show();
		Window->moveToFront();

      // set no active by default
      No->activate();
	};

	// Makes the OK button visible, sets ID.
	void ControllerUI::ConfirmPopup::ShowOK(const ::CEGUI::String& text, uint id)
	{
      Mode = ConfirmPopup::DialogMode::Confirm;

		// set text
		Text->setText(text);

		// set ID
		ID = id;

		// set buttons
		OK->setVisible(true);
		Yes->setVisible(false);
		No->setVisible(false);

		// show popup
		Window->show();
		Window->moveToFront();

      // set button active
      OK->activate();
	};

	void ControllerUI::ConfirmPopup::_RaiseConfirm()
	{
		// execute handler(s)
		if (_confirmed != nullptr)
			_confirmed(nullptr, nullptr);

		// remove handler(s)
		_confirmed = nullptr;
		_cancelled = nullptr;
		ID = 0;
	};

	void ControllerUI::ConfirmPopup::_RaiseCancel()
	{
		// execute handler(s)
		if (_cancelled != nullptr)
			_cancelled(nullptr, nullptr);

		// remove handler(s)
		_confirmed = nullptr;
		_cancelled = nullptr;
		ID = 0;
	};

	bool UICallbacks::ConfirmPopup::OnYesClicked(const CEGUI::EventArgs& e)
	{
		// hide window
		ControllerUI::ConfirmPopup::Window->hide();

		// mark GUIroot active
		ControllerUI::ActivateRoot();
	
		// raise event
		ControllerUI::ConfirmPopup::_RaiseConfirm();

		return true;
	};

	bool UICallbacks::ConfirmPopup::OnNoClicked(const CEGUI::EventArgs& e)
	{
		// hide window
		ControllerUI::ConfirmPopup::Window->hide();

		// mark GUIroot active
		ControllerUI::ActivateRoot();

		// raise event
		ControllerUI::ConfirmPopup::_RaiseCancel();

		return true;
	};

   bool UICallbacks::ConfirmPopup::OnKeyUp(const CEGUI::EventArgs& e)
   {
      CEGUI::KeyEventArgs& args = (CEGUI::KeyEventArgs&)e;

      // Confirm mode
      if (ControllerUI::ConfirmPopup::Mode == ControllerUI::ConfirmPopup::DialogMode::Confirm)
      {
         // raise yes (=confirm) handler for all relevant
         if (args.scancode == CEGUI::Key::Return ||
             args.scancode == CEGUI::Key::NumpadEnter ||
             args.scancode == CEGUI::Key::Space ||
             args.scancode == CEGUI::Key::Escape)
         {
               OnYesClicked(e);
         }
      }

      // YesNo mode
      else if (ControllerUI::ConfirmPopup::Mode == ControllerUI::ConfirmPopup::DialogMode::YesNo)
      {
         // raise no handler for all relevant
         if (args.scancode == CEGUI::Key::Return ||
             args.scancode == CEGUI::Key::NumpadEnter ||
             args.scancode == CEGUI::Key::Space ||
             args.scancode == CEGUI::Key::Escape)
         {
            OnNoClicked(e);
         }
      }

      return true;
   };
};};