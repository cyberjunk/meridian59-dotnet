#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Spells::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_SPELLS_WINDOW));
		List	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_SPELLS_LIST));
		
		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutSpells->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutSpells->getSize());

		// attach listener to avatar spells
		OgreClient::Singleton->Data->AvatarSpells->ListChanged += 
			gcnew ListChangedEventHandler(OnSpellsListChanged);

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Spells::OnKeyUp));
	};

	void ControllerUI::Spells::Destroy()
	{	 
		// detach listener from avatar spells
		OgreClient::Singleton->Data->AvatarSpells->ListChanged -= 
			gcnew ListChangedEventHandler(OnSpellsListChanged);		
	};

	void ControllerUI::Spells::OnSpellsListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				SpellAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				SpellRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				SpellChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Spells::SpellAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		StatList^ obj = OgreClient::Singleton->Data->AvatarSpells[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARSKILLITEM);
		
		// set ID
		widget->setID(obj->ObjectID);

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::Spells::OnItemClicked));
		
		// subscribe doubleclick event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseDoubleClick, 
			CEGUI::Event::Subscriber(UICallbacks::Spells::OnItemDoubleClicked));
			
		// get children
		CEGUI::DragContainer* dragger = 
			(CEGUI::DragContainer*)widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);
		
		CEGUI::Window* icon	= dragger->getChildAtIdx(0);
		CEGUI::Window* name	= widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_NAME);
		CEGUI::Window* percent = widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_PERCENT);
				
		// insert in ui-list
		if ((int)List->getItemCount() > Index)
			List->insertItem(widget, List->getItemFromIndex(Index));
		
		// or add
		else
			List->addItem(widget);
		
		// fix a big with last item not visible
		// when insertItem was used
		List->notifyScreenAreaChanged(true);

		// update values
		SpellChange(Index);
	};

	void ControllerUI::Spells::SpellRemove(int Index)
	{
		// check
		if ((int)List->getItemCount() > Index)		
			List->removeItem(List->getItemFromIndex(Index));
	};

	void ControllerUI::Spells::SpellChange(int Index)
	{
		StatList^ obj = OgreClient::Singleton->Data->AvatarSpells[Index];

		// check
		if ((int)List->getItemCount() > Index)
		{
			CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);
			
			CEGUI::DragContainer* dragger = 
				(CEGUI::DragContainer*)wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);
		
			CEGUI::Window* icon		= dragger->getChildAtIdx(0);				
			CEGUI::Window* name		= wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_NAME);
			CEGUI::Window* percent	= wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_PERCENT);
								
			// set name
			name->setText(StringConvert::CLRToCEGUI(obj->ResourceName));
			percent->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SkillPoints) + '%');

			// set image if available
			if (obj->Resource != nullptr && obj->Resource->Frames->Count > 0)
			{
				Ogre::TextureManager* texMan = Ogre::TextureManager::getSingletonPtr();
					
				// build name
				::Ogre::String oStrName = 
					StringConvert::CLRToOgre(UI_NAMEPREFIX_STATICICON + obj->ResourceIconName + "/0");

				// possibly create texture
				Util::CreateTextureA8R8G8B8(obj->Resource->Frames[0], oStrName, UI_RESGROUP_IMAGESETS, MIP_DEFAULT);

				// reget TexPtr (no return from function works, ugh..)
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					// possibly create cegui wrap around it
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);

					// set image
					icon->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}
		}
	};

	bool UICallbacks::Spells::OnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
		::CEGUI::ItemListbox* list = ControllerUI::Spells::List;
		SkillList^ spells = OgreClient::Singleton->Data->AvatarSpells;

		// 1) Return/Enter
		/*if (args.scancode == CEGUI::Key::Return ||
			args.scancode == CEGUI::Key::NumpadEnter)
		{
			::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

			// activate chat if no selection
			if (!itm)
			{
				ControllerUI::Chat::Window->setVisible(true);
				ControllerUI::Chat::Input->activate();			
			}

			// otherwise try to cast the spell
			else			
				OgreClient::Singleton->SendReqCastMessage(itm->getID());
			
		}*/

		// 2) ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

			// if selection, unset
			if (itm)			
				list->clearAllSelections();
			
			// if no selection, close window
			else
			{
				args.window->hide();

				// mark GUIroot active
				ControllerUI::ActivateRoot();
			}
		}

		// 3) ArrowDown
		else if (args.scancode == CEGUI::Key::ArrowDown)
		{
			::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();
			
			if (itm)
			{
				size_t idx = list->getItemIndex(itm);
				idx++;

				if (idx < list->getItemCount())
				{
					::CEGUI::ItemEntry* nextitm = list->getItemFromIndex(idx);
					nextitm->setSelected(true);
					list->ensureItemIsVisibleVert(*nextitm);
				}
			}		
		}

		// 4) ArrowUp
		else if (args.scancode == CEGUI::Key::ArrowUp)
		{
			::CEGUI::ItemEntry* itm = list->getFirstSelectedItem();

			if (itm)
			{
				size_t idx = list->getItemIndex(itm);
				
				if (idx > 0)
				{
					idx--;

					if (idx < list->getItemCount())
					{
						::CEGUI::ItemEntry* nextitm = list->getItemFromIndex(idx);
						nextitm->setSelected(true);
						list->ensureItemIsVisibleVert(*nextitm);
					}
				}
			}
		}

		// 5) Jump to item with startletter
		else
		{
			if (!ControllerInput::OISKeyboard)
				return true;

			// convert keycode to char
			const ::std::string cstr = ControllerInput::OISKeyboard->getAsString((::OIS::KeyCode)args.scancode);

			// get spells with prefix
			SkillList^ items = spells->GetItemsByPrefix(StringConvert::OgreToCLR(cstr), false);

			// must have 1 match
			if (items->Count == 0)
				return true;

			// look it up by id
			for (size_t i = 0; i < list->getItemCount(); i++)
			{
				::CEGUI::ItemEntry* itm = list->getItemFromIndex(i);

				if (itm->getID() != items[0]->ObjectID)
					continue;

				// select & scroll
				itm->setSelected(true);
				list->ensureItemIsVisibleVert(*itm);
				
				break;
			}
		}

		return true;
	};

	bool UICallbacks::Spells::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// single rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};

	bool UICallbacks::Spells::OnItemDoubleClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// double leftclick casts
		if (args.button == CEGUI::MouseButton::LeftButton)					
			OgreClient::Singleton->SendReqCastMessage(itm->getID());
			
		return true;
	};
};};