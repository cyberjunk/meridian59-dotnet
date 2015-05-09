#include "stdafx.h"

namespace Meridian59 { namespace Ogre
{	
	WeatherParticleEventHandler::WeatherParticleEventHandler(void)
		: ::ParticleUniverse::DoExpireEventHandler()
	{
	};

	void WeatherParticleEventHandler::_handle(::ParticleUniverse::ParticleTechnique* particleTechnique, ::ParticleUniverse::Particle* particle, Real timeElapsed)
	{
		int xint, yint;
		float floorheight;
		::Meridian59::Files::ROO::RooFile^ rooFile;
		::Meridian59::Files::ROO::RooSubSector^ subSector;

		// get roomfile
		rooFile = OgreClient::Singleton->CurrentRoom;

		// no room? set TTL to 0
		if (!rooFile)
		{
			particle->timeToLive = 0.0f;
			return;
		}
		
		// convert to roo coordinates
		xint = (int)((particle->position.x - 64.0f) * 16.0f);
		yint = (int)((particle->position.z - 64.0f) * 16.0f);

		// retrieve the floorheight and the subsector(leaf) for the roo coordinates	
		floorheight = (float)rooFile->GetHeightAt((float)xint, (float)yint, subSector, true, false) * 0.0625f;

		// no subsector/sector
		if (!subSector || !subSector->Sector)
		{
			particle->timeToLive = 0.0f;
			return;
		}

		// kill particles if there is a ceiling and they're below
		if (subSector->Sector->CeilingTexture > 0 && 
			(float)subSector->Sector->CeilingHeight >= particle->position.y - 6.0f)
		{
			particle->timeToLive = 0.0f;
			return;
		}

		// kill particle if it's below a floor
		if (floorheight >= particle->position.y)
		{
			particle->timeToLive = 0.0f;
			return;
		}
	};

/////////////////////////////////////////////////////////////////////////////////////////////////////
};};