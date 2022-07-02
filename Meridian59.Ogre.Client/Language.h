/*
Copyright (c) 2012-2013 Clint Banzhaf
This file is part of "Meridian59 .NET".

"Meridian59 .NET" is free software:
You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
either version 3 of the License, or (at your option) any later version.

"Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
If not, see http://www.gnu.org/licenses/.
*/

namespace LANGSTR
{
   enum Enum
   {
      USERNAME = 0,
      PASSWORD = 1,
      CONNECT  = 2,
      OPTIONS  = 3,
      ON       = 4,
      OFF      = 5,
      KEY      = 6
   };
}

namespace LANGSTR_WINDOW_TITLE
{
   enum Enum
   {
      WELCOME     = 0,
      SPELLS      = 1,
      SKILLS      = 2,
      ACTIONS     = 3,
      INVENTORY   = 4,
      AMOUNT      = 5,
      TRADE       = 6,
      OPTIONS     = 7,
      ATTRIBUTES  = 8,
      MAIL        = 9,
      GUILD       = 10,
      PLAYERS     = 11
   };
}

namespace LANGSTR_TOOLTIP_MOOD
{
   enum Enum
   {
      HAPPY    = 0,
      NEUTRAL  = 1,
      SAD      = 2,
      ANGRY    = 3
   };
}

namespace LANGSTR_TOOLTIP_ONLINEPLAYER
{
   enum Enum
   {
      LAWFUL    = 0,
      MURDERER  = 1,
      OUTLAW    = 2,
      ADMIN     = 3,
      GM        = 4,
      MODERATOR = 5
   };
}

namespace LANGSTR_TOOLTIP_STATUSBAR
{ 
   enum Enum
   {
      FPS_TOOLTIP           = 0,
      PING_TOOLTIP          = 1,
      PLAYERCOUNT_TOOLTIP   = 2,
      MOOD_TOOLTIP          = 3,
      SAFETY_TOOLTIP        = 4,
      TIME_TOOLTIP          = 5,
      ROOM_TOOLTIP          = 6,
      UILOCKED_TOOLTIP      = 7,
      UIUNLOCKED_TOOLTIP    = 8,
      UIRESET_TOOLTIP       = 9,
      UIRESETLOCKED_TOOLTIP = 10
   };
}

namespace LANGSTR_DESCRIPTION_STATUSBAR
{
   enum Enum
   {
      FPS_DESCRIPTION         = 1,
      PING_DESCRIPTION        = 2,
      PLAYERCOUNT_DESCRIPTION = 3,
      MOOD_DESCRIPTION        = 4,
      SAFETY_DESCRIPTION      = 5,
      TIME_DESCRIPTION        = 6,
      ROOM_DESCRIPTION        = 7
   };
}

namespace LANGSTR_CHARINFONOTOKERROR_OKDIALOG
{
   enum Enum
   {
      NOCHARINFOERROR = 0, // NOERROR and NO_ERROR are Windows constants
      GENERICERROR = 1,
      NOTFIRSTTIME = 2,
      NAMETOOLONG = 3,
      NAMEBADCHARACTERS = 4,
      NAMEINUSE = 5,
      NOMOBNAME = 6,
      NONPCNAME = 7,
      NOGUILDNAME = 8,
      NOBADWORDS = 9,
      NOCONFUSINGNAME = 10,
      NORETIREDNAME = 11,
      DESCRIPTIONTOOLONG = 12,
      INVALIDGENDER = 13
   };
}

namespace LANGSTR_CHARSSELECTABILITYERROR
{
   enum Enum
   {
      NOCHARSELECTERROR = 0,
      NOPOINTSLEFTERROR = 1,
      ALREADYHAVESHALILLEERROR = 2,
      ALREADYHAVEQORERROR = 3,
      NOTENOUGHLEVELONEERROR = 4
   };
}

namespace LANGSTR_NPCQUESTUI
{
   enum Enum
   {
      DESCRIPTION = 0,
      REQUIREMENTS = 1,
      INSTRUCTIONS = 2,
      ACCEPT = 3,
      CONTINUE = 4,
      CLOSE = 5,
      HELPTITLE = 6,
      HELPTEXT = 7
   };
}

namespace LANGSTR_MISC
{
   enum Enum
   {
      EQUIPPED = 0
   };
}

namespace LANGSTR_TOOLTIP_TARGET
{
   enum Enum
   {
      INSPECT = 0,
      ATTACK = 1,
      ACTIVATE = 2,
      ITEMS = 3,
      BUY = 4,
      TRADE = 5,
      LOOT = 6,
      QUEST = 7
   };
}

const char* GetLangLabel(const LANGSTR::Enum ID);
const char* GetLangWindowTitle(const LANGSTR_WINDOW_TITLE::Enum ID);
const char* GetLangTooltipMood(const LANGSTR_TOOLTIP_MOOD::Enum ID);
const char* GetLangTooltipOnlinePlayer(const LANGSTR_TOOLTIP_ONLINEPLAYER::Enum ID);
const char* GetLangTooltipStatusBar(const LANGSTR_TOOLTIP_STATUSBAR::Enum ID);
const char* GetLangDescriptionStatusBar(const LANGSTR_DESCRIPTION_STATUSBAR::Enum ID);
const char* GetCharInfoNotOKErrorOkDialog(const LANGSTR_CHARINFONOTOKERROR_OKDIALOG::Enum ID);
const char* GetLangCharSelectAbilityError(const LANGSTR_CHARSSELECTABILITYERROR::Enum ID);
const char* GetLangNPCQuestUI(const LANGSTR_NPCQUESTUI::Enum ID);
const char* GetLangMisc(const LANGSTR_MISC::Enum ID);
const char* GetLangTooltipTarget(const LANGSTR_TOOLTIP_TARGET::Enum ID);
