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

using Meridian59.Common;
using Meridian59.Data;
using Meridian59.Files;
using Meridian59.Common.Enums;

namespace Meridian59.Client
{
    /// <summary>
    /// Extends the BaseClient class by adding a Singleton pattern/instance.
    /// Use this if you want only one client instance within your application
    /// and want it to be easy accessible as Singleton.
    /// </summary>
    /// <remarks>
    /// The generics are supposed to allow you to use the abstract
    /// client class-hierarchy, but plug in your own/modified, deriving module-implementations.
    /// </remarks>
    /// <typeparam name="T">Type of GameTick or deriving class</typeparam>
    /// <typeparam name="R">Type of ResourceManager or deriving class</typeparam>
    /// <typeparam name="D">Type of DataController or deriving class</typeparam>
    /// <typeparam name="C">Type of Config or deriving class</typeparam>
    /// <typeparam name="S">Type of SingletonClient or deriving class</typeparam>
    public abstract class SingletonClient<T, R, D, C, S> : BaseClient<T, R, D, C>
        where T : GameTick, new()
        where R : ResourceManager, new()
        where D : DataController, new()
        where C : Config, new()
        where S : SingletonClient<T, R, D, C, S>, new()
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static S Singleton { get; private set; }

        /// <summary>
        /// Static constructor creating singleton instance.
        /// </summary>
        static SingletonClient()
        {
            // create singleton instance
            Singleton = new S();
        }
    }
}
