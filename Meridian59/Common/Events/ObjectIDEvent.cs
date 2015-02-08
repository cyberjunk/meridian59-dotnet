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
using Meridian59.Data.Models;

namespace Meridian59.Common.Events
{
    /// <summary>
    /// Delegate for ObjectIDEventArgs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ObjectIDEventHandler(object sender, ObjectIDEventArgs e);

    /// <summary>
    /// EventArgs carrying ObjectID model
    /// </summary>
    public class ObjectIDEventArgs : EventArgs
    {
        public ObjectID ID;

        public ObjectIDEventArgs(ObjectID ID)
        {
            this.ID = ID;
        }
    }
}
