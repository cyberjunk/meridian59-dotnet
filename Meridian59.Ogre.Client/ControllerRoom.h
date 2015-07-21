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
#include "Caelum.h"
#include "OgrePrerequisites.h"
#include "OgreString.h"
#include "OgreManualObject.h"
#include "OgreResourceManager.h"
#include "OgreMaterialManager.h"
#include "OgreTextureManager.h"
#include "OgreMeshManager.h"
#include "OgreSharedPtr.h"
#include "OgreResource.h"
#include "OgreStaticGeometry.h"
#pragma managed(pop)

#include "Util.h"
#include "StringConvert.h"
#include "Constants.h"
#include "RemoteNode.h"
#include "RemoteNode2D.h"
#include "RemoteNode3D.h"
#include "ParticleUniverseEventHandlers.h"

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace ::Caelum;
	
	using namespace System::IO;
	using namespace System::Xml;
	using namespace System::Xml::Schema;
	using namespace System::Xml::Serialization;
	using namespace System::Collections::Generic;
	
	using namespace Meridian59::Common;
	using namespace Meridian59::Common::Enums;
	using namespace Meridian59::Common::Constants;
	using namespace Meridian59::Files::ROO;
	using namespace Meridian59::Files::BGF;
	using namespace Meridian59::Data;
	using namespace Meridian59::Protocol::GameMessages;
	using namespace Meridian59::Protocol::Enums;

	/// <summary>
    /// Loads a room to Ogre
    /// </summary>
	public ref class ControllerRoom abstract sealed
	{
	private:
		literal ::System::String^ SKY_DAY		= "skya.bgf";
		literal ::System::String^ SKY_EVENING	= "skyb.bgf";
		literal ::System::String^ SKY_MORNING	= "skyc.bgf";
		literal ::System::String^ SKY_NIGHT		= "skyd.bgf";
		literal ::System::String^ SKY_FRENZY	= "redsky.bgf";
		literal ::System::String^ MODULENAME	= "ControllerRoom";
		literal float SCALE						= 0.0625f;
		literal ::Ogre::Real PARTICLESYSCAMERAOFFSET = (::Ogre::Real)200.f;

		static ::Ogre::ManualObject*				roomDecoration;
		static ::Ogre::SceneNode*					roomNode;
		static ::Ogre::ManualObject*				roomManObj;
		static ::Caelum::CaelumSystem*				caelumSystem;
		static ::Meridian59::Ogre::RemoteNode^		avatarObject;		
		static ::ParticleUniverse::ParticleSystem*	particleSysSnow;
		
		static ::System::Collections::Generic::List<::System::String^>^ recreatequeue;

		static Dictionary<unsigned short, array<System::String^>^>^ grassMaterials;
		static Dictionary<::System::String^, ::System::Collections::Generic::List<V3>^>^ grassPoints;
		static List<::System::String^>^ waterTextures;
		static ::std::vector<::ParticleUniverse::ParticleEventHandler*>* customParticleHandlers;

        /// <summary>
        /// Helper to store vertices processed of a sector
        /// </summary>
        static unsigned int verticesProcessed;

		static void HandlePlayerMessage(PlayerMessage^ Message);
		static void HandleLightAmbient(LightAmbientMessage^ Message);
		static void HandleLightPlayer(LightPlayerMessage^ Message);
		static void HandleLightShading(LightShadingMessage^ Message);
		static void HandleBackground(BackgroundMessage^ Message);
		
		static void AdjustOctree();
		static void AdjustAmbientLight();
		static int GetRoomSectionByMaterial(::Ogre::String Name);
		static int GetDecorationSectionByMaterial(::Ogre::String Name);

		/// <summary>
        /// Static constructor
        /// </summary>
		static ControllerRoom();

		/// <summary>
        /// Creates all walls belonging to a part of a side.
		/// Called from CreateSide()
        /// </summary>
        /// <param name="Side"></param>
		/// <param name="PartType"></param>
		static void CreateSidePart(RooSideDef^ Side, WallPartType PartType);

		/// <summary>
        /// Creates a part of a wall.
		/// Called from CreateSidePart()
        /// </summary>
        /// <param name="Wall">Which wall to create a part from</param>
        /// <param name="PartType">Upper, Middle, Lower</param>
		/// <param name="IsLeftSide">Whether to create left or right side</param>
		/// <param name="TextureWidth">TextureWidth</param>
		/// <param name="TextureHeight">TextureHeight</param>
		/// <param name="TextureShrink">TextureShrink</param>
		static void CreateWallPart(
			RooWall^ Wall, 
			WallPartType PartType, 
			bool IsLeftSide, 
			int TextureWidth,
			int TextureHeight,
			int TextureShrink);

		/// <summary>
		/// Creates a sector floor or ceiling
		/// Called from CreateSector()
		/// </summary>
		/// <param name="Sector"></param>
		/// <param name="IsFloor"></param>
		static void CreateSectorPart(RooSector^ Sector, bool IsFloor);

		/// <summary>
        /// Creates a subsector of a floor or ceiling
		/// Called from CreateSectorPart()
        /// </summary>
        /// <param name="SubSector"></param>
        /// <param name="IsFloor"></param>
        static void CreateSubSector(RooSubSector^ SubSector, bool IsFloor);

		/// <summary>
		/// Creates all floors and sides using specific materialname
		/// </summary>
		/// <param name="MaterialName"></param>
		static void CreateGeometryChunk(::System::String^ MaterialName);

		/// <summary>
		/// Creates decorations
		/// </summary>
		static void CreateDecoration();

		/// <summary>
        /// Possibly creates a single texture and material based on required info.
        /// </summary>
        /// <param name="Texture"></param>
		/// <param name="TextureName"></param>
		/// <param name="MaterialName"></param>
		/// <param name="ScrollSpeed"></param>
		static void CreateTextureAndMaterial(BgfBitmap^ Texture, ::System::String^ TextureName, ::System::String^ MaterialName, V2 ScrollSpeed);

		/// <summary>
        /// Loads the room improvement data (grass, ...) from xml files
        /// </summary>
		static void LoadImproveData();

		/// <summary>
        /// Inits room based particle systems
        /// </summary>
		static void InitParticleSystems();

		/// <summary>
        /// Destroys room based particle systems
        /// </summary>
		static void DestroyParticleSystems();

		/// <summary>
        /// Adds a projectile to the room
        /// </summary>
        /// <param name="Projectile">The RoomObject to add a SceneNode for</param>
        static void ProjectileAdd(Projectile^ Projectile);

		/// <summary>
        /// Removes a projectile from the room
        /// </summary>
        /// <param name="Projectile"></param>
        static void ProjectileRemove(Projectile^ Projectile);
		
		/// <summary>
        /// Adds a RemoteNode for a RoomObject to the scene
        /// </summary>
        /// <param name="roomObject">The RoomObject to add a SceneNode for</param>
        static void RoomObjectAdd(RoomObject^ roomObject);

		/// <summary>
        /// Removes a remotenode from scene
        /// </summary>
        /// <param name="roomObject"></param>
        static void RoomObjectRemove(RoomObject^ roomObject);

		/// <summary>
        /// Handles changes in the projectiles list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnProjectilesListChanged(Object^ sender, ListChangedEventArgs^ e);
		
		/// <summary>
        /// Handles RoomObject List changes, i.e. add new dynamic objects to scene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnRoomObjectsListChanged(Object^ sender, ListChangedEventArgs^ e);

		/// <summary>
        /// Handles WallTexturechanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnRooFileWallTextureChanged(System::Object^ sender, WallTextureChangedEventArgs^ e);
		
		/// <summary>
        /// Handles SectorTexturechanged events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnRooFileSectorTextureChanged(System::Object^ sender, SectorTextureChangedEventArgs^ e);

		/// <summary>
        /// Handles SectorMoved events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnRooFileSectorMoved(System::Object^ sender, SectorMovedEventArgs^ e);

		/// <summary>
        /// Handles changes in the Data layer model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		static void OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);

		/// <summary>
        /// Handles changes in the Snowing effect datamodel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		static void OnEffectSnowingPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);

	public:				
		/// <summary>
		/// Caelum system handling sky
		/// </summary>
		static property ::Caelum::CaelumSystem* CaelumSystem
		{
			public: ::Caelum::CaelumSystem* get() { return caelumSystem; }
			private: void set(::Caelum::CaelumSystem* value) { caelumSystem = value; }
		}

		/// <summary>
        /// The node which is the avatar we're controlling
        /// </summary>
        static property RemoteNode^ AvatarObject 
		{ 
			public: RemoteNode^ get() { return avatarObject; }
			private: void set(RemoteNode^ value) { avatarObject = value; }
		}

		/// <summary>
		/// Shortcut to currently loaded RooFile instance.
		/// References OgreClient::Singleton->Data->RoomInformation->ResourceRoom
		/// </summary>
		static property RooFile^ Room
		{
			public: RooFile^ get();	
		}

		/// <summary>
		/// Shortcut to SceneManager instance.
		/// References OgreClient::Singleton->SceneManager
		/// </summary>
		static property ::Ogre::SceneManager* SceneManager
		{
			public: ::Ogre::SceneManager* get();
		}

		/// <summary>
        /// Set required instance references
        /// </summary>
		static void Initialize();
		
		/// <summary>
        /// Destroys the controller, automatically unloads room
        /// </summary>
		static void Destroy();

		/// <summary>
        /// Initialization state
        /// </summary>
		static bool IsInitialized = false;

		/// <summary>
		/// Inits Caelum
		/// </summary>
		static void InitCaelum();

		/// <summary>
		/// Destroys Caelum
		/// </summary>
		static void DestroyCaelum();
		
		/// <summary>
		/// Refreshs the sky after a change between legacy/caelum
		/// </summary>
		static void UpdateSky();

		/// <summary>
        /// Loads the current room
        /// </summary>
        static void LoadRoom();

		/// <summary>
        /// Unloads the current room
        /// </summary>
        static void UnloadRoom();

		/// <summary>
        /// 
        /// </summary>
		static void Tick(double Tick, double Span);

		/// <summary>
        /// Handle a GameMode message
        /// </summary>
		static void HandleGameModeMessage(GameModeMessage^ Message);
	};
};};

