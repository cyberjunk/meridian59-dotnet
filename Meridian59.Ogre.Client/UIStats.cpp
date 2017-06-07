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

      MainThreadBest = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_MAINTHREADBEST));
      MainThreadAverage = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_MAINTHREADAVERAGE));
      MainThreadWorst = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_MAINTHREADWORST));
      MiniMapThreadBest = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_MINIMAPTHREADBEST));
      MiniMapThreadAverage = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_MINIMAPTHREADAVERAGE));
      MiniMapThreadWorst = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_MINIMAPTHREADWORST));

      OgreMemTextures = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMTEXTURES));
      OgreMemMaterials = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMMATERIALS));
      OgreMemMeshes = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMMESHES));
      OgreMemCompositors = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREMEMCOMPOSITORS));

      CEGUIObjectCacheRoom = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_CEGUIOBJECTCACHEROOM));
      CEGUIObjectCacheObject = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_CEGUIOBJECTCACHEOBJECT));
      CEGUIObjectCacheInventory = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_CEGUIOBJECTCACHEINVENTORY));
      OgreRoomObjectCache = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREROOMOBJECTCACHE));

      OgreFrustumLights = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREFRUSTUMLIGHTS));
      OgreRoomSections = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_OGREROOMSECTIONS));

      GarbageCollectionRuns = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_STATS_GARBAGECOLLECTIONRUNS));

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

      // bytes of 1 MB
      const size_t MB = 1024 * 1024;

      OgreClient^ client = OgreClient::Singleton;

      MaterialManager&   matMan  = MaterialManager::getSingleton();
      TextureManager&    texMan  = TextureManager::getSingleton();
      MeshManager&       meshMan = MeshManager::getSingleton();
      CompositorManager& compMan = CompositorManager::getSingleton();

      //ResourceGroupManager& resGrpMan = ResourceGroupManager::getSingleton();

      /*unsigned int numViewportBatches = client->Viewport->_getNumRenderedBatches();
      unsigned int numViewportFaces = client->Viewport->_getNumRenderedFaces();
      unsigned int numViewportInvisBatches = client->ViewportInvis->_getNumRenderedBatches();
      unsigned int numViewportInvisFaces = client->ViewportInvis->_getNumRenderedFaces();*/

      //CEGUI::ImageManager& imgMan = CEGUI::ImageManager::getSingleton();
      //CEGUI::uint numCEGUIImages = imgMan.getImageCount();

      //----------------------------------------------------------------------------------------------------------//
      // RenderWindow
      size_t numRenderWindowBatches = client->RenderWindow->getBatchCount();
      size_t numRenderWindowTriangles = client->RenderWindow->getTriangleCount();

      RenderWindowBatches->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderWindowBatches));
      RenderWindowTriangles->setText(CEGUI::PropertyHelper<size_t>::toString(numRenderWindowTriangles));

      //----------------------------------------------------------------------------------------------------------//
      // RenderSystem
      unsigned int numRenderSystemBatches = client->RenderSystem->_getBatchCount();
      unsigned int numRenderSystemFaces = client->RenderSystem->_getFaceCount();
      unsigned int numRenderSystemVertices = client->RenderSystem->_getVertexCount();

      RenderSystemBatches->setText(CEGUI::PropertyHelper<double>::toString(numRenderSystemBatches));
      RenderSystemFaces->setText(CEGUI::PropertyHelper<double>::toString(numRenderSystemFaces));
      RenderSystemVertices->setText(CEGUI::PropertyHelper<double>::toString(numRenderSystemVertices));
      
      //----------------------------------------------------------------------------------------------------------//
      // MainThread
      MainThreadBest->setText(CEGUI::PropertyHelper<double>::toString(client->Data->TickBest));
      MainThreadAverage->setText(CEGUI::PropertyHelper<double>::toString(client->Data->TickAverage));
      MainThreadWorst->setText(CEGUI::PropertyHelper<double>::toString(client->Data->TickWorst));

      //----------------------------------------------------------------------------------------------------------//
      // MiniMapThread
      MiniMapThreadBest->setText(CEGUI::PropertyHelper<double>::toString(MiniMapCEGUI::TickBest));
      MiniMapThreadAverage->setText(CEGUI::PropertyHelper<double>::toString(MiniMapCEGUI::TickAvg));
      MiniMapThreadWorst->setText(CEGUI::PropertyHelper<double>::toString(MiniMapCEGUI::TickWorst));

      //----------------------------------------------------------------------------------------------------------//
      // OgreMem
      size_t memTextures    = texMan.getMemoryUsage() / MB;
      size_t memMaterials   = matMan.getMemoryUsage() / MB;
      size_t memMeshes      = meshMan.getMemoryUsage() / MB;
      size_t memCompositors = compMan.getMemoryUsage() / MB;

      OgreMemTextures->setText(CEGUI::PropertyHelper<size_t>::toString(memTextures).append(" MB"));
      OgreMemMaterials->setText(CEGUI::PropertyHelper<size_t>::toString(memMaterials).append(" MB"));
      OgreMemMeshes->setText(CEGUI::PropertyHelper<size_t>::toString(memMeshes).append(" MB"));
      OgreMemCompositors->setText(CEGUI::PropertyHelper<size_t>::toString(memCompositors).append(" MB"));

      //----------------------------------------------------------------------------------------------------------//
      // Other
      size_t numLightsFrustum = client->SceneManager->_getLightsAffectingFrustum().size();
      unsigned int numRoomSections = ControllerRoom::RoomManualObject->getNumSections();

      OgreFrustumLights->setText(CEGUI::PropertyHelper<size_t>::toString(numLightsFrustum));
      OgreRoomSections->setText(CEGUI::PropertyHelper<size_t>::toString(numRoomSections));

      //----------------------------------------------------------------------------------------------------------//
      // Object Caches

      unsigned int memCEGUIObjectCacheRoom = ImageComposerCEGUI<RoomObject^>::Cache::CacheSize / MB;
      unsigned int memCEGUIObjectCacheObject = ImageComposerCEGUI<ObjectBase^>::Cache::CacheSize / MB;
      unsigned int memCEGUIObjectCacheInventory = ImageComposerCEGUI<InventoryObject^>::Cache::CacheSize / MB;
      unsigned int memOgreRoomObjectCache = ImageComposerOgre<RoomObject^>::Cache::CacheSize / MB;

      int numCEGUIObjectCacheRoom = ImageComposerCEGUI<RoomObject^>::Cache::Count;
      int numCEGUIObjectCacheObject = ImageComposerCEGUI<ObjectBase^>::Cache::Count;
      int numCEGUIObjectCacheInventory = ImageComposerCEGUI<InventoryObject^>::Cache::Count;
      int numOgreRoomObjectCache = ImageComposerOgre<RoomObject^>::Cache::Count;

      CEGUIObjectCacheRoom->setText(
         CEGUI::PropertyHelper<int>::toString(numCEGUIObjectCacheRoom) + " (" +
         CEGUI::PropertyHelper<unsigned int>::toString(memCEGUIObjectCacheRoom) + " MB)");

      CEGUIObjectCacheObject->setText(
         CEGUI::PropertyHelper<int>::toString(numCEGUIObjectCacheObject) + " (" +
         CEGUI::PropertyHelper<unsigned int>::toString(memCEGUIObjectCacheObject) + " MB)");

      CEGUIObjectCacheInventory->setText(
         CEGUI::PropertyHelper<int>::toString(numCEGUIObjectCacheInventory) + " (" +
         CEGUI::PropertyHelper<unsigned int>::toString(memCEGUIObjectCacheInventory) + " MB)");

      OgreRoomObjectCache->setText(
         CEGUI::PropertyHelper<int>::toString(numOgreRoomObjectCache) + " (" +
         CEGUI::PropertyHelper<unsigned int>::toString(memOgreRoomObjectCache) + " MB)");
      
      //----------------------------------------------------------------------------------------------------------//
      // OgreMem
      GarbageCollectionRuns->setText(
         CEGUI::PropertyHelper<int>::toString(::System::GC::CollectionCount(0)) + " / " +
         CEGUI::PropertyHelper<int>::toString(::System::GC::CollectionCount(1)) + " / " +
         CEGUI::PropertyHelper<int>::toString(::System::GC::CollectionCount(2)));

      //----------------------------------------------------------------------------------------------------------//
   };
};};
