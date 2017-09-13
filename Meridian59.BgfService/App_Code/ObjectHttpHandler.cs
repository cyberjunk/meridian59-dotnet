
using System;
using System.Web;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using Meridian59.Files.BGF;
using System.IO;

using Meridian59.Common.Constants;
using Meridian59.Common;
using Meridian59.Drawing2D;
using Meridian59.Data.Models;
using Meridian59.Common.Enums;
using System.Globalization;

namespace Meridian59.BgfService
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectRouteHandler : IRouteHandler
    {
        public ObjectRouteHandler()
        {
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ObjectHttpHandler();
        }
    }

    public class ObjectHttpHandler : IHttpHandler
    {
        private const ushort MINSCALE = 10;
        private const ushort MAXSCALE = 80;

        private readonly ImageComposerGDI<ObjectBase> imageComposer = new ImageComposerGDI<ObjectBase>();
        private readonly ObjectBase gameObject = new ObjectBase();

        public ObjectHttpHandler()
        {
            // don't use quality, apply custom scale
            imageComposer.Quality = 16.0f;
            imageComposer.IsCustomShrink = true;
            imageComposer.DataSource = gameObject;
        }

        public void ProcessRequest(HttpContext context)
        {
            // --------------------------------------------------------------------------------------------
            // 1) PARSE URL PARAMETERS
            // --------------------------------------------------------------------------------------------

            // See Global.asax:
            //  object/{scale}/{file}/{group}/{palette}/{angle}
            RouteValueDictionary parms = context.Request.RequestContext.RouteData.Values;
            string parmScale   = parms.ContainsKey("scale")   ? (string)parms["scale"]   : null;
            string parmFile    = parms.ContainsKey("file")    ? (string)parms["file"]    : null;
            string parmGroup   = parms.ContainsKey("group")   ? (string)parms["group"]   : null;
            string parmPalette = parms.ContainsKey("palette") ? (string)parms["palette"] : null;
            string parmAngle   = parms.ContainsKey("angle")   ? (string)parms["angle"]   : null;

            BgfCache.Entry entry;
            ushort scale;
            byte paletteidx = 0;
            ushort angle = 0;

            // verify that minimum parameters are valid/in range and bgf exists
            // angle ranges from [0-7] and are multiples of 512
            if (!UInt16.TryParse(parmScale, out scale)      || scale < MINSCALE || scale > MAXSCALE ||
                !Byte.TryParse(parmPalette, out paletteidx) ||
                !UInt16.TryParse(parmAngle, out angle)      || angle > 7 ||
                String.IsNullOrEmpty(parmFile)              || !BgfCache.GetBGF(parmFile, out entry))
            {
                context.Response.StatusCode = 404;
                return;
            }

            // multiply by 512 and remove full periods from angle
            angle = (ushort)((angle << 9) % GeometryConstants.MAXANGLE);

            // parse animation
            Animation anim = Animation.ExtractAnimation(parmGroup, '-');
            if (anim == null || !anim.IsValid(entry.Bgf.FrameSets.Count))
            {
                context.Response.StatusCode = 404;
                return;
            }

            // stores the latest lastmodified of main and all subov
            DateTime lastModified = entry.LastModified;

            // read suboverlay array params from query parameters:
            //  object/..../?subov={file};{group};{palette};{hotspot}&subov=...
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
                        context.Response.StatusCode = 404;
                        return;
                    }

                    // get suboverlay animation
                    Animation subOvAnim = Animation.ExtractAnimation(subOvParms[1], '-');
                    if (subOvAnim == null || !subOvAnim.IsValid(bgfSubOv.Bgf.FrameSets.Count))
                    {
                        context.Response.StatusCode = 404;
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
            context.Response.ContentType = "image/png";
            context.Response.AddHeader("Content-Disposition", "inline; filename=object.png");

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
                context.Response.StatusCode = 304;
                return;
            }*/

            // --------------------------------------------------------------------------------------------
            // 3) PREPARE RESPONSE
            // --------------------------------------------------------------------------------------------

            // set parsed/created values on game object
            gameObject.Resource = entry.Bgf;
            gameObject.ColorTranslation = paletteidx;
            gameObject.Animation = anim;
            gameObject.ViewerAngle = angle;

            // set imagecomposer scale/shrink
            imageComposer.CustomShrink = (float)scale * 0.1f;

            // tick object (triggers image recreation)
            gameObject.Tick(0, 1);

            if (imageComposer.Image == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            // --------------------------------------------------------------------------------------------
            // 4) CREATE RESPONSE AND FINISH
            // --------------------------------------------------------------------------------------------

            // write image as png
            imageComposer.Image.Save(context.Response.OutputStream, ImageFormat.Png);

            // clear
            imageComposer.Image.Dispose();
            gameObject.SubOverlays.Clear();
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
