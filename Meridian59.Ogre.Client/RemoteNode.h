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
#include "OgreSceneManager.h"
#include "OgreSceneNode.h"
#include "OgreLight.h"
#include "OgreBillboard.h"
#include "OgreBillboardSet.h"
#include "irrKlang.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace ::irrklang;
	using namespace Meridian59::Data::Models;
	using namespace System::Collections::Generic;
	using namespace System::ComponentModel;

	/// <summary>
    /// An abstract engine object based on a RoomObject
    /// </summary>
	public ref class RemoteNode abstract
	{
	private:
		static RemoteNode();

	protected:
		::Ogre::SceneManager*	sceneManager;
		::Ogre::SceneNode*		sceneNode;
		::Ogre::Light*			light;
		::Ogre::Billboard*		billboardName;
        ::Ogre::BillboardSet*	billboardSetName;
		float					nameTextureWidth;
		float					nameTextureHeight;
      float lastNameOffset = 0.0f;
		std::list<::irrklang::ISound*>* sounds;
		RoomObject^ roomObject;
		
		virtual void OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
		
		virtual void CreateLight();
		void UpdateLight();
		void DestroyLight();

		void CreateName();
		void UpdateName();
		void UpdateNamePosition();
      void UpdateCameraPosition();

		virtual void UpdateMaterial();

	public:
		literal float MAXLIGHTRENDERDISTANCE	= 20000.0f;
		literal System::String^ MODULENAME		= "RemoteNode";        

		#pragma region Properties
		property RoomObject^ RoomObject 
		{ 
			public: Data::Models::RoomObject^ get() { return roomObject; }
			protected: void set(Data::Models::RoomObject^ value) { roomObject = value; } 
		};
        
		property ::Ogre::SceneNode* SceneNode 
		{ 
			public: ::Ogre::SceneNode* get() { return sceneNode; } 
			protected: void set(::Ogre::SceneNode* value) { sceneNode = value; }
		};
        
		property ::Ogre::Light* Light 
		{ 
			public: ::Ogre::Light* get() { return light; } 
			protected: void set(::Ogre::Light* value) { light = value; }
		};       

		property std::list<::irrklang::ISound*>* Sounds 
		{ 
			public: std::list<::irrklang::ISound*>* get () { return sounds; }
			protected: void set(std::list<::irrklang::ISound*>* value) { sounds = value; }
		};

		property bool IsAvatar 
		{ 
			public: bool get() { return RoomObject->IsAvatar; } 
		};
        
		property ::Ogre::SceneManager* SceneManager 
		{ 
			public: ::Ogre::SceneManager* get() { return sceneManager; } 
			protected: void set(::Ogre::SceneManager* value) { sceneManager = value; }
		};
		#pragma endregion

		/// <summary>
        /// Constructor
        /// </summary>
		RemoteNode(Data::Models::RoomObject^ RoomObject, ::Ogre::SceneManager* SceneManager);

		/// <summary>
        /// Destructor
        /// </summary>
		~RemoteNode();

		/// <summary>
        /// Updates the orientation of the SceneNode to the orientation of RoomObject data
        /// </summary>
		virtual void RefreshOrientation();

		/// <summary>
        /// Instantly updates the position of the SceneNode to the position of the RoomObject data
        /// and the height from ROO or terrain
        /// </summary>
		virtual void RefreshPosition();

		/// <summary>
        /// Set node visible or not
        /// </summary>
		void SetVisible(bool Value);

		/// <summary>
        /// Attaches a sound to this node.
        /// </summary>
		void AddSound(::irrklang::ISound* Sound);
	};
};};

