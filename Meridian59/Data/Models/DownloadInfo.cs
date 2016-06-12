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
using Meridian59.Common;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// DownloadInfo for updates
    /// </summary>
    [Serializable]
    public class DownloadInfo : IByteSerializableFast, INotifyPropertyChanged, IClearable
    {
        #region Constants
        public const string PROPNAME_MACHINE    = "Machine";
        public const string PROPNAME_PATH       = "Path";
        public const string PROPNAME_REASON     = "Reason";
        public const string PROPNAME_DEMOPATH   = "DemoPath";
        public const string PROPNAME_FILES      = "Files";
        public const string PROPNAME_CURRENTINDEX = "CurrentIndex";
        public const string PROPNAME_PROGRESS = "Progress";
        public const string PROPNAME_ISFINISHED = "IsFinished";
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
                    TypeSizes.SHORT + path.Length +
                    TypeSizes.SHORT + reason.Length +
                    TypeSizes.SHORT + demoPath.Length;

                foreach (DownloadFileInfo obj in files)
                    len += obj.ByteLength;

                return len;
            } 
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            ushort len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            files = new DownloadFileInfo[len];

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            machine = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            path = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            reason = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            len = BitConverter.ToUInt16(Buffer, cursor);
            cursor += TypeSizes.SHORT;

            demoPath = Util.Encoding.GetString(Buffer, cursor, len);
            cursor += len;

            for (int i = 0; i < files.Length; i++)
            {
                DownloadFileInfo obj = new DownloadFileInfo(Buffer, cursor);
                cursor += obj.ByteLength;

                files[i] = obj;               
            }

            return cursor - StartIndex; 
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;
            
            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(files.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(machine.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(machine), 0, Buffer, cursor, machine.Length);
            cursor += machine.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(path.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(path), 0, Buffer, cursor, path.Length);
            cursor += path.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(reason.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(reason), 0, Buffer, cursor, reason.Length);
            cursor += reason.Length;

            Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(demoPath.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
            cursor += TypeSizes.SHORT;

            Array.Copy(Util.Encoding.GetBytes(demoPath), 0, Buffer, cursor, demoPath.Length);
            cursor += demoPath.Length;

            foreach (DownloadFileInfo obj in files)
                cursor += obj.WriteTo(Buffer, cursor);

            return cursor - StartIndex;
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            ushort len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            files = new DownloadFileInfo[len];

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            machine = new string((sbyte*)Buffer, 0, len, Util.Encoding);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            path = new string((sbyte*)Buffer, 0, len, Util.Encoding);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            reason = new string((sbyte*)Buffer, 0, len, Util.Encoding);
            Buffer += len;

            len = *((ushort*)Buffer);
            Buffer += TypeSizes.SHORT;

            demoPath = new string((sbyte*)Buffer, 0, len, Util.Encoding);
            Buffer += len;

            for (int i = 0; i < files.Length; i++)          
                files[i] = new DownloadFileInfo(ref Buffer);                        
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            int a, b; bool c;

            ushort len = (ushort)files.Length;

            *((ushort*)Buffer) = len;
            Buffer += TypeSizes.SHORT;

            fixed (char* pString = machine)
            {
                len = (ushort)machine.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;
               
                Util.Encoding.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = path)
            {
                len = (ushort)path.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Util.Encoding.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = reason)
            {
                len = (ushort)reason.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Util.Encoding.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            fixed (char* pString = demoPath)
            {
                len = (ushort)demoPath.Length;

                *((ushort*)Buffer) = len;
                Buffer += TypeSizes.SHORT;

                Util.Encoding.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                Buffer += len;
            }

            foreach (DownloadFileInfo obj in files)
                obj.WriteTo(ref Buffer);
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
        protected string path;
        protected string reason;
        protected string demoPath;
        protected DownloadFileInfo[] files;
        protected int currentIndex;
        protected int progress;
        protected bool isFinished;
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

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                if (path != value)
                {
                    path = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PATH));
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

        public string DemoPath
        {
            get
            {
                return demoPath;
            }
            set
            {
                if (demoPath != value)
                {
                    demoPath = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_DEMOPATH));
                }
            }
        }

        public DownloadFileInfo[] Files
        {
            get
            {
                return files;
            }
            set
            {
                if (files != value)
                {
                    files = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_FILES));
                }
            }
        }

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
            set
            {
                if (currentIndex != value)
                {
                    currentIndex = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_CURRENTINDEX));
                }
            }
        }

        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_PROGRESS));
                }
            }
        }

        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
            set
            {
                if (isFinished != value)
                {
                    isFinished = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs(PROPNAME_ISFINISHED));
                }
            }
        }
        #endregion

        #region Constructors
        public DownloadInfo()
        {
            Clear(false);
        }

        public DownloadInfo(string Machine, string Path, string Reason, string DemoPath)
        {
            this.machine = Machine;
            this.path = Path;
            this.reason = Reason;
            this.demoPath = DemoPath;
            this.currentIndex = 0;
            this.progress = 0;
            this.isFinished = false;
        }

        public DownloadInfo(byte[] Buffer, int StartIndex = 0) 
        {
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe DownloadInfo(ref byte* Buffer)
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
                Path = String.Empty;
                Reason = String.Empty;
                DemoPath = String.Empty;
                Files = new DownloadFileInfo[0];
                CurrentIndex = 0;
                Progress = 0;
                IsFinished = false;
            }
            else
            {
                machine = String.Empty;
                path = String.Empty;
                reason = String.Empty;
                demoPath = String.Empty;
                files = new DownloadFileInfo[0];
                currentIndex = 0;
                progress = 0;
                isFinished = false;
            }
        }
        #endregion

        public string GetURL(int Index)
        {
            return (Index < files.Length) ?
                "http://" + machine + path + files[Index].FileName :
                String.Empty;
        }
    }
}
