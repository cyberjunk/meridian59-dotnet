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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using System.Text;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    public class UserCommandChangeURL : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.ChangeURL; } }

        #region IByteSerializable implementation
        public override int ByteLength 
        { 
            get 
            { 
                return TypeSizes.BYTE + TypeSizes.INT + TypeSizes.SHORT + URL.Length;
            }
        }       
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;
            cursor++;

            Array.Copy(BitConverter.GetBytes(ObjectID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(URL.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(URL), 0, Buffer, cursor, URL.Length);
            cursor += URL.Length;

            return cursor - StartIndex;
        }
        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if ((UserCommandType)Buffer[cursor] != CommandType)
                throw new Exception(ERRORWRONGTYPEBYTE);
            else
            {
                cursor++;

                ObjectID = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                ushort strlen = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                URL = Util.Encoding.GetString(Buffer, cursor, strlen);
                cursor += strlen;
            }

            return cursor - StartIndex;
        }
        #endregion

        public uint ObjectID;
        public string URL;

        public UserCommandChangeURL(uint ObjectID, string URL)
        {
            this.ObjectID = ObjectID;
            this.URL = URL;
        }

        public UserCommandChangeURL(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
