#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Inventory::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_INVENTORY_WINDOW));
		Pane			= static_cast<CEGUI::ScrollablePane*>(Window->getChild(UI_NAME_INVENTORY_PANE));
		List			= static_cast<CEGUI::GridLayoutContainer*>(Pane->getChild(UI_NAME_INVENTORY_LIST));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutInventory->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutInventory->getSize());

		// attach listener to inventory
		OgreClient::Singleton->Data->InventoryObjects->ListChanged += 
			gcnew ListChangedEventHandler(OnInventoryListChanged);

		// amount of entries the buffgrid can hold
		const int entries = UI_INVENTORY_COLS * UI_INVENTORY_ROWS;

		// set dimension (no. of items per row and no. of cols)
		List->setGridDimensions(UI_INVENTORY_COLS, UI_INVENTORY_ROWS);

		// create image composers for slots
		imageComposers = gcnew array<ImageComposerCEGUI<InventoryObject^>^>(entries);

		for(int i = 0; i < entries; i++)
		{
			imageComposers[i] = gcnew ImageComposerCEGUI<InventoryObject^>();
			imageComposers[i]->ApplyYOffset = false;
			imageComposers[i]->HotspotIndex = 0;
			imageComposers[i]->IsScalePow2 = false;
			imageComposers[i]->UseViewerFrame = false;
			imageComposers[i]->Width = UI_INVENTORYICON_WIDTH;
			imageComposers[i]->Height = UI_INVENTORYICON_HEIGHT;
			imageComposers[i]->CenterHorizontal = true;
			imageComposers[i]->CenterVertical = true;
			imageComposers[i]->NewImageAvailable += gcnew ::System::EventHandler(OnNewImageAvailable);
		}

		// create imagebuttons in slots
		for(int i = 0; i < entries; i++)
		{
			// create widget
			CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)wndMgr->createWindow(UI_WINDOWTYPE_INVENTORYICON);
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

			// add
			List->addChild(dragger);
		}

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

	    // amount of entries the buffgrid can hold
		const int entries = UI_INVENTORY_COLS * UI_INVENTORY_ROWS;
		
		for(int i = 0; i < entries; i++)
			imageComposers[i]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewImageAvailable);		     			
	};

	void ControllerUI::Inventory::OnNewImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		ImageComposerCEGUI<InventoryObject^>^ imageComposer = (ImageComposerCEGUI<InventoryObject^>^)sender;
		int index = ::System::Array::IndexOf(imageComposers, imageComposer);

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

	void ControllerUI::Inventory::InventoryAdd(int Index)
	{
		// get new datamodel entry
		InventoryObject^ obj = OgreClient::Singleton->Data->InventoryObjects[Index];

		// if we have that many slots..
		if ((int)List->getChildCount() > Index &&
			imageComposers->Length > Index)
		{		
			// if not added at the end, rearrange by moving all forward once
			if (Index < OgreClient::Singleton->Data->InventoryObjects->Count - 1)
				for (int i = OgreClient::Singleton->Data->InventoryObjects->Count - 1; i >= Index; i--)
				{
					// swap views
					List->swapChildren(
						List->getChildAtIdx(i),
						List->getChildAtIdx(i + 1));

					SwapImageComposers(i, i + 1);
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
		int childcount = List->getChildCount();

		// if we have that many slots..
		if (childcount > Index &&
			imageComposers->Length > Index)
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

			imgButton->setTooltipText(StringConvert::CLRToCEGUI(obj->Name));
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
		if ((int)Index1 < imageComposers->Length && 
			(int)Index2 < imageComposers->Length)
		{
			// swap composers
			ImageComposerCEGUI<InventoryObject^>^ swap = imageComposers[Index1];
			imageComposers[Index1] = imageComposers[Index2];
			imageComposers[Index2] = swap;
		}
	};

	bool UICallbacks::Inventory::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		InventoryObjectList^ dataModels	  = OgreClient::Singleton->Data->InventoryObjects;
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

		ControllerUI::DraggedWindow = args.window;

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

#if !VANILLA
				// other inventory slot?
				else if (wnd->getParent() == ControllerUI::Inventory::List)
				{
					// try cast to other dragcontainer
					CEGUI::DragContainer* destDrag = (::CEGUI::DragContainer*)wnd;

					if (destDrag)
					{
						// determine the indices of the entries in the viewer/ui
						size_t fromIndex = dataViews->getIdxOfChild(dataView);
						size_t toIndex = dataViews->getIdxOfChild(destDrag);

						// beware: these can point to empty ui inventory slots 
						// (out of bound indices in data).
						if ((int)fromIndex < 0 || (int)fromIndex >= dataModels->Count ||
							(int)toIndex < 0 || (int)toIndex >= dataModels->Count)
							return true;

						// remove and reinsert at position
						InventoryObject^ obj = dataModels[fromIndex];
						dataModels->RemoveAt(fromIndex);
						dataModels->Insert(toIndex, obj);

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
