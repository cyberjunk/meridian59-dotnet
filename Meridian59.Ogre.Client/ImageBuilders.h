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

#define INITGUID
#pragma managed(push, off)
#include "OgreTexture.h"
#include "OgreTextureManager.h"
#include "ddraw.h"
#include <d3d9.h>
#include <d3dx9core.h> 
#pragma managed(pop)

#define CACHESURFACEWIDTH 1024
#define CACHESURFACEHEIGHT 1024
#define BGICONWIDTH 64
#define BGICONHEIGHT 64

namespace Meridian59 { namespace Ogre 
{
	public enum ImageBuilderType
	{
		GDI, DirectDraw, DirectX, Native
	};

	/// <summary>
	/// Builds Ogre Textures for legacy 2D M59 objects.
	/// </summary>
	public ref class ImageBuilder abstract sealed
	{
	private:
		
		static ImageBuilderType builderType = ImageBuilderType::GDI;
		
	public:
		/// <summary>
		///
		/// </summary>
		ref class GDI abstract sealed
		{
		private:
			static ::Ogre::Texture*				texture;
			static ::System::Drawing::Graphics^ graphics;		
			static ::System::Drawing::Bitmap^	dest;		
			static ::System::Drawing::Bitmap^	source;
			static ::System::Drawing::Bitmap^	background;

		public:		
			static ::System::Drawing::Drawing2D::InterpolationMode InterpolationMode = 
				::System::Drawing::Drawing2D::InterpolationMode::Default;

			static ::System::Drawing::Drawing2D::PixelOffsetMode PixelOffsetMode =
				::System::Drawing::Drawing2D::PixelOffsetMode::Default;
			
			static ::System::Drawing::Drawing2D::SmoothingMode SmoothingMode =
				::System::Drawing::Drawing2D::SmoothingMode::Default;
			
			static ::System::Drawing::Drawing2D::CompositingQuality CompositingQuality =
				::System::Drawing::Drawing2D::CompositingQuality::Default;
		
			static bool IsInitialized;
			static bool Initialize();
			static void Destroy();
		
			static void PrepareDraw(::Ogre::String& TextureName, int Width, int Height, bool AddToCEGUI);
			static void FinishDraw();
			static void DrawBackground(int Width, int Height);
			static bool DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, ::System::Drawing::Rectangle DestRect, unsigned char Palette);
		};

		/// <summary>
		///
		/// </summary>
		ref class DirectDraw abstract sealed
		{
		private:		
			static ::Ogre::Texture*		 texture;
			static LPDIRECTDRAW7*		 dd7;
			static LPDIRECTDRAWSURFACE7* surfaceSource;
			static LPDIRECTDRAWSURFACE7* surfaceBG;
			static LPDIRECTDRAWSURFACE7* surfaceDest;
			static DDSURFACEDESC2*		 surfaceSourceDesc;
			static DDSURFACEDESC2*		 surfaceBGDesc;
			static DDSURFACEDESC2*		 surfaceDestDesc;

		public:			
			static bool IsInitialized;
			static bool Initialize();
			static void Destroy();
		
			static void PrepareDraw(::Ogre::String& TextureName, int Width, int Height, bool AddToCEGUI);
			static void FinishDraw();
			static void DrawBackground(int Width, int Height);
			static bool DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, RECT* DestRect, unsigned char Palette);
		
			static ::System::String^ GetError(HRESULT hr);
		};
		
		/// <summary>
		///
		/// </summary>
		ref class DirectX abstract sealed
		{
		protected:
			static IDirect3D9*			dx9;
			static IDirect3DDevice9**	dx9dev;
			static IDirect3DTexture9**	source;
			static ID3DXSprite**		sprite;
	
			static IDirect3DTexture9** dest;
			static IDirect3DSurface9** destSurface;
			static IDirect3DSurface9** backSurface;

			static ::Ogre::Texture* texture;

			static void* buf;
			static int width;
			static int height;

		public:
			static bool IsInitialized = false;
			static bool Initialize();
			static void Destroy();

			static void PrepareDraw(::Ogre::String& TextureName, int Width, int Height, bool AddToCEGUI);
			static void FinishDraw();
			static void DrawBackground(int Width, int Height);
			static bool DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, RECT* DestRect, unsigned char Palette);
		
			static ::System::String^ GetError(HRESULT hr);		
		};

		/// <summary>
		///
		/// </summary>
		ref class Native abstract sealed
		{
		private:
			static ::Ogre::Texture*				texture;
			static int							width;
			static int							height;
			static unsigned int*				texBuffer;

			static ::Meridian59::Files::BGF::BgfBitmap^ background;

		public:			
			static bool IsInitialized;
			static bool Initialize();
			static void Destroy();

			static void PrepareDraw(::Ogre::String& TextureName, int Width, int Height, bool AddToCEGUI);
			static void FinishDraw();
			static void DrawBackground(int Width, int Height);
			static bool DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, unsigned int OverlayX, unsigned int OverlayY, unsigned int OverlayWidth, unsigned int OverlayHeight, unsigned char Palette);
		};

		__forceinline static bool Initialize(ImageBuilderType Builder)
		{
			builderType = Builder;

			if (builderType == ImageBuilderType::GDI)
				return GDI::Initialize();
			
			else if (builderType == ImageBuilderType::DirectDraw)
				return DirectDraw::Initialize();

			else if (builderType == ImageBuilderType::DirectX)
				return DirectX::Initialize();
			
			else if (builderType == ImageBuilderType::Native)
				return Native::Initialize();

			return false;
		};

		__forceinline static void Destroy()
		{
			if (builderType == ImageBuilderType::GDI)
				return GDI::Destroy();
			
			else if (builderType == ImageBuilderType::DirectDraw)
				return DirectDraw::Destroy();

			else if (builderType == ImageBuilderType::DirectX)
				return DirectX::Destroy();

			else if (builderType == ImageBuilderType::Native)
				return Native::Destroy();
		};

		__forceinline static void PrepareDraw(::Ogre::String& TextureName, int Width, int Height, bool AddToCEGUI)
		{
			if (builderType == ImageBuilderType::GDI)
				GDI::PrepareDraw(TextureName, Width, Height, AddToCEGUI);
			
			else if (builderType == ImageBuilderType::DirectDraw)
				DirectDraw::PrepareDraw(TextureName, Width, Height, AddToCEGUI);

			else if (builderType == ImageBuilderType::DirectX)
				DirectX::PrepareDraw(TextureName, Width, Height, AddToCEGUI);

			else if (builderType == ImageBuilderType::Native)
				Native::PrepareDraw(TextureName, Width, Height, AddToCEGUI);
		};

		__forceinline static void FinishDraw()
		{
			if (builderType == ImageBuilderType::GDI)
				GDI::FinishDraw();
			
			else if (builderType == ImageBuilderType::DirectDraw)
				DirectDraw::FinishDraw();

			else if (builderType == ImageBuilderType::DirectX)
				DirectX::FinishDraw();

			else if (builderType == ImageBuilderType::Native)
				Native::FinishDraw();
		};

		__forceinline static void DrawBackground(int Width, int Height)
		{
			if (builderType == ImageBuilderType::GDI)
				GDI::DrawBackground(Width, Height);
			
			else if (builderType == ImageBuilderType::DirectDraw)
				DirectDraw::DrawBackground(Width, Height);

			else if (builderType == ImageBuilderType::DirectX)
				DirectX::DrawBackground(Width, Height);

			else if (builderType == ImageBuilderType::Native)
				Native::DrawBackground(Width, Height);
		};

		__forceinline static bool DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, int DestX, int DestY, int DestWidth, int DestHeight, char Palette)
		{
			if (builderType == ImageBuilderType::GDI)
			{
				System::Drawing::Rectangle rect = System::Drawing::Rectangle(
					DestX, DestY, DestWidth, DestHeight);

				return GDI::DrawBGF(BgfBitmap, rect, Palette);
			}
			else if (builderType == ImageBuilderType::DirectDraw)
			{
				RECT rect;
				rect.left = DestX;
				rect.top = DestY;
				rect.right = rect.left + DestWidth;
				rect.bottom = rect.top + DestHeight;

				return DirectDraw::DrawBGF(BgfBitmap, &rect, Palette);
			}
			else if (builderType == ImageBuilderType::DirectX)
			{
				RECT rect;
				rect.left = DestX;
				rect.top = DestY;
				rect.right = rect.left + DestWidth;
				rect.bottom = rect.top + DestHeight;
				
				return DirectX::DrawBGF(BgfBitmap, &rect, Palette);
			}
			else if (builderType == ImageBuilderType::Native)
			{
				return Native::DrawBGF(BgfBitmap, DestX, DestY, DestWidth, DestHeight, Palette);
			}
			return false;
		};
	};
};};
