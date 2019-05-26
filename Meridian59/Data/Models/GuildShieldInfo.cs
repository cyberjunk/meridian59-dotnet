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
using Meridian59.Drawing2D;
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Data of GuildShield
    /// </summary>
    [Serializable]
    public class GuildShieldInfo : IByteSerializable, INotifyPropertyChanged, IClearable, IUpdatable<GuildShieldInfo>
    {
        #region Constants
        protected const byte WHITECOLOR             = 9;
        protected const string DESIGNNOTAVAILABLE   = "Design not available";
        
        public const string PROPNAME_GUILDID        = "GuildID";
        public const string PROPNAME_GUILDNAME      = "GuildName";
        public const string PROPNAME_COLOR1         = "Color1";
        public const string PROPNAME_COLOR2         = "Color2";
        public const string PROPNAME_DESIGN         = "Design";
        public const string PROPNAME_SHIELDS        = "Shields";
        public const string PROPNAME_EXAMPLEMODEL   = "ExampleModel";
        public const string PROPNAME_GUILDSHIELDERROR = "GuildShieldError";
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
                return guildID.ByteLength + TypeSizes.SHORT + guildName.Length + TypeSizes.BYTE + TypeSizes.BYTE + TypeSizes.BYTE;
            }
        }      
 
        public int WriteTo(byte[] Buffer, int StartIndex=0)
        {
            int cursor = StartIndex;

            cursor += guildID.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(guildName.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(guildName), 0, Buffer, cursor, guildName.Length);
            cursor += guildName.Length;

            Buffer[cursor] = color1;
            cursor += TypeSizes.BYTE;

            Buffer[cursor] = color2;
            cursor += TypeSizes.BYTE;

            Buffer[cursor] = design;
            cursor += TypeSizes.BYTE;

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            guildID = new ObjectID(Buffer, cursor);
            cursor += guildID.ByteLength;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            guildName = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            color1 = Buffer[cursor];
            cursor += TypeSizes.BYTE;

            color2 = Buffer[cursor];
            cursor += TypeSizes.BYTE;

            design = Buffer[cursor];
            cursor += TypeSizes.BYTE;

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
        protected ObjectID guildID;
        protected string guildName;
        protected byte color1;
        protected byte color2;
        protected byte design;
        protected ResourceIDBGF[] shields;
        protected ObjectBase exampleModel;
        protected ServerString guildShieldError;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public ObjectID GuildID
        {
            get { return guildID; }
            set
            {
                // zero id indicates unclaimed shield
                // so reset name unless it's set to illegal shield string
                // for whatever reason the server always sends our own name in this case
                if (guildID.ID == 0 && (color1 != WHITECOLOR || color2 != WHITECOLOR))             
                    GuildName = String.Empty;
                             
                if (guildID != value)
                {
                    guildID = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDID));
                }
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        public string GuildName
        {
            get { return guildName; }
            set
            {
                if (guildID.ID == 0 && (color1 != WHITECOLOR || color2 != WHITECOLOR))
                {
                    guildName = String.Empty;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDNAME));
                }
                else if (guildName != value)
                {
                    guildName = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDNAME));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Color1
        {
            get { return color1; }
            set
            {
                if (value == WHITECOLOR && color2 == WHITECOLOR)
                    GuildName = DESIGNNOTAVAILABLE;

                if (color1 != value)
                {
                    color1 = value;
                    exampleModel.ColorTranslation = ColorTransformation.GetGuildShieldColor(color1, color2);

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COLOR1));                  
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Color2
        {
            get { return color2; }
            set
            {
                if (color1 == WHITECOLOR && value == WHITECOLOR)
                    GuildName = DESIGNNOTAVAILABLE;

                if (color2 != value)
                {
                    color2 = value;
                    exampleModel.ColorTranslation = ColorTransformation.GetGuildShieldColor(color1, color2);

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_COLOR2));
                }
            }
        }

        /// <summary>
        /// The selected design.
        /// Note: This is not zero based. First one is '1'.
        /// </summary>
        public byte Design
        {
            get { return design; }
            set
            {
                if (design != value)
                {
                    design = value;

                    // update examplemodel if we got a shield for this design
                    if (design > 0 && shields != null && shields.Length > (design-1))
                    {
                        exampleModel.OverlayFileRID = shields[design-1].Value;
                        exampleModel.OverlayFile = shields[design-1].Name;
                        exampleModel.Resource = shields[design-1].Resource;
                    }

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DESIGN));
                }
            }
        }

        /// <summary>
        /// Not contained in serialization.
        /// </summary>
        public ResourceIDBGF[] Shields
        {
            get { return shields; }
            set
            {
                if (shields != value)
                {
                    shields = value;

                    // update examplemodel if we got a shield for this design
                    if (design > 0 && shields != null && shields.Length > (design - 1))
                    {
                        exampleModel.OverlayFileRID = shields[design - 1].Value;
                        exampleModel.OverlayFile = shields[design - 1].Name;
                        exampleModel.Resource = shields[design - 1].Resource;
                    }

                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SHIELDS));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObjectBase ExampleModel
        {
            get { return exampleModel; }
            set
            {
                if (exampleModel != value)
                {
                    exampleModel = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_EXAMPLEMODEL));
                }
            }
        }

        /// <summary>
        /// Set when an error ServerString is received for the shield UI.
        /// </summary>
        public ServerString GuildShieldError
        {
            get { return guildShieldError; }
            set
            {
                if (guildShieldError != value)
                {
                    guildShieldError = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_GUILDSHIELDERROR));
                }
            }
        }
        #endregion

        #region Constructors
        public GuildShieldInfo()
        {
            ExampleModel = new ObjectBase();
            
            Clear(false);
        }
        
        public GuildShieldInfo(ObjectID GuildID, string GuildName, byte Color1, byte Color2, byte Design)
        {
            ExampleModel = new ObjectBase();
            
            guildID = GuildID;
            guildName = GuildName;
            color1 = Color1;
            color2 = Color2;
            design = Design;
            guildShieldError = new ServerString();
        }

        public GuildShieldInfo(byte[] Buffer, int StartIndex = 0)
        {
            ExampleModel = new ObjectBase();
            GuildShieldError = new ServerString();

            ReadFrom(Buffer, StartIndex);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {           
            if (RaiseChangedEvent)
            {
                GuildID = new ObjectID();
                GuildName = DESIGNNOTAVAILABLE;
                Color1 = WHITECOLOR;
                Color2 = WHITECOLOR;
                Design = 1;
                GuildShieldError = new ServerString();
            }
            else
            {
                guildID = new ObjectID();
                guildName = DESIGNNOTAVAILABLE;
                color1 = WHITECOLOR;
                color2 = WHITECOLOR;
                design = 1;
                guildShieldError = new ServerString();
                // update examplemodel
                exampleModel.ColorTranslation = ColorTransformation.GetGuildShieldColor(color1, color2);
                
                if (design > 0 && shields != null && shields.Length > (design - 1))
                {
                    exampleModel.OverlayFileRID = shields[design - 1].Value;
                    exampleModel.OverlayFile = shields[design - 1].Name;
                    exampleModel.Resource = shields[design - 1].Resource;
                }
            }
        }
        #endregion

        #region IUpdatable
        public void UpdateFromModel(GuildShieldInfo Model, bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                GuildID = Model.GuildID;
                GuildName = Model.GuildName;
                Color1 = Model.Color1;
                Color2 = Model.Color2;
                Design = Model.Design;
                GuildShieldError = Model.GuildShieldError;
            }
            else
            {
                guildID = Model.GuildID;
                guildName = Model.GuildName;
                color1 = Model.Color1;
                color2 = Model.Color2;
                design = Model.Design;
                guildShieldError = Model.GuildShieldError;

                // update examplemodel
                exampleModel.ColorTranslation = ColorTransformation.GetGuildShieldColor(color1, color2);

                if (design > 0 && shields != null && shields.Length > (design - 1))
                {
                    exampleModel.OverlayFileRID = shields[design - 1].Value;
                    exampleModel.OverlayFile = shields[design - 1].Name;
                    exampleModel.Resource = shields[design - 1].Resource;
                }
            }
        }
        #endregion
    }
}
