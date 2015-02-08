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
using System.ComponentModel;
using Meridian59.Data.Models;

namespace Meridian59.Data.Lists
{
    /// <summary>
    /// List of Projectile
    /// </summary>
    [Serializable]
    public class ProjectileList : BaseList<Projectile>
    {
        public ProjectileList(int Capacity = 5)
            : base(Capacity)
        {
            AllowNew = true;
            AllowEdit = true;
            AllowRemove = true;              
        }

        protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnItemPropertyChanged(sender, e);
            
            Projectile projectile = (Projectile)sender;
            switch (e.PropertyName)
            {
                // remove projectiles which are not moving anymore (finised)                   
                case Projectile.PROPNAME_ISMOVING:
                    if (!projectile.IsMoving)
                        Remove(projectile);
                    break;
            }
        }
    }
}
