#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	static RemoteNode::RemoteNode()
	{		
	};

	RemoteNode::RemoteNode(Data::Models::RoomObject^ RoomObject, ::Ogre::SceneManager* SceneManager)
	{
		roomObject = RoomObject;
        sceneManager = SceneManager;

		// create scenenode
        ::Ogre::String ostr_scenenodename = 
			PREFIX_REMOTENODE_SCENENODE + ::Ogre::StringConverter::toString(roomObject->ID);
		
		SceneNode = SceneManager->getRootSceneNode()->createChildSceneNode(ostr_scenenodename);
		SceneNode->setFixedYawAxis(true);

		// show boundingbox in debug builds
#if DEBUGBOUNDINGBOX
        SceneNode->showBoundingBox(true);
#endif
		// attach listener
        RoomObject->PropertyChanged += 
			gcnew PropertyChangedEventHandler(this, &RemoteNode::OnRoomObjectPropertyChanged);
            
        // create sound holder list
        sounds = new std::list<ISound*>();

		// possibly create a name
		UpdateName();
	};
	
	RemoteNode::~RemoteNode()
	{
		// detach listener
        RoomObject->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(this, &RemoteNode::OnRoomObjectPropertyChanged);

        // LIGHT FIRST! 
        DestroyLight();

		::Ogre::String ostr_nodeid = 
			PREFIX_REMOTENODE_SCENENODE + ::Ogre::StringConverter::toString(roomObject->ID);

		// cleanup attached name
		if (billboardSetName)
        {
			billboardSetName->clear();
			billboardSetName->detachFromParent();

			SceneManager->destroyBillboardSet(billboardSetName);
        }
		
        // cleanup scenenode
        if (SceneManager->hasSceneNode(ostr_nodeid))
            SceneManager->destroySceneNode(ostr_nodeid);

        // cleanup attached sounds
		if (sounds)
        {
			for(std::list<ISound*>::iterator it = sounds->begin(); it != sounds->end(); ++it)
			{
				ISound* sound = *it;
				sound->stop();
				sound->drop();
			}

			sounds->clear();

			delete sounds;
		}

		sceneNode			= nullptr;
		sounds				= nullptr;
		billboardSetName	= nullptr;
		billboardName		= nullptr;
		sceneManager		= nullptr;
	};

	void RemoteNode::OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
    {
		// update scenenode orientation based on datalayer model     
		if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_ANGLE))
			RefreshOrientation();
		
      // Update light if changed
		else if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_LIGHTINGINFO))
		{
            if (RoomObject->LightingInfo->IsLightOn)
            {
                DestroyLight();
                CreateLight();
            }
            else
                DestroyLight();

		}

		else if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_POSITION3D))
		{
            RefreshPosition();                   
		}       

		else if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_NAME))
		{
            UpdateName();           
        }

		else if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_FLAGS))
		{
			UpdateName();
			UpdateMaterial();
		}

		else if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_ISTARGET))
		{
			UpdateMaterial();
		}

		else if (System::String::Equals(e->PropertyName, Data::Models::RoomObject::PROPNAME_ISHIGHLIGHTED))
		{
			UpdateMaterial();
		}
    };

    void RemoteNode::CreateLight()
    {
        ::Ogre::String& ostr_ligtname = 
			PREFIX_REMOTENODE_LIGHT + ::Ogre::StringConverter::toString(roomObject->ID);

		Light = Util::CreateLight(RoomObject, SceneManager, ostr_ligtname);
            
        if (Light != nullptr)
        {
            // maximum distance we render this light or skip it
            Light->setRenderingDistance(MAXLIGHTRENDERDISTANCE);

            SceneNode->attachObject(Light);
        }
    };

    void RemoteNode::UpdateLight()
    {
        if (Light != nullptr)
        {			
            // adjust the light from M59 values (light class extension)
            Util::UpdateFromILightOwner(*Light, RoomObject);
        }            
    };

    void RemoteNode::DestroyLight()
    {
		::Ogre::String& ostr_ligtname = 
			PREFIX_REMOTENODE_LIGHT + ::Ogre::StringConverter::toString(roomObject->ID);

        if (SceneManager->hasLight(ostr_ligtname))
            SceneManager->destroyLight(ostr_ligtname);
       
        Light = nullptr;       
    };

   void RemoteNode::CreateName()
   {
      ::Ogre::String& ostr_billboard = 
         PREFIX_NAMETEXT_BILLBOARD + ::Ogre::StringConverter::toString(roomObject->ID);

      // create BillboardSet for name
      billboardSetName = sceneManager->createBillboardSet(ostr_billboard, 1);
      billboardSetName->setBillboardOrigin(BillboardOrigin::BBO_BOTTOM_CENTER);
      billboardSetName->setBillboardType(BillboardType::BBT_POINT);
      billboardSetName->setUseAccurateFacing(false);
      billboardSetName->setAutoextend(false);

      // no boundingbox for billboardset (we don't wanna catch rays with name)
      billboardSetName->setBounds(AxisAlignedBox::BOX_NULL, 0.0f);
        
      // create Billboard
      billboardName = billboardSetName->createBillboard(::Ogre::Vector3::ZERO);
      billboardName->setColour(ColourValue::ZERO);

      // attach name billboardset to object
      SceneNode->attachObject(billboardSetName);
   };

   void RemoteNode::UpdateName()
   {
#ifdef VANILLA
      bool showName = RoomObject->Flags->IsPlayer;
#else
      bool showName = RoomObject->Flags->IsDisplayName;
#endif
      if (showName && 
         !(RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Invisible) &&
         RoomObject->Name != nullptr &&
         !System::String::Equals(RoomObject->Name, System::String::Empty) &&
         !(RoomObject->IsAvatar && ControllerInput::IsCameraFirstPerson))
      {
         // create or reactivate name text
         if (billboardSetName == nullptr)
            CreateName();

         else
            billboardSetName->setVisible(true);

         // start update
         System::String^ strTex = PREFIX_NAMETEXT_TEXTURE + RoomObject->Name + 
            "/" + NameColors::GetColorFor(RoomObject->Flags).ToString();

         System::String^ strMat = PREFIX_NAMETEXT_MATERIAL + RoomObject->Name + 
            "/" + NameColors::GetColorFor(RoomObject->Flags).ToString();

         ::Ogre::String& texName = StringConvert::CLRToOgre(strTex);
         ::Ogre::String& matName = StringConvert::CLRToOgre(strMat);

         // create Texture and material
         TextureManager* texMan = TextureManager::getSingletonPtr();
         MaterialManager* matMan = MaterialManager::getSingletonPtr();

         if (!texMan->resourceExists(texName))
         {               
            // create bitmap to draw on
            System::Drawing::Bitmap^ bitmap = 
               Meridian59::Drawing2D::ImageComposerGDI<Data::Models::ObjectBase^>::NameBitmap::Get(RoomObject);
                			
            // create texture from bitmap
            Util::CreateTexture(bitmap, texName, TEXTUREGROUP_MOVABLETEXT);
            
            // cleanup
            delete bitmap;
         }

         Util::CreateMaterialLabel(matName, texName, MATERIALGROUP_MOVABLETEXT, nullptr);

         // set material
         billboardSetName->setMaterialName(matName);

         // get size from texture
         TexturePtr texPtr = TextureManager::getSingletonPtr()->createOrRetrieve(
            texName, TEXTUREGROUP_MOVABLETEXT).first.staticCast<Texture>();

         nameTextureWidth = (float)texPtr->getWidth();
         nameTextureHeight = (float)texPtr->getHeight();

         billboardSetName->setDefaultDimensions(nameTextureWidth, nameTextureHeight);
         billboardSetName->setBounds(AxisAlignedBox::BOX_NULL, 0.0f);
         
         // update position of name
         UpdateNamePosition();

         texPtr.setNull();
      }

      else if (billboardSetName != nullptr)
      {
         // hide it
         billboardSetName->setVisible(false);			
      }
   };

   void RemoteNode::UpdateCameraPosition()
   {
      // if this scenenode has the cameranode attached
      // set camera y offset
      if (OgreClient::Singleton->CameraNode->getParentSceneNode() == sceneNode)
      {
         // get new height
         float height = Util::GetSceneNodeHeight(SceneNode) * 0.93f;

         // get old position
         const ::Ogre::Vector3& oldPos = OgreClient::Singleton->CameraNode->getPosition();

         // get difference between old/new
         float diff = height - (float)oldPos.y;

         // ignore small differences, otherwise adjust camera to top of image
         if (abs(diff) > 16.0f)
            OgreClient::Singleton->CameraNode->setPosition(0.0f, height, 0.0f);
      }
   };

   void RemoteNode::UpdateNamePosition()
   {
      if (billboardSetName == nullptr || billboardName == nullptr || sceneNode == nullptr)
         return;

      System::String^ strMat = PREFIX_NAMETEXT_MATERIAL + RoomObject->Name +
         "/" + NameColors::GetColorFor(RoomObject->Flags).ToString();

      ::Ogre::String& matName = StringConvert::CLRToOgre(strMat);

      // create Texture and material
      MaterialManager& matMan = MaterialManager::getSingleton();
      MaterialPtr matPtr = matMan.getByName(matName);

      if (matPtr.isNull())
         return;

      Pass* pass = matPtr->getTechnique(0)->getPass(0);

      // get fragment shader parameters from ambient pass
      const GpuProgramParametersSharedPtr paramsPass =
         pass->getVertexProgramParameters();

      // determine height to place label
      float h = Util::GetSceneNodeHeight(sceneNode) + 1.0f;

      // get change
      float diff = h - lastNameOffset;
      
      // ignore small changes (due to animations)
      if (abs(diff) > 16.0f)
      {
         paramsPass->setNamedConstant("offset", h);
         lastNameOffset = h;
      }
   };

    void RemoteNode::RefreshOrientation()
    {
		// reset scenenode orientation to M59 angle
        Util::SetOrientationFromAngle(*SceneNode, (float)RoomObject->Angle);                
    };

    void RemoteNode::RefreshPosition()
    {
		::Ogre::Vector3& pos = Util::ToOgre(RoomObject->Position3D);

        // update scenenode position from datamodel
		SceneNode->setPosition(pos);
		
		// update sound if any
		if (sounds->size() > 0)
		{
			vec3df irrpos;
			irrpos.X = (ik_f32)pos.x;
			irrpos.Y = (ik_f32)pos.y;
			irrpos.Z = (ik_f32)-pos.z;

			ISound* sound;

			// update position of attached playback sounds
			for(std::list<ISound*>::iterator it=sounds->begin(); 
				it !=sounds->end(); 
				it++)
			{		
				sound = *it;
				sound->setPosition(irrpos);
			}
		}
	};
	
	void RemoteNode::UpdateMaterial()
    {
	};

	void RemoteNode::SetVisible(bool Value)
	{
		if (SceneNode != nullptr)
		{
			// disable everything attached
			SceneNode->setVisible(Value);
			
			// reenable light
			if (light != nullptr)
				light->setVisible(true);

			// update name
			UpdateName();
		}
		
	}

	void RemoteNode::AddSound(ISound* Sound)
    {
        // add sound to list
		sounds->push_back(Sound);
    };
};};
