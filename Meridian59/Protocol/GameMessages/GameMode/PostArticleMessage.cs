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
using System.Text;
using Meridian59.Common.Constants;
using Meridian59.Protocol.Enums;

namespace Meridian59.Protocol.GameMessages
{
    [Serializable]
    public class PostArticleMessage : GameModeMessage
    {        
        #region IByteSerializable
        public override int ByteLength
        {
            get
            {
                int length = base.ByteLength + TypeSizes.SHORT;

                length += TypeSizes.SHORT + Title.Length;
                length += TypeSizes.SHORT + Text.Length;

                return length;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(GlobeID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Title.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(Title), 0, Buffer, cursor, Title.Length);
            cursor += Title.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Text.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(Text), 0, Buffer, cursor, Text.Length);
            cursor += Text.Length;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            GlobeID = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;
           
            Title = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Text = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            return cursor - StartIndex;
        }
        #endregion

        public ushort GlobeID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public PostArticleMessage(ushort GlobeID, string Title, string Text) 
            : base(MessageTypeGameMode.PostArticle)
        {
            this.GlobeID = GlobeID;
            this.Title = Title;
            this.Text = Text;
        }

        public PostArticleMessage(byte[] Buffer, int StartIndex = 0) 
            : base (Buffer, StartIndex = 0) { }        
    }
}
