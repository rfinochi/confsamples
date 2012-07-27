using System;
using System.Web;

namespace CometMVC
{
    public class CometHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(String.Format("<script>alert('{0}')</script>", "COMET1"));
            context.Response.Write(String.Format("<script>alert('{0}')</script>", "COMET2"));
        }

        #endregion
    }
}
