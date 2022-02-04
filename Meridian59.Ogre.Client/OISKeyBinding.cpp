#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   OISKeyBinding::OISKeyBinding(void)
   {
   };

   bool OISKeyBinding::IsMovementKey(::OIS::KeyCode keyCode)
   {
      if ((keyCode == MoveForward) ||
         (keyCode == MoveBackward) ||
         (keyCode == MoveLeft) ||
         (keyCode == MoveRight))
         return true;
      else return false;
   };

   bool OISKeyBinding::IsRotateKey(::OIS::KeyCode keyCode)
   {
      if ((keyCode == RotateLeft) ||
         (keyCode == RotateRight))
         return true;
      else return false;
   };

   OISKeyBinding^ OISKeyBinding::GetDefault()
   {
      OISKeyBinding^ defaultBinding = gcnew OISKeyBinding();

      defaultBinding->RightClickAction = 48;

      // Movement
      defaultBinding->MoveForward   = ::OIS::KeyCode::KC_W;
      defaultBinding->MoveBackward  = ::OIS::KeyCode::KC_S;
      defaultBinding->MoveLeft      = ::OIS::KeyCode::KC_A;
      defaultBinding->MoveRight     = ::OIS::KeyCode::KC_D;

      // Rotation
      defaultBinding->RotateLeft    = ::OIS::KeyCode::KC_LEFT;
      defaultBinding->RotateRight   = ::OIS::KeyCode::KC_RIGHT;

      // Modifiers
      defaultBinding->Walk          = ::OIS::KeyCode::KC_LSHIFT;
      defaultBinding->AutoMove      = ::OIS::KeyCode::KC_NUMLOCK;

      // Targetting
      defaultBinding->NextTarget    = ::OIS::KeyCode::KC_TAB;
      defaultBinding->SelfTarget    = ::OIS::KeyCode::KC_LMENU;

      // Actions
      defaultBinding->ReqGo         = ::OIS::KeyCode::KC_SPACE;
      defaultBinding->Close         = ::OIS::KeyCode::KC_ESCAPE;

      defaultBinding->ActionButton01 = ::OIS::KeyCode::KC_Q;
      defaultBinding->ActionButton02 = ::OIS::KeyCode::KC_E;
      defaultBinding->ActionButton03 = ::OIS::KeyCode::KC_R;
      defaultBinding->ActionButton04 = ::OIS::KeyCode::KC_F;
      defaultBinding->ActionButton05 = ::OIS::KeyCode::KC_T;
      defaultBinding->ActionButton06 = ::OIS::KeyCode::KC_G;
      defaultBinding->ActionButton07 = ::OIS::KeyCode::KC_NUMPAD1;
      defaultBinding->ActionButton08 = ::OIS::KeyCode::KC_NUMPAD1;
      defaultBinding->ActionButton09 = ::OIS::KeyCode::KC_NUMPAD2;
      defaultBinding->ActionButton10 = ::OIS::KeyCode::KC_NUMPAD2;
      defaultBinding->ActionButton11 = ::OIS::KeyCode::KC_NUMPAD3;
      defaultBinding->ActionButton12 = ::OIS::KeyCode::KC_NUMPAD3;
      defaultBinding->ActionButton13 = ::OIS::KeyCode::KC_Z;
      defaultBinding->ActionButton14 = ::OIS::KeyCode::KC_X;
      defaultBinding->ActionButton15 = ::OIS::KeyCode::KC_C;
      defaultBinding->ActionButton16 = ::OIS::KeyCode::KC_V;
      defaultBinding->ActionButton17 = ::OIS::KeyCode::KC_B;
      defaultBinding->ActionButton18 = ::OIS::KeyCode::KC_N;
      defaultBinding->ActionButton19 = ::OIS::KeyCode::KC_NUMPAD0;
      defaultBinding->ActionButton20 = ::OIS::KeyCode::KC_NUMPAD0;
      defaultBinding->ActionButton21 = ::OIS::KeyCode::KC_NUMPAD0;
      defaultBinding->ActionButton22 = ::OIS::KeyCode::KC_NUMPAD0;
      defaultBinding->ActionButton23 = ::OIS::KeyCode::KC_DECIMAL;
      defaultBinding->ActionButton24 = ::OIS::KeyCode::KC_DECIMAL;
      defaultBinding->ActionButton25 = ::OIS::KeyCode::KC_F1;
      defaultBinding->ActionButton26 = ::OIS::KeyCode::KC_F2;
      defaultBinding->ActionButton27 = ::OIS::KeyCode::KC_F3;
      defaultBinding->ActionButton28 = ::OIS::KeyCode::KC_F4;
      defaultBinding->ActionButton29 = ::OIS::KeyCode::KC_F5;
      defaultBinding->ActionButton30 = ::OIS::KeyCode::KC_F6;
      defaultBinding->ActionButton31 = ::OIS::KeyCode::KC_F7;
      defaultBinding->ActionButton32 = ::OIS::KeyCode::KC_F8;
      defaultBinding->ActionButton33 = ::OIS::KeyCode::KC_F9;
      defaultBinding->ActionButton34 = ::OIS::KeyCode::KC_F10;
      defaultBinding->ActionButton35 = ::OIS::KeyCode::KC_F11;
      defaultBinding->ActionButton36 = ::OIS::KeyCode::KC_F12;
      defaultBinding->ActionButton37 = ::OIS::KeyCode::KC_1;
      defaultBinding->ActionButton38 = ::OIS::KeyCode::KC_2;
      defaultBinding->ActionButton39 = ::OIS::KeyCode::KC_3;
      defaultBinding->ActionButton40 = ::OIS::KeyCode::KC_4;
      defaultBinding->ActionButton41 = ::OIS::KeyCode::KC_5;
      defaultBinding->ActionButton42 = ::OIS::KeyCode::KC_6;
      defaultBinding->ActionButton43 = ::OIS::KeyCode::KC_7;
      defaultBinding->ActionButton44 = ::OIS::KeyCode::KC_8;
      defaultBinding->ActionButton45 = ::OIS::KeyCode::KC_9;
      defaultBinding->ActionButton46 = ::OIS::KeyCode::KC_0;
      defaultBinding->ActionButton47 = ::OIS::KeyCode::KC_GRAVE;
      defaultBinding->ActionButton48 = ::OIS::KeyCode::KC_UNASSIGNED;

      defaultBinding->ActionButton49 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton50 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton51 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton52 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton53 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton54 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton55 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton56 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton57 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton58 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton59 = ::OIS::KeyCode::KC_UNASSIGNED;
      defaultBinding->ActionButton60 = ::OIS::KeyCode::KC_UNASSIGNED;

      return defaultBinding;
   };
};};
