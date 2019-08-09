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

namespace Meridian59.Common.Enums
{
    /// <summary>
    /// Error codes used to differentiate errors in skill/spell selection during char creation.
    /// </summary>
    public enum CharSelectAbilityError : byte
    {
        NoError = 0x00,                  // Everything went OK.
        NoPointsLeftError = 0x01,        // Ran out of ability points.
        AlreadyHaveShalilleError = 0x02, // User selected Qor spell but already has Shal'ille.
        AlreadyHaveQorError = 0x03,      // User selected Shalille spell but already has Qor.
        NotEnoughLevelOneError = 0x04    // User selected level 2 but needs more level 1 selected.
    }
}
