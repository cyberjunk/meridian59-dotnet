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
#include "OgreBillboard.h"
#include "OgreBillboardSet.h"
#pragma managed(pop)

#include "ImageComposerOgre.h"
#include "RemoteNode.h"

namespace Meridian59 { namespace Ogre 
{
	using namespace ::Ogre;
	using namespace Meridian59::Drawing2D;
	using namespace Meridian59::Data::Models;
	using namespace System::Drawing::Drawing2D;

	/// <summary>
    /// A visible object based on a RoomObject and legacy resources (2D).
	/// This is a fake 3D object, implemented using Ogre's Billboards.
    /// </summary>
	public ref class RemoteNode2D : public RemoteNode
	{
	protected:
		::Ogre::Billboard* billboard;
        ::Ogre::BillboardSet* billboardSet;     
		ImageComposerOgre<Data::Models::RoomObject^>^ imageComposer;

		void OnNewImageAvailable(Object^ sender, System::EventArgs^ e);
		virtual void CreateLight() override;
		virtual void UpdateMaterial() override;

	public:		
		/// <summary>
        /// Constructor
        /// </summary>
		RemoteNode2D(Data::Models::RoomObject^ RoomObject, ::Ogre::SceneManager* SceneManager);
		
		/// <summary>
        /// Destructor
        /// </summary>
		~RemoteNode2D();

		virtual void RefreshPosition() override;
	};
};};
