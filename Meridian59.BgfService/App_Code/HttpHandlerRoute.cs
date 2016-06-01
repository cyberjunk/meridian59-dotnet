using System;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;

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

    public IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
    {
        IHttpHandler httpHandler = (IHttpHandler)
            BuildManager.CreateInstanceFromVirtualPath(_VirtualPath, typeof(IHttpHandler));
        
        return httpHandler;
    }
}
