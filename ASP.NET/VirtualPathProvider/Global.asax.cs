using System;
using System.Web;
using System.Web.Hosting;

namespace Lagash.ServiceModel
{
    public class Global : HttpApplication
    {
        protected void Application_Start( object sender, EventArgs e )
        {
            HostingEnvironment.RegisterVirtualPathProvider( new ServiceModelVirtualPathProvider( ) );
        }
    }
}