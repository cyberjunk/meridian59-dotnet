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
#include "OgreEntity.h"
#include "ParticleUniverseSystem.h"
#pragma managed(pop)

#include "RemoteNode.h"
#include "RemoteNode3DSub.h"
#include "Model3DInfo.h"

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace System::ComponentModel;
	using namespace System::Collections::Generic;
	using namespace Meridian59::Common::Constants;
	using namespace Meridian59::Data::Models;

	/// <summary>
    /// An engine object based on a RoomObject and new 3D meshes
    /// </summary>
	public ref class RemoteNode3D : public RemoteNode
	{
	protected:
		::Ogre::Entity* entity;
		std::vector<::ParticleUniverse::ParticleSystem*>* particleSystems;
		::System::Collections::Generic::List<RemoteNode3DSub^>^ subNodes;
		::Meridian59::Ogre::Model3DInfo^ model3DInfo;
		
		virtual void OnRoomObjectPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e) override;
		virtual void CreateLight() override;
		void ApplyColorTranslation();
		void CreateMesh();
		void CreateSubNodes();
		void CreateParticles();
		virtual void UpdateMaterial() override;

	public:

		/// <summary>
		/// OgreEntity used by this instance
		/// </summary>
        property ::Ogre::Entity* Entity 
		{ 
			public: ::Ogre::Entity* get() { return entity; } 
			protected: void set(::Ogre::Entity* value) { entity = value; }
		};
        
		/// <summary>
		/// ParticleSystems attached to this instance
		/// </summary>
		property std::vector<::ParticleUniverse::ParticleSystem*>* ParticleSystems 
		{ 
			public: std::vector<::ParticleUniverse::ParticleSystem*>* get() { return particleSystems; } 
			protected: void set(std::vector<::ParticleUniverse::ParticleSystem*>* value) { particleSystems = value; }
		};

		/// <summary>
		/// 
		/// </summary>
        property ::Meridian59::Ogre::Model3DInfo^ Model3DInfo 
		{ 
			public: ::Meridian59::Ogre::Model3DInfo^ get() { return model3DInfo; } 
			protected: void set(::Meridian59::Ogre::Model3DInfo^ value) { model3DInfo = value; }
		};

		/// <summary>
		/// 
		/// </summary>
        property ::System::Collections::Generic::List<RemoteNode3DSub^>^ SubNodes 
		{ 
			public: ::System::Collections::Generic::List<RemoteNode3DSub^>^ get() { return subNodes; } 
			protected: void set(::System::Collections::Generic::List<RemoteNode3DSub^>^ value) { subNodes = value; }
		};
       
		/// <summary>
        /// Constructor
        /// </summary>
		RemoteNode3D(Data::Models::RoomObject^ RoomObject, ::Ogre::SceneManager* SceneManager);

		/// <summary>
        /// Destructor
        /// </summary>
		~RemoteNode3D();
	};
};};

