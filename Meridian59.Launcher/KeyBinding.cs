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
using System.Windows.Forms;

namespace Meridian59.Launcher.Models
{
    [Serializable]
    public class KeyBinding
    {
        public int RightClickAction { get; set; }

        // Movement
        public Keys MoveForward { get; set; }
        public Keys MoveBackward { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        
        // Rotation
        public Keys RotateLeft { get; set; }
        public Keys RotateRight { get; set; }

        // Modifiers
        public Keys Walk { get; set; }
        public Keys AutoMove { get; set; }

        // Targetting
        public Keys NextTarget { get; set; }
        public Keys SelfTarget { get; set; }

        // Actions
        public Keys ReqGo { get; set; }
        public Keys Close { get; set; }

        public Keys ActionButton01 { get; set; }
        public Keys ActionButton02 { get; set; }
        public Keys ActionButton03 { get; set; }
        public Keys ActionButton04 { get; set; }
        public Keys ActionButton05 { get; set; }
        public Keys ActionButton06 { get; set; }
        public Keys ActionButton07 { get; set; }
        public Keys ActionButton08 { get; set; }
        public Keys ActionButton09 { get; set; }
        public Keys ActionButton10 { get; set; }
        public Keys ActionButton11 { get; set; }
        public Keys ActionButton12 { get; set; }
        public Keys ActionButton13 { get; set; }
        public Keys ActionButton14 { get; set; }
        public Keys ActionButton15 { get; set; }
        public Keys ActionButton16 { get; set; }
        public Keys ActionButton17 { get; set; }
        public Keys ActionButton18 { get; set; }
        public Keys ActionButton19 { get; set; }
        public Keys ActionButton20 { get; set; }
        public Keys ActionButton21 { get; set; }
        public Keys ActionButton22 { get; set; }
        public Keys ActionButton23 { get; set; }
        public Keys ActionButton24 { get; set; }
        public Keys ActionButton25 { get; set; }
        public Keys ActionButton26 { get; set; }
        public Keys ActionButton27 { get; set; }
        public Keys ActionButton28 { get; set; }
        public Keys ActionButton29 { get; set; }
        public Keys ActionButton30 { get; set; }
        public Keys ActionButton31 { get; set; }
        public Keys ActionButton32 { get; set; }
        public Keys ActionButton33 { get; set; }
        public Keys ActionButton34 { get; set; }
        public Keys ActionButton35 { get; set; }
        public Keys ActionButton36 { get; set; }
        public Keys ActionButton37 { get; set; }
        public Keys ActionButton38 { get; set; }
        public Keys ActionButton39 { get; set; }
        public Keys ActionButton40 { get; set; }
        public Keys ActionButton41 { get; set; }
        public Keys ActionButton42 { get; set; }
        public Keys ActionButton43 { get; set; }
        public Keys ActionButton44 { get; set; }
        public Keys ActionButton45 { get; set; }
        public Keys ActionButton46 { get; set; }
        public Keys ActionButton47 { get; set; }
        public Keys ActionButton48 { get; set; }

        public bool IsKeySet(Keys Key)
        {
            if (MoveForward == Key ||
                MoveBackward == Key ||
                MoveLeft == Key ||
                MoveRight == Key ||
                RotateLeft == Key ||
                RotateRight == Key ||
                Walk == Key ||
                AutoMove == Key ||
                NextTarget == Key ||
                SelfTarget == Key ||
                ReqGo == Key ||
                ActionButton01 == Key ||
                ActionButton02 == Key ||
                ActionButton03 == Key ||
                ActionButton04 == Key ||
                ActionButton05 == Key ||
                ActionButton06 == Key ||
                ActionButton07 == Key ||
                ActionButton08 == Key ||
                ActionButton09 == Key ||
                ActionButton10 == Key ||
                ActionButton11 == Key ||
                ActionButton12 == Key ||
                ActionButton13 == Key ||
                ActionButton14 == Key ||
                ActionButton15 == Key ||
                ActionButton16 == Key ||
                ActionButton17 == Key ||
                ActionButton18 == Key ||
                ActionButton19 == Key ||
                ActionButton20 == Key ||
                ActionButton21 == Key ||
                ActionButton22 == Key ||
                ActionButton23 == Key ||
                ActionButton24 == Key ||
                ActionButton25 == Key ||
                ActionButton26 == Key ||
                ActionButton27 == Key ||
                ActionButton28 == Key ||
                ActionButton29 == Key ||
                ActionButton30 == Key ||
                ActionButton31 == Key ||
                ActionButton32 == Key ||
                ActionButton33 == Key ||
                ActionButton34 == Key ||
                ActionButton35 == Key ||
                ActionButton36 == Key ||
                ActionButton37 == Key ||
                ActionButton38 == Key ||
                ActionButton39 == Key ||
                ActionButton40 == Key ||
                ActionButton41 == Key ||
                ActionButton42 == Key ||
                ActionButton43 == Key ||
                ActionButton44 == Key ||
                ActionButton45 == Key ||
                ActionButton46 == Key ||
                ActionButton47 == Key ||
                ActionButton48 == Key)
                return true;

            else return false;
        }

        /// <summary>
        /// A static default keybinding.
        /// </summary>
        public static KeyBinding DEFAULT
        {
            get
            {
                KeyBinding defaultBinding = new KeyBinding();

                defaultBinding.RightClickAction = 13;

                // Movement
                defaultBinding.MoveForward = Keys.W;
                defaultBinding.MoveBackward = Keys.S;
                defaultBinding.MoveLeft = Keys.Q;
                defaultBinding.MoveRight = Keys.E;

                // Rotation
                defaultBinding.RotateLeft = Keys.A;
                defaultBinding.RotateRight = Keys.D;

                // Modifiers
                defaultBinding.Walk = Keys.LShiftKey;
                defaultBinding.AutoMove = Keys.NumLock;

                // Targetting
                defaultBinding.NextTarget = Keys.Tab;
                defaultBinding.SelfTarget = Keys.LMenu;

                // Actions
                defaultBinding.ReqGo = Keys.N;
                defaultBinding.Close = Keys.Escape;

                defaultBinding.ActionButton01 = Keys.D1;
                defaultBinding.ActionButton02 = Keys.D2;
                defaultBinding.ActionButton03 = Keys.D3;
                defaultBinding.ActionButton04 = Keys.D4;
                defaultBinding.ActionButton05 = Keys.D5;
                defaultBinding.ActionButton06 = Keys.D6;
                defaultBinding.ActionButton07 = Keys.D7;
                defaultBinding.ActionButton08 = Keys.D8;
                defaultBinding.ActionButton09 = Keys.D9;
                defaultBinding.ActionButton10 = Keys.D0;
                defaultBinding.ActionButton11 = Keys.H;
                defaultBinding.ActionButton12 = Keys.H;
                defaultBinding.ActionButton13 = Keys.Space;
                defaultBinding.ActionButton14 = Keys.R;
                defaultBinding.ActionButton15 = Keys.G;
                defaultBinding.ActionButton16 = Keys.I;
                defaultBinding.ActionButton17 = Keys.U;
                defaultBinding.ActionButton18 = Keys.B;
                defaultBinding.ActionButton19 = Keys.T;
                defaultBinding.ActionButton20 = Keys.F;
                defaultBinding.ActionButton21 = Keys.C;
                defaultBinding.ActionButton22 = Keys.V;
                defaultBinding.ActionButton23 = Keys.J;
                defaultBinding.ActionButton24 = Keys.J;
                defaultBinding.ActionButton25 = Keys.None;
                defaultBinding.ActionButton26 = Keys.None;
                defaultBinding.ActionButton27 = Keys.None;
                defaultBinding.ActionButton28 = Keys.None;
                defaultBinding.ActionButton29 = Keys.None;
                defaultBinding.ActionButton30 = Keys.None;
                defaultBinding.ActionButton31 = Keys.None;
                defaultBinding.ActionButton32 = Keys.None;
                defaultBinding.ActionButton33 = Keys.None;
                defaultBinding.ActionButton34 = Keys.None;
                defaultBinding.ActionButton35 = Keys.None;
                defaultBinding.ActionButton36 = Keys.None;
                defaultBinding.ActionButton37 = Keys.None;
                defaultBinding.ActionButton38 = Keys.None;
                defaultBinding.ActionButton39 = Keys.None;
                defaultBinding.ActionButton40 = Keys.None;
                defaultBinding.ActionButton41 = Keys.None;
                defaultBinding.ActionButton42 = Keys.None;
                defaultBinding.ActionButton43 = Keys.None;
                defaultBinding.ActionButton44 = Keys.None;
                defaultBinding.ActionButton45 = Keys.None;
                defaultBinding.ActionButton46 = Keys.None;
                defaultBinding.ActionButton47 = Keys.None;
                defaultBinding.ActionButton48 = Keys.None;

                return defaultBinding;
            }
        }
    }
}
