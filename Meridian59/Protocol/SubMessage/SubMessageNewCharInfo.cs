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
using Meridian59.Data.Models;
using Meridian59.Protocol.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Protocol.SubMessage
{
    [Serializable]
    public class SubMessageNewCharInfo : SubMessage
    {
        public override byte SubMessageType { get { return (byte)MessageTypeGameMode.NewCharInfo; } }

        #region IByteSerializable implementation
        public override int ByteLength { 
            get { 
                // CommandType + ID + NameLEN + Name + DescLEN + Description + Gender 
                int length = TypeSizes.BYTE + AvatarID.ByteLength + 
                    TypeSizes.SHORT + AvatarName.Length + TypeSizes.SHORT + AvatarDescription.Length + TypeSizes.BYTE;

                // ResourceIDsLEN + ResourceIDs + HairColor + SkinColor 
                length += TypeSizes.SHORT + (ResourceIDs.Length * TypeSizes.INT) + TypeSizes.BYTE + TypeSizes.BYTE;
                
                // AttributesLEN + Attributes + SpellsLEN + Spells + SkillsLEN + Skills
                length += TypeSizes.SHORT + (AvatarAttributesValues.Length * TypeSizes.INT) + 
                    TypeSizes.SHORT + (SpellIDs.Length * TypeSizes.INT) +
                    TypeSizes.SHORT + (SkillIDs.Length * TypeSizes.INT);

                return length;
            }
        }
   
        public override int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
            
            Buffer[cursor] = (byte)SubMessageType;                                                                         // Type     (1 byte)
            cursor++;

            cursor += AvatarID.WriteTo(Buffer, cursor);                                                                 // AvatarID (4/8 bytes)

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(AvatarName.Length)), 0, Buffer, cursor, TypeSizes.SHORT); // AvatarNameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(AvatarName), 0, Buffer, cursor, AvatarName.Length);                    // AvatarName (n bytes)
            cursor += AvatarName.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(AvatarDescription.Length)), 0, Buffer, cursor, TypeSizes.SHORT);    // AvatarDescLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(AvatarDescription), 0, Buffer, cursor, AvatarDescription.Length);      // AvatarDescription (n bytes)
            cursor += AvatarDescription.Length;

            Buffer[cursor] = Gender;                                                                                    // Gender (1 byte)
            cursor++;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(ResourceIDs.Length)), 0, Buffer, cursor, TypeSizes.SHORT);// ResourceIDsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            for (int i = 0; i < ResourceIDs.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(ResourceIDs[i]), 0, Buffer, cursor, TypeSizes.INT);                    // ResourceIDs (n bytes)
                cursor += TypeSizes.INT;
            }

            Buffer[cursor] = HairColor;                                                                                 // HairColor (1 byte)
            cursor++;

            Buffer[cursor] = SkinColor;                                                                                 // SkinColor (1 byte)
            cursor++;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(AvatarAttributesValues.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // AttributesLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            for (int i = 0; i < AvatarAttributesValues.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(AvatarAttributesValues[i]), 0, Buffer, cursor, TypeSizes.INT);         // Attributes (n bytes)
                cursor += TypeSizes.INT;
            }

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(SpellIDs.Length)), 0, Buffer, cursor, TypeSizes.SHORT);   // SpellsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            for (int i = 0; i < SpellIDs.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(SpellIDs[i]), 0, Buffer, cursor, TypeSizes.INT);                       // Spells (n bytes)
                cursor += TypeSizes.INT;
            }

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(SkillIDs.Length)), 0, Buffer, cursor, TypeSizes.SHORT);   // SkillsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            for (int i = 0; i < SkillIDs.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(SkillIDs[i]), 0, Buffer, cursor, TypeSizes.INT);                       // Skills (n bytes)
                cursor += TypeSizes.INT;
            }

            return cursor - StartIndex;
        }

        public override int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            if (Buffer[cursor] != SubMessageType)
                throw new Exception("Wrong 1.Byte (type) for AvatarCreateRequestAvatarCreation");
            else
            {
                cursor++;                                                               // Type     (1 byte)  

                AvatarID = new ObjectID(Buffer, cursor);                                // AvatarID (4/8 bytes)
                cursor += AvatarID.ByteLength;

                ushort len = BitConverter.ToUInt16(Buffer, cursor);                     // NameLEN  (2 bytes)
                cursor += TypeSizes.SHORT;

                AvatarName = Encoding.Default.GetString(Buffer, cursor, len);           // Name     (n bytes)
                cursor += len;

                len = BitConverter.ToUInt16(Buffer, cursor);                            // DescLEN  (2 bytes)
                cursor += TypeSizes.SHORT;

                AvatarDescription = Encoding.Default.GetString(Buffer, cursor, len);    // Name     (n bytes)
                cursor += len;

                Gender = Buffer[cursor];                                                // Gender   (1 byte)
                cursor++;

                len = BitConverter.ToUInt16(Buffer, cursor);                            // ResourceIDsLEN  (2 bytes)
                cursor += TypeSizes.SHORT;

                ResourceIDs = new uint[len];                                            // ResourceIDs (n bytes)
                for (int i = 0; i < len; i++)
                {
                    ResourceIDs[i] = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                }

                HairColor = Buffer[cursor];                                             // HairColor (1 byte)
                cursor++;

                SkinColor = Buffer[cursor];                                             // SkinColor (1 byte)
                cursor++;

                len = BitConverter.ToUInt16(Buffer, cursor);                            // AttributesLEN  (2 bytes)
                cursor += TypeSizes.SHORT;

                AvatarAttributesValues = new uint[len];                                 // Attributes (n bytes)
                for (int i = 0; i < len; i++)
                {
                    AvatarAttributesValues[i] = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                }

                len = BitConverter.ToUInt16(Buffer, cursor);                            // SpellsLEN  (2 bytes)
                cursor += TypeSizes.SHORT;

                SpellIDs = new uint[len];                                               // Spells (n bytes)
                for (int i = 0; i < len; i++)
                {
                    SpellIDs[i] = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                }

                len = BitConverter.ToUInt16(Buffer, cursor);                            // SkillsLEN  (2 bytes)
                cursor += TypeSizes.SHORT;

                SkillIDs = new uint[len];                                               // Skills (n bytes)
                for (int i = 0; i < len; i++)
                {
                    SkillIDs[i] = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                }
            }

            return cursor - StartIndex;
        }     
        #endregion

        public ObjectID AvatarID { get; set; }
        public string AvatarName { get; set; }
        public string AvatarDescription { get; set; }
        public byte Gender { get; set; }
        public uint[] ResourceIDs { get; set; }
        public byte HairColor { get; set; }
        public byte SkinColor { get; set; }
        public uint[] AvatarAttributesValues { get; set; }
        public uint[] SpellIDs { get; set; }
        public uint[] SkillIDs { get; set; }

        public SubMessageNewCharInfo(ObjectID AvatarID, string AvatarName, string AvatarDescription, 
            byte Gender, uint[] ResourceIDs, byte HairColor, byte SkinColor, uint[] Attributes, uint[] Spells, uint[] Skills)
        {
            this.AvatarID = AvatarID;
            this.AvatarName = AvatarName;
            this.AvatarDescription = AvatarDescription;
            this.Gender = Gender;
            this.ResourceIDs = ResourceIDs;
            this.HairColor = HairColor;
            this.SkinColor = SkinColor;
            this.AvatarAttributesValues = Attributes;
            this.SpellIDs = Spells;
            this.SkillIDs = Skills;
        }

        public SubMessageNewCharInfo(byte[] Buffer, int StartIndex = 0)
        {           
            ReadFrom(Buffer, StartIndex);
        }
    }
}
