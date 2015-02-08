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
using System.Windows.Forms;

namespace Meridian59.DebugUI.Generic
{
    public class ThreadSafeLog : GroupBox
    {
        private TextBox txtLog = new TextBox();
        private delegate void LogCallBack(string Message);
        
        public ThreadSafeLog()
        {
            txtLog.Dock = DockStyle.Fill;
            txtLog.Multiline = true;
            this.Controls.Add(txtLog);
        }

        public void Log(string Message)
        {
            if (this.InvokeRequired)
                this.Invoke(new LogCallBack(Log), new object[] { Message });         
            else
                txtLog.AppendText(Message + Environment.NewLine);
        }
    }
}
