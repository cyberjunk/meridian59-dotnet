#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::MainButtonsLeft::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_MAINBUTTONSLEFT_WINDOW));
		Chat	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSLEFT_CHAT));
		Guild	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSLEFT_GUILD));
		Mail	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSLEFT_MAIL));
		Map		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSLEFT_MAP));
		Options = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSLEFT_OPTIONS));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutMainButtonsLeft->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutMainButtonsLeft->getSize());

		// subscribe click to head
		Chat->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnItemClicked));
		Guild->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnItemClicked));
		Mail->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnItemClicked));
		Map->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnItemClicked));
		Options->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnItemClicked));

		// subscribe mouse events
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnMouseDown));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::MainButtonsLeft::OnMouseUp));		
	};

	void ControllerUI::MainButtonsLeft::Destroy()
	{	     			
	};

	bool UICallbacks::MainButtonsLeft::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// chat clicked
		if (args.window == ControllerUI::MainButtonsLeft::Chat)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Chat::Window);
		}

		// guild clicked
		else if (args.window == ControllerUI::MainButtonsLeft::Guild)
		{
			// hide
			if (OgreClient::Singleton->Data->GuildInfo->IsVisible)
				OgreClient::Singleton->Data->GuildInfo->Clear(true);

			// or request info
			else
			{
				OgreClient::Singleton->SendUserCommandGuildInfoReq();
				OgreClient::Singleton->SendUserCommandGuildGuildListReq();
				OgreClient::Singleton->SendUserCommandGuildShieldListReq();
				OgreClient::Singleton->SendUserCommandGuildShieldInfoReq();
			}
		}

		// mail clicked
		else if (args.window == ControllerUI::MainButtonsLeft::Mail)
		{
			// show or hide
			ControllerUI::ToggleVisibility(ControllerUI::Mail::Window);

			// request mail if we made it visible
			if (ControllerUI::Mail::Window->isVisible())						
				OgreClient::Singleton->SendReqGetMail();
		}

		// map clicked
		else if (args.window == ControllerUI::MainButtonsLeft::Map)
		{
			ControllerUI::ToggleVisibility(ControllerUI::MiniMap::Window);
		}

		// options clicked
		else if (args.window == ControllerUI::MainButtonsLeft::Options)
		{
			ControllerUI::ToggleVisibility(ControllerUI::Options::Window);
		}

		return true;
	};

	bool UICallbacks::MainButtonsLeft::OnMouseDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// set this window as moving one
		ControllerUI::MovingWindow = ControllerUI::MainButtonsLeft::Window;

		return true;
	};

	bool UICallbacks::MainButtonsLeft::OnMouseUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// unset this window as moving one
		ControllerUI::MovingWindow = nullptr;

		return true;
	};
};};