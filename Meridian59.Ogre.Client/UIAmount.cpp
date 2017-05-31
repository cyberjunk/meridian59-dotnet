#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Amount::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_AMOUNT_WINDOW));
		Value	= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_AMOUNT_VALUE));
		OK		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_AMOUNT_OK));
		
		// subscribe key event
		Value->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Amount::OnKeyUp));
		
		// subscribe OK button
		OK->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Amount::OnOKClicked));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));				
	};

	void ControllerUI::Amount::Destroy()
	{				
	};

	void ControllerUI::Amount::ApplyLanguage()
	{
		Window->setText(GetLangWindowTitle(LANGSTR_WINDOW_TITLE::AMOUNT));
	};

	void ControllerUI::Amount::ShowValues(unsigned int ID, unsigned int Count)
	{
		// save ID
		ControllerUI::Amount::ID = ID;
		
		// set amount
		Value->setText(::CEGUI::PropertyHelper<unsigned int>::toString(Count));

		// get current mousecursor position
		CEGUI::Vector2f pos = ControllerUI::MouseCursor->getPosition();
		
		// set position to mousecursor
		Window->setPosition(CEGUI::UVector2(
			CEGUI::UDim(0, pos.d_x),
			CEGUI::UDim(0, pos.d_y)));
		
		// show	
		Window->show();
		Window->moveToFront();

		// focus value
		Value->activate();
	};
	
	void ControllerUI::Amount::Drop()
	{				
		// get user entered amount
		CEGUI::String strVal = Value->getText();

		// check
		if (strVal != STRINGEMPTY && ObjectID::IsValid(ID))
		{
			// convert to uint
			unsigned int intVal = CEGUI::PropertyHelper<unsigned int>::fromString(strVal);

			// drop it
			OgreClient::Singleton->SendReqDropMessage(gcnew ObjectID(ID, intVal));

			// hide window
			ControllerUI::Amount::Window->hide();

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}
	};

	bool UICallbacks::Amount::OnOKClicked(const CEGUI::EventArgs& e)
	{
		ControllerUI::Amount::Drop();

		return true;
	};

	bool UICallbacks::Amount::OnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		if (args.scancode == CEGUI::Key::Return ||
			args.scancode == CEGUI::Key::NumpadEnter)
		{
			ControllerUI::Amount::Drop();
		}
		else if (args.scancode == CEGUI::Key::Escape)
		{
         // hide window
         ControllerUI::Amount::Window->hide();

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}
};};
