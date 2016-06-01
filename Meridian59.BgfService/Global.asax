<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{file}/{frame}/{palette}", new HttpHandlerRoute("~/Frame.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{file}/{frame}", new HttpHandlerRoute("~/Frame.ashx")));

        System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
            "frame/{file}", new HttpHandlerRoute("~/Frame.ashx")));
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
