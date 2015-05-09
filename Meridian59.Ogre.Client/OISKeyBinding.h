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
#include "OISKeyboard.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre
{
	using namespace OIS;

	/// <summary>
    /// A keybinding for typical actions,
	/// based on OIS keycodes.
    /// </summary>
	public ref class OISKeyBinding
	{
	public:
		int RightClickAction;

		// Movement
		::OIS::KeyCode MoveForward;
        ::OIS::KeyCode MoveBackward;
        ::OIS::KeyCode MoveLeft;
        ::OIS::KeyCode MoveRight;
        
        // Rotation
        ::OIS::KeyCode RotateLeft;
        ::OIS::KeyCode RotateRight;

        // Others
        ::OIS::KeyCode Walk;
		::OIS::KeyCode AutoMove;
        ::OIS::KeyCode NextTarget;
        ::OIS::KeyCode SelfTarget;
        ::OIS::KeyCode ReqGo;
        ::OIS::KeyCode Close;

		// ActionButtons
        ::OIS::KeyCode ActionButton01;
        ::OIS::KeyCode ActionButton02;
        ::OIS::KeyCode ActionButton03;
        ::OIS::KeyCode ActionButton04;
        ::OIS::KeyCode ActionButton05;
        ::OIS::KeyCode ActionButton06;
        ::OIS::KeyCode ActionButton07;
        ::OIS::KeyCode ActionButton08;
        ::OIS::KeyCode ActionButton09;
        ::OIS::KeyCode ActionButton10;
        ::OIS::KeyCode ActionButton11;
        ::OIS::KeyCode ActionButton12;
        ::OIS::KeyCode ActionButton13;
        ::OIS::KeyCode ActionButton14;
        ::OIS::KeyCode ActionButton15;
        ::OIS::KeyCode ActionButton16;
        ::OIS::KeyCode ActionButton17;
        ::OIS::KeyCode ActionButton18;
        ::OIS::KeyCode ActionButton19;
        ::OIS::KeyCode ActionButton20;
        ::OIS::KeyCode ActionButton21;
        ::OIS::KeyCode ActionButton22;
        ::OIS::KeyCode ActionButton23;
        ::OIS::KeyCode ActionButton24;
		::OIS::KeyCode ActionButton25;
		::OIS::KeyCode ActionButton26;
		::OIS::KeyCode ActionButton27;
		::OIS::KeyCode ActionButton28;
		::OIS::KeyCode ActionButton29;
		::OIS::KeyCode ActionButton30;
		::OIS::KeyCode ActionButton31;
		::OIS::KeyCode ActionButton32;
		::OIS::KeyCode ActionButton33;
		::OIS::KeyCode ActionButton34;
		::OIS::KeyCode ActionButton35;
		::OIS::KeyCode ActionButton36;
		::OIS::KeyCode ActionButton37;
		::OIS::KeyCode ActionButton38;
		::OIS::KeyCode ActionButton39;
		::OIS::KeyCode ActionButton40;
		::OIS::KeyCode ActionButton41;
		::OIS::KeyCode ActionButton42;
		::OIS::KeyCode ActionButton43;
		::OIS::KeyCode ActionButton44;
		::OIS::KeyCode ActionButton45;
		::OIS::KeyCode ActionButton46;
		::OIS::KeyCode ActionButton47;
		::OIS::KeyCode ActionButton48;

		/// <summary>
		/// Constructor
		/// </summary>
		OISKeyBinding(void);
		
		/// <summary>
        /// Whether a keycode is a movement key in this binding.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        bool IsMovementKey(::OIS::KeyCode keyCode);

		/// <summary>
        /// Whether a keycode is a rotate key in this binding.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        bool IsRotateKey(::OIS::KeyCode keyCode);

		/// <summary>
        /// A static default keybinding.
        /// </summary>
        static OISKeyBinding^ GetDefault();
	};
};};
