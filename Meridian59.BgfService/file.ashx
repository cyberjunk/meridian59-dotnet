<%@ WebHandler Language="C#" Class="File" %>

using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using Meridian59.Files.BGF;
using System.IO;

/// <summary>
/// Provides contents from BGF file. This includes meta data
/// like offsets and hotspots as well as frame images as PNG or BMP.
/// </summary>
public class File : IHttpHandler
{
    private static readonly TimeSpan freshness = new TimeSpan(0, 0, 0, 300);
    
    /// <summary>
    /// Handles the HTTP request
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest (HttpContext context)
    {
        BgfCache.Entry entry;
        // -------------------------------------------------------
        // read parameters from url-path (see Global.asax):
        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
        string parmFile = parms.ContainsKey("file")  ? (string)parms["file"]  : null;
        string parmReq  = parms.ContainsKey("req")   ? (string)parms["req"]   : null;
        string parm1    = parms.ContainsKey("parm1") ? (string)parms["parm1"] : null;
        string parm2    = parms.ContainsKey("parm2") ? (string)parms["parm2"] : null;
        string parm3    = parms.ContainsKey("parm3") ? (string)parms["parm3"] : null;
        // -------------------------------------------------------
        // no filename or request type
        if (String.IsNullOrEmpty(parmFile) ||
            String.IsNullOrEmpty(parmReq))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
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
            context.Response.End();
            return;
        }
        // -------------------------------------------------------
        // set cache behaviour
        context.Response.Cache.SetExpires(DateTime.UtcNow.Add(freshness));
        context.Response.Cache.SetMaxAge(freshness);
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.SetValidUntilExpires(true);
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
            if (parm1 != "png" && parm1 != "bmp")
            {
                context.Response.StatusCode = 404;
                context.Response.End();
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
                context.Response.End();
                return;
            }
            // --------------------------------------------------
            // create BMP or PNG
            if (parm1 == "bmp")
            {
                context.Response.ContentType = "image/bmp";
                context.Response.AddHeader(
                    "Content-Disposition",
                    "inline; filename=" + entry.Bgf.Filename + ".bmp");

                Bitmap bmp = entry.Bgf.Frames[index].GetBitmap(palette);
                bmp.Save(context.Response.OutputStream, ImageFormat.Bmp);
                bmp.Dispose();
            }
            else if (parm1 == "png")
            {
                context.Response.ContentType = "image/png";
                context.Response.AddHeader(
                    "Content-Disposition",
                    "inline; filename=" + entry.Bgf.Filename + ".png");

                Bitmap bmp = entry.Bgf.Frames[index].GetBitmapA8R8G8B8(palette);
                bmp.Save(context.Response.OutputStream, ImageFormat.Png);
                bmp.Dispose();
            }
            else
                context.Response.StatusCode = 404;

            context.Response.Flush();
            context.Response.End();
        }
        // -------------------------------------------------------
        // JSON META DATA
        else if (parmReq == "meta")
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.AddHeader(
                "Content-Disposition",
                "inline; filename=" + entry.Bgf.Filename + ".json");

            StreamWriter writer = new StreamWriter(
                context.Response.OutputStream,
                System.Text.Encoding.UTF8, 4096, true);
            /////////////////////////////////////////////////////////////
            writer.Write("{\"shrink\":"+ entry.Bgf.ShrinkFactor + ',');
            /////////////////////////////////////////////////////////////
            writer.Write("\"frames\":[");
            for (int i = 0; i < entry.Bgf.Frames.Count; i++)
            {
                BgfBitmap frame = entry.Bgf.Frames[i];
                writer.Write("{" +
                    "\"w\":" + frame.Width   + ',' +
                    "\"h\":" + frame.Height  + ',' +
                    "\"x\":" + frame.XOffset + ',' +
                    "\"y\":" + frame.YOffset + ",");
                writer.Write("\"hs\":[");
                for (int j = 0; j < frame.HotSpots.Count; j++)
                {
                    BgfBitmapHotspot hs = frame.HotSpots[j];
                    writer.Write("{" +
                        "\"i\":" + hs.Index + ',' +
                        "\"x\":" + hs.X     + ',' +
                        "\"y\":" + hs.Y     + '}');
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
            writer.Write(']');
            /////////////////////////////////////////////////////////////
            writer.Write("}");
            writer.Close();
            writer.Dispose();
            context.Response.Flush();
            context.Response.End();
        }
        // -------------------------------------------------------
        // INVALID
        else
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
