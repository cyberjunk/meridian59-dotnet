
using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Threading;
using Meridian59.Common.Constants;
using Meridian59.Drawing2D;
using Meridian59.Data.Models;
using System.Collections.Concurrent;
using JeremyAnsel.ColorQuant;

namespace Meridian59.BgfService
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderRouteHandler : IRouteHandler
    {
        public const int NUMHANDLERS = 4;
        public const int NUMATTEMPTS = 40;
        public const int NUMDELAY = 250;
        
        public static readonly ConcurrentQueue<RenderHttpHandler> Handlers =
            new ConcurrentQueue<RenderHttpHandler>();

        private static readonly RenderHttpHandler[] Instances = 
            new RenderHttpHandler[NUMHANDLERS];

        static RenderRouteHandler()
        {
            for (int i = 0; i < Instances.Length; i++)
            {
                Instances[i] = new RenderHttpHandler();
                Handlers.Enqueue(Instances[i]);
            }
        }

        public RenderRouteHandler()
        {
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            RenderHttpHandler handler;

            for(int i = 0; i < NUMATTEMPTS; i++)
            {
                if (Handlers.TryDequeue(out handler))
                    return handler;

                else
                    Thread.Sleep(NUMDELAY);
            }

            return new BusyErrorHttpHandler();
        }
    }

    /// <summary>
    /// Creates animated GIF from Meridian 59 object
    /// Very CPU and RAM intense!
    /// </summary>
    public class RenderHttpHandler : IHttpHandler
    {
        private const ushort MINWIDTH = 16;
        private const ushort MAXWIDTH = 256;
        private const ushort MINHEIGHT = 16;
        private const ushort MAXHEIGHT = 256;
        private const ushort MINSCALE = 1;
        private const ushort MAXSCALE = 80;

        private readonly ImageComposerGDI<ObjectBase> imageComposer = new ImageComposerGDI<ObjectBase>();
        private readonly WuAlphaColorQuantizer quant = new WuAlphaColorQuantizer();
        private readonly byte[] pixels = new byte[MAXWIDTH * MAXHEIGHT];
        private readonly Gif gif = new Gif(0, 0);
        private readonly Gif.LZWEncoder encoder = new Gif.LZWEncoder();

        private ushort width;
        private ushort height;
        private ushort scale;
        private double tick;
        private double tickLastAdd;
        private HttpContext context;
        private Gif.Frame frame;

        public RenderHttpHandler()
        {
            imageComposer.CenterHorizontal = true;
            //imageComposer.CenterVertical = true;
            imageComposer.ApplyYOffset = true;
            imageComposer.Quality = 16.0f;
            imageComposer.IsCustomShrink = true;
            imageComposer.NewImageAvailable += OnImageComposerNewImageAvailable;
        }

        public void ProcessRequest(HttpContext context)
        {
            BgfCache.Entry entry;
            this.context = context;

            // -------------------------------------------------------       
            // Read URL routed parameters (see Global.asax):
            //  render/{width}/{height}/{scale}/{file}/{anim}/{palette}/{angle}
            RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
            string parmWidth   = parms.ContainsKey("width")   ? (string)parms["width"]   : null;
            string parmHeight  = parms.ContainsKey("height")  ? (string)parms["height"]  : null;
            string parmScale   = parms.ContainsKey("scale")   ? (string)parms["scale"]   : null;
            string parmFile    = parms.ContainsKey("file")    ? (string)parms["file"]    : null;
            string parmAnim    = parms.ContainsKey("anim")    ? (string)parms["anim"]    : null;
            string parmPalette = parms.ContainsKey("palette") ? (string)parms["palette"] : null;
            string parmAngle   = parms.ContainsKey("angle")   ? (string)parms["angle"]   : null;
            
            // -------------------------------------------------------
            // verify that minimum parameters are valid/in range and bgf exists
            if (String.IsNullOrEmpty(parmFile)           ||
                !UInt16.TryParse(parmWidth, out width)   || width  < MINWIDTH  || width  > MAXWIDTH  ||
                !UInt16.TryParse(parmHeight, out height) || height < MINHEIGHT || height > MAXHEIGHT ||
                !UInt16.TryParse(parmScale, out scale)   || scale  < MINSCALE  || scale  > MAXSCALE)
            {
                Finish(404);
                return;
            }

            // --------------------------------------------------
            // try to get the main BGF from cache or load from disk
            parmFile = parmFile.ToLower();
            if (!BgfCache.GetBGF(parmFile, out entry))
            {
                Finish(404);
                return;
            }

            // stores the latest lastmodified of main and all subov
            DateTime lastModified = entry.LastModified;

            // --------------------------------------------------
            // try to parse palette, angle and animation
            byte paletteidx = 0;
            ushort angle = 0;
            Byte.TryParse(parmPalette, out paletteidx);
            UInt16.TryParse(parmAngle, out angle);

            // angle is in multiples of 512
            // first is 0 (0units), maximum is 7 (3584units)
            if (angle > 7)
            {
                Finish(404);
                return;
            }

            // multiply by 512 and remove full periods from angle
            angle = (ushort)((angle << 9) % GeometryConstants.MAXANGLE);

            // parse animation
            Animation anim = Animation.ExtractAnimation(parmAnim, '-');
            if (anim == null || !anim.IsValid(entry.Bgf.FrameSets.Count))
            {
                Finish(404);
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
            //  object/..../?subov={file};{anim};{palette};{hotspot}&subov=...
            string[] parmSubOverlays = context.Request.Params.GetValues("subov");
            if (parmSubOverlays != null)
            {
                foreach (string s in parmSubOverlays)
                {
                    string[] subOvParms = s.Split(';');

                    if (subOvParms == null || subOvParms.Length < 4)
                    {
                        Finish(404);
                        return;
                    }

                    BgfCache.Entry bgfSubOv;
                    string subOvFile = subOvParms[0].ToLower();

                    if (!BgfCache.GetBGF(subOvFile, out bgfSubOv))
                    {
                        Finish(404);
                        return;
                    }

                    byte subOvPalette;
                    byte subOvHotspot;

                    if (String.IsNullOrEmpty(subOvParms[1]) ||
                        !byte.TryParse(subOvParms[2], out subOvPalette) ||
                        !byte.TryParse(subOvParms[3], out subOvHotspot))
                    {
                        Finish(404);
                        return;
                    }

                    // get suboverlay animation
                    Animation subOvAnim = Animation.ExtractAnimation(subOvParms[1], '-');
                    if (subOvAnim == null || !subOvAnim.IsValid(bgfSubOv.Bgf.FrameSets.Count))
                    {
                        Finish(404);
                        return;
                    }

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
            // set gif instance size
            gif.CanvasWidth = width;
            gif.CanvasHeight = height;

            // set imagecomposer size and shrink
            imageComposer.Width = width;
            imageComposer.Height = height;
            imageComposer.CustomShrink = (float)scale * 0.1f;

            // reset
            tick = 0.0;
            tickLastAdd = 0.0;
            frame = null;

            // set object (triggers first event!)
            imageComposer.DataSource = gameObject;

            // --------------------------------------------------
            // run animationlength in 1 ms steps (causes new image events)
            for (int i = 0; i < gameObject.AnimationLength + 1; i++)
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

            Finish();
        }

        private void OnImageComposerNewImageAvailable(object sender, EventArgs e)
        {
            // reduce to 8bit using custom lib, return palette, store indices in buffer pixels
            uint[] pal = quant.Quantize((Bitmap)imageComposer.Image, 256, pixels, false);

            // get timespan for gif
            double span = tick - tickLastAdd;
            tickLastAdd = tick;

            if (frame != null)
            {
                // set delay on last frame
                frame.GraphicsControl.DelayTime = (ushort)(span * 0.1);

                // add last frame
                gif.Frames.Add(frame);
            }

            // create new gif frame
            frame = new Gif.Frame(
                pixels,
                imageComposer.Image.Width,
                imageComposer.Image.Height,
                pal,
                encoder,
                0,
                0);

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

        private void Finish(int StatusCode = 200)
        {
            // set statuscode
            context.Response.StatusCode = StatusCode;

            // remember locally
            HttpContext con = this.context;

            // reset reference
            this.context = null;

            // end response
            try { con.ApplicationInstance.CompleteRequest(); }
            finally { }

            // reuse the handler
            RenderRouteHandler.Handlers.Enqueue(this);

            // --------------------------------------------------
            // background gc
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, false);
        }
    }
}
