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
#include "OgreBillboard.h"
#include "OgreBillboardSet.h"
#include "OgreSceneManager.h"
#include "OgreSceneNode.h"
#include "OgreLight.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace Meridian59::Data::Models;
	using namespace Meridian59::Drawing2D;
	using namespace Meridian59::Common;
	using namespace System::ComponentModel;

	/// <summary>
    /// A visible object based on a Projectile and legacy resources (2D).
	/// This is a fake 3D object, implemented using Ogre's Billboards.
    /// </summary>
	public ref class ProjectileNode2D
	{
	private:
		static ProjectileNode2D();

	protected:
		::Ogre::Billboard*		billboard;
        ::Ogre::BillboardSet*	billboardSet;
        ::Ogre::SceneManager*	sceneManager;
		::Ogre::SceneNode*		sceneNode;
		::Ogre::Light*			light;
		Projectile^				projectile;
		Murmur3^				hash;
		
		void OnProjectilePropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
		void OnProjectileAppearanceChanged(Object^ sender, System::EventArgs^ e);
		
		void CreateLight();
		void UpdateLight();
		void DestroyLight();

	public:		
		property Projectile^ Projectile 
		{ 
			public: Data::Models::Projectile^ get() { return projectile; } 
			protected: void set(Data::Models::Projectile^ value) { projectile = value; } 
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

		/// <summary>
		/// Constructor
		/// </summary>
		ProjectileNode2D(Data::Models::Projectile^ Projectile, ::Ogre::SceneManager* SceneManager);
		
		/// <summary>
		/// Destructor
		/// </summary>
		~ProjectileNode2D();
	};
};};

