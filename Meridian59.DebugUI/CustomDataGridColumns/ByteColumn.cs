/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.DebugUI".

 "Meridian59.DebugUI" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59.DebugUI" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59.DebugUI".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace Meridian59.DebugUI.CustomDataGridColumns
{
    
    public class ByteCell : DataGridViewTextBoxCell
    {
        public ByteCell()
        {
            
        }
        private byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("-", "");
            hex = hex.Replace("\t", "");
            hex = hex.Replace(" ", "");

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        protected override object GetFormattedValue(object value,
           int rowIndex, ref DataGridViewCellStyle cellStyle,
           TypeConverter valueTypeConverter,
           TypeConverter formattedValueTypeConverter,
           DataGridViewDataErrorContexts context)
        {
            object returnVal = String.Empty;

            if (value != null)
            {
                if (value.GetType() == typeof(byte[]))
                    returnVal = BitConverter.ToString((byte[])value);
                else if (value.GetType() == typeof(byte))
                    returnVal = BitConverter.ToString(new byte[] { (byte)value });
                else if (value.GetType() == typeof(int))
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((int)value)),0);
                else if (value.GetType() == typeof(uint))
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((uint)value)), 0);
                else if (value.GetType() == typeof(short))
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((short)value)), 0);
                else if (value.GetType() == typeof(ushort))
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((ushort)value)), 0);
            }
            return returnVal;
        }

        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            byte[] val = new byte[0];
            try
            {
                val = StringToByteArray((string)formattedValue);
            }
            finally { }

            return val;
        }
    }

    public class ByteColumn : DataGridViewColumn
    {
        public ByteColumn() : base(new ByteCell())
        {
            
        }
        

        public override object Clone()
        {
            ByteColumn col = base.Clone() as ByteColumn;
            return col;
        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if ((value == null) || !(value is ByteCell))
                {
                    throw new ArgumentException("Invalid cell type, ByteColumns can only contain ByteCells");
                }
            }
        }
    }
}
