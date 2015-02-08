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

	OISKeyBinding^ OISKeyBinding::FromKeyBinding(KeyBinding^ KeyBinding)
    {
        OISKeyBinding^ oisBinding = gcnew OISKeyBinding();

		oisBinding->RightClickAction = KeyBinding->RightClickAction;

        oisBinding->MoveForward = KeysToKeyCode(KeyBinding->MoveForward);
        oisBinding->MoveBackward = KeysToKeyCode(KeyBinding->MoveBackward);
        oisBinding->MoveLeft = KeysToKeyCode(KeyBinding->MoveLeft);
        oisBinding->MoveRight = KeysToKeyCode(KeyBinding->MoveRight);
        oisBinding->RotateLeft = KeysToKeyCode(KeyBinding->RotateLeft);
        oisBinding->RotateRight = KeysToKeyCode(KeyBinding->RotateRight);

        oisBinding->Walk = KeysToKeyCode(KeyBinding->Walk);
		oisBinding->AutoMove = KeysToKeyCode(KeyBinding->AutoMove);
        oisBinding->NextTarget = KeysToKeyCode(KeyBinding->NextTarget);
        oisBinding->SelfTarget = KeysToKeyCode(KeyBinding->SelfTarget);
        oisBinding->ReqGo = KeysToKeyCode(KeyBinding->ReqGo);
        oisBinding->Close = KeysToKeyCode(KeyBinding->Close);

        oisBinding->ActionButton01 = KeysToKeyCode(KeyBinding->ActionButton01);
        oisBinding->ActionButton02 = KeysToKeyCode(KeyBinding->ActionButton02);
        oisBinding->ActionButton03 = KeysToKeyCode(KeyBinding->ActionButton03);
        oisBinding->ActionButton04 = KeysToKeyCode(KeyBinding->ActionButton04);
        oisBinding->ActionButton05 = KeysToKeyCode(KeyBinding->ActionButton05);
        oisBinding->ActionButton06 = KeysToKeyCode(KeyBinding->ActionButton06);
        oisBinding->ActionButton07 = KeysToKeyCode(KeyBinding->ActionButton07);
        oisBinding->ActionButton08 = KeysToKeyCode(KeyBinding->ActionButton08);
        oisBinding->ActionButton09 = KeysToKeyCode(KeyBinding->ActionButton09);
        oisBinding->ActionButton10 = KeysToKeyCode(KeyBinding->ActionButton10);
        oisBinding->ActionButton11 = KeysToKeyCode(KeyBinding->ActionButton11);
        oisBinding->ActionButton12 = KeysToKeyCode(KeyBinding->ActionButton12);
        oisBinding->ActionButton13 = KeysToKeyCode(KeyBinding->ActionButton13);
        oisBinding->ActionButton14 = KeysToKeyCode(KeyBinding->ActionButton14);
        oisBinding->ActionButton15 = KeysToKeyCode(KeyBinding->ActionButton15);
        oisBinding->ActionButton16 = KeysToKeyCode(KeyBinding->ActionButton16);
        oisBinding->ActionButton17 = KeysToKeyCode(KeyBinding->ActionButton17);
        oisBinding->ActionButton18 = KeysToKeyCode(KeyBinding->ActionButton18);
        oisBinding->ActionButton19 = KeysToKeyCode(KeyBinding->ActionButton19);
        oisBinding->ActionButton20 = KeysToKeyCode(KeyBinding->ActionButton20);
        oisBinding->ActionButton21 = KeysToKeyCode(KeyBinding->ActionButton21);
        oisBinding->ActionButton22 = KeysToKeyCode(KeyBinding->ActionButton22);
        oisBinding->ActionButton23 = KeysToKeyCode(KeyBinding->ActionButton23);
        oisBinding->ActionButton24 = KeysToKeyCode(KeyBinding->ActionButton24);
		oisBinding->ActionButton25 = KeysToKeyCode(KeyBinding->ActionButton25);
		oisBinding->ActionButton26 = KeysToKeyCode(KeyBinding->ActionButton26);
		oisBinding->ActionButton27 = KeysToKeyCode(KeyBinding->ActionButton27);
		oisBinding->ActionButton28 = KeysToKeyCode(KeyBinding->ActionButton28);
		oisBinding->ActionButton29 = KeysToKeyCode(KeyBinding->ActionButton29);
		oisBinding->ActionButton30 = KeysToKeyCode(KeyBinding->ActionButton30);
		oisBinding->ActionButton31 = KeysToKeyCode(KeyBinding->ActionButton31);
		oisBinding->ActionButton32 = KeysToKeyCode(KeyBinding->ActionButton32);
		oisBinding->ActionButton33 = KeysToKeyCode(KeyBinding->ActionButton33);
		oisBinding->ActionButton34 = KeysToKeyCode(KeyBinding->ActionButton34);
		oisBinding->ActionButton35 = KeysToKeyCode(KeyBinding->ActionButton35);
		oisBinding->ActionButton36 = KeysToKeyCode(KeyBinding->ActionButton36);
		oisBinding->ActionButton37 = KeysToKeyCode(KeyBinding->ActionButton37);
		oisBinding->ActionButton38 = KeysToKeyCode(KeyBinding->ActionButton38);
		oisBinding->ActionButton39 = KeysToKeyCode(KeyBinding->ActionButton39);
		oisBinding->ActionButton40 = KeysToKeyCode(KeyBinding->ActionButton40);
		oisBinding->ActionButton41 = KeysToKeyCode(KeyBinding->ActionButton41);
		oisBinding->ActionButton42 = KeysToKeyCode(KeyBinding->ActionButton42);
		oisBinding->ActionButton43 = KeysToKeyCode(KeyBinding->ActionButton43);
		oisBinding->ActionButton44 = KeysToKeyCode(KeyBinding->ActionButton44);
		oisBinding->ActionButton45 = KeysToKeyCode(KeyBinding->ActionButton45);
		oisBinding->ActionButton46 = KeysToKeyCode(KeyBinding->ActionButton46);
		oisBinding->ActionButton47 = KeysToKeyCode(KeyBinding->ActionButton47);
		oisBinding->ActionButton48 = KeysToKeyCode(KeyBinding->ActionButton48);

        return oisBinding;
    };

	::OIS::KeyCode OISKeyBinding::KeysToKeyCode(Keys Key)
    {
        ::OIS::KeyCode code = ::OIS::KeyCode::KC_RETURN;

        switch (Key)
        {
            // numeric (row)
            case Keys::D0: code = ::OIS::KeyCode::KC_0; break;
            case Keys::D1: code = ::OIS::KeyCode::KC_1; break;
            case Keys::D2: code = ::OIS::KeyCode::KC_2; break;
            case Keys::D3: code = ::OIS::KeyCode::KC_3; break;
            case Keys::D4: code = ::OIS::KeyCode::KC_4; break;
            case Keys::D5: code = ::OIS::KeyCode::KC_5; break;
            case Keys::D6: code = ::OIS::KeyCode::KC_6; break;
            case Keys::D7: code = ::OIS::KeyCode::KC_7; break;
            case Keys::D8: code = ::OIS::KeyCode::KC_8; break;
            case Keys::D9: code = ::OIS::KeyCode::KC_9; break;
                
            // alphabetic
            case Keys::A: code = ::OIS::KeyCode::KC_A; break;
            case Keys::B: code = ::OIS::KeyCode::KC_B; break;
            case Keys::C: code = ::OIS::KeyCode::KC_C; break;
            case Keys::D: code = ::OIS::KeyCode::KC_D; break;
            case Keys::E: code = ::OIS::KeyCode::KC_E; break;
            case Keys::F: code = ::OIS::KeyCode::KC_F; break;
            case Keys::G: code = ::OIS::KeyCode::KC_G; break;
            case Keys::H: code = ::OIS::KeyCode::KC_H; break;
            case Keys::I: code = ::OIS::KeyCode::KC_I; break;
            case Keys::J: code = ::OIS::KeyCode::KC_J; break;
            case Keys::K: code = ::OIS::KeyCode::KC_K; break;
            case Keys::L: code = ::OIS::KeyCode::KC_L; break;
            case Keys::M: code = ::OIS::KeyCode::KC_M; break;
            case Keys::N: code = ::OIS::KeyCode::KC_N; break;
            case Keys::O: code = ::OIS::KeyCode::KC_O; break;
            case Keys::P: code = ::OIS::KeyCode::KC_P; break;
            case Keys::Q: code = ::OIS::KeyCode::KC_Q; break;
            case Keys::R: code = ::OIS::KeyCode::KC_R; break;
            case Keys::S: code = ::OIS::KeyCode::KC_S; break;
            case Keys::T: code = ::OIS::KeyCode::KC_T; break;
            case Keys::U: code = ::OIS::KeyCode::KC_U; break;
            case Keys::V: code = ::OIS::KeyCode::KC_V; break;
            case Keys::W: code = ::OIS::KeyCode::KC_W; break;
            case Keys::X: code = ::OIS::KeyCode::KC_X; break;
            case Keys::Y: code = ::OIS::KeyCode::KC_Y; break;
            case Keys::Z: code = ::OIS::KeyCode::KC_Z; break;

            // block above arrows
            case Keys::Insert:	code = ::OIS::KeyCode::KC_INSERT; break;
            case Keys::Delete:	code = ::OIS::KeyCode::KC_DELETE; break;
            case Keys::PageUp:	code = ::OIS::KeyCode::KC_PGUP; break;
            case Keys::Next:	code = ::OIS::KeyCode::KC_PGDOWN; break;
            case Keys::End:		code = ::OIS::KeyCode::KC_END;	break;
            case Keys::Home:	code = ::OIS::KeyCode::KC_HOME; break;

            // numpad
            case Keys::NumLock:		code = ::OIS::KeyCode::KC_NUMLOCK;	break;
            case Keys::NumPad0:		code = ::OIS::KeyCode::KC_NUMPAD0;	break;
            case Keys::NumPad1:		code = ::OIS::KeyCode::KC_NUMPAD1;	break;
            case Keys::NumPad2:		code = ::OIS::KeyCode::KC_NUMPAD2;	break;
            case Keys::NumPad3:		code = ::OIS::KeyCode::KC_NUMPAD3;	break;
            case Keys::NumPad4:		code = ::OIS::KeyCode::KC_NUMPAD4;	break;
            case Keys::NumPad5:		code = ::OIS::KeyCode::KC_NUMPAD5;	break;
            case Keys::NumPad6:		code = ::OIS::KeyCode::KC_NUMPAD6;	break;
            case Keys::NumPad7:		code = ::OIS::KeyCode::KC_NUMPAD7;	break;
            case Keys::NumPad8:		code = ::OIS::KeyCode::KC_NUMPAD8;	break;
            case Keys::NumPad9:		code = ::OIS::KeyCode::KC_NUMPAD9;	break;
            case Keys::Divide:		code = ::OIS::KeyCode::KC_DIVIDE;	break;
            case Keys::Multiply:	code = ::OIS::KeyCode::KC_MULTIPLY; break;
            case Keys::Add:			code = ::OIS::KeyCode::KC_ADD;		break;
            case Keys::Subtract:	code = ::OIS::KeyCode::KC_SUBTRACT; break;

            // arrows
            case Keys::Up:		code = ::OIS::KeyCode::KC_UP;	break;
            case Keys::Down:	code = ::OIS::KeyCode::KC_DOWN; break;
            case Keys::Left:	code = ::OIS::KeyCode::KC_LEFT; break;
            case Keys::Right:	code = ::OIS::KeyCode::KC_RIGHT; break;

            // F-keys
            case Keys::F1:	code = ::OIS::KeyCode::KC_F1;	break;
            case Keys::F2:	code = ::OIS::KeyCode::KC_F2;	break;
            case Keys::F3:	code = ::OIS::KeyCode::KC_F3;	break;
            case Keys::F4:	code = ::OIS::KeyCode::KC_F4;	break;
            case Keys::F5:	code = ::OIS::KeyCode::KC_F5;	break;
            case Keys::F6:	code = ::OIS::KeyCode::KC_F6;	break;
            case Keys::F7:	code = ::OIS::KeyCode::KC_F7;	break;
            case Keys::F8:	code = ::OIS::KeyCode::KC_F8;	break;
            case Keys::F9:	code = ::OIS::KeyCode::KC_F9;	break;
            case Keys::F10: code = ::OIS::KeyCode::KC_F10;	break;
            case Keys::F11: code = ::OIS::KeyCode::KC_F11;	break;
            case Keys::F12: code = ::OIS::KeyCode::KC_F12;	break;
			
			// ALT (default to left)
            case Keys::Menu:	code = ::OIS::KeyCode::KC_LMENU; break;
			case Keys::LMenu:	code = ::OIS::KeyCode::KC_LMENU; break;
			case Keys::RMenu:	code = ::OIS::KeyCode::KC_RMENU; break;
			
			// CTRL (default to left)
			case Keys::ControlKey:	code = ::OIS::KeyCode::KC_LCONTROL; break;
			case Keys::LControlKey: code = ::OIS::KeyCode::KC_LCONTROL; break;
			case Keys::RControlKey: code = ::OIS::KeyCode::KC_RCONTROL; break;
			
			// SHIFT (default to left)
			case Keys::ShiftKey:	code = ::OIS::KeyCode::KC_LSHIFT; break; 
			case Keys::LShiftKey:	code = ::OIS::KeyCode::KC_LSHIFT; break;               
            case Keys::RShiftKey:	code = ::OIS::KeyCode::KC_RSHIFT; break;               
            
            // others
            case Keys::Escape:		code = ::OIS::KeyCode::KC_ESCAPE;		break;               
            case Keys::Tab:			code = ::OIS::KeyCode::KC_TAB;			break;
            case Keys::Space:		code = ::OIS::KeyCode::KC_SPACE;		break;
            case Keys::Scroll:		code = ::OIS::KeyCode::KC_SCROLL;		break;
            case Keys::Pause:		code = ::OIS::KeyCode::KC_PAUSE;		break;
            case Keys::Back:		code = ::OIS::KeyCode::KC_BACK;			break;
            case Keys::Capital:		code = ::OIS::KeyCode::KC_CAPITAL;		break;

			case Keys::Oem1:		code = ::OIS::KeyCode::KC_LBRACKET;		break;
            case Keys::Oem5:		code = ::OIS::KeyCode::KC_APOSTROPHE;	break;			
			case Keys::Oemcomma:	code = ::OIS::KeyCode::KC_COMMA;		break;
			case Keys::OemPeriod:	code = ::OIS::KeyCode::KC_PERIOD;		break;

			case Keys::None:		code = ::OIS::KeyCode::KC_UNASSIGNED;	break;
        }

        return code;
    };
};};