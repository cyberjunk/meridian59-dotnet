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
using System.ComponentModel;
using Meridian59.Protocol.Enums;

namespace Meridian59.AdminUI.DataGridColumns
{
    /// <summary>
    /// InOutColumn uses InOutCells to display a value
    /// for 'RECEIVE' and 'SEND'.
    /// </summary>
    public class InOutColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InOutColumn() : base(new InOutCell())
        {
        }

        public override object Clone()
        {
            return (InOutColumn)base.Clone();
        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if ((value == null) || !(value is InOutCell))
                {
                    throw new ArgumentException("Invalid cell type, InOutColumns can only contain InOutCells");
                }
            }
        }
    }
}
