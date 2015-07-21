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
#include "OgreString.h"
#include "CEGUI/String.h"
#pragma managed(pop)

#include "Constants.h"
#include <msclr/marshal.h>

namespace Meridian59 { namespace Ogre 
{
	using namespace msclr::interop;

	/// <summary>
	/// Converts CLR strings to OGRE and vice versa.
	/// </summary>
	public ref class StringConvert abstract sealed
	{
	private:
		static marshal_context^ context;
		static System::Text::UTF8Encoding^ encoderUTF8;

		static StringConvert::StringConvert()
		{
			context = gcnew marshal_context();
			encoderUTF8 = gcnew System::Text::UTF8Encoding();
		};

	public:
		/// <summary>
		/// Converts System::String to Ogre::String.
		/// Turns NULL strings into empty "" strings.
		/// </summary>
		__inline static ::Ogre::String CLRToOgre(System::String^ CLRString)
		{
			// handle null and empty string
			if (CLRString == nullptr || CLRString->Length == 0)
				return ::Ogre::String(STRINGEMPTY);
			
			else
				return ::Ogre::String(context->marshal_as<const char*>(CLRString));
		};

		/// <summary>
		/// Converts System::String to Ogre::String*.
		/// Turns NULL strings into empty "" strings.
		/// </summary>
		__inline static ::Ogre::String* CLRToOgrePtr(System::String^ CLRString)
		{
			// handle null and empty string
			if (CLRString == nullptr || CLRString->Length == 0)
				return new ::Ogre::String(STRINGEMPTY);
			
			else
				return new ::Ogre::String(context->marshal_as<const char*>(CLRString));
		};

		/// <summary>
		/// Converts const Ogre::String to System::String 
		/// </summary>
		__inline static System::String^ OgreToCLR(const ::Ogre::String OgreString)
		{
			return gcnew System::String(OgreString.c_str());
		};

		/// <summary>
		/// Converts CEGUI::String (native, UTF32) to System::String (managed, UTF16)
		/// </summary>
		__inline static System::String^ CEGUIToCLR(const ::CEGUI::String CEGUIString)
		{
			// handle empty string
			if (CEGUIString.length() == 0)
				return ::System::String::Empty;

			// otherwise: get pointer to utf32 data
			const CEGUI::utf32* strC = CEGUIString.ptr();

			// how much bytes represent the utf32 string (4 per codepoint)
			const int bytelength = CEGUIString.length() * 4;

			// convert to CLR string
			System::String^ strCLR = gcnew System::String(
				(const char*)strC, 0, bytelength, System::Text::Encoding::UTF32);

			// return
			return strCLR;
		};

		/// <summary>
		/// Converts System::String (managed, UTF16) to CEGUI::String (native, UTF32).
		/// Turns NULL strings into empty "" strings.
		/// </summary>
		__inline static ::CEGUI::String CLRToCEGUI(System::String^ CLRString)
		{
			// TODO: 
			// Make this convert to UTF32 instead of UTF8
			
			// handle null and empty string
			if (CLRString == nullptr || CLRString->Length == 0)
				return ::CEGUI::String(STRINGEMPTY);

			// otherwise: encode as UTF8 into managed byte[]
			array<unsigned char>^ values = encoderUTF8->GetBytes(CLRString);

			// get pointer to first byte by pinning it
			pin_ptr<unsigned char> utf8Data = &values[0];
			
			// initiate CEGUI::String
			::CEGUI::String guiStr = ::CEGUI::String(
				(const CEGUI::utf8*)utf8Data, values->Length);

			// return
			return guiStr;
		};

		/// <summary>
		/// Converts System::String (managed, UTF16) to CEGUI::String* (native, UTF32) 
		/// Turns NULL strings into empty "" strings.
		/// </summary>
		__inline static ::CEGUI::String* CLRToCEGUIPtr(System::String^ CLRString)
		{
			// TODO: 
			// Make this convert to UTF32 instead of UTF8
			
			// handle null and empty string
			if (CLRString == nullptr || CLRString->Length == 0)
				return new ::CEGUI::String(STRINGEMPTY);

			// otherwise: encode as UTF8 into managed byte[]
			array<unsigned char>^ values = encoderUTF8->GetBytes(CLRString);

			// get pointer to first byte by pinning it
			pin_ptr<unsigned char> utf8Data = &values[0];
			
			// initiate CEGUI::String
			::CEGUI::String* guiStr = new ::CEGUI::String(
				(const CEGUI::utf8*)utf8Data, values->Length);

			// return
			return guiStr;
		};
	};
};};
