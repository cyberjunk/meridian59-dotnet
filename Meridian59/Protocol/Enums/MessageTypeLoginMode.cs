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
    /// These are the valid message types for protocol mode 'login'.
    /// See original 'proto.h'.
    /// </summary>
    public enum MessageTypeLoginMode : byte
    {
        Ping            = 1,
        Login           = 2,
        Register        = 3,
        ReqGame         = 4,
        ReqAdmin        = 5,
        Resync          = 6,
        GetClient       = 7,
        GetResource     = 8,
        GetAll          = 9,
        ReqMenu         = 10,
        AdminNote       = 11,
        ClientPatchOld  = 12, // Only valid for MeridianNext
        ClientPatch     = 13, // Only valid for MeridianNext

        GetLogin        = 21,
        GetChoice       = 22,
        LoginOK         = 23,
        LoginFailed     = 24,
        Game            = 25,
        Admin           = 26,
        AccountUsed     = 27,
        TooManyLogins   = 28,
        Timeout         = 29,
        Credits         = 30,
        Download        = 31,
        Upload          = 32,
        NoCredits       = 33,
        Message         = 34,
        DeleteRsc       = 35,
        DeleteAllRsc    = 36,
        NoCharacters    = 37,
        Guest           = 38,
        ServiceReport   = 39
    }
}
