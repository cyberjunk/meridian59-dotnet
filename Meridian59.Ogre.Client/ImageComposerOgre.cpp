#include "stdafx.h"

namespace Meridian59 { namespace Ogre 
{
   void TextureInfoOgre::Delete(void)
   {
      ::Ogre::MaterialManager& matMan = ::Ogre::MaterialManager::getSingleton();
      ::Ogre::TextureManager& texMan  = ::Ogre::TextureManager::getSingleton();

      // remove materials
      matMan.remove(*MaterialNameDefault);
      matMan.remove(*MaterialNameBlack);
      matMan.remove(*MaterialNameTarget);
      matMan.remove(*MaterialNameMouseOver);
      matMan.remove(*MaterialNameTranslucent25);
      matMan.remove(*MaterialNameTranslucent50);
      matMan.remove(*MaterialNameTranslucent75);
      matMan.remove(*MaterialNameInvisible);

      // delete material strings
      delete MaterialNameDefault;
      delete MaterialNameBlack;
      delete MaterialNameTarget;
      delete MaterialNameMouseOver;
      delete MaterialNameTranslucent25;
      delete MaterialNameTranslucent50;
      delete MaterialNameTranslucent75;
      delete MaterialNameInvisible;

      // remove texture
      texMan.remove(*TextureName);

      // delete texture string
      delete TextureName;
   }

   generic <typename T>
   static ImageComposerOgre<T>::ImageComposerOgre(void)
   {
   }

   generic <typename T>
   ImageComposerOgre<T>::ImageComposerOgre(void)
   {
   }

   generic <typename T>
   void ImageComposerOgre<T>::PrepareDraw()
   {
      // get the unique hash for the current appearance of the object
      CLRString^ key = AppearanceHash.ToString();

      // create new texture info
      image = gcnew TextureInfoOgre();
      image->MaterialNameDefault       = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/default");
      image->MaterialNameBlack         = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/black");
      image->MaterialNameTarget        = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/target");
      image->MaterialNameTranslucent25 = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/translucent25");
      image->MaterialNameTranslucent50 = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/translucent50");
      image->MaterialNameTranslucent75 = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/translucent75");
      image->MaterialNameInvisible     = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/invisible");
      image->MaterialNameMouseOver     = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_MATERIAL + key + "/mouseover");
      image->TextureName               = StringConvert::CLRToOgrePtr(PREFIX_REMOTENODE2D_TEXTURE + key);

      unsigned short width = System::Convert::ToUInt16(RenderInfo->Dimension.X);
      unsigned short height = System::Convert::ToUInt16(RenderInfo->Dimension.Y);

      ImageBuilder::PrepareDraw(*image->TextureName, width, height, false);

      // create default material
      Util::CreateMaterial(
         *image->MaterialNameDefault, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D, 
         nullptr, &::Ogre::Vector4(1.0f, 1.0f, 1.0f, 1.0f), false);

      // create black material
      Util::CreateMaterial(
         *image->MaterialNameBlack, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D,
         nullptr, &::Ogre::Vector4(0.0f, 0.0f, 0.0f, 1.0f), false);

      // create target highlight material
      Util::CreateMaterial(
         *image->MaterialNameTarget, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D,
         nullptr, &::Ogre::Vector4(4.0f, 1.2f, 1.2f, 1.0f), false);

      // create mouseover highlight material
      Util::CreateMaterial(
         *image->MaterialNameMouseOver, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D,
         nullptr, &::Ogre::Vector4(1.2f, 4.2f, 1.2f, 1.0f), false);

      // create 25% translucent material
      // note: they call opacity translucent in orig, so it's NOT 1 - translucent for opaque
      Util::CreateMaterial(
         *image->MaterialNameTranslucent25, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D,
         nullptr, &::Ogre::Vector4(1.0f, 1.0f, 1.0f, 0.25f), false);

      // create 50% translucent material
      // note: they call opacity translucent in orig, so it's NOT 1 - translucent for opaque
      Util::CreateMaterial(
         *image->MaterialNameTranslucent50, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D,
         nullptr, &::Ogre::Vector4(1.0f, 1.0f, 1.0f, 0.5f), false);

      // create 75% translucent material (note they call opacity transluent in orig)
      // note: they call opacity translucent in orig, so it's NOT 1 - translucent for opaque
      Util::CreateMaterial(
         *image->MaterialNameTranslucent75, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D,
         nullptr, &::Ogre::Vector4(1.0f, 1.0f, 1.0f, 0.75f), false);

      // create invisible material
      Util::CreateMaterialInvisible(
         *image->MaterialNameInvisible, *image->TextureName,
         MATERIALGROUP_REMOTENODE2D);
   };

   generic <typename T>
   void ImageComposerOgre<T>::DrawBackground() 
   { 
   };

   generic <typename T>
   void ImageComposerOgre<T>::DrawMainOverlay() 
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
   void ImageComposerOgre<T>::DrawSubOverlay(SubOverlay::RenderInfo^ RenderInfo) 
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
   void ImageComposerOgre<T>::DrawPostEffects() 
   { 
   };

   generic <typename T>
   void ImageComposerOgre<T>::FinishDraw() 
   {
      ImageBuilder::FinishDraw();
   };
};};
