#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::MiniMap::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_MINIMAP_WINDOW));
		DrawSurface		= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MINIMAP_DRAWSURFACE));
				
		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutMinimap->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutMinimap->getSize());

		// set minimap sizes
		OgreClient::Singleton->MiniMap->SetDimension(
			::System::Convert::ToInt32(Window->getPixelSize().d_width), 
			::System::Convert::ToInt32(Window->getPixelSize().d_height));

		// attach listener to minimap
		OgreClient::Singleton->MiniMap->ImageChanged += 
			gcnew ::System::EventHandler(OnImageChanged);

		// subscribe mouse events
		Window->subscribeEvent(CEGUI::Window::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseWheel));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseDown));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseUp));
		Window->subscribeEvent(CEGUI::Window::EventMouseDoubleClick, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseDoubleClick));
	};

	void ControllerUI::MiniMap::Destroy()
	{	 
		// detach listener to minimap
		OgreClient::Singleton->MiniMap->ImageChanged -= 
			gcnew ::System::EventHandler(OnImageChanged);
	};

	void ControllerUI::MiniMap::ApplyLanguage()
	{
	};

	void ControllerUI::MiniMap::OnImageChanged(::System::Object^ sender, ::System::EventArgs^ e)
	{
		::CEGUI::String* texName = OgreClient::Singleton->MiniMap->TextureName;

		if (texName)
			DrawSurface->setProperty(UI_PROPNAME_IMAGE, *texName);
	};

	bool ControllerUI::MiniMap::IsMouseOnCircle()
	{
		const CEGUI::Vector2f absMouse	= ControllerUI::MouseCursor->getPosition();
		const CEGUI::Vector2f wndPt		= CEGUI::CoordConverter::screenToWindow(*Window, absMouse);
		const CEGUI::USize size			= Window->getSize();

		// center of the minimap window
		const CEGUI::Vector2f center = CEGUI::Vector2f(
			size.d_width.d_offset * 0.5f,
			size.d_height.d_offset * 0.5f);

		// radius of the minimap circle
		const float radius = 0.5f * (size.d_width.d_offset * 0.93f);
		const float radius2 = radius * radius;

		// get squared distance from clickpoint to center
		const float dx = wndPt.d_x - center.d_x;
		const float dy = wndPt.d_y - center.d_y;
		const float dist2 = dx * dx + dy * dy;

		return (dist2 < radius2);
	};

	bool UICallbacks::MiniMap::OnMouseWheel(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// adjust minimap size
		if (ControllerInput::IsSelfTargetDown)
		{
			const float MINSIZE = 256.0f;
			const float MAXSIZE = 512.0f;

			MiniMapCEGUI^ minimap = OgreClient::Singleton->MiniMap;

			int width  = (int)MathUtil::Bound((float)minimap->Width  + args.wheelChange * -2.0f, MINSIZE, MAXSIZE);
			int height = (int)MathUtil::Bound((float)minimap->Height + args.wheelChange * -2.0f, MINSIZE, MAXSIZE);

			::CEGUI::USize size = ::CEGUI::USize(
				::CEGUI::UDim(0.0f, (float)width), ::CEGUI::UDim(0.0f, (float)height));

			OgreClient::Singleton->MiniMap->SetDimension(width, height);
			ControllerUI::MiniMap::Window->setMaxSize(size);
			ControllerUI::MiniMap::Window->setSize(size);
		}

		// adjust zoomlevel
		else
			OgreClient::Singleton->MiniMap->Zoom += (args.wheelChange * -0.2f);

		return true;
	};

	bool UICallbacks::MiniMap::OnMouseDown(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= static_cast<const CEGUI::MouseEventArgs&>(e);

		// ignore click if not on circle
		if (!ControllerUI::MiniMap::IsMouseOnCircle())
			return true;

		// set this window as moving one
		ControllerUI::MovingWindow = ControllerUI::MiniMap::Window;

		return true;
	};

	bool UICallbacks::MiniMap::OnMouseUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// unset this window as moving one
		ControllerUI::MovingWindow = nullptr;

		return true;
	};

	bool UICallbacks::MiniMap::OnMouseDoubleClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// ignore click if not on circle
		if (!ControllerUI::MiniMap::IsMouseOnCircle())
			return true;

		// toggle visibility of roomobjects
		ControllerUI::ToggleVisibility(ControllerUI::RoomObjects::Window);

		return true;
	};
};};