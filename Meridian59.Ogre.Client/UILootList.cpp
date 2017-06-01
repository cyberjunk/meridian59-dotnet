#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::LootList::Initialize()
   {
      // setup references to children from xml nodes
      Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_LOOTLIST_WINDOW));
      List = static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_LOOTLIST_LIST));
      Get = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_LOOTLIST_GET));
      GetAll = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_LOOTLIST_GETALL));

      // set multiselect
      List->setMultiSelectEnabled(true);

      // init imagecomposers list
      imageComposers = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>();

      // attach listener to lootinfo
      OgreClient::Singleton->Data->RoomObjectsLoot->PropertyChanged +=
         gcnew PropertyChangedEventHandler(OnLootInfoPropertyChanged);

      // attach listener to lootinfo items
      OgreClient::Singleton->Data->RoomObjectsLoot->Items->ListChanged +=
         gcnew ListChangedEventHandler(OnLootInfoListChanged);

      // subscribe Get button
      Get->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::LootList::OnGetClicked));
      GetAll->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::LootList::OnGetAllClicked));

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::LootList::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::LootList::OnWindowKeyUp));
   };

   void ControllerUI::LootList::Destroy()
   {
      OgreClient::Singleton->Data->RoomObjectsLoot->PropertyChanged -=
         gcnew PropertyChangedEventHandler(OnLootInfoPropertyChanged);

      OgreClient::Singleton->Data->RoomObjectsLoot->Items->ListChanged -=
         gcnew ListChangedEventHandler(OnLootInfoListChanged);
   };

   void ControllerUI::LootList::ApplyLanguage()
   {
   };

   void ControllerUI::LootList::OnLootInfoPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      // visible
      if (CLRString::Equals(e->PropertyName, Data::Models::LootInfo::PROPNAME_ISVISIBLE))
      {
         // set window visibility
         Window->setVisible(OgreClient::Singleton->Data->RoomObjectsLoot->IsVisible);
         Window->moveToFront();
      }
   };

   void ControllerUI::LootList::OnLootInfoListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      switch (e->ListChangedType)
      {
      case ::System::ComponentModel::ListChangedType::ItemAdded:
         ItemAdd(e->NewIndex);
         break;

      case ::System::ComponentModel::ListChangedType::ItemDeleted:
         ItemRemove(e->NewIndex);
         break;

      case ::System::ComponentModel::ListChangedType::ItemChanged:
         // react only to changes which may affect the view				
         if (CLRString::Equals(e->PropertyDescriptor->Name, RoomObject::PROPNAME_COUNT) ||
            CLRString::Equals(e->PropertyDescriptor->Name, RoomObject::PROPNAME_NAME))
         {
            ItemChange(e->NewIndex);
         }
         break;
      }
   };

   void ControllerUI::LootList::ItemAdd(int Index)
   {
      CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
      ObjectBase^ obj = OgreClient::Singleton->Data->RoomObjectsLoot->Items[Index];

      // create widget (item)
      CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
         UI_WINDOWTYPE_OBJECTBASELISTBOXITEM);

      // set ID
      widget->setID(obj->ID);

      // subscribe click event
      widget->subscribeEvent(
         CEGUI::ItemEntry::EventMouseClick,
         CEGUI::Event::Subscriber(UICallbacks::LootList::OnItemClicked));

      // check
      if (widget->getChildCount() > 2)
      {
         CEGUI::Window* icon = (CEGUI::Window*)widget->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_ICON);
         CEGUI::Window* name = (CEGUI::Window*)widget->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_NAME);
         CEGUI::Editbox* amount = (CEGUI::Editbox*)widget->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_AMOUNT);

         // NOTE: Currently server does not support specifying an amount in BP_REQ_GET
         // So we can't use our amount here and deactivate it for now
         amount->setEnabled(false);

         // subscribe event to focusleave on textbox
         amount->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::LootList::OnItemAmountDeactivated));
         amount->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::LootList::OnItemAmountDeactivated));
      }

      // insert widget in ui-list
      if ((int)List->getItemCount() > Index)
         List->insertItem(widget, List->getItemFromIndex(Index));

      // or add
      else
         List->addItem(widget);

      // fix a big with last item not highlighted
      List->notifyScreenAreaChanged(true);

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
      imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewItemImageAvailable);

      // insert composer into list at index
      imageComposers->Insert(Index, imageComposer);

      // update values
      ItemChange(Index);
   };

   void ControllerUI::LootList::ItemRemove(int Index)
   {
      // check
      if ((int)List->getItemCount() > Index)
         List->removeItem(List->getItemFromIndex(Index));

      // remove imagecomposer
      if (imageComposers->Count > Index)
      {
         // reset (detaches listeners!)
         imageComposers[Index]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewItemImageAvailable);
         imageComposers[Index]->DataSource = nullptr;

         // remove from list
         imageComposers->RemoveAt(Index);
      }
   };

   void ControllerUI::LootList::ItemChange(int Index)
   {
      ObjectBase^ obj = OgreClient::Singleton->Data->RoomObjectsLoot->Items[Index];

      // set imagecomposer datasource
      if (imageComposers->Count > Index)
         imageComposers[Index]->DataSource = obj;

      // check
      if ((int)List->getItemCount() > Index)
      {
         CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);

         // check
         if (wnd->getChildCount() > 2)
         {
            CEGUI::Window* icon = (CEGUI::Window*)wnd->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_ICON);
            CEGUI::Window* name = (CEGUI::Window*)wnd->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_NAME);
            CEGUI::Editbox* amount = (CEGUI::Editbox*)wnd->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_AMOUNT);

            // get color
            ::CEGUI::Colour color = ::CEGUI::Colour(
               NameColors::GetColorFor(obj->Flags));

            // set color and name
            name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
            name->setText(StringConvert::CLRToCEGUI(obj->Name));

            // set default amount
            amount->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Count));
            amount->setVisible(obj->IsStackable);
         }
      }
   };

   void ControllerUI::LootList::OnNewItemImageAvailable(Object^ sender, ::System::EventArgs^ e)
   {
      ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
      int index = imageComposers->IndexOf(imageComposer);

      if (index > -1 && (int)List->getItemCount() > index)
      {
         // get staticimage
         CEGUI::ItemEntry* img = List->getItemFromIndex(index);
         CEGUI::Window* icon = (CEGUI::Window*)img->getChildAtIdx(UI_LOOTLIST_CHILDINDEX_ICON);

         // set new image on ui widget
         icon->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
      }
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::LootList::OnItemAmountDeactivated(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = (CEGUI::MouseEventArgs&)e;
      CEGUI::Editbox* box = (CEGUI::Editbox*)args.window;
      CEGUI::ItemEntry* itemEntry = (CEGUI::ItemEntry*)box->getParent();
      CEGUI::ItemListbox* list = ControllerUI::LootList::List;

      // get user value from box
      CEGUI::String boxText = box->getText();

      // revert empty input to 1
      if (boxText == STRINGEMPTY)
      {
         boxText = "1";
         box->setText(boxText);
      }

      // data models
      RoomObjectListFiltered^ dataItems =
         OgreClient::Singleton->Data->RoomObjectsLoot->Items;

      // get index of clicked buff/widget in listbox
      int index = (int)list->getItemIndex(itemEntry);

      // found ?
      if (dataItems->Count > index)
         dataItems[index]->Count = ::CEGUI::PropertyHelper<unsigned int>::fromString(boxText);

      return true;
   };

   bool UICallbacks::LootList::OnItemClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = (const CEGUI::MouseEventArgs&)e;
      const CEGUI::ItemEntry* itm = (CEGUI::ItemEntry*)args.window;

      // single rightclick requests object details
      if (args.button == CEGUI::MouseButton::RightButton)
         OgreClient::Singleton->SendReqLookMessage(itm->getID());

      return true;
   };

   bool UICallbacks::LootList::OnGetClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
      CEGUI::ItemListbox* list = ControllerUI::LootList::List;
      RoomObjectListFiltered^ dataItems = OgreClient::Singleton->Data->RoomObjectsLoot->Items;

      // try to get selected ones
      for (size_t i = 0; i < list->getItemCount(); i++)
      {
         if (list->isItemSelected(i) && dataItems->Count >(int)i)
         {
            OgreClient::Singleton->SendReqGetMessage(
               gcnew ObjectID(dataItems[(int)i]->ID, dataItems[(int)i]->Count));
         }
      }

      // hide
      OgreClient::Singleton->Data->RoomObjectsLoot->IsVisible = false;

      ControllerUI::ActivateRoot();

      return true;
   };

   bool UICallbacks::LootList::OnGetAllClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);

      // loot everything in range
      OgreClient::Singleton->LootAll();

      // hide
      OgreClient::Singleton->Data->RoomObjectsLoot->IsVisible = false;

      // reactivate root
      ControllerUI::ActivateRoot();

      return true;
   };

   bool UICallbacks::LootList::OnWindowKeyUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

      // close window on ESC
      if (args.scancode == CEGUI::Key::Escape)
      {
         // hide the lootlist
         OgreClient::Singleton->Data->RoomObjectsLoot->IsVisible = false;

         // mark GUIroot active
         ControllerUI::ActivateRoot();
      }

      return true;
   };

   bool UICallbacks::LootList::OnWindowClosed(const CEGUI::EventArgs& e)
   {
      // hide the lootlist
      OgreClient::Singleton->Data->RoomObjectsLoot->IsVisible = false;

      // mark GUIroot active
      ControllerUI::ActivateRoot();

      return true;
   };
};};
