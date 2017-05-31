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
	"On",					// 4
	"Off",					// 5
	"Key",					// 6
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

const char* EN_TOOLTIPS_STATUSBAR[] =
{
	"Frames per second - Higher is better (more fluid Gameplay).",					// 0
	"Your Connectionspeed to the Server. Lower value means faster connection.",		// 1
	"The number of players online. Click me to show the list of online Players.",	// 2
	"Shows the actual mood of your character.",										// 3
	"If you change your safety to Off you can attack inocent players.",				// 4
	"Here you can see the actual Meridian 59 time.",								// 5
	"This shows you the actual room / area name, in which you actually are."		// 6
};

const char* EN_DESCRIPTIONS_STATUSBAR[] =
{
	"FPS",									// 0
	"Ping",									// 1
	"Players",								// 2
	"Mood",									// 3
	"Safety",								// 4
	"Meridian-Time",						// 5
	"Room / Area"							// 6
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
	"Ein",					// 4
	"Aus",					// 5
	"Taste",				// 6
	"Angriff",				// 7
	"Rasten",				// 8
	"Tanzen",				// 9
	"Winken",				// 10
	"Zeigen",				// 11
	"Plündern",				// 12
	"Kaufen",				// 13
	"Untersuchen",			// 14
	"Handeln",				// 15
	"Aktivieren",			// 16
	"Gilden Einladung",		// 17
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

const char* DE_TOOLTIPS_STATUSBAR[] =
{
	"Frames pro Sekunde - Ein hoher Wert ist besser (flüssiges Spielerlebnis).",			// 0
	"Deine Verbindungsverzögerung (Lag) zum Server. Je niedriger, umso besser.",			// 1
	"Anzahl der aktuell eingeloggten Spieler. Klick mich, um die Spielerliste anzuzeigen.",	// 2
	"Zeigt die aktuelle Stimmung Deines Characters an.",									// 3
	"Wenn Du Deine Sicherheit auf Aus stellst, kannst Du unschuldige Spieler angreifen",	// 4
	"Hier siehst Du die aktuelle Meridian 59 Zeit.",										// 5
	"Zeigt Dir den aktuellen Raum / Bereich an, indem Du Dich befindest."					// 6
};

const char* DE_DESCRIPTIONS_STATUSBAR[] =
{
	"FPS",									// 0
	"Ping",									// 1
	"Spieler",								// 2
	"Stimmung",								// 3
	"Sicherheit",							// 4
	"Meridian-Zeit",						// 5
	"Raum / Gebiet"							// 6
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

const char* GetLangTooltipStatusBar(const LANGSTR_TOOLTIP_STATUSBAR::Enum ID)
{
	using ::Meridian59::Ogre::OgreClient;
	using ::Meridian59::Common::Enums::LanguageCode;

	switch (OgreClient::Singleton->Config->Language)
	{
	case LanguageCode::German:	return DE_TOOLTIPS_STATUSBAR[ID];
	default:					return EN_TOOLTIPS_STATUSBAR[ID];
	}
};

const char* GetLangDescriptionStatusBar(const LANGSTR_DESCRIPTION_STATUSBAR::Enum ID)
{
	using ::Meridian59::Ogre::OgreClient;
	using ::Meridian59::Common::Enums::LanguageCode;

	switch (OgreClient::Singleton->Config->Language)
	{
	case LanguageCode::German:	return DE_DESCRIPTIONS_STATUSBAR[ID];
	default:					return EN_DESCRIPTIONS_STATUSBAR[ID];
	}
};
