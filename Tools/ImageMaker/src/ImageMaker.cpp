#include "stdafx.h"
#include "Ogre.h"
#include "OgreSceneManager.h"
#include "OgreMemoryAllocatorConfig.h"
#include "OgreResourceGroupManager.h"
#include "OgrePrerequisites.h"
#include "OgreLogManager.h"

int _tmain(int argc, _TCHAR* argv[])
{
	// ------------------------- Check for command line argument -------------------------------------------

	if (! argv[1] || ! argv[2] || ! argv[3])
	{
		printf("\n");
		printf("Missing argument.\nExample: \"ImageMaker.exe texture1.png texture2.png output.png\"");		
		return 0;	
	}

	Ogre::String input1 = argv[1];
	Ogre::String input2 = argv[2];
	Ogre::String output = argv[3];

	// ------------------------- Basic Ogre Engine initialization -------------------------------------------

	Ogre::Root* root = new Ogre::Root;	
	Ogre::RenderSystem* rendersys = root->getRenderSystemByName("Direct3D9 Rendering Subsystem");
	
	rendersys->setConfigOption("Full Screen", "No");
	rendersys->setConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
	
	root->setRenderSystem(rendersys);		
	
	Ogre::ResourceGroupManager::getSingleton().addResourceLocation("resource", "FileSystem", "General");
	Ogre::ResourceGroupManager::getSingleton().initialiseAllResourceGroups();

	root->initialise(false);
		
	// --------------------------------- Start combine ----------------------------------------------------

	Ogre::Image* combined = OGRE_NEW Ogre::Image();
		
	combined->loadTwoImagesAsRGBA(input1, input2,
      Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME, Ogre::PF_BYTE_RGBA);

	combined->save(output);

	return 0;
}

