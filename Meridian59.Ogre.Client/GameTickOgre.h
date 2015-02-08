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
#pragma managed(pop)

namespace Meridian59 { namespace Ogre
{
	/// <summary>
    /// Extends the base GameTick class
    /// </summary>
	public ref class GameTickOgre : public ::Meridian59::Common::GameTick
	{
	protected:
		long long chatUpdate;
		
	public:
		unsigned int INTERVALCHATUPDATE;
		
		GameTickOgre() : GameTick()
		{
			INTERVALCHATUPDATE = 500;
		};

		/// <summary>
        /// Tick we last did an update of the chat.
        /// </summary>
		property long long ChatUpdate 
		{ 
			public: long long get() { return chatUpdate; } 
			protected: void set(long long value) { chatUpdate = value; }
		};

		/// <summary>
        /// Milliseconds elapsed since last chat update.
        /// Calculated on-the-fly.
        /// </summary>
        property long long SpanChatUpdate
		{ 
			public: long long get() { return Current - ChatUpdate; } 
		};
	
		/// <summary>
        /// Call this to know if you can update the chatlog
        /// </summary>
        /// <returns></returns>
        bool CanChatUpdate()
        {
            return SpanChatUpdate >= INTERVALCHATUPDATE;
        }

		/// <summary>
        /// Call this when you did a TPS measuring
        /// </summary>
        void DidChatUpdate()
        {
            ChatUpdate = Current;
        }
	};
};};