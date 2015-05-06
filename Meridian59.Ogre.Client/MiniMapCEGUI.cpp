#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	MiniMapCEGUI::MiniMapCEGUI(::Meridian59::Data::DataController^ Data, int Width, int Height, CLRReal Zoom)
		: MiniMap<::System::Drawing::Bitmap^>(Data, Width, Height, Zoom) 
	{
		// default background
        backgroundColor = Color::Transparent;

		wallPen = gcnew Pen(Color::Black, 1.0f);
		purplePen1 = gcnew Pen(Color::Purple, 1.0f);
        purpleBrush = gcnew SolidBrush(Color::Purple);
        redBrush = gcnew SolidBrush(Color::Red);
        blueBrush = gcnew SolidBrush(Color::Blue);
        greenBrush = gcnew SolidBrush(Color::Green);
        orangeBrush = gcnew SolidBrush(Color::Orange);

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
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_WRITE_ONLY);
		
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
		//g->InterpolationMode = InterpolationMode;
	
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
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_WRITE_ONLY);

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
        g->DrawLine(wallPen, (float)x1, (float)y1, (float)x2, (float)y2);
	};

	void MiniMapCEGUI::DrawObject(RoomObject^ RoomObject, CLRReal x, CLRReal y, CLRReal width, CLRReal height)
	{		
		// skip invisible ones
        if (RoomObject->Flags->Drawing == ObjectFlags::DrawingType::Invisible)
            return;

        if (RoomObject->Flags->IsEnemy)
        {
            // guildenemy
            g->FillEllipse(orangeBrush, (float)x, (float)y, (float)width, (float)width);
        }

        else if (RoomObject->Flags->IsGuildMate)
        {
            // guildmate
            g->FillEllipse(greenBrush, (float)x, (float)y, (float)width, (float)width);
        }

        else if (RoomObject->Flags->IsPlayer)
        {
            // player
            g->FillEllipse(blueBrush, (float)x, (float)y, (float)width, (float)width);
        }

        else if (RoomObject->Flags->IsAttackable)
        {
            // attackable: red
            g->FillEllipse(redBrush, (float)x, (float)y, (float)width, (float)width);
        }       
	};

	void MiniMapCEGUI::DrawAvatar(RoomObject^ RoomObject, V2 P1, V2 P2, V2 P3)
	{
		playerArrowPts[0] = PointF(P1.X, P1.Y);
		playerArrowPts[1] = PointF(P2.X, P2.Y);
		playerArrowPts[2] = PointF(P3.X, P3.Y);

		g->FillPolygon(purpleBrush, playerArrowPts);
	};

	void MiniMapCEGUI::FinishDraw()
	{
		// unlock texture buffer
		texture->getBuffer()->unlock();
	};
};};
