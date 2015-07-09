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

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Common.Constants
{
    /// <summary>
    /// Constants for geometry calculations.
    /// </summary>
    /// <remarks>
    /// This library is build in a scale like fine server/KOD coordinate-system/grid
    /// interpreted as floating point locally.
    /// </remarks>
    public static class GeometryConstants
    {
        /// <summary>
        /// Legacy FINENESS value from drawdefs.h
        /// This is the highest resolution the legacy client and the rooms are operating in.
        /// It means 1024 client rows/cols make 1 big row/col in the servergrid.
        /// </summary>
        public const int FINENESS = 1024;

        /// <summary>
        /// Legacy KOD_FINESS value from drawdefs.h.
        /// This is the highest resolution blakserv is running at.
        /// It means 64 fine rows/cols make 1 big row/col in the servergrid.
        /// The new client/renderer also operates in this scale, but with floating point type.
        /// </summary>
        public const int KOD_FINENESS = 64;

        /// <summary>
        /// Multiply by this to scale from server and new client FINENESS to legacy client FINENESS
        /// </summary>
        public const Real KODFINETOCLIENTFINE = (Real)FINENESS / (Real)KOD_FINENESS;

        /// <summary>
        /// Multiply by this to scale from legacy client FINENESS to server and new client FINENESS
        /// </summary>
        public const Real CLIENTFINETOKODFINE = (Real)KOD_FINENESS / (Real)FINENESS;

        /// <summary>
        /// Legacy LOG_FINENESS value from drawdefs.h
        /// </summary>
        public const int LOG_FINENESS = 10;
      
        /// <summary>
        /// Maximum height a player can step across a wall.
        /// From legacy move.c MAX_STEP_HEIGHT.
        /// </summary>
        public const int MAXSTEPHEIGHT = 24 << 4;

        /// <summary>
        /// Max. M59 angle value (in angle units).
        /// 4096 maps back to 0 like 2PI does to 0.
        /// </summary>
        public const ushort MAXANGLE = 4096;

        /// <summary>
        /// Half of the max angle value (in angleunits)
        /// </summary>
        public const ushort HALFMAXANGLE = MAXANGLE / 2;

        /// <summary>
        /// Quarter of the max angle (in angleunits)
        /// </summary>
        public const ushort QUARTERMAXANGLE = MAXANGLE / 4;

        /// <summary>
        /// From angle units to radian
        /// </summary>
        public const Real M59ANGLETORADQUOT = TWOPI / (Real)MAXANGLE;

        /// <summary>
        /// From radian to angle units
        /// </summary>
        public const Real RADTOM59ANGLEQUOT = (Real)MAXANGLE / TWOPI;

        /// <summary>
        /// Base coefficient for object movements.
        /// This is the distance moved in 1ms at a speed of 1.
        /// Distance in floatingpoint scale equal to the server fine-units.
        /// Multiply with dt in ms and speed (viSpeed).
        /// </summary>
        /// <remarks>
        /// Speed unit is defined as # big server square moves per 10000ms
        /// Calculates like this:
        /// ---------------------
        ///  * 0.0001f: # bigsquares per 1ms
        ///      * 64f: 64 finesquares per square
        /// </remarks>
        public const Real MOVEBASECOEFF = (Real)0.0064f;

        /// <summary>
        /// Base coefficient for projectile movement speed
        /// This is the distance moved in 1ms at a speed of 1.
        /// </summary>
        /// <remarks>
        /// Old client interprets projectile speeds 10x faster as
        /// object movements:
        /// # big server square moves per 1000ms
        /// </remarks>
        public const Real PROJECTILEMOVEBASECOEFF = 0.064f;

        /// <summary>
        /// Base coefficient for sector move speed.
        /// This is merged: Multiply by 16 and divide by 10000.
        /// See original code.
        /// </summary>
        public const Real SECTORMOVEBASECOEFF = 0.0016f;

        /// <summary>
        /// This is the gravity acceleration in unit/ms².
        /// Unit is distance in client scale.
        /// </summary>
        public const Real GRAVITYACCELERATION = -5.0f * (Real)FINENESS * 0.001f * 0.001f * 0.0625f;

        /// <summary>
        /// Objects must be this close for get, put, etc.
        /// In client scale.
        /// </summary>
        public const Real CLOSEDISTANCE = 5.0f * (Real)FINENESS * 0.0625f;

        /// <summary>
        /// Full period in radian (2*PI)
        /// </summary>
        public const Real TWOPI = (Real)(2.0 * Math.PI);

        /// <summary>
        /// Radian of quarter period
        /// </summary>
        public const Real QUARTERPERIOD = (Real)(0.25f * TWOPI);

        /// <summary>
        /// Radian of half period
        /// </summary>
        public const Real HALFPERIOD = (Real)(0.5f * TWOPI);
        
        /// <summary>
        /// Estimated player eyes height
        /// </summary>
        public const Real PLAYERHEIGHT = 50.0f;

        /// <summary>
        /// Estimated player width in KOD (also new client float) scale.
        /// </summary>
        /// <remarks>
        /// See 'SetPlayerInfo()' in 'clientd3d/game.c'
        /// </remarks>
        public const Real PLAYERWIDTH = 31.0f * (Real)KOD_FINENESS / 4.0f;

        /// <summary>
        /// Minimum distance player has to stay away from wall.
        /// This is half of the PLAYERWIDTH.
        /// </summary>
        public const Real WALLMINDISTANCE = PLAYERWIDTH / 2.0f;

        /// <summary>
        /// Squared minimum distance player has to stay away from wall
        /// </summary>
        public const Real WALLMINDISTANCE2 = WALLMINDISTANCE * WALLMINDISTANCE;

        /// <summary>
        /// Max. radius for objects behind
        /// to care about them.
        /// </summary>
        public const Real TARGETBEHINDRADIUS = 150.0f;

        /// <summary>
        /// Max. radius for objects in front
        /// to care about them.
        /// </summary>
        public const Real TARGETFRONTRADIUS = 2000.0f;

        /// <summary>
        /// Object block radius in world scale.
        /// See MIN_NOMOVEON in 'clientd3d/move.c'
        /// </summary>
        /// <remarks>
        /// Calculates like this:
        /// ---------------------
        ///   FINENESS: Use legacy definition as start
        ///    * 0.25f: This is (FINENESS / 4) like in orig.code
        ///  * 0.0625f: This lib uses server-scale in FP, orig is ROO/old client scale
        /// </remarks>
        public const Real MIN_NOMOVEON = (Real)FINENESS * 0.25f * 0.0625f;

        /// <summary>
        /// Simply MIN_NOMOVEON * MIN_NOMOVEON
        /// </summary>
        public const Real MIN_NOMOVEON2 = MIN_NOMOVEON * MIN_NOMOVEON;
    }
}
