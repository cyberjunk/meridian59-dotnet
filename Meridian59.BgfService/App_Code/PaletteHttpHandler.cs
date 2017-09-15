
using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using Meridian59.Files.BGF;
using Meridian59.Drawing2D;

namespace Meridian59.BgfService
{
    /// <summary>
    /// 
    /// </summary>
    public class PaletteRouteHandler : IRouteHandler
    {
        public PaletteRouteHandler()
        {
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new PaletteHttpHandler();
        }
    }

    /// <summary>
    /// Returns the M59 color palette in different variants.
    /// </summary>
    public class PaletteHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Handles the HTTP request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            // --------------------------------------------------------------------------------------------
            // 1) PARSE URL PARAMETERS
            // --------------------------------------------------------------------------------------------

            // read parameters from url-path (see Global.asax):
            RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
            string parmNum    = parms.ContainsKey("num")    ? (string)parms["num"]    : null;
            string parmFormat = parms.ContainsKey("format") ? (string)parms["format"] : null;

            byte index;

            // no filename or request type
            if (!Byte.TryParse(parmNum, out index) || String.IsNullOrEmpty(parmFormat))
            {
                context.Response.StatusCode = 404;
                return;
            }

            // set cache behaviour
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["*"] = false;
            context.Response.Cache.SetLastModified(BgfCache.LastModified);

            // --------------------------------------------------------------------------------------------
            // PALETTE IMAGE
            // --------------------------------------------------------------------------------------------
            if (parmFormat == "bmp")
            {
                response.ContentType = "image/bmp";
                response.AddHeader(
                    "Content-Disposition",
                    "inline; filename=palette-" + index.ToString() + ".bmp");

                Bitmap bmp = PalettesGDI.GetPaletteBitmap(PalettesGDI.Palettes[index]);
                bmp.Save(context.Response.OutputStream, ImageFormat.Bmp);
                bmp.Dispose();

                // create BMP (256 col) or PNG (32-bit) or return raw pixels (8bit indices)
                /*if (parm1 == "bmp")
                {
                    response.ContentType = "image/bmp";
                    response.AddHeader(
                        "Content-Disposition",
                        "inline; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".bmp");

                    Bitmap bmp = PalettesGDI.GetPaletteBitmap(PalettesGDI.Palettes[index]);
                    bmp.Save(context.Response.OutputStream, ImageFormat.Bmp);
                    bmp.Dispose();
                }
                else if (parm1 == "png")
                {
                    response.ContentType = "image/png";
                    response.AddHeader(
                        "Content-Disposition",
                        "inline; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".png");

                    Bitmap bmp = entry.Bgf.Frames[index].GetBitmapA8R8G8B8(palette);
                    bmp.Save(context.Response.OutputStream, ImageFormat.Png);
                    bmp.Dispose();
                }
                else if (parm1 == "bin")
                {
                    response.ContentType = "application/octet-stream";
                    response.AddHeader(
                        "Content-Disposition", 
                        "attachment; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".bin");

                    byte[] pixels = entry.Bgf.Frames[index].PixelData;
                    context.Response.OutputStream.Write(pixels, 0, pixels.Length);
                }
                else
                    context.Response.StatusCode = 404;*/
            }

            // --------------------------------------------------------------------------------------------
            // JSON COLOR PALETTE
            // --------------------------------------------------------------------------------------------
            else if (parmFormat == "json")
            {
                // set response type
                response.ContentType = "application/json";
                response.ContentEncoding = new System.Text.UTF8Encoding(false);
                response.AddHeader("Content-Disposition", "inline; filename=palette-" + index.ToString() + ".json");

                /////////////////////////////////////////////////////////////
                response.Write('[');
                for (int i = 0; i < ColorTransformation.Palettes[index].Length; i++)
                {
                    if (i > 0)
                        response.Write(',');

                    response.Write(ColorTransformation.Palettes[index][i].ToString());
                }
                response.Write(']');
            }

            // --------------------------------------------------------------------------------------------
            // INVALID
            // --------------------------------------------------------------------------------------------
            else
            {
                context.Response.StatusCode = 404;
                return;
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
