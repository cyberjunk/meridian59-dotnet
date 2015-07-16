#include "stdafx.h"

namespace Meridian59 {
	namespace Ogre
	{
		void ControllerUI::Quests::Initialize()
		{
			// setup references to children from xml nodes
			Window = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_QUESTS_WINDOW));
			List = static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_QUESTS_LIST));

			// set window layout from config
			//Window->setPosition(OgreClient::Singleton->Config->UILayoutSkills->getPosition());
			//Window->setSize(OgreClient::Singleton->Config->UILayoutSkills->getSize());

			// attach listener to avatar quests
			OgreClient::Singleton->Data->AvatarQuests->ListChanged +=
				gcnew ListChangedEventHandler(OnQuestsListChanged);

			// subscribe close button
			Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

			// subscribe keyup
			Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
		};

		void ControllerUI::Quests::Destroy()
		{
			// detach listener from avatar quests
			OgreClient::Singleton->Data->AvatarQuests->ListChanged -=
				gcnew ListChangedEventHandler(OnQuestsListChanged);
		};

		void ControllerUI::Quests::OnQuestsListChanged(Object^ sender, ListChangedEventArgs^ e)
		{
			switch (e->ListChangedType)
			{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				QuestAdd(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				QuestRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				QuestChange(e->NewIndex);
				break;
			}
		};

		void ControllerUI::Quests::QuestAdd(int Index)
		{
			CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
			StatList^ obj = OgreClient::Singleton->Data->AvatarQuests[Index];

			// create widget (item)
			CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
				UI_WINDOWTYPE_AVATARQUESTITEM);

			// set ID
			widget->setID(obj->ObjectID);

			// subscribe click event
			widget->subscribeEvent(
				CEGUI::ItemEntry::EventMouseClick,
				CEGUI::Event::Subscriber(UICallbacks::Quests::OnItemClicked));
			
			CEGUI::DragContainer* dragger =
				(CEGUI::DragContainer*)widget->getChildAtIdx(UI_QUESTS_CHILDINDEX_ICON);

			CEGUI::Window* icon = dragger->getChildAtIdx(0);
			CEGUI::Window* name = widget->getChildAtIdx(UI_QUESTS_CHILDINDEX_NAME);
			
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
			QuestChange(Index);
		};

		void ControllerUI::Quests::QuestRemove(int Index)
		{
			// check
			if ((int)List->getItemCount() > Index)
				List->removeItem(List->getItemFromIndex(Index));
		};

		void ControllerUI::Quests::QuestChange(int Index)
		{
			StatList^ obj = OgreClient::Singleton->Data->AvatarQuests[Index];

			// check
			if ((int)List->getItemCount() > Index)
			{
				CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);

				CEGUI::DragContainer* dragger =
					(CEGUI::DragContainer*)wnd->getChildAtIdx(UI_QUESTS_CHILDINDEX_ICON);

				CEGUI::Window* icon = dragger->getChildAtIdx(0);
				CEGUI::Window* name = wnd->getChildAtIdx(UI_QUESTS_CHILDINDEX_NAME);
				
				// set name
				name->setText(StringConvert::CLRToCEGUI(obj->ResourceName));
				
				// handle headers
				if (obj->SkillPoints == 0)
				{
					name->setFont(UI_FONT_LIBERATIONSANS10B);
				}
				else
				{
					name->setFont(UI_FONT_LIBERATIONSANS10);
				}

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

		bool UICallbacks::Quests::OnItemClicked(const CEGUI::EventArgs& e)
		{
			const CEGUI::MouseEventArgs& args = (const CEGUI::MouseEventArgs&)e;
			const CEGUI::ItemEntry* itm = (CEGUI::ItemEntry*)args.window;

			// rightclick requests object details
			if (args.button == CEGUI::MouseButton::RightButton)
				OgreClient::Singleton->SendReqLookMessage(itm->getID());

			return true;
		};
	};
};