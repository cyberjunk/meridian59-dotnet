using System;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;

namespace Meridian59.BgfService
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHandlerRoute : IRouteHandler
    {
        private String _VirtualPath = null;

        public HttpHandlerRoute(String virtualPath)
        {
            _VirtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            IHttpHandler httpHandler = (IHttpHandler)
                BuildManager.CreateInstanceFromVirtualPath(_VirtualPath, typeof(IHttpHandler));

            return httpHandler;
        }
    }
}
