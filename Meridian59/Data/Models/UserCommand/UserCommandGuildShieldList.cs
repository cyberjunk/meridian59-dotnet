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

namespace Meridian59.Data.Models
{
    [Serializable]
    public class UserCommandGuildShieldList : UserCommand
    {
        /// <summary>
        /// This command shares commandtype value with its request "UserCommandGuildShieldListReq".
        /// You have to decide based on message flowdirection, which one to parse.
        /// </summary>
        public override UserCommandType CommandType { get { return UserCommandType.GuildShields; } }

        #region IByteSerializable
        public override int ByteLength 
        { 
            get 
            {                
                return TypeSizes.BYTE + TypeSizes.SHORT + (ShieldResources.Length * TypeSizes.INT);
            }
        }    
    
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;
            cursor++;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(ShieldResources.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            foreach (ResourceIDBGF id in ShieldResources)            
                cursor += id.WriteTo(Buffer, cursor);
           
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

                ushort len = BitConverter.ToUInt16(Buffer, cursor);
                cursor += TypeSizes.SHORT;

                ShieldResources = new ResourceIDBGF[len];
                for (int i = 0; i < len; i++)
                {
                    ShieldResources[i] = new ResourceIDBGF(Buffer, cursor);
                    cursor += ShieldResources[i].ByteLength;
                }
            }

            return cursor - StartIndex;
        }        
        #endregion

        public ResourceIDBGF[] ShieldResources { get; set; }

        public UserCommandGuildShieldList(ResourceIDBGF[] ShieldResources)
        {
            this.ShieldResources = ShieldResources;
        }

        public UserCommandGuildShieldList(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
