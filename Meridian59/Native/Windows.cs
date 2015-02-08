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
using System.Runtime.InteropServices;

namespace Meridian59.Native
{
#if WINCLR
    using WORD      = System.UInt16;
    using DWORD     = System.UInt32;
    using INT16     = System.Int16;
    using INT32     = System.Int32;
    using INT64     = System.Int64;
    using UINT16    = System.UInt16;
    using UINT32    = System.UInt32;
    using UINT64    = System.UInt64;
    using ULONG     = System.UInt32;
    using ULONGLONG = System.UInt64;

    /// <summary>
    /// PINVOKE wrapping for Windows API
    /// </summary>
    public static class Windows
    {
        /// <summary>
        /// PINVOKE for kernel32.dll
        /// </summary>
        public static class Kernel32
        {
            public const string FILENAME = "kernel32.dll";

            [DllImport(FILENAME, EntryPoint = "RtlMoveMemory")]
            public static extern void RtlMoveMemory(IntPtr Destination, IntPtr Source, uint Length);
        }

        /// <summary>
        /// PINVOKE for user32.dll
        /// </summary>
        public static class User32
        {
            public const string FILENAME = "user32.dll";

            [StructLayout(LayoutKind.Sequential)]
            public struct DEVMODE
            {
                private const int CCHDEVICENAME = 0x20;
                private const int CCHFORMNAME = 0x20;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
                public string dmDeviceName;
                public short dmSpecVersion;
                public short dmDriverVersion;
                public short dmSize;
                public short dmDriverExtra;
                public int dmFields;
                public int dmPositionX;
                public int dmPositionY;
                public int dmDisplayOrientation;
                public int dmDisplayFixedOutput;
                public short dmColor;
                public short dmDuplex;
                public short dmYResolution;
                public short dmTTOption;
                public short dmCollate;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
                public string dmFormName;
                public short dmLogPixels;
                public int dmBitsPerPel;
                public int dmPelsWidth;
                public int dmPelsHeight;
                public int dmDisplayFlags;
                public int dmDisplayFrequency;
                public int dmICMMethod;
                public int dmICMIntent;
                public int dmMediaType;
                public int dmDitherType;
                public int dmReserved1;
                public int dmReserved2;
                public int dmPanningWidth;
                public int dmPanningHeight;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct DISPLAY_DEVICE
            {
                [MarshalAs(UnmanagedType.U4)]
                public int cb;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string DeviceName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string DeviceString;
                [MarshalAs(UnmanagedType.U4)]
                public DisplayDeviceStateFlags StateFlags;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string DeviceID;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string DeviceKey;
            }

            [Flags()]
            public enum DisplayDeviceStateFlags : int
            {
                AttachedToDesktop   = 0x00000001,
                MultiDriver         = 0x00000002,
                PrimaryDevice       = 0x00000004,
                MirroringDriver     = 0x00000008,
                VGACompatible       = 0x00000010,
                Removable           = 0x00000020,
                ModesPruned         = 0x08000000,
                Remote              = 0x04000000,
                Disconnect          = 0x02000000
            }

            [DllImport(FILENAME)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport(FILENAME, SetLastError = true)]
            public static extern bool SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            [DllImport(FILENAME)]
            public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

            [DllImport(FILENAME)]
            public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

            #region Helpers
            public static List<DISPLAY_DEVICE> GetScreens()
            {
                DISPLAY_DEVICE vDevice = new DISPLAY_DEVICE();
                vDevice.cb = Marshal.SizeOf(vDevice);

                List<DISPLAY_DEVICE> list = new List<DISPLAY_DEVICE>();

                uint i = 0;
                while (EnumDisplayDevices(null, i, ref vDevice, 0))
                {
                    if (vDevice.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                        list.Add(vDevice);

                    i++;
                    vDevice.cb = Marshal.SizeOf(vDevice);
                }

                return list;
            }

            public static List<DEVMODE> GetScreenInfo(string devicename = null)
            {
                DEVMODE vDevMode = new DEVMODE();
                List<DEVMODE> info = new List<DEVMODE>();

                int i = 0;
                while (EnumDisplaySettings(devicename, i, ref vDevMode))
                {
                    if (!Contains(info, vDevMode.dmPelsWidth, vDevMode.dmPelsHeight))
                        AddSorted(info, vDevMode);

                    i++;
                }

                return info;
            }

            private static bool Contains(IEnumerable<DEVMODE> modes, int width, int height)
            {
                foreach (DEVMODE mode in modes)
                    if (mode.dmPelsWidth == width && mode.dmPelsHeight == height)
                        return true;

                return false;
            }

            private static void AddSorted(List<DEVMODE> list, DEVMODE item)
            {
                int i = 0;

                while (i < list.Count && list[i].dmPelsWidth <= item.dmPelsWidth)
                    i++;

                list.Insert(i, item);
            }
            #endregion
        }

        /// <summary>
        /// PINVOKE for WS2_32.dll
        /// </summary>
        public static class WS2_32
        {
            public const string FILENAME = "WS2_32.dll";


            [DllImport(FILENAME, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
            public static extern int connect(IntPtr socket, IntPtr addr, int addrlen);

            [DllImport(FILENAME, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
            public static extern int recv(IntPtr socket, IntPtr buffer, int length, int flags);

            [DllImport(FILENAME, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
            public static extern int send(IntPtr socket, IntPtr buffer, int length, int flags);
            

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
            public delegate int DConnect(IntPtr socket, IntPtr addr, int addrlen);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
            public delegate int DRecv(IntPtr socket, IntPtr buffer, int length, int flags);

            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
            public delegate int DSend(IntPtr socket, IntPtr buffer, int length, int flags);
        }

        /// <summary>
        /// PINVOKE for mscoree.dll
        /// </summary>
        public static class MSCOREE
        {
            public const string FILENAME = "mscoree.dll";

            [DllImport(FILENAME, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
            public static extern void CorExitProcess(int uExitCode);


            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
            public delegate void DCorExitProcess(int uExitCode);
        }
    }
#endif
}
