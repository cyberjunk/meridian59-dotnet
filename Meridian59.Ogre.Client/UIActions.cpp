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
			CEGUI::String strName;

			switch(Type)
			{
				case AvatarAction::Attack:
					strName = GetLangLabel(LANGSTR::ATTACK);
					strIconName = UI_IMAGE_ACTION_ATTACK;
					break;

				case AvatarAction::Rest:
					strName = GetLangLabel(LANGSTR::REST);
					strIconName = UI_IMAGE_ACTION_REST;
					break;

				case AvatarAction::Dance:
					strName = GetLangLabel(LANGSTR::DANCE);
					strIconName = UI_IMAGE_ACTION_DANCE;
					break;

				case AvatarAction::Wave:
					strName = GetLangLabel(LANGSTR::WAVE);
					strIconName = UI_IMAGE_ACTION_WAVE;
					break;

				case AvatarAction::Point:
					strName = GetLangLabel(LANGSTR::POINT);
					strIconName = UI_IMAGE_ACTION_POINT;
					break;

				case AvatarAction::Loot:
					strName = GetLangLabel(LANGSTR::LOOT);
					strIconName = UI_IMAGE_ACTION_LOOT;
					break;

				case AvatarAction::Buy:
					strName = GetLangLabel(LANGSTR::BUY);
					strIconName = UI_IMAGE_ACTION_BUY;
					break;

				case AvatarAction::Inspect:
					strName = GetLangLabel(LANGSTR::INSPECT);
					strIconName = UI_IMAGE_ACTION_INSPECT;
					break;

				case AvatarAction::Trade:
					strName = GetLangLabel(LANGSTR::TRADE);
					strIconName = UI_IMAGE_ACTION_TRADE;
					break;

				case AvatarAction::Activate:
					strName = GetLangLabel(LANGSTR::ACTIVATE);
					strIconName = UI_IMAGE_ACTION_ACTIVATE;
					break;

				case AvatarAction::GuildInvite:
					strName = GetLangLabel(LANGSTR::GUILDINVITE);
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
		const CEGUI::ItemListbox* listBox = ControllerUI::Actions::List;
		
		// check
		if (itm->getChildCount() > 1)
		{
			CEGUI::DragContainer* iconDrag = (CEGUI::DragContainer*)itm->getChildAtIdx(UI_ACTIONS_CHILDINDEX_ICON);			
			CEGUI::Window* icon	= iconDrag->getChildAtIdx(0);				
			CEGUI::Window* name	= (CEGUI::Window*)itm->getChildAtIdx(UI_ACTIONS_CHILDINDEX_NAME);
			
			CEGUI::String strName = name->getText();

			if (strName == GetLangLabel(LANGSTR::ATTACK))
				OgreClient::Singleton->ExecAction(AvatarAction::Attack);
			
			else if (strName == GetLangLabel(LANGSTR::REST))
				OgreClient::Singleton->ExecAction(AvatarAction::Rest);
						
			else if (strName == GetLangLabel(LANGSTR::DANCE))
				OgreClient::Singleton->ExecAction(AvatarAction::Dance);
						
			else if (strName == GetLangLabel(LANGSTR::WAVE))
				OgreClient::Singleton->ExecAction(AvatarAction::Wave);
					
			else if (strName == GetLangLabel(LANGSTR::POINT))
				OgreClient::Singleton->ExecAction(AvatarAction::Point);
						
			else if (strName == GetLangLabel(LANGSTR::LOOT))
				OgreClient::Singleton->ExecAction(AvatarAction::Loot);

			else if (strName == GetLangLabel(LANGSTR::BUY))
				OgreClient::Singleton->ExecAction(AvatarAction::Buy);

			else if (strName == GetLangLabel(LANGSTR::INSPECT))
				OgreClient::Singleton->ExecAction(AvatarAction::Inspect);

			else if (strName == GetLangLabel(LANGSTR::ACTIVATE))
				OgreClient::Singleton->ExecAction(AvatarAction::Activate);

			else if (strName == GetLangLabel(LANGSTR::TRADE))
				OgreClient::Singleton->ExecAction(AvatarAction::Trade);	

			else if (strName == GetLangLabel(LANGSTR::GUILDINVITE))
				OgreClient::Singleton->ExecAction(AvatarAction::GuildInvite);	
		}
		
		return true;
	};
};};