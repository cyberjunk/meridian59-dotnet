#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::MainButtonsRight::Initialize()
   {
      // get windowmanager
      CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

      // setup references to children from xml nodes
      Window      = static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_MAINBUTTONSRIGHT_WINDOW));
      RoomObjects = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_ROOMOBJECTS));
      Attributes  = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_ATTRIBUTES));
      Quests      = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_QUESTS));
      Guild       = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_GUILD));
      Mail        = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_MAIL));
      Options     = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAINBUTTONSRIGHT_OPTIONS));

      // set window layout from config
      Window->setPosition(OgreClient::Singleton->Config->UILayoutMainButtonsRight->getPosition());
      Window->setSize(OgreClient::Singleton->Config->UILayoutMainButtonsRight->getSize());

      // subscribe click to head
      RoomObjects->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
      Attributes->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
      Quests->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
      Guild->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
      Mail->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));
      Options->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnItemClicked));

      // subscribe mouse events
      Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnMouseDown));
      Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::MainButtonsRight::OnMouseUp));
   };

   void ControllerUI::MainButtonsRight::Destroy()
   {
   };

   void ControllerUI::MainButtonsRight::ApplyLanguage()
   {
   };

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::MainButtonsRight::OnItemClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

      // roomobjects clicked
      if (args.window == ControllerUI::MainButtonsRight::RoomObjects)
      {
         ControllerUI::ToggleVisibility(ControllerUI::RoomObjects::Window);
      }

      // attributes clicked
      else if (args.window == ControllerUI::MainButtonsRight::Attributes)
      {
         ControllerUI::ToggleVisibility(ControllerUI::Attributes::Window);
      }

      // quests clicked
      else if (args.window == ControllerUI::MainButtonsRight::Quests)
      {
         ControllerUI::ToggleVisibility(ControllerUI::Quests::Window);
      }

      // guild clicked
      else if (args.window == ControllerUI::MainButtonsRight::Guild)
      {
         // hide
         if (OgreClient::Singleton->Data->GuildInfo->IsVisible)
            OgreClient::Singleton->Data->GuildInfo->Clear(true);

         // or request info
         else
         {
            OgreClient::Singleton->SendUserCommandGuildInfoReq();
            OgreClient::Singleton->SendUserCommandGuildGuildListReq();
            OgreClient::Singleton->SendUserCommandGuildShieldListReq();
            OgreClient::Singleton->SendUserCommandGuildShieldInfoReq();
         }
      }

      // mail clicked
      else if (args.window == ControllerUI::MainButtonsRight::Mail)
      {
         // show or hide
         ControllerUI::ToggleVisibility(ControllerUI::Mail::Window);

         // request mail if we made it visible
         if (ControllerUI::Mail::Window->isVisible())
            OgreClient::Singleton->SendReqGetMail();
      }

      // options clicked
      else if (args.window == ControllerUI::MainButtonsRight::Options)
      {
         ControllerUI::ToggleVisibility(ControllerUI::Options::Window);
      }

      return true;
   };

   bool UICallbacks::MainButtonsRight::OnMouseDown(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

      // set this window as moving one
      ControllerUI::MovingWindow = ControllerUI::MainButtonsRight::Window;

      return true;
   };

   bool UICallbacks::MainButtonsRight::OnMouseUp(const CEGUI::EventArgs& e)
   {
      const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

      // unset this window as moving one
      ControllerUI::MovingWindow = nullptr;

      return true;
   };
};};
