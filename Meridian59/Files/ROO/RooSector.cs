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
using System.Collections.Generic;
using Meridian59.Common.Constants;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Enums;
using Meridian59.Common;
using Meridian59.Files.BGF;
using Meridian59.Data.Models;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Files.ROO
{
    /// <summary>
    /// A sector definition in a map file.
    /// </summary>
    [Serializable]
    public class RooSector : IByteSerializableFast, IResourceResolvable, ITickable
    {
        // from roomanim.h:
        // "Number of milliseconds per pixel scrolled for various scrolling texture speeds"
        protected const int SCROLL_SLOW_PERIOD    = 12; // SCROLL_SLOW_PERIOD
        protected const int SCROLL_MEDIUM_PERIOD  = 6;  // SCROLL_MEDIUM_PERIOD
        protected const int SCROLL_FAST_PERIOD    = 2;  // SCROLL_FAST_PERIOD

        #region IByteSerializable
        public int ByteLength {
            get {
                int len =   TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.SHORT + 
                    TypeSizes.SHORT + TypeSizes.SHORT + TypeSizes.BYTE + TypeSizes.INT;

                if (HasSpeed)
                    len++;

                if (SlopeInfoFloor != null)
                    len += SlopeInfoFloor.ByteLength;

                if (SlopeInfoCeiling != null)
                    len += SlopeInfoCeiling.ByteLength;

                return len; 
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(ServerID), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(FloorTexture), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(CeilingTexture), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(TextureX), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(TextureY), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes((short)FloorHeight), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes((short)CeilingHeight), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Buffer[cursor] = Light1;
            cursor++;

            Array.Copy(BitConverter.GetBytes(Flags.Value), 0, Buffer, cursor, TypeSizes.INT);
            cursor += TypeSizes.INT;

            if (HasSpeed)
            {
                Buffer[cursor] = Speed;
                cursor++;
            }

            // Check for attached subinfo
            if (SlopeInfoFloor != null)
                cursor += SlopeInfoFloor.WriteTo(Buffer, cursor);              
            
            if (SlopeInfoCeiling != null)
                cursor += SlopeInfoCeiling.WriteTo(Buffer, cursor);
             
            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            *((short*)Buffer) = ServerID;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = FloorTexture;
            Buffer += TypeSizes.SHORT;

            *((ushort*)Buffer) = CeilingTexture;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = TextureX;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = TextureY;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = (short)FloorHeight;
            Buffer += TypeSizes.SHORT;

            *((short*)Buffer) = (short)CeilingHeight;
            Buffer += TypeSizes.SHORT;

            Buffer[0] = Light1;
            Buffer++;

            *((uint*)Buffer) = Flags.Value;
            Buffer += TypeSizes.INT;

            if (HasSpeed)
            {
                Buffer[0] = Speed;
                Buffer++;
            }

            // Check for attached subinfo
            if (SlopeInfoFloor != null)
                SlopeInfoFloor.WriteTo(ref Buffer);

            if (SlopeInfoCeiling != null)
                SlopeInfoCeiling.WriteTo(ref Buffer);
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            ServerID = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            FloorTexture = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            CeilingTexture = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            TextureX = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            TextureY = BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            FloorHeight = (Real)BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            CeilingHeight = (Real)BitConverter.ToInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Light1 = Buffer[cursor];
            cursor++ ;

            Flags = new RooSectorFlags(BitConverter.ToUInt32(Buffer, cursor));
            cursor += TypeSizes.INT;

            if (HasSpeed)
            {
                Speed = Buffer[cursor];
                cursor++;
            }
            
            // Check for attached subinfo
            if (Flags.IsSlopedFloor)
            {
                SlopeInfoFloor = new RooSectorSlopeInfo(RooVersion, Buffer, cursor);
                cursor += SlopeInfoFloor.ByteLength;
            }
            if (Flags.IsSlopedCeiling)
            {
                SlopeInfoCeiling = new RooSectorSlopeInfo(RooVersion, Buffer, cursor);
                cursor += SlopeInfoCeiling.ByteLength;
            }

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ServerID = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            FloorTexture = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            CeilingTexture = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            TextureX = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            TextureY = *((short*)Buffer);
            Buffer += TypeSizes.SHORT;

            FloorHeight = (Real)(*((short*)Buffer));
            Buffer += TypeSizes.SHORT;

            CeilingHeight = (Real)(*((short*)Buffer));
            Buffer += TypeSizes.SHORT;

            Light1 = Buffer[0];
            Buffer++;

            Flags = new RooSectorFlags(*((uint*)Buffer));
            Buffer += TypeSizes.INT;

            if (HasSpeed)
            {
                Speed = Buffer[0];
                Buffer++;
            }

            // Check for attached subinfo
            if (Flags.IsSlopedFloor)           
                SlopeInfoFloor = new RooSectorSlopeInfo(RooVersion, ref Buffer);
                           
            if (Flags.IsSlopedCeiling)
                SlopeInfoCeiling = new RooSectorSlopeInfo(RooVersion, ref Buffer);
        }

        public byte[] Bytes
        {
            get
            {
                byte[] returnValue = new byte[ByteLength];
                WriteTo(returnValue);
                return returnValue;
            }
            set
            {
                ReadFrom(value);
            }
        }
        #endregion

        /// <summary>
        /// Raised when texture changed on this sector floor or ceiling.
        /// </summary>
        public event SectorTextureChangedEventHandler TextureChanged;
        
        /// <summary>
        /// Raised when calling Tick() and the sector moved a bit.
        /// </summary>
        public event EventHandler Moved;

        /// <summary>
        /// Adjacent walls
        /// </summary>
        protected readonly List<RooWall> walls = new List<RooWall>();

        /// <summary>
        /// Adjacent side-defs
        /// </summary>
        protected readonly List<RooSideDef> sides = new List<RooSideDef>();

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public uint RooVersion { get; set; }

        /// <summary>
        /// Version9 does not have the speed byte
        /// </summary>
        public bool HasSpeed { get; set; }

        /// <summary>
        /// The non-zero based number of the wall (generated when loading)
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// ID the server uses to reference this sector for changes
        /// </summary>
        public short ServerID { get; set; }

        /// <summary>
        /// grdXXXXX number of floor texture
        /// </summary>
        public ushort FloorTexture { get; set; }

        /// <summary>
        /// grdXXXXX number of ceiling texture
        /// </summary>
        public ushort CeilingTexture { get; set; }
        
        /// <summary>
        /// Texture offset X
        /// </summary>
        public short TextureX { get; set; }
        
        /// <summary>
        /// Texture offset Y
        /// </summary>
        public short TextureY { get; set; }

        /// <summary>
        /// Floor height in server/new client FINENESS (1:64)
        /// </summary>
        public Real FloorHeight { get; set; }

        /// <summary>
        /// Ceiling height in server/new client FINENESS (1:64)
        /// </summary>
        public Real CeilingHeight { get; set; }

        /// <summary>
        /// Light value
        /// </summary>
        public byte Light1 { get; set; }

        /// <summary>
        /// Additional flags
        /// </summary>
        public RooSectorFlags Flags { get; set; }

        /// <summary>
        /// Scrollspeed
        /// </summary>
        public byte Speed { get; set; }

        /// <summary>
        /// Scrollspeed for floor texture expressed as V2 vector in Ogre style.
        /// Created from Speed and floor texture size.
        /// </summary>
        public V2 SpeedFloor { get; protected set; }

        /// <summary>
        /// Scrollspeed for ceiling texture expressed as V2 vector in Ogre style.
        /// Created from Speed and ceiling texture size.
        /// </summary>
        public V2 SpeedCeiling { get; protected set; }

        /// <summary>
        /// Info about sloped floor (NULL for nonsloped)
        /// </summary>
        public RooSectorSlopeInfo SlopeInfoFloor { get; set; }

        /// <summary>
        /// Info about sloped cleiling (NULL for nonsloped)
        /// </summary>
        public RooSectorSlopeInfo SlopeInfoCeiling { get; set; }

        /// <summary>
        /// Texture file for floor
        /// </summary>
        public BgfFile ResourceFloor { get; protected set; }

        /// <summary>
        /// Texture file for ceiling
        /// </summary>
        public BgfFile ResourceCeiling { get; protected set; }

        /// <summary>
        /// Some custom userdata.
        /// </summary>
        public object UserData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialNameFloor { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaterialNameCeiling { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string TextureNameFloor { get; protected set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string TextureNameCeiling { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public BgfBitmap TextureFloor { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public BgfBitmap TextureCeiling { get; protected set; }

        /// <summary>
        /// Sectors with Light value from 128-255 are affected by ambient light.
        /// Others only by their own light.
        /// </summary>
        public bool IsAffectedByAmbientLight
        {
            get { return (Light1 > 127); }
        }

        /// <summary>
        /// This is 0.0f for sector light values between 0 and 127, because
        /// they are not affected by AmbientLight.
        /// For 128 it is still 0, for 192 it is 1 and for 255 it's (almost) 2
        /// </summary>
        public Real AmbientLightModifier
        {
            get
            {
                return !IsAffectedByAmbientLight ? 0.0f :
                    (Real)(Light1 - 128) * (1.0f / 64.0f);
            }
        }

        /// <summary>
        /// This is 0.0f for sector light values between 128 and 255, because
        /// they are affected by AmbientLight.
        /// For 0 it is still 0, for 64 it is 0.5 and for 127 it's (almost) 1
        /// </summary>
        public Real OwnLight
        {
            get
            {
                return IsAffectedByAmbientLight ? 0.0f :
                    (Real)Light1 * (1.0f / 128.0f);
            }
        }

        /// <summary>
        /// Adjacent walls. Will be filled when ResolveIndices() on RooWall is called.
        /// </summary>
        public List<RooWall> Walls
        {
            get { return walls; }
        }

        /// <summary>
        /// Adjacent side-defs. Will be filled when ResolveIndices() on RooWall is called.
        /// </summary>
        public List<RooSideDef> Sides
        {
            get { return sides; }
        }

        /// <summary>
        /// True if floor or ceiling are moving
        /// </summary>
        public bool IsMoving { get { return IsMovingFloor || IsMovingCeiling; } }

        /// <summary>
        /// True if floor has active movement
        /// </summary>
        public bool IsMovingFloor { get; protected set; }

        /// <summary>
        /// True if ceiling has active movement
        /// </summary>
        public bool IsMovingCeiling { get; protected set; }
        
        /// <summary>
        /// Floor target height of move
        /// </summary>
        public Real MoveFloorHeight { get; set; }

        /// <summary>
        /// Ceiling target height of move
        /// </summary>
        public Real MoveCeilingHeight { get; set; }

        /// <summary>
        /// Speed of a possible ongoing floor movement
        /// </summary>
        public byte MoveFloorSpeed { get; set; }

        /// <summary>
        /// Speed of a possible ongoing ceiling movement
        /// </summary>
        public byte MoveCeilingSpeed { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create instance from values
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="ServerID"></param>
        /// <param name="FloorTexture"></param>
        /// <param name="CeilingTexture"></param>
        /// <param name="TextureX"></param>
        /// <param name="TextureY"></param>
        /// <param name="FloorHeight"></param>
        /// <param name="CeilingHeight"></param>
        /// <param name="Light1"></param>
        /// <param name="Light2"></param>
        /// <param name="Flags"></param>
        /// <param name="Unknown4"></param>
        /// <param name="HasSpeed"></param>
        public RooSector(uint RooVersion, 
            short ServerID, 
            ushort FloorTexture, ushort CeilingTexture,
            short TextureX, short TextureY, 
            Real FloorHeight, Real CeilingHeight, 
            byte Light1, byte Light2,
            uint Flags, byte Unknown4, bool HasSpeed = true)
        {
            this.RooVersion = RooVersion;
            this.ServerID = ServerID;
            this.FloorTexture = FloorTexture;
            this.CeilingTexture = CeilingTexture;
            this.TextureX = TextureX;
            this.TextureY = TextureY;
            this.FloorHeight = FloorHeight;
            this.CeilingHeight = CeilingHeight;
            this.Light1 = Light1;
            this.Flags = new RooSectorFlags(Flags);
            this.Speed = Unknown4;

            this.HasSpeed = HasSpeed;

            SpeedCeiling = V2.ZERO;
            SpeedFloor = V2.ZERO;
        }

        /// <summary>
        /// Create instance from parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="StartIndex"></param>
        /// <param name="HasSpeed"></param>
        public RooSector(uint RooVersion, byte[] Buffer, int StartIndex = 0, bool HasSpeed = true)
        {
            this.RooVersion = RooVersion;

            SpeedCeiling = V2.ZERO;
            SpeedFloor = V2.ZERO;

            this.HasSpeed = HasSpeed;
            ReadFrom(Buffer, StartIndex);
        }

        /// <summary>
        /// Create instance from parser
        /// </summary>
        /// <param name="RooVersion"></param>
        /// <param name="Buffer"></param>
        /// <param name="HasSpeed"></param>
        public unsafe RooSector(uint RooVersion, ref byte* Buffer, bool HasSpeed = true)
        {
            this.RooVersion = RooVersion;

            SpeedCeiling = V2.ZERO;
            SpeedFloor = V2.ZERO;

            this.HasSpeed = HasSpeed;
            ReadFrom(ref Buffer);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets references to used textures from a resource manager.
        /// </summary>
        /// <param name="M59ResourceManager"></param>
        /// <param name="RaiseChangedEvent"></param>
        public void ResolveResources(ResourceManager M59ResourceManager, bool RaiseChangedEvent)
        {
            // floor
            if (FloorTexture > 0)
            { 
                ResourceFloor = M59ResourceManager.GetRoomTexture(FloorTexture);

                if (ResourceFloor != null && ResourceFloor.Frames.Count > 0)
                {
                    TextureFloor = ResourceFloor.Frames[0];
                    SpeedFloor = GetSectorScrollSpeed(true, TextureFloor.Width, TextureFloor.Height);

                    TextureNameFloor = RooFile.GetNameForTexture(
                        ResourceFloor, 0);

                    MaterialNameFloor = RooFile.GetNameForMaterial(
                        ResourceFloor, 0, SpeedFloor);
                }
                else
                {
                    TextureFloor = null;
                    TextureNameFloor = null;
                    MaterialNameFloor = null;
                    SpeedFloor = V2.ZERO;
                }
            }
            else
            {
                TextureFloor = null;
                TextureNameFloor = null;
                MaterialNameFloor = null;
                SpeedFloor = V2.ZERO;
            }
            
            // ceiling
            if (CeilingTexture > 0)
            {
                ResourceCeiling = M59ResourceManager.GetRoomTexture(CeilingTexture);

                if (ResourceCeiling != null && ResourceCeiling.Frames.Count > 0)
                {
                    TextureCeiling = ResourceCeiling.Frames[0];
                    SpeedCeiling = GetSectorScrollSpeed(false, TextureCeiling.Width, TextureCeiling.Height);

                    TextureNameCeiling = RooFile.GetNameForTexture(
                        ResourceCeiling, 0);

                    MaterialNameCeiling = RooFile.GetNameForMaterial(
                        ResourceCeiling, 0, SpeedCeiling);
                }
                else
                {
                    TextureCeiling = null;
                    TextureNameCeiling = null;
                    MaterialNameCeiling = null;
                    SpeedCeiling = V2.ZERO;
                }
            }
            else
            {
                TextureCeiling = null;
                TextureNameCeiling = null;
                MaterialNameCeiling = null;
                SpeedCeiling = V2.ZERO;
            }
        }

        /// <summary>
        /// Starts a movement of the sector
        /// </summary>
        /// <param name="SectorMove"></param>
        public void StartMove(SectorMove SectorMove)
        {
            if (SectorMove.Type == AnimationType.FLOORLIFT)
            {
                // set target height and speed for floor
                MoveFloorHeight = (Real)SectorMove.Height;
                MoveFloorSpeed = SectorMove.Speed;
                IsMovingFloor = true;
            }
            else if (SectorMove.Type == AnimationType.CEILINGLIFT)
            {
                // set target height and speed for ceiling
                MoveCeilingHeight = (Real)SectorMove.Height;
                MoveCeilingSpeed = SectorMove.Speed;
                IsMovingCeiling = true;
            }
        }

        /// <summary>
        /// Updates the movement
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(double Tick, double Span)
        {
            const Real EPSILON = 0.01f;

            // nothing to do unless moving at least floor or ceiling
            if (!IsMoving)
                return;

            /************************* FLOOR *************************/
            if (IsMovingFloor)
            {
                // instant update of floor to new height
                if (MoveFloorSpeed == 0)
                {
                    FloorHeight = MoveFloorHeight;
                    IsMovingFloor = false;
                }
                else
                {
                    Real delta = MoveFloorHeight - FloorHeight;
                    Real step = GeometryConstants.SECTORMOVEBASECOEFF * (Real)Span * (Real)MoveFloorSpeed;

                    if (Math.Abs(delta) > EPSILON)
                    {
                        if (Math.Abs(step) > Math.Abs(delta))
                            step = delta;

                        else if (delta < 0.0f)
                            step = -step;

                        // apply step on floor height
                        FloorHeight += step;
                    }
                    else
                        IsMovingFloor = false;
                }
            }

            /************************* CEILING *************************/
            if (IsMovingCeiling)
            {
                if (MoveCeilingSpeed == 0)
                {
                    CeilingHeight = MoveCeilingHeight;
                    IsMovingCeiling = false;
                }
                else
                {
                    Real delta = MoveCeilingHeight - CeilingHeight;
                    Real step = GeometryConstants.SECTORMOVEBASECOEFF * (Real)Span * (Real)MoveCeilingSpeed;

                    if (Math.Abs(delta) > EPSILON)
                    {
                        if (Math.Abs(step) > Math.Abs(delta))
                            step = delta;

                        else if (delta < 0.0f)
                            step = -step;

                        // apply step on floor height
                        CeilingHeight += step;
                    }
                    else
                        IsMovingCeiling = false;
                }
            }

            /************************* SIDES *************************/
            foreach (RooWall wall in walls)
                wall.CalculateWallSideHeights();

            /************************* EVENT *************************/
            if (Moved != null)
                Moved(this, new EventArgs());         
        }

        /// <summary>
        /// Returns the floorheight for a given point in legacy client FINENESS (1:1024),
        /// Note: There is no check if these coordinates are in the sector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="WithSectorDepth"></param>
        /// <returns>Height of the floor in legacy client FINENESS (1:1024)</returns>
        public Real CalculateFloorHeight(Real x, Real y, bool WithSectorDepth = false)
        {
            // get height in client fineness
            Real height = (SlopeInfoFloor == null) ? 
                FloorHeight * GeometryConstants.KODFINETOCLIENTFINE :
                CalculateSlopeHeight(SlopeInfoFloor, x, y);

            if (WithSectorDepth)
            {
                switch (Flags.SectorDepth)
                { 
                    case RooSectorFlags.DepthType.Depth0:
                        height -= (Real)RooFile.SectorDepths[0];
                        break;

                    case RooSectorFlags.DepthType.Depth1:
                        height -= (Real)RooFile.SectorDepths[1];
                        break;

                    case RooSectorFlags.DepthType.Depth2:
                        height -= (Real)RooFile.SectorDepths[2];
                        break;

                    case RooSectorFlags.DepthType.Depth3:
                        height -= (Real)RooFile.SectorDepths[3];
                        break;
                }
            }

            return height;
        }

        /// <summary>
        /// Returns the ceilingheight for a given point in legacy client FINENESS (1:1024),
        /// Note: There is no check if these coordinates are in the sector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Height of the ceiling in legacy client FINENESS (1:1024)</returns>
        public Real CalculateCeilingHeight(Real x, Real y)
        {
            return (SlopeInfoCeiling == null) ? 
                CeilingHeight * GeometryConstants.KODFINETOCLIENTFINE : 
                CalculateSlopeHeight(SlopeInfoCeiling, x, y);
        }

        /// <summary>
        /// Calculates the z coordinate of a point given by it's x,y components and 
        /// a plane equation in slopeinfo.
        /// </summary>
        /// <param name="SlopeInfo"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Height of the point in legacy client FINENESS (1:1024)</returns>
        public static Real CalculateSlopeHeight(RooSectorSlopeInfo SlopeInfo, Real x, Real y)
        {
            return (-SlopeInfo.A * x - SlopeInfo.B * y - SlopeInfo.D) / SlopeInfo.C;
        }

        /// <summary>
        /// Returns Ogre like U, V scrollspeed.
        /// </summary>
        /// <param name="IsFloor"></param>
        /// <param name="TextureWidth"></param>
        /// <param name="TextureHeight"></param>
        /// <returns></returns>
        protected V2 GetSectorScrollSpeed(bool IsFloor, uint TextureWidth, uint TextureHeight)
        {
            V2 sp       = V2.ZERO;
            Real length = 0.0f;

            // no scrolling (0/0) if
            // 1) no texture size
            // 2) scrollspeed set to zero
            // 3) floor and floorscrolling off
            // 4) ceiling and ceilingscrolling off
            if (TextureWidth == 0 || TextureHeight == 0 || 
                Flags.ScrollSpeed == TextureScrollSpeed.NONE ||
                (IsFloor && !Flags.IsScrollFloor) ||
                (!IsFloor && !Flags.IsScrollCeiling))
                return sp;

            // build direction & length
            switch (Flags.ScrollDirection)
            {
                case TextureScrollDirection.N:
                    sp.X   = 0.0f;
                    sp.Y   = -1.0f;
                    length = (Real)TextureHeight;
                    break;

                case TextureScrollDirection.NE:
                    sp.X   = 1.0f;
                    sp.Y   = -1.0f;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;

                case TextureScrollDirection.E:
                    sp.X   = 1.0f;
                    sp.Y   = 0.0f;
                    length = (Real)TextureWidth;
                    break;

                case TextureScrollDirection.SE:
                    sp.X   = 1.0f;
                    sp.Y   = 1.0f;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;

                case TextureScrollDirection.S:
                    sp.X   = 0.0f;
                    sp.Y   = 1.0f;
                    length = (Real)TextureHeight;
                    break;

                case TextureScrollDirection.SW:
                    sp.X   = -1.0f;
                    sp.Y   = 1.0f;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;

                case TextureScrollDirection.W:
                    sp.X   = -1.0f;
                    sp.Y   = 0.0f;
                    length = (Real)TextureWidth;
                    break;

                case TextureScrollDirection.NW:
                    sp.X   = -1.0f;
                    sp.Y   = -1.0f;
                    length = (Real)Math.Sqrt(TextureWidth * TextureWidth + TextureHeight * TextureHeight);
                    break;
            }

            // multiply the amount of pixels
            // with the milliseconds required to scroll by 1 pixel
            // to get the time it requires to scroll the whole texture.
            switch (Flags.ScrollSpeed)
            {
                case TextureScrollSpeed.NONE:
                    length = 0.0f;
                    sp = V2.ZERO;
                    break;

                case TextureScrollSpeed.SLOW:
                    length *= (Real)SCROLL_SLOW_PERIOD;
                    break;

                case TextureScrollSpeed.MEDIUM:
                    length *= (Real)SCROLL_MEDIUM_PERIOD;
                    break;

                case TextureScrollSpeed.FAST:
                    length *= (Real)SCROLL_FAST_PERIOD;
                    break;
            }

            // scale the scrolldirection to match the given speed
            // ogre vectorlen 1.0f is 1 full texture scroll in 1 second
            if (length > 0.0f)
                sp.ScaleToLength(1000.0f / length);
                       
            return sp;
        }

        /// <summary>
        /// Sets the floor texture to another num
        /// </summary>
        /// <param name="TextureNum"></param>
        /// <param name="TextureFile"></param>
        public void SetFloorTexture(ushort TextureNum, BgfFile TextureFile)
        {
            string oldmaterial = MaterialNameFloor;

            FloorTexture = TextureNum;
            ResourceFloor = TextureFile;

            if (ResourceFloor != null && ResourceFloor.Frames.Count > 0)
            {
                TextureFloor = ResourceFloor.Frames[0];
                SpeedFloor = GetSectorScrollSpeed(true, TextureFloor.Width, TextureFloor.Height);

                TextureNameFloor = RooFile.GetNameForTexture(
                    ResourceFloor, 0);

                MaterialNameFloor = RooFile.GetNameForMaterial(
                    ResourceFloor, 0, SpeedFloor);
            }
            else
            {
                TextureFloor = null;
                TextureNameFloor = null;
                MaterialNameFloor = null;
                SpeedFloor = V2.ZERO;
            }

            if (TextureChanged != null)
                TextureChanged(this, new SectorTextureChangedEventArgs(this, true, oldmaterial));
        }

        /// <summary>
        /// Sets the ceiling texture to another num
        /// </summary>
        /// <param name="TextureNum"></param>
        /// <param name="TextureFile"></param>
        public void SetCeilingTexture(ushort TextureNum, BgfFile TextureFile)
        {
            string oldmaterial = MaterialNameCeiling;

            CeilingTexture = TextureNum;
            ResourceCeiling = TextureFile;

            if (ResourceCeiling != null && ResourceCeiling.Frames.Count > 0)
            {
                TextureCeiling = ResourceCeiling.Frames[0];
                SpeedCeiling = GetSectorScrollSpeed(false, TextureCeiling.Width, TextureCeiling.Height);

                TextureNameCeiling = RooFile.GetNameForTexture(
                    ResourceCeiling, 0);

                MaterialNameCeiling = RooFile.GetNameForMaterial(
                    ResourceCeiling, 0, SpeedCeiling);
            }
            else
            {
                TextureCeiling = null;
                TextureNameCeiling = null;
                MaterialNameCeiling = null;
                SpeedCeiling = V2.ZERO;
            }

            if (TextureChanged != null)
                TextureChanged(this, new SectorTextureChangedEventArgs(this, false, oldmaterial));
        }
        #endregion
    }
}
