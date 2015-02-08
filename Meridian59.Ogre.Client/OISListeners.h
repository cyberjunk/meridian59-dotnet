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
#include "OISMouse.h"
#pragma managed(pop)

namespace Meridian59 { namespace Ogre
{	

	public class OISKeyListener : public OIS::KeyListener
	{
	public:
		OISKeyListener(void);
		
		virtual bool keyPressed(const OIS::KeyEvent &arg) override;
		virtual bool keyReleased(const OIS::KeyEvent &arg) override;
	};

	public class OISMouseListener : public OIS::MouseListener
	{
	public:
		OISMouseListener(void);
		
		virtual bool mouseMoved(const OIS::MouseEvent &arg) override;
		virtual bool mousePressed(const OIS::MouseEvent &arg, OIS::MouseButtonID id) override;
		virtual bool mouseReleased(const OIS::MouseEvent &arg, OIS::MouseButtonID id) override;
	};
};};
