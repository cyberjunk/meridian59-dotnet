<%@ WebHandler Language="C#" Class="Frame" %>

using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;

using Meridian59.Files.BGF;
using Meridian59.Common.Constants;

/// <summary>
/// Returns a single frame from a BGF specified by a bgf-name, frame-idx and palette-idx.
/// The returned image is 32-Bit PNG encoded with correct transparency for the Cyan pixels.
/// </summary>
public class Frame : IHttpHandler
{
    private static readonly TimeSpan freshness = new TimeSpan(0, 0, 0, 120);

    public void ProcessRequest (HttpContext context) 
    {
        BgfCache.Entry entry;

        // -------------------------------------------------------       
        // read parameters from url-path (see Global.asax):
        //  frame/{format}/{file}/{group}/{angle}/{palette}
        //  group, angle and palette are optional
        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;

        string parmFormat   = parms.ContainsKey("format")   ? (string)parms["format"] : null;
        string parmFile     = parms.ContainsKey("file")     ? (string)parms["file"] : null;
        string parmGroup    = parms.ContainsKey("group")    ? (string)parms["group"] : null;
        string parmPalette  = parms.ContainsKey("palette")  ? (string)parms["palette"] : null;
        string parmAngle    = parms.ContainsKey("angle")    ? (string)parms["angle"] : null;
        
        // -------------------------------------------------------
        // no format or no filename
        if (String.IsNullOrEmpty(parmFormat) ||
            String.IsNullOrEmpty(parmFile))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }
        // --------------------------------------------------
        // unknown bgf
        if (!BgfCache.GetBGF(parmFile, out entry))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }
        // --------------------------------------------------
        // try to parse additional params
        int group = 0;
        byte paletteidx = 0;
        ushort angle = 0;
        
        Int32.TryParse(parmGroup, out group);
        Byte.TryParse(parmPalette, out paletteidx);
        UInt16.TryParse(parmAngle, out angle);
        
        // remove full periods from angle
        angle %= GeometryConstants.MAXANGLE;

        // map 0 and negative groups to first group
        // group is 1-based
        if (group < 1)
            group = 1;
        // --------------------------------------------------
        // requested group out of range
        if (group > entry.Bgf.FrameSets.Count)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }
        // --------------------------------------------------
        // try get the frame
        BgfBitmap bgfBmp = entry.Bgf.GetFrame(group, angle);
        if (bgfBmp == null)
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

        // --------------------------------------------------
        // create the A8R8G8B8 bitmap
        Bitmap bmp;
        switch(parmFormat)
        {
            case "bmp":
                context.Response.ContentType = "image/bmp";
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + entry.Bgf.Filename + ".bmp");
                bmp = bgfBmp.GetBitmap(paletteidx);
                bmp.Save(context.Response.OutputStream, ImageFormat.Bmp);
                context.Response.Flush();
                context.Response.End();
                bmp.Dispose(); 
                break;
                
            case "png":
                context.Response.ContentType = "image/png";
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + entry.Bgf.Filename + ".png");
                bmp = bgfBmp.GetBitmapA8R8G8B8(paletteidx);
                bmp.Save(context.Response.OutputStream, ImageFormat.Png);
                context.Response.Flush();
                context.Response.End();
                bmp.Dispose(); 
                break;
            
            // invalid format
            default:
                context.Response.StatusCode = 404;
                context.Response.End();
                break;
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
