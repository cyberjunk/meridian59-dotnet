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
using Meridian59.Files;
using Meridian59.Files.ROO;

namespace Meridian59.RooViewer
{
    static class Program
    {
        public static ResourceManager ResourceManager;
        public static MainForm MainForm;
        public static RooFile Room;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // init resources
            ResourceManager = new ResourceManager();
            ResourceManager.Init(
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHSTRINGS,
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHROOMS,
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHOBJECTS,
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHROOMTEXTURES,
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHSOUNDS,
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHMUSIC,
                Properties.Settings.Default.Resources + '/' + ResourceManager.SUBPATHMAILS);

            // create ui
            MainForm = new MainForm();

            // run
            Application.Run(MainForm);
        }
    }
}
