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
#include "OgreResourceGroupManager.h"
#pragma managed(pop)

#include "MeshInfo.h"
#include "MeshHotspot.h"
#include "ParticleSystemInfo.h"
#include "XmlReaderExtensions.h"
#include "Util.h"
#include "StringConvert.h"

namespace Meridian59 { namespace Ogre
{
	using namespace System::Xml;
	using namespace System::Xml::Schema;
	using namespace System::Xml::Serialization;

    /// <summary>
    /// 
    /// </summary>
	public ref class Model3DInfo : IXmlSerializable
    {	
	protected:
		::Meridian59::Ogre::MeshInfo^ meshInfo;
		array<MeshHotspot^>^ hotspots;
		array<ParticleSystemInfo^>^ particleSystemsData;

		void ProcessMesh(XmlReader^ XmlReader)
        {
            XmlReader->ReadToFollowing(TAG_MESH);
            CLRString^ meshfile = XmlReader["file"];
			::Ogre::String* ostr_meshfile = StringConvert::CLRToOgrePtr(meshfile);

            bool castshadows = System::Convert::ToBoolean(XmlReader["castshadows"]);

            XmlReader->ReadToFollowing("orientation");
            Quaternion orientation = XmlReaderExtensions::ReadQuaternion(XmlReader);

            XmlReader->ReadToFollowing("scale");
            ::Ogre::Vector3 scale = XmlReaderExtensions::ReadVector3(XmlReader);

            XmlReader->ReadToFollowing("lightposition");
            ::Ogre::Vector3 lightpos = XmlReaderExtensions::ReadVector3(XmlReader);

            meshInfo = gcnew ::Meridian59::Ogre::MeshInfo(ostr_meshfile, castshadows, 
				new Quaternion(orientation), new ::Ogre::Vector3(scale), new ::Ogre::Vector3(lightpos));
        };

        void ProcessHotspots(XmlReader^ XmlReader)
        {
            // get count of hotspots
            XmlReader->ReadToFollowing(TAG_HOTSPOTS);
            int hotspotscount = XmlReaderExtensions::ReadCount(XmlReader);

            Hotspots = gcnew array<MeshHotspot^>(hotspotscount);

            // process each
            for (int i = 0; i < hotspotscount; i++)
                ProcessHotspot(XmlReader, i);
        };

        void ProcessHotspot(XmlReader^ XmlReader, int ArrayPosition)
        {
            XmlReader->ReadToFollowing(TAG_HOTSPOT);
            signed char index = System::Convert::ToSByte(XmlReader[ATTRIB_INDEX]);

            XmlReader->ReadToFollowing("position");
            ::Ogre::Vector3 position = XmlReaderExtensions::ReadVector3(XmlReader);

            XmlReader->ReadToFollowing("orientation");
            ::Ogre::Quaternion orientation = XmlReaderExtensions::ReadQuaternion(XmlReader);

            Hotspots[ArrayPosition] = gcnew MeshHotspot(index, &position, &orientation);
        };

        void ProcessParticles(XmlReader^ XmlReader)
        {
            // get count of particles
            XmlReader->ReadToFollowing(TAG_PARTICLES);
            int particlescount = XmlReaderExtensions::ReadCount(XmlReader);

            ParticleSystemsData = gcnew array<ParticleSystemInfo^>(particlescount);

            // process each
            for (int i = 0; i < particlescount; i++)
                ProcessParticle(XmlReader, i);
        };

        void ProcessParticle(XmlReader^ XmlReader, int ArrayPosition)
        {
            XmlReader->ReadToFollowing(TAG_PARTICLE);
            ::Ogre::String* name = StringConvert::CLRToOgrePtr(XmlReader["name"]);
            ::Ogre::String* templateVal = StringConvert::CLRToOgrePtr(XmlReader["template"]);
            ::Ogre::String* material = StringConvert::CLRToOgrePtr(XmlReader["material"]);

            XmlReader->ReadToFollowing("position");
            ::Ogre::Vector3 position = XmlReaderExtensions::ReadVector3(XmlReader);

            ParticleSystemsData[ArrayPosition] = gcnew ParticleSystemInfo(
                name, templateVal, material, new ::Ogre::Vector3(position));
        };

	public:
		literal CLRString^ TAG_MODEL = "model";
        literal CLRString^ TAG_MESH = "mesh";
        literal CLRString^ TAG_HOTSPOTS = "hotspots";
        literal CLRString^ TAG_HOTSPOT = "hotspot";
        literal CLRString^ TAG_PARTICLES = "particles";
        literal CLRString^ TAG_PARTICLE = "particle";
        literal CLRString^ ATTRIB_INDEX = "index";

		property ::Meridian59::Ogre::MeshInfo^ MeshInfo 
		{ 
			public: ::Meridian59::Ogre::MeshInfo^ get() { return meshInfo; }
			protected: void set(::Meridian59::Ogre::MeshInfo^ value) { meshInfo = value; } 
		};

		property array<MeshHotspot^>^ Hotspots 
		{ 
			public: array<MeshHotspot^>^ get() { return hotspots; }
			protected: void set(array<MeshHotspot^>^ value) { hotspots = value; } 
		};

		property array<ParticleSystemInfo^>^ ParticleSystemsData 
		{ 
			public: array<ParticleSystemInfo^>^ get() { return particleSystemsData; }
			protected: void set(array<ParticleSystemInfo^>^ value) { particleSystemsData = value; } 
		};

		Model3DInfo(const ::Ogre::String& XmlModelResource, const ::Ogre::String& ResourceGroup)
        {
            // get xmlreader on mogre resource                   
            DataStreamPtr streamPtr = 
				ResourceGroupManager::getSingletonPtr()->openResource(XmlModelResource, ResourceGroup);
            
			XmlReader^ reader = XmlReader::Create(Util::DataPtrToStream(streamPtr));

            ReadXml(reader);

            // cleanup
            reader->Close();
            streamPtr->close();
        };

		~Model3DInfo()
		{
			for(int i=0; i < hotspots->Length; i++)
				delete hotspots[i];
			
			for(int i=0; i < particleSystemsData->Length; i++)
				delete particleSystemsData[i];

			delete meshInfo;
		};

		virtual XmlSchema^ GetSchema()
        {
            return nullptr;
        };

		virtual void ReadXml(XmlReader^ reader)
        {           
            // rootnode
            reader->ReadToFollowing(TAG_MODEL);

            // read mesh
            ProcessMesh(reader);

            // read hotspots
            ProcessHotspots(reader);

            // read particles
            ProcessParticles(reader);
        };

		virtual void WriteXml(XmlWriter^ writer)
        {
            throw gcnew System::NotImplementedException();
        };
	};
};};