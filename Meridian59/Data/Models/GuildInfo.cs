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
using System.ComponentModel;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Info about your own guild.
    /// </summary>
    [Serializable]
    public class GuildInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<GuildInfo>
    {
        #region Constants
        public const string PROPNAME_GUILDNAME = "GuildName";
        public const string PROPNAME_PASSWORDSETFLAG = "PasswordSetFlag";
        public const string PROPNAME_CHESTPASSWORD = "ChestPassword";
        public const string PROPNAME_FLAGS = "Flags";
        public const string PROPNAME_GUILDID = "GuildID";
        public const string PROPNAME_RANK1MALE = "Rank1Male";
        public const string PROPNAME_RANK1FEMALE = "Rank1Female";
        public const string PROPNAME_RANK2MALE = "Rank2Male";
        public const string PROPNAME_RANK2FEMALE = "Rank2Female";
        public const string PROPNAME_RANK3MALE = "Rank3Male";
        public const string PROPNAME_RANK3FEMALE = "Rank3Female";
        public const string PROPNAME_RANK4MALE = "Rank4Male";
        public const string PROPNAME_RANK4FEMALE = "Rank4Female";
        public const string PROPNAME_RANK5MALE = "Rank5Male";
        public const string PROPNAME_RANK5FEMALE = "Rank5Female";
        public const string PROPNAME_SUPPORTEDMEMBER = "SupportedMember";
        public const string PROPNAME_GUILDMEMBERS = "GuildMembers";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public int ByteLength { 
            get { 
                // GuildnameLEN + Guildname + PasswordFLAG
                int len = TypeSizes.SHORT + guildName.Length + 1;
                
                // if password set: + PWLEN + PW
                if (passwordSetFlag == 0x01) 
                    len += TypeSizes.SHORT + chestPassword.Length;

                // guildflags + guildid
                len += TypeSizes.INT + guildID.ByteLength;

                len += TypeSizes.SHORT + rank1Male.Length + TypeSizes.SHORT + rank1Female.Length;
                len += TypeSizes.SHORT + rank2Male.Length + TypeSizes.SHORT + rank2Female.Length;
                len += TypeSizes.SHORT + rank3Male.Length + TypeSizes.SHORT + rank3Female.Length;
                len += TypeSizes.SHORT + rank4Male.Length + TypeSizes.SHORT + rank4Female.Length;
                len += TypeSizes.SHORT + rank5Male.Length + TypeSizes.SHORT + rank5Female.Length;

                // supportedguildmember + guildmemberslistlen
                len += supportedMember.ByteLength + TypeSizes.SHORT;

                foreach(GuildMemberEntry entry in guildMembers)
                    len += entry.ByteLength;

                return len;
            }
        }      
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(guildName.Length)), 0, Buffer, cursor, TypeSizes.SHORT);          // GuildNameLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(guildName), 0, Buffer, cursor, guildName.Length);                              // GuildName (n bytes)
            cursor += guildName.Length;

            Buffer[cursor] = passwordSetFlag;                                                                                   // PasswordSetFlag (1 byte)
            cursor++;

            if (passwordSetFlag == 0x01)
            {
                Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(chestPassword.Length)), 0, Buffer, cursor, TypeSizes.SHORT);  // ChestPasswordLEN (2 bytes)
                cursor += TypeSizes.SHORT;

                Array.Copy(Encoding.Default.GetBytes(chestPassword), 0, Buffer, cursor, chestPassword.Length);                  // ChestPassword (n bytes)
                cursor += chestPassword.Length;
            }

            Array.Copy(BitConverter.GetBytes(flags.Flags), 0, Buffer, cursor, TypeSizes.INT);                                   // Flags (4 bytes)
            cursor += TypeSizes.INT;

            guildID.WriteTo(Buffer, cursor);                                                                                    // GuildID (4/8 bytes)
            cursor += guildID.ByteLength;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank1Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);          // Rank1MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank1Male), 0, Buffer, cursor, rank1Male.Length);                              // Rank1Male (n bytes)
            cursor += rank1Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank1Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);        // Rank1FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank1Female), 0, Buffer, cursor, rank1Female.Length);                          // Rank1Female (n bytes)
            cursor += rank1Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank2Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);          // Rank2MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank2Male), 0, Buffer, cursor, rank2Male.Length);                              // Rank2Male (n bytes)
            cursor += rank2Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank2Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);        // Rank2FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank2Female), 0, Buffer, cursor, rank2Female.Length);                          // Rank2Female (n bytes)
            cursor += rank2Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank3Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);          // Rank3MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank3Male), 0, Buffer, cursor, rank3Male.Length);                              // Rank3Male (n bytes)
            cursor += rank3Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank3Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);        // Rank3FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank3Female), 0, Buffer, cursor, rank3Female.Length);                          // Rank3Female (n bytes)
            cursor += rank3Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank4Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);          // Rank4MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank4Male), 0, Buffer, cursor, rank4Male.Length);                              // Rank4Male (n bytes)
            cursor += rank4Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank4Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);        // Rank4FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank4Female), 0, Buffer, cursor, rank4Female.Length);                          // Rank4Female (n bytes)
            cursor += rank4Female.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank5Male.Length)), 0, Buffer, cursor, TypeSizes.SHORT);          // Rank5MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank5Male), 0, Buffer, cursor, rank5Male.Length);                              // Rank5Male (n bytes)
            cursor += rank5Male.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(rank5Female.Length)), 0, Buffer, cursor, TypeSizes.SHORT);        // Rank5FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(rank5Female), 0, Buffer, cursor, rank5Female.Length);                          // Rank5Female (n bytes)
            cursor += rank5Female.Length;

            cursor += supportedMember.WriteTo(Buffer, cursor);                                                                  // SupportedMember (4/8 bytes)

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(guildMembers.Count)), 0, Buffer, cursor, TypeSizes.SHORT);       // GuildMembersLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (GuildMemberEntry entry in guildMembers)
                cursor += entry.WriteTo(Buffer, cursor);

            return ByteLength;
        }
        public int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);                 // NameLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            guildName = Encoding.Default.GetString(Buffer, cursor, len);        // Name     (n bytes)
            cursor += len;
             
            passwordSetFlag = Buffer[cursor];                                   // PasswordSetFlag (1 byte)
            cursor++;

            if (passwordSetFlag == 0x01)
            {
                len = BitConverter.ToUInt16(Buffer, cursor);                     // PasswordLEN  (2 bytes)
                cursor += TypeSizes.SHORT;
                    
                chestPassword = Encoding.Default.GetString(Buffer, cursor, len); // Password     (n bytes)
                cursor += len;             
            }

            flags = new GuildFlags(BitConverter.ToUInt32(Buffer, cursor));       // Flags (4 bytes)
            cursor += TypeSizes.INT;

            guildID = new ObjectID(Buffer, cursor);                             // ID       (4/8 bytes)
            cursor += guildID.ByteLength;
             
            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank1MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank1Male = Encoding.Default.GetString(Buffer, cursor, len);        // Rank1Male    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank1FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank1Female = Encoding.Default.GetString(Buffer, cursor, len);      // Rank1Female    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank2MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank2Male = Encoding.Default.GetString(Buffer, cursor, len);        // Rank2Male    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank2FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank2Female = Encoding.Default.GetString(Buffer, cursor, len);      // Rank2Female    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank3MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank3Male = Encoding.Default.GetString(Buffer, cursor, len);        // Rank3Male    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank3FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank3Female = Encoding.Default.GetString(Buffer, cursor, len);      // Rank3Female    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank4MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank4Male = Encoding.Default.GetString(Buffer, cursor, len);        // Rank4Male    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank4FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank4Female = Encoding.Default.GetString(Buffer, cursor, len);      // Rank4Female    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank5MaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank5Male = Encoding.Default.GetString(Buffer, cursor, len);        // Rank5Male    (n bytes)
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // Rank5FemaleLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            rank5Female = Encoding.Default.GetString(Buffer, cursor, len);      // Rank5Female    (n bytes)
            cursor += len;

            supportedMember = new ObjectID(Buffer, cursor);                     // SupportedMember(4/8 bytes)
            cursor += supportedMember.ByteLength;

            len = BitConverter.ToUInt16(Buffer, cursor);                        // ListLEN
            cursor += TypeSizes.SHORT;

            guildMembers = new GuildMemberList(len);
            for (ushort i = 0; i < len; i++)
            {
                GuildMemberEntry obj = new GuildMemberEntry(Buffer, cursor);    // GuildMemberEntry (n bytes)
                guildMembers.Add(obj); 
                cursor += obj.ByteLength;
            }
            
            return ByteLength;
        }
        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            guildName = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            passwordSetFlag = Buffer[0];
            Buffer++;

            if (passwordSetFlag == 0x01)
            {
                len = *((ushort*)Buffer);
                Buffer += TypeSizes.SHORT;

                chestPassword = new string((sbyte*)Buffer, 0, len);
                Buffer += len;
            }

            flags = new GuildFlags(*((uint*)Buffer));
            Buffer += TypeSizes.INT;

            guildID = new ObjectID(ref Buffer);
            
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank1Male = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank1Female = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank2Male = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank2Female = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank3Male = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank3Female = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank4Male = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank4Female = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank5Male = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            rank5Female = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            supportedMember = new ObjectID(ref Buffer);
            
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            guildMembers = new GuildMemberList(len);
            for (ushort i = 0; i < len; i++)
                guildMembers.Add(new GuildMemberEntry(ref Buffer));                                   
        }
        public unsafe void WriteTo(ref byte* Buffer)
        {
            int a, b; bool c;

            fixed (char* pString = guildName)
            {
                ushort len = (ushort)guildName.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;
               
                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            Buffer[0] = passwordSetFlag;
            Buffer++;

            if (passwordSetFlag == 0x01)
            {
                fixed (char* pString = chestPassword)
                {
                    ushort len = (ushort)chestPassword.Length;

                    *((ushort*)Buffer) = len;
                    Buffer += TypeSizes.SHORT;
                    
                    Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                    Buffer += len;
                }
            }

            *((uint*)Buffer) = flags.Flags;
            Buffer += TypeSizes.INT;

            guildID.WriteTo(ref Buffer);

            fixed (char* pString = rank1Male)
            {
                ushort len = (ushort)rank1Male.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank1Female)
            {
                ushort len = (ushort)rank1Female.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank2Male)
            {
                ushort len = (ushort)rank2Male.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank2Female)
            {
                ushort len = (ushort)rank2Female.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank3Male)
            {
                ushort len = (ushort)rank3Male.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank3Female)
            {
                ushort len = (ushort)rank3Female.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank4Male)
            {
                ushort len = (ushort)rank4Male.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank4Female)
            {
                ushort len = (ushort)rank4Female.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank5Male)
            {
                ushort len = (ushort)rank5Male.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = rank5Female)
            {
                ushort len = (ushort)rank5Female.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            supportedMember.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)guildMembers.Count;           
            foreach (GuildMemberEntry entry in guildMembers)
                entry.WriteTo(ref Buffer);
        }
        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
        }
        #endregion

        #region Fields
        protected string guildName;
        protected byte passwordSetFlag;
        protected string chestPassword;
        protected GuildFlags flags;
        protected ObjectID guildID;
        protected string rank1Male;
        protected string rank1Female;
        protected string rank2Male;
        protected string rank2Female;
        protected string rank3Male;
        protected string rank3Female;
        protected string rank4Male;
        protected string rank4Female;
        protected string rank5Male;
        protected string rank5Female;
        protected ObjectID supportedMember;
        protected GuildMemberList guildMembers;
        protected bool isVisible;
        #endregion

        #region Properties
        
        /// <summary>
        /// Name of the guild
        /// </summary>
        public string GuildName
        {
            get { return guildName; }
            set
            {
                if (guildName != value)
                {
                    guildName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDNAME));
                }
            }
        }
        
        /// <summary>
        /// If chestpassword is set or not
        /// </summary>
        public byte PasswordSetFlag
        {
            get { return passwordSetFlag; }
            set
            {
                if (passwordSetFlag != value)
                {
                    passwordSetFlag = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PASSWORDSETFLAG));
                }
            }
        }
        
        /// <summary>
        /// Chest password
        /// </summary>
        public string ChestPassword
        {
            get { return chestPassword; }
            set
            {
                if (chestPassword != value)
                {
                    chestPassword = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CHESTPASSWORD));
                }
            }
        }
        
        /// <summary>
        /// Flags
        /// </summary>
        public GuildFlags Flags
        {
            get { return flags; }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FLAGS));
                }
            }
        }
        
        /// <summary>
        /// Unique ID of the guild on the server.
        /// </summary>
        public ObjectID GuildID
        {
            get { return guildID; }
            set
            {
                if (guildID != value)
                {
                    guildID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDID));
                }
            }
        }
        
        /// <summary>
        /// Male rank 1 name
        /// </summary>
        public string Rank1Male
        {
            get { return rank1Male; }
            set
            {
                if (rank1Male != value)
                {
                    rank1Male = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK1MALE));
                }
            }
        }

        /// <summary>
        /// Female rank 1 name
        /// </summary>
        public string Rank1Female
        {
            get { return rank1Female; }
            set
            {
                if (rank1Female != value)
                {
                    rank1Female = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK1FEMALE));
                }
            }
        }

        /// <summary>
        /// Male rank 2 name
        /// </summary>
        public string Rank2Male
        {
            get { return rank2Male; }
            set
            {
                if (rank2Male != value)
                {
                    rank2Male = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK2MALE));
                }
            }
        }

        /// <summary>
        /// Female rank 2 name
        /// </summary>
        public string Rank2Female
        {
            get { return rank2Female; }
            set
            {
                if (rank2Female != value)
                {
                    rank2Female = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK2FEMALE));
                }
            }
        }

        /// <summary>
        /// Male rank 3 name
        /// </summary>
        public string Rank3Male
        {
            get { return rank3Male; }
            set
            {
                if (rank3Male != value)
                {
                    rank3Male = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK3MALE));
                }
            }
        }

        /// <summary>
        /// Female rank 3 name
        /// </summary>
        public string Rank3Female
        {
            get { return rank3Female; }
            set
            {
                if (rank3Female != value)
                {
                    rank3Female = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK3FEMALE));
                }
            }
        }

        /// <summary>
        /// Male rank 4 name
        /// </summary>
        public string Rank4Male
        {
            get { return rank4Male; }
            set
            {
                if (rank4Male != value)
                {
                    rank4Male = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK4MALE));
                }
            }
        }

        /// <summary>
        /// Female rank 4 name
        /// </summary>
        public string Rank4Female
        {
            get { return rank4Female; }
            set
            {
                if (rank4Female != value)
                {
                    rank4Female = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK4FEMALE));
                }
            }
        }

        /// <summary>
        /// Male rank 5 name
        /// </summary>
        public string Rank5Male
        {
            get { return rank5Male; }
            set
            {
                if (rank5Male != value)
                {
                    rank5Male = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK5MALE));
                }
            }
        }

        /// <summary>
        /// Female rank 5 name
        /// </summary>
        public string Rank5Female
        {
            get { return rank5Female; }
            set
            {
                if (rank5Female != value)
                {
                    rank5Female = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_RANK5FEMALE));
                }
            }
        }
        
        /// <summary>
        /// The member you currently support for leadership
        /// </summary>
        public ObjectID SupportedMember
        {
            get { return supportedMember; }
            set
            {
                if (supportedMember != value)
                {
                    supportedMember = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SUPPORTEDMEMBER));
                }
            }
        }
        
        /// <summary>
        /// List of guild members
        /// </summary>
        public GuildMemberList GuildMembers
        {
            get { return guildMembers; }
            set
            {
                if (guildMembers != value)
                {
                    guildMembers = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDMEMBERS));
                }
            }
        }

        /// <summary>
        /// Whether the corresponding view attached to this model
        /// should be visible or not.
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISVISIBLE));
                }
            }
        }
        
        #endregion

        #region Constructors
        public GuildInfo()
        {
            guildMembers = new GuildMemberList(50);
            Clear(false);
        }

        public GuildInfo(string GuildName, byte PasswordSetFlag, string ChestPassword, GuildFlags Flags, ObjectID GuildID,
            string Rank1Male, string Rank1Female, string Rank2Male, string Rank2Female, string Rank3Male, string Rank3Female, 
            string Rank4Male, string Rank4Female, string Rank5Male, string Rank5Female,
            ObjectID SupportedMember, GuildMemberList GuildMembers)
        {
            this.guildName = GuildName;
            this.passwordSetFlag = PasswordSetFlag;          
            this.chestPassword = ChestPassword;
            this.flags = Flags;
            this.guildID = GuildID;
            this.rank1Male = Rank1Male;
            this.rank1Female = Rank1Female;
            this.rank2Male = Rank2Male;
            this.rank2Female = Rank2Female;
            this.rank3Male = Rank3Male;
            this.rank3Female = Rank3Female;
            this.rank4Male = Rank4Male;
            this.rank4Female = Rank4Female;
            this.rank5Male = Rank5Male;
            this.rank5Female = Rank5Female;
            this.supportedMember = SupportedMember;
            this.guildMembers = GuildMembers;            
        }
        
        public GuildInfo(byte[] RawData, int startIndex = 0)
        {
            guildMembers = new GuildMemberList(50);
            ReadFrom(RawData, startIndex);
        }

        public unsafe GuildInfo(ref byte* Buffer)
        {
            guildMembers = new GuildMemberList(50);
            ReadFrom(ref Buffer);
        }
        #endregion     

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                GuildName = String.Empty;
                PasswordSetFlag = 0;
                ChestPassword = String.Empty;
                Flags = new GuildFlags();
                GuildID = new ObjectID(0);
                Rank1Male = String.Empty;
                Rank1Female = String.Empty;
                Rank2Male = String.Empty;
                Rank2Female = String.Empty;
                Rank3Male = String.Empty;
                Rank3Female = String.Empty;
                Rank4Male = String.Empty;
                Rank4Female = String.Empty;
                Rank5Male = String.Empty;
                Rank5Female = String.Empty;
                SupportedMember = new ObjectID(0);
                GuildMembers.Clear();
                IsVisible = false;
            }
            else
            {
                guildName = String.Empty;
                passwordSetFlag = 0;
                chestPassword = String.Empty;
                flags = new GuildFlags();
                guildID = new ObjectID(0);
                rank1Male = String.Empty;
                rank1Female = String.Empty;
                rank2Male = String.Empty;
                rank2Female = String.Empty;
                rank3Male = String.Empty;
                rank3Female = String.Empty;
                rank4Male = String.Empty;
                rank4Female = String.Empty;
                rank5Male = String.Empty;
                rank5Female = String.Empty;
                supportedMember = new ObjectID(0);
                guildMembers.Clear();
                isVisible = false;
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(GuildInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                GuildName = Model.GuildName;
                PasswordSetFlag = Model.PasswordSetFlag;
                ChestPassword = Model.ChestPassword;
                Flags = Model.Flags;
                GuildID = Model.GuildID;
                Rank1Male = Model.Rank1Male;
                Rank1Female = Model.Rank1Female;
                Rank2Male = Model.Rank2Male;
                Rank2Female = Model.Rank2Female;
                Rank3Male = Model.Rank3Male;
                Rank3Female = Model.Rank3Female;
                Rank4Male = Model.Rank4Male;
                Rank4Female = Model.Rank4Female;
                Rank5Male = Model.Rank5Male;
                Rank5Female = Model.Rank5Female;
                SupportedMember = Model.SupportedMember;

                GuildMembers.Clear();
                foreach (GuildMemberEntry obj in Model.GuildMembers)
                    GuildMembers.Add(obj);

                // not isvisible
            }
            else
            {
                guildName = Model.GuildName;
                passwordSetFlag = Model.PasswordSetFlag;
                chestPassword = Model.ChestPassword;
                flags = Model.Flags;
                guildID = Model.GuildID;
                rank1Male = Model.Rank1Male;
                rank1Female = Model.Rank1Female;
                rank2Male = Model.Rank2Male;
                rank2Female = Model.Rank2Female;
                rank3Male = Model.Rank3Male;
                rank3Female = Model.Rank3Female;
                rank4Male = Model.Rank4Male;
                rank4Female = Model.Rank4Female;
                rank5Male = Model.Rank5Male;
                rank5Female = Model.Rank5Female;
                supportedMember = Model.SupportedMember;
                
                guildMembers.Clear();
                foreach (GuildMemberEntry obj in Model.GuildMembers)
                    guildMembers.Add(obj); 

                // not isvisible
            }
        }
        #endregion
    }
}
