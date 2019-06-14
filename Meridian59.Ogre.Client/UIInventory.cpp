#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::Inventory::Initialize()
   {
      // setup references to children from xml nodes
      Window   = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_INVENTORY_WINDOW));
      Pane     = static_cast<CEGUI::ScrollablePane*>(Window->getChild(UI_NAME_INVENTORY_PANE));
      List     = static_cast<CEGUI::GridLayoutContainer*>(Pane->getChild(UI_NAME_INVENTORY_LIST));

      // set window layout from config
      Window->setPosition(OgreClient::Singleton->Config->UILayoutInventory->getPosition());
      Window->setSize(OgreClient::Singleton->Config->UILayoutInventory->getSize());

      // attach listener to inventory
      OgreClient::Singleton->Data->InventoryObjects->ListChanged += 
         gcnew ListChangedEventHandler(OnInventoryListChanged);
 
      // create image composers list for inventory slots
      imageComposers = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<InventoryObject^>^>();

      // Init row counter to 0.
      currentInventoryRows = 0;

      // start with minimum row number
      for(unsigned int i = 0; i < UI_INVENTORY_MIN_ROWS; i++)
         AddInventoryRow();

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
   };

   void ControllerUI::Inventory::Destroy()
   {
      // detach listener from inventory
      OgreClient::Singleton->Data->InventoryObjects->ListChanged -= 
         gcnew ListChangedEventHandler(OnInventoryListChanged);

      // amount of entries the inventory grid can hold
      const int entries = UI_INVENTORY_COLS * currentInventoryRows;

      for(int i = 0; i < entries; i++)
         imageComposers[i]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewImageAvailable);
   };

   void ControllerUI::Inventory::ApplyLanguage()
   {
      Window->setText(GetLangWindowTitle(LANGSTR_WINDOW_TITLE::INVENTORY));
   };

   void ControllerUI::Inventory::OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e)
   {
      ImageComposerCEGUI<InventoryObject^>^ imageComposer = (ImageComposerCEGUI<InventoryObject^>^)sender;
      int index = imageComposers->IndexOf(imageComposer);

      if ((int)List->getChildCount() > index)
      {
         CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)List->getChildAtIdx(index);
         CEGUI::Window* imgButton = dragger->getChildAtIdx(0);

         imgButton->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
      }
   };

   void ControllerUI::Inventory::OnInventoryListChanged(Object^ sender, ListChangedEventArgs^ e)
   {
      switch(e->ListChangedType)
      {
      case ::System::ComponentModel::ListChangedType::ItemAdded:
         InventoryAdd(e->NewIndex);
         break;

      case ::System::ComponentModel::ListChangedType::ItemDeleted:
         InventoryRemove(e->NewIndex);
         break;

      case ::System::ComponentModel::ListChangedType::ItemChanged:
         InventoryChange(e->NewIndex);
         break;
      }
   };

   void ControllerUI::Inventory::AddInventoryRow()
   {
      // Don't modify inventory grid while rearranging items.
      if (IsRearrangingInventory)
         return;

      // get windowmanager
      CEGUI::WindowManager& wndMgr = CEGUI::WindowManager::getSingleton();

      // Increment number of rows.
      ++currentInventoryRows;

      // Resize grid dimensions.
      List->setGridDimensions(UI_INVENTORY_COLS, currentInventoryRows);

      for (int i = 0; i < UI_INVENTORY_COLS; ++i)
      {
         // create image composer for slot
         ImageComposerCEGUI<InventoryObject^>^ imageComposer = gcnew ImageComposerCEGUI<InventoryObject^>();
         imageComposer->ApplyYOffset = false;
         imageComposer->HotspotIndex = 0;
         imageComposer->IsScalePow2 = false;
         imageComposer->UseViewerFrame = false;
         imageComposer->Width = UI_INVENTORYICON_WIDTH;
         imageComposer->Height = UI_INVENTORYICON_HEIGHT;
         imageComposer->CenterHorizontal = true;
         imageComposer->CenterVertical = true;
         imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewImageAvailable);
         // Add to list
         imageComposers->Add(imageComposer);

         // create imagebuttons in slots
         // create widget
         CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)wndMgr.createWindow(UI_WINDOWTYPE_INVENTORYICON);
         CEGUI::Window* widget = dragger->getChildAtIdx(0);

         // size of elements
         CEGUI::USize size = CEGUI::USize(
            CEGUI::UDim(0, UI_INVENTORYICON_WIDTH + 12),
            CEGUI::UDim(0, UI_INVENTORYICON_HEIGHT + 12));

         // some settings
         dragger->setSize(size);
         dragger->setMouseInputPropagationEnabled(true);
         dragger->setMouseCursor(UI_DEFAULTARROW);
         dragger->setWantsMultiClickEvents(false);
         dragger->setDraggingEnabled(false);

         widget->setSize(size);
         widget->setFont(UI_FONT_LIBERATIONSANS10B);
         widget->setProperty(UI_PROPNAME_FRAMEENABLED, "True");
         widget->setProperty(UI_PROPNAME_BACKGROUNDENABLED, "True");

         // subscribe events
         dragger->subscribeEvent(CEGUI::DragContainer::EventDragEnded, CEGUI::Event::Subscriber(UICallbacks::Inventory::OnDragEnded));
         dragger->subscribeEvent(CEGUI::DragContainer::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Inventory::OnItemClicked));
         dragger->subscribeEvent(CEGUI::DragContainer::EventDragStarted, CEGUI::Event::Subscriber(UICallbacks::Inventory::OnDragStarted));

         // add to grid (0-based so rows - 1)
         List->addChildToPosition(dragger, i, currentInventoryRows - 1);
      }
   };

   void ControllerUI::Inventory::RemoveInventoryRow()
   {
      // Don't modify inventory grid while rearranging items,
      // and don't lower beyond min rows (even if grid is empty).
      if (IsRearrangingInventory || currentInventoryRows <= UI_INVENTORY_MIN_ROWS)
         return;

      // get windowmanager
      CEGUI::WindowManager& wndMgr = CEGUI::WindowManager::getSingleton();

      // Lower numRows here first, makes indexing easier.
      currentInventoryRows--;

      for (int i = UI_INVENTORY_COLS - 1; i >= 0; --i)
      {
         imageComposers[currentInventoryRows * UI_INVENTORY_COLS + i]->NewImageAvailable -=
            gcnew ::System::EventHandler(OnNewImageAvailable);

         // Clean up dragger window/events.
         CEGUI::DragContainer* dragger =
            (CEGUI::DragContainer*) List->getChildAtIdx(currentInventoryRows * UI_INVENTORY_COLS + i);
         List->removeChild(dragger);
         dragger->removeAllEvents();
         wndMgr.destroyWindow(dragger);
      }

      imageComposers->RemoveRange(currentInventoryRows * UI_INVENTORY_COLS, 5);

      // Resets grid dimensions.
      List->setGridDimensions(UI_INVENTORY_COLS, currentInventoryRows);
   };

   void ControllerUI::Inventory::InventoryAdd(int Index)
   {
      // get new datamodel entry
      InventoryObject^ obj = OgreClient::Singleton->Data->InventoryObjects[Index];

      if ((int)currentInventoryRows * UI_INVENTORY_COLS <= Index)
      {
         AddInventoryRow();
      }

      // if we have that many slots..
      if ((int)List->getChildCount() > Index &&
         imageComposers->Count > Index)
      {
         // if not added at the end, rearrange by moving all forward once
         if (Index < OgreClient::Singleton->Data->InventoryObjects->Count - 1)
         {
            for (int i = OgreClient::Singleton->Data->InventoryObjects->Count - 1; i >= Index; i--)
            {
               // swap views
               List->swapChildren(
                  List->getChildAtIdx(i),
                  List->getChildAtIdx(i + 1));

               SwapImageComposers(i, i + 1);
            }
         }

         CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)List->getChildAtIdx(Index);
         CEGUI::Window* imgButton = dragger->getChildAtIdx(0);

         // set new datasource on composer
         imageComposers[Index]->DataSource = obj;

         // set tooltip to name and mousecursor to target
         dragger->setTooltipText(StringConvert::CLRToCEGUI(obj->Name));
         dragger->setMouseCursor(UI_MOUSECURSOR_TARGET);	
         dragger->setID(obj->ID);
         dragger->setDraggingEnabled(true);

         if (obj->Count > 0)
            imgButton->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Count));

         List->notifyScreenAreaChanged(true);
      }
   };

   void ControllerUI::Inventory::InventoryRemove(int Index)
   {
      int childcount = (int)List->getChildCount();

      // if we have that many slots..
      if (childcount > Index &&
         imageComposers->Count > Index)
      {
         // get dragcontainer
         CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)List->getChildAtIdx(Index);
         CEGUI::Window* imgButton = dragger->getChildAtIdx(0);

         imgButton->setProperty(UI_PROPNAME_IMAGE, STRINGEMPTY);
         imgButton->setText(STRINGEMPTY);
         dragger->setTooltipText(STRINGEMPTY);
         dragger->setMouseCursor(UI_DEFAULTARROW);
         dragger->setDraggingEnabled(false);

         // reset datasource
         imageComposers[Index]->DataSource = nullptr;

         // rearrange
         for (int i = Index; i < childcount - 1; i++)
         {
            // swap views
            List->swapChildren(
               List->getChildAtIdx(i), 
               List->getChildAtIdx(i+1));

            SwapImageComposers(i, i + 1);
         }

         // Delete the empty row if we have one.
         if (OgreClient::Singleton->Data->InventoryObjects->Count <=
            (int)(currentInventoryRows - 1) * UI_INVENTORY_COLS)
         {
            RemoveInventoryRow();
         }

         List->notifyScreenAreaChanged(true);
      }
   };

   void ControllerUI::Inventory::InventoryChange(int Index)
   {
      InventoryObject^ obj = OgreClient::Singleton->Data->InventoryObjects[Index];

      // check
      if ((int)List->getChildCount() > Index)
      {
         // get imagebutton
         CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)List->getChildAtIdx(Index);
         CEGUI::Window* imgButton = dragger->getChildAtIdx(0);

         dragger->setTooltipText(StringConvert::CLRToCEGUI(obj->Name));
         dragger->setDraggingEnabled(true);

         if (obj->Count > 0)
            imgButton->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Count));
      }
   };

   void ControllerUI::Inventory::Tick(double Tick, double Span)
   {
      if (ClickObject && DoClick && OgreClient::Singleton->GameTick->CanInventoryClick())
      {
         // reset singleclick executor
         DoClick = false;

         OgreClient::Singleton->Data->TargetID = ClickObject->ID;
      }
   };

   void ControllerUI::Inventory::SwapImageComposers(unsigned int Index1, unsigned int Index2)
   {
      if ((int)Index1 < imageComposers->Count && 
          (int)Index2 < imageComposers->Count)
      {
         // swap composers
         ImageComposerCEGUI<InventoryObject^>^ swap = imageComposers[Index1];
         imageComposers[Index1] = imageComposers[Index2];
         imageComposers[Index2] = swap;
      }
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::Inventory::OnItemClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
      InventoryObjectList^ dataModels   = OgreClient::Singleton->Data->InventoryObjects;
      InventoryObject^ obj              = dataModels->GetItemByID(args.window->getID());

      if (obj)
      {
         // single clicks (delayed due to doubleclick)
         if (OgreClient::Singleton->GameTick->CanInventoryClick())
         {
            // left click targets
            if (args.button == CEGUI::MouseButton::LeftButton)
            {
               // prepare single click execution
               ControllerUI::Inventory::DoClick = true;
               ControllerUI::Inventory::ClickObject = obj;
            }

            // right click requests info window
            else if (args.button == CEGUI::MouseButton::RightButton)
            {
               OgreClient::Singleton->SendReqLookMessage(obj->ID);
            }
         }

         // double click
         else
         {
            // left doubleclick uses/applies item
            if (args.button == CEGUI::MouseButton::LeftButton)
            {
               // reset singleclick execution
               ControllerUI::Inventory::DoClick = false;
               OgreClient::Singleton->UseUnuseApply(obj);
            }

            // right doubleclick currently not assigned
            else if (args.button == CEGUI::MouseButton::RightButton)
            {
            }
         }

         OgreClient::Singleton->GameTick->DidInventoryClick();
      }

      return false;
   };

   bool UICallbacks::Inventory::OnDragStarted(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);

      CEGUI::DragContainer* drag = (CEGUI::DragContainer*)args.window;
      
      if (!drag->isDraggingEnabled())
         return true;

      ControllerUI::DraggedWindow = args.window;
      ControllerUI::Inventory::Window->setUsingAutoRenderingSurface(false);

      return true;
   };

   bool UICallbacks::Inventory::OnDragEnded(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
      const CEGUI::GridLayoutContainer* dataViews = ControllerUI::Inventory::List;
      InventoryObjectList^ dataModels = OgreClient::Singleton->Data->InventoryObjects;
      CEGUI::DragContainer* dataView = nullptr;
      InventoryObject^ dataItem = nullptr;

      // reset dragwindow
      ControllerUI::DraggedWindow = nullptr;
      ControllerUI::Inventory::Window->setUsingAutoRenderingSurface(true);

      // get index and dragcontainer
      int childcount = (int)dataViews->getChildCount();
      for (int i = 0; i < childcount; i++)
      {
         // get dragcontainer at this index
         CEGUI::DragContainer* child = 
            (CEGUI::DragContainer*)dataViews->getChildAtIdx(i);

         // match?
         if (child == args.window && dataModels->Count > i)
         {
            dataView = child;
            dataItem = dataModels[i];
            break;
         }
      }

      // found
      if (dataView != nullptr && dataItem != nullptr)
      {
         CEGUI::Window* wnd = dataView->getCurrentDropTarget();

         if (wnd)
         {
            CEGUI::Window* wndParent = wnd->getParent();

            // dropped on rootwindow?
            if (wnd == ControllerUI::GUIRoot)
            {
               // see if dropped on a container roomobject such as chest
               RoomObject^ mouseOverObj = OgreClient::Singleton->Data->RoomObjects->GetHighlightedItem();

               if (mouseOverObj && mouseOverObj->Flags->IsContainer)
               {
                  // put into object
                  OgreClient::Singleton->SendReqPut(
                     gcnew ObjectID(dataItem->ID, dataItem->Count),
                     gcnew ObjectID(mouseOverObj->ID, 0));
               }

               // drop it
               else
               {
                  // drop directly
                  if (!dataItem->IsStackable)
                     OgreClient::Singleton->SendReqDropMessage(gcnew ObjectID(dataItem->ID, 0));

                  // show amount input
                  else
                     ControllerUI::Amount::ShowValues(dataItem->ID, dataItem->Count);
               }
            }

            // dropped on objectscontents list or an entry of it
            else if (wnd == ControllerUI::ObjectContents::List ||
                     wndParent == ControllerUI::ObjectContents::List ||
                     (wndParent && wndParent->getParent() == ControllerUI::ObjectContents::List))
            {
               // datamodel
               Meridian59::Data::Models::ObjectContents^ objContents = 
                  OgreClient::Singleton->Data->ObjectContents;

               // find according roomobject
               RoomObject^ roomObj = OgreClient::Singleton->Data->RoomObjects->GetItemByID(
                  objContents->ObjectID->ID);

               // found and is a container
               if (roomObj && roomObj->Flags->IsContainer)
               {
                  // put into container
                  OgreClient::Singleton->SendReqPut(
                     gcnew ObjectID(dataItem->ID, dataItem->Count),
                     gcnew ObjectID(roomObj->ID, 0));

                  // request contents again to receive update
                  OgreClient::Singleton->SendSendObjectContents(roomObj->ID);
               }
            }

#if !VANILLA
            // other inventory slot?
            else if (wndParent == ControllerUI::Inventory::List)
            {
               // try cast to other dragcontainer
               CEGUI::DragContainer* destDrag = (::CEGUI::DragContainer*)wnd;

               if (destDrag)
               {
                  // determine the indices of the entries in the viewer/ui
                  int fromIndex = (int)dataViews->getIdxOfChild(dataView);
                  int toIndex = (int)dataViews->getIdxOfChild(destDrag);

                  // beware: these can point to empty ui inventory slots 
                  // (out of bound indices in data).
                  if (fromIndex < 0 || fromIndex >= dataModels->Count ||
                     toIndex < 0 || toIndex >= dataModels->Count)
                     return true;

                  // Set rearranging boolean so inventory doesn't modify rows.
                  ControllerUI::Inventory::IsRearrangingInventory = true;
                  // remove and reinsert at position
                  InventoryObject^ obj = dataModels[fromIndex];
                  dataModels->RemoveAt(fromIndex);
                  dataModels->Insert(toIndex, obj);

                  // Unset after inventory item move complete.
                  ControllerUI::Inventory::IsRearrangingInventory = false;

                  // tell server
                  OgreClient::Singleton->SendReqInventoryMoveMessage(
                     dataView->getID(), destDrag->getID());

                  ControllerUI::Inventory::List->notifyScreenAreaChanged(true);
               }
            }
#endif
         }
      }

      return true;
   };
};};
