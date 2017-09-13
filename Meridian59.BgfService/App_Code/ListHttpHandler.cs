
using System;
using System.Web;
using System.Web.Routing;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Meridian59.BgfService
{
    /// <summary>
    /// 
    /// </summary>
    public class ListRouteHandler : IRouteHandler
    {
        public ListRouteHandler()
        {
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ListHttpHandler();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ListHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Handles the HTTP request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            UTF8Encoding utf8 = new UTF8Encoding(false);

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["*"] = false;
            context.Response.Cache.SetLastModified(BgfCache.LastModified);
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = utf8;
            context.Response.AddHeader("Content-Disposition", "inline; filename=list.json");

            StreamWriter writer = new StreamWriter(
                context.Response.OutputStream, utf8, 4096, true);

            IEnumerator<KeyValuePair<string, BgfCache.Entry>> enumerator = 
                BgfCache.GetEnumerator();

            /////////////////////////////////////////////////////////////
            bool isComma = false;
            writer.Write("[");
            while(enumerator.MoveNext())
            {
                if (isComma)
                    writer.Write(',');
                else
                    isComma = true;

                BgfCache.Entry entry = enumerator.Current.Value;

                // unix timestamp
                long stamp = (entry.LastModified.Ticks - 621355968000000000) / 10000000;

                writer.Write('{' +
                    "\"file\":" + "\"" + entry.Bgf.Filename + "\"" + ',' +
                    "\"size\":" + entry.Size + ',' +
                    "\"modified\":" + stamp + ',' +
                    "\"shrink\":" + entry.Bgf.ShrinkFactor + ',' +
                    "\"frames\":" + entry.Bgf.Frames.Count + ',' +
                    "\"groups\":" + entry.Bgf.FrameSets.Count + '}');

                /*writer.Write('[' +
                    "\"" + entry.Bgf.Filename + "\"" + ',' +
                    entry.Size + ',' +
                    stamp + ',' +
                    entry.Bgf.ShrinkFactor + ',' +
                    entry.Bgf.Frames.Count + ',' +
                    entry.Bgf.FrameSets.Count + ']');*/
            }

            writer.Write(']');
            /////////////////////////////////////////////////////////////
            writer.Close();
            writer.Dispose();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
