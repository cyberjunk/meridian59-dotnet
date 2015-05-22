#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::Attributes::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_ATTRIBUTES_WINDOW));
		List	= static_cast<CEGUI::ItemListbox*>(Window->getChild(UI_NAME_ATTRIBUTES_LIST));

		// set window layout from config
		Window->setPosition(OgreClient::Singleton->Config->UILayoutAttributes->getPosition());
		Window->setSize(OgreClient::Singleton->Config->UILayoutAttributes->getSize());

		// attach listener to avatar attributes
		OgreClient::Singleton->Data->AvatarAttributes->ListChanged += 
			gcnew ListChangedEventHandler(OnAttributesListChanged);
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
	};

	void ControllerUI::Attributes::Destroy()
	{
		// detach listener from avatar attributes
		OgreClient::Singleton->Data->AvatarAttributes->ListChanged -= 
			gcnew ListChangedEventHandler(OnAttributesListChanged);		
	};

	void ControllerUI::Attributes::OnAttributesListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				AttributeAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				AttributeRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				AttributeChange(e->NewIndex);
				break;
		}
	};

	void ControllerUI::Attributes::AttributeAdd(int Index)
	{
		CEGUI::WindowManager* wndMgr = CEGUI::WindowManager::getSingletonPtr();
		StatNumeric^ obj = OgreClient::Singleton->Data->AvatarAttributes[Index];

		// create widget (item)
		CEGUI::ItemEntry* widget = (CEGUI::ItemEntry*)wndMgr->createWindow(
			UI_WINDOWTYPE_AVATARATTRIBUTEITEM);
				
		// insert in ui-list
		if ((int)List->getItemCount() > Index)
			List->insertItem(widget, List->getItemFromIndex(Index));
		
		// or add
		else
			List->addItem(widget);

		// update values
		AttributeChange(Index);

		List->notifyScreenAreaChanged(true);
	};

	void ControllerUI::Attributes::AttributeRemove(int Index)
	{
		// check
		if ((int)List->getItemCount() > Index)		
			List->removeItem(List->getItemFromIndex(Index));
	};

	void ControllerUI::Attributes::AttributeChange(int Index)
	{
		StatNumeric^ obj = OgreClient::Singleton->Data->AvatarAttributes[Index];

		// check
		if ((int)List->getItemCount() > Index)
		{
			CEGUI::ItemEntry* wnd = (CEGUI::ItemEntry*)List->getItemFromIndex(Index);

			// check
			if (wnd->getChildCount() > 1)
			{
				CEGUI::Window* wndName		= (CEGUI::Window*)wnd->getChildAtIdx(UI_ATTRIBUTES_CHILDINDEX_NAME);
				CEGUI::ProgressBar* wndBar	= (CEGUI::ProgressBar*)wnd->getChildAtIdx(UI_ATTRIBUTES_CHILDINDEX_BAR);
								
				// set name
				wndName->setText(StringConvert::CLRToCEGUI(obj->ResourceName));
				
				// map to range 0 - 1
				int range	= obj->ValueRenderMax - obj->ValueRenderMin;
				int fill	= obj->ValueCurrent - obj->ValueRenderMin;
				int range_b	= ::System::Math::Max(range, 1);
				float step	= 1.0f / (float)range_b;
				float perc	= (float)fill * step;

				// set progress
				wndBar->setProgress(perc);

				// set text
				wndBar->setText(CEGUI::PropertyHelper<int>::toString(obj->ValueCurrent));
			}
		}
	};
};};