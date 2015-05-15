#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	static ControllerSound::ControllerSound()
	{
		soundEngine		= nullptr;
        listenerNode	= nullptr;
		sounds			= nullptr;
		backgroundMusic = nullptr;
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
				gcnew PropertyChangedEventHandler(&ControllerSound::OnRoomObjectPropertyChanged);
		}

		soundEngine		= nullptr;
        listenerNode	= nullptr;
		sounds			= nullptr;
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
				gcnew PropertyChangedEventHandler(&ControllerSound::OnRoomObjectPropertyChanged);
		}

		// set new value
		listenerNode = AvatarNode;

		// if not null
		if (listenerNode)
		{
			// hook up listener
			listenerNode->RoomObject->PropertyChanged += 
				gcnew PropertyChangedEventHandler(&ControllerSound::OnRoomObjectPropertyChanged);

			// initial set
			UpdateListener(listenerNode);
		}		
    };

	void ControllerSound::OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
    {
		if(System::String::Equals(e->PropertyName, RoomObject::PROPNAME_ANGLE) ||
		   System::String::Equals(e->PropertyName, RoomObject::PROPNAME_POSITION3D))
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
		irrpos.X = pos.X;
		irrpos.Y = pos.Y;
		irrpos.Z = -pos.Z;

		vec3df irrlook;
		irrlook.X = dir.X;
		irrlook.Y = 0.0f;
		irrlook.Z = -dir.Y;
		
		soundEngine->setListenerPosition(irrpos, irrlook);
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

			case MessageTypeGameMode::PlayMusic:
				HandlePlayMusicMessage((PlayMusicMessage^)Message);
				break;

			case MessageTypeGameMode::PlayMidi:
				HandlePlayMidiMessage((PlayMidiMessage^)Message);
				break;
		}		
    };

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
		if (!IsInitialized || !soundEngine || !Message->PlayInfo->Resource || !Message->PlayInfo->ResourceName)
			return;

		if (Message->PlayInfo->PlayFlags->IsLoop && OgreClient::Singleton->Config->DisableLoopSounds)
			return;

		// if source is a object, we save it here
		RemoteNode^ attachNode = nullptr;

		// initial playback position
		float x = 0;
		float y = 0;
		float z = 0;

		// source given by object id
		if (Message->PlayInfo->ID > 0)
		{
			// try get source object
			RoomObject^ source = OgreClient::Singleton->Data->RoomObjects->GetItemByID(Message->PlayInfo->ID);

			if (source && source->UserData)
			{
				// get attached remotenode
				attachNode = (RemoteNode^)source->UserData;

				if (attachNode && attachNode->SceneNode)
				{
					::Ogre::Vector3 pos = attachNode->SceneNode->getPosition();
					x = pos.x;
					y = pos.y;
					z = -pos.z;
				}
			}
		}

		// source given by row/col
		else if (Message->PlayInfo->Row > 0 && Message->PlayInfo->Column > 0)
		{
			// convert the center of the server grid square to coords of roo file
			x = (float)(Message->PlayInfo->Column - 1) * 1024.0f + 512.0f;
			z = (float)(Message->PlayInfo->Row - 1) * 1024.0f + 512.0f;

			RooSubSector^ out;
			
			if (OgreClient::Singleton->CurrentRoom)
				y = OgreClient::Singleton->CurrentRoom->GetHeightAt(x, z, out, true, false);

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
					x = pos.x;
					y = pos.y;
					z = -pos.z;
				}
			}
		}

		// get resource name of wav file
		System::String^ sourcename = Message->PlayInfo->ResourceName->ToLower();

		// native string
		::Ogre::String o_str = StringConvert::CLRToOgre(sourcename);
		const char* c_str = o_str.c_str();

		// check if sound is known to irrklang
		ISoundSource* soundsrc = soundEngine->getSoundSource(c_str, false);

		// add it if not
		if (!soundsrc)
		{
			// memory info for wav data
			System::IntPtr ptr = Message->PlayInfo->Resource->Item1;
			unsigned int len = Message->PlayInfo->Resource->Item2;

			// add as playback from raw ptr
			if (len > 0 && ptr != ::System::IntPtr::Zero)
				soundsrc = soundEngine->addSoundSourceFromMemory(ptr.ToPointer(), len, c_str, false);
		}

		if (!soundsrc)
			return;

		// try start 3D playback
		ISound* sound = soundEngine->play3D(soundsrc,
			vec3df(x, y, z), Message->PlayInfo->PlayFlags->IsLoop, false, true, false);

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
		}
	};

	void ControllerSound::StartMusic(PlayMusic^ Info)
    {
		if (!IsInitialized || !soundEngine || !Info || !Info->Resource || !Info->ResourceName)
			return;

		if (OgreClient::Singleton->Config->MusicVolume > 0.0f)
		{			
			// native string
			::Ogre::String o_str = StringConvert::CLRToOgre(Info->ResourceName);
			const char* c_str = o_str.c_str();

			// check if sound is already known to irrklang
			ISoundSource* soundsrc = soundEngine->getSoundSource(c_str, false);

			// try add it if not
			if (!soundsrc)
			{
				// memory info for mp3 data
				System::IntPtr ptr	= Info->Resource->Item1;
				unsigned int len	= Info->Resource->Item2;
			
				// add as playback from raw ptr
				if (len > 0 && ptr != ::System::IntPtr::Zero)				
					soundsrc = soundEngine->addSoundSourceFromMemory(ptr.ToPointer(), len, c_str, false);
			}

			if (!soundsrc)
				return;

			// no current background music
			if (!backgroundMusic)
			{
				backgroundMusic = soundEngine->play2D(soundsrc, true, false, true, false);

				if (backgroundMusic)
					backgroundMusic->setVolume(OgreClient::Singleton->Config->MusicVolume / 10.0f);
			}
			
			// stop old background music if another one is to be played
			else if (soundsrc != backgroundMusic->getSoundSource())
			{
				backgroundMusic->stop();
				backgroundMusic->drop();

				backgroundMusic = soundEngine->play2D(soundsrc, true, false, true, false);

				if (backgroundMusic)
					backgroundMusic->setVolume(OgreClient::Singleton->Config->MusicVolume / 10.0f);
			}
		}
	};
};};
