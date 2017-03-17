#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::ConfirmPopup::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_CONFIRMPOPUP_WINDOW));
		Text	= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_CONFIRMPOPUP_TEXT));
		Yes		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_CONFIRMPOPUP_YES));
		No		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_CONFIRMPOPUP_NO));
		
		// subscribe key event
		Yes->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnYesClicked));
		
		// subscribe OK button
		No->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::ConfirmPopup::OnNoClicked));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));				
	};

	void ControllerUI::ConfirmPopup::Destroy()
	{				
	};

	void ControllerUI::ConfirmPopup::ApplyLanguage()
	{
	};

	void ControllerUI::ConfirmPopup::Show(const ::CEGUI::String& text)
	{
		// set text
		Text->setText(text);

		// show popup
		Window->show();
		Window->moveToFront();
	};

	void ControllerUI::ConfirmPopup::_RaiseConfirm()
	{
		// execute handler(s)
		if (_confirmed != nullptr)
			_confirmed(nullptr, nullptr);

		// remove handler(s)
		_confirmed = nullptr;
	};

	void ControllerUI::ConfirmPopup::_RaiseCancel()
	{
		// execute handler(s)
		if (_cancelled != nullptr)
			_cancelled(nullptr, nullptr);

		// remove handler(s)
		_cancelled = nullptr;
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
};};