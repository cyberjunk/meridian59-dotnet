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
using Meridian59.Common.Constants;
using Meridian59.Common.Enums;
using Meridian59.Common;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// The quest list you get from NPCs
    /// </summary>
    [Serializable]
    public class QuestUIListMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + QuestGiver.ByteLength + TypeSizes.SHORT;

                foreach (QuestObjectInfo obj in Quests)
                    len += obj.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            cursor += QuestGiver.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Quests.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (QuestObjectInfo obj in Quests)
                cursor += obj.WriteTo(Buffer, cursor);
            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            QuestGiver = new ObjectBase(true, Buffer, cursor);
            cursor += QuestGiver.ByteLength;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Quests = new QuestObjectInfo[len];
            for (int i = 0; i < len; i++)
            {
                Quests[i] = new QuestObjectInfo(LookupList, Buffer, cursor);
                cursor += Quests[i].ByteLength;
            }

            return cursor - StartIndex;
        }
        #endregion

        public ObjectBase QuestGiver { get; set; }
        public QuestObjectInfo[] Quests { get; set; }
        public StringDictionary LookupList { get; private set; }

        public QuestUIListMessage(ObjectBase QuestGiver, QuestObjectInfo[] Quests, StringDictionary LookupList)
            : base(MessageTypeGameMode.QuestUIList)
        {
            this.QuestGiver = QuestGiver; 
            this.Quests = Quests;
            this.LookupList = LookupList;
        }

        public QuestUIListMessage(StringDictionary LookupList, byte[] Buffer, int StartIndex = 0) 
            : base ()
        {
            this.LookupList = LookupList;
            ReadFrom(Buffer, StartIndex);
        }
    }
}
