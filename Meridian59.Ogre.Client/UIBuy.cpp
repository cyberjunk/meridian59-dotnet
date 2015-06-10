#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Buy::Initialize()
	{
		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_BUY_WINDOW));
		List			= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_BUY_LIST));
		OK				= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_BUY_OK));
		SumDescription	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_BUY_SUMDESC));
		SumValue		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_BUY_SUMVAL));
		
		// set multiselect
		List->setMultiSelectEnabled(true);

		// init imagecomposers list
		imageComposers = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<ObjectBase^>^>();

		// attach listener to buyinfo
		OgreClient::Singleton->Data->Buy->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnBuyPropertyChanged);
        
		// attach listener to buyinfo items
		OgreClient::Singleton->Data->Buy->Items->ListChanged += 
			gcnew ListChangedEventHandler(OnBuyListChanged);

		// subscribe selection change
		List->subscribeEvent(CEGUI::ItemListbox::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::Buy::OnListSelectionChanged));

		// subscribe OK buttno
		OK->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::Buy::OnOKClicked));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::Buy::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::Buy::OnWindowKeyUp));
	};

	void ControllerUI::Buy::Destroy()
	{	
		OgreClient::Singleton->Data->Buy->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnBuyPropertyChanged);
        
		OgreClient::Singleton->Data->Buy->Items->ListChanged -= 
			gcnew ListChangedEventHandler(OnBuyListChanged);			
	};

	void ControllerUI::Buy::OnBuyPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// tradepartner
		if (::System::String::Equals(e->PropertyName, BuyInfo::PROPNAME_TRADEPARTNER))
		{
		}

		// visible
		else if (::System::String::Equals(e->PropertyName, BuyInfo::PROPNAME_ISVISIBLE))
		{
			// set window visibility
			Window->setVisible(OgreClient::Singleton->Data->Buy->IsVisible);
			Window->moveToFront();
		}
	};

	void ControllerUI::Buy::OnBuyListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				BuyItemAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				BuyItemRemove(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Buy::BuyItemAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		TradeOfferObject^ obj = OgreClient::Singleton->Data->Buy->Items[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_BUYLISTBOXITEM);
		
		// set ID
		widget->setID(obj->ID);

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::Buy::OnItemClicked));
		
		// check
		if (widget->getChildCount() > 3)
		{
			CEGUI::Window* price	= (CEGUI::Window*)widget->getChildAtIdx(UI_BUY_CHILDINDEX_PRICE);
			CEGUI::Window* icon		= (CEGUI::Window*)widget->getChildAtIdx(UI_BUY_CHILDINDEX_ICON);
			CEGUI::Window* name		= (CEGUI::Window*)widget->getChildAtIdx(UI_BUY_CHILDINDEX_NAME);
			CEGUI::Editbox* amount	= (CEGUI::Editbox*)widget->getChildAtIdx(UI_BUY_CHILDINDEX_AMOUNT);

			// subscribe event to focusleave on textbox
			amount->subscribeEvent(CEGUI::Editbox::EventDeactivated, CEGUI::Event::Subscriber(UICallbacks::Buy::OnItemAmountDeactivated));
			amount->subscribeEvent(CEGUI::Editbox::EventTextAccepted, CEGUI::Event::Subscriber(UICallbacks::Buy::OnItemAmountDeactivated));

			// get color
			::CEGUI::Colour color = ::CEGUI::Colour(
				NameColors::GetColorFor(obj->Flags));

			// set color and name
			name->setProperty(UI_PROPNAME_NORMALTEXTCOLOUR, ::CEGUI::PropertyHelper<::CEGUI::Colour>::toString(color));
			name->setText(StringConvert::CLRToCEGUI(obj->Name));

			// set price
			price->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Price));

			// set default amount
			amount->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->Count));
			amount->setVisible(obj->IsStackable);
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
		imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnNewBuyItemImageAvailable);

		// insert composer into list at index
		imageComposers->Insert(Index, imageComposer);

		// set image
		imageComposer->DataSource = obj;
	};

	void ControllerUI::Buy::BuyItemRemove(int Index)
	{
		// check
		if ((int)List->getItemCount() > Index)		
			List->removeItem(List->getItemFromIndex(Index));

		// remove imagecomposer
		if (imageComposers->Count > Index)
		{
			// reset (detaches listeners!)
			imageComposers[Index]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewBuyItemImageAvailable);
			imageComposers[Index]->DataSource = nullptr;
			
			// remove from list
			imageComposers->RemoveAt(Index);
		}
	};

	void ControllerUI::Buy::OnNewBuyItemImageAvailable(Object^ sender, ::System::EventArgs^ e)
	{
		ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
		int index = imageComposers->IndexOf(imageComposer);

		if (index > -1 && (int)List->getItemCount() > index)
		{
			// get staticimage
			CEGUI::ItemEntry* img	= List->getItemFromIndex(index);
			CEGUI::Window* icon		= (CEGUI::Window*)img->getChildAtIdx(UI_BUY_CHILDINDEX_ICON);
				
			// set new image on ui widget
			icon->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);
		}
	};

	void ControllerUI::Buy::CalculateSum()
	{
		ObjectBaseList<TradeOfferObject^>^ dataItems = OgreClient::Singleton->Data->Buy->Items;

		int sum = 0;
		int itemcount = (int)List->getItemCount();
		for(int i = 0; i < itemcount; i++)
		{
			if (List->isItemSelected(i) && dataItems->Count > i)
			{
				if (dataItems[i]->IsStackable)
					sum += dataItems[i]->Count * dataItems[i]->Price;

				else
					sum += dataItems[i]->Price;
			}
		}

		SumValue->setText(CEGUI::PropertyHelper<int>::toString(sum));
	};

	bool UICallbacks::Buy::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// single rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};

	bool UICallbacks::Buy::OnItemAmountDeactivated(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (CEGUI::MouseEventArgs&)e;		
		CEGUI::Editbox* box			= (CEGUI::Editbox*)args.window;
		CEGUI::ItemEntry* itemEntry = (CEGUI::ItemEntry*)box->getParent();
		CEGUI::ItemListbox* list	= ControllerUI::Buy::List;
		
		// get user value from box
		CEGUI::String boxText = box->getText();
				
		// revert empty input to 1
		if (boxText == STRINGEMPTY)
		{
			boxText = "1";
			box->setText(boxText);
		}

		// data models
		ObjectBaseList<TradeOfferObject^>^ dataItems = 
			OgreClient::Singleton->Data->Buy->Items;

		// get index of clicked buff/widget in listbox
		int index = (int)list->getItemIndex(itemEntry);

		// found ?
		if (dataItems->Count > index)
			dataItems[index]->Count = ::CEGUI::PropertyHelper<unsigned int>::fromString(boxText);
		
		// refresh sum
		ControllerUI::Buy::CalculateSum();

		return true;
	};
	
	bool UICallbacks::Buy::OnListSelectionChanged(const CEGUI::EventArgs& e)
	{
		// update sum
		ControllerUI::Buy::CalculateSum();

		return true;
	};

	bool UICallbacks::Buy::OnOKClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);		
		CEGUI::ItemListbox* list = ControllerUI::Buy::List;
		ObjectBaseList<TradeOfferObject^>^ dataItems = OgreClient::Singleton->Data->Buy->Items;
		
		::System::Collections::Generic::List<ObjectID^>^ idList = 
			gcnew ::System::Collections::Generic::List<ObjectID^>();

		// build buy ids
		for(size_t i = 0; i < list->getItemCount(); i++)	
			if (list->isItemSelected(i) && dataItems->Count > (int)i)			
				idList->Add(gcnew ObjectID(dataItems[(int)i]->ID, dataItems[(int)i]->Count));
		
		if (OgreClient::Singleton->Data->Buy->TradePartner != nullptr)
		{
			// send
			OgreClient::Singleton->SendReqBuyItemsMessage(
				OgreClient::Singleton->Data->Buy->TradePartner->ID,
				idList->ToArray());
		}

		// hide
		OgreClient::Singleton->Data->Buy->IsVisible = false;
		ControllerUI::ActivateRoot();

		return true;
	};

	bool UICallbacks::Buy::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// clear (view will react)
			OgreClient::Singleton->Data->Buy->IsVisible = false;
			OgreClient::Singleton->Data->Buy->Clear(true);

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	};

	bool UICallbacks::Buy::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// set not visible in datalayer (view will react)
		OgreClient::Singleton->Data->Buy->IsVisible = false;
		OgreClient::Singleton->Data->Buy->Clear(true);

		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}
};};