#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	static ControllerEffects::ControllerEffects(void)
	{
		compInvert		= nullptr;
		compPain		= nullptr;
		compBlack		= nullptr;
		compWhiteout	= nullptr;
		compBlur		= nullptr;
		compBlend		= nullptr;
		
		listenerPain	 = nullptr;
		listenerWhiteout = nullptr;
	};

	void ControllerEffects::Initialize()
	{
		if (IsInitialized)
			return;

		Effects^ effects = OgreClient::Singleton->Data->Effects;

		// attach propertychanged event to effects in Data
		effects->Blind->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectBlindPropertyChanged);

		effects->Invert->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectInvertPropertyChanged);

		effects->Pain->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectPainPropertyChanged);

		effects->Whiteout->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectWhiteoutPropertyChanged);

		effects->Blur->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectBlurPropertyChanged);

		effects->FlashXLat->PropertyChanged += 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectFlashXLatPropertyChanged);

		// register compositors
		::Ogre::CompositorManager* compMan	= ::Ogre::CompositorManager::getSingletonPtr();
		::Ogre::Viewport* viewport			= OgreClient::Singleton->Viewport;

		// compositors using blend mechanism
		compBlack		= compMan->addCompositor(viewport, COMPOSITOR_BLACK);
		compWhiteout	= compMan->addCompositor(viewport, COMPOSITOR_WHITEOUT);
		compPain		= compMan->addCompositor(viewport, COMPOSITOR_PAIN);
		compBlend		= compMan->addCompositor(viewport, COMPOSITOR_BLEND);

		// other compositors
		compInvert	= compMan->addCompositor(viewport, COMPOSITOR_INVERT);
		compBlur	= compMan->addCompositor(viewport, COMPOSITOR_BLUR);
		
		// create listeners
		listenerPain = new CompositorPainListener();
		listenerWhiteout = new CompositorWhiteoutListener();

		// attach listeners for compositors which need shaderparam updates at runtime
		compPain->addListener(listenerPain);
		compWhiteout->addListener(listenerWhiteout);

		// mark initialized
		IsInitialized = true;
	};

	void ControllerEffects::Destroy()
	{
		if (!IsInitialized)
			return;

		Effects^ effects = OgreClient::Singleton->Data->Effects;

		// detach propertychanged event to effects in Data
		effects->Blind->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectBlindPropertyChanged);

		effects->Invert->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectInvertPropertyChanged);

		effects->Pain->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectPainPropertyChanged);

		effects->Whiteout->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectWhiteoutPropertyChanged);

		effects->Blur->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectBlurPropertyChanged);

		effects->FlashXLat->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(&ControllerEffects::EffectFlashXLatPropertyChanged);
		
		compPain->removeListener(listenerPain);
		compWhiteout->removeListener(listenerWhiteout);
			
		delete listenerPain;
		delete listenerWhiteout;
				
		// remove compositors
		::Ogre::CompositorManager* compMan	= ::Ogre::CompositorManager::getSingletonPtr();
		::Ogre::Viewport* viewport			= OgreClient::Singleton->Viewport;

		compMan->removeCompositor(viewport, COMPOSITOR_BLACK);
		compMan->removeCompositor(viewport, COMPOSITOR_WHITEOUT);
		compMan->removeCompositor(viewport, COMPOSITOR_PAIN);
		compMan->removeCompositor(viewport, COMPOSITOR_BLEND);
		compMan->removeCompositor(viewport, COMPOSITOR_INVERT);
		compMan->removeCompositor(viewport, COMPOSITOR_BLUR);
		
		compInvert		= nullptr;
		compPain		= nullptr;
		compBlack		= nullptr;
		compWhiteout	= nullptr;
		compBlur		= nullptr;
		compBlend		= nullptr;
		
		listenerPain	 = nullptr;
		listenerWhiteout = nullptr;

		// mark not initialized
		IsInitialized = false;
	};

	void ControllerEffects::EffectBlindPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (CLRString::Equals(e->PropertyName, EffectBlind::PROPNAME_ISACTIVE))
		{			
			CompositorManager* compMan = CompositorManager::getSingletonPtr();
		
			// build a full black blendcolor
			float blendcolor[4];
			blendcolor[0] = 0.0f;  // r
			blendcolor[1] = 0.0f;  // g
			blendcolor[2] = 0.0f;  // b
			blendcolor[3] = 1.0f;  // a

			// get pass
			CompositionTargetPass* compPass = 
				compBlack->getTechnique()->getOutputTargetPass();

			// get material
			const MaterialPtr& matPtr = 
				compPass->getPass(0)->getMaterial();
         
			// get shader parameters
			const GpuProgramParametersSharedPtr& list = 
				matPtr->getTechnique(0)->getPass(0)->getFragmentProgramParameters();

			// set blendcolor
			list->setNamedConstant(SHADERBLENDCOLOR, blendcolor, 1);
			
			compMan->setCompositorEnabled(
				OgreClient::Singleton->Viewport, 
				COMPOSITOR_BLACK, 
				OgreClient::Singleton->Data->Effects->Blind->IsActive);
		}
	};

	void ControllerEffects::EffectInvertPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (CLRString::Equals(e->PropertyName, EffectInvert::PROPNAME_ISACTIVE))
		{		
			CompositorManager* compMan = CompositorManager::getSingletonPtr();
		
			compMan->setCompositorEnabled(
				OgreClient::Singleton->Viewport, 
				COMPOSITOR_INVERT, 
				OgreClient::Singleton->Data->Effects->Invert->IsActive);
		}
	};

	void ControllerEffects::EffectPainPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (CLRString::Equals(e->PropertyName, EffectPain::PROPNAME_ISACTIVE))
		{			
			CompositorManager* compMan = CompositorManager::getSingletonPtr();

			compMan->setCompositorEnabled(
				OgreClient::Singleton->Viewport, 
				COMPOSITOR_PAIN, 
				OgreClient::Singleton->Data->Effects->Pain->IsActive);
		}
	};

	void ControllerEffects::EffectWhiteoutPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (CLRString::Equals(e->PropertyName, EffectWhiteOut::PROPNAME_ISACTIVE))
		{			
			CompositorManager* compMan = CompositorManager::getSingletonPtr();

			compMan->setCompositorEnabled(
				OgreClient::Singleton->Viewport, 
				COMPOSITOR_WHITEOUT, 
				OgreClient::Singleton->Data->Effects->Whiteout->IsActive);
		}
	};

	void ControllerEffects::EffectBlurPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		if (CLRString::Equals(e->PropertyName, EffectBlur::PROPNAME_ISACTIVE))
		{			
			CompositorManager* compMan = CompositorManager::getSingletonPtr();

			compMan->setCompositorEnabled(
				OgreClient::Singleton->Viewport, 
				COMPOSITOR_BLUR, 
				OgreClient::Singleton->Data->Effects->Blur->IsActive);
		}
	};

	void ControllerEffects::EffectFlashXLatPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		CompositorManager* compMan = CompositorManager::getSingletonPtr();
			
		if (CLRString::Equals(e->PropertyName, EffectFlashXLat::PROPNAME_ISACTIVE))
		{		
			if (OgreClient::Singleton->Data->Effects->FlashXLat->IsActive)
			{
				// used blendcolor
				float blendcolor[4];
				
				switch (OgreClient::Singleton->Data->Effects->FlashXLat->XLat)
				{
					/////// RED 10-100% //////

					case ColorTransformation::BLEND10RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.1f;
						break;

					case ColorTransformation::BLEND20RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.2f;
						break;

					case ColorTransformation::BLEND30RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.3f;
						break;

					case ColorTransformation::BLEND40RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.4f;
						break;

					case ColorTransformation::BLEND50RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.5f;
						break;

					case ColorTransformation::BLEND60RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.6f;
						break;

					case ColorTransformation::BLEND70RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.7f;
						break;

					case ColorTransformation::BLEND80RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.8f;
						break;

					case ColorTransformation::BLEND90RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.9f;
						break;

					case ColorTransformation::BLEND100RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 1.0f;
						break;
					
					/////// WHITE 10-100% //////

					case ColorTransformation::BLEND10WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.1f;
						break;

					case ColorTransformation::BLEND20WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.2f;
						break;

					case ColorTransformation::BLEND30WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.3f;
						break;

					case ColorTransformation::BLEND40WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.4f;
						break;

					case ColorTransformation::BLEND50WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.5f;
						break;

					case ColorTransformation::BLEND60WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.6f;
						break;

					case ColorTransformation::BLEND70WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.7f;
						break;

					case ColorTransformation::BLEND80WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.8f;
						break;

					case ColorTransformation::BLEND90WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.9f;
						break;

					case ColorTransformation::BLEND100WHITE:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 1.0f;
						break;
					
					/////// OTHERS //////

					case ColorTransformation::BLEND25RED:
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.25f;
						break;

					case ColorTransformation::BLEND25BLUE:
						blendcolor[0] = 0.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.25f;
						break;

					case ColorTransformation::BLEND25GREEN:						
						blendcolor[0] = 0.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.25f;
						break;

					case ColorTransformation::BLEND50BLUE:						
						blendcolor[0] = 0.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.5f;
						break;

					case ColorTransformation::BLEND50GREEN:						
						blendcolor[0] = 0.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.5f;
						break;

					case ColorTransformation::BLEND75RED:						
						blendcolor[0] = 1.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.75f;
						break;

					case ColorTransformation::BLEND75BLUE:						
						blendcolor[0] = 0.0f;
						blendcolor[1] = 0.0f;
						blendcolor[2] = 1.0f;
						blendcolor[3] = 0.75f;
						break;

					case ColorTransformation::BLEND75GREEN:						
						blendcolor[0] = 0.0f;
						blendcolor[1] = 1.0f;
						blendcolor[2] = 0.0f;
						blendcolor[3] = 0.75f;
						break;
				}

				// get pass
				CompositionTargetPass* compPass = 
					compBlend->getTechnique()->getOutputTargetPass();

				// get material
				const MaterialPtr& matPtr = 
					compPass->getPass(0)->getMaterial();
         
				// get shader parameters
				const GpuProgramParametersSharedPtr& list = 
					matPtr->getTechnique(0)->getPass(0)->getFragmentProgramParameters();

				// set blendcolor
				list->setNamedConstant(SHADERBLENDCOLOR, blendcolor, 1);
			
				// enable or disable
				compMan->setCompositorEnabled(
					OgreClient::Singleton->Viewport, 
					COMPOSITOR_BLEND, 
					true);
			}
			else
			{
				//  disable
				compMan->setCompositorEnabled(
					OgreClient::Singleton->Viewport, 
					COMPOSITOR_BLEND, 
					false);
			}
		}
	};
};};
