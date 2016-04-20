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

namespace Meridian59.Data.Models
{
    /// <summary>
    /// ClientPatchInfo for updates
    /// </summary>
    [Serializable]
    public class ClientPatchInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_MACHINE     = "Machine";            // ww1.meridiannext.com
        public const string PROPNAME_PATCHPATH   = "PatchPath";          // /105/clientpatch
        public const string PROPNAME_PATCHCACHEPATH  = "PatchCachePath"; // /105/
        public const string PROPNAME_UPDATERFILE = "UpdaterFile";        // Meridian59.Ogre.Patcher.exe
        public const string PROPNAME_PATCHFILE   = "PatchFile";          // patchinfo.txt
        public const string PROPNAME_REASON      = "Reason";             // "Update is required"
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
                int len = TypeSizes.SHORT + 
                    TypeSizes.SHORT + machine.Length +
                    TypeSizes.SHORT + patchPath.Length +
                    TypeSizes.SHORT + patchCachePath.Length +
                    TypeSizes.SHORT + patchFile.Length +
                    TypeSizes.SHORT + updaterFile.Length + 
                    TypeSizes.SHORT + reason.Length;

                return len;
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            machine = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            patchPath = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            patchCachePath = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            patchFile = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            updaterFile = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            reason = Encoding.Default.GetString(Buffer, cursor, len);
            cursor += len;

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(machine.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(machine), 0, Buffer, cursor, machine.Length);
            cursor += machine.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(patchPath.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(patchPath), 0, Buffer, cursor, patchPath.Length);
            cursor += patchPath.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(patchCachePath.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(patchCachePath), 0, Buffer, cursor, patchCachePath.Length);
            cursor += patchCachePath.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(patchFile.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(patchFile), 0, Buffer, cursor, patchFile.Length);
            cursor += patchFile.Length;
            
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(updaterFile.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(updaterFile), 0, Buffer, cursor, updaterFile.Length);
            cursor += updaterFile.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(reason.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Encoding.Default.GetBytes(reason), 0, Buffer, cursor, reason.Length);
            cursor += reason.Length;

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            machine = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            patchPath = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            patchCachePath = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            patchFile = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            updaterFile = new string((sbyte*)Buffer, 0, len);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            reason = new string((sbyte*)Buffer, 0, len);
            Buffer += len;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            int a, b;
            bool c;
            ushort len;

            fixed (char* pString = machine)
            {
                len = (ushort)machine.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;
               
                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = patchPath)
            {
                len = (ushort)patchPath.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = patchCachePath)
            {
                len = (ushort)patchCachePath.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = patchFile)
            {
                len = (ushort)patchFile.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = updaterFile)
            {
                len = (ushort)updaterFile.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = reason)
            {
                len = (ushort)reason.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }
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
        protected string machine;
        protected string patchPath;
        protected string patchCachePath;
        protected string patchFile;
        protected string updaterFile;
        protected string reason;
        #endregion

        #region Properties
        public string Machine
        {
            get
            {
                return machine;
            }
            set
            {
                if (machine != value)
                {
                    machine = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_MACHINE));
                }
            }
        }

        public string PatchPath
        {
            get
            {
                return patchPath;
            }
            set
            {
                if (patchPath != value)
                {
                    patchPath = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PATCHPATH));
                }
            }
        }

        public string PatchCachePath
        {
            get
            {
                return patchCachePath;
            }
            set
            {
                if (patchCachePath != value)
                {
                    patchCachePath = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PATCHCACHEPATH));
                }
            }
        }

        public string PatchFile
        {
            get
            {
                return patchFile;
            }
            set
            {
                if (patchFile != value)
                {
                    patchFile = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PATCHFILE));
                }
            }
        }

        public string UpdaterFile
        {
            get
            {
                return updaterFile;
            }
            set
            {
                if (updaterFile != value)
                {
                    updaterFile = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_UPDATERFILE));
                }
            }
        }

        public string Reason
        {
            get
            {
                return reason;
            }
            set
            {
                if (reason != value)
                {
                    reason = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_REASON));
                }
            }
        }
        #endregion

        #region Constructors
        public ClientPatchInfo()
        {
            Clear(false);
        }

        public ClientPatchInfo(string Machine, string PatchPath, string PatchCachePath,
                               string PatchFile, string UpdaterFile, string Reason)
        {
            this.machine = Machine;
            this.patchPath = PatchPath;
            this.patchCachePath = PatchCachePath;
            this.patchFile = PatchFile;
            this.updaterFile = UpdaterFile;
            this.reason = Reason;
        }

        public ClientPatchInfo(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe ClientPatchInfo(ref byte* Buffer)
        {
            ReadFrom(ref Buffer);
        }
        #endregion

        #region IClearable
        public void Clear(bool RaiseChangedEvent)
        {
            if (RaiseChangedEvent)
            {
                Machine = String.Empty;
                PatchPath = String.Empty;
                PatchCachePath = String.Empty;
                PatchFile = String.Empty;
                UpdaterFile = String.Empty;
                Reason = String.Empty;
            }
            else
            {
                machine = String.Empty;
                patchPath = String.Empty;
                patchCachePath = String.Empty;
                patchFile = String.Empty;
                updaterFile = String.Empty;
                reason = String.Empty;
            }
        }
        #endregion

        public Uri GetUpdaterURL()
        {
            return new Uri("http://" + machine + patchPath + "/" + updaterFile);
        }
    }
}
