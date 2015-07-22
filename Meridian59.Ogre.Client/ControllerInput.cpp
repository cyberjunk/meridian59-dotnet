#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	static ControllerInput::ControllerInput()
	{
		inputManager		= nullptr;
		oisKeyboard			= nullptr;
		oisMouse			= nullptr;	
		keylistener			= nullptr;
		mouselistener		= nullptr;

		tickMouseDownLeft	= 0;
		tickMouseDownRight	= 0;
		
		isCameraFirstPerson = true;
		isInitialized		= false;		
		isMouseInWindow		= false;
		isMouseWentDownOnUI = false;
		isAltgrDown			= false;
		isAutoMove			= false;
		isAutoMoveOnMove	= false;

		cameraPitchCurrent	= 0.0f;
		cameraPitchDelta	= 0.0f;
		cameraPitchStep		= 0.0f;
		cameraYawCurrent	= 0.0f;
		cameraYawDelta		= 0.0f;
		cameraYawStep		= 0.0f;
		cameraZDelta		= 0.0f;
		cameraZStep			= 0.0f;
		avatarYawDelta		= 0.0f;
		avatarYawStep		= 0.0f;

		mouseDownWindowsPosition = new POINT();
		mouseDownWindowsPosition->x = 0;
		mouseDownWindowsPosition->y = 0;
	};

	void ControllerInput::Initialize()
	{	
		std::ostringstream windowHndStr;
		windowHndStr << (size_t)OgreClient::Singleton->RenderWindowHandle;
		
		OIS::ParamList oisParameters = OIS::ParamList();	
        oisParameters.insert(std::make_pair(std::string("WINDOW"), windowHndStr.str()));
        oisParameters.insert(std::make_pair(std::string("w32_mouse"), "DISCL_NONEXCLUSIVE"));
        oisParameters.insert(std::make_pair(std::string("w32_mouse"), "DISCL_BACKGROUND"));
           
        // init OIS
        InputManager = OIS::InputManager::createInputSystem(oisParameters);
        OISKeyboard	 = (OIS::Keyboard*)InputManager->createInputObject(OIS::Type::OISKeyboard, true);
        OISMouse	 = (OIS::Mouse*)InputManager->createInputObject(OIS::Type::OISMouse, true);

		// sets boundaries for mouse from current viewport
		SetDisplaySize();

        // setup callbacks for keyboard and mouse events	
		keylistener = new OISKeyListener();
		mouselistener = new OISMouseListener();

		OISKeyboard->setEventCallback(keylistener);
		OISMouse->setEventCallback(mouselistener);	

		// mark initialized
        IsInitialized = true;
	};
	
	void ControllerInput::Destroy()
    {
		if (!IsInitialized)
			return;

        if (inputManager)
		{
			if (oisKeyboard)
			{	
				// remove eventcallback
				oisKeyboard->setEventCallback(0);

				// destroy keyboard (will also free)
				inputManager->destroyInputObject(oisKeyboard);
			}

			if (oisMouse)
			{
				// remove eventcallback
				oisMouse->setEventCallback(0);

				// destroy mouse (will also free)
				inputManager->destroyInputObject(oisMouse);
			}

			// destroy inputsystem (will also free)
			OIS::InputManager::destroyInputSystem(inputManager);
		}

		if (keylistener)
			delete keylistener;

		if (mouselistener)
			delete mouselistener;

		inputManager		= nullptr;
		oisKeyboard			= nullptr;
		oisMouse			= nullptr;
		keylistener			= nullptr;
		mouselistener		= nullptr;

		tickMouseDownLeft	= 0;
		tickMouseDownRight	= 0;
		
		isCameraFirstPerson = true;
		isInitialized		= false;		
		isMouseInWindow		= false;
		isMouseWentDownOnUI = false;
		isAltgrDown			= false;
		isAutoMove			= false;
		isAutoMoveOnMove	= false;

		cameraPitchCurrent	= 0.0f;
		cameraPitchDelta	= 0.0f;
		cameraPitchStep		= 0.0f;
		cameraYawCurrent	= 0.0f;
		cameraYawDelta		= 0.0f;
		cameraYawStep		= 0.0f;
		cameraZDelta		= 0.0f;
		cameraZStep			= 0.0f;
		avatarYawDelta		= 0.0f;
		avatarYawStep		= 0.0f;

		mouseDownWindowsPosition->x = 0;
		mouseDownWindowsPosition->y = 0;

		// mark not initialized
		IsInitialized = false;
    };
	
	void ControllerInput::SetDisplaySize()
	{
		const OIS::MouseState &mouseState = OISMouse->getMouseState();
		mouseState.width = OgreClient::Singleton->Viewport->getActualWidth();
		mouseState.height = OgreClient::Singleton->Viewport->getActualHeight();
	};

	OISKeyBinding^ ControllerInput::ActiveKeyBinding::get()
	{
		return OgreClient::Singleton->Config->KeyBinding;
	};

	void ControllerInput::PerformMouseOver(int MouseX, int MouseY, bool IsClick)
	{
		// normalise mouse coordinates to [0,1]
        float scrx = (float)MouseX / (float)OgreClient::Singleton->Viewport->getActualWidth();
        float scry = (float)MouseY / (float)OgreClient::Singleton->Viewport->getActualHeight();

        // create ray & query                      
		Ray ray = OgreClient::Singleton->Camera->getCameraToViewportRay(scrx, scry);                       
		RaySceneQuery* query = OgreClient::Singleton->SceneManager->createRayQuery(ray, 0xFFFFFFFF);
        
		// make it also find billboards and sort by distance
		query->setQueryTypeMask(0xFFFFFFFF);
        query->setSortByDistance(true);

		// execute query
        RaySceneQueryResult result = query->execute();
			
		if (result.size() > 0)
		{
			List<unsigned int>^ objectIDs = gcnew List<unsigned int>(result.size());

			// iterate hits
			for(unsigned int i=0; i<result.size();i++)
			{
				// get next hit and its type
				RaySceneQueryResultEntry entry = result.at(i);
				::Ogre::String type = entry.movable->getMovableType();
				
				// we only care for types Entity (3d model) 
				// and billboards (2d model)
				if (type.compare("Entity") == 0 || 
					type.compare("BillboardSet") == 0)
				{
					// get name as CLR string
					::Ogre::String ostr = entry.movable->getName();
					System::String^ s = StringConvert::OgreToCLR(ostr);
                
					// try parse an id out of name string
					unsigned int objectid;
					System::UInt32::TryParse(s->Substring(s->LastIndexOf('/') + 1), objectid);
                       
					if (objectid > 0)	
					{
						RoomObject^ obj = OgreClient::Singleton->Data->RoomObjects->GetItemByID(objectid);

						// don't select own avatar by clicks
						if (obj != nullptr && !obj->IsAvatar)
						{
							objectIDs->Add(objectid);	
							
							// for mouseover we just need the first hit
							if (!IsClick)
								break;
						}
					}
				}
			}

			// reset mouseover object
			if (!IsClick)
				OgreClient::Singleton->Data->RoomObjects->ResetHighlighted();

			// execute target clicker or mouseover
			if (objectIDs->Count > 0)
			{		
				ControllerUI::MouseCursor->setImage(UI_MOUSECURSOR_TARGET);

				if (IsClick)				
					OgreClient::Singleton->Data->ClickTarget(objectIDs, !IsSelfTargetDown);
				
				else
					OgreClient::Singleton->Data->RoomObjects->HighlightObject(objectIDs[0]);				
			}
			else
			{
				ControllerUI::MouseCursor->setImage(UI_DEFAULTARROW);
			}
		}

		// cleanup query
		OgreClient::Singleton->SceneManager->destroyQuery(query);
	};

	CEGUI::MouseButton ControllerInput::GetCEGUIMouseButton(OIS::MouseButtonID buttonID)
	{
		switch (buttonID)
		{
		case OIS::MB_Left:
			return CEGUI::LeftButton;
 
		case OIS::MB_Right:
			return CEGUI::RightButton;
 
		case OIS::MB_Middle:
			return CEGUI::MiddleButton;
 
		case OIS::MB_Button3:
			return CEGUI::X1Button;

		case OIS::MB_Button4:
			return CEGUI::X2Button;

		default:
			return CEGUI::LeftButton;
		}
	};

	bool ControllerInput::OISMouse_MouseReleased(const OIS::MouseEvent &arg, OIS::MouseButtonID id)
    {		
		// exit conditions
		if (OgreClient::Singleton->Data->IsWaiting ||
			!OgreClient::Singleton->RenderWindow ||
			!OgreClient::Singleton->RenderWindow->isVisible() ||
			!OgreClient::Singleton->RenderWindow->isActive() ||
			!OgreClient::Singleton->HasFocus ||
			!isMouseInWindow)
			return true;
			
		// inject mouseup to cegui
		ControllerUI::InjectMouseButtonUp(GetCEGUIMouseButton(id));
		
		// below here must be in playing mode (no login etc.)
		if (OgreClient::Singleton->Data->UIMode != UIMode::Playing)
			return true;

		// perform leftclick select
        if (id == OIS::MouseButtonID::MB_Left &&
			!isMouseWentDownOnUI && 
            !IsRightMouseDown &&
            (OgreClient::Singleton->GameTick->Current - tickMouseDownLeft < MOUSECLICKMAXDELAY))
        {
			// actually perform this click
			PerformMouseOver(arg.state.X.abs, arg.state.Y.abs, true);           
        }
        
		// perform rightclick action
		if (id == OIS::MouseButtonID::MB_Right &&
			!isMouseWentDownOnUI &&
			OgreClient::Singleton->GameTick->Current - tickMouseDownRight < MOUSECLICKMAXDELAY &&
			OgreClient::Singleton->Data->RoomObjects->HighlightedID > 0)
		{			
			// mark highlighted for next target on some actions
			OgreClient::Singleton->Data->IsNextAttackApplyCastOnHighlightedObject = true;

			// activate the mapped acton for rightlicck
			OgreClient::Singleton->Data->ActionButtons[ActiveKeyBinding->RightClickAction - 1]->Activate();		
		}

		isMouseWentDownOnUI = false;

        return true;
    };

	bool ControllerInput::OISMouse_MousePressed(const OIS::MouseEvent &arg, OIS::MouseButtonID id)
    {   
		// exit conditions
		if (OgreClient::Singleton->Data->IsWaiting ||
			!OgreClient::Singleton->RenderWindow ||
			!OgreClient::Singleton->RenderWindow->isVisible() ||
			!OgreClient::Singleton->RenderWindow->isActive() ||
			!OgreClient::Singleton->HasFocus ||
			!isMouseInWindow)
			return true;

		// save windows cursor position
		GetCursorPos(mouseDownWindowsPosition);

		// inject mousedown to cegui
		ControllerUI::InjectMouseButtonDown(GetCEGUIMouseButton(id));
			
		// save whether the mouse went down on a UI control which should not be ignored
		isMouseWentDownOnUI = !ControllerUI::IgnoreTopControlForMouseInput;

		// save down tick
		if (id == OIS::MouseButtonID::MB_Left)
			tickMouseDownLeft = OgreClient::Singleton->GameTick->Current;
		
		if (id == OIS::MouseButtonID::MB_Right)
			tickMouseDownRight = OgreClient::Singleton->GameTick->Current;

		/******************************************************/

		// rightclick: resync the avatars orientation to the camera lookat (on x,z)
		if (IsRightMouseDown && !isMouseWentDownOnUI)
			SetAvatarOrientationFromCamera();

		// stop automove on manual forward/backward
		if (isAutoMove && 
			oisMouse->getMouseState().buttonDown(OIS::MouseButtonID::MB_Left) &&
			oisMouse->getMouseState().buttonDown(OIS::MouseButtonID::MB_Right))
		{
			isAutoMove = false;
			isAutoMoveOnMove = false;
		}

        return true;
    };

	bool ControllerInput::OISMouse_MouseMoved(const OIS::MouseEvent &arg)
    {   		
		// delta pixels
		int dx = arg.state.X.rel;
		int dy = arg.state.Y.rel;
		int dz = arg.state.Z.rel;
		
		if (dx == 0 && dy == 0 && dz == 0)
			return true;

		bool isAiming = false;

		// update flag whether mouse is in window or not
		IsMouseInWindow = (
			arg.state.X.abs > 0 && arg.state.Y.abs > 0 &&
			arg.state.X.abs < arg.state.width && arg.state.Y.abs < arg.state.height);

		// inject mousemove to cegui	
		ControllerUI::InjectMousePosition((float)arg.state.X.abs, (float)arg.state.Y.abs);
		
		// if the cursor moved outside the window
		// make sure to release any pressed keys
		if (!isMouseInWindow)
		{
			if (IsLeftMouseDown)
				ControllerUI::InjectMouseButtonUp(CEGUI::MouseButton::LeftButton);

			if (IsRightMouseDown)
				ControllerUI::InjectMouseButtonUp(CEGUI::MouseButton::RightButton);
		}

		// exit conditions
		if (!OgreClient::Singleton->RenderWindow ||
			!OgreClient::Singleton->RenderWindow->isVisible() ||
			!OgreClient::Singleton->RenderWindow->isActive() ||
			!OgreClient::Singleton->HasFocus ||
			!isMouseInWindow)
			return true;
		
		// inject wheel
		ControllerUI::InjectMouseWheelChange((float)dz / 120.0f);
						
		// the cameranode
		SceneNode* cameraNode = OgreClient::Singleton->CameraNode;

		// exit conditions for 'actual playing' e.g. with avatar set
		if (OgreClient::Singleton->Data->IsWaiting || 
			Avatar == nullptr ||                
            Avatar->SceneNode == nullptr ||
			cameraNode == nullptr)
			return true;

		if (!isMouseWentDownOnUI)
		{
			// there is a small delay until aiming starts, to not shackle the
			// camera with any short mouseclick
			double dtRightButton = OgreClient::Singleton->GameTick->Current - tickMouseDownRight;
			double dtLeftButton = OgreClient::Singleton->GameTick->Current - tickMouseDownLeft;

			// right mousebutton (or both) pressed and dleay for mouseaim exceeded
			if (IsRightMouseDown && dtRightButton > MOUSELOOKMINDELAY)
			{				
				if (dx != 0)					    
				{
					// stop immediately if we switched directions
					if (::System::Math::Sign(dx) != ::System::Math::Sign(avatarYawDelta))
						avatarYawDelta = 0.0f;

					// set a new delta and stepsize
					// this will be processed tick based
					avatarYawDelta += MOUSELOOKSPEED * (float)OgreClient::Singleton->Config->MouseAimSpeed * (float)dx;
					avatarYawStep = MOUSELOOKSTEPFACT * avatarYawDelta * ::System::Math::Max((float)OgreClient::Singleton->GameTick->Span, 1.0f);

					isAiming = true;
				}

				if (dy != 0)
				{
					// invert mouse y if enabled in config
					if (OgreClient::Singleton->Config->InvertMouseY)
						dy = -dy;

					// stop immediately if we switched directions
					if (::System::Math::Sign(dy) != ::System::Math::Sign(cameraPitchDelta))
						cameraPitchDelta = 0.0f;

					// set a new delta and stepsize
					// this will be processed tick based
					cameraPitchDelta += MOUSELOOKSPEED * (float)OgreClient::Singleton->Config->MouseAimSpeed * (float)dy;
					cameraPitchStep = MOUSELOOKSTEPFACT * cameraPitchDelta * ::System::Math::Max((float)OgreClient::Singleton->GameTick->Span, 1.0f);

					isAiming = true;
				}
			}

			// left mousebutton pressed and delay for mouseaim exceeded
			else if (IsLeftMouseDown && dtLeftButton > MOUSELOOKMINDELAY)
			{                    
				if (dx != 0)
				{
					// stop immediately if we switched directions
					if (::System::Math::Sign(dx) != ::System::Math::Sign(cameraYawDelta))
						cameraYawDelta = 0.0f;

					// set a new delta and stepsize
					// this will be processed tick based
					cameraYawDelta += MOUSELOOKSPEED * (float)OgreClient::Singleton->Config->MouseAimSpeed * (float)dx;
					cameraYawStep = MOUSELOOKSTEPFACT * cameraYawDelta * ::System::Math::Max((float)OgreClient::Singleton->GameTick->Span, 1.0f);

					isAiming = true;
				}
	
				if (dy != 0)
				{	
					// invert mouse y if enabled in config
					if (OgreClient::Singleton->Config->InvertMouseY)
						dy = -dy;

					// stop immediately if we switched directions
					if (::System::Math::Sign(dy) != ::System::Math::Sign(cameraPitchDelta))
						cameraPitchDelta = 0.0f;

					// set a new delta and stepsize
					// this will be processed tick based
					cameraPitchDelta += MOUSELOOKSPEED * (float)OgreClient::Singleton->Config->MouseAimSpeed * (float)dy;
					cameraPitchStep = MOUSELOOKSTEPFACT * cameraPitchDelta * ::System::Math::Max((float)OgreClient::Singleton->GameTick->Span, 1.0f);

					isAiming = true;
				}
			}

			// mousewheel / zoom
			if (dz != 0 && ControllerUI::IgnoreTopControlForMouseInput)
			{			
				// set a new delta and stepsize
				// this will be processed tick based
				cameraZDelta += ZOOMSPEED * (float)dz;
				cameraZStep = MOUSELOOKSTEPFACT * cameraZDelta * ::System::Math::Max((float)OgreClient::Singleton->GameTick->Span, 1.0f);
			}

			// restore/fixed windows cursor position on mouse look										
			if (IsAnyMouseDown)							
				SetCursorPos(mouseDownWindowsPosition->x, mouseDownWindowsPosition->y);	
		}
		
		if (!isAiming && ControllerUI::IgnoreTopControlForMouseInput)
			PerformMouseOver(arg.state.X.abs, arg.state.Y.abs, false);												 

        return true;
    };

	bool ControllerInput::OISKeyboard_KeyPressed(const OIS::KeyEvent &arg)
    {
		// inject keypressed to cegui
		ControllerUI::InjectKeyDown((CEGUI::Key::Scan)arg.key);
		ControllerUI::InjectChar((CEGUI::Key::Scan)arg.text);

		// exit conditions
		if (ControllerUI::ProcessingInput ||
			OgreClient::Singleton->Data->IsWaiting ||
			!OgreClient::Singleton->RenderWindow ||
			!OgreClient::Singleton->RenderWindow->isActive() ||
			!OgreClient::Singleton->HasFocus ||
			OgreClient::Singleton->CameraNode == nullptr ||
			Avatar == nullptr ||
			Avatar->SceneNode == nullptr)
			return true;
        
        if (arg.key == ActiveKeyBinding->ReqGo)
        {
            OgreClient::Singleton->SendReqGo(true);                  
        }
			
		else if (arg.key == ActiveKeyBinding->Close)
		{
			if (ControllerUI::FocusedControl == nullptr || 
				ControllerUI::FocusedControl == ControllerUI::GUIRoot)
			{
				OgreClient::Singleton->Data->TargetID = 0xFFFFFFFFU;
			}
		}

        else if (arg.key == ActiveKeyBinding->NextTarget)
        {
            OgreClient::Singleton->Data->NextTarget();
        }            
        
		else if (arg.key == ActiveKeyBinding->AutoMove)
		{
			// if turned on, save whether turned on while moving
			isAutoMoveOnMove = (!isAutoMove && (IsBothMouseDown || 
				OISKeyboard->isKeyDown(ActiveKeyBinding->MoveForward) ||
                OISKeyboard->isKeyDown(ActiveKeyBinding->MoveBackward)));

			// flip automove
			isAutoMove = !isAutoMove;			
		}

        return true;
    };

	bool ControllerInput::OISKeyboard_KeyReleased(const OIS::KeyEvent &arg)
    {
		// inject keyreleased to cegui
		ControllerUI::InjectKeyUp((CEGUI::Key::Scan)arg.key);

		// exit conditions
		if (ControllerUI::ProcessingInput ||
			OgreClient::Singleton->Data->IsWaiting ||
			!OgreClient::Singleton->RenderWindow ||
			!OgreClient::Singleton->RenderWindow->isActive() ||
			!OgreClient::Singleton->HasFocus ||
			OgreClient::Singleton->CameraNode == nullptr ||
			Avatar == nullptr ||
			Avatar->SceneNode == nullptr)
			return true;
        
        // update orientation
        if (ActiveKeyBinding->IsRotateKey(arg.key))
        {
            // possibly send update
            OgreClient::Singleton->SendReqTurnMessage();  
        }

        // send a forced final position update
        // if we released a movement key
        if (ActiveKeyBinding->IsMovementKey(arg.key))
            OgreClient::Singleton->SendReqMoveMessage(true);                
        
		// stop automove on manual forward/backward
		if (isAutoMove && (
			arg.key == ActiveKeyBinding->MoveForward ||
            arg.key == ActiveKeyBinding->MoveBackward))
		{
			if (!isAutoMoveOnMove) 
				isAutoMove = false;

			else
				isAutoMoveOnMove = false;
		}

        return true;
    };

	void ControllerInput::SetAvatarOrientationFromCamera()
    {
		if (Avatar != nullptr && 
			Avatar->SceneNode != nullptr &&
			Avatar->RoomObject != nullptr &&
			OgreClient::Singleton->Camera != nullptr &&
			OgreClient::Singleton->CameraNode != nullptr)
		{
			/*::Ogre::Vector3 dir = OgreClient::Singleton->Camera->getRealDirection();
			Quaternion camOrient = OgreClient::Singleton->Camera->getRealOrientation();

			//::System::Console::WriteLine(
			//	dir.x.ToString() + " " + dir.y.ToString() + " " + dir.z.ToString());
		
			Avatar->RoomObject->Angle = MathUtil::GetRadianForDirection(
				V2(dir.x, dir.z));
			
			OgreClient::Singleton->CameraNode->_setDerivedOrientation(camOrient);
			
			// update orientation on server
			OgreClient::Singleton->SendReqTurnMessage();*/
			
			/****************************************************************************/

			Quaternion camOrient = OgreClient::Singleton->Camera->getRealOrientation();
			Quaternion avatarOrient = Avatar->SceneNode->getOrientation();
			
			// rotate the avatar to the cam "direction"
			::Ogre::Vector3 xAxis = avatarOrient.xAxis();
			Quaternion quat1 = xAxis.getRotationTo(camOrient.xAxis());
			Avatar->SceneNode->rotate(quat1, Node::TransformSpace::TS_WORLD);

			// get changed orientation
			avatarOrient = Avatar->SceneNode->getOrientation();

			// fix some ugly wrong orientation by making sure we're vertical
			::Ogre::Vector3 yAxis = avatarOrient.yAxis();
			Quaternion quat2 = yAxis.getRotationTo(::Ogre::Vector3::UNIT_Y);
			Avatar->SceneNode->rotate(quat2, Node::TransformSpace::TS_WORLD);

			// get changed orientation again
			avatarOrient = Avatar->SceneNode->getOrientation();	
        
			// restore cam position (was rotated above because attached)
			OgreClient::Singleton->CameraNode->_setDerivedOrientation(camOrient);

			// update datalayey with new orientation
			Avatar->RoomObject->Angle = Util::ToRadianAngle(&avatarOrient);

			// update orientation on server
			OgreClient::Singleton->SendReqTurnMessage();
		}
    };
	
	V2 ControllerInput::GetMoveVector()
    {
        // direction vector
        ::Ogre::Vector3 move = ::Ogre::Vector3::ZERO;

        // get key states
        bool isForward		= OISKeyboard->isKeyDown(ActiveKeyBinding->MoveForward);
        bool isBackwards	= OISKeyboard->isKeyDown(ActiveKeyBinding->MoveBackward);
        bool isLeft			= OISKeyboard->isKeyDown(ActiveKeyBinding->MoveLeft);
        bool isRight		= OISKeyboard->isKeyDown(ActiveKeyBinding->MoveRight);
		bool uiReadsKeys	= ControllerUI::ProcessingInput;

        // orientation axes of controlled node
		Quaternion orient = Avatar->SceneNode->getOrientation();

        ::Ogre::Vector3 xAxis = orient.xAxis().normalisedCopy();
        ::Ogre::Vector3 zAxis = orient.zAxis().normalisedCopy();

        // apply keystates on direction vector
        if ((isForward && !uiReadsKeys) || IsBothMouseDown || isAutoMove)            
            move += -zAxis;
            
        if (isBackwards && !uiReadsKeys)
            move += zAxis;

        if (isLeft && !uiReadsKeys)
            move += -xAxis;

        if (isRight && !uiReadsKeys)
            move += xAxis;

        // get a V2 variant on ground
        V2 direction;
		direction.X = move.x;
		direction.Y = move.z;
		
		// normalize
		direction.Normalize();
      
        return direction;
    };

	void ControllerInput::Tick(double Tick, double Span)
    {	
		if (!IsInitialized)
			return;

		/********************* READ INPUT *********************/
		/*        This will raise the event handlers          */   
		/******************************************************/
		
		if (!oisKeyboard || !oisMouse)
			return;

		oisKeyboard->capture();
		oisMouse->capture();
				
		/********************* EXIT CASES *********************/
		/*        Don't go on if any of these match           */
		/******************************************************/
		
		if (OgreClient::Singleton->Data->IsWaiting ||
			!OgreClient::Singleton->RenderWindow ||
			!OgreClient::Singleton->RenderWindow->isVisible() ||
			!OgreClient::Singleton->RenderWindow->isActive() ||
			!OgreClient::Singleton->HasFocus ||
			Avatar == nullptr ||
			Avatar->SceneNode == nullptr)
			return;
		
		/*************** SELF TARGET MODIFIER *****************/
		/*         Process keydown of some specific keys      */
		/******************************************************/

		// update flag whether selftarget modifier key is down     
		OgreClient::Singleton->Data->SelfTarget = IsSelfTargetDown;

		/****************** CAMERA PITCH/YAW ******************/
		/*      Apply frame-based smooth camera pitch/yaw     */
		/******************************************************/	
		
		SceneNode* cameraNode = OgreClient::Singleton->CameraNode;
		Camera* camera = OgreClient::Singleton->Camera;

		if (cameraNode && camera)
		{
			// get cameranode orientation
			Quaternion orientation = cameraNode->_getDerivedOrientation();

			// save/update current pitch and yaw values
			cameraPitchCurrent = orientation.getPitch().valueRadians();
			cameraYawCurrent = orientation.getYaw().valueRadians();

			// calculate new pitch and yaw values for this step/frame
			float destPitch = cameraPitchCurrent + cameraPitchStep;
			float destYaw = cameraYawCurrent + cameraYawStep;			

			// 1. PITCH						
			if (cameraPitchDelta != 0.0f)
			{
				// apply pitchstep on camera
				cameraNode->pitch(Radian(-cameraPitchStep));	
				
				// verify pitchover
				Quaternion orientation = cameraNode->_getDerivedOrientation();
				float pitchOver = orientation.getPitch().valueRadians();
			
				// don't allow overpitching, so pitch back possibly
				if (pitchOver >= Math::HALF_PI ||
					pitchOver <= -Math::HALF_PI)
				{
					cameraNode->pitch(Radian(cameraPitchStep));
					cameraPitchDelta = 0.0f;
					cameraPitchStep = 0.0f;
				}
				else if (cameraPitchDelta > 0.0f)
				{
					cameraPitchDelta -= cameraPitchStep;
					cameraPitchDelta = ::System::Math::Max(cameraPitchDelta, 0.0f);
				}
				else if (cameraPitchDelta < 0.0f)
				{
					cameraPitchDelta -= cameraPitchStep;
					cameraPitchDelta = ::System::Math::Min(cameraPitchDelta, 0.0f);
				}			
			}
			
			// 2. YAW
			if (cameraYawDelta != 0.0f)
			{
				// apply yawstep on camera
				cameraNode->yaw(Radian(-cameraYawStep), Node::TransformSpace::TS_WORLD);

				if (cameraYawDelta > 0.0f)
				{
					cameraYawDelta -= cameraYawStep;
					cameraYawDelta = ::System::Math::Max(cameraYawDelta, 0.0f);
				}
				else if (cameraYawDelta < 0.0f)
				{
					cameraYawDelta -= cameraYawStep;
					cameraYawDelta = ::System::Math::Min(cameraYawDelta, 0.0f);
				}
			}

			// 3. ZOOM
			if (cameraZDelta != 0.0f)
			{	
				float destZ = camera->getPosition().z - cameraZStep;

				//
				if (destZ > 0.0f)
				{
					// mark as 3. person camera mode
					IsCameraFirstPerson = false;
					
					// hide 1. person overlays
					ControllerUI::PlayerOverlays::HideOverlays();

					// apply the move
					camera->moveRelative(::Ogre::Vector3(0.0f, 0.0f, -cameraZStep));
					camera->_notifyMoved();
 
					if (cameraZDelta > 0.0f)
					{									
						cameraZDelta -= cameraZStep;
						cameraZDelta = ::System::Math::Max(cameraZDelta, 0.0f);
					}
					else if (cameraZDelta < 0.0f)
					{
						cameraZDelta -= cameraZStep;
						cameraZDelta = ::System::Math::Min(cameraZDelta, 0.0f);
					}
				}
				else
				{
					// mark as 1. person camera mode
					IsCameraFirstPerson = true;

					// show 1. person overlays
					ControllerUI::PlayerOverlays::ShowOverlays();

					// apply the move to the 0.0f
					camera->moveRelative(::Ogre::Vector3(0.0f, 0.0f, -cameraZStep - destZ));
					camera->_notifyMoved();
 
					cameraZDelta = 0.0f;
				}
			}
		}

		/********************* AVATAR YAW *********************/
		/*         Apply frame-based smooth avatar yaw        */
		/******************************************************/	
		
		// YAW
		if (avatarYawDelta != 0.0f)
		{
			// apply yawstep on avatar
			OgreClient::Singleton->TryYaw(avatarYawStep);

			if (avatarYawDelta > 0.0f)
			{
				avatarYawDelta -= avatarYawStep;
				avatarYawDelta = ::System::Math::Max(avatarYawDelta, 0.0f);
			}
			else if (avatarYawDelta < 0.0f)
			{
				avatarYawDelta -= avatarYawStep;
				avatarYawDelta = ::System::Math::Min(avatarYawDelta, 0.0f);
			}
		}

		/********************* MOVEMENT INPUT *****************/
		/*  left, right, up, down keys or both mouse buttons  */
		/******************************************************/	
		
		bool isMoveByMouseOrKeysNotUsedByUI = 
			(IsMovementInput && (IsBothMouseDown || !ControllerUI::ProcessingInput));

		if (isAutoMove || isMoveByMouseOrKeysNotUsedByUI)
		{
			// get movespeed based on walk-modifier key
			MovementSpeed speed = (IsWalkKeyDown) ? MovementSpeed::Walk : MovementSpeed::Run;
			
			// get movement direction vector
			V2 direction = GetMoveVector();

			// get the height of the avatar in ROO format
			float playerheight = 0.88f * 16.0f * Avatar->SceneNode->_getWorldAABB().getSize().y;
				
			// try to do the move (might get blocked)				
			OgreClient::Singleton->TryMove(direction, (unsigned char)speed, playerheight);
		}
				
		/************** KEYBOARD ONLY FROM HERE ON ************/
		/*               exit if UI is reading it             */
		/******************************************************/

		if (ControllerUI::ProcessingInput)
			return;
		
		/******************** ROTATION KEYS *******************/
		/*             rotate left/right by keyboard          */
		/******************************************************/	
		
        // Update Angle in dataobject and possibly send a update packet                  
		if (IsRotateKeyDown)
        {									          
            // scaled base delta
            float diff = KEYROTATESPEED * (float)OgreClient::Singleton->Config->KeyRotateSpeed * (float)OgreClient::Singleton->GameTick->Span;
			
			// half rotation speed on walk
			if (IsWalkKeyDown)
				diff *= 0.5f;

            // left
            if (OISKeyboard->isKeyDown(ActiveKeyBinding->RotateLeft))
            {
				// try to yaw and yaw camera back internally if suceeded
                if (OgreClient::Singleton->TryYaw(-diff) && IsLeftMouseDown && !isMouseWentDownOnUI)
                    cameraNode->yaw(Radian(-diff), Node::TransformSpace::TS_WORLD);
            }

            // right
            if (OISKeyboard->isKeyDown(ActiveKeyBinding->RotateRight))
            {
                // try to yaw and yaw camera back internally if suceeded                   
                if (OgreClient::Singleton->TryYaw(diff) && IsLeftMouseDown && !isMouseWentDownOnUI)
                    cameraNode->yaw(Radian(diff), Node::TransformSpace::TS_WORLD);
            }
		}
		
		/**************** ACTION BUTTON KEYS ******************/
		/*       these will trigger the defined actions       */
		/******************************************************/	

		ActionButtonList^ actionButtons = OgreClient::Singleton->Data->ActionButtons;

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton01))		
			actionButtons[0]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton02))		
			actionButtons[1]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton03))		
			actionButtons[2]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton04))		
			actionButtons[3]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton05))	
			actionButtons[4]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton06))		
			actionButtons[5]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton07))	
			actionButtons[6]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton08))		
			actionButtons[7]->Activate();		
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton09))		
			actionButtons[8]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton10))		
			actionButtons[9]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton11))		
			actionButtons[10]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton12))		
			actionButtons[11]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton13))
			actionButtons[12]->Activate();
				
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton14))
			actionButtons[13]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton15))		
			actionButtons[14]->Activate();
				
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton16))		
			actionButtons[15]->Activate();	
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton17))		
			actionButtons[16]->Activate();
					
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton18))		
			actionButtons[17]->Activate();
				
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton19))		
			actionButtons[18]->Activate();		
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton20))		
			actionButtons[19]->Activate();		
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton21))	
			actionButtons[20]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton22))	
			actionButtons[21]->Activate();		
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton23))
			actionButtons[22]->Activate();
			
		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton24))
			actionButtons[23]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton25))
			actionButtons[24]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton26))
			actionButtons[25]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton27))
			actionButtons[26]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton28))
			actionButtons[27]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton29))
			actionButtons[28]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton30))
			actionButtons[29]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton31))
			actionButtons[30]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton32))
			actionButtons[31]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton33))
			actionButtons[32]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton34))
			actionButtons[33]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton35))
			actionButtons[34]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton36))
			actionButtons[35]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton37))
			actionButtons[36]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton38))
			actionButtons[37]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton39))
			actionButtons[38]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton40))
			actionButtons[39]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton41))
			actionButtons[40]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton42))
			actionButtons[41]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton43))
			actionButtons[42]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton44))
			actionButtons[43]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton45))
			actionButtons[44]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton46))
			actionButtons[45]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton47))
			actionButtons[46]->Activate();

		if (oisKeyboard->isKeyDown(ActiveKeyBinding->ActionButton48))
			actionButtons[47]->Activate();
    };
};};
