#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::RoomEnchantments::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_ROOMENCHANTMENTS_WINDOW));
		Grid	= static_cast<CEGUI::GridLayoutContainer*>(Window->getChild(UI_NAME_ROOMENCHANTMENTS_GRID));

		// attach listener to room enchantments
		OgreClient::Singleton->Data->RoomBuffs->ListChanged += 
			gcnew ListChangedEventHandler(OnBuffListChanged);

		// amount of entries the buffgrid can hold
		const int entries = UI_ROOMENCHANTMENTS_COLS * UI_ROOMENCHANTMENTS_ROWS;

		// set dimension (no. of items per row and no. of cols)
		Grid->setGridDimensions(UI_ROOMENCHANTMENTS_COLS, UI_ROOMENCHANTMENTS_ROWS);
		
		// create image composers for buffslots
		imageComposersBuffs = gcnew array<ImageComposerCEGUI<ObjectBase^>^>(entries);

		for(int i = 0; i < entries; i++)
		{
			imageComposersBuffs[i] = gcnew ImageComposerCEGUI<ObjectBase^>();
			imageComposersBuffs[i]->ApplyYOffset = false;
			imageComposersBuffs[i]->HotspotIndex = 0;
			imageComposersBuffs[i]->IsScalePow2 = false;
			imageComposersBuffs[i]->UseViewerFrame = false;
			imageComposersBuffs[i]->Width = UI_BUFFICON_WIDTH;
			imageComposersBuffs[i]->Height = UI_BUFFICON_HEIGHT;
			imageComposersBuffs[i]->CenterHorizontal = true;
			imageComposersBuffs[i]->CenterVertical = true;
			imageComposersBuffs[i]->NewImageAvailable += gcnew ::System::EventHandler(OnNewBuffImageAvailable);
		}
			
		// create imagebuttons in slots
		for(int i = 0; i < entries; i++)
		{
			// create widget
			CEGUI::Window* widget = (CEGUI::Window*)wndMgr->createWindow(UI_WINDOWTYPE_BUFFICON);

			// some settings
			widget->setSize(CEGUI::USize(CEGUI::UDim(0, UI_BUFFICON_WIDTH), CEGUI::UDim(0, UI_BUFFICON_HEIGHT)));
			widget->setMouseCursor(UI_DEFAULTARROW);

#ifdef _DEBUG
			widget->setText(StringConvert::CLRToCEGUI(i.ToString()));
#endif
			// subscribe click
			widget->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::RoomEnchantments::OnBuffMouseClick));

			// add
			Grid->addChild(widget);
		}

		// subscribe mouse events
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::RoomEnchantments::OnMouseDown));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::RoomEnchantments::OnMouseUp));		
	};

	void ControllerUI::RoomEnchantments::Destroy()
	{	 
		// detach listener from room enchantments
		OgreClient::Singleton->Data->RoomBuffs->ListChanged -= 
			gcnew ListChangedEventHandler(OnBuffListChanged);
		
		// amount of entries the buffgrid can hold
		const int entries = UI_ROOMENCHANTMENTS_COLS * UI_ROOMENCHANTMENTS_ROWS;

		for(int i = 0; i < entries; i++)
			imageComposersBuffs[i]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewBuffImageAvailable);		
	};

	void ControllerUI::RoomEnchantments::OnNewBuffImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
		int index = ::System::Array::IndexOf(imageComposersBuffs, imageComposer);

		if ((int)Grid->getChildCount() > index)
		{
			// get imagebutton
			CEGUI::Window* imgButton = Grid->getChildAtIdx(index);
			
			imgButton->setProperty(UI_PROPNAME_NORMALIMAGE, *imageComposer->Image->TextureName);
			imgButton->setProperty(UI_PROPNAME_HOVERIMAGE, *imageComposer->Image->TextureName);
			imgButton->setProperty(UI_PROPNAME_PUSHEDIMAGE, *imageComposer->Image->TextureName);
		}
	};

	void ControllerUI::RoomEnchantments::OnBuffListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				BuffAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				BuffRemove(e->NewIndex);
				break;
		}
	};

	void ControllerUI::RoomEnchantments::BuffAdd(int Index)
	{
		// get new datamodel entry
		ObjectBase^ buffObject = OgreClient::Singleton->Data->RoomBuffs[Index];

		// if we have that many slots..
		if ((int)Grid->getChildCount() > Index &&
			imageComposersBuffs->Length > Index)
		{			
			// get imagebutton
			CEGUI::Window* imgButton = Grid->getChildAtIdx(Index);
			
			// set new datasource on composer
			imageComposersBuffs[Index]->DataSource = buffObject;

			// set tooltip to name and mousecursor to target
			imgButton->setID(buffObject->ID);
			imgButton->setTooltipText(StringConvert::CLRToCEGUI(buffObject->Name));
			imgButton->setMouseCursor(UI_MOUSECURSOR_HAND);
		}
	};

	void ControllerUI::RoomEnchantments::BuffRemove(int Index)
	{
		int childcount = Grid->getChildCount();

		// if we have that many slots..
		if (childcount > Index &&
			imageComposersBuffs->Length > Index)
		{
			// get imagebutton
			CEGUI::Window* imgButton = Grid->getChildAtIdx(Index);
			
			imgButton->setProperty(UI_PROPNAME_NORMALIMAGE, STRINGEMPTY);
			imgButton->setProperty(UI_PROPNAME_HOVERIMAGE, STRINGEMPTY);
			imgButton->setProperty(UI_PROPNAME_PUSHEDIMAGE, STRINGEMPTY);
			imgButton->setTooltipText(STRINGEMPTY);
			imgButton->setMouseCursor(UI_DEFAULTARROW);
			imgButton->setID(0);

			// reset datasource
			imageComposersBuffs[Index]->DataSource = nullptr;

			// rearrange
			ImageComposerCEGUI<ObjectBase^>^ swap;
			for (int i = Index; i < childcount - 1; i++)
			{
				// swap views
				Grid->swapChildren(
					Grid->getChildAtIdx(i), 
					Grid->getChildAtIdx(i+1));

				// swap composers
				swap = imageComposersBuffs[i];
				imageComposersBuffs[i] = imageComposersBuffs[i+1];
				imageComposersBuffs[i+1] = swap;
			}
		}
	};

	bool UICallbacks::RoomEnchantments::OnBuffMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::Window* itm			= (CEGUI::Window*)args.window;

		// single rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};

	bool UICallbacks::RoomEnchantments::OnMouseDown(const CEGUI::EventArgs& e)
	{
		// set this window as moving one
		ControllerUI::MovingWindow = ControllerUI::RoomEnchantments::Window;

		return true;
	};

	bool UICallbacks::RoomEnchantments::OnMouseUp(const CEGUI::EventArgs& e)
	{
		// unset this window as moving one
		ControllerUI::MovingWindow = nullptr;

		return true;
	};
};};
