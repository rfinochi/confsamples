using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MinificationAndRemoveHeaders
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start( )
        {
            //MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas( );

            WebApiConfig.Register( GlobalConfiguration.Configuration );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );
            AuthConfig.RegisterAuth( );
        }
    }
}