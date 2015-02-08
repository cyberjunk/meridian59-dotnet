/*
 This file was adopted from http://damieng.com/blog/2006/08/08/calculating_crc32_in_c_and_net 
 All credits & copyright go to the original author: Damien Guard (damieng@gmail.com)
*/

using System;
using System.Security.Cryptography;

namespace Meridian59.Common
{
    /// <summary>
    /// Generic CRC32 hash generator.
    /// </summary>
    public class Crc32 : HashAlgorithm
    {
        public const UInt32 DEFAULTPOLYNOMIAL = 0xEDB88320;
        public const UInt32 DEFAULTSEED = 0xFFFFFFFF;

        protected static UInt32[] defaultTable;

        protected UInt32 hash;
        protected UInt32 seed;
        protected UInt32[] table;
        
        public Crc32()
        {
            table = InitializeTable(DEFAULTPOLYNOMIAL);
            seed = DEFAULTSEED;
            Initialize();
        }

        public Crc32(UInt32 Polynomial, UInt32 Seed)
        {
            table = InitializeTable(Polynomial);
            this.seed = Seed;
            Initialize();
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] Buffer, int Start, int Length)
        {
            hash = CalculateHash(table, hash, Buffer, Start, Length);
        }

        protected override byte[] HashFinal()
        {
            byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize
        {
            get { return 32; }
        }

        public static UInt32 Compute(byte[] Buffer)
        {
            return ~CalculateHash(InitializeTable(DEFAULTPOLYNOMIAL), DEFAULTSEED, Buffer, 0, Buffer.Length);
        }

        public static UInt32 Compute(UInt32 Seed, byte[] Buffer)
        {
            return ~CalculateHash(InitializeTable(DEFAULTPOLYNOMIAL), Seed, Buffer, 0, Buffer.Length);
        }

        public static UInt32 Compute(UInt32 Polynomial, UInt32 Seed, byte[] Buffer)
        {
            return ~CalculateHash(InitializeTable(Polynomial), Seed, Buffer, 0, Buffer.Length);
        }

        private static UInt32[] InitializeTable(UInt32 Polynomial)
        {
            if (Polynomial == DEFAULTPOLYNOMIAL && defaultTable != null)
                return defaultTable;

            UInt32[] createTable = new UInt32[256];
            for (int i = 0; i < 256; i++)
            {
                UInt32 entry = (UInt32)i;
                for (int j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ Polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            if (Polynomial == DEFAULTPOLYNOMIAL)
                defaultTable = createTable;

            return createTable;
        }

        private static UInt32 CalculateHash(UInt32[] Table, UInt32 Seed, byte[] Buffer, int Start, int Size)
        {
            UInt32 crc = Seed;
            for (int i = Start; i < Size; i++)
            {
                unchecked
                {
                    crc = (crc >> 8) ^ Table[Buffer[i] ^ crc & 0xFF];
                }
            }

            return crc;
        }

        private byte[] UInt32ToBigEndianBytes(UInt32 X)
        {
            return new byte[] 
            {
			    (byte)((X >> 24) & 0xFF),
			    (byte)((X >> 16) & 0xFF),
			    (byte)((X >> 8) & 0xFF),
			    (byte)(X & 0xFF) 
            };
		}
    }
}
