#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::Branding::Initialize()
   {
      // setup references to children from xml nodes
      Logo   = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_BRANDINGLOGO_WINDOW));

      // set according branding logo
#if VANILLA
      Logo->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_BRANDING_VANILLA);
#elif OPENMERIDIAN
      Logo->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_BRANDING_OPENMERIDIAN);
#else
      Logo->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_BRANDING_MERIDIANNEXT);
#endif
   };

   void ControllerUI::Branding::Destroy()
   {
   };
};};
