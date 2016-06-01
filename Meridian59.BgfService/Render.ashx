<%@ WebHandler Language="C#" Class="Render" %>

using System;
using System.Web;
using System.Collections.Specialized;
using System.Web.Routing;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Meridian59.Common;
using Meridian59.Common.Constants;
using Meridian59.Data.Models;
using Meridian59.Drawing2D;
using Meridian59.Files.BGF;

public class Render : IHttpHandler 
{
    static Render()
    {              
    }
    
    public void ProcessRequest (HttpContext context)
    {
        
    }
 
    public bool IsReusable 
    {
        get 
        {
            return false;
        }
    }

}