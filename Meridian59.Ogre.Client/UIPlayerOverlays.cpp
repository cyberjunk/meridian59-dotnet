#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::PlayerOverlays::Initialize()
	{	
		// images are so small for big renderwindows... let's scale them all up a bit
		scale = guiRoot->getPixelSize().d_width / 800.0f;

		// init imagecomposers list
		imageComposers = gcnew ::System::Collections::Generic::List<ImageComposerCEGUI<PlayerOverlay^>^>();
		
		// init list to store overlay window references
		overlayWindows = new ::std::vector<::CEGUI::Window*>();

		// attach listener to playeroverlays list
		OgreClient::Singleton->Data->PlayerOverlays->ListChanged += 
			gcnew ListChangedEventHandler(OnPlayerOverlaysListChanged);				
	};

	void ControllerUI::PlayerOverlays::Destroy()
	{
		// detach listener
		OgreClient::Singleton->Data->PlayerOverlays->ListChanged -= 
			gcnew ListChangedEventHandler(OnPlayerOverlaysListChanged);

		delete overlayWindows;
	};

	bool ControllerUI::PlayerOverlays::IsOverlayWindow(::CEGUI::Window* Window)
	{
		if (!Window)
			return false;

		for (size_t i = 0; i < overlayWindows->size(); i++)		
			if (Window == overlayWindows->at(i))
				return true;

		return false;
	};

	void ControllerUI::PlayerOverlays::OnPlayerOverlaysListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				PlayerOverlayAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				PlayerOverlayRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				PlayerOverlayChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::PlayerOverlays::PlayerOverlayAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		PlayerOverlay^ obj = OgreClient::Singleton->Data->PlayerOverlays[Index];

		// create widget (item)
		CEGUI::Window* widget = (CEGUI::Window*)wndMgr->createWindow(
			UI_WINDOWTYPE_PLAYEROVERLAY);
		
		// set properties
		//widget->setID(obj->ID);
		widget->setProperty(UI_PROPNAME_FRAMEENABLED, "false");
		widget->setProperty(UI_PROPNAME_BACKGROUNDENABLED, "false");
		widget->setMousePassThroughEnabled(true);
		widget->setMouseInputPropagationEnabled(true);
		widget->setName(UI_PLAYEROVERLAY_WIDGETPREFIX + ::CEGUI::PropertyHelper<::CEGUI::uint32>::toString(obj->ID));
		widget->releaseInput();
		widget->setDisabled(true);
		widget->setZOrderingEnabled(false);

		// add window to own list
		overlayWindows->push_back(widget);

		// save last active window
		CEGUI::Window* lastActive = ControllerUI::GUIRoot->getActiveChild();

		// attach to guiroot
		guiRoot->addChild(widget);
		
		// restore last active window
		if (!lastActive)
			ControllerUI::GUIRoot->activate();
		
		else		
			lastActive->activate();
					
		// create imagecomposer
		ImageComposerCEGUI<PlayerOverlay^>^ imageComposer = gcnew ImageComposerCEGUI<PlayerOverlay^>();
		imageComposer->ApplyYOffset = false;
        imageComposer->HotspotIndex = 0;
        imageComposer->IsScalePow2 = false;
        imageComposer->UseViewerFrame = false;
        imageComposer->CenterHorizontal = false;
        imageComposer->CenterVertical = false;
		imageComposer->NewImageAvailable += gcnew ::System::EventHandler(OnImageAvailable);

		// insert composer into list at index
		imageComposers->Insert(Index, imageComposer);

		// set datasource (triggers first image)
		imageComposer->DataSource = obj;

		// don't show if we're not in 1. person
		if (!ControllerInput::IsCameraFirstPerson)
			widget->hide();
	};

	void ControllerUI::PlayerOverlays::PlayerOverlayRemove(int Index)
	{	
		// get window from own list
		CEGUI::Window* window = overlayWindows->at(Index);

		// remove from own list
		overlayWindows->erase(overlayWindows->begin() + Index);

		// destroy window
		guiRoot->destroyChild(window);

		// remove imagecomposer
		if (imageComposers->Count > Index)
		{
			// reset (detaches listeners!)
			imageComposers[Index]->NewImageAvailable -= gcnew ::System::EventHandler(OnImageAvailable);
			imageComposers[Index]->DataSource = nullptr;
			
			// remove from list
			imageComposers->RemoveAt(Index);
		}
	};

	void ControllerUI::PlayerOverlays::PlayerOverlayChange(int Index)
	{		
	};

	void ControllerUI::PlayerOverlays::OnImageAvailable(Object^ sender, ::System::EventArgs^ e)
	{
		// imagecomposer which created event
		ImageComposerCEGUI<PlayerOverlay^>^ imageComposer = (ImageComposerCEGUI<PlayerOverlay^>^)sender;
		
		// get index of this imagecomposer
		int index = imageComposers->IndexOf(imageComposer);

		if ((int)overlayWindows->size() > index)
		{
			// get overlay window
			CEGUI::Window* window = overlayWindows->at(index);

			// set new image
			window->setProperty(UI_PROPNAME_IMAGE, *imageComposer->Image->TextureName);

			// calc size with scale
			float x = scale * imageComposer->RenderInfo->Dimension.X;
			float y = scale * imageComposer->RenderInfo->Dimension.Y;

			// set size
			window->setSize(CEGUI::USize(CEGUI::UDim(0, x), CEGUI::UDim(0, y)));

			// set position
			window->setPosition(GetScreenPositionForHotspot(imageComposer));
		}
	};

	::CEGUI::UVector2 ControllerUI::PlayerOverlays::GetScreenPositionForHotspot(ImageComposerCEGUI<PlayerOverlay^>^ ImageComposer)
	{
		::CEGUI::UVector2 value = ::CEGUI::UVector2();

		// build x-offset
		switch (ImageComposer->DataSource->RenderPosition)
		{
			case PlayerOverlayHotspot::HOTSPOT_NW:
			case PlayerOverlayHotspot::HOTSPOT_W:
			case PlayerOverlayHotspot::HOTSPOT_SW:
				value.d_x.d_offset = 0.0f;
				break;

			case PlayerOverlayHotspot::HOTSPOT_SE:
			case PlayerOverlayHotspot::HOTSPOT_E:
			case PlayerOverlayHotspot::HOTSPOT_NE:
				value.d_x.d_offset = guiRoot->getPixelSize().d_width - (scale * ImageComposer->RenderInfo->Dimension.X);
				break;

			case PlayerOverlayHotspot::HOTSPOT_N:
			case PlayerOverlayHotspot::HOTSPOT_S:
			case PlayerOverlayHotspot::HOTSPOT_CENTER:
				value.d_x.d_offset = 0.5f * (guiRoot->getPixelSize().d_width - (scale * ImageComposer->RenderInfo->Dimension.X));
				break;
		}

		// build y-offset
		switch (ImageComposer->DataSource->RenderPosition)
		{
			case PlayerOverlayHotspot::HOTSPOT_NW:
			case PlayerOverlayHotspot::HOTSPOT_N:
			case PlayerOverlayHotspot::HOTSPOT_NE:
				value.d_y.d_offset = 0.0f;
				break;

			case PlayerOverlayHotspot::HOTSPOT_SW:
			case PlayerOverlayHotspot::HOTSPOT_S:
			case PlayerOverlayHotspot::HOTSPOT_SE:
				value.d_y.d_offset = guiRoot->getPixelSize().d_height - (scale * ImageComposer->RenderInfo->Dimension.Y);
				break;

			case PlayerOverlayHotspot::HOTSPOT_W:
			case PlayerOverlayHotspot::HOTSPOT_E:
			case PlayerOverlayHotspot::HOTSPOT_CENTER:
				value.d_y.d_offset = 0.5f * (guiRoot->getPixelSize().d_height - (scale * ImageComposer->RenderInfo->Dimension.Y));
				break;
		}

		// build relative from absolute
		value.d_x.d_scale = value.d_x.d_offset / guiRoot->getPixelSize().d_width;
		value.d_y.d_scale = value.d_y.d_offset / guiRoot->getPixelSize().d_height;

		value.d_x.d_offset = 0;
		value.d_y.d_offset = 0;

		return value;
	};

	void ControllerUI::PlayerOverlays::ShowOverlays()
	{
		for(int i = 0; i < (int)overlayWindows->size(); i++)		
			overlayWindows->at(i)->show();		
	};

	void ControllerUI::PlayerOverlays::HideOverlays()
	{
		for(int i = 0; i < (int)overlayWindows->size(); i++)		
			overlayWindows->at(i)->hide();
	};

	void ControllerUI::PlayerOverlays::WindowResized(int Width, int Height)
	{
		if ((int)overlayWindows->size() != imageComposers->Count)
			return;

		scale = (float)Width / 800.0f;

		for (int i = 0; i < (int)overlayWindows->size(); i++)
		{
			// get overlay window
			CEGUI::Window* window = overlayWindows->at(i);
			ImageComposerCEGUI<PlayerOverlay^>^ imageComposer = imageComposers[i];

			// calc size with scale
			float x = scale * imageComposer->RenderInfo->Dimension.X;
			float y = scale * imageComposer->RenderInfo->Dimension.Y;

			// set size
			window->setSize(CEGUI::USize(CEGUI::UDim(0, x), CEGUI::UDim(0, y)));

			// set position
			window->setPosition(GetScreenPositionForHotspot(imageComposer));
		}
	};
};};