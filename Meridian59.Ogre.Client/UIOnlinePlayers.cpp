#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::OnlinePlayers::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_ONLINEPLAYERS_WINDOW));
		List	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_ONLINEPLAYERS_LIST));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutOnlinePlayers->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutOnlinePlayers->getSize());

		// attach listener to onlineplayers
		OgreClient::Singleton->Data->OnlinePlayers->ListChanged += 
			gcnew ListChangedEventHandler(&ControllerUI::OnlinePlayers::OnOnlinePlayersListChanged);
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
	};

	void ControllerUI::OnlinePlayers::Destroy()
	{	 
		// detach listener from onlineplayers
		OgreClient::Singleton->Data->OnlinePlayers->ListChanged -= 
			gcnew ListChangedEventHandler(&ControllerUI::OnlinePlayers::OnOnlinePlayersListChanged);		     		
	};

	void ControllerUI::OnlinePlayers::OnOnlinePlayersListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				OnlinePlayerAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				OnlinePlayerRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				OnlinePlayerChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::OnlinePlayers::OnlinePlayerAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		OnlinePlayer^ player = OgreClient::Singleton->Data->OnlinePlayers[Index];
		
		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_ONLINEPLAYERLISTBOXITEM);
		
		// set ID
		widget->setID(player->ID);

		// get checkbox child
		CEGUI::ToggleButton* wndCheckbox	= (CEGUI::ToggleButton*)widget->getChildAtIdx(0);
		
		if (OgreClient::Singleton->Data->IgnoreList->Contains(player->Name))
			wndCheckbox->setSelected(true);

		// subscribe check change event
		wndCheckbox->subscribeEvent(
			CEGUI::ToggleButton::EventSelectStateChanged, 
			CEGUI::Event::Subscriber(UICallbacks::OnlinePlayers::OnIgnoreSelectStateChanged));

		// insert in ui-list
		if ((int)List->getItemCount() > Index)
			List->insertItem(widget, List->getItemFromIndex(Index));
		
		// or add
		else
			List->addItem(widget);

		// fix a big with last item not selectable
		List->notifyScreenAreaChanged(true);

		// update values
		OnlinePlayerChange(Index);
	};

	void ControllerUI::OnlinePlayers::OnlinePlayerRemove(int Index)
	{
		// check
		if ((int)List->getItemCount() > Index)		
			List->removeItem(List->getItemFromIndex(Index));
	};

	void ControllerUI::OnlinePlayers::OnlinePlayerChange(int Index)
	{
		OnlinePlayer^ player = OgreClient::Singleton->Data->OnlinePlayers[Index];

		// check
		if ((int)List->getItemCount() > Index)
		{
			CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);

			// check
			if (wnd->getChildCount() > 1)
			{
				CEGUI::ToggleButton* wndCheckbox	= (CEGUI::ToggleButton*)wnd->getChildAtIdx(UI_ONLINEPLAYERS_CHILDINDEX_CHECKBOX);
				CEGUI::Window* wndName				= (CEGUI::Window*)wnd->getChildAtIdx(UI_ONLINEPLAYERS_CHILDINDEX_NAME);
			
				// get color
				::CEGUI::Colour color = ::CEGUI::Colour(
					NameColors::GetColorFor(player->Flags));
		
				// set color and name
				wndName->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				wndName->setText(StringConvert::CLRToCEGUI(player->Name));

#ifndef VANILLA
				// set tooltip
				if (player->Flags->Player == ObjectFlags::PlayerType::Moderator)
					wndName->setTooltipText(UI_TOOLTIPTEXT_MODERATOR);
				
				else if (player->Flags->Player == ObjectFlags::PlayerType::Creator)
					wndName->setTooltipText(UI_TOOLTIPTEXT_ADMIN);
#else
				if (player->Flags->Player == ObjectFlags::PlayerType::Creator)
					wndName->setTooltipText(UI_TOOLTIPTEXT_ADMIN);
#endif
				else if (player->Flags->Player == ObjectFlags::PlayerType::SuperDM || 
						 player->Flags->Player == ObjectFlags::PlayerType::DM)
					wndName->setTooltipText(UI_TOOLTIPTEXT_GM);
				
				else if (player->Flags->Player == ObjectFlags::PlayerType::Killer)
					wndName->setTooltipText(UI_TOOLTIPTEXT_MURDERER);

				else if (player->Flags->Player == ObjectFlags::PlayerType::Outlaw)
					wndName->setTooltipText(UI_TOOLTIPTEXT_OUTLAW);
			
				else
					wndName->setTooltipText(UI_TOOLTIPTEXT_LAWFUL);
			
				
				// todo: set ignorestate
			}
		}
	};

	bool UICallbacks::OnlinePlayers::OnIgnoreSelectStateChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);		
		const CEGUI::ToggleButton* itm = (CEGUI::ToggleButton*)args.window;
		
		unsigned int id = itm->getParent()->getID();

		OnlinePlayer^ player = 
			OgreClient::Singleton->Data->OnlinePlayers->GetItemByID(id);

		if (player)
		{
			if (itm->isSelected())
				OgreClient::Singleton->Data->IgnoreList->Add(player->Name);

			else
				OgreClient::Singleton->Data->IgnoreList->Remove(player->Name);
		}

		return true;
	};
};};