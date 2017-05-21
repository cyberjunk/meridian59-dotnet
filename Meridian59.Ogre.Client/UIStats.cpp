#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
   void ControllerUI::Stats::Initialize()
   {
      // setup references to children from xml nodes
      Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_STATS_WINDOW));
      RenderWindowBatches = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_RENDERWINDOWBATCHES));
      RenderWindowTriangles = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_RENDERWINDOWTRIANGLES));
      RenderSystemBatches = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_RENDERSYSTEMBATCHES));
      RenderSystemFaces = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_RENDERSYSTEMFACES));
      RenderSystemVertices = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_RENDERSYSTEMVERTICES));

      OgreMemTextures = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMTEXTURES));
      OgreMemMaterials = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMMATERIALS));

      CEGUIObjectCacheRoom = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_CEGUIOBJECTCACHEROOM));
      CEGUIObjectCacheObject = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_CEGUIOBJECTCACHEOBJECT));
      CEGUIObjectCacheInventory = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_CEGUIOBJECTCACHEINVENTORY));
      OgreRoomObjectCache = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREROOMOBJECTCACHE));

      OgreFrustumLights = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREFRUSTUMLIGHTS));
      OgreRoomSections = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREROOMSECTIONS));

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

      // RenderWindow
      size_t numRenderWindowBatches   = client->RenderWindow->getBatchCount();
      size_t numRenderWindowTriangles = client->RenderWindow->getTriangleCount();

      // RenderSystem
      unsigned int numRenderSystemBatches = client->RenderSystem->_getBatchCount();
      unsigned int numRenderSystemFaces = client->RenderSystem->_getFaceCount();
      unsigned int numRenderSystemVertices = client->RenderSystem->_getVertexCount();


      unsigned int numViewportBatches = client->Viewport->_getNumRenderedBatches();
      unsigned int numViewportFaces = client->Viewport->_getNumRenderedFaces();
      unsigned int numViewportInvisBatches = client->ViewportInvis->_getNumRenderedBatches();
      unsigned int numViewportInvisFaces = client->ViewportInvis->_getNumRenderedFaces();

      // OgreMem
      size_t memTextures  = texMan.getMemoryUsage() / MB;
      size_t memMaterials = matMan.getMemoryUsage() / MB;

      CEGUI::ImageManager& imgMan = CEGUI::ImageManager::getSingleton();
      CEGUI::uint numCEGUIImages = imgMan.getImageCount();
      
      size_t numLightsFrustum = client->SceneManager->_getLightsAffectingFrustum().size();
      unsigned int numRoomSections = ControllerRoom::RoomManualObject->getNumSections();

      unsigned int memCEGUIObjectCacheRoom = ImageComposerCEGUI<RoomObject^>::Cache::CacheSize / MB;
      unsigned int memCEGUIObjectCacheObject = ImageComposerCEGUI<ObjectBase^>::Cache::CacheSize / MB;
      unsigned int memCEGUIObjectCacheInventory = ImageComposerCEGUI<InventoryObject^>::Cache::CacheSize / MB;
      unsigned int memOgreRoomObjectCache = ImageComposerOgre<RoomObject^>::Cache::CacheSize / MB;

      RenderWindowBatches->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderWindowBatches));
      RenderWindowTriangles->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderWindowTriangles));

      RenderSystemBatches->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderSystemBatches));
      RenderSystemFaces->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderSystemFaces));
      RenderSystemVertices->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderSystemVertices));

      OgreMemTextures->setText(CEGUI::PropertyHelper<size_t>::toString(memTextures).append(" MB"));
      OgreMemMaterials->setText(CEGUI::PropertyHelper<size_t>::toString(memMaterials).append(" MB"));

      CEGUIObjectCacheRoom->setText(CEGUI::PropertyHelper<size_t>::toString(memCEGUIObjectCacheRoom).append(" MB"));
      CEGUIObjectCacheObject->setText(CEGUI::PropertyHelper<size_t>::toString(memCEGUIObjectCacheObject).append(" MB"));
      CEGUIObjectCacheInventory->setText(CEGUI::PropertyHelper<size_t>::toString(memCEGUIObjectCacheInventory).append(" MB"));
      OgreRoomObjectCache->setText(CEGUI::PropertyHelper<size_t>::toString(memOgreRoomObjectCache).append(" MB"));

      OgreFrustumLights->setText(CEGUI::PropertyHelper<size_t>::toString(numLightsFrustum));
      OgreRoomSections->setText(CEGUI::PropertyHelper<size_t>::toString(numRoomSections));
   };
};};
