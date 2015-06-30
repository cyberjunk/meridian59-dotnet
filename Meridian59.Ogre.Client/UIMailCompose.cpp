#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::MailCompose::Initialize()
	{
		// setup references to children from xml nodes
		Window			= static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_MAILCOMPOSE_WINDOW));
		Error			= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAILCOMPOSE_ERROR));
		RecipientsDesc	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAILCOMPOSE_GROUPDESC));
		Recipients		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_MAILCOMPOSE_GROUP));
		HeadLineDesc	= static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAILCOMPOSE_HEADLINEDESC));
		HeadLine		= static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_MAILCOMPOSE_HEADLINE));		
		Text			= static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_MAILCOMPOSE_TEXT));
		Send			= static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAILCOMPOSE_SEND));
		
		// set maximum length for title and body (check with server values...)
		HeadLine->setMaxTextLength(UI_MAIL_MAXTITLELENGTH);
		Text->setMaxTextLength(UI_MAIL_MAXBODYLENGTH);

		// subscribe send button
		Send->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::MailCompose::OnSendClicked));
		
		// subscribe keydown on headline box, recipients and text
		Recipients->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		HeadLine->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		Text->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
		
		// subscribe close button
		Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

		// subscribe keyup
		Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
	};

	void ControllerUI::MailCompose::Destroy()
	{	 
	};
	
	void ControllerUI::MailCompose::ProcessResult(array<ObjectID^>^ Result)
	{
		// check
		if (LastLookupNames == nullptr || Result == nullptr || LastLookupNames->Length != Result->Length)
			return;

		// build not found string
		::System::String^ notfound = ::System::String::Empty;

		// look for not found name
		for(int i = 0; i < Result->Length; i++)
		{
			// no id found for this one
			if (Result[i]->ID == 0)
			{
				// add comma if not first
				if (!::System::String::Equals(notfound, ::System::String::Empty))
					notfound += ",";

				notfound += LastLookupNames[i];
			}
		}

		// all found?
		if (::System::String::Equals(notfound, ::System::String::Empty))
		{
			// hide error text
			Error->setVisible(false);

			// send mail
			OgreClient::Singleton->SendSendMail(
				Result,
				StringConvert::CEGUIToCLR(HeadLine->getText()),
				StringConvert::CEGUIToCLR(Text->getText()));

			// hide window
			Window->hide();
		}

		// not found?
		else
		{
			// show error
			Error->setText("Unknown recipients: " + StringConvert::CLRToCEGUI(notfound));
			Error->setVisible(true);
		}
	};

	bool UICallbacks::MailCompose::OnSendClicked(const CEGUI::EventArgs& e)
	{
		const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;		

		// get recipients string
		::System::String^ recipients = StringConvert::CEGUIToCLR(
			ControllerUI::MailCompose::Recipients->getText());

		// check
		if (recipients != nullptr && !::System::String::Equals(recipients, ::System::String::Empty))
		{			
			// split up into single names by ','
			array<::System::String^>^ splitted  = recipients->Split(',');

			// trim them
			for (int i = 0; i < splitted->Length; i++)
				splitted[i] = splitted[i]->Trim();
			
			// save it
			ControllerUI::MailCompose::LastLookupNames = splitted;

			// request IDs of these names
			OgreClient::Singleton->SendReqLookupNames(ControllerUI::MailCompose::LastLookupNames);
		}

		return true;
	};
};};