#include "stdafx.h"

namespace Meridian59 { namespace Ogre {

   DataControllerOgre::DataControllerOgre()
      : DataController()
   {
   };

   void DataControllerOgre::HandleRoomContents(RoomContentsMessage^ Message)
   {
      double tick1 = OgreClient::Singleton->GameTick->GetUpdatedTick();
      DataController::HandleRoomContents(Message);
      double tick2 = OgreClient::Singleton->GameTick->GetUpdatedTick();
      double span = tick2 - tick1;

      Logger::Log("DataControllerOgre", LogType::Info, "BP_ROOM_CONTENTS: " + span.ToString() + " ms");
   };

   void DataControllerOgre::HandleSpells(SpellsMessage^ Message)
   {
      double tick1 = OgreClient::Singleton->GameTick->GetUpdatedTick();
      DataController::HandleSpells(Message);
      double tick2 = OgreClient::Singleton->GameTick->GetUpdatedTick();
      double span = tick2 - tick1;

      Logger::Log("DataControllerOgre", LogType::Info, "BP_SPELLS: " + span.ToString() + " ms");
   };

   void DataControllerOgre::HandleInventory(InventoryMessage^ Message)
   {
      double tick1 = OgreClient::Singleton->GameTick->GetUpdatedTick();
      DataController::HandleInventory(Message);
      double tick2 = OgreClient::Singleton->GameTick->GetUpdatedTick();
      double span = tick2 - tick1;

      Logger::Log("DataControllerOgre", LogType::Info, "BP_INVENTORY: " + span.ToString() + " ms");
   };
};};