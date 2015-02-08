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
using Meridian59.Data.Models;

namespace Meridian59.Protocol.GameMessages
{
    /// <summary>
    /// Part of the message overview/list in a news globe
    /// </summary>
    [Serializable]
    public class ArticlesMessage : GameModeMessage
    {        
        #region IByteSerializable implementation
        public override int ByteLength
        {
            get
            {
                int len = base.ByteLength + TypeSizes.SHORT + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.SHORT;

                foreach (ArticleHead obj in Articles)
                    len += obj.ByteLength;

                return len;
            }
        }

        public override int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(NewsgroupID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = Part;
            cursor++;

            Buffer[cursor] = MaxPart;
            cursor++;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Articles.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (ArticleHead obj in Articles)
                cursor += obj.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            cursor += base.ReadFrom(Buffer, cursor);

            NewsgroupID = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Part = Buffer[cursor];
            cursor++;

            MaxPart = Buffer[cursor];
            cursor++;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Articles = new ArticleHead[len];
            for (int i = 0; i < len; i++)
            {
                Articles[i] = new ArticleHead(Buffer, cursor);
                cursor += Articles[i].ByteLength;
            }

            return cursor - StartIndex;
        }

        public override unsafe void WriteTo(ref byte* Buffer)
        {
            base.WriteTo(ref Buffer);

            *((ushort*)Buffer) = NewsgroupID;
            Buffer += TypeSizes.SHORT;

            Buffer[0] = Part;
            Buffer++;

            Buffer[0] = MaxPart;
            Buffer++;

            *((ushort*)Buffer) = (ushort)Articles.Length;
            Buffer += TypeSizes.SHORT;

            foreach (ArticleHead obj in Articles)
                obj.WriteTo(ref Buffer);
        }

        public override unsafe void ReadFrom(ref byte* Buffer)
        {
            base.ReadFrom(ref Buffer);

            NewsgroupID = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Part = Buffer[0];
            Buffer++;

            MaxPart = Buffer[0];
            Buffer++;

            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Articles = new ArticleHead[len];
            for (int i = 0; i < len; i++)
                Articles[i] = new ArticleHead(ref Buffer);
            
        }
        #endregion

        public ushort NewsgroupID { get; set; }
        public byte Part { get; set; }
        public byte MaxPart { get; set; }       
        public ArticleHead[] Articles { get; set; }

        public ArticlesMessage(ArticleHead[] Articles) 
            : base(MessageTypeGameMode.Articles)
        {
            this.Articles = Articles;
        }

        public ArticlesMessage(byte[] Buffer, int StartIndex = 0)
            : base(Buffer, StartIndex = 0) { }

        public unsafe ArticlesMessage(ref byte* Buffer)
            : base(ref Buffer) { }
    }
}
