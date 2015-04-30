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
using Meridian59.Common;

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

        public static List<Tuple<RooSubSector, V2, float>> FindVertexMismatches(V2 Vertex)
        {
            const float CLOSE = 2;

            List<Tuple<RooSubSector, V2, float>> list = new List<Tuple<RooSubSector, V2, float>>();

            foreach (RooSubSector s in Room.BSPTreeLeaves)
            {
                foreach (V2 v in s.Vertices)
                {
                    float absdx = (float)Math.Abs(v.X - Vertex.X);
                    float absdy = (float)Math.Abs(v.Y - Vertex.Y);

                    // must be very close but not same pos
                    if (absdx <= CLOSE && absdy <= CLOSE && (absdx > 0 || absdy > 0))
                        list.Add(new Tuple<RooSubSector, V2, float>(s, v, Math.Max(absdx, absdy)));
                }
            }

            return list;
        }
        /*public static V2 FindClosestLineEndpoint(RooVertex Vertex)
        {            
            V2 p1, p2, p1p2;
            float len;
            float min = 9999999999f;
            V2 val = new V2();
            RooWall closest;

            //val.X = 0;
            //val.Y = 0;

            //if (Room.Walls.Count == 0)
            //    return val;

            p1.X = Vertex.X;
            p1.Y = Vertex.Y;

            foreach(RooWall wall in Room.Walls)
            {
                // first point
                p2.X = wall.X1;
                p2.Y = wall.Y1;

                p1p2 = p2 - p1;
                len = p1p2.LengthSquared;

                if (len < min)
                {
                    min = len;
                    closest = wall;
                    val = p2;
                }

                // second
                p2.X = wall.X2;
                p2.Y = wall.Y2;

                p1p2 = p2 - p1;
                len = p1p2.LengthSquared;

                if (len < min)
                {
                    min = len;
                    closest = wall;
                    val = p2;
                }
            }

            return val;
        }*/     
    }
}
