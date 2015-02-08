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
using System.Runtime.InteropServices;

namespace Meridian59.Native
{
    /// <summary>
    /// Access functions based on hosting CLR.
    /// Might directly call Windows API on MS .NET
    /// </summary>
    public static class Wrapper
    {
        public static void CopyMem(byte[] Source, int SrcIndex, byte[] Destination, int DestIndex, uint Length)
        {
            Array.Copy(Source, SrcIndex, Destination, DestIndex, Length);
        }

        public static void CopyMem(byte[] Source, int Index, IntPtr Destination, uint Length)
        {
            Marshal.Copy(Source, Index, Destination, (int)Length);
        }

        public static void CopyMem(IntPtr Source, byte[] Destination, int Length)
        {
            Marshal.Copy(Source, Destination, 0, Length);
        }
// optimized for windows
#if WINCLR
        public static void CopyMem(IntPtr Source, IntPtr Destination, uint Length)
        {
            Windows.Kernel32.RtlMoveMemory(Destination, Source, Length); 
        }
// optimized for linux
#if MONO
        public static void CopyMem(IntPtr Source, IntPtr Destination, uint Length)
        {
            Linux.Libc.memcpy(Destination, Source, Length); 
        }
#endif
// managed implementation / any CLR
#else
        

        // ptr to ptr copy missing here, no fast managed variant
#endif
    }
}
