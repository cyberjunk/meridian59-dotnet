<%@ WebHandler Language="C#" Class="Meridian59.BgfService.RenderHttpHandler" %>

using System;
using System.Web;
using System.Web.Routing;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;

namespace Meridian59.BgfService
{
    public class RenderHttpHandler : IHttpHandler
    {
        private const ushort MINWIDTH = 16;
        private const ushort MAXWIDTH = 512;
        private const ushort MINHEIGHT = 16;
        private const ushort MAXHEIGHT = 512;
        private const ushort MINSCALE = 10;
        private const ushort MAXSCALE = 80;

        private readonly ImageComposerGDI<ObjectBase> imageComposer = new ImageComposerGDI<ObjectBase>();
        private readonly JeremyAnsel.ColorQuant.WuAlphaColorQuantizer quant = new JeremyAnsel.ColorQuant.WuAlphaColorQuantizer();
        private readonly byte[] pixels = new byte[MAXWIDTH * MAXHEIGHT];
        private readonly Gif gif = new Gif(0, 0);
        private readonly Gif.LZWEncoder encoder = new Gif.LZWEncoder();

        private ushort width;
        private ushort height;
        private ushort scale;
        private double tick;
        private double tickLastAdd;
        private HttpContext context;

        public RenderHttpHandler()
        {
            imageComposer.CenterHorizontal = true;
            //imageComposer.ApplyYOffset = true;
            //imageComposer.CenterVertical = true;
            imageComposer.Quality = 16.0f;
            imageComposer.IsCustomShrink = true;

            // create imagecomposer to render objects
            imageComposer.NewImageAvailable += OnImageComposerNewImageAvailable;
        }

        public void ProcessRequest(HttpContext context)
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

            // set gif instance size
            gif.CanvasWidth = width;
            gif.CanvasHeight = height;

            imageComposer.Width = width;
            imageComposer.Height = height;
            imageComposer.CustomShrink = (float)scale * 0.1f;
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
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["*"] = false;
            context.Response.Cache.SetLastModified(lastModified);
            // --------------------------------------------------
            // write the response (encode to gif)
            context.Response.ContentType = "image/gif";
            context.Response.AddHeader("Content-Disposition", "inline; filename=object.gif");
            gif.Write(context.Response.OutputStream);
            gif.Frames.Clear();

            this.context = null;

            // --------------------------------------------------
            // cleanup

            tick = 0;
            tickLastAdd = 0;

            // background gc
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, false);
        }

        private void OnImageComposerNewImageAvailable(object sender, EventArgs e)
        {
            // reduce to 8bit using custom lib, return palette, store indices in buffer pixels
            uint[] pal = quant.Quantize((Bitmap)imageComposer.Image, 256, pixels, false);

            // get timespan for gif
            double span = tick - tickLastAdd;
            tickLastAdd = tick;

            // create gif frame
            Gif.Frame frame = new Gif.Frame(
                pixels,
                imageComposer.Image.Width,
                imageComposer.Image.Height,
                pal,
                encoder,
                (ushort)(span * 0.1),
                0);

            // add it
            gif.Frames.Add(frame);

            // cleanup
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
}
