
// Converts RAW heightmaps (N x N 4byte FLOAT) and Texture layers to OGT terrain

#include "stdafx.h"
#include "Ogre.h"

#include "OgreTerrain.h"
#include "OgreTerrainGroup.h"
#include "OgreTerrainMaterialGenerator.h"
#include "OgreTerrainMaterialGeneratorA.h"

#include "OgreSceneManager.h"
#include "OgreMemoryAllocatorConfig.h"
#include "OgreResourceGroupManager.h"
#include "OgrePrerequisites.h"
#include "OgreLogManager.h"
#include "OgreResourceBackgroundQueue.h"

int _tmain(int argc, _TCHAR* argv[])
{
	// ------------------------- Check for command line argument -------------------------------------------

	if (! argv[1])
	{
		printf("\n");
		printf("Missing argument.\nExample: \"Converter.exe job1.cfg\"");		
		return 0;	
	}

	// ------------------------- Basic Ogre Engine initialization -------------------------------------------

	Ogre::Root* root = new Ogre::Root;	
	Ogre::RenderSystem* rendersys = root->getRenderSystemByName("Direct3D9 Rendering Subsystem");
	
	rendersys->setConfigOption("Full Screen", "No");
	rendersys->setConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
	
	root->setRenderSystem(rendersys);		
	
	Ogre::ResourceGroupManager::getSingleton().addResourceLocation("resource", "FileSystem", "General");
	Ogre::ResourceGroupManager::getSingleton().initialiseAllResourceGroups();

	root->initialise(false);

	Ogre::RenderWindow* window = root->createRenderWindow("RAW2OGT", 800, 600, false);		
	Ogre::SceneManager* scenemgr = root->createSceneManager(Ogre::SceneType::ST_GENERIC);
	Ogre::Camera* camera = scenemgr->createCamera("camera");
	Ogre::Viewport* viewport = window->addViewport(camera);
	/*Ogre::Vector3 lightdir(0, -0.3, 0.75);
	lightdir.normalise();

	Ogre::Light* l = scenemgr->createLight("tstLight");
	l->setType(Ogre::Light::LT_DIRECTIONAL);
	l->setDirection(lightdir);
	l->setDiffuseColour(Ogre::ColourValue(1.0, 1.0, 1.0));
	l->setSpecularColour(Ogre::ColourValue(0.4, 0.4, 0.4));*/

	scenemgr->setAmbientLight(Ogre::ColourValue(0.7, 0.7, 0.7));
	
	// --------------------------------- Start convert ----------------------------------------------------

	// Load job config
	Ogre::ConfigFile* terrainconfig = OGRE_NEW Ogre::ConfigFile();
	terrainconfig->loadDirect(argv[1]);
		
	// Load info from [general] block
	Ogre::String heightmapfile = terrainconfig->getSetting("heightmap", "general");
	Ogre::Real heightmapscale = Ogre::StringConverter::parseReal(terrainconfig->getSetting("heightmapscale", "general"));
	Ogre::Real heightmapoffset = Ogre::StringConverter::parseReal(terrainconfig->getSetting("heightmapoffset", "general"));
	Ogre::uint16 terrainsize = Ogre::StringConverter::parseUnsignedInt(terrainconfig->getSetting("terrainsize", "general"));
	Ogre::Real worldsize = Ogre::StringConverter::parseReal(terrainconfig->getSetting("worldsize", "general"));
	Ogre::uint16 layercount = Ogre::StringConverter::parseUnsignedInt(terrainconfig->getSetting("layercount", "general"));

	// initialise stream to heightmapfile
	Ogre::DataStreamPtr stream = Ogre::ResourceGroupManager::getSingleton().openResource(heightmapfile, "General");
	size_t size = stream.get()->size();
		
	// verify size
	if(size != terrainsize * terrainsize * 4)
		OGRE_EXCEPT( Ogre::Exception::ERR_INTERNAL_ERROR, "Size of stream does not match terrainsize!", "TerrainPage" );
					
	// load to buffer
	float* buffer = OGRE_ALLOC_T(float, size, Ogre::MEMCATEGORY_GENERAL);
	stream->read(buffer, size);

	// apply scale and offset 
	for(int i=0;i<terrainsize*terrainsize;i++)
	{
		buffer[i]  = (buffer[i] + heightmapoffset) * heightmapscale;
	}

	// Terrain initialization
	Ogre::TerrainGlobalOptions* terrainglobals = OGRE_NEW Ogre::TerrainGlobalOptions();
	terrainglobals->setMaxPixelError(1);
	//terrainglobals->setCompositeMapDistance(30000);
	//terrainglobals->setLightMapDirection(lightdir);
    //terrainglobals->setCompositeMapAmbient(scenemgr->getAmbientLight());
    //terrainglobals->setCompositeMapDiffuse(l->getDiffuseColour());
	
	Ogre::TerrainMaterialGeneratorA::SM2Profile* pMatProfile = 
      static_cast<Ogre::TerrainMaterialGeneratorA::SM2Profile*>(terrainglobals->getDefaultMaterialGenerator()->getActiveProfile());
	
	pMatProfile->setLightmapEnabled(false);
	pMatProfile->setCompositeMapEnabled(false);
	
	Ogre::TerrainGroup* terraingroup = OGRE_NEW Ogre::TerrainGroup(scenemgr, Ogre::Terrain::ALIGN_X_Z, terrainsize, worldsize);
    terraingroup->setFilenameConvention(Ogre::String("terrain"), Ogre::String("ogt"));
    terraingroup->setOrigin(Ogre::Vector3::ZERO);
		
	Ogre::Terrain* terrain = OGRE_NEW Ogre::Terrain(scenemgr);

	// terrainsettings							
	Ogre::Terrain::ImportData& imp = terraingroup->getDefaultImportSettings();	
	imp.terrainSize = terrainsize;
	imp.worldSize = worldsize;
	imp.minBatchSize = 33;
	imp.maxBatchSize = 65;

	// use float RAW heightmap as input
	imp.inputFloat = buffer;
	
	// process texture layers
	imp.layerList.resize(layercount);
	Ogre::StringVector blendmaps(layercount);
	for(int i=0;i<layercount;i++)
	{
		// load layer info
		Ogre::String sectionStr = Ogre::StringConverter::toString(i);
		Ogre::Real layerworldsize = Ogre::StringConverter::parseReal(terrainconfig->getSetting("worldsize", sectionStr));
		
		if (i==0)
		{			
			// no blendmap at layer 0 (baselayer)
			Ogre::String specular = terrainconfig->getSetting("specular", sectionStr);
			Ogre::String normal = terrainconfig->getSetting("normal", sectionStr);

			// add layer		
			imp.layerList[i].textureNames.push_back(specular);
			imp.layerList[i].textureNames.push_back(normal);
			imp.layerList[i].worldSize = layerworldsize;			
		}
		else
		{
			Ogre::String specular = terrainconfig->getSetting("specular", sectionStr);
			Ogre::String normal = terrainconfig->getSetting("normal", sectionStr);
			Ogre::String blend = terrainconfig->getSetting("blend", sectionStr);
		
			// add layer		
			imp.layerList[i].textureNames.push_back(specular);
			imp.layerList[i].textureNames.push_back(normal);	
			imp.layerList[i].worldSize = layerworldsize;

			blendmaps[i] = blend; 
		}
	}

	// load the terrain
	terrain->prepare(imp);
	terrain->load();	
	
	// load those blendmaps into the layers
	for(int j = 1;j < terrain->getLayerCount();j++)
	{
		Ogre::TerrainLayerBlendMap *blendmap = terrain->getLayerBlendMap(j);
		Ogre::Image img;
		img.load(blendmaps[j],"General");
		int blendmapsize = terrain->getLayerBlendMapSize();
		if(img.getWidth() != blendmapsize)
			img.resize(blendmapsize, blendmapsize);

		float *ptr = blendmap->getBlendPointer();
		Ogre::uint8 *data = static_cast<Ogre::uint8*>(img.getPixelBox().data);

		for(int bp = 0;bp < blendmapsize * blendmapsize;bp++)
			ptr[bp] = static_cast<float>(data[bp]) / 255.0f;

		blendmap->dirty();
		blendmap->update();
	}

	// create filename for writing
	int pos = heightmapfile.find_last_of('.');
	if (pos < 0)	
		heightmapfile = heightmapfile + ".ogt";
	else	
		heightmapfile = heightmapfile.substr(0, pos) + ".ogt";
			
	// save as Ogre .OGT
	terrain->save(heightmapfile);	
	Ogre::LogManager::getSingletonPtr()->logMessage(Ogre::LogMessageLevel::LML_NORMAL, heightmapfile + " successfully written.");
	
	// debug viewing (exit with CTRL+C)
	camera->setPosition(-terrainsize, 7000, -terrainsize);
	camera->lookAt(terrainsize/2,0,terrainsize/2);	
	root->startRendering();

	return 0;
}

