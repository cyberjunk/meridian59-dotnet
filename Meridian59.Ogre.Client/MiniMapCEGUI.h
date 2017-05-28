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
#include "OgreTexture.h"
#include "CEGUI/String.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre 
{
   using namespace ::Ogre;
   using namespace Meridian59::Drawing2D;
   using namespace Meridian59::Data::Models;
   using namespace Meridian59::Files::ROO;
   using namespace System::Drawing;
   using namespace System::Drawing::Drawing2D;
   using namespace System::Drawing::Imaging;

   /// <summary>
      /// Implements MiniMap class from core library
      /// </summary>
   public ref class MiniMapCEGUI
   {
   public:
      /// <summary>
      /// Minimal internal map data of a wall
      /// </summary>
      value class MapWall
      {
      public:
         V2 S, E;

         void UpdateFromModel(RooWall^ Wall)
         {
            // save locations
            S = Wall->P1;
            E = Wall->P2;

            // convert from ROO to world scale
            S.ConvertToWorld();
            E.ConvertToWorld();
         }
      };

      /// <summary>
      /// Minimal internal map data of a roomobject
      /// </summary>
      value class MapObject
      {
      public:
         V2 P;
         ObjectFlags^ Flags;

         void UpdateFromModel(RoomObject^ RoomObject)
         {
            P = RoomObject->Position2D.Clone();
            Flags->UpdateFromModel(RoomObject->Flags, false);
         }
      };

      //                                      AARRGGBB
      literal uint COLOR_MAP_WALL         = 0xFF000000; //PALETTERGB(0, 0, 0)
      literal uint COLOR_MAP_PLAYER       = 0xFF0000FF; //PALETTERGB(0, 0, 255)
      literal uint COLOR_MAP_PLAYER_FRONT = 0xFF000000; //PALETTERGB(0, 0, 0)      // Pixel at front of player
      literal uint COLOR_MAP_OBJECT       = 0xFFFF0000; //PALETTERGB(255, 0, 0)    // Red
      literal uint COLOR_MAP_FRIEND       = 0xFF00FF78; //PALETTERGB(0, 255, 120)  // Green with blue tint
      literal uint COLOR_MAP_ENEMY        = 0xFFFF0000; //PALETTERGB(255, 0, 0)    // Red
      literal uint COLOR_MAP_GUILDMATE    = 0xFFFFFF00; //PALETTERGB(255, 255, 0)  // Yellow
#if !VANILLA        
      literal uint COLOR_MAP_MINION       = 0xFF00C800; //PALETTERGB(0, 200, 0)    // Green
      literal uint COLOR_MAP_MINION_OTH   = 0xFF460582; //PALETTERGB(70,5,130)     // Purple
      literal uint COLOR_MAP_BUILDGRP     = 0xFF00FF00; //PALETTERGB(0, 255, 0)    // Bright Green
      literal uint COLOR_MAP_NPC          = 0xFF000000; //PALETTERGB(0, 0, 0)      // Black
      literal uint COLOR_MAP_TEMPSAFE     = 0xFF00AAFF; //PALETTERGB(0,170,255)    // Cyan
      literal uint COLOR_MAP_MINIBOSS     = 0xFFA042C2; //PALETTERGB(160, 66, 194) // Purple
      literal uint COLOR_MAP_BOSS         = 0xFF7F0000; //PALETTERGB(127, 0, 0)    // Dark Red
      literal uint COLOR_MAP_RARE_ITEM    = 0xFFEDFF09; //PALETTERGB(237, 255, 9)  // Orange
      literal uint COLOR_MAP_NO_PVP       = 0xFFFFFFFF; //PALETTERGB(255,255,255)  // White
      literal uint COLOR_MAP_AGGRO_SELF   = 0xFF000000; //PALETTERGB(0, 0, 0)      // Black
      literal uint COLOR_MAP_AGGRO_OTHER  = 0xFFFFFFFF; //PALETTERGB(255,255,255)  // White
      literal uint COLOR_MAP_MERCENARY    = 0xFFFFA91B; //PALETTERGB(255, 169, 27) // Gold
#endif

      literal int       MAXWALLS       = 2048;
      literal int       MAXOBJECTS     = 256;
      literal CLRReal   DEFAULTZOOM    = 4.0f;
      literal CLRReal   MINZOOM        = 0.05f;
      literal CLRReal   MAXZOOM        = 20.0f;
      literal int       DEFAULTWIDTH   = 256;
      literal int       DEFAULTHEIGHT  = 256;

      /// <summary>
      /// Fired when there is an update image to show
      /// </summary>
      static event ::System::EventHandler^ ImageChanged;

   protected:

      static ::System::Threading::Thread^ thread;
      static ::System::Drawing::Graphics^ g;
      static void* imgMem;

      // INTERNAL
      static CLRReal zoom;
      static CLRReal width;
      static CLRReal height;

      // EXTERNAL UPDATES
      static volatile CLRReal zoomNew;
      static volatile CLRReal widthNew;
      static volatile CLRReal heightNew;


      static volatile bool isImageReady;
      //static volatile bool isRecreateGraphics;

      static ::System::Object^ locker = gcnew ::System::Object();
      static array<MapWall>^ mapWalls = gcnew array<MapWall>(MAXWALLS);
      static array<MapObject>^ mapObjects = gcnew array<MapObject>(MAXOBJECTS);
      static int countWalls;
      static int countObjects;
      static MapObject avatarObject = MapObject();
      static Real avatarAngle;

      static array<::System::Drawing::PointF>^ playerArrowPts;

      static ::System::Drawing::Bitmap^ Image;

      static ::System::Drawing::Pen^ penWall;

      static ::System::Drawing::SolidBrush^ brushPlayer;
      static ::System::Drawing::SolidBrush^ brushObject;
      static ::System::Drawing::SolidBrush^ brushFriend;
      static ::System::Drawing::SolidBrush^ brushEnemy;
      static ::System::Drawing::SolidBrush^ brushGuildMate;
#if !VANILLA
      static ::System::Drawing::SolidBrush^ brushMinion;
      static ::System::Drawing::SolidBrush^ brushMinionOther;
      static ::System::Drawing::SolidBrush^ brushBuildGroup;
      static ::System::Drawing::SolidBrush^ brushNPC;
      static ::System::Drawing::SolidBrush^ brushTempSafe;
      static ::System::Drawing::SolidBrush^ brushMiniBoss;
      static ::System::Drawing::SolidBrush^ brushBoss;
      static ::System::Drawing::SolidBrush^ brushItem;
      static ::System::Drawing::SolidBrush^ brushNonPvP;
      static ::System::Drawing::SolidBrush^ brushAggroSelf;
      static ::System::Drawing::SolidBrush^ brushAggroOther;
      static ::System::Drawing::SolidBrush^ brushMercenary;
#endif

      static void RecreateImageAndGraphics();
      static void ThreadProc();

      inline static void DrawObject(MapObject% RoomObject, float x, float y, float width, float height);
      inline static void DrawObjectOutter(MapObject% RoomObject, float x, float y, float width, float height);

   public:
      static bool IsRunning;
      static int NewWidth;
      static int NewHeight;

      static void Initialize(int Width, int Height, CLRReal Zoom);
      static void Tick(double Tick, double Span);
      static void SetMapData(::System::Collections::Generic::IEnumerable<RooWall^>^ Walls);
      static void SetMapData(::System::Collections::Generic::IEnumerable<RoomObject^>^ Objects);
      static void SetDimension(int Width, int Height);

      inline static void SetZoom(CLRReal value) { zoomNew = value; }
      inline static CLRReal GetZoom() { return zoom; }
   };

};};
