using System;
using System.Net;

namespace Meridian59.Patcher
{
   /// <summary>
   /// WebClient with 'Accept-Encoding: gzip'
   /// </summary>
   public class WebClientGzip : WebClient
   {
      public WebClientGzip() : base()
      {
         this.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
      }

      protected override WebRequest GetWebRequest(Uri address)
      {
         HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
         request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
         return request;
      }
   }
}
