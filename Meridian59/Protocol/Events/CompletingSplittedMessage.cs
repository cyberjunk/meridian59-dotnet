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

namespace Meridian59.Protocol.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CompletingSplittedMessageEventHandler(object sender, CompletingSplittedMessageEventArgs e);

    /// <summary>
    /// Event which carries data of a situation when a game message
    /// was split across TCP buffers.
    /// </summary>
    public class CompletingSplittedMessageEventArgs : EventArgs
    {
        public int BufferLen;
        public int BufferCursorPos;
        public int BufferMemPos;
        public int MessageMemPos;
        public int MessageLength;

        public CompletingSplittedMessageEventArgs(int BufferMemPos, int BufferLen, int BufferCursorPos, int MessageMemPos, int MessageLength)
        {
            this.BufferMemPos = BufferMemPos;
            this.BufferLen = BufferLen;
            this.BufferCursorPos = BufferCursorPos;
            this.MessageMemPos = MessageMemPos;
            this.MessageLength = MessageLength;
        }
    }
}
