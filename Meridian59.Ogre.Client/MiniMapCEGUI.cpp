#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
   void MiniMapCEGUI::Initialize()
   {
      // mark running
      IsRunning = true;

      // start own workthread
      thread = gcnew ::System::Threading::Thread(gcnew ::System::Threading::ThreadStart(&MiniMapCEGUI::ThreadProc));
      thread->IsBackground = true;
      thread->Start();
   };

   void MiniMapCEGUI::DrawObjectOutter(Token^ Token, MapObject% RoomObject, float x, float y, float width, float height)
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
            Token->gdi->FillEllipse(brushBuildGroup, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapFriend)
            Token->gdi->FillEllipse(brushFriend, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapEnemy)
            Token->gdi->FillEllipse(brushEnemy, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapGuildMate)
            Token->gdi->FillEllipse(brushGuildMate, x, y, width, width);
      }
      else
      {
         if (RoomObject.Flags->IsMinimapAggroSelf)
            Token->gdi->FillEllipse(brushAggroSelf, x, y, width, width);

         else if (RoomObject.Flags->IsMinimapAggroOther)
            Token->gdi->FillEllipse(brushAggroOther, x, y, width, width);
      }
#endif
   };

   void MiniMapCEGUI::DrawObject(Token^ Token, MapObject% RoomObject, float x, float y, float width, float height)
   {
      // skip invisible ones
      if (RoomObject.Flags->Drawing == ObjectFlags::DrawingType::Invisible)
         return;

#ifndef VANILLA
      /**********************************************************************************/
      // MERIDIANNEXT

      // draw inner
      if (RoomObject.Flags->IsMinimapPlayer)
         Token->gdi->FillEllipse(brushPlayer, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapTempSafe)
         Token->gdi->FillEllipse(brushTempSafe, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMinionSelf)
         Token->gdi->FillEllipse(brushMinion, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMinionOther)
         Token->gdi->FillEllipse(brushMinionOther, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMercenary)
         Token->gdi->FillEllipse(brushMercenary, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMonster)
         Token->gdi->FillEllipse(brushObject, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapNPC)
         Token->gdi->FillEllipse(brushNPC, x, y, width, width);

      else if (RoomObject.Flags->IsRareItem)
         Token->gdi->FillEllipse(brushItem, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapMiniBoss)
         Token->gdi->FillEllipse(brushMiniBoss, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapBoss)
         Token->gdi->FillEllipse(brushBoss, x, y, width, width);

      else if (RoomObject.Flags->IsNonPvP)
         Token->gdi->FillEllipse(brushNonPvP, x, y, width, width);

#else
      /**********************************************************************************/
      // VANILLA

      if (RoomObject.Flags->IsMinimapEnemy)
         Token->gdi->FillEllipse(brushEnemy, x, y, width, width);

      else if (RoomObject.Flags->IsMinimapGuildMate)
         Token->gdi->FillEllipse(brushFriend, x, y, width, width);

      else if (RoomObject.Flags->IsPlayer)
         Token->gdi->FillEllipse(brushPlayer, x, y, width, width);

      else if (RoomObject.Flags->IsAttackable)
         Token->gdi->FillEllipse(brushEnemy, x, y, width, width);
#endif
   };

   void MiniMapCEGUI::Tick(::CEGUI::Window* Window, ::CEGUI::Window* Surface, CLRReal Zoom, IEnumerable<RoomObject^>^ Objects)
   {
      Token^ token;

      // get window size and position
      // this is the container, not the drawsurface!
      ::CEGUI::USize    size = Window->getSize();
      ::CEGUI::UVector2 position = Window->getPosition();

      // ogre texture manager
      ::Ogre::TextureManager& texMan = TextureManager::getSingleton();

      // try to get the minimap texture
      TexturePtr texPtr = texMan.getByName(UI_MINIMAP_TEXNAME, UI_RESGROUP_IMAGESETS);

      // must create or recreate texture because window size was changed
      if (!texPtr ||
         texPtr->getWidth() != (uint32)size.d_width.d_offset ||
         texPtr->getHeight() != (uint32)size.d_height.d_offset)
      {
         // cegui imager manager
         ::CEGUI::ImageManager&  imgMan = CEGUI::ImageManager::getSingleton();

         // IMPORTANT: unset old texture
         Surface->setProperty(UI_PROPNAME_IMAGE, STRINGEMPTY);

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
         // beware: TU_DYNAMIC_WRITE_ONLY_DISCARDABLE seems to auto create multiple of 32 row-widths
         texPtr = texMan.createManual(
            UI_MINIMAP_TEXNAME,
            UI_RESGROUP_IMAGESETS,
            TextureType::TEX_TYPE_2D,
            (uint32)size.d_width.d_offset, 
            (uint32)size.d_height.d_offset, 
            0, ::Ogre::PixelFormat::PF_A8R8G8B8,
            TU_DYNAMIC_WRITE_ONLY_DISCARDABLE, 0, false, 0);

         // make ogre texture available as texture & image in CEGUI
         if (texPtr)
            Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
      }

      // got a token back from drawer, use it
      if (queueOut->TryDequeue(token))
      {
         // only use tokens if matches our texture
         if (texPtr &&
             (uint32)token->width  == texPtr->getWidth() &&
             (uint32)token->height == texPtr->getHeight())
         {
            // get pointer to pixelbuf
            HardwarePixelBufferSharedPtr pixPtr = texPtr->getBuffer();

            // create pixelbox of img mem
            PixelBox box = PixelBox(
               (uint32)token->width,
               (uint32)token->height,
               1, ::Ogre::PixelFormat::PF_A8R8G8B8,
               token->mem);

            // use fastest discard locking
            ::Ogre::PixelBox pixBox = pixPtr->lock(box, 
               ::Ogre::HardwareBuffer::LockOptions::HBL_DISCARD);

            // width * height * ARGB
            const uint32 numBytes = (uint32)token->width * (uint32)token->height * 4;

            // plain copy pixels from source into texture
            memcpy(pixBox.data, box.data, numBytes);

            // unlock
            pixPtr->unlock();

            // set/refresh texture
            Surface->setProperty(UI_PROPNAME_IMAGE, UI_MINIMAP_TEXNAME);
         }

         // update token data
         token->Update(
            (int)size.d_width.d_offset, 
            (int)size.d_height.d_offset, 
            Zoom, Objects);

         // reuse the token with updated values
         queueIn->Enqueue(token);
      }
      else if (countTokens < MAXTOKENS)
      {
         // create new token
         token = gcnew Token(
            (int)size.d_width.d_offset,
            (int)size.d_height.d_offset,
            Zoom, Objects);

         // give it to worker
         queueIn->Enqueue(token);

         // raise count of created tokens
         countTokens++;
      }
   };

   void MiniMapCEGUI::ThreadProc()
   {
      BoundingBox2D scope = BoundingBox2D();

      // set priority one below normal
      ::System::Threading::Thread::CurrentThread->Priority = 
         ::System::Threading::ThreadPriority::BelowNormal;
      
      while (IsRunning)
      {
         // get start tick
         double tickStart = GameTick::GetUpdatedTick();

         //////////////////////////////////////////////////////////////////
         
         Token^ token;

         // no new data, sleep a bit then check again
         if (!queueIn->TryDequeue(token))
         {
            ::System::Threading::Thread::Sleep(1);
            continue;
         }

         // get values from token
         CLRReal width = token->width;
         CLRReal height = token->height;
         CLRReal zoom = token->zoom;
         CLRReal zoomInv = 1.0f / zoom;

         // get the deltas based on zoom, zoombase and mapsize
         // the center of the bounding box is the player position
         V2 delta = V2(
            0.5f * zoom * width,
            0.5f * zoom * height);

         // clear pixels
         token->ClearPixels();

         // update box boundaries (box = what to draw from map)
         scope.Min.X = token->avatarObject.P.X - delta.X;
         scope.Min.Y = token->avatarObject.P.Y - delta.Y;
         scope.Max.X = token->avatarObject.P.X + delta.X;
         scope.Max.Y = token->avatarObject.P.Y + delta.Y;

         /***************************************************************************/
         // enter walls lock (set in SetData())
         ::System::Threading::Monitor::Enter(locker);
         
         try
         {
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
               token->gdi->DrawLine(penWall, (float)trans1.X, (float)trans1.Y, (float)trans2.X, (float)trans2.Y);
            }
         }
         finally { ::System::Threading::Monitor::Exit(locker); }

         /***************************************************************************/
         // draw roomobjects
         for (int i = 0; i < token->countObjects; i++)
         {
            MapObject% obj = token->mapObjects[i];

            // transform
            V2 trans;
            trans.X = (obj.P.X - scope.Min.X) * zoomInv;
            trans.Y = (obj.P.Y - scope.Min.Y) * zoomInv;

            if (!obj.Equals(token->avatarObject))
            {
               CLRReal width = 10.0f;
               CLRReal widthhalf = width * 0.5f;
               CLRReal rectx = trans.X - widthhalf;
               CLRReal recty = trans.Y - widthhalf;

               DrawObjectOutter(token, obj, (float)rectx, (float)recty, (float)width, (float)width);

               //

               width = 6.0f;
               widthhalf = width * 0.5f;
               rectx = trans.X - widthhalf;
               recty = trans.Y - widthhalf;

               DrawObject(token, obj, (float)rectx, (float)recty, (float)width, (float)width);
            }
            else
            {
               V2% line1 = MathUtil::GetDirectionForRadian(token->avatarAngle);
               line1.Scale(8.0f);

               V2% line2 = line1.Clone();
               V2% line3 = line1.Clone();

               line2.Rotate(GeometryConstants::HALFPERIOD - 0.5f);
               line3.Rotate(-GeometryConstants::HALFPERIOD + 0.5f);

               V2% P1 = trans + line1;
               V2% P2 = trans + line2;
               V2% P3 = trans + line3;

               playerArrowPts[0] = PointF((float)P1.X, (float)P1.Y);
               playerArrowPts[1] = PointF((float)P2.X, (float)P2.Y);
               playerArrowPts[2] = PointF((float)P3.X, (float)P3.Y);

               token->gdi->FillPolygon(brushPlayer, playerArrowPts);
            }
         }

         // enqueue result
         queueOut->Enqueue(token);

         //////////////////////////////////////////////////////////////////

         // calculate exec time
         double tickEnd  = GameTick::GetUpdatedTick();
         double tickSpan = tickEnd - tickStart;

         // add exec time on sum and raise counter
         tpsSumTick += tickSpan;
         tpsCounter++;

         // possibly adjust max min
         if (tickSpan < TickBest)
            TickBest = tickSpan;

         else if (tickSpan > TickWorst)
            TickWorst = tickSpan;

         // since we last updated stats and reset
         double spanMeasure = tickEnd - tpsMeasureTick;

         // time to update avg and reset
         if (spanMeasure > 1000.0 && tpsCounter > 0.0)
         {
            // calc avg
            TickAvg = tpsSumTick / tpsCounter;

            // reset
            tpsMeasureTick = tickEnd;
            tpsSumTick = 0.0;
            tpsCounter = 0;

            // init best/worst with this one
            TickBest = tickSpan;
            TickWorst = tickSpan;
         }
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
};};
