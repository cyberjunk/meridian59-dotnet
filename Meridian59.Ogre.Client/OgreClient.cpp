#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	OgreClient::OgreClient()
		: SingletonClient()
	{							
		// Initialize MiniMap instance
		miniMap = gcnew MiniMapCEGUI(Data, 256, 256, 8.0f);

		SLEEPTIME = 0;
		isWinCursorVisible = true;
	};

	void OgreClient::Init()
    {
		// call base init
		SingletonClient::Init();

#ifdef _DEBUG
        // initialize the DebugForm
        ShowDebugForm(); 
#endif	
		/********************************************************************************************************/

		// init sound-engine (irrklang)
        ControllerSound::Initialize();

		/********************************************************************************************************/

		// init the ogre root object, dx9 rendersystem and plugins
		root					= OGRE_NEW ::Ogre::Root();
		renderSystem			= OGRE_NEW ::Ogre::D3D9RenderSystem(0);						
		pluginOctree			= OGRE_NEW ::Ogre::OctreePlugin();
		pluginCaelum			= OGRE_NEW ::Caelum::CaelumPlugin();
		pluginParticleUniverse	= OGRE_NEW ::ParticleUniverse::ParticleUniversePlugin();

		// install plugins into root
		root->installPlugin(pluginOctree);
		root->installPlugin(pluginCaelum);
		root->installPlugin(pluginParticleUniverse);

		// set basic config options on RenderSystem
		// some of these are required for multi monitor support
		renderSystem->setConfigOption("Resource Creation Policy", "Create on all devices");
		renderSystem->setConfigOption("Multi device memory hint", "Auto hardware buffers management");
		renderSystem->setConfigOption("Use Multihead", "Yes");

		// set rendersystem
		root->setRenderSystem(renderSystem);
		
		// init root
		root->initialise(false, WINDOWNAME);

		// get ogre singleton managers
		::Ogre::ResourceGroupManager* resMan	= ::Ogre::ResourceGroupManager::getSingletonPtr();
		::Ogre::TextureManager* texMan			= ::Ogre::TextureManager::getSingletonPtr();
		::Ogre::MaterialManager* matMan			= ::Ogre::MaterialManager::getSingletonPtr();

		/********************************************************************************************************/

		// settings for the dummy renderwindow
		// which serves as hidden primary window
		// the purpose: holds dx9 resources, therefore
		// allows us to destroy/recreate the actual renderwindow
        ::Ogre::NameValuePairList misc;
        misc["FSAA"]			= "0";
        misc["monitorIndex"]	= "0";
		misc["vsync"]			= "false";
        misc["border"]			= "false";
		misc["hidden"]			= "true";
		
		// create the hidden, primary dummy renderwindow
        renderWindowDummy = (D3D9RenderWindow*)root->createRenderWindow(
            "PrimaryWindowDummy", 1, 1, false, &misc);

		renderWindowDummy->setActive(false);
		renderWindowDummy->setAutoUpdated(false);

		/********************************************************************************************************/
		
		// make sure basic resource groups are created
		if (!resMan->resourceGroupExists(RESOURCEGROUPSHADER))
            resMan->createResourceGroup(RESOURCEGROUPSHADER);
		
		if (!resMan->resourceGroupExists(RESOURCEGROUPMODELS))
			resMan->createResourceGroup(RESOURCEGROUPMODELS);

		if (!resMan->resourceGroupExists(MATERIALGROUP_REMOTENODE2D))
            resMan->createResourceGroup(MATERIALGROUP_REMOTENODE2D);

        if (!resMan->resourceGroupExists(TEXTUREGROUP_REMOTENODE2D))
            resMan->createResourceGroup(TEXTUREGROUP_REMOTENODE2D);
		
		if (!resMan->resourceGroupExists(MATERIALGROUP_MOVABLETEXT))
            resMan->createResourceGroup(MATERIALGROUP_MOVABLETEXT);

        if (!resMan->resourceGroupExists(TEXTUREGROUP_MOVABLETEXT))
            resMan->createResourceGroup(TEXTUREGROUP_MOVABLETEXT);
		
		if (!resMan->resourceGroupExists(MATERIALGROUP_PROJECTILENODE2D))
            resMan->createResourceGroup(MATERIALGROUP_PROJECTILENODE2D);

        if (!resMan->resourceGroupExists(TEXTUREGROUP_PROJECTILENODE2D))
            resMan->createResourceGroup(TEXTUREGROUP_PROJECTILENODE2D);

		if (!resMan->resourceGroupExists(MATERIALGROUP_ROOLOADER))
			resMan->createResourceGroup(MATERIALGROUP_ROOLOADER);

		if (!resMan->resourceGroupExists(TEXTUREGROUP_ROOLOADER))
			resMan->createResourceGroup(TEXTUREGROUP_ROOLOADER);
		
		/********************************************************************************************************/

		// init scenemanager
		sceneManager = (OctreeSceneManager*)root->createSceneManager(SceneType::ST_GENERIC);
		sceneManager->setCameraRelativeRendering(true);

		/********************************************************************************************************/

		// create camera listener
		cameraListener = new CameraListener();

		// create camera
		camera = (OctreeCamera*)sceneManager->createCamera(CAMERANAME);
		camera->setPosition(::Ogre::Vector3(0, 0, 0));
		camera->setNearClipDistance(1.0f);
		camera->setListener(cameraListener);

		// create camera node
		cameraNode = sceneManager->createSceneNode(AVATARCAMNODE);
		cameraNode->attachObject(camera);
		cameraNode->setFixedYawAxis(true);
		cameraNode->setInitialState();

		/********************************************************************************************************/

		// create invis refraction texture required for invis shader
		// this must be loaded before InitResources()
		TexturePtr texPtr = texMan->createManual(
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



		/********************************************************************************************************/
		/*                                     CREATE RENDERWINDOW                                              */
		/********************************************************************************************************/

		RenderWindowCreate();


		/********************************************************************************************************/
		/*                                CREATE SCENEMANAGER, CAMERA, VIEWPORT                                 */
		/********************************************************************************************************/

		/********************************************************************************************************/
		/*                          APPLY TEXTUREFILTERING SETTINGS                                             */
		/********************************************************************************************************/

		// set default mipmaps count
		texMan->setDefaultNumMipmaps(Config->NoMipmaps ? 0 : 5);

		if (::System::String::Equals(Config->TextureFiltering, "Off"))
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_NONE);

		else if (::System::String::Equals(Config->TextureFiltering, "Bilinear"))
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_BILINEAR);

		else if (::System::String::Equals(Config->TextureFiltering, "Trilinear"))
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_TRILINEAR);

		else if (::System::String::Equals(Config->TextureFiltering, "Anisotropic x4"))
		{
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
			matMan->setDefaultAnisotropy(4);
		}
		else if (::System::String::Equals(Config->TextureFiltering, "Anisotropic x16"))
		{
			matMan->setDefaultTextureFiltering(TextureFilterOptions::TFO_ANISOTROPIC);
			matMan->setDefaultAnisotropy(16);
		}

		/********************************************************************************************************/
		/*                             APPLY BITMAPSCALING SETTINGS                                             */
		/********************************************************************************************************/

		if (::System::String::Equals(Config->ImageBuilder, "GDI"))
			ImageBuilder::Initialize(ImageBuilderType::GDI);

		else if (::System::String::Equals(Config->ImageBuilder, "DirectDraw"))
			ImageBuilder::Initialize(ImageBuilderType::DirectDraw);

		else if (::System::String::Equals(Config->ImageBuilder, "DirectX"))
			ImageBuilder::Initialize(ImageBuilderType::DirectX);

		if (System::String::Equals(Config->BitmapScaling, "Low"))
		{
			ImageBuilder::GDI::InterpolationMode = ::System::Drawing::Drawing2D::InterpolationMode::NearestNeighbor;
		}
		else if (System::String::Equals(Config->BitmapScaling, "Default"))
		{
			ImageBuilder::GDI::InterpolationMode = ::System::Drawing::Drawing2D::InterpolationMode::Default;
		}
		else if (System::String::Equals(Config->BitmapScaling, "High"))
		{
			ImageBuilder::GDI::InterpolationMode = ::System::Drawing::Drawing2D::InterpolationMode::HighQualityBicubic;
		}

		if (System::String::Equals(Config->TextureQuality, "Low"))
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality = 0.25f; // used in RemoteNode2D		
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality = 0.25f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality = 0.25f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.25f; // used in CEGUI
		}
		else if (System::String::Equals(Config->TextureQuality, "Default"))
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality = 0.5f; // used in RemoteNode2D
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality = 0.5f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality = 0.5f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.5f; // used in CEGUI
		}
		else if (System::String::Equals(Config->TextureQuality, "High"))
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality = 1.0f; // used in RemoteNode2D
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality = 1.0f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality = 1.0f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 1.0f; // used in CEGUI
		}

		/********************************************************************************************************/
		/*                                                                                                      */
		/********************************************************************************************************/

		ControllerInput::Initialize();

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
		misc2["FSAA"]			= StringConvert::CLRToOgre(Config->FSAA);
		misc2["vsync"]			= StringConvert::CLRToOgre(Config->VSync.ToString());
		misc2["border"]			= Config->WindowFrame ? "fixed" : "none";
		misc2["monitorIndex"]	= ::Ogre::StringConverter::toString(Config->Display);

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

		// keep rendering without focus
		renderWindow->setDeactivateOnFocusChange(false);

		// create window event listener
		windowListener = new MyWindowEventListener();

		// attach window event listener
		::Ogre::WindowEventUtilities::addWindowEventListener(
			renderWindow, windowListener);

		// set icon on gamewindow
		LONG iconID = (LONG)LoadIcon(GetModuleHandle(0), MAKEINTRESOURCE(1));
		SetClassLongPtr(renderWindowHandle, GCLP_HICON, iconID);

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
		
		miniMap->Tick(GameTick->Current, GameTick->Span);
		
		// update the invis viewport every second frame
		// and only if there's an invis object
		if (viewportInvis && Data->RoomObjects->HasInvisibleRoomObject())
		{
			if (invisViewportUpdateFlip)			
				viewportInvis->update();

			invisViewportUpdateFlip = !invisViewportUpdateFlip;
		}
	
		/********************************************************************************************************/
		/*                                     RENDER FRAME                                                     */
		/********************************************************************************************************/
		
		if (root)
			root->renderOneFrame();
		
		//if (renderWindow)
		//	::System::Console::WriteLine(((int)renderWindow->getBatchCount()).ToString());

		/********************************************************************************************************/
		/*                                      WM_MESSAGES                                                     */
		/********************************************************************************************************/
		
		::Ogre::WindowEventUtilities::messagePump();

		// .NET alternative
		//::System::Windows::Forms::Application::DoEvents();
    };

	void OgreClient::Cleanup()
    {
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

		ImageComposerCEGUI<ObjectBase^>::Cache->Clear();
		ImageComposerCEGUI<RoomObject^>::Cache->Clear();
		ImageComposerCEGUI<InventoryObject^>::Cache->Clear();
		ImageComposerOgre<RoomObject^>::Cache->Clear();

		cameraListener = nullptr;
		camera = nullptr;
		cameraNode = nullptr;
		viewport = nullptr;
		viewportInvis = nullptr;
		sceneManager = nullptr;

		/********************************************************************************************************/
		/*                                 ENGINE FINALIZATION                                                  */
		/********************************************************************************************************/
		
		// get singleton managers		
		::Ogre::CompositorManager* compMan		= ::Ogre::CompositorManager::getSingletonPtr();
		::Ogre::ParticleSystemManager* partMan	= ::Ogre::ParticleSystemManager::getSingletonPtr();
		::Ogre::ResourceGroupManager* resGrpMan	= ::Ogre::ResourceGroupManager::getSingletonPtr();
		
		// detach all viewports from window
		renderWindowDummy->removeAllListeners();
		renderWindowDummy->removeAllViewports();

		// destroy primary dummywindow and scenemanager
		root->destroyRenderTarget(renderWindowDummy);
		
		// some important shutdowns
		compMan->removeAll();
		partMan->removeAllTemplates();
		resGrpMan->shutdownAll();

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

		pluginOctree			= nullptr;
		pluginCaelum			= nullptr;
		pluginParticleUniverse	= nullptr;
		renderSystem			= nullptr;
		root					= nullptr;

        /********************************************************************************************************/
		/*                              ACTION BUTTON / CONFIG SAVING                                           */
		/********************************************************************************************************/
		
		if (Data->ActionButtons->HasPlayerName)
        {
            ActionButtonList^ set = 
                Config->GetActionButtonSetByName(Data->ActionButtons->PlayerName);

            if (set != nullptr)
            {
                // clear old assignments
                set->Clear();

                // fill new assigned actionbuttons
                for each (ActionButtonConfig^ btn in Data->ActionButtons)
                    set->Add(btn);
            }
            else
            {
                // create new buttonlist
                ActionButtonList^ btnList = gcnew ActionButtonList();
                btnList->PlayerName = Data->ActionButtons->PlayerName;

                // fill new assigned actionbuttons
                for each (ActionButtonConfig^ btn in Data->ActionButtons)
                    btnList->Add(btn);

                Config->ActionButtonSets->Add(btnList);
            }
        }

		// update ignorelist from data to config
		ConnectionInfo^ conInfo = Config->SelectedConnectionInfo;

		if (conInfo)
		{
			conInfo->IgnoreList->Clear();
			for each(::System::String^ s in Data->IgnoreList)
				conInfo->IgnoreList->Add(s);
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
        //LauncherClient::OnServerConnectionException(Error);
        
		// tell user about unknown exception
        ::System::Windows::Forms::MessageBox::Show(
			Error->Message, "Error",
            ::System::Windows::Forms::MessageBoxButtons::OK, 
			::System::Windows::Forms::MessageBoxIcon::Error,
			::System::Windows::Forms::MessageBoxDefaultButton::Button1,
            ::System::Windows::Forms::MessageBoxOptions::DefaultDesktopOnly, 
			false);

		Disconnect();
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
		InitResourceGroup("Caelum", true, false, System::IO::SearchOption::TopDirectoryOnly, true, true);
		
		// 10. load legacy resources
		ResourceManager->Preload(
			Config->PreloadObjects,
			Config->PreloadRoomTextures,
			Config->PreloadRooms,
			Config->PreloadSound,
			Config->PreloadMusic);

		// .NET 4.5
		// next gc run, defragment the largeobjectheap
		::System::Runtime::GCSettings::LargeObjectHeapCompactionMode =
			System::Runtime::GCLargeObjectHeapCompactionMode::CompactOnce;
		
		// make maximum gc run
		::System::GC::Collect(2, ::System::GCCollectionMode::Forced, true);
		//::System::GC::Collect(2, ::System::GCCollectionMode::Forced);

        // 10. initialize general group
        resMan->initialiseResourceGroup(RESOURCEGROUPGENERAL);
			 
        // remove loadingbar
        ControllerUI::LoadingBar::Finish();
    };

	void OgreClient::InitResourceGroup(
		::System::String^ Name, 
		bool AddRoot, 
		bool AddSubfolders, 
        ::System::IO::SearchOption Recursive, 
		bool Initialize, 
		bool Load)
    {
		::Ogre::ResourceGroupManager* resMan = ::Ogre::ResourceGroupManager::getSingletonPtr();
		
		::System::String^ path	= Path::Combine(Config->ResourcesPath, Name);
		::Ogre::String ostr_name = StringConvert::CLRToOgre(Name);
		::Ogre::String ostr_path = StringConvert::CLRToOgre(path);
			
        // create if not exists
        if (!resMan->resourceGroupExists(ostr_name))
            resMan->createResourceGroup(ostr_name);
 
        // if path exists
        if (::System::IO::Directory::Exists(path))
        {
			
			// possibly add folder itself
			if (AddRoot)
				resMan->addResourceLocation(ostr_path, "FileSystem", ostr_name);

			// possibly add subfolders
			if (AddSubfolders)
			{
				// get all subfolders (possibly recursive)
				array<System::String^>^ folders = Directory::GetDirectories(path, "*", Recursive);

				// add subfolders as resource locations
				for each (System::String^ s in folders)				
					resMan->addResourceLocation(StringConvert::CLRToOgre(s), "FileSystem", ostr_name);				
			}
			
            // possibly initialize it
            if (Initialize)
                resMan->initialiseResourceGroup(ostr_name);

            // possibly also load it
            if (Load)
                resMan->loadResourceGroup(ostr_name);
        }   
    };

	void OgreClient::InitResourceGroupManually(
		::System::String^ Name, 
		bool Initialize, 
		bool Load, 
		::System::String^ Type, 
		::System::String^ Pattern)
	{
		::Ogre::ResourceGroupManager* resMan = ResourceGroupManager::getSingletonPtr();
		
		::System::String^ path		= ::System::IO::Path::Combine(Config->ResourcesPath, Name);
        ::Ogre::String ostr_name	= StringConvert::CLRToOgre(Name);
		::Ogre::String ostr_type	= StringConvert::CLRToOgre(Type);
		::Ogre::String ostr_pattern = StringConvert::CLRToOgre(Pattern);
		::Ogre::String ostr_path	= StringConvert::CLRToOgre(path);
			
        // create if not exists
        if (!resMan->resourceGroupExists(ostr_name))
            resMan->createResourceGroup(ostr_name);
            
        // if path exists
        if (::System::IO::Directory::Exists(path))
        {		
			// add folder
			resMan->addResourceLocation(ostr_path, "FileSystem", ostr_name);
			
			// add files manually because not referenced in materials or other reasons
			::Ogre::FileInfoListPtr fileList = resMan->findResourceFileInfo(ostr_name, ostr_pattern);
			
			for(unsigned int f = 0; f < fileList->size(); f++)		
				resMan->declareResource(fileList->at(f).filename, ostr_type, ostr_name);
			
            // possibly initialize it
            if (Initialize)
                resMan->initialiseResourceGroup(ostr_name);

            // possibly also load it
            if (Load)
                resMan->loadResourceGroup(ostr_name);
		}
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

        // tell user about wrong credentials
        ::System::Windows::Forms::MessageBox::Show(WRONGCREDENTIALS);

		Disconnect();
	};

	void OgreClient::HandleNoCharactersMessage(NoCharactersMessage^ Message)
    {
        // call base handler
        SingletonClient::HandleNoCharactersMessage(Message);

        // tell user about wrong credentials
        ::System::Windows::Forms::MessageBox::Show(NOCHARACTERS);

		Disconnect();
    };

	void OgreClient::HandleLoginModeMessageMessage(LoginModeMessageMessage^ Message)
    {
        // tell user about wrong credentials
        ::System::Windows::Forms::MessageBox::Show(Message->Message);

		Disconnect();
    };

	void OgreClient::HandleGetClientMessage(GetClientMessage^ Message)
	{
		// tell user about mismatching major/minor version
        ::System::Windows::Forms::MessageBox::Show(APPVERSIONMISMATCH);

		Disconnect();
	};

	void OgreClient::HandleDownloadMessage(DownloadMessage^ Message)
	{
		SingletonClient::HandleDownloadMessage(Message);

		// tell user about mismatching resources version
        ::System::Windows::Forms::MessageBox::Show("Resources mismatch");

		Disconnect();
	};
		
	void OgreClient::HandleCharactersMessage(CharactersMessage^ Message)
    {
		// switch ui to avatar selection
        Data->UIMode = UIMode::AvatarSelection;        
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

	void OgreClient::SendUseCharacterMessage(ObjectID^ ID, bool RequestBasicInfo, ::System::String^ Name)
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
        }
    };

	void OgreClient::Suicide()
    {
		// show	
		ControllerUI::ConfirmPopup::Window->show();
		ControllerUI::ConfirmPopup::Window->moveToFront();
	};

#ifdef _DEBUG
	void OgreClient::ShowDebugForm()
    {
        debugForm = gcnew DebugForm();
        debugForm->DataController = Data;
        debugForm->ResourceManager = ResourceManager;
        debugForm->PacketSend += gcnew GameMessageEventHandler(this, &OgreClient::debugForm_PacketSend);
        debugForm->PacketLogChanged += gcnew PacketLogChangeEventHandler(this, &OgreClient::debugForm_PacketLogChanged);
        debugForm->Show();
    };

	void OgreClient::debugForm_PacketLogChanged(Object^ sender, PacketLogChangeEventArgs^ e)
    {
        // update the setings in the Data
        Data->LogIncomingMessages = e->LogIncoming;
        Data->LogOutgoingMessages = e->LogOutgoing;
        Data->LogPingMessages = e->LogPings;

        // tell networkclient to potentially loopback sent messages
        ServerConnection->IsOutgoingPacketLogEnabled = e->LogOutgoing;
    };

	void OgreClient::debugForm_PacketSend(Object^ sender, GameMessageEventArgs^ e)
    {
        ServerConnection->SendQueue->Enqueue(e->Message);
    };
#endif


	void OgreClient::DemoSceneDestroy()
	{
		CameraNode->resetToInitialState();
		
		ControllerRoom::UnloadRoom();
	};

	void OgreClient::DemoSceneLoadBrax()
	{
		RoomInfo^ roomInfo = Data->RoomInformation;


		CameraNode->resetToInitialState(); 
		CameraNode->setPosition(1266, 460, 1344);		
		CameraNode->rotate(::Ogre::Vector3::UNIT_Y, ::Ogre::Radian(-0.55f));
		
		roomInfo->RoomFile = "necarea3.roo";
		roomInfo->AmbientLight = 40;
		roomInfo->ResolveResources(OgreClient::Singleton->ResourceManager, false);

		ControllerRoom::LoadRoom();

		// tree1
		RoomObject^ tree1 = gcnew RoomObject();
		tree1->ID = 1;
		tree1->Animation = gcnew AnimationNone(1);
		tree1->OverlayFile = "nectree3.bgf";
		tree1->Flags->Value = 131136;
		tree1->LightFlags = 1;
		tree1->LightIntensity = 50;
		tree1->LightColor = 320;
		tree1->Position3D = V3(1696.0f, 360.0f, 1120.0f);
		tree1->ResolveResources(OgreClient::Singleton->ResourceManager, false);
		Data->RoomObjects->Add(tree1);

		// tree2
		RoomObject^ tree2 = gcnew RoomObject();
		tree2->ID = 2;
		tree2->Animation = gcnew AnimationNone(1);
		tree2->OverlayFile = "nectree2.bgf";
		tree2->Flags->Value = 131136;
		tree2->LightFlags = 1;
		tree2->LightIntensity = 50;
		tree2->LightColor = 15360;
		tree2->Position3D = V3(1280.0f, 357.25f, 896.0f);
		tree2->ResolveResources(OgreClient::Singleton->ResourceManager, false);
		Data->RoomObjects->Add(tree2);

		// brazier
		RoomObject^ brazier = gcnew RoomObject();
		brazier->ID = 3;
		brazier->Animation = gcnew AnimationCycle(120, 2, 7);
		brazier->OverlayFile = "brazier.bgf";
		brazier->Flags->Value = 131136;
		brazier->LightFlags = 1;
		brazier->LightIntensity = 40;
		brazier->LightColor = 32518;
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
		lich->LightFlags = 0;
		lich->LightIntensity = 0;
		lich->LightColor = 0;
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
		worm->LightFlags = 0;
		worm->LightIntensity = 0;
		worm->LightColor = 0;
		worm->Position3D = V3(1500.0f, 360.0f, 1130.0f);
		worm->ResolveResources(OgreClient::Singleton->ResourceManager, false);
		Data->RoomObjects->Add(worm);

		PlayMusic^ music = gcnew PlayMusic();
		music->ResourceName = "nec02.mp3";
		music->ResolveResources(OgreClient::Singleton->ResourceManager, false);
		ControllerSound::StartMusic(music);
	};
};};
