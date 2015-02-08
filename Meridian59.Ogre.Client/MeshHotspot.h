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
#pragma managed(pop)

namespace Meridian59 { namespace Ogre
{	
    /// <summary>
    /// A 3D variant of the BGF hotspot. 
    /// Index means index in BGF, Position and Orientation are 3D.
    /// </summary>
    public ref class MeshHotspot
    {
	protected:
		signed char index;
		::Ogre::Vector3* position;
		::Ogre::Quaternion* orientation;

	public:
		property signed char Index 
		{ 
			public: signed char get() { return index; }
			protected: void set(signed char value) { index = value; }
		};
        
		property ::Ogre::Vector3* Position 
		{ 
			public: ::Ogre::Vector3* get() { return position; }
			protected: void set(::Ogre::Vector3* value) { position = value; } 
		};
        
		property ::Ogre::Quaternion* Orientation 
		{ 
			public: ::Ogre::Quaternion* get() { return orientation; }
			protected: void set(::Ogre::Quaternion* value) { orientation = value; } 
		};

        MeshHotspot(signed char Index, ::Ogre::Vector3* Position, ::Ogre::Quaternion* Orientation)
        {
            index = Index;
            position = Position;
            orientation = Orientation;
        };

		~MeshHotspot()
		{
			if (position)
				delete position;

			if (orientation)
				delete orientation;

			position	= nullptr;
            orientation = nullptr;
		};

        /// <summary>
        /// Tries to find a given Hotspot index in an enumeration.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Hotspots"></param>
        /// <returns>Hotspot with given Index or NULL</returns>
        static MeshHotspot^ Find(signed char Index, IEnumerable<MeshHotspot^>^ Hotspots)
        {
            for each (MeshHotspot^ hotspot in Hotspots)
                if (System::Math::Abs(hotspot->Index) == Index)
                    return hotspot;

            return nullptr;
        };
    };
};};