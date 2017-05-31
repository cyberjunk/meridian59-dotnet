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
#include "OgreCompositorManager.h"
#include "OgreCompositorChain.h"
#include "OgreCompositionPass.h"
#include "OgreCompositionTargetPass.h"
#pragma managed(pop)

#include "OgreListeners.h"

namespace Meridian59 { namespace Ogre 
{
   using namespace System::ComponentModel;
   using namespace Meridian59::Data;
   using namespace Meridian59::Data::Lists;
   using namespace Meridian59::Data::Models;

   /// <summary>
   /// Handles effects
   /// </summary>
   public ref class ControllerEffects abstract sealed
   {
   private:
      static ::Ogre::CompositorInstance* compInvert;
      static ::Ogre::CompositorInstance* compPain;
      static ::Ogre::CompositorInstance* compBlack;
      static ::Ogre::CompositorInstance* compWhiteout;
      static ::Ogre::CompositorInstance* compBlur;
      static ::Ogre::CompositorInstance* compBlend;

      static CompositorPainListener* listenerPain;
      static CompositorWhiteoutListener* listenerWhiteout;

      static ControllerEffects(void);

      static void EffectInvertPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
      static void EffectBlindPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
      static void EffectPainPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
      static void EffectWhiteoutPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
      static void EffectBlurPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);
      static void EffectFlashXLatPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e);

   public:
      /// <summary>
      /// Initialization state
      /// </summary>
      static bool IsInitialized;

      static void Initialize();
      static void Destroy();
   };
};};
