using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MovieIndex
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start( )
        {
            AutofacConfig.RegisterDependencies( );
            
            AreaRegistration.RegisterAllAreas( );

            WebApiConfig.Register( GlobalConfiguration.Configuration );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );
        }
    }
}