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

namespace Meridian59.Common.Enums
{
    /// <summary>
    /// Subtype of a UserCommand
    /// </summary>
    public enum UserCommandType : byte
    {
        SendQuit            = 1,
        LookPlayer          = 2,
        ChangeURL           = 3,
        SpellSchools        = 4,
        
        Rest                = 5,
        Stand               = 6,
        Safety              = 7,
        Suicide             = 8,
#if !VANILLA
        TempSafe            = 9,
#endif
        ReqGuildInfo        = 10,
        GuildInfo           = 11,
        Invite              = 12,
        Exile               = 13,
        Renounce            = 14,
        Abdicate            = 15,
        Vote                = 16,
        SetRank             = 17,
        GuildAsk            = 18,
        GuildCreate         = 19,
        Disband             = 20,
        ReqGuildList        = 21,
        GuildList           = 22,
        MakeAlliance        = 23,
        EndAlliance         = 24,
        MakeEnemy           = 25,
        EndEnemy            = 26,
        GuildHalls          = 27,
        AbandonGuildHall    = 28,
        GuildRent           = 29,
        GuildSetPassword    = 30,
        GuildShield         = 31,
        GuildShields        = 32,
        ClaimShield         = 33,
#if !VANILLA
        Grouping            = 34,
#endif
        Deposit             = 35,
        WithDraw            = 36,
        Balance             = 37,

        Appeal              = 40,
        ReqRescue           = 41,

        MiniGameStart       = 45,
        MiniGameState       = 46,
        MiniGameMove        = 47,
        MiniGamePlayer      = 48,
        MiniGameResetPlayers= 49        
    }
}
