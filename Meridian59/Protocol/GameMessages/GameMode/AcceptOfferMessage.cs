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

using System;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Sent from client to server when a pending trade is accepted
    /// </summary>
    [Serializable]
    public class AcceptOfferMessage : GameModeMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AcceptOfferMessage()
            : base(MessageTypeGameMode.AcceptOffer) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        public AcceptOfferMessage(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex = 0) { }
    }
}
