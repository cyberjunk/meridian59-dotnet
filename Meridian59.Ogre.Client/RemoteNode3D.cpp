#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	RemoteNode3D::RemoteNode3D(Data::Models::RoomObject^ RoomObject, ::Ogre::SceneManager* SceneManager)
		: RemoteNode(RoomObject, SceneManager)
	{
		this->SubNodes = gcnew List<RemoteNode3DSub^>();
        
		CLRString^ mainOverlay		= RoomObject->OverlayFile->Replace(FileExtensions::BGF, FileExtensions::XML);
		::Ogre::String& ostr_mainOverlay = StringConvert::CLRToOgre(mainOverlay);

		ResourceGroupManager* resMan = ResourceGroupManager::getSingletonPtr();

        // check if model exists in resourcegroup
        if (resMan->resourceExists(RESOURCEGROUPMODELS, ostr_mainOverlay))
        {
            // load info
			Model3DInfo = gcnew ::Meridian59::Ogre::Model3DInfo(ostr_mainOverlay, RESOURCEGROUPMODELS);

            // create node
            CreateMesh();
            CreateParticles();
            CreateSubNodes();

            // possibly create light
            CreateLight();

            // possibly switch texture
            ApplyColorTranslation();

            // initial position & orientation
            RefreshPosition();
            RefreshOrientation();
			UpdateNamePosition();
        }      
	};

	RemoteNode3D::~RemoteNode3D()
	{
		::Ogre::String& ostr_entity = 
			PREFIX_REMOTENODE_ENTITY + ::Ogre::StringConverter::toString(roomObject->ID);
		
        // cleanup entity
        if (SceneManager->hasEntity(ostr_entity))
            SceneManager->destroyEntity(ostr_entity);

        // cleanup particle systems                
        if (ParticleSystems)
        {
            for(unsigned int i=0; i<ParticleSystems->size(); i++)
            {
				::ParticleUniverse::ParticleSystem* particleSystem = ParticleSystems->at(i);

				::ParticleUniverse::ParticleSystemManager* particleMan = 
					::ParticleUniverse::ParticleSystemManager::getSingletonPtr();

				particleMan->destroyParticleSystem(particleSystem, sceneManager);
            }

			delete ParticleSystems;           
        }

		// cleanup subnodes
        if (SubNodes)
        {
            for each (RemoteNode3DSub^ subNode in SubNodes)
                subNode->Destroy();

            SubNodes->Clear();            
        }

		if (model3DInfo)
			delete model3DInfo;

		model3DInfo		= nullptr;
		entity			= nullptr;
		particleSystems = nullptr;
		subNodes		= nullptr;
	};

	void RemoteNode3D::OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
    {
		RemoteNode::OnRoomObjectPropertyChanged(sender, e);

		if (CLRString::Equals(e->PropertyName, ObjectBase::PROPNAME_COLORTRANSLATION))
			ApplyColorTranslation();                      
    };

	void RemoteNode3D::CreateLight()
    {
        RemoteNode::CreateLight();

        // use custom light position from 3d model info
        if (Light != nullptr)
            Light->setPosition(*Model3DInfo->MeshInfo->LightPosition);
    };

	void RemoteNode3D::ApplyColorTranslation()
    {
        if (Entity != nullptr)
        {
            // iterate all subentities
            for (uint i = 0; i < Entity->getNumSubEntities(); i++)
            {
                // get this submesh
                SubEntity* subEntity = Entity->getSubEntity(i);

                // get current material of this subentity
                MaterialPtr oldMaterial = subEntity->getMaterial();
                    
                // find marker
                // material name scheme:
                // group/material_modifier
				CLRString^ str = StringConvert::OgreToCLR(oldMaterial->getName());
				
				int pos_mod = str->LastIndexOf('/');
				
				if (pos_mod > -1)
				{
					int pos_col = str->LastIndexOf('/', pos_mod - 1);

					if (pos_col > - 1)
					{
						// build materialname with color modifier
						CLRString^ prefix = str->Substring(0, pos_col + 1);
						CLRString^ appendix = str->Substring(pos_mod, str->Length - pos_mod);

						CLRString^ newMaterialName =
							prefix + RoomObject->ColorTranslation.ToString() + appendix;
	
						::Ogre::String& ostr_newmatname = StringConvert::CLRToOgre(newMaterialName);
				
						if (MaterialManager::getSingletonPtr()->resourceExists(ostr_newmatname))					
							subEntity->setMaterialName(ostr_newmatname);
					}
				}

                // dispose pointer to old material
                oldMaterial.setNull();
            }
        }
    };

	void RemoteNode3D::CreateMesh()
    {
        // get info from datamodel
        MeshInfo^ info = Model3DInfo->MeshInfo;

		::Ogre::String& ostr_entity = 
			PREFIX_REMOTENODE_ENTITY + ::Ogre::StringConverter::toString(roomObject->ID);
        
		// create entity
        Entity = sceneManager->createEntity(
            ostr_entity,
            *Model3DInfo->MeshInfo->MeshFile,
            RESOURCEGROUPMODELS);

        Entity->setCastShadows(info->CastShadows);

        // attach entity
        SceneNode->attachObject(Entity);

        SceneNode->setOrientation(*info->Orientation);
        SceneNode->scale(*info->Scale);
        SceneNode->setInitialState();

        // possibly add a text billboard with name
        //ProcessNameText();
    };

	void RemoteNode3D::CreateSubNodes()
    {
        // process attached subnodes/suboverlays
        for each (SubOverlay^ subOverlay in RoomObject->SubOverlays)
        {
            // get all subnodes belonging to this mainoverlay/mainnode
            MeshHotspot^ hotspot = MeshHotspot::Find(subOverlay->HotSpot, Model3DInfo->Hotspots);
            if (hotspot != nullptr)
                SubNodes->Add(gcnew RemoteNode3DSub(subOverlay, this->SceneNode, this, hotspot));
        }
    };

	void RemoteNode3D::CreateParticles()
    {
        ParticleSystems = new std::vector<ParticleUniverse::ParticleSystem*>(Model3DInfo->ParticleSystemsData->Length);

        // process each
        for (unsigned int i = 0; i < ParticleSystems->size(); i++)
        {			
            // get info from datamodel
			ParticleSystemInfo^ info = (ParticleSystemInfo^)Model3DInfo->ParticleSystemsData[i];
			
			if (!info->TemplateValue || !info->Position)
				continue;

			::Ogre::String& ostr_particleid = ::Ogre::StringConverter::toString(roomObject->ID);
			::Ogre::String& ostr_particlename = ostr_particleid.append("_").append(*info->Name);

			::ParticleUniverse::ParticleSystemManager* particleMan =
				::ParticleUniverse::ParticleSystemManager::getSingletonPtr();
			
			::ParticleUniverse::ParticleSystem* particleSystem = 
				particleMan->createParticleSystem(ostr_particlename, *info->TemplateValue, sceneManager);
			
			for (size_t j = 0; j < particleSystem->getNumTechniques(); j++)
				particleSystem->getTechnique(j)->position = *info->Position;
			
			// don't extend scenenode bbox because of particles
			particleSystem->setBoundsAutoUpdated(false);
            
			// attach particlesystem to parent scenenode
            SceneNode->attachObject(particleSystem);

			// start particles
			particleSystem->start();

            // save reference to this particle system
            ParticleSystems->at(i) = particleSystem;
        }
    };

	void RemoteNode3D::UpdateMaterial()
    {
		if (!SceneNode)
			return;
		
		::Ogre::MaterialManager* matMan = MaterialManager::getSingletonPtr();
		::Ogre::SceneNode::ObjectIterator it = SceneNode->getAttachedObjectIterator();

		while(it.hasMoreElements())
		{
			MovableObject* obj = it.getNext();
			const ::Ogre::String& movableType = obj->getMovableType();

			if (movableType == "Entity")
			{
				::Ogre::Entity* entity = static_cast<::Ogre::Entity*>(obj);
				const ::Ogre::String& name = entity->getName();					
					
				int numSubs = entity->getNumSubEntities();
					
				for(int i = 0; i < numSubs; i++)
				{
					SubEntity* subEntity = entity->getSubEntity(i);
					const ::Ogre::String& matName = subEntity->getMaterialName();
					size_t pos = matName.find_last_of('/');

					if (pos != ::std::string::npos)
					{
						::Ogre::String& prefix = matName.substr(0, pos + 1);
						::Ogre::String newmat;

						// INVISIBLE
						if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Invisible)
						{	
							newmat = prefix.append("invisible");							
						}

						// TRANSLUCENT						
						// 75%
						else if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Translucent75)
						{
							// to do
						}

						// 50%
						else if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Translucent50)
						{
							// to do
						}

						// 25%
						else if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Translucent25)
						{
							// to do
						}
							

						// SHADOWFORM
						else if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Black)
						{
							newmat = prefix.append("black");
						}		

						// TARGET
						else if (RoomObject->IsTarget)
						{
							newmat = prefix.append("target");
						}	

						// MOUSEOVER
						else if (RoomObject->IsHighlighted)
						{
							newmat = prefix.append("mouseover");
						}

						// DEFAULT
						else
						{
							newmat = prefix.append("default");
						}

						// apply material
						if (matMan->resourceExists(newmat))
							subEntity->setMaterialName(newmat);
					}
				}
			}
		}		
	};
};};
