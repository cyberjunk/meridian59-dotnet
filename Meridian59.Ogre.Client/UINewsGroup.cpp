#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::NewsGroup::Initialize()
	{
		// setup references to children from xml nodes
		Window		= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_NEWSGROUP_WINDOW));
		List		= static_cast<CEGUI::MultiColumnList*>(Window->getChild(UI_NAME_NEWSGROUP_LIST));
		HeadLine	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_NEWSGROUP_HEADLINE));
		Create		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NEWSGROUP_CREATE));
		Respond		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NEWSGROUP_RESPOND));
		MailAuthor	= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NEWSGROUP_MAILAUTHOR));
		Refresh		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NEWSGROUP_REFRESH));
		Delete		= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_NEWSGROUP_DELETE));
		Text		= static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_NEWSGROUP_TEXT));

		List->setShowVertScrollbar(true);
		List->setSelectionMode(CEGUI::MultiColumnList::SelectionMode::RowSingle);
		List->setUserSortControlEnabled(false);

		List->addColumn("Title", 0, CEGUI::UDim(0.6f, -60));
		List->addColumn("Author", 1, CEGUI::UDim(0.4f, -60));
		List->addColumn("Date", 2, CEGUI::UDim(0.0f, 105));

		// attach listener to newsgroup data
		OgreClient::Singleton->Data->NewsGroup->PropertyChanged += 
			gcnew PropertyChangedEventHandler(OnNewsGroupPropertyChanged);
        
		// attach listener to newsgroup articleheads
		OgreClient::Singleton->Data->NewsGroup->Articles->ListChanged += 
			gcnew ListChangedEventHandler(OnArticleHeadListChanged);
		
		// subscribe buttons
		Create->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnCreateClicked));
		Respond->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnRespondClicked));
		MailAuthor->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnMailAuthorClicked));
		Refresh->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnRefreshClicked));
		Delete->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnDeleteClicked));

		// subscribe selection change
		List->subscribeEvent(CEGUI::MultiColumnList::EventSelectionChanged, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnSelectionChanged));
		List->subscribeEvent(CEGUI::MultiColumnList::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnKeyUp));
		
		// subscribe keydown on headline box and text
		Text->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::NewsGroup::OnWindowKeyUp));
	};

	void ControllerUI::NewsGroup::Destroy()
	{	 
		// detach listener from newsgroup data
		OgreClient::Singleton->Data->NewsGroup->PropertyChanged -= 
			gcnew PropertyChangedEventHandler(OnNewsGroupPropertyChanged);
        
		// detach listener from newsgroup articleheads
		OgreClient::Singleton->Data->NewsGroup->Articles->ListChanged -= 
			gcnew ListChangedEventHandler(OnArticleHeadListChanged);		
	};

	void ControllerUI::NewsGroup::ApplyLanguage()
	{
	};

	void ControllerUI::NewsGroup::OnNewsGroupPropertyChanged(Object^ sender, PropertyChangedEventArgs^ e)
	{
		Data::Models::NewsGroup^ obj = OgreClient::Singleton->Data->NewsGroup;
		
		// visible
		if (::System::String::Equals(e->PropertyName, Data::Models::NewsGroup::PROPNAME_ISVISIBLE))
		{
			// hide or show
			Window->setVisible(obj->IsVisible);

			// bring to front
			if (obj->IsVisible)
				Window->moveToFront();
		}

		// headline
		else if (::System::String::Equals(e->PropertyName, Data::Models::NewsGroup::PROPNAME_HEADLINE))
		{
			HeadLine->setText(StringConvert::CLRToCEGUI(obj->Headline));
		}

		// globeobject
		else if (::System::String::Equals(e->PropertyName, Data::Models::NewsGroup::PROPNAME_NEWSGLOBEOBJECT))
		{
			if (obj->NewsGlobeObject != nullptr)
				Window->setText(StringConvert::CLRToCEGUI(obj->NewsGlobeObject->Name));
		}

		// text
		else if (::System::String::Equals(e->PropertyName, Data::Models::NewsGroup::PROPNAME_TEXT))
		{
			Text->setText(StringConvert::CLRToCEGUI(obj->Text));
		}
	};
	
	void ControllerUI::NewsGroup::OnArticleHeadListChanged(Object^ sender, ListChangedEventArgs^ e)
	{
		switch(e->ListChangedType)
		{
			case ::System::ComponentModel::ListChangedType::ItemAdded:
				ArticleHeadAdd(e->NewIndex);			
				break;

			case ::System::ComponentModel::ListChangedType::ItemDeleted:
				ArticleHeadRemove(e->NewIndex);
				break;

			case ::System::ComponentModel::ListChangedType::ItemChanged:
				ArticleHeadChange(e->NewIndex);
				break;
		}
	};

	
	void ControllerUI::NewsGroup::ArticleHeadAdd(int Index)
	{
		ArticleHead^ obj = OgreClient::Singleton->Data->NewsGroup->Articles[Index];

		CEGUI::ListboxTextItem* itmTitle = new CEGUI::ListboxTextItem(
			StringConvert::CLRToCEGUI(obj->Title));

		CEGUI::ListboxTextItem* itmAuthor = new CEGUI::ListboxTextItem(
			StringConvert::CLRToCEGUI(obj->Poster));
		
		CEGUI::ListboxTextItem* itmDate = new CEGUI::ListboxTextItem(
			StringConvert::CLRToCEGUI(obj->Time.ToShortDateString() + " " + obj->Time.ToShortTimeString()));

		itmTitle->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
		itmAuthor->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");
		itmDate->setSelectionBrushImage("TaharezLook/ListboxSelectionBrush");

		// insert widget in ui-list
		if ((int)List->getRowCount() > Index)
			List->insertRow(Index);
		
		// or add
		else
			List->addRow();

		List->setItem(itmTitle, 0, Index); 
		List->setItem(itmAuthor, 1, Index);
		List->setItem(itmDate, 2, Index);
	};

	void ControllerUI::NewsGroup::ArticleHeadRemove(int Index)
	{
		// check
		if ((int)List->getRowCount() > Index)		
			List->removeRow(Index);
	};

	void ControllerUI::NewsGroup::ArticleHeadChange(int Index)
	{
		ArticleHead^ obj = OgreClient::Singleton->Data->NewsGroup->Articles[Index];
	};

	bool UICallbacks::NewsGroup::OnSelectionChanged(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args = static_cast<const CEGUI::WindowEventArgs&>(e);
		CEGUI::MultiColumnList* list = ControllerUI::NewsGroup::List;
		CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
		
		if (itm != nullptr)
		{
			unsigned int index = list->getItemRowIndex(itm);

			OgreClient::Singleton->SendReqArticle(
				OgreClient::Singleton->Data->NewsGroup->Articles[index]->Number);		
		}

		return true;
	};
	
	bool UICallbacks::NewsGroup::OnCreateClicked(const CEGUI::EventArgs& e)
	{		
		// show empty compose window
		ControllerUI::NewsGroupCompose::HeadLine->setText(STRINGEMPTY);
		ControllerUI::NewsGroupCompose::Text->setText(STRINGEMPTY);
		ControllerUI::NewsGroupCompose::Window->setVisible(true);
		ControllerUI::NewsGroupCompose::Window->moveToFront();
	
		return true;
	}

	bool UICallbacks::NewsGroup::OnRespondClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		CEGUI::MultiColumnList* list = ControllerUI::NewsGroup::List;
		ArticleHeadList^ articles = OgreClient::Singleton->Data->NewsGroup->Articles;

		// need selection to be able to respond
		if (list->getSelectedCount() > 0)
		{
			// get selection
			CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
			unsigned int index = list->getItemRowIndex(itm);

			if (articles->Count > (int)index)
			{
				ArticleHead^ article = articles[index];

				::System::String^ newTitle = Common::Util::Truncate(
					"Re: " + article->Title, BlakservStringLengths::NEWS_POSTING_MAX_SUBJECT_LENGTH);
				
					// show prefilled compose window
				ControllerUI::NewsGroupCompose::HeadLine->setText(
					StringConvert::CLRToCEGUI(newTitle));
				
				ControllerUI::NewsGroupCompose::Text->setText(STRINGEMPTY);
				ControllerUI::NewsGroupCompose::Window->setVisible(true);
				ControllerUI::NewsGroupCompose::Window->moveToFront();
			}
		}

		return true;
	}

	bool UICallbacks::NewsGroup::OnMailAuthorClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;
		CEGUI::MultiColumnList* list = ControllerUI::NewsGroup::List;
		
		return true;
	}

	bool UICallbacks::NewsGroup::OnRefreshClicked(const CEGUI::EventArgs& e)
	{
		// re-request articles
		OgreClient::Singleton->SendReqArticles();

		return true;
	}

	bool UICallbacks::NewsGroup::OnDeleteClicked(const CEGUI::EventArgs& e)
	{
#if !VANILLA
		const CEGUI::WindowEventArgs& args = (const CEGUI::WindowEventArgs&)e;
		CEGUI::MultiColumnList* list = ControllerUI::NewsGroup::List;
		CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
		Models::NewsGroup^ news = OgreClient::Singleton->Data->NewsGroup;

		if (itm != nullptr && news != nullptr)
		{
			unsigned int index = list->getItemRowIndex(itm);

			if (news->Articles->Count > index)
			{
				// num of the article and id of currently viewed newsgroup
				unsigned int num = news->Articles[index]->Number;
				unsigned short id = news->NewsGlobeID;

				// try to delete
				OgreClient::Singleton->SendDeleteNews(id, num);

				// re-request articles
				OgreClient::Singleton->SendReqArticles();
			}			
		}	
#endif
		return true;
	}

	bool UICallbacks::NewsGroup::OnKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);
		CEGUI::MultiColumnList* list = ControllerUI::NewsGroup::List;		
		CEGUI::ListboxItem* itm = list->getFirstSelectedItem();
		int count = (int)list->getRowCount();

		if (itm != nullptr)
		{
			unsigned int index = list->getItemRowIndex(itm);

			if (args.scancode == CEGUI::Key::Scan::ArrowUp && index > 0)
			{
				// unselect
				list->setItemSelectState(CEGUI::MCLGridRef(index, 0), false);
				list->setItemSelectState(CEGUI::MCLGridRef(index, 1), false);
				list->setItemSelectState(CEGUI::MCLGridRef(index, 2), false);
				
				// select
				list->setItemSelectState(CEGUI::MCLGridRef(index-1, 0), true);
				list->setItemSelectState(CEGUI::MCLGridRef(index-1, 1), true);
				list->setItemSelectState(CEGUI::MCLGridRef(index-1, 2), true);			
			}
			else if (args.scancode == CEGUI::Key::Scan::ArrowDown && (int)index < count - 1)
			{
				// unselect
				list->setItemSelectState(CEGUI::MCLGridRef(index, 0), false);
				list->setItemSelectState(CEGUI::MCLGridRef(index, 1), false);
				list->setItemSelectState(CEGUI::MCLGridRef(index, 2), false);
				
				// select
				list->setItemSelectState(CEGUI::MCLGridRef(index+1, 0), true);
				list->setItemSelectState(CEGUI::MCLGridRef(index+1, 1), true);
				list->setItemSelectState(CEGUI::MCLGridRef(index+1, 2), true);	
			}
		}

		return true;
	};

	bool UICallbacks::NewsGroup::OnWindowClosed(const CEGUI::EventArgs& e)
	{
		// clear (view will react)
		OgreClient::Singleton->Data->NewsGroup->Clear(true);
		
		// mark GUIroot active
		ControllerUI::ActivateRoot();

		return true;
	}

	bool UICallbacks::NewsGroup::OnWindowKeyUp(const CEGUI::EventArgs& e)
	{
		const CEGUI::KeyEventArgs& args = static_cast<const CEGUI::KeyEventArgs&>(e);

		// close window on ESC
		if (args.scancode == CEGUI::Key::Escape)
		{
			// clear (view will react)
			OgreClient::Singleton->Data->NewsGroup->Clear(true);

			// mark GUIroot active
			ControllerUI::ActivateRoot();
		}

		return true;
	}
};};