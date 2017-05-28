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
		CEGUI::UVector2 pos = OgreClient::Singleton->Config->UILayoutMinimap->getPosition();
		CEGUI::USize size = OgreClient::Singleton->Config->UILayoutMinimap->getSize();

		Window->setPosition(pos);
		Window->setMaxSize(size);
		Window->setSize(size);

		// set minimap sizes
      MiniMapCEGUI::SetDimension(
			::System::Convert::ToInt32(size.d_width.d_offset), 
			::System::Convert::ToInt32(size.d_height.d_offset));

		// attach listener to minimap
      MiniMapCEGUI::ImageChanged +=
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
      MiniMapCEGUI::ImageChanged -=
			gcnew ::System::EventHandler(OnImageChanged);
	};

	void ControllerUI::MiniMap::ApplyLanguage()
	{
	};

   void ControllerUI::MiniMap::OnImageChanged(::System::Object^ sender, ::System::EventArgs^ e)
   {
      // force update
      DrawSurface->setProperty(UI_PROPNAME_IMAGE, UI_MINIMAP_TEXNAME);

      // reenable resize
      IsWaitingForSize = false;

      // resize
      ::CEGUI::USize size = Window->getSize();

      int oldwidth = (int)size.d_width.d_offset;
      int oldheight = (int)size.d_height.d_offset;
      int newwidth = MiniMapCEGUI::NewWidth;
      int newHeight = MiniMapCEGUI::NewHeight;

      // same size
      if (oldwidth == newwidth && oldheight == newHeight )
         return;

      size = ::CEGUI::USize(
         ::CEGUI::UDim(0.0f, (float)newwidth), 
         ::CEGUI::UDim(0.0f, (float)newHeight));

      int dw = newwidth - oldwidth;
      int dh = newHeight - oldheight;

      ::CEGUI::UVector2 position = Window->getPosition();

      // adjust position on size differences
      // e.g. move half of the additional width to the left,
      // so we grow into all direction
      position.d_x.d_offset -= 0.5f * (float)dw;
      position.d_y.d_offset -= 0.5f * (float)dh;

      // apply size on cegui window
      Window->setMaxSize(size);
      Window->setSize(size);
      Window->setPosition(position);
   };

	bool ControllerUI::MiniMap::IsMouseOnCircle()
	{
		const CEGUI::Vector2f absMouse	= ControllerUI::MouseCursor->getPosition();
		const CEGUI::Vector2f wndPt		= CEGUI::CoordConverter::screenToWindow(*Window, absMouse);
		const CEGUI::Sizef size			= Window->getPixelSize();

		// center of the minimap window
		const CEGUI::Vector2f center = CEGUI::Vector2f(
			size.d_width * 0.5f,
			size.d_height * 0.5f);

		// radius of the minimap circle
		const float radius = 0.5f * (size.d_width * 0.97f);
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
         // don't start another resize while still waiting for resized image
         if (ControllerUI::MiniMap::IsWaitingForSize)
            return true;

         const float MINSIZE = 256.0f;
         const float MAXSIZE = 512.0f;

         ::CEGUI::USize size = ControllerUI::MiniMap::Window->getSize();

         float oldwidth = size.d_width.d_offset;
         float oldheight = size.d_height.d_offset;

         int width  = (int)MathUtil::Bound(oldwidth + args.wheelChange * -2.0f, MINSIZE, MAXSIZE);
         int height = (int)MathUtil::Bound(oldheight + args.wheelChange * -2.0f, MINSIZE, MAXSIZE);

         int dw = width - (int)oldwidth;
         int dh = height - (int)oldheight;

         size = ::CEGUI::USize(
            ::CEGUI::UDim(0.0f, (float)width), 
            ::CEGUI::UDim(0.0f, (float)height));

         // apply size on minimap
         MiniMapCEGUI::SetDimension(width, height);

         // must not resize 
         ControllerUI::MiniMap::IsWaitingForSize = true;
      }

      // adjust zoomlevel
      else
         MiniMapCEGUI::Zoom(args.wheelChange * -0.2f);

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