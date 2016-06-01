<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // warning: don't use cache, it has no limits so far
        Meridian59.Drawing2D.ImageComposerGDI<Meridian59.Data.Models.ObjectBase>.IsCacheEnabled = false;
        
        // interpolation mode for GDI+ alpha-blending/scaling
        Meridian59.Drawing2D.ImageComposerGDI<Meridian59.Data.Models.ObjectBase>.InterpolationMode = 
            System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        // ----------------------------------------------------------------

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{format}/{file}/{group}/{palette}/{angle}", new HttpHandlerRoute("~/Frame.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{format}/{file}/{group}/{palette}", new HttpHandlerRoute("~/Frame.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{format}/{file}/{group}", new HttpHandlerRoute("~/Frame.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{format}/{file}", new HttpHandlerRoute("~/Frame.ashx")));

        // ----------------------------------------------------------------

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "object/{file}/{group}/{palette}/{angle}", new HttpHandlerRoute("~/Object.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "object/{file}/{group}/{palette}", new HttpHandlerRoute("~/Object.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "object/{file}/{group}", new HttpHandlerRoute("~/Object.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "object/{file}", new HttpHandlerRoute("~/Object.ashx")));
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
