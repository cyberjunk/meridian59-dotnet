#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
	void ControllerUI::DownloadBar::Initialize()
	{
		// setup references to children from xml nodes
		Window	= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_DOWNLOADBAR_WINDOW));
      Content = static_cast<CEGUI::ProgressBar*>(Window->getChild(UI_NAME_DOWNLOADBAR_CONTENT));

		// Attach listeners to download handler
      OgreClient::Singleton->DownloadHandler->DownloadStarted +=
         gcnew ::System::EventHandler(OnDownloadStarted);

      OgreClient::Singleton->DownloadHandler->DownloadFinished +=
         gcnew ::System::EventHandler<StringEventArgs^>(OnDownloadFinished);

      OgreClient::Singleton->DownloadHandler->DownloadProgress +=
         gcnew ::System::EventHandler<IntegerEventArgs^>(OnDownloadProgress);

      OgreClient::Singleton->DownloadHandler->DownloadText +=
         gcnew ::System::EventHandler<StringEventArgs^>(OnDownloadFile);

	};

   void ControllerUI::DownloadBar::Destroy()
	{	
		// detach listeners from download handler
      OgreClient::Singleton->DownloadHandler->DownloadStarted -=
         gcnew ::System::EventHandler(OnDownloadStarted);

      OgreClient::Singleton->DownloadHandler->DownloadFinished -=
         gcnew ::System::EventHandler<StringEventArgs^>(OnDownloadFinished);

      OgreClient::Singleton->DownloadHandler->DownloadProgress -=
         gcnew ::System::EventHandler<IntegerEventArgs^>(OnDownloadProgress);

      OgreClient::Singleton->DownloadHandler->DownloadText -=
         gcnew ::System::EventHandler<StringEventArgs^>(OnDownloadFile);
	};

   void ControllerUI::DownloadBar::ApplyLanguage()
   {
   };

   void ControllerUI::DownloadBar::Start()
   {
      Content->setProgress(0.0f);
   };

   void ControllerUI::DownloadBar::Finish()
    {
    };

   void ControllerUI::DownloadBar::OnDownloadStarted(Object^ sender, ::System::EventArgs^ e)
	{
		if (!OgreClient::Singleton->RenderWindow->isClosed())
			Content->setProgress(0.0f);
	};

   void ControllerUI::DownloadBar::OnDownloadFinished(Object^ sender, StringEventArgs^ e)
	{
      if (!OgreClient::Singleton->RenderWindow->isClosed())
         Content->setText(StringConvert::CLRToCEGUI(e->Value));
      Finish();
	};

   void ControllerUI::DownloadBar::OnDownloadFile(Object^ sender, StringEventArgs^ e)
	{
		if (!OgreClient::Singleton->RenderWindow->isClosed())
			Content->setText(StringConvert::CLRToCEGUI(e->Value));
	};

   void ControllerUI::DownloadBar::OnDownloadProgress(Object^ sender, IntegerEventArgs^ e)
   {
      if (!OgreClient::Singleton->RenderWindow->isClosed())
         Content->setProgress((float)e->Value / 100);
   };
};};
