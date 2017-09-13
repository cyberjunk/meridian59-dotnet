
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
using System.Globalization;

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
        private readonly ObjectBase gameObject = new ObjectBase();

        private double tick;
        private double tickLastAdd;
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
            // --------------------------------------------------------------------------------------------
            // 1) PARSE URL PARAMETERS
            // --------------------------------------------------------------------------------------------

            // See Global.asax:
            //  render/{width}/{height}/{scale}/{file}/{anim}/{palette}/{angle}
            RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
            string parmWidth   = parms.ContainsKey("width")   ? (string)parms["width"]   : null;
            string parmHeight  = parms.ContainsKey("height")  ? (string)parms["height"]  : null;
            string parmScale   = parms.ContainsKey("scale")   ? (string)parms["scale"]   : null;
            string parmFile    = parms.ContainsKey("file")    ? (string)parms["file"]    : null;
            string parmAnim    = parms.ContainsKey("anim")    ? (string)parms["anim"]    : null;
            string parmPalette = parms.ContainsKey("palette") ? (string)parms["palette"] : null;
            string parmAngle   = parms.ContainsKey("angle")   ? (string)parms["angle"]   : null;

            BgfCache.Entry entry;
            byte paletteidx;
            ushort angle;
            ushort width;
            ushort height;
            ushort scale;

            // verify that minimum parameters are valid/in range and bgf exists
            // angle ranges from [0-7] and are multiples of 512
            if (!UInt16.TryParse(parmWidth, out width)      || width  < MINWIDTH  || width  > MAXWIDTH  ||
                !UInt16.TryParse(parmHeight, out height)    || height < MINHEIGHT || height > MAXHEIGHT ||
                !UInt16.TryParse(parmScale, out scale)      || scale  < MINSCALE  || scale  > MAXSCALE  ||
                !Byte.TryParse(parmPalette, out paletteidx) ||
                !UInt16.TryParse(parmAngle, out angle)      || angle > 7 ||
                String.IsNullOrEmpty(parmFile)              || !BgfCache.GetBGF(parmFile, out entry))
            {
                Finish(context, 404);
                return;
            }

            // multiply by 512 and remove full periods from angle
            angle = (ushort)((angle << 9) % GeometryConstants.MAXANGLE);

            // parse animation
            Animation anim = Animation.ExtractAnimation(parmAnim, '-');
            if (anim == null || !anim.IsValid(entry.Bgf.FrameSets.Count))
            {
                Finish(context, 404);
                return;
            }

            // stores the latest lastmodified of main and all subov
            DateTime lastModified = entry.LastModified;

            // read suboverlay array params from query parameters:
            //  object/..../?subov={file};{anim};{palette};{hotspot}&subov=...
            string[] parmSubOverlays = context.Request.Params.GetValues("s");
            if (parmSubOverlays != null)
            {
                foreach (string s in parmSubOverlays)
                {
                    string[] subOvParms = s.Split(';');

                    BgfCache.Entry bgfSubOv;
                    byte subOvPalette;
                    byte subOvHotspot;

                    if (subOvParms == null || subOvParms.Length < 4 ||
                        String.IsNullOrEmpty(subOvParms[0]) || 
                        String.IsNullOrEmpty(subOvParms[1]) ||
                        !byte.TryParse(subOvParms[2], out subOvPalette) ||
                        !byte.TryParse(subOvParms[3], out subOvHotspot) || subOvHotspot > 127 ||
                        !BgfCache.GetBGF(subOvParms[0], out bgfSubOv))
                    {
                        Finish(context, 404);
                        return;
                    }

                    // get suboverlay animation
                    Animation subOvAnim = Animation.ExtractAnimation(subOvParms[1], '-');
                    if (subOvAnim == null || !subOvAnim.IsValid(bgfSubOv.Bgf.FrameSets.Count))
                    {
                        Finish(context, 404);
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

            // --------------------------------------------------------------------------------------------
            // 2) SET CACHE HEADERS AND COMPARE FOR 304 RETURN
            // --------------------------------------------------------------------------------------------

            // set cache behaviour
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["*"] = false;
            context.Response.Cache.SetLastModified(lastModified);

            // set return type
            context.Response.ContentType = "image/gif";
            context.Response.AddHeader("Content-Disposition", "inline; filename=object.gif");

            // check if client has valid cached version (returns 304)
            /*DateTime dateIfModifiedSince;
            string modSince = context.Request.Headers["If-Modified-Since"];

            // try to parse received client header
            bool parseOk = DateTime.TryParse(
                modSince, CultureInfo.CurrentCulture, 
                DateTimeStyles.AdjustToUniversal, out dateIfModifiedSince);

            // send 304 and stop if last file write equals client's cache timestamp
            if (parseOk && dateIfModifiedSince == lastModified)
            {
                context.Response.SuppressContent = true;
                Finish(context, 304);
                return;
            }*/

            // --------------------------------------------------------------------------------------------
            // 3) PREPARE RESPONSE
            // --------------------------------------------------------------------------------------------

            // set gif instance size
            gif.CanvasWidth = width;
            gif.CanvasHeight = height;

            // set parsed/created values on game-object
            gameObject.Resource = entry.Bgf;
            gameObject.ColorTranslation = paletteidx;
            gameObject.Animation = anim;
            gameObject.ViewerAngle = angle;

            // set imagecomposer size and shrink
            imageComposer.Width = width;
            imageComposer.Height = height;
            imageComposer.CustomShrink = (float)scale * 0.1f;

            // set object (triggers first event!)
            imageComposer.DataSource = gameObject;

            // run animationlength in 1 ms steps (causes new image events)
            for (int i = 0; i < gameObject.AnimationLength + 10; i++)
            {
                tick += 1.0;
                gameObject.Tick(tick, 1.0);
            }

            // --------------------------------------------------------------------------------------------
            // 4) CREATE RESPONSE AND FINISH
            // --------------------------------------------------------------------------------------------

            // write the gif to output stream
            gif.Write(context.Response.OutputStream);

            // cleanup
            Finish(context);
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

        private void Finish(HttpContext context, int StatusCode = 200)
        {
            // reset
            tick = 0.0;
            tickLastAdd = 0.0;
            frame = null;

            // clean up reusable member instances
            gif.Frames.Clear();
            gameObject.SubOverlays.Clear();

            // set statuscode
            context.Response.StatusCode = StatusCode;

            // end response
            try { context.ApplicationInstance.CompleteRequest(); }
            finally { }

            // reuse the handler
            RenderRouteHandler.Handlers.Enqueue(this);
        }
    }
}
