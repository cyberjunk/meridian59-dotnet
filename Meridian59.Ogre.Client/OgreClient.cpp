#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	OgreClient::OgreClient()
		: SingletonClient()
	{							
        // set separator for xml files
        XmlReaderExtensions::NumberFormatInfo->NumberDecimalSeparator = ".";

		// Initialize MiniMap instance
		miniMap = gcnew MiniMapCEGUI(Data, 256, 256);

		SLEEPTIME = 0;
		isEngineInitialized = false;
		isWinCursorVisible = true;
	};

	void OgreClient::Init()
    {
		// init the legacy resources
		ResourceManager->Init(
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHSTRINGS,
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHROOMS,
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHOBJECTS,
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHROOMTEXTURES,
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHSOUNDS,
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHMUSIC,
			Config->ResourcesPath + "/" + Meridian59::Files::ResourceManager::SUBPATHMAILS);

        // show launcher ui
        ShowLauncherForm();

#ifdef _DEBUG
        // initialize the DebugForm
        ShowDebugForm(); 
#endif	
		// init sound-engine (irrklang)
        ControllerSound::Initialize();

		// init the ogre root object, dx9 rendersystem and plugins
		root					= OGRE_NEW ::Ogre::Root();
		renderSystem			= OGRE_NEW ::Ogre::D3D9RenderSystem(0);						
		pluginOctree			= OGRE_NEW ::Ogre::OctreePlugin();
		pluginCG				= OGRE_NEW ::Ogre::CgPlugin();
		pluginCaelum			= OGRE_NEW ::Caelum::CaelumPlugin();
		pluginParticleUniverse	= OGRE_NEW ::ParticleUniverse::ParticleUniversePlugin();

		// install plugins into root
		root->installPlugin(pluginOctree);
		root->installPlugin(pluginCG);
		root->installPlugin(pluginCaelum);
		root->installPlugin(pluginParticleUniverse);

		// set static config options on RenderSystem
		renderSystem->setConfigOption("Resource Creation Policy", "Create on all devices");
		renderSystem->setConfigOption("Multi device memory hint", "Auto hardware buffers management");
		renderSystem->setConfigOption("Use Multihead", "Yes");
		//renderSystem->setConfigOption("Allow DirectX9Ex", "Yes");
		//renderSystem->setConfigOption("Full Screen", isFullScreen);
		//renderSystem->setConfigOption("Video Mode", "640 x 480 @ 32-bit colour");
		//renderSystem->setConfigOption("FSAA", "0");

		// set rendersystem
		root->setRenderSystem(renderSystem);
		
		// init root
		root->initialise(false, WINDOWNAME);

		// settings for the dummy renderwindow
		// which serves as hidden primary window
        ::Ogre::NameValuePairList misc;
        misc["FSAA"]			= "0";
        misc["monitorIndex"]	= "0";
		misc["vsync"]			= "false";
        misc["border"]			= "false";
		misc["hidden"]			= "true";
		
		// create the hidden, primary dummy renderwindow
        renderWindowDummy = root->createRenderWindow(
            "PrimaryWindowDummy", 1, 1, false, &misc);

		renderWindowDummy->setActive(false);
		renderWindowDummy->setAutoUpdated(false);

		// get singleton managers
		::Ogre::ResourceGroupManager* resMan = ::Ogre::ResourceGroupManager::getSingletonPtr();

		// make sure some resource groups are created
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
    };

	void OgreClient::InitEngine()
	{
		if (isEngineInitialized)
			return;
										
		::Ogre::MaterialManager* matMan	= ::Ogre::MaterialManager::getSingletonPtr();
		::Ogre::TextureManager* texMan	= ::Ogre::TextureManager::getSingletonPtr();
		
		/********************************************************************************************************/
		/*                                       INIT IMAGEBUILDER                                              */
		/********************************************************************************************************/

		if (::System::String::Equals(Config->ImageBuilder, "GDI"))  
			ImageBuilder::Initialize(ImageBuilderType::GDI);
		
		else if (::System::String::Equals(Config->ImageBuilder, "DirectDraw"))
			ImageBuilder::Initialize(ImageBuilderType::DirectDraw);

		else if (::System::String::Equals(Config->ImageBuilder, "DirectX"))
			ImageBuilder::Initialize(ImageBuilderType::DirectX);
		
		/********************************************************************************************************/
		/*                                     CREATE RENDERWINDOW                                              */
		/********************************************************************************************************/

		// settings for the main (but not primary) renderwindow
        ::Ogre::NameValuePairList misc;
        misc["FSAA"]			= StringConvert::CLRToOgre(Config->FSAA);
        misc["vsync"]			= StringConvert::CLRToOgre(Config->VSync.ToString());
        misc["border"]			= Config->WindowFrame ? "fixed" : "none";
		misc["monitorIndex"]	= ::Ogre::StringConverter::toString(Config->Display);
				
		// get window height & width from options
        int index = Config->Resolution->IndexOf('x');
		System::UInt32 windowwidth = System::Convert::ToUInt32(Config->Resolution->Substring(0, index - 1));
        System::UInt32 windowheight = System::Convert::ToUInt32(Config->Resolution->Substring(index + 2));
		
		// create the main (but not primary) renderwindow
        renderWindow = root->createRenderWindow(
            WINDOWNAME,
            windowwidth,
            windowheight,
            !Config->WindowMode,
            &misc);
				
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
		LONG iconID = (LONG)LoadIcon( GetModuleHandle(0), MAKEINTRESOURCE(1) );
		SetClassLongPtr(renderWindowHandle, GCLP_HICON, iconID );
		
		/********************************************************************************************************/
		/*                                CREATE SCENEMANAGER, CAMERA, VIEWPORT                                 */
		/********************************************************************************************************/
		
        // init scenemanager
        sceneManager = root->createSceneManager(SceneType::ST_GENERIC);
        sceneManager->setCameraRelativeRendering(true);

        // create camera listener
        cameraListener = new CameraListener();

        // create camera
        camera = sceneManager->createCamera(CAMERANAME);
        camera->setPosition(::Ogre::Vector3(0, 0, 0));
        camera->setNearClipDistance(1.0f);
        camera->setListener(cameraListener);
		
        // create camera node
        cameraNode = sceneManager->createSceneNode(AVATARCAMNODE);
        cameraNode->attachObject(camera);
		cameraNode->setFixedYawAxis(true);
			   
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

        // create viewport
        viewport = renderWindow->addViewport(camera, 0);

		// set camera aspect ratio based on viewport
		::Ogre::Real aspectRatio = 
			::Ogre::Real(viewport->getActualWidth()) / Ogre::Real(viewport->getActualHeight());
        
		camera->setAspectRatio(aspectRatio);

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
			ImageComposerOgre<RoomObject^>::DefaultQuality		 = 0.25f; // used in RemoteNode2D		
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality		 = 0.25f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality		 = 0.25f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.25f; // used in CEGUI
		}
		else if (System::String::Equals(Config->TextureQuality, "Default"))
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality		 = 0.5f; // used in RemoteNode2D
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality		 = 0.5f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality		 = 0.5f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 0.5f; // used in CEGUI
		}
		else if (System::String::Equals(Config->TextureQuality, "High"))
		{
			ImageComposerOgre<RoomObject^>::DefaultQuality		 = 1.0f; // used in RemoteNode2D
			ImageComposerCEGUI<ObjectBase^>::DefaultQuality		 = 1.0f; // used in CEGUI
			ImageComposerCEGUI<RoomObject^>::DefaultQuality		 = 1.0f; // used in CEGUI
			ImageComposerCEGUI<InventoryObject^>::DefaultQuality = 1.0f; // used in CEGUI
		}

		/********************************************************************************************************/
		/*                                                                                                      */
		/********************************************************************************************************/
		
		// init cegui
		ControllerUI::Initialize((::Ogre::RenderTarget*)renderWindow);
		
		// set ui to loadingbar
		Data->UIMode = UIMode::LoadingBar;
			
		// initial framerendering (no loop yet)
		root->renderOneFrame();
		
        // initialize resources
        InitResources();

        // don't go on if window doesn't exit anymore
        if (!renderWindow->isClosed())
        {            
			ControllerSound::MusicVolume = (float)Config->MusicVolume / 10.0f;
			
            // Init controllers
            ControllerRoom::Initialize();
            ControllerInput::Initialize();
			ControllerEffects::Initialize();
		
			// set UI to avatarselect
			Data->UIMode = UIMode::AvatarSelection;
        }

		isEngineInitialized = true;
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
				
		if (isEngineInitialized)
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
		
		::System::Windows::Forms::Application::DoEvents();
    };

	void OgreClient::Cleanup()
    {
		CleanupEngine();
		
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
		root->uninstallPlugin(pluginCG);
		root->uninstallPlugin(pluginCaelum);
		root->uninstallPlugin(pluginParticleUniverse);

		// delete plugins
		OGRE_DELETE pluginOctree;
		OGRE_DELETE pluginCG;
		//OGRE_DELETE pluginCaelum; // deletes itself at uninstall
		OGRE_DELETE pluginParticleUniverse;

		OGRE_DELETE root;

		// cleanup sound-engine
		ControllerSound::Destroy();

		pluginOctree			= nullptr;
		pluginCG				= nullptr;
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

		// base class call
		SingletonClient::Cleanup();      
    };

	void OgreClient::CleanupEngine()
    {
		if (!isEngineInitialized)
			return;
				
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
			sceneManager->destroySceneNode(cameraNode);

		// clear all remaining stuff
		sceneManager->clearScene();
		
		// destroy scenemanager
		root->destroySceneManager(sceneManager);
		
		// cleanup renderwindow
		if (renderWindow)
		{
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
		}
       
		if (cameraListener)
			OGRE_DELETE cameraListener;

		ImageComposerCEGUI<ObjectBase^>::Cache->Clear();
		ImageComposerCEGUI<RoomObject^>::Cache->Clear();
		ImageComposerCEGUI<InventoryObject^>::Cache->Clear();
		ImageComposerOgre<RoomObject^>::Cache->Clear();
		
		cameraListener	= nullptr;
		windowListener	= nullptr;
		camera			= nullptr;
		cameraNode		= nullptr;		
        renderWindow	= nullptr;       
		viewport		= nullptr;
		viewportInvis	= nullptr;
		sceneManager	= nullptr;
		
		isEngineInitialized = false;
    };

	void OgreClient::ShowLauncherForm()
    {
        if (launcherForm == nullptr || launcherForm->IsDisposed)
        {
            launcherForm = gcnew LauncherForm();
            launcherForm->Options = Config;
			launcherForm->ResourceManager = ResourceManager;
            launcherForm->ConnectRequest += gcnew ::System::EventHandler(this, &OgreClient::OnLauncherConnectRequest);
            launcherForm->Exit += gcnew ::System::EventHandler(this, &OgreClient::OnLauncherFormExit);
            launcherForm->Show();
        }
    };

	void OgreClient::OnLauncherConnectRequest(::System::Object^ sender, ::System::EventArgs^ e)
    {
		Connect();
    };

	void OgreClient::OnLauncherFormExit(::System::Object^ sender, ::System::EventArgs^ e)
    {
        // exit app when user exits launcherform
        IsRunning = false;
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

        Data->UIMode = UIMode::None;

        // reenable launcher controls
        launcherForm->SwitchEnabled();


		Data->Reset();

        CleanupEngine();

        ShowLauncherForm();
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
        InitResourceGroup(RESOURCEGROUPSKY, true, false, System::IO::SearchOption::TopDirectoryOnly, true, true);
		
		// 5. initialize decoration
		InitResourceGroup(RESOURCEGROUPDECORATION, true, true, System::IO::SearchOption::TopDirectoryOnly, true, true);
		
        // 6. initialize custom room textures if not disabled
		if (!Config->DisableNewRoomTextures)
			InitResourceGroupManually(TEXTUREGROUP_ROOLOADER, true, Config->PreloadRoomTextures, "Texture", "*.png");
	
		// 7. initialize caelum group
		if (!Config->DisableNewSky)		
			InitResourceGroup("Caelum", true, false, System::IO::SearchOption::TopDirectoryOnly, true, true);
		
        // 8. init addition resources from resources.cfg
        // they go into ogre's default "General" group
        System::String^ file		= Path::Combine(Config->ResourcesPath, RESOURCESCFGFILE);
        ::Ogre::String ostr_file	= StringConvert::CLRToOgre(file);

		::Ogre::ConfigFile cf = ::Ogre::ConfigFile();
        cf.load(ostr_file, "\t:=", true);

        ::Ogre::ConfigFile::SectionIterator seci = cf.getSectionIterator();
        ::Ogre::String secName;
		::Ogre::String typeName;
		::Ogre::String archName;

        while (seci.hasMoreElements())
        {
			secName = seci.peekNextKey();

			ConfigFile::SettingsMultiMap* settings = seci.getNext();
			ConfigFile::SettingsMultiMap::iterator i;

			for (i = settings->begin(); i != settings->end(); ++i)
			{
				typeName = i->first;
				archName = i->second;
				resMan->addResourceLocation(archName, typeName, secName);
			}
        }

		// 10. load legacy resources
		if (Config->PreloadObjects)	
			ResourceManager->PreloadObjects();

		if (Config->PreloadRoomTextures)
			ResourceManager->PreloadRoomTextures();

		if (Config->PreloadRooms)
			ResourceManager->PreloadRooms();

		if (Config->PreloadSound)
			ResourceManager->PreloadSounds();

		if (Config->PreloadMusic)
			ResourceManager->PreloadMusic();

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

        // try disconnect
        ServerConnection->Disconnect();

        // tell user about wrong credentials
        ::System::Windows::Forms::MessageBox::Show(WRONGCREDENTIALS);

        // reenable launchercontrols
        launcherForm->SwitchEnabled();
	};

	void OgreClient::HandleNoCharactersMessage(NoCharactersMessage^ Message)
    {
        // call base handler
        SingletonClient::HandleNoCharactersMessage(Message);

        // tell user about wrong credentials
        ::System::Windows::Forms::MessageBox::Show(NOCHARACTERS);

        // reenable launchercontrols
        launcherForm->SwitchEnabled();
    };

	void OgreClient::HandleLoginModeMessageMessage(LoginModeMessageMessage^ Message)
    {
        // tell user about wrong credentials
        ::System::Windows::Forms::MessageBox::Show(Message->Message);

        // reenable launchercontrols
        launcherForm->SwitchEnabled();
    };

	void OgreClient::HandleGetClientMessage(GetClientMessage^ Message)
	{
		// tell user about mismatching major/minor version
        ::System::Windows::Forms::MessageBox::Show(APPVERSIONMISMATCH);

        // close connection, we're not going to download the proposed meridian.exe
        ServerConnection->Disconnect();

        // reenable launcher controls
        launcherForm->SwitchEnabled();
	};

	void OgreClient::HandleDownloadMessage(DownloadMessage^ Message)
	{
		SingletonClient::HandleDownloadMessage(Message);

		// tell user about mismatching resources version
        ::System::Windows::Forms::MessageBox::Show("Resources mismatch");

        // close connection, we're not going to download
        ServerConnection->Disconnect();

        // reenable launcher controls
        launcherForm->SwitchEnabled();
	};
		
	void OgreClient::HandleCharactersMessage(CharactersMessage^ Message)
    {
		// execute parentclass handler
        //LauncherClient::HandleCharactersMessage(Message);

		// close launcher
        launcherForm->Close();

        // call engine initialization
        if (Data->UIMode == UIMode::None)      
            InitEngine();
        
        else      
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
            foundset[0]->Label = Config->KeyBinding->ActionButton01.ToString();
            foundset[1]->Label = Config->KeyBinding->ActionButton02.ToString();
            foundset[2]->Label = Config->KeyBinding->ActionButton03.ToString();
            foundset[3]->Label = Config->KeyBinding->ActionButton04.ToString();
            foundset[4]->Label = Config->KeyBinding->ActionButton05.ToString();
            foundset[5]->Label = Config->KeyBinding->ActionButton06.ToString();
            foundset[6]->Label = Config->KeyBinding->ActionButton07.ToString();
            foundset[7]->Label = Config->KeyBinding->ActionButton08.ToString();
            foundset[8]->Label = Config->KeyBinding->ActionButton09.ToString();
            foundset[9]->Label = Config->KeyBinding->ActionButton10.ToString();
            foundset[10]->Label = Config->KeyBinding->ActionButton11.ToString();
            foundset[11]->Label = Config->KeyBinding->ActionButton12.ToString();
            foundset[12]->Label = Config->KeyBinding->ActionButton13.ToString();
            foundset[13]->Label = Config->KeyBinding->ActionButton14.ToString();
            foundset[14]->Label = Config->KeyBinding->ActionButton15.ToString();
            foundset[15]->Label = Config->KeyBinding->ActionButton16.ToString();
            foundset[16]->Label = Config->KeyBinding->ActionButton17.ToString();
            foundset[17]->Label = Config->KeyBinding->ActionButton18.ToString();
            foundset[18]->Label = Config->KeyBinding->ActionButton19.ToString();
            foundset[19]->Label = Config->KeyBinding->ActionButton20.ToString();
            foundset[20]->Label = Config->KeyBinding->ActionButton21.ToString();
            foundset[21]->Label = Config->KeyBinding->ActionButton22.ToString();
            foundset[22]->Label = Config->KeyBinding->ActionButton23.ToString();
            foundset[23]->Label = Config->KeyBinding->ActionButton24.ToString();
            foundset[24]->Label = Config->KeyBinding->ActionButton25.ToString();
            foundset[25]->Label = Config->KeyBinding->ActionButton26.ToString();
            foundset[26]->Label = Config->KeyBinding->ActionButton27.ToString();
            foundset[27]->Label = Config->KeyBinding->ActionButton28.ToString();
            foundset[28]->Label = Config->KeyBinding->ActionButton29.ToString();
            foundset[29]->Label = Config->KeyBinding->ActionButton30.ToString();
            foundset[30]->Label = Config->KeyBinding->ActionButton31.ToString();
            foundset[31]->Label = Config->KeyBinding->ActionButton32.ToString();
            foundset[32]->Label = Config->KeyBinding->ActionButton33.ToString();
            foundset[33]->Label = Config->KeyBinding->ActionButton34.ToString();
            foundset[34]->Label = Config->KeyBinding->ActionButton35.ToString();
            foundset[35]->Label = Config->KeyBinding->ActionButton36.ToString();
            foundset[36]->Label = Config->KeyBinding->ActionButton37.ToString();
            foundset[37]->Label = Config->KeyBinding->ActionButton38.ToString();
            foundset[38]->Label = Config->KeyBinding->ActionButton39.ToString();
            foundset[39]->Label = Config->KeyBinding->ActionButton40.ToString();
            foundset[40]->Label = Config->KeyBinding->ActionButton41.ToString();
            foundset[41]->Label = Config->KeyBinding->ActionButton42.ToString();
            foundset[42]->Label = Config->KeyBinding->ActionButton43.ToString();
            foundset[43]->Label = Config->KeyBinding->ActionButton44.ToString();
            foundset[44]->Label = Config->KeyBinding->ActionButton45.ToString();
            foundset[45]->Label = Config->KeyBinding->ActionButton46.ToString();
            foundset[46]->Label = Config->KeyBinding->ActionButton47.ToString();
            foundset[47]->Label = Config->KeyBinding->ActionButton48.ToString();
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
};};
