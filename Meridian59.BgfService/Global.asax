<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="Meridian59.Drawing2D" %>
<%@ Import Namespace="Meridian59.Data.Models" %>
<%@ Import Namespace="Meridian59.BgfService" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        // preload all BGF from disk to RAM
        BgfCache.Load();

        // don't use imagecomposer cache for images (nginx has caching enabled!)
        ImageComposerNative<ObjectBase>.Cache.IsEnabled = false;
        ImageComposerGDI<ObjectBase>.Cache.IsEnabled = false;

        // create route handlers
        ListRouteHandler routeList = new ListRouteHandler();
        FileRouteHandler routeFile = new FileRouteHandler();
        ObjectRouteHandler routeObject = new ObjectRouteHandler();
        RenderRouteHandler routeRender = new RenderRouteHandler();

        // create routes
        // ----------------------------------------------------------------
        RouteTable.Routes.Add(new Route("list", routeList));
        // ----------------------------------------------------------------
        RouteTable.Routes.Add(new Route("file/{file}/{req}/{parm1}/{parm2}/{parm3}", routeFile));
        RouteTable.Routes.Add(new Route("file/{file}/{req}/{parm1}/{parm2}", routeFile));
        RouteTable.Routes.Add(new Route("file/{file}/{req}/{parm1}", routeFile));
        RouteTable.Routes.Add(new Route("file/{file}/{req}", routeFile));
        // ----------------------------------------------------------------
        RouteTable.Routes.Add(new Route("object/{scale}/{file}/{group}/{palette}/{angle}", routeObject));
        RouteTable.Routes.Add(new Route("object/{scale}/{file}/{group}/{palette}", routeObject));
        RouteTable.Routes.Add(new Route("object/{scale}/{file}/{group}", routeObject));
        // ----------------------------------------------------------------
        RouteTable.Routes.Add(new Route("render/{width}/{height}/{scale}/{file}/{anim}/{palette}/{angle}", routeRender));
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
