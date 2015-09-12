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
    /// <summary>
    /// Extends base TextBox. Allows only hex values.
    /// </summary>
    public class HexTextBox : TextBox
    {
        protected bool isCtrl;

        protected static readonly char[] allowedChars = new char[] {
            'A', 'B', 'C', 'D', 'E', 'F',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '\b'
        };

        protected static readonly Keys[] allowedKeys = new Keys[] {
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F,
            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, 
            Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, 
            Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
            Keys.Left, Keys.Right, Keys.Back, Keys.Delete, Keys.End };

        protected static bool IsValidKey(Keys Key)
        {
            foreach (Keys k in allowedKeys)
                if (Key == k)
                    return true;

            return false;
        }

        protected static bool IsValidChar(char Key)
        {
            foreach (char k in allowedChars)
                if (Key == k)
                    return true;

            return false;
        }

        public HexTextBox()
            : base()
        {
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            isCtrl = e.Control;

            if (!IsValidKey(e.KeyCode) && !e.Control)
            {
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Control)
                isCtrl = false;

            if (!IsValidKey(e.KeyCode) && !e.Control)
            {
                e.Handled = true;
                return;
            }

            base.OnKeyUp(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // turn hex lowercase to uppercase
            switch (e.KeyChar)
            {
                case 'a': e.KeyChar = 'A'; break;
                case 'b': e.KeyChar = 'B'; break;
                case 'c': e.KeyChar = 'C'; break;
                case 'd': e.KeyChar = 'D'; break;
                case 'e': e.KeyChar = 'E'; break;
                case 'f': e.KeyChar = 'F'; break;
            }

            if (!IsValidChar(e.KeyChar) && !isCtrl)
            {
                e.Handled = true;
                return;
            }

            base.OnKeyPress(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                // intercept CTRL+V (WM_PASTE)
                case 0x302:
                    string pasteString = Clipboard.GetText();
                    pasteString = pasteString.Replace("-", String.Empty);
                    pasteString = pasteString.Replace(" ", String.Empty);
                    pasteString = pasteString.Replace("\t", String.Empty);
                    pasteString = pasteString.Replace("\n", String.Empty);
                    pasteString = pasteString.Replace("a", "A");
                    pasteString = pasteString.Replace("b", "B");
                    pasteString = pasteString.Replace("c", "C");
                    pasteString = pasteString.Replace("d", "D");
                    pasteString = pasteString.Replace("e", "E");
                    pasteString = pasteString.Replace("f", "F");

                    for (int j = 0; j < pasteString.Length; j++)
                        if (!IsValidChar(pasteString[j]))
                            return;

                    int cursor = SelectionStart;
                    this.Text = this.Text.Remove(SelectionStart, SelectionLength).Insert(cursor, pasteString);
                    SelectionStart = cursor + pasteString.Length;

                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public byte[] GetBinaryValue()
        {
            string hex = String.Copy(Text);

            if (hex.Length % 2 != 0)
                hex = hex.Insert(hex.Length - 1, "0");

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
