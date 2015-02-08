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
    /// Possible connectionstates of the ServerConnection
    /// </summary>
    /// <remarks>
    /// Explanation of values:
    /// ----------------------
    /// Offline         =   Not connected at all
    /// Connected       =   Received the very first message (GetLoginMessage = AP_GET_LOGIN) after successful TCP connect
    /// Authenticated   =   Got a positive result to "LoginMessage", either "LoginOK" or "GetClient"
    /// Waiting         =   Received a "WaitMessage" (BP_WAIT), game is frozen during server save
    /// Online          =   Received a "GameStateMessage" (AP_GAME), this is also the transition from LoginMode to GameMode in the protocol messages
    /// Playing         =   Set when tracked an sent/outgoing "BP_USE_CHARACTER" message
    /// </remarks>
    public enum ConnectionState 
    { 
        Offline         = 0, 
        Connected       = 1,
        Authenticated   = 2, 
        Waiting         = 3,
        Online          = 4, 
        Playing         = 5
    };
}
