#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	static ControllerRoom::ControllerRoom()
	{
		roomDecoration			= nullptr;
		roomNode				= nullptr;
		roomManObj				= nullptr;
		grassMaterials			= nullptr;
		grassPoints				= nullptr;
		waterTextures			= nullptr;
		caelumSystem			= nullptr;
		avatarObject			= nullptr;
		particleSysSnow			= nullptr;
		customParticleHandlers	= nullptr;
		recreatequeue			= nullptr;
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
		grassPoints = gcnew ::System::Collections::Generic::Dictionary<::System::String^, ::System::Collections::Generic::List<V3>^>();
		waterTextures = gcnew ::System::Collections::Generic::List<::System::String^>();

		// create the queue storing materialnames (chunks of the room) which will be recreated
		// at the end of the tick
		recreatequeue = gcnew ::System::Collections::Generic::List<::System::String^>();

		// a manualobject for the room geometry
		roomManObj = OGRE_NEW ManualObject(NAME_ROOM);
		roomManObj->setDynamic(true);
		
		// a manualobject for the room decoration
		roomDecoration = OGRE_NEW ManualObject(NAME_ROOMDECORATION);
		roomDecoration->setDynamic(false);

		// create room scenenode
		roomNode = SceneManager->getRootSceneNode()->createChildSceneNode(NAME_ROOMNODE);
		roomNode->setPosition(::Ogre::Vector3(64.0f, 0, 64.0f));
		roomNode->attachObject(roomManObj);
		roomNode->attachObject(roomDecoration);
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
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_POINT_STARFIELD |
			::Caelum::CaelumSystem::CaelumComponent::CAELUM_COMPONENT_CLOUDS);

		// the ones we don't use 
		//CaelumComponent::CAELUM_COMPONENT_IMAGE_STARFIELD
		//CaelumComponent::CAELUM_COMPONENT_PRECIPITATION
		//CaelumComponent::CAELUM_COMPONENT_SCREEN_SPACE_FOG
		//CaelumComponent::CAELUM_COMPONENT_GROUND_FOG

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
		const int MONTH = 4;
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

		if (SceneManager->hasManualObject(NAME_ROOMDECORATION))
			SceneManager->destroyManualObject(NAME_ROOMDECORATION);

		if (SceneManager->hasManualObject(NAME_ROOM))
			SceneManager->destroyManualObject(NAME_ROOM);

		/******************************************************************/

		delete grassMaterials;			
		delete grassPoints;
		delete recreatequeue;
		delete waterTextures;

		/******************************************************************/

		roomDecoration		= nullptr;
		roomNode			= nullptr;
		roomManObj			= nullptr;
		caelumSystem		= nullptr;
		grassMaterials		= nullptr;
		grassPoints			= nullptr;
		waterTextures		= nullptr;
		avatarObject		= nullptr;
		recreatequeue		= nullptr;
		verticesProcessed	= 0;
		
		/******************************************************************/

		IsInitialized = false;
	};
	
	void ControllerRoom::LoadRoom()
	{
		double tick1, tick2, span;

		/*********************************************************************************************/

		// roomfile must be present
		if (!Room)
		{
			// log
			Logger::Log(MODULENAME, LogType::Error,
				"Error: Room (.roo) resource not attached to RoomInformation.");

			return;
		}
			
		/*********************************************************************************************/

		// attach handlers for changes in the room
		Room->WallTextureChanged	+= gcnew WallTextureChangedEventHandler(OnRooFileWallTextureChanged);
		Room->SectorTextureChanged	+= gcnew SectorTextureChangedEventHandler(OnRooFileSectorTextureChanged);
		Room->SectorMoved			+= gcnew SectorMovedEventHandler(OnRooFileSectorMoved);

		/*********************************************************************************************/

		// adjust octree
		AdjustOctree();

		// adjust ambient light       
		AdjustAmbientLight();

		// set sky
		UpdateSky();

		// get materialinfos
		::System::Collections::Generic::Dictionary<::System::String^, RooFile::MaterialInfo>^ dict =
			Room->GetMaterialInfos();
		
		Logger::Log(MODULENAME, LogType::Info, "Start loading room: " + Room->Filename + FileExtensions::ROO);

		/*********************************************************************************************/
		/*                              ROOM TEXTURES                                                */
		/*********************************************************************************************/

		tick1 = OgreClient::Singleton->GameTick->GetUpdatedTick();

		// create the materials & textures
		for each(KeyValuePair<::System::String^, RooFile::MaterialInfo> pair in dict)
		{
			// create texture & material
			CreateTextureAndMaterial(
				pair.Value.Texture,
				pair.Value.TextureName,
				pair.Value.MaterialName,
				pair.Value.ScrollSpeed);
		}

		tick2 = OgreClient::Singleton->GameTick->GetUpdatedTick();
		span = tick2 - tick1;
		
		Logger::Log(MODULENAME, LogType::Info, "Textures: " + span.ToString() + " ms");

		/*********************************************************************************************/
		/*                              ROOM GEOMETRY                                                */
		/*********************************************************************************************/

		tick1 = tick2;

		// create room geometry
		for each(KeyValuePair<::System::String^, RooFile::MaterialInfo> pair in dict)
			CreateGeometryChunk(pair.Value.MaterialName);
						
		tick2 = OgreClient::Singleton->GameTick->GetUpdatedTick();
		span = tick2 - tick1;

		Logger::Log(MODULENAME, LogType::Info, "Geometry: " + span.ToString() + " ms");

		/*********************************************************************************************/
		/*                               ROOM DECORATION                                             */
		/*********************************************************************************************/
		
		tick1 = tick2;

		// create room decoration
		CreateDecoration();

		tick2 = OgreClient::Singleton->GameTick->GetUpdatedTick();
		span = tick2 - tick1;

		Logger::Log(MODULENAME, LogType::Info, "Decoration: " + span.ToString() + " ms");

		/*********************************************************************************************/
		/*                                    OTHERS                                                 */
		/*********************************************************************************************/

    };

    void ControllerRoom::UnloadRoom()
    {	
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

        // clear room decoration
		if (roomDecoration)
			roomDecoration->clear();
        
		// clear room geometry
		if (roomManObj)
			roomManObj->clear();

		if (grassPoints)
			grassPoints->Clear();

		if (Room)
		{
			// detach listeners
			Room->WallTextureChanged	-= gcnew WallTextureChangedEventHandler(OnRooFileWallTextureChanged);
			Room->SectorTextureChanged	-= gcnew SectorTextureChangedEventHandler(OnRooFileSectorTextureChanged);
			Room->SectorMoved			-= gcnew SectorMovedEventHandler(OnRooFileSectorMoved);
		}
    };

	int ControllerRoom::GetRoomSectionByMaterial(::Ogre::String Name)
	{
		::Ogre::ManualObject::ManualObjectSection* section;

		if (!roomManObj || Name == STRINGEMPTY)
			return -1;

		for (size_t i = 0; i < roomManObj->getNumSections(); i++)
		{
			section = roomManObj->getSection(i);

			if (section->getMaterialName() == Name)
				return i;
		}

		return -1;
	};

	int ControllerRoom::GetDecorationSectionByMaterial(::Ogre::String Name)
	{
		::Ogre::ManualObject::ManualObjectSection* section;

		if (!roomDecoration || Name == STRINGEMPTY)
			return -1;

		for (size_t i = 0; i < roomDecoration->getNumSections(); i++)
		{
			section = roomDecoration->getSection(i);

			if (section->getMaterialName() == Name)
				return i;
		}

		return -1;
	};

	void ControllerRoom::CreateGeometryChunk(::System::String^ MaterialName)
	{
		::Ogre::String material = StringConvert::CLRToOgre(MaterialName);
		int sectionindex		= GetRoomSectionByMaterial(material);

		// create new geometry chunk (vertexbuffer+indexbuffer+...)
		// for this material or get existing one
		if (sectionindex > -1)		
			roomManObj->beginUpdate(sectionindex);
		
		else		
			roomManObj->begin(material);

		// reset vertex counter
		verticesProcessed = 0;

		// create all sector floors and ceilings using this material
		for each (RooSector^ sector in Room->Sectors)
		{
			if (sector->MaterialNameFloor == MaterialName)
				CreateSectorPart(sector, true);

			if (sector->MaterialNameCeiling == MaterialName)
				CreateSectorPart(sector, false);
		}

		// create all side parts using this material
		for each(RooSideDef^ side in Room->SideDefs)
		{
			if (side->MaterialNameLower == MaterialName)
				CreateSidePart(side, WallPartType::Lower);

			if (side->MaterialNameMiddle == MaterialName)
				CreateSidePart(side, WallPartType::Middle);

			if (side->MaterialNameUpper == MaterialName)
				CreateSidePart(side, WallPartType::Upper);
		}

		// finish this chunk
		roomManObj->end();
	}

	void ControllerRoom::Tick(double Tick, double Span)
	{		
		if (!IsInitialized)
			return;

		// process the queued subsections for recreation
		for each(::System::String^ s in recreatequeue)
			CreateGeometryChunk(s);

		// clear recreate queue
		recreatequeue->Clear();

		if (caelumSystem && OgreClient::Singleton->Camera)
		{
			caelumSystem->updateSubcomponents((CLRReal)Span * 0.001f);
			caelumSystem->getMoon()->setPhase(0.0f); // overwrite moon				
			caelumSystem->notifyCameraChanged(OgreClient::Singleton->Camera);
		}
	};

	void ControllerRoom::CreateSidePart(RooSideDef^ Side, WallPartType PartType)
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
				CreateWallPart(wall, PartType, true, texture->Width, texture->Height, textureFile->ShrinkFactor);
			
			if (wall->RightSide == Side)
				CreateWallPart(wall, PartType, false, texture->Width, texture->Height, textureFile->ShrinkFactor);
		}
	};

	void ControllerRoom::CreateWallPart(		
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
		RooWall::VertexData^ RI = Wall->GetVertexData(
			PartType, 
			IsLeftSide,
			TextureWidth,
			TextureHeight,
			TextureShrink,
			SCALE);
			
		// P0
		roomManObj->position(RI->P0.X, RI->P0.Z, RI->P0.Y);
		roomManObj->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		roomManObj->textureCoord(RI->UV0.Y, RI->UV0.X);

		// P1
		roomManObj->position(RI->P1.X, RI->P1.Z, RI->P1.Y);
		roomManObj->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		roomManObj->textureCoord(RI->UV1.Y, RI->UV1.X);

		// P2
		roomManObj->position(RI->P2.X, RI->P2.Z, RI->P2.Y);
		roomManObj->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		roomManObj->textureCoord(RI->UV2.Y, RI->UV2.X);

		// P3
		roomManObj->position(RI->P3.X, RI->P3.Z, RI->P3.Y);
		roomManObj->normal(RI->Normal.X, RI->Normal.Z, RI->Normal.Y);
		roomManObj->textureCoord(RI->UV3.Y, RI->UV3.X);

		// create the rectangle by 2 triangles
		roomManObj->triangle(verticesProcessed, verticesProcessed + 1, verticesProcessed + 2);
		roomManObj->triangle(verticesProcessed, verticesProcessed + 2, verticesProcessed + 3);

		// increase counter
		verticesProcessed += 4;	
	};

	void ControllerRoom::CreateSectorPart(RooSector^ Sector, bool IsFloor)
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
	
		// add vertexdata of subsectors
		for each (RooSubSector^ subSector in Room->BSPTreeLeaves)
			if (subSector->Sector == Sector)
				CreateSubSector(subSector, IsFloor);
	};

	void ControllerRoom::CreateSubSector(RooSubSector^ SubSector, bool IsFloor)
	{
		// update vertexdata for this subsector
        SubSector->UpdateVertexData(IsFloor, SCALE);
		
		// shortcuts to select basedon floor/ceiling
		array<V3>^ P;
		array<V2>^ UV;
		V3 Normal;

		if (IsFloor)
		{
			P = SubSector->FloorP;
			UV = SubSector->FloorUV;
			Normal = SubSector->FloorNormal;
		}
		else
		{
			P = SubSector->CeilingP;
			UV = SubSector->CeilingUV;
			Normal = SubSector->CeilingNormal;
		}

		// add vertices from vertexdata
        for (int i = 0; i < P->Length; i++)
        {
			roomManObj->position(P[i].X, P[i].Z, P[i].Y);
			roomManObj->textureCoord(UV[i].Y, UV[i].X);
			roomManObj->normal(Normal.X, Normal.Z, Normal.Y);
        }

        // This is a simple triangulation algorithm for convex polygons (which subsectors guarantee to be)
        // It is: Connect the first vertex with any other vertex, except for it's direct neighbours
        int triangles = P->Length - 2;

        if (IsFloor)
        {
            // forward
            for (int j = 0; j < triangles; j++)
				roomManObj->triangle(verticesProcessed + j + 2, verticesProcessed + j + 1, verticesProcessed);
        }
        else
        {
            // inverse
            for (int j = 0; j < triangles; j++)
				roomManObj->triangle(verticesProcessed, verticesProcessed + j + 1, verticesProcessed + j + 2);
        }

        // save the vertices we processed, so we know where to start triangulation next time this is called
        verticesProcessed += P->Length;
	};
	
	void ControllerRoom::CreateDecoration()
	{		
		const float WIDTH = 10.0f;
		const float HEIGHT = 10.0f;
		const float HALFWIDTH = WIDTH / 2.0f;

		int intensity = OgreClient::Singleton->Config->DecorationIntensity;
		int numplanes = 3;
		::Ogre::Vector3 vec(WIDTH / 2, 0, 0);
		::Ogre::Quaternion rot;

		array<::System::String^>^ items;
		V2 A, B, C, rnd2D;
		V3 rnd3D;

		float area;
		int num;
		int randomindex;
		int vertexindex;
		::System::Collections::Generic::List<V3>^ points;

		if (intensity <= 0)
			return;
		
		/**************************************************************************************/
		/*                     GENERATE RANDOM POINTS FOR GRASS MATERIALS                     */
		/**************************************************************************************/

		// loop all subsectors
		for each(RooSubSector^ subsect in Room->BSPTreeLeaves)
		{
			// try to find a decoration definition for this floortexture from lookup dictionary
			if (!grassMaterials->TryGetValue(subsect->Sector->FloorTexture, items))
				continue;

			// process triangles of this subsector
			for (int i = 0; i < subsect->Vertices->Count - 2; i++)
			{
				// pick a 2D triangle for this iteration
				// of subsector by using next 3 points of it
				A.X = (float)subsect->Vertices[0].X;
				A.Y = (float)subsect->Vertices[0].Y;
				B.X = (float)subsect->Vertices[i + 1].X;
				B.Y = (float)subsect->Vertices[i + 1].Y;
				C.X = (float)subsect->Vertices[i + 2].X;
				C.Y = (float)subsect->Vertices[i + 2].Y;

				// calc area
				area = MathUtil::TriangleArea(A, B, C);

				// create an amount of grass to create for this triangle
				// scaled by the area of the triangle and intensity
				num = (int)(0.0000001f * intensity * area) + 1;

				// create num random points in triangle
				for (int k = 0; k < num; k++)
				{
					// generate random 2D point in triangle
					rnd2D = MathUtil::RandomPointInTriangle(A, B, C);
						
					// retrieve height for random coordinates
					// also flip y/z and scale to server/newclient
					rnd3D.X = rnd2D.X;
					rnd3D.Y = subsect->Sector->CalculateFloorHeight(rnd2D.X, rnd2D.Y, false);
					rnd3D.Z = rnd2D.Y;
					rnd3D.Scale(GeometryConstants::CLIENTFINETOKODFINE);

					// pick random decoration from mapping
					randomindex = ::System::Convert::ToInt32(
						MathUtil::Random->NextDouble() * (items->Length - 1));

					// if this material does not yet have a section, create one
					if (!grassPoints->TryGetValue(items[randomindex], points))
					{
						points = gcnew ::System::Collections::Generic::List<V3>();
						grassPoints->Add(items[randomindex], points);
					}

					// add random point to according materiallist
					points->Add(rnd3D);
				}			
			}
		}

		/**************************************************************************************/
		/*                                 GENERATE GRASS                                     */
		/**************************************************************************************/

		// loop grass materials with their attached randompoints
		for each(KeyValuePair<::System::String^, ::System::Collections::Generic::List<V3>^> pair in grassPoints)
		{
			// create a new subsection for all grass using this material
			roomDecoration->begin(StringConvert::CLRToOgre(pair.Key), ::Ogre::RenderOperation::OT_TRIANGLE_LIST);

			// reset vertexcounter
			vertexindex = 0;

			// loop points
			for each(V3 p in pair.Value)
			{				
				// rotate by this for each grassplane
				rot.FromAngleAxis(
					::Ogre::Degree(180.0f / (float)numplanes), ::Ogre::Vector3::UNIT_Y);

				for (int j = 0; j < numplanes; ++j)
				{
					roomDecoration->position(p.X - vec.x, p.Y + HEIGHT, p.Z - vec.z);
					roomDecoration->textureCoord(0, 0);

					roomDecoration->position(p.X + vec.x, p.Y + HEIGHT, p.Z + vec.z);
					roomDecoration->textureCoord(1, 0);

					roomDecoration->position(p.X - vec.x, p.Y, p.Z - vec.z);
					roomDecoration->textureCoord(0, 1);

					roomDecoration->position(p.X + vec.x, p.Y, p.Z + vec.z);
					roomDecoration->textureCoord(1, 1);

					// front side
					roomDecoration->triangle(vertexindex, vertexindex + 3, vertexindex + 1);
					roomDecoration->triangle(vertexindex, vertexindex + 2, vertexindex + 3);

					// back side
					roomDecoration->triangle(vertexindex + 1, vertexindex + 3, vertexindex);
					roomDecoration->triangle(vertexindex + 3, vertexindex + 2, vertexindex);

					// rotate grassplane for next iteration
					vec = rot * vec;

					// increase vertexcounter
					vertexindex += 4;
				}
			}

			// finish this subsection
			roomDecoration->end();
		}
	};

	void ControllerRoom::CreateTextureAndMaterial(BgfBitmap^ Texture, ::System::String^ TextureName, ::System::String^ MaterialName, V2 ScrollSpeed)
	{
		if (!Texture || !TextureName || !MaterialName || TextureName == STRINGEMPTY || MaterialName == STRINGEMPTY)
			return;

		::Ogre::String ostr_texname = StringConvert::CLRToOgre(TextureName);
		::Ogre::String ostr_matname = StringConvert::CLRToOgre(MaterialName);
		
		// possibly create texture
        Util::CreateTextureA8R8G8B8(Texture, ostr_texname, TEXTUREGROUP_ROOLOADER, MIP_DEFAULT);
        
		// scrolling texture data
        Vector2* scrollSpeed = nullptr;

		//if (TextureInfo->ScrollSpeed != nullptr)
		scrollSpeed = &Util::ToOgre(ScrollSpeed);

		if (waterTextures->Contains(TextureName))
		{
			Util::CreateMaterialWater(
				ostr_matname, ostr_texname,
				MATERIALGROUP_ROOLOADER,
				scrollSpeed);
		}
		// possibly create material			
		else
			Util::CreateMaterial(
				ostr_matname, ostr_texname, 
				MATERIALGROUP_ROOLOADER,
				scrollSpeed, nullptr);
		
	};

	void ControllerRoom::OnRooFileWallTextureChanged(System::Object^ sender, WallTextureChangedEventArgs^ e)
	{	
		if (!e || !e->ChangedSide)
			return;
		
		/******************************************************************************/

		::System::String^ material	= nullptr;
		::System::String^ texname	= nullptr;
		BgfBitmap^ texture			= nullptr;
		V2 scrollspeed				= V2::ZERO;
		
		/******************************************************************************/

		switch (e->WallPartType)
		{
		case WallPartType::Upper:
			texture		= e->ChangedSide->TextureUpper;
			scrollspeed = e->ChangedSide->SpeedUpper;
			texname		= e->ChangedSide->TextureNameUpper;
			material	= e->ChangedSide->MaterialNameUpper;
			break;

		case WallPartType::Middle:
			texture		= e->ChangedSide->TextureMiddle;
			scrollspeed = e->ChangedSide->SpeedMiddle;
			texname		= e->ChangedSide->TextureNameMiddle;
			material	= e->ChangedSide->MaterialNameMiddle;
			break;

		case WallPartType::Lower:
			texture		= e->ChangedSide->TextureLower;
			scrollspeed = e->ChangedSide->SpeedLower;
			texname		= e->ChangedSide->TextureNameLower;
			material	= e->ChangedSide->MaterialNameLower;
			break;
		}

		// no materialchange? nothing to do
		if (e->OldMaterialName == material)
			return;

		// possibly create new texture and material
		CreateTextureAndMaterial(texture, texname, material, scrollspeed);

		// enqueue old material subsection for recreation
		if (!recreatequeue->Contains(e->OldMaterialName))
			recreatequeue->Add(e->OldMaterialName);

		// enqueue new material subsection for recreation
		if (!recreatequeue->Contains(material))
			recreatequeue->Add(material);
	};

	void ControllerRoom::OnRooFileSectorTextureChanged(System::Object^ sender, SectorTextureChangedEventArgs^ e)
	{			
		if (!e || !e->ChangedSector)
			return;

		/******************************************************************************/

		::System::String^ material	= nullptr;
		::System::String^ texname	= nullptr;
		BgfBitmap^ texture			= nullptr;
		V2 scrollspeed				= V2::ZERO;
		
		/******************************************************************************/

		// floor
		if (e->IsFloor)
		{
			texture		= e->ChangedSector->TextureFloor;
			scrollspeed = e->ChangedSector->SpeedFloor;
			texname		= e->ChangedSector->TextureNameFloor;
			material	= e->ChangedSector->MaterialNameFloor;
		}

		// ceiling
		else if (!e->IsFloor)
		{
			texture		= e->ChangedSector->TextureCeiling;
			scrollspeed = e->ChangedSector->SpeedCeiling;
			texname		= e->ChangedSector->TextureNameCeiling;
			material	= e->ChangedSector->MaterialNameCeiling;
		}

		// no materialchange? nothing to do
		if (e->OldMaterialName == material)
			return;

		// possibly create new texture and material
		CreateTextureAndMaterial(texture, texname, material, scrollspeed);
		
		// enqueue old material subsection for recreation
		if (!recreatequeue->Contains(e->OldMaterialName))
			recreatequeue->Add(e->OldMaterialName);

		// enqueue new material subsection for recreation
		if (!recreatequeue->Contains(material))
			recreatequeue->Add(material);
	};

	void ControllerRoom::OnRooFileSectorMoved(System::Object^ sender, SectorMovedEventArgs^ e)
	{
		if (!e || !e->Sector)
			return;

		/******************************************************************************/

		// possibly add floor material to recreation
		if (e->Sector->MaterialNameFloor &&
			e->Sector->MaterialNameFloor != STRINGEMPTY && 
			!recreatequeue->Contains(e->Sector->MaterialNameFloor))
		{
			recreatequeue->Add(e->Sector->MaterialNameFloor);
		}

		// possibly add ceiling material to recreation
		if (e->Sector->MaterialNameCeiling &&
			e->Sector->MaterialNameCeiling != STRINGEMPTY &&
			!recreatequeue->Contains(e->Sector->MaterialNameCeiling))
		{
			recreatequeue->Add(e->Sector->MaterialNameCeiling);
		}

		// possibly affected sides to recreation
		for each(RooSideDef^ side in e->Sector->Sides)
		{
			if (side->MaterialNameLower &&
				side->MaterialNameLower != STRINGEMPTY &&
				!recreatequeue->Contains(side->MaterialNameLower))
			{
				recreatequeue->Add(side->MaterialNameLower);
			}

			if (side->MaterialNameMiddle &&
				side->MaterialNameMiddle != STRINGEMPTY &&
				!recreatequeue->Contains(side->MaterialNameMiddle))
			{
				recreatequeue->Add(side->MaterialNameMiddle);
			}

			if (side->MaterialNameUpper &&
				side->MaterialNameUpper != STRINGEMPTY &&
				!recreatequeue->Contains(side->MaterialNameUpper))
			{
				recreatequeue->Add(side->MaterialNameUpper);
			}
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
							BoundingBox3D^ bBox = Room->GetBoundingBox3D(true);

							// turn max into ogre world (scale, flip)
							::Ogre::Vector3 max = Util::ToOgreYZFlipped(bBox->Max) * SCALE;

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
		BoundingBox3D^ bbBox = Room->GetBoundingBox3D(true);
		
		// scaled and flipped ogre variants
		::Ogre::Vector3 min = Util::ToOgreYZFlipped(bbBox->Min) * 0.0625f + ::Ogre::Vector3(64.0f, 0, 64.0f) + ::Ogre::Vector3(-1.0f, -1.0f, -1.0f);
		::Ogre::Vector3 max = Util::ToOgreYZFlipped(bbBox->Max) * 0.0625f + ::Ogre::Vector3(64.0f, 0, 64.0f) + ::Ogre::Vector3(1.0f, 1.0f, 1.0f);
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

		// update caelum heights
		if (caelumSystem)
		{
			::Caelum::CloudSystem* clouds = caelumSystem->getCloudSystem();

			if (clouds)
			{
				if (clouds->getLayerCount() > 0)
					clouds->getLayer(0)->setHeight(newMax.y + 2000.0f);

				if (clouds->getLayerCount() > 1)
					clouds->getLayer(1)->setHeight(newMax.y + 5000.0f);
			}
		}
	};

	void ControllerRoom::AdjustAmbientLight()
	{
		unsigned char ambient		= OgreClient::Singleton->Data->RoomInformation->AmbientLight;
		unsigned char avatar		= OgreClient::Singleton->Data->RoomInformation->AvatarLight;
		unsigned char directional	= OgreClient::Singleton->Data->LightShading->LightIntensity;
		
		// simply use the maximum of avatarlight (nightvision..) and ambientlight for ambientlight
		unsigned char max = System::Math::Max(ambient, avatar);

		// adjust ambientlight        
		SceneManager->setAmbientLight(Util::LightIntensityToOgreRGB(max));

		// log
		Logger::Log(MODULENAME, LogType::Info,
			"Setting AmbientLight to " + max.ToString());

		// directional sun of Caelum
		if (caelumSystem)
		{
			::Caelum::BaseSkyLight* sun  = caelumSystem->getSun();
			::Caelum::BaseSkyLight* moon = caelumSystem->getMoon();
			
			::Ogre::ColourValue color = ::Ogre::ColourValue(
				(float)directional * 0.1f,
				(float)directional * 0.1f,
				(float)directional * 0.1f);

			if (sun)
			{
				sun->setDiffuseMultiplier(color);
				sun->setSpecularMultiplier(color);
			}

			if (moon)
			{
				moon->setDiffuseMultiplier(color);
				moon->setSpecularMultiplier(color);
			}

			// log
			Logger::Log(MODULENAME, LogType::Info,
				"Setting DirectionalLight to " + directional.ToString());
		}
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
			//Logger::Log(MODULENAME, LogType::Info,
            //    "Adding 3D object " + roomObject->ID.ToString() + " (" + roomObject->Name + ") to scene.");

            // 3d model
            newObject = gcnew RemoteNode3D(roomObject, SceneManager);
        }
        else
        {
            // log
            //Logger::Log(MODULENAME, LogType::Info,
            //    "Adding 2D object " + roomObject->ID.ToString() + " (" + roomObject->Name + ") to scene.");

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
				"Found own avatar: " + roomObject->ID.ToString() + " (" + roomObject->Name + ")");

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

			// if we've hidden the avatar-scenenode due to 1.person above
			// make sure a light attached is still visible!
			if (AvatarObject->Light)
				AvatarObject->Light->setVisible(true);
        }
    };

	void ControllerRoom::RoomObjectRemove(RoomObject^ roomObject)
    {
        // log
        //Logger::Log(MODULENAME, LogType::Info,
        //    "Removing object " + roomObject->ID.ToString() + " (" + roomObject->Name + ")" + " from scene.");

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

			case MessageTypeGameMode::LightShading:
				HandleLightShading((LightShadingMessage^)Message);
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

	void ControllerRoom::HandleLightShading(LightShadingMessage^ Message)
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

		/////////////////////// WATER ///////////////////////////////////////////////

		// path to water.xml
		System::String^ waterpath = Path::Combine(path, "water.xml");

		// dont go on if file missing
		if (!System::IO::File::Exists(waterpath))
		{
			// log
			Logger::Log(MODULENAME, LogType::Warning,
				"water.xml file missing");

			return;
		}

		// create reader
		reader = XmlReader::Create(waterpath);

		// rootnode
		reader->ReadToFollowing("water");

		// loop entries
		if (reader->ReadToDescendant("texture"))
		{
			do
			{
				waterTextures->Add(reader["name"]);

			} while (reader->ReadToNextSibling("texture"));
		}

		// finish read
		reader->Close();
	};
};};
