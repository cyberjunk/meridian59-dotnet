#include "stdafx.h"

namespace Meridian59 {
   namespace Ogre
   {
      void ControllerUI::GuildHallBuy::Initialize()
      {
         // setup references to children from xml nodes
         Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_GUILDBUY_WINDOW));
         List = static_cast<CEGUI::MultiColumnList*>(Window->getChild(UI_NAME_GUILDBUY_LIST));
         ButtonBuy = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_GUILDBUY_BUTTONBUY));
         ButtonCancel = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_GUILDBUY_BUTTONCANCEL));
         GuildPassword = static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_GUILDBUY_GUILDPASSWORD));
         SelectedHall = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_GUILDBUY_SELECTEDNAME));
         PasswordInvalid = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_GUILDBUY_PASSWORDINVALID));

         List->setShowVertScrollbar(true);
         List->setSelectionMode(CEGUI::MultiColumnList::SelectionMode::RowSingle);
         List->setUserSortControlEnabled(false);

         List->addColumn("Guild hall name", 0, CEGUI::UDim(0.75f, -60));
         List->addColumn("Cost", 1, CEGUI::UDim(0.25f, -40));
         List->addColumn("Daily rent", 2, CEGUI::UDim(0.0f, 80));

         // attach listener to guildhalls data
         OgreClient::Singleton->Data->GuildHallsInfo->PropertyChanged +=
            gcnew PropertyChangedEventHandler(OnGuildHallsInfoPropertyChanged);
         OgreClient::Singleton->Data->GuildHallsInfo->GuildHalls->ListChanged +=
            gcnew ListChangedEventHandler(OnGuildHallsListChanged);

         // subscribe buttons
         ButtonBuy->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::GuildHallBuy::OnBuyClicked));
         ButtonCancel->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::GuildHallBuy::OnCancelClicked));

         // subscribe selection change
         List->subscribeEvent(CEGUI::MultiColumnList::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::GuildHallBuy::OnSelectionChanged));
         List->subscribeEvent(CEGUI::MultiColumnList::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::GuildHallBuy::OnKeyUp));

         // subscribe close button
         Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::GuildHallBuy::OnWindowClosed));

         // subscribe keyup
         Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::GuildHallBuy::OnWindowKeyUp));
      };

      void ControllerUI::GuildHallBuy::Destroy()
      {
         // detach listeners
         OgreClient::Singleton->Data->GuildHallsInfo->PropertyChanged -=
            gcnew PropertyChangedEventHandler(OnGuildHallsInfoPropertyChanged);
         OgreClient::Singleton->Data->GuildHallsInfo->GuildHalls->ListChanged -=
            gcnew ListChangedEventHandler(OnGuildHallsListChanged);
      };

      void ControllerUI::GuildHallBuy::ApplyLanguage()
      {
      };

      void ControllerUI::GuildHallBuy::OnGuildHallsInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
      {
         if (CLRString::Equals(e->PropertyName, BuyInfo::PROPNAME_ISVISIBLE))
         {
            // Reset PW warning.
            PasswordInvalid->setVisible(false);

            // set window visibility
            Window->setVisible(OgreClient::Singleton->Data->GuildHallsInfo->IsVisible);
            Window->moveToFront();
         }

      }
      void ControllerUI::GuildHallBuy::OnGuildHallsListChanged(Object^ sender, ListChangedEventArgs^ e)
      {
         switch (e->ListChangedType)
         {
         case ::System::ComponentModel::ListChangedType::ItemAdded:
            GuildHallAdd(e->NewIndex);
            break;

         case ::System::ComponentModel::ListChangedType::ItemDeleted:
            GuildHallRemove(e->NewIndex);
            break;

         case ::System::ComponentModel::ListChangedType::ItemChanged:
            GuildHallChange(e->NewIndex);
            break;
         }
      };

      void ControllerUI::GuildHallBuy::GuildHallAdd(int Index)
      {
         GuildHall^ obj = OgreClient::Singleton->Data->GuildHallsInfo->GuildHalls[Index];

         CEGUI::ListboxTextItem* itmName = new CEGUI::ListboxTextItem(
            StringConvert::CLRToCEGUI(obj->Name));

         CEGUI::ListboxTextItem* itmCost = new CEGUI::ListboxTextItem(
            StringConvert::CLRToCEGUI(obj->Cost.ToString()));

         CEGUI::ListboxTextItem* itmRent = new CEGUI::ListboxTextItem(
            StringConvert::CLRToCEGUI(obj->Rent.ToString()));

         itmName->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
         itmCost->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
         itmRent->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");

         itmName->setSelectionColours(CEGUI::Colour(0xFF444444));
         itmCost->setSelectionColours(CEGUI::Colour(0xFF444444));
         itmRent->setSelectionColours(CEGUI::Colour(0xFF444444));
         // insert widget in ui-list
         if ((int)List->getRowCount() > Index)
            List->insertRow(Index);

         // or add
         else
            List->addRow();

         List->setItem(itmName, 0, Index);
         List->setItem(itmCost, 1, Index);
         List->setItem(itmRent, 2, Index);
      };

      void ControllerUI::GuildHallBuy::GuildHallRemove(int Index)
      {
         // check
         if ((int)List->getRowCount() > Index)
         {
            CEGUI::ListboxItem* itm = List->getFirstSelectedItem();
            if (itm != nullptr
               && itm->getText().compare(SelectedHall->getText()) == 0)
            {
               SelectedHall->setText("");
            }

            List->removeRow(Index);
         }

         if (List->getRowCount() == 0)
         {
            // hide window, return control to root
            OgreClient::Singleton->Data->GuildHallsInfo->IsVisible = false;
            ControllerUI::ActivateRoot();
         }
      };

      void ControllerUI::GuildHallBuy::GuildHallChange(int Index)
      {
         GuildHall^ obj = OgreClient::Singleton->Data->GuildHallsInfo->GuildHalls[Index];
      };

      bool UICallbacks::GuildHallBuy::OnSelectionChanged(const CEGUI::EventArgs& e)
      {
         const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
         CEGUI::MultiColumnList* list = ControllerUI::GuildHallBuy::List;
         CEGUI::ListboxItem* itm = list->getFirstSelectedItem();

         if (itm != nullptr)
         {
            unsigned int index = list->getItemRowIndex(itm);
            // Set the name in SelectedHall box to our selected hall.
            GuildHall^ gHall = OgreClient::Singleton->Data->GuildHallsInfo->GuildHalls[index];
            if (gHall != nullptr)
            {
               ControllerUI::GuildHallBuy::SelectedHall->setText(
                  StringConvert::CLRToCEGUI(gHall->Name));
            }
         }

         return true;
      };

      bool UICallbacks::GuildHallBuy::OnKeyUp(const CEGUI::EventArgs& e)
      {
         const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
         CEGUI::MultiColumnList* list = ControllerUI::GuildHallBuy::List;
         CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
         int count = (int)list->getRowCount();

         if (itm != nullptr)
         {
            unsigned int index = list->getItemRowIndex(itm);

            if (args.scancode == CEGUI::Key::Scan::ArrowUp && index > 0)
            {
               // unselect
               list->setItemSelectState(CEGUI::MCLGridRef(index, 0), false);
               list->setItemSelectState(CEGUI::MCLGridRef(index, 1), false);
               list->setItemSelectState(CEGUI::MCLGridRef(index, 2), false);

               // select
               list->setItemSelectState(CEGUI::MCLGridRef(index - 1, 0), true);
               list->setItemSelectState(CEGUI::MCLGridRef(index - 1, 1), true);
               list->setItemSelectState(CEGUI::MCLGridRef(index - 1, 2), true);

               // make sure new selected row is visible (scroll if necessary)
               list->ensureRowIsVisible(index - 1);
            }
            else if (args.scancode == CEGUI::Key::Scan::ArrowDown && (int)index < count - 1)
            {
               // unselect
               list->setItemSelectState(CEGUI::MCLGridRef(index, 0), false);
               list->setItemSelectState(CEGUI::MCLGridRef(index, 1), false);
               list->setItemSelectState(CEGUI::MCLGridRef(index, 2), false);

               // select
               list->setItemSelectState(CEGUI::MCLGridRef(index + 1, 0), true);
               list->setItemSelectState(CEGUI::MCLGridRef(index + 1, 1), true);
               list->setItemSelectState(CEGUI::MCLGridRef(index + 1, 2), true);

               // make sure new selected row is visible (scroll if necessary)
               list->ensureRowIsVisible(index + 1);
            }
         }

         return true;
      };

      bool UICallbacks::GuildHallBuy::OnBuyClicked(const CEGUI::EventArgs& e)
      {
         CEGUI::MultiColumnList* list = ControllerUI::GuildHallBuy::List;
         CEGUI::ListboxItem* itm = list->getFirstSelectedItem();

         // Must have something selected.
         if (itm == nullptr)
         {
            return true;
         }

         // convert userinput to managed strings
         CLRString^ guildPW = StringConvert::CEGUIToCLR(ControllerUI::GuildHallBuy::GuildPassword->getText());

         // If no PW entered, make the error message visible and don't send.
         if (CLRString::Equals(guildPW, CLRString::Empty))
         {
            ControllerUI::GuildHallBuy::PasswordInvalid->setVisible(true);

            return true;
         }

         // Get guildhall object.
         unsigned int index = list->getItemRowIndex(itm);
         // Set the name in SelectedHall box to our selected hall.
         GuildHall^ gHall = OgreClient::Singleton->Data->GuildHallsInfo->GuildHalls[index];

         // Send buy message.
         OgreClient::Singleton->SendUserCommandGuildRent(gHall->ID, guildPW);

         OgreClient::Singleton->Data->GuildHallsInfo->Clear(true);
         OgreClient::Singleton->Data->GuildHallsInfo->IsVisible = false;
         ControllerUI::ActivateRoot();

         return true;
      }

      bool UICallbacks::GuildHallBuy::OnCancelClicked(const CEGUI::EventArgs& e)
      {
         OgreClient::Singleton->Data->GuildHallsInfo->Clear(true);
         OgreClient::Singleton->Data->GuildHallsInfo->IsVisible = false;
         ControllerUI::ActivateRoot();

         return true;
      }

      bool UICallbacks::GuildHallBuy::OnWindowClosed(const CEGUI::EventArgs& e)
      {
         OgreClient::Singleton->Data->GuildHallsInfo->Clear(true);
         OgreClient::Singleton->Data->GuildHallsInfo->IsVisible = false;
         ControllerUI::ActivateRoot();

         return true;
      }

      bool UICallbacks::GuildHallBuy::OnWindowKeyUp(const CEGUI::EventArgs& e)
      {
         const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

         // close window on ESC
         if (args.scancode == CEGUI::Key::Escape)
         {
            OgreClient::Singleton->Data->GuildHallsInfo->Clear(true);
            OgreClient::Singleton->Data->GuildHallsInfo->IsVisible = false;
            ControllerUI::ActivateRoot();
         }

         return true;
      }
   };
};