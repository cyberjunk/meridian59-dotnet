#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
   void ControllerUI::Stats::Initialize()
   {
      // setup references to children from xml nodes
      Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_STATS_WINDOW));
      BatchCount = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_BATCHCOUNT));
      TriangleCount = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_TRIANGLECOUNT));
      OgreMemTextures = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMTEXTURES));
      OgreMemMaterials = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMMATERIALS));

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
   };

   void ControllerUI::Stats::Destroy()
   {
   };

   void ControllerUI::Stats::ApplyLanguage()
   {
   };

   void ControllerUI::Stats::Tick()
   {
      if (!Window->isVisible())
         return;

      OgreClient^ client = OgreClient::Singleton;
      MaterialManager& matMan = MaterialManager::getSingleton();
      TextureManager& texMan = TextureManager::getSingleton();
      //ResourceGroupManager& resGrpMan = ResourceGroupManager::getSingleton();
      const size_t MB = 1024 * 1024;

      BatchCount->setText(CEGUI::PropertyHelper<size_t>::toString(client->RenderWindow->getBatchCount()));
      TriangleCount->setText(CEGUI::PropertyHelper<size_t>::toString(client->RenderWindow->getTriangleCount()));
      OgreMemTextures->setText(CEGUI::PropertyHelper<size_t>::toString(texMan.getMemoryUsage() / MB).append(" MB"));
      OgreMemMaterials->setText(CEGUI::PropertyHelper<size_t>::toString(matMan.getMemoryUsage() / MB).append(" MB"));
   };
};};
