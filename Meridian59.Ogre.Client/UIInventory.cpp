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

			widget->setSize(size);
			widget->setFont(UI_FONT_LIBERATIONSANS10B);
			widget->setProperty(UI_PROPNAME_FRAMEENABLED, "True");
			widget->setProperty(UI_PROPNAME_BACKGROUNDENABLED, "True");

#ifdef _DEBUG
			widget->setText(CEGUI::PropertyHelper<int>::toString(i));
#endif
			// subscribe events
			dragger->subscribeEvent(CEGUI::DragContainer::EventDragEnded, CEGUI::Event::Subscriber(UICallbacks::Inventory::OnDragEnded));
			dragger->subscribeEvent(CEGUI::DragContainer::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Inventory::OnItemClicked));
		
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
			CEGUI::DragContainer* dragger = (CEGUI::DragContainer*)List->getChildAtIdx(Index);
			CEGUI::Window* imgButton = dragger->getChildAtIdx(0);
			
			// set new datasource on composer
			imageComposers[Index]->DataSource = obj;

			// set tooltip to name and mousecursor to target
			dragger->setTooltipText(StringConvert::CLRToCEGUI(obj->Name));
			dragger->setMouseCursor(UI_MOUSECURSOR_TARGET);	

			if (obj->Count > 0)
				imgButton->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Count));				
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
			
			// reset datasource
			imageComposers[Index]->DataSource = nullptr;

			// rearrange
			ImageComposerCEGUI<InventoryObject^>^ swap;
			for (int i = Index; i < childcount - 1; i++)
			{
				// swap views
				List->swapChildren(
					List->getChildAtIdx(i), 
					List->getChildAtIdx(i+1));

				// swap composers
				swap = imageComposers[i];
				imageComposers[i] = imageComposers[i+1];
				imageComposers[i+1] = swap;
			}
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

			if (obj->Count > 0)
				imgButton->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Count));
		}
	};

	void ControllerUI::Inventory::Update()
	{
		if (DoClick && (OgreClient::Singleton->GameTick->Current - TickMouseClick > UI_MOUSE_CLICKDELAY))
		{
			// reset singleclick executor
			DoClick = false;

			OgreClient::Singleton->Data->TargetID = 
				OgreClient::Singleton->Data->InventoryObjects[ClickIndex]->ID;			
		}
	};

	bool UICallbacks::Inventory::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);
		const CEGUI::GridLayoutContainer* dataViews = ControllerUI::Inventory::List;
		InventoryObjectList^ dataModels = OgreClient::Singleton->Data->InventoryObjects;

		// get index of clicked buff/widget in grid
		int index = -1;
		for (int i = 0; i < (int)dataViews->getChildCount(); i++)
		{			
			if (dataViews->getChildAtIdx(i) == args.window)
			{
				index = i;
				break;
			}
		}

		// found ?
		if (index > -1 && dataModels->Count > index)
		{
			// get id of this buff
			unsigned int id = dataModels[index]->ID;

			long long span = OgreClient::Singleton->GameTick->Current -
				ControllerUI::Inventory::TickMouseClick;

			// new
			if (span > UI_MOUSE_CLICKDELAY)
			{
				if (args.button == CEGUI::MouseButton::LeftButton)
				{
					// prepare single click execution
					ControllerUI::Inventory::DoClick = true;
					ControllerUI::Inventory::ClickIndex = index;
				}
				else if (args.button == CEGUI::MouseButton::RightButton)
				{
					OgreClient::Singleton->SendReqLookMessage(id);					
				}
			}

			// double click
			else
			{
				if (args.button == CEGUI::MouseButton::LeftButton)
				{
					// reset singleclick execution
					ControllerUI::Inventory::DoClick = false;

					OgreClient::Singleton->UseUnuseApply(dataModels[index]);
				}
				else if (args.button == CEGUI::MouseButton::RightButton)
				{				
				}
			}

			// save tick to execute click later
			ControllerUI::Inventory::TickMouseClick = OgreClient::Singleton->GameTick->Current;
		}

		return false;
	};

	bool UICallbacks::Inventory::OnDragEnded(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
		const CEGUI::GridLayoutContainer* dataViews = ControllerUI::Inventory::List;
		InventoryObjectList^ dataModels = OgreClient::Singleton->Data->InventoryObjects;
		CEGUI::DragContainer* dataView = nullptr;
		InventoryObject^ dataItem = nullptr;
		
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

			// dropped on rootwindow? drop item
			if (wnd == ControllerUI::GUIRoot)
			{
				// drop directly
				if (!dataItem->IsStackable)
					OgreClient::Singleton->SendReqDropMessage(gcnew ObjectID(dataItem->ID, 0));

				// show amount input
				else			
					ControllerUI::Amount::ShowValues(dataItem->ID, dataItem->Count);			
			}
		}
		
		return true;
	};
};};
