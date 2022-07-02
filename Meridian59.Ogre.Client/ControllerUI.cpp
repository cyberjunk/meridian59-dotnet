#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   static ControllerUI::ControllerUI(void)
   {
      renderer        = nullptr;
      system          = nullptr;
      guiContext      = nullptr;
      guiRoot         = nullptr;
      mouseCursor     = nullptr;
      scheme          = nullptr;
      topControl      = nullptr;
      focusedControl  = nullptr;
      movingWindow    = nullptr;
      keyDown         = CEGUI::Key::Scan::Unknown;
      keyChar         = CEGUI::Key::Scan::Unknown;
      processingInput = false;
      fastKeyRepeat   = false;
   };

   void ControllerUI::Initialize(::Ogre::RenderTarget* Target)
   {
      if (IsInitialized)
         return;

      // init UI
      renderer    = &::CEGUI::OgreRenderer::bootstrapSystem(*Target);
      system      = ::CEGUI::System::getSingletonPtr();
      guiContext  = &system->getDefaultGUIContext();
      mouseCursor = &guiContext->getMouseCursor();

      renderer->setUsingShaders(true);

      // load resource to ogre
      ::Ogre::ResourceGroupManager& resMan = ::Ogre::ResourceGroupManager::getSingleton();

      // create resource groups
      if (!resMan.resourceGroupExists(UI_RESGROUP_IMAGESETS))
         resMan.createResourceGroup(UI_RESGROUP_IMAGESETS);

      if (!resMan.resourceGroupExists(UI_RESGROUP_FONTS))
         resMan.createResourceGroup(UI_RESGROUP_FONTS);

      if (!resMan.resourceGroupExists(UI_RESGROUP_SCHEMES))
         resMan.createResourceGroup(UI_RESGROUP_SCHEMES);

      if (!resMan.resourceGroupExists(UI_RESGROUP_LOOKNFEEL))
         resMan.createResourceGroup(UI_RESGROUP_LOOKNFEEL);

      if (!resMan.resourceGroupExists(UI_RESGROUP_LAYOUTS))
         resMan.createResourceGroup(UI_RESGROUP_LAYOUTS);

      CLRString^ baseFolder = ::System::IO::Path::Combine(
         OgreClient::Singleton->Config->ResourcesPath,
         UI_RESOURCESUBFOLDER);

      CLRString^ imageFolder      = ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_IMAGESETS);
      CLRString^ fontsFolder      = ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_FONTS);
      CLRString^ schemesFolder    = ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_SCHEMES);
      CLRString^ looknfeelFolder  = ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_LOOKNFEEL);
      CLRString^ layoutsFolder    = ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_LAYOUTS);
      CLRString^ animationsFolder = ::System::IO::Path::Combine(baseFolder, UI_RESGROUP_ANIMATIONS);

      resMan.addResourceLocation(StringConvert::CLRToOgre(imageFolder), "FileSystem", UI_RESGROUP_IMAGESETS);
      resMan.addResourceLocation(StringConvert::CLRToOgre(fontsFolder), "FileSystem", UI_RESGROUP_FONTS);
      resMan.addResourceLocation(StringConvert::CLRToOgre(schemesFolder), "FileSystem", UI_RESGROUP_SCHEMES);
      resMan.addResourceLocation(StringConvert::CLRToOgre(looknfeelFolder), "FileSystem", UI_RESGROUP_LOOKNFEEL);
      resMan.addResourceLocation(StringConvert::CLRToOgre(layoutsFolder), "FileSystem", UI_RESGROUP_LAYOUTS);
      resMan.addResourceLocation(StringConvert::CLRToOgre(animationsFolder), "FileSystem", UI_RESGROUP_ANIMATIONS);

      resMan.initialiseResourceGroup(UI_RESGROUP_IMAGESETS);
      resMan.initialiseResourceGroup(UI_RESGROUP_FONTS);
      resMan.initialiseResourceGroup(UI_RESGROUP_SCHEMES);
      resMan.initialiseResourceGroup(UI_RESGROUP_LOOKNFEEL);
      resMan.initialiseResourceGroup(UI_RESGROUP_LAYOUTS);
      resMan.initialiseResourceGroup(UI_RESGROUP_ANIMATIONS);

      resMan.loadResourceGroup(UI_RESGROUP_IMAGESETS);
      resMan.loadResourceGroup(UI_RESGROUP_FONTS);
      resMan.loadResourceGroup(UI_RESGROUP_SCHEMES);
      resMan.loadResourceGroup(UI_RESGROUP_LOOKNFEEL);
      resMan.loadResourceGroup(UI_RESGROUP_LAYOUTS);
      resMan.loadResourceGroup(UI_RESGROUP_ANIMATIONS);

      // set UI resourcegroups
      CEGUI::ImageManager::setImagesetDefaultResourceGroup(UI_RESGROUP_IMAGESETS);
      CEGUI::Font::setDefaultResourceGroup(UI_RESGROUP_FONTS);
      CEGUI::Scheme::setDefaultResourceGroup(UI_RESGROUP_SCHEMES);
      CEGUI::WidgetLookManager::setDefaultResourceGroup(UI_RESGROUP_LOOKNFEEL);
      CEGUI::WindowManager::setDefaultResourceGroup(UI_RESGROUP_LAYOUTS);
      CEGUI::AnimationManager::setDefaultResourceGroup(UI_RESGROUP_ANIMATIONS);

      // load animations
      CEGUI::AnimationManager::getSingleton().loadAnimationsFromXML(UI_FILE_ANIMATIONS, UI_RESGROUP_ANIMATIONS);

      // load scheme
      scheme = &CEGUI::SchemeManager::getSingleton().createFromFile(UI_FILE_SCHEME, UI_RESGROUP_SCHEMES);

      // load layout/rootelement
      guiRoot = CEGUI::WindowManager::getSingleton().loadLayoutFromFile(UI_FILE_LAYOUT, UI_RESGROUP_LAYOUTS); 
      guiRoot->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::OnRootMouseDown));
      guiRoot->subscribeEvent(CEGUI::Window::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnRootKeyUp));

      // set mouse defaultcursor image
      mouseCursor->setDefaultImage(UI_DEFAULTARROW);

      // set settings on guiContext
      guiContext->setDefaultTooltipType(UI_DEFAULTTOOLTIP);
      guiContext->setDefaultFont(UI_DEFAULTFONT);
      guiContext->setRootWindow(guiRoot);
      guiContext->setMouseButtonClickTimeout(UI_MOUSE_SINGLECLICKTIMEOUT);

      // setup children
      LoadingBar::Initialize();
      Branding::Initialize();
      DownloadBar::Initialize();
      Welcome::Initialize();
      StatusBar::Initialize();
      OnlinePlayers::Initialize();
      RoomObjects::Initialize();
      Chat::Initialize();
      Avatar::Initialize();
      ObjectDetails::Initialize();
      PlayerDetails::Initialize();
      SpellDetails::Initialize();
      SkillDetails::Initialize();
      Target::Initialize();
      NPCQuestList::Initialize();
      SplashNotifier::Initialize();
      MiniMap::Initialize();
      RoomEnchantments::Initialize();
      Buy::Initialize();
      Attributes::Initialize();
      Skills::Initialize();
      Spells::Initialize();
      Quests::Initialize();
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
      GuildHallBuy::Initialize();
      AvatarCreateWizard::Initialize();
      StatChangeWizard::Initialize();
      ConfirmPopup::Initialize();
      PlayerOverlays::Initialize();
      ObjectContents::Initialize();
      LootList::Initialize();
      Login::Initialize();
      Options::Initialize();
      Stats::Initialize();

      // attach listener to Data
      OgreClient::Singleton->Data->PropertyChanged += 
         gcnew PropertyChangedEventHandler(OnDataPropertyChanged);

      // mark initialized
      IsInitialized = true;

      // apply language
      ApplyLanguage();

      // apply lockstate
      ApplyLock();
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
      Branding::Destroy();
      DownloadBar::Destroy();
      Welcome::Destroy();
      StatusBar::Destroy();
      OnlinePlayers::Destroy();
      RoomObjects::Destroy();
      Chat::Destroy();
      Avatar::Destroy();
      ObjectDetails::Destroy();
      PlayerDetails::Destroy();
      SpellDetails::Destroy();
      SkillDetails::Destroy();
      Target::Destroy();
      NPCQuestList::Destroy();
      SplashNotifier::Destroy();
      MiniMap::Destroy();
      RoomEnchantments::Destroy();
      Buy::Destroy();
      Attributes::Destroy();
      Skills::Destroy();
      Spells::Destroy();
      Quests::Destroy();
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
      GuildHallBuy::Destroy();
      AvatarCreateWizard::Destroy();
      StatChangeWizard::Destroy();
      ConfirmPopup::Destroy();
      PlayerOverlays::Destroy();
      ObjectContents::Destroy();
      LootList::Destroy();
      Login::Destroy();
      Options::Destroy();
      Stats::Destroy();

      // destroy cegui system
      renderer->destroySystem();

      renderer        = nullptr;
      system          = nullptr;
      guiContext      = nullptr;
      guiRoot         = nullptr;
      mouseCursor     = nullptr;
      scheme          = nullptr;
      topControl      = nullptr;
      focusedControl  = nullptr;
      movingWindow    = nullptr;
      keyDown         = CEGUI::Key::Scan::Unknown;
      keyChar         = CEGUI::Key::Scan::Unknown;
      processingInput = false;
      fastKeyRepeat   = false;

      // mark not initialized
      IsInitialized = false;
   };

   void ControllerUI::ApplyLanguage()
   {
      if (!IsInitialized)
         return;

      // apply on all sub-controls
      LoadingBar::ApplyLanguage();
      DownloadBar::ApplyLanguage();
      Welcome::ApplyLanguage();
      StatusBar::ApplyLanguage();
      OnlinePlayers::ApplyLanguage();
      RoomObjects::ApplyLanguage();
      Chat::ApplyLanguage();
      Avatar::ApplyLanguage();
      ObjectDetails::ApplyLanguage();
      PlayerDetails::ApplyLanguage();
      SpellDetails::ApplyLanguage();
      SkillDetails::ApplyLanguage();
      Target::ApplyLanguage();
      NPCQuestList::ApplyLanguage();
      SplashNotifier::ApplyLanguage();
      MiniMap::ApplyLanguage();
      RoomEnchantments::ApplyLanguage();
      Buy::ApplyLanguage();
      Attributes::ApplyLanguage();
      Skills::ApplyLanguage();
      Spells::ApplyLanguage();
      Quests::ApplyLanguage();
      Actions::ApplyLanguage();
      Inventory::ApplyLanguage();
      MainButtonsLeft::ApplyLanguage();
      MainButtonsRight::ApplyLanguage();
      Amount::ApplyLanguage();
      Trade::ApplyLanguage();
      ActionButtons::ApplyLanguage();
      NewsGroup::ApplyLanguage();
      NewsGroupCompose::ApplyLanguage();
      Mail::ApplyLanguage();
      MailCompose::ApplyLanguage();
      Guild::ApplyLanguage();
      GuildCreate::ApplyLanguage();
      GuildHallBuy::ApplyLanguage();
      AvatarCreateWizard::ApplyLanguage();
      StatChangeWizard::ApplyLanguage();
      ConfirmPopup::ApplyLanguage();
      PlayerOverlays::ApplyLanguage();
      ObjectContents::ApplyLanguage();
      LootList::ApplyLanguage();
      Login::ApplyLanguage();
      Options::ApplyLanguage();
      Stats::ApplyLanguage();
   };

   void ControllerUI::ApplyLock()
   {
      const bool LOCKED = OgreClient::Singleton->Config->UILocked;
      const size_t ACTIONBUTTONS = ActionButtons::Grid->getChildCount();

      // reset any moving window
      movingWindow = nullptr;

      if (LOCKED)
      {
         // non-framewindow elements
         Avatar::Window->setMouseCursor(UI_DEFAULTARROW);
         Target::Window->setMouseCursor(UI_DEFAULTARROW);
         RoomEnchantments::Window->setMouseCursor(UI_DEFAULTARROW);

         // switch lock image
         StatusBar::Lock->setProperty(UI_PROPNAME_NORMALIMAGE, UI_IMAGE_LOCKBUTTON_LOCKED_NORMAL);
         StatusBar::Lock->setProperty(UI_PROPNAME_HOVERIMAGE, UI_IMAGE_LOCKBUTTON_LOCKED_HOVER);
         StatusBar::Lock->setProperty(UI_PROPNAME_PUSHEDIMAGE, UI_IMAGE_LOCKBUTTON_LOCKED_PUSHED);

         // update tooltips
         StatusBar::Lock->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UILOCKED_TOOLTIP));
         StatusBar::Reset->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UIRESETLOCKED_TOOLTIP));

         // disable dragging of actionbuttons
         for (size_t i = 0; i < ACTIONBUTTONS; i++)
            ((CEGUI::DragContainer*)ActionButtons::Grid->getChildAtIdx(i))->setDraggingEnabled(false);

         // framewindows

         Inventory::Window->setDragMovingEnabled(false);
         Inventory::Window->setSizingEnabled(false);
         Inventory::Window->getTitlebar()->setMouseCursor(UI_DEFAULTARROW);

         Chat::Window->setDragMovingEnabled(false);
         Chat::Window->setSizingEnabled(false);
         Chat::Window->getTitlebar()->setMouseCursor(UI_DEFAULTARROW);

         OnlinePlayers::Window->setDragMovingEnabled(false);
         OnlinePlayers::Window->setSizingEnabled(false);
         OnlinePlayers::Window->getTitlebar()->setMouseCursor(UI_DEFAULTARROW);

         RoomObjects::Window->setDragMovingEnabled(false);
         RoomObjects::Window->setSizingEnabled(false);
         RoomObjects::Window->getTitlebar()->setMouseCursor(UI_DEFAULTARROW);
      }
      else
      {
         // non-framewindow elements
         Avatar::Window->setMouseCursor(UI_MOUSECURSOR_DRAG);
         Target::Window->setMouseCursor(UI_MOUSECURSOR_DRAG);
         RoomEnchantments::Window->setMouseCursor(UI_MOUSECURSOR_DRAG);

         // switch lock image
         StatusBar::Lock->setProperty(UI_PROPNAME_NORMALIMAGE, UI_IMAGE_LOCKBUTTON_UNLOCKED_NORMAL);
         StatusBar::Lock->setProperty(UI_PROPNAME_HOVERIMAGE, UI_IMAGE_LOCKBUTTON_UNLOCKED_HOVER);
         StatusBar::Lock->setProperty(UI_PROPNAME_PUSHEDIMAGE, UI_IMAGE_LOCKBUTTON_UNLOCKED_PUSHED);

         // update tooltips
         StatusBar::Lock->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UIUNLOCKED_TOOLTIP));
         StatusBar::Reset->setTooltipText(GetLangTooltipStatusBar(LANGSTR_TOOLTIP_STATUSBAR::UIRESET_TOOLTIP));

         // enable dragging of actionbuttons
         for (size_t i = 0; i < ACTIONBUTTONS; i++)
            ((CEGUI::DragContainer*)ActionButtons::Grid->getChildAtIdx(i))->setDraggingEnabled(true);

         // framewindows

         Inventory::Window->setDragMovingEnabled(true);
         Inventory::Window->setSizingEnabled(true);
         Inventory::Window->getTitlebar()->setMouseCursor(UI_MOUSECURSOR_DRAG);

         Chat::Window->setDragMovingEnabled(true);
         Chat::Window->setSizingEnabled(true);
         Chat::Window->getTitlebar()->setMouseCursor(UI_MOUSECURSOR_DRAG);

         OnlinePlayers::Window->setDragMovingEnabled(true);
         OnlinePlayers::Window->setSizingEnabled(true);
         OnlinePlayers::Window->getTitlebar()->setMouseCursor(UI_MOUSECURSOR_DRAG);

         RoomObjects::Window->setDragMovingEnabled(true);
         RoomObjects::Window->setSizingEnabled(true);
         RoomObjects::Window->getTitlebar()->setMouseCursor(UI_MOUSECURSOR_DRAG);
      }
   };

   void ControllerUI::Tick(double Tick, double Span)
   {
      if (!IsInitialized)
         return;

      if (guiRoot)
      {
         focusedControl = guiRoot->getActiveChild();
         processingInput = false;

         // 1) Null focus, make sure the root is the focused window
         if (!focusedControl)
         {
            guiRoot->activate();
            focusedControl = guiRoot;
         }

         // 2) A subwindow is selected which consumes all input
         else if (IsRecursiveChildOf(focusedControl, Spells::Window)
            || IsRecursiveChildOf(focusedControl, Skills::Window))
            processingInput = true;

         // 3) Specifi UI elements always consume all input (like textboxes)
         else
         {
            const CEGUI::String& type = focusedControl->getType();

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

         // tick sub components
         Inventory::Tick(Tick, Span);
         Chat::Tick(Tick, Span);

         // keyrepeat
         if (keyDown != CEGUI::Key::Scan::Unknown &&
             keyDown != CEGUI::Key::Scan::LeftShift &&
             keyDown != CEGUI::Key::Scan::RightShift &&
             keyDown != CEGUI::Key::Scan::LeftAlt &&
             keyDown != CEGUI::Key::Scan::RightAlt &&
             keyDown != CEGUI::Key::Scan::LeftControl &&
             keyDown != CEGUI::Key::Scan::RightControl &&
            ((!fastKeyRepeat && OgreClient::Singleton->GameTick->CanKeyRepeatStart()) ||
            (fastKeyRepeat && OgreClient::Singleton->GameTick->CanKeyRepeat())))
         {
            // mark for shorther keyrepeat once delay elapsed/first run
            fastKeyRepeat = true;

            guiContext->injectKeyUp(keyDown);
            guiContext->injectKeyDown(keyDown);

            if (keyChar != CEGUI::Key::Scan::Unknown)
               guiContext->injectChar(keyChar);

            // update repeat tick
            OgreClient::Singleton->GameTick->DidKeyRepeat();
         }

         Stats::Tick();

         // tick minimap in playing mode (get image, enqueue next)
         if (OgreClient::Singleton->Data->UIMode == UIMode::Playing && MiniMap::Window->isVisible())
         {
            MiniMapCEGUI::Tick(
               MiniMap::Window,
               MiniMap::DrawSurface,
               MiniMap::Zoom,
               OgreClient::Singleton->Data->RoomObjects);
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

   bool ControllerUI::IsRecursiveChildOf(::CEGUI::Window* Child, ::CEGUI::Window* Parent)
   {
      if (!Child || !Parent)
         return false;

      if (Child == Parent)
         return true;

      while (Child->getParent())	
      {
         if (Child->getParent() == Parent)
            return true;

         Child = Child->getParent();
      }

      return false;
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
      CLRString^ clipText = ::System::Windows::Forms::Clipboard::GetText(
         ::System::Windows::Forms::TextDataFormat::Text);

      // no text
      if (!clipText || clipText->Length == 0)
         return;

      // get possibly typed instances we process
      CEGUI::Editbox* box            = dynamic_cast<CEGUI::Editbox*>(EditBox);
      CEGUI::MultiLineEditbox* mlbox = dynamic_cast<CEGUI::MultiLineEditbox*>(EditBox);

      // type of Editbox
      if (box && !box->isReadOnly())
      {
         // replace newline characters
         clipText = clipText->Replace(
            ::System::Environment::NewLine, CLRString::Empty);

         // shorten clipboard text in case it exceeds maxlen of box
         size_t maxlen = box->getMaxTextLength();
         size_t remain = maxlen - box->getText().length();
         clipText = Common::Util::Truncate(clipText, (int)remain);

         // erase selected part
         box->eraseText(box->getSelectionStartIndex(), box->getSelectionLength());

         // insert new text
         size_t caretindex = box->getCaretIndex();
         box->insertText(StringConvert::CLRToCEGUI(clipText), caretindex);

         // set caret at the end of inserted text
         box->setCaretIndex(caretindex + (size_t)clipText->Length);
      }

      // type of MultiLineEditbox
      else if (mlbox && !mlbox->isReadOnly())
      {
         // shorten clipboard text in case it exceeds maxlen of box
         size_t maxlen = mlbox->getMaxTextLength();
         size_t remain = maxlen - mlbox->getText().length();
         clipText = Common::Util::Truncate(clipText, (int)remain);

         // insert new text
         size_t caretindex = mlbox->getCaretIndex();
         mlbox->insertText(StringConvert::CLRToCEGUI(clipText), caretindex);

         // set caret at the end of inserted text
         mlbox->setCaretIndex(caretindex + (size_t)clipText->Length);
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

   void ControllerUI::ResetLayout()
   {
      OgreClient::Singleton->Config->ResetUIElements();

      // avatar
      Avatar::Window->setPosition(OgreClient::Singleton->Config->UILayoutAvatar->getPosition());
      Avatar::Window->setSize(OgreClient::Singleton->Config->UILayoutAvatar->getSize());

      // target
      Target::Window->setPosition(OgreClient::Singleton->Config->UILayoutTarget->getPosition());
      Target::Window->setSize(OgreClient::Singleton->Config->UILayoutTarget->getSize());

      // minimap
      MiniMap::Window->setPosition(OgreClient::Singleton->Config->UILayoutMinimap->getPosition());
      MiniMap::Window->setSize(OgreClient::Singleton->Config->UILayoutMinimap->getSize());

      // roomenchantments
      RoomEnchantments::Window->setPosition(OgreClient::Singleton->Config->UILayoutRoomEnchantments->getPosition());
      RoomEnchantments::Window->setSize(OgreClient::Singleton->Config->UILayoutRoomEnchantments->getSize());

      // chat
      Chat::Window->setPosition(OgreClient::Singleton->Config->UILayoutChat->getPosition());
      Chat::Window->setSize(OgreClient::Singleton->Config->UILayoutChat->getSize());

      // inventory
      Inventory::Window->setPosition(OgreClient::Singleton->Config->UILayoutInventory->getPosition());
      Inventory::Window->setSize(OgreClient::Singleton->Config->UILayoutInventory->getSize());

      // spells
      Spells::Window->setPosition(OgreClient::Singleton->Config->UILayoutSpells->getPosition());
      Spells::Window->setSize(OgreClient::Singleton->Config->UILayoutSpells->getSize());

      // skills
      Skills::Window->setPosition(OgreClient::Singleton->Config->UILayoutSkills->getPosition());
      Skills::Window->setSize(OgreClient::Singleton->Config->UILayoutSkills->getSize());

      // actions
      Actions::Window->setPosition(OgreClient::Singleton->Config->UILayoutActions->getPosition());
      Actions::Window->setSize(OgreClient::Singleton->Config->UILayoutActions->getSize());

      // attributes
      Attributes::Window->setPosition(OgreClient::Singleton->Config->UILayoutAttributes->getPosition());
      Attributes::Window->setSize(OgreClient::Singleton->Config->UILayoutAttributes->getSize());

      // mainbuttonsleft
      MainButtonsLeft::Window->setPosition(OgreClient::Singleton->Config->UILayoutMainButtonsLeft->getPosition());
      MainButtonsLeft::Window->setSize(OgreClient::Singleton->Config->UILayoutMainButtonsLeft->getSize());

      // mainbuttonsright
      MainButtonsRight::Window->setPosition(OgreClient::Singleton->Config->UILayoutMainButtonsRight->getPosition());
      MainButtonsRight::Window->setSize(OgreClient::Singleton->Config->UILayoutMainButtonsRight->getSize());

      // actionbuttons
      ActionButtons::Window->setPosition(OgreClient::Singleton->Config->UILayoutActionButtons->getPosition());
      ActionButtons::Window->setSize(OgreClient::Singleton->Config->UILayoutActionButtons->getSize());

      // onlineplayers
      OnlinePlayers::Window->setPosition(OgreClient::Singleton->Config->UILayoutOnlinePlayers->getPosition());
      OnlinePlayers::Window->setSize(OgreClient::Singleton->Config->UILayoutOnlinePlayers->getSize());

      // roomobjects
      RoomObjects::Window->setPosition(OgreClient::Singleton->Config->UILayoutRoomObjects->getPosition());
      RoomObjects::Window->setSize(OgreClient::Singleton->Config->UILayoutRoomObjects->getSize());
   };

   void ControllerUI::SaveLayoutToConfig()
   {
      if (!IsInitialized)
         return;

      // avatar
      OgreClient::Singleton->Config->UILayoutAvatar->setPosition(Avatar::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutAvatar->setSize(Avatar::Window->getSize());

      // target
      OgreClient::Singleton->Config->UILayoutTarget->setPosition(Target::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutTarget->setSize(Target::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityTarget = Target::Window->isVisible();

      // minimap
      OgreClient::Singleton->Config->UILayoutMinimap->setPosition(MiniMap::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutMinimap->setSize(MiniMap::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityMiniMap = MiniMap::Window->isVisible();

      // roomenchantments
      OgreClient::Singleton->Config->UILayoutRoomEnchantments->setPosition(RoomEnchantments::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutRoomEnchantments->setSize(RoomEnchantments::Window->getSize());

      // chat
      OgreClient::Singleton->Config->UILayoutChat->setPosition(Chat::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutChat->setSize(Chat::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityChat = Chat::Window->isVisible();

      // inventory
      OgreClient::Singleton->Config->UILayoutInventory->setPosition(Inventory::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutInventory->setSize(Inventory::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityInventory = Inventory::Window->isVisible();

      // spells
      OgreClient::Singleton->Config->UILayoutSpells->setPosition(Spells::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutSpells->setSize(Spells::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilitySpells = Spells::Window->isVisible();

      // skills
      OgreClient::Singleton->Config->UILayoutSkills->setPosition(Skills::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutSkills->setSize(Skills::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilitySkills = Skills::Window->isVisible();

      // actions
      OgreClient::Singleton->Config->UILayoutActions->setPosition(Actions::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutActions->setSize(Actions::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityActions = Actions::Window->isVisible();

      // attributes
      OgreClient::Singleton->Config->UILayoutAttributes->setPosition(Attributes::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutAttributes->setSize(Attributes::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityAttributes = Attributes::Window->isVisible();

      // mainbuttonsleft
      OgreClient::Singleton->Config->UILayoutMainButtonsLeft->setPosition(MainButtonsLeft::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutMainButtonsLeft->setSize(MainButtonsLeft::Window->getSize());

      // mainbuttonsright
      OgreClient::Singleton->Config->UILayoutMainButtonsRight->setPosition(MainButtonsRight::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutMainButtonsRight->setSize(MainButtonsRight::Window->getSize());

      // actionbuttons
      OgreClient::Singleton->Config->UILayoutActionButtons->setPosition(ActionButtons::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutActionButtons->setSize(ActionButtons::Window->getSize());

      // onlineplayers
      OgreClient::Singleton->Config->UILayoutOnlinePlayers->setPosition(OnlinePlayers::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutOnlinePlayers->setSize(OnlinePlayers::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityOnlinePlayers = OnlinePlayers::Window->isVisible();

      // roomobjects
      OgreClient::Singleton->Config->UILayoutRoomObjects->setPosition(RoomObjects::Window->getPosition());
      OgreClient::Singleton->Config->UILayoutRoomObjects->setSize(RoomObjects::Window->getSize());
      OgreClient::Singleton->Config->UIVisibilityRoomObjects = RoomObjects::Window->isVisible();
   };

   void ControllerUI::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      if (CLRString::Equals(e->PropertyName, DataController::PROPNAME_UIMODE))
      {
         UIMode mode = OgreClient::Singleton->Data->UIMode;

         // set controls to default visibility for this mode
         LoadingBar::Window->setVisible(mode == UIMode::LoadingBar);
         Branding::Logo->setVisible(mode == UIMode::Login || mode == UIMode::AvatarSelection || mode == UIMode::AvatarCreation);
         Branding::Text->setVisible(mode == UIMode::Login || mode == UIMode::AvatarSelection || mode == UIMode::AvatarCreation);
         DownloadBar::Window->setVisible(mode == UIMode::Download);
         Welcome::Window->setVisible(mode == UIMode::AvatarSelection);
         StatusBar::Window->setVisible(mode == UIMode::Playing);
         OnlinePlayers::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityOnlinePlayers);
         RoomObjects::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityRoomObjects);
         Chat::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityChat);
         Avatar::Window->setVisible(mode == UIMode::Playing);
         ObjectDetails::Window->setVisible(false);
         PlayerDetails::Window->setVisible(false);
         SpellDetails::Window->setVisible(false);
         SkillDetails::Window->setVisible(false);
         Target::Window->setVisible(false);
         NPCQuestList::Window->setVisible(false);
         RoomEnchantments::Window->setVisible(mode == UIMode::Playing);
         MiniMap::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityMiniMap);
         Buy::Window->setVisible(false);
         Attributes::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityAttributes);
         Actions::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityActions);
         Spells::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilitySpells);
         Skills::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilitySkills);
         Quests::Window->setVisible(false);
         Inventory::Window->setVisible(mode == UIMode::Playing && OgreClient::Singleton->Config->UIVisibilityInventory);
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
         GuildHallBuy::Window->setVisible(false);
         AvatarCreateWizard::Window->setVisible(mode == UIMode::AvatarCreation);
         StatChangeWizard::Window->setVisible(false);
         ConfirmPopup::Window->setVisible(false);
         ObjectContents::Window->setVisible(false);
         LootList::Window->setVisible(false);
         Login::Window->setVisible(mode == UIMode::Login);
         Options::Window->setVisible(false);
         Stats::Window->setVisible(false);

         /////////////////////////////////////////////////////////////////////////////////
         // initial focus settings when changing modes

         if (mode == UIMode::AvatarSelection)
            ControllerUI::Welcome::Avatars->activate();

         if (mode == UIMode::Login)
            ControllerUI::Login::AutoFocus();
      }
   };

   void ControllerUI::SetVUMeterColorFromProgress(::CEGUI::ProgressBar* VUMeter)
   {
      const float MAXVAL = 128.0f;
      const float progress = VUMeter->getProgress();

      // build red and green components
      const float r = (progress <= 0.5f) ? MAXVAL : (float)MathUtil::Bound(MAXVAL - ((progress - 0.5f) * 2.0f * MAXVAL), 0.0f, MAXVAL);
      const float g = (progress >= 0.5f) ? MAXVAL : (float)MathUtil::Bound(progress * 2.0f * MAXVAL, 0.0f, MAXVAL);

      // create colourrect from components
      const CEGUI::Colour colour = CEGUI::Colour(0xFF000000 | (unsigned int)r << 16 | (unsigned int)g << 8);
      const CEGUI::ColourRect colours = CEGUI::ColourRect(colour, colour, colour, colour);

      // set color on property
      VUMeter->setProperty(UI_PROPNAME_BARCOLOURS, CEGUI::PropertyHelper<CEGUI::ColourRect>::toString(colours));
   };

   void ControllerUI::BuildIconAtlas()
   {
      ::CEGUI::ImageManager* imgMan = CEGUI::ImageManager::getSingletonPtr();
      ::Ogre::TextureManager* texMan = Ogre::TextureManager::getSingletonPtr();
      ::Meridian59::Files::ResourceManager^ resMan = OgreClient::Singleton->ResourceManager;

      const char* TEXNAME = "ICONS-16x16";
      const int WIDTH = 256;
      const int HEIGHT = 256;
      const int ICONWIDTH = 16;
      const int ICONHEIGHT = 16;

      // create manual (empty) ogre texture
      TexturePtr texPtr = texMan->createManual(
         TEXNAME,
         UI_RESGROUP_IMAGESETS,
         TextureType::TEX_TYPE_2D,
         WIDTH, HEIGHT, 0,
         ::Ogre::PixelFormat::PF_A8R8G8B8,
         TU_DEFAULT, 0, false, 0);

      // make ogre texture visible to CEGUI
      CEGUI::Texture* mTexture = &renderer->createTexture(TEXNAME, texPtr);

      // get pixelbuffer
      HardwarePixelBufferSharedPtr pixPtr = texPtr->getBuffer();

      // lock it
      unsigned int* pixels = (unsigned int*)pixPtr->lock(HardwareBuffer::LockOptions::HBL_WRITE_ONLY);

      int x = 0;
      int y = 0;

      for each(CLRString^ icon in ResourceStrings::BGF::Icons)
      {
         // try to get bgf file for filename from resource manager
         BgfFile^ bgf = resMan->GetObject(icon);

         // not found or 0 frames
         if (bgf == nullptr || bgf->Frames->Count == 0)
            continue;

         BgfBitmap^ frame = bgf->Frames[0];

         if (frame->Width != ICONWIDTH || frame->Height != ICONHEIGHT)
            continue;

         // build cegui image name for icon
         ::Ogre::String& imgName =
            StringConvert::CLRToOgre(UI_NAMEPREFIX_STATICICON + icon + "/0");

         // already defined
         if (imgMan->isDefined(imgName))
            continue;

         // create cegui image
         CEGUI::BasicImage* img = (CEGUI::BasicImage*)&imgMan->create("BasicImage", imgName);

         // set texture and uv-coords
         img->setTexture(mTexture);
         img->setArea(CEGUI::Rectf((float)x, (float)y, (float)(x + ICONWIDTH), (float)(y + ICONHEIGHT)));

         // fill the pixeldata to buffer
         frame->FillPixelDataAsA8R8G8B8TransparencyBlack(pixels, WIDTH);

         // move to next slot in row
         pixels += 16;
         x += 16;

         // next row
         if (x > WIDTH - 16)
         {
            x = 0;
            y += 16;

            pixels += (ICONHEIGHT - 1) * WIDTH;
         }

         // icon texture can't hold any more
         if (y > HEIGHT - 16)
            break;
      }

      // unlock buffer
      pixPtr->unlock();
   };

   /// <summary>
   /// Calculates the adjusted height of a window containing a MultiLineEditbox
   /// with some text that may not fit inside the box. Useful to resize windows
   /// like object/skill/spell descriptions that have variable text length.
   /// </summary>
   /// <param name="Window">Parent window of the MultiLineEditbox (MLEB)</param>
   /// <param name="MLEditbox">MultiLineEditbox window</param>
   float ControllerUI::GetAdjustedWindowHeightWithMLEB(
      ::CEGUI::Window* Window,
      ::CEGUI::MultiLineEditbox* MLEditbox)
   {
      // Height of entire window (e.g. object inspection window).
      float heightWindow = Window->getHeight().d_offset;

      // Get the text that was added to the editbox as a list of lines.
      const ::CEGUI::MultiLineEditbox::LineList& textLines = MLEditbox->getFormattedLines();

      // Height of each line of text in the editbox (NOTE: at 1.0f scaling).
      float heightLine = MLEditbox->getFont()->getLineSpacing();

      // Current height of the editbox.
      float heightMLEditbox = MLEditbox->getInnerRectClipper().getHeight();

      // Calculate new height required for editbox.
      float newHeightMLEditbox = textLines.size() * heightLine;

      // Extra padding required to avoid placing scrollbar when it isn't needed.
      float newWindowHeight = heightWindow + newHeightMLEditbox - heightMLEditbox + UI_DESCRIPTIONPADDING * 2;

      // Bound at reasonable min/max.
      return CLRMath::Min(CLRMath::Max(newWindowHeight, 230.0f), 512.0f);
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

      // unset moving window if ui is locked
      if (OgreClient::Singleton->Config->UILocked)
         movingWindow = nullptr;

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
      //if (mouseCursor != nullptr && IgnoreTopControlForMouseInput)
      //mouseCursor->hide();
   };

   void ControllerUI::InjectMouseButtonUp(::CEGUI::MouseButton Button)
   {
      if (guiContext != nullptr)
         guiContext->injectMouseButtonUp(Button);

      //if (mouseCursor != nullptr && ControllerInput::IsMouseInWindow && !ControllerInput::IsAnyMouseDown)
      //mouseCursor->show();
   };

   void ControllerUI::InjectKeyDown(::CEGUI::Key::Scan Key)
   {
      keyDown = Key;

      // save tick for repeating keys in Tick()
      OgreClient::Singleton->GameTick->DidKeyRepeat();

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

      return OnRootKeyUp(args);
   };
 
   bool UICallbacks::OnKeyUpBlock(const CEGUI::EventArgs& e)
   {
      return true;
   };

   bool UICallbacks::OnRootMouseDown(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

      ControllerUI::ActivateRoot();

      return true;
   };

   bool UICallbacks::OnRootKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

      if (args.scancode == CEGUI::Key::Return ||
          args.scancode == CEGUI::Key::NumpadEnter)
      {
         // show chatwindow
         if (OgreClient::Singleton->Data->UIMode == UIMode::Playing)
         {
            ControllerUI::Chat::Window->setVisible(true);
            ControllerUI::Chat::Input->activate();
         }
      }

      else if (args.scancode == CEGUI::Key::Escape)
      {
         // disconnect on ESC in welcome screen
         if (OgreClient::Singleton->Data->UIMode == UIMode::AvatarSelection)
            OgreClient::Singleton->Disconnect();
      }

      return true;
   };

   bool UICallbacks::OnCopyPasteKeyDown(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
      bool handled                    = false;

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

   bool UICallbacks::OnItemListboxSelectionChangedUndo(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args	= static_cast<const CEGUI::WindowEventArgs&>(e);
      CEGUI::ItemListbox* list = static_cast<CEGUI::ItemListbox*>(args.window);

      // undo any selection
      if (list->getFirstSelectedItem() != nullptr)
         list->clearAllSelections();

      return true;
   };
#pragma endregion
};};
