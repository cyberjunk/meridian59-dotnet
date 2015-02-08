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
#include "OgreFrameListener.h"
#include "OgreMovableObject.h"
#include "OgreCompositorInstance.h"
#include "OgreResourceGroupManager.h"
#include "OgreWindowEventUtilities.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre 
{
	/// <summary>
	/// Custom FrameListener for Ogre
	/// </summary>  
	public class MyFrameListener : public ::Ogre::FrameListener
	{
	public:
		MyFrameListener();

		virtual bool frameRenderingQueued (const ::Ogre::FrameEvent &evt) override;
	};
	
	/// <summary>
	/// Custom WindowEventListener for Ogre
	/// </summary>  
	public class MyWindowEventListener : public ::Ogre::WindowEventListener 
	{
	public:
		MyWindowEventListener();

		virtual bool windowClosing (::Ogre::RenderWindow* rw) override;
		virtual void windowFocusChange (::Ogre::RenderWindow* rw) override;
	};

	/// <summary>
    /// Custom CameraListener for Ogre
    /// </summary>
	public class CameraListener : public ::Ogre::MovableObject::Listener
	{
	public:
		CameraListener();
		~CameraListener(void);

		virtual void objectMoved(::Ogre::MovableObject* obj) override;
	};

	/// <summary>
    /// CompositorListener for Pain effect
    /// </summary>
	public class CompositorPainListener : public ::Ogre::CompositorInstance::Listener
	{
	public:	
		void notifyMaterialSetup(uint32 pass_id, ::Ogre::MaterialPtr& mat);
		void notifyMaterialRender(uint32 pass_id, ::Ogre::MaterialPtr& mat);
		void notifyResourcesCreated(bool forResizeOnly);
	};

	/// <summary>
    /// CompositorListener for Whiteout effect
    /// </summary>
	public class CompositorWhiteoutListener : public ::Ogre::CompositorInstance::Listener
	{
	public:	
		void notifyMaterialSetup(uint32 pass_id, ::Ogre::MaterialPtr& mat);
		void notifyMaterialRender(uint32 pass_id, ::Ogre::MaterialPtr& mat);
		void notifyResourcesCreated(bool forResizeOnly);
	};

	/// <summary>
    /// ResourceGroupListener for Ogre
    /// </summary>
	public class LoadingBarResourceGroupListener :
		public ::Ogre::ResourceGroupListener
	{
	public:
		LoadingBarResourceGroupListener() { };
		~LoadingBarResourceGroupListener(void) { };
		
		virtual void scriptParseStarted(const ::Ogre::String &scriptName, bool &skipThisScript) override;
		virtual void scriptParseEnded(const ::Ogre::String &scriptName, bool skipped) override;
		
		virtual void resourceGroupScriptingStarted(const ::Ogre::String &groupName, size_t scriptCount) override;
		virtual void resourceGroupScriptingEnded(const ::Ogre::String &groupName) override { };
		virtual void resourceGroupLoadStarted(const ::Ogre::String &groupName, size_t resourceCount) override;
		virtual void resourceGroupLoadEnded(const ::Ogre::String &groupName) override { };
		virtual void resourceGroupPrepareStarted(const ::Ogre::String &groupName, size_t resourceCount) override;
		virtual void resourceGroupPrepareEnded(const ::Ogre::String &groupName) override { };

		virtual void resourceLoadStarted(const ::Ogre::ResourcePtr &resource) override;
		virtual void resourceLoadEnded() { };
		
		virtual void worldGeometryStageStarted(const ::Ogre::String &description) override;
		virtual void worldGeometryStageEnded() override;		
	};	
};};
