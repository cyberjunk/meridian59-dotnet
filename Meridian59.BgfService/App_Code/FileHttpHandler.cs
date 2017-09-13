
using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using Meridian59.Files.BGF;

namespace Meridian59.BgfService
{
    /// <summary>
    /// 
    /// </summary>
    public class FileRouteHandler : IRouteHandler
    {
        public FileRouteHandler()
        {
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new FileHttpHandler();
        }
    }

    /// <summary>
    /// Provides contents from BGF file. This includes meta data
    /// like offsets and hotspots as well as frame images as PNG or BMP.
    /// </summary>
    public class FileHttpHandler : IHttpHandler
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
            string parmFile = parms.ContainsKey("file") ? (string)parms["file"] : null;
            string parmReq = parms.ContainsKey("req") ? (string)parms["req"] : null;
            string parm1 = parms.ContainsKey("parm1") ? (string)parms["parm1"] : null;
            string parm2 = parms.ContainsKey("parm2") ? (string)parms["parm2"] : null;
            string parm3 = parms.ContainsKey("parm3") ? (string)parms["parm3"] : null;

            BgfCache.Entry entry;

            // no filename or request type
            if (String.IsNullOrEmpty(parmFile) || !BgfCache.GetBGF(parmFile, out entry) ||
                String.IsNullOrEmpty(parmReq))
            {
                context.Response.StatusCode = 404;
                return;
            }

            // set cache behaviour
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["*"] = false;
            context.Response.Cache.SetLastModified(entry.LastModified);

            // --------------------------------------------------------------------------------------------
            // FRAME IMAGE
            // --------------------------------------------------------------------------------------------
            if (parmReq == "frame")
            {
                ushort index;
                byte palette = 0;
                Byte.TryParse(parm3, out palette);

                // try to parse index and palette and validate range
                if (!UInt16.TryParse(parm2, out index) || index >= entry.Bgf.Frames.Count)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                // create BMP (256 col) or PNG (32-bit) or return raw pixels (8bit indices)
                if (parm1 == "bmp")
                {
                    response.ContentType = "image/bmp";
                    response.AddHeader(
                        "Content-Disposition",
                        "inline; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".bmp");

                    Bitmap bmp = entry.Bgf.Frames[index].GetBitmap(palette);
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
                    context.Response.StatusCode = 404;
            }

            // --------------------------------------------------------------------------------------------
            // JSON META DATA
            // --------------------------------------------------------------------------------------------
            else if (parmReq == "meta")
            {
                // set response type
                response.ContentType = "application/json";
                response.ContentEncoding = new System.Text.UTF8Encoding(false);
                response.AddHeader("Content-Disposition", "inline; filename=" + entry.Bgf.Filename + ".json");

                // unix timestamp
                long stamp = (entry.LastModified.Ticks - 621355968000000000) / 10000000;

                /////////////////////////////////////////////////////////////
                response.Write("{\"file\":\"");
                response.Write(entry.Bgf.Filename);
                response.Write("\",\"size\":");
                response.Write(entry.Size.ToString());
                response.Write(",\"modified\":");
                response.Write(stamp.ToString());
                response.Write(",\"shrink\":");
                response.Write(entry.Bgf.ShrinkFactor.ToString());
                response.Write(",\"frames\":[");
                for (int i = 0; i < entry.Bgf.Frames.Count; i++)
                {
                    BgfBitmap frame = entry.Bgf.Frames[i];

                    if (i > 0)
                        response.Write(',');

                    response.Write("{\"w\":");
                    response.Write(frame.Width.ToString());
                    response.Write(",\"h\":");
                    response.Write(frame.Height.ToString());
                    response.Write(",\"x\":");
                    response.Write(frame.XOffset.ToString());
                    response.Write(",\"y\":");
                    response.Write(frame.YOffset.ToString());
                    response.Write(",\"hs\":[");
                    for (int j = 0; j < frame.HotSpots.Count; j++)
                    {
                        BgfBitmapHotspot hs = frame.HotSpots[j];

                        if (j > 0)
                            response.Write(',');

                        response.Write("{\"i\":");
                        response.Write(hs.Index.ToString());
                        response.Write(",\"x\":");
                        response.Write(hs.X.ToString());
                        response.Write(",\"y\":");
                        response.Write(hs.Y.ToString());
                        response.Write('}');
                    }
                    response.Write("]}");
                }
                response.Write("],\"groups\":[");
                for (int i = 0; i < entry.Bgf.FrameSets.Count; i++)
                {
                    BgfFrameSet group = entry.Bgf.FrameSets[i];

                    if (i > 0)
                        response.Write(',');

                    response.Write('[');
                    for (int j = 0; j < group.FrameIndices.Count; j++)
                    {
                        if (j > 0)
                            response.Write(',');

                        response.Write(group.FrameIndices[j].ToString());
                    }
                    response.Write(']');
                }
                response.Write("]}");
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
