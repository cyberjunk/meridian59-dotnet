#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Skills::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_SKILLS_WINDOW));
		List	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_SKILLS_LIST));
		
		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutSkills->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutSkills->getSize());

		// attach listener to avatar skills
		OgreClient::Singleton->Data->AvatarSkills->ListChanged += 
			gcnew ListChangedEventHandler(OnSkillsListChanged);
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
	};

	void ControllerUI::Skills::Destroy()
	{	 
		// detach listener from avatar skills
		OgreClient::Singleton->Data->AvatarSkills->ListChanged -= 
			gcnew ListChangedEventHandler(OnSkillsListChanged);	
	};

	void ControllerUI::Skills::OnSkillsListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				SkillAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				SkillRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				SkillChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Skills::SkillAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		StatList^ obj = OgreClient::Singleton->Data->AvatarSkills[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARSKILLITEM);
		
		// set ID
		widget->setID(obj->ObjectID);

		// subscribe click event
		widget->subscribeEvent(
			CEGUI::ItemEntry::EventMouseClick, 
			CEGUI::Event::Subscriber(UICallbacks::Skills::OnItemClicked));
		
		CEGUI::DragContainer* dragger = 
			(CEGUI::DragContainer*)widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);
			
		CEGUI::Window* icon		= dragger->getChildAtIdx(0);				
		CEGUI::Window* name		= widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_NAME);
		CEGUI::Window* percent	= widget->getChildAtIdx(UI_SKILLS_CHILDINDEX_PERCENT);

		// insert in ui-list
		if ((int)List->getItemCount() > Index)
			List->insertItem(widget, List->getItemFromIndex(Index));
		
		// or add
		else
			List->addItem(widget);
		
		// fix a big with last item not visible
		// when insertItem was used
		List->notifyScreenAreaChanged(true);

		// update values
		SkillChange(Index);
	};

	void ControllerUI::Skills::SkillRemove(int Index)
	{
		// check
		if ((int)List->getItemCount() > Index)		
			List->removeItem(List->getItemFromIndex(Index));
	};

	void ControllerUI::Skills::SkillChange(int Index)
	{
		StatList^ obj = OgreClient::Singleton->Data->AvatarSkills[Index];

		// check
		if ((int)List->getItemCount() > Index)
		{
			CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);
			
			CEGUI::DragContainer* dragger = 
				(CEGUI::DragContainer*)wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_ICON);
			
			CEGUI::Window* icon		= dragger->getChildAtIdx(0);				
			CEGUI::Window* name		= wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_NAME);
			CEGUI::Window* percent	= wnd->getChildAtIdx(UI_SKILLS_CHILDINDEX_PERCENT);
								
			// set name
			name->setText(StringConvert::CLRToCEGUI(obj->ResourceName));
			percent->setText(CEGUI::PropertyHelper<unsigned int>::toString(obj->SkillPoints) + '%');

			// set image if available
			if (obj->Resource != nullptr && obj->Resource->Frames->Count > 0)
			{
				Ogre::TextureManager* texMan = Ogre::TextureManager::getSingletonPtr();
					
				// build name
				::Ogre::String oStrName = 
					StringConvert::CLRToOgre(UI_NAMEPREFIX_STATICICON + obj->ResourceIconName + "/0");

				// possibly create texture
				Util::CreateTextureA8R8G8B8(obj->Resource->Frames[0], oStrName, UI_RESGROUP_IMAGESETS, MIP_DEFAULT);

				// reget TexPtr (no return from function works, ugh..)
				TexturePtr texPtr = texMan->getByName(oStrName);

				if (!texPtr.isNull())
				{
					// possibly create cegui wrap around it
					Util::CreateCEGUITextureFromOgre(ControllerUI::Renderer, texPtr);

					// set image
					icon->setProperty(UI_PROPNAME_IMAGE, oStrName);
				}
			}			
		}
	};

	bool UICallbacks::Skills::OnItemClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::MouseEventArgs& args	= (const CEGUI::MouseEventArgs&)e;
		const CEGUI::ItemEntry* itm			= (CEGUI::ItemEntry*)args.window;

		// rightclick requests object details
		if (args.button == CEGUI::MouseButton::RightButton)	
			OgreClient::Singleton->SendReqLookMessage(itm->getID());					
		
		return true;
	};
};};