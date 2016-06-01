<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Concurrent;

using Meridian59.Files.BGF;
using Meridian59.Drawing2D;

/// <summary>
/// Returns a single frame from a BGF specified by a bgf-name, frame-idx and palette-idx.
/// The returned image is 32-Bit PNG encoded with correct transparency for the Cyan pixels.
/// </summary>
public class Handler : IHttpHandler
{
    public void ProcessRequest (HttpContext context) 
    {
        BgfFile bgfFile;

        // -------------------------------------------------------       
        // read parameters from url-path: {file}/{frame}/{palette}
        
        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
           
        string parmFile     = parms.ContainsKey("file")     ? (string)parms["file"] : null;
        string parmFrameIdx = parms.ContainsKey("frame")    ? (string)parms["frame"] : null;
        string parmPalette  = parms.ContainsKey("palette")  ? (string)parms["palette"] : null;

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
            string filePath = context.Server.MapPath("~") + "bgf/" + parmFile + ".bgf";
            if (!File.Exists(filePath))
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }
            else
            {
                try
                {
                    // read from disk
                    bgfFile = new BgfFile(filePath);
                    
                    // must decompress all due to multithreading (important!)
                    bgfFile.DecompressAll();
                    
                    // add to cache
                    Cache.AddBGF(parmFile, bgfFile);
                }
                catch(Exception) { }
            }
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
        // try to parse additional frame and palette param
        
        int frameidx = 0;
        byte paletteidx = 0;

        Int32.TryParse(parmFrameIdx, out frameidx);
        Byte.TryParse(parmPalette, out paletteidx);
        
        // --------------------------------------------------
        // requested frameindex out of range
        
        if (frameidx < 0 || frameidx >= bgfFile.Frames.Count)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // get the frmae as A8R8G8B8 bitmap
        
        BgfBitmap bgfBmp = bgfFile.Frames[frameidx];
        Bitmap bmp = bgfBmp.GetBitmapA8R8G8B8(paletteidx);

        // --------------------------------------------------
        // write the response (encode to png)
        
        context.Response.ContentType = "image/png";
        bmp.Save(context.Response.OutputStream, ImageFormat.Png);       
        context.Response.Flush();
        context.Response.End();
        bmp.Dispose();                
    }
 
    public bool IsReusable 
    {
        get 
        {
            return true;
        }
    }
}
