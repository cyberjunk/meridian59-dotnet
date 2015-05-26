#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	MiniMapCEGUI::MiniMapCEGUI(::Meridian59::Data::DataController^ Data, int Width, int Height, CLRReal Zoom)
		: MiniMap<::System::Drawing::Bitmap^>(Data, Width, Height, Zoom) 
	{
		// default background
        backgroundColor = Color::Transparent;

		penWall = gcnew Pen(Color::Black, 1.0f);

		brushPlayer		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_PLAYER));
		brushObject		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_OBJECT));
		brushFriend		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_FRIEND));
		brushEnemy		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_ENEMY));
		brushGuildMate	= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_GUILDMATE));

#ifndef VANILLA
		brushMinion		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_MINION));
		brushMinionOther= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_MINION_OTH));
		brushBuildGroup = gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_BUILDGRP));
		brushNPC		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_NPC));
		brushTempSafe	= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_TEMPSAFE));
		brushMiniBoss	= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_MINIBOSS));
		brushBoss		= gcnew SolidBrush(::System::Drawing::Color::FromArgb(MiniMap::COLOR_MAP_BOSS));
#endif
		
		playerArrowPts = gcnew array<::System::Drawing::PointF>(3);
	};

	void MiniMapCEGUI::SetDimension(int Width, int Height)
	{
		MiniMap::SetDimension(Width, Height);

		// managers
		::CEGUI::ImageManager* imgMan = CEGUI::ImageManager::getSingletonPtr();
		::Ogre::TextureManager* texMan = TextureManager::getSingletonPtr();
		
		// remove image
		if (imgMan->isDefined(UI_MINIMAP_TEXNAME))
			imgMan->destroy(UI_MINIMAP_TEXNAME);

		// remove texture
		if (ControllerUI::Renderer->isTextureDefined(UI_MINIMAP_TEXNAME))									
			ControllerUI::Renderer->destroyTexture(UI_MINIMAP_TEXNAME);

		// remove old
		if (texMan->resourceExists(UI_MINIMAP_TEXNAME))
			texMan->remove(UI_MINIMAP_TEXNAME);

		if (TextureName)
			delete TextureName;

		// create new texture info
		TextureName = StringConvert::CLRToCEGUIPtr(UI_MINIMAP_TEXNAME);
		
		// create manual (empty) texture
		TexturePtr texPtr = texMan->createManual(
            UI_MINIMAP_TEXNAME,
            UI_RESGROUP_IMAGESETS,
            TextureType::TEX_TYPE_2D,
			(unsigned short)Width, (unsigned short)Height, MIP_DEFAULT,
            ::Ogre::PixelFormat::PF_A8R8G8B8,
			TU_DEFAULT, 0, false, 0);
		
		// save reference
		texture = texPtr.get();
		
		// lock the texturebuffer
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_DISCARD);
		
		// save the pointer for later comparison
		// we must adjust the gdi bitmap and graphics
		texbuf = texBuffer;

		// (re)create gdi bitmap and graphics for drawing in the texture buffer
		RecreateImageAndGraphics();

		// make ogre texture available as texture & image in CEGUI
		Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);

		// release texture buffer for now (relock in prepare)
		texture->getBuffer()->unlock();
	};
		
	void MiniMapCEGUI::RecreateImageAndGraphics()
	{
		if (Image)
			delete Image;

		if (g)
			delete g;

		// create bitmap on the texture buffer
		Image = gcnew Bitmap(
			(int)Width, 
			(int)Height, 
			(int)Width * (int)ImageComposerCEGUI<ObjectBase^>::BYTESPERPIXEL, 
			System::Drawing::Imaging::PixelFormat::Format32bppArgb, 
			(System::IntPtr)texbuf);

		// initialize the Drawing object
		g = Graphics::FromImage(Image);
		g->InterpolationMode = InterpolationMode::HighQualityBicubic;
		g->SmoothingMode = SmoothingMode::HighQuality;

		// create pie clipping
		GraphicsPath^ gpath = gcnew GraphicsPath();
		gpath->AddPie(
			UI_MINIMAP_CLIPPADDING, 
			UI_MINIMAP_CLIPPADDING, 
			Width - (2 * UI_MINIMAP_CLIPPADDING), 
			Height - (2 * UI_MINIMAP_CLIPPADDING), 
			0, 360);
        
		gpath->CloseFigure();

		// set pie clipping on graphics
		g->Clip = gcnew Region(gpath);
	};

	void MiniMapCEGUI::PrepareDraw()
	{
		// lock the texturebuffer (released in finish)
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_DISCARD);
		
		// check if buffer was moved
		if (texBuffer != texbuf)
		{
			// update
			texbuf = texBuffer;

			// recreate gdi stuff on new buffer loc
			RecreateImageAndGraphics();		
		}

		// clear 
		g->Clear(backgroundColor);
	};

	void MiniMapCEGUI::DrawWall(RooWall^ Wall, CLRReal x1, CLRReal y1, CLRReal x2, CLRReal y2)
	{
		// draw
        g->DrawLine(penWall, (float)x1, (float)y1, (float)x2, (float)y2);
	};

	void MiniMapCEGUI::DrawObject(RoomObject^ RoomObject, CLRReal x, CLRReal y, CLRReal width, CLRReal height)
	{		
		// skip invisible ones
        if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Invisible)
            return;

		/**********************************************************************************/

#ifndef VANILLA
		if (RoomObject->Flags->IsMinimapMinionSelf)
			g->FillEllipse(brushMinion, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsMinimapMinionOther)
			g->FillEllipse(brushMinionOther, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsMinimapBoss)
			g->FillEllipse(brushBoss, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsMinimapMiniBoss)
			g->FillEllipse(brushMiniBoss, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsMinimapBuilderGroup)
			g->FillEllipse(brushBuildGroup, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsMinimapNPC)
			g->FillEllipse(brushNPC, (float)x, (float)y, (float)width, (float)width);

		else
#endif
		if (RoomObject->Flags->IsMinimapEnemy)
            g->FillEllipse(brushEnemy, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsMinimapGuildMate)
            g->FillEllipse(brushFriend, (float)x, (float)y, (float)width, (float)width);
#ifndef VANILLA
		else if (RoomObject->Flags->IsMinimapTempSafe)
			g->FillEllipse(brushTempSafe, (float)x, (float)y, (float)width, (float)width);
#endif
		else if (RoomObject->Flags->IsPlayer)
			g->FillEllipse(brushPlayer, (float)x, (float)y, (float)width, (float)width);

		else if (RoomObject->Flags->IsAttackable)
			g->FillEllipse(brushEnemy, (float)x, (float)y, (float)width, (float)width);
	};

	void MiniMapCEGUI::DrawAvatar(RoomObject^ RoomObject, V2 P1, V2 P2, V2 P3)
	{
		playerArrowPts[0] = PointF(P1.X, P1.Y);
		playerArrowPts[1] = PointF(P2.X, P2.Y);
		playerArrowPts[2] = PointF(P3.X, P3.Y);

		g->FillPolygon(brushPlayer, playerArrowPts);
	};

	void MiniMapCEGUI::FinishDraw()
	{
		// unlock texture buffer
		texture->getBuffer()->unlock();
	};
};};
