using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WuQuantWrap
{
    public static class WuQuant
    {

#if __MonoCS__
        const string LIBNAME = @"libwuquant.so";
#else
        const string LIBNAME = @"WuQuant.dll";
#endif

        /// <summary>
        /// Creates a native WuQuantizer instance
        /// </summary>
        /// <returns></returns>
        [DllImport(LIBNAME, CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr Create();

        /// <summary>
        /// Destroys a native WuQuantizer instance
        /// </summary>
        /// <param name="quantizer"></param>
        [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Destroy(IntPtr quantizer);

        /// <summary>
        /// Quantizes an image using a native WuQuantizer instance
        /// </summary>
        /// <param name="quantizer"></param>
        /// <param name="image"></param>
        /// <param name="palette"></param>
        /// <param name="colorCount"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="destPixels"></param>
        /// <param name="padMultiple4"></param>
        /// <returns></returns>
        [DllImport(LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int Quantize(IntPtr quantizer, uint* image, uint* palette, int* colorCount, int width, int height, byte* destPixels, int padMultiple4);

        /// <summary>
        /// Quantizes an image using a native WuQuantizer instance
        /// </summary>
        /// <param name="quantizer"></param>
        /// <param name="image"></param>
        /// <param name="colorCount"></param>
        /// <param name="destPixels"></param>
        /// <param name="padMultiple4"></param>
        /// <returns></returns>
        public static unsafe uint[] Quantize(IntPtr quantizer, Bitmap image, ref int colorCount, byte[] destPixels, int padMultiple4)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            if (destPixels == null)
                throw new ArgumentNullException("destPixels");

            if (colorCount < 1 || colorCount > 256)
                throw new ArgumentOutOfRangeException("colorCount");

            BitmapData imgdata = image.LockBits(
                Rectangle.FromLTRB(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                image.PixelFormat);

            uint[] pal = new uint[colorCount];
            fixed (byte* ptrOutput = destPixels)
            {
                fixed (uint* ptrPal = pal)
                {
                    fixed (int* ptrColors = &colorCount)
                    {
                        Quantize(
                           quantizer,
                           (uint*)imgdata.Scan0.ToPointer(),
                           ptrPal,
                           ptrColors,
                           image.Width,
                           image.Height,
                           ptrOutput,
                           padMultiple4);
                    }
                }
            }

            image.UnlockBits(imgdata);

            return pal;
        }
    }
}
