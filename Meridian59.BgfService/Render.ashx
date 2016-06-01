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

    private readonly ImageComposerGDI<ObjectBase> imageComposer;   
    private readonly Gif gif;

    private ushort width;
    private ushort height;
    private ushort scale;

    private double tick;
    private double tickLastAdd;
 
    public Render()
    {
        // create imagecomposer to render objects
        imageComposer = new ImageComposerGDI<ObjectBase>();
        imageComposer.NewImageAvailable += OnImageComposerNewImageAvailable;

        // create gif instance
        gif = new Gif();
    }
    
    public void ProcessRequest (HttpContext context)
    {        
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

        UInt16.TryParse(parmWidth, out width);
        UInt16.TryParse(parmHeight, out height);
        UInt16.TryParse(parmScale, out scale);

        width = MathUtil.Bound(width, MINWIDTH, MAXWIDTH);
        height = MathUtil.Bound(height, MINHEIGHT, MAXHEIGHT);
        scale = MathUtil.Bound(scale, MINSCALE, MAXSCALE);
        
        // --------------------------------------------------
        // try to get the main BGF from cache or load from disk
        BgfFile bgfFile;
        if (!Cache.GetBGF(parmFile, out bgfFile))
        {
            context.Response.StatusCode = 404;
            context.Response.End();
            return;
        }

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
        gameObject.Resource = bgfFile;
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

                BgfFile bgfSubOv;
                string subOvFile = subOvParms[0];
                if (!Cache.GetBGF(subOvFile, out bgfSubOv))
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
                subOv.Resource = bgfSubOv;

                // add to gameobject's suboverlays
                gameObject.SubOverlays.Add(subOv);
            }
        }

        // tick to do some calcs on the object
        gameObject.Tick(0, 1);
        
        // --------------------------------------------------
        // set game object on image composer (causes drawing)  

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
           
        // --------------------------------------------------
        // write the response (encode to gif)

        context.Response.ContentType = "image/gif";
        gif.Save(context.Response.OutputStream);
        context.Response.Flush();
        context.Response.End();

        // --------------------------------------------------
        // cleanup
        
        tick        = 0;
        tickLastAdd = 0;
        
        // dispose the single frames ('surface' in new image event)
        foreach (Gif.GifFrame f in gif.Frames)
            if (f.Image != null) 
                f.Image.Dispose();

        gif.Clear(); 
    }

    private void OnImageComposerNewImageAvailable(object sender, EventArgs e)
    {
        Bitmap surface = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        Graphics graphics = Graphics.FromImage(surface);

        // get object size in world size
        float scaledwidth = (float)((imageComposer.RenderInfo.UVEnd.X * imageComposer.RenderInfo.Dimension.X) / imageComposer.RenderInfo.Scaling) * (float)scale * 0.01f;
        float scaledheight = (float)((imageComposer.RenderInfo.UVEnd.Y * imageComposer.RenderInfo.Dimension.Y) / imageComposer.RenderInfo.Scaling) * (float)scale * 0.01f;

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
        gif.AddFrame(surface, span);
        
        // cleanup the imagecomposer image and the graphics
        // the 'surface' bitmap is required until stream is written and disposed later
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