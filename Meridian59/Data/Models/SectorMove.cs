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
using Meridian59.Common.Enums;
using Meridian59.Files.ROO;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Sector movement info.
    /// </summary>
    [Serializable]
    public class SectorMove : IByteSerializableFast, INotifyPropertyChanged, IClearable, ITickable
    {
        #region Constants
        public const string PROPNAME_TYPE       = "Type";
        public const string PROPNAME_SECTORNR   = "SectorNr";
        public const string PROPNAME_HEIGHT     = "Height";
        public const string PROPNAME_SPEED      = "Speed";
        #endregion

        /// <summary>
        /// Raised when calling Tick() and the sector moved a bit.
        /// </summary>
        public event EventHandler Moved;

        /// <summary>
        /// Raised when calling Tick() and the sectormove finished.
        /// </summary>
        public event EventHandler Finished;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        #endregion

        #region Fields
        protected AnimationType type;
        protected ushort sectorNr;
        protected ushort height;
        protected byte speed;
        protected RooSector sector;
        protected readonly List<RooWall> walls = new List<RooWall>();
        protected readonly List<RooSideDef> sides = new List<RooSideDef>();
        protected Real currentHeight;
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public AnimationType Type
        {
            get { return type; }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_TYPE));
                }
            }
        }

        public ushort SectorNr
        {
            get { return sectorNr; }
            set
            {
                if (sectorNr != value)
                {
                    sectorNr = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SECTORNR));
                }
            }
        }

        public ushort Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_HEIGHT));
                }
            }
        }

        public byte Speed
        {
            get { return speed; }
            set
            {
                if (this.speed != value)
                {
                    this.speed = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_SPEED));
                }
            }
        }

        public RooSector Sector
        {
            get { return sector; }
        }

        public List<RooWall> Walls
        {
            get { return walls; }
        }

        public List<RooSideDef> Sides
        {
            get { return sides; }
        }

        public Real CurrentHeight
        {
            get { return currentHeight; }
        }
        #endregion

        #region IByteSerializable
        public virtual int ByteLength
        {
            get
            {
                return TypeSizes.BYTE + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE;
            }
        }

        public virtual int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Buffer[cursor] = (Byte)Type;
            cursor++;
                    
            Array.Copy(BitConverter.GetBytes(sectorNr), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(height), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = speed;
            cursor++;
            
            return cursor - StartIndex;
        }

        public virtual int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            type = (AnimationType)Buffer[cursor];
            cursor++;

            sectorNr = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            height = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            speed = Buffer[cursor];
            cursor++;

            return cursor - StartIndex;;
        }

        public virtual unsafe void WriteTo(ref byte* Buffer)
        {
            Buffer[0] = (byte)type;
            Buffer++;

            *((ushort*)Buffer) = sectorNr;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = height;
            Buffer += TypeSizes.SHORT;

            Buffer[0] = speed;
            Buffer++;
        }

        public virtual unsafe void ReadFrom(ref byte* Buffer)
        {
            type = (AnimationType)Buffer[0];
            Buffer++;

            sectorNr = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;  

            height = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT; 

            speed = Buffer[0];
            Buffer++;
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

        #region Constructors
        public SectorMove()
        {
            Clear(false);
        }
        
        public SectorMove(AnimationType Type, ushort SectorNr, ushort Height, byte Speed)
        {
            this.type = Type;
            this.sectorNr = SectorNr;
            this.height = Height;
            this.speed = Speed;
        }

        public SectorMove(byte[] Buffer, int StartIndex = 0)
        {
            this.ReadFrom(Buffer, StartIndex);
        }

        public unsafe SectorMove(ref byte* Buffer)
        {
            this.ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RaiseChangedEvent"></param>
        public virtual void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Type = 0;
                SectorNr = 0;
                Height = 0;
                Speed = 0;
                sector = null;
                walls.Clear();
            }
            else
            {
                type = 0;
                sectorNr = 0;
                height = 0;
                speed = 0;
                sector = null;
                walls.Clear();
            }
        }
        #endregion

        #region ITickable
        /// <summary>
        /// Updates the movement
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(long Tick, long Span)
        {
            // instant updates for speed = 0
            if (Speed == 0)
            {
                if (type == AnimationType.FLOORLIFT)
                    sector.FloorHeight = (short)Height;

                else if (type == AnimationType.CEILINGLIFT)
                    sector.CeilingHeight = (short)Height;

                // update wallside height values on attached walls
                foreach (RooWall wall in walls)
                    wall.CalculateWallSideHeights();

                if (Moved != null)
                    Moved(this, new EventArgs());

                if (Finished != null)
                    Finished(this, new EventArgs());
            }
            else
            {
                Real delta = Height - currentHeight; ;
                Real step = GeometryConstants.SECTORMOVEBASECOEFF * (Real)Span * (Real)speed;
                const Real EPSILON = 0.01f;

                if (Math.Abs(delta) > EPSILON)
                {
                    if (Math.Abs(step) > Math.Abs(delta))
                        step = delta;

                    else if (delta < 0)
                        step = -step;

                    currentHeight += step;

                    if (type == AnimationType.FLOORLIFT)
                        sector.FloorHeight = Convert.ToInt16(currentHeight);

                    else if (type == AnimationType.CEILINGLIFT)
                        sector.CeilingHeight = Convert.ToInt16(currentHeight);

                    // update wallside height values on attached walls
                    foreach (RooWall wall in walls)
                        wall.CalculateWallSideHeights();

                    if (Moved != null)
                        Moved(this, new EventArgs());
                }
                else
                {
                    if (Finished != null)
                        Finished(this, new EventArgs());
                }
            }
        }
        #endregion

        /// <summary>
        /// Adjusts a SectorMove by new SectorMove information,
        /// e.g. starts moving down again before reached top and others.
        /// </summary>
        /// <param name="Move"></param>
        public void Adjust(SectorMove Move)
        {
            // make sure this model fits the new one
            if (SectorNr == Move.SectorNr && Type == Move.Type)
            {
                Type = Move.Type;
                SectorNr = Move.SectorNr;
                Height = Move.Height;
                Speed = Move.Speed;
            }
        }

        /// <summary>
        /// Sets the Sector property and fills the Walls and Sides lists
        /// with the elements affected by this sector move.
        /// </summary>
        /// <param name="RooFile">A roomfile this move is applied on</param>
        public void ResolveAffected(RooFile RooFile)
        {
            sector = null;
            walls.Clear();
            sides.Clear();

            // find sector
            foreach (RooSector obj in RooFile.Sectors)
            {
                if (obj.ServerID == SectorNr)
                {
                    sector = obj;
                    
                    if (Type == AnimationType.FLOORLIFT)
                        currentHeight = sector.FloorHeight;

                    else if (Type == AnimationType.CEILINGLIFT)
                        currentHeight = sector.CeilingHeight;

                    break;
                }
            }

            // if sector found, find walls
            if (sector != null)
            {
                foreach (RooWall obj in RooFile.Walls)
                {
                    // if this wall is adjacent to the moving sector
                    // it must be edited also
                    if (obj.LeftSector == sector ||
                        obj.RightSector == sector)
                    {
                        walls.Add(obj);

                        if (obj.LeftSide != null && !sides.Contains(obj.LeftSide))
                            sides.Add(obj.LeftSide);

                        if (obj.RightSide != null && !sides.Contains(obj.RightSide))
                            sides.Add(obj.RightSide);
                    }
                }
            }
        } 
    }
}
