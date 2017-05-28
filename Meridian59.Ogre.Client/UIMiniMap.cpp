#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::MiniMap::Initialize()
   {
      // setup references to children from xml nodes
      Window      = static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_MINIMAP_WINDOW));
      DrawSurface = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MINIMAP_DRAWSURFACE));

      // set window layout from config
      CEGUI::UVector2 pos = OgreClient::Singleton->Config->UILayoutMinimap->getPosition();
      CEGUI::USize size = OgreClient::Singleton->Config->UILayoutMinimap->getSize();

      Window->setPosition(pos);
      Window->setMaxSize(size);
      Window->setSize(size);

      // subscribe mouse events
      Window->subscribeEvent(CEGUI::Window::EventMouseWheel, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseWheel));
      Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseDown));
      Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseUp));
      Window->subscribeEvent(CEGUI::Window::EventMouseDoubleClick, CEGUI::Event::Subscriber(UICallbacks::MiniMap::OnMouseDoubleClick));
   };

   void ControllerUI::MiniMap::Destroy()
   {
   };

   void ControllerUI::MiniMap::ApplyLanguage()
   {
   };

   bool ControllerUI::MiniMap::IsMouseOnCircle()
   {
      const CEGUI::Vector2f absMouse   = ControllerUI::MouseCursor->getPosition();
      const CEGUI::Vector2f wndPt      = CEGUI::CoordConverter::screenToWindow(*Window, absMouse);
      const CEGUI::Sizef size          = Window->getPixelSize();

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
         const float MINSIZE = 256.0f;
         const float MAXSIZE = 512.0f;

         ::CEGUI::USize size = ControllerUI::MiniMap::Window->getSize();

         float oldwidth = size.d_width.d_offset;
         float oldheight = size.d_height.d_offset;

         // MUST step in 32 pixel-steps (tex requirement for DYN DISCARD)
         float step = -32.0f * args.wheelChange;

         int width  = (int)MathUtil::Bound(oldwidth  + step, MINSIZE, MAXSIZE);
         int height = (int)MathUtil::Bound(oldheight + step, MINSIZE, MAXSIZE);

         int dw = width - (int)oldwidth;
         int dh = height - (int)oldheight;

         size = ::CEGUI::USize(
            ::CEGUI::UDim(0.0f, (float)width), 
            ::CEGUI::UDim(0.0f, (float)height));

         ::CEGUI::UVector2 position = ControllerUI::MiniMap::Window->getPosition();

         // adjust position on size differences
         // e.g. move half of the additional width to the left,
         // so we grow into all direction
         position.d_x.d_offset -= 0.5f * (float)dw;
         position.d_y.d_offset -= 0.5f * (float)dh;

         // apply size on cegui window
         ControllerUI::MiniMap::Window->setMaxSize(size);
         ControllerUI::MiniMap::Window->setSize(size);
         ControllerUI::MiniMap::Window->setPosition(position);
      }

      // adjust zoomlevel
      else
      {
         ControllerUI::MiniMap::Zoom += args.wheelChange * -0.2f;
      }

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
