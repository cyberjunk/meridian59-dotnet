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
#include "OgreColourValue.h"
#include "OgreDataStream.h"
#include "OgreVector3.h"
#include "OgreVector2.h"
#include "OgrePixelFormat.h"
#include "OgreTextureManager.h"
#include "OgreHardwarePixelBuffer.h"
#include "OgreSceneNode.h"
#include "OgreSceneManager.h"
#include "OgreMaterialSerializer.h"
#include "CEGUI/CEGUI.h"
#include "CEGUI/RendererModules/Ogre/Renderer.h"
#pragma managed(pop)

#include "Constants.h"

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace System::IO;
	using namespace System::Drawing;
	using namespace Meridian59::Common;
	using namespace Meridian59::Common::Constants;
	using namespace Meridian59::Common::Interfaces;
	
	/// <summary>
    /// Basic converters and widespread used Ogre interactions.
    /// </summary>
	public ref class Util abstract sealed
	{		
	public:
		#pragma region Vectors
        /// <summary>
        /// Creates an Ogre Vector3 from a V3 instance
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::Vector3 ToOgre(::Meridian59::Common::V3 Vector)
        {
            return ::Ogre::Vector3(Vector.X, Vector.Y, Vector.Z);
        };

        /// <summary>
        /// Creates a V3 from an Ogre Vector3
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Meridian59::Common::V3 ToV3(::Ogre::Vector3 Vector)
        {
            return ::Meridian59::Common::V3(Vector.x, Vector.y, Vector.z);
        };

        /// <summary>
        /// Creates an Ogre Vector3 from a V3 instance
        /// with Y, Z parts flipped.
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::Vector3 ToOgreYZFlipped(V3 Vector)
        {
            return ::Ogre::Vector3(Vector.X, Vector.Z, Vector.Y);
        };

        /// <summary>
        /// Creates a V3 from an Ogre Vector3 instance
        /// with Y, Z parts flipped.
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Meridian59::Common::V3 ToV3YZFlipped(::Ogre::Vector3 Vector)
        {
            return ::Meridian59::Common::V3(Vector.x, Vector.z, Vector.y);
        };

        /// <summary>
        /// Creates an Ogre Vector2 from a V2 instance
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::Vector2 ToOgre(V2 Vector)
        {
            return ::Ogre::Vector2(Vector.X, Vector.Y);
        };

		/// <summary>
        /// Creates an Ogre Vector2 from a V2 instance with X,Y flipped
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Ogre::Vector2 ToOgreXYFlipped(V2 Vector)
        {
            return ::Ogre::Vector2(Vector.Y, Vector.X);
        };

        /// <summary>
        /// Creates a V2 instance from Ogre Vector2
        /// </summary>
        /// <param name="Vector"></param>
        /// <returns></returns>
        __forceinline static ::Meridian59::Common::V2 ToV2(::Ogre::Vector2* Vector)
        {
			::Meridian59::Common::V2 val;
			val.X = Vector->x;
			val.Y = Vector->y;

            return val;
        };
        #pragma endregion

        #pragma region Orientation
        /// <summary>
        /// Converts Ogre Orientation to a radian angle orientation.
        /// </summary>
        /// <param name="Orientation"></param>
        /// <returns></returns>
        __forceinline static float ToRadianAngle(Quaternion* Orientation)
        {
            // normalized direction of nodeorientation on ground plane           
			V2 direction;
			direction.X = Orientation->xAxis().z;
			direction.Y = -Orientation->xAxis().x;
			
            // return radian angle
            return MathUtil::GetRadianForDirection(direction);     
        };

        /// <summary>
        /// Rotates the SceneNode to match an angle in radians.
        /// </summary>
        /// <param name="SceneNode"></param>
        /// <param name="RadianAngle"></param>
        __inline static void SetOrientationFromAngle(SceneNode* SceneNode, float RadianAngle)
        {
            // add quarter of full period offset
            RadianAngle += GeometryConstants::QUARTERPERIOD;
            
            // get ogre radian from value
            Radian rRadian = Radian(RadianAngle);
          
            // reset and yaw to given angle
            SceneNode->resetOrientation();
            SceneNode->yaw(-rRadian);
        };
        #pragma endregion
		
        #pragma region Light		
        /// <summary>
        /// Converts a server ushort LightColor to an Ogre RGB color
        /// </summary>
        /// <param name="LightColor"></param>
        /// <returns></returns>
        __inline static ColourValue LightColorToOgreRGB(ushort LightColor)
        {
            // decode color from ushort (see d3drender.c for formulas)
            int r = ((LightColor >> 10) & 31) * 255 / 31;
            int g = ((LightColor >> 5) & 31) * 255 / 31;
            int b = (LightColor & 31) * 255 / 31;
            
			return ColourValue(r / 255.0f, g / 255.0f, b / 255.0f);
        };

        /// <summary>
        /// Converts a GrayScale value (0-255) to an Ogre RGB color
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        __inline static ColourValue LightIntensityToOgreRGB(unsigned char Value)
        {
            float ratio = (float)Value / 255.0f;          
			return ColourValue(ratio, ratio, ratio);
        };

        /// <summary>
        /// Sets Ogre light parameters (color/attenuation) according to M59 info.
        /// </summary>
        /// <param name="Light"></param>
        /// <param name="LightOwner"></param>
        static void UpdateFromILightOwner(Light* Light, ILightOwner^ LightOwner)
        {
            // decode color from ushort (see d3drender.c for formulas)                
            ColourValue baseColor = Util::LightColorToOgreRGB(LightOwner->LightColor);

            // brigthness ratio (between 0.0 and 1.0)
            float ratio = (float)LightOwner->LightIntensity / 255.0f;

            // set colors based on decoded color and intensity ratio
            Light->setDiffuseColour(baseColor);
            Light->setSpecularColour(baseColor);

            // attenuation scaling
            float range = 0.0f;
            float constant = 1.0f;
            float linear = 0.01f; // / ratio;
            float quadratic = 0.0001f;// / (ratio * ratio);

            if (LightOwner->LightIntensity > 0)
                range = 120.0f + 460.0f * ratio;

            Light->setAttenuation(range, constant, linear, quadratic);
        };

        /// <summary>
        /// Possibly creates an Ogre Light from M59 light parameters,
        /// if light parameters indicate so.
        /// </summary>
        /// <param name="LightOwner">The object having the M59 light</param>
        /// <param name="SceneManager">SceneManager used for creation</param>
        /// <param name="LightName">Unique name of the light</param>
        /// <returns>Ogre Light instance or NULL</returns>
        static ::Ogre::Light* CreateLight(
			::Meridian59::Common::Interfaces::ILightOwner^ LightOwner, 
			::Ogre::SceneManager* SceneManager, 
			::Ogre::String LightName)
        {
			::Ogre::Light* light = nullptr;

			// check if the m59 object is supposed to have a light
            if (LightOwner->LightFlags > 0)
            {
				// create ogre light
                light = SceneManager->createLight(LightName);

				// set type to pointlight
                light->setType(Light::LightTypes::LT_POINT);
			
                // set color/attenuation
                UpdateFromILightOwner(light, LightOwner);

                return light;
            }
            
			return light;
        };
        #pragma endregion
		
		#pragma region Textures & Materials

		/// <summary>
        /// Creates an Ogre texture from the Meridian 59 color palettes.
		/// This will be a A8R8G8B8 texture of size 256x256.
		/// It will store all the colors of one palette in one row.
        /// </summary>
        /// <param name="TextureName">Name of new Ogre texture</param>
        /// <param name="TextureGroup">ResourceGroup of new Ogre texture</param>
		static void CreatePalettesTexture(::Ogre::String TextureName, ::Ogre::String TextureGroup)
		{
			TextureManager* texMan	= TextureManager::getSingletonPtr();
			TexturePtr texPtr		= texMan->getByName(TextureName, TextureGroup);
			
			// if the texture does not yet exist, create it
            if (texPtr.isNull())
            {
				texPtr = texMan->createManual(
					TextureName, TextureGroup,
					::Ogre::TextureType::TEX_TYPE_2D,
					::Meridian59::Drawing2D::ColorTransformation::COLORCOUNT, 
					::Meridian59::Drawing2D::ColorTransformation::PALETTECOUNT, 
					0,
					::Ogre::PixelFormat::PF_A8R8G8B8,
					::Ogre::TU_DEFAULT, 0, false, 0);
	
				// lock
				unsigned int* texBuffer = 
					(unsigned int*)texPtr->getBuffer()->lock(::Ogre::HardwareBuffer::LockOptions::HBL_WRITE_ONLY);
				
				int k = 0;
				for(int i = 0; i < ::Meridian59::Drawing2D::ColorTransformation::PALETTECOUNT; i++)
				{
					for (int j = 0; j < ::Meridian59::Drawing2D::ColorTransformation::COLORCOUNT; j++)
					{
						texBuffer[k] = ::Meridian59::Drawing2D::ColorTransformation::Palettes[i][j];
						k++;
					}		
				}

				// unlock
				texPtr->getBuffer()->unlock();
			}
		};

		/// <summary>
        /// Creates A8 Ogre texture from 8-Bit indexed BgfBitmap.
		/// The alpha channel will store the palette index.
		/// So this is some kind of "native" 8-Bit texture.
        /// </summary>
        /// <param name="BgfBitmap">BgfBitmap instance (source)</param>
        /// <param name="TextureName">Name of new Ogre texture</param>
        /// <param name="TextureGroup">ResourceGroup of new Ogre texture</param>
		static void CreateTextureA8(
			::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, 
			::Ogre::String TextureName, 
			::Ogre::String TextureGroup)
		{
			if (!BgfBitmap || BgfBitmap->Width == 0 || BgfBitmap->Height == 0)
				return;

			TextureManager* texMan	= TextureManager::getSingletonPtr();
			TexturePtr texPtr		= texMan->getByName(TextureName, TextureGroup);
			
			// if the texture does not yet exist, create it
            if (texPtr.isNull())
            {
				unsigned short width, height;
                HardwarePixelBufferSharedPtr pixPtr;
                                  
                width = (unsigned short)BgfBitmap->Width;
                height = (unsigned short)BgfBitmap->Height;
                        
                // create manual (empty) texture
                texPtr = texMan->createManual(
                    TextureName,
                    TextureGroup,
                    TextureType::TEX_TYPE_2D,
                    width, height, 0,
                    ::Ogre::PixelFormat::PF_A8,
					TU_DEFAULT, 0, false, 0);
                            
                // get pixelbuffer
                pixPtr = texPtr->getBuffer();
                        
                // lock it
				void* ptr = pixPtr->lock(HardwareBuffer::LockOptions::HBL_WRITE_ONLY);
                            
                // fill the pixeldata to buffer
				BgfBitmap->FillPixelDataTo((::System::IntPtr)ptr, false, true);

                // unlock buffer
                pixPtr->unlock();

                // cleanup
                pixPtr.setNull();
                texPtr.setNull();
            }
		};

		/// <summary>
        /// Creates A8R8G8B8 Ogre texture from 8-Bit indexed BgfBitmap.
		/// A fixed default palette with black transparency will be used.
		/// Will only do something, if there is no texture with that name yet.
		/// For creating images of objects see ImageComposers.
        /// </summary>
        /// <param name="BgfBitmap">BgfBitmap instance (source)</param>
        /// <param name="TextureName">Name of new Ogre texture</param>
        /// <param name="TextureGroup">ResourceGroup of new Ogre texture</param>
        static void CreateTextureA8R8G8B8(
			::Meridian59::Files::BGF::BgfBitmap^ BgfBitmap, 
			::Ogre::String TextureName, 
			::Ogre::String TextureGroup,
			int Mipmaps)
		{
			if (!BgfBitmap || BgfBitmap->Width == 0 || BgfBitmap->Height == 0)
				return;

			TextureManager* texMan	= TextureManager::getSingletonPtr();
			TexturePtr texPtr		= texMan->getByName(TextureName, TextureGroup);
			
			// if the texture does not yet exist, create it
            if (texPtr.isNull())
            {
				unsigned short width, height;
                HardwarePixelBufferSharedPtr pixPtr;
                                  
                width = (unsigned short)BgfBitmap->Width;
                height = (unsigned short)BgfBitmap->Height;
                        
                // create manual (empty) texture
                texPtr = texMan->createManual(
                    TextureName,
                    TextureGroup,
                    TextureType::TEX_TYPE_2D,
                    width, height, Mipmaps,
                    ::Ogre::PixelFormat::PF_A8R8G8B8,
					TU_DEFAULT, 0, false, 0);
                            
                // get pixelbuffer
                pixPtr = texPtr->getBuffer();
                        
                // lock it
				void* ptr = pixPtr->lock(HardwareBuffer::LockOptions::HBL_WRITE_ONLY);
                            
                // fill the pixeldata to buffer
				BgfBitmap->FillPixelDataAsA8R8G8B8TransparencyBlack((unsigned int*)ptr, width);

                // unlock buffer
                pixPtr->unlock();

                // cleanup
                pixPtr.setNull();
                texPtr.setNull();
            }
		};
		
		/// <summary>
        /// Creates A8R8G8B8 Ogre texture from A8R8G8B8 CLR Bitmap.
		/// Only if there is no texture with that name yet.
        /// </summary>
        /// <param name="Bitmap">CLR Bitmap instance</param>
        /// <param name="TextureName">Name of new Ogre texture</param>
        /// <param name="TextureGroup">ResourceGroup of new Ogre texture</param>
        static void CreateTexture(
			::System::Drawing::Bitmap^ Bitmap, 
			::Ogre::String TextureName, 
			::Ogre::String TextureGroup)
		{
			if (!Bitmap)
				return;

			TextureManager* texMan = TextureManager::getSingletonPtr();
			
			// if the texture does not yet exist, create it
            if (!texMan->resourceExists(TextureName))
            {
				unsigned short width = (unsigned short)Bitmap->Width;
				unsigned short height = (unsigned short)Bitmap->Height;

				const System::Drawing::Rectangle rectangle = 
					System::Drawing::Rectangle(0, 0, Bitmap->Width, Bitmap->Height);

				// lock source bitmap to read pixeldata
				System::Drawing::Imaging::BitmapData^ bmpData = Bitmap->LockBits(
					rectangle,
					System::Drawing::Imaging::ImageLockMode::ReadOnly,
					System::Drawing::Imaging::PixelFormat::Format32bppArgb);
			
				// create manual (empty) texture
				TexturePtr texPtr = texMan->createManual(
					TextureName,
					TextureGroup,
					TextureType::TEX_TYPE_2D,
					width, height, MIP_DEFAULT,
					::Ogre::PixelFormat::PF_A8R8G8B8,
					TU_DEFAULT, 0, false, 0);

				// get pointer to pixelbuf
				HardwarePixelBufferSharedPtr pixPtr = texPtr->getBuffer();

				PixelBox box = PixelBox(width, height, 1, ::Ogre::PixelFormat::PF_A8R8G8B8, (void*)bmpData->Scan0);

				// get pixels from souce bitmap
				pixPtr->blitFromMemory(box);
          
				// unlock source bitmap
				Bitmap->UnlockBits(bmpData);

				// cleanup
				pixPtr.setNull();
				texPtr.setNull();
			}
		};
		
		/// <summary>
        /// Makes an OgreTexture available as image to CEGUI.
        /// </summary>
        /// <param name="Bitmap">CLR Bitmap instance</param>
		static void CreateCEGUITextureFromOgre(CEGUI::OgreRenderer* OgreRenderer, TexturePtr OgreTexture)
		{
			if (!OgreTexture.isNull() && OgreRenderer != nullptr)
			{
				// get name of texture
				::Ogre::String texName = OgreTexture->getName();

				if (!OgreRenderer->isTextureDefined(texName))
				{					
					// make ogre texture visible to CEGUI
					CEGUI::Texture* mTexture = &OgreRenderer->createTexture(texName, OgreTexture);

					// define image (use same name)
					CEGUI::ImageManager* imgMan = CEGUI::ImageManager::getSingletonPtr();

					if (!imgMan->isDefined(texName))
					{
						CEGUI::BasicImage* img = (CEGUI::BasicImage*)&imgMan->create("BasicImage", texName);

						// apply texture to image and set size (because of subimaging)
						img->setTexture(mTexture);
						img->setArea(CEGUI::Rectf(0.0f, 0.0f, (float)OgreTexture->getWidth(), (float)OgreTexture->getHeight()));
					}
				}
			}
		};
		
		/// <summary>
        /// Clones the base material to a new material and applies a texture.
		/// Only if there is no material with that name yet.
        /// </summary>
        /// <param name="MaterialName">Name of new material</param>
        /// <param name="TextureName">Name of texture to set on new material</param>
        /// <param name="MaterialGroup">ResourceGroup of new material</param>
		/// <param name="ScrollSpeed">NULL (default) or texture scrolling speed</param>
		/// <param name="ColorModifier">NULL (= 1 1 1 1) or a vector which components get multiplied with light components</param>
		static void CreateMaterial(
			::Ogre::String MaterialName, 
			::Ogre::String TextureName, 
			::Ogre::String MaterialGroup, 
			::Ogre::Vector2* ScrollSpeed, 
			::Ogre::Vector4* ColorModifier)
		{
			MaterialManager* matMan = MaterialManager::getSingletonPtr();
			
			// if no material if the targetname exists
			if (!matMan->resourceExists(MaterialName))
			{
				// try to get existing base material
				MaterialPtr baseMaterial = 
					matMan->getByName(BASEMATERIAL, RESOURCEGROUPSHADER);

				if (!baseMaterial.isNull())
				{
					// clone base material to different group
					MaterialPtr matPtr = baseMaterial->clone(MaterialName, true, MaterialGroup);

					// set the texture_unit part with name of the texture
					AliasTextureNamePairList pairs = AliasTextureNamePairList();
					pairs[TEXTUREUNITALIAS] = TextureName;

					// apply texture name
					matPtr->applyTextureAliases(pairs);
					
					// get shader passes (0 = ambient, 1 = diffuse pointlights)
					Pass* ambientPass = matPtr->getTechnique(0)->getPass(0);
					Pass* diffusePass = matPtr->getTechnique(0)->getPass(1);

					// get fragment shader parameters from ambient pass					
					const GpuProgramParametersSharedPtr paramsAmbient = 
						ambientPass->getFragmentProgramParameters();
					
					// get fragment shader parameters from diffuse pass				
					const GpuProgramParametersSharedPtr paramsDiffuse = 
						diffusePass->getFragmentProgramParameters();
					
					// apply a custom color modifier on the shaders
					// its components get multiplied with the color components
					if (ColorModifier != nullptr)
					{					
						// set the light modifier on ambient pass params
						paramsAmbient->setNamedConstant(SHADERCOLORMODIFIER, *ColorModifier);
												
						// set the light modifier on pointlight pass params
						paramsDiffuse->setNamedConstant(SHADERCOLORMODIFIER, *ColorModifier);						
					}

					// apply a scrolling if set
					if (ScrollSpeed != nullptr)											
						ambientPass->getTextureUnitState(0)->setScrollAnimation(ScrollSpeed->x, ScrollSpeed->y);
					
					// cleanup
					baseMaterial.setNull();
					matPtr.setNull();
				}
			}
		};
		
		/// <summary>
		/// Clones the base water material to a new material and applies a texture.
		/// Only if there is no material with that name yet.
		/// </summary>
		/// <param name="MaterialName">Name of new material</param>
		/// <param name="TextureName">Name of texture to set on new material</param>
		/// <param name="MaterialGroup">ResourceGroup of new material</param>
		/// <param name="ScrollSpeed">NULL (default) or texture scrolling speed</param>
		static void CreateMaterialWater(
			::Ogre::String MaterialName,
			::Ogre::String TextureName,
			::Ogre::String MaterialGroup,
			::Ogre::Vector2* ScrollSpeed)
		{
			MaterialManager* matMan = MaterialManager::getSingletonPtr();

			// if no material if the targetname exists
			if (matMan->resourceExists(MaterialName))
				return;

			// try to get existing base material
			MaterialPtr baseMaterial =
				matMan->getByName(BASEMATERIALWATER, RESOURCEGROUPSHADER);

			if (baseMaterial.isNull())
				return;
	
			// clone base material to different group
			MaterialPtr matPtr = baseMaterial->clone(MaterialName, true, MaterialGroup);
			
			// set the texture_unit part with name of the texture
			AliasTextureNamePairList pairs = AliasTextureNamePairList();
			pairs[TEXTUREUNITALIAS] = TextureName;

			// apply texture name
			matPtr->applyTextureAliases(pairs);

			// get shader passes (0 = ambient, 1 = diffuse pointlights)
			Pass* ambientPass = matPtr->getTechnique(0)->getPass(0);
			//Pass* diffusePass = matPtr->getTechnique(0)->getPass(1);

			// get vertex shader parameters from ambient pass					
			const GpuProgramParametersSharedPtr paramsAmbient =
				ambientPass->getVertexProgramParameters();

			// get fragment shader parameters from diffuse pass				
			//const GpuProgramParametersSharedPtr paramsDiffuse =
			//	diffusePass->getFragmentProgramParameters();

			// set scrollspeed
			if (ScrollSpeed)
				paramsAmbient->setNamedConstant("waveSpeed", 0.3f * -(*ScrollSpeed));
			
			// cleanup
			baseMaterial.setNull();
			matPtr.setNull();			
		};

		/// <summary>
        /// Clones the base invisible material to a new material and applies a texture.
		/// Only if there is no material with that name yet.
        /// </summary>
        /// <param name="MaterialName">Name of new material</param>
        /// <param name="TextureName">Name of texture to set on new material</param>
        /// <param name="MaterialGroup">ResourceGroup of new material</param>
		static void CreateMaterialInvisible(
			::Ogre::String MaterialName, 
			::Ogre::String TextureName, 
			::Ogre::String MaterialGroup)
		{
			MaterialManager* matMan = MaterialManager::getSingletonPtr();
			
			// if no material if the targetname exists
			if (!matMan->resourceExists(MaterialName))
			{
				// try to get existing base material
				MaterialPtr baseMaterial = 
					matMan->getByName(BASEMATERIALINVISIBLE, RESOURCEGROUPSHADER);

				if (!baseMaterial.isNull())
				{
					// clone base material to different group
					MaterialPtr matPtr = baseMaterial->clone(MaterialName, true, MaterialGroup);

					// set the texture_unit part with name of the texture
					AliasTextureNamePairList pairs = AliasTextureNamePairList();
					pairs[TEXTUREUNITALIAS] = TextureName;

					// apply texture name
					matPtr->applyTextureAliases(pairs);
					
					// get shader passes (0 = ambient, 1 = diffuse pointlights)
					//Pass* ambientPass = matPtr->getTechnique(0)->getPass(0);
					
					// get fragment shader parameters from ambient pass					
					//const GpuProgramParametersSharedPtr paramsAmbient = 
					//	ambientPass->getFragmentProgramParameters();
															
					// set opaque values
					//paramsAmbient->setNamedConstant(SHADEROPAQUE, Opaque);
					
					// cleanup
					baseMaterial.setNull();
					matPtr.setNull();
				}
			}
		};
		#pragma endregion
		
		#pragma region Others	
		/// <summary>
        /// Returns a memorystream from Ogre DataStreamPtr
        /// </summary>
        /// <param name="dataPtr"></param>
        /// <returns></returns>
        static ::System::IO::MemoryStream^ DataPtrToStream(::Ogre::DataStreamPtr dataPtr)
        {
            if (dataPtr->size() > 0)
            {
                array<unsigned char>^ buffer = gcnew array<unsigned char>(dataPtr->size());
				pin_ptr<unsigned char> p = &buffer[0];

                //Read buffer.Length amount of data into bufferPtr
                dataPtr->read((void*)p, (uint)dataPtr->size());
                
                System::IO::MemoryStream^ stream = gcnew MemoryStream(buffer);

                return stream;

            }
            return nullptr;
        };
		
		/// <summary>
        /// Gets the boundingbox height of the biggest attached MovableObject
        /// </summary>
        /// <param name="Filename">Full path and filename to write</param>
		static float GetSceneNodeHeight(SceneNode* SceneNode)
		{
			// put name above of node
			// since _getWorldAABB is not updated yet, we calculate the height			
			::Ogre::SceneNode::ObjectIterator iterator = 
				SceneNode->getAttachedObjectIterator();

			float maxheight = 0.0f;
			float candidate;
			MovableObject* obj;

			while(iterator.hasMoreElements())
			{
				obj = iterator.getNext();
				candidate = obj->getBoundingBox().getSize().y;

				if (candidate > maxheight)
					maxheight = candidate;
			}

			return maxheight;
		};
		#pragma endregion
		
		static bool GetScreenspaceCoords(::Ogre::MovableObject* object, ::Ogre::Camera* camera, ::Ogre::Vector2& result)
		{
			if(!object->isInScene())
				return false;
 
			const ::Ogre::AxisAlignedBox &AABB = object->getWorldBoundingBox(true);
 
			/**
			* If you need the point above the object instead of the center point:
			* This snippet derives the average point between the top-most corners of the bounding box
			* Ogre::Vector3 point = (AABB.getCorner(AxisAlignedBox::FAR_LEFT_TOP)
			*    + AABB.getCorner(AxisAlignedBox::FAR_RIGHT_TOP)
			*    + AABB.getCorner(AxisAlignedBox::NEAR_LEFT_TOP)
			*    + AABB.getCorner(AxisAlignedBox::NEAR_RIGHT_TOP)) / 4;
			*/
 
			// Get the center point of the object's bounding box
			::Ogre::Vector3 point = AABB.getCenter();
 
			// Is the camera facing that point? If not, return false
			::Ogre::Plane cameraPlane = ::Ogre::Plane(::Ogre::Vector3(camera->getDerivedOrientation().zAxis()), camera->getDerivedPosition());
			if(cameraPlane.getSide(point) != ::Ogre::Plane::NEGATIVE_SIDE)
				return false;
 
			// Transform the 3D point into screen space
			point = camera->getProjectionMatrix() * (camera->getViewMatrix() * point);
 
			// Transform from coordinate space [-1, 1] to [0, 1] and update in-value
			result.x = (point.x / 2) + 0.5f;
			result.y = 1 - ((point.y / 2) + 0.5f);
 
			return true;
		};

		/// <summary>
        /// Exports all Materials to a set of .material files.
        /// </summary>
        /// <param name="Folder"></param>
        static void ExportMaterials(::Ogre::String Folder)
		{
			//::Ogre::MaterialSerializer matSerializerModels = ::Ogre::MaterialSerializer();
			/*::Ogre::MaterialSerializer matSerializerRoom = MaterialSerializer();
			::Ogre::MaterialSerializer matSerializerRemoteNode2D = MaterialSerializer();
			::Ogre::MaterialSerializer matSerializerMovableText = MaterialSerializer();
			::Ogre::MaterialSerializer matSerializerProjectileNode = MaterialSerializer();*/

            /*::Ogre::ResourceManager::ResourceMapIterator iterator = 
				::Ogre::MaterialManager::getSingletonPtr()->getResourceIterator();

            while (iterator.hasMoreElements())
			{
				::Ogre::MaterialPtr matPtr = iterator.getNext().staticCast<::Ogre::Material>();
                
				if (matPtr->getGroup() == RESOURCEGROUPMODELS)
					matSerializerModels.queueForExport(matPtr);

				else if (matPtr->getGroup() == MATERIALGROUP_ROOLOADER)
					matSerializerRoom.queueForExport(matPtr);

				else if (matPtr->getGroup() == MATERIALGROUP_REMOTENODE2D)
					matSerializerRemoteNode2D.queueForExport(matPtr);

				else if (matPtr->getGroup() == MATERIALGROUP_MOVABLETEXT)
					matSerializerMovableText.queueForExport(matPtr);

				else if (matPtr->getGroup() == MATERIALGROUP_PROJECTILENODE2D)
					matSerializerProjectileNode.queueForExport(matPtr);
			}

			::Ogre::String fileModels = ::Ogre::String(Folder);
			::Ogre::String fileRoom = ::Ogre::String(Folder);
			::Ogre::String fileRemoteNode2D = ::Ogre::String(Folder);
			::Ogre::String fileMovableText = ::Ogre::String(Folder);
			::Ogre::String fileProjectileNode = ::Ogre::String(Folder);

			fileModels.append("/models.material");
			fileRoom.append("/room.material");
			fileRemoteNode2D.append("/remotenode2d.material");
			fileMovableText.append("/movabletext.material");
			fileProjectileNode.append("/projectilenode.material");
			
            matSerializerModels.exportQueued(fileModels);
			matSerializerRoom.exportQueued(fileRoom);
			matSerializerRemoteNode2D.exportQueued(fileRemoteNode2D);
			matSerializerMovableText.exportQueued(fileMovableText);
			//matSerializerProjectileNode.exportQueued(fileProjectileNode);*/
		};
	};
};};