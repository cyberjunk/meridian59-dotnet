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
      double chatUpdate;
      double keyRepeat;
      double inventoryClick;
      double manualFrameRendered;

   public:
      double INTERVALCHATUPDATE;
      double INTERVALKEYREPEAT;
      double INTERVALKEYREPEATSTART;
      double INTERVALINVENTORYCLICK;
      double INTERVALMANUALFRAMERENDERED;

      GameTickOgre() : GameTick()
      {
         INTERVALCHATUPDATE      = 50.0;
         INTERVALKEYREPEAT       = 25.0;
         INTERVALKEYREPEATSTART  = 500.0;
         INTERVALINVENTORYCLICK  = 250.0;
         INTERVALMANUALFRAMERENDERED = 33.3;
      };

      /// <summary>
      /// Tick we last did an update of the chat.
      /// </summary>
      property double ChatUpdate 
      { 
         public: double get() { return chatUpdate; } 
         protected: void set(double value) { chatUpdate = value; }
      };

      /// <summary>
      /// Tick we last repeated a key hold down.
      /// </summary>
      property double KeyRepeat
      {
         public: double get() { return keyRepeat; }
         protected: void set(double value) { keyRepeat = value; }
      };

      /// <summary>
      /// Tick we last clicked an inventory item
      /// </summary>
      property double InventoryClick
      {
         public: double get() { return inventoryClick; }
         protected: void set(double value) { inventoryClick = value; }
      };

      /// <summary>
      /// Tick we last rendered a frame manually
      /// </summary>
      property double ManualFrameRendered
      {
         public: double get() { return manualFrameRendered; }
         protected: void set(double value) { manualFrameRendered = value; }
      };

      /// <summary>
      /// Milliseconds elapsed since last chat update.
      /// Calculated on-the-fly.
      /// </summary>
      property double SpanChatUpdate
      { 
         public: double get() { return Current - ChatUpdate; } 
      };

      /// <summary>
      /// Milliseconds elapsed since last repeated a key hold down
      /// Calculated on-the-fly.
      /// </summary>
      property double SpanKeyRepeat
      {
         public: double get() { return Current - KeyRepeat; }
      };

      /// <summary>
      /// Milliseconds elapsed since we last clicked on inventory item
      /// Calculated on-the-fly.
      /// </summary>
      property double SpanInventoryClick
      {
         public: double get() { return Current - InventoryClick; }
      };

      /// <summary>
      /// Milliseconds elapsed since we last rendered a frame manually
      /// Calculated on-the-fly.
      /// </summary>
      property double SpanManualFrameRendered
      {
         public: double get() { return Current - ManualFrameRendered; }
      };

      /// <summary>
      /// Call this to know if you can update the chatlog
      /// </summary>
      /// <returns></returns>
      inline bool CanChatUpdate()
      {
         return SpanChatUpdate >= INTERVALCHATUPDATE;
      }

      /// <summary>
      /// Call this to know if you can repeat a key hold down
      /// </summary>
      /// <returns></returns>
      inline bool CanKeyRepeat()
      {
         return SpanKeyRepeat >= INTERVALKEYREPEAT;
      }

      /// <summary>
      /// Call this to know if you can start repeating a key hold down
      /// </summary>
      /// <returns></returns>
      inline bool CanKeyRepeatStart()
      {
         return SpanKeyRepeat >= INTERVALKEYREPEATSTART;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      inline bool CanInventoryClick()
      {
         return SpanInventoryClick >= INTERVALINVENTORYCLICK;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      inline bool CanManualFrameRendered()
      {
         return SpanManualFrameRendered >= INTERVALMANUALFRAMERENDERED;
      }

      /// <summary>
      /// Call this when you did a Chat update
      /// </summary>
      inline void DidChatUpdate()
      {
         ChatUpdate = Current;
      }

      /// <summary>
      /// Call this when you did a keyrepeat
      /// </summary>
      inline void DidKeyRepeat()
      {
         KeyRepeat = Current;
      }

      /// <summary>
      /// Call this when you did an inventoryclick
      /// </summary>
      inline void DidInventoryClick()
      {
         InventoryClick = Current;
      }

      /// <summary>
      /// Call this when you rendered a frame manually
      /// </summary>
      inline void DidManualFrameRendered()
      {
         ManualFrameRendered = Current;
      }
   };
};};
