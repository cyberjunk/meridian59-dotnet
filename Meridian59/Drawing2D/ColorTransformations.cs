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
using Meridian59.Properties;

// Switch FP precision based on architecture
#if X64
using Real = System.Double;
#else 
using Real = System.Single;
#endif

namespace Meridian59.Drawing2D
{
    /// <summary>
    /// Provides Meridian59 color palettes
    /// </summary>
    public static class ColorTransformation
    {
        #region ColorTanslation IDs
        public const byte IDENTITY          = 0x00;
        
        public const byte DBLUETOSKIN1      = 0x01;
        public const byte DBLUETOSKIN2      = 0x02;
        public const byte DBLUETOSKIN3      = 0x03;
        public const byte DBLUETOSKIN4      = 0x04;
        public const byte DBLUETOSICKGREEN  = 0x05;
        public const byte DBLUETOSICKYELLOW = 0x06;
        public const byte DBLUETOGRAY       = 0x07;
        public const byte DBLUETOLBLUE      = 0x08;
        public const byte DBLUETOASHEN      = 0x09; // also makes gray eyes reddish

        public const byte GRAYTOORANGE      = 0x0A;
        public const byte GRAYTODGREEN      = 0x0B;
        public const byte GRAYTOBGREEN      = 0x0C;
        public const byte GRAYTOSKY         = 0x0D;
        public const byte GRAYTODBLUE       = 0x0E;
        public const byte GRAYTOPURPLE      = 0x0F;
        public const byte GRAYTOGOLD        = 0x10;
        public const byte GRAYTOBBLUE       = 0x11;
        public const byte GRAYTORED         = 0x12;
        public const byte GRAYTOLORANGE     = 0x13;
        public const byte GRAYTOLGREEN      = 0x14;
        public const byte GRAYTOLBGREEN     = 0x15;
        public const byte GRAYTOLSKY        = 0x16;
        public const byte GRAYTOLBLUE       = 0x17;
        public const byte GRAYTOLPURPLE     = 0x18;
        public const byte GRAYTOLGOLD       = 0x19;
        public const byte GRAYTOSKIN1       = 0x1A;
        public const byte GRAYTOSKIN2       = 0x1B;
        public const byte GRAYTOSKIN3       = 0x1C;
        public const byte GRAYTOSKIN4       = 0x1D;
        public const byte GRAYTOSKIN5       = 0x1E;
        public const byte GRAYTOLBBLUE      = 0x20;
        public const byte GRAYTOLRED        = 0x21;
        public const byte GRAYTOKORANGE     = 0x22;
        public const byte GRAYTOKGREEN      = 0x23;
        public const byte GRAYTOKBGREEN     = 0x24;
        public const byte GRAYTOKSKY        = 0x25;
        public const byte GRAYTOKBLUE       = 0x26;
        public const byte GRAYTOKPURPLE     = 0x27;
        public const byte GRAYTOKGOLD       = 0x28;
        public const byte GRAYTOKBBLUE      = 0x29;
        public const byte GRAYTOKRED        = 0x2A;
        public const byte GRAYTOKGRAY       = 0x2B;
        public const byte GRAYTOBLACK       = 0x2C;
        public const byte GRAYTOOLDHAIR1    = 0x2D;
        public const byte GRAYTOOLDHAIR2    = 0x2E;
        public const byte GRAYTOOLDHAIR3    = 0x2F;
        public const byte GRAYTOBLOND       = GRAYTOOLDHAIR3;
        public const byte GRAYTOPLATBLOND   = 0x30;

        public const byte FILTERWHITE90     = 0x31;
        public const byte FILTERWHITE80     = 0x32;
        public const byte FILTERWHITE70     = 0x33;
        public const byte FILTERBRIGHT1     = 0x36;
        public const byte FILTERBRIGHT2     = 0x37;
        public const byte FILTERBRIGHT3     = 0x38;

        public const byte BLEND25YELLOW     = 0x39;

        public const byte PURPLETOLBLUE     = 0x3A;
        public const byte PURPLETOBRED      = 0x3B;
        public const byte PURPLETOGREEN     = 0x3C;
        public const byte PURPLETOYELLOW    = 0x3D;

        public const byte BLEND10RED        = 0x41;
        public const byte BLEND20RED        = 0x42;
        public const byte BLEND30RED        = 0x43;
        public const byte BLEND40RED        = 0x44;
        public const byte BLEND50RED        = 0x45;
        public const byte BLEND60RED        = 0x46;
        public const byte BLEND70RED        = 0x47;
        public const byte BLEND80RED        = 0x48;
        public const byte BLEND90RED        = 0x49;
        public const byte BLEND100RED       = 0x4A;
        public const byte FILTERRED         = 0x4D;
        public const byte FILTERBLUE        = 0x4E;
        public const byte FILTERGREEN       = 0x4F;
        
        public const byte BLEND25RED        = 0x51;
        public const byte BLEND25BLUE       = 0x52;
        public const byte BLEND25GREEN      = 0x53;
        public const byte BLEND50BLUE       = 0x55;
        public const byte BLEND50GREEN      = 0x56;
        public const byte BLEND75RED        = 0x57;
        public const byte BLEND75BLUE       = 0x58;
        public const byte BLEND75GREEN      = 0x59;
#if !VANILLA
        public const byte REDTOBLACK        = 0x5A;
        public const byte BLUETOBLACK       = 0x5B;
        public const byte PURPLETOBLACK     = 0x5C;
#endif
        public const byte RAMPUP1           = 0x60;
        public const byte RAMPUP2           = 0x61;
        public const byte RAMPDOWN2         = 0x6E;
        public const byte RAMPDOWN1         = 0x6F;
        
        public const byte BLEND10WHITE      = 0x70;
        public const byte BLEND20WHITE      = 0x71;
        public const byte BLEND30WHITE      = 0x72;
        public const byte BLEND40WHITE      = 0x73;
        public const byte BLEND50WHITE      = 0x74;
        public const byte BLEND60WHITE      = 0x75;
        public const byte BLEND70WHITE      = 0x76;
        public const byte BLEND80WHITE      = 0x77;
        public const byte BLEND90WHITE      = 0x78;
        public const byte BLEND100WHITE     = 0x79;

        public const byte REDTODGREEN1      = 0x7A;
        public const byte REDTODGREEN2      = 0x7B;
        public const byte REDTODGREEN3      = 0x7C;
        public const byte REDTOBLACK1       = 0x7D;
        public const byte REDTOBLACK2       = 0x7E;
        public const byte REDTOBLACK3       = 0x7F;
#if !VANILLA
        public const byte REDTODKBLACK1     = 0x80;
        public const byte REDTODKBLACK2     = 0x81;
        public const byte REDTODKBLACK3     = 0x82;
        public const byte REDBLK_BLWHT      = 0x83;
        public const byte BLBLK_REDWHT      = 0x84;
#endif
        public const byte GUILDCOLOR_BASE   = 0x87;
        public const byte GUILDCOLOR_END    = 0xFF;
        #endregion

        #region Constants
        public const byte TRANSPARENTCOLORINDEX = 254;
        public const uint COLORCOUNT            = 256;
        public const uint PALETTECOUNT          = 256;
        public const uint LIGHTLEVELS           = 64;
        public const ushort PALETTEROWS         = 16;
        public const ushort PALETTECOLUMNS      = 16;
        
        /// <summary>
        /// See GuildShieldInit() in merintr/guildshi.c
        /// </summary>
        public static readonly int NUMGUILDCOLORS = (int)Math.Sqrt((Real)(GUILDCOLOR_END - GUILDCOLOR_BASE + 1));

        /// <summary>
        /// These are the real color ramp indices (red, blue, ...) from the palette.
        /// Does not include rows with special purposes/colors.
        /// </summary>
        private static readonly byte[] RAMPS = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x07, 0x09, 0x0A, 0x0C, 0x0D, 0x0E };
        
        /// <summary>
        /// Palette indexes which are not found, computed or changed, ever.
        /// These are the magic four Windows colors, plus any colors reserved for
        /// special effects like luminant colors.
        /// </summary>
        private static readonly int[] NEVERMAP_LIST = 
        {
	        8, 9, 10, 11, 12, 13, 14, 15,
	        247, 246, 245, 244,
	        -1
        };

        private static readonly bool[] NEVERMAP = new bool[COLORCOUNT];
        
        private static readonly byte[] OLDHAIR1_INDEXES = {
           0x23, 0x32, 0x34, 0x36, 0x59, 0x39, 0x3A, 0x3B, 
           0x36, 0x38, 0x5B, 0x47, 0x3C, 0x5C, 0x5E, 0x5E,
        };

        private static readonly byte[] OLDHAIR2_INDEXES = {
           0x50, 0x23, 0x24, 0x33, 0x25, 0x34, 0x26, 0x35, 
           0x27, 0x36, 0x28, 0x37, 0x29, 0x38, 0x2A, 0x39,
        };

        private static readonly byte[] OLDHAIR3_INDEXES = {
           0xC4, 0x52, 0xC6, 0x53, 0xC8, 0x54, 0xCA, 0x55, 
           0x56, 0x57, 0x58, 0x58, 0x59, 0x59, 0x5A, 0x5B,
        };

        private static readonly byte[] PLATBLOND_INDEXES = {
           0xB0, 0xB0, 0xB1, 0xB1, 0xB2, 0xB2, 0xD0, 0xD1, 
           0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD9, 0xDA,
        };

        private static readonly byte[] SKIN1_INDEXES = {
            0x20, 0xF0, 0xF0, 0x21, 0x21, 0x22, 0x22, 0x23, 
            0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B 
        };

        private static readonly byte[] SKIN2_INDEXES = {
           0x20, 0x20, 0xF0, 0x21, 0x22, 0x23, 0x23, 0x24, 
           0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C,
        };

        private static readonly byte[] GREEN_SKIN_INDEXES = {
           0xD0, 0xD0, 0xB0, 0xB8, 0x60, 0x60, 0x61, 0x61, 
           0x62, 0x63, 0x64, 0x66, 0x67, 0x68, 0x6A, 0x6C,
        };

        private static readonly byte[] YELLOW_SKIN_INDEXES = {
           0xD0, 0xD0, 0xB0, 0xB0, 0xB1, 0xB2, 0xB3, 0xC0, 
           0xC6, 0xC9, 0xCA, 0xCC, 0xCD, 0xCE, 0xCF, 0xCF,
        };
        #endregion
       
        /// <summary>
        /// Static constructor
        /// </summary>
        static ColorTransformation()
        {
            Palettes = new uint[PALETTECOUNT][];
            LightPalettes = new uint[LIGHTLEVELS + 1][];

            // load default palette from resource (also called IDENTITY)
            Palettes[0] = GetDefaultPalette();

            BlockIndices();

            // create light palettes (must be done before normal palettes)
            CreateLightPalettes();

            // create all transformed palettes
            CreatePalettes();

            // create a clone of the default palette
            // with transparent color set to black
            DefaultPaletteBlackTransparency = ClonePalette(DefaultPalette);
            DefaultPaletteBlackTransparency[TRANSPARENTCOLORINDEX] = 0x00000000;
        }

        /// <summary>
        /// All available, transformed colorpalettes
        /// </summary>
        public static readonly uint[][] Palettes;

        /// <summary>
        /// All brightened or darkened palette transformations
        /// </summary>
        public static readonly uint[][] LightPalettes;

        /// <summary>
        /// The default palette
        /// </summary>
        public static uint[] DefaultPalette
        {
            get { return Palettes[0]; }
        }

        /// <summary>
        /// A clone of DefaultPalette but with color 254 set to
        /// black instead of cyan.
        /// </summary>
        public static uint[] DefaultPaletteBlackTransparency
        {
            get;
            private set;
        }

        #region Manipulations / XLats
        /// <summary>
        /// Writes all the colors (16) from one row to another
        /// </summary>
        /// <param name="FromPalette">Palette to read from.</param>
        /// <param name="FromRamp">Index of Row to read colors from.</param>
        /// <param name="ToPalette">Palette to write to.</param>
        /// <param name="ToRamp">Index of Row to write colors to.</param>
        private static void RampXLat(uint[] FromPalette, byte FromRamp, uint[] ToPalette, byte ToRamp)
        {
            int offsetRead = FromRamp * PALETTECOLUMNS;
            int offsetWrite = ToRamp * PALETTECOLUMNS;

            for (int i = 0; i < PALETTECOLUMNS; i++)
                ToPalette[offsetWrite + i] = FromPalette[offsetRead + i];
        }

        /// <summary>
        /// Writes all colors from the given table indices to target row.
        /// </summary>
        /// <param name="FromPalette">Palette to read from.</param>
        /// <param name="FromIndices">Indices to read from (array must have length of 16)</param>
        /// <param name="ToPalette">Palette to write to.</param>
        /// <param name="ToRamp">Index of Row to write colors to.</param>
        private static void RampXLat(uint[] FromPalette, byte[] FromIndices, uint[] ToPalette, byte ToRamp)
        {
            int offsetWrite = ToRamp * PALETTECOLUMNS;

            for (int i = 0; i < PALETTECOLUMNS; i++)
                ToPalette[offsetWrite + i] = FromPalette[FromIndices[i]];
        }

        /// <summary>
        /// Copies half a row starting at offset to a full destination row,
        /// uses a color twice.
        /// </summary>
        /// <param name="FromPalette"></param>
        /// <param name="FromRamp"></param>
        /// <param name="ToPalette"></param>
        /// <param name="ToRamp"></param>
        /// <param name="FromOffset"></param>
        private static void HalfRampXLat(uint[] FromPalette, byte FromRamp, uint[] ToPalette, byte ToRamp, byte FromOffset)
        {
            int offsetRead = FromRamp * PALETTECOLUMNS + FromOffset;
            int offsetWrite = ToRamp * PALETTECOLUMNS;

            for (int i = 0; i < PALETTECOLUMNS; i++)
                ToPalette[offsetWrite + i] = FromPalette[offsetRead + i / 2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FromPalette"></param>
        /// <param name="FilterColor"></param>
        /// <param name="ToPalette"></param>
        private static void FilterXLat(uint[] FromPalette, uint FilterColor, uint[] ToPalette)
        {
            // taken from GetRGBLightness() in xlat.c

            int l;
            byte byLightness;
            uint color;
            byte a, r, g, b;
            byte peRed, peGreen, peBlue;
            int colorindex;

            for (int i = 0; i < COLORCOUNT; i++)
            {
                color = FromPalette[i];

                // get alpha of this original color
                a = (byte)((color & 0xFF000000) >> 24);

                // only continue to lookup for non transparent colors
                // because we want the transparent color to stay transparent
                if (a > 0)
                {
                    // get r, g, b components of current color
                    r = (byte)((color & 0x00FF0000) >> 16);
                    g = (byte)((color & 0x0000FF00) >> 8);
                    b = (byte)(color & 0x000000FF);

                    // calc ligtness by rgb
                    byLightness = (byte)(Math.Max(Math.Max(r, g), b) & 0xFF);

                    // get components of filter color                
                    peRed = (byte)((FilterColor & 0x00FF0000) >> 16);
                    peGreen = (byte)((FilterColor & 0x0000FF00) >> 8);
                    peBlue = (byte)(FilterColor & 0x000000FF);

                    l = ((int)peRed) * ((int)byLightness) / 256;
                    peRed = (byte)(l & 0xFF);

                    l = ((int)peGreen) * ((int)byLightness) / 256;
                    peGreen = (byte)(l & 0xFF);

                    l = ((int)peBlue) * ((int)byLightness) / 256;
                    peBlue = (byte)(l & 0xFF);

                    // compose A|R|G|B value
                    color = 0xFF000000 | ((uint)peRed << 16) | ((uint)peGreen << 8) | (uint)peBlue;

                    // get closest color
                    colorindex = GetClosestPaletteIndex(FromPalette, color);
                }
                else
                    colorindex = TRANSPARENTCOLORINDEX;

                // save
                ToPalette[i] = FromPalette[colorindex];
            }
        }

        /// <summary>
        /// Brighten up or darken transformations based on light palettes
        /// </summary>
        /// <param name="Palette"></param>
        /// <param name="FromRamp"></param>
        /// <param name="Light"></param>
        private static void LightXLat(uint[] Palette, byte FromRamp, byte Light)
        {
            int offset = FromRamp * PALETTECOLUMNS;

            for (int i = 0; i < PALETTECOLUMNS; i++)
            {
                Palette[offset] = LightPalettes[Light][offset];
                offset++;
            }
        }

        /// <summary>
        /// Blends the palettes colors with given Color using the given Weights for blendcolor/original one.
        /// </summary>
        /// <param name="FromPalette"></param>
        /// <param name="ToPalette"></param>
        /// <param name="BlendColor"></param>
        /// <param name="WeightOriginal"></param>
        /// <param name="WeightBlend"></param>
        private static void BlendXLat(uint[] FromPalette, uint[] ToPalette, uint BlendColor, uint WeightOriginal, uint WeightBlend)
        {
            // taken from CalcBlendXlat() in xlat.c

            int l;
            byte r, g, b;
            byte peAlpha, peRed, peGreen, peBlue;
            uint color;
            int colorindex;
            uint sumweight = WeightOriginal + WeightBlend;

            for (int i = 0; i < COLORCOUNT; i++)
            {
                // get original color which is to be modified in this loop
                color = FromPalette[i];

                // get alpha of this original color
                peAlpha = (byte)((color & 0xFF000000) >> 24);

                // only continue to lookup for non transparent colors
                // because we want the transparent color to stay transparent
                if (peAlpha > 0)
                {
                    // get components of original color                
                    peRed = (byte)((color & 0x00FF0000) >> 16);
                    peGreen = (byte)((color & 0x0000FF00) >> 8);
                    peBlue = (byte)(color & 0x000000FF);

                    // get components of blendcolor
                    r = (byte)((BlendColor & 0x00FF0000) >> 16);
                    g = (byte)((BlendColor & 0x0000FF00) >> 8);
                    b = (byte)(BlendColor & 0x000000FF);

                    // build target red
                    l = (int)(((peRed * WeightOriginal) + (r * WeightBlend)) / sumweight);
                    peRed = (byte)(l & 0xFF);

                    // build target green
                    l = (int)(((peGreen * WeightOriginal) + (g * WeightBlend)) / sumweight);
                    peGreen = (byte)(l & 0xFF);

                    // build target blue
                    l = (int)(((peBlue * WeightOriginal) + (b * WeightBlend)) / sumweight);
                    peBlue = (byte)(l & 0xFF);

                    // compose A|R|G|B value
                    color = 0xFF000000 | ((uint)peRed << 16) | ((uint)peGreen << 8) | (uint)peBlue;

                    // get closest color
                    colorindex = GetClosestPaletteIndex(DefaultPalette, color);
                }
                else
                    colorindex = TRANSPARENTCOLORINDEX;

                // set output color
                ToPalette[i] = FromPalette[colorindex];
            }
        }

        /// <summary>
        /// This simply replaces any entry in the palette with the given color.
        /// EXCEPT the transparent index.
        /// </summary>
        /// <param name="Palette"></param>
        /// <param name="Color"></param>
        private static void ReplaceXLat(uint[] Palette, uint Color)
        {
            for (int i = 0; i < COLORCOUNT; i++)
                if (i != TRANSPARENTCOLORINDEX)
                    Palette[i] = Color;
        }

        /// <summary>
        /// Scrolls the palette rows noted in "ramps" (so not all!) up or down by RowOffset (pos/neg).
        /// </summary>
        /// <param name="FromPalette"></param>
        /// <param name="ToPalette"></param>
        /// <param name="RowOffset"></param>
        private static void RampOffsetXLat(uint[] FromPalette, uint[] ToPalette, int RowOffset)
        {
            int srcrow;
            int dstrow;
            int destindex;
            int srcindex;
            int rampidx;

            // walk ramps
            for (int i = 0; i < RAMPS.Length; i++)
            {
                // get the index in ramps after applied offset
                rampidx = i - RowOffset;

                // map exceedings back (scroll out)
                if (rampidx < 0)
                    rampidx = RAMPS.Length + rampidx;
                else if (rampidx >= RAMPS.Length)
                    rampidx = rampidx - RAMPS.Length;

                // rows in the palette
                srcrow = RAMPS[i];
                dstrow = RAMPS[rampidx];

                // walk'n copy
                for (int j = 0; j < PALETTECOLUMNS; j++)
                {
                    // palette indexes
                    srcindex = j + (srcrow * PALETTECOLUMNS);
                    destindex = j + (dstrow * PALETTECOLUMNS);

                    // copy
                    ToPalette[destindex] = FromPalette[srcindex];
                }
            }
        }
        #endregion

        /// <summary>
        /// Returns a cloned instance of ColorPalette
        /// </summary>
        /// <param name="Palette"></param>
        /// <returns></returns>
        private static uint[] ClonePalette(uint[] Palette)
        {
            uint[] pal = new uint[COLORCOUNT];

            for (int i = 0; i < COLORCOUNT; i++)
                pal[i] = Palette[i];

            return pal;
        }

        /// <summary>
        /// Returns the index in Palette of the color closest to InputColor.
        /// </summary>
        /// <param name="Palette">256 ARGB colors</param>
        /// <param name="InputColor">ARGB color</param>
        /// <returns></returns>
        private static int GetClosestPaletteIndex(uint[] Palette, uint InputColor)
        {
            uint color;
            byte r, g, b;
            byte r2, g2, b2;
            
            int rdist, bdist, gdist;
            int distance;
            int minindex = -1;
            int mindistance = 1 << 30;

            r2 = (byte)((InputColor & 0x00FF0000) >> 16);
            g2 = (byte)((InputColor & 0x0000FF00) >> 8);
            b2 = (byte)(InputColor & 0x000000FF);

            for (int i = 0; i < COLORCOUNT; i++)
            {
                color = Palette[i];
                r = (byte)((color & 0x00FF0000) >> 16);
                g = (byte)((color & 0x0000FF00) >> 8);
                b = (byte)(color & 0x000000FF);

                // diff
                rdist = r - r2;
                gdist = g - g2;
                bdist = b - b2;
                distance = rdist * rdist + gdist * gdist + bdist * bdist;

                if (distance < mindistance)
                {
                    mindistance = distance;
                    minindex = i;
                }
            }

            return minindex;
        }

        /// <summary>
        /// Populates the NEVERMAP array.
        /// </summary>
        private static void BlockIndices()
        {
            int i;
            for (i = 0; i < COLORCOUNT; i++)
                NEVERMAP[i] = false;

            for (i = 0; NEVERMAP_LIST[i] >= 0; i++)
                NEVERMAP[NEVERMAP_LIST[i]] = true;
        }

        /// <summary>
        /// Returns the default colorpalette from embedded byte[] colortable
        /// </summary>
        /// <returns></returns>
        private static unsafe uint[] GetDefaultPalette()
        {
            uint[] pal = new uint[COLORCOUNT];
            byte[] colortable = Resources.BitmapColorTable;

            fixed (byte* ptable = colortable)
            {
                uint* pcursor = (uint*)ptable;

                for (int i = 0; i < COLORCOUNT; i++)
                {
                    pal[i] = pcursor[0];
                    pcursor++;
                }
            }

            return pal;
        }

        /// <summary>
        /// Precompute palettes for varying light levels based on defaultpalette.
        /// </summary>
        private static void CreateLightPalettes()
        {
            uint r, g, b;
            uint color;
            int index;

            /* Fraction of full brightness in a particular palette */
            Real factor;

            for (int i = 0; i < LIGHTLEVELS; i++)
            {
                // init with default palette (costly?change to empty dummy)
                LightPalettes[i] = ClonePalette(Palettes[0]);

                /* Quadratic dependence so that there are more palettes close to full brightness */
                factor = 1 - ((Real)LIGHTLEVELS - i) / LIGHTLEVELS *
                    ((Real)LIGHTLEVELS - i) / LIGHTLEVELS;

                for (int j = 0; j < COLORCOUNT; j++)
                {
                    if (NEVERMAP[j])
                        continue;

                    // color
                    color = Palettes[0][j];

                    // get color components
                    r = (color & 0x00FF0000) >> 16;
                    g = (color & 0x0000FF00) >> 8;
                    b = color & 0x000000FF;

                    // brighten up (max 255)
                    r = System.Math.Min((uint)(r * factor), 255);
                    g = System.Math.Min((uint)(g * factor), 255);
                    b = System.Math.Min((uint)(b * factor), 255);

                    // compose A|R|G|B value
                    color = 0xFF000000 | (r << 16) | (g << 8) | b;

                    // find closest color
                    index = GetClosestPaletteIndex(Palettes[0], color);

                    // save it
                    LightPalettes[i][j] = Palettes[0][index];
                }
            }

            // INVERT 

            // init with default palette (costly?change to empty dummy)
            LightPalettes[LIGHTLEVELS] = ClonePalette(Palettes[0]);

            // Make inverted effect palette
            for (int j = 0; j < COLORCOUNT; j++)
            {
                if (NEVERMAP[LIGHTLEVELS])
                    continue;

                // inverted components
                r = 255U - ((Palettes[0][j] & 0x00FF0000) >> 16);
                g = 255U - ((Palettes[0][j] & 0x0000FF00) >> 8);
                b = 255U - ((Palettes[0][j] & 0x000000FF));

                // compose A|R|G|B
                color = 0xFF000000 | (r << 16) | (g << 8) | b;

                // find closest color
                index = GetClosestPaletteIndex(Palettes[0], color);

                // save it
                LightPalettes[LIGHTLEVELS][j] = Palettes[0][index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void CreatePalettes()
        {
            // (1) --- Create all palette instances (just clone)

            for (int k = 1; k < COLORCOUNT; k++)
                Palettes[k] = ClonePalette(DefaultPalette);
            
            // (2) --- Manipulate the custom ones (incomplete)
                   
            RampXLat(DefaultPalette, SKIN1_INDEXES, Palettes[DBLUETOSKIN1], 9);                 // 0x01
            RampXLat(DefaultPalette, SKIN2_INDEXES, Palettes[DBLUETOSKIN2], 9);                 // 0x02
            RampXLat(DefaultPalette, 3, Palettes[DBLUETOSKIN3], 9);                             // 0x03
            RampXLat(DefaultPalette, 4, Palettes[DBLUETOSKIN4], 9);                             // 0x04
            RampXLat(DefaultPalette, GREEN_SKIN_INDEXES, Palettes[DBLUETOSICKGREEN], 9);        // 0x05
            RampXLat(DefaultPalette, YELLOW_SKIN_INDEXES, Palettes[DBLUETOSICKYELLOW], 9);      // 0x06
            RampXLat(DefaultPalette, 13, Palettes[DBLUETOGRAY], 9);                             // 0x07
            RampXLat(DefaultPalette, 8, Palettes[DBLUETOLBLUE], 9);                             // 0x08
            
            RampXLat(DefaultPalette, 13, Palettes[DBLUETOASHEN], 9);                            // 0x09
            RampXLat(DefaultPalette, 1, Palettes[DBLUETOASHEN], 13);                            // 0x09
            
            RampXLat(DefaultPalette, 5, Palettes[GRAYTOORANGE], 13);                            // 0x0A
            RampXLat(DefaultPalette, 6, Palettes[GRAYTODGREEN], 13);                            // 0x0B
            RampXLat(DefaultPalette, 7, Palettes[GRAYTOBGREEN], 13);                            // 0x0C
            RampXLat(DefaultPalette, 8, Palettes[GRAYTOSKY], 13);                               // 0x0D
            RampXLat(DefaultPalette, 9, Palettes[GRAYTODBLUE], 13);                             // 0x0E          
            RampXLat(DefaultPalette, 10, Palettes[GRAYTOPURPLE], 13);                           // 0x0F
            RampXLat(DefaultPalette, 12, Palettes[GRAYTOGOLD], 13);                             // 0x10
            RampXLat(DefaultPalette, 14, Palettes[GRAYTOBBLUE], 13);                            // 0x11
            RampXLat(DefaultPalette, 1, Palettes[GRAYTORED], 13);                               // 0x12
            HalfRampXLat(DefaultPalette, 5, Palettes[GRAYTOLORANGE], 13, 0);                    // 0x13
            HalfRampXLat(DefaultPalette, 6, Palettes[GRAYTOLGREEN], 13, 0);                     // 0x14
            HalfRampXLat(DefaultPalette, 7, Palettes[GRAYTOLBGREEN], 13, 0);                    // 0x15
            HalfRampXLat(DefaultPalette, 8, Palettes[GRAYTOLSKY], 13, 0);                       // 0x16
            HalfRampXLat(DefaultPalette, 14, Palettes[GRAYTOLBLUE], 13, 0);                     // 0x17
            HalfRampXLat(DefaultPalette, 10, Palettes[GRAYTOLPURPLE], 13, 0);                   // 0x18
            HalfRampXLat(DefaultPalette, 12, Palettes[GRAYTOLGOLD], 13, 0);                     // 0x19
            RampXLat(DefaultPalette, 2, Palettes[GRAYTOSKIN1], 13);                             // 0x1A
            RampXLat(DefaultPalette, 3, Palettes[GRAYTOSKIN2], 13);                             // 0x1B
            HalfRampXLat(DefaultPalette, 3, Palettes[GRAYTOSKIN3], 13, 5);                      // 0x1C
            RampXLat(DefaultPalette, 4, Palettes[GRAYTOSKIN4], 13);                             // 0x1D
            HalfRampXLat(DefaultPalette, 4, Palettes[GRAYTOSKIN5], 13, 8);                      // 0x1E

            HalfRampXLat(DefaultPalette, 9, Palettes[GRAYTOLBBLUE], 13, 0);                     // 0x20
            HalfRampXLat(DefaultPalette, 1, Palettes[GRAYTOLRED], 13, 0);                       // 0x21
            HalfRampXLat(DefaultPalette, 5, Palettes[GRAYTOKORANGE], 13, 8);                    // 0x22
            HalfRampXLat(DefaultPalette, 6, Palettes[GRAYTOKGREEN], 13, 8);                     // 0x23
            HalfRampXLat(DefaultPalette, 7, Palettes[GRAYTOKBGREEN], 13, 8);                    // 0x24
            HalfRampXLat(DefaultPalette, 8, Palettes[GRAYTOKSKY], 13, 8);                       // 0x25
            HalfRampXLat(DefaultPalette, 14, Palettes[GRAYTOKBLUE], 13, 8);                     // 0x26
            HalfRampXLat(DefaultPalette, 10, Palettes[GRAYTOKPURPLE], 13, 8);                   // 0x27
            HalfRampXLat(DefaultPalette, 12, Palettes[GRAYTOKGOLD], 13, 8);                     // 0x28
            HalfRampXLat(DefaultPalette, 9, Palettes[GRAYTOKBBLUE], 13, 8);                     // 0x29

            HalfRampXLat(DefaultPalette, 1, Palettes[GRAYTOKRED], 13, 8);                       // 0x2A
            HalfRampXLat(DefaultPalette, 13, Palettes[GRAYTOKGRAY], 13, 8);                     // 0x2B
            LightXLat(Palettes[GRAYTOBLACK], 13, (byte)(LIGHTLEVELS / 6));                      // 0x2C
            RampXLat(DefaultPalette, OLDHAIR1_INDEXES, Palettes[GRAYTOOLDHAIR1], 13);           // 0x2D
            RampXLat(DefaultPalette, OLDHAIR2_INDEXES, Palettes[GRAYTOOLDHAIR2], 13);           // 0x2E
            RampXLat(DefaultPalette, OLDHAIR3_INDEXES, Palettes[GRAYTOOLDHAIR3], 13);           // 0x2F
            RampXLat(DefaultPalette, PLATBLOND_INDEXES, Palettes[GRAYTOPLATBLOND], 13);         // 0x30          
            FilterXLat(DefaultPalette, 0xFFF0F0F0, Palettes[FILTERWHITE90]);                    // 0x31
            FilterXLat(DefaultPalette, 0xFFDCDCDC, Palettes[FILTERWHITE80]);                    // 0x32
            FilterXLat(DefaultPalette, 0xFFC8C8C8, Palettes[FILTERWHITE70]);                    // 0x33
            
            FilterXLat(DefaultPalette, 0xFFFDFDFD, Palettes[FILTERBRIGHT1]);                    // 0x36
            FilterXLat(DefaultPalette, 0xFFFAFAFA, Palettes[FILTERBRIGHT2]);                    // 0x37
            FilterXLat(DefaultPalette, 0xFFF5F5F5, Palettes[FILTERBRIGHT3]);                    // 0x38
            
            BlendXLat(DefaultPalette, Palettes[BLEND25YELLOW], 0xFFFFFF00, 75, 25);             // 0x39

            RampXLat(DefaultPalette, 8, Palettes[PURPLETOLBLUE], 10);                           // 0x3A          
            RampXLat(DefaultPalette, 1, Palettes[PURPLETOBRED], 10);                            // 0x3B          
            RampXLat(DefaultPalette, 7, Palettes[PURPLETOGREEN], 10);                           // 0x3C          
            RampXLat(DefaultPalette, 5, Palettes[PURPLETOYELLOW], 10);                          // 0x3D          
            
            BlendXLat(DefaultPalette, Palettes[BLEND10RED], 0xFFFF0000, 90, 10);                // 0x41
            BlendXLat(DefaultPalette, Palettes[BLEND20RED], 0xFFFF0000, 80, 20);                // 0x42
            BlendXLat(DefaultPalette, Palettes[BLEND30RED], 0xFFFF0000, 70, 30);                // 0x43
            BlendXLat(DefaultPalette, Palettes[BLEND40RED], 0xFFFF0000, 60, 40);                // 0x44
            BlendXLat(DefaultPalette, Palettes[BLEND50RED], 0xFFFF0000, 50, 50);                // 0x45
            BlendXLat(DefaultPalette, Palettes[BLEND60RED], 0xFFFF0000, 40, 60);                // 0x46
            BlendXLat(DefaultPalette, Palettes[BLEND70RED], 0xFFFF0000, 30, 70);                // 0x47
            BlendXLat(DefaultPalette, Palettes[BLEND80RED], 0xFFFF0000, 20, 80);                // 0x48
            BlendXLat(DefaultPalette, Palettes[BLEND90RED], 0xFFFF0000, 10, 90);                // 0x49
            ReplaceXLat(Palettes[BLEND100RED], 0xFFFF0000);                                     // 0x4A

            FilterXLat(DefaultPalette, 0xFFFF0000, Palettes[FILTERRED]);                        // 0x4D
            FilterXLat(DefaultPalette, 0xFF0000FF, Palettes[FILTERBLUE]);                       // 0x4E
            FilterXLat(DefaultPalette, 0xFF00FF00, Palettes[FILTERGREEN]);                      // 0x4F

            BlendXLat(DefaultPalette, Palettes[BLEND25RED], 0xFFFF0000, 75, 25);                // 0x51
            BlendXLat(DefaultPalette, Palettes[BLEND25BLUE], 0xFF0000FF, 75, 25);               // 0x52
            BlendXLat(DefaultPalette, Palettes[BLEND25GREEN], 0xFF00FF00, 75, 25);              // 0x53
            BlendXLat(DefaultPalette, Palettes[BLEND50BLUE], 0xFF0000FF, 50, 50);               // 0x55
            BlendXLat(DefaultPalette, Palettes[BLEND50GREEN], 0xFF00FF00, 50, 50);              // 0x56
            BlendXLat(DefaultPalette, Palettes[BLEND75RED], 0xFFFF0000, 25, 75);                // 0x57
            BlendXLat(DefaultPalette, Palettes[BLEND75BLUE], 0xFF0000FF, 25, 75);               // 0x58
            BlendXLat(DefaultPalette, Palettes[BLEND75GREEN], 0xFF00FF00, 25, 75);              // 0x59
#if !VANILLA
            LightXLat(Palettes[REDTOBLACK], 1, 0);                                              // 0x5A
            LightXLat(Palettes[BLUETOBLACK], 9, 0);                                             // 0x5B
            LightXLat(Palettes[PURPLETOBLACK], 10, 0);                                          // 0x5C
#endif
            RampOffsetXLat(DefaultPalette, Palettes[RAMPUP1], +1);                              // 0x60
            RampOffsetXLat(DefaultPalette, Palettes[RAMPUP2], +2);                              // 0x61
            RampOffsetXLat(DefaultPalette, Palettes[RAMPDOWN2], -2);                            // 0x6E
            RampOffsetXLat(DefaultPalette, Palettes[RAMPDOWN1], -1);                            // 0x6F

            BlendXLat(DefaultPalette, Palettes[BLEND10WHITE], 0xFFFFFFFF, 90, 10);              // 0x70
            BlendXLat(DefaultPalette, Palettes[BLEND20WHITE], 0xFFFFFFFF, 80, 20);              // 0x71
            BlendXLat(DefaultPalette, Palettes[BLEND30WHITE], 0xFFFFFFFF, 70, 30);              // 0x72
            BlendXLat(DefaultPalette, Palettes[BLEND40WHITE], 0xFFFFFFFF, 60, 40);              // 0x73
            BlendXLat(DefaultPalette, Palettes[BLEND50WHITE], 0xFFFFFFFF, 50, 50);              // 0x74
            BlendXLat(DefaultPalette, Palettes[BLEND60WHITE], 0xFFFFFFFF, 40, 60);              // 0x75
            BlendXLat(DefaultPalette, Palettes[BLEND70WHITE], 0xFFFFFFFF, 30, 70);              // 0x76
            BlendXLat(DefaultPalette, Palettes[BLEND80WHITE], 0xFFFFFFFF, 20, 80);              // 0x77
            BlendXLat(DefaultPalette, Palettes[BLEND90WHITE], 0xFFFFFFFF, 10, 90);              // 0x78
            ReplaceXLat(Palettes[BLEND100WHITE], 0xFFFFFFFF);                                   // 0x79

            HalfRampXLat(DefaultPalette, 7, Palettes[REDTODGREEN1], 1, 8);                      // 0x7A
            RampXLat(DefaultPalette, 2, Palettes[REDTODGREEN1], 9);                             // 0x7A

            HalfRampXLat(DefaultPalette, 7, Palettes[REDTODGREEN2], 1, 8);                      // 0x7B
            RampXLat(DefaultPalette, 3, Palettes[REDTODGREEN2], 9);                             // 0x7B

            HalfRampXLat(DefaultPalette, 7, Palettes[REDTODGREEN3], 1, 8);                      // 0x7C
            RampXLat(DefaultPalette, 4, Palettes[REDTODGREEN3], 9);                             // 0x7C
            
            HalfRampXLat(DefaultPalette, 13, Palettes[REDTOBLACK1], 1, 8);                      // 0x7D
            RampXLat(DefaultPalette, 2, Palettes[REDTOBLACK1], 9);                              // 0x7D
            
            HalfRampXLat(DefaultPalette, 13, Palettes[REDTOBLACK2], 1, 8);                      // 0x7E
            RampXLat(DefaultPalette, 3, Palettes[REDTOBLACK2], 9);                              // 0x7E
            
            HalfRampXLat(DefaultPalette, 13, Palettes[REDTOBLACK3], 1, 8);                      // 0x7F
            RampXLat(DefaultPalette, 4, Palettes[REDTOBLACK3], 9);                              // 0x7F

#if !VANILLA
            LightXLat(Palettes[REDTODKBLACK1], 1, 0);                                           // 0x80
            RampXLat(DefaultPalette, 2, Palettes[REDTODKBLACK1], 9);                            // 0x80

            LightXLat(Palettes[REDTODKBLACK2], 1, 0);                                           // 0x81
            RampXLat(DefaultPalette, 3, Palettes[REDTODKBLACK2], 9);                            // 0x81

            LightXLat(Palettes[REDTODKBLACK3], 1, 0);                                           // 0x82
            RampXLat(DefaultPalette, 4, Palettes[REDTODKBLACK3], 9);                            // 0x82

            LightXLat(Palettes[REDBLK_BLWHT], 1, 0);                                            // 0x83
            RampXLat(DefaultPalette, 13, Palettes[REDBLK_BLWHT], 9);                            // 0x83

            LightXLat(Palettes[BLBLK_REDWHT], 9, 0);                                            // 0x84
            RampXLat(DefaultPalette, 13, Palettes[BLBLK_REDWHT], 1);                            // 0x84
#endif
            // (3) --- Manipulate the systematical ones (swap red and blue rows)

            for (int i = 0; i < RAMPS.Length; i++)
            {
                for (int j = 0; j < RAMPS.Length; j++)
                {
                    byte val = (byte)(GUILDCOLOR_BASE + i * RAMPS.Length + j);

                    RampXLat(DefaultPalette, RAMPS[i], Palettes[val], 1);    // red
                    RampXLat(DefaultPalette, RAMPS[j], Palettes[val], 9);    // blue
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color1"></param>
        /// <param name="Color2"></param>
        /// <returns></returns>
        public static byte GetGuildShieldColor(byte Color1, byte Color2)
        {
            return (byte)(GUILDCOLOR_BASE + NUMGUILDCOLORS * Color1 + Color2);
        }

        /// <summary>
        /// Returns the name of the palette given by Index
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetNameOfPalette(byte Index)
        {
            if (Index >= GUILDCOLOR_BASE)
            {
                return "GUILDCOLOR" + (Index - GUILDCOLOR_BASE).ToString();
            }
            else
            {
                switch (Index)
                {
                    case IDENTITY:          return "IDENTITY";

                    case DBLUETOSKIN1:      return "DBLUETOSKIN1";
                    case DBLUETOSKIN2:      return "DBLUETOSKIN2";
                    case DBLUETOSKIN3:      return "DBLUETOSKIN3";
                    case DBLUETOSKIN4:      return "DBLUETOSKIN4";
                    case DBLUETOSICKGREEN:  return "DBLUETOSICKGREEN";
                    case DBLUETOSICKYELLOW: return "DBLUETOSICKYELLOW";
                    case DBLUETOGRAY:       return "DBLUETOGRAY";
                    case DBLUETOLBLUE:      return "DBLUETOLBLUE";
                    case DBLUETOASHEN:      return "DBLUETOASHEN";

                    case GRAYTOORANGE:      return "GRAYTOORANGE";
                    case GRAYTODGREEN:      return "GRAYTODGREEN";
                    case GRAYTOBGREEN:      return "GRAYTOBGREEN";
                    case GRAYTOSKY:         return "GRAYTOSKY";
                    case GRAYTODBLUE:       return "GRAYTODBLUE";
                    case GRAYTOPURPLE:      return "GRAYTOPURPLE";
                    case GRAYTOGOLD:        return "GRAYTOGOLD";
                    case GRAYTOBBLUE:       return "GRAYTOBBLUE";
                    case GRAYTORED:         return "GRAYTORED";
                    case GRAYTOLORANGE:     return "GRAYTOLORANGE";
                    case GRAYTOLGREEN:      return "GRAYTOLGREEN";
                    case GRAYTOLBGREEN:     return "GRAYTOLBGREEN";
                    case GRAYTOLSKY:        return "GRAYTOLSKY";
                    case GRAYTOLBLUE:       return "GRAYTOLBLUE";
                    case GRAYTOLPURPLE:     return "GRAYTOLPURPLE";
                    case GRAYTOLGOLD:       return "GRAYTOLGOLD";
                    case GRAYTOSKIN1:       return "GRAYTOSKIN1";
                    case GRAYTOSKIN2:       return "GRAYTOSKIN2";
                    case GRAYTOSKIN3:       return "GRAYTOSKIN3";
                    case GRAYTOSKIN4:       return "GRAYTOSKIN4";
                    case GRAYTOSKIN5:       return "GRAYTOSKIN5";
                    case GRAYTOLBBLUE:      return "GRAYTOLBBLUE";
                    case GRAYTOLRED:        return "GRAYTOLRED";
                    case GRAYTOKORANGE:     return "GRAYTOKORANGE";
                    case GRAYTOKGREEN:      return "GRAYTOKGREEN";
                    case GRAYTOKBGREEN:     return "GRAYTOKBGREEN";
                    case GRAYTOKSKY:        return "GRAYTOKSKY";
                    case GRAYTOKBLUE:       return "GRAYTOKBLUE";
                    case GRAYTOKPURPLE:     return "GRAYTOKPURPLE";
                    case GRAYTOKGOLD:       return "GRAYTOKGOLD";
                    case GRAYTOKBBLUE:      return "GRAYTOKBBLUE";
                    case GRAYTOKRED:        return "GRAYTOKRED";
                    case GRAYTOKGRAY:       return "GRAYTOKGRAY";
                    case GRAYTOBLACK:       return "GRAYTOBLACK";
                    case GRAYTOOLDHAIR1:    return "GRAYTOOLDHAIR1";
                    case GRAYTOOLDHAIR2:    return "GRAYTOOLDHAIR2";
                    case GRAYTOOLDHAIR3:    return "GRAYTOOLDHAIR3";
                    case GRAYTOPLATBLOND:   return "GRAYTOPLATBLOND";

                    case FILTERWHITE90:     return "FILTERWHITE90";
                    case FILTERWHITE80:     return "FILTERWHITE80";
                    case FILTERWHITE70:     return "FILTERWHITE70";
                    case FILTERBRIGHT1:     return "FILTERBRIGHT1";
                    case FILTERBRIGHT2:     return "FILTERBRIGHT2";
                    case FILTERBRIGHT3:     return "FILTERBRIGHT3";

                    case BLEND25YELLOW:     return "BLEND25YELLOW";
                    
                    case PURPLETOLBLUE:     return "PURPLETOLBLUE";
                    case PURPLETOBRED:      return "PURPLETOBRED";
                    case PURPLETOGREEN:     return "PURPLETOGREEN";
                    case PURPLETOYELLOW:    return "PURPLETOYELLOW";

                    case BLEND10RED:        return "BLEND10RED";
                    case BLEND20RED:        return "BLEND20RED";
                    case BLEND30RED:        return "BLEND30RED";
                    case BLEND40RED:        return "BLEND40RED";
                    case BLEND50RED:        return "BLEND50RED";
                    case BLEND60RED:        return "BLEND60RED";
                    case BLEND70RED:        return "BLEND70RED";
                    case BLEND80RED:        return "BLEND80RED";
                    case BLEND90RED:        return "BLEND90RED";
                    case BLEND100RED:       return "BLEND100RED";

                    case FILTERRED:         return "FILTERRED";
                    case FILTERBLUE:        return "FILTERBLUE";
                    case FILTERGREEN:       return "FILTERGREEN";
                    case BLEND25RED:        return "BLEND25RED";
                    case BLEND25BLUE:       return "BLEND25BLUE";
                    case BLEND25GREEN:      return "BLEND25GREEN";
                    case BLEND50BLUE:       return "BLEND50BLUE";
                    case BLEND50GREEN:      return "BLEND50GREEN";
                    case BLEND75RED:        return "BLEND75RED";
                    case BLEND75BLUE:       return "BLEND75BLUE";
                    case BLEND75GREEN:      return "BLEND75GREEN";
#if !VANILLA
                    case REDTOBLACK:        return "REDTOBLACK";
                    case BLUETOBLACK:       return "BLUETOBLACK";
                    case PURPLETOBLACK:     return "PURPLETOBLACK";
#endif
                    case RAMPUP1:           return "RAMPUP1";                  
                    case RAMPUP2:           return "RAMPUP2";                    
                    case RAMPDOWN2:         return "RAMPDOWN2";                 
                    case RAMPDOWN1:         return "RAMPDOWN1";

                    case BLEND10WHITE:      return "BLEND10WHITE";
                    case BLEND20WHITE:      return "BLEND20WHITE";
                    case BLEND30WHITE:      return "BLEND30WHITE";
                    case BLEND40WHITE:      return "BLEND40WHITE";
                    case BLEND50WHITE:      return "BLEND50WHITE";
                    case BLEND60WHITE:      return "BLEND60WHITE";
                    case BLEND70WHITE:      return "BLEND70WHITE";
                    case BLEND80WHITE:      return "BLEND80WHITE";
                    case BLEND90WHITE:      return "BLEND90WHITE";
                    case BLEND100WHITE:     return "BLEND100WHITE";

                    case REDTODGREEN1:      return "REDTODGREEN1";
                    case REDTODGREEN2:      return "REDTODGREEN2";
                    case REDTODGREEN3:      return "REDTODGREEN3";

                    case REDTOBLACK1:       return "REDTOBLACK1";
                    case REDTOBLACK2:       return "REDTOBLACK2";
                    case REDTOBLACK3:       return "REDTOBLACK3";
#if !VANILLA
                    case REDTODKBLACK1:     return "REDTODKBLACK1";
                    case REDTODKBLACK2:     return "REDTODKBLACK2";
                    case REDTODKBLACK3:     return "REDTODKBLACK3";

                    case REDBLK_BLWHT:      return "REDBLK_BLWHT";
                    case BLBLK_REDWHT:      return "BLBLK_REDWHT";
#endif
                    default:                return "UNSET";
                }
            }
        }
    }
}
