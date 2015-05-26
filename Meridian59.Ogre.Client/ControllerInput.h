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
#include "OISInputManager.h"
#include "OISKeyboard.h"
#include "OISMouse.h"
#pragma managed(pop)

#include "RemoteNode.h"
#include "OISKeyBinding.h"
#include "OISListeners.h"
#include "ControllerRoom.h"

namespace Meridian59 { namespace Ogre
{	
	using namespace Meridian59::Data;
	using namespace Meridian59::Common;
	using namespace Meridian59::Common::Enums;

	/// <summary>
    /// Handles user input from OIS
    /// </summary>
	public ref class ControllerInput abstract sealed
	{
	private:
		literal float ZOOMSPEED			= 0.10f;
		literal float MOUSELOOKSPEED	= 0.0004f;
		literal float MOUSELOOKSTEPFACT = 0.013f;
		literal long MOUSECLICKMAXDELAY	= 350;
        literal long MOUSELOOKMINDELAY	= 100;
		literal long KEYDOWNINTERVAL	= 333;
		literal float KEYROTATESPEED	= 0.00012f;

		static ControllerInput();

		static OIS::InputManager* inputManager;
		static OIS::Keyboard* oisKeyboard;
		static OIS::Mouse* oisMouse;
		static OISKeyListener* keylistener;
		static OISMouseListener* mouselistener;

		static LPPOINT mouseDownWindowsPosition;
		static double tickMouseDownLeft;
		static double tickMouseDownRight;
		static bool isCameraFirstPerson;
		static bool isInitialized;		
		static bool isMouseInWindow;
		static bool isMouseWentDownOnUI;
		static bool isAltgrDown;
		static bool isAutoMove;
		static bool isAutoMoveOnMove;

		static float cameraPitchCurrent;
		static float cameraPitchDelta;
		static float cameraPitchStep;
		static float cameraYawCurrent;
		static float cameraYawDelta;
		static float cameraYawStep;
		static float cameraZDelta;
		static float cameraZStep;
		static float avatarYawDelta;
		static float avatarYawStep;
		
		static property RemoteNode^ Avatar { RemoteNode^ get() { return ControllerRoom::AvatarObject; } };

		/// <summary>
        /// Composes a normalized 2D direction vector on ground plane based on input state/pressed keys
        /// </summary>
        /// <returns></returns>
        static V2 GetMoveVector();

		/// <summary>
        /// Converts a OIS button-id into CEGUI button-id
        /// </summary>
        /// <returns></returns>
		static ::CEGUI::MouseButton GetCEGUIMouseButton(::OIS::MouseButtonID OISButton);
	
	public:
		/// <summary>
        /// OIS input manager
        /// </summary>
		static property OIS::InputManager* InputManager 
		{ 
			public: OIS::InputManager* get() { return inputManager; }
			private: void set(OIS::InputManager* value) { inputManager = value; } 
		};
        
		/// <summary>
        /// OIS keyboard object
        /// </summary>
		static property OIS::Keyboard* OISKeyboard 
		{ 
			public: OIS::Keyboard* get() { return oisKeyboard; }
			private: void set(OIS::Keyboard* value) { oisKeyboard = value; } 
		};
        
		/// <summary>
        /// OIS mouse object
        /// </summary>
		static property OIS::Mouse* OISMouse 
		{ 
			public: OIS::Mouse* get() { return oisMouse; }
			private: void set(OIS::Mouse* value) { oisMouse = value; } 
		};

		/// <summary>
        /// True if Initialized was called
        /// </summary>
		static property bool IsInitialized 
		{ 
			public: bool get() { return isInitialized; }
			private: void set(bool value) { isInitialized = value; } 
		};

		/// <summary>
        /// True if the camera is in first person mode
        /// </summary>
		static property bool IsCameraFirstPerson 
		{ 
			public: bool get() { return isCameraFirstPerson; }
			private: 
				void set(bool value) 
				{ 
					if (isCameraFirstPerson != value)
					{
						isCameraFirstPerson = value;
						
						if (Avatar != nullptr && Avatar->SceneNode != nullptr)						
							Avatar->SetVisible(!value);						
					}
				} 
		};

		/// <summary>
        /// True if the mouse is on the game window (!= focus)
        /// </summary>
		static property bool IsMouseInWindow
		{ 
			public: bool get() { return isMouseInWindow; }
			private: 
				void set(bool value) 
				{ 
					if (isMouseInWindow != value)
					{
						isMouseInWindow = value;						
					}
				} 
		};

		/// <summary>
        /// Currently loaded keybinding (from OgreClient::Singleton->Config)
        /// </summary>
		static property OISKeyBinding^ ActiveKeyBinding 
		{ 
			public: OISKeyBinding^ get();
		};

		/// <summary>
        /// True if left mousebutton is down
        /// </summary>
		static property bool IsLeftMouseDown 
		{ 
			bool get() 
			{ 
				return oisMouse && oisMouse->getMouseState().buttonDown(OIS::MouseButtonID::MB_Left);
			} 
		};

		/// <summary>
        /// True if right mousebutton is down
        /// </summary>
        static property bool IsRightMouseDown 
		{ 
			bool get() 
			{ 
				return oisMouse && oisMouse->getMouseState().buttonDown(OIS::MouseButtonID::MB_Right); 
			} 
		};
        
		/// <summary>
        /// True if at least left or right mousebutton is down
        /// </summary>
		static property bool IsAnyMouseDown 
		{ 
			bool get() 
			{ 
				return (IsLeftMouseDown || IsRightMouseDown); 
			} 
		};

		/// <summary>
        /// True if left and right mousebuttons are down
        /// </summary>
		static property bool IsBothMouseDown 
		{ 
			bool get() 
			{ 
				return (IsLeftMouseDown && IsRightMouseDown); 
			} 
		};
        
		/// <summary>
        /// True if the defined walkmodifier key is down
        /// </summary>
		static property bool IsWalkKeyDown 
		{ 
			bool get() 
			{ 
				return oisKeyboard && oisKeyboard->isKeyDown(ActiveKeyBinding->Walk); 
			} 
		};
        
		/// <summary>
        /// True if any of the defined movement keys (forward, backward, left, right)
		/// or both mouse buttons are down.
        /// </summary>
		static property bool IsMovementInput
        {
            bool get()
            {
				if (!oisKeyboard)
					return false;

                if (oisKeyboard->isKeyDown(ActiveKeyBinding->MoveForward) ||
                    oisKeyboard->isKeyDown(ActiveKeyBinding->MoveBackward) ||
                    oisKeyboard->isKeyDown(ActiveKeyBinding->MoveLeft) ||
                    oisKeyboard->isKeyDown(ActiveKeyBinding->MoveRight) ||
                    IsBothMouseDown)
                    return true;
                else return false;
            }
        };

		/// <summary>
        /// True if one of the defined rotation keys is down
        /// </summary>
        static property bool IsRotateKeyDown
        {
            bool get()
            {
				if (!oisKeyboard)
					return false;

                if (oisKeyboard->isKeyDown(ActiveKeyBinding->RotateLeft) ||
                    oisKeyboard->isKeyDown(ActiveKeyBinding->RotateRight))
                    return true;
                else return false;
            }
        };

		/// <summary>
        /// True if the defined selftarget modifier a.k.a modifier key is down
        /// </summary>
        static property bool IsSelfTargetDown
        {
            bool get()
            {
				if (!oisKeyboard)
					return false;

                if (oisKeyboard->isKeyDown(ActiveKeyBinding->SelfTarget))
                    return true;
                else return false;
            }
        };

		/// <summary>
        /// Initialize.
        /// </summary>
		static void Initialize();
	
		/// <summary>
		/// Initialize.
		/// </summary>
		static void SetDisplaySize();

		/// <summary>
        /// This is executed by OIS listener once during capture() when mouse button is released.
        /// </summary>
		static bool OISMouse_MouseReleased(const OIS::MouseEvent &arg, OIS::MouseButtonID id);
		
		/// <summary>
        /// This is executed by OIS listener once during capture()when mouse button goes down
        /// </summary>
		static bool OISMouse_MousePressed(const OIS::MouseEvent &arg, OIS::MouseButtonID id);
		
		/// <summary>
        /// This is executed by OIS listener during capture() whenever the mouse moved a bit
        /// </summary>
		static bool OISMouse_MouseMoved(const OIS::MouseEvent &arg);
		
		/// <summary>
        /// This is executed by OIS listener once during capture() when a key goes down
        /// </summary>
		static bool OISKeyboard_KeyPressed(const OIS::KeyEvent &arg);
		
		/// <summary>
        /// This is executed by OIS listener once during capture() when a key is released
        /// </summary>
		static bool OISKeyboard_KeyReleased(const OIS::KeyEvent &arg);

		/// <summary>
        /// Sets the orientation of the controlled avatar to the orientation the camera has.
        /// </summary>
        static void SetAvatarOrientationFromCamera();

		/// <summary>
        /// Performs rayquery to do a click or a mouseover effect.
        /// </summary>
		static void PerformMouseOver(int MouseX, int MouseY, bool IsClick);

		/// <summary>
        /// Cleanup
        /// </summary>
        static void Destroy();

		/// <summary>
        /// Call this once per mainthread loop		
		/// to process input and act accordingly.
        /// </summary>
		static void Tick(double Tick, double Span);
	};
};};
