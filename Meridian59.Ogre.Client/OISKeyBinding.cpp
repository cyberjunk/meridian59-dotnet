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

		defaultBinding->RightClickAction = 13;

        // Movement
        defaultBinding->MoveForward		= ::OIS::KeyCode::KC_W;
        defaultBinding->MoveBackward	= ::OIS::KeyCode::KC_S;
        defaultBinding->MoveLeft		= ::OIS::KeyCode::KC_Q;
        defaultBinding->MoveRight		= ::OIS::KeyCode::KC_E;

        // Rotation
        defaultBinding->RotateLeft		= ::OIS::KeyCode::KC_A;
        defaultBinding->RotateRight		= ::OIS::KeyCode::KC_D;

        // Modifiers
        defaultBinding->Walk			= ::OIS::KeyCode::KC_LSHIFT;
        defaultBinding->AutoMove		= ::OIS::KeyCode::KC_NUMLOCK;
        
        // Targetting
        defaultBinding->NextTarget		= ::OIS::KeyCode::KC_TAB;
        defaultBinding->SelfTarget		= ::OIS::KeyCode::KC_LMENU;

        // Actions
        defaultBinding->ReqGo			= ::OIS::KeyCode::KC_N;
        defaultBinding->Close			= ::OIS::KeyCode::KC_ESCAPE;

        defaultBinding->ActionButton01 = ::OIS::KeyCode::KC_1;
        defaultBinding->ActionButton02 = ::OIS::KeyCode::KC_2;
        defaultBinding->ActionButton03 = ::OIS::KeyCode::KC_3;
        defaultBinding->ActionButton04 = ::OIS::KeyCode::KC_4;
        defaultBinding->ActionButton05 = ::OIS::KeyCode::KC_5;
        defaultBinding->ActionButton06 = ::OIS::KeyCode::KC_6;
        defaultBinding->ActionButton07 = ::OIS::KeyCode::KC_7;
        defaultBinding->ActionButton08 = ::OIS::KeyCode::KC_8;
        defaultBinding->ActionButton09 = ::OIS::KeyCode::KC_9;
        defaultBinding->ActionButton10 = ::OIS::KeyCode::KC_0;
        defaultBinding->ActionButton11 = ::OIS::KeyCode::KC_H;
        defaultBinding->ActionButton12 = ::OIS::KeyCode::KC_H;
        defaultBinding->ActionButton13 = ::OIS::KeyCode::KC_SPACE;
        defaultBinding->ActionButton14 = ::OIS::KeyCode::KC_R;
        defaultBinding->ActionButton15 = ::OIS::KeyCode::KC_G;
        defaultBinding->ActionButton16 = ::OIS::KeyCode::KC_I;
        defaultBinding->ActionButton17 = ::OIS::KeyCode::KC_U;
        defaultBinding->ActionButton18 = ::OIS::KeyCode::KC_B;
        defaultBinding->ActionButton19 = ::OIS::KeyCode::KC_T;
        defaultBinding->ActionButton20 = ::OIS::KeyCode::KC_F;
        defaultBinding->ActionButton21 = ::OIS::KeyCode::KC_C;
        defaultBinding->ActionButton22 = ::OIS::KeyCode::KC_V;
        defaultBinding->ActionButton23 = ::OIS::KeyCode::KC_J;
        defaultBinding->ActionButton24 = ::OIS::KeyCode::KC_J;
		defaultBinding->ActionButton25 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton26 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton27 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton28 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton29 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton30 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton31 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton32 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton33 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton34 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton35 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton36 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton37 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton38 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton39 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton40 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton41 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton42 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton43 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton44 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton45 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton46 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton47 = ::OIS::KeyCode::KC_UNASSIGNED;
		defaultBinding->ActionButton48 = ::OIS::KeyCode::KC_UNASSIGNED;

        return defaultBinding;      
    };	
};};