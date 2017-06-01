#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{
   void ControllerUI::LoadingBar::Initialize()
   {
      // setup references to children from xml nodes
      Window	= static_cast<CEGUI::Window*>(guiRoot->getChild(UI_NAME_LOADINGBAR_WINDOW));
      Group	= static_cast<CEGUI::ProgressBar*>(Window->getChild(UI_NAME_LOADINGBAR_GROUP));
      Content	= static_cast<CEGUI::ProgressBar*>(Window->getChild(UI_NAME_LOADINGBAR_CONTENT));

      groupListener = new LoadingBarResourceGroupListener();

      // attach listeners to legacy resourcemanager
      OgreClient::Singleton->ResourceManager->PreloadingStarted +=
         gcnew ::System::EventHandler(OnPreloadingGroupStarted);

      OgreClient::Singleton->ResourceManager->PreloadingEnded +=
         gcnew ::System::EventHandler(OnPreloadingGroupEnded);

      OgreClient::Singleton->ResourceManager->PreloadingFile +=
         gcnew ::System::EventHandler<StringEventArgs^>(OnPreloadingFile);
   };

   void ControllerUI::LoadingBar::Destroy()
   {
      // detach listeners from legacy resourcemanager
      OgreClient::Singleton->ResourceManager->PreloadingStarted -=
         gcnew ::System::EventHandler(OnPreloadingGroupStarted);

      OgreClient::Singleton->ResourceManager->PreloadingEnded -=
         gcnew ::System::EventHandler(OnPreloadingGroupEnded);

      OgreClient::Singleton->ResourceManager->PreloadingFile -=
         gcnew ::System::EventHandler<StringEventArgs^>(OnPreloadingFile);
   };

   void ControllerUI::LoadingBar::ApplyLanguage()
   {
   };

   void ControllerUI::LoadingBar::Start(unsigned short numGroupsInit) 
   {
      ResourceGroupManager& resMan = ResourceGroupManager::getSingleton();

      // attach listener
      resMan.addResourceGroupListener(groupListener);

      Group->setProgress(0.0f);
      Content->setProgress(0.0f);

      float div = (float)::System::Math::Max(numGroupsInit, (unsigned short)1);
      stepSizeGroup = 1.0f / div;
   };

   void ControllerUI::LoadingBar::Finish()
   {
      ResourceGroupManager& resMan = ResourceGroupManager::getSingleton();

      // detach listener
      resMan.removeResourceGroupListener(groupListener);
   };

   void ControllerUI::LoadingBar::ResourceGroupLoadStarted(const String* groupName, size_t resourceCount)
   {
      if (!OgreClient::Singleton->RenderWindow->isClosed())
      {
         Group->setText(*groupName);
         Group->setProgress(Group->getProgress() + stepSizeGroup);

         Content->setProgress(0.0f);

         float div = (float)::System::Math::Max((unsigned short)resourceCount, (unsigned short)1);
         stepSizeContent = 1.0f / div;

         OgreClient::Singleton->RenderManually();
      }
   };

   void ControllerUI::LoadingBar::ResourceGroupPrepareStarted(const String* groupName, size_t resourceCount)
   {
   };

   void ControllerUI::LoadingBar::ResourceGroupScriptingStarted(const String* groupName, size_t scriptCount)
   {
   };

   void ControllerUI::LoadingBar::ScriptParseStarted(const String* scriptName, bool &skipThisScript)
   {
      if (!OgreClient::Singleton->RenderWindow->isClosed())
      {
         Content->setText(*scriptName);
         Content->setProgress(Content->getProgress() + stepSizeContent);
         
         OgreClient::Singleton->RenderManually();
      }
   };

   void ControllerUI::LoadingBar::ScriptParseEnded(const String* scriptName, bool skipped)
   {
   };

   void ControllerUI::LoadingBar::ResourceLoadStarted(ResourcePtr resource)
   {
      if (!OgreClient::Singleton->RenderWindow->isClosed())
      {
         Content->setText(resource->getName());
         Content->setProgress(Content->getProgress() + stepSizeContent);

         OgreClient::Singleton->RenderManually();
      }
   };

   void ControllerUI::LoadingBar::WorldGeometryStageStarted(const String* description)
   {
   };

   void ControllerUI::LoadingBar::WorldGeometryStageEnded()
   {
   };

   void ControllerUI::LoadingBar::OnPreloadingGroupStarted(Object^ sender, ::System::EventArgs^ e)
   {
      if (!OgreClient::Singleton->RenderWindow->isClosed())
      {
         Group->setText(StringConvert::CLRToCEGUI("legacy resources"));
         Group->setProgress(Group->getProgress() + stepSizeGroup);

         Content->setProgress(0.0f);

         int items = 0;

         if (OgreClient::Singleton->Config->PreloadObjects)
            items += OgreClient::Singleton->ResourceManager->Objects->Count;

         if (OgreClient::Singleton->Config->PreloadRoomTextures)
            items += OgreClient::Singleton->ResourceManager->RoomTextures->Count;

         if (OgreClient::Singleton->Config->PreloadRooms)
            items += OgreClient::Singleton->ResourceManager->Rooms->Count;

         if (OgreClient::Singleton->Config->PreloadSound)
            items += OgreClient::Singleton->ResourceManager->Wavs->Count;

         if (OgreClient::Singleton->Config->PreloadMusic)
            items += OgreClient::Singleton->ResourceManager->Music->Count;

         float div = (float)::System::Math::Max(items, 1);
         stepSizeContent = 1.0f / div;

         OgreClient::Singleton->RenderManually();
      }
   };

   void ControllerUI::LoadingBar::OnPreloadingGroupEnded(Object^ sender, ::System::EventArgs^ e)
   {
   };

   void ControllerUI::LoadingBar::OnPreloadingFile(Object^ sender, StringEventArgs^ e)
   {
      if (!OgreClient::Singleton->RenderWindow->isClosed())
      {
         Content->setText(StringConvert::CLRToCEGUI(e->Value));
         Content->setProgress(Content->getProgress() + stepSizeContent);

         OgreClient::Singleton->RenderManually();
      }
   };
};};
