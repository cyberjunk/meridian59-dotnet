<%@ WebHandler Language="C#" Class="BlakObj" %>

using System;
using System.Web;
using System.Web.Routing;

using System.Drawing;
using System.Drawing.Imaging;

using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;

public class BlakObj : IHttpHandler
{
    private const ushort MINSCALE = 10;
    private const ushort MAXSCALE = 80;
    private readonly ImageComposerGDI<ObjectBase> imageComposer = new ImageComposerGDI<ObjectBase>();

    public BlakObj()
    {
        // don't use quality, apply custom scale
        imageComposer.Quality = 16.0f; 
        imageComposer.IsCustomShrink = true;
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

    public void ProcessRequest (HttpContext context)
    {
        // -------------------------------------------------------       
        // read basic and mainoverlay parameters from url-path (see Global.asax):
        //  object/{scale}/{file}/{group}/{palette}/{angle}
        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
        string parmScale = parms.ContainsKey("scale") ? (string)parms["scale"] : null;
        string parmFile = parms.ContainsKey("file") ? (string)parms["file"] : null;
        string parmGroup = parms.ContainsKey("group") ? (string)parms["group"] : null;
        string parmPalette = parms.ContainsKey("palette") ? (string)parms["palette"] : null;
        string parmAngle = parms.ContainsKey("angle") ? (string)parms["angle"] : null;

        // -------------------------------------------------------
        // verify minimum parameters exist
        if (String.IsNullOrEmpty(parmScale) ||
            String.IsNullOrEmpty(parmFile))
        {
            context.Response.StatusCode = 404;
            return;
        }

        // convert to lowercase
        parmFile = parmFile.ToLower();

        ushort scale;
        UInt16.TryParse(parmScale, out scale);
        scale = MathUtil.Bound(scale, MINSCALE, MAXSCALE);

        // --------------------------------------------------
        // try to get the main BGF from cache or load from disk
        BgfCache.Entry entry;
        if (!BgfCache.GetBGF(parmFile, out entry))
        {
            context.Response.StatusCode = 404;
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
                string subOvFile = subOvParms[0].ToLower();
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
        imageComposer.CustomShrink = (float)scale * 0.1f;
        imageComposer.DataSource = gameObject;

        if (imageComposer.Image == null)
        {
            context.Response.StatusCode = 404;
            return;
        }

        // -------------------------------------------------------
        // set cache behaviour
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.Cache.VaryByParams["*"] = false;
        context.Response.Cache.SetLastModified(lastModified);
        // --------------------------------------------------
        // write the response (encode to png)
        context.Response.ContentType = "image/png";
        context.Response.AddHeader("Content-Disposition", "inline; filename=object.png");
        imageComposer.Image.Save(context.Response.OutputStream, ImageFormat.Png);
        imageComposer.Image.Dispose();
    }
}
