#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::Branding::Initialize()
   {
      // setup references to children from xml nodes
      Logo = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_BRANDINGLOGO_WINDOW));
      Text = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_BRANDINGLOGO_TEXT));

      // get version as string
      const CEGUI::String strVersion = StringConvert::CLRToCEGUI(
         ::System::Reflection::Assembly::GetExecutingAssembly()->GetName()->Version->ToString());
       
      // get x64/x86 as string
      const CEGUI::String strArch = ::System::Environment::Is64BitProcess ? "x64" : "x86";

      // build branding base text with version, release/debug and 32/64 bit
      const CEGUI::String baseStr =
         "Version " + strVersion + "\n" +
         UI_BUILDTYPE + " (" + strArch + ")\n";

      // set according branding logo and text
#if VANILLA
      Logo->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_BRANDING_VANILLA);
      Text->setText(baseStr + "101 | 102");
#elif OPENMERIDIAN
      Logo->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_BRANDING_OPENMERIDIAN);
      Text->setText(baseStr + "103");
#else
      Logo->setProperty(UI_PROPNAME_IMAGE, UI_IMAGE_BRANDING_MERIDIANNEXT);
      Text->setText(baseStr + "105 | 112");
#endif
   };

   void ControllerUI::Branding::Destroy()
   {
   };
};};
