#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
#pragma region DirectDraw7
	bool ImageBuilder::DirectDraw::Initialize()
	{
		HRESULT hr;
		
		// don't init twice
		if (IsInitialized)
			return false;

		/*********************************************************************************************/

		// create internal ptr-to-ptr (works around managed class limitations)
		dd7					= (LPDIRECTDRAW7*)malloc(sizeof(LPDIRECTDRAW7));	
		surfaceSource		= (LPDIRECTDRAWSURFACE7*)malloc(sizeof(LPDIRECTDRAWSURFACE7));
		surfaceBG			= (LPDIRECTDRAWSURFACE7*)malloc(sizeof(LPDIRECTDRAWSURFACE7));
		surfaceDest			= (LPDIRECTDRAWSURFACE7*)malloc(sizeof(LPDIRECTDRAWSURFACE7));
		surfaceSourceDesc	= (DDSURFACEDESC2*)malloc(sizeof(DDSURFACEDESC2));
		surfaceBGDesc		= (DDSURFACEDESC2*)malloc(sizeof(DDSURFACEDESC2));
		surfaceDestDesc		= (DDSURFACEDESC2*)malloc(sizeof(DDSURFACEDESC2));
		*dd7				= NULL;
		*surfaceSource		= NULL;
		*surfaceBG			= NULL;
		*surfaceDest		= NULL;

		/*********************************************************************************************/

		// try initialize DirectDraw7+
		hr = DirectDrawCreateEx(NULL, (LPVOID*)dd7, IID_IDirectDraw7, NULL);
		
		if (!SUCCEEDED(hr))					
			return false;
		
		// try set cooperative-level
		hr = (**dd7).SetCooperativeLevel(NULL, DDSCL_NORMAL);
		
		if (!SUCCEEDED(hr))			
			return false;

		/*********************************************************************************************/

		// set surface description for internal cache surface
		// used as blitsource for the next subimage to draw
		ZeroMemory(surfaceSourceDesc, sizeof(DDSURFACEDESC2));
		ZeroMemory(&surfaceSourceDesc->ddpfPixelFormat, sizeof(DDPIXELFORMAT));
		surfaceSourceDesc->dwSize = sizeof(DDSURFACEDESC2);
		surfaceSourceDesc->dwFlags = DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT | DDSD_CKSRCBLT;
		surfaceSourceDesc->ddsCaps.dwCaps =  DDSCAPS_OFFSCREENPLAIN | DDSCAPS_SYSTEMMEMORY;
		surfaceSourceDesc->dwWidth  = CACHESURFACEWIDTH;
		surfaceSourceDesc->dwHeight = CACHESURFACEHEIGHT;		
		surfaceSourceDesc->ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
		surfaceSourceDesc->ddpfPixelFormat.dwFlags= DDPF_RGB | DDPF_ALPHAPIXELS;
		surfaceSourceDesc->ddpfPixelFormat.dwRGBBitCount		= 32;
		surfaceSourceDesc->ddpfPixelFormat.dwRBitMask			= 0x00FF0000;
		surfaceSourceDesc->ddpfPixelFormat.dwGBitMask			= 0x0000FF00;
		surfaceSourceDesc->ddpfPixelFormat.dwBBitMask			= 0x000000FF;
		surfaceSourceDesc->ddpfPixelFormat.dwRGBAlphaBitMask	= 0xFF000000;
		surfaceSourceDesc->ddckCKSrcBlt.dwColorSpaceHighValue	= 0x0000FFFF;	// magenta, palette #254/255
		surfaceSourceDesc->ddckCKSrcBlt.dwColorSpaceLowValue	= 0x0000FFFF;   // magenta, palette #254/255

		// create internal cache/source surface
		hr = (**dd7).CreateSurface(surfaceSourceDesc, surfaceSource, NULL);
		
		if (!SUCCEEDED(hr))			
			return false;

		/*********************************************************************************************/

		// set surface description for background glow surface
		// used as blitsource for the background
		ZeroMemory(surfaceBGDesc, sizeof(DDSURFACEDESC2));
		ZeroMemory(&surfaceBGDesc->ddpfPixelFormat, sizeof(DDPIXELFORMAT));
		surfaceBGDesc->dwSize = sizeof(DDSURFACEDESC2);
		surfaceBGDesc->dwFlags = DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT | DDSD_CKSRCBLT;
		surfaceBGDesc->ddsCaps.dwCaps =  DDSCAPS_OFFSCREENPLAIN | DDSCAPS_SYSTEMMEMORY;
		surfaceBGDesc->dwWidth  = BGICONWIDTH;
		surfaceBGDesc->dwHeight = BGICONHEIGHT;		
		surfaceBGDesc->ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
		surfaceBGDesc->ddpfPixelFormat.dwFlags= DDPF_RGB | DDPF_ALPHAPIXELS;
		surfaceBGDesc->ddpfPixelFormat.dwRGBBitCount		= 32;
		surfaceBGDesc->ddpfPixelFormat.dwRBitMask			= 0x00FF0000;
		surfaceBGDesc->ddpfPixelFormat.dwGBitMask			= 0x0000FF00;
		surfaceBGDesc->ddpfPixelFormat.dwBBitMask			= 0x000000FF;
		surfaceBGDesc->ddpfPixelFormat.dwRGBAlphaBitMask	= 0xFF000000;
		surfaceBGDesc->ddckCKSrcBlt.dwColorSpaceHighValue	= 0x0000FFFF;	// magenta, palette #254/255
		surfaceBGDesc->ddckCKSrcBlt.dwColorSpaceLowValue	= 0x0000FFFF;   // magenta, palette #254/255

		// create background surface
		hr = (**dd7).CreateSurface(surfaceBGDesc, surfaceBG, NULL);
		
		if (!SUCCEEDED(hr))			
			return false;
		
		// load glow background resource
		HANDLE handle = ::LoadImage(
			::GetModuleHandle(0),
			MAKEINTRESOURCE(IDB_GLOW),
			IMAGE_BITMAP, // type
			BGICONWIDTH, // actual width
			BGICONHEIGHT, // actual height
			0); // no flags
				
		// borrow gdi to get a 32bit variant of glow background (maketransparent = convert)
		::System::Drawing::Bitmap^ bg = ::System::Drawing::Image::FromHbitmap(::System::IntPtr(handle));
		bg->MakeTransparent(System::Drawing::Color::Cyan);
		::System::Drawing::Imaging::BitmapData^ data = bg->LockBits(
			::System::Drawing::Rectangle(0, 0, BGICONWIDTH, BGICONHEIGHT),
			::System::Drawing::Imaging::ImageLockMode::ReadOnly,
			::System::Drawing::Imaging::PixelFormat::Format32bppArgb);

		// lock
		hr = (**surfaceBG).Lock(NULL, surfaceBGDesc, DDLOCK_WRITEONLY, NULL);
		if (!SUCCEEDED(hr))			
			return false;

		// copy pixels
		memcpy(surfaceBGDesc->lpSurface, data->Scan0.ToPointer(), BGICONWIDTH * BGICONHEIGHT * 4);			

		// unlock
		hr = (**surfaceBG).Unlock(NULL);
		if (!SUCCEEDED(hr))			
			return false;
						
		bg->UnlockBits(data);
		delete bg;
		
		/*********************************************************************************************/

		// create (incomplete) surface description for destination surface
		// this surface must be initialized by SetDestination
		ZeroMemory(surfaceDestDesc, sizeof(DDSURFACEDESC2));
		ZeroMemory(&surfaceDestDesc->ddpfPixelFormat, sizeof(DDPIXELFORMAT));
		surfaceDestDesc->dwSize = sizeof(DDSURFACEDESC2);
		surfaceDestDesc->dwFlags = DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT | DDSD_LPSURFACE | DDSD_PITCH;
		surfaceDestDesc->ddsCaps.dwCaps =  DDSCAPS_OFFSCREENPLAIN | DDSCAPS_SYSTEMMEMORY;//DDSCAPS_VIDEOMEMORY
		surfaceDestDesc->ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
		surfaceDestDesc->ddpfPixelFormat.dwFlags= DDPF_RGB | DDPF_ALPHAPIXELS;
		surfaceDestDesc->ddpfPixelFormat.dwRGBBitCount		= 32;
		surfaceDestDesc->ddpfPixelFormat.dwRBitMask			= 0x00FF0000;
		surfaceDestDesc->ddpfPixelFormat.dwGBitMask			= 0x0000FF00;
		surfaceDestDesc->ddpfPixelFormat.dwBBitMask			= 0x000000FF;
		surfaceDestDesc->ddpfPixelFormat.dwRGBAlphaBitMask	= 0xFF000000;

		/*********************************************************************************************/

		// mark initialized
		IsInitialized = true;
			
		return true;
	};

	void ImageBuilder::DirectDraw::Destroy()
	{
		// must be initialized to destroy
		if (!IsInitialized)
			return;

		// free internal surface
		if (*surfaceSource)
			(**surfaceSource).Release();

		// free bg surface
		if (*surfaceBG)
			(**surfaceBG).Release();

		// free dd7
		if (*dd7)
			(**dd7).Release();

		// free work-around ptr-to-ptr
		free(dd7);
		free(surfaceSource);
		free(surfaceBG);
		free(surfaceDest);
		free(surfaceSourceDesc);		
		free(surfaceBGDesc);
		free(surfaceDestDesc);
		
		// mark not initialized
		IsInitialized = false;
	}

	void ImageBuilder::DirectDraw::PrepareDraw(::Ogre::String TextureName, int Width, int Height, bool AddToCEGUI)
	{
		HRESULT hr;

		if (!surfaceDestDesc)
			return;
	
		::Ogre::TextureManager* texMan = ::Ogre::TextureManager::getSingletonPtr();

		// create manual (empty) texture
		::Ogre::TexturePtr texPtr = texMan->createManual(
			TextureName,
			TEXTUREGROUP_REMOTENODE2D,
			TextureType::TEX_TYPE_2D,
			Width, Height, MIP_DEFAULT,
			::Ogre::PixelFormat::PF_A8R8G8B8,
			TU_DEFAULT, 0, false, 0);
				
		if (AddToCEGUI)
			Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
		
		texture = texPtr.get();

		// lock the texturebuffer (released in finish)
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_WRITE_ONLY);
		
		// set width/height on surfacedesc
		surfaceDestDesc->dwWidth  = Width;
		surfaceDestDesc->dwHeight = Height;
		surfaceDestDesc->lpSurface = texBuffer;
		surfaceDestDesc->lPitch = Width * 4;
		
		// create surface on buffer
		hr = (**dd7).CreateSurface(surfaceDestDesc, surfaceDest, NULL);
		if (!SUCCEEDED(hr))
			return;	
	}
	
	void ImageBuilder::DirectDraw::FinishDraw()
	{		
		// release
		(**surfaceDest).Release();

		// release texture buffer
		texture->getBuffer()->unlock();
	}

	void ImageBuilder::DirectDraw::DrawBackground(int Width, int Height)
	{
		RECT dest;
		HRESULT hr;		

		// define dest rect
		dest.left = 0;
		dest.top = 0;
		dest.right = Width;
		dest.bottom = Height;

		// blit
		hr = (**surfaceDest).Blt(&dest, *surfaceBG, NULL, DDBLT_WAIT | DDBLT_KEYSRC, NULL);
		if (!SUCCEEDED(hr))
			return;
	}

	bool ImageBuilder::DirectDraw::DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, RECT* DestRect, unsigned char Palette)
	{		
		RECT src;
		HRESULT hr;		

		if (!BgfBitmap || !DestRect)
			return false;

		// lock
		hr = (**surfaceSource).Lock(NULL, surfaceSourceDesc, DDLOCK_WRITEONLY, NULL);
		if (!SUCCEEDED(hr))			
			return false;
					
		// clean bufferparts we use
		//ZeroMemory(surfaceSourceDesc->lpSurface, BgfBitmap->Height * CACHESURFACEWIDTH * 4); 

		// fill pixeldata	
		BgfBitmap->FillPixelDataAsA8R8G8B8((unsigned int*)surfaceSourceDesc->lpSurface, Palette, CACHESURFACEWIDTH);			

		// unlock
		hr = (**surfaceSource).Unlock(NULL);
		if (!SUCCEEDED(hr))			
			return false;
				
		// define source rectangle (what we filled in the buffer above)
		src.left = 0;
		src.top = 0;
		src.right = BgfBitmap->Width;
		src.bottom = BgfBitmap->Height;

		// blit the source rectangle from the filled cache-surface
		// into the target surface
		hr = (**surfaceDest).Blt(DestRect, *surfaceSource, &src, DDBLT_WAIT | DDBLT_KEYSRC, NULL);
		if (!SUCCEEDED(hr))
			return false;

		return true;
	}

	::System::String^ ImageBuilder::DirectDraw::GetError(HRESULT hr)
	{
		switch(hr)
		{
		case DDERR_GENERIC:					return "DDERR_GENERIC";
		case DDERR_INVALIDCLIPLIST:			return "DDERR_INVALIDCLIPLIST";
		case DDERR_INVALIDRECT:				return "DDERR_INVALIDRECT";
		case DDERR_NOBLTHW:					return "DDERR_NOBLTHW";
		case DDERR_NOCLIPLIST:				return "DDERR_NOCLIPLIST";
		case DDERR_NODDROPSHW:				return "DDERR_NODDROPSHW";
		case DDERR_NOMIRRORHW:				return "DDERR_NOMIRRORHW";
		case DDERR_NORASTEROPHW:			return "DDERR_NORASTEROPHW";
		case DDERR_NOROTATIONHW:			return "DDERR_NOROTATIONHW";			
		case DDERR_NOSTRETCHHW:				return "DDERR_NOSTRETCHHW";
		case DDERR_NOZBUFFERHW:				return "DDERR_NOZBUFFERHW";		
		case DDERR_SURFACEBUSY:				return "DDERR_SURFACEBUSY";
		case DDERR_SURFACELOST:				return "DDERR_SURFACELOST";
		case DDERR_UNSUPPORTED:				return "DDERR_UNSUPPORTED";
		case DDERR_WASSTILLDRAWING:			return "DDERR_WASSTILLDRAWING";	
		case DDERR_INCOMPATIBLEPRIMARY:		return "DDERR_INCOMPATIBLEPRIMARY";
		case DDERR_INVALIDCAPS:				return "DDERR_INVALIDCAPS";
		case DDERR_INVALIDOBJECT:			return "DDERR_INVALIDOBJECT";
		case DDERR_INVALIDPARAMS:			return "DDERR_INVALIDPARAMS";
		case DDERR_INVALIDPIXELFORMAT:		return "DDERR_INVALIDPIXELFORMAT";
		case DDERR_NOALPHAHW:				return "DDERR_NOALPHAHW";
		case DDERR_NOCOOPERATIVELEVELSET:	return "DDERR_NOCOOPERATIVELEVELSET";
		case DDERR_NOFLIPHW:				return "DDERR_NOFLIPHW";
		case DDERR_NOOVERLAYHW:				return "DDERR_NOOVERLAYHW";
		case DDERR_OUTOFMEMORY:				return "DDERR_OUTOFMEMORY";
		case DDERR_OUTOFVIDEOMEMORY:		return "DDERR_OUTOFVIDEOMEMORY";
		case DDERR_PRIMARYSURFACEALREADYEXISTS: return "DDERR_PRIMARYSURFACEALREADYEXISTS";
		case DDERR_NODIRECTDRAWHW:			return "DDERR_NODIRECTDRAWHW";
		case DDERR_NOEMULATION:				return "DDERR_NOEMULATION";
		case DDERR_NOEXCLUSIVEMODE:			return "DDERR_NOEXCLUSIVEMODE";
    	case DDERR_NOMIPMAPHW:				return "DDERR_NOMIPMAPHW";
		case DDERR_UNSUPPORTEDMODE:			return "DDERR_UNSUPPORTEDMODE";
		default:							return "UNKNOWN ERROR";
		}
	}
#pragma endregion

#pragma region GDI
	bool ImageBuilder::GDI::Initialize()
	{		
		// don't init twice
		if (IsInitialized)
			return false;

		// create temporary source image
		source = gcnew ::System::Drawing::Bitmap(
			CACHESURFACEWIDTH, CACHESURFACEHEIGHT, ::System::Drawing::Imaging::PixelFormat::Format32bppArgb);
		
		// load glow background resource
		HANDLE handle = ::LoadImage(
			::GetModuleHandle(0),
			MAKEINTRESOURCE(IDB_GLOW),
			IMAGE_BITMAP, // type
			BGICONWIDTH, // actual width
			BGICONHEIGHT, // actual height
			0); // no flags
		
		// create gdi bitmap for background
		background = ::System::Drawing::Image::FromHbitmap(::System::IntPtr(handle));
		background->MakeTransparent(System::Drawing::Color::Cyan);
		
		// clean
		DeleteObject(handle);

		// mark initialized
		IsInitialized = true;

		return true;
	};

	void ImageBuilder::GDI::Destroy()
	{
		// must be initialized to destroy
		if (!IsInitialized)
			return;

		if (dest)
			delete dest;

		if (source)
			delete source;

		if (background)
			delete background;

		if (graphics)
			delete graphics;

		IsInitialized = false;
	}

	void ImageBuilder::GDI::PrepareDraw(::Ogre::String TextureName, int Width, int Height, bool AddToCEGUI)
	{
		if (dest)
			delete dest;

		if (graphics)
			delete graphics;

		::Ogre::TextureManager* texMan = ::Ogre::TextureManager::getSingletonPtr();

		// create manual (empty) texture
		::Ogre::TexturePtr texPtr = texMan->createManual(
			TextureName,
			TEXTUREGROUP_REMOTENODE2D,
			TextureType::TEX_TYPE_2D,
			Width, Height, MIP_DEFAULT,
			::Ogre::PixelFormat::PF_A8R8G8B8,
			TU_DEFAULT, 0, false, 0);
				
		if (AddToCEGUI)
			Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
		
		texture = texPtr.get();

		// lock the texturebuffer (released in finish)
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_WRITE_ONLY);
		
		dest = gcnew ::System::Drawing::Bitmap(
			Width, Height, Width * 4, ::System::Drawing::Imaging::PixelFormat::Format32bppArgb, (::System::IntPtr)texBuffer);
			
		// create painer
		graphics = ::System::Drawing::Graphics::FromImage(dest);
		graphics->InterpolationMode = InterpolationMode;
		graphics->SmoothingMode = SmoothingMode::HighQuality;

#ifdef _DEBUG
		graphics->Clear(Color::Black);
#endif
	}

	void ImageBuilder::GDI::FinishDraw()
	{
		// release texture buffer
		texture->getBuffer()->unlock();
	}

	void ImageBuilder::GDI::DrawBackground(int Width, int Height)
	{
		// copy from this rectangle in source
		System::Drawing::Rectangle fromRectangle = System::Drawing::Rectangle(
			0, 0, BGICONWIDTH, BGICONHEIGHT);

		// copy to this rectangle in destination
		System::Drawing::Rectangle toRectangle = System::Drawing::Rectangle(
			0, 0, Width, Height);

		// draw from mainbitmap into DrawTo object using rectangles
		graphics->DrawImage(background, toRectangle, fromRectangle, GraphicsUnit::Pixel);
	}

	bool ImageBuilder::GDI::DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, ::System::Drawing::Rectangle DestRect, unsigned char Palette)
	{	
		// lock
		::System::Drawing::Imaging::BitmapData^ data = source->LockBits(
			::System::Drawing::Rectangle(0, 0, CACHESURFACEWIDTH, CACHESURFACEHEIGHT),
			::System::Drawing::Imaging::ImageLockMode::WriteOnly,
			::System::Drawing::Imaging::PixelFormat::Format32bppArgb);

		ZeroMemory(data->Scan0.ToPointer(), (BgfBitmap->Height+1) * CACHESURFACEWIDTH * 4); 

		// fill with new data
		BgfBitmap->FillPixelDataAsA8R8G8B8((unsigned int*)data->Scan0.ToPointer(), Palette, CACHESURFACEWIDTH);
		
		// unlock
		source->UnlockBits(data);

		// source rectangle of just filled pixels
		::System::Drawing::Rectangle srcRect = 
			::System::Drawing::Rectangle(0, 0, BgfBitmap->Width, BgfBitmap->Height);
		
		// draw into destinaation
		graphics->DrawImage(source, DestRect, srcRect, ::System::Drawing::GraphicsUnit::Pixel);

		return true;
	}
#pragma endregion

#pragma region DirectX
	bool ImageBuilder::DirectX::Initialize()
	{		
		HRESULT hr;
		D3DPRESENT_PARAMETERS params;
		
		///// DX9 INIT
		
		dx9 = Direct3DCreate9(D3D_SDK_VERSION);

		if (!dx9)
			return false;

		dx9dev  = (IDirect3DDevice9**)malloc(sizeof(IDirect3DDevice9*));
		source  = (IDirect3DTexture9**)malloc(sizeof(IDirect3DTexture9*));
		sprite  = (ID3DXSprite**)malloc(sizeof(ID3DXSprite*));
		*dx9dev = NULL;		
		*source = NULL;		
		*sprite = NULL;

		// headless params (rtt only)
		ZeroMemory(&params, sizeof(D3DPRESENT_PARAMETERS));
		params.BackBufferWidth  = 1;
		params.BackBufferHeight = 1;
		params.BackBufferFormat = D3DFMT_A8R8G8B8;
		params.BackBufferCount  = 1;
		params.SwapEffect		= D3DSWAPEFFECT_DISCARD;
		params.Windowed			= TRUE;
		params.hDeviceWindow	= NULL;
		
		// create device
		hr = dx9->CreateDevice(
			D3DADAPTER_DEFAULT, 
			D3DDEVTYPE_HAL, 
			NULL,
			D3DCREATE_MIXED_VERTEXPROCESSING,
			&params, 
			dx9dev);
		
		if (!SUCCEEDED(hr))		
			return false;
					
		// create cache texture to write 32-bit pixeldata to
		hr = (**dx9dev).CreateTexture(
			CACHESURFACEWIDTH, 
			CACHESURFACEHEIGHT, 
			0, 0, 
			D3DFMT_A8R8G8B8, 
			D3DPOOL_MANAGED, 
			source, NULL); 		
		
		if (!SUCCEEDED(hr))		
			return false;
		
		// create sprite
		hr = D3DXCreateSprite(*dx9dev, sprite);
		
		if (!SUCCEEDED(hr))		
			return false;
		
		// setup device renderstate	
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_ZENABLE, false)))		
			return false;
			
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_ALPHABLENDENABLE, TRUE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_ALPHAFUNC, D3DCMP_GREATER)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_ALPHAREF, (DWORD)0x000000F0)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_ALPHATESTENABLE, TRUE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_BLENDOP, D3DBLENDOP_ADD)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_CLIPPING, TRUE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_CLIPPLANEENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_COLORWRITEENABLE, D3DCOLORWRITEENABLE_ALPHA | D3DCOLORWRITEENABLE_BLUE | D3DCOLORWRITEENABLE_GREEN | D3DCOLORWRITEENABLE_RED)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_CULLMODE, D3DCULL_NONE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_DESTBLEND, D3DBLEND_INVSRCALPHA)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_DIFFUSEMATERIALSOURCE, D3DMCS_COLOR1)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_ENABLEADAPTIVETESSELLATION, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_FILLMODE, D3DFILL_SOLID)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_FOGENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_INDEXEDVERTEXBLENDENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_LIGHTING, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_RANGEFOGENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_SEPARATEALPHABLENDENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_SHADEMODE, D3DSHADE_GOURAUD)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_SPECULARENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_SRCBLEND, D3DBLEND_SRCALPHA)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_SRGBWRITEENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_STENCILENABLE, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_VERTEXBLEND, FALSE)))		
			return false;
		
		if (!SUCCEEDED((**dx9dev).SetRenderState(D3DRS_WRAP0, 0)))		
			return false;
			
		// setup device samplerstate
		// D3DTEXF_NONE  D3DTEXF_POINT  D3DTEXF_LINEAR  D3DTEXF_ANISOTROPIC  D3DTEXF_PYRAMIDALQUAD  D3DTEXF_GAUSSIANQUAD 
		if (!SUCCEEDED((**dx9dev).SetSamplerState(0, D3DSAMP_MINFILTER, D3DTEXF_ANISOTROPIC)))			
			return false;
			
		if (!SUCCEEDED((**dx9dev).SetSamplerState(0, D3DSAMP_MAGFILTER, D3DTEXF_ANISOTROPIC)))		
			return false;
			
		if (!SUCCEEDED((**dx9dev).SetSamplerState(0, D3DSAMP_MIPFILTER, D3DTEXF_NONE)))		
			return false;
		
		//////////////////////////////////////////////////////////////////////////////////////////

		
		dest  = (IDirect3DTexture9**)malloc(sizeof(IDirect3DTexture9*));
		destSurface  = (IDirect3DSurface9**)malloc(sizeof(IDirect3DSurface9*));
		backSurface  = (IDirect3DSurface9**)malloc(sizeof(IDirect3DSurface9*));
		*dest = NULL;
		*destSurface = NULL;
		*backSurface = NULL;



		//////////////////////////////////////////////////////////////////////////////////////////

		// mark initialized
		IsInitialized = true;

		return true;
	};

	void ImageBuilder::DirectX::Destroy()
	{
		if (!IsInitialized)
			return;

		if (*sprite)
			(**sprite).Release();
			
		if (*source)
			(**source).Release();
		
		if (*dx9dev)
			(**dx9dev).Release();

		if (dx9)
			dx9->Release();

		// free work-around ptr-to-ptr
		free(dx9dev);
		free(source);
		//free(surfaceBG);
		free(sprite);
		free(dest);		
		free(destSurface);
		free(backSurface);
		
		IsInitialized = false;
	}

	void ImageBuilder::DirectX::PrepareDraw(::Ogre::String TextureName, int Width, int Height, bool AddToCEGUI)
	{
		::Ogre::TextureManager* texMan = ::Ogre::TextureManager::getSingletonPtr();
		
		// create manual (empty) texture
		::Ogre::TexturePtr texPtr = texMan->createManual(
			TextureName,
			TEXTUREGROUP_REMOTENODE2D,
			TextureType::TEX_TYPE_2D,
			Width, Height, MIP_DEFAULT,
			::Ogre::PixelFormat::PF_A8R8G8B8,
			TU_DEFAULT, 0, false, 0);
				
		//::Ogre::D3D9Texture* d3dx9tex = (::Ogre::D3D9Texture*)image->Texture;
		//IDirect3DTexture9* d9tex = d3dx9tex->getNormTexture();
			
		if (AddToCEGUI)
			Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);
		
		texture = texPtr.get();

		// lock the texturebuffer (released in finish)
		void* texBuffer = texture->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_WRITE_ONLY);

		buf = texBuffer;
		width = Width;
		height = Height;

		HRESULT hr;
		
		//// DESTTEX / RTT
		hr = (**dx9dev).CreateTexture(width, height, 0, D3DUSAGE_RENDERTARGET, D3DFMT_A8R8G8B8, D3DPOOL_DEFAULT, dest, NULL); 
		if (!SUCCEEDED(hr))
			return;
		
		// BACKSURFACE
		hr = (**dx9dev).CreateOffscreenPlainSurface(width, height, D3DFMT_A8R8G8B8, D3DPOOL_SYSTEMMEM, backSurface, NULL);
		if (!SUCCEEDED(hr))
			return;

		// get render surface of texture
		hr = (**dest).GetSurfaceLevel(0, destSurface);
		if (!SUCCEEDED(hr))
			return;

		// set surface as render target
		hr = (**dx9dev).SetRenderTarget(0, *destSurface);
		if (!SUCCEEDED(hr))
			return;

		////////////////////////////////////////////////////////////////////////////////////////
		// Clear
#if DEBUG
		hr = (**dx9dev).Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_ARGB(0,0,0,0), 1.0f, 0);
#else
		hr = (**dx9dev).Clear(0, NULL, D3DCLEAR_TARGET, D3DCOLOR_ARGB(0,0,0,0), 1.0f, 0);
#endif
		if (!SUCCEEDED(hr))
			return;
		
		// Begin scene
		hr = (**dx9dev).BeginScene();
		if (!SUCCEEDED(hr))
			return;
			
		// Begin sprite (without touching renderstate)
		hr = (**sprite).Begin(D3DXSPRITE_DONOTSAVESTATE | D3DXSPRITE_DONOTMODIFY_RENDERSTATE);
		if (!SUCCEEDED(hr))
			return;
	}

	void ImageBuilder::DirectX::FinishDraw()
	{
		HRESULT hr;
		
		////////////////////////////////////////////////////////////////////////////////////////
		
		// End sprite
		hr = (**sprite).End();
		if (!SUCCEEDED(hr))
			return;

		// End scene
		hr = (**dx9dev).EndScene();
		if (!SUCCEEDED(hr))
			return;
		
		// Present
		hr = (**dx9dev).Present(NULL, NULL, NULL, NULL);
		if (!SUCCEEDED(hr))
			return;

		////////////////////////////////////////////////////////////////////////////////////////
		// COPYBACK	

		D3DLOCKED_RECT lockRect;
		RECT destrect;

		hr = (**dx9dev).GetRenderTargetData(*destSurface, *backSurface);
		if (!SUCCEEDED(hr))
			return;

		destrect.left   = 0; 
		destrect.top    = 0; 
		destrect.right  = width;
		destrect.bottom = height; 
		
		hr = (**backSurface).LockRect(&lockRect, &destrect, D3DLOCK_DISCARD);
		if (!SUCCEEDED(hr))
			return;
		
		memcpy(buf, lockRect.pBits, width*height*4);

		hr = (**backSurface).UnlockRect();
		if (!SUCCEEDED(hr))
			return;	

		// release texture buffer
		texture->getBuffer()->unlock();

		(**destSurface).Release();
		(**dest).Release();
		(**backSurface).Release();
	}

	void ImageBuilder::DirectX::DrawBackground(int Width, int Height)
	{
		
	}

	bool ImageBuilder::DirectX::DrawBGF(::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, RECT* DestRect, unsigned char Palette)
	{	
		if (BgfBitmap->Width == 0 || BgfBitmap->Height == 0)
				return false;

		HRESULT hr;
		D3DLOCKED_RECT lockRect;		
		RECT srcRect;
		D3DXMATRIX mat;
	
		// rectangle to fill with pixels in source texture
		srcRect.left   = 0;
		srcRect.top	   = 0;
		srcRect.right  = BgfBitmap->Width;
		srcRect.bottom = BgfBitmap->Height;

		////////////////////////////////////////////////////////////////////////////////////////

		// lock
		hr = (**source).LockRect(0, &lockRect, &srcRect, D3DLOCK_DISCARD);
		if (!SUCCEEDED(hr))
			return false;
		
		//ZeroMemory(lockRect.pBits, 1024 * 1024 * 4); 

		// fill
		BgfBitmap->FillPixelDataAsA8R8G8B8((unsigned int*)lockRect.pBits, Palette, CACHESURFACEWIDTH);

		// unlock
		hr = (**source).UnlockRect(0);
		if (!SUCCEEDED(hr))
			return false;

		////////////////////////////////////////////////////////////////////////////////////////
	
		FLOAT scalex = (FLOAT)(DestRect->right - DestRect->left) / (FLOAT)BgfBitmap->Width;
		FLOAT scaley = (FLOAT)(DestRect->bottom - DestRect->top) / (FLOAT)BgfBitmap->Height;
			
		D3DXMatrixTransformation2D(&mat, NULL, 0.0f, &D3DXVECTOR2(scalex, scaley),  
			NULL, 0.0f, &D3DXVECTOR2((float)DestRect->left, (float)DestRect->top));

		// transform
		hr = (**sprite).SetTransform(&mat);
		if (!SUCCEEDED(hr))
			return false;

		// draw
		hr = (**sprite).Draw(*source, &srcRect, NULL, NULL, D3DCOLOR_RGBA(255,255,255,255));  
		if (!SUCCEEDED(hr))
			return false;
		
		// flush
		hr = (**sprite).Flush();
		if (!SUCCEEDED(hr))
			return false;

		return true;
	}

	::System::String^ ImageBuilder::DirectX::GetError(HRESULT hr)
	{
		switch(hr)
		{
		case D3DERR_DEVICELOST:				return "D3DERR_DEVICELOST";
		case D3DERR_INVALIDCALL:			return "D3DERR_INVALIDCALL";
		case D3DERR_NOTAVAILABLE:			return "D3DERR_NOTAVAILABLE";
		case D3DERR_OUTOFVIDEOMEMORY:		return "D3DERR_OUTOFVIDEOMEMORY";		
		case E_OUTOFMEMORY:					return "E_OUTOFMEMORY";	
		case D3DXERR_INVALIDDATA:			return "D3DXERR_INVALIDDATA";			
		default:							return "UNKNOWN ERROR";
		}
	}
#pragma endregion
};};