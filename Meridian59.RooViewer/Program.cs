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
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Meridian59.Files.BGF;
using Meridian59.Files;
using Meridian59.Files.ROO;
using Meridian59.Drawing2D;

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

            PalettesGDI.Initialize();

            // init resources
            ResourceManager = new ResourceManager();
            ResourceManager.Init(
                "",
                "",
                "",
                Properties.Settings.Default.PathTextures,
                "",
                "",
                "");

            // create ui
            MainForm = new MainForm();

            // run
            Application.Run(MainForm);
        }

        public static void OpenRoom(string File)
        {
            Room = new RooFile(File);
            Room.ResolveResources(ResourceManager);

            MainForm.Room = Room;          
        }

        public static void SaveRoom(string File)
        {
            Room.Save(File);
        }

        public static void ExtractAllTextures(string Folder)
        {
            if (Room == null)
                return;

            Dictionary<string, RooFile.MaterialInfo> textures = Room.GetMaterialInfos();
            BgfBitmap bgfbmp;
            Bitmap bmp;

            foreach(KeyValuePair<string, RooFile.MaterialInfo> obj in textures)
            {
                bgfbmp = obj.Value.Texture;

                if (bgfbmp == null)
                    continue;
                
                bmp = bgfbmp.GetBitmap();
                bmp.MakeTransparent(Color.Cyan);
                bmp.Save(Folder + '/' + obj.Value.TextureName, ImageFormat.Png);
                bmp.Dispose();
            }
        }
    }
}
