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
using Meridian59.Common;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class LookMessage : GameModeMessage
    {       
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {                
                return base.ByteLength + ObjectInfo.ByteLength;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);
            cursor += ObjectInfo.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            ObjectInfo = new ObjectInfo(LookupList, Buffer, cursor);
            cursor += ObjectInfo.ByteLength;
        
            return cursor - StartIndex;
        }
        #endregion

        public ObjectInfo ObjectInfo { get; set; }

		public StringDictionary LookupList { get; private set; }

		public LookMessage(ObjectInfo ObjectInfo, StringDictionary LookupList)
            : base(MessageTypeGameMode.Look)
        {
            this.ObjectInfo = ObjectInfo;
            this.LookupList = LookupList;         
        }

		public LookMessage(StringDictionary LookupList, byte[] Buffer, int StartIndex = 0) 
            : base () 
        {
            this.LookupList = LookupList;
            ReadFrom(Buffer, StartIndex);
        }       
    }
}
