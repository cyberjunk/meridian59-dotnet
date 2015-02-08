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
#include "OgreVector3.h"
#include "OgreQuaternion.h"
#include "OgreString.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre
{
    /// <summary>
    /// 
    /// </summary>
    public ref class MeshInfo
    {
	protected:
		::Ogre::String* meshFile;
        ::Ogre::Quaternion* orientation;
        ::Ogre::Vector3* scale;
        ::Ogre::Vector3* lightPosition;
		bool castShadows;
        
	public:
		property ::Ogre::String* MeshFile 
		{ 
			public: ::Ogre::String* get() { return meshFile; }
			protected: void set(::Ogre::String* value) { meshFile = value; }
		};

		property ::Ogre::Quaternion* Orientation 
		{ 
			public: ::Ogre::Quaternion* get() { return orientation; }
			protected: void set(::Ogre::Quaternion* value) { orientation = value; } 
		};
	        
		property ::Ogre::Vector3* Scale 
		{ 
			public: ::Ogre::Vector3* get() { return scale; }
			protected: void set(::Ogre::Vector3* value) { scale = value; } 
		};

		property ::Ogre::Vector3* LightPosition 
		{ 
			public: ::Ogre::Vector3* get() { return lightPosition; }
			protected: void set(::Ogre::Vector3* value) { lightPosition = value; } 
		};

		property bool CastShadows 
		{ 
			public: bool get() { return castShadows; }
			protected: void set(bool value) { castShadows = value; }
		};
       
        MeshInfo(
			::Ogre::String* MeshFile, 
			bool CastShadows, 
			::Ogre::Quaternion* Orientation, 
			::Ogre::Vector3* Scale, 
			::Ogre::Vector3* LightPosition)
        {
            meshFile		= MeshFile;
            castShadows		= CastShadows;
            orientation		= Orientation;
            scale			= Scale;
            lightPosition	= LightPosition;
        };

		~MeshInfo()
		{
			if (meshFile)
				delete meshFile;
		
			if (orientation)
				delete orientation;

			if (scale)
				delete scale;

			if (lightPosition)
				delete lightPosition;

			meshFile		= nullptr;
            orientation		= nullptr;
            scale			= nullptr;
            lightPosition	= nullptr;
		};
    };
};};