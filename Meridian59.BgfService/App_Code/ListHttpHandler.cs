
using System.Web;
using System.Web.Routing;
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
    /// Creates a JSON list with all BGF names and basic info
    /// </summary>
    public class ListHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Handles the HTTP request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;

            // set cache behaviour
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.VaryByParams["*"] = false;
            response.Cache.SetLastModified(BgfCache.LastModified);

            // set response type
            response.ContentType = "application/json";
            response.ContentEncoding = new UTF8Encoding(false);
            response.AddHeader("Content-Disposition", "inline; filename=list.json");

            // get enumerator on all bgf in cache
            IEnumerator<KeyValuePair<string, BgfCache.Entry>> enumerator =
                BgfCache.GetEnumerator();

            // start output
            bool isComma = false;
            response.Write('[');
            while(enumerator.MoveNext())
            {
                BgfCache.Entry entry = enumerator.Current.Value;

                // write comma for previous entry
                if (isComma)
                    response.Write(',');
                else
                    isComma = true;

                // unix timestamp
                long stamp = (entry.LastModified.Ticks - 621355968000000000) / 10000000;

                // write json object
                response.Write("{\"file\":\"");
                response.Write(entry.Bgf.Filename);
                response.Write("\",\"size\":");
                response.Write(entry.Size.ToString());
                response.Write(",\"modified\":");
                response.Write(stamp.ToString());
                response.Write(",\"shrink\":");
                response.Write(entry.Bgf.ShrinkFactor.ToString());
                response.Write(",\"frames\":");
                response.Write(entry.Bgf.Frames.Count.ToString());
                response.Write(",\"groups\":");
                response.Write(entry.Bgf.FrameSets.Count.ToString());
                response.Write('}');
            }
            response.Write(']');
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
