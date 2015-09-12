/*
 Copyright (c) 2012 Clint Banzhaf
 This file is part of "Meridian59.AdminUI".

 "Meridian59.AdminUI" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59.AdminUI" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59.AdminUI".
 If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Windows.Forms;

namespace Meridian59.AdminUI.DataGridColumns
{ 
    /// <summary>
    /// HexColumn uses HexCells for hexidecimal display.
    /// </summary>
    public class HexColumn : DataGridViewColumn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HexColumn() : base(new HexCell())
        {           
        }
        
        public override object Clone()
        {
            return (HexColumn)base.Clone();
        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if ((value == null) || !(value is HexCell))
                {
                    throw new ArgumentException("Invalid cell type, HexColumns can only contain HexCells");
                }
            }
        }
    }
}
