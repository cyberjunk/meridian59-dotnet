#include "stdafx.h"

#define PARSE_BOOL_ATTRIB(node, attribname, outval) \
   (node && node->Attributes[attribname] && ::System::Boolean::TryParse(node->Attributes[attribname]->Value, outval))

#define PARSE_UINT32_ATTRIB(node, attribname, outval) \
   (node && node->Attributes[attribname] && ::System::UInt32::TryParse(node->Attributes[attribname]->Value, outval))

#define PARSE_INT32_ATTRIB(node, attribname, outval) \
   (node && node->Attributes[attribname] && ::System::Int32::TryParse(node->Attributes[attribname]->Value, outval))

#define PARSE_UINT16_ATTRIB(node, attribname, outval) \
   (node && node->Attributes[attribname] && ::System::UInt16::TryParse(node->Attributes[attribname]->Value, outval))

#define PARSE_INT16_ATTRIB(node, attribname, outval) \
   (node && node->Attributes[attribname] && ::System::Int16::TryParse(node->Attributes[attribname]->Value, outval))

#define PARSE_FLOAT_ATTRIB(node, attribname, outval) \
   (node && node->Attributes[attribname] && ::System::Single::TryParse(node->Attributes[attribname]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, outval))

namespace Meridian59 { namespace Ogre
{
   OgreClientConfig::OgreClientConfig() : Config()
   {
      UILayoutAvatar = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.0f, 2.0f), ::CEGUI::UDim(0.0f, 25.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 250.0f), ::CEGUI::UDim(0.0f, 141.0f)));

      UILayoutTarget = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.0f, 254.0f), ::CEGUI::UDim(0.0f, 25.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 250.0f), ::CEGUI::UDim(0.0f, 90.0f)));

      UILayoutRoomEnchantments = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.0f, 2.0f), ::CEGUI::UDim(0.0f, 168.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 250.0f), ::CEGUI::UDim(0.0f, 42.0f)));

      UILayoutMainButtonsLeft = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -174.0f), ::CEGUI::UDim(1.0f, -32.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 175.0f), ::CEGUI::UDim(0.0f, 30.0f)));

      UILayoutMainButtonsRight = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, 0.0f), ::CEGUI::UDim(1.0f, -32.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 175.0f), ::CEGUI::UDim(0.0f, 30.0f)));

      UILayoutActionButtons = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -174.0f), ::CEGUI::UDim(1.0f, -174.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 349.0f), ::CEGUI::UDim(0.0f, 146.0f)));

      UILayoutChat = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.0f, 2.0f), ::CEGUI::UDim(1.0f, -186.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 463.0f), ::CEGUI::UDim(0.0f, 184.0f)));

      UILayoutInventory = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(1.0f, -286.0f), ::CEGUI::UDim(1.0f, -186.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 284.0f), ::CEGUI::UDim(0.0f, 184.0f)));

      UILayoutMinimap = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(1.0f, -258.0f), ::CEGUI::UDim(0.0f, 25.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 256.0f), ::CEGUI::UDim(0.0f, 256.0f)));

      UILayoutOnlinePlayers = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -96.0f), ::CEGUI::UDim(0.3f, -24.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 192.0f), ::CEGUI::UDim(0.0f, 192.0f)));

      UILayoutSpells = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -96.0f), ::CEGUI::UDim(0.3f, 0.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 192.0f), ::CEGUI::UDim(0.0f, 192.0f)));

      UILayoutSkills = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -96.0f), ::CEGUI::UDim(0.3f, 24.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 192.0f), ::CEGUI::UDim(0.0f, 192.0f)));

      UILayoutActions = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -96.0f), ::CEGUI::UDim(0.3f, 48.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 192.0f), ::CEGUI::UDim(0.0f, 192.0f)));

      UILayoutRoomObjects = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -96.0f), ::CEGUI::UDim(0.3f, 72.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 192.0f), ::CEGUI::UDim(0.0f, 192.0f)));

      UILayoutAttributes = new ::CEGUI::URect(
         ::CEGUI::Vector2<::CEGUI::UDim>(::CEGUI::UDim(0.5f, -144.0f), ::CEGUI::UDim(0.3f, 96.0f)),
         ::CEGUI::Size   <::CEGUI::UDim>(::CEGUI::UDim(0.0f, 288.0f), ::CEGUI::UDim(0.0f, 192.0f)));
   };

   void OgreClientConfig::ReadUILayout(::CEGUI::URect* Layout, XmlNode^ Node, bool DoSize)
   {
      if (!Layout || !Node)
         return;

      // must have all defined or we keep default
      if (!Node->Attributes[XMLATTRIB_XREL] ||
         !Node->Attributes[XMLATTRIB_XABS] ||
         !Node->Attributes[XMLATTRIB_YREL] ||
         !Node->Attributes[XMLATTRIB_YABS] ||
         (!Node->Attributes[XMLATTRIB_WREL] && DoSize) ||
         (!Node->Attributes[XMLATTRIB_WABS] && DoSize) ||
         (!Node->Attributes[XMLATTRIB_HREL] && DoSize) ||
         (!Node->Attributes[XMLATTRIB_HABS] && DoSize))
         return;

      float xrel, xabs, yrel, yabs;

      // position
      // must all be parsable or we keep default
      if (!::System::Single::TryParse(Node->Attributes[XMLATTRIB_XREL]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, xrel) ||
         !::System::Single::TryParse(Node->Attributes[XMLATTRIB_XABS]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, xabs) ||
         !::System::Single::TryParse(Node->Attributes[XMLATTRIB_YREL]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, yrel) ||
         !::System::Single::TryParse(Node->Attributes[XMLATTRIB_YABS]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, yabs))
         return;

      // set position
      Layout->setPosition(::CEGUI::Vector2<::CEGUI::UDim>(
         ::CEGUI::UDim(xrel, xabs),
         ::CEGUI::UDim(yrel, yabs)));

      // size
      if (DoSize)
      {
         float wrel, wabs, hrel, habs;

         if (!::System::Single::TryParse(Node->Attributes[XMLATTRIB_WREL]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, wrel) ||
            !::System::Single::TryParse(Node->Attributes[XMLATTRIB_WABS]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, wabs) ||
            !::System::Single::TryParse(Node->Attributes[XMLATTRIB_HREL]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, hrel) ||
            !::System::Single::TryParse(Node->Attributes[XMLATTRIB_HABS]->Value, ::System::Globalization::NumberStyles::Float, Config::NumberFormatInfo, habs))
            return;

         // set size
         Layout->setSize(::CEGUI::Size<::CEGUI::UDim>(
            ::CEGUI::UDim(wrel, wabs),
            ::CEGUI::UDim(hrel, habs)));
      }
   };

   void OgreClientConfig::ReadXml(::System::Xml::XmlDocument^ Document)
   {
      Config::ReadXml(Document);

      XmlNode^     node;
      int	       val_int;
      bool         val_bool;
      float        val_float;
      unsigned int val_uint;

      /******************************************************************************/
      /*  1) Engine
      /******************************************************************************/

      // Display
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DISPLAY);
      Display = (PARSE_INT32_ATTRIB(node, XMLATTRIB_VALUE, val_int)) ? val_int : DEFAULTVAL_ENGINE_DISPLAY;

      // Resolution
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_RESOLUTION);
      Resolution = (node && node->Attributes[XMLATTRIB_VALUE]) ? node->Attributes[XMLATTRIB_VALUE]->Value : DEFAULTVAL_ENGINE_RESOLUTION;

      // WindowMode (force windowmode until fullscreen works reliable)
      //node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_WINDOWMODE);		
      //WindowMode = (PARSE_BOOL_ATTRIB(node, ATTRIB_ENABLED, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_WINDOWMODE;
      WindowMode = true; 

      // WindowFrame
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_WINDOWFRAME);
      WindowFrame = (PARSE_BOOL_ATTRIB(node, ATTRIB_ENABLED, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_WINDOWFRAME;

      // VSync
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_VSYNC);
      VSync = (PARSE_BOOL_ATTRIB(node, ATTRIB_ENABLED, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_VSYNC;

      // FSAA
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_FSAA);
      FSAA = (node && node->Attributes[XMLATTRIB_VALUE]) ? node->Attributes[XMLATTRIB_VALUE]->Value : DEFAULTVAL_ENGINE_FSAA;

      // NoMipmaps
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_NOMIPMAPS);
      NoMipmaps = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_NOMIPMAPS;

      // TextureFiltering
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_TEXTUREFILTERING);
      TextureFiltering = (node && node->Attributes[XMLATTRIB_VALUE]) ? node->Attributes[XMLATTRIB_VALUE]->Value : DEFAULTVAL_ENGINE_TEXFILTERING;

      // ImageBuilder
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_IMAGEBUILDER);
      ImageBuilder = (node && node->Attributes[XMLATTRIB_VALUE]) ? node->Attributes[XMLATTRIB_VALUE]->Value : DEFAULTVAL_ENGINE_IMAGEBUILDER;

      // BitmapScaling
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_BITMAPSCALING);
      BitmapScaling = (node && node->Attributes[XMLATTRIB_VALUE]) ? node->Attributes[XMLATTRIB_VALUE]->Value : DEFAULTVAL_ENGINE_BITMAPSCALING;

      // TextureQuality
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_TEXTUREQUALITY);
      TextureQuality = (node && node->Attributes[XMLATTRIB_VALUE]) ? node->Attributes[XMLATTRIB_VALUE]->Value : DEFAULTVAL_ENGINE_TEXQUALITY;

      // DecorationIntensity
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DECORATIONINTENSITY);
      DecorationIntensity = (PARSE_INT32_ATTRIB(node, XMLATTRIB_VALUE, val_int)) ? val_int : DEFAULTVAL_ENGINE_DECORATIONINTENSITY;

      // DisableNewRoomTextures
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DISABLENEWROOMTEXTURES);
      DisableNewRoomTextures = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_DISABLENEWROOMTEX;

      // Disable3DModels
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DISABLE3DMODELS);
      Disable3DModels = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_DISABLE3DMODELS;

      // DisableNewSky
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DISABLENEWSKY);
      DisableNewSky = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_DISABLENEWSKY;

      // DisableWeatherEffects
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DISABLEWEATHEREFFECTS);
      DisableWeatherEffects = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_DISABLEWEATHEREFFECTS;

      // BrightnessFactor
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_BRIGHTNESSFACTOR);
      BrightnessFactor = (PARSE_FLOAT_ATTRIB(node, XMLATTRIB_VALUE, val_float)) ? val_float : DEFAULTVAL_ENGINE_BRIGHTNESSFACTOR;

      // WeatherParticles
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_WEATHERPARTICLES);
      WeatherParticles = (PARSE_INT32_ATTRIB(node, XMLATTRIB_VALUE, val_int)) ? val_int : DEFAULTVAL_ENGINE_WEATHERPARTICLES;

      // MusicVolume
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_MUSICVOLUME);
      MusicVolume = (PARSE_FLOAT_ATTRIB(node, XMLATTRIB_VALUE, val_float)) ? val_float : DEFAULTVAL_ENGINE_MUSICVOLUME;

      // SoundVolume
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_SOUNDVOLUME);
      SoundVolume = (PARSE_FLOAT_ATTRIB(node, XMLATTRIB_VALUE, val_float)) ? val_float : DEFAULTVAL_ENGINE_SOUNDVOLUME;

      // DisableLoopSounds
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_ENGINE + "/" + TAG_DISABLELOOPSOUNDS);
      DisableLoopSounds = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_ENGINE_DISABLELOOPSOUNDS;

      /******************************************************************************/
      /*  2) UI
      /******************************************************************************/

      // locked state
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT);
      UILocked = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_LOCKED, val_bool)) ? val_bool : DEFAULTVAL_UI_LOCKED;

      // avatar
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_AVATAR);
      ReadUILayout(UILayoutAvatar, node, false);

      // target
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_TARGET);
      ReadUILayout(UILayoutTarget, node, false);

      // target visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_TARGETVISIBILITY);
      UIVisibilityTarget = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYTARGET;

      // minimap
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_MINIMAP);
      ReadUILayout(UILayoutMinimap, node, true);

      // minimap visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_MINIMAPVISIBILITY);
      UIVisibilityMiniMap = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYMINIMAP;

      // roomenchantments
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ROOMENCHANTMENTS);
      ReadUILayout(UILayoutRoomEnchantments, node, false);

      // chat
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_CHAT);
      ReadUILayout(UILayoutChat, node, true);

      // chat visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_CHATVISIBILITY);
      UIVisibilityChat = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYCHAT;

      // inventory
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_INVENTORY);
      ReadUILayout(UILayoutInventory, node, true);

      // inventory visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_INVENTORYVISIBILITY);
      UIVisibilityInventory = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYINVENTORY;

      // spells
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_SPELLS);
      ReadUILayout(UILayoutSpells, node, true);

      // spells visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_SPELLSVISIBILITY);
      UIVisibilitySpells = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYSPELLS;

      // skills
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_SKILLS);
      ReadUILayout(UILayoutSkills, node, true);

      // skills visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_SKILLSVISIBILITY);
      UIVisibilitySkills = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYSKILLS;

      // actions
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ACTIONS);
      ReadUILayout(UILayoutActions, node, true);

      // actions visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ACTIONSVISIBILITY);
      UIVisibilityActions = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYACTIONS;

      // attributes
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ATTRIBUTES);
      ReadUILayout(UILayoutAttributes, node, true);

      // attributes visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ATTRIBUTESVISIBILITY);
      UIVisibilityAttributes = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYATTRIBUTES;

      // mainbuttonsleft
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_MAINBUTTONSLEFT);
      ReadUILayout(UILayoutMainButtonsLeft, node, false);

      // mainbuttonsright
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_MAINBUTTONSRIGHT);
      ReadUILayout(UILayoutMainButtonsRight, node, false);

      // actionbuttons
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ACTIONBUTTONGRID);
      ReadUILayout(UILayoutActionButtons, node, false);

      // onlineplayers
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ONLINEPLAYERS);
      ReadUILayout(UILayoutOnlinePlayers, node, true);

      // onlineplayers visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ONLINEPLAYERSVISIBILITY);
      UIVisibilityOnlinePlayers = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYONLINEPLAYERS;

      // roomobjects
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ROOMOBJECTS);
      ReadUILayout(UILayoutRoomObjects, node, true);

      // roomobjects visibility
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_UI + "/" + TAG_LAYOUT + "/" + TAG_ROOMOBJECTSVISIBILITY);
      UIVisibilityRoomObjects = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_UI_VISIBILITYROOMOBJECTS;

      /******************************************************************************/
      /*  3) Input
      /******************************************************************************/

      // MouseAimSpeed
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_MOUSEAIMSPEED);
      MouseAimSpeed = (PARSE_INT32_ATTRIB(node, XMLATTRIB_VALUE, val_int)) ? val_int : DEFAULTVAL_INPUT_MOUSEAIMSPEED;
      
      // MouseAimDistance
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_MOUSEAIMDISTANCE);
      MouseAimDistance = (PARSE_INT32_ATTRIB(node, XMLATTRIB_VALUE, val_int)) ? val_int : DEFAULTVAL_INPUT_MOUSEAIMDISTANCE;

      // KeyRotateSpeed
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYROTATESPEED);
      KeyRotateSpeed = (PARSE_INT32_ATTRIB(node, XMLATTRIB_VALUE, val_int)) ? val_int : DEFAULTVAL_INPUT_KEYROTATESPEED;

      // InvertMouseY
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_INVERTMOUSEY);
      InvertMouseY = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_INPUT_INVERTMOUSEY;

      // CameraCollisions
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_CAMERACOLLISIONS);
      CameraCollisions = (PARSE_BOOL_ATTRIB(node, XMLATTRIB_VALUE, val_bool)) ? val_bool : DEFAULTVAL_INPUT_CAMERACOLLISIONS;

      // CameraDistanceMax
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_CAMERADISTANCEMAX);
      CameraDistanceMax = (PARSE_FLOAT_ATTRIB(node, XMLATTRIB_VALUE, val_float)) ? val_float : DEFAULTVAL_INPUT_CAMERADISTANCEMAX;

      // CameraPitchMax
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_CAMERAPITCHMAX);
      CameraPitchMax = (PARSE_FLOAT_ATTRIB(node, XMLATTRIB_VALUE, val_float)) ? val_float : DEFAULTVAL_INPUT_CAMERAPITCHMAX;
      CameraPitchMax = MathUtil::Bound(CameraPitchMax, 0.0f, 1.0f);

      //

      KeyBinding = OISKeyBinding::GetDefault();

      // RightClickAction
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING);
      if (PARSE_INT32_ATTRIB(node, ATTRIB_RIGHTCLICKACTION, val_int))
         KeyBinding->RightClickAction = val_int;

      // MoveForward
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_MOVEFORWARD);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->MoveForward = (::OIS::KeyCode)val_uint;

      // MoveBackward
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_MOVEBACKWARD);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->MoveBackward = (::OIS::KeyCode)val_uint;

      // MoveLeft
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_MOVELEFT);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->MoveLeft = (::OIS::KeyCode)val_uint;

      // MoveRight
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_MOVERIGHT);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->MoveRight = (::OIS::KeyCode)val_uint;

      // RotateLeft
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ROTATELEFT);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->RotateLeft = (::OIS::KeyCode)val_uint;

      // RotateRight
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ROTATERIGHT);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->RotateRight = (::OIS::KeyCode)val_uint;

      // Walk
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_WALK);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->Walk = (::OIS::KeyCode)val_uint;

      // AutoMove
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_AUTOMOVE);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->AutoMove = (::OIS::KeyCode)val_uint;

      // NextTarget
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_NEXTTARGET);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->NextTarget = (::OIS::KeyCode)val_uint;

      // SelfTarget
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_SELFTARGET);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->SelfTarget = (::OIS::KeyCode)val_uint;

      // ReqGo
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_OPEN);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ReqGo = (::OIS::KeyCode)val_uint;

      // Close
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_CLOSE);	
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->Close = (::OIS::KeyCode)val_uint;

      // ActionButtons
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION01);		
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton01 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION02);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton02 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION03);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton03 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION04);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton04 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION05);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton05 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION06);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton06 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION07);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton07 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION08);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton08 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION09);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton09 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION10);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton10 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION11);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton11 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION12);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton12 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION13);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton13 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION14);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton14 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION15);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton15 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION16);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton16 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION17);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton17 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION18);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton18 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION19);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton19 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION20);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton20 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION21);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton21 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION22);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton22 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION23);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton23 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION24);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton24 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION25);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton25 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION26);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton26 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION27);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton27 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION28);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton28 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION29);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton29 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION30);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton30 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION31);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton31 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION32);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton32 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION33);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton33 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION34);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton34 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION35);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton35 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION36);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton36 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION37);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton37 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION38);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton38 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION39);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton39 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION40);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton40 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION41);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton41 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION42);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton42 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION43);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton43 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION44);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton44 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION45);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton45 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION46);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton46 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION47);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton47 = (::OIS::KeyCode)val_uint;
      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION48);
      if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
         KeyBinding->ActionButton48 = (::OIS::KeyCode)val_uint;

	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION49);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton49 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION50);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton50 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION51);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton51 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION52);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton52 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION53);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton53 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION54);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton54 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION55);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton55 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION56);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton56 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION57);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton57 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION58);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton58 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION59);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton59 = (::OIS::KeyCode)val_uint;
	  node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_KEYBINDING + "/" + TAG_ACTION60);
	  if (PARSE_UINT32_ATTRIB(node, XMLATTRIB_KEY, val_uint))
		  KeyBinding->ActionButton60 = (::OIS::KeyCode)val_uint;
	  /******************************************************************************/

      node = Document->DocumentElement->SelectSingleNode("/" + XMLTAG_CONFIGURATION + "/" + TAG_INPUT + "/" + TAG_ACTIONBUTTONSETS);

      if (node)
      {
         for each(::System::Xml::XmlNode^ child in node->ChildNodes)
         {
            if (child->Name != TAG_ACTIONBUTTONS || !child->Attributes[ATTRIB_PLAYER])
               continue;

            ActionButtonList^ buttonSet = gcnew ActionButtonList();
            buttonSet->PlayerName = child->Attributes[ATTRIB_PLAYER]->Value;

            for each (::System::Xml::XmlNode^ subchild in child->ChildNodes)
            {
               if (subchild->Name != TAG_ACTIONBUTTON)
                  continue;

               int num;
               CLRString^ type;
               CLRString^ name;
               unsigned int numofsamename;

               if (!PARSE_INT32_ATTRIB(subchild, ATTRIB_NUM, num) ||
                  !subchild->Attributes[ATTRIB_TYPE] ||
                  !subchild->Attributes[XMLATTRIB_NAME] ||
                  !PARSE_UINT32_ATTRIB(subchild, ATTRIB_NUMOFSAMENAME, numofsamename))
                  continue;

               type = subchild->Attributes[ATTRIB_TYPE]->Value;
               name = subchild->Attributes[XMLATTRIB_NAME]->Value;

               ActionButtonType btntype = GetButtonType(type);
               Object^ data = nullptr;

               // resolve the alias references
               if (btntype == ActionButtonType::Alias)
               {
                  // try to find alias
                  data = aliases->GetItemByKey(name);

                  // if not found, reset button to unset
                  if (data == nullptr)
                  {
                     btntype = ActionButtonType::Unset;
                     name = "";
                     numofsamename = 0;
                  }
               }

               buttonSet->Add(gcnew ActionButtonConfig(
                  num, 
                  btntype,
                  name,
                  data,
                  nullptr,
                  numofsamename));
            }

            ActionButtonSets->Add(buttonSet);
         }
      }
   };

   void OgreClientConfig::WriteXml(::System::Xml::XmlWriter^ Writer)
   {
      Config::WriteXml(Writer);

      /******************************************************************************/
      // engine

      Writer->WriteStartElement(TAG_ENGINE);

      Writer->WriteStartElement(TAG_DISPLAY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, Display.ToString());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_RESOLUTION);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, Resolution);
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_WINDOWMODE);
      Writer->WriteAttributeString(ATTRIB_ENABLED, WindowMode.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_WINDOWFRAME);
      Writer->WriteAttributeString(ATTRIB_ENABLED, WindowFrame.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_VSYNC);
      Writer->WriteAttributeString(ATTRIB_ENABLED, VSync.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_FSAA);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, FSAA);
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_NOMIPMAPS);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, NoMipmaps.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_TEXTUREFILTERING);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, TextureFiltering);
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_IMAGEBUILDER);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, ImageBuilder);
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_BITMAPSCALING);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, BitmapScaling);
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_TEXTUREQUALITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, TextureQuality);
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_DECORATIONINTENSITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, DecorationIntensity.ToString());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_DISABLENEWROOMTEXTURES);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableNewRoomTextures.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_DISABLE3DMODELS);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, Disable3DModels.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_DISABLENEWSKY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableNewSky.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_DISABLEWEATHEREFFECTS);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableWeatherEffects.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_BRIGHTNESSFACTOR);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, BrightnessFactor.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_WEATHERPARTICLES);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, WeatherParticles.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_MUSICVOLUME);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, MusicVolume.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_SOUNDVOLUME);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, SoundVolume.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      Writer->WriteStartElement(TAG_DISABLELOOPSOUNDS);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, DisableLoopSounds.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteEndElement();

      /******************************************************************************/
      // ui

      Writer->WriteStartElement(TAG_UI);
      Writer->WriteStartElement(TAG_LAYOUT);
      Writer->WriteAttributeString(XMLATTRIB_LOCKED, UILocked.ToString()->ToLower());

      // avatar
      Writer->WriteStartElement(TAG_AVATAR);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutAvatar->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutAvatar->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutAvatar->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutAvatar->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutAvatar->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutAvatar->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutAvatar->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutAvatar->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // target
      Writer->WriteStartElement(TAG_TARGET);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutTarget->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutTarget->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutTarget->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutTarget->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutTarget->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutTarget->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutTarget->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutTarget->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // target visibility
      Writer->WriteStartElement(TAG_TARGETVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityTarget.ToString()->ToLower());
      Writer->WriteEndElement();

      // minimap
      Writer->WriteStartElement(TAG_MINIMAP);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutMinimap->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutMinimap->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutMinimap->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutMinimap->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutMinimap->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutMinimap->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutMinimap->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutMinimap->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // minimap visibility
      Writer->WriteStartElement(TAG_MINIMAPVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityMiniMap.ToString()->ToLower());
      Writer->WriteEndElement();

      // roomenchantments
      Writer->WriteStartElement(TAG_ROOMENCHANTMENTS);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutRoomEnchantments->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutRoomEnchantments->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutRoomEnchantments->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutRoomEnchantments->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutRoomEnchantments->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutRoomEnchantments->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutRoomEnchantments->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutRoomEnchantments->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // chat
      Writer->WriteStartElement(TAG_CHAT);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutChat->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutChat->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutChat->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutChat->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutChat->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutChat->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutChat->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutChat->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // chat visibility
      Writer->WriteStartElement(TAG_CHATVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityChat.ToString()->ToLower());
      Writer->WriteEndElement();

      // inventory
      Writer->WriteStartElement(TAG_INVENTORY);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutInventory->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutInventory->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutInventory->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutInventory->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutInventory->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutInventory->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutInventory->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutInventory->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // inventory visibility
      Writer->WriteStartElement(TAG_INVENTORYVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityInventory.ToString()->ToLower());
      Writer->WriteEndElement();

      // spells
      Writer->WriteStartElement(TAG_SPELLS);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutSpells->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutSpells->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutSpells->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutSpells->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutSpells->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutSpells->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutSpells->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutSpells->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // spells visibility
      Writer->WriteStartElement(TAG_SPELLSVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilitySpells.ToString()->ToLower());
      Writer->WriteEndElement();

      // skills
      Writer->WriteStartElement(TAG_SKILLS);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutSkills->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutSkills->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutSkills->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutSkills->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutSkills->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutSkills->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutSkills->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutSkills->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // skills visibility
      Writer->WriteStartElement(TAG_SKILLSVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilitySkills.ToString()->ToLower());
      Writer->WriteEndElement();

      // actions
      Writer->WriteStartElement(TAG_ACTIONS);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutActions->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutActions->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutActions->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutActions->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutActions->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutActions->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutActions->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutActions->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // actions visibility
      Writer->WriteStartElement(TAG_ACTIONSVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityActions.ToString()->ToLower());
      Writer->WriteEndElement();

      // attributes
      Writer->WriteStartElement(TAG_ATTRIBUTES);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutAttributes->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutAttributes->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutAttributes->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutAttributes->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutAttributes->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutAttributes->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutAttributes->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutAttributes->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // attributes visibility
      Writer->WriteStartElement(TAG_ATTRIBUTESVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityAttributes.ToString()->ToLower());
      Writer->WriteEndElement();

      // mainbuttonsleft
      Writer->WriteStartElement(TAG_MAINBUTTONSLEFT);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutMainButtonsLeft->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutMainButtonsLeft->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutMainButtonsLeft->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutMainButtonsLeft->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutMainButtonsLeft->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutMainButtonsLeft->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutMainButtonsLeft->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutMainButtonsLeft->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // mainbuttonsright
      Writer->WriteStartElement(TAG_MAINBUTTONSRIGHT);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutMainButtonsRight->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutMainButtonsRight->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutMainButtonsRight->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutMainButtonsRight->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutMainButtonsRight->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutMainButtonsRight->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutMainButtonsRight->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutMainButtonsRight->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // actionbuttons
      Writer->WriteStartElement(TAG_ACTIONBUTTONGRID);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutActionButtons->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutActionButtons->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutActionButtons->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutActionButtons->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutActionButtons->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutActionButtons->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutActionButtons->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutActionButtons->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // onlineplayers
      Writer->WriteStartElement(TAG_ONLINEPLAYERS);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutOnlinePlayers->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutOnlinePlayers->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutOnlinePlayers->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutOnlinePlayers->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutOnlinePlayers->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutOnlinePlayers->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutOnlinePlayers->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutOnlinePlayers->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // onlineplayers visibility
      Writer->WriteStartElement(TAG_ONLINEPLAYERSVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityOnlinePlayers.ToString()->ToLower());
      Writer->WriteEndElement();

      // roomobjects
      Writer->WriteStartElement(TAG_ROOMOBJECTS);
      Writer->WriteAttributeString(XMLATTRIB_XREL, UILayoutRoomObjects->getPosition().d_x.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_XABS, UILayoutRoomObjects->getPosition().d_x.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YREL, UILayoutRoomObjects->getPosition().d_y.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_YABS, UILayoutRoomObjects->getPosition().d_y.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WREL, UILayoutRoomObjects->getSize().d_width.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_WABS, UILayoutRoomObjects->getSize().d_width.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HREL, UILayoutRoomObjects->getSize().d_height.d_scale.ToString(Config::NumberFormatInfo));
      Writer->WriteAttributeString(XMLATTRIB_HABS, UILayoutRoomObjects->getSize().d_height.d_offset.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // roomobjects visibility
      Writer->WriteStartElement(TAG_ROOMOBJECTSVISIBILITY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, UIVisibilityRoomObjects.ToString()->ToLower());
      Writer->WriteEndElement();

      Writer->WriteEndElement();
      Writer->WriteEndElement();

      /******************************************************************************/
      // input

      Writer->WriteStartElement(TAG_INPUT);

      // mouseaimspeed
      Writer->WriteStartElement(TAG_MOUSEAIMSPEED);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, MouseAimSpeed.ToString());
      Writer->WriteEndElement();

      // mouseaimdistance
      Writer->WriteStartElement(TAG_MOUSEAIMDISTANCE);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, MouseAimDistance.ToString());
      Writer->WriteEndElement();

      // keyrotatespeed
      Writer->WriteStartElement(TAG_KEYROTATESPEED);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, KeyRotateSpeed.ToString());
      Writer->WriteEndElement();

      // invertmousey
      Writer->WriteStartElement(TAG_INVERTMOUSEY);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, InvertMouseY.ToString());
      Writer->WriteEndElement();

      // cameracollisions
      Writer->WriteStartElement(TAG_CAMERACOLLISIONS);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, CameraCollisions.ToString());
      Writer->WriteEndElement();

      // cameradistancemax
      Writer->WriteStartElement(TAG_CAMERADISTANCEMAX);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, CameraDistanceMax.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // camerapitchmax
      Writer->WriteStartElement(TAG_CAMERAPITCHMAX);
      Writer->WriteAttributeString(XMLATTRIB_VALUE, CameraPitchMax.ToString(Config::NumberFormatInfo));
      Writer->WriteEndElement();

      // keybinding
      Writer->WriteStartElement(TAG_KEYBINDING);
      Writer->WriteAttributeString(ATTRIB_RIGHTCLICKACTION, KeyBinding->RightClickAction.ToString());

      Writer->WriteStartElement(TAG_MOVEFORWARD);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveForward).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_MOVEBACKWARD);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveBackward).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_MOVELEFT);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveLeft).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_MOVERIGHT);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->MoveRight).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ROTATELEFT);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->RotateLeft).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ROTATERIGHT);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->RotateRight).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_WALK);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->Walk).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_AUTOMOVE);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->AutoMove).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_NEXTTARGET);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->NextTarget).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_SELFTARGET);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->SelfTarget).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_OPEN);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ReqGo).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_CLOSE);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->Close).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION01);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton01).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION02);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton02).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION03);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton03).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION04);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton04).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION05);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton05).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION06);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton06).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION07);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton07).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION08);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton08).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION09);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton09).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION10);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton10).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION11);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton11).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION12);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton12).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION13);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton13).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION14);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton14).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION15);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton15).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION16);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton16).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION17);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton17).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION18);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton18).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION19);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton19).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION20);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton20).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION21);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton21).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION22);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton22).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION23);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton23).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION24);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton24).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION25);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton25).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION26);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton26).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION27);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton27).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION28);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton28).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION29);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton29).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION30);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton30).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION31);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton31).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION32);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton32).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION33);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton33).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION34);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton34).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION35);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton35).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION36);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton36).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION37);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton37).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION38);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton38).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION39);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton39).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION40);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton40).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION41);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton41).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION42);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton42).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION43);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton43).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION44);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton44).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION45);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton45).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION46);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton46).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION47);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton47).ToString());
      Writer->WriteEndElement();
      Writer->WriteStartElement(TAG_ACTION48);
      Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton48).ToString());
      Writer->WriteEndElement();

	  Writer->WriteStartElement(TAG_ACTION49);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton49).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION50);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton50).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION51);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton51).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION52);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton52).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION53);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton53).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION54);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton54).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION55);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton55).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION56);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton56).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION57);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton57).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION58);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton58).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION59);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton59).ToString());
	  Writer->WriteEndElement();
	  Writer->WriteStartElement(TAG_ACTION60);
	  Writer->WriteAttributeString(XMLATTRIB_KEY, ((unsigned int)KeyBinding->ActionButton60).ToString());
	  Writer->WriteEndElement();

      // keybinding end
      Writer->WriteEndElement();

      // actionbuttonsets
      Writer->WriteStartElement(TAG_ACTIONBUTTONSETS);
      Writer->WriteAttributeString(ATTRIB_COUNT, ActionButtonSets->Count.ToString());

      for (int i = 0; i < ActionButtonSets->Count; i++)
      {
         ActionButtonList^ set = ActionButtonSets[i];

         // actionbuttons
         Writer->WriteStartElement(TAG_ACTIONBUTTONS);
         Writer->WriteAttributeString(ATTRIB_PLAYER, set->PlayerName);
         Writer->WriteAttributeString(ATTRIB_COUNT, set->Count.ToString());

         for (int j = 0; j < set->Count; j++)
         {
            // actionbutton
            Writer->WriteStartElement(TAG_ACTIONBUTTON);
            Writer->WriteAttributeString(ATTRIB_NUM, set[j]->Num.ToString());
            Writer->WriteAttributeString(ATTRIB_TYPE, set[j]->ButtonType.ToString()->ToLower());
            Writer->WriteAttributeString(XMLATTRIB_NAME, set[j]->Name->ToLower());
            Writer->WriteAttributeString(ATTRIB_NUMOFSAMENAME, set[j]->NumOfSameName.ToString());
            Writer->WriteEndElement();
         }

         // actionbuttons end
         Writer->WriteEndElement();
      }

      // actionbuttonsets end
      Writer->WriteEndElement();

      // input end
      Writer->WriteEndElement();
   };

   ActionButtonType OgreClientConfig::GetButtonType(CLRString^ ButtonType)
   {
      if (CLRString::Equals(ButtonType, BUTTONTYPE_SPELL))
         return ActionButtonType::Spell;

      if (CLRString::Equals(ButtonType, BUTTONTYPE_SKILL))
         return ActionButtonType::Skill;

      else if (CLRString::Equals(ButtonType, BUTTONTYPE_ACTION))
         return ActionButtonType::Action;

      else if (CLRString::Equals(ButtonType, BUTTONTYPE_ITEM))
         return ActionButtonType::Item;

      else if (CLRString::Equals(ButtonType, BUTTONTYPE_ALIAS))
         return ActionButtonType::Alias;

      else
         return ActionButtonType::Unset;
   };

   ActionButtonList^ OgreClientConfig::GetActionButtonSetByName(CLRString^ Name)
   {
      for each(ActionButtonList^ set in ActionButtonSets)
         if (CLRString::Equals(set->PlayerName, Name))
            return set;

      return nullptr;
   };

   void OgreClientConfig::AddOrUpdateActionButtonSet(ActionButtonList^ Buttons)
   {
      ActionButtonList^ set = GetActionButtonSetByName(Buttons->PlayerName);

      if (set != nullptr)
      {
         // clear old assignments
         set->Clear();

         // fill new assigned actionbuttons
         for each (ActionButtonConfig^ btn in Buttons)
            set->Add(btn);
      }
      else
      {
         // create new buttonlist
         ActionButtonList^ btnList = gcnew ActionButtonList();
         btnList->PlayerName = Buttons->PlayerName;

         // fill new assigned actionbuttons
         for each (ActionButtonConfig^ btn in Buttons)
            btnList->Add(btn);

         ActionButtonSets->Add(btnList);
      }
   };
};};
