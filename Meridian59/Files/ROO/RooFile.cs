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
using System.IO;
using System.Collections.Generic;
using Meridian59.Common.Interfaces;
using Meridian59.Common.Constants;
using Meridian59.Files.BGF;
using Meridian59.Common;
using Meridian59.Common.Enums;
using Meridian59.Protocol.GameMessages;
using Meridian59.Protocol.Enums;
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
    /// Meridian 59 map file. Containing BSP-Tree, Texture information and more.
    /// </summary>
    [Serializable]
    public class RooFile : IGameFile, IByteSerializableFast, ITickable
    {
        /// <summary>
        /// Bundles some info, used as return type in GetMaterialInfos()
        /// </summary>
        public struct MaterialInfo
        {
            public BgfBitmap Texture;
            public string TextureName;
            public string MaterialName;          
            public V2 ScrollSpeed;

            public MaterialInfo(
                BgfBitmap Texture,
                string TextureName,
                string MaterialName,          
                V2 ScrollSpeed)
            {
                this.Texture = Texture;
                this.TextureName = TextureName;
                this.MaterialName = MaterialName;
                this.ScrollSpeed = ScrollSpeed;
            }
        }

        /// <summary>
        /// Stores intermediate information about 2D intersections
        /// wich must be evaluated later
        /// </summary>
        public struct IntersectInfo
        {
            public RooSector  SectorS;
            public RooSector  SectorE;
            public RooSideDef SideS;
            public RooSideDef SideE;
            public RooWall    Wall;
            public V2         Q;
            public Real       FloorHeight;
            public Real       Distance2;

            public void Set(
                RooSector SectorS,
                RooSector SectorE,
                RooSideDef SideS,
                RooSideDef SideE,
                RooWall Wall,
                V2 Q,
                Real FloorHeight,
                Real Distance2)
            {
                this.SectorS = SectorS;
                this.SectorE = SectorE;
                this.SideS = SideS;
                this.SideE = SideE;
                this.Wall = Wall;
                this.Q = Q;
                this.FloorHeight = FloorHeight;
                this.Distance2 = Distance2;
            }

            public void CalcDist2(ref V2 S)
            {
               this.Distance2 = (Q - S).LengthSquared;
            }
        }

        #region Constants
        public const uint SIGNATURE             = 0xB14F4F52;   // first expected bytes in file
        public const uint VERSION               = 13;           // current
        public const uint VERSIONSPEED          = 10;           // first one that has additional "speed" values
        public const uint VERSIONMONSTERGRID    = 12;           // first one with monster grid
        public const uint VERSIONHIGHRESGRID    = 13;           // first one with highres grid
        public const uint VERSIONFLOATCOORDS    = 14;           // first one with floating points
        public const uint VERSIONNOGRIDS        = 15;           // first one that does not include grids
        public const uint MINVERSION            = 9;            // absolute minimum we can handle
        public const uint ENCRYPTIONFLAG        = 0xFFFFFFFF;
        public const byte ENCRYPTIONINFOLENGTH  = 12;
        public const int DEFAULTCLIENTOFFSET    = 20;
        public const int DEFAULTDEPTH0          = 0;
        public const int DEFAULTDEPTH1          = GeometryConstants.FINENESS / 5;
        public const int DEFAULTDEPTH2          = 2 * GeometryConstants.FINENESS / 5;
        public const int DEFAULTDEPTH3          = 3 * GeometryConstants.FINENESS / 5;
        public const string ERRORCRUSHPLATFORM  = "Crusher is only supported in x86 builds on Windows.";      
        public static byte[] PASSWORDV12 = new byte[] { 0x6F, 0xCA, 0x54, 0xB7, 0xEC, 0x64, 0xB7, 0x00 };
        public static byte[] PASSWORDV10 = new byte[] { 0x15, 0x20, 0x53, 0x01, 0xFC, 0xAA, 0x64, 0x00 };
        public static readonly int[] SectorDepths = new int[] { DEFAULTDEPTH0, DEFAULTDEPTH1, DEFAULTDEPTH2, DEFAULTDEPTH3 };     
        public const int MAXINTERSECTIONS = 16;
        #endregion

        #region Events
        /// <summary>
        /// Raised when a texture on a wallside changed.
        /// </summary>
        public event WallTextureChangedEventHandler WallTextureChanged;

        /// <summary>
        /// Raised when a texture on a floor or ceiling changed.
        /// </summary>
        public event SectorTextureChangedEventHandler SectorTextureChanged;

        /// <summary>
        /// Raised when Update() is called and a sector moved a bit.
        /// </summary>
        public event SectorMovedEventHandler SectorMoved;
        #endregion

        #region IByteSerializable
        public int ByteLength
        {
            get 
            {
                int len = TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;

                if (EncryptionEnabled)
                    len += TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;

                // basicinfo (roomsize, offsets.. 8*4=32 bytes)
                len += TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + 
                    TypeSizes.INT + TypeSizes.INT + TypeSizes.INT + TypeSizes.INT;

                // section 1
                len += TypeSizes.SHORT;
                foreach (RooBSPItem bspItem in BSPTreeLeaves)
                    len += bspItem.ByteLength;
                
                foreach (RooBSPItem bspItem in BSPTreeNodes)
                    len += bspItem.ByteLength;

                // section 2
                len += TypeSizes.SHORT;
                foreach (RooWall lineDef in Walls)
                    len += lineDef.ByteLength;

                // section 3
                len += TypeSizes.SHORT;
                foreach (RooWallEditor section3Item in WallsEditor)
                    len += section3Item.ByteLength;

                // section 4
                len += TypeSizes.SHORT;
                foreach (RooSideDef sideDef in SideDefs)
                    len += sideDef.ByteLength;

                // section 5
                len += TypeSizes.SHORT;
                foreach (RooSector sectorDef in Sectors)
                    len += sectorDef.ByteLength;

                // section 6
                len += TypeSizes.SHORT;
                foreach (RooThing section6Item in Things)
                    len += section6Item.ByteLength;

                // roomid
                len += TypeSizes.INT;

                len += Grids.ByteLength;

                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            int encryptionStartOffset = -1;

            // Write header
            Array.Copy(BitConverter.GetBytes(SIGNATURE), 0, Buffer, cursor, TypeSizes.INT);  // Signature    (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(RooVersion), 0, Buffer, cursor, TypeSizes.INT);        // MapType      (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(Challenge), 0, Buffer, cursor, TypeSizes.INT);         // Challenge    (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetClient), 0, Buffer, cursor, TypeSizes.INT);      // OffsetClient (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetServer), 0, Buffer, cursor, TypeSizes.INT);      // OffsetServer (4 bytes)
            cursor += TypeSizes.INT;

            //
            // Client part (was encrypted once)
            // 

            if (EncryptionEnabled)
            {
                Array.Copy(BitConverter.GetBytes(ENCRYPTIONFLAG), 0, Buffer, cursor, TypeSizes.INT);            // EncryptionFlag (4 bytes)
                cursor += TypeSizes.INT;

                Array.Copy(BitConverter.GetBytes(EncryptedStreamLength), 0, Buffer, cursor, TypeSizes.INT);     // EncryptedStreamLength (4 bytes)
                cursor += TypeSizes.INT;

                // This is the ExpectedResponse, but we don't know it yet, see below...             // ExpectedResponse (4 bytes)
                cursor += TypeSizes.INT;

                // save the cursorposition as startindex for encryption later
                encryptionStartOffset = cursor;
            }

            // BasicInfo            
            Array.Copy(BitConverter.GetBytes(RoomSizeX), 0, Buffer, cursor, TypeSizes.INT);            // RoomSizeX        (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(RoomSizeY), 0, Buffer, cursor, TypeSizes.INT);            // RoomSizeY        (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetBSPTree), 0, Buffer, cursor, TypeSizes.INT);        // OffsetBSPTree    (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetWalls), 0, Buffer, cursor, TypeSizes.INT);       // OffsetLineDefs   (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetWallsEditor), 0, Buffer, cursor, TypeSizes.INT);       // OffsetSection3   (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetSideDefs), 0, Buffer, cursor, TypeSizes.INT);       // OffsetSideDefs   (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetSectors), 0, Buffer, cursor, TypeSizes.INT);     // OffsetSectorDefs (4 bytes)
            cursor += TypeSizes.INT;

            Array.Copy(BitConverter.GetBytes(OffsetThings), 0, Buffer, cursor, TypeSizes.INT);       // OffsetSection6   (4 bytes)
            cursor += TypeSizes.INT;

            // Section 1: BSP-Tree
            ushort len = Convert.ToUInt16(BSPTree.Count);
            Array.Copy(BitConverter.GetBytes(len), 0, Buffer, cursor, TypeSizes.SHORT);                  // Section1ItemsLEN (2 bytes)
            cursor += TypeSizes.SHORT;
            
            foreach (RooBSPItem bspItem in BSPTree)
                cursor += bspItem.WriteTo(Buffer, cursor);
          
            // Section 2: Walls
            len = Convert.ToUInt16(Walls.Count);
            Array.Copy(BitConverter.GetBytes(len), 0, Buffer, cursor, TypeSizes.SHORT);                  // Section2ItemsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (RooWall wall in Walls)
                cursor += wall.WriteTo(Buffer, cursor);

            // Section 3: Unknown
            len = Convert.ToUInt16(WallsEditor.Count);
            Array.Copy(BitConverter.GetBytes(len), 0, Buffer, cursor, TypeSizes.SHORT);                  // Section3ItemsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (RooWallEditor section3Item in WallsEditor)
                cursor += section3Item.WriteTo(Buffer, cursor);

            // Section 4: SideDefs
            len = Convert.ToUInt16(SideDefs.Count);
            Array.Copy(BitConverter.GetBytes(len), 0, Buffer, cursor, TypeSizes.SHORT);                  // SideDefsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (RooSideDef sideDef in SideDefs)
                cursor += sideDef.WriteTo(Buffer, cursor);

            // Section 5: Sectors
            len = Convert.ToUInt16(Sectors.Count);
            Array.Copy(BitConverter.GetBytes(len), 0, Buffer, cursor, TypeSizes.SHORT);                  // SectorDefsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (RooSector sectorDef in Sectors)
                cursor += sectorDef.WriteTo(Buffer, cursor);

            // Section 6: Unknown
            len = Convert.ToUInt16(Things.Count);
            Array.Copy(BitConverter.GetBytes(len), 0, Buffer, cursor, TypeSizes.SHORT);                  // Section6ItemsLEN (2 bytes)
            cursor += TypeSizes.SHORT;

            foreach (RooThing section6Item in Things)
                cursor += section6Item.WriteTo(Buffer, cursor);

            Array.Copy(BitConverter.GetBytes(RoomID), 0, Buffer, cursor, TypeSizes.INT);               // RoomID (4 bytes)
            cursor += TypeSizes.INT;

            // Do the encryption
            if (EncryptionEnabled)
            {
#if WINCLR && X86
                ExpectedResponse = Crush32.Encrypt(Buffer, encryptionStartOffset, cursor - encryptionStartOffset, Challenge, GetPassword());

                // Write the expected response ( was updated in Encrypt() )
                Array.Copy(BitConverter.GetBytes(ExpectedResponse), 0, Buffer, StartIndex + DEFAULTCLIENTOFFSET + 8, TypeSizes.INT); 
#else
                throw new Exception(ERRORCRUSHPLATFORM);
#endif
            }

            //
            // server part (was never encrypted)
            // 

            cursor += Grids.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            int encryptionStartOffset = -1;

            // Write header
            *((uint*)Buffer) = SIGNATURE;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = RooVersion;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = Challenge;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = OffsetClient;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetServer;
            Buffer += TypeSizes.INT;

            //
            // Client part (was encrypted once)
            // 

            if (EncryptionEnabled)
            {
                *((uint*)Buffer) = ENCRYPTIONFLAG;
                Buffer += TypeSizes.INT;

                *((int*)Buffer) = EncryptedStreamLength;
                Buffer += TypeSizes.INT;

                // This is the ExpectedResponse, but we don't know it yet, see below...
                Buffer += TypeSizes.INT;

                // save the cursorposition as startindex for encryption later
                encryptionStartOffset = (int)Buffer;
            }

            // BasicInfo            
            *((uint*)Buffer) = RoomSizeX;
            Buffer += TypeSizes.INT;

            *((uint*)Buffer) = RoomSizeY;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetBSPTree;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetWalls;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetWallsEditor;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetSideDefs;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetSectors;
            Buffer += TypeSizes.INT;

            *((int*)Buffer) = OffsetThings;
            Buffer += TypeSizes.INT;

            // Section 1: BSP-Tree
            *((ushort*)Buffer) = (ushort)BSPTree.Count;
            Buffer += TypeSizes.SHORT;

            foreach (RooBSPItem bspItem in BSPTree)
                bspItem.WriteTo(ref Buffer);

            // Section 2: Walls
            *((ushort*)Buffer) = (ushort)Walls.Count; 
            Buffer += TypeSizes.SHORT;

            foreach (RooWall wall in Walls)
                wall.WriteTo(ref Buffer);

            // Section 3: Unknown
            *((ushort*)Buffer) = (ushort)WallsEditor.Count; 
            Buffer += TypeSizes.SHORT;

            foreach (RooWallEditor section3Item in WallsEditor)
                section3Item.WriteTo(ref Buffer);

            // Section 4: SideDefs
            *((ushort*)Buffer) = (ushort)SideDefs.Count; 
            Buffer += TypeSizes.SHORT;

            foreach (RooSideDef sideDef in SideDefs)
                sideDef.WriteTo(ref Buffer);

            // Section 5: Sectors
            *((ushort*)Buffer) = (ushort)Sectors.Count; 
            Buffer += TypeSizes.SHORT;

            foreach (RooSector sectorDef in Sectors)
                sectorDef.WriteTo(ref Buffer);

            // Section 6: Unknown
            *((ushort*)Buffer) = (ushort)Things.Count; 
            Buffer += TypeSizes.SHORT;

            foreach (RooThing section6Item in Things)
                section6Item.WriteTo(ref Buffer);

            // RoomID
            *((int*)Buffer) = RoomID;
            Buffer += TypeSizes.INT;

            // Do the encryption
            if (EncryptionEnabled)
            {
#if WINCLR && X86
                ExpectedResponse = Crush32.Encrypt((byte*)encryptionStartOffset, (int)Buffer - encryptionStartOffset, Challenge, GetPassword());

                // Write the expected response ( was updated in Encrypt() )
                *((uint*)(encryptionStartOffset - TypeSizes.INT)) = ExpectedResponse;
#else
                throw new Exception(ERRORCRUSHPLATFORM);
#endif
            }

            //
            // server parts
            //

            Grids.WriteTo(ref Buffer);
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            uint signature = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            if (signature == SIGNATURE)
            {
                RooVersion = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                // supported version ?
                if (RooVersion >= RooFile.MINVERSION)
                {
                    Challenge = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;

                    OffsetClient = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                   
                    OffsetServer = BitConverter.ToInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                }
                else
                    throw new Exception("RooVersion too old, got: " + RooVersion + " Expected greater " + RooFile.MINVERSION);
            }
            else
                throw new Exception("Wrong file signature: " + signature + " (expected " + SIGNATURE + ").");

            //
            // Client part (was encrypted once)
            // 

            // Check if file is encrypted
            if (BitConverter.ToUInt32(Buffer, cursor) == ENCRYPTIONFLAG)
            {
#if WINCLR && X86
                EncryptionEnabled = true;
                cursor += TypeSizes.INT;

                // get additional values for decryption
                EncryptedStreamLength = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                ExpectedResponse = BitConverter.ToUInt32(Buffer, cursor);
                cursor += TypeSizes.INT;

                Crush32.Decrypt(Buffer, cursor, EncryptedStreamLength, Challenge, ExpectedResponse, GetPassword());
#else
                throw new Exception(ERRORCRUSHPLATFORM);
#endif
            }
            else
                EncryptionEnabled = false;

            // Get the basic infos, like room dimension and section offsets
            RoomSizeX = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            RoomSizeY = BitConverter.ToUInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            OffsetBSPTree = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            OffsetWalls = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            OffsetWallsEditor = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            OffsetSideDefs = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            OffsetSectors = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            OffsetThings = BitConverter.ToInt32(Buffer, cursor);
            cursor += TypeSizes.INT;

            // used to save section item counts
            ushort len;

            // Section 1: BSP-Tree           
            cursor = OffsetBSPTree + (Convert.ToByte(EncryptionEnabled) * 12);
            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            BSPTree.Clear();
            BSPTree.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooBSPItem bspItem = RooBSPItem.ExtractBSPItem(RooVersion, Buffer, cursor);
                cursor += bspItem.ByteLength;

                BSPTree.Add(bspItem);     
            }

            // Section 2: Walls
            cursor = OffsetWalls + (Convert.ToByte(EncryptionEnabled) * 12);         
            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Walls.Clear();
            Walls.Capacity = len + 10;           
            for (int i = 0; i < len; i++)
            {
                RooWall lineDef = new RooWall(RooVersion, Buffer, cursor);               
                cursor += lineDef.ByteLength;

                lineDef.Num = i + 1;
                Walls.Add(lineDef);               
            }

            // Section 3: WallsEditor
            cursor = OffsetWallsEditor + (Convert.ToByte(EncryptionEnabled) * 12);
            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            WallsEditor.Clear();
            WallsEditor.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooWallEditor wallEditor = new RooWallEditor(Buffer, cursor);
                cursor += wallEditor.ByteLength;

                wallEditor.Num = i + 1;
                WallsEditor.Add(wallEditor);
            }

            // Section 4: SideDefs
            cursor = OffsetSideDefs + (Convert.ToByte(EncryptionEnabled) * 12);
            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            SideDefs.Clear();
            SideDefs.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooSideDef sideDef = new RooSideDef(Buffer, cursor);
                cursor += sideDef.ByteLength;
                
                sideDef.Num = i + 1;
                sideDef.TextureChanged += OnSideTextureChanged;
                SideDefs.Add(sideDef);
            }

            // Section 5: Sectors
            cursor = OffsetSectors + (Convert.ToByte(EncryptionEnabled) * 12);
            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            bool hasSpeed = (RooVersion >= VERSIONSPEED);
            Sectors.Clear();
            Sectors.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooSector sectorDef = new RooSector(RooVersion, Buffer, cursor, hasSpeed);
                cursor += sectorDef.ByteLength;

                sectorDef.Num = i + 1;
                sectorDef.TextureChanged += OnSectorTextureChanged;
                sectorDef.Moved += OnSectorMoved;
                Sectors.Add(sectorDef);
            }

            // Section 6: Things
            cursor = OffsetThings + (Convert.ToByte(EncryptionEnabled) * 12);
            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            Things.Clear();
            Things.Capacity = len + 10;
            if (len > 2)
            {
                for (int i = 0; i < len; i++)
                {
                    RooThingExtended rooThing = new RooThingExtended(Buffer, cursor);
                    cursor += rooThing.ByteLength;

                    Things.Add(rooThing);
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    RooThing rooThing = new RooThing(Buffer, cursor);
                    cursor += rooThing.ByteLength;

                    Things.Add(rooThing);
                }
            }

            // some older maps don't have the roomid
            // so if we've already reached the serveroffset, set it to 0
            if (cursor == OffsetServer)
            {
                RoomID = 0;
            }
            else
            {
                // get roomid
                RoomID = BitConverter.ToInt32(Buffer, cursor);
                cursor += TypeSizes.INT;
            }

            //
            // Server part
            // 

            // load grids
            Grids = new RooGrids(RooVersion, Buffer, cursor);
            cursor += Grids.ByteLength;
            
            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            // save the startptr, we need it later
            byte* readStartPtr = Buffer;

            uint signature = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            if (signature == SIGNATURE)
            {
                RooVersion = *((uint*)Buffer);
                Buffer += TypeSizes.INT;

                // supported version ?
                if (RooVersion >= RooFile.MINVERSION)
                {
                    Challenge = *((uint*)Buffer);
                    Buffer += TypeSizes.INT;

                    OffsetClient = *((uint*)Buffer);
                    Buffer += TypeSizes.INT;
                  
                    OffsetServer = *((int*)Buffer);
                    Buffer += TypeSizes.INT;                 
                }
                else
                    throw new Exception("RooVersion too old, got: " + RooVersion + " Expected greater " + RooFile.MINVERSION);
            }
            else
                throw new Exception("Wrong file signature: " + signature + " (expected " + SIGNATURE + ").");

            //
            // Client part (was encrypted once)
            // 

            // Check if file is encrypted
            if (*((uint*)Buffer) == ENCRYPTIONFLAG)
            {
#if WINCLR && X86
                EncryptionEnabled = true;
                Buffer += TypeSizes.INT;

                // get additional values for decryption
                EncryptedStreamLength = *((int*)Buffer);
                Buffer += TypeSizes.INT;

                ExpectedResponse = *((uint*)Buffer);
                Buffer += TypeSizes.INT;

                Crush32.Decrypt(Buffer, EncryptedStreamLength, Challenge, ExpectedResponse, GetPassword());
#else
                throw new Exception(ERRORCRUSHPLATFORM); 
#endif               
            }
            else
                EncryptionEnabled = false;

            // Get the basic infos, like room dimension and section offsets
            RoomSizeX = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            RoomSizeY = *((uint*)Buffer);
            Buffer += TypeSizes.INT;

            OffsetBSPTree = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            OffsetWalls = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            OffsetWallsEditor = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            OffsetSideDefs = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            OffsetSectors = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            OffsetThings = *((int*)Buffer);
            Buffer += TypeSizes.INT;

            // used to save section item counts
            ushort len;

            // Section 1: BSP-Tree
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            BSPTree.Clear();
            BSPTree.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooBSPItem bspItem = RooBSPItem.ExtractBSPItem(RooVersion, ref Buffer);
                
                BSPTree.Add(bspItem);
            }

            // Section 2: Walls
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Walls.Clear();
            Walls.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooWall lineDef = new RooWall(RooVersion, ref Buffer);
                
                lineDef.Num = i + 1;
                Walls.Add(lineDef);
            }

            // Section 3: WallsEditor
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            WallsEditor.Clear();
            WallsEditor.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooWallEditor wallEditor = new RooWallEditor(ref Buffer);

                wallEditor.Num = i + 1;
                WallsEditor.Add(wallEditor);
            }

            // Section 4: SideDefs
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            SideDefs.Clear();
            SideDefs.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooSideDef sideDef = new RooSideDef(ref Buffer);
                
                sideDef.Num = i + 1;
                sideDef.TextureChanged += OnSideTextureChanged;
                SideDefs.Add(sideDef);
            }

            // Section 5: Sectors
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            bool hasSpeed = (RooVersion >= VERSIONSPEED);
            Sectors.Clear();
            Sectors.Capacity = len + 10;
            for (int i = 0; i < len; i++)
            {
                RooSector sectorDef = new RooSector(RooVersion, ref Buffer, hasSpeed);
                sectorDef.TextureChanged += OnSectorTextureChanged;
                sectorDef.Moved += OnSectorMoved;
                sectorDef.Num = i + 1;
                Sectors.Add(sectorDef);
            }

            // Section 6: Things
            // There is old and new variant of "thing"
            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            Things.Clear();
            Things.Capacity = len + 10;
            if (len > 2)
            {
                for (int i = 0; i < len; i++)
                    Things.Add(new RooThingExtended(ref Buffer));
            }
            else
            {
                for (int i = 0; i < len; i++)
                    Things.Add(new RooThing(ref Buffer));
            }

            // some older maps don't have the roomid
            // so if we've already reached the serveroffset, set it to 0
            if (Buffer - readStartPtr == OffsetServer)
            {
                RoomID = 0;
            }
            else
            {
                // roomid
                RoomID = *((int*)Buffer);
                Buffer += TypeSizes.INT;
            }

            //
            // Server part
            // 

            Grids = new RooGrids(RooVersion, ref Buffer);
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

        #region IGameFile
        public unsafe void Load(string FilePath, byte[] Buffer = null)
        {
            Filename = Path.GetFileNameWithoutExtension(FilePath);

            if (Buffer == null)
               Buffer = File.ReadAllBytes(FilePath);
            
            //ReadFrom(FileBytes, 0);
            fixed (byte* ptrBytes = Buffer)
            {
                byte* ptr = ptrBytes;
                ReadFrom(ref ptr);
            }

            // resolve links (creates object references in models out of loaded indices)
            ResolveIndices();

            // more after load code
            DoCalculations();
        }

        public void Save(string FilePath)
        {
            File.WriteAllBytes(FilePath, Bytes);
        }
        
        public string Filename { get; set; }        
        #endregion

        #region Fields
        protected readonly List<RooBSPItem> bspTree = new List<RooBSPItem>();
        protected readonly List<RooWall> walls = new List<RooWall>();
        protected readonly List<RooWallEditor> wallsEditor = new List<RooWallEditor>();
        protected readonly List<RooSideDef> sideDefs = new List<RooSideDef>();
        protected readonly List<RooSector> sectors = new List<RooSector>();
        protected readonly List<RooThing> things = new List<RooThing>();
        protected readonly IntersectInfo[] intersections = new IntersectInfo[MAXINTERSECTIONS];
        protected int intersectionsCount = 0;
        #endregion

        #region Properties
        /// <summary>
        /// Version of this file/instance.
        /// </summary>
        public uint RooVersion { get; set; }

        /// <summary>
        /// Checksum against room manipulation.
        /// </summary>
        public uint Challenge { get; set; }

        /// <summary>
        /// The offset of the client data sections start
        /// </summary>
        public uint OffsetClient { get; private set; }
        
        /// <summary>
        /// The offset of the server data section to start
        /// </summary>
        public int OffsetServer { get; set; }

        /// <summary>
        /// Whether or not encryption is enabled.
        /// </summary>
        public bool EncryptionEnabled { get; set; }

        /// <summary>
        /// Length of encrypted stream.
        /// </summary>
        public int EncryptedStreamLength { get; protected set; }

        /// <summary>
        /// Used to verify decryption password.
        /// </summary>
        public uint ExpectedResponse { get; protected set; }
        
        /// <summary>
        /// Length of the room.
        /// </summary>
        public uint RoomSizeX { get; set; }
        
        /// <summary>
        /// Width of the room.
        /// </summary>
        public uint RoomSizeY { get; set; }

        /// <summary>
        /// Offset of BSPTree section in bytes.
        /// </summary>
        public int OffsetBSPTree { get; protected set; }

        /// <summary>
        /// Offset of walls section in bytes.
        /// </summary>
        public int OffsetWalls { get; protected set; }

        /// <summary>
        /// Offset of walls (used in windeu) section in bytes.
        /// </summary>
        public int OffsetWallsEditor { get; protected set; }

        /// <summary>
        /// Offset of SideDefs section in bytes.
        /// </summary>
        public int OffsetSideDefs { get; protected set; }

        /// <summary>
        /// Offset of sectors section in bytes.
        /// </summary>
        public int OffsetSectors { get; protected set; }

        /// <summary>
        /// Offset of Things section in bytes.
        /// </summary>
        public int OffsetThings { get; protected set; }
        
        /// <summary>
        /// The populated BSP tree for this room.
        /// </summary>
        public List<RooBSPItem> BSPTree { get { return bspTree; } }
       
        /// <summary>
        /// Existing walls/linedefs in this room, as used by the client.
        /// </summary>
        public List<RooWall> Walls { get { return walls; } }

        /// <summary>
        /// Existing walls/linedefs in this room, as used by WINDEU.
        /// </summary>
        public List<RooWallEditor> WallsEditor { get { return wallsEditor; } }
        
        /// <summary>
        /// Wall-sides in the room.
        /// Each Wall has 2 sides (with up to 3 parts).
        /// </summary>
        public List<RooSideDef> SideDefs { get { return sideDefs; } }
        
        /// <summary>
        /// Sectors in a room are composed by referenced SubSectors (convex-polygons).
        /// SubSectors are part of the BSP-tree.
        /// </summary>
        public List<RooSector> Sectors { get { return sectors; } }

        /// <summary>
        /// Things data.
        /// </summary>
        public List<RooThing> Things { get { return things; } }
        
        /// <summary>
        /// RoomID (Note: This is mostly unset or uncorrect).
        /// WinDEU always saves it (save.cpp), but only loads it for NumThings > 2 (load.cpp)
        /// </summary>
        public int RoomID { get; set; }

        /// <summary>
        /// 2D server grids for the room
        /// </summary>
        public RooGrids Grids { get; set; }

        /// <summary>
        /// Returns all BSP tree-nodes of type RooPartitionLine
        /// from BSPTree property.
        /// </summary>
        public List<RooPartitionLine> BSPTreeNodes
        {
            get
            {
                List<RooPartitionLine> list = new List<RooPartitionLine>();

                foreach (RooBSPItem item in BSPTree)
                {
                    if (item.Type == RooBSPItem.NodeType.Node)
                        list.Add((RooPartitionLine)item);
                }

                return list;
            }
        }

        /// <summary>
        /// Returns all BSP tree-nodes of type RooSubSector
        /// from BSPTree property.
        /// </summary>
        public List<RooSubSector> BSPTreeLeaves
        {
            get
            {
                List<RooSubSector> list = new List<RooSubSector>();

                foreach (RooBSPItem item in BSPTree)
                {
                    if (item.Type == RooBSPItem.NodeType.Leaf)
                        list.Add((RooSubSector)item);
                }

                return list;
            }
        }

        /// <summary>
        /// True if the indices read from file have been resolved to object references.
        /// See ResolveIndices().
        /// </summary>
        public bool IsIndicesResolved { get; protected set; }
        
        /// <summary>
        /// True if the required texture resources have been resolved to references.
        /// See ResolveResources().
        /// </summary>
        public bool IsResourcesResolved { get; protected set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public RooFile()
        {
        }

        /// <summary>
        /// Constructor by file
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Buffer"></param>
        public RooFile(string FilePath, byte[] Buffer = null)
        {
            Load(FilePath, Buffer);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Gets password based on Version
        /// </summary>
        /// <returns></returns>
        public byte[] GetPassword()
        {
            // ROO versions 9/10/11
            if (RooVersion <= 11)
                return PASSWORDV10;

            // all others
            else return PASSWORDV12;
        }

        /// <summary>
        /// Resolves all index references to real object references
        /// </summary>
        public virtual void ResolveIndices()
        {
            // clear all side->walls refs before resolving again
            foreach (RooSideDef side in SideDefs)
            {
                side.WallsLeft.Clear();
                side.WallsRight.Clear();
            }

            // clear all sector->leafs refs before resolving again
            foreach (RooSector sector in Sectors)
                sector.Leafs.Clear();

            foreach (RooWall wall in Walls)
                wall.ResolveIndices(this);

            foreach (RooBSPItem item in BSPTree)
                item.ResolveIndices(this);

            IsIndicesResolved = true;
        }

        /// <summary>
        /// Uses a ResourceManager to resolve/load all resources (textures) references from
        /// </summary>
        /// <param name="M59ResourceManager"></param>
        public virtual void ResolveResources(ResourceManager M59ResourceManager)
        {
            foreach (RooSector sector in Sectors)
                sector.ResolveResources(M59ResourceManager, false);

            foreach (RooSideDef wallSide in SideDefs)
                wallSide.ResolveResources(M59ResourceManager, false);

            IsResourcesResolved = true;
        }

        /// <summary>
        /// Uncompresses all attached resources
        /// </summary>
        public void UncompressAll()
        {
            foreach (RooSector sector in Sectors)
            {
                if (sector.ResourceCeiling != null)
                    foreach (BgfBitmap bgf in sector.ResourceCeiling.Frames)
                        if (bgf.IsCompressed)
                            bgf.IsCompressed = false;

                if (sector.ResourceFloor != null)
                    foreach (BgfBitmap bgf in sector.ResourceFloor.Frames)
                        if (bgf.IsCompressed)
                            bgf.IsCompressed = false;
            }

            foreach (RooSideDef side in SideDefs)
            {
                if (side.ResourceUpper!= null)
                    foreach (BgfBitmap bgf in side.ResourceUpper.Frames)
                        if (bgf.IsCompressed)
                            bgf.IsCompressed = false;

                if (side.ResourceMiddle != null)
                    foreach (BgfBitmap bgf in side.ResourceMiddle.Frames)
                        if (bgf.IsCompressed)
                            bgf.IsCompressed = false;

                if (side.ResourceLower != null)
                    foreach (BgfBitmap bgf in side.ResourceLower.Frames)
                        if (bgf.IsCompressed)
                            bgf.IsCompressed = false;
            }          
        }

        /// <summary>
        /// Runs all additional computations (i.e. wall heights)
        /// </summary>
        public virtual void DoCalculations()
        {
            foreach (RooWall wall in Walls)
                wall.CalculateWallSideHeights();

            foreach (RooSubSector leaf in BSPTreeLeaves)
            {
                leaf.UpdateVertexPositions(true);
                leaf.UpdateVertexPositions(false);
                leaf.UpdateVertexUV(true);
                leaf.UpdateVertexUV(false);
                leaf.UpdateNormals();
            }

            foreach (RooPartitionLine line in BSPTreeNodes)
            {
                line.NormalizeKoefficients();
            }
        }

        /// <summary>
        /// Looks up the height of a point from the sector it's included in.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>        
        /// <param name="SubSector">The subsector the point was found in or NULL</param>
        /// <param name="IsFloor"></param>
        /// <param name="WithSectorDepth"></param>
        /// <returns>Height of point or -1 if no sector found for point</returns>
        public Real GetHeightAt(Real x, Real y, out RooSubSector SubSector, bool IsFloor = true, bool WithSectorDepth = false)
        {
            SubSector = null;

            // walk BSP tree and get subsector containing point
            SubSector = GetSubSectorAt(x, y);

            if (SubSector != null && SubSector.Sector != null)
            {
                if (IsFloor)
                    return SubSector.Sector.CalculateFloorHeight(x, y, WithSectorDepth);

                else
                    return SubSector.Sector.CalculateCeilingHeight(x, y);
            }

            return -1.0f;
        }
        
        /// <summary>
        /// Returns the subsector containing a given point in ROO coordinates.
        /// Starts searching from Root node.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>SubSector (leaf) or null</returns>
        public RooSubSector GetSubSectorAt(Real x, Real y)
        {           
            if (BSPTree.Count <= 0)
                return null;

            RooBSPItem node = BSPTree[0];
            while(node != null)
            {
                // traverse down the tree
                if (node.Type == RooBSPItem.NodeType.Node)
                {
                    RooPartitionLine line = (RooPartitionLine)node;
                    int side = Math.Sign(line.A * x + line.B * y + line.C);
                    node = (side >= 0) ? line.RightChild : line.LeftChild;
                }

                // found leaf point belongs to
                else if (node.Type == RooBSPItem.NodeType.Leaf)
                    return (RooSubSector)node;

                // unknown node-type, abort
                else
                    return null;

            }

            // should not get here
            return null;
        }

        /// <summary>
        /// Returns the sidedef matching the given server id
        /// </summary>
        /// <param name="ServerID"></param>
        /// <returns></returns>
        public RooSideDef GetSideByServerID(int ServerID)
        {
            foreach (RooSideDef sideDef in SideDefs)
                if (sideDef.ServerID == ServerID)
                    return sideDef;
            
            return null;
        }
        
        /// <summary>
        /// Calculates a two dimensional boundingbox for this room
        /// on the fly - either based on walls or editor walls.
        /// </summary>
        /// <param name="UseClientWalls"></param>
        /// <returns>
        /// If UseClientWalls=true, BoundingBox in 1:1024 scale based on RooWalls,
        /// else BoundingBox in 1:64 based on RooEditorWalls
        /// </returns>
        public BoundingBox2D GetBoundingBox2D(bool UseClientWalls = true)
        {
            BoundingBox2D box;

            // must have at least a wall 'or' editorwall
            if ((UseClientWalls && Walls.Count == 0) || (!UseClientWalls && WallsEditor.Count == 0))
                return BoundingBox2D.NULL;

            // build based on clientwalls (1:1024)
            if (UseClientWalls)
            {
                box = new BoundingBox2D(Walls[0].P1, Walls[0].P2);

                for(int i = 1; i < Walls.Count; i++)
                {
                    box.ExtendByPoint(Walls[i].P1);
                    box.ExtendByPoint(Walls[i].P2);
                }
            }

            // build based on editorwalls (1:64)
            else
            {
                box = new BoundingBox2D(WallsEditor[0].P0, WallsEditor[0].P1);

                for(int i = 1; i < WallsEditor.Count; i++)
                {
                    box.ExtendByPoint(WallsEditor[i].P0);
                    box.ExtendByPoint(WallsEditor[i].P1);
                }
            }

            return box;          
        }

        /// <summary>
        /// Calculates a three dimensional boundingbox for this room
        /// on the fly - either based on walls or editor walls and on sectors.
        /// Note: Z is the height.
        /// </summary>
        /// <returns>
        /// If UseClientWalls=true, BoundingBox in 1:1024 scale based on RooWalls,
        /// else BoundingBox in 1:64 based on RooEditorWalls
        /// </returns>
        public BoundingBox3D GetBoundingBox3D(bool UseClientWalls = true)
        {
            // must have at least a sector
            if (Sectors.Count == 0)
                return BoundingBox3D.NULL;

            // get 2D boundingbox
            BoundingBox2D box2D = GetBoundingBox2D(UseClientWalls);

            // zero 2d box -> zero 3d box
            if (box2D == BoundingBox2D.NULL)
                return BoundingBox3D.NULL;

            // initial 3d box based on 2d box and first sector
            BoundingBox3D box3D = new BoundingBox3D(
                new V3(box2D.Min.X, box2D.Min.Y, Sectors[0].FloorHeight),
                new V3(box2D.Max.X, box2D.Max.Y, Sectors[0].CeilingHeight));

            // extend by sector heights
            for(int i = 1; i < Sectors.Count; i++)
            {
                box3D.ExtendByPoint(new V3(box3D.Min.X, box3D.Min.Y, Sectors[i].FloorHeight));
                box3D.ExtendByPoint(new V3(box3D.Max.X, box3D.Max.Y, Sectors[i].CeilingHeight));
            }
           
            // scale height from kod/editor fineness (1:64) to oldclient fineness (1:1024)
            if (UseClientWalls)
            {
                box3D.Min.Z *= 16f;
                box3D.Max.Z *= 16f;
            }

            return box3D;
        }

        /// <summary>
        /// Returns the two dimensional boundingbox as stored
        /// in Things section.
        /// </summary>
        /// <returns></returns>
        public BoundingBox2D GetBoundingBox2DFromThings()
        {
            if (Things.Count < 2)
                return BoundingBox2D.NULL;

            BoundingBox2D box;
            box.Min.X = (Real)Things[0].PositionX;
            box.Min.Y = (Real)Things[0].PositionY;
            box.Max.X = (Real)Things[0].PositionX;
            box.Max.Y = (Real)Things[0].PositionY;
            box.Min.X = Math.Min(box.Min.X, (Real)Things[1].PositionX);
            box.Min.Y = Math.Min(box.Min.Y, (Real)Things[1].PositionY);
            box.Max.X = Math.Max(box.Max.X, (Real)Things[1].PositionX);
            box.Max.Y = Math.Max(box.Max.Y, (Real)Things[1].PositionY);
            
            return box;
        }

        /// <summary>
        /// Processes updates at runtime, like animations.
        /// Call regularly.
        /// </summary>
        /// <param name="Tick"></param>
        /// <param name="Span"></param>
        public void Tick(double Tick, double Span)
        {
            // update wall animations
            foreach (RooSideDef wallSide in SideDefs)
                if (wallSide.Animation != null)
                    wallSide.Animation.Tick(Tick, Span);

            // update sectors
            foreach (RooSector sector in Sectors)
                sector.Tick(Tick, Span);
        }

        /// <summary>
        /// Returns a MaterialInfo dictionary with all materials used in this roofile.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, MaterialInfo> GetMaterialInfos()
        {
            Dictionary<string, MaterialInfo> list =
                new Dictionary<string, MaterialInfo>();
            
            // add materials used on sector floors & ceilings
            foreach (RooSector obj in Sectors)
            {
                if (obj.MaterialNameFloor != null && 
                    obj.MaterialNameFloor != String.Empty && 
                    !list.ContainsKey(obj.MaterialNameFloor))
                { 
                    list.Add(obj.MaterialNameFloor, new MaterialInfo(
                        obj.TextureFloor,
                        obj.TextureNameFloor,
                        obj.MaterialNameFloor,
                        obj.SpeedFloor));
                }

                if (obj.MaterialNameCeiling != null && 
                    obj.MaterialNameCeiling != String.Empty && 
                    !list.ContainsKey(obj.MaterialNameCeiling))
                { 
                    list.Add(obj.MaterialNameCeiling, new MaterialInfo(
                        obj.TextureCeiling,
                        obj.TextureNameCeiling,
                        obj.MaterialNameCeiling,
                        obj.SpeedCeiling));
                }
            }

            // add materials used on sides
            foreach (RooSideDef obj in SideDefs)
            {
                if (obj.MaterialNameLower != null && 
                    obj.MaterialNameLower != String.Empty && 
                    !list.ContainsKey(obj.MaterialNameLower))
                { 
                    list.Add(obj.MaterialNameLower, new MaterialInfo(
                        obj.TextureLower,
                        obj.TextureNameLower,
                        obj.MaterialNameLower,
                        obj.SpeedLower));
                }

                if (obj.MaterialNameMiddle != null && 
                    obj.MaterialNameMiddle != String.Empty &&
                    !list.ContainsKey(obj.MaterialNameMiddle))
                {
                    list.Add(obj.MaterialNameMiddle, new MaterialInfo(
                        obj.TextureMiddle,
                        obj.TextureNameMiddle,
                        obj.MaterialNameMiddle,
                        obj.SpeedMiddle));
                }

                if (obj.MaterialNameUpper != null && 
                    obj.MaterialNameUpper != String.Empty &&
                    !list.ContainsKey(obj.MaterialNameUpper))
                { 
                    list.Add(obj.MaterialNameUpper, new MaterialInfo(
                        obj.TextureUpper,
                        obj.TextureNameUpper,
                        obj.MaterialNameUpper,
                        obj.SpeedUpper));
                }
            }

            return list;
        }

        /// <summary>
        /// Checks a movement vector (Start->End) for wall collisions and
        /// returns a possibly adjusted movement vector to use instead.
        /// This might be a slide along a wall or a zero-vector for a full block.
        /// </summary>
        /// <param name="Start">Startpoint of movement. In FINENESS units (1:1024), Y is height.</param>
        /// <param name="End">Endpoint of movement. In FINENESS units (1:1024).</param>
        /// <param name="Speed">Speed the move is applied with</param>
        /// <returns>Same or adjusted delta vector between Start and end (in ROO coords)</returns>
        public V2 VerifyMove(ref V3 Start, ref V2 End, Real Speed)
        {                       
            V2 Start2D = new V2(Start.X, Start.Z);
            V2 newend = End;           
            V2 rot;
            RooWall wall;

            const int MAXATTEMPTS = 8;
            const Real ANGLESTEP = GeometryConstants.QUARTERPERIOD / 8;

            /**************************************************************/

            // no collision with untouched move
            if (CanMoveInRoom(ref Start2D, ref End, Start.Y, Speed, out wall))
                return End - Start2D;

            /**************************************************************/

            // try to slide along collision wall
            newend  = wall.SlideAlong(Start2D, End);

            // no collision with 'slide along' move
            if (CanMoveInRoom(ref Start2D, ref newend, Start.Y, Speed, out wall))
                return newend - Start2D;

            /**************************************************************/

            // sliding on collision walls does not work, try rotate a bit
            for(int i = 0; i < MAXATTEMPTS; i++)
            {
                rot = End - Start2D;
                rot.Rotate(-ANGLESTEP * (Real)i);
                newend = Start2D + rot;

                // no collision
                if (CanMoveInRoom(ref Start2D, ref newend, Start.Y, Speed, out wall))
                    return newend - Start2D;

                rot = End - Start2D;
                rot.Rotate(ANGLESTEP * (Real)i);
                newend = Start2D + rot;

                // no collision
                if (CanMoveInRoom(ref Start2D, ref newend, Start.Y, Speed, out wall))
                    return newend - Start2D;

            }

            return new V2(0.0f, 0.0f);
        }

        /// <summary>
        /// Swaps two entries in the intersections list
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        protected void IntersectionsSwap(int a, int b)
        {
            IntersectInfo temp = intersections[a];
            intersections[a] = intersections[b];
            intersections[b] = temp;
        }
        
        /// <summary>
        /// QuickSorts the intersections list based on Distance2
        /// </summary>
        /// <param name="beg"></param>
        /// <param name="end"></param>
        protected void IntersectionsSort(int beg, int end)
        {
            // Recursive QuickSort implementation
            // sorting (potential) intersections by squared distance from start

            if (end > beg + 1)
            {
               IntersectInfo piv = intersections[beg];
               int l = beg + 1, r = end;
               while (l < r)
               {
                  if (intersections[l].Distance2 < piv.Distance2
                       || (intersections[l].Distance2 == piv.Distance2
                           && intersections[l].FloorHeight < piv.FloorHeight))
                     l++;
                  else
                     IntersectionsSwap(l, --r);
               }
               IntersectionsSwap(--l, beg);
               IntersectionsSort(beg, l);
               IntersectionsSort(r, end);
            }
        }

        /// <summary>
        /// Used in CanMoveInRoomTree
        /// </summary>
        protected bool CanMoveInRoomTree3DInternal(RooSector SectorS, RooSector SectorE, RooSideDef SideS, RooSideDef SideE, RooWall Wall, ref V2 Q)
        {
            // block moves with end outside
            if (SectorE == null || SideE == null)
               return false;

            // sides which have no passable flag set always block
            if (SideS != null && !SideS.Flags.IsPassable)
               return false;
#if !VANILLA && !OPENMERIDIAN
            // endsector must not be marked SF_NOMOVE
            if (SectorE.Flags.IsNoMove)
               return false;
#endif
            // get floor and ceiling height at end
            Real hFloorE = SectorE.CalculateFloorHeight(Q.X, Q.Y, true);
            Real hCeilingE = SectorE.CalculateCeilingHeight(Q.X, Q.Y);

            // check endsector height
            if (hCeilingE - hFloorE < GeometryConstants.OBJECTHEIGHTROO)
               return false;

            // get floor height at start
            Real hFloorS = (SectorS != null) ? SectorS.CalculateFloorHeight(Q.X, Q.Y, true) : 0.0f;

            if (intersectionsCount >= MAXINTERSECTIONS)
            {
               Logger.Log("RooFile", LogType.Error, "Too many intersections in CanMoveInRoomTree3DInternal");
               return true;
            }

            // must evaluate heights at transition later
            intersections[intersectionsCount].Set(SectorS, SectorE, SideS, SideE, Wall, Q, hFloorS, 0.0f);
            intersectionsCount++;

            return true;
        }

        /// <summary>
        /// Used in CanMoveInRoom
        /// </summary>
        protected bool CanMoveInRoomTree(RooBSPItem Node, ref V2 S, ref V2 E, out RooWall BlockWall)
        {
            // reached a leaf or nullchild, movements not blocked by leafs
            if (Node == null || Node.Type != RooBSPItem.NodeType.Node)
            {
               BlockWall = null;
               return true;
            }

            /****************************************************************/

            RooPartitionLine line = (RooPartitionLine)Node;

            // get signed distances from splitter to both endpoints of move
            Real distS = line.GetDistance(ref S);
            Real distE = line.GetDistance(ref E);

            /****************************************************************/
            // both endpoints far away enough on positive (right) side
            // --> climb down only right subtree
            if ((distS > GeometryConstants.WALLMINDISTANCE) && (distE > GeometryConstants.WALLMINDISTANCE))
               return CanMoveInRoomTree(line.RightChild, ref S, ref E, out BlockWall);

            // both endpoints far away enough on negative (left) side
            // --> climb down only left subtree
            else if ((distS < -GeometryConstants.WALLMINDISTANCE) && (distE < -GeometryConstants.WALLMINDISTANCE))
               return CanMoveInRoomTree(line.LeftChild, ref S, ref E, out BlockWall);

            // endpoints are on different sides, or one/both on infinite line or potentially too close
            // --> check walls of splitter first and then possibly climb down both subtrees
            else
            {
               RooSideDef sideS;
               RooSector sectorS;
               RooSideDef sideE;
               RooSector sectorE;

               // CASE 1) The move line actually crosses this infinite splitter.
               // This case handles long movelines where S and E can be far away from each other and
               // just checking the distance of E to the line would fail.
               // q contains the intersection point
               if (((distS > 0.0f) && (distE < 0.0f)) ||
                   ((distS < 0.0f) && (distE > 0.0f)))
               {
                  // intersect finite move-line SE with infinite splitter line
                  // q stores possible intersection point
                  V2 q;
                  if (line.IntersectWithFiniteLine(ref S, ref E, out q, 0.1f))
                  {
                     // iterate finite segments (walls) in this splitter
                     RooWall wall = line.Wall;
                     while (wall != null)
                     {
                        // OLD: infinite intersection point must also be in bbox of wall
                        // otherwise no intersect
                        //if (!wall.IsInsideBoundingBox(ref q, 0.1f))
                        // NEW: Check if the line of the wall intersects a circle consisting
                        // of player x, y and radius of min distance allowed to walls. Intersection
                        // includes the wall being totally inside the circle.
                        V2 p1 = wall.P1;
                        V2 p2 = wall.P2;
                        if (!MathUtil.IntersectOrInsideLineCircle(ref p1, ref p2, ref q, GeometryConstants.WALLMINDISTANCE))
                        {
                           wall = wall.NextWallInPlane;
                           continue;
                        }

                        // set from and to sector / side
                        if (distS > 0.0f)
                        {
                           sideS = wall.RightSide;
                           sectorS = wall.RightSector;
                        }
                        else
                        {
                           sideS = wall.LeftSide;
                           sectorS = wall.LeftSector;
                        }

                        if (distE > 0.0f)
                        {
                           sideE = wall.RightSide;
                           sectorE = wall.RightSector;
                        }
                        else
                        {
                           sideE = wall.LeftSide;
                           sectorE = wall.LeftSector;
                        }

                        // check the transition data for this wall, use intersection point q
                        bool ok = CanMoveInRoomTree3DInternal(sectorS, sectorE, sideS, sideE, wall, ref q);

                        if (!ok)
                        {
                           BlockWall = wall;
                           return false;
                        }

                        wall = wall.NextWallInPlane;
                     }
                  }
               }

               // CASE 2) The move line does not cross the infinite splitter, both move endpoints are on the same side.
               // This handles short moves where walls are not intersected, but the endpoint may be too close
               else
               {
                  // check only getting closer
                  if (Math.Abs(distE) <= Math.Abs(distS))
                  {
                     // iterate finite segments (walls) in this splitter
                     RooWall wall = line.Wall;
                     while (wall != null)
                     {
                        V2 p1 = wall.P1;
                        V2 p2 = wall.P2;
                        int useCase;

                        // get min. squared distance from move endpoint to line segment
                        Real dist2 = E.MinSquaredDistanceToLineSegment(ref p1, ref p2, out useCase);

                        // skip if far enough away
                        if (dist2 > GeometryConstants.WALLMINDISTANCE2)
                        {
                           wall = wall.NextWallInPlane;
                           continue;
                        }

                        // q stores closest point on line
                        V2 q;
                        if (useCase == 1)      q = p1; // p1 is closest
                        else if (useCase == 2) q = p2; // p2 is closest
                        else
                        {
                           float sign = (distE < 0.0f) ? 1.0f : -1.0f; // pick the correct sign (one the two possible normals)
                           V2 normal = new V2(line.A, line.B) * sign;  // normal = normalized line eq. coefficients
                           q = E + (normal * (Real)Math.Abs(distE));   // q=E moved along the normal onto the line
                        }

                        // set from and to sector / side
                        // for case 2 (too close) these are based on (S),
                        // and (E) is assumed to be on the other side.
                        if (distS >= 0.0f)
                        {
                           sideS = wall.RightSide;
                           sectorS = wall.RightSector;
                           sideE = wall.LeftSide;
                           sectorE = wall.LeftSector;
                        }
                        else
                        {
                           sideS = wall.LeftSide;
                           sectorS = wall.LeftSector;
                           sideE = wall.RightSide;
                           sectorE = wall.RightSector;
                        }

                        // check the transition data for this wall, use E for intersectpoint
                        bool ok = CanMoveInRoomTree3DInternal(sectorS, sectorE, sideS, sideE, wall, ref q);

                        if (!ok)
                        {
                           BlockWall = wall;
                           return false;
                        }

                        wall = wall.NextWallInPlane;
                     }
                  }
               }
            }

            /****************************************************************/

            // try right subtree first
            bool retval = CanMoveInRoomTree(line.RightChild, ref S, ref E, out BlockWall);

            // found a collision there? return it
            if (!retval)
               return retval;

            // otherwise try left subtree
            return CanMoveInRoomTree(line.LeftChild, ref S, ref E, out BlockWall);
        }

        /// <summary>
        /// Test if can move from S to E using Speed starting at Height
        /// </summary>
        public bool CanMoveInRoom(ref V2 S, ref V2 E, Real Height, Real Speed, out RooWall BlockWall)
        {
           // clear old intersections
           intersectionsCount = 0;

           if (!CanMoveInRoomTree(BSPTreeNodes[0], ref S, ref E, out BlockWall))
              return false;

           // but still got to validate height transitions list
           // calculate the squared distances
           for (int i = 0; i < intersectionsCount; i++)
              intersections[i].CalcDist2(ref S);
            
           // sort the potential intersections by squared distance from start
           IntersectionsSort(0, intersectionsCount);

           // iterate from intersection to intersection, starting and start and ending at end
           // check the transition data for each one, use intersection point q
           Real distanceDone = 0.0f;
           Real heightModified = Height;
           V2 p = S;
           for (int i = 0; i < intersectionsCount; i++)
           {
              IntersectInfo transit = intersections[i];

              // deal with null start sector/side
              if (transit.SideS == null || transit.SectorS == null)
              {
                 if (i == 0) break; // allow if first segment (start outside)
               
                 // deny otherwise (segment on the line outside)
                 else
                 {
                    BlockWall = transit.Wall;
                    return false; 
                 }
              }

              // get floor heights
              Real hFloorSP = transit.SectorS.CalculateFloorHeight(p.X, p.Y, true);
              Real hFloorSQ = transit.SectorS.CalculateFloorHeight(transit.Q.X, transit.Q.Y, true);
              Real hFloorEQ = transit.SectorE.CalculateFloorHeight(transit.Q.X, transit.Q.Y, true);

              // squared length of this segment
              Real stepLen2 = transit.Distance2 - distanceDone;

              // reduce height by what we lose across the SectorS from p to q
              if (heightModified > hFloorSP)
              {
                 // workaround div by 0, todo? these are teleports
                 if (Speed < 0.001f && Speed > -0.001f) Speed = 9999999;

                 // S = 1/2*a*t² where t=dist/speed
                 // S = 1/2*a*(dist²/speed²)
                 // t² = (dist/speed)² = dist²/speed²
                 Real stepDt2 = stepLen2 / (Speed*Speed);
                 Real stepFall = 0.5f * GeometryConstants.GRAVITYACCELERATION * stepDt2;

                 // apply fallheight
                 heightModified += stepFall;
              }

              // too far below sector
              else if (heightModified < (hFloorSP - GeometryConstants.MAXSTEPHEIGHT))
              {
                 BlockWall = transit.Wall;
                 return false;
              }

              // make sure we're at least at startsector's groundheight at Q when we reach Q from P
              // in case we stepped up or fell below it
              heightModified = MathUtil.Max(hFloorSQ, heightModified);

              // check stepheight (this also requires a lower texture set)
              //if (transit->SideS->TextureLower > 0 && (hFloorE - hFloorQ > MAXSTEPHEIGHT))
              if (transit.SideS.LowerTexture > 0 && (hFloorEQ - heightModified > GeometryConstants.MAXSTEPHEIGHT))
              {
                 BlockWall = transit.Wall;
                 return false;
              }

              // get ceiling height
              Real hCeilingEQ = transit.SectorE.CalculateCeilingHeight(transit.Q.X, transit.Q.Y);

              // check ceilingheight (this also requires an upper texture set)
              if (transit.SideS.UpperTexture > 0 && (hCeilingEQ - hFloorSQ < GeometryConstants.OBJECTHEIGHTROO))
              {
                 BlockWall = transit.Wall;
                 return false;
              }

              // we actually made it across that intersection
              heightModified = MathUtil.Max(hFloorEQ, heightModified); // keep our height or set it at least to sector
              distanceDone += stepLen2;                                // add squared length of processed segment
              p = transit.Q;                                           // set end to next start
         }

           return true;
        }

        /// <summary>
        /// Verifies Line of Sight from Start to End.
        /// Checking against Walls only (no sectors/ceilings).
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="I">Intersection</param>
        /// <returns>True if OK, false if collision.</returns>
        public bool VerifySight(ref V3 Start, ref V3 End, ref V3 I)
        {
            return VerifySightByTree(BSPTree[0], ref Start, ref End, ref I);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="I">Intersection Point</param>
        /// <returns>True if OK, false if collision.</returns>
        protected bool VerifySightByTree(RooBSPItem Node, ref V3 Start, ref V3 End, ref V3 I)
        {
            if (Node == null)
                return true;

            /*************************************************************/

            if (Node.Type == RooBSPItem.NodeType.Leaf)
            {
                RooSubSector leaf = (RooSubSector)Node;
                V3 s = Start.XZY;
                V3 e = End.XZY;

                // check floor
                if (leaf.Sector.FloorTexture > 0 && 
                    leaf.IsBlockingLine(true, ref s, ref e, ref I))
                {
                    return false;
                }

                // check ceiling
                if (leaf.Sector.CeilingTexture > 0 &&
                    leaf.IsBlockingLine(false, ref s, ref e, ref I))
                {
                    return false;
                }

                // not blocked by leaf
                return true;
            }

            /*************************************************************/

            RooPartitionLine line = (RooPartitionLine)Node;
            RooWall wall = line.Wall;
            V2 start2D = Start.XZ;
            V2 end2D = End.XZ;

            /*************************************************************/

            Real startDist  = line.GetDistance(ref start2D);
            Real endDist    = line.GetDistance(ref end2D);

            /*************************************************************/

            // both endpoints on negative side
            if (startDist < 0.0f && endDist < 0.0f)
                return VerifySightByTree(line.LeftChild, ref Start, ref End, ref I);

            // both endpoints on positive side
            else if (startDist > 0.0f && endDist > 0.0f)
                return VerifySightByTree(line.RightChild, ref Start, ref End, ref I);

            // crosses infinite splitter or one or both points on splitter
            else
            {
                // test walls of splitter
                while (wall != null)
                {
                    if (wall.IsBlockingSight(ref Start, ref End, ref I))
                        return false;

                    // loop over next wall in same plane
                    wall = wall.NextWallInPlane;
                }

                // must climb down both subtrees, go left first
                bool wl = VerifySightByTree(line.LeftChild, ref Start, ref End, ref I);

                // return collision if already found
                if (wl == false)
                    return wl;

                // try other subtree otherwise
                else
                    return VerifySightByTree(line.RightChild, ref Start, ref End, ref I);
            }
        }

        /// <summary>
        /// Small helper function which tries to create a heightmap from roomdata.
        /// Not specifically used right now.
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="IsFloor"></param>
        /// <param name="WithSectorDepths"></param>
        /// <returns></returns>
        public Real[] GenerateHeightMap(uint Size, bool IsFloor, bool WithSectorDepths)
	    {		
		    // allocate array to store heights
            Real[] heights = new Real[Size * Size];

            // get the scale factor between longer side and size
            Real scale;
            if (RoomSizeX > RoomSizeY)
            {
                scale = (Real)RoomSizeX / (Real)Size;
            }
            else
            {
                scale = (Real)RoomSizeY / (Real)Size;
            }

		    RooSubSector subsector;

            // iterate rows of heightmap
            for (uint i = 0; i < Size; i++)
            {
                // iterate columns of heightmap
                for (uint j = 0; j < Size; j++)
                {
                    // get mapcoordinates for this pixel
                    Real x = i * scale;
                    Real y = j * scale;

                    // lookup the height from roomdata
                    Real height = GetHeightAt(x, y, out subsector, IsFloor, WithSectorDepths);
                    
                    // process it
                    if (height == -1.0f && (i > 0.0f || j > 0.0f))
                    {
                        // use prece
                        heights[(i * Size) + j] = heights[(i * Size) + j - 1];
                    }
                    else
                    {
                        heights[(i * Size) + j] = height / scale;
                    }
                }
            }

            return heights;
	    }

        /// <summary>
        /// Executed when sector moved.
        /// Raised SectorMoved event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSectorMoved(object sender, EventArgs e)
        {
            RooSector sector = (RooSector)sender;

            if (SectorMoved != null)
                SectorMoved(this, new SectorMovedEventArgs(sector));
        }

        /// <summary>
        /// Executed when walltexture changed, triggers event
        /// </summary>
        protected void OnSideTextureChanged(object sender, WallTextureChangedEventArgs e)
        {
            if (WallTextureChanged != null)
                WallTextureChanged(this, e);
        }

        /// <summary>
        /// Executed when sectortexture changed, triggers event
        /// </summary>
        protected void OnSectorTextureChanged(object sender, SectorTextureChangedEventArgs e)
        {
            if (SectorTextureChanged != null)
                SectorTextureChanged(this, e);
        }
        #endregion

        #region Message handlers
        /// <summary>
        /// Handles some GameMode messages
        /// </summary>
        /// <param name="Message"></param>
        public void HandleGameModeMessage(GameModeMessage Message)
        {
            switch ((MessageTypeGameMode)Message.PI)
            {
                case MessageTypeGameMode.ChangeTexture:
                    HandleChangeTexture((ChangeTextureMessage)Message);
                    break;

                case MessageTypeGameMode.SectorMove:
                    HandleSectorMove((SectorMoveMessage)Message);
                    break;

                case MessageTypeGameMode.WallAnimate:
                    HandleWallAnimateMessage((WallAnimateMessage)Message);
                    break;

                case MessageTypeGameMode.SectorChange:
                    HandleSectorChange((SectorChangeMessage)Message);
                    break;
            }
        }

        /// <summary>
        /// Reset all properties to values from original ROO file
        /// </summary>
        public void Reset()
        {
            // reset sectors
            foreach (RooSector s in Sectors)
                s.Reset();

            // reset sides
            foreach (RooSideDef s in SideDefs)
                s.Reset();

            // todo: reset walls

            // recalculate some stuff for initial state
            DoCalculations();
        }

        /// <summary>
        /// Handle ChangeTextureMessage
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleChangeTexture(ChangeTextureMessage Message)
        {
            TextureChangeInfo info = Message.TextureChangeInfo;

            // TEXTURE CHANGE ON SIDE
            if (info.Flags.IsAboveWall ||
                info.Flags.IsNormalWall ||
                info.Flags.IsBelowWall)
            {
                foreach (RooSideDef side in SideDefs)
                {
                    if (side.ServerID == info.ServerID)
                    {
                        if (info.Flags.IsAboveWall)
                            side.SetUpperTexture(info.TextureNum, info.Resource);

                        else if (info.Flags.IsNormalWall)
                            side.SetMiddleTexture(info.TextureNum, info.Resource);

                        else if (info.Flags.IsBelowWall)
                            side.SetLowerTexture(info.TextureNum, info.Resource);
                    }
                }
            }

            // TEXTURE CHANGE ON SECTOR
            else if (info.Flags.IsCeiling || info.Flags.IsFloor)
            {
                foreach (RooSector sector in Sectors)
                {
                    if (sector.ServerID == info.ServerID)
                    {
                        if (info.Flags.IsFloor)
                            sector.SetFloorTexture(info.TextureNum, info.Resource);

                        else if (info.Flags.IsCeiling)
                            sector.SetCeilingTexture(info.TextureNum, info.Resource);
                    }
                }
            }
        }

        /// <summary>
        /// Handle SectorMoveMessage
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleSectorMove(SectorMoveMessage Message)
        {
            SectorMove info = Message.SectorMove;

            foreach (RooSector sector in Sectors)
            {
                // start / adjust movement on matching sectors
                if (sector.ServerID == info.SectorNr)
                    sector.StartMove(info);
            }
        }

        /// <summary>
        /// Handle SectorChangeMessage
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleSectorChange(SectorChangeMessage Message)
        {
            SectorChange info = Message.SectorChange;

            foreach (RooSector sector in Sectors)
                if (sector.ServerID == info.SectorNr)
                    sector.ApplyChange(info);
        }

        /// <summary>
        /// Handles a wall animate message, updates the according animation object
        /// </summary>
        /// <param name="Message"></param>
        protected void HandleWallAnimateMessage(WallAnimateMessage Message)
        {
            WallAnimationChange info = Message.WallAnimationChange;

            foreach (RooSideDef wallSide in SideDefs)
            {
                if (wallSide.ServerID == info.SideDefServerID)
                    wallSide.ApplyChange(info);
            }
        }
        #endregion

        /// <summary>
        /// Returns a name based on a name convention for textures.
        /// Basically the bgf filename, the frameindex added,
        /// and a dummy extension to potentially load it from a file
        /// Example: "grd00024-0.png"
        /// </summary>
        /// <param name="TextureFile"></param>
        /// <param name="FrameIndex"></param>
        /// <returns></returns>
        public static string GetNameForTexture(BgfFile TextureFile, int FrameIndex)
        {
            return TextureFile.Filename + '-' + FrameIndex + FileExtensions.PNG;
        }

        /// <summary>
        /// Returns a name based on a name convention for Mogre from Bgf.
        /// </summary>
        /// <param name="TextureFile"></param>
        /// <param name="FrameIndex"></param>
        /// <param name="ScrollSpeed"></param>
        /// <returns></returns>
        public static string GetNameForMaterial(BgfFile TextureFile, int FrameIndex, V2 ScrollSpeed)
        {
            // examplereturn (0 is index in bgf)
            // Room/grd00352-0
            string matName = "Room/" + TextureFile.Filename + "-" + FrameIndex;

            // add appendix for scrollspeed
            // Room/grd00352-0-SCROLL-1.0-0.0
            matName += "-SCROLL-" + ScrollSpeed.X.ToString() + "-" + ScrollSpeed.Y.ToString();

            return matName;
        }    
    }
}
