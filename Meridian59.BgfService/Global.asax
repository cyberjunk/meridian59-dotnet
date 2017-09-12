<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="Meridian59.Drawing2D" %>
<%@ Import Namespace="Meridian59.Data.Models" %>
<%@ Import Namespace="Meridian59.BgfService" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // preload all BGF from disk to RAM
        BgfCache.Preload();

        // don't use imagecomposer cache for images (nginx has caching enabled!)
        ImageComposerNative<ObjectBase>.Cache.IsEnabled = false;
        ImageComposerGDI<ObjectBase>.Cache.IsEnabled = false;

        // ----------------------------------------------------------------
        FileRouteHandler routeFile = new FileRouteHandler();

        RouteTable.Routes.Add(new Route("file/{file}/{req}/{parm1}/{parm2}/{parm3}", routeFile));
        RouteTable.Routes.Add(new Route("file/{file}/{req}/{parm1}/{parm2}", routeFile));
        RouteTable.Routes.Add(new Route("file/{file}/{req}/{parm1}", routeFile));
        RouteTable.Routes.Add(new Route("file/{file}/{req}", routeFile));

        // ----------------------------------------------------------------
        ObjectRouteHandler routeObject = new ObjectRouteHandler();

        RouteTable.Routes.Add(new Route("object/{scale}/{file}/{group}/{palette}/{angle}", routeObject));
        RouteTable.Routes.Add(new Route("object/{scale}/{file}/{group}/{palette}", routeObject));
        RouteTable.Routes.Add(new Route("object/{scale}/{file}/{group}", routeObject));

        // ----------------------------------------------------------------
        RenderRouteHandler routeRender = new RenderRouteHandler();

        RouteTable.Routes.Add(new Route("render/{width}/{height}/{scale}/{file}/{anim}/{palette}/{angle}", routeRender));
        RouteTable.Routes.Add(new Route("render/{width}/{height}/{scale}/{file}/{anim}/{palette}", routeRender));
        RouteTable.Routes.Add(new Route("render/{width}/{height}/{scale}/{file}/{anim}", routeRender));
    }

    void Application_End(object sender, EventArgs e)
    {
    }

    void Application_Error(object sender, EventArgs e)
    {
    }

    void Session_Start(object sender, EventArgs e)
    {
    }

    void Session_End(object sender, EventArgs e)
    {
    }

</script>
