
using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using Meridian59.Files.BGF;
using System.IO;

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
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Handles the HTTP request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            BgfCache.Entry entry;
            // -------------------------------------------------------
            // read parameters from url-path (see Global.asax):
            RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
            string parmFile = parms.ContainsKey("file") ? (string)parms["file"] : null;
            string parmReq = parms.ContainsKey("req") ? (string)parms["req"] : null;
            string parm1 = parms.ContainsKey("parm1") ? (string)parms["parm1"] : null;
            string parm2 = parms.ContainsKey("parm2") ? (string)parms["parm2"] : null;
            string parm3 = parms.ContainsKey("parm3") ? (string)parms["parm3"] : null;
            // -------------------------------------------------------
            // no filename or request type
            if (String.IsNullOrEmpty(parmFile) ||
                String.IsNullOrEmpty(parmReq))
            {
                context.Response.StatusCode = 404;
                return;
            }
            // convert to lowercase
            parmFile = parmFile.ToLower();
            parmReq = parmReq.ToLower();
            // --------------------------------------------------
            // unknown bgf
            if (!BgfCache.GetBGF(parmFile, out entry))
            {
                context.Response.StatusCode = 404;
                return;
            }
            // -------------------------------------------------------
            // set cache behaviour
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["*"] = false;
            context.Response.Cache.SetLastModified(entry.LastModified);
            // -------------------------------------------------------
            // -------------------------------------------------------
            // FRAME IMAGE
            if (parmReq == "frame")
            {
                if (parm1 != null)
                    parm1 = parm1.ToLower();

                // invalid format
                if (parm1 != "png" && parm1 != "bmp" && parm1 != "bin")
                {
                    context.Response.StatusCode = 404;
                    return;
                }
                ushort index = 0;
                byte palette = 0;
                UInt16.TryParse(parm2, out index);
                Byte.TryParse(parm3, out palette);
                // --------------------------------------------------
                // requested frame index out of range
                if (index >= entry.Bgf.Frames.Count)
                {
                    context.Response.StatusCode = 404;
                    return;
                }
                // --------------------------------------------------
                // create BMP (256 col) or PNG (32-bit) or return raw pixels (8bit indices)
                if (parm1 == "bmp")
                {
                    context.Response.ContentType = "image/bmp";
                    context.Response.AddHeader(
                        "Content-Disposition",
                        "inline; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".bmp");

                    Bitmap bmp = entry.Bgf.Frames[index].GetBitmap(palette);
                    bmp.Save(context.Response.OutputStream, ImageFormat.Bmp);
                    bmp.Dispose();
                }
                else if (parm1 == "png")
                {
                    context.Response.ContentType = "image/png";
                    context.Response.AddHeader(
                        "Content-Disposition",
                        "inline; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".png");

                    Bitmap bmp = entry.Bgf.Frames[index].GetBitmapA8R8G8B8(palette);
                    bmp.Save(context.Response.OutputStream, ImageFormat.Png);
                    bmp.Dispose();
                }
                else if (parm1 == "bin")
                {
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader(
                        "Content-Disposition",
                        "attachment; filename=" + entry.Bgf.Filename + "-" + index.ToString() + ".bin");

                    byte[] pixels = entry.Bgf.Frames[index].PixelData;
                    context.Response.OutputStream.Write(pixels, 0, pixels.Length);
                }
                else
                    context.Response.StatusCode = 404;
            }
            // -------------------------------------------------------
            // JSON META DATA
            else if (parmReq == "meta")
            {
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = new System.Text.UTF8Encoding(false);
                context.Response.AddHeader(
                    "Content-Disposition",
                    "inline; filename=" + entry.Bgf.Filename + ".json");

                StreamWriter writer = new StreamWriter(
                    context.Response.OutputStream,
                    new System.Text.UTF8Encoding(false), 4096, true);

                /////////////////////////////////////////////////////////////
                writer.Write("{\"shrink\":" + entry.Bgf.ShrinkFactor + ',');
                /////////////////////////////////////////////////////////////
                writer.Write("\"frames\":[");
                for (int i = 0; i < entry.Bgf.Frames.Count; i++)
                {
                    BgfBitmap frame = entry.Bgf.Frames[i];
                    writer.Write("{" +
                        "\"w\":" + frame.Width + ',' +
                        "\"h\":" + frame.Height + ',' +
                        "\"x\":" + frame.XOffset + ',' +
                        "\"y\":" + frame.YOffset + ",");
                    writer.Write("\"hs\":[");
                    for (int j = 0; j < frame.HotSpots.Count; j++)
                    {
                        BgfBitmapHotspot hs = frame.HotSpots[j];
                        writer.Write("{" +
                            "\"i\":" + hs.Index + ',' +
                            "\"x\":" + hs.X + ',' +
                            "\"y\":" + hs.Y + '}');
                        if (j < frame.HotSpots.Count - 1)
                            writer.Write(',');
                    }
                    writer.Write("]}");
                    if (i < entry.Bgf.Frames.Count - 1)
                        writer.Write(',');
                }
                writer.Write("],");
                /////////////////////////////////////////////////////////////
                writer.Write("\"groups\":[");
                for (int i = 0; i < entry.Bgf.FrameSets.Count; i++)
                {
                    BgfFrameSet group = entry.Bgf.FrameSets[i];
                    writer.Write('[');
                    for (int j = 0; j < group.FrameIndices.Count; j++)
                    {
                        writer.Write(group.FrameIndices[j].ToString());
                        if (j < group.FrameIndices.Count - 1)
                            writer.Write(',');
                    }
                    writer.Write(']');
                    if (i < entry.Bgf.FrameSets.Count - 1)
                        writer.Write(',');
                }
                writer.Write("]}");
                /////////////////////////////////////////////////////////////
                writer.Close();
                writer.Dispose();
            }
            // -------------------------------------------------------
            // INVALID
            else
            {
                context.Response.StatusCode = 404;
                return;
            }
        }
    }
}
