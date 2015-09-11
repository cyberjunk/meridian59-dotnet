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

namespace Meridian59.AdminUI.Generic
{
    public enum Endian { Big, Little }

    public class HexTextBox : TextBox
    {
        private bool suppressNextKey = false;
        private static Keys[] ValidCharKeys = new Keys[] { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F };
        private static Keys[] ValidNumericKeys = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };

        private Endian _endianType = Endian.Little;
        public Endian EndianType { get { return _endianType; } set { _endianType = value; } }

        private uint _maxBytesLength = 20000;
        public uint MaxBytesLength { get { return _maxBytesLength; } set { _maxBytesLength = value; } }

        private byte[] _value;
        public byte[] Value
        {
            get { return _value; }
            set { 
                _value = value;
                if (value != null)
                    this.Text = BitConverter.ToString(value).Replace("-",String.Empty);
            }
        }

        public HexTextBox()
        {
            this.Multiline = false;
            this.KeyDown += new KeyEventHandler(HexTextBox_KeyDown);
            this.KeyPress += new KeyPressEventHandler(HexTextBox_KeyPress);
            this.TextChanged += new EventHandler(HexTextBox_TextChanged);
        }
      
        protected override void WndProc(ref Message m)
        {           
            switch(m.Msg)
            {
                // intercept CTRL+V (WM_PASTE)
                case 0x302:
                    string pasteString = Clipboard.GetText();
                    pasteString = pasteString.Replace("-", String.Empty);
                    pasteString = pasteString.Replace(" ", String.Empty);
                    pasteString = pasteString.Replace("\t", String.Empty);
                    
                    bool isStringValid = true;
                    for (int j = 0; j < pasteString.Length; j++)
                    {
                        bool isCharValid = false;
                        for (int i = 0; i < ValidCharKeys.Length; i++)
                            if (ValidCharKeys[i] == (Keys)(byte)char.ToUpper(pasteString[j]))
                            {
                                isCharValid = true;
                                break;
                            }

                        if(!isCharValid)
                            for (int i = 0; i < ValidNumericKeys.Length; i++)
                                if (ValidNumericKeys[i] == (Keys)(byte)char.ToUpper(pasteString[j]))
                                {
                                    isCharValid = true;
                                    break;
                                }

                        if (!isCharValid)
                        {
                            isStringValid = false;
                            break;
                        }
                    }

                    if (isStringValid)
                    {
                        this.Text = this.Text.Substring(0, this.SelectionStart) + pasteString + this.Text.Substring(this.SelectionStart);
                        _value = StringToByteArray(this.Text);
                    }
                    break;   
    
                default:
                    base.WndProc(ref m);
                    break;
            }          
        }
        
        private void HexTextBox_TextChanged(object sender, EventArgs e)
        {
            _value = StringToByteArray(this.Text);
        }

        private void HexTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // suppress keys by default
            suppressNextKey = true;

            // allow only if defined max not reached
            if (this.Text.Length < _maxBytesLength * 2)
            {
                // allow charskeys (also with shift pressed)
                for (int i = 0; i < ValidCharKeys.Length; i++)
                    if (e.KeyCode == ValidCharKeys[i])
                    {
                        suppressNextKey = false;
                        break;
                    }

                // if key not yet allowed, allow numerickeys (without shift pressed)
                if ((suppressNextKey) && (!e.Shift))
                    for (int i = 0; i < ValidNumericKeys.Length; i++)
                        if (e.KeyCode == ValidNumericKeys[i])
                        {
                            suppressNextKey = false;
                            break;
                        }
            }

            // if it wasn't a hexkey but a ctrl+c, ctrl+v, backspace allow also
            if ((suppressNextKey) && (e.Control && e.KeyCode == Keys.C) || (e.Control && e.KeyCode == Keys.V) || (e.KeyCode == Keys.Back))
                suppressNextKey = false;
        }

        private void HexTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // suppress key depending on flag saved by keydown
            e.Handled = suppressNextKey;
        }
        
        private byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                switch (EndianType)
                {
                    case Endian.Little:
                        hex = hex.Insert(hex.Length - 1, "0");
                        break;
                    case Endian.Big:
                        hex = hex.Insert(0, "0");
                        break;
                }                
            }
                
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
