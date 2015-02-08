#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
	generic <typename T>
	static ImageComposerCEGUI<T>::ImageComposerCEGUI(void)
	{
	}

	generic <typename T>
	ImageComposerCEGUI<T>::ImageComposerCEGUI(void)
	{		
	}

	generic <typename T>
	void ImageComposerCEGUI<T>::PrepareDraw()
	{	
		::Ogre::TextureManager* texMan = ::Ogre::TextureManager::getSingletonPtr();
						
		// get the unique hash for the current appearance of the object
        System::String^ key = AppearanceHash.ToString();
        
		// create new texture info
		image = gcnew TextureInfoCEGUI();
		image->TextureName = StringConvert::CLRToCEGUIPtr(PREFIX_CEGUI_TEXTURE + key);

		unsigned short width = System::Convert::ToUInt16(RenderInfo->Dimension.X);
		unsigned short height = System::Convert::ToUInt16(RenderInfo->Dimension.Y);
	
		ImageBuilder::PrepareDraw(StringConvert::CLRToOgre(PREFIX_CEGUI_TEXTURE + key), width, height, true);
	};
	
	generic <typename T>
	void ImageComposerCEGUI<T>::DrawBackground() 
	{ 		
		ImageBuilder::DrawBackground(
			System::Convert::ToInt32(RenderInfo->Dimension.X),
			System::Convert::ToInt32(RenderInfo->Dimension.Y));
	};
	
	generic <typename T>
	void ImageComposerCEGUI<T>::DrawMainOverlay() 
	{
		ImageBuilder::DrawBGF(
			RenderInfo->Bgf, 
			System::Convert::ToInt32(RenderInfo->Origin.X),
			System::Convert::ToInt32(RenderInfo->Origin.Y),
			System::Convert::ToInt32(RenderInfo->Size.X),
			System::Convert::ToInt32(RenderInfo->Size.Y),
			RenderInfo->BgfColor);	
	};
	
	generic <typename T>
	void ImageComposerCEGUI<T>::DrawSubOverlay(SubOverlay::RenderInfo^ RenderInfo) 
	{
		ImageBuilder::DrawBGF(
			RenderInfo->Bgf, 
			System::Convert::ToInt32(RenderInfo->Origin.X),
			System::Convert::ToInt32(RenderInfo->Origin.Y),
			System::Convert::ToInt32(RenderInfo->Size.X),
			System::Convert::ToInt32(RenderInfo->Size.Y),
			RenderInfo->SubOverlay->ColorTranslation);				
	};
	
	generic <typename T>
	void ImageComposerCEGUI<T>::DrawPostEffects() 
	{ 
	};
	
	generic <typename T>
	void ImageComposerCEGUI<T>::FinishDraw() 
	{ 
		ImageBuilder::FinishDraw();
	};
};};
