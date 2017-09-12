using System.Web;

namespace Meridian59.BgfService
{
    public class BusyErrorHttpHandler : IHttpHandler
    {
        public BusyErrorHttpHandler()
        {
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = 503;
            context.Response.Headers.Add("Retry-After", "30");
        }
    }
}
