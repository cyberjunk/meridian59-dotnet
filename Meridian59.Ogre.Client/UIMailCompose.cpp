#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::MailCompose::Initialize()
   {
      // setup references to children from xml nodes
      Window         = static_cast<CEGUI::FrameWindow*>(guiRoot->getChild(UI_NAME_MAILCOMPOSE_WINDOW));
      Error	         = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAILCOMPOSE_ERROR));
      RecipientsDesc = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAILCOMPOSE_GROUPDESC));
      Recipients     = static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_MAILCOMPOSE_GROUP));
      HeadLineDesc   = static_cast<CEGUI::Window*>(Window->getChild(UI_NAME_MAILCOMPOSE_HEADLINEDESC));
      HeadLine       = static_cast<CEGUI::Editbox*>(Window->getChild(UI_NAME_MAILCOMPOSE_HEADLINE));		
      Text           = static_cast<CEGUI::MultiLineEditbox*>(Window->getChild(UI_NAME_MAILCOMPOSE_TEXT));
      Send           = static_cast<CEGUI::PushButton*>(Window->getChild(UI_NAME_MAILCOMPOSE_SEND));

      // how much of maximum message length we use for title and body
      // they get combined with an additional 'subject: ' string chunk when being sent in BaseClient.cs
      // the max applies to the overall mail
      const size_t OTHCHARS = 10;
      const size_t MAXTITLE = 60;
      const size_t MAXBODY  = BlakservStringLengths::MAIL_MESSAGE_MAX_LENGTH - MAXTITLE - OTHCHARS - 1;

      // set maximum length for title and body
      HeadLine->setMaxTextLength(MAXTITLE);
      Text->setMaxTextLength(MAXBODY);

      // subscribe send button
      Send->subscribeEvent(CEGUI::PushButton::EventClicked, CEGUI::Event::Subscriber(UICallbacks::MailCompose::OnSendClicked));

      // subscribe keydown on headline box, recipients and text
      Recipients->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
      HeadLine->subscribeEvent(CEGUI::Editbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));
      Text->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyDown, CEGUI::Event::Subscriber(UICallbacks::OnCopyPasteKeyDown));

      // block chatpopup on return
      Recipients->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUpBlock));
      HeadLine->subscribeEvent(CEGUI::Editbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUpBlock));
      Text->subscribeEvent(CEGUI::MultiLineEditbox::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUpBlock));

      // subscribe close button
      Window->subscribeEvent(CEGUI::FrameWindow::EventCloseClicked, CEGUI::Event::Subscriber(UICallbacks::OnWindowClosed));

      // subscribe keyup
      Window->subscribeEvent(CEGUI::FrameWindow::EventKeyUp, CEGUI::Event::Subscriber(UICallbacks::OnKeyUp));
   };

   void ControllerUI::MailCompose::Destroy()
   {
   };

   void ControllerUI::MailCompose::ApplyLanguage()
   {
   };

   void ControllerUI::MailCompose::ProcessResult(array<ObjectID^>^ Result)
   {
      // check
      if (LastLookupNames == nullptr || Result == nullptr || LastLookupNames->Length != Result->Length)
         return;

      // build not found string
      CLRString^ notfound = CLRString::Empty;

      // look for not found name
      for(int i = 0; i < Result->Length; i++)
      {
         // no id found for this one
         if (Result[i]->ID == 0)
         {
            // add comma if not first
            if (!CLRString::Equals(notfound, CLRString::Empty))
               notfound += ",";

            notfound += LastLookupNames[i];
         }
      }

      // all found?
      if (CLRString::Equals(notfound, CLRString::Empty))
      {
         // hide error text
         Error->setVisible(false);

         // send mail
         OgreClient::Singleton->SendSendMail(
            Result,
            StringConvert::CEGUIToCLR(HeadLine->getText()),
            StringConvert::CEGUIToCLR(Text->getText()));

         // clean text
         Recipients->setText(STRINGEMPTY);
         HeadLine->setText(STRINGEMPTY);
         Text->setText(STRINGEMPTY);

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

   //////////////////////////////////////////////////////////////////////////////////////////////////////////
   //////////////////////////////////////////////////////////////////////////////////////////////////////////

   bool UICallbacks::MailCompose::OnSendClicked(const CEGUI::EventArgs& e)
   {
      const CEGUI::WindowEventArgs& args	= (const CEGUI::WindowEventArgs&)e;		

      // get recipients string
      CLRString^ recipients = StringConvert::CEGUIToCLR(
         ControllerUI::MailCompose::Recipients->getText());

      // check
      if (recipients != nullptr && !CLRString::Equals(recipients, CLRString::Empty))
      {
         // split up into single names by ','
         array<CLRString^>^ splitted  = recipients->Split(',');

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
