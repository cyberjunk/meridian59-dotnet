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

namespace Meridian59.Protocol.Enums
{
    /// <summary>
    /// These are the valid message types for protocol mode 'game'.
    /// See original 'proto.h'.
    /// </summary>
    public enum MessageTypeGameMode
    {                                  
        EchoPing            = 1,
        Resync              = 2,
        Ping                = 3,
        RoundTrip1          = 4,
        RoundTrip2          = 5,
        System              = 6,
        
        Logoff              = 20,
        Wait                = 21,
        Unwait              = 22,
        ChangePassword      = 23,
        AdSelected          = 24,

        ChangeResource      = 30,        
        SysMessage          = 31,
        Message             = 32,

        SendPlayer          = 40,
        SendStats           = 41,
        SendRoomContents    = 42,
        SendObjectContents  = 43,
        SendPlayers         = 44,
        SendCharacters      = 45,
        UseCharacter        = 46,
        DeleteCharacter     = 47,
        NewCharInfo         = 48,
        SendCharInfo        = 49,
        SendSpells          = 50,
        SendSkills          = 51,
        SendStatGroups      = 52,
        SendEnchantments    = 53,
        ReqQuit             = 54,
        SayBlocked          = 55,
        CharInfoOk          = 56,
        CharInfoNotOk       = 57,
        LoadModule          = 58,
        UnloadModule        = 59,
        
        ReqAdmin            = 60,
        ReqDM               = 61,
        ReqAdminQuest       = 62,

        Effect              = 70,

        Mail                = 80,
        ReqGetMail          = 81,
        SendMail            = 82,
        DeleteMail          = 83,
        
        ReqArticles         = 85,
        ReqArticle          = 86,
        PostArticle         = 87,
        
        ReqLookupNames      = 88,
        
        Action              = 90,
        ReqMove             = 100,
        ReqTurn             = 101,
        ReqGo               = 102,
        ReqAttack           = 103,
        ReqShoot            = 104,
        ReqCast             = 105,
        ReqUse              = 106,
        ReqUnuse            = 107,
        ReqApply            = 108,
        ReqActivate         = 109,
        SayTo               = 110,
        SayGroup            = 111,
        ReqPut              = 112,
        ReqGet              = 113,
        ReqGive             = 114,
        ReqTake             = 115,
        ReqLook             = 116,
        ReqInventory        = 117,
        ReqDrop             = 118,
        ReqHide             = 119,
        ReqOffer            = 120,
        AcceptOffer         = 121,
        CancelOffer         = 122,
        ReqCounterOffer     = 123,
        ReqBuy              = 124,
        ReqBuyItems         = 125,
        ChangeDescription   = 126,
#if !VANILLA
        ReqInventoryMove    = 127,
#endif
        Player              = 130,
        Stat                = 131,
        StatGroup           = 132,
        StatGroups          = 133,
        RoomContents        = 134,
        ObjectContents      = 135,
        Players             = 136,
        PlayerAdd           = 137,
        PlayerRemove        = 138,
        Characters          = 139,
        CharInfo            = 140,
        Spells              = 141,
        SpellAdd            = 142,
        SpellRemove         = 143,
        Skills              = 144,
        SkillAdd            = 145,
        SkillRemove         = 146,
        AddEnchantment      = 147,
        RemoveEnchantment   = 148,
        Quit                = 149,
        Background          = 150,
        PlayerOverlay       = 151,
        AddBgOverlay        = 152,
        RemoveBgOverlay     = 153,
        ChangeBgOverlay     = 154,
        UserCommand         = 155,
#if !VANILLA
        ReqStatChange       = 156,
        ChangedStats        = 157,
        //ChangedStatsOk      = 158,    // unused so far
        //ChangedStatsNotOk   = 159,    // unused so far
#endif
        PasswordOK          = 160,
        PasswordNotOK       = 161,
        Admin               = 162,

        PlayWave            = 170,
        PlayMusic           = 171,
        PlayMidi            = 172,

        LookNewsGroup       = 180,
        Articles            = 181,
        Article             = 182,

        LookupNames         = 190,

        Move                = 200,
        Turn                = 201,
        Shoot               = 202,
        Use                 = 203,
        Unuse               = 204,
        UseList             = 205,
        Said                = 206,
        Look                = 207,
        Inventory           = 208,
        InventoryAdd        = 209,
        InventoryRemove     = 210,
        Offer               = 211,
        OfferCanceled       = 212,
        Offered             = 213,
        CounterOffer        = 214,
        CounterOffered      = 215,
        BuyList             = 216,
        Create              = 217,
        Remove              = 218,
        Change              = 219,
        LightAmbient        = 220,
        LightPlayer         = 221,
        LightShading        = 222,
        SectorMove          = 223,
        SectorLight         = 224,
        WallAnimate         = 225,
        SectorAnimate       = 226,
        ChangeTexture       = 227,
        InvalidateData      = 228,

        ReqDeposit          = 230,
        WithDrawAlList      = 231,
        ReqWithdrawAl       = 232,
        ReqWithdrawAlItems  = 233,
        XlatOverride        = 234,
        WallScroll          = 235,
        SectorScroll        = 236,
        SetView             = 237,
        ResetView           = 238,

        Blacklisted         = 0xFF  // NOTE: THIS DOES NOT REALLY EXIST, DEBUG PURPOSE
    }
}
