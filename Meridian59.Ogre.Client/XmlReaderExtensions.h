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
#include "OgreColourValue.h"
#include "OgreQuaternion.h"
#include "OgreVector3.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre 
{
	/// <summary>
    /// Some xml parser functions
    /// </summary>
	public ref class XmlReaderExtensions abstract sealed
    {
	public:
        literal ::System::String^ ATTRIB_COUNT	= "count";
        literal ::System::String^ ATTRIB_ENABLED	= "enabled";
        
        /// <summary>
        /// Number format used (e.g. ',' or '.')
        /// </summary>
		static ::System::Globalization::NumberFormatInfo^ NumberFormatInfo;

		static XmlReaderExtensions()
		{
			// set separator for xml files
			NumberFormatInfo = gcnew ::System::Globalization::NumberFormatInfo();
			NumberFormatInfo->NumberDecimalSeparator = ".";
		};

        /// <summary>
        /// Extracts a ColourValue from a XmlReaders current position
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::ColourValue ReadColor(::System::Xml::XmlReader^ XmlReader)
        {
            float r = float::Parse(XmlReader["r"], NumberFormatInfo);
            float g = float::Parse(XmlReader["g"], NumberFormatInfo);
            float b = float::Parse(XmlReader["b"], NumberFormatInfo);

            return ::Ogre::ColourValue(r, g, b);
        };

        /// <summary>
        /// Extracts a vector from a XmlReaders current position
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::Vector3 ReadVector3(::System::Xml::XmlReader^ XmlReader)
        {
            float x = float::Parse(XmlReader["x"], NumberFormatInfo);
            float y = float::Parse(XmlReader["y"], NumberFormatInfo);
            float z = float::Parse(XmlReader["z"], NumberFormatInfo);

            return ::Ogre::Vector3(x, y, z);
        };

        /// <summary>
        /// Extracts a quaternion from a XmlReaders current position
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::Quaternion ReadQuaternion(::System::Xml::XmlReader^ XmlReader)
        {
            float w = float::Parse(XmlReader["qw"], NumberFormatInfo);
            float x = float::Parse(XmlReader["qx"], NumberFormatInfo);
            float y = float::Parse(XmlReader["qy"], NumberFormatInfo);
            float z = float::Parse(XmlReader["qz"], NumberFormatInfo);

            return ::Ogre::Quaternion(w, x, y, z);
        };

        /// <summary>
        /// Reads an integer value from current node's attribute named "count".
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <returns></returns>
        __forceinline static int ReadCount(::System::Xml::XmlReader^ XmlReader)
        {
            return int::Parse(XmlReader[ATTRIB_COUNT]);
        };

        /// <summary>
        /// Reads a boolean value from current node's attribute named "enabled".
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <returns></returns>
        __forceinline static bool ReadEnabled(::System::Xml::XmlReader^ XmlReader)
        {
            return bool::Parse(XmlReader[ATTRIB_ENABLED]);
        };

        /// <summary>
        /// Reads a float from current node's attribute given by name AttributeName
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        __forceinline static float ReadFloat(::System::Xml::XmlReader^ XmlReader, ::System::String^ AttributeName)
        {
            return float::Parse(XmlReader[AttributeName], NumberFormatInfo);
        };

        /// <summary>
        /// Reads a boolean from current node's attribute given by name AttributeName
        /// </summary>
        /// <param name="XmlReader"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        __forceinline static bool ReadBoolean(::System::Xml::XmlReader^ XmlReader, ::System::String^ AttributeName)
        {
            return bool::Parse(XmlReader[AttributeName]);
        };
    };
};};