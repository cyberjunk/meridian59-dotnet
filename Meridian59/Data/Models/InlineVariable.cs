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
using Meridian59.Common.Interfaces;
using Meridian59.Common.Enums;
using Meridian59.Common.Constants;

namespace Meridian59.Data.Models
{
    /// <summary>
    /// Inline variables like %s, %i. Used in chat, stringresources, ...
    /// </summary>
    [Serializable]
    public class InlineVariable : IByteSerializableFast
    {
        #region IByteSerializable
        public int ByteLength
        {
            get
            {
                int len = 0;
                switch (Type)
                {
                    case InlineVariableType.Integer:
                    case InlineVariableType.Resource:
                        len = TypeSizes.INT;
                        break;

                    case InlineVariableType.String:
                        len = TypeSizes.SHORT + ((string)Data).Length;
                        break;
                }
                               
                return len;
            }
        }

        public int WriteTo(byte[] Buffer, int StartIndex = 0)
        {
            int cursor = StartIndex;

            switch(Type)
            {
                case InlineVariableType.Integer:
                    int i = (int)Data;
                    Array.Copy(BitConverter.GetBytes(i), 0, Buffer, cursor, TypeSizes.INT);
                    cursor += TypeSizes.INT;
                    break;

                case InlineVariableType.Resource:
                    uint ui = (uint)Data;
                    Array.Copy(BitConverter.GetBytes(ui), 0, Buffer, cursor, TypeSizes.INT);
                    cursor += TypeSizes.INT;
                    break;

                case InlineVariableType.String:
                    string s = (string)Data;

                    Array.Copy(BitConverter.GetBytes(Convert.ToUInt16(s.Length)), 0, Buffer, cursor, TypeSizes.SHORT);
                    cursor += TypeSizes.SHORT;

                    Array.Copy(Encoding.Default.GetBytes(s), 0, Buffer, cursor, s.Length);
                    cursor += s.Length;
                    break;
            }

            return cursor - StartIndex;
        }

        public int ReadFrom(byte[] Buffer, int StartIndex = 0)
        {          
            int cursor = StartIndex;

            switch (Type)
            {
                case InlineVariableType.Integer:
                    Data = BitConverter.ToInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                    break;

                case InlineVariableType.Resource:
                    Data = BitConverter.ToUInt32(Buffer, cursor);
                    cursor += TypeSizes.INT;
                    break;

                case InlineVariableType.String:
                    ushort len = BitConverter.ToUInt16(Buffer, cursor);
                    cursor += TypeSizes.SHORT;

                    Data = Encoding.Default.GetString(Buffer, cursor, len);
                    cursor += len;
                    break;
            }

            return cursor - StartIndex;
        }

        public unsafe void WriteTo(ref byte* Buffer)
        {
            switch (Type)
            {
                case InlineVariableType.Integer:
                    int i = (int)Data;
                    *((int*)Buffer) = i;
                    Buffer += TypeSizes.INT;
                    break;

                case InlineVariableType.Resource:
                    uint ui = (uint)Data;
                    *((uint*)Buffer) = ui;
                    Buffer += TypeSizes.INT;
                    break;

                case InlineVariableType.String:
                    string s = (string)Data;

                    fixed (char* pString = s)
                    {
                        ushort len = (ushort)s.Length;

                        *((ushort*)Buffer) = len;
                        Buffer += TypeSizes.SHORT;

                        int a, b; bool c;
                        Encoding.Default.GetEncoder().Convert(pString, len, Buffer, len, true, out a, out b, out c);
                        Buffer += len;
                    }                   
                    break;
            }
        }

        public unsafe void ReadFrom(ref byte* Buffer)
        {
            switch (Type)
            {
                case InlineVariableType.Integer:
                    Data = *((int*)Buffer);
                    Buffer += TypeSizes.INT;
                    break;

                case InlineVariableType.Resource:
                    Data = *((uint*)Buffer);
                    Buffer += TypeSizes.INT;
                    break;

                case InlineVariableType.String:
                    ushort len = *((ushort*)Buffer);
                    Buffer += TypeSizes.SHORT;

                    Data = new string((sbyte*)Buffer, 0, len);
                    Buffer += len;
                    break;
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

        #region Properties
        public InlineVariableType Type { get; set; }
        public object Data { get; set; }
        #endregion

        #region Constructors
        public InlineVariable(InlineVariableType Type, object Data)
        {
            this.Type = Type;
            this.Data = Data;
        }

        public InlineVariable(InlineVariableType Type, byte[] Buffer, int StartIndex = 0)
        {
            this.Type = Type;
            ReadFrom(Buffer, StartIndex);
        }

        public unsafe InlineVariable(InlineVariableType Type, ref byte* Buffer)
        {
            this.Type = Type;
            ReadFrom(ref Buffer);
        }
        #endregion
    }
}
