#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Actions::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_ACTIONS_WINDOW));
		List	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_ACTIONS_LIST));
		
		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutActions->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutActions->getSize());

		// create action entries
		CreateItem(AvatarAction::Attack);
		CreateItem(AvatarAction::Rest);
		CreateItem(AvatarAction::Dance);
		CreateItem(AvatarAction::Wave);
		CreateItem(AvatarAction::Point);
		CreateItem(AvatarAction::Loot);
		CreateItem(AvatarAction::Buy);
		CreateItem(AvatarAction::Inspect);
		CreateItem(AvatarAction::Trade);
		CreateItem(AvatarAction::Activate);
		CreateItem(AvatarAction::GuildInvite);

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
	};

	void ControllerUI::Actions::Destroy()
	{				
	};

	void ControllerUI::Actions::ApplyLanguage()
	{
		Window->setText(GetLangWindowTitle(LANGSTR_WINDOW_TITLE::ACTIONS));
	};

	void ControllerUI::Actions::CreateItem(AvatarAction Type)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_ACTIONITEM);

		// save avataraction enum value in ID
		widget->setID((unsigned int)Type);

		// subscribe doubleclick event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseDoubleClick, 
			CEGUI::Event::Subscriber(UICallbacks::Actions::OnItemDoubleClicked));
		
		// check
		if (widget->getChildCount() > 1)
		{
			CEGUI::DragContainer* iconDrag = (CEGUI::DragContainer*)widget->getChildAtIdx(UI_ACTIONS_CHILDINDEX_ICON);
			CEGUI::Window* icon	= iconDrag->getChildAtIdx(0);			
			CEGUI::Window* name	= (CEGUI::Window*)widget->getChildAtIdx(UI_ACTIONS_CHILDINDEX_NAME);

			CEGUI::String strIconName;

			// note: this can not be translated yet, because it must match the enum value
			CEGUI::String strName = StringConvert::CLRToCEGUI(Type.ToString());

			switch(Type)
			{
				case AvatarAction::Attack:
					strIconName = UI_IMAGE_ACTION_ATTACK;
					break;

				case AvatarAction::Rest:
					strIconName = UI_IMAGE_ACTION_REST;
					break;

				case AvatarAction::Dance:
					strIconName = UI_IMAGE_ACTION_DANCE;
					break;

				case AvatarAction::Wave:
					strIconName = UI_IMAGE_ACTION_WAVE;
					break;

				case AvatarAction::Point:
					strIconName = UI_IMAGE_ACTION_POINT;
					break;

				case AvatarAction::Loot:
					strIconName = UI_IMAGE_ACTION_LOOT;
					break;

				case AvatarAction::Buy:
					strIconName = UI_IMAGE_ACTION_BUY;
					break;

				case AvatarAction::Inspect:
					strIconName = UI_IMAGE_ACTION_INSPECT;
					break;

				case AvatarAction::Trade:
					strIconName = UI_IMAGE_ACTION_TRADE;
					break;

				case AvatarAction::Activate:
					strIconName = UI_IMAGE_ACTION_ACTIVATE;
					break;

				case AvatarAction::GuildInvite:
					strIconName = UI_IMAGE_ACTION_GUILDINVITE;
					break;

				default:
					strIconName = STRINGEMPTY;
					strName = STRINGEMPTY;
					break;
			}

			// set name
			name->setText(strName);

			if (strIconName != STRINGEMPTY)
				icon->setProperty(UI_PROPNAME_IMAGE, strIconName);
		}

		// add
		List->addItem(widget);

		// fix a big with last item not selectable
		// when insertItem was used
		List->notifyScreenAreaChanged(true);
	};
	
	bool UICallbacks::Actions::OnItemDoubleClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		const CEGUI::ItemEntry* itm = (CEGUI::ItemEntry*)args.window;
		
		// get avataraction enum value set in ID
		AvatarAction action = (AvatarAction)itm->getID();

		// execute action
		OgreClient::Singleton->ExecAction(action);

		return true;
	};
};};