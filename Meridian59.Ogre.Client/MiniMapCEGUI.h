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

      /// <summary>
      /// A token is handed between mainthread and worker.
      /// It has memory, a bitmap on that memory, a gdi on the bitmap
      /// and current player positions and flags attached.
      /// </summary>
      ref class Token
      {
      public:
         Graphics^         gdi;
         Bitmap^           bitmap;
         void*             mem;
         CLRReal           width;
         CLRReal           height;
         CLRReal           zoom;
         array<MapObject>^ mapObjects;
         int               countObjects;
         MapObject         avatarObject;
         Real              avatarAngle;

         inline Token(int Width, int Height, CLRReal Zoom, IEnumerable<RoomObject^>^ Objects)
         {
            // create array for objectdata to draw
            mapObjects   = gcnew array<MapObject>(MAXOBJECTS);
            countObjects = 0;

            // must create class instances manually
            for (int i = 0; i < mapObjects->Length; i++)
            {
               mapObjects[i].Flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0,
                  ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);
            }

            // update to values
            Update(Width, Height, Zoom, Objects);
         };

         inline ~Token()
         {
            // free bitmap
            if (bitmap)
               delete bitmap;

            // free graphics
            if (gdi)
               delete gdi;

            // free bitmap mem
            free(mem);
         };

         inline void Update(int Width, int Height, CLRReal Zoom, IEnumerable<RoomObject^>^ Objects)
         {
            // update zoom
            this->zoom = Zoom;
            
            // need to recreate gdi+ bitmap and graphics
            if (width != Width || height != Height ||!mem || !bitmap || !gdi)
            {
               // save dimensions
               this->width = (CLRReal)Width;
               this->height = (CLRReal)Height;

               // possible release old imgMem
               free(mem);

               // same with image
               if (bitmap)
                  delete bitmap;

               // and graphics
               if (gdi)
                  delete gdi;

               // allocate new memroy
               mem = malloc((size_t)width * (size_t)height * 4);

               // create bitmap on own memory
               bitmap = gcnew Bitmap(
                  (int)width,
                  (int)height,
                  (int)width * 4,
                  System::Drawing::Imaging::PixelFormat::Format32bppArgb,
                  (::System::IntPtr)mem);

               // initialize the gdi+ drawing object on own bitmap on own memory
               gdi = Graphics::FromImage(bitmap);
               gdi->InterpolationMode = InterpolationMode::HighQualityBicubic;
               gdi->PixelOffsetMode = PixelOffsetMode::HighQuality;
               gdi->SmoothingMode = SmoothingMode::HighQuality;
               gdi->CompositingMode = CompositingMode::SourceOver;
               gdi->CompositingQuality = CompositingQuality::HighQuality;

               // create pie clipping
               GraphicsPath^ gpath = gcnew GraphicsPath();
               gpath->AddPie(
                  UI_MINIMAP_CLIPPADDING * (float)width,
                  UI_MINIMAP_CLIPPADDING * (float)height,
                  (float)width - (2.0f * UI_MINIMAP_CLIPPADDING * (float)width),
                  (float)height - (2.0f * UI_MINIMAP_CLIPPADDING * (float)height),
                  0.0f, 360.0f);

               gpath->CloseFigure();

               // set pie clipping on graphics
               gdi->Clip = gcnew Region(gpath);
            }

            // now update roomobject positions
            countObjects = 0;

            // iterate roomobjects
            for each(RoomObject^ o in Objects)
            {
               // reached maximum supported internal objects
               if (countObjects >= MAXOBJECTS)
                  break;

               // set object data
               mapObjects[countObjects].UpdateFromModel(o);

               // check for avatar
               if (o->IsAvatar)
               {
                  avatarObject = mapObjects[countObjects];
                  avatarAngle = o->Angle;
               }

               // increment index
               countObjects++;
            }
         }

         inline void ClearPixels()
         {
            ZeroMemory(mem, (int)width*(int)height * 4);
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
      literal int       DEFAULTWIDTH   = 256;
      literal int       DEFAULTHEIGHT  = 256;
      literal int       MAXTOKENS      = 4;

   protected:
      static ::System::Threading::Thread^ thread;
      static LockingQueue<Token^>^        queueOut       = gcnew LockingQueue<Token^>();
      static LockingQueue<Token^>^        queueIn        = gcnew LockingQueue<Token^>();
      static array<PointF>^               playerArrowPts = gcnew array<PointF>(3);
      static ::System::Object^            locker         = gcnew ::System::Object();
      static array<MapWall>^              mapWalls       = gcnew array<MapWall>(MAXWALLS);
      
      static double tpsMeasureTick;
      static double tpsCounter;
      static double tpsSumTick;

      // tracking counts of used walls and tokens
      static int countWalls;
      static int countTokens;

      // wall line pen
      static Pen^ penWall = gcnew Pen(Color::Black, 2.0f);

      // brushes for objects
      static SolidBrush^ brushPlayer      = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_PLAYER));
      static SolidBrush^ brushObject      = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_OBJECT));
      static SolidBrush^ brushFriend      = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_FRIEND));
      static SolidBrush^ brushEnemy       = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_ENEMY));
      static SolidBrush^ brushGuildMate   = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_GUILDMATE));
#ifndef VANILLA
      static SolidBrush^ brushMinion      = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_MINION));
      static SolidBrush^ brushMinionOther = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_MINION_OTH));
      static SolidBrush^ brushBuildGroup  = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_BUILDGRP));
      static SolidBrush^ brushNPC         = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_NPC));
      static SolidBrush^ brushTempSafe    = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_TEMPSAFE));
      static SolidBrush^ brushMiniBoss    = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_MINIBOSS));
      static SolidBrush^ brushBoss        = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_BOSS));
      static SolidBrush^ brushItem        = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_RARE_ITEM));
      static SolidBrush^ brushNonPvP      = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_NO_PVP));
      static SolidBrush^ brushAggroSelf   = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_AGGRO_SELF));
      static SolidBrush^ brushAggroOther  = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_AGGRO_OTHER));
      static SolidBrush^ brushMercenary   = gcnew SolidBrush(Color::FromArgb(COLOR_MAP_MERCENARY));
#endif

      static void ThreadProc();
      inline static void DrawObject(Token^ Token, MapObject% RoomObject, float x, float y, float width, float height);
      inline static void DrawObjectOutter(Token^ Token, MapObject% RoomObject, float x, float y, float width, float height);

   public:
      static bool IsRunning = false;
      static double TickWorst = 0.0f;
      static double TickBest = 0.0f;
      static double TickAvg = 0.0f;

      static void Initialize();
      static void Tick(::CEGUI::Window* Window, ::CEGUI::Window* Surface, CLRReal Zoom, IEnumerable<RoomObject^>^ Objects);
      static void SetMapData(::System::Collections::Generic::IEnumerable<RooWall^>^ Walls);
   };
};};
