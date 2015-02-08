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
	};

	void ControllerUI::LoadingBar::Destroy()
	{	     			
	};

	void ControllerUI::LoadingBar::Start(unsigned short numGroupsInit) 
    {          
		ResourceGroupManager* resMan = ResourceGroupManager::getSingletonPtr();
			
		// attach listener
		resMan->addResourceGroupListener(groupListener);   

		Group->setProgress(0.0f);
		Content->setProgress(0.0f);

		float div = (float)::System::Math::Max(numGroupsInit, (unsigned short)1);
		stepSizeGroup = 1.0f / div;
    };

	void ControllerUI::LoadingBar::Finish()
    {
		ResourceGroupManager* resMan = ResourceGroupManager::getSingletonPtr();
		
		// detach listener
		resMan->removeResourceGroupListener(groupListener);
    };
	
	void ControllerUI::LoadingBar::ResourceGroupLoadStarted(const String* groupName, size_t resourceCount)
    {
    };

	void ControllerUI::LoadingBar::ResourceGroupPrepareStarted(const String* groupName, size_t resourceCount)
    {
	};

	void ControllerUI::LoadingBar::ResourceGroupScriptingStarted(const String* groupName, size_t scriptCount)
    {        		
		if (!OgreClient::Singleton->RenderWindow->isClosed())
		{
			Group->setText(*groupName);
			Group->setProgress(Group->getProgress() + stepSizeGroup);
			
			Content->setProgress(0.0f);
			
			float div = (float)::System::Math::Max((unsigned short)scriptCount, (unsigned short)1);
			stepSizeContent = 1.0f / div;
			
			// render a frame to update (not yet in apploop)
			OgreClient::Singleton->Root->renderOneFrame();
		}
    };

	void ControllerUI::LoadingBar::ScriptParseStarted(const String* scriptName, bool &skipThisScript)
    {
        if (!OgreClient::Singleton->RenderWindow->isClosed())
		{		
			Content->setText(*scriptName);
			Content->setProgress(Content->getProgress() + stepSizeContent);
			
			// render a frame to update (not yet in apploop)
			OgreClient::Singleton->Root->renderOneFrame();
		}
    };

	void ControllerUI::LoadingBar::ScriptParseEnded(const String* scriptName, bool skipped)
    {
    };

	void ControllerUI::LoadingBar::ResourceLoadStarted(ResourcePtr resource)
    {
    };

	void ControllerUI::LoadingBar::WorldGeometryStageStarted(const String* description)
    {
    };

	void ControllerUI::LoadingBar::WorldGeometryStageEnded()
    {
    };
};};