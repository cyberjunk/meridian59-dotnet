#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	static ControllerRoom::ControllerRoom()
	{
		roomGeometry			= nullptr;
		roomNode				= nullptr;
		grassMaterials			= nullptr;
		decoration				= nullptr;
		caelumSystem			= nullptr;
		avatarObject			= nullptr;
		particleSysSnow			= nullptr;
		customParticleHandlers	= nullptr;		
		verticesProcessed		= 0;
	};
	
	RooFile^ ControllerRoom::Room::get()
	{
		return OgreClient::Singleton->Data->RoomInformation->ResourceRoom;
	};

	::Ogre::SceneManager* ControllerRoom::SceneManager::get()
	{
		return OgreClient::Singleton->SceneManager;
	};

	void ControllerRoom::Initialize()
	{	
		if (IsInitialized)
			return;
		
		// init collections
		grassMaterials	= gcnew ::System::Collections::Generic::Dictionary<unsigned short, array<System::String^>^>();
		decoration		= new ::std::vector<ManualObject*>();
		
		// create static geometry
		roomGeometry = SceneManager->createStaticGeometry(NAME_ROOMGEOMETRY);
		roomGeometry->setRegionDimensions(::Ogre::Vector3(5000.0f, 5000.0f, 5000.0f));

		// create room scenenode
		roomNode = SceneManager->getRootSceneNode()->createChildSceneNode(NAME_ROOMNODE);
		roomNode->setPosition(::Ogre::Vector3(64.0f, 0, 64.0f));
        roomNode->setInitialState();
        
		// create decoration mapping
		LoadImproveData();

		// init caelum
		InitCaelum();
	
		// init room based particle systems
		InitParticleSystems();

		/******************************************************************/

		// projectiles listener
        OgreClient::Singleton->Data->Projectiles->ListChanged += 
			gcnew ListChangedEventHandler(&ControllerRoom::OnProjectilesListChanged);
            
		// roomobjects listener
        OgreClient::Singleton->Data->RoomObjects->ListChanged += 
			gcnew ListChangedEventHandler(&ControllerRoom::OnRoomObjectsListChanged);
        
		// camera-position listener
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerRoom::OnDataPropertyChanged);

		// effects listeners
		OgreClient::Singleton->Data->Effects->Snowing->PropertyChanged +=
			gcnew PropertyChangedEventHandler(&ControllerRoom::OnEffectSnowingPropertyChanged);

		/******************************************************************/

        // add existing objects to scene
        for each (RoomObject^ roomObject in OgreClient::Singleton->Data->RoomObjects)
            RoomObjectAdd(roomObject);

		// add existing projectiles to scene
		for each (Projectile^ projectile in OgreClient::Singleton->Data->Projectiles)
			ProjectileAdd(projectile);

		/******************************************************************/

		IsInitialized = true;		
	};
	
	void ControllerRoom::InitCaelum()
	{
		// don't init twice or if disabled
		if (caelumSystem || OgreClient::Singleton->Config->DisableNewSky)
			return;

		/**************************** 1. INIT *******************************************************/
		
		// configuration flags for caelum
		::Caelum::CaelumSystem::CaelumComponent flags = (::Caelum::CaelumSystem::CaelumComponent)(
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_SKY_DOME |
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_MOON |
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_SUN |
			//::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_IMAGE_STARFIELD |
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_POINT_STARFIELD |
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_PRECIPITATION | 
			//::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_SCREEN_SPACE_FOG |
			//::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_GROUND_FOG |
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_CLOUDS);

		// init caelumsystem
		caelumSystem = new ::Caelum::CaelumSystem(
			OgreClient::Singleton->Root, 
			SceneManager, 			
			flags);

		// don't manage ambientlight
		caelumSystem->setManageAmbientLight(false);
		
		// no fog
		caelumSystem->setManageSceneFog(::Ogre::FogMode::FOG_NONE);
		//CaelumSystem->setSceneFogDensityMultiplier(0.0f);
		
		// attach viewport
		caelumSystem->attachViewport(OgreClient::Singleton->Viewport);
		//CaelumSystem->attachViewport(OgreClient::Singleton->ViewportInvis);
		
		// hookup listeners
		//OgreClient::Singleton->Root->addFrameListener(CaelumSystem);
		//OgreClient::Singleton->RenderWindow->addListener(CaelumSystem);

		/**************************** 2. TIME/DAY DURATION *******************************************/
		
		const int YEAR = 1;
		const int MONTH = 5;
		const int DAY = 1;

		// get caelum clock instance and current m59 time
		::Caelum::UniversalClock* clock = caelumSystem->getUniversalClock();
		::System::DateTime time			= MeridianDate::GetMeridianTime();
		
		// set caelum day duration to m59 day duration
		clock->setTimeScale(MeridianDate::M59SECONDSPERSECOND);		
			
		// set caelum time to m59 time using dummy date
		clock->setGregorianDateTime(YEAR, MONTH, DAY, time.Hour, time.Minute, time.Second);

		/**************************** 3. CLOUDS ******************************************************/
		
		::Caelum::CloudSystem* clouds = caelumSystem->getCloudSystem();

		if (clouds)
		{
			clouds->createLayerAtHeight(2000.0f);
			clouds->createLayerAtHeight(5000.0f);
			clouds->getLayer(0)->setCloudSpeed(Ogre::Vector2(0.00010f, -0.00018f));
			clouds->getLayer(1)->setCloudSpeed(Ogre::Vector2(0.00009f, -0.00017f));		
		}			
	};
	
	void ControllerRoom::InitParticleSystems()
	{
		// don't init twice or if disabled
		if (IsInitialized || OgreClient::Singleton->Config->DisableWeatherEffects)
			return;
		
		customParticleHandlers = new ::std::vector<::ParticleUniverse::ParticleEventHandler*>();

		::ParticleUniverse::ParticleSystemManager* particleMan =
			::ParticleUniverse::ParticleSystemManager::getSingletonPtr();

		// create room based particle systems
		particleSysSnow = particleMan->createParticleSystem(
			PARTICLES_SNOW_NAME, PARTICLES_SNOW_TEMPLATE, SceneManager);
  
		// setup particle system: snow
		if (particleSysSnow->getNumTechniques() > 0)
		{
			::ParticleUniverse::ParticleTechnique* technique =
				particleSysSnow->getTechnique(0);

			if (technique->getNumObservers() > 0)
			{
				::ParticleUniverse::OnPositionObserver* observer = (::ParticleUniverse::OnPositionObserver*)
					technique->getObserver(0);

				if (observer)
				{	
					// create custom handler for OnPosition
					// this will track the position and adjust
					WeatherParticleEventHandler* posHandler = 
						new WeatherParticleEventHandler();
					
					observer->addEventHandler(
						(::ParticleUniverse::ParticleEventHandler*)posHandler);

					// save reference for cleanup
					customParticleHandlers->push_back(
						(::ParticleUniverse::ParticleEventHandler*)posHandler);
				}
			}
		
			// set particles count from config
			technique->setVisualParticleQuota(OgreClient::Singleton->Config->WeatherParticles);
					
			// adjust emission rate to 1/10 of max quota
			if (technique->getNumEmitters() > 0)
			{
				::ParticleUniverse::DynamicAttributeFixed* val = (::ParticleUniverse::DynamicAttributeFixed*)
					technique->getEmitter(0)->getDynEmissionRate();

				val->setValue((::ParticleUniverse::Real)(OgreClient::Singleton->Config->WeatherParticles / 10));
			}
		}
	};

	void ControllerRoom::DestroyParticleSystems()
	{
		if (!IsInitialized)
			return;

		::ParticleUniverse::ParticleSystemManager* particleMan =
			::ParticleUniverse::ParticleSystemManager::getSingletonPtr();

		if (particleSysSnow)
			particleMan->destroyParticleSystem(particleSysSnow, SceneManager);

		if (customParticleHandlers)
		{
			// free custom event handler allocations
			for(size_t i = 0; i < customParticleHandlers->size(); i++)		
				delete customParticleHandlers->at(i);
			
			// clear custom particle handler list
			customParticleHandlers->clear();

			delete customParticleHandlers;
		}

		particleSysSnow = nullptr;
	};

	void ControllerRoom::DestroyCaelum()
	{
		if (!caelumSystem)
			return;
		
		caelumSystem->detachViewport(OgreClient::Singleton->Viewport);
				
		//OgreClient::Singleton->Root->removeFrameListener(CaelumSystem);
		//OgreClient::Singleton->RenderWindow->removeListener(CaelumSystem);

		caelumSystem->shutdown(true);
		caelumSystem = NULL;
		
	};

	void ControllerRoom::Destroy()
	{
		if (!IsInitialized)
			return;
		
		UnloadRoom();		
		DestroyCaelum();
		DestroyParticleSystems();

		/******************************************************************/

		// remove listener from projectiles
        OgreClient::Singleton->Data->Projectiles->ListChanged -= 
			gcnew ListChangedEventHandler(&ControllerRoom::OnProjectilesListChanged);
		
		// remove listener from roomobjects
        OgreClient::Singleton->Data->RoomObjects->ListChanged -= 
			gcnew ListChangedEventHandler(&ControllerRoom::OnRoomObjectsListChanged);
	
		// remove listener
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerRoom::OnDataPropertyChanged);

		// remove effects listeners
		OgreClient::Singleton->Data->Effects->Snowing->PropertyChanged -=
			gcnew PropertyChangedEventHandler(&ControllerRoom::OnEffectSnowingPropertyChanged);

		/******************************************************************/

		if (SceneManager->hasSceneNode(NAME_ROOMNODE))
			SceneManager->destroySceneNode(NAME_ROOMNODE);

		if (SceneManager->hasStaticGeometry(NAME_ROOMGEOMETRY))
			SceneManager->destroyStaticGeometry(NAME_ROOMGEOMETRY);

		/******************************************************************/

		delete grassMaterials;			
		delete decoration;
		
		/******************************************************************/

		roomGeometry		= nullptr;
		roomNode			= nullptr;
		decoration			= nullptr;
		caelumSystem		= nullptr;
		grassMaterials		= nullptr;
		avatarObject		= nullptr;
		verticesProcessed	= 0;
		
		/******************************************************************/

		IsInitialized = false;
	};
	
	void ControllerRoom::LoadRoom()
	{
		MeshManager* meshMan = MeshManager::getSingletonPtr();
		std::list<MeshPtr> tempMeshs = std::list<MeshPtr>();
		std::list<Entity*> tempEntities = std::list<Entity*>();
		ManualObject* manObj;
		MeshPtr temp_mesh;
		Entity* temp_entity;
		SceneNode* child;

		/*********************************************************************************************/

		if (!Room)
		{
			// log
			Logger::Log(MODULENAME, LogType::Error,
				"Error: Map resource not attached to RoomInformation.");

			// don't go on
			return;
		}
		else
			Logger::Log(MODULENAME, LogType::Info,
				"Loading map: " + Room->Filename);

		// attach handler for texture change on walls
		Room->WallTextureChanged += gcnew WallTextureChangedEventHandler(OnRooFileWallTextureChanged);
		Room->SectorTextureChanged += gcnew SectorTextureChangedEventHandler(OnRooFileSectorTextureChanged);
		Room->SectorMoved += gcnew SectorMovedEventHandler(OnRooFileSectorMoved);

		/*********************************************************************************************/

		// adjust octree
		AdjustOctree();

		// adjust ambient light       
		AdjustAmbientLight();

		// set sky
		UpdateSky();
			
		/*********************************************************************************************/

		// create all sides
		for each(RooSideDef^ side in Room->SideDefs)
			CreateSide(side);	

		// create all sectors
		for each (RooSector^ sector in Room->Sectors)
            CreateSector(sector);

		/*********************************************************************************************/

		// add decoration
		for (std::vector<ManualObject*>::iterator it = decoration->begin(); it != decoration->end(); it++)
		{
			manObj = (*it);

			if (manObj->getNumSections() > 0)
			{
				// convert to mesh
				temp_mesh = manObj->convertToMesh(manObj->getName() + "/TempMesh");
				temp_entity = SceneManager->createEntity(temp_mesh);

				// add to geometry
				roomGeometry->addEntity(temp_entity, ::Ogre::Vector3(64, 0, 64));

				tempMeshs.push_back(temp_mesh);
				tempEntities.push_back(temp_entity);
			}
		}

		// build geometry
        roomGeometry->build();

		// clean temporary
		for (std::list<Entity*>::iterator it = tempEntities.begin(); it != tempEntities.end(); it++)		
			SceneManager->destroyEntity(*it);                

		// clean temporary
		for (std::list<MeshPtr>::iterator it = tempMeshs.begin(); it != tempMeshs.end(); it++)
		{
			temp_mesh = (*it);
			temp_mesh->unload();
			meshMan->remove((ResourcePtr)temp_mesh);                               
		}
    };

    void ControllerRoom::UnloadRoom()
    {	
		ManualObject* manObj;
		
		// stop all particle systems
		if (particleSysSnow)
		{
			particleSysSnow->stop();

			if (particleSysSnow->isAttached())
				particleSysSnow->detachFromParent();
		}

		// childnodes/room elements
		if (roomNode)
			roomNode->removeAllChildren();

        // static geometry
		if (roomGeometry)
			roomGeometry->reset();
        
		if (Room)
		{
			// detach listeners
			Room->WallTextureChanged -= gcnew WallTextureChangedEventHandler(OnRooFileWallTextureChanged);
			Room->SectorTextureChanged -= gcnew SectorTextureChangedEventHandler(OnRooFileSectorTextureChanged);
			Room->SectorMoved -= gcnew SectorMovedEventHandler(OnRooFileSectorMoved);

			// destroy sides
			for each(RooSideDef^ side in Room->SideDefs)
			{
				if (side->UserData)
				{
					manObj = (ManualObject*)((::System::IntPtr)side->UserData).ToPointer();

					if (manObj)
						OGRE_DELETE manObj;

					side->UserData = nullptr;
					side->UserData2 = nullptr;
					side->UserDataLower = nullptr;
					side->UserDataMiddle = nullptr;
					side->UserDataUpper = nullptr;
				}
			}

			// destroy sectors
			for each(RooSector^ sector in Room->Sectors)
			{
				if (sector->UserData)
				{
					manObj = (ManualObject*)((::System::IntPtr)sector->UserData).ToPointer();

					if (manObj)
						OGRE_DELETE manObj;

					sector->UserData = nullptr;
					sector->UserData2 = nullptr;
					sector->UserDataCeiling = nullptr;
					sector->UserDataFloor = nullptr;
				}
			}
		}
			
        // destroy decoration
		for (std::vector<ManualObject*>::iterator it = decoration->begin(); it != decoration->end(); it++)
		{
			manObj = (*it);

			OGRE_DELETE manObj;
		}
				
		// clear lists
		decoration->clear();
    };

	void ControllerRoom::Tick(long long Tick, long long Span)
	{		
		if (!IsInitialized)
			return;

		if (caelumSystem && OgreClient::Singleton->Camera)
		{
			caelumSystem->updateSubcomponents((CLRReal)Span * 0.001f);
			caelumSystem->getMoon()->setPhase(0.0f); // overwrite moon				
			caelumSystem->notifyCameraChanged(OgreClient::Singleton->Camera);
		}
	};

	void ControllerRoom::CreateSide(RooSideDef^ Side)
	{
		SceneNode* scenenode;
		ManualObject* target;
		ManualObject::ManualObjectSection* section;
		
		/******************************************************************************/

		// use existing ManualObject
		if (Side->UserData)
		{
			target = (ManualObject*)((::System::IntPtr)Side->UserData).ToPointer();

			if (!target)
				return;

			target->clear();
		}
		else
		{
			// build name
			::Ogre::String ostr_sidename =
				PREFIX_ROOM_SIDE + ::Ogre::StringConverter::toString(Side->Num);

			// create a manualobject for the side
			target = OGRE_NEW ManualObject(ostr_sidename);
			
			// save reference
			Side->UserData = (::System::IntPtr)target;
		}

		/******************************************************************************/

		::Ogre::String ostr_upper  = StringConvert::CLRToOgre(Side->MaterialNameUpper);
		::Ogre::String ostr_middle = StringConvert::CLRToOgre(Side->MaterialNameMiddle);
		::Ogre::String ostr_lower  = StringConvert::CLRToOgre(Side->MaterialNameLower);

		// note: all sideparts using the same material will be put into the same
		// ManualObject section (=Vertexbuffer+Indexbuffer), to keep batchcount low

		// CASE 1: All 3 sideparts use same material
		if (ostr_upper == ostr_middle &&
			ostr_upper == ostr_lower &&
			ostr_upper != STRINGEMPTY)
		{
			verticesProcessed = 0;
			target->begin(ostr_upper, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
			CreateSidePart(target, Side, WallPartType::Upper);
			CreateSidePart(target, Side, WallPartType::Middle);
			CreateSidePart(target, Side, WallPartType::Lower);
			section = target->end();
			Side->UserDataUpper = (::System::IntPtr)section;
			Side->UserDataMiddle = (::System::IntPtr)section;
			Side->UserDataLower = (::System::IntPtr)section;
		}

		// CASE 2: All different materials
		else if (ostr_upper != ostr_middle &&
			ostr_upper != ostr_lower &&
			ostr_middle != ostr_lower)
		{
			if (ostr_upper != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_upper, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Upper);
				section = target->end();
				Side->UserDataUpper = (::System::IntPtr)section;
			}

			if (ostr_middle != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_middle, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Middle);
				section = target->end();
				Side->UserDataMiddle = (::System::IntPtr)section;
			}

			if (ostr_lower != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_lower, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Lower);
				section = target->end();
				Side->UserDataLower = (::System::IntPtr)section;
			}
		}

		// CASE 3: Two have the same material, third is different
		else if (ostr_upper == ostr_middle)
		{
			if (ostr_upper != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_upper, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Upper);			
				CreateSidePart(target, Side, WallPartType::Middle);
				section = target->end();
				Side->UserDataUpper = (::System::IntPtr)section;
				Side->UserDataMiddle = (::System::IntPtr)section;
			}

			if (ostr_lower != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_lower, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Lower);
				section = target->end();
				Side->UserDataLower = (::System::IntPtr)section;
			}
		}
		else if (ostr_upper == ostr_lower)
		{
			if (ostr_upper != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_upper, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Upper);
				CreateSidePart(target, Side, WallPartType::Lower);
				section = target->end();
				Side->UserDataUpper = (::System::IntPtr)section;
				Side->UserDataLower = (::System::IntPtr)section;
			}

			if (ostr_middle != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_middle, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Middle);
				section = target->end();
				Side->UserDataMiddle = (::System::IntPtr)section;
			}
		}
		else if (ostr_middle == ostr_lower)
		{
			if (ostr_upper != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_upper, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Upper);
				section = target->end();
				Side->UserDataUpper = (::System::IntPtr)section;		
			}

			if (ostr_middle != STRINGEMPTY)
			{
				verticesProcessed = 0;
				target->begin(ostr_middle, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
				CreateSidePart(target, Side, WallPartType::Middle);			
				CreateSidePart(target, Side, WallPartType::Lower);
				section = target->end();
				Side->UserDataMiddle = (::System::IntPtr)section;
				Side->UserDataLower = (::System::IntPtr)section;
			}
		}

		/******************************************************************************/
		
		// use existing scenenode
		if (Side->UserData2)
		{
			scenenode = (SceneNode*)((::System::IntPtr)Side->UserData2).ToPointer();
		}
		else
		{
			// add it to the scene
			if (target->getNumSections() > 0)
			{
				scenenode = roomNode->createChildSceneNode();
				scenenode->attachObject(target);
				
				Side->UserData2 = (::System::IntPtr)scenenode;

			#ifdef _DEBUG
				scenenode->showBoundingBox(true);
			#endif
			}
		}
	};

	void ControllerRoom::CreateSidePart(ManualObject* Target, RooSideDef^ Side, WallPartType PartType)
    {
		BgfFile^ textureFile		= nullptr;
		BgfBitmap^ texture			= nullptr;
		V2 sp						= V2::ZERO;
		::System::String^ texname	= nullptr;
		::System::String^ material	= nullptr;
		
		/******************************************************************************/

		// select texturefile based on wallpart
		switch (PartType)
		{
		case WallPartType::Upper:
			textureFile = Side->ResourceUpper;
			texture		= Side->TextureUpper;
			texname		= Side->TextureNameUpper;
			sp			= Side->SpeedUpper;
			material	= Side->MaterialNameUpper;
			break;

		case WallPartType::Middle:
			textureFile = Side->ResourceMiddle;
			texture		= Side->TextureMiddle;
			texname		= Side->TextureNameMiddle;
			sp			= Side->SpeedMiddle;
			material	= Side->MaterialNameMiddle;
			break;

		case WallPartType::Lower:
			textureFile = Side->ResourceLower;
			texture		= Side->TextureLower;
			texname		= Side->TextureNameLower;
			sp			= Side->SpeedLower;
			material	= Side->MaterialNameLower;
			break;
		}

		/******************************************************************************/

		// check
		if (!textureFile || !texture || !material || material == STRINGEMPTY)
			return;

		// possibly create texture & material
		CreateTextureAndMaterial(texture, texname, material, sp);

		/******************************************************************************/
		
		// add vertexdata from walls using this sidedef
		for each(RooWall^ wall in Room->Walls)
		{
			if (wall->LeftSide == Side)			
				CreateWallPart(Target, wall, PartType, true, texture->Width, texture->Height, textureFile->ShrinkFactor);
			
			if (wall->RightSide == Side)
				CreateWallPart(Target, wall, PartType, false, texture->Width, texture->Height, textureFile->ShrinkFactor);
		}
	};

	void ControllerRoom::CreateWallPart(
		ManualObject* Target, 
		RooWall^ Wall, 
		WallPartType PartType, 
		bool IsLeftSide, 
		int TextureWidth, 
		int TextureHeight, 
		int TextureShrink)
    {		
		// select side
		RooSideDef^ side = (IsLeftSide) ? Wall->LeftSide : Wall->RightSide;
		
		// may not have a side defined
		if (!side)
			return;

		// get vertexdata for this wallpart
		RooWall::RenderInfo^ RI = Wall->GetRenderInfo(
			PartType, 
			IsLeftSide,
			TextureWidth,
			TextureHeight,
			TextureShrink,
			SCALE);
			
		// P0
		Target->position(RI->P0.X, RI->P0.Z, RI->P0.Y);
		Target->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		Target->textureCoord(RI->UV0.Y, RI->UV0.X);

		// P1
		Target->position(RI->P1.X, RI->P1.Z, RI->P1.Y);
		Target->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		Target->textureCoord(RI->UV1.Y, RI->UV1.X);

		// P2
		Target->position(RI->P2.X, RI->P2.Z, RI->P2.Y);
		Target->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		Target->textureCoord(RI->UV2.Y, RI->UV2.X);

		// P3
		Target->position(RI->P3.X, RI->P3.Z, RI->P3.Y);
		Target->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		Target->textureCoord(RI->UV3.Y, RI->UV3.X);

		// create the rectangle by 2 triangles
		Target->triangle(verticesProcessed, verticesProcessed + 1, verticesProcessed + 2);
		Target->triangle(verticesProcessed, verticesProcessed + 2, verticesProcessed + 3);

		// increase counter
		verticesProcessed += 4;	
	};
	
	void ControllerRoom::CreateSector(RooSector^ Sector)
	{
		SceneNode* scenenode;
		ManualObject* target;
		ManualObject::ManualObjectSection* section;
		
		/******************************************************************************/

		// use existing ManualObject
		if (Sector->UserData)
		{
			target = (ManualObject*)((::System::IntPtr)Sector->UserData).ToPointer();

			if (!target)
				return;

			target->clear();
		}
		else
		{
			// build name
			::Ogre::String ostr_sectorname =
				PREFIX_ROOM_SECTOR + ::Ogre::StringConverter::toString(Sector->Num);

			// a manualobject for the sector
			target = OGRE_NEW ManualObject(ostr_sectorname);

			// attach reference to ManualObject
			Sector->UserData = (::System::IntPtr)target;
		}

		/******************************************************************************/
		
		::Ogre::String ostr_floor	= StringConvert::CLRToOgre(Sector->MaterialNameFloor);
		::Ogre::String ostr_ceiling = StringConvert::CLRToOgre(Sector->MaterialNameCeiling);

		// note: if floor and ceiling share the same material, they will be put into the same
		// ManualObject section (=VertexBuffer+IndexBuffer) - to keep batchcount low.

		// CASE 1: Floor and Ceiling have same material
		if (ostr_floor == ostr_ceiling &&
			ostr_floor != STRINGEMPTY)
		{
			verticesProcessed = 0;
			target->begin(ostr_floor, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
			CreateSectorPart(target, Sector, true);
			CreateSectorPart(target, Sector, false);
			section = target->end();
			Sector->UserDataFloor = (::System::IntPtr)section;
			Sector->UserDataCeiling = (::System::IntPtr)section;
		}

		// CASE 2: Different materials
		else if (ostr_floor != ostr_ceiling &&
			ostr_floor != STRINGEMPTY)
		{
			verticesProcessed = 0;
			target->begin(ostr_floor, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
			CreateSectorPart(target, Sector, true);
			section = target->end();
			Sector->UserDataFloor = (::System::IntPtr)section;

			verticesProcessed = 0;
			target->begin(ostr_ceiling, ::Ogre::RenderOperation::OT_TRIANGLE_LIST);
			CreateSectorPart(target, Sector, false);
			section = target->end();
			Sector->UserDataCeiling = (::System::IntPtr)section;
		}
        
		/******************************************************************************/

		// use existing scenenode
		if (Sector->UserData2)
		{
			scenenode = (SceneNode*)((::System::IntPtr)Sector->UserData2).ToPointer();
		}
		else
		{
			// add it to the scene
			if (target->getNumSections() > 0)
			{
				scenenode = roomNode->createChildSceneNode();
				scenenode->attachObject(target);

				Sector->UserData2 = (::System::IntPtr)scenenode;
			
			#ifdef _DEBUG
				scenenode->showBoundingBox(true);
			#endif
			}
		}
	};

	void ControllerRoom::CreateSectorPart(ManualObject* Target, RooSector^ Sector, bool IsFloor)
	{
		::System::String^ material		= nullptr;
		::System::String^ texname		= nullptr;
		V2 sp							= V2::ZERO;
		BgfFile^ textureFile			= nullptr;
		BgfBitmap^ texture				= nullptr;

		/******************************************************************************/

		// ceiling
		if (!IsFloor)
		{
			textureFile	= Sector->ResourceCeiling;
			texture		= Sector->TextureCeiling;
			texname		= Sector->TextureNameCeiling;
			sp			= Sector->SpeedCeiling;
			material	= Sector->MaterialNameCeiling;
		}

		// floor
		else
		{
			textureFile = Sector->ResourceFloor;
			texture		= Sector->TextureFloor;
			texname		= Sector->TextureNameFloor;
			sp			= Sector->SpeedFloor;
			material	= Sector->MaterialNameFloor;
		}

		/******************************************************************************/

		// check
		if (!textureFile || !texture || !material || material == STRINGEMPTY)
			return;

		// possibly create texture & material
		CreateTextureAndMaterial(texture, texname, material, sp);

		/******************************************************************************/
		
		// Add vertexdata of subsectors of this sector
		for each (RooSubSector^ subSector in Room->BSPTreeLeaves)
			if (subSector->Sector == Sector)
				CreateSubSector(Target, subSector, IsFloor);		
	};

	void ControllerRoom::CreateSubSector(ManualObject* Target, RooSubSector^ SubSector, bool IsFloor)
	{
		// get renderinfo for this subsector
        RooSubSector::RenderInfo^ RI = SubSector->GetRenderInfo(IsFloor, SCALE);

		// shortcuts
        array<V3>^ P = RI->P;
        array<V2>^ UV = RI->UV;

        // add vertices from renderinfo
        for (int i = 0; i < P->Length; i++)
        {
            Target->position(P[i].X, P[i].Z, P[i].Y);
            Target->textureCoord(UV[i].Y, UV[i].X);
            Target->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
        }

        // This is a simple triangulation algorithm for convex polygons (which subsectors guarantee to be)
        // It is: Connect the first vertex with any other vertex, except for it's direct neighbours
        int triangles = P->Length - 2;

        if (IsFloor)
        {
            // forward
            for (int j = 0; j < triangles; j++)
                Target->triangle(verticesProcessed + j + 2, verticesProcessed + j + 1, verticesProcessed);
        }
        else
        {
            // inverse
            for (int j = 0; j < triangles; j++)
                Target->triangle(verticesProcessed, verticesProcessed + j + 1, verticesProcessed + j + 2);
        }

		// create decoration objects on this subsector
		CreateDecoration(SubSector, RI, IsFloor);

        // save the vertices we processed, so we know where to start triangulation next time this is called
        verticesProcessed += P->Length;
	};
	
	void ControllerRoom::CreateDecoration(RooSubSector^ SubSector, RooSubSector::RenderInfo^ RI, bool IsFloor)
	{
		array<V3>^ P  = RI->P;
		int intensity = OgreClient::Singleton->Config->DecorationIntensity;
				
		// no decoration if disabled or a ceiling
		if (intensity <= 0 || !IsFloor || SubSector->Sector->SlopeInfoFloor)
			return;

		array<System::String^>^ items;

		// try to find a decoration definition for this floortexture from lookup dictionary
		if (grassMaterials->TryGetValue(SubSector->Sector->FloorTexture, items) && items->Length > 0)
		{
			// create grass geometry object
			::Ogre::String s = "Decoration_";
			ManualObject* grass = OGRE_NEW ManualObject(
				s.append(::Ogre::StringConverter::toString(decoration->size())));

			// process triangles of this subsector
			for (int i = 0; i < P->Length - 2; i++)
			{
				// 2D triangle of subsector for this iteration
				V2 A = V2(P[0].X, P[0].Y);
				V2 B = V2(P[i + 1].X, P[i + 1].Y);
				V2 C = V2(P[i + 2].X, P[i + 2].Y);

				// calc area
				float area = MathUtil::TriangleArea(A, B, C);

				// map to num of decoration with intensity
				int num = (int)(0.00002f * intensity * area) + 1;

				// create num random points in triangle
				for (int k = 0; k < num; k++)
				{
					// random point
					V2 rnd2D = MathUtil::RandomPointInTriangle(A, B, C);

					// make 3D by using height from first P
					::Ogre::Vector3 rnd3D = ::Ogre::Vector3(
						rnd2D.X, P[0].Z, rnd2D.Y);

					// pick random decoration from mapping
					int randomindex = ::System::Convert::ToInt32(
						MathUtil::Random->NextDouble() * (items->Length - 1));
					
					// decoration size (numplanes is quality)
					const float width = 10;
					const float height = 10;
					int numplanes = OgreClient::Singleton->Config->DecorationQuality;

					::Ogre::Vector3 vec(width / 2, 0, 0);
					::Ogre::Quaternion rot;

					// rotate by this for each grassplane
					rot.FromAngleAxis(
						::Ogre::Degree(180.0f / (float)numplanes), ::Ogre::Vector3::UNIT_Y);

					// begin grass object creation
					grass->begin(StringConvert::CLRToOgre(items[randomindex]), ::Ogre::RenderOperation::OT_TRIANGLE_LIST);

					for (int j = 0; j < numplanes; ++j)
					{
						grass->position(rnd3D.x - vec.x, P[0].Z + height, rnd3D.z - vec.z);
						grass->textureCoord(0, 0);

						grass->position(rnd3D.x + vec.x, P[0].Z + height, rnd3D.z + vec.z);
						grass->textureCoord(1, 0);

						grass->position(rnd3D.x - vec.x, P[0].Z, rnd3D.z - vec.z);
						grass->textureCoord(0, 1);

						grass->position(rnd3D.x + vec.x, P[0].Z, rnd3D.z + vec.z);
						grass->textureCoord(1, 1);

						int offset = j * 4;

						// front side
						grass->triangle(offset, offset + 3, offset + 1);
						grass->triangle(offset, offset + 2, offset + 3);

						// back side
						grass->triangle(offset + 1, offset + 3, offset);
						grass->triangle(offset + 3, offset + 2, offset);

						// rotate grassplane for next iteration
						vec = rot * vec;
					}

					grass->end();

				}
			}

			// add to list
			decoration->push_back(grass);
		}
		
	};

	void ControllerRoom::CreateTextureAndMaterial(BgfBitmap^ Texture, ::System::String^ TextureName, ::System::String^ MaterialName, V2 ScrollSpeed)
	{
		if (!Texture || !TextureName || !MaterialName || TextureName == STRINGEMPTY || MaterialName == STRINGEMPTY)
			return;

		::Ogre::String ostr_texname = StringConvert::CLRToOgre(TextureName);
		::Ogre::String ostr_matname = StringConvert::CLRToOgre(MaterialName);

		// possibly create texture
        Util::CreateTextureA8R8G8B8(Texture, ostr_texname, TEXTUREGROUP_ROOLOADER);
        
		// scrolling texture data
        Vector2* scrollSpeed = nullptr;

		//if (TextureInfo->ScrollSpeed != nullptr)
		scrollSpeed = &Util::ToOgre(ScrollSpeed);

		// possibly create material			
		Util::CreateMaterial(
			ostr_matname, ostr_texname, 
			MATERIALGROUP_ROOLOADER,
			scrollSpeed, nullptr, 1.0f);
	};

	void ControllerRoom::OnRooFileWallTextureChanged(System::Object^ sender, WallTextureChangedEventArgs^ e)
	{	
		if (!e || !e->ChangedSide)
			return;
		
		/******************************************************************************/

		RooSideDef^ side			= e->ChangedSide;
		::System::String^ material	= nullptr;
		::System::String^ texname	= nullptr;
		::System::Object^ userdata	= nullptr;
		BgfBitmap^ texture			= nullptr;
		V2 scrollspeed				= V2::ZERO;
		::Ogre::ManualObject* manObj = nullptr;
		::Ogre::ManualObject::ManualObjectSection* manSect = nullptr;

		/******************************************************************************/

		void* ptr1 = side->UserDataUpper ? ((::System::IntPtr)side->UserDataUpper).ToPointer() : nullptr;
		void* ptr2 = side->UserDataMiddle ? ((::System::IntPtr)side->UserDataMiddle).ToPointer() : nullptr;
		void* ptr3 = side->UserDataLower ? ((::System::IntPtr)side->UserDataLower).ToPointer() : nullptr;

		// this is a merged side and must be recreated due to possible split up
		if ((ptr1 && ptr1 == ptr2) ||
			(ptr1 && ptr1 == ptr3) ||
			(ptr2 && ptr2 == ptr3))
		{
			CreateSide(e->ChangedSide);
		}
		else
		{
			switch (e->WallPartType)
			{
			case WallPartType::Upper:
				userdata = e->ChangedSide->UserDataUpper;
				texture = e->ChangedSide->TextureUpper;
				scrollspeed = e->ChangedSide->SpeedUpper;
				texname = e->ChangedSide->TextureNameUpper;
				material = e->ChangedSide->MaterialNameUpper;
				break;

			case WallPartType::Middle:
				userdata = e->ChangedSide->UserDataMiddle;
				texture = e->ChangedSide->TextureMiddle;
				scrollspeed = e->ChangedSide->SpeedMiddle;
				texname = e->ChangedSide->TextureNameMiddle;
				material = e->ChangedSide->MaterialNameMiddle;
				break;

			case WallPartType::Lower:
				userdata = e->ChangedSide->UserDataLower;
				texture = e->ChangedSide->TextureLower;
				scrollspeed = e->ChangedSide->SpeedLower;
				texname = e->ChangedSide->TextureNameLower;
				material = e->ChangedSide->MaterialNameLower;
				break;
			}

			/******************************************************************************/

			// must have userdata
			if (!userdata)
				return;

			// try get native instance from it
			manSect = (::Ogre::ManualObject::ManualObjectSection*)((::System::IntPtr)userdata).ToPointer();

			if (!manSect)
				return;

			// possibly create textures and materials for new side
			CreateTextureAndMaterial(texture, texname, material, scrollspeed);

			// turn empty material strings into transparent materialname
			// because we've already created the side and not destroy it, just "hide"
			material = (!material || material == STRINGEMPTY) ? TRANSPARENTMATERIAL : material;

			// set to new material
			manSect->setMaterialName(StringConvert::CLRToOgre(material));
		}
	};

	void ControllerRoom::OnRooFileSectorTextureChanged(System::Object^ sender, SectorTextureChangedEventArgs^ e)
	{			
		if (!e || !e->ChangedSector)
			return;

		/******************************************************************************/

		RooSector^ sector			= e->ChangedSector;
		::System::String^ material	= nullptr;
		::System::String^ texname	= nullptr;
		::System::Object^ userdata	= nullptr;
		BgfBitmap^ texture			= nullptr;
		V2 scrollspeed				= V2::ZERO;
		::Ogre::ManualObject::ManualObjectSection* manSect = nullptr;

		/******************************************************************************/

		void* ptr1 = sector->UserDataFloor ? ((::System::IntPtr)sector->UserDataFloor).ToPointer() : nullptr;
		void* ptr2 = sector->UserDataCeiling ? ((::System::IntPtr)sector->UserDataCeiling).ToPointer() : nullptr;
		
		// this is a merged sector and must be recreated due to possible split up
		if (ptr1 && ptr1 == ptr2)
		{
			CreateSector(sector);
		}
		else
		{
			// floor
			if (e->IsFloor && e->ChangedSector->UserDataFloor)
			{
				userdata	= e->ChangedSector->UserDataFloor;
				texture		= e->ChangedSector->TextureFloor;
				scrollspeed = e->ChangedSector->SpeedFloor;
				texname		= e->ChangedSector->TextureNameFloor;
				material	= e->ChangedSector->MaterialNameFloor;
			}

			// ceiling
			else if (!e->IsFloor && e->ChangedSector->UserDataCeiling)
			{
				userdata	= e->ChangedSector->UserDataCeiling;
				texture		= e->ChangedSector->TextureCeiling;
				scrollspeed = e->ChangedSector->SpeedCeiling;
				texname		= e->ChangedSector->TextureNameCeiling;
				material	= e->ChangedSector->MaterialNameCeiling;
			}

			/******************************************************************************/

			// must have userdata
			if (!userdata)
				return;

			// try get native instance from it
			manSect = (::Ogre::ManualObject::ManualObjectSection*)((::System::IntPtr)userdata).ToPointer();

			if (!manSect)
				return;

			// possibly create textures and materials
			CreateTextureAndMaterial(texture, texname, material, scrollspeed);

			// turn empty material strings into transparent materialname
			material = (!material || material == STRINGEMPTY) ? TRANSPARENTMATERIAL : material;

			// set to new material
			manSect->setMaterialName(StringConvert::CLRToOgre(material));
		}
	};

	void ControllerRoom::OnRooFileSectorMoved(System::Object^ sender, SectorMovedEventArgs^ e)
	{
		if (!e || !e->SectorMove)
			return;

		/******************************************************************************/

		// recreate sector with changed vertexdata
		if (e->SectorMove->Sector)
			CreateSector(e->SectorMove->Sector);

		// recreate affected sides with changed vertexdata
		for each(RooSideDef^ side in e->SectorMove->Sides)
		{
			if (side)
				CreateSide(side);
		}	
	};
	
	void ControllerRoom::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
    {
		if (!IsInitialized)
			return;

		if (System::String::Equals(e->PropertyName, DataController::PROPNAME_VIEWERPOSITION))
		{
			// move particle systems with viewer position
			if (particleSysSnow &&
				OgreClient::Singleton->Data->Effects->Snowing &&
				!OgreClient::Singleton->Config->DisableWeatherEffects &&
				particleSysSnow->getNumTechniques() > 0)
			{		
				// get technique
				::ParticleUniverse::ParticleTechnique* technique = 
					particleSysSnow->getTechnique(0);

				// get new camera position
				::Ogre::Vector3 newPos = Util::ToOgre(
					OgreClient::Singleton->Data->ViewerPosition);

				// squared distance to current particle sys location
				::Ogre::Real dist2 = (technique->position - newPos).squaredLength();

				// set particlesystem position above camera
				technique->position = newPos;
				technique->position.y += PARTICLESYSCAMERAOFFSET;
				technique->latestPosition = technique->position;
				
				// start it if it's not yet started
				if (particleSysSnow->getState() == ::ParticleUniverse::ParticleSystem::ParticleSystemState::PSS_STOPPED)
					particleSysSnow->start();

				// if the new position is far away from the last
				// do a fast forward to create particles
				if (dist2 > 100000.0f)
				{
					particleSysSnow->setFastForward(5.0f, 1.0f);
					particleSysSnow->fastForward();
					particleSysSnow->setFastForward(0.0f, 1.0f);
				}
			}			
		}
	};

	void ControllerRoom::OnEffectSnowingPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (!IsInitialized || !particleSysSnow || !Room)
			return;

		if (System::String::Equals(e->PropertyName, EffectSnowing::PROPNAME_ISACTIVE) &&
			!OgreClient::Singleton->Config->DisableWeatherEffects)
		{
			// start or stop snow weather
			if (OgreClient::Singleton->Data->Effects->Snowing->IsActive)
			{													
				// possibly attach to roomnode
				if (!particleSysSnow->isAttached())
					SceneManager->getRootSceneNode()->attachObject(particleSysSnow);
				
				if (particleSysSnow->getNumTechniques() > 0)
				{			
					// get technique
					::ParticleUniverse::ParticleTechnique* technique = 
						particleSysSnow->getTechnique(0);
					
					// modify observer 0
					// setup observer threshold, start observing particles
					// once they entered the roomboundingbox height
					if (technique->getNumObservers() > 0)
					{
						::ParticleUniverse::OnPositionObserver* observer = (::ParticleUniverse::OnPositionObserver*)
							technique->getObserver(0);

						if (observer)
						{
							// get room bounding box
							::System::Tuple<::Meridian59::Common::V3, ::Meridian59::Common::V3>^ bBox = 
								Room->GetBoundingBox();

							// turn max into ogre world (scale, flip)
							::Ogre::Vector3 max = Util::ToOgreYZFlipped(bBox->Item2) * SCALE;

							// set threshold
							observer->setPositionYThreshold(max.y + 5.0f);		
						}
					}

					// set particle system to camera
					technique->position = Util::ToOgre(
						OgreClient::Singleton->Data->ViewerPosition);

					// but place it above...
					technique->position.y += PARTICLESYSCAMERAOFFSET;
					technique->latestPosition = technique->position;
				}
			}
			else
			{				
				particleSysSnow->stop();
				
				// possibly detach from parent
				if (particleSysSnow->isAttached())
					particleSysSnow->detachFromParent();
			}
		}
	};

	void ControllerRoom::UpdateSky()
	{
		if (!OgreClient::Singleton->Config->DisableNewSky)
		{
			// disable ogre skybox, caelum will be used
			SceneManager->setSkyBox(false, "");
		}
		else
		{
			System::String^ bgfFilename = OgreClient::Singleton->Data->RoomInformation->BackgroundFile;
		
			if (bgfFilename)
			{
				if (bgfFilename->Contains(SKY_DAY))
					SceneManager->setSkyBox(true, SKY_DAY_MAT);
                
				else if (bgfFilename->Contains(SKY_EVENING))
					SceneManager->setSkyBox(true, SKY_EVENING_MAT);
                
				else if (bgfFilename->Contains(SKY_MORNING))
					SceneManager->setSkyBox(true, SKY_MORNING_MAT);
               
				else if (bgfFilename->Contains(SKY_NIGHT))
					SceneManager->setSkyBox(true, SKY_NIGHT_MAT);
                
				else if (bgfFilename->Contains(SKY_FRENZY))
					SceneManager->setSkyBox(true, SKY_FRENZY_MAT);    
			}
		}	
	};

	void ControllerRoom::AdjustOctree()
	{
		// get room boundingbox
		System::Tuple<V3, V3>^ bbBox = Room->GetBoundingBox();

		// scaled and flipped ogre variants
		::Ogre::Vector3 min = Util::ToOgreYZFlipped(bbBox->Item1) * 0.0625f + ::Ogre::Vector3(64.0f, 0, 64.0f) + ::Ogre::Vector3(-1.0f, -1.0f, -1.0f);
		::Ogre::Vector3 max = Util::ToOgreYZFlipped(bbBox->Item2) * 0.0625f + ::Ogre::Vector3(64.0f, 0, 64.0f) + ::Ogre::Vector3(1.0f, 1.0f, 1.0f);
		::Ogre::Vector3 diff = max - min;
		
		// get biggest side
		float maxSide = System::Math::Max(diff.x, System::Math::Max(diff.y, diff.z));

		// the new maximum based on biggest side
		::Ogre::Vector3 newMax = ::Ogre::Vector3(min.x + maxSide, min.y + maxSide, min.z + maxSide);
		
		// adjust size of octree to an cube using max-side
		const AxisAlignedBox octreeBox = AxisAlignedBox(min, newMax);		
		SceneManager->setOption("Size", &octreeBox);
		
#ifdef _DEBUG
		const bool showOctree = true;
		SceneManager->setOption("ShowOctree", &showOctree);
#endif
	};

	void ControllerRoom::AdjustAmbientLight()
	{
		// simply use the maximum of avatarlight (nightvision..) and ambientlight.
		unsigned char ambient = OgreClient::Singleton->Data->RoomInformation->AmbientLight;
		unsigned char avatar = OgreClient::Singleton->Data->RoomInformation->AvatarLight;
		unsigned char max = System::Math::Max(ambient, avatar);

		// adjust ambientlight        
        SceneManager->setAmbientLight(Util::LightIntensityToOgreRGB(max));

		// log
        Logger::Log(MODULENAME, LogType::Info,
            "Setting AmbientLight to " + max.ToString());  
	};
	
	void ControllerRoom::ProjectileAdd(Projectile^ Projectile)
    {
        // log
        Logger::Log(MODULENAME, LogType::Info,
            "Adding 2D projectile " + Projectile->ID.ToString() + " to scene.");

        // create 2d projectile
        ProjectileNode2D^ newObject = gcnew ProjectileNode2D(Projectile, SceneManager);
        
		// attach a reference to the RemoteNode instance to the basic model
        Projectile->UserData = newObject;          
    };

	void ControllerRoom::ProjectileRemove(Projectile^ Projectile)
    {
        // log
		Logger::Log(MODULENAME, LogType::Info,
            "Removing projectile " + Projectile->ID.ToString() + " from scene.");

		// try to cast remotenode attached to userdata
		ProjectileNode2D^ engineObject = dynamic_cast<ProjectileNode2D^>(Projectile->UserData);

		// dispose
        if (engineObject)		
			delete engineObject;
		
		// remove reference
		Projectile->UserData = nullptr;
    };

	void ControllerRoom::RoomObjectAdd(RoomObject^ roomObject)
    {
        // remotenode we're creating
        RemoteNode^ newObject;

        // the name of the 3d model .xml if existant
        System::String^ mainOverlay		= roomObject->OverlayFile->Replace(FileExtensions::BGF, FileExtensions::XML);
		::Ogre::String ostr_mainOverlay = StringConvert::CLRToOgre(mainOverlay);

        // Check if there is a 3D model available
        if (ResourceGroupManager::getSingletonPtr()->resourceExists(RESOURCEGROUPMODELS, ostr_mainOverlay))
        {
            // log
			Logger::Log(MODULENAME, LogType::Info,
                "Adding 3D object " + roomObject->ID.ToString() + " (" + roomObject->Name + ") to scene.");

            // 3d model
            newObject = gcnew RemoteNode3D(roomObject, SceneManager);
        }
        else
        {
            // log
            Logger::Log(MODULENAME, LogType::Info,
                "Adding 2D object " + roomObject->ID.ToString() + " (" + roomObject->Name + ") to scene.");

            // legacy object
            newObject = gcnew RemoteNode2D(roomObject, SceneManager);
        }

        // attach a reference to the RemoteNode instance to the basic model
        roomObject->UserData = newObject;

        // check if this is our avatar we're controlling
        if (roomObject->IsAvatar)
        {
            // log
            Logger::Log(MODULENAME, LogType::Info,
				"New AvatarObject: " + roomObject->ID.ToString() + " (" + roomObject->Name + ")");

            // save a reference to the avatar object
            AvatarObject = newObject;

            float height = Util::GetSceneNodeHeight(AvatarObject->SceneNode);						

			// put camera on top of avatarnode
			OgreClient::Singleton->CameraNode->setPosition(
				0.0f, height * 0.93f, 0.0f);
                
            // Attach cameranode on avatarnode
            AvatarObject->SceneNode->addChild(OgreClient::Singleton->CameraNode);               
            AvatarObject->SceneNode->setFixedYawAxis(true);
                
            // set this node as sound listener
            ControllerSound::SetListenerNode(AvatarObject);

			// set initial visibility
			AvatarObject->SceneNode->setVisible(!ControllerInput::IsCameraFirstPerson);
        }
    };

	void ControllerRoom::RoomObjectRemove(RoomObject^ roomObject)
    {
        // log
        Logger::Log(MODULENAME, LogType::Info,
            "Removing object " + roomObject->ID.ToString() + " (" + roomObject->Name + ")" + " from scene.");

        // reset avatar reference in case it was removed
		if (roomObject->IsAvatar)
		{
			AvatarObject = nullptr;

			// unset listenernode
			ControllerSound::SetListenerNode(nullptr);
		}

		// try to cast remotenode attached to userdata
		RemoteNode^ engineObject = dynamic_cast<RemoteNode^>(roomObject->UserData);

		// dispose
        if (engineObject)		
			delete engineObject;
		
		// remove reference
		roomObject->UserData = nullptr;
    };

	void ControllerRoom::OnProjectilesListChanged(Object^ sender, ListChangedEventArgs^ e)
    {
        switch (e->ListChangedType)
        {
			case System::ComponentModel::ListChangedType::ItemAdded:
                ProjectileAdd(OgreClient::Singleton->Data->Projectiles[e->NewIndex]);
                break;

			case System::ComponentModel::ListChangedType::ItemDeleted:
                ProjectileRemove(OgreClient::Singleton->Data->Projectiles->LastDeletedItem);
                break;
        }
    };
	
	void ControllerRoom::OnRoomObjectsListChanged(Object^ sender, ListChangedEventArgs^ e)
    {
        switch (e->ListChangedType)
        {
			case System::ComponentModel::ListChangedType::ItemAdded:
					RoomObjectAdd(OgreClient::Singleton->Data->RoomObjects[e->NewIndex]);
					break;

			case System::ComponentModel::ListChangedType::ItemDeleted:
					RoomObjectRemove(OgreClient::Singleton->Data->RoomObjects->LastDeletedItem);
					break;
        }
    };

	void ControllerRoom::HandleGameModeMessage(GameModeMessage^ Message)
	{
		switch ((MessageTypeGameMode)Message->PI)
        {
			case MessageTypeGameMode::Player:
                HandlePlayerMessage((PlayerMessage^)Message);
                break;

            case MessageTypeGameMode::LightAmbient:
                HandleLightAmbient((LightAmbientMessage^)Message);
                break;

            case MessageTypeGameMode::LightPlayer:
                HandleLightPlayer((LightPlayerMessage^)Message);
                break;

            case MessageTypeGameMode::Background:
                HandleBackground((BackgroundMessage^)Message);
                break;

            default:
                break;
        }
	};

	void ControllerRoom::HandlePlayerMessage(PlayerMessage^ Message)
	{
		// unload the current scene
		UnloadRoom();

        // load new scene
        LoadRoom();
	};

	void ControllerRoom::HandleLightAmbient(LightAmbientMessage^ Message)
	{
		AdjustAmbientLight();  
	};

	void ControllerRoom::HandleLightPlayer(LightPlayerMessage^ Message)
	{
		AdjustAmbientLight();
	};

	void ControllerRoom::HandleBackground(BackgroundMessage^ Message)
	{
		UpdateSky();
	};

	void ControllerRoom::LoadImproveData()
	{
		//////////////////////// PATHS ////////////////////////////////////////////

		// build path to decoration resource path
        System::String^ path = Path::Combine(
			OgreClient::Singleton->Config->ResourcesPath, RESOURCEGROUPDECORATION);
        		
		/////////////////////// GRASS ///////////////////////////////////////////////
		
		// path to grass.xml
		System::String^ grasspath = Path::Combine(path, "grass/grass.xml");
        
		// dont go on if file missing
		if (!System::IO::File::Exists(grasspath))
		{
			// log
			Logger::Log(MODULENAME, LogType::Warning,
				"grass.xml decoration file missing");

			return;
		}

		// dictionary to store sets definition
		Dictionary<unsigned int, List<System::String^>^>^ grasssets = 
			gcnew Dictionary<unsigned int, List<System::String^>^>();
		
		// store parsed ids
		unsigned int texid = 0;
		unsigned int setid = 0;
		
		// temporary used
		List<System::String^>^ grassset; 

		// create reader
		XmlReader^ reader = XmlReader::Create(grasspath);

		// rootnode
        reader->ReadToFollowing("grass");

		// sets
		reader->ReadToFollowing("sets");

		// loop sets
		if (reader->ReadToDescendant("set"))
        {
            do
            {
				// valid id
				if (::System::UInt32::TryParse(reader["id"], setid))
				{
					// create material list
					grassset = gcnew List<System::String^>();

					// loop materials
					if (reader->ReadToDescendant("material"))
					{					
						do
						{
							// add material to set
							grassset->Add(reader["name"]);
						}
						while (reader->ReadToNextSibling("material"));
					}
				
					// add set to sets
					grasssets->Add(setid, grassset);
				}
            }
            while (reader->ReadToNextSibling("set"));
        }

		// mappings
		reader->ReadToFollowing("mappings");

		// loop mappings
		if (reader->ReadToDescendant("texture"))
		{					
			do
			{
				// try get texid and setid
				if (::System::UInt32::TryParse(reader["id"], texid) &&
					::System::UInt32::TryParse(reader["set"], setid))
				{
					if (grasssets->TryGetValue(setid, grassset))
					{
						grassMaterials->Add(texid, grassset->ToArray());
					}
				}
			}
			while (reader->ReadToNextSibling("texture"));
		}

		// finish read
        reader->Close();
	};
};};
