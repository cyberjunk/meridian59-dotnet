<%@ WebHandler Language="C#" Class="BlakObj" %>

using System;
using System.Web;
using System.Collections.Specialized;
using System.Web.Routing;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;
using Meridian59.Files.BGF;

public class BlakObj : IHttpHandler
{
    static BlakObj()
    {
    }

    private static readonly TimeSpan freshness = new TimeSpan(0, 0, 0, 300);

    public void ProcessRequest (HttpContext context)
    {
        BgfCache.Entry entry;

        // -------------------------------------------------------       
        // read basic and mainoverlay parameters from url-path (see Global.asax):
        //  object/{file}/{group}/{palette}/{angle}

        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;

        string parmFile = parms.ContainsKey("file") ? (string)parms["file"] : null;
        string parmGroup = parms.ContainsKey("group") ? (string)parms["group"] : null;
        string parmPalette = parms.ContainsKey("palette") ? (string)parms["palette"] : null;
        string parmAngle = parms.ContainsKey("angle") ? (string)parms["angle"] : null;

        // -------------------------------------------------------
        // verify minimum parameters exist

        if (String.IsNullOrEmpty(parmFile))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // try to get the main BGF from cache or load from disk

        if (!BgfCache.GetBGF(parmFile, out entry))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // stores the latest lastmodified of main and all subov
        DateTime lastModified = entry.LastModified;

        // --------------------------------------------------
        // try to parse other params

        byte paletteidx = 0;
        ushort angle = 0;

        Byte.TryParse(parmPalette, out paletteidx);
        UInt16.TryParse(parmAngle, out angle);

        // remove full periods from angle
        angle %= GeometryConstants.MAXANGLE;

        // parse animation
        Animation anim = Animation.ExtractAnimation(parmGroup, '-');
        if (anim == null)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // create gameobject

        ObjectBase gameObject = new ObjectBase();
        gameObject.Resource = entry.Bgf;
        gameObject.ColorTranslation = paletteidx;
        gameObject.Animation = anim;
        gameObject.ViewerAngle = angle;

        // -------------------------------------------------------       
        // read suboverlay array params from query parameters:
        //  object/..../?subov={file};{group};{palette};{hotspot}&subov=...

        string[] parmSubOverlays = context.Request.Params.GetValues("subov");

        if (parmSubOverlays != null)
        {
            foreach(string s in parmSubOverlays)
            {
                string[] subOvParms = s.Split(';');

                if (subOvParms == null || subOvParms.Length < 4)
                    continue;

                BgfCache.Entry bgfSubOv;
                string subOvFile = subOvParms[0];
                if (!BgfCache.GetBGF(subOvFile, out bgfSubOv))
                    continue;

                byte subOvPalette;
                byte subOvHotspot;

                if (String.IsNullOrEmpty(subOvParms[1]) ||
                    !byte.TryParse(subOvParms[2], out subOvPalette) ||
                    !byte.TryParse(subOvParms[3], out subOvHotspot))
                {
                    continue;
                }

                Animation subOvAnim = Animation.ExtractAnimation(subOvParms[1], '-');

                if (subOvAnim == null)
                    continue;

                // create suboverlay
                SubOverlay subOv = new SubOverlay(0, subOvAnim, subOvHotspot, subOvPalette, 0);

                // set bgf resource
                subOv.Resource = bgfSubOv.Bgf;

                // update lastModified if subov is newer
                if (bgfSubOv.LastModified > lastModified)
                    lastModified = bgfSubOv.LastModified;

                // add to gameobject's suboverlays
                gameObject.SubOverlays.Add(subOv);
            }
        }

        // tick object
        gameObject.Tick(0, 1);

        // --------------------------------------------------
        // create composed image
        ImageComposerNative<ObjectBase> imageComposer = new ImageComposerNative<ObjectBase>();
        imageComposer.DataSource = gameObject;

        if (imageComposer.Image == null)
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
        context.Response.Cache.SetLastModified(lastModified);

        // --------------------------------------------------
        // write the response (encode to png)
        context.Response.ContentType = "image/png";
        context.Response.AddHeader("Content-Disposition", "inline; filename=object.png");
        imageComposer.Image.Save(context.Response.OutputStream, ImageFormat.Png);
        context.Response.Flush();
        context.Response.End();

        imageComposer.Image.Dispose();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}