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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Data.Lists;
using Meridian59.Common;
using Meridian59.Common.Enums;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// A set of basic information about a newsglobe object
    /// </summary>
    [Serializable]
    public class NewsGroup : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<NewsGroup>, IStringResolvable
    {
        #region Constants
        public const string PROPNAME_NEWSGLOBEID = "NewsGlobeID";
        public const string PROPNAME_ACCESSTYPE = "AccessType";
        public const string PROPNAME_NEWSGLOBEOBJECT = "NewsGlobeObject";
        public const string PROPNAME_HEADLINERESOURCEID = "HeadlineResourceID";
        public const string PROPNAME_HEADLINE = "Headline";
        public const string PROPNAME_ARTICLES = "Articles";
        public const string PROPNAME_ISVISIBLE = "IsVisible";
        public const string PROPNAME_TEXT = "Text";
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region IByteSerializable
        public int ByteLength
        {
            get
            {
                return TypeSizes.SHORT + TypeSizes.BYTE + NewsGlobeObject.ByteLength + TypeSizes.INT;
            }
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
           
            NewsGlobeID = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            AccessType = Buffer[cursor];
            cursor++;

            NewsGlobeObject = new ObjectBase(true, Buffer, cursor);
            cursor += NewsGlobeObject.ByteLength;

            HeadlineResourceID = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(NewsGlobeID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = AccessType;
            cursor++;

            cursor += NewsGlobeObject.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(HeadlineResourceID), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            return cursor - StartIndex;
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
        protected ushort newsGlobeID;
        protected byte accessType;
        protected ObjectBase newsGlobeObject;
        protected uint headlineResourceID;
        protected string headline;
        protected ArticleHeadList articles;
        protected bool isVisible;
        protected string text;
        #endregion

        #region Properties
        public ushort NewsGlobeID
        {
            get
            {
                return newsGlobeID;
            }
            set
            {
                if (newsGlobeID != value)
                {
                    newsGlobeID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NEWSGLOBEID));
                }
            }
        }

        public byte AccessType
        {
            get
            {
                return accessType;
            }
            set
            {
                if (accessType != value)
                {
                    accessType = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ACCESSTYPE));
                }
            }
        }

        public ObjectBase NewsGlobeObject
        {
            get
            {
                return newsGlobeObject;
            }
            set
            {
                if (newsGlobeObject != value)
                {
                    newsGlobeObject = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_NEWSGLOBEOBJECT));
                }
            }
        }

        public uint HeadlineResourceID
        {
            get
            {
                return headlineResourceID;
            }
            set
            {
                if (headlineResourceID != value)
                {
                    headlineResourceID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEADLINERESOURCEID));
                }
            }
        }

        public string Headline
        {
            get
            {
                return headline;
            }
            set
            {
                if (headline != value)
                {
                    headline = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEADLINE));
                }
            }
        }

        public ArticleHeadList Articles
        {
            get
            {
                return articles;
            }
            set
            {
                if (articles != value)
                {
                    articles = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ARTICLES));
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISVISIBLE));
                }
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TEXT));
                }
            }
        }
        #endregion

        #region Constructors
        public NewsGroup()
        {
            // articles are not included and are added later to this model
            articles = new ArticleHeadList();
            
            Clear(false);
        }

        public NewsGroup(ushort NewsGlobeID, byte AccessType, ObjectBase NewsGlobeObject, uint HeadlineResourceID, string Headline)
        {
            articles = new ArticleHeadList();

            newsGlobeID = NewsGlobeID;
            accessType = AccessType;
            newsGlobeObject = NewsGlobeObject;
            headlineResourceID = HeadlineResourceID;
            headline = Headline;
        }

        public NewsGroup(byte[] Buffer, int StartIndex = 0)
        {
            articles = new ArticleHeadList();

            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                NewsGlobeID = 0;
                AccessType = 0;
                NewsGlobeObject = new ObjectBase();
                HeadlineResourceID = 0;
                Headline = String.Empty;
                IsVisible = false;
                Text = String.Empty;
                articles.Clear();
            }
            else
            {
                newsGlobeID = 0;
                accessType = 0;
                newsGlobeObject = new ObjectBase();
                headlineResourceID = 0;
                headline = String.Empty;
                isVisible = false;
                text = String.Empty;
                articles.Clear();
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(NewsGroup Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                NewsGlobeID = Model.NewsGlobeID;
                AccessType = Model.AccessType;
                NewsGlobeObject = Model.NewsGlobeObject;
                HeadlineResourceID = Model.HeadlineResourceID;
                Headline = Model.Headline;
                Text = String.Empty;
                articles.Clear();   // there is no articles in the model
            }
            else
            {
                newsGlobeID = Model.NewsGlobeID;
                accessType = Model.AccessType;
                newsGlobeObject = Model.NewsGlobeObject;
                headlineResourceID = Model.HeadlineResourceID;
                headline = Model.Headline;
                text = String.Empty;
                articles.Clear();   // there is no articles in the model
            }
        }
        #endregion

        #region IStringResolvable
		public virtual void ResolveStrings(StringDictionary StringResources, bool RaiseChangedEvent)
        {
            string res_name;

			StringResources.TryGetValue(headlineResourceID, out res_name);
            
            if (RaiseChangedEvent)
            {
                if (res_name != null) Headline = res_name;
                else Headline = String.Empty;
            }
            else
            {
                if (res_name != null) headline = res_name;
                else headline = String.Empty;
            }
        }
        #endregion
    }
}
