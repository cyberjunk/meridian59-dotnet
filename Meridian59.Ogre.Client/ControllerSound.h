/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

#pragma once

#pragma managed(push, off)
#include "irrKlang.h"
#pragma managed(pop)

#include "RemoteNode.h"

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace ::irrklang;
	using namespace Meridian59::Data::Models;
	using namespace Meridian59::Data;
	using namespace Meridian59::Protocol::Enums;
	using namespace Meridian59::Protocol::GameMessages;

	/// <summary>
    /// Handles playback of sound resources
    /// </summary>
	public ref class ControllerSound abstract sealed
	{
	private:
		static ControllerSound();

		static ::irrklang::ISoundEngine*		soundEngine;
        static ::irrklang::ISound*				backgroundMusic;
		static std::list<::irrklang::ISound*>*	sounds;
		static RemoteNode^						listenerNode;
		
		/// <summary>
        /// Executed when listener object triggers changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);

		/// <summary>
        /// Sets the sound listener object position and orientation
        /// from RemoteNode object.
        /// </summary>
        /// <param name="AvatarNode"></param>
        static void UpdateListener(RemoteNode^ AvatarNode);

		static void HandlePlayerMessage(PlayerMessage^ Message);
		static void HandlePlayWaveMessage(PlayWaveMessage^ Message);
		static void HandlePlayMusicMessage(PlayMusicMessage^ Message);
		static void HandlePlayMidiMessage(PlayMidiMessage^ Message);

	public:
		/// <summary>
        /// All sounds not attached to roomobject IDs (i.e. mapsounds)
        /// </summary>
        static property std::list<::irrklang::ISound*>* Sounds 
		{ 
			public: std::list<::irrklang::ISound*>* get() { return sounds; }
			private: void set(std::list<::irrklang::ISound*>* value) { sounds = value; } 
		}

		/// <summary>
        /// Initializes the sound engine
        /// </summary>
        static void Initialize();

		/// <summary>
        /// Shutdown the sound engine
        /// </summary>
		static void Destroy();

		/// <summary>
        /// Initialization state
        /// </summary>
		static bool IsInitialized;

		/// <summary>
		/// Manually starts to play given music data
		/// </summary>
		static void StartMusic(PlayMusic^ Info);

		/// <summary>
        /// Sets the node which position and orientation is observed
        /// for 3D sound.
        /// </summary>
        /// <param name="AvatarNode"></param>
        static void SetListenerNode(RemoteNode^ AvatarNode);

		static void AdjustMusicVolume();
		static void AdjustSoundVolume();

		static void HandleGameModeMessage(GameModeMessage^ Message);
	};
};};

