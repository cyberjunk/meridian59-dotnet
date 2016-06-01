<%@ WebHandler Language="C#" Class="Frame" %>

using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Concurrent;

using Meridian59.Files.BGF;
using Meridian59.Drawing2D;
using Meridian59.Common.Constants;

/// <summary>
/// Returns a single frame from a BGF specified by a bgf-name, frame-idx and palette-idx.
/// The returned image is 32-Bit PNG encoded with correct transparency for the Cyan pixels.
/// </summary>
public class Frame : IHttpHandler
{
    public void ProcessRequest (HttpContext context) 
    {
        BgfFile bgfFile;

        // -------------------------------------------------------       
        // read parameters from url-path (see Global.asax):
        //  frame/{format}/{file}/{group}/{angle}/{palette}
        //  group, angle and palette are optional
        
        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;

        string parmFormat   = parms.ContainsKey("format")   ? (string)parms["format"] : null;
        string parmFile     = parms.ContainsKey("file")     ? (string)parms["file"] : null;
        string parmGroup    = parms.ContainsKey("group")    ? (string)parms["group"] : null;
        string parmAngle    = parms.ContainsKey("angle")    ? (string)parms["angle"] : null;
        string parmPalette  = parms.ContainsKey("palette")  ? (string)parms["palette"] : null;

        // -------------------------------------------------------
        // no format provided or empty

        if (String.IsNullOrEmpty(parmFormat))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }
              
        // -------------------------------------------------------
        // no filename provided or empty
        
        if (String.IsNullOrEmpty(parmFile))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // try to get the BGF from cache or load from disk
        
        if (!Cache.GetBGF(parmFile, out bgfFile))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;           
        }

        // --------------------------------------------------
        // no bgf file found
        
        if (bgfFile == null)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // try to parse additional params
        
        int group = 0;
        ushort angle = 0;
        byte paletteidx = 0;
        
        Int32.TryParse(parmGroup, out group);
        UInt16.TryParse(parmAngle, out angle);
        Byte.TryParse(parmPalette, out paletteidx);

        // remove full periods from angle
        angle %= GeometryConstants.MAXANGLE;

        // map 0 and negative groups to first group
        // group is 1-based ...
        if (group < 1)
            group = 1;
        
        // --------------------------------------------------
        // requested group out of range
        
        if (group > bgfFile.FrameSets.Count)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // try get the frame

        BgfBitmap bgfBmp = bgfFile.GetFrame(group, angle);
        if (bgfBmp == null)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // create the A8R8G8B8 bitmap

        Bitmap bmp;
        switch(parmFormat)
        {
            case "bmp":
                context.Response.ContentType = "image/bmp";

                bmp = bgfBmp.GetBitmap(paletteidx);
                bmp.Save(context.Response.OutputStream, ImageFormat.Bmp);
                
                context.Response.Flush();
                context.Response.End();
                bmp.Dispose(); 
                break;
                
            case "png":
                context.Response.ContentType = "image/png";

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
