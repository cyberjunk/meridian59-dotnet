<%@ WebHandler Language="C#" Class="Object" %>

using System;
using System.Web;
using System.Collections.Specialized;

using System.Drawing;
using System.Drawing.Imaging;

using Meridian59.Files.BGF;

public class Object : IHttpHandler 
{
    
    public void ProcessRequest (HttpContext context)
    {
        BgfFile bgfFile;

        // -------------------------------------------------------       
        // read parameters from url-path: {file}/{frame}/{palette}

        NameValueCollection parms = context.Request.Params;

        string parmMain = parms["main"];
        //string parmFrameIdx = parms.ContainsKey("frame") ? (string)parms["frame"] : null;
        //string parmPalette = parms.ContainsKey("palette") ? (string)parms["palette"] : null;

        // --------------------------------------------------
        // write the response (encode to png)

        context.Response.ContentType = "image/png";
        //bmp.Save(context.Response.OutputStream, ImageFormat.Png);
        context.Response.Flush();
        context.Response.End();
        //bmp.Dispose();
    }
 
    public bool IsReusable 
    {
        get 
        {
            return false;
        }
    }

}