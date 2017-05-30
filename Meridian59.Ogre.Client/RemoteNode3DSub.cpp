#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	RemoteNode3DSub::RemoteNode3DSub(Data::Models::SubOverlay^ SubOverlay, ::Ogre::SceneNode* Parent, RemoteNode3D^ RootNode, MeshHotspot^ AttachedHotspot)
	{
		this->SubNodes = gcnew List<RemoteNode3DSub^>();
        this->SubOverlay = SubOverlay;
        this->Parent = Parent;
        this->RootNode = RootNode;
        this->AttachedHotspot = AttachedHotspot;

        CLRString^ resource = SubOverlay->Name->Replace(FileExtensions::BGF, FileExtensions::XML);
        ::Ogre::String& ostr_resource = StringConvert::CLRToOgre(resource);

		ResourceGroupManager* resMan = ResourceGroupManager::getSingletonPtr();
                  
        if (resMan->resourceExists(RESOURCEGROUPMODELS, ostr_resource))
        {
            // load info
			Model3DInfo = gcnew ::Meridian59::Ogre::Model3DInfo(ostr_resource, RESOURCEGROUPMODELS);
                    
            // create 3D object
            CreateMesh();
            CreateParticles();
            CreateSubNodes();
        }
	};

	CLRString^ RemoteNode3DSub::ID::get() 
	{ 
		return RootNode->RoomObject->ID + "_" + SubOverlay->HotSpot + "_" + SubOverlay->Name; 
	};

	void RemoteNode3DSub::CreateMesh()
    {
        // get info from datamodel
        MeshInfo^ info = Model3DInfo->MeshInfo;

		::Ogre::String& ostr_name = StringConvert::CLRToOgre(PREFIX_REMOTENODE_ENTITY + ID);
		::Ogre::String& ostr_name2 = StringConvert::CLRToOgre(PREFIX_REMOTENODE_SCENENODE + ID);

        // create entity
        Entity = RootNode->SceneManager->createEntity(
            ostr_name, 
            *Model3DInfo->MeshInfo->MeshFile,
            RESOURCEGROUPMODELS);

        Entity->setCastShadows(info->CastShadows);

        // create scenenode
        SceneNode = Parent->createChildSceneNode(ostr_name2);
        SceneNode->attachObject(Entity);
                       
        SceneNode->setPosition(*AttachedHotspot->Position);
		SceneNode->setOrientation(*info->Orientation);
        SceneNode->scale(*info->Scale);
        SceneNode->setInitialState();
    };

	void RemoteNode3DSub::CreateSubNodes()
    {
        // process attached subnodes/suboverlays
        for each (Data::Models::SubOverlay^ subOverlay in RootNode->RoomObject->SubOverlays)
        {
            // get all subnodes belonging to this subnode
            MeshHotspot^ hotspot = MeshHotspot::Find(subOverlay->HotSpot, Model3DInfo->Hotspots);
            if (hotspot != nullptr)
                SubNodes->Add(gcnew RemoteNode3DSub(subOverlay, SceneNode, RootNode, hotspot));
        }
    };

	void RemoteNode3DSub::CreateParticles()
    {          
        ParticleSystems = new std::vector<::ParticleUniverse::ParticleSystem*>(Model3DInfo->ParticleSystemsData->Length);

        // process each
        for (unsigned int i = 0; i < ParticleSystems->size(); i++)
        {
            // get info from datamodel
            ParticleSystemInfo^ info = (ParticleSystemInfo^)Model3DInfo->ParticleSystemsData[i];

			if (!info->TemplateValue || !info->Position)
				continue;

			::Ogre::String& ostr_particleid = StringConvert::CLRToOgre(ID);
			::Ogre::String& ostr_particlename = ostr_particleid.append("_").append(*info->Name);

           ::ParticleUniverse::ParticleSystemManager* particleMan =
				::ParticleUniverse::ParticleSystemManager::getSingletonPtr();

			::ParticleUniverse::ParticleSystem* particleSystem = 
				particleMan->createParticleSystem(ostr_particlename + *info->TemplateValue, RootNode->SceneManager);
			
			for (size_t j = 0; j < particleSystem->getNumTechniques(); j++)
				particleSystem->getTechnique(j)->position = *info->Position;
			
            // attach particlesystem to parent scenenode
            SceneNode->attachObject(particleSystem);

			// start particles
			particleSystem->start();

            // save reference to this particle system
            ParticleSystems->at(i) = particleSystem;
        }
    };

	void RemoteNode3DSub::Destroy()
    {
		::Ogre::String& ostr_scenenode = StringConvert::CLRToOgre(PREFIX_REMOTENODE_SCENENODE + ID);
		::Ogre::String& ostr_entity = StringConvert::CLRToOgre(PREFIX_REMOTENODE_ENTITY + ID);
		
        // cleanup scenenode
        if (RootNode->SceneManager->hasSceneNode(ostr_scenenode))
            RootNode->SceneManager->destroySceneNode(ostr_scenenode);

        if (SceneNode != nullptr)
        {
            //SceneNode.Dispose();
            SceneNode = nullptr;
        }

        // cleanup entity
        if (RootNode->SceneManager->hasEntity(ostr_entity))
            RootNode->SceneManager->destroyEntity(ostr_entity);

        if (Entity != nullptr)
        {
            //Entity.Dispose();
            Entity = nullptr;
        }

        if (ParticleSystems != nullptr)
        {
            // cleanup particle systems                
            for(unsigned int i=0; i<ParticleSystems->size(); i++)
            {
                ::ParticleUniverse::ParticleSystem* particleSystem = ParticleSystems->at(i);

				::ParticleUniverse::ParticleSystemManager* particleMan = 
					::ParticleUniverse::ParticleSystemManager::getSingletonPtr();

                particleMan->destroyParticleSystem(particleSystem, RootNode->SceneManager);
            }

			delete ParticleSystems;

            ParticleSystems = nullptr;
        }

        if (SubNodes != nullptr)
        {
            for each (RemoteNode3DSub^ subNode in SubNodes)
                subNode->Destroy();

            SubNodes->Clear();
            SubNodes = nullptr;
        }
    };
};};
