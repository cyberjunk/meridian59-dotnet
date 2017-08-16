<%@ WebHandler Language="C#" Class="Render" %>

using System;
using System.Web;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.Routing;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;
using Meridian59.Files.BGF;

public class Render : IHttpHandler
{
    private const ushort MINWIDTH = 16;
    private const ushort MAXWIDTH = 512;
    private const ushort MINHEIGHT = 16;
    private const ushort MAXHEIGHT = 512;
    private const ushort MINSCALE = 10;
    private const ushort MAXSCALE = 1000;

    private readonly ImageComposerNative<ObjectBase> imageComposer;
    private Gif gif;

    private ushort width;
    private ushort height;
    private ushort scale;

    private double tick;
    private double tickLastAdd;

    private HttpContext context;
    private static readonly TimeSpan freshness = new TimeSpan(0, 0, 0, 300);

    public Render()
    {
        // create imagecomposer to render objects
        imageComposer = new ImageComposerNative<ObjectBase>();
        imageComposer.NewImageAvailable += OnImageComposerNewImageAvailable;
    }

    public void ProcessRequest (HttpContext context)
    {
        BgfCache.Entry entry;
        this.context = context;

        // -------------------------------------------------------       
        // read basic and mainoverlay parameters from url-path (see Global.asax):
        //  render/{width}/{height}/{scale}/{file}/{group}/{palette}/{angle}

        RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;

        string parmWidth = parms.ContainsKey("width") ? (string)parms["width"] : null;
        string parmHeight = parms.ContainsKey("height") ? (string)parms["height"] : null;
        string parmScale = parms.ContainsKey("scale") ? (string)parms["scale"] : null;
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

        // convert to lowercase
        parmFile = parmFile.ToLower();

        UInt16.TryParse(parmWidth, out width);
        UInt16.TryParse(parmHeight, out height);
        UInt16.TryParse(parmScale, out scale);

        width = MathUtil.Bound(width, MINWIDTH, MAXWIDTH);
        height = MathUtil.Bound(height, MINHEIGHT, MAXHEIGHT);
        scale = MathUtil.Bound(scale, MINSCALE, MAXSCALE);

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

        // read suboverlay array params from query parameters:
        //  object/..../?subov={file};{group};{palette};{hotspot}&subov=...
        string[] parmSubOverlays = context.Request.Params.GetValues("subov");

        if (parmSubOverlays != null)
        {
            foreach (string s in parmSubOverlays)
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

        // tick to do some calcs on the object
        gameObject.Tick(0, 1);

        // --------------------------------------------------
        // set game object on image composer (causes drawing)  

        // create gif instance
        gif = new Gif(width, height);

        //imageComposer.ApplyYOffset = false;
        //imageComposer.IsScalePow2 = true;
        //imageComposer.CenterVertical = true;
        //imageComposer.Width = width;
        //imageComposer.Height = height;
        imageComposer.Quality = 16.0f;
        imageComposer.IsCustomShrink = true;
        imageComposer.CustomShrink = (float)scale / 100.0f;

        imageComposer.DataSource = gameObject;

        if (imageComposer.Image == null)
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

        // --------------------------------------------------
        // run animationlength in 1 ms steps (causes new image events)

        tick = 0.0;
        for (int i = 0; i < gameObject.AnimationLength; i++)
        {
            tick += 1.0;
            gameObject.Tick(tick, 1.0);
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
        // write the response (encode to gif)
        context.Response.ContentType = "image/gif";
        context.Response.AddHeader("Content-Disposition", "inline; filename=object.gif");
        gif.Write(context.Response.OutputStream);
        context.Response.Flush();
        context.Response.End();

        this.context = null;

        // --------------------------------------------------
        // cleanup

        tick        = 0;
        tickLastAdd = 0;
    }

    private void OnImageComposerNewImageAvailable(object sender, EventArgs e)
    {
        Bitmap surface = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        Graphics graphics = Graphics.FromImage(surface);

        // get object size in world size
        //float scaledwidth = (float)imageComposer.RenderInfo.WorldSize.X * (float)scale * 0.01f;
        //float scaledheight = (float)imageComposer.RenderInfo.WorldSize.Y * (float)scale * 0.01f;
        float scaledwidth = (float)imageComposer.RenderInfo.WorldSize.X * (float)scale * 0.01f;
        float scaledheight = (float)imageComposer.RenderInfo.WorldSize.Y * (float)scale * 0.01f;

        // important:
        // center x (extends x, -x -> right, left)
        // fix y to bottom (extends y -> up)
        float posx = ((float)width * 0.5f) - (scaledwidth * 0.5f);
        float posy = (float)height - scaledheight;

        graphics.Clear(Color.Transparent);
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // draw mainbmp into target bitmap
        graphics.DrawImage(imageComposer.Image,
            new Rectangle((int)posx, (int)posy, (int)scaledwidth, (int)scaledheight),
            new Rectangle(0, 0, imageComposer.Image.Width, imageComposer.Image.Height),
            GraphicsUnit.Pixel);

        // get timespan for gif
        double span = tick - tickLastAdd;
        tickLastAdd = tick;

        // add it
        gif.AddFrame(surface, (ushort)(span * 0.1));
        //gif.AddFrame(imageComposer.Image, (ushort)(span * 0.1));

        // cleanup
        surface.Dispose();
        imageComposer.Image.Dispose();
        graphics.Dispose();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}