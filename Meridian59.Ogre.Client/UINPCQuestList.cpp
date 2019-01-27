#include "stdafx.h"

namespace Meridian59 {
   namespace Ogre
   {
      void ControllerUI::NPCQuestList::Initialize()
      {
         // setup references to children from xml nodes
         Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_NPCQUESTLIST_WINDOW));
         Name = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NPCQUESTLIST_NAME));
         Image = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NPCQUESTLIST_IMAGE));
         QuestList = static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_NPCQUESTLIST_QUESTLIST));
         DescriptionLabel = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NPCQUESTLIST_DESCLABEL));
         Description = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NPCQUESTLIST_DESC));
         DescriptionPlain = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_NPCQUESTLIST_DESCPLAIN));
         RequirementsLabel = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NPCQUESTLIST_REQLABEL));
         Requirements = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NPCQUESTLIST_REQ));
         RequirementsPlain = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_NPCQUESTLIST_REQPLAIN));
         Accept = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NPCQUESTLIST_ACCEPT));
         Help = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NPCQUESTLIST_HELP));
         Close = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NPCQUESTLIST_CLOSE));

         // set multiselect
         QuestList->setMultiSelectEnabled(false);
         
         // disable caret on plain info boxes
         DescriptionPlain->setEnsureCaretVisible(false);
         RequirementsPlain->setEnsureCaretVisible(false);

         // init imagecomposers list for questlist icons
         imageComposers = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>();

         // image composer for NPC picture
         imageComposerNPC = gcnew ImageComposerCEGUI<ObjectBase^>();
         imageComposerNPC->ApplyYOffset = false;
         imageComposerNPC->IsScalePow2 = false;
         imageComposerNPC->UseViewerFrame = true;
         imageComposerNPC->Width = (unsigned int)Image->getPixelSize().d_width;
         imageComposerNPC->Height = (unsigned int)Image->getPixelSize().d_height;
         imageComposerNPC->CenterHorizontal = true;
         imageComposerNPC->CenterVertical = true;
         imageComposerNPC->NewImageAvailable += gcnew ::System::EventHandler(OnNewNPCImageAvailable);

         // attach listener to questUIInfo
         OgreClient::Singleton->Data->QuestUIInfo->PropertyChanged +=
            gcnew PropertyChangedEventHandler(OnNPCQuestListPropertyChanged);

         // attach listener to quest list items
         OgreClient::Singleton->Data->QuestUIInfo->QuestList->ListChanged +=
            gcnew ListChangedEventHandler(OnQuestListChanged);

         // subscribe selection change
         QuestList->subscribeEvent(CEGUI::ItemListbox::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnListSelectionChanged));

         // subscribe clicks for window toggles
         Description->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnDescriptionTextClicked));
         DescriptionPlain->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnDescriptionTextClicked));
         Requirements->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnRequirementsTextClicked));
         RequirementsPlain->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnRequirementsTextClicked));

         // subscripe copy paste handlers on plain infobox text
         DescriptionPlain->subscribeEvent(CEGUI::Window::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
         RequirementsPlain->subscribeEvent(CEGUI::Window::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

         // subscribe buttons
         Accept->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnAcceptClicked));
         Help->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnHelpClicked));
         Close->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnCloseClicked));

         // subscribe close button
         Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnWindowClosed));

         // subscribe keyup
         Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::NPCQuestList::OnWindowKeyUp));
      };

      void ControllerUI::NPCQuestList::Destroy()
      {
         OgreClient::Singleton->Data->QuestUIInfo->PropertyChanged -=
            gcnew PropertyChangedEventHandler(OnNPCQuestListPropertyChanged);

         imageComposerNPC->NewImageAvailable -=
            gcnew ::System::EventHandler(OnNewNPCImageAvailable);

         OgreClient::Singleton->Data->QuestUIInfo->QuestList->ListChanged -=
            gcnew ListChangedEventHandler(OnQuestListChanged);
      };

      void ControllerUI::NPCQuestList::ApplyLanguage()
      {
         // update window title
         DescriptionLabel->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::DESCRIPTION));
         RequirementsLabel->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::REQUIREMENTS));
         Accept->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::ACCEPT));
         Close->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::CLOSE));
      };

      void ControllerUI::NPCQuestList::OnNPCQuestListPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
      {
         // quest giver
         if (CLRString::Equals(e->PropertyName, QuestUIInfo::PROPNAME_QUESTGIVER))
         {
            imageComposerNPC->DataSource = OgreClient::Singleton->Data->QuestUIInfo->QuestGiver;
         }

         // visible
         else if (CLRString::Equals(e->PropertyName, QuestUIInfo::PROPNAME_ISVISIBLE))
         {
            // set window visibility
            Window->setVisible(OgreClient::Singleton->Data->QuestUIInfo->IsVisible);
            Window->moveToFront();
         }
      };

      void ControllerUI::NPCQuestList::OnNewNPCImageAvailable(Object^ sender, ::System::EventArgs^ e)
      {
         Image->setProperty(UI_PROPNAME_IMAGE, *imageComposerNPC->Image->TextureName);
      };

      void ControllerUI::NPCQuestList::OnQuestListChanged(Object^ sender, ListChangedEventArgs^ e)
      {
         switch (e->ListChangedType)
         {
         case ::System::ComponentModel::ListChangedType::ItemAdded:
            QuestItemAdd(e->NewIndex);
            break;

         case ::System::ComponentModel::ListChangedType::ItemDeleted:
            QuestItemRemove(e->NewIndex);
            break;
         }
      };

      void ControllerUI::NPCQuestList::QuestItemAdd(int Index)
      {
         CEGUI::WindowManager& wndMgr = CEGUI::WindowManager::getSingleton();
         QuestObjectInfo^ obj = OgreClient::Singleton->Data->QuestUIInfo->QuestList[Index];

         // create widget (item)
         CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr.createWindow(
            UI_WINDOWTYPE_QUESTLISTITEM);

         // set ID
         widget->setID(obj->ObjectBase->ID);

         CEGUI::Window* icon = (CEGUI::Window*)widget->getChildAtIdx(UI_QUESTS_CHILDINDEX_ICON);
         CEGUI::Window* name = (CEGUI::Window*)widget->getChildAtIdx(UI_QUESTS_CHILDINDEX_NAME);

         // get color for list item (based on object type of quest template object)
         ::CEGUI::Colour color = ::CEGUI::Colour(QuestTypeColors::GetColorFor(obj->ObjectBase->Flags));

         // set color and name
         name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
         name->setText(StringConvert::CLRToCEGUI(obj->ObjectBase->Name));

         // insert widget in ui-list
         if ((int)QuestList->getItemCount() > Index)
            QuestList->insertItem(widget, QuestList->getItemFromIndex(Index));

         // or add
         else
            QuestList->addItem(widget);

         // select first item when adding it
         if ((int)QuestList->getItemCount() == 1)
            QuestList->selectRange(0, 0);

         // fix a big with last item not highlighted
         QuestList->notifyScreenAreaChanged(true);

         // create imagecomposer
         ImageComposerCEGUI<ObjectBase^>^ imageComposer = gcnew ImageComposerCEGUI<ObjectBase^>();
         imageComposer->ApplyYOffset = false;
         imageComposer->HotspotIndex = 0;
         imageComposer->IsScalePow2 = false;
         imageComposer->UseViewerFrame = false;
         imageComposer->Width = UI_BUFFICON_WIDTH;
         imageComposer->Height = UI_BUFFICON_HEIGHT;
         imageComposer->CenterHorizontal = true;
         imageComposer->CenterVertical = true;
         imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewQuestListImageAvailable);

         // insert composer into list at index
         imageComposers->Insert(Index, imageComposer);

         // set image
         imageComposer->DataSource = obj->ObjectBase;

         // possibly need to change text for title/description/requirements
         SetQuestText();
      };

      void ControllerUI::NPCQuestList::QuestItemRemove(int Index)
      {
         // check
         if ((int)QuestList->getItemCount() > Index)
            QuestList->removeItem(QuestList->getItemFromIndex(Index));

         // remove imagecomposer
         if (imageComposers->Count > Index)
         {
            // reset (detaches listeners!)
            imageComposers[Index]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewQuestListImageAvailable);
            imageComposers[Index]->DataSource = nullptr;

            // remove from list
            imageComposers->RemoveAt(Index);
         }

         // possibly need to change text for title/description/requirements
         SetQuestText();
      };

      void ControllerUI::NPCQuestList::OnNewQuestListImageAvailable(Object^ sender, ::System::EventArgs^ e)
      {
         ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
         int index = imageComposers->IndexOf(imageComposer);

         if (index > -1 && (int)QuestList->getItemCount() > index)
         {
            // get staticimage
            CEGUI::ItemEntry* img = QuestList->getItemFromIndex(index);
            CEGUI::Window* icon = (CEGUI::Window*)img->getChildAtIdx(UI_QUESTS_CHILDINDEX_ICON);

            // set new image on ui widget
            icon->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
         }
      };

      void ControllerUI::NPCQuestList::OnHelpOKConfirmed(Object^ sender, ::System::EventArgs^ e)
      {
         // set our window active
         Window->activate();

         return;
      };

      void ControllerUI::NPCQuestList::SetQuestText()
      {
         CEGUI::ItemEntry* item = QuestList->getFirstSelectedItem();

         if (item)
         {
            // get index of clicked buff/widget in listbox
            int index = (int)QuestList->getItemIndex(item);

            // get questinfo object
            QuestObjectInfo^ obj = OgreClient::Singleton->Data->QuestUIInfo->QuestList[index];

            // set title text to name of quest
            Name->setText(StringConvert::CLRToCEGUI(obj->ObjectBase->Name));

            // set description, use fullstring to strip any kod styling/colors
            // shouldn't be any in a description, but do it anyway for consistency in case there is
            Description->setText(StringConvert::CLRToCEGUI(obj->Description->FullString));
            DescriptionPlain->setText(StringConvert::CLRToCEGUI(obj->Description->FullString));

            // set requirements or instructions, use kod styling/colors to differentiate met requirements vs unmet ones
            Requirements->setText(GetChatString(obj->Requirements));
            RequirementsPlain->setText(StringConvert::CLRToCEGUI(obj->Requirements->FullString));

            // reset text boxes to the non-plain version
            Description->setVisible(true);
            DescriptionPlain->setVisible(false);
            Requirements->setVisible(true);
            RequirementsPlain->setVisible(false);

            // change UI text depending on whether selected quest can be started/continued
            if (obj->ObjectBase->Flags->Player == ObjectFlags::PlayerType::QuestActive)
            {
               RequirementsLabel->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::INSTRUCTIONS));
               Accept->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::CONTINUE));
               Accept->enable();
            }
            else
            {
               RequirementsLabel->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::REQUIREMENTS));
               Accept->setText(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::ACCEPT));
               // disable Accept button for quests that can't be started
               if (obj->ObjectBase->Flags->Player == ObjectFlags::PlayerType::QuestInvalid)
                  Accept->disable();
               else
                  Accept->enable();
            }
         }
      };

      ::CEGUI::String ControllerUI::NPCQuestList::GetChatString(ServerString^ ChatMessage)
      {
         // text with CEGUI markup and escapes
         CLRString^ text = CLRString::Empty;

         // walk style definitions
         for each (ChatStyle^ style in ChatMessage->Styles)
         {
            // add fontselection
            // normal
            if (!style->IsBold && !style->IsCursive)
               text += UI_TAGFONT_NORMAL;

            // bold not italic
            else if (style->IsBold && !style->IsCursive)
               text += UI_TAGFONT_BOLD;

            // not bold but italic
            else if (!style->IsBold && style->IsCursive)
               text += UI_TAGFONT_ITALIC;

            // bold and italic
            else if (style->IsBold && style->IsCursive)
               text += UI_TAGFONT_BOLDITALIC;

            // add color
            switch (style->Color)
            {
            case ChatColor::Black: text += UI_TAGCOLOR_BLACK; break;
            case ChatColor::Blue: text += UI_TAGCOLOR_BLUE; break;
            case ChatColor::Green: text += UI_TAGCOLOR_CHATGREEN; break;
            case ChatColor::Purple: text += UI_TAGCOLOR_CHATPURPLE; break;
            case ChatColor::Red: text += UI_TAGCOLOR_CHATRED; break;
            case ChatColor::White: text += UI_TAGCOLOR_WHITE; break;
#ifndef VANILLA
            case ChatColor::BrightRed: text += UI_TAGCOLOR_CHATBRIGHTRED; break;
            case ChatColor::LightGreen: text += UI_TAGCOLOR_CHATLIGHTGREEN; break;
            case ChatColor::Yellow: text += UI_TAGCOLOR_CHATYELLOW; break;
            case ChatColor::Pink: text += UI_TAGCOLOR_CHATPINK; break;
            case ChatColor::Orange: text += UI_TAGCOLOR_CHATORANGE; break;
            case ChatColor::Aquamarine: text += UI_TAGCOLOR_CHATAQUAMARINE; break;
            case ChatColor::Cyan: text += UI_TAGCOLOR_CHATCYAN; break;
            case ChatColor::Teal: text += UI_TAGCOLOR_CHATTEAL; break;
            case ChatColor::DarkGrey: text += UI_TAGCOLOR_CHATDARKGREY; break;
            case ChatColor::Violet: text += UI_TAGCOLOR_CHATVIOLET; break;
            case ChatColor::Magenta: text += UI_TAGCOLOR_CHATMAGENTA; break;
#endif
            }

            // get substring for this style
            CLRString^ str = ChatMessage->FullString->Substring(style->StartIndex, style->Length);

            // replace "\" with "\\"
            // and "[" with "\["
            str = str->Replace("\\", "\\\\");
            str = str->Replace("[", "\\[");

            // add textpart
            text += str;
         }

         // append line break
         text += ::System::Environment::NewLine;

         // return as CEGUI string
         return StringConvert::CLRToCEGUI(text);
      };

      //////////////////////////////////////////////////////////////////////////////////////////////////////////
      //////////////////////////////////////////////////////////////////////////////////////////////////////////

      bool UICallbacks::NPCQuestList::OnListSelectionChanged(const CEGUI::EventArgs& e)
      {
         // have to set title/desc/req to quest details
         ControllerUI::NPCQuestList::SetQuestText();

         return true;
      };

      bool UICallbacks::NPCQuestList::OnDescriptionTextClicked(const CEGUI::EventArgs& e)
      {
         const CEGUI::MouseEventArgs& e2 = static_cast<const CEGUI::MouseEventArgs&>(e);

         // only for right mousebutton
         if (e2.button != ::CEGUI::MouseButton::RightButton)
            return true;

         // toggle windows
         ControllerUI::NPCQuestList::Description->setVisible(!ControllerUI::NPCQuestList::Description->isVisible());
         ControllerUI::NPCQuestList::DescriptionPlain->setVisible(!ControllerUI::NPCQuestList::DescriptionPlain->isVisible());

         return true;
      };

      bool UICallbacks::NPCQuestList::OnRequirementsTextClicked(const CEGUI::EventArgs& e)
      {
         const CEGUI::MouseEventArgs& e2 = static_cast<const CEGUI::MouseEventArgs&>(e);

         // only for right mousebutton
         if (e2.button != ::CEGUI::MouseButton::RightButton)
            return true;

         // toggle windows
         ControllerUI::NPCQuestList::Requirements->setVisible(!ControllerUI::NPCQuestList::Requirements->isVisible());
         ControllerUI::NPCQuestList::RequirementsPlain->setVisible(!ControllerUI::NPCQuestList::RequirementsPlain->isVisible());

         return true;
      };

      bool UICallbacks::NPCQuestList::OnAcceptClicked(const CEGUI::EventArgs& e)
      {
         CEGUI::ItemEntry* item = ControllerUI::NPCQuestList::QuestList->getFirstSelectedItem();

         if (item)
         {
            // get index of clicked buff/widget in listbox
            int index = (int)ControllerUI::NPCQuestList::QuestList->getItemIndex(item);

            // get questinfo object
            QuestObjectInfo^ obj = OgreClient::Singleton->Data->QuestUIInfo->QuestList[index];
            // request quests from current target
            OgreClient::Singleton->SendReqTriggerQuestMessage(
               gcnew ObjectID(OgreClient::Singleton->Data->QuestUIInfo->QuestGiver->ID,0),
               gcnew ObjectID(obj->ObjectBase->ID,0));

            OgreClient::Singleton->Data->QuestUIInfo->IsVisible = false;
            OgreClient::Singleton->Data->QuestUIInfo->Clear(true);
            ControllerUI::ActivateRoot();
         }

         return true;
      };

      bool UICallbacks::NPCQuestList::OnHelpClicked(const CEGUI::EventArgs& e)
      {
         // callback needed to reactivate quest UI window
         ControllerUI::ConfirmPopup::Confirmed += gcnew System::EventHandler(ControllerUI::NPCQuestList::OnHelpOKConfirmed);
         ControllerUI::ConfirmPopup::ShowOKLarge(GetLangNPCQuestUI(LANGSTR_NPCQUESTUI::HELPTEXT), 0);

         return true;
      };

      bool UICallbacks::NPCQuestList::OnCloseClicked(const CEGUI::EventArgs& e)
      {
         // set not visible in datalayer (view will react)
         OgreClient::Singleton->Data->QuestUIInfo->IsVisible = false;
         OgreClient::Singleton->Data->QuestUIInfo->Clear(true);

         // mark GUIroot active
         ControllerUI::ActivateRoot();

         return true;
      };

      bool UICallbacks::NPCQuestList::OnWindowKeyUp(const CEGUI::EventArgs& e)
      {
         const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

         // close window on ESC
         if (args.scancode == CEGUI::Key::Escape)
         {
            // clear (view will react)
            OgreClient::Singleton->Data->QuestUIInfo->IsVisible = false;
            OgreClient::Singleton->Data->QuestUIInfo->Clear(true);
         }

         return UICallbacks::OnKeyUp(args);
      };

      bool UICallbacks::NPCQuestList::OnWindowClosed(const CEGUI::EventArgs& e)
      {
         // set not visible in datalayer (view will react)
         OgreClient::Singleton->Data->QuestUIInfo->IsVisible = false;
         OgreClient::Singleton->Data->QuestUIInfo->Clear(true);

         // mark GUIroot active
         ControllerUI::ActivateRoot();

         return true;
      }
   };
};
