#include "stdafx.h"

/**************************************************************************************/
/***************************      ENGLISH     *****************************************/
/**************************************************************************************/

const char* EN[] =
{
	"Username",				// 0
	"Password",				// 1
	"Connect",				// 2
	"Options",				// 3
};

const char* EN_WINDOW_TITLES[] =
{
	"Welcome",				// 0
	"Spells",				// 1
	"Skills",				// 2
	"Actions",				// 3
	"Inventory",			// 4
	"Amount",				// 5
	"Trade",				// 6
	"Options",				// 7
	"Attributes",			// 8
	"Mail",					// 9
	"Guild",				// 10
	"Players",				// 11
};

const char* EN_TOOLTIPS_MOOD[] =
{
	"Click me to change your mood to happy.",			// 0
	"Click me to change your mood to neutral.",			// 1
	"Click me to change your mood to sad.",				// 2
	"Click me to change your mood to angry.",			// 3
};

const char* EN_TOOLTIPS_ONLINEPLAYER[] =
{
	"This is a lawful player.",												// 0
	"This player has murdered someone. Be careful when you meet him!",		// 1
	"This player is an outlaw and may be dangerous!",						// 2
	"This player is an admin of the server.",								// 3
	"This player is a guide/barde or other kind of member of the staff.",	// 4
	"This player is a moderator."											// 5
};

/**************************************************************************************/
/***************************      GERMAN      *****************************************/
/**************************************************************************************/

const char* DE[] =
{
	"Benutzername",			// 0
	"Passwort",				// 1
	"Verbinden",			// 2
	"Optionen",				// 3
};

const char* DE_WINDOW_TITLES[] =
{
	"Willkommen",			// 0
	"Zaubersprüche",		// 1
	"Fertigkeiten",			// 2
	"Aktionen",				// 3
	"Inventar",				// 4
	"Menge",				// 5
	"Handel",				// 6
	"Optionen",				// 7
	"Attribute",			// 8
	"Post",					// 9
	"Gilde",				// 10
	"Spieler",				// 11
};

const char* DE_TOOLTIPS_MOOD[] =
{
	"Klick mich, um deine Stimmung in glücklich zu ändern.",	// 0
	"Klick mich, um deine Stimmung in neutral zu ändern.",		// 1
	"Klick mich, um deine Stimmung in traurig zu ändern.",		// 2
	"Klick mich, um deine Stimmung in wütend zu ändern.",		// 3
};

const char* DE_TOOLTIPS_ONLINEPLAYER[] =
{
	"Ein gesetztestreuer Spieler.",									// 0
	"Ein Mörder! Sei vorsichtig, wenn du ihn triffst.",				// 1
	"Ein Gesetzloser, der gefährlich sein könnte.",					// 2
	"Ein Administrator des Servers.",								// 3
	"Ein Guide, Barde oder sonstiges Mitglied des Admin-Teams.",	// 4
	"Ein Moderator."												// 5
};

/**************************************************************************************/
/**************************************************************************************/

const char* GetLangLabel(const LANGSTR::Enum ID)
{
	using ::Meridian59::Ogre::OgreClient;
	using ::Meridian59::Common::Enums::LanguageCode;

	switch (OgreClient::Singleton->Config->Language)
	{
	case LanguageCode::German:	return DE[ID];
	default:					return EN[ID];
	}
};

const char* GetLangWindowTitle(const LANGSTR_WINDOW_TITLE::Enum ID)
{
	using ::Meridian59::Ogre::OgreClient;
	using ::Meridian59::Common::Enums::LanguageCode;

	switch (OgreClient::Singleton->Config->Language)
	{
	case LanguageCode::German:	return DE_WINDOW_TITLES[ID];
	default:					return EN_WINDOW_TITLES[ID];
	}
};

const char* GetLangTooltipMood(const LANGSTR_TOOLTIP_MOOD::Enum ID)
{
	using ::Meridian59::Ogre::OgreClient;
	using ::Meridian59::Common::Enums::LanguageCode;

	switch (OgreClient::Singleton->Config->Language)
	{
	case LanguageCode::German:	return DE_TOOLTIPS_MOOD[ID];
	default:					return EN_TOOLTIPS_MOOD[ID];
	}
};

const char* GetLangTooltipOnlinePlayer(const LANGSTR_TOOLTIP_ONLINEPLAYER::Enum ID)
{
	using ::Meridian59::Ogre::OgreClient;
	using ::Meridian59::Common::Enums::LanguageCode;

	switch (OgreClient::Singleton->Config->Language)
	{
	case LanguageCode::German:	return DE_TOOLTIPS_ONLINEPLAYER[ID];
	default:					return EN_TOOLTIPS_ONLINEPLAYER[ID];
	}
};
