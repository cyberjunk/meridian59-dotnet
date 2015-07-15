#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::MainButtonsRight::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_MAINBUTTONSRIGHT_WINDOW));
		Inventory	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_INVENTORY));
		Spells		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_SPELLS));
		Skills		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_SKILLS));
		Actions		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_ACTIONS));
		Attributes	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_ATTRIBUTES));
		Quests		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_QUESTS));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutMainButtonsRight->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutMainButtonsRight->getSize());

		// subscribe click to head
		Inventory->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
		Spells->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
		Skills->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
		Actions->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
		Attributes->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
		Quests->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));

		// subscribe mouse events
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnMouseDown));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnMouseUp));		
	};

	void ControllerUI::MainButtonsRight::Destroy()
	{	     			
	};

	bool UICallbacks::MainButtonsRight::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// inventory clicked
		if (args.window == ControllerUI::MainButtonsRight::Inventory)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Inventory::Window);
		}

		// spells clicked
		else if (args.window == ControllerUI::MainButtonsRight::Spells)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Spells::Window);
		}

		// skills clicked
		else if (args.window == ControllerUI::MainButtonsRight::Skills)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Skills::Window);
		}

		// actions clicked
		else if (args.window == ControllerUI::MainButtonsRight::Actions)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Actions::Window);
		}
		
		// attributes clicked
		else if (args.window == ControllerUI::MainButtonsRight::Attributes)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Attributes::Window);
		}

		// quests clicked
		else if (args.window == ControllerUI::MainButtonsRight::Quests)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Quests::Window);
		}

		return true;
	};

	bool UICallbacks::MainButtonsRight::OnMouseDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// set this window as moving one
		ControllerUI::MovingWindow = ControllerUI::MainButtonsRight::Window;

		return true;
	};

	bool UICallbacks::MainButtonsRight::OnMouseUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// unset this window as moving one
		ControllerUI::MovingWindow = nullptr;

		return true;
	};
};};