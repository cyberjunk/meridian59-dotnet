#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   OgreClient::OgreClient() : SingletonClient()
   {
      SLEEPTIME = 0;
      isWinCursorVisible = true;
   };

   void OgreClient::Init()
   {
      // call base init
      SingletonClient::Init();

      /********************************************************************************************************/

      // init sound-engine (irrklang)
      ControllerSound::Initialize();

      /********************************************************************************************************/

      // init the ogre root object, dx9 rendersystem and plugins
      root                   = OGRE_NEW ::Ogre::Root();
      renderSystem           = OGRE_NEW ::Ogre::D3D9RenderSystem(0);
      pluginOctree           = OGRE_NEW ::Ogre::OctreePlugin();
      pluginCaelum           = OGRE_NEW ::Caelum::CaelumPlugin();
      pluginParticleUniverse = OGRE_NEW ::ParticleUniverse::ParticleUniversePlugin();

      // install plugins into root
      root->installPlugin(pluginOctree);
      root->installPlugin(pluginCaelum);
      root->installPlugin(pluginParticleUniverse);

      // set basic config options on RenderSystem
      // some of these are required for multi monitor support
      renderSystem->setConfigOption("Resource Creation Policy", "Create on active device");
      renderSystem->setConfigOption("Multi device memory hint", "Auto hardware buffers management");
      renderSystem->setConfigOption("Use Multihead", "No");
      
      // other options
      //renderSystem->setConfigOption("Resource Creation Policy", "Create on all devices");
      //renderSystem->setConfigOption("Multi device memory hint", "Use minimum system memory");
      //renderSystem->setConfigOption("Fixed Pipeline Enabled", "No");

      // set rendersystem
      root->setRenderSystem(renderSystem);

      // init root
      root->initialise(false, WINDOWNAME);

      /********************************************************************************************************/

      // settings for the dummy renderwindow
      // which serves as hidden primary window
      // the purpose: holds dx9 resources, therefore
      // allows us to destroy/recreate the actual renderwindow
      ::Ogre::NameValuePairList misc;
      misc["FSAA"]         = "0";
      misc["monitorIndex"] = ::Ogre::StringConverter::toString(Config->Display);
      misc["vsync"]        = "false";
      misc["hidden"]       = "true";
      misc["depthBuffer"]  = "false";
      misc["border"]       = "none";

      // create the hidden, primary dummy renderwindow
      renderWindowDummy = (D3D9RenderWindow*)root->createRenderWindow(
         "PrimaryWindowDummy", 1, 1, false, &misc);

      renderWindowDummy->setActive(false);
      renderWindowDummy->setAutoUpdated(false);

      /********************************************************************************************************/

      ::Ogre::ResourceGroupManager& resMan = ::Ogre::ResourceGroupManager::getSingleton();
      ::Ogre::TextureManager& texMan = ::Ogre::TextureManager::getSingleton();
      ::Ogre::MaterialManager& matMan = ::Ogre::MaterialManager::getSingleton();

      // make sure basic resource groups are created
      if (!resMan.resourceGroupExists(RESOURCEGROUPSHADER))
         resMan.createResourceGroup(RESOURCEGROUPSHADER);

      if (!resMan.resourceGroupExists(RESOURCEGROUPMODELS))
         resMan.createResourceGroup(RESOURCEGROUPMODELS);

      if (!resMan.resourceGroupExists(MATERIALGROUP_REMOTENODE2D))
         resMan.createResourceGroup(MATERIALGROUP_REMOTENODE2D);

      if (!resMan.resourceGroupExists(TEXTUREGROUP_REMOTENODE2D))
         resMan.createResourceGroup(TEXTUREGROUP_REMOTENODE2D);

      if (!resMan.resourceGroupExists(MATERIALGROUP_MOVABLETEXT))
         resMan.createResourceGroup(MATERIALGROUP_MOVABLETEXT);

      if (!resMan.resourceGroupExists(TEXTUREGROUP_MOVABLETEXT))
         resMan.createResourceGroup(TEXTUREGROUP_MOVABLETEXT);

      if (!resMan.resourceGroupExists(MATERIALGROUP_PROJECTILENODE2D))
         resMan.createResourceGroup(MATERIALGROUP_PROJECTILENODE2D);

      if (!resMan.resourceGroupExists(TEXTUREGROUP_PROJECTILENODE2D))
         resMan.createResourceGroup(TEXTUREGROUP_PROJECTILENODE2D);

      if (!resMan.resourceGroupExists(MATERIALGROUP_ROOLOADER))
         resMan.createResourceGroup(MATERIALGROUP_ROOLOADER);

      if (!resMan.resourceGroupExists(TEXTUREGROUP_ROOLOADER))
         resMan.createResourceGroup(TEXTUREGROUP_ROOLOADER);

      /********************************************************************************************************/
      /*                                 CREATE SCENEMANAGER + CAMERA,                                        */
      /********************************************************************************************************/

      // init scenemanager
      sceneManager = (OctreeSceneManager*)root->createSceneManager(SceneType::ST_GENERIC);
      sceneManager->setCameraRelativeRendering(true);

      // make sure no time is spent on disabled shadows
      sceneManager->setShadowTechnique(ShadowTechnique::SHADOWTYPE_NONE);
      sceneManager->getRenderQueue()->getQueueGroup(Ogre::RENDER_QUEUE_MAIN)->setShadowsEnabled(false);

      // create camera listener
      cameraListener = new CameraListener();

      // create camera
      camera = (OctreeCamera*)sceneManager->createCamera(CAMERANAME);
      camera->setPosition(::Ogre::Vector3(0, 0, 0));
      camera->setNearClipDistance(1.0f);
      camera->setUseRenderingDistance(false);

      // create camera node (this is placed at the avatar roughly at eye height)
      cameraNode = sceneManager->createSceneNode(AVATARCAMNODE);
      cameraNode->setPosition(::Ogre::Vector3(0, 0, 0));
      cameraNode->setFixedYawAxis(true);
      cameraNode->setInitialState();

      // create camera node in orbit (this is where the actual camera is, with z offset)
      cameraNodeOrbit = cameraNode->createChildSceneNode(AVATARCAMNODEORBIT);
      cameraNodeOrbit->setPosition(::Ogre::Vector3(0, 0, 0));
      cameraNodeOrbit->setFixedYawAxis(true);
      cameraNodeOrbit->setInitialState();

      // attach camera
      cameraNodeOrbit->attachObject(camera);

      /********************************************************************************************************/
      /*                                       INVIS EFFECT RTT                                               */
      /********************************************************************************************************/

      // create invis refraction texture required for invis shader
      // this must be loaded before InitResources()
      TexturePtr texPtr = texMan.createManual(
         "refraction",
         RESOURCEGROUPSHADER,
         TextureType::TEX_TYPE_2D,
         512, 512, 0,
         ::Ogre::PixelFormat::PF_R8G8B8,
         TU_RENDERTARGET, 0, false, 0);

      RenderTarget* rtt = texPtr->getBuffer()->getRenderTarget();

      // create viewport for invis effect
      // this must happen before the real viewport
      // or Caelum will accidentally grab it.
      viewportInvis = rtt->addViewport(camera);
      viewportInvis->setOverlaysEnabled(false);
      viewportInvis->setAutoUpdated(false);

      /********************************************************************************************************/
      /*                                     CREATE RENDERWINDOW                                              */
      /********************************************************************************************************/

      RenderWindowCreate();


      /********************************************************************************************************/
      /*                          APPLY TEXTUREFILTERING SETTINGS                                             */
      /********************************************************************************************************/

      // set default mipmaps count
      texMan.setDefaultNumMipmaps(Config->NoMipmaps ? 0 : 5);

      if (CLRString::Equals(Config->TextureFiltering, "Off"))
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_NONE);

      else if (CLRString::Equals(Config->TextureFiltering, "Bilinear"))
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_BILINEAR);

      else if (CLRString::Equals(Config->TextureFiltering, "Trilinear"))
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_TRILINEAR);

      else if (CLRString::Equals(Config->TextureFiltering, "Anisotropic x4"))
      {
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
         matMan.setDefaultAnisotropy(4);
      }
      else if (CLRString::Equals(Config->TextureFiltering, "Anisotropic x16"))
      {
         matMan.setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
         matMan.setDefaultAnisotropy(16);
      }

      /********************************************************************************************************/
      /*                             APPLY BITMAPSCALING SETTINGS                                             */
      /********************************************************************************************************/

      // select imagebuilder
      if (CLRString::Equals(Config->ImageBuilder, "GDI"))
         ImageBuilder::Initialize(ImageBuilderType::GDI);

      else if (CLRString::Equals(Config->ImageBuilder, "DirectDraw"))
         ImageBuilder::Initialize(ImageBuilderType::DirectDraw);

      else if (CLRString::Equals(Config->ImageBuilder, "DirectX"))
         ImageBuilder::Initialize(ImageBuilderType::DirectX);

      else if (CLRString::Equals(Config->ImageBuilder, "Native"))
         ImageBuilder::Initialize(ImageBuilderType::Native);

      // texturescaling in gdi+
      if (CLRString::Equals(Config->BitmapScaling, "Low"))
      {
         ImageBuilder::GDI::InterpolationMode  = ::System::Drawing::Drawing2D::InterpolationMode::NearestNeighbor;
         ImageBuilder::GDI::PixelOffsetMode    = ::System::Drawing::Drawing2D::PixelOffsetMode::Half;
         ImageBuilder::GDI::CompositingQuality = ::System::Drawing::Drawing2D::CompositingQuality::HighSpeed;
         ImageBuilder::GDI::SmoothingMode      = ::System::Drawing::Drawing2D::SmoothingMode::None;
      }
      else if (CLRString::Equals(Config->BitmapScaling, "High"))
      {
         ImageBuilder::GDI::InterpolationMode  = ::System::Drawing::Drawing2D::InterpolationMode::HighQualityBicubic;
         ImageBuilder::GDI::PixelOffsetMode    = ::System::Drawing::Drawing2D::PixelOffsetMode::HighQuality;
         ImageBuilder::GDI::CompositingQuality = ::System::Drawing::Drawing2D::CompositingQuality::HighSpeed;
         ImageBuilder::GDI::SmoothingMode      = ::System::Drawing::Drawing2D::SmoothingMode::None;
      }
      else // "Default"
      {
         ImageBuilder::GDI::InterpolationMode  = ::System::Drawing::Drawing2D::InterpolationMode::Bilinear;
         ImageBuilder::GDI::PixelOffsetMode    = ::System::Drawing::Drawing2D::PixelOffsetMode::HighSpeed;
         ImageBuilder::GDI::CompositingQuality = ::System::Drawing::Drawing2D::CompositingQuality::HighSpeed;
         ImageBuilder::GDI::SmoothingMode      = ::System::Drawing::Drawing2D::SmoothingMode::None;
      }

      // texturequality
      if (CLRString::Equals(Config->TextureQuality, "Low"))
      {
         ImageComposerOgre<RoomObject^>::DefaultQuality = 0.25f; // used in RemoteNode2D		
         ImageComposerCEGUI<ObjectBase^>::DefaultQuality = 0.25f; // used in CEGUI
         ImageComposerCEGUI<RoomObject^>::DefaultQuality = 0.25f; // used in CEGUI
         ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.25f; // used in CEGUI
      }
      else if (CLRString::Equals(Config->TextureQuality, "High"))
      {
         ImageComposerOgre<RoomObject^>::DefaultQuality = 1.0f; // used in RemoteNode2D
         ImageComposerCEGUI<ObjectBase^>::DefaultQuality = 1.0f; // used in CEGUI
         ImageComposerCEGUI<RoomObject^>::DefaultQuality = 1.0f; // used in CEGUI
         ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 1.0f; // used in CEGUI
      }
      else
      {
         ImageComposerOgre<RoomObject^>::DefaultQuality = 0.5f; // used in RemoteNode2D
         ImageComposerCEGUI<ObjectBase^>::DefaultQuality = 0.5f; // used in CEGUI
         ImageComposerCEGUI<RoomObject^>::DefaultQuality = 0.5f; // used in CEGUI
         ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.5f; // used in CEGUI
      }

      /********************************************************************************************************/
      /*                                   Setup ImageComposer Caches                                         */
      /********************************************************************************************************/

      ImageComposerOgre<RoomObject^>::Cache::CacheSizeMax         = CACHESIZEIMGECOMPOSEROGREROOMOBJ;
      ImageComposerCEGUI<ObjectBase^>::Cache::CacheSizeMax        = CACHESIZEIMGECOMPOSERCEGUIOBJECT;
      ImageComposerCEGUI<RoomObject^>::Cache::CacheSizeMax        = CACHESIZEIMGECOMPOSERCEGUIROOMOBJ;
      ImageComposerCEGUI<InventoryObject^>::Cache::CacheSizeMax   = CACHESIZEIMGECOMPOSERCEGUIINVENTORYOBJ;

      ImageComposerOgre<RoomObject^>::Cache::RemoveSuggested +=
         gcnew ::System::EventHandler<ImageComposerOgre<RoomObject^>::Cache::ItemEventArgs^>(this, &OgreClient::OnImageComposerOgreRoomObjectCacheRemove);

      ImageComposerCEGUI<ObjectBase^>::Cache::RemoveSuggested +=
         gcnew ::System::EventHandler<ImageComposerCEGUI<ObjectBase^>::Cache::ItemEventArgs^>(this, &OgreClient::OnImageComposerCEGUIObjectBaseCacheRemove);

      ImageComposerCEGUI<RoomObject^>::Cache::RemoveSuggested +=
         gcnew ::System::EventHandler<ImageComposerCEGUI<RoomObject^>::Cache::ItemEventArgs^>(this, &OgreClient::OnImageComposerCEGUIRoomObjectCacheRemove);

      ImageComposerCEGUI<InventoryObject^>::Cache::RemoveSuggested +=
         gcnew ::System::EventHandler<ImageComposerCEGUI<InventoryObject^>::Cache::ItemEventArgs^>(this, &OgreClient::OnImageComposerCEGUIInventoryObjectCacheRemove);

      /********************************************************************************************************/
      /*                                                                                                      */
      /********************************************************************************************************/

      ControllerInput::Initialize();
      MiniMapCEGUI::Initialize();

      // init cegui
      ControllerUI::Initialize((::Ogre::RenderTarget*)renderWindow);

      // set ui to loadingbar
      Data->UIMode = UIMode::LoadingBar;

      // initial framerendering (no loop yet)
      root->renderOneFrame();

      // initialize resources
      InitResources();

      // Init controllers
      ControllerRoom::Initialize();
      ControllerEffects::Initialize();

      // load demoscene
      DemoSceneLoadBrax();

      /********************************************************************************************************/

      // set UI intially to login panel
      Data->UIMode = UIMode::Login;
   };

   void OgreClient::RenderWindowCreate()
   {
      if (renderWindow)
         return;

      // settings for the main (but not primary) renderwindow
      ::Ogre::NameValuePairList misc2;
      misc2["FSAA"]           = StringConvert::CLRToOgre(Config->FSAA);
      misc2["vsync"]          = StringConvert::CLRToOgre(Config->VSync.ToString());
      misc2["border"]         = Config->WindowFrame ? "fixed" : "none";
      misc2["monitorIndex"]   = ::Ogre::StringConverter::toString(Config->Display);

      // get window height & width from options
      int idx1 = Config->Resolution->IndexOf('x');
      int idx2 = Config->Resolution->IndexOf('@');
      System::UInt32 windowwidth = System::Convert::ToUInt32(Config->Resolution->Substring(0, idx1 - 1));
      System::UInt32 windowheight = System::Convert::ToUInt32(Config->Resolution->Substring(idx1 + 2, idx2 - idx1 - 2));

      // create the main (but not primary) renderwindow
      renderWindow = (::Ogre::D3D9RenderWindow*)root->createRenderWindow(
         WINDOWNAME,
         windowwidth,
         windowheight,
         !Config->WindowMode,
         &misc2);

      // get window handle and save as HWND
      size_t val = 0;
      renderWindow->getCustomAttribute("WINDOW", &val);
      renderWindowHandle = (HWND)val;
      renderMonitorHandle = MonitorFromWindow(renderWindowHandle, MONITOR_DEFAULTTONEAREST);

      // keep rendering without focus
      renderWindow->setDeactivateOnFocusChange(false);

      // create window event listener
      windowListener = new MyWindowEventListener();

      // attach window event listener
      ::Ogre::WindowEventUtilities::addWindowEventListener(
         renderWindow, windowListener);

      // set icon on gamewindow
      HICON iconID = LoadIcon(GetModuleHandle(0), MAKEINTRESOURCE(1));
      SetClassLongPtr(renderWindowHandle, GCLP_HICON, (LONG_PTR)iconID);

      // create viewport
      viewport = renderWindow->addViewport(camera, 0);

      int actualwidth = viewport->getActualWidth();
      int actualheight = viewport->getActualHeight();

      ::Ogre::Real aspectRatio = ::Ogre::Real(actualwidth) / ::Ogre::Real(actualheight);

      // set camera aspect ratio based on viewport
      camera->setAspectRatio(aspectRatio);

      // make sure to reinit stuff in case of a window recreate
      // cegui survives renderwindow change applying new renderwindow and size
      if (ControllerUI::IsInitialized)
      {
         ControllerUI::Renderer->setDefaultRootRenderTarget(*((::Ogre::RenderTarget*)renderWindow));
         ControllerUI::Renderer->setDisplaySize(::CEGUI::Sizef((float)actualwidth, (float)actualheight));
         ControllerUI::PlayerOverlays::WindowResized(actualwidth, actualheight);
      }
   };

   void OgreClient::RenderWindowDestroy()
   {
      if (!renderWindow)
         return;

      // compositors are linked to renderwindow viewports
      ControllerEffects::Destroy();

      // input is linked to the renderwindow
      ControllerInput::Destroy();

      if (windowListener)
      {
         ::Ogre::WindowEventUtilities::removeWindowEventListener(
            renderWindow, windowListener);

         OGRE_DELETE windowListener;
      }

      // detach all viewports from window
      renderWindow->removeAllListeners();
      renderWindow->removeAllViewports();

      if (root)
         root->destroyRenderTarget(renderWindow);

      renderWindow	= nullptr;
      windowListener	= nullptr;
   };

   void OgreClient::Update()
   {
      // call base update
      SingletonClient::Update();

      // don't do anything if the application
      // is supposed to shut down completely
      if (!IsRunning)
         return;

      /********************************************************************************************************/

      ::Ogre::WindowEventUtilities::messagePump();

      /********************************************************************************************************/

      // get monitor of window to check for change d3d9 device gets lost on monitor change
      // causing lot of trouble, this allows handling before trouble starts..
      HMONITOR mon = MonitorFromWindow(renderWindowHandle, MONITOR_DEFAULTTONEAREST);

      // window was moved to different monitor
      if (renderMonitorHandle != mon)
      {
         // save new one
         renderMonitorHandle = mon;

         // change config value
         Config->Display = Config->Display == 0 ? 1 : 0;

         // mark window for recreation below
         RecreateWindow = true;
      }

      /********************************************************************************************************/

      if (RecreateWindow)
      {
         RenderWindowDestroy();
         RenderWindowCreate();

         // must reinit effects and input due to recreated window
         ControllerEffects::Initialize();
         ControllerInput::Initialize();

         RecreateWindow = false;
      }

      /********************************************************************************************************/
      /*                               UPDATE FOCUSSTATE AND CURSOR                                           */
      /********************************************************************************************************/

      // update focus state of the render window
      hasFocus = (renderWindowHandle == GetFocus());

      // whether the windows mousecursor should be shown or not
      bool iswincursorvisible = (!ControllerInput::IsMouseInWindow || !hasFocus);

      // hide or show win cursor if changed
      if (iswincursorvisible != isWinCursorVisible)
      {
         isWinCursorVisible = iswincursorvisible;
         ShowCursor(iswincursorvisible);

         // show CEGUI cursor when win cursor is hidden
         if (ControllerUI::MouseCursor)
            ControllerUI::MouseCursor->setVisible(!isWinCursorVisible);
      }

      /********************************************************************************************************/
      /*                                     TICK SUBCOMPONENTS                                               */
      /********************************************************************************************************/

      ControllerInput::Tick(GameTick->Current, GameTick->Span);          
      ControllerUI::Tick(GameTick->Current, GameTick->Span);
      ControllerRoom::Tick(GameTick->Current, GameTick->Span);

      /********************************************************************************************************/
      /*                                     RENDER FRAME                                                     */
      /********************************************************************************************************/

      // update the invis viewport every second frame
      // and only if there's an invis object
      if (viewportInvis && Data->RoomObjects->HasInvisibleRoomObject())
      {
         if (invisViewportUpdateFlip)
            viewportInvis->update();

         invisViewportUpdateFlip = !invisViewportUpdateFlip;
      }

      // render a frame
      if (root)
         root->renderOneFrame();
   };

   void OgreClient::Cleanup()
   {
      // stop minimap thread
      MiniMapCEGUI::IsRunning = false;

      // save layout
      ControllerUI::SaveLayoutToConfig();

      // cleanup imagebuilder
      ImageBuilder::Destroy();

      // cleanup sub controllers
      ControllerInput::Destroy();
      ControllerEffects::Destroy();
      ControllerUI::Destroy();
      ControllerRoom::Destroy();

      // cleanup camera
      if (sceneManager->hasCamera(CAMERANAME))
         sceneManager->destroyCamera(camera);

      // cleanup cameranode in orbit
      if (sceneManager->hasSceneNode(AVATARCAMNODEORBIT))
         sceneManager->destroySceneNode(AVATARCAMNODEORBIT);

      // cleanup cameranode
      if (sceneManager->hasSceneNode(AVATARCAMNODE))
         sceneManager->destroySceneNode(AVATARCAMNODE);

      // clear all remaining stuff
      sceneManager->clearScene();

      // destroy scenemanager
      root->destroySceneManager(sceneManager);

      RenderWindowDestroy();

      if (cameraListener)
         OGRE_DELETE cameraListener;

      ImageComposerCEGUI<ObjectBase^>::Cache::Clear();
      ImageComposerCEGUI<RoomObject^>::Cache::Clear();
      ImageComposerCEGUI<InventoryObject^>::Cache::Clear();
      ImageComposerOgre<RoomObject^>::Cache::Clear();

      cameraListener = nullptr;
      camera = nullptr;
      cameraNode = nullptr;
      cameraNodeOrbit = nullptr;
      viewport = nullptr;
      viewportInvis = nullptr;
      sceneManager = nullptr;

      /********************************************************************************************************/
      /*                                 ENGINE FINALIZATION                                                  */
      /********************************************************************************************************/

      // get singleton managers
      ::Ogre::CompositorManager& compMan      = ::Ogre::CompositorManager::getSingleton();
      ::Ogre::ParticleSystemManager& partMan  = ::Ogre::ParticleSystemManager::getSingleton();
      ::Ogre::ResourceGroupManager& resGrpMan = ::Ogre::ResourceGroupManager::getSingleton();

      // detach all viewports from window
      renderWindowDummy->removeAllListeners();
      renderWindowDummy->removeAllViewports();

      // destroy primary dummywindow and scenemanager
      root->destroyRenderTarget(renderWindowDummy);

      // some important shutdowns
      compMan.removeAll();
      partMan.removeAllTemplates();
      resGrpMan.shutdownAll();

      // uninstall plugins
      root->uninstallPlugin(pluginOctree);
      root->uninstallPlugin(pluginCaelum);
      root->uninstallPlugin(pluginParticleUniverse);

      // delete plugins
      OGRE_DELETE pluginOctree;
      //OGRE_DELETE pluginCaelum; // deletes itself at uninstall
      OGRE_DELETE pluginParticleUniverse;

      OGRE_DELETE root;

      // cleanup sound-engine
      ControllerSound::Destroy();

      pluginOctree            = nullptr;
      pluginCaelum            = nullptr;
      pluginParticleUniverse  = nullptr;
      renderSystem            = nullptr;
      root                    = nullptr;

      /********************************************************************************************************/
      /*                              ACTION BUTTON / CONFIG SAVING                                           */
      /********************************************************************************************************/

      // update or add the currently played actionbuttons to config
      if (Data->ActionButtons->HasPlayerName)
         Config->AddOrUpdateActionButtonSet(Data->ActionButtons);

      // TODO: Move this to the corelib's Cleanup(), it's all from there
      ConnectionInfo^ conInfo = Config->SelectedConnectionInfo;
      if (conInfo)
      {
         // update ignorelist from data to config
         conInfo->IgnoreList->Clear();
         conInfo->IgnoreList->AddRange(Data->IgnoreList);

         // update groups from data to config
         conInfo->Groups->Clear();
         conInfo->Groups->AddRange(Data->Groups);
      }

      // save config
      Config->Save();

      /********************************************************************************************************/

      // base class call
      SingletonClient::Cleanup();
   };

   void OgreClient::Disconnect()
   {
      // call base disconnect
      SingletonClient::Disconnect();

      DemoSceneLoadBrax();

      ControllerUI::Login::Window->setEnabled(true);
   };

   void OgreClient::OnServerConnectionException(System::Exception^ Error)
   {
      // attach OK listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed +=
         gcnew System::EventHandler(this, &OgreClient::OnLoginErrorConfirmed);

      // tell user about failed connection
      ControllerUI::ConfirmPopup::ShowOK("Connection failed", 0, false);
   };

   void OgreClient::InitResources()
   {
      // how many, for progress feedback
      const ushort groups = 8;

      // resourcegroupmanager
      ::Ogre::ResourceGroupManager* resMan = ::Ogre::ResourceGroupManager::getSingletonPtr();
      ::Ogre::TextureManager* texMan = ::Ogre::TextureManager::getSingletonPtr();

      // initialize a loadingbar          
      ControllerUI::LoadingBar::Start(groups);

      // 1. initialize shader scripts&materials (BEFORE models)
      InitResourceGroup(RESOURCEGROUPSHADER, true, false, System::IO::SearchOption::TopDirectoryOnly, true, true);

      // 2. initialize 3d models for roomobjects, each in a subfolder at depth 1
      if (!Config->Disable3DModels)
         InitResourceGroup(RESOURCEGROUPMODELS, false, true, System::IO::SearchOption::TopDirectoryOnly, true, Config->PreloadObjects);

      // 3. initialize particles, all saved in same folder
      InitResourceGroup(RESOURCEGROUPPARTICLES, true, false, System::IO::SearchOption::TopDirectoryOnly, true, false);

      // 4. initialize legacy sky textures
      InitResourceGroup(RESOURCEGROUPSKY, true, false, System::IO::SearchOption::TopDirectoryOnly, true, false);

      // 5. initialize decoration
      InitResourceGroup(RESOURCEGROUPDECORATION, true, true, System::IO::SearchOption::TopDirectoryOnly, true, true);

      // 6. initialize custom room textures if not disabled
      if (!Config->DisableNewRoomTextures)
         InitResourceGroupManually(TEXTUREGROUP_ROOLOADER, true, Config->PreloadRoomTextures, "Texture", "*.png");

      // 7. initialize caelum group
      InitResourceGroup(RESOURCEGROUPCAELUM, true, false, System::IO::SearchOption::TopDirectoryOnly, true, true);

      // 10. load legacy resources
      ResourceManager->Preload(
         Config->PreloadObjects,
         Config->PreloadRoomTextures,
         Config->PreloadRooms,
         Config->PreloadSound,
         Config->PreloadMusic);

      // build a texture-atlas for all the small 16x16 spell/skill icons which are not built from object
      ControllerUI::BuildIconAtlas();

      // 10. initialize general group
      resMan->initialiseResourceGroup(RESOURCEGROUPGENERAL);

      // remove loadingbar
      ControllerUI::LoadingBar::Finish();

      // run maximum GC
      Common::Util::ForceMaximumGC();
   };

   void OgreClient::InitResourceGroup(
      CLRString^ Name, 
      bool AddRoot, 
      bool AddSubfolders, 
      ::System::IO::SearchOption Recursive, 
      bool Initialize, 
      bool Load)
   {
      // get ogre resource manager
      ::Ogre::ResourceGroupManager& resMan = ::Ogre::ResourceGroupManager::getSingleton();

      // ogrestring for plain name
      const ::Ogre::String& ostr_name = StringConvert::CLRToOgre(Name);

      // create ogre resource group if not exists yet
      if (!resMan.resourceGroupExists(ostr_name))
         resMan.createResourceGroup(ostr_name);

      // path to potential folder and potential zip
      CLRString^ pathDir = Path::Combine(Config->ResourcesPath, Name);
      CLRString^ pathZip = Path::Combine(Config->ResourcesPath, Name + ".zip");

      // check whether folder and or zip exists
      bool existsZip = ::System::IO::File::Exists(pathZip);
      bool existsDir = ::System::IO::Directory::Exists(pathDir);

      // neither zip nor folder found, stop here
      if (!existsZip && !existsDir)
      {
         Logger::Log(MODULENAME, LogType::Error, "Failed to find resource subdirectory or zip: " + Name);
         return;
      }

      // prefer zip file
      // note: zips can only have 1 folder with all files or only files, NO hierarchy.
      if (existsZip)
         resMan.addResourceLocation(StringConvert::CLRToOgre(pathZip), "Zip", ostr_name);

      // otherwise process folder contents
      else
      {
         // see if the folder files should be added
         if (AddRoot)
            resMan.addResourceLocation(StringConvert::CLRToOgre(pathDir), "FileSystem", ostr_name);

         // possibly add subfolders
         if (AddSubfolders)
         {
            // get all subfolders and zip files
            array<CLRString^>^ arrFolders = Directory::GetDirectories(pathDir, "*", SearchOption::TopDirectoryOnly);
            array<CLRString^>^ arrZips = Directory::GetFiles(pathDir, "*.zip", SearchOption::TopDirectoryOnly);

            // add all zips to the resourcegroup
            for each (CLRString^ s in arrZips)
               resMan.addResourceLocation(StringConvert::CLRToOgre(s), "Zip", ostr_name);

            // add subfolders which don't have a zip
            for each (CLRString^ folder in arrFolders)
            {
               bool skip = false;
               for each(CLRString^ zip in arrZips)
               {
                  if ((folder + ".zip") == zip)
                  {
                     skip = true;
                     break;
                  }
               }

               if (!skip)
                  resMan.addResourceLocation(StringConvert::CLRToOgre(folder), "FileSystem", ostr_name);
            }
         }
      }

      // possibly initialize the resource group
      if (Initialize)
      {
         resMan.initialiseResourceGroup(ostr_name);
         resMan.prepareResourceGroup(ostr_name);
      }

      // possibly also load it
      if (Load)
         resMan.loadResourceGroup(ostr_name);
   };

   void OgreClient::InitResourceGroupManually(
      CLRString^ Name, 
      bool Initialize, 
      bool Load, 
      CLRString^ Type, 
      CLRString^ Pattern)
   {
      // get ogre resource manager
      ::Ogre::ResourceGroupManager& resMan = ResourceGroupManager::getSingleton();

      // ogre string for plain name
      const ::Ogre::String& ostr_name = StringConvert::CLRToOgre(Name);
      
      // create resource group if not exists
      if (!resMan.resourceGroupExists(ostr_name))
         resMan.createResourceGroup(ostr_name);

      // path to potential folder and potential zip
      CLRString^ pathDir = Path::Combine(Config->ResourcesPath, Name);
      CLRString^ pathZip = Path::Combine(Config->ResourcesPath, Name + ".zip");

      // check whether folder and or zip exists
      bool existsZip = ::System::IO::File::Exists(pathZip);
      bool existsDir = ::System::IO::Directory::Exists(pathDir);

      // neither zip nor path found
      if (!existsZip && !existsDir)
      {
         Logger::Log(MODULENAME, LogType::Error, "Failed to find resource subdirectory or zip: " + Name);
         return;
      }

      // prefer zip
      CLRString^ pathUse = (existsZip) ? pathZip : pathDir;
      ::Ogre::String& ostr_path = StringConvert::CLRToOgre(pathUse);
      ::Ogre::String ostr_restype  = (existsZip) ? "Zip" : "FileSystem";

      // add folder
      resMan.addResourceLocation(ostr_path, ostr_restype, ostr_name);

      // ogre strings for file types and pattern
      const ::Ogre::String& ostr_type = StringConvert::CLRToOgre(Type);
      const ::Ogre::String& ostr_pattern = StringConvert::CLRToOgre(Pattern);

      // add files manually because not referenced in materials or other reasons
      ::Ogre::FileInfoListPtr fileList = resMan.findResourceFileInfo(ostr_name, ostr_pattern);

      for(unsigned int f = 0; f < fileList->size(); f++)
         resMan.declareResource(fileList->at(f).filename, ostr_type, ostr_name);

      // possibly initialize it
      if (Initialize)
      {
         resMan.initialiseResourceGroup(ostr_name);
         resMan.prepareResourceGroup(ostr_name);
      }

      // possibly also load it
      if (Load)
         resMan.loadResourceGroup(ostr_name);
   };

   void OgreClient::HandleLoginModeMessage(LoginModeMessage^ Message)
   {
      // execute mainapp handling
      SingletonClient::HandleLoginModeMessage(Message);
   };

   void OgreClient::HandleGameModeMessage(GameModeMessage^ Message)
   {
      // execute parentclass handler
      SingletonClient::HandleGameModeMessage(Message);

      // if engine is initialized
      if (root != nullptr)
      {
         // let main controllers hookup their handlers
         ControllerRoom::HandleGameModeMessage(Message);
         ControllerSound::HandleGameModeMessage(Message);
      }
   };

   void OgreClient::HandleLoginOKMessage(LoginOKMessage^ Message)
   {
   };

   void OgreClient::HandleLoginFailedMessage(LoginFailedMessage^ Message)
   {
      // call base handler
      SingletonClient::HandleLoginFailedMessage(Message);

      // attach OK listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed +=
         gcnew System::EventHandler(this, &OgreClient::OnLoginErrorConfirmed);

      // tell user about wrong credentials
      ControllerUI::ConfirmPopup::ShowOK("Your account credentials are not correct.", 0, false);
   };

   void OgreClient::HandleNoCharactersMessage(NoCharactersMessage^ Message)
   {
      // call base handler
      SingletonClient::HandleNoCharactersMessage(Message);

      // attach OK listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed +=
         gcnew System::EventHandler(this, &OgreClient::OnLoginErrorConfirmed);

      // tell user about no characters
      ControllerUI::ConfirmPopup::ShowOK("Your account doesn't have any character slots.", 0, false);
   };

   void OgreClient::HandleLoginModeMessageMessage(LoginModeMessageMessage^ Message)
   {
      // attach OK listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed +=
         gcnew System::EventHandler(this, &OgreClient::OnLoginErrorConfirmed);

      // tell user about wrong credentials
      ControllerUI::ConfirmPopup::ShowOK(StringConvert::CLRToCEGUI(Message->Message), 0, false);
   };

   void OgreClient::HandleGetClientMessage(GetClientMessage^ Message)
   {
      // attach OK listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed +=
         gcnew System::EventHandler(this, &OgreClient::OnLoginErrorConfirmed);
      // tell user about mismatching major/minor version
      ControllerUI::ConfirmPopup::ShowOK("Your major/minor versions don't match the server.", 0, false);
   };

   void OgreClient::HandleDownloadMessage(DownloadMessage^ Message)
   {
      SingletonClient::HandleDownloadMessage(Message);

      // attach OK listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed +=
         gcnew System::EventHandler(this, &OgreClient::OnLoginErrorConfirmed);
      // tell user about mismatching resources version
      ControllerUI::ConfirmPopup::ShowOK("Resources mismatch", 0, false);
   };

   void OgreClient::OnLoginErrorConfirmed(Object ^sender, ::System::EventArgs ^e)
   {
      Disconnect();
   };

   void OgreClient::HandleCharactersMessage(CharactersMessage^ Message)
   {
      // switch ui to avatar selection
      Data->UIMode = UIMode::AvatarSelection;        
   };

   void OgreClient::HandleQuitMessage(QuitMessage^ Message)
   {
      // update or add the currently played actionbuttons to config
      if (Data->ActionButtons->HasPlayerName)
         Config->AddOrUpdateActionButtonSet(Data->ActionButtons);

      SingletonClient::HandleQuitMessage(Message);

      // reload the demoscene
      DemoSceneLoadBrax();
   };

   void OgreClient::HandleGetLoginMessage(GetLoginMessage^ Message)
   {
      ConnectionInfo^ info = Config->SelectedConnectionInfo;
      SendLoginMessage(info->Username, info->Password);
   };

   void OgreClient::HandleLookupNamesMessage(LookupNamesMessage^ Message)
   {
      // currently this is only used by mail recipient lookup
      // so we process it here

      ControllerUI::MailCompose::ProcessResult(Message->ResolvedIDs);
   };

   void OgreClient::SendUseCharacterMessage(ObjectID^ ID, bool RequestBasicInfo, CLRString^ Name)
   {
      // destroy the demo scene, we're going to play!
      DemoSceneDestroy();

      // call base method
      SingletonClient::SendUseCharacterMessage(ID, RequestBasicInfo, Name);

      // try get set
      ActionButtonList^ foundset = Config->GetActionButtonSetByName(Name);

      // none? use default
      if (foundset == nullptr)
         foundset = ActionButtonList::DEFAULT;
           
      // add those from config to active list
      for each (ActionButtonConfig^ btn in foundset)
         Data->ActionButtons->Add(btn);
            
      // set labels to keybinding strings
      if (foundset != nullptr && foundset->Count > 47)
      {
         OISKeyBinding^ keybinding = Config->KeyBinding;
         ::OIS::Keyboard* keyboard = ControllerInput::OISKeyboard;

         foundset[0]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton01));
         foundset[1]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton02));
         foundset[2]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton03));
         foundset[3]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton04));
         foundset[4]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton05));
         foundset[5]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton06));
         foundset[6]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton07));
         foundset[7]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton08));
         foundset[8]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton09));
         foundset[9]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton10));
         foundset[10]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton11));
         foundset[11]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton12));
         foundset[12]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton13));
         foundset[13]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton14));
         foundset[14]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton15));
         foundset[15]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton16));
         foundset[16]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton17));
         foundset[17]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton18));
         foundset[18]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton19));
         foundset[19]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton20));
         foundset[20]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton21));
         foundset[21]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton22));
         foundset[22]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton23));
         foundset[23]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton24));
         foundset[24]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton25));
         foundset[25]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton26));
         foundset[26]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton27));
         foundset[27]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton28));
         foundset[28]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton29));
         foundset[29]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton30));
         foundset[30]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton31));
         foundset[31]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton32));
         foundset[32]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton33));
         foundset[33]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton34));
         foundset[34]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton35));
         foundset[35]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton36));
         foundset[36]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton37));
         foundset[37]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton38));
         foundset[38]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton39));
         foundset[39]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton40));
         foundset[40]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton41));
         foundset[41]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton42));
         foundset[42]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton43));
         foundset[43]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton44));
         foundset[44]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton45));
         foundset[45]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton46));
         foundset[46]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton47));
         foundset[47]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton48));
         foundset[48]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton49));
         foundset[49]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton50));
         foundset[50]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton51));
         foundset[51]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton52));
         foundset[52]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton53));
         foundset[53]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton54));
         foundset[54]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton55));
         foundset[55]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton56));
         foundset[56]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton57));
         foundset[57]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton58));
         foundset[58]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton59));
         foundset[59]->Label = StringConvert::OgreToCLR(keyboard->getAsString(keybinding->ActionButton60));
      }
   };

   void OgreClient::SendReqQuit()
   {
      // call base method
      SingletonClient::SendReqQuit();

      // save layout to config
      ControllerUI::SaveLayoutToConfig();
   };

   void OgreClient::Suicide()
   {
      // attach yes listener to confirm popup
      ControllerUI::ConfirmPopup::Confirmed += 
         gcnew System::EventHandler(this, &OgreClient::OnSuicideConfirmed);

      // show a yes/no dialog
      ControllerUI::ConfirmPopup::ShowChoice("Are you sure?", 0, false);
   };

   void OgreClient::HandlePasswordOKMessage(PasswordOKMessage^ Message)
   {
      // tell user about the change and close the password window
      ControllerUI::ConfirmPopup::ShowOK("Password changed successfully.", 0, false);
   };

   void OgreClient::HandlePasswordNotOKMessage(PasswordNotOKMessage^ Message)
   {
      // tell user about the change and close the password window
      ControllerUI::ConfirmPopup::ShowOK("The server did not accept your new password.", 0, false);
   };

   void OgreClient::ShowAdminForm()
   {
      if (!adminForm || adminForm->IsDisposed)
      {
         adminForm = gcnew AdminForm();
         adminForm->DataController = Data;
         adminForm->ResourceManager = ResourceManager;
         adminForm->PacketSend += gcnew GameMessageEventHandler(this, &OgreClient::OnAdminFormPacketSend);
         adminForm->PacketLogChanged += gcnew PacketLogChangeEventHandler(this, &OgreClient::OnAdminFormPacketLogChanged);
         adminForm->Show();
      }
   };

   void OgreClient::OnAdminFormPacketLogChanged(Object^ sender, PacketLogChangeEventArgs^ e)
   {
      // update the setings in the Data
      Data->LogIncomingMessages = e->LogIncoming;
      Data->LogOutgoingMessages = e->LogOutgoing;
      Data->LogPingMessages = e->LogPings;

      // tell networkclient to potentially loopback sent messages
      ServerConnection->IsOutgoingPacketLogEnabled = e->LogOutgoing;
   };

   void OgreClient::OnSuicideConfirmed(Object ^sender, ::System::EventArgs ^e)
   {
      // suicide the avatar
      OgreClient::Singleton->SendUserCommandSuicide();
   };

   void OgreClient::OnAdminFormPacketSend(Object^ sender, GameMessageEventArgs^ e)
   {
      ServerConnection->SendQueue->Enqueue(e->Message);
   };

   void OgreClient::OnImageComposerOgreRoomObjectCacheRemove(Object^ sender, ImageComposerOgre<RoomObject^>::Cache::ItemEventArgs^ e)
   {
      for each(ImageComposerOgre<RoomObject^>::Cache::Item^ item in e->Items)
      {
         // delete materials and texture
         item->Image->Delete();

         // remove from cache
         ImageComposerOgre<RoomObject^>::Cache::Remove(item);
      }
   };

   void OgreClient::OnImageComposerCEGUIObjectBaseCacheRemove(Object^ sender, ImageComposerCEGUI<ObjectBase^>::Cache::ItemEventArgs^ e)
   {
      for each(ImageComposerCEGUI<ObjectBase^>::Cache::Item^ item in e->Items)
      {
         // delete materials and texture
         item->Image->Delete();

         // remove from cache
         ImageComposerCEGUI<ObjectBase^>::Cache::Remove(item);
      }
   };

   void OgreClient::OnImageComposerCEGUIRoomObjectCacheRemove(Object^ sender, ImageComposerCEGUI<RoomObject^>::Cache::ItemEventArgs^ e)
   {
      for each(ImageComposerCEGUI<RoomObject^>::Cache::Item^ item in e->Items)
      {
         // delete materials and texture
         item->Image->Delete();

         // remove from cache
         ImageComposerCEGUI<RoomObject^>::Cache::Remove(item);
      }
   };

   void OgreClient::OnImageComposerCEGUIInventoryObjectCacheRemove(Object^ sender, ImageComposerCEGUI<InventoryObject^>::Cache::ItemEventArgs^ e)
   {
      for each(ImageComposerCEGUI<InventoryObject^>::Cache::Item^ item in e->Items)
      {
         // delete materials and texture
         item->Image->Delete();

         // remove from cache
         ImageComposerCEGUI<InventoryObject^>::Cache::Remove(item);
      }
   };

   void OgreClient::DemoSceneDestroy()
   {
      IsCameraListenerEnabled = false;
      ControllerRoom::UnloadRoom();
      Data->RoomObjects->Clear();

      sceneManager->getRootSceneNode()->removeChild(CameraNode);

      Camera->setPosition(::Ogre::Vector3(0.0f, 0.0f, 0.0f));
      CameraNode->resetToInitialState();
      CameraNodeOrbit->resetToInitialState();
   };

   void OgreClient::DemoSceneLoadBrax()
   {
      DemoSceneDestroy();

      RoomInfo^ roomInfo = Data->RoomInformation;
      roomInfo->RoomFile = "necarea3.roo";
      roomInfo->AmbientLight = 40;
      roomInfo->ResourceRoom = OgreClient::Singleton->ResourceManager->GetRoom(roomInfo->RoomFile);
      if (roomInfo->ResourceRoom)
         roomInfo->ResourceRoom->ResolveResources(OgreClient::Singleton->ResourceManager);

      ControllerRoom::LoadRoom();

      IsCameraListenerEnabled = true;
      sceneManager->getRootSceneNode()->addChild(CameraNode);

      CameraNode->setPosition(1266, 460, 1344);
      CameraNode->rotate(::Ogre::Vector3::UNIT_Y, ::Ogre::Radian(-0.55f));

      // tree1
      RoomObject^ tree1 = gcnew RoomObject();
      tree1->ID = 1;
      tree1->Animation = gcnew AnimationNone(1);
      tree1->OverlayFile = "nectree3.bgf";
      tree1->Flags->Value = 131136;
      tree1->LightingInfo = gcnew LightingInfo(1, 50, 320);
      tree1->Position3D = V3(1696.0f, 360.0f, 1120.0f);
      tree1->ResolveResources(OgreClient::Singleton->ResourceManager, false);
      Data->RoomObjects->Add(tree1);

      // tree2
      RoomObject^ tree2 = gcnew RoomObject();
      tree2->ID = 2;
      tree2->Animation = gcnew AnimationNone(1);
      tree2->OverlayFile = "nectree2.bgf";
      tree2->Flags->Value = 131136;
      tree2->LightingInfo = gcnew LightingInfo(1, 50, 15360);
      tree2->Position3D = V3(1280.0f, 357.25f, 896.0f);
      tree2->ResolveResources(OgreClient::Singleton->ResourceManager, false);
      Data->RoomObjects->Add(tree2);

      // brazier
      RoomObject^ brazier = gcnew RoomObject();
      brazier->ID = 3;
      brazier->Animation = gcnew AnimationCycle(120, 2, 7);
      brazier->OverlayFile = "brazier.bgf";
      brazier->Flags->Value = 131136;
      brazier->LightingInfo = gcnew LightingInfo(1, 40, 32518);
      brazier->Position3D = V3(1430.0f, 360.0f, 1120.0f);
      brazier->ResolveResources(OgreClient::Singleton->ResourceManager, false);
      Data->RoomObjects->Add(brazier);

      // venya'cyr
      RoomObject^ lich = gcnew RoomObject();
      lich->ID = 4;
      lich->Animation = gcnew AnimationNone(1);
      lich->OverlayFile = "licha.bgf";
      lich->Flags->Value = 1544;
      lich->Angle = 1.49f;
      lich->LightingInfo = gcnew LightingInfo();
      lich->Position3D = V3(1400.0f, 360.0f, 1080.0f);
      lich->ResolveResources(OgreClient::Singleton->ResourceManager, false);
      Data->RoomObjects->Add(lich);

      // narthyl worm
      RoomObject^ worm = gcnew RoomObject();
      worm->ID = 5;
      worm->Animation = gcnew AnimationCycle(150, 1, 4);
      worm->OverlayFile = "darkbeas.bgf";
      worm->Flags->Value = 0;
      worm->Angle = 2.8f;
      worm->LightingInfo = gcnew LightingInfo();
      worm->Position3D = V3(1500.0f, 360.0f, 1130.0f);
      worm->ResolveResources(OgreClient::Singleton->ResourceManager, false);
      Data->RoomObjects->Add(worm);

      PlayMusic^ music = gcnew PlayMusic();
      music->ResourceName = "nec02.ogg";
      music->ResolveResources(OgreClient::Singleton->ResourceManager, false);
      ControllerSound::StartMusic(music);
   };

   void OgreClient::RenderManually()
   {
      // update tick
      GameTick->Tick();

      if (GameTick->CanManualFrameRendered())
      {
         // render frame
         Root->renderOneFrame();
         
         // messagepump
         ::Ogre::WindowEventUtilities::messagePump();
         
         // save tick
         OgreClient::Singleton->GameTick->DidManualFrameRendered();
      }
   };
};};
