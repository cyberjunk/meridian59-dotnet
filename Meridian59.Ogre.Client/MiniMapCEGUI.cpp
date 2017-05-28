#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
   void MiniMapCEGUI::Initialize(int Width, int Height, CLRReal Zoom)
   {
      penWall = gcnew Pen(Color::Black, 1.0f);

      brushPlayer    = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_PLAYER));
      brushObject    = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_OBJECT));
      brushFriend    = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_FRIEND));
      brushEnemy     = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_ENEMY));
      brushGuildMate = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_GUILDMATE));

#ifndef VANILLA
      brushMinion       = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_MINION));
      brushMinionOther  = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_MINION_OTH));
      brushBuildGroup   = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_BUILDGRP));
      brushNPC          = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_NPC));
      brushTempSafe     = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_TEMPSAFE));
      brushMiniBoss     = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_MINIBOSS));
      brushBoss         = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_BOSS));
      brushItem         = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_RARE_ITEM));
      brushNonPvP       = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_NO_PVP));
      brushAggroSelf    = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_AGGRO_SELF));
      brushAggroOther   = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_AGGRO_OTHER));
      brushMercenary    = gcnew SolidBrush(::System::Drawing::Color::FromArgb(COLOR_MAP_MERCENARY));
#endif

      playerArrowPts = gcnew array<::System::Drawing::PointF>(3);

      MiniMapCEGUI::width = (CLRReal)Width;
      MiniMapCEGUI::height = (CLRReal)Height;
      MiniMapCEGUI::zoom = Zoom;
      MiniMapCEGUI::zoomInv = 1.0f / zoom;

      // must create class instances manually
      for (int i = 0; i < mapObjects->Length; i++)
         mapObjects[i].Flags = gcnew ObjectFlags(0, ObjectFlags::DrawingType::Plain, 0, 0, ObjectFlags::PlayerType::None, ObjectFlags::MoveOnType::Yes);

      // start own workthread
      thread = gcnew ::System::Threading::Thread(gcnew ::System::Threading::ThreadStart(&MiniMapCEGUI::ThreadProc));
      thread->IsBackground = true;
      thread->Start();
   };

   void MiniMapCEGUI::SetDimension(int Width, int Height)
   {
      // lock
      ::System::Threading::Monitor::Enter(locker);

      try
      {
         // update dimension
         MiniMapCEGUI::width = (Real)Width;
         MiniMapCEGUI::height = (Real)Height;

         // mark for redraw, force recreate of graphics and cegui tex
         isImageReady = false;
         isRecreateGraphics = true;
      }
      finally { ::System::Threading::Monitor::Exit(locker); }
   };

   void MiniMapCEGUI::RecreateImageAndGraphics()
   {
      // free bitmap
      if (Image)
         delete Image;
      
      // free graphics
      if (g)
         delete g;

      // free bitmap mem
      free(imgMem);

      // allocate new memroy
      imgMem = malloc((size_t)width * (size_t)height * 4);

      // create bitmap on the texture buffer
      Image = gcnew Bitmap(
         (int)width,
         (int)height,
         (int)width * 4,
         System::Drawing::Imaging::PixelFormat::Format32bppArgb, 
         (::System::IntPtr)imgMem);

      // initialize the Drawing object
      g = Graphics::FromImage(Image);
      g->InterpolationMode  = InterpolationMode::Bilinear;
      g->PixelOffsetMode    = PixelOffsetMode::HighSpeed;
      g->SmoothingMode      = SmoothingMode::HighQuality;
      g->CompositingMode    = CompositingMode::SourceOver;
      g->CompositingQuality = CompositingQuality::HighSpeed;

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
      g->Clip = gcnew Region(gpath);
   };

   void MiniMapCEGUI::DrawObjectOutter(MapObject% RoomObject, float x, float y, float width, float height)
   {
      // skip invisible ones
      if (RoomObject.Flags->Drawing == ObjectFlags::DrawingType::Invisible)
         return;

#ifndef VANILLA
      /**********************************************************************************/
      // MERIDIANNEXT

      // draw outter circle
      if (RoomObject.Flags->IsPlayer)
      {
         if (RoomObject.Flags->IsMinimapBuilderGroup)
            g->FillEllipse(brushBuildGroup, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapFriend)
            g->FillEllipse(brushFriend, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapEnemy)
            g->FillEllipse(brushEnemy, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapGuildMate)
            g->FillEllipse(brushGuildMate, x, y, width, width);
      }
      else
      {
         if (RoomObject.Flags->IsMinimapAggroSelf)
            g->FillEllipse(brushAggroSelf, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapAggroOther)
            g->FillEllipse(brushAggroOther, x, y, width, width);
      }
#endif
   };

   void MiniMapCEGUI::DrawObject(MapObject% RoomObject, float x, float y, float width, float height)
   {
      // skip invisible ones
      if (RoomObject.Flags->Drawing == ObjectFlags::DrawingType::Invisible)
         return;

#ifndef VANILLA
      /**********************************************************************************/
      // MERIDIANNEXT

      // draw inner
      if (RoomObject.Flags->IsMinimapPlayer)
         g->FillEllipse(brushPlayer, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapTempSafe)
         g->FillEllipse(brushTempSafe, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMinionSelf)
         g->FillEllipse(brushMinion, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMinionOther)
         g->FillEllipse(brushMinionOther, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMercenary)
         g->FillEllipse(brushMercenary, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMonster)
         g->FillEllipse(brushObject, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapNPC)
         g->FillEllipse(brushNPC, x, y, width, width);

      else if (RoomObject.Flags->IsRareItem)
         g->FillEllipse(brushItem, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMiniBoss)
         g->FillEllipse(brushMiniBoss, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapBoss)
         g->FillEllipse(brushBoss, x, y, width, width);

      else if (RoomObject.Flags->IsNonPvP)
         g->FillEllipse(brushNonPvP, x, y, width, width);

#else
      /**********************************************************************************/
      // VANILLA

      if (RoomObject.Flags->IsMinimapEnemy)
         g->FillEllipse(brushEnemy, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapGuildMate)
         g->FillEllipse(brushFriend, x, y, width, width);

      else if (RoomObject.Flags->IsPlayer)
         g->FillEllipse(brushPlayer, x, y, width, width);

      else if (RoomObject.Flags->IsAttackable)
         g->FillEllipse(brushEnemy, x, y, width, width);
#endif
   };

   void MiniMapCEGUI::Tick(double Tick, double Span)
   {
      // no new image available
      if (!isImageReady)
         return;

      //////////////////////////////////////////////////

      // try to get the new image but don't wait too long
      if (!::System::Threading::Monitor::TryEnter(locker, 1))
         return;

      try
      {
         // save new width and height
         NewWidth = (unsigned short)Image->Width;
         NewHeight = (unsigned short)Image->Height;

         // managers
         ::CEGUI::ImageManager& imgMan = CEGUI::ImageManager::getSingleton();
         ::Ogre::TextureManager& texMan = TextureManager::getSingleton();

         // try to get the minimap texture
         TexturePtr texPtr = texMan.getByName(UI_MINIMAP_TEXNAME, UI_RESGROUP_IMAGESETS);

         // recreate texture if not exist or different size
         if (texPtr.isNull() || texPtr->getSrcWidth() != NewWidth || texPtr->getSrcHeight() != NewHeight)
         {
            // remove cegui image
            if (imgMan.isDefined(UI_MINIMAP_TEXNAME))
               imgMan.destroy(UI_MINIMAP_TEXNAME);

            // remove cegui texture (wrapped)
            if (ControllerUI::Renderer->isTextureDefined(UI_MINIMAP_TEXNAME))
               ControllerUI::Renderer->destroyTexture(UI_MINIMAP_TEXNAME);

            // remove ogre tex
            if (texMan.resourceExists(UI_MINIMAP_TEXNAME))
               texMan.remove(UI_MINIMAP_TEXNAME);

            // create manual (empty) texture
            texPtr = texMan.createManual(
               UI_MINIMAP_TEXNAME,
               UI_RESGROUP_IMAGESETS,
               TextureType::TEX_TYPE_2D,
               NewWidth, NewHeight, 0,
               ::Ogre::PixelFormat::PF_A8R8G8B8,
               TU_DYNAMIC_WRITE_ONLY_DISCARDABLE, 0, false, 0);

            // make ogre texture available as texture & image in CEGUI
            if (!texPtr.isNull())
               Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
         }

         if (!texPtr.isNull())
         {
            // get pointer to pixelbuf
            HardwarePixelBufferSharedPtr pixPtr = texPtr->getBuffer();

            // create pixelbox of img mem
            PixelBox box = PixelBox(NewWidth, NewHeight, 1, ::Ogre::PixelFormat::PF_A8R8G8B8, imgMem);

            // get pixels from souce bitmap
            pixPtr->blitFromMemory(box);

            // trigger event
            ImageChanged(nullptr, gcnew ::System::EventArgs());
         }
      }
      finally { ::System::Threading::Monitor::Exit(locker); }

      //////////////////////////////////////////////////

      // flip to draw a new one
      isImageReady = false;
   };

   void MiniMapCEGUI::ThreadProc()
   {
      IsRunning = true;
      isRecreateGraphics = true;

      while (IsRunning)
      {
         // last image not yet collected, sleep
         if (isImageReady)
         {
            ::System::Threading::Thread::Sleep(33);
            continue;
         }

         // enter lock
         ::System::Threading::Monitor::Enter(locker);

         try
         {
            // recreate internal graphics and bitmap
            if (isRecreateGraphics)
            {
               RecreateImageAndGraphics();
               isRecreateGraphics = false;
            }

            // get the deltas based on zoom, zoombase and mapsize
            // the center of the bounding box is the player position
            V2 delta = V2(
               0.5f * zoom * width,
               0.5f * zoom * height);

            // update box boundaries (box = what to draw from map)
            scope.Min.X = avatarObject.P.X - delta.X;
            scope.Min.Y = avatarObject.P.Y - delta.Y;
            scope.Max.X = avatarObject.P.X + delta.X;
            scope.Max.Y = avatarObject.P.Y + delta.Y;

            // prepare drawing
            ZeroMemory(imgMem, (int)width*(int)height * 4);

            /***************************************************************************/
            // start drawing walls from roo
            for (int i = 0; i < countWalls; i++)
            {
               MapWall% rld = mapWalls[i];

               // transform wall points
               V2 trans1;
               V2 trans2;

               trans1.X = (rld.S.X - scope.Min.X) * zoomInv;
               trans1.Y = (rld.S.Y - scope.Min.Y) * zoomInv;
               trans2.X = (rld.E.X - scope.Min.X) * zoomInv;
               trans2.Y = (rld.E.Y - scope.Min.Y) * zoomInv;

               // draw wall
               g->DrawLine(penWall, (float)trans1.X, (float)trans1.Y, (float)trans2.X, (float)trans2.Y);
            }

            /***************************************************************************/
            // draw roomobjects
            for (int i = 0; i < countObjects; i++)
            {
               MapObject% obj = mapObjects[i];

               // transform
               V2 trans;
               trans.X = (obj.P.X - scope.Min.X) * zoomInv;
               trans.Y = (obj.P.Y - scope.Min.Y) * zoomInv;

               if (!obj.Equals(avatarObject))
               {
                  CLRReal width = 100.0f * zoomInv;
                  CLRReal widthhalf = width * 0.5f;
                  CLRReal rectx = trans.X - widthhalf;
                  CLRReal recty = trans.Y - widthhalf;

                  DrawObjectOutter(obj, rectx, recty, width, width);

                  //

                  width = 50.0f * zoomInv;
                  widthhalf = width * 0.5f;
                  rectx = trans.X - widthhalf;
                  recty = trans.Y - widthhalf;

                  DrawObject(obj, rectx, recty, width, width);
               }
               else
               {
                  V2 line1 = MathUtil::GetDirectionForRadian(avatarAngle) * 50.0f * zoomInv;
                  V2 line2 = line1.Clone();
                  V2 line3 = line1.Clone();

                  line2.Rotate(GeometryConstants::HALFPERIOD - 0.5f);
                  line3.Rotate(-GeometryConstants::HALFPERIOD + 0.5f);

                  V2 P1 = trans + line1;
                  V2 P2 = trans + line2;
                  V2 P3 = trans + line3;

                  playerArrowPts[0] = PointF((float)P1.X, (float)P1.Y);
                  playerArrowPts[1] = PointF((float)P2.X, (float)P2.Y);
                  playerArrowPts[2] = PointF((float)P3.X, (float)P3.Y);

                  g->FillPolygon(brushPlayer, playerArrowPts);
               }
            }
         }
         finally { ::System::Threading::Monitor::Exit(locker); }

         // mark new image being ready
         isImageReady = true;

         // sleep
         ::System::Threading::Thread::Sleep(33);
      }
   };

   void MiniMapCEGUI::SetMapData(::System::Collections::Generic::IEnumerable<RooWall^>^ Walls)
   {
      // lock shared walls data, waits until done
      // since only happening once
      ::System::Threading::Monitor::Enter(locker);

      try
      {
         // index into our internal mapdata array
         countWalls = 0;

         // iterate walls
         for each(RooWall^ o in Walls)
         {
            // reached maximum supported internal walls
            if (countWalls >= MAXWALLS)
               break;

            // Don't show line if:
            // 1) both sides not set
            // 2) left side set to not show up on map, right side unset
            // 3) right side set to not show up on map, left side unset
            // 4) both sides set and set to not show up on map
            if ((o->LeftSide == nullptr && o->RightSide == nullptr) ||
               (o->LeftSide != nullptr && o->RightSide == nullptr && o->LeftSide->Flags->IsMapNever) ||
               (o->LeftSide == nullptr && o->RightSide != nullptr && o->RightSide->Flags->IsMapNever) ||
               (o->LeftSide != nullptr && o->LeftSide != nullptr && o->LeftSide->Flags->IsMapNever && o->RightSide->Flags->IsMapNever))
               continue;

            // set mapwall data
            mapWalls[countWalls].UpdateFromModel(o);

            // increment internal index
            countWalls++;
         }
      }
      finally { ::System::Threading::Monitor::Exit(locker); }
   };

   void MiniMapCEGUI::SetMapData(::System::Collections::Generic::IEnumerable<RoomObject^>^ Objects)
   {
      // skip update if painter is busy right now
      if (!::System::Threading::Monitor::TryEnter(locker, 1))
         return;

      try
      {
         // index into our internal mapobjects array
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
      finally { ::System::Threading::Monitor::Exit(locker); }
   };

   void MiniMapCEGUI::Zoom(CLRReal value)
   {
      ::System::Threading::Monitor::Enter(locker);

      try
      {
         zoom = ::System::Math::Min(::System::Math::Max(zoom + value, MINZOOM), MAXZOOM);
         zoomInv = 1.0f / zoom;
      }
      finally { ::System::Threading::Monitor::Exit(locker); }
   };
};};
