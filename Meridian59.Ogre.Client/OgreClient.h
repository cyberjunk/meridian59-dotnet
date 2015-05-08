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
#include "OgreRoot.h"
#include "OgreRenderWindow.h"
#include "OgreRenderSystem.h"
#include "OgreCamera.h"
#include "OgreSceneManager.h"
#include "OgreSceneNode.h"
#include "OgreViewport.h"
#include "OgreString.h"
#include "OgreConfigFile.h"
#include "OgreD3D9RenderSystem.h"
#include "OgreOctreePlugin.h"
#include "OgreCgPlugin.h"
#include "ParticleUniversePlugin.h"
#pragma managed(pop)

#include "resource.h"
#include "StringDefines.h"
#include "StringConvert.h"
#include "ControllerSound.h"
#include "ControllerInput.h"
#include "ControllerEffects.h"
#include "ControllerRoom.h"
#include "ControllerUI.h"
#include "OgreListeners.h"
#include "OISListeners.h"
#include "ProjectileNode2D.h"
#include "MiniMapCEGUI.h"
#include "ImageBuilders.h"
#include "GameTickOgre.h"
#include "ResourceManagerOgre.h"
#include "DataControllerOgre.h"
#include "OgreClientConfig.h"

namespace Meridian59 { namespace Ogre
{
	using namespace ::Ogre;
	using namespace System::IO;
	using namespace System::Collections::Generic;
	using namespace Meridian59::Protocol::Events;
	using namespace Meridian59::Protocol::GameMessages;
	using namespace Meridian59::Launcher;
	using namespace Meridian59::Data::Models;
	using namespace Meridian59::Drawing2D;
	using namespace Meridian59::Ogre;
#ifdef _DEBUG
	using namespace Meridian59::DebugUI;
	using namespace Meridian59::DebugUI::Events;
#endif

	/// <summary>
    /// Meridian 59 client implementation based on Ogre3D and other open-source frameworks.
	/// This class implements the core library client class.
    /// </summary>
	public ref class OgreClient : public ::Meridian59::Client::SingletonClient<
		::Meridian59::Ogre::GameTickOgre^, 
		::Meridian59::Ogre::ResourceManagerOgre^, 
		::Meridian59::Ogre::DataControllerOgre^,
		::Meridian59::Ogre::OgreClientConfig^,
		::Meridian59::Ogre::OgreClient^>
	{
	protected:
        ::Ogre::Root*			root;
		::Ogre::RenderWindow*	renderWindowDummy;
        ::Ogre::RenderWindow*	renderWindow;
        ::Ogre::RenderSystem*	renderSystem;
		::Ogre::Camera*			camera;
        ::Ogre::SceneNode*		cameraNode;
        ::Ogre::Viewport*		viewport;
		::Ogre::Viewport*		viewportInvis;
        ::Ogre::SceneManager*	sceneManager;
		::Ogre::OctreePlugin*	pluginOctree;
		::Ogre::CgPlugin*		pluginCG;
		::Caelum::CaelumPlugin*	pluginCaelum;
		::ParticleUniverse::ParticleUniversePlugin* pluginParticleUniverse;

		HWND					renderWindowHandle;
		CameraListener*			cameraListener;
		MyWindowEventListener*	windowListener;
        MiniMapCEGUI^			miniMap;
		LauncherForm^			launcherForm;

        bool isEngineInitialized;
		bool hasFocus;
		bool isWinCursorVisible;
		bool invisViewportUpdateFlip;

#ifdef _DEBUG
		DebugForm^ debugForm;
#endif
		/// <summary>
        /// 
        /// </summary>
		virtual void Cleanup() override;
		
		/// <summary>
        /// 
        /// </summary>
		void CleanupEngine();
		
		/// <summary>
        /// Shows the launcher form
        /// </summary>
		void ShowLauncherForm();

		/// <summary>
        /// Handle network client exception
        /// </summary>
        /// <param name="Error"></param>
        virtual void OnServerConnectionException(System::Exception^ Error) override;
		
		/// <summary>
        /// 
        /// </summary>
		void OnLauncherConnectRequest(::System::Object^ sender, ::System::EventArgs^ e);
		
		/// <summary>
        /// 
        /// </summary>
		void OnLauncherFormExit(::System::Object^ sender, ::System::EventArgs^ e);

		/// <summary>
        /// Initializes resouces
        /// </summary>
		void InitResources();

		/// <summary>
        /// Initializes a resourcegroup by Ogre itself
        /// </summary>
		void InitResourceGroup(::System::String^ Name, bool AddRoot, bool AddSubfolders, System::IO::SearchOption Recursive, bool Initialize, bool Load);
		
		/// <summary>
        /// Registers files in a folder of Type (e.g. Texture) and a pattern (e.g. *.png) into a resourcegroup,
		/// even if not referenced anywhere yet.
        /// </summary>
		void InitResourceGroupManually(::System::String^ Name, bool Initialize, bool Load, ::System::String^ Type, System::String^ Pattern);

		/// <summary>
        /// Handler for any incoming LoginMode message
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleLoginModeMessage(LoginModeMessage^ Message) override;
		
		/// <summary>
        /// Handler for any incoming GameMode message
        /// </summary>
        /// <param name="Message"></param>
        virtual void HandleGameModeMessage(GameModeMessage^ Message) override;

		/// <summary>
        /// Handler for a successful verification of login credentials
        /// </summary>
        /// <param name="Message"></param>
        virtual void HandleLoginOKMessage(LoginOKMessage^ Message) override;
		
		/// <summary>
        /// Handler for failed login/wrong credentials.
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleLoginFailedMessage(LoginFailedMessage^ Message) override;

		/// <summary>
        /// Handler in case the account had no character slots.
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleNoCharactersMessage(NoCharactersMessage^ Message) override;

		/// <summary>
        /// Handler for a custom error at login.
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleLoginModeMessageMessage(LoginModeMessageMessage^ Message) override;

		/// <summary>
        /// Handler for a mismatch application versions message (update).
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleGetClientMessage(GetClientMessage^ Message) override;
		
		/// <summary>
        /// Handler for a mismatch resource versions message (update).
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleDownloadMessage(DownloadMessage^ Message) override;
		
		/// <summary>
        /// Handler for message containing selectable avatars and welcome msg
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleCharactersMessage(CharactersMessage^ Message) override;

		/// <summary>
        /// Handler for server requesting login credentials.
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleGetLoginMessage(GetLoginMessage^ Message) override;

		/// <summary>
        /// Handler for NamesLookupMessage.
		/// Contains the response IDs for name lookup.
        /// </summary>
        /// <param name="Message"></param>
		virtual void HandleLookupNamesMessage(LookupNamesMessage^ Message) override;

		/// <summary>
        /// Overwritten suicide method to show UI for validation
        /// </summary>
		virtual void Suicide() override;
		
#ifdef _DEBUG
		void debugForm_PacketLogChanged(Object^ sender, PacketLogChangeEventArgs^ e);
		void debugForm_PacketSend(Object^ sender, GameMessageEventArgs^ e);
		void ShowDebugForm();
#endif

	public:
		property unsigned char AppVersionMajor
		{ 
			public: virtual unsigned char get() override { return 90; } 			
		};

		property unsigned char AppVersionMinor
		{ 
			public: virtual unsigned char get() override { return 1; } 			
		};
		
		property ::Ogre::Root* Root 
		{ 
			public: ::Ogre::Root* get() { return root; } 
			protected: void set(Ogre::Root* value) { root = value; }
		};

		property ::Ogre::RenderWindow* RenderWindow 
		{ 
			public: ::Ogre::RenderWindow* get() { return renderWindow; } 
			protected: void set(Ogre::RenderWindow* value) { renderWindow = value; }
		};
		
		property ::Ogre::RenderSystem* RenderSystem 
		{ 
			public: ::Ogre::RenderSystem* get() { return renderSystem; } 
			protected: void set(Ogre::RenderSystem* value) { renderSystem = value; }
		};

		property ::Ogre::Camera* Camera 
		{ 
			public: ::Ogre::Camera* get() { return camera; } 
			protected: void set(Ogre::Camera* value) { camera = value; }
		};

		property ::Ogre::SceneNode* CameraNode 
		{ 
			public: ::Ogre::SceneNode* get() { return cameraNode; } 
			protected: void set(Ogre::SceneNode* value) { cameraNode = value; }
		};

		property ::Ogre::Viewport* Viewport 
		{ 
			public: ::Ogre::Viewport* get() { return viewport; } 
			protected: void set(Ogre::Viewport* value) { viewport = value; }
		};

		property ::Ogre::Viewport* ViewportInvis 
		{ 
			public: ::Ogre::Viewport* get() { return viewportInvis; } 
			protected: void set(Ogre::Viewport* value) { viewportInvis = value; }
		};

		property ::Ogre::SceneManager* SceneManager 
		{ 
			public: ::Ogre::SceneManager* get() { return sceneManager; } 
			protected: void set(Ogre::SceneManager* value) { sceneManager = value; }
		};

		property HWND RenderWindowHandle
		{ 
			public: HWND get() { return renderWindowHandle; } 
			protected: void set(HWND value) { renderWindowHandle = value; }
		};

		property bool HasFocus 
		{ 
			public: bool get() { return hasFocus; } 
			public: void set(bool value) { hasFocus = value; }
		};

		property MiniMapCEGUI^ MiniMap 
		{ 
			public: MiniMapCEGUI^ get() { return miniMap; } 
			protected: void set(MiniMapCEGUI^ value) { miniMap = value; }
		};
		
		/// <summary>
        /// Constructor
        /// </summary>
		OgreClient();

		/// <summary>
        /// Custom startup code
        /// </summary>
		virtual void Init() override;

		/// <summary>
        /// Initializes the engine
        /// </summary>
        void InitEngine();

		/// <summary>
        /// Called each mainthread loop
        /// </summary>
        virtual void Update() override;
		
		/// <summary>
        /// Overwritten from base class to also set the 
		/// ActionButtons from Config.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="RequestBasicInfo"></param>
        /// <param name="Name"></param>
        virtual void SendUseCharacterMessage(ObjectID^ ID, bool RequestBasicInfo, ::System::String^ Name) override;
	};
};};

