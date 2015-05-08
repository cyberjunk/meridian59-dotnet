#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	static ControllerUI::ControllerUI(void)
	{
		renderer		= nullptr;
		system			= nullptr;
		guiContext		= nullptr;
		guiRoot			= nullptr;
		mouseCursor		= nullptr;
		scheme			= nullptr;
		topControl		= nullptr;
		focusedControl	= nullptr;
		movingWindow	= nullptr;
		keyDown			= CEGUI::Key::Scan::Unknown;
		keyChar			= CEGUI::Key::Scan::Unknown;
		tickKeyRepeat	= 0;
		processingInput = false;
		fastKeyRepeat	= false;
	};

	void ControllerUI::Initialize(::Ogre::RenderTarget* Target)
	{		
		if (IsInitialized)
			return;
		
		// init UI
		renderer	= &::CEGUI::OgreRenderer::bootstrapSystem(*Target);
		system		= ::CEGUI::System::getSingletonPtr();
		guiContext	= &system->getDefaultGUIContext();
		mouseCursor = &guiContext->getMouseCursor();

		// load resource to ogre
		::Ogre::ResourceGroupManager* resMan = 
			::Ogre::ResourceGroupManager::getSingletonPtr();

		// create resource groups
		if (!resMan->resourceGroupExists(UI_RESGROUP_IMAGESETS))
			resMan->createResourceGroup(UI_RESGROUP_IMAGESETS);
		
		if (!resMan->resourceGroupExists(UI_RESGROUP_FONTS))
			resMan->createResourceGroup(UI_RESGROUP_FONTS);
		
		if (!resMan->resourceGroupExists(UI_RESGROUP_SCHEMES))
			resMan->createResourceGroup(UI_RESGROUP_SCHEMES);
		
		if (!resMan->resourceGroupExists(UI_RESGROUP_LOOKNFEEL))
			resMan->createResourceGroup(UI_RESGROUP_LOOKNFEEL);
		
		if (!resMan->resourceGroupExists(UI_RESGROUP_LAYOUTS))
			resMan->createResourceGroup(UI_RESGROUP_LAYOUTS);

		::System::String^ baseFolder = ::System::IO::Path::Combine(
			OgreClient::Singleton->Config->ResourcesPath,
			UI_RESOURCESUBFOLDER);

		::System::String^ imageFolder		= ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_IMAGESETS);
		::System::String^ fontsFolder		= ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_FONTS);
		::System::String^ schemesFolder		= ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_SCHEMES);
		::System::String^ looknfeelFolder	= ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_LOOKNFEEL);
		::System::String^ layoutsFolder		= ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_LAYOUTS);

		resMan->addResourceLocation(StringConvert::CLRToOgre(imageFolder), "FileSystem", UI_RESGROUP_IMAGESETS);
		resMan->addResourceLocation(StringConvert::CLRToOgre(fontsFolder), "FileSystem", UI_RESGROUP_FONTS);
		resMan->addResourceLocation(StringConvert::CLRToOgre(schemesFolder), "FileSystem", UI_RESGROUP_SCHEMES);
		resMan->addResourceLocation(StringConvert::CLRToOgre(looknfeelFolder), "FileSystem", UI_RESGROUP_LOOKNFEEL);
		resMan->addResourceLocation(StringConvert::CLRToOgre(layoutsFolder), "FileSystem", UI_RESGROUP_LAYOUTS);

		resMan->initialiseResourceGroup(UI_RESGROUP_IMAGESETS);
		resMan->initialiseResourceGroup(UI_RESGROUP_FONTS);
		resMan->initialiseResourceGroup(UI_RESGROUP_SCHEMES);
		resMan->initialiseResourceGroup(UI_RESGROUP_LOOKNFEEL);
		resMan->initialiseResourceGroup(UI_RESGROUP_LAYOUTS);
				
		resMan->loadResourceGroup(UI_RESGROUP_IMAGESETS);
		resMan->loadResourceGroup(UI_RESGROUP_FONTS);
		resMan->loadResourceGroup(UI_RESGROUP_SCHEMES);
		resMan->loadResourceGroup(UI_RESGROUP_LOOKNFEEL);
		resMan->loadResourceGroup(UI_RESGROUP_LAYOUTS);

		// set UI resourcegroups
		CEGUI::ImageManager::setImagesetDefaultResourceGroup(UI_RESGROUP_IMAGESETS);
		CEGUI::Font::setDefaultResourceGroup(UI_RESGROUP_FONTS);
		CEGUI::Scheme::setDefaultResourceGroup(UI_RESGROUP_SCHEMES);
		CEGUI::WidgetLookManager::setDefaultResourceGroup(UI_RESGROUP_LOOKNFEEL);
		CEGUI::WindowManager::setDefaultResourceGroup(UI_RESGROUP_LAYOUTS);
		
		// load scheme
		scheme = &CEGUI::SchemeManager::getSingleton().createFromFile(UI_FILE_SCHEME);
		
		// load layout/rootelement
		guiRoot = CEGUI::WindowManager::getSingleton().loadLayoutFromFile(UI_FILE_LAYOUT); 
		guiRoot->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::OnRootClicked));
		guiRoot->subscribeEvent(CEGUI::Window::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnRootKeyDown));

		// set mouse defaultcursor image
		mouseCursor->setDefaultImage(UI_DEFAULTARROW);		

		// set settings on guiContext
		guiContext->setDefaultTooltipType(UI_DEFAULTTOOLTIP);
		guiContext->setDefaultFont(UI_DEFAULTFONT);
		guiContext->setRootWindow(guiRoot);
		guiContext->setMouseButtonClickTimeout(UI_MOUSE_SINGLECLICKTIMEOUT);

		// setup children
		LoadingBar::Initialize();
		Welcome::Initialize();
		StatusBar::Initialize();
		OnlinePlayers::Initialize();
		RoomObjects::Initialize();
		Chat::Initialize();
		Avatar::Initialize();
		ObjectDetails::Initialize();
		PlayerDetails::Initialize();
		Target::Initialize();
		SplashNotifier::Initialize();
		MiniMap::Initialize();
		RoomEnchantments::Initialize();
		Buy::Initialize();
		Attributes::Initialize();
		Skills::Initialize();
		Spells::Initialize();
		Actions::Initialize();
		Inventory::Initialize();
		MainButtonsLeft::Initialize();
		MainButtonsRight::Initialize();
		Amount::Initialize();
		Trade::Initialize();
		ActionButtons::Initialize();
		NewsGroup::Initialize();
		NewsGroupCompose::Initialize();
		Mail::Initialize();
		MailCompose::Initialize();
		Guild::Initialize();
		GuildCreate::Initialize();
		AvatarCreateWizard::Initialize();
		ConfirmPopup::Initialize();
		PlayerOverlays::Initialize();
		ObjectContents::Initialize();
		Login::Initialize();
		Options::Initialize();

		// attach listener to Data
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
      
		// mark initialized
		IsInitialized = true;
	};

	void ControllerUI::Destroy()
	{
		if (!IsInitialized)
			return;

		// detach listener
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
      
		// destroy children
		LoadingBar::Destroy();
		Welcome::Destroy();
		StatusBar::Destroy();
		OnlinePlayers::Destroy();
		RoomObjects::Destroy();
		Chat::Destroy();
		Avatar::Destroy();
		ObjectDetails::Destroy();
		PlayerDetails::Destroy();
		Target::Destroy();
		SplashNotifier::Destroy();
		MiniMap::Destroy();
		RoomEnchantments::Destroy();
		Buy::Destroy();
		Attributes::Destroy();
		Skills::Destroy();
		Spells::Destroy();
		Actions::Destroy();
		Inventory::Destroy();
		MainButtonsLeft::Destroy();
		MainButtonsRight::Destroy();
		Amount::Destroy();
		Trade::Destroy();
		ActionButtons::Destroy();
		NewsGroup::Destroy();
		NewsGroupCompose::Destroy();
		Mail::Destroy();
		MailCompose::Destroy();
		Guild::Destroy();
		GuildCreate::Destroy();
		AvatarCreateWizard::Destroy();
		ConfirmPopup::Destroy();
		PlayerOverlays::Destroy();
		ObjectContents::Destroy();
		Login::Destroy();
		Options::Destroy();

		// destroy cegui system
		renderer->destroySystem();
		
		renderer		= nullptr;
		system			= nullptr;
		guiContext		= nullptr;
		guiRoot			= nullptr;
		mouseCursor		= nullptr;
		scheme			= nullptr;
		topControl		= nullptr;
		focusedControl	= nullptr;
		movingWindow	= nullptr;
		keyDown			= CEGUI::Key::Scan::Unknown;
		keyChar			= CEGUI::Key::Scan::Unknown;
		tickKeyRepeat	= 0;
		processingInput = false;
		fastKeyRepeat	= false;

		// mark not initialized
		IsInitialized = false;
	};

	void ControllerUI::Tick(long long Tick, long long Span)
	{
		if (!IsInitialized)
			return;

		if (guiRoot)
		{
			focusedControl = guiRoot->getActiveChild();
			processingInput = false;

			if (!focusedControl)
			{
				guiRoot->activate();
				focusedControl = guiRoot;
			}
			else
			{			
				const CEGUI::String type = focusedControl->getType();

				processingInput = 
					(type.compare(UI_WINDOWTYPE_EDITBOX) == 0) ||
					(type.compare(UI_WINDOWTYPE_MULTILINEEDITBOX) == 0) ||
					(type.compare(UI_WINDOWTYPE_BUTTON) == 0);
			}
		}

		if (system && guiContext)
		{	
			// inject timepulses (BOTH required)
			// CEGUI wants a fraction of seconds, Span is ms
			system->injectTimePulse((float)Span / 1000.0f);
			guiContext->injectTimePulse((float)Span / 1000.0f);

			// update mouseclick executor
			Inventory::Update();
			Chat::Tick(Tick, Span);

			long long delta = OgreClient::Singleton->GameTick->Current - tickKeyRepeat;

			// keyrepeat
			if (keyDown != CEGUI::Key::Scan::Unknown &&
				((!fastKeyRepeat && delta >= KEYREPEATINTERVALDELAYMS) ||
				(fastKeyRepeat && delta >= KEYREPEATINTERVALMS)))
			{	
				// mark for shorther keyrepeat once delay elapsed/first run
				fastKeyRepeat = true;

				guiContext->injectKeyUp(keyDown);
				guiContext->injectKeyDown(keyDown);

				if (keyChar != CEGUI::Key::Scan::Unknown)
					guiContext->injectChar(keyChar);

				// update repeat tick
				tickKeyRepeat = OgreClient::Singleton->GameTick->Current;
			}
		}
	};

	void ControllerUI::ToggleVisibility(::CEGUI::Window* Window)
	{
		if (Window != nullptr)
		{
			if (Window->isVisible())
				Window->hide();
			
			else
			{
				Window->show();
				Window->moveToFront();
			}
		}
	};

	void ControllerUI::ActivateRoot()
	{
		CEGUI::Window* wnd = guiRoot->getActiveChild();
		
		if (!wnd)
			guiRoot->activate();

		else
		{
			while (wnd && wnd != ControllerUI::GUIRoot)
			{
				wnd->deactivate();
				wnd = guiRoot->getActiveChild();
			}

			if (!wnd)
				guiRoot->activate();
		}
	};

	void ControllerUI::PasteFromClipboard(::CEGUI::Window* EditBox)
	{
		if (!EditBox || !::System::Windows::Forms::Clipboard::ContainsText())
			return;

		// get text from clipboard
		::System::String^ clipText = ::System::Windows::Forms::Clipboard::GetText(
			::System::Windows::Forms::TextDataFormat::Text);
		
		// no text
		if (!clipText || clipText->Length == 0)
			return;
		
		// get possibly typed instances we process
		CEGUI::Editbox* box				= dynamic_cast<CEGUI::Editbox*>(EditBox);
		CEGUI::MultiLineEditbox* mlbox	= dynamic_cast<CEGUI::MultiLineEditbox*>(EditBox);
		
		// type of Editbox
		if (box && !box->isReadOnly())
		{					
			// replace newline characters
			clipText = clipText->Replace(
				::System::Environment::NewLine, ::System::String::Empty);

			// get caretindex
			size_t caretindex = box->getCaretIndex();

			// insert new text
			box->insertText(StringConvert::CLRToCEGUI(clipText), caretindex);

			// set caret at the end of inserted text
			box->setCaretIndex(caretindex + clipText->Length);
		}

		// type of MultiLineEditbox
		else if (mlbox && !mlbox->isReadOnly())
		{
			// get caretindex
			size_t caretindex = mlbox->getCaretIndex();

			// insert new text
			mlbox->insertText(StringConvert::CLRToCEGUI(clipText), caretindex);

			// set caret at the end of inserted text
			mlbox->setCaretIndex(caretindex + clipText->Length);
		}
	};
	
	void ControllerUI::CopyToClipboard(::CEGUI::Window* EditBox, bool Cut)
	{
		if (!EditBox)
			return;

		// get possibly typed instances we process
		CEGUI::Editbox* box				= dynamic_cast<CEGUI::Editbox*>(EditBox);
		CEGUI::MultiLineEditbox* mlbox	= dynamic_cast<CEGUI::MultiLineEditbox*>(EditBox);
		
		// type of Editbox
		if (box)
		{						
			// get text from cegui textbox
			::CEGUI::String boxStr = box->getText();

			size_t start = box->getSelectionStartIndex();
			size_t len = box->getSelectionLength();

			// get substring which is selected
			boxStr = boxStr.substr(start, len);
			
			// copy to clipboard
			if (boxStr.length() > 0)
			{
				::System::Windows::Forms::Clipboard::SetText(StringConvert::CEGUIToCLR(boxStr));
			
				if (Cut)			
					box->eraseSelectedText();
			}
		}

		// type of MultiLineEditbox
		else if (mlbox)
		{
			// get text from cegui textbox
			::CEGUI::String boxStr = mlbox->getText();

			size_t start = mlbox->getSelectionStartIndex();
			size_t len = mlbox->getSelectionLength();

			// get substring which is selected
			boxStr = boxStr.substr(start, len);
			
			// copy to clipboard
			if (boxStr.length() > 0)
			{
				::System::Windows::Forms::Clipboard::SetText(StringConvert::CEGUIToCLR(boxStr));
			
				if (Cut)			
					mlbox->eraseSelectedText();
			}
		}	
	};

	void ControllerUI::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{		
		if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_UIMODE))
		{
			UIMode mode = OgreClient::Singleton->Data->UIMode;

			// set controls to default visibility for this mode
			LoadingBar::Window->setVisible(mode == UIMode::LoadingBar);
			Welcome::Window->setVisible(mode == UIMode::AvatarSelection);
			StatusBar::Window->setVisible(mode == UIMode::Playing);
			OnlinePlayers::Window->setVisible(false);
			RoomObjects::Window->setVisible(false);
			Chat::Window->setVisible(mode == UIMode::Playing);
			Avatar::Window->setVisible(mode == UIMode::Playing);
			ObjectDetails::Window->setVisible(false);
			PlayerDetails::Window->setVisible(false);
			Target::Window->setVisible(false);
			RoomEnchantments::Window->setVisible(mode == UIMode::Playing);
			MiniMap::Window->setVisible(mode == UIMode::Playing);
			Buy::Window->setVisible(false);
			Attributes::Window->setVisible(false);
			Actions::Window->setVisible(false);
			Spells::Window->setVisible(false);
			Skills::Window->setVisible(false);
			Inventory::Window->setVisible(false);
			MainButtonsLeft::Window->setVisible(mode == UIMode::Playing);
			MainButtonsRight::Window->setVisible(mode == UIMode::Playing);
			Amount::Window->setVisible(false);
			Trade::Window->setVisible(false);
			ActionButtons::Window->setVisible(mode == UIMode::Playing);
			NewsGroup::Window->setVisible(false);
			NewsGroupCompose::Window->setVisible(false);
			Mail::Window->setVisible(false);
			MailCompose::Window->setVisible(false);
			Guild::Window->setVisible(false);
			GuildCreate::Window->setVisible(false);
			AvatarCreateWizard::Window->setVisible(mode == UIMode::AvatarCreation);
			ConfirmPopup::Window->setVisible(false);
			ObjectContents::Window->setVisible(false);
			//Login::Window->setVisible(mode == UIMode::Login);
			Options::Window->setVisible(false);
		}
	};

#pragma region Input injection

	void ControllerUI::InjectMousePosition(float x, float y)
	{
		CEGUI::Vector2f oldMousePos = mouseCursor->getPosition();

		float dx = x - oldMousePos.d_x;
		float dy = y - oldMousePos.d_y;

		if (guiContext != nullptr)
			guiContext->injectMousePosition(x, y);
		
		// set current top level control
		if (guiRoot != nullptr)		
			topControl = guiRoot->getChildAtPosition(::CEGUI::Vector2f(x, y));

		// move grabbed window (hanldes moves on non FrameWindow widgets)
		if (movingWindow != nullptr)
		{			
			::CEGUI::UVector2 moveUV(cegui_absdim(dx), cegui_absdim(dy));
			movingWindow->setPosition(movingWindow->getPosition() + moveUV);
		}
	};

	void ControllerUI::InjectMouseWheelChange(float z)
	{
		if (guiContext != nullptr && z != 0)
			guiContext->injectMouseWheelChange(z);
	};

	void ControllerUI::InjectMouseButtonDown(::CEGUI::MouseButton Button)
	{
		if (guiContext != nullptr)		
			guiContext->injectMouseButtonDown(Button);
		
		// hide mouse if not went down on our UI
		if (mouseCursor != nullptr && IgnoreTopControlForMouseInput)
			mouseCursor->hide();
	};

	void ControllerUI::InjectMouseButtonUp(::CEGUI::MouseButton Button)
	{
		if (guiContext != nullptr)
			guiContext->injectMouseButtonUp(Button);

		if (mouseCursor != nullptr && ControllerInput::IsMouseInWindow)
			mouseCursor->show();
	};

	void ControllerUI::InjectKeyDown(::CEGUI::Key::Scan Key)
	{
		keyDown = Key;
		tickKeyRepeat = OgreClient::Singleton->GameTick->Current;
		
		if (guiContext != nullptr)
			guiContext->injectKeyDown(Key);
	};

	void ControllerUI::InjectKeyUp(::CEGUI::Key::Scan Key)
	{
		keyDown = CEGUI::Key::Scan::Unknown;
		keyChar = CEGUI::Key::Scan::Unknown;
		fastKeyRepeat = false;

		if (guiContext != nullptr)
			guiContext->injectKeyUp(Key);
	};

	void ControllerUI::InjectChar(::CEGUI::Key::Scan Key)
	{
		keyChar = Key;

		if (guiContext != nullptr)
			guiContext->injectChar(Key);
	};
	
#pragma endregion

#pragma region UI callbacks
	bool UICallbacks::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);

		args.window->hide();
		ControllerUI::ActivateRoot();

		return true;
	};

	bool UICallbacks::OnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			args.window->hide();

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	};
	
	bool UICallbacks::OnRootClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		ControllerUI::ActivateRoot();

		return true;
	};

	bool UICallbacks::OnRootKeyDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Return ||
			args.scancode == CEGUI::Key::NumpadEnter)
		{
			ControllerUI::Chat::Window->setVisible(true);
			ControllerUI::Chat::Input->activate();
		}

		return true;
	};

	bool UICallbacks::OnCopyPasteKeyDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
		bool handled					= false;
		
		switch(args.scancode)
		{			
			// paste clipboard to textbox				
			case CEGUI::Key::Scan::V:				
				if (args.sysKeys == ::CEGUI::SystemKey::Control)
				{
					ControllerUI::PasteFromClipboard(args.window);
					handled = true;
				}
				break;

			// copy text to clipboard
			case CEGUI::Key::Scan::C:			
				if (args.sysKeys == ::CEGUI::SystemKey::Control)
				{
					ControllerUI::CopyToClipboard(args.window, false);
					handled = true;
				}
				break;

			// cut text to clipboard
			case CEGUI::Key::Scan::X:			
				if (args.sysKeys == ::CEGUI::SystemKey::Control)
				{
					ControllerUI::CopyToClipboard(args.window, true);
					handled = true;
				}
				break;
		}	

		return handled;
	};
#pragma endregion
};};