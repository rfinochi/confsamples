using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using DataAnnotationSample.Attributes;
using DataAnnotationSample.Validators;

namespace DataAnnotationSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start( )
        {
            AreaRegistration.RegisterAllAreas( );

            WebApiConfig.Register( GlobalConfiguration.Configuration );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );

            DataAnnotationsModelValidatorProvider.RegisterAdapter( typeof( SameAsAttribute ), typeof( SameAsValidator ) );
        }
    }
}