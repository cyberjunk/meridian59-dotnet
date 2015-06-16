#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Avatar::Initialize()
	{		
		// get windowmanager
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();

		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_AVATAR_WINDOW));
		Head			= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_AVATAR_HEAD));
		Enchantments	= static_cast<CEGUI::GridLayoutContainer*>(Window->getChild(UI_NAME_AVATAR_ENCHANTMENTS));
		Conditions		= static_cast<CEGUI::VerticalLayoutContainer*>(Window->getChild(UI_NAME_AVATAR_CONDITIONS));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutAvatar->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutAvatar->getSize());

		// attach listener to Data
		OgreClient::Singleton->Data->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
        
		// attach listener to personal enchantments
		OgreClient::Singleton->Data->AvatarBuffs->ListChanged += 
			gcnew ListChangedEventHandler(OnBuffListChanged);

		// attach listener to condition stats
		OgreClient::Singleton->Data->AvatarCondition->ListChanged += 
			gcnew ListChangedEventHandler(OnConditionListChanged);

		// amount of entries the buffgrid can hold
		const int entries = UI_AVATAR_ENCHANTMENTS_COLS * UI_AVATAR_ENCHANTMENTS_ROWS;

		// set dimension (no. of items per row and no. of cols)
		Enchantments->setGridDimensions(UI_AVATAR_ENCHANTMENTS_COLS, UI_AVATAR_ENCHANTMENTS_ROWS);

		// image composer for head picture
		imageComposerHead = gcnew ImageComposerCEGUI<RoomObject^>();
		imageComposerHead->ApplyYOffset = false;
		imageComposerHead->HotspotIndex = (unsigned char)KnownHotspot::HEAD;
        imageComposerHead->IsScalePow2 = false;
        imageComposerHead->UseViewerFrame = false;
		imageComposerHead->Width = (unsigned int)Head->getPixelSize().d_width;
        imageComposerHead->Height = (unsigned int)Head->getPixelSize().d_height;
        imageComposerHead->CenterHorizontal = true;
        imageComposerHead->CenterVertical = true;
		imageComposerHead->NewImageAvailable += gcnew ::System::EventHandler(OnNewHeadImageAvailable);
	
		// create image composers for buffslots
		imageComposersBuffs = gcnew array<ImageComposerCEGUI<ObjectBase^>^>(entries);

		for(int i = 0; i < entries; i++)
		{
			imageComposersBuffs[i] = gcnew ImageComposerCEGUI<ObjectBase^>();
			imageComposersBuffs[i]->ApplyYOffset = false;
			imageComposersBuffs[i]->HotspotIndex = 0;
			imageComposersBuffs[i]->IsScalePow2 = false;
			imageComposersBuffs[i]->UseViewerFrame = false;
			imageComposersBuffs[i]->Width = UI_BUFFICON_WIDTH;
			imageComposersBuffs[i]->Height = UI_BUFFICON_HEIGHT;
			imageComposersBuffs[i]->CenterHorizontal = true;
			imageComposersBuffs[i]->CenterVertical = true;
			imageComposersBuffs[i]->NewImageAvailable += gcnew ::System::EventHandler(OnNewBuffImageAvailable);
		}
			
		// create imagebuttons in slots
		for(int i = 0; i < entries; i++)
		{
			// create widget
			CEGUI::Window* widget = (CEGUI::Window*)wndMgr->createWindow(UI_WINDOWTYPE_BUFFICON);

			// some settings
			widget->setSize(CEGUI::USize(CEGUI::UDim(0, UI_BUFFICON_WIDTH), CEGUI::UDim(0, UI_BUFFICON_HEIGHT)));
			widget->setMouseCursor(UI_DEFAULTARROW);

#ifdef _DEBUG
			widget->setText(StringConvert::CLRToCEGUI(i.ToString()));
#endif
			// subscribe click
			widget->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Avatar::OnBuffMouseClick));

			// add
			Enchantments->addChild(widget);
		}

		// subscribe click to head
		Head->subscribeEvent(CEGUI::Window::EventMouseClick, CEGUI::Event::Subscriber(UICallbacks::Avatar::OnHeadMouseClick));

		// subscribe mouse events
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonDown, CEGUI::Event::Subscriber(UICallbacks::Avatar::OnMouseDown));
		Window->subscribeEvent(CEGUI::Window::EventMouseButtonUp, CEGUI::Event::Subscriber(UICallbacks::Avatar::OnMouseUp));		
	};

	void ControllerUI::Avatar::Destroy()
	{	
		// detach listener from Data
		OgreClient::Singleton->Data->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnDataPropertyChanged);
        
		// detach listener from personal enchantments
		OgreClient::Singleton->Data->AvatarBuffs->ListChanged -= 
			gcnew ListChangedEventHandler(OnBuffListChanged);

		// detach listener from condition stats
		OgreClient::Singleton->Data->AvatarCondition->ListChanged -= 
			gcnew ListChangedEventHandler(OnConditionListChanged);

		// detach listener from imagecompoesr head
		imageComposerHead->NewImageAvailable -= gcnew ::System::EventHandler(OnNewHeadImageAvailable);
		
		// amount of entries the buffgrid can hold
		const int entries = UI_AVATAR_ENCHANTMENTS_COLS * UI_AVATAR_ENCHANTMENTS_ROWS;
		
		// detach listeners from avatar buffs
		for(int i = 0; i < entries; i++)		
			imageComposersBuffs[i]->NewImageAvailable -= gcnew ::System::EventHandler(OnNewBuffImageAvailable);		
	};

	void ControllerUI::Avatar::OnDataPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		// avatar
		if (::System::String::Equals(e->PropertyName, DataController::PROPNAME_AVATAROBJECT))
		{
			RoomObject^ avatarObject = OgreClient::Singleton->Data->AvatarObject;

			// possibly set to null
			imageComposerHead->DataSource = avatarObject;

			if (avatarObject != nullptr)
			{
				Head->setTooltipText(StringConvert::CLRToCEGUI(avatarObject->Name));
			}
			else
			{
				// unset
				Head->setProperty(UI_PROPNAME_NORMALIMAGE, STRINGEMPTY);
				Head->setProperty(UI_PROPNAME_HOVERIMAGE, STRINGEMPTY);
				Head->setProperty(UI_PROPNAME_PUSHEDIMAGE, STRINGEMPTY);
				Head->setTooltipText(STRINGEMPTY);
			}
		}
	};

	void ControllerUI::Avatar::OnNewHeadImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		Head->setProperty(UI_PROPNAME_NORMALIMAGE, *imageComposerHead->Image->TextureName);
		Head->setProperty(UI_PROPNAME_HOVERIMAGE, *imageComposerHead->Image->TextureName);
		Head->setProperty(UI_PROPNAME_PUSHEDIMAGE, *imageComposerHead->Image->TextureName);
	};

	void ControllerUI::Avatar::OnNewBuffImageAvailable(Object^ sender, ::System::EventArgs^ e)
    {
		ImageComposerCEGUI<ObjectBase^>^ imageComposer = (ImageComposerCEGUI<ObjectBase^>^)sender;
		int index = ::System::Array::IndexOf(imageComposersBuffs, imageComposer);

		if ((int)Enchantments->getChildCount() > index)
		{
			// get imagebutton
			CEGUI::Window* imgButton = Enchantments->getChildAtIdx(index);
			
			imgButton->setProperty(UI_PROPNAME_NORMALIMAGE, *imageComposer->Image->TextureName);
			imgButton->setProperty(UI_PROPNAME_HOVERIMAGE, *imageComposer->Image->TextureName);
			imgButton->setProperty(UI_PROPNAME_PUSHEDIMAGE, *imageComposer->Image->TextureName);
		}
	};

	void ControllerUI::Avatar::OnBuffListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				BuffAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				BuffRemove(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Avatar::OnConditionListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				ConditionAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				ConditionRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				ConditionChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Avatar::BuffAdd(int Index)
	{
		// get new datamodel entry
		ObjectBase^ buffObject = OgreClient::Singleton->Data->AvatarBuffs[Index];

		// if we have that many slots..
		if ((int)Enchantments->getChildCount() > Index &&
			imageComposersBuffs->Length > Index)
		{			
			// get imagebutton
			CEGUI::Window* imgButton = Enchantments->getChildAtIdx(Index);
			
			// set new datasource on composer
			imageComposersBuffs[Index]->DataSource = buffObject;

			// set tooltip to name and mousecursor to target
			imgButton->setID(buffObject->ID);
			imgButton->setTooltipText(StringConvert::CLRToCEGUI(buffObject->Name));
			imgButton->setMouseCursor(UI_MOUSECURSOR_HAND);		
		}
	};

	void ControllerUI::Avatar::BuffRemove(int Index)
	{
		int childcount = Enchantments->getChildCount();

		// if we have that many slots..
		if (childcount > Index &&
			imageComposersBuffs->Length > Index)
		{
			// get imagebutton
			CEGUI::Window* imgButton = Enchantments->getChildAtIdx(Index);
			
			imgButton->setProperty(UI_PROPNAME_NORMALIMAGE, STRINGEMPTY);
			imgButton->setProperty(UI_PROPNAME_HOVERIMAGE, STRINGEMPTY);
			imgButton->setProperty(UI_PROPNAME_PUSHEDIMAGE, STRINGEMPTY);
			imgButton->setTooltipText(STRINGEMPTY);
			imgButton->setMouseCursor(UI_DEFAULTARROW);
			imgButton->setID(0);

			// reset datasource
			imageComposersBuffs[Index]->DataSource = nullptr;

			// rearrange
			ImageComposerCEGUI<ObjectBase^>^ swap;
			for (int i = Index; i < childcount - 1; i++)
			{
				// swap views
				Enchantments->swapChildren(
					Enchantments->getChildAtIdx(i), 
					Enchantments->getChildAtIdx(i+1));

				// swap composers
				swap = imageComposersBuffs[i];
				imageComposersBuffs[i] = imageComposersBuffs[i+1];
				imageComposersBuffs[i+1] = swap;
			}
		}
	};

	void ControllerUI::Avatar::ConditionAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		StatNumeric^ condition = OgreClient::Singleton->Data->AvatarCondition[Index];

		// create widget
		CEGUI::Window* widget = (CEGUI::Window*)wndMgr->createWindow(UI_WINDOWTYPE_AVATARCONDITIONITEM);
		
		// add it
		Conditions->addChild(widget);

		// update values
		ConditionChange(Index);
	};

	void ControllerUI::Avatar::ConditionRemove(int Index)
	{
		// check
		if ((int)Conditions->getChildCount() > Index)		
			Conditions->removeChildFromPosition(Index);		
	};

	void ControllerUI::Avatar::ConditionChange(int Index)
	{
		StatNumeric^ condition	= OgreClient::Singleton->Data->AvatarCondition[Index];

		// check
		if ((int)Conditions->getChildCount() > Index)
		{
			CEGUI::Window* wnd = (CEGUI::Window*)Conditions->getChildAtIdx(Index);

			// check
			if (wnd->getChildCount() > 1)
			{
				CEGUI::Window* wndImage		= (CEGUI::Window*)wnd->getChildAtIdx(UI_AVATAR_CHILDINDEX_CONDITION_IMAGE);
				CEGUI::ProgressBar* wndBar	= (CEGUI::ProgressBar*)wnd->getChildAtIdx(UI_AVATAR_CHILDINDEX_CONDITION_BAR);

				// either use maximum or rendermaximum
				int max = (condition->Num == StatNums::VIGOR || condition->Num == StatNums::TOUGHERCHANCE) ?
					condition->ValueRenderMax : condition->ValueMaximum;

				// map to range 0 - 1
				int range	= max - condition->ValueRenderMin;
				int fill	= condition->ValueCurrent - condition->ValueRenderMin;
				int range_b	= ::System::Math::Max(range, 1);
				float step	= 1.0f / (float)range_b;
				float perc	= (float)fill * step;

				// set progress
				wndBar->setProgress(perc);

				// set text
				wndBar->setText(
					CEGUI::PropertyHelper<int>::toString(condition->ValueCurrent) + "/" + CEGUI::PropertyHelper<int>::toString(max));

				// set tooltip
				switch(condition->Num)
				{
					case StatNums::HITPOINTS:
						wndBar->setTooltipText(UI_TOOLTIPTEXT_HPBAR);
						break;

					case StatNums::MANA:
						wndBar->setTooltipText(UI_TOOLTIPTEXT_MPBAR);
						break;

					case StatNums::VIGOR:
						wndBar->setTooltipText(UI_TOOLTIPTEXT_VIGBAR);
						break;

					case StatNums::TOUGHERCHANCE:
						wndBar->setTooltipText(UI_TOOLTIPTEXT_TOUGHERCHANCE);
						break;
				}

				// set image if available
				if (condition->Resource != nullptr && condition->Resource->Frames->Count > 0)
				{
					Ogre::TextureManager* texMan = Ogre::TextureManager::getSingletonPtr();
					
					// build name
					::Ogre::String oStrName = 
						StringConvert::CLRToOgre(UI_NAMEPREFIX_STATICICON + condition->ResourceName + "/0");

					// possibly create texture
					Util::CreateTextureA8R8G8B8(condition->Resource->Frames[0], oStrName, UI_RESGROUP_IMAGESETS, MIP_DEFAULT);

					// reget TexPtr (no return from function works, ugh..)
					TexturePtr texPtr = texMan->getByName(oStrName);

					if (!texPtr.isNull())
					{
						// possibly create cegui wrap around it
						Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);

						// set image
						wndImage->setProperty(UI_PROPNAME_IMAGE, oStrName);
					}
				}
			}
		}
	};

	bool UICallbacks::Avatar::OnHeadMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args = static_cast<const CEGUI::MouseEventArgs&>(e);

		// get id of our avatar
		unsigned int id = OgreClient::Singleton->Data->AvatarID;

		// if the avatar is actually set correctly
		if (ObjectID::IsValid(id))
		{
			// leftclick targets avatar
			if (args.button == CEGUI::MouseButton::LeftButton)
				OgreClient::Singleton->Data->TargetID = id;
			
			// rightclick requests avatar details
			else if (args.button == CEGUI::MouseButton::RightButton)			
				OgreClient::Singleton->SendReqLookMessage(id);		
		}

		return true;
	};

	bool UICallbacks::Avatar::OnBuffMouseClick(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::Window* itm			= (CEGUI::Window*)args.window;

		// single rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)		
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};

	bool UICallbacks::Avatar::OnMouseDown(const CEGUI::EventArgs& e)
	{
		// set this window as moving one
		ControllerUI::MovingWindow = ControllerUI::Avatar::Window;

		return true;
	};

	bool UICallbacks::Avatar::OnMouseUp(const CEGUI::EventArgs& e)
	{
		// unset this window as moving one
		ControllerUI::MovingWindow = nullptr;

		return true;
	};
};};