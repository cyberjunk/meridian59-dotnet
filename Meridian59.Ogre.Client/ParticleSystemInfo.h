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
#include "OgreString.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre
{
   /// <summary>
   /// 
   /// </summary>
   public ref class ParticleSystemInfo
   {
   protected:
      ::Ogre::String* name;
      ::Ogre::String* templateValue;
      ::Ogre::String* material;
      ::Ogre::Vector3* position;

   public:
      property ::Ogre::String* Name 
      { 
         public: ::Ogre::String* get() { return name; }
         protected: void set(::Ogre::String* value) { name = value; }
      };

      property ::Ogre::String* TemplateValue 
      { 
         public: ::Ogre::String* get() { return templateValue; }
         protected: void set(::Ogre::String* value) { templateValue = value; } 
      };

      property ::Ogre::String* Material 
      { 
         public: ::Ogre::String* get() { return material; }
         protected: void set(::Ogre::String* value) { material = value; } 
      };

      property ::Ogre::Vector3* Position 
      { 
         public: ::Ogre::Vector3* get() { return position; }
         protected: void set(::Ogre::Vector3* value) { position = value; } 
      };

      inline ParticleSystemInfo(
         ::Ogre::String*  Name, 
         ::Ogre::String*  Template, 
         ::Ogre::String*  Material, 
         ::Ogre::Vector3* Position)
      {
         name          = Name;
         templateValue = Template;
         material      = Material;
         position      = Position;
      };

      inline ~ParticleSystemInfo()
      {
         if (name)
            delete name;

         if (templateValue)
            delete templateValue;

         if (material)
            delete material;

         if (position)
            delete position;

         name           = nullptr;
         templateValue  = nullptr;
         material       = nullptr;
         position       = nullptr;
      };
    };
};};