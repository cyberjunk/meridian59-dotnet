#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
   static ControllerSound::ControllerSound()
   {
      soundEngine     = nullptr;
      listenerNode    = nullptr;
      sounds          = nullptr;
      backgroundMusic = nullptr;
      tickWadingPlayed = 0;
      lastListenerPosition = V3(0.0f, 0.0f, 0.0f);
   };

   void ControllerSound::Initialize()
   {
      if (IsInitialized)
         return;

      // init sound list
      sounds = new std::list<ISound*>();

      ISoundDeviceList* deviceList = ::irrklang::createSoundDeviceList();
      ik_s32 devicecount = deviceList->getDeviceCount();

      // we need at least one outputdriver (this can be a NULL device!)
      if (devicecount > 0)
      {
         // sound engine options, may be adjusted
         E_SOUND_ENGINE_OPTIONS options = (E_SOUND_ENGINE_OPTIONS)
            (E_SOUND_ENGINE_OPTIONS::ESEO_MULTI_THREADED |
            E_SOUND_ENGINE_OPTIONS::ESEO_LOAD_PLUGINS |
            E_SOUND_ENGINE_OPTIONS::ESEO_USE_3D_BUFFERS);

         E_SOUND_OUTPUT_DRIVER driver = E_SOUND_OUTPUT_DRIVER::ESOD_AUTO_DETECT;

         // try to initialize irrKlang
         soundEngine = ::irrklang::createIrrKlangDevice(driver, options);

         // if engine is initialized 
         // this is not null, it's null if no sound device in device manager
         if (soundEngine)
         {
            // sound engine properties
            soundEngine->setDefault3DSoundMaxDistance(2000.0f);
            soundEngine->setDefault3DSoundMinDistance(0.0f);
            soundEngine->setRolloffFactor(0.005f);
         }
      }
      else
         Logger::Log("SoundController", LogType::Warning, "No sound device found!");

      // cleanup
      deviceList->drop();

      // mark initialized
      IsInitialized = true;
   };

   void ControllerSound::Destroy()
   {
      if (!IsInitialized)
         return;

      if (sounds)
      {
         sounds->clear();
         delete sounds;
      }

      if (soundEngine)
      {
         soundEngine->stopAllSounds();
         soundEngine->drop();	
      }

      if (listenerNode != nullptr)
      {
         listenerNode->RoomObject->PropertyChanged -= 
            gcnew PropertyChangedEventHandler(&OnRoomObjectPropertyChanged);
      }

      soundEngine     = nullptr;
      listenerNode    = nullptr;
      sounds          = nullptr;
      backgroundMusic = nullptr;

      // mark not initialized
      IsInitialized = false;
   };

   void ControllerSound::SetListenerNode(RemoteNode^ AvatarNode)
   {
      if (!IsInitialized || !soundEngine)
         return;

      // possibly detach old listener
      if (listenerNode)
      {
         listenerNode->RoomObject->PropertyChanged -= 
            gcnew PropertyChangedEventHandler(&OnRoomObjectPropertyChanged);
      }

      // set new value
      listenerNode = AvatarNode;

      // if not null
      if (listenerNode)
      {
         // hook up listener
         listenerNode->RoomObject->PropertyChanged += 
            gcnew PropertyChangedEventHandler(&OnRoomObjectPropertyChanged);

         // initial set
         UpdateListener(listenerNode);
      }
   };

   void ControllerSound::OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
   {
      if(CLRString::Equals(e->PropertyName, RoomObject::PROPNAME_ANGLE) ||
         CLRString::Equals(e->PropertyName, RoomObject::PROPNAME_POSITION3D))
      {
         // update listener if listener object changed position or orientation
         UpdateListener(listenerNode);
      }
   };

   void ControllerSound::UpdateListener(RemoteNode^ AvatarNode)
   {
      if (!IsInitialized || !soundEngine || !AvatarNode || !AvatarNode->SceneNode)
         return;

      RoomObject^ avatar = OgreClient::Singleton->Data->AvatarObject;

      V3 pos = avatar->Position3D;
      V2 dir = MathUtil::GetDirectionForRadian(avatar->Angle);

      vec3df irrpos;
      irrpos.X = (ik_f32)pos.X;
      irrpos.Y = (ik_f32)pos.Y;
      irrpos.Z = (ik_f32)-pos.Z;

      vec3df irrlook;
      irrlook.X = (ik_f32)dir.X;
      irrlook.Y = 0.0f;
      irrlook.Z = (ik_f32)-dir.Y;

      soundEngine->setListenerPosition(irrpos, irrlook);

      // leaf avatar is in
      RooSubSector^ leaf = avatar->SubSector;

      // check for water sector wading sound
      if (leaf != nullptr && leaf->Sector != nullptr && lastListenerPosition != pos)
      {
         V2 pos2D = avatar->Position2D; // get avatar position
         pos2D.ConvertToROO();          // convert to ROO

         // get floor texture height at avatar position and convert back to world
         Real hFloor = 0.0625f * leaf->Sector->CalculateFloorHeight(pos2D.X, pos2D.Y, false);

         // shortcuts
         GameTickOgre^ tick = OgreClient::Singleton->GameTick;
         RooSectorFlags^ flags = leaf->Sector->Flags;

         // below floor, sector with depth and delay passed
         if (pos.Y < hFloor && flags->SectorDepth != RooSectorFlags::DepthType::Depth0 &&
             (tick->Current - tickWadingPlayed > 500*(unsigned int)flags->SectorDepth))
         {
            // get roominfo which stores wading info
            RoomInfo^ roomInfo = OgreClient::Singleton->Data->RoomInformation;

            // the soundinfo to play
            PlaySound^ soundInfo  = gcnew PlaySound();
            
            // set so it's replayed as avatar attached sound
            soundInfo->ID = 0;
            soundInfo->Row = 0;
            soundInfo->Column = 0;

            // set filename and resource
            soundInfo->ResourceName = roomInfo->WadingSoundFile;
            soundInfo->Resource = roomInfo->ResourceWadingSound;

            // play wading sound
            StartSound(soundInfo);

            // save tick we played it
            tickWadingPlayed = tick->Current;
         }
      }

      // save last listener position
      lastListenerPosition = pos;
   };

   void ControllerSound::AdjustMusicVolume()
   {
      if (!backgroundMusic)
         return;

      backgroundMusic->setVolume(OgreClient::Singleton->Config->MusicVolume / 10.0f);
   };

   void ControllerSound::AdjustSoundVolume()
   {
      if (sounds)
      {
         for (std::list<ISound*>::iterator it = sounds->begin(); it != sounds->end(); it++)
            (*it)->setVolume(OgreClient::Singleton->Config->SoundVolume / 10.0f);
      }

      for each(RoomObject^ obj in OgreClient::Singleton->Data->RoomObjects)
      {
         if (!obj->UserData)
            continue;

         RemoteNode^ node = (RemoteNode^)obj->UserData;

         if (!node || !node->Sounds)
            return;

         for (std::list<ISound*>::iterator it = node->Sounds->begin(); it != node->Sounds->end(); it++)
            (*it)->setVolume(OgreClient::Singleton->Config->SoundVolume / 10.0f);
      }
   };

   void ControllerSound::HandleGameModeMessage(GameModeMessage^ Message)
   {
      if (!IsInitialized || !soundEngine)
         return;

      switch ((MessageTypeGameMode)Message->PI)
      {
         case MessageTypeGameMode::Player:
            HandlePlayerMessage((PlayerMessage^)Message);
            break;

         case MessageTypeGameMode::PlayWave:
            HandlePlayWaveMessage((PlayWaveMessage^)Message);
            break;
#if !VANILLA
         case MessageTypeGameMode::StopWave:
            HandleStopWaveMessage((StopWaveMessage^)Message);
            break;
#endif
         case MessageTypeGameMode::PlayMusic:
            HandlePlayMusicMessage((PlayMusicMessage^)Message);
            break;

         case MessageTypeGameMode::PlayMidi:
            HandlePlayMidiMessage((PlayMidiMessage^)Message);
            break;
      }
   };

#if !VANILLA
   void ControllerSound::HandleStopWaveMessage(StopWaveMessage^ Message)
   {
      if (!IsInitialized || !soundEngine || !Message->PlayInfo->Resource || !Message->PlayInfo->ResourceName)
         return;

      // get resource name of wav file
      CLRString^ sourcename = Message->PlayInfo->ResourceName->ToLower();

      // native string
      ::Ogre::String& o_str = StringConvert::CLRToOgre(sourcename);
      const char* c_str = o_str.c_str();

      // check if sound is known to irrklang
      ISoundSource* soundsrc = soundEngine->getSoundSource(c_str, false);

      // Return if not found
      if (!soundsrc)
         return;

      // Source given by object id
      if (Message->PlayInfo->ID > 0)
      {
         // try get source object
         RoomObject^ source = OgreClient::Singleton->Data->RoomObjects->GetItemByID(Message->PlayInfo->ID);

         if (source && source->UserData)
         {
            // get attached remotenode
            RemoteNode^ node = (RemoteNode^)source->UserData;

            if (node && node->Sounds)
            {
               for (std::list<ISound*>::iterator it = node->Sounds->begin(); it != node->Sounds->end();++it)
               {
                  if ((*it)->getSoundSource() == soundsrc)
                  {
                     (*it)->stop();
                     (*it)->drop();
                     it = node->Sounds->erase(it);
                     return;
                  }
               }
            }
         }
      }

      // Check our avatar's sound list
      if (OgreClient::Singleton->Data->AvatarObject &&
          OgreClient::Singleton->Data->AvatarObject->UserData)
      {
         RemoteNode^ node = (RemoteNode^)OgreClient::Singleton->Data->AvatarObject->UserData;

         if (node && node->Sounds)
         {
            for (std::list<ISound*>::iterator it = node->Sounds->begin(); it != node->Sounds->end(); ++it)
            {
               if ((*it)->getSoundSource() == soundsrc)
               {
                  (*it)->stop();
                  (*it)->drop();
                  it = node->Sounds->erase(it);
                  return;
               }
            }
         }
      }

      // Check sounds list
      for (std::list<ISound*>::iterator it = sounds->begin(); it != sounds->end();++it)
      {
         if ((*it)->getSoundSource() == soundsrc)
         {
            (*it)->stop();
            (*it)->drop();
            it = sounds->erase(it);
            return;
         }
      }
      return;
   };
#endif

   void ControllerSound::HandlePlayerMessage(PlayerMessage^ Message)
   {
      if (!IsInitialized || !soundEngine || !sounds)
         return;

      // stop playback of all sounds
      //soundEngine->stopAllSounds();

      for(std::list<ISound*>::iterator it=sounds->begin(); it !=sounds->end(); it++)
      {
         (*it)->stop();
         (*it)->drop();
      }

      // clear references
      sounds->clear();
   };

   void ControllerSound::HandlePlayMusicMessage(PlayMusicMessage^ Message)
   {
      StartMusic(Message->PlayInfo);
   };

   void ControllerSound::HandlePlayMidiMessage(PlayMidiMessage^ Message)
   {
      StartMusic(Message->PlayInfo);
   };

   void ControllerSound::HandlePlayWaveMessage(PlayWaveMessage^ Message)
   {
      StartSound(Message->PlayInfo);
   };

   void ControllerSound::StartSound(PlaySound^ Info)
   {
      if (!IsInitialized || !soundEngine || !Info || !Info->ResourceName || !Info->Resource)
         return;

      if (Info->PlayFlags->IsLoop && OgreClient::Singleton->Config->DisableLoopSounds)
         return;

      ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      // if source is a object, we save it here
      RemoteNode^ attachNode = nullptr;

      // initial playback position
      float x = 0;
      float y = 0;
      float z = 0;

      // source given by object id
      if (Info->ID > 0)
      {
         // try get source object
         RoomObject^ source = OgreClient::Singleton->Data->RoomObjects->GetItemByID(Info->ID);

         if (source && source->UserData)
         {
            // get attached remotenode
            attachNode = (RemoteNode^)source->UserData;

            if (attachNode && attachNode->SceneNode)
            {
               ::Ogre::Vector3 pos = attachNode->SceneNode->getPosition();
               x = (float)pos.x;
               y = (float)pos.y;
               z = (float)-pos.z;
            }
         }
      }

      // source given by row/col
      else if (Info->Row > 0 && Info->Column > 0)
      {
         // convert the center of the server grid square to coords of roo file
         x = (float)(Info->Column - 1) * 1024.0f + 512.0f;
         z = (float)(Info->Row - 1) * 1024.0f + 512.0f;

         RooSubSector^ out;

         if (OgreClient::Singleton->CurrentRoom)
            y = (float)OgreClient::Singleton->CurrentRoom->GetHeightAt(x, z, out, true, false);

         // scale from roo to client and add 1 based num offset
         x = (x * 0.0625f) + 64.0f;
         y = (y * 0.0625f);
         z = -((z * 0.0625f) + 64.0f);
      }

      // source is own avatar
      else
      {
         if (OgreClient::Singleton->Data->AvatarObject &&
            OgreClient::Singleton->Data->AvatarObject->UserData)
         {
            attachNode = (RemoteNode^)OgreClient::Singleton->Data->AvatarObject->UserData;

            if (attachNode && attachNode->SceneNode)
            {
               ::Ogre::Vector3 pos = attachNode->SceneNode->getPosition();
               x = (float)pos.x;
               y = (float)pos.y;
               z = (float)-pos.z;
            }
         }
      }

      ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      // native strings
      const ::Ogre::String& o_StrFull = StringConvert::CLRToOgre(Info->Resource);

      // try start 3D playback
      ISound* sound = soundEngine->play3D(
         o_StrFull.c_str(),
         vec3df(x, y, z), 
         Info->PlayFlags->IsLoop, 
         true, 
         true, 
         ::irrklang::E_STREAM_MODE::ESM_AUTO_DETECT, 
         false);

      // success
      if (sound)
      {
         // set volume
         sound->setVolume(OgreClient::Singleton->Config->SoundVolume / 10.0f);

         // save reference to sound for adjusting (i.e. position)
         if (attachNode)
            attachNode->AddSound(sound);

         // if no soundowner save it ourself
         else
            sounds->push_back(sound);

         // start playback
         sound->setIsPaused(false);
      }
   }

   void ControllerSound::StartMusic(PlayMusic^ Info)
   {
      if (!IsInitialized || !soundEngine || !Info || !Info->ResourceName || !Info->Resource)
         return;

      if (OgreClient::Singleton->Config->MusicVolume > 0.0f)
      {
         const ::Ogre::String& name = StringConvert::CLRToOgre(Info->Resource);

         // no current background music
         if (!backgroundMusic)
         {
            backgroundMusic = soundEngine->play2D(
               name.c_str(), true, true, true, ::irrklang::E_STREAM_MODE::ESM_AUTO_DETECT, false);

            if (backgroundMusic)
            {
               backgroundMusic->setVolume(OgreClient::Singleton->Config->MusicVolume / 10.0f);
               backgroundMusic->setIsPaused(false);
            }
         }

         // stop old background music if another one is to be played
         else if (name != backgroundMusic->getSoundSource()->getName())
         {
            backgroundMusic->stop();
            backgroundMusic->drop();

            backgroundMusic = soundEngine->play2D(
               name.c_str(), true, true, true, ::irrklang::E_STREAM_MODE::ESM_AUTO_DETECT, false);

            if (backgroundMusic)
            {
               backgroundMusic->setVolume(OgreClient::Singleton->Config->MusicVolume / 10.0f);
               backgroundMusic->setIsPaused(false);
            }
         }
      }
   };
};};
