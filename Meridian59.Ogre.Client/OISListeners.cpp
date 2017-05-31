#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   OISKeyListener::OISKeyListener(void)
   {
   };

   bool OISKeyListener::keyPressed(const OIS::KeyEvent &arg)
   {
      return ControllerInput::OISKeyboard_KeyPressed(arg);
   };

   bool OISKeyListener::keyReleased(const OIS::KeyEvent &arg) 
   {
      return ControllerInput::OISKeyboard_KeyReleased(arg);
   };

   /////////////////////////////////////////////////////////////////////////////////////////////////////

   OISMouseListener::OISMouseListener(void)
   {
   };

   bool OISMouseListener::mouseMoved( const OIS::MouseEvent &arg )
   {
      return ControllerInput::OISMouse_MouseMoved(arg);
   };

   bool OISMouseListener::mousePressed( const OIS::MouseEvent &arg, OIS::MouseButtonID id )
   {
      return ControllerInput::OISMouse_MousePressed(arg, id);
   };

   bool OISMouseListener::mouseReleased( const OIS::MouseEvent &arg, OIS::MouseButtonID id )
   {
      return ControllerInput::OISMouse_MouseReleased(arg, id);
   };
};};
