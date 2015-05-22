#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::RoomObjects::Initialize()
	{
		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_ROOMOBJECTS_WINDOW));
		List		= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_ROOMOBJECTS_LIST));
		ShowAll		= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWALL));
		ShowGuild	= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWGUILD));
		ShowEnemy	= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWENEMY));
		ShowFriend	= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWFRIEND));
		ShowAttack	= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWATTACK));
		ShowGet		= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWGET));
		ShowMinion	= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWMINION));
		ShowPK		= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWPK));
		ShowBuy		= static_cast<CEGUI::ToggleButton*>(Window->getChild(UI_NAME_ROOMOBJECTS_SHOWBUY));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutRoomObjects->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutRoomObjects->getSize());

		// attach listener to roomobjectsfiltered
		OgreClient::Singleton->Data->RoomObjectsFiltered->ListChanged += 
			gcnew ListChangedEventHandler(&ControllerUI::RoomObjects::OnRoomObjectsFilteredListChanged);
		
		// attach listener to data for targetobject
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerUI::RoomObjects::OnDataPropertyChanged);
		
		// initial togglebutton state
		ShowAll->setSelected(true);
		ShowGuild->setSelected(false);
		ShowEnemy->setSelected(false);
		ShowFriend->setSelected(false);
		ShowAttack->setSelected(false);
		ShowGet->setSelected(false);
		ShowMinion->setSelected(false);
		ShowPK->setSelected(false);
		ShowBuy->setSelected(false);

		ShowAll->setEnabled(!ShowAll->isSelected());
		ShowGuild->setEnabled(ShowAll->isSelected());
		ShowEnemy->setEnabled(ShowAll->isSelected());
		ShowFriend->setEnabled(ShowAll->isSelected());
		ShowAttack->setEnabled(ShowAll->isSelected());
		ShowGet->setEnabled(ShowAll->isSelected());
		ShowMinion->setEnabled(ShowAll->isSelected());
		ShowPK->setEnabled(ShowAll->isSelected());
		ShowBuy->setEnabled(ShowAll->isSelected());

		// subscribe togglebuttons
		ShowAll->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowGuild->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowEnemy->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowFriend->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowAttack->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowGet->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowMinion->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowPK->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));
		ShowBuy->subscribeEvent(CEGUI::ToggleButton::EventSelectStateChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnFilterSelectStateChanged));

		// subscribe selection change of list
		List->subscribeEvent(CEGUI::ItemListbox::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::RoomObjects::OnListSelectionChanged));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
	};

	void ControllerUI::RoomObjects::Destroy()
	{	 
		// detach listener from roomobjectsfiltered
		OgreClient::Singleton->Data->RoomObjectsFiltered->ListChanged -= 
			gcnew ListChangedEventHandler(&ControllerUI::RoomObjects::OnRoomObjectsFilteredListChanged);

		// detach listener from data
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerUI::RoomObjects::OnDataPropertyChanged);		
	};

	void ControllerUI::RoomObjects::OnRoomObjectsFilteredListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				RoomObjectAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				RoomObjectRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				// react only to changes which affect the view				
				if (::System::String::Equals(e->PropertyDescriptor->Name, RoomObject::PROPNAME_FLAGS) ||
					::System::String::Equals(e->PropertyDescriptor->Name, RoomObject::PROPNAME_HEALTHSTATUS) ||
					::System::String::Equals(e->PropertyDescriptor->Name, RoomObject::PROPNAME_SUBOVERLAYS))
				{
					RoomObjectChange(e->NewIndex);
				}
				break;
		}
	};

	void ControllerUI::RoomObjects::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// targetobject
		if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_TARGETOBJECT))
		{
			// try select target also in list
			if (OgreClient::Singleton->Data->TargetObject)
			{
				for(size_t i = 0; i < List->getItemCount(); i++)
				{
					::CEGUI::ItemEntry* itm = List->getItemFromIndex(i);

					itm->setSelected(itm->getID() == OgreClient::Singleton->Data->TargetObject->ID);
				}
			}

			// no target, make sure nothing is selected
			else
			{
				for(size_t i = 0; i < List->getItemCount(); i++)			
					List->getItemFromIndex(i)->setSelected(false);			
			}
		}
	};

	void ControllerUI::RoomObjects::RoomObjectAdd(int Index)
	{
		if (Index < 0)
			return;

		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		RoomObject^ roomobj = OgreClient::Singleton->Data->RoomObjectsFiltered[Index];
		
		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_ROOMOBJECTLISTBOXITEM);
		
		// set ID
		widget->setID(roomobj->ID);

		// insert in ui-list
		if ((int)List->getItemCount() > Index)
			List->insertItem(widget, List->getItemFromIndex(Index));
		
		// or add
		else
			List->addItem(widget);

		// fix a bug with last item not selectable
		List->notifyScreenAreaChanged(true);

		// mark our target as selected if we add it
		if (roomobj->IsTarget)
			widget->setSelected(true);

		// update values
		RoomObjectChange(Index);
	};

	void ControllerUI::RoomObjects::RoomObjectRemove(int Index)
	{
		if (Index < 0)
			return;

		// check
		if ((int)List->getItemCount() > Index)		
			List->removeItem(List->getItemFromIndex(Index));
	};

	void ControllerUI::RoomObjects::RoomObjectChange(int Index)
	{
		if (Index < 0)
			return;

		RoomObject^ roomobj = OgreClient::Singleton->Data->RoomObjectsFiltered[Index];

		// check
		if ((int)List->getItemCount() > Index)
		{
			CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);

			// check
			if (wnd->getChildCount() > 2)
			{
				CEGUI::Window* wndFaction	= (CEGUI::Window*)wnd->getChildAtIdx(UI_ROOMOBJECTS_CHILDINDEX_FACTION);
				CEGUI::Window* wndName		= (CEGUI::Window*)wnd->getChildAtIdx(UI_ROOMOBJECTS_CHILDINDEX_NAME);
				CEGUI::Window* wndHealth	= (CEGUI::Window*)wnd->getChildAtIdx(UI_ROOMOBJECTS_CHILDINDEX_HEALTH);
				
				// set faction shield color
				if (roomobj->HasEmblemDuke())
					wndFaction->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_PURPLE);

				else if (roomobj->HasEmblemPrincess())
					wndFaction->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_YELLOW);
				
				else if (roomobj->HasEmblemJonas())
					wndFaction->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_RED);

				else
					wndFaction->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_BLACK);

				// get name color
				::CEGUI::Colour color = ::CEGUI::Colour(
					NameColors::GetColorFor(roomobj->Flags));
		
				// set name color and name
				wndName->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
				wndName->setText(StringConvert::CLRToCEGUI(roomobj->Name));

				// some special name background for enemy/guild
				if (roomobj->Flags->IsMinimapEnemy)
					wndName->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_ROOMOBJECTS_BACKGROUND_ENEMY);

				else if (roomobj->Flags->IsMinimapGuildMate || roomobj->Flags->IsMinimapFriend)
					wndName->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_ROOMOBJECTS_BACKGROUND_FRIEND);
				
				else
					wndName->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_ROOMOBJECTS_BACKGROUND_NONE);

				// set healthstatus
				switch(roomobj->HealthStatus)
				{
				case HealthStatus::Unknown:
					wndHealth->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_BLACK);
					break;
				
				case HealthStatus::Green:
					wndHealth->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_GREEN);
					break;
				
				case HealthStatus::Yellow:
					wndHealth->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_YELLOW);
					break;
				
				case HealthStatus::Orange:
					wndHealth->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_ORANGE);
					break;
				
				case HealthStatus::Red:
					wndHealth->setProperty(UI_PROPNAME_BACKGROUNDCOLOURS, UI_COLOURRECT_RED);
					break;

				default:
					break;
				}
			}
		}
	};

	bool UICallbacks::RoomObjects::OnFilterSelectStateChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);		
		const CEGUI::ToggleButton* itm = (CEGUI::ToggleButton*)args.window;
		
		// get shortcut references to togglebuttons
		CEGUI::ToggleButton* all = ControllerUI::RoomObjects::ShowAll;
		CEGUI::ToggleButton* guild = ControllerUI::RoomObjects::ShowGuild;
		CEGUI::ToggleButton* enemy = ControllerUI::RoomObjects::ShowEnemy;
		CEGUI::ToggleButton* frienD = ControllerUI::RoomObjects::ShowFriend;
		CEGUI::ToggleButton* attack = ControllerUI::RoomObjects::ShowAttack;
		CEGUI::ToggleButton* get = ControllerUI::RoomObjects::ShowGet;
		CEGUI::ToggleButton* minion = ControllerUI::RoomObjects::ShowMinion;
		CEGUI::ToggleButton* pk = ControllerUI::RoomObjects::ShowPK;
		CEGUI::ToggleButton* buy = ControllerUI::RoomObjects::ShowBuy;

		// get filtered list
		RoomObjectListFiltered^ filteredList = 
			OgreClient::Singleton->Data->RoomObjectsFiltered;

		// if "all" got selected by userclick
		if (itm == all && itm->isSelected())
		{
			// disable "all" togglebutton
			all->setEnabled(false);
		
			// deselect the others
			// make sure they not execute their handler for this
			ControllerUI::RoomObjects::FlippedbyAll = true;

			guild->setSelected(false);
			enemy->setSelected(false);
			frienD->setSelected(false);
			attack->setSelected(false);
			get->setSelected(false);
			minion->setSelected(false);
			pk->setSelected(false);
			buy->setSelected(false);
		
			// reset flag
			ControllerUI::RoomObjects::FlippedbyAll = false;
			
			// clear old filter and refresh
			filteredList->FlagsFilter->Clear();
			filteredList->PlayerTypesFilter->Clear();
			filteredList->Refresh();
		}
		else
		{
			// do not execute the handler below
			// if it was caused by a flip on "all"
			if (ControllerUI::RoomObjects::FlippedbyAll)
				return true;

			// if no filter is set anymore, reactivate "all"
			if (!guild->isSelected() &&
				!enemy->isSelected() &&
				!frienD->isSelected() &&
				!frienD->isSelected() &&
				!attack->isSelected() &&
				!get->isSelected() &&
				!minion->isSelected() &&
				!pk->isSelected() &&
				!buy->isSelected())
			{
				all->setSelected(true);
			}
			else
			{
				// specific filter, deselect "all" checkbox, but enable it
				all->setSelected(false);
				all->setEnabled(true);
			
				// clear old filter
				filteredList->FlagsFilter->Clear();
				filteredList->PlayerTypesFilter->Clear();

				// build new
				ObjectFlags^ flags;

				if (guild->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
#else
					flags = gcnew ObjectFlags(0);
#endif
					flags->IsMinimapGuildMate = true;

					filteredList->FlagsFilter->Add(flags);
				}

				if (enemy->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
#else
					flags = gcnew ObjectFlags(0);
#endif
					flags->IsMinimapEnemy = true;

					filteredList->FlagsFilter->Add(flags);
				}

				if (frienD->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
#else
					flags = gcnew ObjectFlags(0);
#endif
					flags->IsMinimapFriend = true;

					filteredList->FlagsFilter->Add(flags);
				}
		
				if (attack->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
#else
					flags = gcnew ObjectFlags(0);
#endif
					flags->IsAttackable = true;

					filteredList->FlagsFilter->Add(flags);
				}
			
				if (get->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
#else
					flags = gcnew ObjectFlags(0);
#endif
					flags->IsGettable = true;

					filteredList->FlagsFilter->Add(flags);
				}
		
				if (minion->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
					flags->IsMinimapMinionOther = true;

					filteredList->FlagsFilter->Add(flags);

					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
					flags->IsMinimapMinionSelf = true;

					filteredList->FlagsFilter->Add(flags);
#endif
				}

				if (pk->isSelected())
				{
					filteredList->PlayerTypesFilter->Add(ObjectFlags::PlayerType::Killer);
					filteredList->PlayerTypesFilter->Add(ObjectFlags::PlayerType::Outlaw);
				}

				if (buy->isSelected())
				{
#ifndef VANILLA
					flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
#else
					flags = gcnew ObjectFlags(0);
#endif					
					flags->IsBuyable = true;

					filteredList->FlagsFilter->Add(flags);
				}

				filteredList->Refresh();
			}
		}

		return true;
	};

	bool UICallbacks::RoomObjects::OnListSelectionChanged(const CEGUI::EventArgs& e)
	{
		CEGUI::ItemListbox* list = ControllerUI::RoomObjects::List;
		CEGUI::ItemEntry* item = list->getFirstSelectedItem();

		// set target
		if (item)
			OgreClient::Singleton->Data->TargetID = item->getID();
		
		return true;
	};
};};