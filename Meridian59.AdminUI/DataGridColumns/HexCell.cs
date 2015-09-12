/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.AdminUI".

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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Meridian59.AdminUI.DataGridColumns
{
    /// <summary>
    /// This cell will show a hexadecimal representation of
    /// the provided value.
    /// </summary>
    public class HexCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HexCell()
        {
        }

        /// <summary>
        /// Converts hex-string to byte[]
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("-", "");
            hex = hex.Replace("\t", "");
            hex = hex.Replace(" ", "");

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// This function must return a string representation
        /// of the value. In this case a hexidecimal string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellStyle"></param>
        /// <param name="valueTypeConverter"></param>
        /// <param name="formattedValueTypeConverter"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override object GetFormattedValue(object value,
           int rowIndex, ref DataGridViewCellStyle cellStyle,
           TypeConverter valueTypeConverter,
           TypeConverter formattedValueTypeConverter,
           DataGridViewDataErrorContexts context)
        {
            object returnVal = String.Empty;

            if (value != null)
            {
                if (value is byte[])
                    returnVal = BitConverter.ToString((byte[])value);

                else if (value is byte)
                    returnVal = BitConverter.ToString(new byte[] { (byte)value });

                else if (value is int)
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((int)value)), 0);

                else if (value is uint)
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((uint)value)), 0);

                else if (value is short)
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((short)value)), 0);

                else if (value is ushort)
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((ushort)value)), 0);

                else if (value is long)
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((long)value)), 0);

                else if (value is ulong)
                    returnVal = BitConverter.ToString(BitConverter.GetBytes(((ulong)value)), 0);
            }

            return returnVal;
        }

        /// <summary>
        /// This function must provide a value from hexidecimal string in cell.
        /// </summary>
        /// <param name="formattedValue"></param>
        /// <param name="cellStyle"></param>
        /// <param name="formattedValueTypeConverter"></param>
        /// <param name="valueTypeConverter"></param>
        /// <returns></returns>
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
}
