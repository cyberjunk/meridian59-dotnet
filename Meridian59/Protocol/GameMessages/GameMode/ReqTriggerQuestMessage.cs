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
using Meridian59.Protocol.Enums;
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class ReqTriggerQuestMessage : GameModeMessage
    {        
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                return base.ByteLength + NPCID.ByteLength + QuestID.ByteLength;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
            cursor += NPCID.WriteTo(Buffer, cursor);
            cursor += QuestID.WriteTo(Buffer, cursor);
           
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            NPCID = new ObjectID(Buffer, cursor);
            cursor += NPCID.ByteLength;

            QuestID = new ObjectID(Buffer, cursor);
            cursor += QuestID.ByteLength;

            return cursor - StartIndex;
        }
        #endregion
              
        public ObjectID NPCID { get; set; }
        public ObjectID QuestID { get; set; }

        public ReqTriggerQuestMessage(ObjectID NPCID, ObjectID QuestID) 
            : base(MessageTypeGameMode.ReqTriggerQuest)
        {
            this.NPCID = NPCID;
            this.QuestID = QuestID;
        }

        public ReqTriggerQuestMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }      
    }
}
