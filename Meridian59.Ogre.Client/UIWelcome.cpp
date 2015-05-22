#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Welcome::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_WELCOME_WINDOW));
		Avatars	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_WELCOME_AVATARS));
		Select	= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_WELCOME_SELECT));
		MOTD	= static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_WELCOME_MOTD));

		// attach listener to chatmessage list
		OgreClient::Singleton->Data->WelcomeInfo->Characters->ListChanged += 
			gcnew ListChangedEventHandler(OnCharactersListChanged);
           
		OgreClient::Singleton->Data->WelcomeInfo->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnWelcomeInfoPropertyChanged);

		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Welcome::OnWindowKeyUp));
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::Welcome::OnWindowCloseClick));

		// subscribe avatar list selection change
		Avatars->subscribeEvent(CEGUI::ItemEntry::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::Welcome::OnAvatarSelectionChanged));
		
		// subscribe selectbutton
		Select->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Welcome::OnSelectClicked));
		
		// subscribe keydown on MOTD
		MOTD->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		
		// init existing ones
		for(int i = 0; i < OgreClient::Singleton->Data->WelcomeInfo->Characters->Count; i++)
			CharacterAdd(i);

		// set MOTD
		MOTD->setText(
			StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->WelcomeInfo->MOTD));
	};

	void ControllerUI::Welcome::Destroy()
	{	
		OgreClient::Singleton->Data->WelcomeInfo->Characters->ListChanged -= 
			gcnew ListChangedEventHandler(OnCharactersListChanged);
           
		OgreClient::Singleton->Data->WelcomeInfo->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnWelcomeInfoPropertyChanged);
        
	};

	void ControllerUI::Welcome::OnWelcomeInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// MOTD
		if (::System::String::Equals(e->PropertyName, WelcomeInfo::PROPNAME_MOTD))
		{
			// set MOTD
			MOTD->setText(
				StringConvert::CLRToCEGUI(OgreClient::Singleton->Data->WelcomeInfo->MOTD));
		}
	};

	void ControllerUI::Welcome::OnCharactersListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				CharacterAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				CharacterRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				//CharacterChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Welcome::CharacterAdd(int Index)
	{
		WelcomeInfo^ info = OgreClient::Singleton->Data->WelcomeInfo;

		// windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_CHARACTERLISTBOXITEM);
		
		// set id
		widget->setID(info->Characters[Index]->ID);

		// get namelabel child
		CEGUI::Window* name = widget->getChildAtIdx(UI_WELCOME_CHILDINDEX_CHARACTERS_NAME);

		// determine avatarname
		CEGUI::String avatarName = (info->Characters[Index]->IsEmptySlot) 
			? UI_AVATARNAME_FOR_UNSET : StringConvert::CLRToCEGUI(info->Characters[Index]->Name);

		// set avatarname
		name->setText(avatarName);

		name->subscribeEvent(
			CEGUI::Window::EventMouseDoubleClick,
			CEGUI::Event::Subscriber(UICallbacks::Welcome::OnItemDoubleClick));

		// insert in ui-list
		if ((int)Avatars->getItemCount() > Index)
			Avatars->insertItem(widget, Avatars->getItemFromIndex(Index));
		
		// or add
		else
			Avatars->addItem(widget);
		
		// fix a big with last item not selectable
		// when insertItem was used
		Avatars->notifyScreenAreaChanged(true);

		ConnectionInfo^ coninfo = OgreClient::Singleton->Config->SelectedConnectionInfo;

		// preselect the charactername which we last used
		if (coninfo && info->Characters[Index]->Name == coninfo->Character)
			widget->setSelected(true);		
	};

	void ControllerUI::Welcome::CharacterRemove(int Index)
	{
		// check
		if ((int)Avatars->getItemCount() > Index)		
			Avatars->removeItem(Avatars->getItemFromIndex(Index));
	};

	bool UICallbacks::Welcome::OnItemDoubleClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::ItemListbox* listBox = ControllerUI::Welcome::Avatars;

		// welcome info data model
		WelcomeInfo^ welcomeInfo = OgreClient::Singleton->Data->WelcomeInfo;

		// get selection
		CEGUI::ItemEntry* selectedItem = listBox->getFirstSelectedItem();

		if (!selectedItem)
			return true;

		int index = (int)listBox->getItemIndex(selectedItem);

		// show creation wizard
		if (welcomeInfo->Characters[index]->IsEmptySlot)
		{
			// request avatar creation data
			OgreClient::Singleton->SendSystemMessageSendCharInfo(
				welcomeInfo->Characters[index]->ID);
		
			// set UI mode to avatar creation
			OgreClient::Singleton->Data->UIMode = UIMode::AvatarCreation;
		}

		// login selected avatar
		else
		{
			// save last logged in avatarname in config
			OgreClient::Singleton->Config->SelectedConnectionInfo->Character =
				welcomeInfo->Characters[index]->Name;

			// log it in
			OgreClient::Singleton->SendUseCharacterMessage(index, true);
			OgreClient::Singleton->SendUserCommandSafetyMessage(true);
		}
		
		return true;
	};

	bool UICallbacks::Welcome::OnSelectClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::ItemListbox* listBox = ControllerUI::Welcome::Avatars;

		// welcome info data model
		WelcomeInfo^ welcomeInfo = OgreClient::Singleton->Data->WelcomeInfo;

		// get selection
		CEGUI::ItemEntry* selectedItem = listBox->getFirstSelectedItem();

		// active selection required
		if (selectedItem != NULL)
		{
			int index = (int)listBox->getItemIndex(selectedItem);

			// show creation wizard
			if (welcomeInfo->Characters[index]->IsEmptySlot)
			{
				// request avatar creation data
				OgreClient::Singleton->SendSystemMessageSendCharInfo(
					welcomeInfo->Characters[index]->ID);

				// set UI mode to avatar creation
				OgreClient::Singleton->Data->UIMode = UIMode::AvatarCreation;
			}

			// login selected avatar
			else
			{
				// save last logged in avatarname in config
				OgreClient::Singleton->Config->SelectedConnectionInfo->Character =
					welcomeInfo->Characters[index]->Name;

				// log it in
				OgreClient::Singleton->SendUseCharacterMessage(index, true);
				OgreClient::Singleton->SendUserCommandSafetyMessage(true);
			}
		}

		return true;
	};

	bool UICallbacks::Welcome::OnAvatarSelectionChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = (CEGUI::WindowEventArgs&)e;
		const CEGUI::ItemListbox* listBox = ControllerUI::Welcome::Avatars;
		CEGUI::PushButton* selectButton = ControllerUI::Welcome::Select;

		WelcomeInfo^ welcomeInfo = OgreClient::Singleton->Data->WelcomeInfo;
		
		// get selection
		CEGUI::ItemEntry* selectedItem = listBox->getFirstSelectedItem();

		if (selectedItem != NULL)
		{
			int index = (int)listBox->getItemIndex(selectedItem);
			
			// check if empty slot
			CEGUI::String buttonText = (welcomeInfo->Characters[index]->IsEmptySlot)
				? "Create" : "Select";

			selectButton->setEnabled(true);
			selectButton->setText(buttonText);
		}
		else
		{
			selectButton->setEnabled(false);
		}

		return true;
	};
	
	bool UICallbacks::Welcome::OnWindowCloseClick(const CEGUI::EventArgs& e)
	{
		OgreClient::Singleton->Disconnect();
		
		return true;
	};

	bool UICallbacks::Welcome::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = (const CEGUI::KeyEventArgs&)e;
		const CEGUI::ItemListbox* listBox = ControllerUI::Welcome::Avatars;
		WelcomeInfo^ welcomeInfo = OgreClient::Singleton->Data->WelcomeInfo;

		if (args.scancode == ::CEGUI::Key::Scan::Escape)
		{
			OgreClient::Singleton->Disconnect();
		}
		else if (args.scancode == ::CEGUI::Key::Scan::Return ||
			args.scancode == ::CEGUI::Key::Scan::NumpadEnter)
		{
			// get selection
			CEGUI::ItemEntry* selectedItem = listBox->getFirstSelectedItem();

			if (!selectedItem)
				return true;

			int index = (int)listBox->getItemIndex(selectedItem);

			// show creation wizard
			if (welcomeInfo->Characters[index]->IsEmptySlot)
			{
				// request avatar creation data
				OgreClient::Singleton->SendSystemMessageSendCharInfo(
					welcomeInfo->Characters[index]->ID);

				// set UI mode to avatar creation
				OgreClient::Singleton->Data->UIMode = UIMode::AvatarCreation;
			}

			// login selected avatar
			else
			{
				// save last logged in avatarname in config
				OgreClient::Singleton->Config->SelectedConnectionInfo->Character =
					welcomeInfo->Characters[index]->Name;

				// log it in
				OgreClient::Singleton->SendUseCharacterMessage(index, true);
				OgreClient::Singleton->SendUserCommandSafetyMessage(true);
			}
		}

		return true;
	};
};};