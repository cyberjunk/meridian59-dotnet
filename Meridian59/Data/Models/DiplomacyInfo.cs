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
using System.ComponentModel;
using System.Collections.Generic;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;

namespace Meridian59.Data.Models
{
    [Serializable]
    public class DiplomacyInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable, IUpdatable<DiplomacyInfo>
    {
        #region Constants
        public const string PROPNAME_GUILDS = "Guilds";
        public const string PROPNAME_YOUDECLAREDALLYLIST = "YouDeclaredAllyList";
        public const string PROPNAME_YOUDECLAREDENEMYLIST = "YouDeclaredEnemyList";
        public const string PROPNAME_DECLAREDYOUALLYLIST = "DeclaredYouAllyList";
        public const string PROPNAME_DECLAREDYOUENEMYLIST = "DeclaredYouEnemyList";
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
                // guildlist
                int len = TypeSizes.SHORT;
                foreach(GuildEntry entry in guilds)
                    len += entry.ByteLength;

                // you decl. ally.
                len += TypeSizes.SHORT;
                foreach (ObjectID entry in youDeclaredAllyList)
                    len += entry.ByteLength;

                // you decl. enemy.
                len += TypeSizes.SHORT;
                foreach (ObjectID entry in youDeclaredEnemyList)
                    len += entry.ByteLength;

                // decl. you ally.
                len += TypeSizes.SHORT;
                foreach (ObjectID entry in declaredYouAllyList)
                    len += entry.ByteLength;

                // decl. you enemy.
                len += TypeSizes.SHORT;
                foreach (ObjectID entry in declaredYouEnemyList)
                    len += entry.ByteLength;

                return len;
            }
        }      
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(guilds.Count)), 0, Buffer, cursor, TypeSizes.SHORT);                 // GuildsListLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (GuildEntry entry in guilds)
                cursor += entry.WriteTo(Buffer, cursor);


            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(youDeclaredAllyList.Count)), 0, Buffer, cursor, TypeSizes.SHORT);    // YouDeclAllyListLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (ObjectID entry in youDeclaredAllyList)
                cursor += entry.WriteTo(Buffer, cursor);


            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(youDeclaredEnemyList.Count)), 0, Buffer, cursor, TypeSizes.SHORT);   // YouDeclEnemyListLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (ObjectID entry in youDeclaredEnemyList)
                cursor += entry.WriteTo(Buffer, cursor);


            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(declaredYouAllyList.Count)), 0, Buffer, cursor, TypeSizes.SHORT);    // DeclAllyListLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (ObjectID entry in declaredYouAllyList)
                cursor += entry.WriteTo(Buffer, cursor);


            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(declaredYouEnemyList.Count)), 0, Buffer, cursor, TypeSizes.SHORT);   // DeclEnemyListLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (ObjectID entry in declaredYouEnemyList)
                cursor += entry.WriteTo(Buffer, cursor);
            return ByteLength;
        }
        public int ReadFrom(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;
         
            ushort len = BitConverter.ToUInt16(Buffer, cursor);                 // GuildsLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            guilds.Clear();
            guilds.Capacity = len;
            for (ushort i = 0; i < len; i++)
            {
                GuildEntry obj = new GuildEntry(Buffer, cursor);                // GuildEntry (n bytes)
                guilds.Add(obj);
                cursor += obj.ByteLength;
            }

            len = BitConverter.ToUInt16(Buffer, cursor);                        // YouDeclAllyLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            youDeclaredAllyList.Clear();
            youDeclaredAllyList.Capacity = len;
            for (ushort i = 0; i < len; i++)
            {
                ObjectID obj = new ObjectID(Buffer, cursor);                    // ObjectID (4/8 bytes)
                youDeclaredAllyList.Add(obj);
                cursor += obj.ByteLength;                  
            }

            len = BitConverter.ToUInt16(Buffer, cursor);                        // YouDeclEnemyLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            youDeclaredEnemyList.Clear();
            youDeclaredEnemyList.Capacity = len;
            for (ushort i = 0; i < len; i++)
            {
                ObjectID obj = new ObjectID(Buffer, cursor);                    // ObjectID (4/8 bytes)
                youDeclaredEnemyList.Add(obj);
                cursor += obj.ByteLength;
            }  

            len = BitConverter.ToUInt16(Buffer, cursor);                        // DeclYouAllyLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            declaredYouAllyList.Clear();
            declaredYouAllyList.Capacity = len;
            for (ushort i = 0; i < len; i++)
            {
                ObjectID obj = new ObjectID(Buffer, cursor);                    // ObjectID (4/8 bytes)
                declaredYouAllyList.Add(obj);
                cursor += obj.ByteLength;
            }

            len = BitConverter.ToUInt16(Buffer, cursor);                        // DeclYouEnemyLEN  (2 bytes)
            cursor += TypeSizes.SHORT;

            declaredYouEnemyList.Clear();
            declaredYouEnemyList.Capacity = len;
            for (ushort i = 0; i < len; i++)
            {
                ObjectID obj = new ObjectID(Buffer, cursor);                    // ObjectID (4/8 bytes)
                declaredYouEnemyList.Add(obj);
                cursor += obj.ByteLength;
            }  
            
            return ByteLength;
        }
        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((ushort*)Buffer) = (ushort)guilds.Count;
            Buffer += TypeSizes.SHORT;
            
            foreach (GuildEntry entry in guilds)
                entry.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)youDeclaredAllyList.Count;
            Buffer += TypeSizes.SHORT;

            foreach (ObjectID entry in youDeclaredAllyList)
                entry.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)youDeclaredEnemyList.Count;
            Buffer += TypeSizes.SHORT;

            foreach (ObjectID entry in youDeclaredEnemyList)
                entry.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)declaredYouAllyList.Count;
            Buffer += TypeSizes.SHORT;

            foreach (ObjectID entry in declaredYouAllyList)
                entry.WriteTo(ref Buffer);

            *((ushort*)Buffer) = (ushort)declaredYouEnemyList.Count;
            Buffer += TypeSizes.SHORT;

            foreach (ObjectID entry in declaredYouEnemyList)
                entry.WriteTo(ref Buffer);
        }
        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            guilds.Clear();
            guilds.Capacity = len;
            for (ushort i = 0; i < len; i++)
                guilds.Add(new GuildEntry(ref Buffer));

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            youDeclaredAllyList.Clear();
            youDeclaredAllyList.Capacity = len;
            for (ushort i = 0; i < len; i++)         
                youDeclaredAllyList.Add(new ObjectID(ref Buffer));


            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            youDeclaredEnemyList.Clear();
            youDeclaredEnemyList.Capacity = len;
            for (ushort i = 0; i < len; i++)            
                youDeclaredEnemyList.Add(new ObjectID(ref Buffer));
            

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            declaredYouAllyList.Clear();
            declaredYouAllyList.Capacity = len;
            for (ushort i = 0; i < len; i++)            
                declaredYouAllyList.Add(new ObjectID(ref Buffer));             
            

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            declaredYouEnemyList.Clear();
            declaredYouEnemyList.Capacity = len;
            for (ushort i = 0; i < len; i++)           
                declaredYouEnemyList.Add(new ObjectID(ref Buffer)); 
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
        protected readonly BaseList<GuildEntry> guilds = new BaseList<GuildEntry>();
        protected readonly ObjectIDList<ObjectID> youDeclaredAllyList = new ObjectIDList<ObjectID>();
        protected readonly ObjectIDList<ObjectID> youDeclaredEnemyList = new ObjectIDList<ObjectID>();
        protected readonly ObjectIDList<ObjectID> declaredYouAllyList = new ObjectIDList<ObjectID>();
        protected readonly ObjectIDList<ObjectID> declaredYouEnemyList = new ObjectIDList<ObjectID>();
        #endregion

        #region Properties
        public BaseList<GuildEntry> Guilds
        {
            get { return guilds; }
        }
        public ObjectIDList<ObjectID> YouDeclaredAllyList
        {
            get { return youDeclaredAllyList; }
        }
        public ObjectIDList<ObjectID> YouDeclaredEnemyList
        {
            get { return youDeclaredEnemyList; }
        }
        public ObjectIDList<ObjectID> DeclaredYouAllyList
        {
            get { return declaredYouAllyList; }
        }
        public ObjectIDList<ObjectID> DeclaredYouEnemyList
        {
            get { return declaredYouEnemyList; }
        }
        #endregion

        #region Constructors
        public DiplomacyInfo()
        {
            Clear(false);
        }

        public DiplomacyInfo(IEnumerable<GuildEntry> Guilds,
            IEnumerable<ObjectID> YouDeclaredAllyList, IEnumerable<ObjectID> YouDeclaredEnemyList,
            IEnumerable<ObjectID> DeclaredYouAllyList, IEnumerable<ObjectID> DeclaredYouEnemyList)
        {
            guilds.AddRange(Guilds);
            youDeclaredAllyList.AddRange(YouDeclaredAllyList);
            youDeclaredEnemyList.AddRange(YouDeclaredEnemyList);
            declaredYouAllyList.AddRange(DeclaredYouAllyList);
            declaredYouEnemyList.AddRange(DeclaredYouEnemyList);
        }
        
        public DiplomacyInfo(byte[] RawData, int startIndex = 0)
        {
            ReadFrom(RawData, startIndex);
        }

        public unsafe DiplomacyInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Guilds.Clear();
                YouDeclaredAllyList.Clear();
                YouDeclaredEnemyList.Clear();
                DeclaredYouAllyList.Clear();
                DeclaredYouEnemyList.Clear();
            }
            else
            {
                guilds.Clear();
                youDeclaredAllyList.Clear();
                youDeclaredEnemyList.Clear();
                declaredYouAllyList.Clear();
                declaredYouEnemyList.Clear();
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(DiplomacyInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                // update states first to look them up in guilds
                YouDeclaredAllyList.Clear();
                foreach (ObjectID obj in Model.YouDeclaredAllyList)
                    YouDeclaredAllyList.Add(obj);

                YouDeclaredEnemyList.Clear();
                foreach (ObjectID obj in Model.YouDeclaredEnemyList)
                    YouDeclaredEnemyList.Add(obj);

                DeclaredYouAllyList.Clear();
                foreach (ObjectID obj in Model.DeclaredYouAllyList)
                    DeclaredYouAllyList.Add(obj);

                DeclaredYouEnemyList.Clear();
                foreach (ObjectID obj in Model.DeclaredYouEnemyList)
                    DeclaredYouEnemyList.Add(obj); 

                // then guilds
                Guilds.Clear();
                foreach (GuildEntry obj in Model.Guilds)
                    Guilds.Add(obj);
            
            }
            else
            {
                // update states first to look them up in guilds               
                youDeclaredAllyList.Clear();
                foreach (ObjectID obj in Model.YouDeclaredAllyList)
                    youDeclaredAllyList.Add(obj);

                youDeclaredEnemyList.Clear();
                foreach (ObjectID obj in Model.YouDeclaredEnemyList)
                    youDeclaredEnemyList.Add(obj);

                declaredYouAllyList.Clear();
                foreach (ObjectID obj in Model.DeclaredYouAllyList)
                    declaredYouAllyList.Add(obj);

                declaredYouEnemyList.Clear();
                foreach (ObjectID obj in Model.DeclaredYouEnemyList)
                    declaredYouEnemyList.Add(obj);

                // then guilds
                guilds.Clear();
                foreach (GuildEntry obj in Model.Guilds)
                    guilds.Add(obj);
            }
        }
        #endregion
    }
}
