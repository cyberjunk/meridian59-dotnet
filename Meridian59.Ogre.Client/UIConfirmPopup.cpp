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

	bool UICallbacks::ConfirmPopup::OnYesClicked(const CEGUI::EventArgs& e)
	{
		// suicide the avatar
		OgreClient::Singleton->SendUserCommandSuicide();

		// hide window
		ControllerUI::ConfirmPopup::Window->hide();

		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	};

	bool UICallbacks::ConfirmPopup::OnNoClicked(const CEGUI::EventArgs& e)
	{
		// hide window
		ControllerUI::ConfirmPopup::Window->hide();

		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	};
};};