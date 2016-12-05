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
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Sent from client to server to create a guild.
    /// </summary>
    [Serializable]
    public class UserCommandGuildCreate : UserCommand
    {
        public override UserCommandType CommandType { get { return UserCommandType.GuildCreate; } }

        #region IByteSerializable implementation
        public override int ByteLength { 
            get { 
                // CommandType + GuildNameLEN + GuildName + ... + SecretGuild
                return TypeSizes.BYTE + TypeSizes.SHORT + GuildName.Length
                    + TypeSizes.SHORT + Rank1Male.Length
                    + TypeSizes.SHORT + Rank1Female.Length
                    + TypeSizes.SHORT + Rank2Male.Length
                    + TypeSizes.SHORT + Rank2Female.Length
                    + TypeSizes.SHORT + Rank3Male.Length
                    + TypeSizes.SHORT + Rank3Female.Length
                    + TypeSizes.SHORT + Rank4Male.Length
                    + TypeSizes.SHORT + Rank4Female.Length
                    + TypeSizes.SHORT + Rank5Male.Length
                    + TypeSizes.SHORT + Rank5Female.Length
                    + TypeSizes.BYTE;
            }
        }
        
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)CommandType;                                                                         // Type     (1 byte)
            cursor++;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(GuildName.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // GuildNameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(GuildName), 0, Buffer, cursor, GuildName.Length);                      // GuildName (n bytes)
            cursor += GuildName.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank1Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // Rank1MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank1Male), 0, Buffer, cursor, Rank1Male.Length);                      // Rank1Male (n bytes)
            cursor += Rank1Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank1Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);// Rank1FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank1Female), 0, Buffer, cursor, Rank1Female.Length);                  // Rank1Female (n bytes)
            cursor += Rank1Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank2Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // Rank2MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank2Male), 0, Buffer, cursor, Rank2Male.Length);                      // Rank2Male (n bytes)
            cursor += Rank2Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank2Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);// Rank2FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank2Female), 0, Buffer, cursor, Rank2Female.Length);                  // Rank2Female (n bytes)
            cursor += Rank2Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank3Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // Rank3MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank3Male), 0, Buffer, cursor, Rank3Male.Length);                      // Rank3Male (n bytes)
            cursor += Rank3Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank3Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);// Rank3FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank3Female), 0, Buffer, cursor, Rank3Female.Length);                  // Rank3Female (n bytes)
            cursor += Rank3Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank4Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // Rank4MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank4Male), 0, Buffer, cursor, Rank4Male.Length);                      // Rank4Male (n bytes)
            cursor += Rank4Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank4Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);// Rank4FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank4Female), 0, Buffer, cursor, Rank4Female.Length);                  // Rank4Female (n bytes)
            cursor += Rank4Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank5Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // Rank5MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank5Male), 0, Buffer, cursor, Rank5Male.Length);                      // Rank5Male (n bytes)
            cursor += Rank5Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(Rank5Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);// Rank5FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(Rank5Female), 0, Buffer, cursor, Rank5Female.Length);                  // Rank5Female (n bytes)
            cursor += Rank5Female.Length;

            Buffer[cursor] = Convert.ToByte(SecretGuild);                                                               // SecretGuild (1 byte)
            cursor++;

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if ((UserCommandType)Buffer[cursor] != CommandType)
                throw new Exception(ERRORWRONGTYPEBYTE);
            else
            {
                cursor++;                                                           // Type     (1 byte)
                
                ushort len = BitConverter.ToUInt16(Buffer, cursor);                 // GuildNameLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                GuildName = Util.Encoding.GetString(Buffer, cursor, len);        // GuildName    (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank1MaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank1Male = Util.Encoding.GetString(Buffer, cursor, len);        // Rank1Male    (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank1FemaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank1Female = Util.Encoding.GetString(Buffer, cursor, len);      // Rank1Female    (n bytes)
                cursor += len;
                
                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank2MaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank2Male = Util.Encoding.GetString(Buffer, cursor, len);        // Rank2Male    (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank2FemaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank2Female = Util.Encoding.GetString(Buffer, cursor, len);      // Rank2Female    (n bytes)
                cursor += len;
                
                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank3MaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank3Male = Util.Encoding.GetString(Buffer, cursor, len);        // Rank3Male    (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank3FemaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank3Female = Util.Encoding.GetString(Buffer, cursor, len);      // Rank3Female    (n bytes)
                cursor += len;
              
                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank4MaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank4Male = Util.Encoding.GetString(Buffer, cursor, len);        // Rank4Male    (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank4FemaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank4Female = Util.Encoding.GetString(Buffer, cursor, len);      // Rank4Female    (n bytes)
                cursor += len;
               
                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank5MaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank5Male = Util.Encoding.GetString(Buffer, cursor, len);        // Rank5Male    (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank5FemaleLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Rank5Female = Util.Encoding.GetString(Buffer, cursor, len);      // Rank5Female    (n bytes)
                cursor += len;

                SecretGuild = Convert.ToBoolean(Buffer[cursor]);                    // SecretGuild    (1 byte)
                cursor++;
            }

            return cursor - StartIndex;
        }      
        #endregion

        public string GuildName { get; set; }
        public string Rank1Male { get; set; }
        public string Rank2Male { get; set; }
        public string Rank3Male { get; set; }
        public string Rank4Male { get; set; }
        public string Rank5Male { get; set; }
        public string Rank1Female { get; set; }
        public string Rank2Female { get; set; }
        public string Rank3Female { get; set; }
        public string Rank4Female { get; set; }
        public string Rank5Female { get; set; }
        public bool SecretGuild { get; set; }

        public UserCommandGuildCreate(
            string GuildName,
            string Rank1Male, string Rank2Male, string Rank3Male, string Rank4Male, string Rank5Male, 
            string Rank1Female, string Rank2Female, string Rank3Female, string Rank4Female, string Rank5Female, 
            bool SecretGuild)
        {
            this.GuildName = GuildName;
            this.Rank1Male = Rank1Male;
            this.Rank2Male = Rank2Male;
            this.Rank3Male = Rank3Male;
            this.Rank4Male = Rank4Male;
            this.Rank5Male = Rank5Male;
            this.Rank1Female = Rank1Female;
            this.Rank2Female = Rank2Female;
            this.Rank3Female = Rank3Female;
            this.Rank4Female = Rank4Female;
            this.Rank5Female = Rank5Female;
            this.SecretGuild = SecretGuild;
        }

        public UserCommandGuildCreate(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
