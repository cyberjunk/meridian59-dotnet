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
        PaletteRouteHandler routePalette = new PaletteRouteHandler();
        FileRouteHandler routeFile = new FileRouteHandler();
        ObjectRouteHandler routeObject = new ObjectRouteHandler();
        RenderRouteHandler routeRender = new RenderRouteHandler();

        // create routes
        Route rList = new Route("list", routeList);
        Route rPalette = new Route("palette/{num}/{format}", routePalette);
        Route rFile1 = new Route("file/{file}/{req}/{parm1}/{parm2}/{parm3}", routeFile);
        Route rFile2 = new Route("file/{file}/{req}/{parm1}/{parm2}", routeFile);
        Route rFile3 = new Route("file/{file}/{req}/{parm1}", routeFile);
        Route rFile4 = new Route("file/{file}/{req}", routeFile);
        Route rObject = new Route("object/{scale}/{file}/{group}/{palette}/{angle}", routeObject);
        Route rRender = new Route("render/{width}/{height}/{scale}/{file}/{anim}/{palette}/{angle}", routeRender);

        // add routes
        RouteTable.Routes.Add(rList);
        RouteTable.Routes.Add(rPalette);
        RouteTable.Routes.Add(rFile1);
        RouteTable.Routes.Add(rFile2);
        RouteTable.Routes.Add(rFile3);
        RouteTable.Routes.Add(rFile4);
        RouteTable.Routes.Add(rObject);
        RouteTable.Routes.Add(rRender);
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
