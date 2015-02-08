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

/* CRUSH32 is only available on Win32 */

#if WINCLR && X86
using System;
using System.Runtime.InteropServices;

namespace Meridian59.Common
{
    /// <summary>
    /// A wrapperclass to access legacy crush32.dll
    /// </summary>
    public unsafe static class Crush32
    {
        #region Private PINVOKE signatures
        /// <summary>
        /// Name of legacy crush32.dll file (put it in same folder!)
        /// </summary>
        public const string DLLName = "crush32.dll";
        
        /// <summary>
        /// The cxERROR for SUCCESS
        /// </summary>
        private const short CX_SUCCESS = 0;

        /// <summary>
        /// Initializes the library
        /// </summary>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxInit();
        
        /// <summary>
        /// Cleanup
        /// </summary>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxCleanup();

        /// <summary>
        /// Initializes a buffer to buffer operation.
        /// </summary>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxBuf2BufInit();

        /// <summary>
        /// Closes a buffer to buffer operation
        /// </summary>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxBuf2BufClose();
       
        /// <summary>
        /// Decrypts data in a buffer.
        /// </summary>
        /// <param name="Buffer">A pointer to the data to be decrypted.</param>
        /// <param name="Length">How many bytes should be decrypted.</param>
        /// <param name="Challenge">The Challenge used to test your password first.</param>
        /// <param name="ExpectedResponse">The ExpectedResponse matching your Challenge and your password.</param>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxBufDecrypt(byte* Buffer, int Length, uint Challenge, uint ExpectedResponse);

        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxBufDecrypt(IntPtr Buffer, int Length, uint Challenge, uint ExpectedResponse);


        /// <summary>
        /// Encrypts data in a buffer.
        /// </summary>
        /// <param name="Buffer">A pointer to the data to be encrypted.</param>
        /// <param name="Length">How many bytes should be encrypted.</param>
        /// <param name="Challenge">The Challenge used to create the returnvalue.</param>
        /// <returns>The "ExpectedResponse" matching your given Challenge and your Password, use this in decryption later.</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint cxBufEncrypt(IntPtr Buffer, int Length, uint Challenge);

        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint cxBufEncrypt(byte* Buffer, int Length, uint Challenge);


        /// <summary>
        /// Sets a password for the encryption and decryption methods.
        /// </summary>
        /// <param name="PtrPasswordString">A pointer to the nullterminated passwordstring in memory.</param>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxSetPassword(IntPtr PtrPasswordString);

        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxSetPassword(byte* Password);

        /// <summary>
        /// Compresses the data from the InputBuffer to the OutputBuffer.
        /// </summary>
        /// <param name="InputBuffer">A pointer to read inputdata from.</param>
        /// <param name="OutputBuffer">A pointer to write outputdata to.</param>
        /// <param name="InputLength">How many bytes should be read from input buffer.</param>
        /// <returns>The length of the compressed data.</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int cxBuf2BufCompress(IntPtr InputBuffer, IntPtr OutputBuffer, int InputLength);

        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int cxBuf2BufCompress(byte* InputBuffer, byte* OutputBuffer, int InputLength);

        /// <summary>
        /// Decompresses the data from the InputBuffer to the OutputBuffer.
        /// </summary>
        /// <param name="InputBuffer">A pointer to read inputdata from.</param>
        /// <param name="OutputBuffer">A pointer to write outputdata to.</param>
        /// <param name="DecompressedLength">The length of the data in decompressed state.</param>
        /// <param name="CompressedLength">The length of the data in compressed state.</param>
        /// <returns>cxERROR statevalue</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxBuf2BufExpand(IntPtr InputBuffer, IntPtr OutputBuffer, int DecompressedLength, int CompressedLength);

        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern short cxBuf2BufExpand(byte* InputBuffer, byte* OutputBuffer, int DecompressedLength, int CompressedLength);

        /// <summary>
        /// Creates a CRC32 for the data in the Buffer.
        /// Note: The used algorithm is equal to
        /// http://damieng.com/blog/2006/08/08/calculating_crc32_in_c_and_net
        /// </summary>
        /// <param name="Buffer">A pointer to read the data to CRC.</param>
        /// <param name="Length">How many bytes should be read from pointer.</param>
        /// <returns>The created CRC32</returns>
        [DllImport(DLLName, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint cxBufCRC32(IntPtr Buffer, int Length);

        #endregion

        #region Public Wrappers

        /// <summary>
        /// Fast decompresses data from managed sourcebuffer to managed targetbuffer using pointers
        /// </summary>
        /// <param name="SourceBuffer">SourceBuffer containing compressed data</param>
        /// <param name="SourceIndex">Cursor in SourceBuffer to start reading</param>
        /// <param name="TargetBuffer">TargetBuffer for decompressed data</param>
        /// <param name="TargetIndex">Cursor in TargetBuffer to start writing</param>
        /// <param name="UncompressedLength">Uncompressed data length</param>
        /// <param name="CompressedLength">Compressed data length</param>
        /// <returns>True if the operation was successful.</returns>
        public static bool Decompress(byte[] SourceBuffer, int SourceIndex, byte[] TargetBuffer, int TargetIndex, int UncompressedLength, int CompressedLength)
        {
            bool isSuccessful = false;

            // init crush32
            if (Crush32.CX_SUCCESS == Crush32.cxInit())
            {
                if (Crush32.CX_SUCCESS == Crush32.cxBuf2BufInit())
                {
                    // pin the managed source & targetbuffers in memory so crush32 can access them
                    // and we can directly use them without marshaling.
                    fixed (byte* ptrSourceBuffer = SourceBuffer, ptrTargetBuffer = TargetBuffer)
                    {
                        // add the offsets to the pointers (they still point to the beginning of the buffer)
                        byte* ptrSourceIndex = ptrSourceBuffer + SourceIndex;
                        byte* ptrTargetIndex = ptrTargetBuffer + TargetIndex;

                        if (Crush32.CX_SUCCESS == Crush32.cxBuf2BufExpand(ptrSourceIndex, ptrTargetIndex, UncompressedLength, CompressedLength))
                            isSuccessful = true;
                    }
                    
                    Crush32.cxBuf2BufClose();
                }

                Crush32.cxCleanup();
            }

            return isSuccessful;
        }

        /// <summary>
        /// Fast compresses data from managed sourcebuffer to managed targetbuffer using pointers
        /// </summary>
        /// <param name="SourceBuffer">SourceBuffer containing data to be compressed</param>
        /// <param name="SourceIndex">Cursor in SourceBuffer to start reading</param>
        /// <param name="TargetBuffer">TargetBuffer for compressed data</param>
        /// <param name="TargetIndex">Cursor in TargetBuffer to start writing</param>
        /// <param name="UncompressedLength">Uncompressed data length in SourceBuffer</param>
        /// <returns>Compressed length</returns>
        public static int Compress(byte[] SourceBuffer, int SourceIndex, byte[] TargetBuffer, int TargetIndex, int UncompressedLength)
        {
            int compressedLength = 0;

            // init crush32
            if (Crush32.CX_SUCCESS == Crush32.cxInit())
            {
                if (Crush32.CX_SUCCESS == Crush32.cxBuf2BufInit())
                {
                    // pin the managed source & targetbuffers in memory so crush32 can access them
                    // and we can directly use them without marshaling.
                    fixed (byte* ptrSourceBuffer = SourceBuffer, ptrTargetBuffer = TargetBuffer)
                    {
                        // add the offsets to the pointers (they still point to the beginning of the buffer)
                        byte* ptrSourceIndex = ptrSourceBuffer + SourceIndex;
                        byte* ptrTargetIndex = ptrTargetBuffer + TargetIndex;

                        // compress
                        compressedLength = Crush32.cxBuf2BufCompress(ptrSourceIndex, ptrTargetIndex, UncompressedLength);                       
                    }
                   
                    Crush32.cxBuf2BufClose();
                }

                Crush32.cxCleanup();
            }

            return compressedLength;
        }

        /// <summary>
        /// Fast decrypt data in managed buffer using pointer
        /// </summary>
        /// <param name="SourceBuffer">SourceBuffer containing encrypted data</param>
        /// <param name="SourceIndex">Cursor in SourceBuffer to start reading</param>
        /// <param name="Length">Encrypted data length</param>
        /// <param name="Challenge">Challenge to use</param>
        /// <param name="ExpectedResponse">ExpectedRespone to use</param>
        /// <param name="Password">Password to use</param>
        /// <returns>True if the operation was successful.</returns>
        public static bool Decrypt(byte[] SourceBuffer, int SourceIndex, int Length, uint Challenge, uint ExpectedResponse, byte[] Password)
        {
            bool isSuccessful = false;
           
            // pin the managed sourcebuffer in memory so crush32 can access
            // and we can directly use without marshaling.
            fixed (byte* ptrSourceBuffer = SourceBuffer)
            {
                // add the offsets to the pointers (they still point to the beginning of the buffer)
                byte* ptrSourceIndex = ptrSourceBuffer + SourceIndex;
                isSuccessful = Decrypt(ptrSourceIndex, Length, Challenge, ExpectedResponse, Password);                     
            }

            return isSuccessful;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceBuffer"></param>
        /// <param name="Length"></param>
        /// <param name="Challenge"></param>
        /// <param name="ExpectedResponse"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static unsafe bool Decrypt(byte* SourceBuffer, int Length, uint Challenge, uint ExpectedResponse, byte[] Password)
        {
            bool isSuccessful = false;

            // init crush32
            if (Crush32.CX_SUCCESS == Crush32.cxInit())
            {
                if (Crush32.CX_SUCCESS == Crush32.cxBuf2BufInit())
                {
                    // pin the managed sourcebuffer in memory so crush32 can access
                    // and we can directly use without marshaling.
                    fixed (byte* ptrPassword = Password)
                    {                       
                        // set password && decrypt
                        if (Crush32.CX_SUCCESS == Crush32.cxSetPassword(ptrPassword))
                            if (Crush32.CX_SUCCESS == Crush32.cxBufDecrypt(SourceBuffer, Length, Challenge, ExpectedResponse))
                                isSuccessful = true;
                    }

                    Crush32.cxBuf2BufClose();
                }

                Crush32.cxCleanup();
            }

            return isSuccessful;
        }

        /// <summary>
        /// Fast encrypt data in managed buffer using pointer
        /// </summary>
        /// <param name="SourceBuffer">SourceBuffer containing data to be encrypted</param>
        /// <param name="SourceIndex">Cursor in SourceBuffer to start reading</param>
        /// <param name="Length">Length of data to be encrypted</param>
        /// <param name="Challenge">Challenge to use</param>
        /// <param name="Password">Password to use</param>
        /// <returns>ExpectedResponse for Challenge/Password combination.</returns>
        public static uint Encrypt(byte[] SourceBuffer, int SourceIndex, int Length, uint Challenge, byte[] Password)
        {
            uint expectedResponse = 0;
           
            // pin the managed sourcebuffer in memory so crush32 can access
            // and we can directly use without marshaling.
            fixed (byte* ptrSourceBuffer = SourceBuffer, ptrPassword = Password)
            {
                // add the offsets to the pointers (they still point to the beginning of the buffer)
                byte* ptrSourceIndex = ptrSourceBuffer + SourceIndex;
                expectedResponse = Encrypt(ptrSourceIndex, Length, Challenge, Password);
            }

            return expectedResponse;
        }

        /// <summary>
        /// Fast encrypt data on unmanaged pointer
        /// </summary>
        /// <param name="SourceBuffer"></param>
        /// <param name="Length"></param>
        /// <param name="Challenge"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static unsafe uint Encrypt(byte* SourceBuffer, int Length, uint Challenge, byte[] Password)
        {
            uint expectedResponse = 0;

            // init crush32
            if (Crush32.CX_SUCCESS == Crush32.cxInit())
            {
                if (Crush32.CX_SUCCESS == Crush32.cxBuf2BufInit())
                {
                    // pin the managed sourcebuffer in memory so crush32 can access
                    // and we can directly use without marshaling.
                    fixed (byte* ptrPassword = Password)
                    {                       
                        // set password && encrypt
                        if (Crush32.CX_SUCCESS == Crush32.cxSetPassword(ptrPassword))
                            expectedResponse = Crush32.cxBufEncrypt(SourceBuffer, Length, Challenge);
                    }

                    Crush32.cxBuf2BufClose();
                }

                Crush32.cxCleanup();
            }

            return expectedResponse;
        }
        #endregion
    }
}
#endif