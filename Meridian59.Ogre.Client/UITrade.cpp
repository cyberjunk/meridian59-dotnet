#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Trade::Initialize()
	{
		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_TRADE_WINDOW));
		NameYou		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_TRADE_NAMEYOU));
		NamePartner	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_TRADE_NAMEPARTNER));
		ListYou		= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_TRADE_LISTYOU));
		ListPartner	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_TRADE_LISTPARTNER));
		Offer		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_TRADE_OFFER));
		Accept		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_TRADE_ACCEPT));

		// attach listener to trade data
		OgreClient::Singleton->Data->Trade->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnTradePropertyChanged);
        
		// attach listener to own items
		OgreClient::Singleton->Data->Trade->ItemsYou->ListChanged += 
			gcnew ListChangedEventHandler(OnItemsYouListChanged);

		// attach listener to offered items
		OgreClient::Singleton->Data->Trade->ItemsPartner->ListChanged += 
			gcnew ListChangedEventHandler(OnItemsPartnerListChanged);
		
		// init imagecomposers list
		imageComposersYou = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>();
		imageComposersPartner = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>();

		// accept not visible in defaultstate
		Accept->setVisible(false);

		// subscribe selection change
		ListYou->subscribeEvent(CEGUI::ItemListbox::EventDragDropItemDropped, CEGUI::Event::Subscriber(UICallbacks::Trade::OnListYouItemDropped));

		// subscribe OK buttno
		Offer->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Trade::OnOfferClicked));
		Accept->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Trade::OnAcceptClicked));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::Trade::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Trade::OnKeyUp));
	};

	void ControllerUI::Trade::Destroy()
	{	 
		// detach listener from trade data
		OgreClient::Singleton->Data->Trade->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnTradePropertyChanged);
        
		// detach listener from own items
		OgreClient::Singleton->Data->Trade->ItemsYou->ListChanged -= 
			gcnew ListChangedEventHandler(OnItemsYouListChanged);

		// detach listener from offered items
		OgreClient::Singleton->Data->Trade->ItemsPartner->ListChanged -= 
			gcnew ListChangedEventHandler(OnItemsPartnerListChanged);			
	};

	void ControllerUI::Trade::OnTradePropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		TradeInfo^ tradeInfo = OgreClient::Singleton->Data->Trade;

		// visible
		if (::System::String::Equals(e->PropertyName, TradeInfo::PROPNAME_ISVISIBLE))
		{
			// hide or show
			Window->setVisible(tradeInfo->IsVisible);

			// bring to front
			if (tradeInfo->IsVisible)
				Window->moveToFront();
		}

		// pending
		else if (::System::String::Equals(e->PropertyName, TradeInfo::PROPNAME_ISPENDING))
		{

		}

		// itemsyouset
		else if (::System::String::Equals(e->PropertyName, TradeInfo::PROPNAME_ISITEMSYOUSET))
		{
			Offer->setVisible(!tradeInfo->IsItemsYouSet);
			
			Accept->setVisible(
				tradeInfo->IsItemsYouSet &&
				tradeInfo->IsItemsPartnerSet &&
                !tradeInfo->IsBackgroundOffer);
		}

		// itemspartnerset
		else if (::System::String::Equals(e->PropertyName, TradeInfo::PROPNAME_ISITEMSPARTNERSET))
		{
			Accept->setVisible(
                tradeInfo->IsItemsYouSet &&
                tradeInfo->IsItemsPartnerSet &&
                !tradeInfo->IsBackgroundOffer);
		}

		// tradepartner
		else if (::System::String::Equals(e->PropertyName, TradeInfo::PROPNAME_TRADEPARTNER))
		{
			if (tradeInfo->TradePartner != nullptr)
			{
				NamePartner->setText(StringConvert::CLRToCEGUI(tradeInfo->TradePartner->Name));

				// set visibility based on drawing type
				NamePartner->setVisible(
					tradeInfo->TradePartner->Flags->Drawing != ObjectFlags::DrawingType::Invisible);
			}
		}
	};
	
	void ControllerUI::Trade::OnItemsYouListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				ItemYouAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				ItemYouRemove(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Trade::OnItemsPartnerListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				ItemPartnerAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				ItemPartnerRemove(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Trade::ItemYouAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		ObjectBase^ obj = OgreClient::Singleton->Data->Trade->ItemsYou[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_TRADELISTBOXITEM);
		
		// set ID
		widget->setID(obj->ID);

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::Trade::OnItemYouClicked));
		
		// forwarder for drops on widget
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventDragDropItemDropped,
			CEGUI::Event::Subscriber(UICallbacks::Trade::OnListYouItemDropped));

		// check
		if (widget->getChildCount() > 2)
		{
			CEGUI::Window* icon		= (CEGUI::Window*)widget->getChildAtIdx(UI_TRADE_CHILDINDEX_ICON);
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_TRADE_CHILDINDEX_NAME);
			CEGUI::Editbox* amount	= (CEGUI::Editbox*)widget->getChildAtIdx(UI_TRADE_CHILDINDEX_AMOUNT);

			// subscribe event to focusleave on textbox
			amount->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Trade::OnItemYouAmountDeactivated));
			amount->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Trade::OnItemYouAmountDeactivated));

			// forwarder for drops on widget
			icon->subscribeEvent(
				CEGUI::Window::EventDragDropItemDropped,
				CEGUI::Event::Subscriber(UICallbacks::Trade::OnListYouItemDropped));
			name->subscribeEvent(
				CEGUI::Window::EventDragDropItemDropped,
				CEGUI::Event::Subscriber(UICallbacks::Trade::OnListYouItemDropped));
			amount->subscribeEvent(
				CEGUI::Editbox::EventDragDropItemDropped,
				CEGUI::Event::Subscriber(UICallbacks::Trade::OnListYouItemDropped));

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

		// insert widget in ui-list
		if ((int)ListYou->getItemCount() > Index)
			ListYou->insertItem(widget, ListYou->getItemFromIndex(Index));
		
		// or add
		else
			ListYou->addItem(widget);
		
		// fix a big with last item not highlighted
		ListYou->notifyScreenAreaChanged(true);
		
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
		imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewItemYouImageAvailable);
		
		// insert composer into list at index
		imageComposersYou->Insert(Index, imageComposer);
		
		// create image
		imageComposer->DataSource = obj;
	};

	void ControllerUI::Trade::ItemYouRemove(int Index)
	{
		// check
		if ((int)ListYou->getItemCount() > Index)		
			ListYou->removeItem(ListYou->getItemFromIndex(Index));

		// remove imagecomposer
		if (imageComposersYou->Count > Index)
		{
			// reset (detaches listeners!)
			imageComposersYou[Index]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewItemYouImageAvailable);
			imageComposersYou[Index]->DataSource = nullptr;
			
			// remove from list
			imageComposersYou->RemoveAt(Index);
		}
	};

	void ControllerUI::Trade::ItemPartnerAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		ObjectBase^ obj = OgreClient::Singleton->Data->Trade->ItemsPartner[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_TRADELISTBOXITEM);
		
		// set id
		widget->setID(obj->ID);

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::Trade::OnItemPartnerClicked));
		
		// check
		if (widget->getChildCount() > 2)
		{
			CEGUI::Window* icon		= (CEGUI::Window*)widget->getChildAtIdx(UI_TRADE_CHILDINDEX_ICON);
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_TRADE_CHILDINDEX_NAME);
			CEGUI::Editbox* amount	= (CEGUI::Editbox*)widget->getChildAtIdx(UI_TRADE_CHILDINDEX_AMOUNT);

			amount->setReadOnly(true);

			// subscribe event to focusleave on textbox
			amount->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Buy::OnItemAmountDeactivated));
			amount->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Buy::OnItemAmountDeactivated));

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

		// insert widget in ui-list
		if ((int)ListPartner->getItemCount() > Index)
			ListPartner->insertItem(widget, ListPartner->getItemFromIndex(Index));
		
		// or add
		else
			ListPartner->addItem(widget);
		
		// fix a big with last item not highlighted
		ListPartner->notifyScreenAreaChanged(true);
		
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
		imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewItemPartnerImageAvailable);
		
		// insert composer into list at index
		imageComposersPartner->Insert(Index, imageComposer);

		// create image
		imageComposer->DataSource = obj;
	};

	void ControllerUI::Trade::ItemPartnerRemove(int Index)
	{
		// check
		if ((int)ListPartner->getItemCount() > Index)		
			ListPartner->removeItem(ListPartner->getItemFromIndex(Index));

		// remove imagecomposer
		if (imageComposersPartner->Count > Index)
		{
			// reset (detaches listeners!)
			imageComposersPartner[Index]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewItemPartnerImageAvailable);
			imageComposersPartner[Index]->DataSource = nullptr;
			
			// remove from list
			imageComposersPartner->RemoveAt(Index);
		}
	};

	void ControllerUI::Trade::OnNewItemYouImageAvailable(Object^ sender, ::System::EventArgs^ e)
	{
		ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
		int index = imageComposersYou->IndexOf(imageComposer);

		if (index > -1 && (int)ListYou->getItemCount() > index)
		{
			// get staticimage
			CEGUI::ItemEntry* img	= ListYou->getItemFromIndex(index);
			CEGUI::Window* icon		= (CEGUI::Window*)img->getChildAtIdx(UI_TRADE_CHILDINDEX_ICON);
				
			// set new image on ui widget
			icon->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
		}
	};
	
	void ControllerUI::Trade::OnNewItemPartnerImageAvailable(Object^ sender, ::System::EventArgs^ e)
	{
		ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
		int index = imageComposersPartner->IndexOf(imageComposer);

		if (index > -1 && (int)ListPartner->getItemCount() > index)
		{
			// get staticimage
			CEGUI::ItemEntry* img	= ListPartner->getItemFromIndex(index);
			CEGUI::Window* icon		= (CEGUI::Window*)img->getChildAtIdx(UI_TRADE_CHILDINDEX_ICON);
				
			// set new image on ui widget
			icon->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
		}
	};

	bool UICallbacks::Trade::OnOfferClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);		
		CEGUI::ItemListbox* list = ControllerUI::Trade::ListYou;
		TradeInfo^ tradeInfo = OgreClient::Singleton->Data->Trade;
		ObjectBaseList<ObjectBase^>^ dataItems = tradeInfo->ItemsYou;
		
		if (tradeInfo->TradePartner != nullptr)
		{
			::System::Collections::Generic::List<ObjectID^>^ idList = 
				gcnew ::System::Collections::Generic::List<ObjectID^>();

			// build trade ids, DO NOT USE count on objectmodel (nonupdated)
			int count = (int)list->getItemCount();
			for(int i = 0; i < count; i++)
			{
				if (dataItems->Count > i)
				{
					// get text from box
					CEGUI::String strItemCount = list->getItemFromIndex(i)->getChildAtIdx(
						UI_TRADE_CHILDINDEX_AMOUNT)->getText();

					if (strItemCount == STRINGEMPTY)
						strItemCount = "1";

					idList->Add(gcnew ObjectID(
						dataItems[i]->ID, 
						CEGUI::PropertyHelper<unsigned int>::fromString(strItemCount)));
				}
			}

			// either send reqoffer or reqcounteroffer		
			if (!tradeInfo->IsBackgroundOffer)			
				OgreClient::Singleton->SendReqOffer(tradeInfo->TradePartner, idList->ToArray());
			
			else			
				OgreClient::Singleton->SendReqCounterOffer(idList->ToArray());				
		}

		return true;
	};

	bool UICallbacks::Trade::OnAcceptClicked(const CEGUI::EventArgs& e)
	{
		OgreClient::Singleton->SendAcceptOffer();

		return true;
	};

	bool UICallbacks::Trade::OnItemYouClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// single rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};

	bool UICallbacks::Trade::OnItemPartnerClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// single rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};

	bool UICallbacks::Trade::OnItemYouAmountDeactivated(const CEGUI::EventArgs& e)
	{		
		return true;
	};
	
	bool UICallbacks::Trade::OnListYouItemDropped(const CEGUI::EventArgs& e)
	{
		const CEGUI::DragDropEventArgs& args		= (CEGUI::DragDropEventArgs&)e;
		CEGUI::GridLayoutContainer* inventoryGrid	= ControllerUI::Inventory::List;
		CEGUI::Window* parent	= args.dragDropItem->getParent();
		
		InventoryObjectList^ dataModels = OgreClient::Singleton->Data->InventoryObjects;

		// from inventory
		if (parent == inventoryGrid)
		{
			int index = (int)inventoryGrid->getIdxOfChild(args.dragDropItem);
			
			// create an entry in our offerlist from the inventory datamodel
			if (dataModels->Count > index)
			{
				if (!OgreClient::Singleton->Data->Trade->ItemsYou->Contains(dataModels[index]))
					OgreClient::Singleton->Data->Trade->ItemsYou->Add(dataModels[index]);					
			}
		}

		return true;
	}

	bool UICallbacks::Trade::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// cancel if pending
		if (OgreClient::Singleton->Data->Trade->IsPending)
			OgreClient::Singleton->SendCancelOffer();
		
		// clear (view will react)
		OgreClient::Singleton->Data->Trade->Clear(true);

		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}

	bool UICallbacks::Trade::OnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// cancel if pending
			if (OgreClient::Singleton->Data->Trade->IsPending)
				OgreClient::Singleton->SendCancelOffer();
		
			// clear (view will react)
			OgreClient::Singleton->Data->Trade->Clear(true);

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}
};};